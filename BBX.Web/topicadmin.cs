using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Web;
using BBX.Common;
using BBX.Config;
using BBX.Entity;
using BBX.Forum;
using NewLife.Log;
using XCode;

namespace BBX.Web
{
    public class topicadmin : PageBase
    {
        public string operationtitle = "操作提示";
        public string operation = DNTRequest.GetQueryString("operation").ToLower();
        public string action = DNTRequest.GetQueryString("action");
        public string topiclist = DNTRequest.GetString("topicid");
        public string postidlist = DNTRequest.GetString("postid");
        public string forumname = "";
        public string forumnav = "";
        public string title = "";
        public string poster = "";
        public int forumid = DNTRequest.GetInt("forumid", -1);
        public string forumlist = Caches.GetForumListBoxOptionsCache(true);
        public int displayorder;
        public int digest = DNTRequest.GetFormInt("level", -1);
        public string highlight_color = DNTRequest.GetFormString("highlight_color");
        public string highlight_style_b = DNTRequest.GetFormString("highlight_style_b");
        public string highlight_style_i = DNTRequest.GetFormString("highlight_style_i");
        public string highlight_style_u = DNTRequest.GetFormString("highlight_style_u");
        public int close;
        public int moveto = DNTRequest.GetFormInt("moveto", 0);
        public string type = DNTRequest.GetFormString("type");
        public EntityList<Post> postlist;
        public DataTable scorelist;
        public List<TopicIdentify> identifylist = TopicIdentify.Meta.Cache.Entities;
        public string identifyjsarray = TopicIdentify.GetFileNameJsArray();
        public string topictypeselectoptions;
        public DataTable ratelog = new DataTable();
        public int ratelogcount;
        public Topic topicinfo;
        public int opinion = DNTRequest.GetInt("opinion", -1);
        protected bool ismoder;
        protected bool issubmit;
        public bool titlemessage;
        protected int RateIsReady;
        private IXForum forum;
        public bool issendmessage;
        public bool isreason;

        protected override void ShowPage()
        {
            this.ValidatePermission();
            this.BindTitle();
        }

        private void ValidatePermission()
        {
            if (this.userid == -1)
            {
                base.AddErrLine("请先登录.");
                return;
            }
            if (ForumUtils.IsCrossSitePost() || String.IsNullOrEmpty(this.action))
            {
                base.AddErrLine("非法提交.");
                return;
            }
            var userGroupInfo = UserGroup.FindByID(BBX.Forum.Users.GetUserInfo(this.userid).GroupID);
            switch (usergroupinfo.ReasonPm)
            {
                case 1:
                    this.isreason = true;
                    break;

                case 2:
                    this.issendmessage = true;
                    break;

                case 3:
                    this.isreason = true;
                    this.issendmessage = true;
                    break;
            }
            this.ismoder = BBX.Forum.Moderators.IsModer(this.useradminid, this.userid, this.forumid);
            var adminGroupInfo = AdminGroup.FindByID(this.usergroupid);
            if (!this.operation.Equals("rate") && !this.operation.Equals("bonus") && !this.operation.Equals("banpost") && !DNTRequest.GetString("operat").Equals("rate") && !DNTRequest.GetString("operat").Equals("bonus") && !DNTRequest.GetString("operat").Equals("banpost") && adminGroupInfo == null)
            {
                base.AddErrLine("你没有管理权限");
                return;
            }
            if (String.IsNullOrEmpty(this.action))
            {
                base.AddErrLine("操作类型参数为空.");
                return;
            }
            if (this.forumid == -1)
            {
                base.AddErrLine("版块ID必须为数字.");
                return;
            }
            if (DNTRequest.GetFormString("topicid") != "" && !Topic.InSameForum(this.topiclist, this.forumid))
            {
                base.AddErrLine("无法对非本版块主题进行管理操作.");
                return;
            }
            this.displayorder = Topic.GetDisplayorder(this.topiclist);
            this.digest = Topic.GetDigest(this.topiclist);
            this.forum = BBX.Forum.Forums.GetForumInfo(this.forumid);
            this.forumname = this.forum.Name;
            this.topictypeselectoptions = BBX.Forum.Forums.GetCurrentTopicTypesOption(this.forum.Fid, this.forum.TopicTypes);
            this.pagetitle = Utils.RemoveHtml(this.forumname);
            this.forumnav = ForumUtils.UpdatePathListExtname(this.forum.Pathlist.Trim(), this.config.Extname);
            if (this.operation == "delposts")
            {
                base.SetUrl(base.ShowForumAspxRewrite(this.forumid, 0));
            }
            else
            {
                base.SetUrl(DNTRequest.GetUrlReferrer());
            }
            if (!forum.AllowView(this.usergroupid))
            {
                base.AddErrLine("您没有浏览该版块的权限.");
                return;
            }
            if (this.topiclist.CompareTo("") == 0)
            {
                base.AddErrLine("您没有选择主题或相应的管理操作.");
                return;
            }
            if (this.operation.CompareTo("") != 0)
            {
                if (!this.DoOperations(this.forum, adminGroupInfo, this.config.Reasonpm))
                {
                    return;
                }
                ForumUtils.DeleteTopicCacheFile(this.topiclist);
                this.issubmit = true;
            }
            if (this.action.CompareTo("moderate") == 0)
            {
                if (this.operation.CompareTo("") == 0)
                {
                    this.operation = DNTRequest.GetString("operat");
                    if (this.operation.CompareTo("") == 0)
                    {
                        base.AddErrLine("您没有选择主题或相应的管理操作.");
                    }
                }
                return;
            }
            if ("delete,move,type,highlight,close,displayorder,digest,copy,split,merge,bump,repair,delposts,banpost".IndexOf(this.operation) == -1)
            {
                base.AddErrLine("你无权操作此功能");
                return;
            }
            this.operation = this.action;
        }

