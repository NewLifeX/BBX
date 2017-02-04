using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using BBX.Common;
using BBX.Config;
using BBX.Entity;
using BBX.Forum;
using XCode;
using NewLife.Web;

namespace BBX.Web
{
    public class editpost : PageBase
    {
        public int forumid;
        public Post postinfo;
        public Topic topic;
        public bool isfirstpost;
        public EntityList<PollOption> polloptionlist;
        public Poll pollinfo;
        public Debate debateinfo;
        public List<Attachment> attachmentlist;
        public int attachmentcount;
        public string message;
        public int parseurloff;
        public int smileyoff;
        public int bbcodeoff;
        public int usesig;
        //public int allowimg;
        public Boolean disablepostctrl;
        public string attachextensions;
        public string attachextensionsnosize;
        public int attachsize;
        public UserExtcreditsInfo userextcreditsinfo;
        public UserExtcreditsInfo bonusextcreditsinfo = new UserExtcreditsInfo();
        public IXForum forum = new XForum();
        public string topictypeselectoptions;
        public DataTable smilietypes;
        public bool canpostattach;
        public bool allowviewattach;
        public bool canhtmltitle;
        public string htmltitle = string.Empty;
        public string topictags = string.Empty;
        public bool enabletag;
        public int forumpageid = DNTRequest.GetInt("forumpage", 1);
        public int htmlon;
        private string msg = "";
        private bool alloweditpost;
        public float mybonustranscredits;
        public int bonusCreditsTrans = Scoresets.GetBonusCreditsTrans();
        public IUser userinfo;
        public List<UserGroup> userGroupInfoList = UserGroup.FindAllWithCache();
        public int topicid = DNTRequest.GetInt("topicid", -1);
        public int postid = DNTRequest.GetInt("postid", -1);
        public int pageid = DNTRequest.GetInt("pageid", 1);
        public bool needaudit;
        private string postMessage = DNTRequest.GetString(GeneralConfigInfo.Current.Antispampostmessage);
        private string postTitle = DNTRequest.GetString(GeneralConfigInfo.Current.Antispamposttitle);
        public string customeditbuttons;
        protected String nowdate = DateTime.Now.ToString("yyyy-MM-dd");
        protected String nowdatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        protected override void ShowPage()
        {
            var adminGroupInfo = AdminGroup.FindByID(this.usergroupid);
            this.disablepostctrl = false;
            if (adminGroupInfo != null)
            {
                this.disablepostctrl = adminGroupInfo.DisablePostctrl;
            }
            if (this.userid == -1)
            {
                this.forum = new XForum();
                this.topic = new Topic();
                this.postinfo = new Post();
                base.AddErrLine("您尚未登录");
                return;
            }
            if (this.postid == -1)
            {
                base.AddErrLine("无效的帖子ID");
                return;
            }
            //this.postinfo = Posts.GetPostInfo(this.topicid, this.postid);
            postinfo = Post.FindByID(postid);
            if (this.postinfo == null)
            {
                base.AddErrLine("不存在的帖子ID");
                return;
            }
            this.pagetitle = ((String.IsNullOrEmpty(this.postinfo.Title)) ? "编辑帖子" : this.postinfo.Title);
            this.htmlon = this.postinfo.HtmlOn;
            this.message = this.postinfo.Message;
            this.isfirstpost = (this.postinfo.Layer == 0);
            if (this.topicid != this.postinfo.Tid || this.postinfo.Tid == -1)
            {
                base.AddErrLine("无效的主题ID");
                return;
            }
            //this.topic = Topics.GetTopicInfo(this.postinfo.Tid);
            topic = Topic.FindByID(postinfo.Tid);
            if (this.topic == null)
            {
                base.AddErrLine("不存在的主题ID");
                return;
            }
            if (this.topic.Special == 1 && this.postinfo.Layer == 0)
            {
                //修改为Xcode方法
                this.pollinfo = Poll.FindByTid(topic.ID);
                this.polloptionlist = PollOption.FindAllByTid(topic.ID);
            }
            if (this.topic.Special == 4 && this.postinfo.Layer == 0)
            {
                this.debateinfo = Debate.FindByTid(this.topic.ID);
            }
            this.forumid = this.topic.Fid;
            this.forum = Forums.GetForumInfo(this.forumid);
            this.needaudit = UserAuthority.NeedAudit(forum.Fid, forum.Modnewposts, this.useradminid, this.userid, this.usergroupinfo, this.topic);
            if (this.forum == null || this.forum.Layer == 0)
            {
                base.AddErrLine("版块已不存在");
                this.forum = new XForum();
                return;
            }
            if (!this.forum.Password.IsNullOrEmpty() && Utils.MD5(this.forum.Password) != ForumUtils.GetCookie("forum" + this.forumid + "password"))
            {
                base.AddErrLine("本版块被管理员设置了密码");
                base.SetBackLink(base.ShowForumAspxRewrite(this.forumid, 0));
                return;
            }
            if (this.forum.ApplytopicType == 1)
            {
                this.topictypeselectoptions = Forums.GetCurrentTopicTypesOption(this.forum.Fid, this.forum.Topictypes);
            }
            this.customeditbuttons = Caches.GetCustomEditButtonList();
            if (!UserAuthority.CanEditPost(this.postinfo, this.userid, this.useradminid, ref this.msg))
            {
                base.AddErrLine(this.msg);
                return;
            }
            //string allowAttachmentType = Attachments.GetAllowAttachmentType(this.usergroupinfo, this.forum);
            this.attachextensions = AttachType.GetAttachmentTypeArray(usergroupinfo, forum);
            this.attachextensionsnosize = AttachType.GetAttachmentTypeString(usergroupinfo, forum);
            int num = (this.userid > 0) ? Attachment.GetUploadFileSizeByuserid(this.userid) : 0;
            this.attachsize = this.usergroupinfo.MaxSizeperday - num;
            this.canpostattach = UserAuthority.PostAttachAuthority(this.forum, this.usergroupinfo, this.userid, ref this.msg);
            this.userinfo = BBX.Entity.User.FindByID(this.userid);
            this.attachmentlist = Attachment.FindAllByPid(this.postinfo.ID);
            this.attachmentcount = this.attachmentlist.Count;
            this.allowviewattach = UserAuthority.DownloadAttachment(this.forum, this.userid, this.usergroupinfo);
            this.smileyoff = ((!DNTRequest.IsPost()) ? this.postinfo.SmileyOff : (forum.AllowSmilies ? 0 : 1));
            //this.allowimg = this.forum.Allowimgcode;
            this.parseurloff = this.postinfo.ParseUrlOff;
            this.bbcodeoff = this.usergroupinfo.AllowCusbbCode ? this.postinfo.BBCodeOff : 1;
            this.usesig = this.postinfo.UseSig;
            this.userextcreditsinfo = Scoresets.GetScoreSet(Scoresets.GetTopicAttachCreditsTrans());
            if (this.bonusCreditsTrans > 0 && this.bonusCreditsTrans < 9)
            {
                this.bonusextcreditsinfo = Scoresets.GetScoreSet(this.bonusCreditsTrans);
                this.mybonustranscredits = Users.GetUserExtCredits(this.userid, this.bonusCreditsTrans);
            }
            if (!UserAuthority.VisitAuthority(this.forum, this.usergroupinfo, this.userid, ref this.msg))
            {
                base.AddErrLine(this.msg);
                return;
            }
            if (!Moderators.IsModer(this.useradminid, this.userid, this.forumid))
            {
                if (this.postinfo.PosterID != this.userid)
                {
                    base.AddErrLine("你并非作者, 且你当前的身份 \"" + this.usergroupinfo.GroupTitle + "\" 没有修改该帖的权限");
                    return;
                }
                if (this.config.Edittimelimit > 0 && this.postinfo.PostDateTime.AddMinutes(this.config.Edittimelimit) < DateTime.Now)
                {
                    base.AddErrLine("抱歉, 系统规定只能在帖子发表" + this.config.Edittimelimit + "分钟内才可以修改");
                    return;
                }
                if (this.config.Edittimelimit == -1)
                {
                    base.AddErrLine("抱歉，系统不允许修改帖子");
                    return;
                }
                this.alloweditpost = true;
            }
            else
            {
                if (adminGroupInfo != null && adminGroupInfo.AllowEditPost && Moderators.IsModer(this.useradminid, this.userid, this.forumid))
                {
                    this.alloweditpost = true;
                }
            }
            if (!this.alloweditpost && this.postinfo.PosterID != this.userid)
            {
                base.AddErrLine("您当前的身份没有编辑帖子的权限");
                return;
            }
            if (this.postinfo.Layer == 0)
            {
                this.canhtmltitle = this.usergroupinfo.AllowHtmlTitle;
            }
            if (Topics.GetMagicValue(this.topic.Magic, MagicType.HtmlTitle) == 1)
            {
                this.htmltitle = Topics.GetHtmlTitle(this.topic.ID).Replace("\"", "\\\"").Replace("'", "\\'");
            }
            this.enabletag = config.Enabletag && forum.AllowTag;
            if (this.enabletag && Topics.GetMagicValue(this.topic.Magic, MagicType.TopicTag) == 1)
            {
                foreach (var item in Tag.GetTagsListByTopic(this.topic.ID))
                {
                    if (item.OrderID > -1)
                    {
                        this.topictags += string.Format(" {0}", item.Name);
                    }
                }
                this.topictags = this.topictags.Trim();
            }
            this.userGroupInfoList.Sort((x, y) => x.Readaccess - y.Readaccess + (y.ID - x.ID));
            if (this.ispost)
            {
                base.SetBackLink("editpost.aspx?topicid=" + this.postinfo.Tid + "&postid=" + this.postinfo.ID);
                if (ForumUtils.IsCrossSitePost())
                {
                    base.AddErrLine("您的请求来路不正确，无法提交。如果您安装了某种默认屏蔽来路信息的个人防火墙软件(如 Norton Internet Security)，请设置其不要禁止来路信息后再试。");
                    return;
                }
                this.SetPostInfo(adminGroupInfo, this.userinfo, DNTRequest.GetString("htmlon").ToInt(0) == 1);
                if (base.IsErr())
                {
                    return;
                }
                //Posts.UpdatePost(this.postinfo);
                postinfo.Update();
                var stringBuilder = this.SetAttachmentInfo();
                if (base.IsErr())
                {
                    return;
                }
                CreditsFacade.UpdateUserCredits(this.userid);
                string url;
                if (this.topic.Special == 4)
                {
                    url = Urls.ShowTopicAspxRewrite(this.topic.ID, this.pageid);
                }
                else
                {
                    if (DNTRequest.GetQueryString("referer") != "")
                    {
                        url = string.Format("showtopic.aspx?page=end&forumpage={2}&topicid={0}#{1}", this.topic.ID, this.postinfo.ID, this.forumpageid);
                    }
                    else
                    {
                        if (this.pageid > 1)
                        {
                            if (this.config.Aspxrewrite == 1)
                            {
                                url = string.Format("showtopic-{0}-{2}{1}#{3}", new object[]
                                {
                                    this.topic.ID,
                                    this.config.Extname,
                                    this.pageid,
                                    this.postinfo.ID
                                });
                            }
                            else
                            {
                                url = string.Format("showtopic.aspx?topicid={0}&forumpage={3}&page={2}#{1}", new object[]
                                {
                                    this.topic.ID,
                                    this.postinfo.ID,
                                    this.pageid,
                                    this.forumpageid
                                });
                            }
                        }
                        else
                        {
                            if (this.config.Aspxrewrite == 1)
                            {
                                url = string.Format("showtopic-{0}{1}", this.topic.ID, this.config.Extname);
                            }
                            else
                            {
                                url = string.Format("showtopic.aspx?topicid={0}&forumpage={1}", this.topic.ID, this.forumpageid);
                            }
                        }
                    }
                }
                base.SetUrl(url);
                if (stringBuilder.Length > 0)
                {
                    base.SetMetaRefresh(5);
                    base.SetShowBackLink(true);
                    if (this.infloat == 1)
                    {
                        base.AddErrLine(stringBuilder.ToString());
                        return;
                    }
                    stringBuilder.Insert(0, "<table cellspacing=\"0\" cellpadding=\"4\" border=\"0\"><tr><td colspan=2 align=\"left\"><span class=\"bold\"><nobr>编辑帖子成功,但图片/附件上传出现问题:</nobr></span><br /></td></tr>");
                    stringBuilder.Append("</table>");
                    base.AddMsgLine(stringBuilder.ToString());
                }
                else
                {
                    if (this.postinfo.Layer == 0)
                    {
                        base.SetMetaRefresh(2, url);
                    }
                    else
                    {
                        base.SetMetaRefresh();
                    }
                    base.SetShowBackLink(false);
                    if (this.useradminid != 1 && (this.needaudit || this.topic.DisplayOrder == -2 || this.postinfo.Invisible == 1))
                    {
                        if (this.postinfo.Layer == 0)
                        {
                            base.SetUrl(base.ShowForumAspxRewrite(this.forumid, this.forumpageid));
                        }
                        else
                        {
                            base.SetUrl(base.ShowTopicAspxRewrite(this.topic.ID, this.forumpageid));
                        }
                        base.AddMsgLine("编辑成功, 但需要经过审核才可以显示");
                    }
                    else
                    {
                        base.MsgForward("editpost_succeed");
                        base.AddMsgLine("编辑帖子成功, 返回该主题");
                    }
                }
                if (this.postinfo.Layer == 0)
                {
                    ForumUtils.DeleteTopicCacheFile(this.topic.ID);
                    return;
                }
            }
            else
            {
                base.AddLinkCss(BaseConfigs.GetForumPath + "templates/" + this.templatepath + "/editor.css", "css");
            }
        }

