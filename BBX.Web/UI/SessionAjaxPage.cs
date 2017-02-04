using System;
using NewLife.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using BBX.Common;
using BBX.Config;
using BBX.Entity;
using BBX.Forum;
using NewLife.Web;

namespace BBX.Web.UI
{
    public class SessionAjaxPage //: PageBase
    {
        Int32 userid;
        String username;
        Int32 useradminid;
        Int32 usergroupid;
        GeneralConfigInfo config = GeneralConfigInfo.Current;

        UserGroup usergroupinfo;
        String forumpath = BaseConfigs.GetForumPath;

        Online oluserinfo;

        HttpResponse Response = HttpContext.Current.Response;

        public SessionAjaxPage()
        {
            userid = Utils.StrToInt(ForumUtils.GetCookie("userid"), -1);
            if (config.Nocacheheaders == 1)
            {
                Response.BufferOutput = false;
                Response.ExpiresAbsolute = DateTime.Now.AddDays(-1.0);
                Response.Cache.SetExpires(DateTime.Now.AddDays(-1.0));
                Response.Expires = 0;
                Response.CacheControl = "no-cache";
                Response.Cache.SetNoStore();
            }
            oluserinfo = Online.UpdateInfo();
            if (config.PostTimeStorageMedia == 1 && Utils.GetCookie("lastposttime") != "")
            {
                var lptime = DateTime.MinValue;
                if (DateTime.TryParse(Utils.GetCookie("lastposttime"), out lptime)) oluserinfo.LastPostTime = lptime;
            }
            userid = oluserinfo.UserID;
            usergroupid = (int)oluserinfo.GroupID;
            username = oluserinfo.UserName;
            usergroupinfo = UserGroup.FindByID(usergroupid);
            useradminid = usergroupinfo.RadminID;

            string t = DNTRequest.GetString("t");
            string key;
            switch (key = t)
            {
                case "forumtree":
                    GetForumTree();
                    return;

                case "topictree":
                    GetTopicTree();
                    return;

                case "quickreply":
                    QuickReply();
                    return;

                case "report":
                    Report();
                    return;

                case "diggdebates":
                    DiggDebates();
                    return;

                case "debatevote":
                    Detatevote();
                    return;

                case "getdebatepostpage":
                    GetDebatePostPage();
                    return;

                case "checkuserextcredit":
                    CheckUserExtCredit(DNTRequest.GetInt("aid", 0));
                    return;

                case "confirmbuyattach":
                    ConfirmBuyAttach(DNTRequest.GetInt("aid", 0));
                    return;

                case "getnewpms":
                    GetNewPms();
                    return;

                case "getnewnotifications":
                    GetNewNotifications();
                    return;

                case "getajaxforums":
                    GetAjaxForumsJsonList();
                    return;

                case "passpost":
                case "deletepost":
                case "ignorepost":
                case "passtopic":
                case "ignoretopic":
                case "deletetopic":
                    AuditPost(t, DNTRequest.GetString("reason"));
                    return;

                case "deletepostsbyuidanddays":
                    DeletePostsByUidAndDays(DNTRequest.GetInt("uid", 0), 7);
                    return;

                case "getattachlist":
                    GetAttachList();
                    return;

                case "deleteattach":
                    DeleteAttach();
                    return;

                case "imagelist":
                    GetImageList();
                    break;
            }
        }

        private void DeleteAttach()
        {
            int aid = DNTRequest.GetInt("aid", 0);
            var att = Attachment.FindByID(aid);
            if (att.Uid == userid || Moderators.IsModer(useradminid, userid, Post.FindByID(att.Pid).Fid))
            {
                //BBX.Forum.Attachments.DeleteAttachment(aid.ToString());
                att.Delete();
                ResponseJSON(string.Format("[{{'aid':{0}}}]", aid).ToString());
                return;
            }
            ResponseJSON(string.Format("[{{'erraid':{0}}}]", aid).ToString());
        }

        private void GetAttachList()
        {
            var sb = new StringBuilder();
            var posttime = WebHelper.RequestDateTime("posttime");
            var file = DNTRequest.GetString("file") + "";
            var list = Attachment.FindAllNoUsed(userid, posttime, file != "" ? AttachmentFileType.FileAttachment : AttachmentFileType.All);
            sb.Append("[");
            foreach (var att in list)
            {
                sb.Append(string.Format("{{'aid':{0},'attachment':'{1}','filetype':'{2}','readperm':{3},'attachprice':{4},'width':{5},'height':{6},'extname':'{7}','attachkey':'{8}'}},", new object[]
                {
                    att.ID,
                    att.Name.Replace("'", "\\'"),
                    att.FileType,
                    att.ReadPerm,
                    att.AttachPrice,
                    att.Width,
                    att.Height,
                    Utils.GetFileExtName(att.Name),
                    Thumbnail.GetKey(att.ID)
                }));
            }
            ResponseJSON(sb.ToString().TrimEnd(',') + "]");
        }

        private void GetImageList()
        {
            var sb = new StringBuilder();
            var posttime = WebHelper.RequestDateTime("posttime");
            var list = Attachment.FindAllNoUsed(userid, posttime, AttachmentFileType.ImageAttachment);
            sb.Append("[");
            foreach (var att in list)
            {
                sb.AppendFormat("{{'aid':{0},'attachment':'{1}','attachkey':'{2}','description':''}},", att.ID, att.Name.Replace("'", "\\'"), Thumbnail.GetKey(att.ID));
            }
            ResponseJSON(sb.ToString().TrimEnd(',') + "]");
        }

        private void GetAjaxForumsJsonList()
        {
            var sb = new StringBuilder();
            //List<ForumInfo> subForumList = BBX.Forum.Forums.GetSubForumList(DNTRequest.GetInt("fid", 0));
            var subForumList = XForum.FindAllByParent(DNTRequest.GetInt("fid", 0));
            sb.Append("[");
            if (subForumList != null && subForumList.Count > 0)
            {
                foreach (IXForum current in subForumList)
                {
                    if (config.Hideprivate != 1 || !(current.ViewPerm != "") || Utils.InArray(usergroupid.ToString(), current.ViewPerm))
                    {
                        sb.Append(string.Format("{{'forumname':'{0}','fid':{1},'parentid':{2},'applytopictype':{3},'topictypeselectoptions':'{4}','postbytopictype':{5}}},", new object[]
                        {
                            current.Name.Trim(),
                            current.ID,
                            current.ParentID,
                            current.ApplytopicType,
                            BBX.Forum.Forums.GetCurrentTopicTypesOption(current.ID, current.TopicTypes),
                            current.PostbytopicType
                        }));
                    }
                }
                if (sb.ToString() != "")
                {
                    ResponseJSON(sb.ToString().Remove(sb.ToString().Length - 1) + "]");
                }
            }
            ResponseJSON(sb.Append("]").ToString());
        }

        private StringBuilder IsValidGetPostInfo(Post info)
        {
            var sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");
            if (!DNTRequest.IsPost() || ForumUtils.IsCrossSitePost())
            {
                sb.Append("<error>您的请求来路不正确，无法提交。如果您安装了某种默认屏蔽来路信息的个人防火墙软件(如 Norton Internet Security)，请设置其不要禁止来路信息后再试。</error>");
                return sb;
            }
            if (info == null)
            {
                sb.Append("<error>读取帖子失败</error>");
                return sb;
            }
            return sb;
        }