        private bool BindTitle()
        {
            if (this.operation == null) return true;

            switch (operation)
            {
                //case "split":
                //    this.operationtitle = "分割主题";
                //    if (Utils.StrToInt(this.topiclist, 0) <= 0)
                //    {
                //        base.AddErrLine(string.Format("您的身份 \"{0}\" 没有分割主题的权限.", this.usergroupinfo.GroupTitle));
                //        return false;
                //    }
                //    this.postlist = BBX.Forum.Posts.GetPostListTitle(Utils.StrToInt(this.topiclist, 0));
                //    if (this.postlist != null && this.postlist.Rows.Count > 0)
                //    {
                //        this.postlist.Rows[0].Delete();
                //        this.postlist.AcceptChanges();
                //        return true;
                //    }
                //    return true;
                case "rate":
                    {
                        this.operationtitle = "参与评分";
                        if (!this.CheckRatePermission())
                        {
                            return false;
                        }
                        string text = BBX.Forum.TopicAdmins.CheckRateState(this.postidlist, this.userid);
                        if (this.config.Dupkarmarate != 1 && !String.IsNullOrEmpty(text) && this.RateIsReady != 1)
                        {
                            base.AddErrLine("对不起,您不能对同一个帖子重复评分.");
                            return false;
                        }
                        this.scorelist = BBX.Forum.UserGroups.GroupParticipateScore(this.userid, this.usergroupid);
                        if (this.scorelist.Rows.Count < 1)
                        {
                            base.AddErrLine(string.Format("您的身份 \"{0}\" 没有设置评分范围或者今日可用评分已经用完", this.usergroupinfo.GroupTitle));
                            return false;
                        }
                        var pi = Post.FindByID(postidlist.ToInt());
                        if (pi == null)
                        {
                            base.AddErrLine("您没有选择要评分的帖子.");
                            return false;
                        }
                        this.poster = pi.Poster;
                        if (pi.PosterID == this.userid)
                        {
                            base.AddErrLine("您不能对自已的帖子评分.");
                            return false;
                        }
                        this.title = pi.Title;
                        this.topiclist = pi.Tid.ToString();
                        return true;
                    }
                case "cancelrate":
                    {
                        this.operationtitle = "撤销评分";
                        var pi = Post.FindByID(postidlist.ToInt());
                        if (pi == null)
                        {
                            base.AddErrLine("您没有选择要撤消评分的帖子");
                            return false;
                        }
                        if (!this.ismoder)
                        {
                            base.AddErrLine("您的身份 \"" + this.usergroupinfo.GroupTitle + "\" 没有撤消评分的权限.");
                            return false;
                        }
                        this.poster = pi.Poster;
                        this.title = pi.Title;
                        this.topiclist = pi.Tid.ToString();
                        //this.ratelogcount = AdminRateLogs.RecordCount("pid = " + this.postidlist);
                        //this.ratelog = AdminRateLogs.LogList(this.ratelogcount, 1, "pid = " + this.postidlist);
                        // 考虑到一个帖子不会有太多评分，一次查出来全部
                        var list = RateLog.SearchByPid(this.postidlist.ToInt(), 0, ratelogcount);
                        this.ratelogcount = list.Count;
                        this.ratelog = list.ToDataTable(false);
                        this.ratelog.Columns.Add("extcreditname", typeof(String));
                        DataTable scoreSet = Scoresets.GetScoreSet();
                        foreach (DataRow dataRow in ratelog.Rows)
                        {
                            int num2 = Utils.StrToInt(dataRow["extcredits"].ToString(), 0);
                            if ((num2 > 0 && num2 < 9) || scoreSet.Columns.Count > num2 + 1)
                            {
                                dataRow["extcreditname"] = scoreSet.Rows[0][num2 + 1].ToString();
                            }
                            else
                            {
                                dataRow["extcreditname"] = "";
                            }
                        }
                        return true;
                    }
                case "bonus":
                    {
                        this.operationtitle = "结帖";
                        int tid = Utils.StrToInt(this.topiclist, 0);
                        this.postlist = Post.FindAllByTid(tid);
                        // 不明白为什么要删掉第一个帖子
                        //if (this.postlist != null && this.postlist.Rows.Count > 0)
                        //{
                        //    this.postlist.Rows[0].Delete();
                        //    this.postlist.AcceptChanges();
                        //}
                        // 明白了，原来只是为了去掉主贴
                        postlist = postlist.FindAll(e => e.Layer > 0);
                        if (this.postlist.Count == 0)
                        {
                            base.AddErrLine("无法对没有回复的悬赏进行结帖.");
                            return false;
                        }
                        this.topicinfo = Topic.FindByID(tid);
                        if (this.topicinfo.Special == 3)
                        {
                            base.AddErrLine("本主题的悬赏已经结束.");
                            return false;
                        }
                        return true;
                    }
                case "delete":
                    this.operationtitle = "删除主题";
                    return true;
                case "move":
                    this.operationtitle = "移动主题";
                    return true;
                case "type":
                    this.operationtitle = "主题分类";
                    return true;
                case "highlight":
                    this.operationtitle = "高亮显示";
                    return true;
                case "close":
                    this.operationtitle = "关闭/打开主题";
                    return true;
                case "displayorder":
                    this.operationtitle = "置顶/解除置顶";
                    return true;
                case "digest":
                    this.operationtitle = "加入/解除精华 ";
                    return true;
                case "copy":
                    this.operationtitle = "复制主题";
                    return true;
                case "merge":
                    this.operationtitle = "合并主题";
                    return true;
                case "bump":
                    this.operationtitle = "提升/下沉主题";
                    return true;
                case "repair":
                    this.operationtitle = "修复主题";
                    return true;
                case "delposts":
                    this.operationtitle = "批量删帖";
                    return true;
                case "banpost":
                    this.operationtitle = "单帖屏蔽";
                    return true;
                case "identify":
                    this.operationtitle = "鉴定主题";
                    return true;
                default:
                    this.operationtitle = "未知操作";
                    return true;
            }
        }

