using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using BBX.Common;
using BBX.Config;
using BBX.Entity;
using BBX.Forum;
using NewLife;

namespace BBX.Web
{
    public class postreply : PageBase
    {
        public Topic topic = new Topic();
        public Post postinfo = new Post();
        public bool isfirstpost;
        public int forumid;
        public int topicid = DNTRequest.GetInt("topicid", -1);
        public int postid = DNTRequest.GetInt("postid", -1);
        public string message = "";
        public string topictitle = "";
        public int parseurloff;
        public int smileyoff;
        public int bbcodeoff;
        public int usesig = (ForumUtils.GetCookie("sigstatus") == "0") ? 0 : 1;
        //public int allowimg;
        public Boolean disablepost;
        public string attachextensions;
        public string attachextensionsnosize;
        public int attachsize;
        public string continuereply = DNTRequest.IsPost() ? "" : DNTRequest.GetQueryString("continuereply");
        public IXForum forum = new XForum();
        public bool canpostattach;
        public bool needlogin;
        public int forumpageid = DNTRequest.GetInt("forumpage", 1);
        public int htmlon;
        public User userinfo;
        private string msg = "";
        private string posttitle = DNTRequest.GetString(GeneralConfigInfo.Current.Antispamposttitle);
        private string postmessage = DNTRequest.GetString(GeneralConfigInfo.Current.Antispampostmessage);
        public string topictags = "";
        public UserExtcreditsInfo userextcreditsinfo = new UserExtcreditsInfo();
        public bool canhtmltitle;
        public string htmltitle = "";
        public string topictypeselectoptions = "";
        public bool enabletag;
        public List<Attachment> attachmentlist = new List<Attachment>();
        public List<UserGroup> userGroupInfoList = UserGroup.FindAllWithCache();
        public bool needaudit;
        public string customeditbuttons;

