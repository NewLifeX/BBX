using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using BBX.Common;
using BBX.Config;
using BBX.Entity;
using BBX.Forum;
using NewLife;

namespace BBX.Web
{
    public class posttopic : PageBase
    {
        public Topic topic = new Topic();
        public Post postinfo = new Post();
        public bool isfirstpost = true;
        public int forumid = DNTRequest.GetInt("forumid", -1);
        public string message = "";
        public bool allowposttopic = true;
        public int parseurloff;
        public int smileyoff;
        public int bbcodeoff = 1;
        public int usesig = (ForumUtils.GetCookie("sigstatus") == "0") ? 0 : 1;
        //public int allowimg;
        public Boolean disablepost;
        public string attachextensions;
        public string attachextensionsnosize;
        public int attachsize;
        public UserExtcreditsInfo userextcreditsinfo;
        public UserExtcreditsInfo bonusextcreditsinfo;
        public IXForum forum = new XForum();
        public string topictypeselectoptions;
        public bool canpostattach;
        public int creditstrans;
        public string enddatetime = DateTime.Today.AddDays(1.0).ToString("yyyy-MM-dd");
        public bool canhtmltitle;
        public int spaceid;
        public bool enabletag;
        public string type = DNTRequest.GetString("type").ToLower();
        public float mybonustranscredits;
        public bool needlogin;
        public int forumpageid = DNTRequest.GetInt("forumpage", 1);
        public int htmlon;
        public User userinfo;
        private bool createpoll = DNTRequest.GetString("createpoll") == "1";
        private string[] pollitem = new string[0];
        //private string curdatetime = Utils.GetDateTime();
        private string msg = "";
        private string posttitle = DNTRequest.GetString(GeneralConfigInfo.Current.Antispamposttitle);
        private string postmessage = DNTRequest.GetString(GeneralConfigInfo.Current.Antispampostmessage);
        public string topictags = "";
        public string htmltitle = "";
        public List<Attachment> attachmentlist = new List<Attachment>();
        public List<UserGroup> userGroupInfoList = UserGroup.FindAllWithCache();
        public string customeditbuttons;
        public bool feedstatus = DNTRequest.GetInt("ispublishfeed", 0) != 0;
        public int topicid;
        public bool needaudit;
        public int fromindex = DNTRequest.GetInt("fromindex", 0);