        private bool CheckRatePermission()
        {
            if (String.IsNullOrEmpty(this.usergroupinfo.Raterange))
            {
                base.AddErrLine(string.Format("您的身份 \"{0}\" 没有评分的权限.", this.usergroupinfo.GroupTitle));
                return false;
            }
            bool flag = false;
            string[] array = this.usergroupinfo.Raterange.Split('|');
            for (int i = 0; i < array.Length; i++)
            {
                string text = array[i];
                if (!String.IsNullOrEmpty(text) && text.Contains(","))
                {
                    var ss = text.Split(',');
                    if (ss.Length > 1 && ss[1].ToBoolean())
                    {
                        flag = true;
                    }
                }
                else
                {
                    // 调试错误
                    XTrace.WriteLine("用户组{0}的权限{1}有问题", usergroupinfo.GroupTitle, usergroupinfo.Raterange);
                }
            }
            if (!flag)
            {
                base.AddErrLine(string.Format("您的身份 \"{0}\" 没有评分的权限.", this.usergroupinfo.GroupTitle));
                return false;
            }
            return true;
        }

        private bool DoOperations(IXForum forum, AdminGroup admininfo, int reasonpm)
        {
            string next = DNTRequest.GetFormString("next");
            string text = Utils.InArray(this.operation, "delete,move") ? (this.forumpath + Urls.ShowForumAspxRewrite(this.forumid, 1, forum.Rewritename)) : DNTRequest.GetUrlReferrer();
            //DataTable dataTable = null;
            string reason = DNTRequest.GetHtmlEncodeString("reason");
            int sendmessage = DNTRequest.GetFormInt("sendmessage", 0);
            if (this.issendmessage && sendmessage == 0)
            {
                this.titlemessage = true;
                base.AddErrLine("操作必须发送短消息通知用户");
                return false;
            }
            if (!Utils.InArray(this.operation, "identify,bonus") && this.isreason)
            {
                if (reason.IsNullOrEmpty())
                {
                    this.titlemessage = true;
                    base.AddErrLine("操作原因不能为空");
                    return false;
                }
                if (reason.Length > 200)
                {
                    this.titlemessage = true;
                    base.AddErrLine("操作原因不能多于200个字符");
                    return false;
                }
            }
            if (!Utils.InArray(this.operation, "delete,move,type,highlight,close,displayorder,digest,copy,split,merge,bump,repair,rate,cancelrate,delposts,identify,bonus,banpost"))
            {
                this.titlemessage = true;
                base.AddErrLine("未知的操作参数");
                return false;
            }
            if (!Utils.StrIsNullOrEmpty(next.Trim()))
            {
                text = string.Format("topicadmin.aspx?action={0}&forumid={1}&topicid={2}", next, this.forumid, this.topiclist);
            }
            int operationid = 0;
            bool istopic = false;
            var dictionary = new Dictionary<int, string>();
            string subjecttype;
            IEntityList datalist = null;
            if (Utils.InArray(this.operation, "rate,delposts,banpost,cancelrate"))
            {
                //dataTable = BBX.Forum.Posts.GetPostList(this.postidlist, this.topiclist);
                subjecttype = "帖子";
                //var list = BBX.Forum.Posts.GetPostList(this.postidlist, this.topiclist);
                var list = Post.FindAllByIDs(postidlist);
                datalist = list;
                foreach (var pt in list)
                {
                    dictionary.Add(pt.ID, pt.Message);
                }
                //IEnumerator enumerator = dataTable.Rows.GetEnumerator();
                //try
                //{
                //	while (enumerator.MoveNext())
                //	{
                //		DataRow dataRow = (DataRow)enumerator.Current;
                //		dictionary.Add(TypeConverter.ObjectToInt(dataRow["pid"]), dataRow["message"].ToString());
                //	}
                //	goto IL_24D;
                //}
                //finally
                //{
                //	IDisposable disposable = enumerator as IDisposable;
                //	if (disposable != null)
                //	{
                //		disposable.Dispose();
                //	}
                //}
            }
            else
            {
                //dataTable = BBX.Forum.Topics.GetTopicList(this.topiclist, -1);
                var list = Topic.FindAllByTidsAndDisplayOrder(topiclist, -1);
                datalist = list;
                istopic = true;
                subjecttype = "主题";
                foreach (var tp in list)
                {
                    dictionary.Add(tp.ID, tp.Title);
                }
            }
            //IL_24D:
            #region 动作分流
            string key;
            string action;
            switch (key = this.operation)
            {
                case "delete":
                    action = "删除主题";
                    if (!this.DoDeleteOperation(forum))
                    {
                        return false;
                    }
                    operationid = 1;
                    break;
                case "move":
                    action = "移动主题";
                    if (!this.DoMoveOperation())
                    {
                        return false;
                    }
                    operationid = 2;
                    break;
                case "type":
                    action = "主题分类";
                    if (!this.DoTypeOperation())
                    {
                        return false;
                    }
                    operationid = 3;
                    break;
                case "highlight":
                    action = "设置高亮";
                    if (!this.DoHighlightOperation())
                    {
                        return false;
                    }
                    operationid = 4;
                    break;
                case "close":
                    action = "关闭主题/取消";
                    if (!this.DoCloseOperation())
                    {
                        return false;
                    }
                    operationid = 5;
                    break;
                case "displayorder":
                    action = "主题置顶/取消";
                    if (!this.DoDisplayOrderOperation(admininfo))
                    {
                        return false;
                    }
                    operationid = 6;
                    break;
                case "digest":
                    action = "设置精华/取消";
                    if (!this.DoDigestOperation())
                    {
                        return false;
                    }
                    operationid = 7;
                    break;
                case "copy":
                    action = "复制主题";
                    if (!this.DoCopyOperation())
                    {
                        return false;
                    }
                    operationid = 8;
                    break;
                case "split":
                    action = "分割主题";
                    //if (!this.DoSplitOperation())
                    //{
                    //    return false;
                    //}
                    throw new NotSupportedException("不支持分割主题！");
                //operationid = 9;
                //break;
                case "merge":
                    action = "合并主题";
                    //if (!this.DoMergeOperation())
                    //{
                    //    return false;
                    //}
                    throw new NotSupportedException("不支持合并主题！");
                //operationid = 10;
                //break;
                case "bump":
                    action = "提升/下沉主题";
                    if (!this.DoBumpTopicsOperation())
                    {
                        return false;
                    }
                    operationid = 11;
                    break;
                case "repair":
                    action = "修复主题";
                    if (!this.ismoder)
                    {
                        this.titlemessage = true;
                        base.AddErrLine("您没有修复主题的权限");
                        return false;
                    }
                    BBX.Forum.TopicAdmins.RepairTopicList(this.topiclist);
                    operationid = 12;
                    break;
                case "rate":
                    action = "帖子评分";
                    if (!this.DoRateOperation(reason))
                    {
                        return false;
                    }
                    operationid = 13;
                    break;
                case "delposts":
                    {
                        action = "批量删帖";
                        int num2 = 1;
                        bool flag = this.DoDelpostsOperation(reason, forum, ref num2);
                        if (num2 == 0)
                        {
                            return true;
                        }
                        if (!flag)
                        {
                            return false;
                        }
                        operationid = 14;
                        break;
                    }
                case "identify":
                    action = "鉴定主题";
                    if (!this.DoIndentifyOperation())
                    {
                        return false;
                    }
                    operationid = 15;
                    break;
                case "cancelrate":
                    action = "撤销评分";
                    if (!this.DoCancelRateOperation(reason))
                    {
                        return false;
                    }
                    operationid = 16;
                    break;
                case "bonus":
                    action = "结帖";
                    if (!this.DoBonusOperation())
                    {
                        return false;
                    }
                    operationid = 16;
                    break;
                case "banpost":
                    action = "屏蔽帖子";
                    if (!this.DoBanPostOperatopn())
                    {
                        return false;
                    }
                    operationid = 17;
                    break;
                default:
                    action = "未知操作";
                    break;
            }
            #endregion
            //IL_5A6:
            base.AddMsgLine((next.CompareTo("") == 0) ? "管理操作成功,现在将转入主题列表" : "管理操作成功,现在将转入后续操作");
            if (!this.operation.Equals("rate") && !this.operation.Equals("split") && this.config.Modworkstatus == 1)
            {
                if (String.IsNullOrEmpty(this.postidlist))
                {
                    string[] array = this.topiclist.Split(',');
                    for (int i = 0; i < array.Length; i++)
                    {
                        string text4 = "";
                        var tid = Int32.Parse(array[i]);
                        dictionary.TryGetValue(tid, out text4);
                        if (string.IsNullOrEmpty(text4))
                        {
                            var topicInfo = Topic.FindByID(tid);
                            text4 = ((topicInfo == null) ? text4 : topicInfo.Title);
                        }
                        ModeratorManageLog.Add(userid, username, usergroupid, usergroupinfo.GroupTitle, forumid, forumname, tid, text4, action, reason);
                    }
                }
                else
                {
                    int tid = Utils.StrToInt(this.topiclist, -1);
                    var topicInfo2 = Topic.FindByID(tid);
                    string[] array2 = this.postidlist.Split(',');
                    for (int j = 0; j < array2.Length; j++)
                    {
                        string text5 = array2[j];
                        //var postInfo = new PostInfo();
                        string text6 = dictionary[Utils.StrToInt(text5, 0)];
                        subjecttype = "回复的主题";
                        string text7 = text6.Replace(" ", "").Replace("|", "");
                        if (text7.Length > 100)
                        {
                            text7 = text7.Substring(0, 20) + "...";
                        }
                        text7 = "(pid:" + text5 + ")" + text7;
                        if (this.operation != "delposts")
                        {
                            var postInfo = Post.FindByID(text5.ToInt());
                            text7 = ((postInfo == null) ? text7 : postInfo.Title);
                        }
                        ModeratorManageLog.Add(userid, username, usergroupid, usergroupinfo.GroupTitle, forumid, forumname, topicInfo2.ID, text7, action, reason);
                    }
                }
            }
            this.SendMessage(operationid, datalist, istopic, action, reason, sendmessage, subjecttype);
            base.SetUrl(text);
            if (next != string.Empty)
            {
                HttpContext.Current.Response.Redirect(BaseConfigs.GetForumPath + text, false);
            }
            else
            {
                base.AddScript("window.setTimeout('redirectURL()', 2000);function redirectURL() {window.location='" + text + "';}");
            }
            base.SetShowBackLink(false);
            return true;
        }