        private int GetAttachmentUpdatedIndex(string attachmentId, string[] updatedAttArray)
        {
            for (int i = 0; i < updatedAttArray.Length; i++)
            {
                if (updatedAttArray[i] == attachmentId)
                {
                    return i;
                }
            }
            return -1;
        }

        private void SetPostInfo(AdminGroup admininfo, IUser user, bool ishtmlon)
        {
            if (this.postinfo.Layer == 0 && this.forum.ApplytopicType == 1 && this.forum.PostbytopicType == 1 && this.topictypeselectoptions != string.Empty)
            {
                var typeid = Request["typeid"].ToInt();
                if (typeid == 0)
                {
                    base.AddErrLine("主题类型不能为空");
                    return;
                }
                if (!Forums.IsCurrentForumTopicType(typeid, this.forum.Topictypes))
                {
                    base.AddErrLine("错误的主题类型");
                    return;
                }
            }
            if (DNTRequest.GetInt("isdeleteatt", 0) == 1)
            {
                var id = WebHelper.RequestInt("aid");
                var att = Attachment.FindByID(id);
                if (att != null)
                {
                    att.Delete();
                    this.attachmentlist = Attachment.FindAllByPid(this.postinfo.ID);
                    this.attachmentcount = attachmentlist.Count;
                }
                base.AddLinkCss(BaseConfigs.GetForumPath + "templates/" + this.templatepath + "/editor.css", "css");
                this.message = this.postinfo.Message;
                this.ispost = false;
                return;
            }
            if (string.IsNullOrEmpty(this.postTitle.Trim().Replace("\u3000", "")) && this.postinfo.Layer == 0)
            {
                base.AddErrLine("标题不能为空");
            }
            else
            {
                if (this.postTitle.Length > 60)
                {
                    base.AddErrLine("标题最大长度为60个字符,当前为 " + this.postTitle.Length.ToString() + " 个字符");
                }
            }
            if (String.IsNullOrEmpty(this.postMessage) || this.postMessage.Replace("\u3000", "").Equals(""))
            {
                base.AddErrLine("内容不能为空");
            }
            if (admininfo != null && !disablepostctrl)
            {
                if (this.postMessage.Length < this.config.Minpostsize)
                {
                    base.AddErrLine("您发表的内容过少, 系统设置要求帖子内容不得少于 " + this.config.Minpostsize.ToString() + " 字多于 " + this.config.Maxpostsize.ToString() + " 字");
                }
                else
                {
                    if (this.postMessage.Length > this.config.Maxpostsize)
                    {
                        base.AddErrLine("您发表的内容过多, 系统设置要求帖子内容不得少于 " + this.config.Minpostsize.ToString() + " 字多于 " + this.config.Maxpostsize.ToString() + " 字");
                    }
                }
            }
            if (this.config.DisablePostAD && this.useradminid < 1 && ((this.config.DisablePostADPostCount != 0 && user.Posts <= this.config.DisablePostADPostCount) || (this.config.DisablePostADRegMinute != 0 && DateTime.Now.AddMinutes((double)(-(double)this.config.DisablePostADRegMinute)) <= user.JoinDate.ToDateTime())))
            {
                string[] array = this.config.DisablePostADRegular.Replace("\r", "").Split('\n');
                for (int i = 0; i < array.Length; i++)
                {
                    string regular = array[i];
                    if (Posts.IsAD(regular, this.postTitle, this.postMessage))
                    {
                        base.AddErrLine("发帖失败，内容中有不符合新用户强力广告屏蔽规则的字符，请检查标题和内容，如有疑问请与管理员联系");
                        return;
                    }
                }
            }
            string[] array2 = Utils.SplitString(DNTRequest.GetString("PollItemname"), "\r\n");
            int num = 0;
            string @string = DNTRequest.GetString("topicprice");
            if (this.postinfo.Layer == 0)
            {
                if (!Utils.StrIsNullOrEmpty(DNTRequest.GetString("updatepoll")) && this.topic.Special == 1)
                {
                    this.pollinfo.Multiple = DNTRequest.GetInt("multiple", 0);
                    if (!this.usergroupinfo.AllowPostpoll)
                    {
                        base.AddErrLine("您当前的身份 \"" + this.usergroupinfo.GroupTitle + "\" 没有发布投票的权限");
                        return;
                    }
                    if (array2.Length < 2)
                    {
                        base.AddErrLine("投票项不得少于2个");
                    }
                    else
                    {
                        if (array2.Length > this.config.Maxpolloptions)
                        {
                            base.AddErrLine("系统设置为投票项不得多于" + this.config.Maxpolloptions + "个");
                        }
                        else
                        {
                            for (int j = 0; j < array2.Length; j++)
                            {
                                if (Utils.StrIsNullOrEmpty(array2[j]))
                                {
                                    base.AddErrLine("投票项不能为空");
                                }
                            }
                        }
                    }
                }
                if (Regex.IsMatch(@string, "^[0-9]*[0-9][0-9]*$") || @string == string.Empty)
                {
                    num = ((@string.ToInt(0) > 32767) ? 32767 : @string.ToInt(0));
                    if (this.topic.Special != 2)
                    {
                        if (num > this.usergroupinfo.MaxPrice && this.usergroupinfo.MaxPrice > 0)
                        {
                            if (String.IsNullOrEmpty(this.userextcreditsinfo.Unit))
                            {
                                base.AddErrLine(string.Format("主题售价不能高于 {0} {1}", this.usergroupinfo.MaxPrice, this.userextcreditsinfo.Name));
                            }
                            else
                            {
                                base.AddErrLine(string.Format("主题售价不能高于 {0} {1}({2})", this.usergroupinfo.MaxPrice, this.userextcreditsinfo.Name, this.userextcreditsinfo.Unit));
                            }
                        }
                        else
                        {
                            if (num > 0 && this.usergroupinfo.MaxPrice <= 0)
                            {
                                base.AddErrLine(string.Format("您当前的身份 \"{0}\" 未被允许出售主题", this.usergroupinfo.GroupTitle));
                            }
                            else
                            {
                                if (num < 0)
                                {
                                    base.AddErrLine("主题售价不能为负数");
                                }
                            }
                        }
                    }
                    else
                    {
                        if (!usergroupinfo.Is管理员)
                        {
                            if (!this.usergroupinfo.AllowBonus)
                            {
                                base.AddErrLine(string.Format("您当前的身份 \"{0}\" 未被允许进行悬赏", this.usergroupinfo.GroupTitle));
                            }
                            if (num < this.usergroupinfo.MinBonusprice || num > this.usergroupinfo.MaxBonusprice)
                            {
                                base.AddErrLine(string.Format("悬赏价格超出范围, 您应在 {0} - {1} {2}{3} 范围内进行悬赏", new object[]
                                {
                                    this.usergroupinfo.MinBonusprice,
                                    this.usergroupinfo.MaxBonusprice,
                                    this.userextcreditsinfo.Unit,
                                    this.userextcreditsinfo.Name
                                }));
                            }
                        }
                    }
                }
                else
                {
                    if (this.topic.Special != 2)
                    {
                        base.AddErrLine("主题售价只能为整数");
                    }
                    else
                    {
                        base.AddErrLine("悬赏价格只能为整数");
                    }
                }
                if (!Utils.StrIsNullOrEmpty(DNTRequest.GetString("updatedebate")) && this.topic.Special == 4)
                {
                    if (!this.usergroupinfo.AllowDebate)
                    {
                        base.AddErrLine("您当前的身份 \"" + this.usergroupinfo.GroupTitle + "\" 没有发布辩论的权限");
                        return;
                    }
                    if (Utils.StrIsNullOrEmpty(DNTRequest.GetString("positiveopinion")))
                    {
                        base.AddErrLine("正方观点不能为空");
                        return;
                    }
                    if (Utils.StrIsNullOrEmpty(DNTRequest.GetString("negativeopinion")))
                    {
                        base.AddErrLine("反方观点不能为空");
                        return;
                    }
                    if (Utils.StrIsNullOrEmpty(DNTRequest.GetString("terminaltime")))
                    {
                        base.AddErrLine("辩论的结束日期不能为空");
                        return;
                    }
                    if (!Utils.IsDateString(DNTRequest.GetString("terminaltime")))
                    {
                        base.AddErrLine("结束日期格式不正确");
                        return;
                    }
                }
            }
            var msg = postinfo.Message;
            if (this.useradminid == 1)
            {
                this.postinfo.Title = Utils.HtmlEncode(this.postTitle);
                if (!this.usergroupinfo.AllowHtml)
                {
                    msg = Utils.HtmlEncode(this.postMessage);
                }
                else
                {
                    msg = (ishtmlon ? this.postMessage : Utils.HtmlEncode(this.postMessage));
                }
            }
            else
            {
                this.postinfo.Title = Utils.HtmlEncode(ForumUtils.BanWordFilter(this.postTitle));
                if (!this.usergroupinfo.AllowHtml)
                {
                    msg = Utils.HtmlEncode(ForumUtils.BanWordFilter(this.postMessage));
                }
                else
                {
                    msg = (ishtmlon ? ForumUtils.BanWordFilter(this.postMessage) : Utils.HtmlEncode(ForumUtils.BanWordFilter(this.postMessage)));
                }
            }
            //postinfo.Html = msg;
            postinfo.Message = msg;
            this.postinfo.Title = ((this.postinfo.Title.Length > 60) ? this.postinfo.Title.Substring(0, 60) : this.postinfo.Title);
            if (this.useradminid != 1 && (ForumUtils.HasBannedWord(this.postTitle) || ForumUtils.HasBannedWord(this.postMessage)))
            {
                string arg = (ForumUtils.GetBannedWord(this.postTitle) == string.Empty) ? ForumUtils.GetBannedWord(this.postMessage) : ForumUtils.GetBannedWord(this.postTitle);
                base.AddErrLine(string.Format("对不起, 您提交的内容包含不良信息  <font color=\"red\">{0}</font>, 请返回修改!", arg));
                return;
            }
            this.topic.DisplayOrder = Topics.GetTitleDisplayOrder(this.usergroupinfo, this.useradminid, this.forum, this.topic, this.message, this.disablepostctrl);
            if (base.IsErr())
            {
                return;
            }
            if (this.postinfo.PostDateTime.AddSeconds(60) < DateTime.Now && this.config.Editedby == 1 && this.useradminid != 1)
            {
                this.postinfo.LastEdit = this.username + " 最后编辑于 " + Utils.GetDateTime();
            }
            this.postinfo.UseSig = DNTRequest.GetString("usesig").ToInt(0);
            this.postinfo.HtmlOn = ((this.usergroupinfo.AllowHtml && ishtmlon) ? 1 : 0);
            this.postinfo.SmileyOff = ((this.smileyoff == 0) ? DNTRequest.GetInt("smileyoff") : this.smileyoff);
            this.postinfo.BBCodeOff = ((this.usergroupinfo.AllowCusbbCode) ? DNTRequest.GetInt("bbcodeoff") : 1);
            this.postinfo.ParseUrlOff = DNTRequest.GetInt("parseurloff");
            this.postinfo.Invisible = (this.needaudit ? 1 : 0);
            if (this.alloweditpost)
            {
                this.SetTopicInfo(array2, num, this.postMessage);
            }
            return;
        }