        protected override void ShowPage()
        {
            if (this.oluserinfo.GroupID == 4)
            {
                base.AddErrLine("你所在的用户组，为禁止发言");
                return;
            }
            if (this.userid > 0)
            {
                this.userinfo = BBX.Entity.User.FindByID(this.userid);
            }
            if (HttpContext.Current.Request.RawUrl.Contains("javascript:"))
            {
                base.AddErrLine("非法的链接");
                return;
            }
            this.forum = XForum.FindByID(forumid);
            if (this.forum == null || this.forum.Layer == 0)
            {
                this.forum = new XForum();
                this.allowposttopic = false;
                base.AddErrLine("错误的论坛ID");
                return;
            }
            this.pagetitle = Utils.RemoveHtml(this.forum.Name);
            this.enabletag = config.Enabletag && this.forum.AllowTag;
            if (this.forum.ApplytopicType == 1)
            {
                this.topictypeselectoptions = Forums.GetCurrentTopicTypesOption(this.forum.Fid, this.forum.TopicTypes);
            }
            if (!String.IsNullOrEmpty(this.forum.Password) && Utils.MD5(this.forum.Password) != ForumUtils.GetCookie("forum" + this.forumid + "password"))
            {
                base.AddErrLine("本版块被管理员设置了密码");
                base.SetBackLink(base.ShowForumAspxRewrite(this.forumid, 0));
                return;
            }
            this.needaudit = UserAuthority.NeedAudit(forum.Fid, forum.Modnewposts, this.useradminid, this.userid, this.usergroupinfo);
            this.smileyoff = forum.AllowSmilies ? 0 : 1;
            this.bbcodeoff = ((this.forum.Allowbbcode == 1 && this.usergroupinfo.AllowCusbbCode) ? 0 : 1);
            //this.allowimg = this.forum.Allowimgcode;
            this.customeditbuttons = Caches.GetCustomEditButtonList();
            if (!UserAuthority.VisitAuthority(this.forum, this.usergroupinfo, this.userid, ref this.msg))
            {
                base.AddErrLine(this.msg);
                this.needlogin = true;
                return;
            }
            if (!UserAuthority.PostAuthority(this.forum, this.usergroupinfo, this.userid, ref this.msg))
            {
                base.AddErrLine(this.msg);
                this.needlogin = true;
                return;
            }
            //string allowAttachmentType = Attachments.GetAllowAttachmentType(this.usergroupinfo, this.forum);
            this.attachextensions = AttachType.GetAttachmentTypeArray(usergroupinfo, this.forum);
            this.attachextensionsnosize = AttachType.GetAttachmentTypeString(usergroupinfo, this.forum);
            int num = (this.userid > 0) ? Attachment.GetUploadFileSizeByuserid(this.userid) : 0;
            this.attachsize = this.usergroupinfo.MaxSizeperday - num;
            this.canpostattach = UserAuthority.PostAttachAuthority(this.forum, this.usergroupinfo, this.userid, ref this.msg);
            this.canhtmltitle = this.usergroupinfo.AllowHtmlTitle;
            this.creditstrans = Scoresets.GetTopicAttachCreditsTrans();
            this.userextcreditsinfo = Scoresets.GetScoreSet(this.creditstrans);
            this.bonusextcreditsinfo = Scoresets.GetScoreSet(Scoresets.GetBonusCreditsTrans());
            if (this.forum.AllowSpecialOnly && !Utils.InArray(this.type, "poll,bonus,debate"))
            {
                base.AddErrLine(string.Format("当前版块 \"{0}\" 不允许发表普通主题", this.forum.Name));
                return;
            }
            if (!UserAuthority.PostSpecialAuthority(this.forum, this.type, ref this.msg))
            {
                base.AddErrLine(this.msg);
                return;
            }
            if (!UserAuthority.PostSpecialAuthority(this.usergroupinfo, this.type, ref this.msg))
            {
                base.AddErrLine(this.msg);
                this.needlogin = true;
                return;
            }
            if (this.type == "bonus")
            {
                int bonusCreditsTrans = Scoresets.GetBonusCreditsTrans();
                if (bonusCreditsTrans <= 0)
                {
                    base.AddErrLine("系统未设置\"交易积分设置\", 无法判断当前要使用的(扩展)积分字段, 暂时无法发布悬赏");
                    return;
                }
                this.mybonustranscredits = Users.GetUserExtCredits(this.userid, bonusCreditsTrans);
            }
            this.userGroupInfoList.Sort((x, y) => x.Readaccess - y.Readaccess + (y.ID - x.ID));
            var adminGroupInfo = AdminGroup.FindByID(this.usergroupid);
            this.disablepost = adminGroupInfo != null ? adminGroupInfo.DisablePostctrl : this.usergroupinfo.DisablePeriodctrl;
            if (this.ispost)
            {
                if (!UserAuthority.CheckPostTimeSpan(this.usergroupinfo, adminGroupInfo, this.oluserinfo, this.userinfo, ref this.msg))
                {
                    base.AddErrLine(this.msg);
                    return;
                }
                base.SetBackLink(string.Format("posttopic.aspx?forumid={0}&restore=1&type={1}", this.forumid, this.type));
                ForumUtils.WriteCookie("postmessage", this.postmessage);
                this.NormalValidate(adminGroupInfo, this.postmessage, this.userinfo);
                if (base.IsErr())
                {
                    return;
                }
                if (ForumUtils.IsPostFile())
                {
                    if (Utils.StrIsNullOrEmpty(AttachType.GetAttachmentTypeArray(usergroupinfo, forum)))
                    {
                        base.AddErrLine("系统不允许上传附件");
                    }
                    if (!UserAuthority.PostAttachAuthority(this.forum, this.usergroupinfo, this.userid, ref this.msg))
                    {
                        base.AddErrLine(this.msg);
                    }
                }
                int topicprice = 0;
                bool isbonus = this.type == "bonus";
                this.ValidateBonus(ref topicprice, ref isbonus);
                this.ValidatePollAndDebate();
                if (base.IsErr())
                {
                    return;
                }
                if (ForumUtils.IsHidePost(this.postmessage))
                {
                    int arg_5B2_0 = this.usergroupinfo.AllowHideCode ? 1 : 0;
                }
                var topicInfo = this.CreateTopic(adminGroupInfo, this.postmessage, isbonus, topicprice);
                if (base.IsErr())
                {
                    return;
                }
                //var postInfo = this.CreatePost(topicInfo);
                var postInfo = Post.FindByID(topicInfo.LastPostID);
                //if (base.IsErr())
                //{
                //	return;
                //}
                var stringBuilder = new StringBuilder();
                //AttachmentInfo[] array = null;
                string formString = DNTRequest.GetFormString("attachid");
                if (!string.IsNullOrEmpty(formString))
                {
                    var array = Attachments.GetNoUsedAttachmentArray(this.userid, formString);
                    Attachments.UpdateAttachment(array, topicInfo.ID, postInfo.ID, postInfo, ref stringBuilder, this.userid, this.config, this.usergroupinfo);
                }
                Online.UpdateAction(this.olid, UserAction.PostTopic, this.forumid, this.forum.Name, -1, "");
                if (this.isbindconnect && this.feedstatus)
                {
                    // 推送到QQ空间和微博
                    //PushFeed pushFeed = new PushFeed();
                    //pushFeed.TopicPushFeed(topicInfo, postInfo, array, this.feedstatus);
                }
                if (stringBuilder.Length > 0)
                {
                    base.SetUrl(base.ShowTopicAspxRewrite(topicInfo.ID, 0));
                    base.SetMetaRefresh(5);
                    base.SetShowBackLink(true);
                    if (this.infloat == 1)
                    {
                        base.AddErrLine(stringBuilder.ToString());
                        return;
                    }
                    stringBuilder.Insert(0, "<table cellspacing=\"0\" cellpadding=\"4\" border=\"0\"><tr><td colspan=2 align=\"left\"><span class=\"bold\"><nobr>发表主题成功,但图片/附件上传出现问题:</nobr></span><br /></td></tr>");
                    base.AddMsgLine(stringBuilder.Append("</table>").ToString());
                }
                else
                {
                    base.SetShowBackLink(false);
                    if (this.useradminid != 1 && (UserAuthority.NeedAudit(forum.Fid, forum.Modnewposts, this.useradminid, this.userid, this.usergroupinfo) || topicInfo.DisplayOrder == -2))
                    {
                        ForumUtils.WriteCookie("postmessage", "");
                        this.SetLastPostedForumCookie();
                        base.SetUrl(base.ShowForumAspxRewrite(this.forumid, this.forumpageid));
                        base.SetMetaRefresh();
                        base.AddMsgLine("发表主题成功, 但需要经过审核才可以显示. 返回该版块");
                    }
                    else
                    {
                        this.PostTopicSucceed(this.forum, topicInfo);
                    }
                }
                if (this.needlogin && this.userid > 0)
                {
                    this.needlogin = false;
                    return;
                }
            }
            else
            {
                base.AddLinkCss(BaseConfigs.GetForumPath + "templates/" + this.templatepath + "/editor.css", "css");
            }
        }