        private string GetOperatePostUrl(int tid, string title)
        {
            return string.Format("[url={0}{1}]{2}[/url]", Utils.GetRootUrl(BaseConfigs.GetForumPath), Urls.ShowTopicAspxRewrite(tid, 1), title);
        }

        private void SendMessage(int operationid, IEntityList list, bool istopic, string operationName, string reason, int sendmsg, string subjecttype)
        {
            if (istopic)
            {
                //BBX.Forum.Topics.UpdateTopicModerated(this.topiclist, operationid);
                var tps = Topic.FindAllByIDs(topiclist);
                tps.ForEach(e => e.Moderated = operationid);
                tps.Save();
            }
            if (list != null && list.Count > 0)
            {
                if (this.useradminid != 1 && ForumUtils.HasBannedWord(reason))
                {
                    base.AddErrLine(string.Format("您提交的内容包含不良信息 <font color=\"red\">{0}</font>", ForumUtils.GetBannedWord(reason)));
                    return;
                }
                reason = ForumUtils.BanWordFilter(reason);
                foreach (var dr in list)
                {
                    if (sendmsg == 1)
                    {
                        this.MessagePost(dr, operationName, subjecttype, reason);
                    }
                }
                //list.Dispose();
            }
        }

        private void MessagePost(IEntity entity, string operationName, string subjecttype, string reason)
        {
            //int uid = Utils.StrToInt(entity["posterid"], -1);
            //if (uid == -1) return;

            var tp = entity as Topic;
            var pi = entity as Post;
            var uid = 0;
            if (tp != null)
                uid = tp.PosterID;
            else if (pi != null)
                uid = pi.PosterID;
            if (uid == 0) return;

            var noticeInfo = new Notice();
            noticeInfo.New = 1;
            noticeInfo.PostDateTime = DateTime.Now;
            noticeInfo.Type = (Int32)NoticeType.TopicAdmin;
            noticeInfo.Poster = this.username;
            noticeInfo.PosterID = this.userid;
            noticeInfo.Uid = uid;
            reason = (string.IsNullOrEmpty(reason) ? reason : ("理由:" + reason));
            if (subjecttype == "主题" || entity["layer"].ToInt(-1) == 0)
            {
                string text = (this.operation != "delete") ? this.GetOperatePostUrl(tp.ID, tp.Title) : tp.Title;
                noticeInfo.Note = Utils.HtmlEncode(string.Format("您发表的主题 {0} 被 {1} 执行了{2}操作 {3}", new object[]
                {
                    text,
                    "<a href=\"" + base.UserInfoAspxRewrite(this.userid) + "\" target=\"_blank\" >" + this.username + "</a>",
                    operationName,
                    reason
                }));
            }
            else
            {
                string text = this.GetOperatePostUrl(pi.Tid, pi.TopicTitle);
                noticeInfo.Note = Utils.HtmlEncode(string.Format("您在 {0} 回复的帖子被 {1} 执行了{2}操作 {3}", new object[]
                {
                    text,
                    "<a href=\"" + base.UserInfoAspxRewrite(this.userid) + "\" target=\"_blank\" >" + this.username + "</a>",
                    operationName,
                    reason
                }));
            }
            //BBX.Forum.Notices.CreateNoticeInfo(noticeInfo);
            noticeInfo.Insert();
        }