        private void SetTopicInfo(string[] pollitem, int topicprice, string postmessage)
        {
            if (this.postinfo.Layer == 0)
            {
                new StringBuilder("");
                if (this.topic.Special == 1)
                {
                    string text = Utils.HtmlEncode(DNTRequest.GetFormString("PollItemname").Trim());
                    if (text.IsNullOrEmpty())
                    {
                        base.AddErrLine("投票项为空");
                        return;
                    }
                    int num = (DNTRequest.GetString("multiple") == "on") ? 1 : 0;
                    int num2 = DNTRequest.GetInt("maxchoices", 0);
                    if (num == 1 && num2 > pollitem.Length)
                    {
                        num2 = pollitem.Length;
                    }
                    if (!Poll.UpdatePoll(
                        this.topic.ID, num, pollitem.Length,
                        DNTRequest.GetFormString("PollOptionID").Trim(),
                        text,
                        DNTRequest.GetFormString("PollOptionDisplayOrder").Trim(),
                        Utility.ToDateTime(DNTRequest.GetString("enddatetime")),
                        num2,
                        (DNTRequest.GetString("visiblepoll") == "on") ? 1 : 0,
                        (DNTRequest.GetString("allowview") == "on") ? true : false))
                    {
                        base.AddErrLine("投票错误,请检查显示顺序");
                        return;
                    }
                }
                if (this.topic.Special == 4)
                {
                    this.debateinfo.PositiveOpinion = DNTRequest.GetString("positiveopinion");
                    this.debateinfo.NegativeOpinion = DNTRequest.GetString("negativeopinion");
                    this.debateinfo.TerminalTime = Request["terminaltime"].ToDateTime();
                    if (this.debateinfo.Update() < 1)
                    {
                        base.AddErrLine("辩论修改选择了无效的主题");
                        return;
                    }
                }
                int @int = DNTRequest.GetInt("iconid", 0);
                this.topic.IconID = ((@int > 15 || @int < 0) ? 0 : @int);
                this.topic.Title = this.postinfo.Title;
                if (this.topic.Special == 2)
                {
                    int num3 = topicprice - this.topic.Price;
                    if (num3 > 0)
                    {
                        if (this.bonusCreditsTrans < 1 || this.bonusCreditsTrans > 8)
                        {
                            base.AddErrLine("系统未设置\"交易积分设置\", 无法判断当前要使用的(扩展)积分字段, 暂时无法发布悬赏");
                            return;
                        }
                        if (!usergroupinfo.Is管理员 && Users.GetUserExtCredits(this.topic.PosterID, this.bonusCreditsTrans) < (float)num3)
                        {
                            base.AddErrLine("主题作者 " + Scoresets.GetValidScoreName()[this.bonusCreditsTrans] + " 不足, 无法追加悬赏");
                            return;
                        }
                        this.topic.Price = topicprice;
                        BBX.Entity.User.UpdateUserExtCredits(this.topic.PosterID, this.bonusCreditsTrans, (float)(-(float)num3) * (Scoresets.GetCreditsTax() + 1f));
                    }
                    else
                    {
                        if (num3 < 0 && !usergroupinfo.Is管理员)
                        {
                            base.AddErrLine("不能降低悬赏价格");
                            return;
                        }
                    }
                }
                else
                {
                    if (this.topic.Special == 0)
                    {
                        this.topic.Price = topicprice;
                    }
                }
                if (this.usergroupinfo.AllowSetreadPerm)
                {
                    this.topic.ReadPerm = ((DNTRequest.GetInt("topicreadperm", 0) > 255) ? 255 : DNTRequest.GetInt("topicreadperm", 0));
                }
                if (ForumUtils.IsHidePost(postmessage) && this.usergroupinfo.AllowHideCode)
                {
                    this.topic.Hide = 1;
                }
                this.topic.TypeID = DNTRequest.GetFormInt("typeid", 0);
                this.htmltitle = DNTRequest.GetString("htmltitle").Trim();
                if (!this.htmltitle.IsNullOrEmpty() && Utils.HtmlDecode(this.htmltitle).Trim() != this.topic.Title)
                {
                    this.topic.Magic = 11000;
                }
                else
                {
                    this.topic.Magic = 0;
                }
                this.topic.DisplayOrder = Topics.GetTitleDisplayOrder(this.usergroupinfo, this.useradminid, this.forum, this.topic, this.message, this.disablepostctrl);
                Tag.DeleteTopicTags(this.topic.ID);
                //Topics.DeleteRelatedTopics(this.topic.ID);
                TopicTagCache.DeleteRelatedTopics(this.topic.ID);
                string text2 = DNTRequest.GetString("tags").Trim();
                if (this.enabletag && !text2.IsNullOrEmpty())
                {
                    if (ForumUtils.InBanWordArray(text2))
                    {
                        base.AddErrLine("标签中含有系统禁止词语,请修改");
                        return;
                    }
                    string[] array = Utils.SplitString(text2, " ", true, 2, 10);
                    if (array.Length <= 0 || array.Length > 5)
                    {
                        base.AddErrLine("超过标签数的最大限制或单个标签长度没有介于2-10之间，最多可填写 5 个标签");
                        return;
                    }
                    this.topic.Magic = Topics.SetMagicValue(this.topic.Magic, MagicType.TopicTag, 1);
                    Tag.CreateTopicTags(array, this.topic.ID, this.userid, Utils.GetDateTime());
                }
                //Topics.UpdateTopic(this.topic);
                topic.Update();
                if (this.canhtmltitle && !this.htmltitle.IsNullOrEmpty() && this.htmltitle != this.topic.Title)
                {
                    Topics.WriteHtmlTitleFile(Utils.RemoveUnsafeHtml(this.htmltitle), this.topic.ID);
                    return;
                }
            }
            else
            {
                if (ForumUtils.IsHidePost(postmessage) && this.usergroupinfo.AllowHideCode)
                {
                    this.topic.Hide = 1;
                    //Topics.UpdateTopic(this.topic);
                    topic.Update();
                }
            }
        }