        private void GetDebatePostPage()
        {
            int opinion = DNTRequest.GetInt("opinion", 0);
            int page = DNTRequest.GetInt("page", 1);
            int tid = DNTRequest.GetInt("tid", 0);
            int num2 = 0;
            int debatepagesize = config.Debatepagesize;
            var topicInfo = Topic.FindByID(tid);
            int hide = 1;
            if (topicInfo == null)
            {
                string text = "该主题不存在";
                ResponseText(text);
                return;
            }
            var forumInfo = BBX.Forum.Forums.GetForumInfo(topicInfo.Fid);
            if (useradminid != 0)
            {
                num2 = (BBX.Forum.Moderators.IsModer(useradminid, userid, forumInfo.ID) ? 1 : 0);
                //var adminGroupInfo = AdminGroup.FindByID(usergroupid);
                //if (adminGroupInfo != null)
                //{
                //    byte arg_B7_0 = adminGroupInfo.DisablePostctrl;
                //}
            }
            if (topicInfo.Hide == 1 && (Post.IsReplier(tid, userid) || num2 == 1))
            {
                hide = -1;
            }
            if (topicInfo.Closed > 1)
            {
                tid = topicInfo.Closed;
                //topicInfo = BBX.Forum.Topics.GetTopicInfo(tid);
                topicInfo = Topic.FindByID(tid);
                if (topicInfo == null || topicInfo.Closed > 1)
                {
                    ResponseText("不存在的主题ID");
                    return;
                }
            }
            if (topicInfo.Deleted)
            {
                ResponseText("此主题已被删除！");
                return;
            }
            if (topicInfo.NotAudited)
            {
                ResponseText("此主题未经审核！");
                return;
            }
            if (forumInfo.Password != "" && Utils.MD5(forumInfo.Password) != ForumUtils.GetCookie("forum" + forumInfo.ID + "password"))
            {
                ResponseText("本版块被管理员设置了密码");
                Response.Redirect(BaseConfigs.GetForumPath + "showforum-" + forumInfo.ID + config.Extname, true);
                return;
            }
            string text2 = "";
            if (!UserAuthority.VisitAuthority(forumInfo, usergroupinfo, userid, ref text2))
            {
                ResponseText(text2);
                return;
            }
            if (topicInfo.Special != 4)
            {
                ResponseText("本主题不是辩论主题");
                return;
            }
            var postpramsInfo = new PostpramsInfo();
            postpramsInfo.Fid = forumInfo.ID;
            postpramsInfo.Tid = tid;
            postpramsInfo.Jammer = forumInfo.Jammer;
            postpramsInfo.Pagesize = debatepagesize;
            postpramsInfo.Pageindex = page;
            postpramsInfo.Getattachperm = forumInfo.GetattachPerm;
            postpramsInfo.Usergroupid = usergroupid;
            postpramsInfo.Attachimgpost = config.Attachimgpost;
            postpramsInfo.Showattachmentpath = config.Showattachmentpath;
            postpramsInfo.Hide = hide;
            postpramsInfo.Price = 0;
            postpramsInfo.Usergroupreadaccess = ((num2 == 1) ? 2147483647 : usergroupinfo.Readaccess);
            postpramsInfo.CurrentUserid = userid;
            postpramsInfo.Showimages = forumInfo.AllowImgCode ? 1 : 0;
            postpramsInfo.Smiliesinfo = BBX.Forum.Smilies.GetSmiliesListWithInfo();
            postpramsInfo.Customeditorbuttoninfo = BBX.Forum.Editors.GetCustomEditButtonListWithInfo();
            postpramsInfo.Smiliesmax = config.Smiliesmax;
            postpramsInfo.Bbcodemode = config.Bbcodemode;
            postpramsInfo.CurrentUserGroup = usergroupinfo;
            var value = new List<Post>();
            if (opinion == 1)
            {
                List<Attachment> list;
                value = BBX.Forum.Debates.GetPositivePostList(postpramsInfo, out list, num2 == 1);
            }
            else
            {
                if (opinion == 2)
                {
                    List<Attachment> list2;
                    value = BBX.Forum.Debates.GetNegativePostList(postpramsInfo, out list2, num2 == 1);
                }
            }
            var debateTopic = Debate.FindByTid(topicInfo.ID);
            int debatesPostCount = PostDebateField.FindCountByTidAndOpinion(postpramsInfo.Tid, 1);
            int debatesPostCount2 = PostDebateField.FindCountByTidAndOpinion(postpramsInfo.Tid, 2);
            int countPage = (debatesPostCount % debatepagesize == 0) ? (debatesPostCount / debatepagesize) : (debatesPostCount / debatepagesize + 1);
            int countPage2 = (debatesPostCount2 % debatepagesize == 0) ? (debatesPostCount2 / debatepagesize) : (debatesPostCount2 / debatepagesize + 1);
            bool flag = debateTopic.Terminaled;
            int num3 = (forumInfo.AllowBbCode && usergroupinfo.AllowCusbbCode) ? 0 : 1;
            int num4 = forumInfo.AllowSmilies ? 0 : 1;
            int num5 = 0;
            var stringBuilder = new StringBuilder("{\"postlist\":");
            stringBuilder.Append(value.ToJson());
            stringBuilder.Append(",'debateexpand':");
            stringBuilder.Append(debateTopic.ToJson());
            stringBuilder.Append(",'pagenumbers':'");
            if (opinion == 1)
            {
                stringBuilder.Append(Utils.GetAjaxPageNumbers(postpramsInfo.Pageindex, countPage, "showdebatepage(\\'" + forumpath + "tools/ajax.ashx?t=getdebatepostpage&opinion=1&tid=" + topicInfo.ID + "&{0}\\'," + num5 + ", " + num4 + ", " + num3 + ",\\'" + flag + "\\',1," + userid + "," + tid + ")", 8));
            }
            else
            {
                stringBuilder.Append(Utils.GetAjaxPageNumbers(postpramsInfo.Pageindex, countPage2, "showdebatepage(\\'" + forumpath + "tools/ajax.ashx?t=getdebatepostpage&opinion=2&tid=" + topicInfo.ID + "&{0}\\'," + num5 + ", " + num4 + ", " + num3 + ",\\'" + flag + "\\',2," + userid + "," + tid + ")", 8));
            }
            stringBuilder.Append("'}");
            ResponseText(stringBuilder);
        }

        private void ResponseText(string text)
        {
            Response.Clear();
            Response.Write(text);
            Response.End();
        }

        private void ResponseText(StringBuilder builder)
        {
            ResponseText(builder.ToString());
        }

        private void DiggDebates()
        {
            StringBuilder stringBuilder = IsValidDebates(DNTRequest.GetInt("tid", 0), DNTRequest.GetInt("pid", 0), DNTRequest.GetInt("type", -1));
            if (!stringBuilder.ToString().Contains("<error>"))
            {
                BBX.Forum.Debates.AddDebateDigg(DNTRequest.GetInt("tid", 0), DNTRequest.GetInt("pid", 0), DNTRequest.GetInt("type", -1), userid);
                if (UserGroup.Guest.AllowDiggs)
                {
                    BBX.Forum.Debates.WriteCookies(DNTRequest.GetInt("pid", 0));
                }
            }
            ResponseXML(stringBuilder);
        }

        private void Detatevote()
        {
            int stand = DNTRequest.GetInt("stand", 0);
            int tid = DNTRequest.GetInt("tid", 0);
            //string text = BBX.Forum.Debates.CheckDebateVoter(tid, 1);
            //string text2 = BBX.Forum.Debates.CheckDebateVoter(tid, 2);
            var db = Debate.FindByTid(tid);
            var text = db.PositiveVoterids;
            var text2 = db.NegativeVoterids;
            if (Utils.IsEqualStr(userid.ToString(), text, ",") || Utils.IsEqualStr(userid.ToString(), text2, ","))
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("<root>您已投过票</root>");
                ResponseXML(stringBuilder);
                return;
            }
            string text3 = (stand == 1) ? text : text2;
            text3 = text3 + "," + userid.ToString();
            Debate.InsertDebateVoter(tid, text3.Trim(','), stand);
            var sb = new StringBuilder();
            sb.Append("<root>投票成功</root>");
            ResponseXML(sb);
        }

        private StringBuilder IsValidDebates(int tid, int pid, int CountenanceType)
        {
            var sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");
            if (!DNTRequest.IsPost() || ForumUtils.IsCrossSitePost())
            {
                sb.Append("<error>您的请求来路不正确，无法提交。如果您安装了某种默认屏蔽来路信息的个人防火墙软件(如 Norton Internet Security)，请设置其不要禁止来路信息后再试。</error>");
                return sb;
            }
            if (!BBX.Forum.Debates.AllowDiggs(userid))
            {
                sb.Append("<error>您所在的用户组不允许此操作</error>");
                return sb;
            }
            var topicInfo = Topic.FindByID(tid);
            if (tid == 0 || topicInfo.Special != 4 || pid == 0)
            {
                sb.Append("<error>本主题不是辩论帖，无法支持</error>");
                return sb;
            }
            if (Debate.FindByTid(tid).Terminaled)
            {
                sb.Append("<error>本辩论帖结束时间已到，无法再参与</error>");
                return sb;
            }
            if (CountenanceType != 1 && CountenanceType != 2)
            {
                sb.Append("<error>支持方不能为空</error>");
                return sb;
            }
            if (BBX.Forum.Debates.IsDigged(pid, userid))
            {
                sb.Append("<error>投过票了</error>");
                return sb;
            }
            return sb;
        }