        private bool DoRateOperation(string reason)
        {
            if (!this.CheckRatePermission())
            {
                return false;
            }
            if (this.postidlist.IsNullOrEmpty())
            {
                this.titlemessage = true;
                base.AddErrLine("您没有选择要评分的帖子");
                return false;
            }
            //if (this.config.Dupkarmarate != 1 && AdminRateLogs.RecordCount(AdminRateLogs.GetRateLogCountCondition(this.userid, this.postidlist)) > 0)
            if (this.config.Dupkarmarate != 1 && RateLog.SearchCount(this.userid, this.postidlist.ToInt()) > 0)
            {
                this.titlemessage = true;
                base.AddErrLine("您不能对本帖重复评分");
                return false;
            }
            this.scorelist = BBX.Forum.UserGroups.GroupParticipateScore(this.userid, this.usergroupid);
            string[] array = Utils.SplitString(DNTRequest.GetFormString("score").Replace("+", ""), ",");
            string[] array2 = Utils.SplitString(DNTRequest.GetFormString("extcredits"), ",");
            string text = "";
            string text2 = "";
            int num = 0;
            for (int i = 0; i < array.Length; i++)
            {
                if (Utils.IsNumeric(array[i].ToString()) && array[i].ToString() != "0" && !array[i].ToString().Contains("."))
                {
                    text = text + array[i] + ",";
                    text2 = text2 + array2[i] + ",";
                }
            }
            if (text.Length == 0)
            {
                this.titlemessage = true;
                base.AddErrLine("分值超过限制.");
                return false;
            }
            foreach (DataRow dr in this.scorelist.Rows)
            {
                var sc = array[num].ToInt();
                if (dr["ScoreCode"].ToString().Equals(array2[num]) && (dr["MaxInDay"].ToInt() < Math.Abs(sc) || dr["Max"].ToInt() < sc || (sc != 0 && dr["Min"].ToInt() > sc)))
                {
                    this.titlemessage = true;
                    base.AddErrLine("分值超过限制.");
                    return false;
                }
                num++;
            }
            BBX.Forum.TopicAdmins.RatePosts(Utils.StrToInt(this.topiclist, 0), this.postidlist, text, text2, this.userid, this.username, reason);
            BBX.Forum.Posts.UpdatePostRateTimes(Utils.StrToInt(this.topiclist, 0), this.postidlist);
            this.RateIsReady = 1;
            return true;
        }

        private bool DoCancelRateOperation(string reason)
        {
            if (!this.CheckRatePermission())
            {
                return false;
            }
            if (String.IsNullOrEmpty(this.postidlist))
            {
                this.titlemessage = true;
                base.AddErrLine("您未选择要撤销评分的帖子");
                return false;
            }
            if (!this.ismoder)
            {
                this.titlemessage = true;
                base.AddErrLine("您没有撤消评分的权限");
                return false;
            }
            if (String.IsNullOrEmpty(DNTRequest.GetFormString("ratelogid")))
            {
                this.titlemessage = true;
                base.AddErrLine("您未选择要撤销评分的记录");
                return false;
            }
            BBX.Forum.TopicAdmins.CancelRatePosts(DNTRequest.GetFormString("ratelogid"), Utils.StrToInt(this.topiclist, 0), this.postidlist, this.userid, this.username, this.usergroupinfo.ID, this.usergroupinfo.GroupTitle, this.forumid, this.forumname, reason);
            BBX.Forum.Posts.UpdatePostRateTimes(Utils.StrToInt(this.topiclist, 0), this.postidlist);
            return true;
        }

        //private bool DoMergeOperation()
        //{
        //    if (!this.ismoder)
        //    {
        //        this.titlemessage = true;
        //        base.AddErrLine("您没有合并主题的权限");
        //        return false;
        //    }
        //    if (DNTRequest.GetFormInt("othertid", 0) == 0)
        //    {
        //        this.titlemessage = true;
        //        base.AddErrLine("您没有输入要合并的主题ID");
        //        return false;
        //    }
        //    if (DNTRequest.GetFormInt("othertid", 0) == Utils.StrToInt(this.topiclist, 0))
        //    {
        //        this.titlemessage = true;
        //        base.AddErrLine("不能对同一主题进行合并操作");
        //        return false;
        //    }
        //    if (BBX.Forum.Topics.GetTopicInfo(DNTRequest.GetFormInt("othertid", 0)) == null)
        //    {
        //        this.titlemessage = true;
        //        base.AddErrLine("目标主题不存在");
        //        return false;
        //    }
        //    if (TableList.GetPostTableId(DNTRequest.GetFormInt("othertid", 0)) != TableList.GetPostTableId(Utils.StrToInt(this.topiclist, 0)))
        //    {
        //        this.titlemessage = true;
        //        base.AddErrLine("不允许跨分表合并主题");
        //        return false;
        //    }
        //    BBX.Forum.TopicAdmins.MerrgeTopics(this.topiclist, DNTRequest.GetFormInt("othertid", 0));
        //    return true;
        //}