        public Topic CreateTopic(AdminGroup admininfo, string postmessage, bool isbonus, int topicprice)
        {
            var tp = new Topic();
            tp.Fid = this.forumid;

            var iconid = DNTRequest.GetInt("iconid", 0);
            if (iconid < 0 || iconid > 15) iconid = 0;
            tp.IconID = iconid;
            this.message = Posts.GetPostMessage(this.usergroupinfo, admininfo, postmessage, DNTRequest.GetInt("htmlon") == 1);
            tp.Title = ((this.useradminid == 1) ? Utils.HtmlEncode(this.posttitle) : Utils.HtmlEncode(ForumUtils.BanWordFilter(this.posttitle)));
            if (this.useradminid != 1 && (ForumUtils.HasBannedWord(this.posttitle) || ForumUtils.HasBannedWord(postmessage)))
            {
                string arg = (ForumUtils.GetBannedWord(this.posttitle) == string.Empty) ? ForumUtils.GetBannedWord(postmessage) : ForumUtils.GetBannedWord(this.posttitle);
                base.AddErrLine(string.Format("对不起, 您提交的内容包含不良信息  <font color=\"red\">{0}</font>, 请返回修改!", arg));
                return tp;
            }
            if (Utils.GetCookie("lasttopictitle") == Utils.MD5(tp.Title) || Utils.GetCookie("lasttopicmessage") == Utils.MD5(this.message))
            {
                base.AddErrLine("请勿重复发帖");
                return tp;
            }
            tp.TypeID = DNTRequest.GetInt("typeid", 0);
            if (this.usergroupinfo.AllowSetreadPerm)
            {
                tp.ReadPerm = ((DNTRequest.GetInt("topicreadperm", 0) > 255) ? 255 : DNTRequest.GetInt("topicreadperm", 0));
            }
            tp.Price = topicprice;
            //tp.Poster = this.username;
            //tp.Posterid = this.userid;
            //tp.Postdatetime = this.curdatetime;
            //tp.Lastpost = this.curdatetime;
            //tp.Lastposter = this.username;
            tp.DisplayOrder = Topics.GetTitleDisplayOrder(this.usergroupinfo, this.useradminid, this.forum, tp, this.message, this.disablepost);
            string text = DNTRequest.GetString("htmltitle").Trim();
            if (!text.IsNullOrEmpty() && Utils.HtmlDecode(text).Trim() != tp.Title)
            {
                tp.Magic = 11000;
            }
            string text2 = DNTRequest.GetString("tags").Trim();
            string[] array = null;
            if (this.enabletag && !text2.IsNullOrEmpty())
            {
                if (ForumUtils.InBanWordArray(text2))
                {
                    base.AddErrLine("标签中含有系统禁止词语,请修改");
                    return tp;
                }
                array = Utils.SplitString(text2, " ", true, 2, 10);
                if (array.Length <= 0 || array.Length > 5)
                {
                    base.AddErrLine("超过标签数的最大限制或单个标签长度没有介于2-10之间，最多可填写 5 个标签");
                    return tp;
                }
                if (tp.Magic == 0)
                {
                    tp.Magic = 10000;
                }
                tp.Magic = Utils.StrToInt(tp.Magic.ToString() + "1", 0);
            }
            if (isbonus)
            {
                tp.Special = 2;
                if (this.mybonustranscredits < (float)topicprice && !usergroupinfo.Is管理员)
                {
                    base.AddErrLine(string.Format("无法进行悬赏<br /><br />您当前的{0}为 {1} {3}<br/>悬赏需要{0} {2} {3}", new object[]
                    {
                        this.bonusextcreditsinfo.Name,
                        this.mybonustranscredits,
                        topicprice,
                        this.bonusextcreditsinfo.Unit
                    }));
                    return tp;
                }
                BBX.Entity.User.UpdateUserExtCredits(userid, Scoresets.GetBonusCreditsTrans(), (float)(-(float)topicprice) * (Scoresets.GetCreditsTax() + 1f));
            }
            if (this.type == "poll")
            {
                tp.Special = 1;
            }
            if (this.type == "debate")
            {
                tp.Special = 4;
            }
            if (!Moderators.IsModer(this.useradminid, this.userid, this.forumid))
            {
                tp.Attention = 1;
            }
            if (ForumUtils.IsHidePost(postmessage) && this.usergroupinfo.AllowHideCode)
            {
                tp.Hide = 1;
            }
            //tp.Tid = Topics.CreateTopic(tp);
            //tp.Insert();
            var postInfo = this.CreatePost(tp);
            tp.Create(postInfo);
            if (this.canhtmltitle && !text.IsNullOrEmpty() && text != tp.Title)
            {
                Topics.WriteHtmlTitleFile(Utils.RemoveUnsafeHtml(text), tp.ID);
            }
            if (this.enabletag && array != null && array.Length > 0)
            {
                if (this.useradminid != 1 && ForumUtils.HasBannedWord(text2))
                {
                    string bannedWord = ForumUtils.GetBannedWord(text2);
                    base.AddErrLine(string.Format("标签中含有系统禁止词语 <font color=\"red\">{0}</font>,请修改", bannedWord));
                    return tp;
                }
                Tag.CreateTopicTags(array, tp.ID, this.userid, DateTime.Now.ToFullString());
            }
            if (this.type == "debate")
            {
                var db = new Debate();
                db.Tid = tp.ID;
                db.PositiveOpinion = DNTRequest.GetString("positiveopinion");
                db.NegativeOpinion = DNTRequest.GetString("negativeopinion");
                db.TerminalTime = Request["terminaltime"].ToDateTime();
                db.Insert();
                //Topics.CreateDebateTopic(new DebateInfo
                //{
                //    Tid = tp.ID,
                //    Positiveopinion = DNTRequest.GetString("positiveopinion"),
                //    Negativeopinion = DNTRequest.GetString("negativeopinion"),
                //    Terminaltime = Convert.ToDateTime(DNTRequest.GetString("terminaltime"))
                //});
            }
            //Topics.AddParentForumTopics(this.forum.Parentidlist.Trim(), 1);
            //// 所有上级论坛帖子数增加
            //var ff = forum;
            //while (ff != null && ff.ID != 0)
            //{
            //	ff.Topics++;
            //	ff.Save();

            //	ff = ff.Parent;
            //}

            return tp;
        }