        private bool IsQuickReplyValid(StringBuilder xmlnode, int topicid, Topic topic, IXForum forum, AdminGroup admininfo, string postmessage)
        {
            if (!DNTRequest.IsPost() || ForumUtils.IsCrossSitePost())
            {
                xmlnode.Append("<error>您的请求来路不正确，无法提交。如果您安装了某种默认屏蔽来路信息的个人防火墙软件(如 Norton Internet Security)，请设置其不要禁止来路信息后再试。</error>");
                return false;
            }
            if (topicid == -1)
            {
                xmlnode.Append("<error>无效的主题ID</error>");
                return false;
            }
            if (topic == null)
            {
                xmlnode.Append("<error>不存在的主题ID</error>");
                return false;
            }
            string str = "";
            if (!UserAuthority.CheckPostTimeSpan(usergroupinfo, admininfo, oluserinfo, BBX.Entity.User.FindByID(oluserinfo.UserID), ref str))
            {
                xmlnode.AppendFormat("<error>" + str + "</error>", new object[0]);
                return false;
            }
            if ((admininfo == null || !BBX.Forum.Moderators.IsModer(admininfo.ID, userid, forum.ID)) && topic.Closed == 1)
            {
                xmlnode.Append("<error>主题已关闭无法回复</error>");
                return false;
            }
            if (!UserAuthority.PostReply(forum, userid, usergroupinfo, topic))
            {
                xmlnode.AppendFormat("<error>" + ((topic.Closed == 1) ? "主题已关闭无法回复" : "您没有发表回复的权限") + "</error>", new object[0]);
                return false;
            }
            if (DNTRequest.GetString(config.Antispamposttitle).IndexOf("\u3000") != -1)
            {
                xmlnode.Append("<error>主题不能包含全角空格符</error>");
                return false;
            }
            if (DNTRequest.GetString(config.Antispamposttitle).Length > 60)
            {
                xmlnode.AppendFormat("<error>主题最大长度为60个字符,当前为 {0} 个字符</error>", DNTRequest.GetString(config.Antispamposttitle).Length.ToString());
                return false;
            }
            if (postmessage.IsNullOrEmpty())
            {
                xmlnode.Append("<error>内容不能为空</error>");
                return false;
            }
            if (admininfo != null && !admininfo.DisablePostctrl)
            {
                if (postmessage.Length < config.Minpostsize)
                {
                    xmlnode.AppendFormat("<error>您发表的内容过少, 系统设置要求帖子内容不得少于 {0} 字多于 {1} 字</error>", config.Minpostsize.ToString(), config.Maxpostsize.ToString());
                    return false;
                }
                if (postmessage.Length > config.Maxpostsize)
                {
                    xmlnode.AppendFormat("<error>您发表的内容过多, 系统设置要求帖子内容不得少于 {0} 字多于 {1} 字</error>", config.Minpostsize.ToString(), config.Maxpostsize.ToString());
                    return false;
                }
            }
            if (topic.Special == 4 && DNTRequest.GetInt("debateopinion", 0) == 0)
            {
                xmlnode.AppendFormat("<error>请选择您在辩论中的观点</error>", new object[0]);
                return false;
            }
            var shortUserInfo = BBX.Entity.User.FindByID(userid);

            if (config.DisablePostAD && useradminid < 1 && ((config.DisablePostADPostCount != 0 && shortUserInfo.Posts <= config.DisablePostADPostCount) || (config.DisablePostADRegMinute != 0 && DateTime.Now.AddMinutes((double)(-(double)config.DisablePostADRegMinute)) <= shortUserInfo.JoinDate.ToDateTime())))
            {
                string[] array = config.DisablePostADRegular.Replace("\r", "").Split('\n');
                for (int i = 0; i < array.Length; i++)
                {
                    string regular = array[i];
                    if (BBX.Forum.Posts.IsAD(regular, DNTRequest.GetString(config.Antispamposttitle), postmessage))
                    {
                        xmlnode.AppendFormat("<error>发帖失败，内容中有不符合新用户强力广告屏蔽规则的字符，请检查标题和内容，如有疑问请与管理员联系</error>", new object[0]);
                        return false;
                    }
                }
            }
            return true;
        }

        private void QuickReply()
        {
            var sb = new StringBuilder("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");
            int topicid = DNTRequest.GetInt("topicid", -1);
            int num = 0;
            string topictitle = "";
            int layer = 1;
            int parentid = 0;

            var toreplay_user = (HttpContext.Current.Request["toreplay_user"] + "").Trim();
            if (!String.IsNullOrEmpty(toreplay_user)) toreplay_user += "\n\n";
            var msg = toreplay_user + DNTRequest.GetString(config.Antispampostmessage).Trim();
            //string msg = ((!DNTRequest.GetString("toreplay_user").IsNullOrEmpty()) ? (DNTRequest.GetString("toreplay_user").Trim() + "\n\n") : "") + DNTRequest.GetString(config.Antispampostmessage).TrimEnd(new char[0]);
            string title = DNTRequest.GetString(config.Antispamposttitle).Trim();
            if (title != "" && Utils.GetCookie("lastposttitle") == Utils.MD5(title) || Utils.GetCookie("lastpostmessage") == Utils.MD5(msg))
            {
                ResponseXML(sb.Append("<error>请勿重复发帖</error>"));
                return;
            }
            if (useradminid != 1 && (ForumUtils.HasBannedWord(title) || ForumUtils.HasBannedWord(msg)))
            {
                string arg = ForumUtils.GetBannedWord(title) == string.Empty ? ForumUtils.GetBannedWord(msg) : ForumUtils.GetBannedWord(title);
                ResponseXML(sb.Append(string.Format("<error>对不起, 您提交的内容包含不良信息 ({0}), 因此无法提交, 请返回修改!</error>", arg)));
                return;
            }
            var topic = Topic.FindByID(topicid);
            int fid = topic.Fid;
            IXForum forumInfo = XForum.FindByID(fid);
            if (forumInfo == null || forumInfo.ID < 1 || forumInfo.Layer == 0)
            {
                ResponseXML(sb.Append("<error>版块信息无效!</error>"));
                return;
            }
            forumInfo.Name.Trim();
            var adminGroupInfo = AdminGroup.FindByID(usergroupid);
            var disablepost = adminGroupInfo != null ? adminGroupInfo.DisablePostctrl : config.DisablePostAD;
            bool flag = UserAuthority.NeedAudit(forumInfo.ID, forumInfo.Modnewposts, useradminid, userid, usergroupinfo, topic);
            if (flag && topic.DisplayOrder == -2)
            {
                ResponseXML(sb.Append("<error>主题尚未通过审核, 不能执行回复操作!</error>"));
                return;
            }
            if (!IsQuickReplyValid(sb, topicid, topic, forumInfo, adminGroupInfo, msg))
            {
                ResponseXML(sb);
                return;
            }
            int postid = DNTRequest.GetInt("postid", -1);
            int replyuserid = 0;

            var pi = new Post();
            if (postid > 0 && DNTRequest.GetString("postreplynotice") == "true")
            {
                //pi = BBX.Forum.Posts.GetPostInfo(topicid, postid);
                pi = Post.FindByID(postid);
                if (pi == null)
                {
                    ResponseXML(sb.Append("<error>无效的帖子ID!</error>"));
                    return;
                }
                if (topicid != pi.Tid)
                {
                    ResponseXML(sb.Append("<error>主题ID无效!</error>"));
                    return;
                }
                replyuserid = pi.PosterID;
            }
            else
            {
                replyuserid = topic.PosterID;
            }
            bool flag2 = DNTRequest.GetInt("htmlon") == 1;
            var msg2 = pi.Message;
            if (useradminid == 1)
            {
                pi.Title = Utils.HtmlEncode(title);
                msg2 = ((!usergroupinfo.AllowHtml) ? Utils.HtmlEncode(msg) : (flag2 ? msg : Utils.HtmlEncode(msg)));
            }
            else
            {
                pi.Title = Utils.HtmlEncode(ForumUtils.BanWordFilter(title));
                if (!usergroupinfo.AllowHtml)
                    msg2 = Utils.HtmlEncode(ForumUtils.BanWordFilter(msg));
                else
                    msg2 = (flag2 ? ForumUtils.BanWordFilter(msg) : Utils.HtmlEncode(ForumUtils.BanWordFilter(msg)));
            }
            pi.Message = msg2;
            //pi.Title = topicInfo.Title;
            pi.Fid = fid;
            pi.Tid = topicid;
            pi.ParentID = parentid;
            pi.Layer = layer;
            pi.Poster = username;
            pi.PosterID = userid;
            //pi.Postdatetime = Utils.GetDateTime();
            //pi.Ip = WebHelper.UserHost;
            //pi.Lastedit = "";
            //if (adminGroupInfo != null)
            //{
            //    byte arg_3C8_0 = adminGroupInfo.DisablePostctrl;
            //}
            pi.Invisible = (flag ? 1 : 0);
            if (pi.Invisible != 1 && useradminid != 1 && (Scoresets.BetweenTime(config.Postmodperiods) || ForumUtils.HasAuditWord(pi.Title) || ForumUtils.HasAuditWord(pi.Message)))
            {
                pi.Invisible = 1;
            }
            pi.UseSig = DNTRequest.GetInt("usesig");
            pi.HtmlOn = (usergroupinfo.AllowHtml && flag2) ? 1 : 0;
            pi.SmileyOff = forumInfo.AllowSmilies ? DNTRequest.GetInt("smileyoff") : 1;
            pi.BBCodeOff = ((usergroupinfo.AllowCusbbCode && forumInfo.AllowBbCode) ? DNTRequest.GetInt("bbcodeoff") : 0);
            pi.ParseUrlOff = DNTRequest.GetInt("parseurloff");
            //pi.Attachment = 0;
            //pi.Rate = 0;
            //pi.Ratetimes = 0;
            //pi.Debateopinion = DNTRequest.GetInt("debateopinion", 0);
            var debateopinion = DNTRequest.GetInt("debateopinion", 0);
            try
            {
                //num = BBX.Forum.Posts.CreatePost(pi);
                pi.Create();
                num = pi.ID;

                if (debateopinion > 0)
                {
                    Debate.CreateDebateExpandInfo(pi.Tid, pi.ID, debateopinion, 0);
                }
                Utils.WriteCookie("lastposttitle", Utils.MD5(title));
                Utils.WriteCookie("lastpostmessage", Utils.MD5(msg));
            }
            catch
            {
                ResponseXML(sb.Append("<error>提交失败,请稍后重试！</error>"));
                return;
            }
            var stringBuilder2 = new StringBuilder();
            string formString = DNTRequest.GetFormString("attachid");
            if (!string.IsNullOrEmpty(formString))
            {
                var noUsedAttachmentArray = BBX.Forum.Attachments.GetNoUsedAttachmentArray(userid, formString);
                BBX.Forum.Attachments.UpdateAttachment(noUsedAttachmentArray, topic.ID, num, pi, ref stringBuilder2, userid, config, usergroupinfo);
            }
            Online.UpdateAction(oluserinfo.ID, UserAction.PostReply, fid, forumInfo.Name, topicid, topictitle);
            if (pi.Invisible != 1)
            {
                if (pi.ID > 0 && DNTRequest.GetString("postreplynotice") == "true")
                {
                    //pi.Pid = num;
                    Notice.SendPostReplyNotice(pi, topic, replyuserid);
                }
                Sync.Reply(num.ToString(), topic.ID.ToString(), topic.Title, pi.Poster, pi.PosterID.ToString(), topic.Fid.ToString(), "");
                int num3 = (ForumUtils.IsHidePost(msg) && usergroupinfo.AllowHideCode) ? 1 : 0;
                if (num3 == 1)
                {
                    topic.Hide = num3;
                    //BBX.Forum.Topics.UpdateTopicHide(topicid);
                    topic.Save();
                }
                if (BBX.Forum.Moderators.IsModer(useradminid, userid, topic.Fid) && topic.Attention == 1)
                {
                    Topic.UpdateTopicAttentionByTidList(0, topicid);
                }
                else
                {
                    if (topic.PosterID != -1 && userid == topic.PosterID)
                    {
                        Topic.UpdateTopicAttentionByTidList(1, topicid);
                    }
                }
                BBX.Forum.Topics.UpdateTopicReplyCount(topicid);
                CreditsFacade.PostReply(userid, forumInfo.ReplycrEdits, true);
                sb = GetNewPostXML(sb, pi, forumInfo, topic, num, debateopinion);
                if (topic.Replies < config.Ppp + 10)
                {
                    ForumUtils.DeleteTopicCacheFile(topicid);
                }
                ResponseXML(sb);
                return;
            }
            ResponseXML(sb.Append("<error>发表回复成功, 但需要经过审核才可以显示!</error>"));
        }