        //private bool DoSplitOperation()
        //{
        //    if (!this.ismoder)
        //    {
        //        this.titlemessage = true;
        //        base.AddErrLine("您没有分割主题的权限");
        //        return false;
        //    }
        //    if (String.IsNullOrEmpty(DNTRequest.GetString("subject")))
        //    {
        //        this.titlemessage = true;
        //        base.AddErrLine("您没有输入标题");
        //        return false;
        //    }
        //    if (DNTRequest.GetString("subject").Length > 60)
        //    {
        //        this.titlemessage = true;
        //        base.AddErrLine("标题长为60字以内");
        //        return false;
        //    }
        //    if (String.IsNullOrEmpty(this.postidlist))
        //    {
        //        this.titlemessage = true;
        //        base.AddErrLine("请选择要分割入新主题的帖子");
        //        return false;
        //    }
        //    if (TableList.GetPostTableId(this.topiclist.ToInt()) != TableList.GetPostTableId())
        //    {
        //        this.titlemessage = true;
        //        base.AddErrLine("主题过旧,无法分割");
        //        return false;
        //    }
        //    BBX.Forum.TopicAdmins.SplitTopics(this.postidlist, DNTRequest.GetString("subject"), this.topiclist);
        //    return true;
        //}

        private bool DoCopyOperation()
        {
            if (!this.ismoder)
            {
                this.titlemessage = true;
                base.AddErrLine("您没有复制主题的权限");
                return false;
            }
            if (DNTRequest.GetFormInt("copyto", 0) == 0)
            {
                this.titlemessage = true;
                base.AddErrLine("您没有选择目标论坛/分类");
                return false;
            }
            BBX.Forum.TopicAdmins.CopyTopics(this.topiclist, DNTRequest.GetFormInt("copyto", 0));
            return true;
        }

        private bool DoDigestOperation()
        {
            if (!this.ismoder)
            {
                this.titlemessage = true;
                base.AddErrLine("您没有设置精华的权限");
                return false;
            }
            this.digest = DNTRequest.GetFormInt("level", -1);
            if (this.digest > 3 || this.digest < 0)
            {
                this.digest = -1;
            }
            if (this.digest == -1)
            {
                this.titlemessage = true;
                base.AddErrLine("您没有选择精华级别");
                return false;
            }
            BBX.Forum.TopicAdmins.SetDigest(this.topiclist, (int)short.Parse(this.digest.ToString()));
            return true;
        }

        private bool DoDisplayOrderOperation(AdminGroup admininfo)
        {
            if (!this.ismoder)
            {
                this.titlemessage = true;
                base.AddErrLine("您没有置顶的管理权限");
                return false;
            }
            this.displayorder = DNTRequest.GetFormInt("level", -1);
            if (this.displayorder < 0 || this.displayorder > 3)
            {
                this.titlemessage = true;
                base.AddErrLine("置顶参数超出范围");
                return false;
            }
            if (admininfo.ID != 1 && admininfo.AllowStickthread < displayorder)
            {
                this.titlemessage = true;
                base.AddErrLine(string.Format("您没有{0}级置顶的管理权限", this.displayorder));
                return false;
            }
            BBX.Forum.TopicAdmins.SetTopTopicList(this.forumid, this.topiclist, short.Parse(this.displayorder.ToString()));
            return true;
        }

        private bool DoCloseOperation()
        {
            if (!this.ismoder)
            {
                this.titlemessage = true;
                base.AddErrLine("您没有关闭主题的权限");
                return false;
            }
            if (DNTRequest.GetFormInt("close", -1) == -1)
            {
                this.titlemessage = true;
                base.AddErrLine("您没选择打开还是关闭");
                return false;
            }
            //if (BBX.Forum.TopicAdmins.SetClose(this.topiclist, short.Parse(DNTRequest.GetFormInt("close", -1).ToString())) < 1)
            var list = Topic.FindAllByIDs(topiclist);
            var close = Request["close"].ToInt(-1);
            list.ForEach(e => e.Closed = close);
            list.Update();
            if (list.Count == 0)
            {
                this.titlemessage = true;
                base.AddErrLine("要操作的主题未找到");
                return false;
            }
            return true;
        }

        private bool DoHighlightOperation()
        {
            if (!this.ismoder)
            {
                this.titlemessage = true;
                base.AddErrLine("您没有设置高亮的权限");
                return false;
            }
            string text = "";
            if (!String.IsNullOrEmpty(this.highlight_style_b))
            {
                text += "font-weight:bold;";
            }
            if (!String.IsNullOrEmpty(this.highlight_style_i))
            {
                text += "font-style:italic;";
            }
            if (!String.IsNullOrEmpty(this.highlight_style_u))
            {
                text += "text-decoration:underline;";
            }
            if (!String.IsNullOrEmpty(this.highlight_color))
            {
                text = text + "color:" + this.highlight_color + ";";
            }
            if (String.IsNullOrEmpty(text))
            {
                this.titlemessage = true;
                base.AddErrLine("您没有选择字体样式及颜色");
                return false;
            }
            //BBX.Forum.TopicAdmins.SetHighlight(this.topiclist, text);
            var list = Topic.FindAllByIDs(topiclist);
            list.ForEach(e => e.Highlight = text);
            list.Save();

            return true;
        }

        private bool DoTypeOperation()
        {
            if (!this.ismoder)
            {
                this.titlemessage = true;
                base.AddErrLine("您没有修改主题分类的权限");
                return false;
            }
            var typeid = DNTRequest.GetFormInt("typeid", 0);
            if (typeid == 0)
            {
                this.titlemessage = true;
                base.AddErrLine("你没有选择相应的主题分类");
                return false;
            }
            //BBX.Forum.TopicAdmins.ResetTopicTypes(DNTRequest.GetFormInt("typeid", 0), this.topiclist);
            var list = Topic.FindAllByIDs(topiclist);
            list.ForEach(e => e.TypeID = typeid);
            list.Save();
            return true;
        }