        public Post CreatePost(Topic topicinfo)
        {
            var pi = new Post();
            pi.Fid = this.forumid;
            pi.Tid = topicinfo.ID;
            pi.Poster = this.username;
            pi.PosterID = this.userid;
            pi.Title = ((this.useradminid == 1) ? Utils.HtmlEncode(this.posttitle) : Utils.HtmlEncode(ForumUtils.BanWordFilter(this.posttitle)));
            //pi.Postdatetime = DateTime.Now.ToFullString();
            //pi.Title = topicinfo.Title;
            pi.Message = this.message;
            //pi.Ip = WebHelper.UserHost;
            pi.Invisible = UserAuthority.GetTopicPostInvisible(this.forum, this.useradminid, this.userid, this.usergroupinfo, pi);
            pi.UseSig = DNTRequest.GetInt("usesig");
            pi.HtmlOn = ((this.usergroupinfo.AllowHtml && DNTRequest.GetInt("htmlon") == 1) ? 1 : 0);
            pi.SmileyOff = (smileyoff == 0 && forum.AllowSmilies) ? DNTRequest.GetInt("smileyoff") : this.smileyoff;
            pi.BBCodeOff = (this.usergroupinfo.AllowCusbbCode && this.forum.Allowbbcode == 1) ? DNTRequest.GetInt("bbcodeoff") : 1;
            pi.ParseUrlOff = DNTRequest.GetInt("parseurloff");
            //pi.Topictitle = topicinfo.Title;
            //try
            //{
            //pi.Pid = Posts.CreatePost(pi);
            //pi.Insert();
            Utils.WriteCookie("lasttopictitle", Utils.MD5(pi.Title));
            Utils.WriteCookie("lasttopicmessage", Utils.MD5(pi.Message));
            //}
            //catch
            //{
            //	TopicAdmins.DeleteTopics(topicinfo.ID.ToString(), false);
            //	base.AddErrLine("帖子保存出现异常");
            //}
            if (this.createpoll)
            {
                this.msg = Poll.CreatePoll(
                    DNTRequest.GetFormString("PollItemname"),
                    (DNTRequest.GetString("multiple") == "on") ? 1 : 0,
                    DNTRequest.GetInt("maxchoices", 1),
                    (DNTRequest.GetString("visiblepoll") == "on") ? 1 : 0,
                    (DNTRequest.GetString("allowview") == "on") ? true : false,
                    Utility.ToDateTime(this.enddatetime), topicinfo.ID, this.pollitem, this.userid);
            }
            return pi;
        }