        private StringBuilder GetNewPostXML(StringBuilder xmlnode, Post pi, IXForum forum, Topic topic, int postid, Int32 debateopinion)
        {
            int hide = (topic.Hide == 1 || ForumUtils.IsHidePost(pi.Message)) ? -1 : 1;
            if (!usergroupinfo.AllowHideCode) hide = 0;

            int price = 0;
            if (topic.Price > 0) price = (PaymentLog.IsBuyer(topic.ID, userid) ? -1 : topic.Price);

            var ppi = new PostpramsInfo();
            ppi.Fid = forum.Fid;
            ppi.Tid = pi.Tid;
            ppi.Pid = pi.ID;
            ppi.Jammer = forum.Jammer;
            ppi.Pagesize = 1;
            ppi.Pageindex = 1;
            ppi.Getattachperm = forum.GetattachPerm;
            ppi.Usergroupid = usergroupid;
            ppi.Attachimgpost = config.Attachimgpost;
            ppi.Showattachmentpath = config.Showattachmentpath;
            ppi.Hide = hide;
            ppi.Price = price;
            ppi.Ubbmode = false;
            ppi.Showimages = forum.AllowImgCode ? 1 : 0;
            ppi.Smiliesinfo = BBX.Forum.Smilies.GetSmiliesListWithInfo();
            ppi.Customeditorbuttoninfo = BBX.Forum.Editors.GetCustomEditButtonListWithInfo();
            ppi.Smiliesmax = config.Smiliesmax;
            ppi.Bbcodemode = config.Bbcodemode;
            ppi.Smileyoff = pi.SmileyOff;
            ppi.BBCode = pi.BBCodeOff < 1;
            ppi.Parseurloff = pi.ParseUrlOff;
            ppi.Allowhtml = pi.HtmlOn;
            ppi.Sdetail = pi.Message;
            ppi.CurrentUserid = userid;
            User userInfo = BBX.Forum.Users.GetUserInfo(ppi.CurrentUserid);
            ppi.CurrentUserGroup = UserGroup.FindByID(userInfo.GroupID);
            ppi.Usercredits = ((userInfo == null) ? 0 : userInfo.Credits);

            var sb = new StringBuilder(UBB.UBBToHTML(ppi), 3000);
            Regex regex = new Regex("\\[attach\\](\\d+?)\\[\\/attach\\]", RegexOptions.IgnoreCase);
            new Regex("\\s*\\[hide\\][\\n\\r]*([\\s\\S]+?)[\\n\\r]*\\[\\/hide\\]\\s*", RegexOptions.IgnoreCase);
            Regex regex2 = new Regex("\\[attachimg\\](\\d+?)\\[\\/attachimg\\]", RegexOptions.IgnoreCase);
            var attachmentList = BBX.Forum.Attachments.GetAttachmentList(ppi, pi.ID.ToString());
            pi.Attachment = attachmentList.Count;
            if (pi.Attachment > 0 || regex.IsMatch(sb.ToString()) || regex2.IsMatch(sb.ToString()))
            {
                string[] hiddenAttachIdList = SessionAjaxPage.GetHiddenAttachIdList(ppi.Sdetail, ppi.Hide);
                var list = new List<Attachment>();
                int allowGetAttachValue = SessionAjaxPage.GetAllowGetAttachValue(ppi);
                //var pi2 = new ShowtopicPagePostInfo();
                //pi2.Posterid = pi.PosterID;
                //pi2.Pid = pi.ID;
                foreach (var att in attachmentList)
                {
                    sb = new StringBuilder(BBX.Forum.Attachments.GetMessageWithAttachInfo(ppi, allowGetAttachValue, hiddenAttachIdList, pi.ID, pi.PosterID, att, sb.ToString()), 300);
                    if (att.Inserted || Utils.InArray(att.ID.ToString(), hiddenAttachIdList))
                    {
                        list.Add(att);
                    }
                }
                foreach (var att in list)
                {
                    attachmentList.Remove(att);
                }
                pi.Message = sb.ToString();
                if (attachmentList.Count > 0)
                {
                    sb.Append("<div class=\"postattachlist\"><div id=\"BOX_overlay\" style=\"background: #000; position: absolute; z-index:100; filter:alpha(opacity=50);-moz-opacity: 0.6;opacity: 0.6;\"></div><div id=\"attachpaymentlog\" style=\"display: none; background :Aliceblue;  border:0px solid #999; width:503px; height:443px;\"></div><div id=\"buyattach\" style=\"display: none; background :Aliceblue; border:0px solid #999; width:503px; height:323px;\"></div>");
                    foreach (var att in attachmentList)
                    {
                        if (att.ImgPost)
                            sb.Append("<dl class=\"t_attachlist_img attachimg cl\">");
                        else
                            sb.Append("<dl class=\"t_attachlist attachimg cl\">");

                        if (att.ImgPost)
                            sb.Append("<dt></dt>");
                        else
                        {
                            if (att.FileName.EndsWithIgnoreCase("rar", "zip"))
                                sb.Append("<dt><img class=\"absmiddle\" border=\"0\" src=\"images/attachicons/rar.gif\"/></dt>");
                            else
                                sb.Append("<dt><img class=\"absmiddle\" border=\"0\" src=\"images/attachicons/attachment.gif\"/></dt>");
                        }
                        sb.AppendFormat("<dd><a target=\"_blank\" onclick=\"return ShowDownloadTip({0});\" href=\"attachment.aspx?attachmentid={1}\" class=\"xg2\">{2}</a><em class=\"xg1\">(<script type=\"text/javascript\">ShowFormatBytesStr({3});</script>, 下载次数:{4})</em><p><span style=\"color:#666\">({5} 上传)</span></p>", new object[]
                        {
                            pi.PosterID,
                            att.ID,
                            att.Name,
                            att.FileSize,
                            att.Downloads,
                            att.PostDateTime
                        });
                        if (att.Preview != "")
                        {
                            sb.AppendFormat("<p>{0}</p>", att.Preview);
                        }
                        sb.AppendFormat("<p><a name=\"attach{0}\"></a>", att.ID);
                        if (config.Showimages == 1 && att.ImgPost)
                        {
                            if (config.Showimgattachmode == 0)
                            {
                                sb.AppendFormat("<img imageid=\"{0}\" alt=\"{1}\" ", att.ID, att.Name);
                                if (config.Showattachmentpath == 1)
                                {
                                    sb.AppendFormat("src=\"{0}\"", !att.IsLocal ? att.FileName : ("attachment.aspx?attachmentid=" + att.ID));
                                }
                                else
                                {
                                    sb.AppendFormat("src=\"attachment.aspx?attachmentid={0}\"", att.ID);
                                }
                                sb.Append(" onmouseover=\"attachimg(this, 'mouseover')\" onload=\"attachimg(this, 'load');\" onclick=\"zoom(this, src);\" />");
                            }
                            else
                            {
                                sb.AppendFormat("<img imageid=\"{0}\" alt=\"点击加载图片\" ", att.ID);
                                if (config.Showattachmentpath == 1)
                                {
                                    sb.AppendFormat("newsrc=\"{0}\"", !att.IsLocal ? att.FileName : ("upload/" + att.FileName));
                                }
                                else
                                {
                                    sb.AppendFormat("newsrc=\"attachment.aspx?attachmentid={0}\"", att.ID);
                                }
                                sb.Append("src=\"/images/common/imgloading.png\" onload=\"attachimg(this, 'load');\" onclick=\"loadImg(this);\" />");
                            }
                        }
                        sb.Append("</p></dd></dl>");
                    }
                }
            }
            if (userid == -1)
            {
                xmlnode.Append("<post>\r\n\t");
                xmlnode.AppendFormat("<id>{0}</id>\r\n\t", topic.Replies + 2);
                xmlnode.AppendFormat("<postdatetime>{0}</postdatetime>\r\n\t", pi.PostDateTime.ToFullString());
                xmlnode.AppendFormat("<message><![CDATA[{0}]]></message>\r\n\t", sb);
                xmlnode.AppendFormat("<olimg><![CDATA[{0}]]></olimg>\r\n", Online.GetGroupImg(7));
                xmlnode.AppendFormat("<location>{0}</location>\r\n", Utils.SplitString(IPAddress.Parse(pi.IP).GetAddress(), " ")[0] + "网友");
                xmlnode.Append("</post>\r\n");
                return xmlnode;
            }
            var userInfo2 = BBX.Forum.Users.GetUserInfo(userid);
            Advertisement.GetInPostAdCount("", pi.Fid);
            new Random((int)DateTime.Now.Ticks);
            var userGroupInfo = UserGroup.FindByID(usergroupid);
            string arg = (!userGroupInfo.Color.IsNullOrEmpty()) ? "<font color=\"" + userGroupInfo.Color + "\">" + userGroupInfo.GroupTitle + "</font>" : userGroupInfo.GroupTitle;
            string arg2 = userInfo2.Field.Medals.IsNullOrEmpty() ? "" : Medal.GetMedalsList(userInfo2.Field.Medals);
            xmlnode.Append("<post>\r\n\t");
            xmlnode.AppendFormat("<ismoder>{0}</ismoder>", BBX.Forum.Moderators.IsModer(useradminid, userid, topic.Fid) ? 1 : 0);
            xmlnode.AppendFormat(Advertisement.GetInPostAdXMLByFloor("", pi.Fid, (topic.Replies + 2) % 15), new object[0]);
            xmlnode.AppendFormat("<id>{0}</id>\r\n\t", topic.Replies + 2);
            xmlnode.AppendFormat("<status><![CDATA[{0}]]></status>\r\n\t", arg);
            xmlnode.AppendFormat("<stars>{0}</stars>\r\n\t", userGroupInfo.Stars);
            xmlnode.AppendFormat("<fid>{0}</fid>\r\n\t", pi.Fid);
            xmlnode.AppendFormat("<invisible>{0}</invisible>\r\n\t", pi.Invisible);
            xmlnode.AppendFormat("<ip>{0}</ip>\r\n\t", pi.IP);
            xmlnode.AppendFormat("<lastedit>{0}</lastedit>\r\n\t", pi.LastEdit);
            xmlnode.AppendFormat("<layer>{0}</layer>\r\n\t", pi.Layer);
            xmlnode.AppendFormat("<message><![CDATA[{0}]]></message>\r\n\t", sb.ToString());
            xmlnode.AppendFormat("<parentid>{0}</parentid>\r\n\t", pi.ParentID);
            xmlnode.AppendFormat("<pid>{0}</pid>\r\n\t", postid);
            xmlnode.AppendFormat("<postdatetime>{0}</postdatetime>\r\n\t", pi.PostDateTime.ToFullString());
            xmlnode.AppendFormat("<poster>{0}</poster>\r\n\t", pi.Poster);
            xmlnode.AppendFormat("<posterid>{0}</posterid>\r\n\t", pi.PosterID);
            xmlnode.AppendFormat("<smileyoff>{0}</smileyoff>\r\n\t", pi.SmileyOff);
            xmlnode.AppendFormat("<topicid>{0}</topicid>\r\n\t", pi.Tid);
            xmlnode.AppendFormat("<title>{0}</title>\r\n\t", Utils.HtmlEncode(pi.Title));
            xmlnode.AppendFormat("<usesig>{0}</usesig>\r\n", pi.UseSig);
            xmlnode.AppendFormat("<debateopinion>{0}</debateopinion>", debateopinion);
            xmlnode.AppendFormat("<uid>{0}</uid>\r\n\t", userInfo2.ID);
            xmlnode.AppendFormat("<accessmasks>{0}</accessmasks>\r\n\t", userInfo2.AccessMasks);
            xmlnode.AppendFormat("<adminid>{0}</adminid>\r\n\t", userInfo2.AdminID);
            xmlnode.AppendFormat("<bday>{0}</bday>\r\n\t", userInfo2.Bday);
            xmlnode.AppendFormat("<credits>{0}</credits>\r\n\t", userInfo2.Credits);
            xmlnode.AppendFormat("<digestposts>{0}</digestposts>\r\n\t", userInfo2.DigestPosts);
            xmlnode.AppendFormat("<email>{0}</email>\r\n\t", userInfo2.Email);
            string[] validScoreName = Scoresets.GetValidScoreName();
            xmlnode.AppendFormat("<score1>{0}</score1>\r\n\t", validScoreName[1]);
            xmlnode.AppendFormat("<score2>{0}</score2>\r\n\t", validScoreName[2]);
            xmlnode.AppendFormat("<score3>{0}</score3>\r\n\t", validScoreName[3]);
            xmlnode.AppendFormat("<score4>{0}</score4>\r\n\t", validScoreName[4]);
            xmlnode.AppendFormat("<score5>{0}</score5>\r\n\t", validScoreName[5]);
            xmlnode.AppendFormat("<score6>{0}</score6>\r\n\t", validScoreName[6]);
            xmlnode.AppendFormat("<score7>{0}</score7>\r\n\t", validScoreName[7]);
            xmlnode.AppendFormat("<score8>{0}</score8>\r\n\t", validScoreName[8]);
            string[] validScoreUnit = Scoresets.GetValidScoreUnit();
            xmlnode.AppendFormat("<scoreunit1>{0}</scoreunit1>\r\n\t", validScoreUnit[1]);
            xmlnode.AppendFormat("<scoreunit2>{0}</scoreunit2>\r\n\t", validScoreUnit[2]);
            xmlnode.AppendFormat("<scoreunit3>{0}</scoreunit3>\r\n\t", validScoreUnit[3]);
            xmlnode.AppendFormat("<scoreunit4>{0}</scoreunit4>\r\n\t", validScoreUnit[4]);
            xmlnode.AppendFormat("<scoreunit5>{0}</scoreunit5>\r\n\t", validScoreUnit[5]);
            xmlnode.AppendFormat("<scoreunit6>{0}</scoreunit6>\r\n\t", validScoreUnit[6]);
            xmlnode.AppendFormat("<scoreunit7>{0}</scoreunit7>\r\n\t", validScoreUnit[7]);
            xmlnode.AppendFormat("<scoreunit8>{0}</scoreunit8>\r\n\t", validScoreUnit[8]);
            xmlnode.AppendFormat("<extcredits1>{0}</extcredits1>\r\n\t", userInfo2.ExtCredits1);
            xmlnode.AppendFormat("<extcredits2>{0}</extcredits2>\r\n\t", userInfo2.ExtCredits2);
            xmlnode.AppendFormat("<extcredits3>{0}</extcredits3>\r\n\t", userInfo2.ExtCredits3);
            xmlnode.AppendFormat("<extcredits4>{0}</extcredits4>\r\n\t", userInfo2.ExtCredits4);
            xmlnode.AppendFormat("<extcredits5>{0}</extcredits5>\r\n\t", userInfo2.ExtCredits5);
            xmlnode.AppendFormat("<extcredits6>{0}</extcredits6>\r\n\t", userInfo2.ExtCredits6);
            xmlnode.AppendFormat("<extcredits7>{0}</extcredits7>\r\n\t", userInfo2.ExtCredits7);
            xmlnode.AppendFormat("<extcredits8>{0}</extcredits8>\r\n\t", userInfo2.ExtCredits8);
            xmlnode.AppendFormat("<extgroupids>{0}</extgroupids>\r\n\t", (userInfo2.ExtGroupIds + "").Trim());
            xmlnode.AppendFormat("<gender>{0}</gender>\r\n\t", userInfo2.Gender);
            xmlnode.AppendFormat("<icq>{0}</icq>\r\n\t", userInfo2.Field.Icq);
            xmlnode.AppendFormat("<joindate>{0}</joindate>\r\n\t", userInfo2.JoinDate);
            xmlnode.AppendFormat("<lastactivity>{0}</lastactivity>\r\n\t", userInfo2.LastActivity);
            xmlnode.AppendFormat("<medals><![CDATA[{0}]]></medals>\r\n\t", arg2);
            xmlnode.AppendFormat("<nickname>{0}</nickname>\r\n\t", userInfo2.NickName);
            xmlnode.AppendFormat("<oltime>{0}</oltime>\r\n\t", userInfo2.OLTime);
            xmlnode.AppendFormat("<onlinestate>{0}</onlinestate>\r\n\t", userInfo2.OnlineState);
            xmlnode.AppendFormat("<showemail>{0}</showemail>\r\n\t", userInfo2.ShowEmail);

            var uf = userInfo2.Field;
            xmlnode.AppendFormat("<signature><![CDATA[{0}]]></signature>\r\n\t", uf.Sightml);
            xmlnode.AppendFormat("<sigstatus>{0}</sigstatus>\r\n\t", userInfo2.Sigstatus);
            xmlnode.AppendFormat("<skype>{0}</skype>\r\n\t", uf.Skype);
            xmlnode.AppendFormat("<website>{0}</website>\r\n\t", uf.Website);
            xmlnode.AppendFormat("<yahoo>{0}</yahoo>\r\n", uf.Yahoo);
            xmlnode.AppendFormat("<qq>{0}</qq>\r\n", uf.qq);
            xmlnode.AppendFormat("<msn>{0}</msn>\r\n", uf.Msn);
            xmlnode.AppendFormat("<posts>{0}</posts>\r\n", userInfo2.Posts);
            xmlnode.AppendFormat("<location>{0}</location>\r\n", uf.Location);
            xmlnode.AppendFormat("<showavatars>{0}</showavatars>\r\n", config.Showavatars);
            xmlnode.AppendFormat("<userstatusby>{0}</userstatusby>\r\n", config.Userstatusby);
            xmlnode.AppendFormat("<starthreshold>{0}</starthreshold>\r\n", config.Starthreshold);
            xmlnode.AppendFormat("<forumtitle>{0}</forumtitle>\r\n", config.Forumtitle);
            xmlnode.AppendFormat("<showsignatures>{0}</showsignatures>\r\n", config.Showsignatures);
            xmlnode.AppendFormat("<maxsigrows>{0}</maxsigrows>\r\n", config.Maxsigrows);
            xmlnode.AppendFormat("<olimg><![CDATA[{0}]]></olimg>\r\n", Online.GetGroupImg(userInfo2.GroupID));
            string[] array = GeneralConfigInfo.Current.Customauthorinfo.Split('|');
            xmlnode.AppendFormat("<postleftshow><![CDATA[{0}]]></postleftshow>\r\n", array[0]);
            xmlnode.AppendFormat("<userfaceshow><![CDATA[{0}]]></userfaceshow>\r\n", array[1]);
            xmlnode.AppendFormat("<lastvisit>{0}</lastvisit>\r\n", userInfo2.LastVisit);
            if (userInfo2.ID == topic.PosterID)
            {
                xmlnode.AppendFormat("<onlyauthor>{0}</onlyauthor>\r\n", 1);
            }
            else
            {
                xmlnode.AppendFormat("<onlyauthor>{0}</onlyauthor>\r\n", 0);
            }
            xmlnode.Append("</post>\r\n");
            return xmlnode;
        }