        protected override void ShowPage()
        {
            if (userid > 0) userinfo = BBX.Entity.User.FindByID(this.userid);

            var adminGroupInfo = AdminGroup.FindByID(this.usergroupid);
            if (adminGroupInfo != null) disablepost = adminGroupInfo.DisablePostctrl;

            if (!UserAuthority.CheckPostTimeSpan(this.usergroupinfo, adminGroupInfo, this.oluserinfo, this.userinfo, ref this.msg))
            {
                if (this.continuereply != "")
                {
                    base.AddErrLine("<b>回帖成功</b><br />由于" + this.msg + "后刷新继续");
                    return;
                }
                base.AddErrLine(this.msg);
                return;
            }
            else
            {
                var postInfo = this.GetPostAndTopic(adminGroupInfo);
                if (base.IsErr()) return;

                this.forum = Forums.GetForumInfo(this.forumid);
                this.smileyoff = this.forum.AllowSmilies ? 0 : 1;
                this.bbcodeoff = ((this.forum.AllowBbCode && this.usergroupinfo.AllowCusbbCode) ? 0 : 1);
                //this.allowimg = this.forum.AllowImgCode ? 1 : 0;
                this.needaudit = UserAuthority.NeedAudit(forum.Fid, forum.Modnewposts, this.useradminid, this.userid, this.usergroupinfo, this.topic);
                if (this.needaudit && this.topic.DisplayOrder == -2)
                {
                    base.AddErrLine("主题尚未通过审核, 不能执行回复操作");
                    return;
                }
                //string allowAttachmentType = Attachments.GetAllowAttachmentType(this.usergroupinfo, this.forum);
                this.attachextensions = AttachType.GetAttachmentTypeArray(usergroupinfo, this.forum);
                this.attachextensionsnosize = AttachType.GetAttachmentTypeString(usergroupinfo, this.forum);
                int num = (this.userid > 0) ? Attachment.GetUploadFileSizeByuserid(this.userid) : 0;
                this.attachsize = this.usergroupinfo.MaxSizeperday - num;
                this.canpostattach = UserAuthority.PostAttachAuthority(this.forum, this.usergroupinfo, this.userid, ref this.msg);
                if (!this.forum.Password.IsNullOrEmpty() && Utils.MD5(this.forum.Password) != ForumUtils.GetCookie("forum" + this.forumid + "password"))
                {
                    base.AddErrLine("本版块被管理员设置了密码");
                    base.SetBackLink(base.ShowForumAspxRewrite(this.forumid, 0));
                    return;
                }
                if (!UserAuthority.VisitAuthority(this.forum, this.usergroupinfo, this.userid, ref this.msg))
                {
                    base.AddErrLine(this.msg);
                    this.needlogin = true;
                    return;
                }
                if (!UserAuthority.PostReply(forum, this.userid, this.usergroupinfo, this.topic))
                {
                    base.AddErrLine((this.topic.Closed == 1) ? "主题已关闭无法回复" : "您没有发表回复的权限");
                    this.needlogin = (this.topic.Closed != 1);
                    return;
                }
                if (!UserAuthority.CheckPostTimeSpan(this.usergroupinfo, adminGroupInfo, this.oluserinfo, this.userinfo, ref this.msg))
                {
                    base.AddErrLine(this.msg);
                    return;
                }
                if (adminGroupInfo != null) this.disablepost = adminGroupInfo.DisablePostctrl;
                if (this.forum.TemplateID > 0) this.templatepath = Template.FindByID(this.forum.TemplateID).Directory;

                base.AddLinkCss(BaseConfigs.GetForumPath + "templates/" + this.templatepath + "/editor.css", "css");
                this.customeditbuttons = Caches.GetCustomEditButtonList();
                if (this.ispost)
                {
                    string text = (DNTRequest.GetInt("topicid", -1) > 0) ? string.Format("postreply.aspx?topicid={0}&restore=1&forumpage=" + this.forumpageid, this.topicid) : string.Format("postreply.aspx?postid={0}&restore=1&forumpage=" + this.forumpageid, this.postid);
                    if (!String.IsNullOrEmpty(DNTRequest.GetString("quote")))
                    {
                        text = string.Format("{0}&quote={1}", text, DNTRequest.GetString("quote"));
                    }
                    base.SetBackLink(text);
                    this.NormalValidate(adminGroupInfo, this.postmessage, this.userinfo);
                    if (base.IsErr()) return;

                    this.canpostattach = UserAuthority.PostAttachAuthority(this.forum, this.usergroupinfo, this.userid, ref this.msg);
                    if (!string.IsNullOrEmpty(DNTRequest.GetFormString("toreplay_user").Trim()))
                    {
                        this.postmessage = DNTRequest.GetFormString("toreplay_user").Trim() + "\n\n" + this.postmessage;
                    }
                    postInfo = this.CreatePostInfo(this.postmessage);
                    int replyuserid = (this.postid > 0) ? Post.FindByID(this.postid).PosterID : postInfo.PosterID;
                    this.postid = postInfo.ID;
                    if (base.IsErr()) return;

                    if (postInfo.ID > 0 && DNTRequest.GetString("postreplynotice") == "on")
                    {
                        Notice.SendPostReplyNotice(postInfo, this.topic, replyuserid);
                    }
                    Sync.Reply(this.postid.ToString(), this.topic.ID.ToString(), this.topic.Title, postInfo.Poster, postInfo.PosterID.ToString(), this.topic.Fid.ToString(), "");
                    StringBuilder stringBuilder = new StringBuilder();
                    //AttachmentInfo[] array = null;
                    string formString = DNTRequest.GetFormString("attachid");
                    if (!string.IsNullOrEmpty(formString))
                    {
                        var array = Attachments.GetNoUsedAttachmentArray(this.userid, formString);
                        Attachments.UpdateAttachment(array, this.topic.ID, postInfo.ID, postInfo, ref stringBuilder, this.userid, this.config, this.usergroupinfo);
                    }
                    Online.UpdateAction(this.olid, UserAction.PostReply, this.forumid, this.forum.Name, this.topicid, this.topictitle);
                    if (this.topic.Special == 4)
                    {
                        base.SetUrl(Urls.ShowDebateAspxRewrite(this.topicid));
                    }
                    else
                    {
                        if (this.infloat == 0)
                        {
                            base.SetUrl(string.Format("showtopic.aspx?forumpage={0}&topicid={1}&page=end&jump=pid#{2}", this.forumpageid, this.topicid, this.postid));
                        }
                    }
                    if (DNTRequest.GetFormString("continuereply") == "on")
                    {
                        base.SetUrl("postreply.aspx?topicid=" + this.topicid + "&forumpage=" + this.forumpageid + "&continuereply=yes");
                    }
                    if (stringBuilder.Length > 0)
                    {
                        this.UpdateUserCredits();
                        base.SetMetaRefresh(5);
                        base.SetShowBackLink(true);
                        if (this.infloat == 1)
                        {
                            base.AddErrLine(stringBuilder.ToString());
                            return;
                        }
                        base.AddMsgLine("<table cellspacing=\"0\" cellpadding=\"4\" border=\"0\"><tr><td colspan=2 align=\"left\"><span class=\"bold\"><nobr>发表回复成功,但图片/附件上传出现问题:</nobr></span><br /></td></tr></table>");
                    }
                    else
                    {
                        base.SetMetaRefresh();
                        base.SetShowBackLink(false);
                        if (postInfo.Invisible == 1)
                        {
                            base.AddMsgLine(string.Format("发表回复成功, 但需要经过审核才可以显示. {0}<br /><br />(<a href=\"" + base.ShowForumAspxRewrite(this.forumid, 0) + "\">点击这里返回 {1}</a>)", (DNTRequest.GetFormString("continuereply") == "on") ? "继续回复" : "返回该主题", this.forum.Name));
                        }
                        else
                        {
                            this.UpdateUserCredits();
                            base.MsgForward("postreply_succeed");
                            base.AddMsgLine(string.Format("发表回复成功, {0}<br />(<a href=\"" + base.ShowForumAspxRewrite(this.forumid, 0) + "\">点击这里返回 {1}</a>)<br />", (DNTRequest.GetFormString("continuereply") == "on") ? "继续回复" : "返回该主题", this.forum.Name));
                        }
                    }
                    if (this.topic.Replies < this.config.Ppp + 10)
                    {
                        ForumUtils.DeleteTopicCacheFile(this.topicid);
                    }
                    if (DNTRequest.GetString("emailnotify") == "on" && this.topic.PosterID != -1 && this.topic.PosterID != this.userid)
                    {
                        this.SendNotifyEmail(BBX.Entity.User.FindByID(this.topic.PosterID).Email.Trim(), postInfo, Utils.GetRootUrl(BaseConfigs.GetForumPath) + string.Format("showtopic.aspx?topicid={0}&page=end&jump=pid#{1}", this.topicid, this.postid));
                    }
                }
                return;
            }
        }