        private void PostTopicSucceed(IXForum forum, Topic topicinfo)
        {
            CreditsFacade.PostTopic(this.userid, forum, true);
            int tid = topicinfo.ID;
            if (this.config.Aspxrewrite == 1)
            {
                base.SetUrl(base.ShowTopicAspxRewrite(tid, 0));
            }
            else
            {
                base.SetUrl(base.ShowTopicAspxRewrite(tid, 0) + "&forumpage=" + this.forumpageid);
            }
            ForumUtils.WriteCookie("postmessage", "");
            ForumUtils.WriteCookie("clearUserdata", "forum");
            this.SetLastPostedForumCookie();
            base.SetMetaRefresh();
            base.MsgForward("posttopic_succeed");
            base.AddMsgLine("发表主题成功, 返回该主题<br />(<a href=\"" + base.ShowForumAspxRewrite(this.forumid, this.forumpageid) + "\">点击这里返回 " + forum.Name + "</a>)<br />");
            Sync.NewTopic(tid.ToString(), topicinfo.Title, topicinfo.Poster, topicinfo.PosterID.ToString(), topicinfo.Fid.ToString(), "");
        }

        private void NormalValidate(AdminGroup admininfo, string postmessage, IUser user)
        {
            if (ForumUtils.IsCrossSitePost())
            {
                base.AddErrLine("您的请求来路不正确，无法提交。如果您安装了某种默认屏蔽来路信息的个人防火墙软件(如 Norton Internet Security)，请设置其不要禁止来路信息后再试。");
                return;
            }
            if (this.forum.ApplytopicType == 1 && this.forum.PostbytopicType == 1 && !this.topictypeselectoptions.IsNullOrEmpty())
            {
                var typeid = Request["typeid"].ToInt();
                if (typeid == 0)
                {
                    base.AddErrLine("主题类型不能为空");
                }
                if (!Forums.IsCurrentForumTopicType(typeid, this.forum.Topictypes))
                {
                    base.AddErrLine("错误的主题类型");
                }
            }
            if (Utils.StrIsNullOrEmpty(DNTRequest.GetString(this.config.Antispamposttitle).Trim().Replace("\u3000", "")))
            {
                base.AddErrLine("标题不能为空");
            }
            else
            {
                if (DNTRequest.GetString(this.config.Antispamposttitle).Length > 60)
                {
                    base.AddErrLine("标题最大长度为60个字符,当前为 " + DNTRequest.GetString(this.config.Antispamposttitle).Length + " 个字符");
                }
            }
            if (Utils.StrIsNullOrEmpty(postmessage.Replace("\u3000", "")))
            {
                base.AddErrLine("内容不能为空");
            }
            if (admininfo != null && !admininfo.DisablePostctrl)
            {
                if (postmessage.Length < this.config.Minpostsize)
                {
                    base.AddErrLine("您发表的内容过少, 系统设置要求帖子内容不得少于 " + this.config.Minpostsize + " 字多于 " + this.config.Maxpostsize + " 字");
                }
                else
                {
                    if (postmessage.Length > this.config.Maxpostsize)
                    {
                        base.AddErrLine("您发表的内容过多, 系统设置要求帖子内容不得少于 " + this.config.Minpostsize + " 字多于 " + this.config.Maxpostsize + " 字");
                    }
                }
            }
            if (this.config.DisablePostAD && this.useradminid < 1 && ((this.config.DisablePostADPostCount != 0 && user.Posts <= this.config.DisablePostADPostCount) || (this.config.DisablePostADRegMinute != 0 && DateTime.Now.AddMinutes((double)(-(double)this.config.DisablePostADRegMinute)) <= user.JoinDate.ToDateTime())))
            {
                string[] array = this.config.DisablePostADRegular.Replace("\r", "").Split('\n');
                for (int i = 0; i < array.Length; i++)
                {
                    string regular = array[i];
                    if (Posts.IsAD(regular, DNTRequest.GetString(this.config.Antispamposttitle), postmessage))
                    {
                        base.AddErrLine("发帖失败，内容中有不符合新用户强力广告屏蔽规则的字符，请检查标题和内容，如有疑问请与管理员联系");
                    }
                }
            }
        }