        private static int GetAllowGetAttachValue(PostpramsInfo postpramsInfo)
        {
            if (BBX.Forum.Forums.AllowGetAttachByUserID(BBX.Forum.Forums.GetForumInfo(postpramsInfo.Fid).Permuserlist, postpramsInfo.CurrentUserid))
            {
                return 1;
            }
            int result = 0;
            if (String.IsNullOrEmpty(postpramsInfo.Getattachperm))
            {
                result = postpramsInfo.CurrentUserGroup.AllowGetattach ? 1 : 0;
            }
            else
            {
                if (BBX.Forum.Forums.AllowGetAttach(postpramsInfo.Getattachperm, postpramsInfo.Usergroupid))
                {
                    result = 1;
                }
            }
            return result;
        }

        public static string[] GetHiddenAttachIdList(string content, int hide)
        {
            Regex regex = new Regex("\\[attach\\](\\d+?)\\[\\/attach\\]", RegexOptions.IgnoreCase);
            Regex regex2 = new Regex("\\s*\\[hide\\][\\n\\r]*([\\s\\S]+?)[\\n\\r]*\\[\\/hide\\]\\s*", RegexOptions.IgnoreCase);
            Regex regex3 = new Regex("\\[attachimg\\](\\d+?)\\[\\/attachimg\\]", RegexOptions.IgnoreCase);
            if (hide == 0)
            {
                return new string[0];
            }
            StringBuilder stringBuilder = new StringBuilder();
            StringBuilder stringBuilder2 = new StringBuilder();
            foreach (Match match in regex2.Matches(content))
            {
                if (hide == 1)
                {
                    stringBuilder2.Append(match.Groups[0].ToString());
                }
            }
            foreach (Match match2 in regex.Matches(stringBuilder2.ToString()))
            {
                stringBuilder.Append(match2.Groups[1].ToString());
                stringBuilder.Append(",");
            }
            foreach (Match match3 in regex3.Matches(stringBuilder2.ToString()))
            {
                stringBuilder.Append(match3.Groups[1].ToString());
                stringBuilder.Append(",");
            }
            if (stringBuilder.Length == 0)
            {
                return new string[0];
            }
            return stringBuilder.Remove(stringBuilder.Length - 1, 1).ToString().Split(',');
        }