        private bool DoMoveOperation()
        {
            if (!this.ismoder)
            {
                this.titlemessage = true;
                base.AddErrLine("您没有移动主题权限");
                return false;
            }
            if (this.moveto == 0 || this.type.CompareTo("") == 0 || ",normal,redirect,".IndexOf("," + this.type.Trim() + ",") == -1)
            {
                this.titlemessage = true;
                base.AddErrLine("您没选择分类或移动方式");
                return false;
            }
            if (this.moveto == this.forumid)
            {
                this.titlemessage = true;
                base.AddErrLine("主题不能在相同分类内移动");
                return false;
            }
            var forumInfo = BBX.Forum.Forums.GetForumInfo(this.moveto);
            if (forumInfo == null)
            {
                this.titlemessage = true;
                base.AddErrLine("目标版块不存在");
                return false;
            }
            if (forumInfo.Layer == 0)
            {
                this.titlemessage = true;
                base.AddErrLine("主题不能在分类间移动");
                return false;
            }
            int num = DNTRequest.GetInt("movetopictype", 0);
            bool flag = false;
            if (!String.IsNullOrEmpty(forumInfo.TopicTypes))
            {
                string[] array = forumInfo.TopicTypes.Split('|');
                for (int i = 0; i < array.Length; i++)
                {
                    string text = array[i];
                    if (num == text.Split(',')[0].ToInt())
                    {
                        flag = true;
                        break;
                    }
                }
            }
            num = (flag ? num : 0);
            BBX.Forum.TopicAdmins.MoveTopics(this.topiclist, this.moveto, this.forumid, this.type.CompareTo("redirect") == 0, num);
            return true;
        }

        private bool DoDeleteOperation(IXForum forum)
        {
            var adg = AdminGroup.FindByID(useradminid);
            if (!this.ismoder || !adg.AllowDelPost)
            {
                this.titlemessage = true;
                base.AddErrLine("您没有删除权限");
                return false;
            }
            if (Utils.SplitString(this.topiclist, ",", true).Length > 1 && !adg.AllowMassprune)
            {
                this.titlemessage = true;
                base.AddErrLine("您没有批量删除权限");
                return false;
            }
            BBX.Forum.TopicAdmins.DeleteTopics(this.topiclist, forum.Recyclebin != 0, DNTRequest.GetInt("reserveattach", 0) == 1);
            //XForum.SetRealCurrentTopics(forum.Fid);
            //BBX.Forum.Forums.UpdateLastPost(forum);
            (forum as XForum).ResetLastPost();
            return true;
        }

        private bool DoBumpTopicsOperation()
        {
            if (!this.ismoder)
            {
                this.titlemessage = true;
                base.AddErrLine("您没有提升/下沉主题的权限");
                return false;
            }
            if (!Utils.IsNumericList(this.topiclist))
            {
                this.titlemessage = true;
                base.AddErrLine("非法的主题ID");
                return false;
            }
            if (Math.Abs(DNTRequest.GetFormInt("bumptype", 0)) != 1)
            {
                this.titlemessage = true;
                base.AddErrLine("错误的参数");
                return false;
            }
            BBX.Forum.TopicAdmins.BumpTopics(this.topiclist, DNTRequest.GetFormInt("bumptype", 0));
            return true;
        }

        private bool DoBanPostOperatopn()
        {
            if (!this.ismoder)
            {
                this.titlemessage = true;
                base.AddErrLine("您没有单帖屏蔽的权限");
                return false;
            }
            if (!Utils.IsNumeric(this.topiclist))
            {
                this.titlemessage = true;
                base.AddErrLine("无效的主题ID");
                return false;
            }
            var topicInfo = Topic.FindByID(Utils.StrToInt(this.topiclist, 0));
            if (topicInfo == null)
            {
                this.titlemessage = true;
                base.AddErrLine("不存在的主题");
                return false;
            }
            if (!Utils.IsNumericList(this.postidlist))
            {
                this.titlemessage = true;
                base.AddErrLine("非法的帖子ID");
                return false;
            }
            //return BBX.Forum.Posts.BanPosts(topicInfo.ID, this.postidlist, DNTRequest.GetFormInt("banpost", -1));
            var invisible = Request["banpost"].ToInt();
            var list = Post.FindAllByIDs(postidlist);
            list.ForEach(pi => pi.Invisible = invisible);
            list.Save();

            return true;
        }

        private bool DoDelpostsOperation(string reason, IXForum forum, ref int layer)
        {
            var adg = AdminGroup.FindByID(useradminid);
            if (!this.ismoder || !adg.AllowDelPost)
            {
                this.titlemessage = true;
                base.AddErrLine("您没有批量删帖的权限");
                return false;
            }
            if (Utils.SplitString(this.postidlist, ",", true).Length > 1 && !adg.AllowMassprune)
            {
                this.titlemessage = true;
                base.AddErrLine("您没有批量删除的权限");
                return false;
            }
            if (!Utils.IsNumeric(this.topiclist))
            {
                this.titlemessage = true;
                base.AddErrLine("无效的主题ID");
                return false;
            }
            var topicInfo = Topic.FindByID(Utils.StrToInt(this.topiclist, 0));
            if (topicInfo == null)
            {
                this.titlemessage = true;
                base.AddErrLine("不存在的主题");
                return false;
            }
            if (!Utils.IsNumericList(this.postidlist))
            {
                this.titlemessage = true;
                base.AddErrLine("非法的帖子ID");
                return false;
            }
            var reserveattach = DNTRequest.GetInt("reserveattach", 0) == 1;
            bool result = false;
            string[] array = this.postidlist.Split(',');
            int i = 0;
            while (i < array.Length)
            {
                string expression = array[i];
                var postInfo = Post.FindByID(expression.ToInt());
                bool result2;
                if (postInfo == null || (postInfo.Layer <= 0 && topicInfo.Replies > 0) || topicInfo.ID != postInfo.Tid)
                {
                    this.titlemessage = true;
                    base.AddErrLine("主题无效或者已被回复");
                    result2 = false;
                }
                else
                {
                    if (postInfo.Layer == 0)
                    {
                        BBX.Forum.TopicAdmins.DeleteTopics(topicInfo.ID.ToString(), forum.Recyclebin != 0, reserveattach);
                        layer = 0;
                        break;
                    }
                    BBX.Forum.Posts.DeletePost(postInfo, reserveattach, true);
                    if (topicInfo.Special == 4)
                    {
                        if (this.opinion != 1 && this.opinion != 2)
                        {
                            this.titlemessage = true;
                            base.AddErrLine("参数错误");
                            result2 = false;
                            return result2;
                        }
                        //string text = "";
                        //switch (this.opinion)
                        //{
                        //    case 1:
                        //        text = "positivediggs";
                        //        break;

                        //    case 2:
                        //        text = "negativediggs";
                        //        break;
                        //}
                        Debate.DeleteDebatePost(topicInfo.ID, opinion, Utils.StrToInt(expression, -1));
                    }
                    result = true;
                    i++;
                    continue;
                }
                return result2;
            }
            BBX.Forum.Topics.UpdateTopicReplyCount(topicInfo.ID);
            //BBX.Forum.Forums.UpdateLastPost(forum);
            (forum as XForum).ResetLastPost();
            return result;
        }