        private void ValidateBonus(ref int topicprice, ref bool isbonus)
        {
            isbonus = (this.type == "bonus");
            topicprice = 0;
            string @string = DNTRequest.GetString("topicprice");
            if (Regex.IsMatch(@string, "^[0-9]*[0-9][0-9]*$") || String.IsNullOrEmpty(@string))
            {
                topicprice = ((Utils.StrToInt(@string, 0) > 32767) ? 32767 : Utils.StrToInt(@string, 0));
                if (!isbonus)
                {
                    if (topicprice > this.usergroupinfo.MaxPrice && this.usergroupinfo.MaxPrice > 0)
                    {
                        if (String.IsNullOrEmpty(this.userextcreditsinfo.Unit))
                        {
                            base.AddErrLine(string.Format("主题售价不能高于 {0} {1}", this.usergroupinfo.MaxPrice, this.userextcreditsinfo.Name));
                            return;
                        }
                        base.AddErrLine(string.Format("主题售价不能高于 {0} {1}({2})", this.usergroupinfo.MaxPrice, this.userextcreditsinfo.Name, this.userextcreditsinfo.Unit));
                        return;
                    }
                    else
                    {
                        if (topicprice > 0 && this.usergroupinfo.MaxPrice <= 0)
                        {
                            base.AddErrLine(string.Format("您当前的身份 \"{0}\" 未被允许出售主题", this.usergroupinfo.GroupTitle));
                            return;
                        }
                        if (topicprice < 0)
                        {
                            base.AddErrLine("主题售价不能为负数");
                            return;
                        }
                    }
                }
                else
                {
                    if (!this.usergroupinfo.AllowBonus)
                    {
                        base.AddErrLine(string.Format("您当前的身份 \"{0}\" 未被允许进行悬赏", this.usergroupinfo.GroupTitle));
                    }
                    if (topicprice > this.mybonustranscredits)
                    {
                        base.AddErrLine(string.Format("您悬赏的{0},已超过您所能支付的范围", this.bonusextcreditsinfo.Name));
                    }
                    if (topicprice < this.usergroupinfo.MinBonusprice || topicprice > this.usergroupinfo.MaxBonusprice)
                    {
                        base.AddErrLine(string.Format("悬赏价格超出范围, 您应在 {0} - {1} {2}{3} 范围内进行悬赏", new object[]
                        {
                            this.usergroupinfo.MinBonusprice,
                            this.usergroupinfo.MaxBonusprice,
                            this.bonusextcreditsinfo.Unit,
                            this.bonusextcreditsinfo.Name
                        }));
                        return;
                    }
                }
            }
            else
            {
                if (!isbonus)
                {
                    base.AddErrLine("主题售价只能为整数");
                    return;
                }
                base.AddErrLine("悬赏价格只能为整数");
            }
        }