        private void Report()
        {
            if (ForumUtils.IsCrossSitePost())
            {
                return;
            }
            if (userid == -1)
            {
                return;
            }
            string @string = DNTRequest.GetString("report_url");
            string string2 = DNTRequest.GetString("reportmessage");
            StringBuilder stringBuilder = new StringBuilder("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");
            int @int = DNTRequest.GetInt("fid", 0);
            if (string2 == string.Empty || string2.Length < 15)
            {
                stringBuilder.Append("<error>您的理由必须多于15个字</error>");
            }
            else
            {
                if (!Utils.StrIsNullOrEmpty(@string))
                {
                    //PrivateMessageInfo msg = new PrivateMessageInfo();
                    string message = string.Format("下面的链接地址被举报,<br /><a href='{0}' target='_blank'>{0}</a><br />请检查<br/>举报理由：{1}", @string, Utils.HtmlEncode(string2));
                    string dateTime = Utils.GetDateTime();
                    var reportUsers = BBX.Forum.Users.GetReportUsers();
                    foreach (var item in reportUsers)
                    {
                        var userInfo = item;
                        if (BBX.Forum.Moderators.IsModer(userInfo.AdminID, userInfo.ID, @int))
                        {
                            var msg = new ShortMessage();
                            msg.Message = message;
                            msg.Subject = "举报信息";
                            msg.Msgto = item.Name;
                            msg.MsgtoID = item.ID;
                            msg.Msgfrom = username;
                            msg.MsgfromID = userid;
                            //msg.New = 1;
                            //msg.PostDateTime = dateTime;
                            msg.Folder = 0;
                            //BBX.Forum.PrivateMessages.CreatePrivateMessage(msg, 0);
                            msg.Create();
                            //int olidByUid = BBX.Forum.OnlineUsers.GetOlidByUid(msg.MsgtoID);
                            //if (olidByUid > 0)
                            var entity = Online.FindByUserID(msg.MsgtoID);
                            if (entity != null)
                            {
                                BBX.Forum.Users.UpdateUserNewPMCount(msg.MsgtoID, entity.ID);
                            }
                        }
                    }
                }
            }
            ResponseXML(stringBuilder);
        }