        private StringBuilder SetAttachmentInfo()
        {
            string formString = DNTRequest.GetFormString("attachid");
            var stringBuilder = new StringBuilder();
            var atts = Attachment.FindAllByUid(0, formString);
            //var list = new List<Attachment>();
            //var array = atts;
            //for (int i = 0; i < array.Length; i++)
            //{
            //    var attachmentInfo = array[i];
            //    if (attachmentInfo.Pid == 0)
            //    {
            //        list.Add(attachmentInfo);
            //    }
            //}
            var list = atts.ToList().Where(e => e.Pid == 0);
            if (!string.IsNullOrEmpty(formString))
            {
                Attachments.UpdateAttachment(atts.ToArray(), this.topic.ID, this.postinfo.ID, this.postinfo, ref stringBuilder, this.userid, this.config, this.usergroupinfo);
            }
            return stringBuilder;
        }

        protected string FormatDateTimeString(DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd");
        }

        protected string AttachmentList()
        {
            var sb = new StringBuilder();
            sb.Append("[");
            foreach (var att in this.attachmentlist)
            {
                if (att.IsImage)
                {
                    sb.AppendFormat("{{\\'aid\\':{0},\\'uid\\':{1},\\'attachment\\':\\'{2}\\',\\'attachkey\\':\\'{3}\\',\\'description\\':\\'{4}\\'}},", att.ID, this.userid.ToString(), att.Name.Replace("'", "\\'"), Thumbnail.GetKey(att.ID), (att.Description + "").Replace("'", "\\'"));
                }
            }
            return sb.ToString().TrimEnd(',') + "]";
        }
    }
}