        private void ValidatePollAndDebate()
        {
            if (DNTRequest.GetString("createpoll") == "1")
            {
                if (!this.usergroupinfo.AllowPostpoll)
                {
                    base.AddErrLine(string.Format("您当前的身份 \"{0}\" 没有发布投票的权限", this.usergroupinfo.GroupTitle));
                }
                this.pollitem = Utils.SplitString(DNTRequest.GetString("PollItemname"), "\r\n");
                if (this.pollitem.Length < 2)
                {
                    base.AddErrLine("投票项不得少于2个");
                }
                else
                {
                    if (this.pollitem.Length > this.config.Maxpolloptions)
                    {
                        base.AddErrLine(string.Format("系统设置为投票项不得多于{0}个", this.config.Maxpolloptions));
                    }
                    else
                    {
                        for (int i = 0; i < this.pollitem.Length; i++)
                        {
                            if (this.pollitem[i].IsNullOrWhiteSpace())
                            {
                                base.AddErrLine("投票项不能为空");
                            }
                        }
                    }
                }
                this.enddatetime = DNTRequest.GetString("enddatetime");
                if (!Utils.IsDateString(this.enddatetime))
                {
                    base.AddErrLine("投票结束日期格式错误");
                }
            }
            if (this.type == "debate")
            {
                if (!this.usergroupinfo.AllowDebate)
                {
                    base.AddErrLine(string.Format("您当前的身份 \"{0}\" 没有发起辩论的权限", this.usergroupinfo.GroupTitle));
                }
                if (string.IsNullOrEmpty(DNTRequest.GetString("positiveopinion")))
                {
                    base.AddErrLine("正方观点不能为空");
                }
                if (String.IsNullOrEmpty(DNTRequest.GetString("negativeopinion")))
                {
                    base.AddErrLine("反方观点不能为空");
                }
                if (!Utils.IsDateString(DNTRequest.GetString("terminaltime")))
                {
                    base.AddErrLine("结束日期格式不正确");
                }
            }
        }

        private void SetLastPostedForumCookie()
        {
            Utils.WriteCookie("lastpostedforum", this.forum.Fid.ToString(), 525600);
        }

        protected string AttachmentList()
        {
            return "";
        }
    }
}