        public void GetForumTree()
        {
            var sb = new StringBuilder("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");
            sb.Append("<data>\n");
            //DataTable forumList = BBX.Forum.Forums.GetForumList(DNTRequest.GetInt("fid", 0));
            var xf = XForum.FindByID(WebHelper.RequestInt("fid"));
            if (xf != null)
            {
                foreach (var fi in xf.AllChilds)
                {
                    if (config.Hideprivate != 1 || fi.AllowView(usergroupid))
                    {
                        sb.Append("<forum name=\"");
                        sb.Append(Utils.RemoveHtml(fi.Name).Replace("&", "&amp;"));
                        sb.Append("\" fid=\"");
                        sb.Append(fi.ID);
                        sb.Append("\" subforumcount=\"");
                        sb.Append(fi.Childs.Count);
                        sb.Append("\" layer=\"");
                        sb.Append(fi.Layer);
                        sb.Append("\" parentid=\"");
                        sb.Append(fi.ParentID);
                        sb.Append("\" parentidlist=\"");
                        sb.Append(fi.GetFullPath(false, ","));
                        sb.Append("\" />\n");
                    }
                }
            }
            sb.Append("</data>\n");
            ResponseXML(sb);
        }

        public void GetTopicTree()
        {
            var sb = new StringBuilder("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");
            var topicInfo = Topic.FindByID(DNTRequest.GetInt("topicid", 0));
            var forumInfo = topicInfo.Forum as IXForum;
            if (topicInfo.ReadPerm > usergroupinfo.Readaccess && topicInfo.PosterID != userid && useradminid != 1 && !Utils.InArray(username, forumInfo.Moderators))
            {
                sb.Append("<error>本主题阅读权限为: " + topicInfo.ReadPerm + ", 您当前的身份 \"" + usergroupinfo.GroupTitle + "\" 阅读权限不够</error>");
                ResponseXML(sb);
                return;
            }
            sb.Append("<data>\n");
            var postTree = BBX.Forum.Posts.GetPostTree(DNTRequest.GetInt("topicid", 0), -1, BBX.Forum.Users.GetUserInfo(userid).Credits);
            if (postTree != null && postTree.Count > 0)
            {
                foreach (var pi in postTree.ToList().Where(e => e.Layer > 0).OrderBy(e => e.ID))
                {
                    var msg = pi.Message;
                    sb.AppendFormat("<post title=\"{0}\"", UBB.ClearBR(Utils.ClearUBB(pi.Title)));
                    sb.AppendFormat(" pid=\"{0}\"", pi.ID);
                    sb.Append(" message=\"");
                    if (Utils.StrIsNullOrEmpty(UBB.ClearBR(Utils.ClearUBB(msg))))
                    {
                        sb.Append(pi.Title);
                    }
                    else
                    {
                        sb.Append((msg.IndexOf("[hide]") > -1) ? "*** 隐藏帖 ***" : UBB.ClearBR(Utils.ClearUBB(msg)));
                    }
                    sb.AppendFormat("\" postdatetime=\"{0}\"", pi.PostDateTime.ToString("yyyy-MM-dd HH:mm"));
                    sb.AppendFormat(" poster=\"{0}\"", Utils.HtmlEncode(pi.Poster));
                    sb.AppendFormat(" posterid=\"{0}\" />\n", pi.PosterID);
                }
                //}
                if (sb.Length > 0)
                {
                    sb = sb.Replace("&", "");
                }
            }
            sb.Append("</data>\n");
            ResponseXML(sb);
        }