        //private void UpdateTopicInfo(string postmessage)
        //{
        //	int num = (ForumUtils.IsHidePost(postmessage) && this.usergroupinfo.AllowHideCode) ? 1 : 0;
        //	if (num == 1 && this.topic.Hide != 1)
        //	{
        //		this.topic.Hide = num;
        //		Topics.UpdateTopicHide(this.topicid);
        //	}
        //	if (Moderators.IsModer(this.useradminid, this.userid, this.topic.Fid) && this.topic.Attention == 1)
        //	{
        //		Topic.UpdateTopicAttentionByTidList(0, topicid);
        //	}
        //	else
        //	{
        //		if (this.topic.PosterID != -1 && this.userid == this.topic.PosterID)
        //		{
        //			Topic.UpdateTopicAttentionByTidList(1, topicid);
        //		}
        //	}
        //	Topics.UpdateTopicReplyCount(this.topicid);
        //}

        private void NormalValidate(AdminGroup admininfo, string postmessage, IUser user)
        {
            if (ForumUtils.IsCrossSitePost())
            {
                base.AddErrLine("您的请求来路不正确，无法提交。如果您安装了某种默认屏蔽来路信息的个人防火墙软件(如 Norton Internet Security)，请设置其不要禁止来路信息后再试。");
                return;
            }
            if (this.posttitle.Length > 60)
            {
                base.AddErrLine("标题最大长度为60个字符,当前为 " + this.posttitle.Length + " 个字符");
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
            if (this.topic.Special == 4 && DNTRequest.GetInt("debateopinion", 0) == 0)
            {
                base.AddErrLine("请选择您在辩论中的观点");
            }
            if (this.topic.Special == 4)
            {
                var debateTopic = Debate.FindByTid(this.topic.ID);
                if (debateTopic.Terminaled)
                {
                    base.AddErrLine("此辩论主题已经到期");
                }
            }
            if (this.config.DisablePostAD && this.useradminid < 1 && ((this.config.DisablePostADPostCount != 0 && user.Posts <= this.config.DisablePostADPostCount) || (this.config.DisablePostADRegMinute != 0 && DateTime.Now.AddMinutes((double)(-(double)this.config.DisablePostADRegMinute)) <= user.JoinDate.ToDateTime())))
            {
                string[] array = this.config.DisablePostADRegular.Replace("\r", "").Split('\n');
                for (int i = 0; i < array.Length; i++)
                {
                    string regular = array[i];
                    if (Posts.IsAD(regular, this.posttitle, postmessage))
                    {
                        base.AddErrLine("发帖失败，内容中有不符合新用户强力广告屏蔽规则的字符，请检查标题和内容，如有疑问请与管理员联系");
                    }
                }
            }
        }

        public Post CreatePostInfo(string postmessage)
        {
            var pi = new Post();
            pi.Fid = this.forumid;
            pi.Tid = this.topicid;
            pi.Layer++;
            //pi.Poster = this.username;
            //pi.Posterid = this.userid;
            bool flag = DNTRequest.GetString("htmlon").ToInt(0) == 1;
            var msg = pi.Message;
            if (this.useradminid == 1)
            {
                pi.Title = Utils.HtmlEncode(this.posttitle);
                if (!this.usergroupinfo.AllowHtml)
                    msg = Utils.HtmlEncode(postmessage);
                else
                    msg = (flag ? postmessage : Utils.HtmlEncode(postmessage));
            }
            else
            {
                pi.Title = Utils.HtmlEncode(ForumUtils.BanWordFilter(DNTRequest.GetString(this.config.Antispamposttitle)));
                if (!this.usergroupinfo.AllowHtml)
                    msg = Utils.HtmlEncode(ForumUtils.BanWordFilter(postmessage));
                else
                    msg = (flag ? ForumUtils.BanWordFilter(postmessage) : Utils.HtmlEncode(ForumUtils.BanWordFilter(postmessage)));
            }
            pi.Message = msg;
            //pi.Postdatetime = Utils.GetDateTime();
            if (Utils.StrIsNullOrEmpty(msg.Replace("\u3000", "")))
            {
                base.AddErrLine("内容不能为空, 请返回修改!");
                return pi;
            }
            if (this.useradminid != 1 && (ForumUtils.HasBannedWord(this.posttitle) || ForumUtils.HasBannedWord(postmessage)))
            {
                string arg = (ForumUtils.GetBannedWord(this.posttitle) == string.Empty) ? ForumUtils.GetBannedWord(postmessage) : ForumUtils.GetBannedWord(this.posttitle);
                base.AddErrLine(string.Format("对不起, 您提交的内容包含不良信息  <font color=\"red\">{0}</font>, 请返回修改!", arg));
                return pi;
            }
            //pi.Ip = WebHelper.UserHost;
            //pi.Lastedit = "";
            //pi.Debateopinion = DNTRequest.GetInt("debateopinion", 0);
            var debateopinion = DNTRequest.GetInt("debateopinion", 0);
            pi.Invisible = (this.needaudit ? 1 : 0);
            if (this.useradminid != 1 && !Moderators.IsModer(this.useradminid, this.userid, this.forumid) && (Scoresets.BetweenTime(this.config.Postmodperiods) || ForumUtils.HasAuditWord(pi.Title) || ForumUtils.HasAuditWord(msg)))
            {
                pi.Invisible = 1;
            }
            pi.UseSig = DNTRequest.GetInt("usesig");
            pi.HtmlOn = (this.usergroupinfo.AllowHtml && flag) ? 1 : 0;
            pi.SmileyOff = ((this.smileyoff != 0) ? this.smileyoff : DNTRequest.GetInt("smileyoff"));
            pi.BBCodeOff = ((this.usergroupinfo.AllowCusbbCode && this.forum.Allowbbcode == 1) ? DNTRequest.GetInt("bbcodeoff") : 1);
            pi.ParseUrlOff = DNTRequest.GetInt("parseurloff");
            //pi.Attachment = 0;
            //pi.Rate = 0;
            //pi.Ratetimes = 0;
            //pi.Title = this.topic.Title;
            if ((pi.Title != "" && Utils.GetCookie("lastposttitle") == Utils.MD5(pi.Title)) || Utils.GetCookie("lastpostmessage") == Utils.MD5(msg))
            {
                base.AddErrLine("请勿重复发帖");
                return pi;
            }
            //pi.Pid = Posts.CreatePost(pi);
            pi.Create();

            if (debateopinion > 0)
            {
                Debate.CreateDebateExpandInfo(pi.Tid, pi.ID, debateopinion, 0);
            }

            Utils.WriteCookie("lastposttitle", Utils.MD5(pi.Title));
            Utils.WriteCookie("lastpostmessage", Utils.MD5(msg));
            ForumUtils.WriteCookie("clearUserdata", "forum");
            return pi;
        }

        public Post GetPostAndTopic(AdminGroup admininfo)
        {
            var pi = new Post();
            if (this.postid == -1 && this.topicid == -1)
            {
                base.AddErrLine("无效的主题ID");
                return pi;
            }
            if (this.postid != -1)
            {
                //pi = Posts.GetPostInfo(this.topicid, this.postid);
                pi = Post.FindByID(postid);
                if (pi == null)
                {
                    base.AddErrLine("无效的帖子ID");
                    return pi;
                }
                if (this.topicid != pi.Tid)
                {
                    base.AddErrLine("主题ID无效");
                    return pi;
                }
                if (!string.IsNullOrEmpty(DNTRequest.GetString("quote")))
                {
                    var msg = pi.Message;
                    if (pi.Invisible != 0)
                    {
                        msg = "**** 作者被禁止或删除 内容自动屏蔽 ****";
                    }
                    else
                    {
                        string str = (pi.PosterID > 0) ? BBX.Entity.User.FindByID(pi.PosterID).GroupID.ToString() : null;
                        if (Utils.InArray(str, "4.5.6"))
                        {
                            msg = "**** 作者被禁止或删除 内容自动屏蔽 ****";
                        }
                    }
                    if (msg.IndexOf("[hide]") > -1 && msg.IndexOf("[/hide]") > -1)
                    {
                        this.message = string.Format("[quote] 原帖由 [b]{0}[/b] 于 {1} 发表\r\n ***隐藏帖*** [/quote]", pi.Poster, pi.PostDateTime.ToFullString());
                    }
                    else
                    {
                        this.message = string.Format("[quote]{0}\r\n [color=#999999]{1} 发表于 {2} [/color][url={5}showtopic.aspx?topicid={3}&postid={4}#{4}][img]{5}images/common/back.gif[/img][/url][/size][/quote]", new object[]
                        {
                            UBB.ClearAttachUBB(Utils.GetSubString(Regex.Replace(msg, "\\[quote\\](.|\\n)+\\[/quote\\]", "", RegexOptions.Multiline), 200, "......")),
                            pi.Poster,
                            pi.PostDateTime,
                            this.topicid,
                            this.postid,
                            Utils.GetRootUrl(this.forumpath)
                        });
                    }
                    pi.Html = msg;
                }
            }
            //this.topic = Topics.GetTopicInfo(this.topicid);
            this.topic = Topic.FindByID(this.topicid);
            if (this.topic == null)
            {
                base.AddErrLine("不存在的主题ID");
                return pi;
            }
            this.topictitle = this.topic.Title.Trim();
            this.pagetitle = this.topictitle;
            this.forumid = this.topic.Fid;
            if ((admininfo == null || !Moderators.IsModer(admininfo.ID, this.userid, this.forumid)) && this.topic.Closed == 1)
            {
                base.AddErrLine("主题已关闭无法回复");
                return pi;
            }
            if (this.topic.ReadPerm > this.usergroupinfo.Readaccess && this.topic.PosterID != this.userid && this.useradminid != 1 && this.forum.Moderators != null && !Utils.InArray(this.username, this.forum.Moderators.Split(',')))
            {
                base.AddErrLine("本主题阅读权限为: " + this.topic.ReadPerm + ", 您当前的身份 \"" + this.usergroupinfo.GroupTitle + "\" 阅读权限不够");
            }
            return pi;
        }

        private void UpdateUserCredits()
        {
            if (this.userid != -1)
            {
                CreditsFacade.PostReply(this.userid, this.forum.Replyperm, true);
            }
        }

        public void SendNotifyEmail(string email, Post postinfo, string jumpurl)
        {
            if (email.IsNullOrEmpty()) return;

            var sb = new StringBuilder("# 回复: <a href=\"" + jumpurl + "\" target=\"_blank\">" + this.topic.Title + "</a>");
            string text = "";
            if (this.userid > 0) text = this.userinfo.Email.Trim();

            sb.Append("\r\n");
            sb.Append("\r\n");
            sb.Append(UBB.ParseSimpleUBB(postinfo.Message));
            sb.Append("\r\n<hr/>");
            sb.Append("作 者:" + postinfo.Poster);
            sb.Append("\r\n");
            sb.Append("Email:<a href=\"mailto:" + text + "\" target=\"_blank\">" + text + "</a>");
            sb.Append("\r\n");
            sb.Append("URL:<a href=\"" + jumpurl + "\" target=\"_blank\">" + jumpurl + "</a>");
            sb.Append("\r\n");
            sb.Append("时 间:" + postinfo.PostDateTime);
            Emails.SendEmailNotify(email, "[" + this.config.Forumtitle + "回复通知]" + this.topic.Title, sb.ToString());
        }

        protected string AttachmentList() { return ""; }
    }
}