        private bool DoIndentifyOperation()
        {
            if (!this.ismoder)
            {
                this.titlemessage = true;
                base.AddErrLine("您没有鉴定主题的权限");
                return false;
            }
            int selectidentify = DNTRequest.GetInt("selectidentify", 0);
            if (selectidentify > 0 || selectidentify == -1)
            {
                //BBX.Forum.TopicAdmins.IdentifyTopic(this.topiclist, @selectidentify);
                var list = Topic.FindAllByIDs(topiclist);
                list.ForEach(e => e.Identify = selectidentify);
                list.Save();
                return true;
            }
            this.titlemessage = true;
            base.AddErrLine("请选择签定类型");
            return false;
        }

        private bool DoBonusOperation()
        {
            this.topicinfo = Topic.FindByID(DNTRequest.GetInt("topicid", 0));
            if (this.topicinfo.Special == 3)
            {
                this.titlemessage = true;
                base.AddErrLine("本主题的悬赏已经结束");
                return false;
            }
            if (this.topicinfo.PosterID <= 0)
            {
                this.titlemessage = true;
                base.AddErrLine("无法结束游客发布的悬赏");
                return false;
            }
            if (this.topicinfo.PosterID != this.userid && !this.ismoder)
            {
                this.titlemessage = true;
                base.AddErrLine("您没有权限结束此悬赏");
                return false;
            }
            int num = 0;
            string[] array = DNTRequest.GetString("postbonus").Split(',');
            string[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                string expression = array2[i];
                num += Utils.StrToInt(expression, 0);
            }
            if (num != this.topicinfo.Price)
            {
                this.titlemessage = true;
                base.AddErrLine("获奖分数与悬赏分数不一致");
                return false;
            }
            string[] array3 = DNTRequest.GetFormString("addons").Split(',');
            int[] array4 = new int[array3.Length];
            int[] array5 = new int[array3.Length];
            string[] array6 = new string[array3.Length];
            string[] array7 = array3;
            for (int j = 0; j < array7.Length; j++)
            {
                string text = array7[j];
                if (Utils.StrToInt(text.Split('|')[0], 0) == this.topicinfo.PosterID)
                {
                    this.titlemessage = true;
                    base.AddErrLine("不能向悬赏者发放积分奖励");
                    return false;
                }
            }
            if (array.Length != array3.Length)
            {
                this.titlemessage = true;
                base.AddErrLine("获奖者数量与积分奖励数量不一致");
                return false;
            }
            if (base.IsErr())
            {
                return false;
            }
            for (int k = 0; k < array3.Length; k++)
            {
                array4[k] = Utils.StrToInt(array3[k].Split('|')[0], 0);
                array5[k] = Utils.StrToInt(array3[k].Split('|')[1], 0);
                array6[k] = array3[k].Split('|')[2];
            }
            BonusLog.CloseBonus(this.topicinfo, this.userid, array5, array4, array6, array, DNTRequest.GetFormString("valuableAnswers").Split(','), DNTRequest.GetFormInt("bestAnswer", 0));
            if (DNTRequest.GetFormInt("sendmessage", 0) == 0)
            {
                return true;
            }
            for (int l = 0; l < array4.Length; l++)
            {
                int num2 = array[l].ToInt();
                if (num2 != 0)
                {
                    this.BonusPostMessage(this.topicinfo, array5[l], array4[l], array5[l] == DNTRequest.GetFormInt("bestAnswer", 0), num2);
                }
            }
            return true;
        }

        private void BonusPostMessage(Topic topicInfo, int pid, int answerUid, bool isBeta, int num)
        {
            var notice = new Notice
            {
                New = 1,
                PostDateTime = DateTime.Now,
                Type = (Int32)NoticeType.TopicAdmin,
                Poster = this.username,
                PosterID = this.userid,
                Uid = answerUid,
                FromID = pid,
                Note = Utils.HtmlEncode(string.Format("您发表的 {0} 被 {1} 评为 {2} ,给予 {3}{4} 奖励", new object[]
                {
                    "<a href=\"" + Urls.ShowTopicAspxRewrite(topicInfo.ID, 0, topicInfo.TypeID) + "#" + pid + "\" target=\"_blank\" >回帖</a>",
                    "<a href=\"" + base.UserInfoAspxRewrite(this.userid) + "\" target=\"_blank\" >" + this.username + "</a>",
                    isBeta ? "最佳答案" : "有价值的答案",
                    num,
                    Scoresets.GetScoreSet(Scoresets.GetTopicAttachCreditsTrans()).Name
                }))
            };
            notice.Insert();
        }

        protected string TransAttachImgUbb(string message)
        {
            return Regex.Replace(message, "\\[attachimg\\](\\d+)\\[/attachimg\\]", "<a href='javascript:void(0)' aid='$1' class='floatimg'>[图片]</a><img id='img$1' src='attachment.aspx?attachmentid=$1' width='150' style='display:none;' />", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }
    }
}