        private void CheckUserExtCredit(int aid)
        {
            Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1.0);
            Response.Expires = -1;
            Response.Clear();
            Response.Write(Utils.JsonCharFilter(GetCheckUserExtCreditJson(Attachment.FindByID(aid))));
            Response.End();
        }

        private string GetCheckUserExtCreditJson(Attachment att)
        {
            string result = "";
            if (userid <= 0)
            {
                result = "[{'haserror': true, 'errormsg' : '您尚未登录, 因此无法买卖附件'}]";
            }
            if (att.ID <= 0)
            {
                result = "[{'haserror': true, 'errormsg' : '当前的附件信息无效'}]";
            }
            if (userid > 0 && att.ID > 0)
            {
                if (att.Uid == userid)
                {
                }
                //int credit = BBX.Forum.Users.GetUserExtCreditsByUserid(userid, Scoresets.GetTopicAttachCreditsTrans());
                var user = User.FindByID(userid);
                var credit = (Int32)user["extcredits" + Scoresets.GetTopicAttachCreditsTrans()];
                if (credit >= att.AttachPrice)
                {
                    result = string.Format("[{{'haserror': false, 'errormsg' : '', 'attachname': '{0}', 'posterid':{1}, 'poster':'{2}', 'attachprice':{3}, 'extname':'{4}', 'leavemoney':{5}, 'aid':{6}}}]", new object[]
                    {
                        att.Name,
                        att.Uid,
                        BBX.Forum.Users.GetUserInfo(att.Uid).Name,
                        att.AttachPrice,
                        Scoresets.GetTopicAttachCreditsTransName(),
                        credit - att.AttachPrice,
                        att.ID
                    });
                }
                else
                {
                    string text = "";
                    if (EPayments.IsOpenEPayments())
                    {
                        text = "<a style=\"color:#FF0000\" href=\"usercpcreditspay.aspx\">充值</a>";
                    }
                    result = "[{'haserror': true, 'errormsg' : '对不起,您的账户余额不足 " + credit + ", 请返回!" + text + "'}]";
                }
            }
            return result;
        }

        public void ConfirmBuyAttach(int aid)
        {
            Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1.0);
            Response.Expires = -1;
            Response.Clear();
            var attachmentInfo = Attachment.FindByID(aid);
            string text = GetCheckUserExtCreditJson(attachmentInfo);
            if (text.StartsWith("[{'haserror': false"))
            {
                User.UpdateUserExtCredits(attachmentInfo.Uid, Scoresets.GetTopicAttachCreditsTrans(), (float)attachmentInfo.AttachPrice * (1f - Scoresets.GetCreditsTax()));
                User.UpdateUserExtCredits(userid, Scoresets.GetTopicAttachCreditsTrans(), (float)(-(float)attachmentInfo.AttachPrice));
                var log = new AttachPaymentLog
                {
                    Aid = aid,
                    Amount = attachmentInfo.AttachPrice,
                    AuthorID = attachmentInfo.Uid,
                    NetAmount = (Int32)((float)attachmentInfo.AttachPrice * (1f - Scoresets.GetCreditsTax())),
                    PostDateTime = DateTime.Now,
                    UserName = username,
                    Uid = userid
                };
                log.Insert();
                text = "[{'haserror': false, 'errormsg' : '', 'aid' : " + aid + "}]";
            }
            Response.Write(Utils.JsonCharFilter(text));
            //Response.End();
        }

        public void GetNewPms()
        {
            if (userid < 1)
            {
                return;
            }
            ResponseJSON<ShortMessage[]>(ShortMessage.GetList(userid, 1, 50, 1, 1).ToArray());
        }

        public void GetNewNotifications()
        {
            if (userid < 1) return;

            //ResponseJSON<NoticeInfo[]>(BBX.Forum.Notices.GetNewNotices(userid));
            ResponseJSON<Notice[]>(Notice.GetNewNotices(userid));
        }

        private void ResponseXML(StringBuilder xmlnode)
        {
            Response.Clear();
            Response.ContentType = "Text/XML";
            Response.Expires = 0;
            Response.Cache.SetNoStore();
            Response.Write(xmlnode.ToString());
            //Response.End();
        }

        private void ResponseJSON(string json)
        {
            Response.Clear();
            Response.ContentType = "application/json";
            Response.Expires = 0;
            Response.Cache.SetNoStore();
            Response.Write(json);
            //Response.End();
        }

        private void ResponseJSON<T>(T jsonobj)
        {
            ResponseJSON(jsonobj.ToJson());
        }

        public void AuditPost(string type, string reason)
        {
            if (usergroupinfo.Is管理团队)
            {
                string tid = DNTRequest.GetString("tid");
                string text = "";
                //int tableid = DNTRequest.GetInt("tableid", TableList.GetPostTableId().ToInt());
                if (usergroupinfo.Is版主 && "passtopic,ignoretopic,deletetopic".Contains(type) && !BBX.Forum.Topics.GetModTopicCountByTidList(username, tid))
                {
                    return;
                }
                if ("passpost,ignorepost,deletepost".Contains(type))
                {
                    string pid = DNTRequest.GetString("pid");
                    string[] array = pid.Split(',');
                    for (int i = 0; i < array.Length; i++)
                    {
                        string text2 = array[i];
                        text = text + text2.Split('|')[0] + ",";
                    }
                    text = text.TrimEnd(',');
                }
                if (usergroupinfo.Is版主 && "passpost,ignorepost,deletepost".Contains(type) && !BBX.Forum.Posts.GetModPostCountByPidList(username, text))
                {
                    return;
                }
                if (("passtopic,ignoretopic,deletetopic".Contains(type) && !CreateNoticeInfo(type, tid, reason)) || ("passpost,ignorepost,deletepost".Contains(type) && !CreateNoticeInfo(type, DNTRequest.GetString("pid"), reason)))
                {
                    return;
                }
                if (type != null)
                {
                    var fids = DNTRequest.GetString("forumid").SplitAsInt(",");
                    if (type == "passtopic")
                    {
                        BBX.Forum.Topics.PassAuditNewTopic("", tid, "", fids);
                        CallbackJson("tid", tid);
                        return;
                    }
                    if (type == "ignoretopic")
                    {
                        BBX.Forum.Topics.PassAuditNewTopic(tid, "", "", fids);
                        CallbackJson("tid", tid);
                        return;
                    }
                    if (type == "deletetopic")
                    {
                        BBX.Forum.Topics.PassAuditNewTopic("", "", tid, fids);
                        CallbackJson("tid", tid);
                        return;
                    }
                    if (type == "passpost")
                    {
                        BBX.Forum.Posts.AuditPost(text, "", "", "");
                        CallbackJson("pid", text);
                        return;
                    }
                    if (type == "ignorepost")
                    {
                        BBX.Forum.Posts.AuditPost("", "", text, "");
                        CallbackJson("pid", text);
                        return;
                    }
                    if (!(type == "deletepost"))
                    {
                        return;
                    }
                    BBX.Forum.Posts.AuditPost("", text, "", "");
                    CallbackJson("pid", text);
                }
            }
        }

        private bool CreateNoticeInfo(string type, string idList, string reason)
        {
            var userGroupInfo = UserGroup.FindByID(BBX.Forum.Users.GetUserInfo(userid).GroupID);
            if ((usergroupinfo.ReasonPm == 1 || usergroupinfo.ReasonPm == 3) && String.IsNullOrEmpty(reason))
            {
                CallbackJson("message", "\"请输入操作理由\"");
                return false;
            }
            string actions = "";
            if (type != null)
            {
                if (!(type == "passtopic"))
                {
                    if (!(type == "ignoretopic"))
                    {
                        if (!(type == "deletetopic"))
                        {
                            if (!(type == "passpost"))
                            {
                                if (!(type == "ignorepost"))
                                {
                                    if (type == "deletepost")
                                    {
                                        actions = "删除帖子";
                                    }
                                }
                                else
                                {
                                    actions = "忽略帖子";
                                }
                            }
                            else
                            {
                                actions = "审核帖子";
                            }
                        }
                        else
                        {
                            actions = "删除主题";
                        }
                    }
                    else
                    {
                        actions = "忽略主题";
                    }
                }
                else
                {
                    actions = "审核主题";
                }
            }
            if ("passtopic,ignoretopic,deletetopic".Contains(type))
            {
                string[] array = idList.Split(',');
                for (int i = 0; i < array.Length; i++)
                {
                    string str = array[i];
                    var tp = Topic.FindByID(str.ToInt());
                    if (tp != null && tp.PosterID != -1)
                    {
                        if (usergroupinfo.ReasonPm == 3 || reason != "")
                        {
                            string text = "你发表的主题 ";
                            if (type == "passtopic")
                            {
                                text += string.Format("<a href='{0}' target='_blank'>{1}</a> 已经审核通过！ &nbsp; <a href='{0}' target='_blank'>查看 &rsaquo;</a> ", Urls.ShowTopicAspxRewrite(str.ToInt(), 0), tp.Title);
                            }
                            if (type == "deletetopic")
                            {
                                text += string.Format("{0} 没有通过审核，现已被删除！", tp.Title);
                            }
                            if (reason != "")
                            {
                                text += string.Format("<div class='notequote'><blockquote>{0}</blockquote></div>", Utils.EncodeHtml(reason));
                            }
                            SendNoticeInfo(text, tp.PosterID, str.ToInt());
                        }
                        ModeratorManageLog.Add(userid, username, usergroupid, userGroupInfo.GroupTitle, tp.Fid, tp.ForumName, tp.ID, tp.Title, actions, reason);
                    }
                }
            }
            else
            {
                string[] array2 = idList.Split(',');
                for (int j = 0; j < array2.Length; j++)
                {
                    string text2 = array2[j];
                    int num = text2.Split('|')[0].ToInt();
                    int num2 = text2.Split('|')[1].ToInt();
                    var postInfo = Post.FindByID(num);
                    if (postInfo != null && postInfo.PosterID != -1)
                    {
                        if (usergroupinfo.ReasonPm == 3 || reason != "")
                        {
                            string text3 = "你发表的回复";
                            if (type == "passpost")
                            {
                                text3 += string.Format("已经审核通过！ &nbsp; <a target='_blank' href='{0}#{1}'>查看 &rsaquo;</a>", Urls.ShowTopicAspxRewrite(num2, 0), num);
                            }
                            if (type == "deletepost")
                            {
                                text3 += "没有通过审核，现已被删除！";
                            }
                            if (reason != "")
                            {
                                text3 += string.Format(" <p class='summary'>回复内容：<span class='xg1'>{0}</span></p> <div class='notequote'><blockquote>{1}</blockquote></div>", Utils.GetSubString(Utils.ClearUBB(postInfo.Message), 15, "..."), Utils.EncodeHtml(reason));
                            }
                            SendNoticeInfo(text3, postInfo.PosterID, num);
                        }
                        var tp = Topic.FindByID(num2);
                        ModeratorManageLog.Add(userid, username, usergroupid, userGroupInfo.GroupTitle, tp.Fid, tp.ForumName, tp.ID, tp.Title, actions, reason);
                    }
                }
            }
            return true;
        }

        private void SendNoticeInfo(string message, int uid, int fromid)
        {
            var notice = new Notice
            {
                New = 1,
                Note = message,
                PostDateTime = DateTime.Now,
                Type = (Int32)NoticeType.TopicAdmin,
                Poster = username,
                PosterID = userid,
                Uid = uid,
                FromID = fromid
            };

            notice.Insert();
        }

        public void DeletePostsByUidAndDays(int uid, int days)
        {
            if (useradminid == 1 || useradminid == 2)
            {
                BBX.Forum.Posts.ClearPosts(uid, days);
                CallbackJson("uid", uid.ToString());
            }
        }

        public void CallbackJson(string type, string id)
        {
            ResponseJSON(String.Format("[{{0}:{1}}]", type, id));
        }
    }
}