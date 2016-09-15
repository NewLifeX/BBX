using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using BBX.Common;
using BBX.Config;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web.UI
{
    public class TopicPage : PageBase
    {
        public Topic topic;
        public int forumid = DNTRequest.GetInt("forumid", 0);
        public int topicid = DNTRequest.GetInt("topicid", -1);
        public bool needlogin;
        public IXForum forum = new XForum();
        public bool canreply;
        public int ismoder;
        protected string msg = "";
        public bool canposttopic;
        public bool isenddebate;
        public List<Attachment> attachmentlist;
        public string doublead = "";
        public string floatad = "";
        public string[] quickbgad;
        public int pageid;
        public int postcount;
        public int pagecount;
        public string pagenumbers = "";
        public int topicviews;
        public string[] pagewordad = new string[0];
        public bool allowvote;
        public int parseurloff = Utils.StrToInt(DNTRequest.GetString("parseurloff"), 0);
        public int smileyoff;
        public int bbcodeoff = 1;
        public int usesig = (ForumUtils.GetCookie("sigstatus") == "0") ? 0 : 1;
        public string voters = "";
        public Boolean disablepostctrl;
        public AdminGroup admininfo;
        public string onlyauthor = DNTRequest.GetString("onlyauthor");
        public string topictypes = "";
        public TopicIdentify topicidentify;
        public string[] score = new string[0];
        public string[] scoreunit = new string[0];
        public string inpostad = "";
        public string quickeditorad = "";
        public List<Topic> relatedtopics = new List<Topic>();
        public int ppp = Utils.StrToInt(ForumUtils.GetCookie("ppp"), GeneralConfigInfo.Current.Ppp);
        public bool enabletag;
        public string postleaderboardad = "";
        public int replynotificationstatus = GeneralConfigInfo.Current.Replynotificationstatus;
        public int replyemailstatus = GeneralConfigInfo.Current.Replyemailstatus;
        public string downloadattachmenttip = "";
        public bool isnewbie;
        public int postid = DNTRequest.GetInt("postid", 0);
        public int forumpid = Utils.GetCookie("forumpageid").ToInt(1);

        protected bool ValidateInfo()
        {
            if (this.topicid == -1 || this.topic == null)
            {
                base.AddErrLine("无效或不存在的主题ID");
                return false;
            }
            if (this.topic.Closed > 1)
            {
                this.topicid = this.topic.Closed;
                //this.topic = Topics.GetTopicInfo(this.topicid);
                this.topic = Topic.FindByID(this.topicid);
                if (this.topic == null || this.topic.Closed > 1)
                {
                    base.AddErrLine("不存在的主题ID");
                    return false;
                }
            }
            if (this.topic.ReadPerm > this.usergroupinfo.Readaccess && this.topic.PosterID != this.userid && this.useradminid != 1 && this.ismoder != 1)
            {
                base.AddErrLine(string.Format("本主题阅读权限为: {0}, 您当前的身份 \"{1}\" 阅读权限不够", this.topic.ReadPerm, this.usergroupinfo.GroupTitle));
                if (this.userid == -1)
                {
                    this.needlogin = true;
                }
                return false;
            }
            if (this.topic.DisplayOrder == -1)
            {
                base.AddErrLine("此主题已被删除！");
                return false;
            }
            if (this.topic.DisplayOrder == -2 && this.topic.PosterID != this.userid)
            {
                base.AddErrLine("此主题未经审核！");
                return false;
            }
            return this.ValidateForumPassword() && this.ValidateAuthority();
        }

        public bool ValidateAuthority()
        {
            if (!UserAuthority.VisitAuthority(this.forum, this.usergroupinfo, this.userid, ref this.msg))
            {
                base.AddErrLine(this.msg);
                if (this.userid == -1)
                {
                    this.needlogin = true;
                }
                return false;
            }
            this.canreply = (this.ismoder == 1 || UserAuthority.PostReply(forum, this.userid, this.usergroupinfo, this.topic));
            if (this.userid > -1)
            {
                this.canposttopic = UserAuthority.PostAuthority(this.forum, this.usergroupinfo, this.userid, ref this.msg);
                if (!this.canposttopic && !this.pagename.StartsWith("showtopic") && !this.pagename.StartsWith("showtree"))
                {
                    base.AddErrLine(this.msg);
                    return false;
                }
            }
            if (this.useradminid != 1 && !this.usergroupinfo.DisablePeriodctrl)
            {
                string text = "";
                if (Scoresets.BetweenTime(this.config.Postbanperiods, out text))
                {
                    this.canposttopic = false;
                }
                this.isnewbie = UserAuthority.CheckNewbieSpan(this.userid);
            }
            return true;
        }

        public bool ValidateForumPassword()
        {
            if (!Utils.StrIsNullOrEmpty(this.forum.Password) && Utils.MD5(this.forum.Password) != ForumUtils.GetCookie("forum" + this.forumid + "password"))
            {
                base.AddErrLine("本版块被管理员设置了密码");
                if (this.config.Aspxrewrite == 1)
                {
                    HttpContext.Current.Response.Redirect(BaseConfigs.GetForumPath + "showforum-" + this.forumid + this.config.Extname, true);
                }
                else
                {
                    HttpContext.Current.Response.Redirect(BaseConfigs.GetForumPath + "showforum.aspx?forumid=" + this.forumid, true);
                }
                return false;
            }
            return true;
        }

        public void GetForumAds(int forumid)
        {
            this.headerad = Advertisement.GetOneHeaderAd("", forumid);
            this.footerad = Advertisement.GetOneFooterAd("", forumid);
            this.pagewordad = Advertisement.GetPageWordAd("", forumid);
            this.doublead = Advertisement.GetDoubleAd("", forumid);
            this.floatad = Advertisement.GetFloatAd("", forumid);
            if (forumid > 0)
            {
                this.postleaderboardad = Advertisement.GetOnePostLeaderboardAD("", forumid);
            }
        }

        public void EditorState()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("var Allowhtml=1;\r\n");
            this.smileyoff = forum.AllowSmilies ? 0 : 1;
            stringBuilder.Append("var Allowsmilies=" + (1 - this.smileyoff) + ";\r\n");
            if (this.forum.Allowbbcode == 1 && this.usergroupinfo.AllowCusbbCode)
            {
                this.bbcodeoff = 0;
            }
            stringBuilder.Append("var Allowbbcode=" + (1 - this.bbcodeoff) + ";\r\n");
            stringBuilder.Append("var Allowimgcode=" + this.forum.Allowimgcode + ";\r\n");
            base.AddScript(stringBuilder.ToString());
        }

        public void UpdateMetaInfo(string metadescritpion)
        {
            string seokeywords = this.config.Seokeywords;
            metadescritpion = ((metadescritpion.Length > 100) ? metadescritpion.Substring(0, 100) : metadescritpion);
            if (this.enabletag && Topics.GetMagicValue(this.topic.Magic, MagicType.TopicTag) == 1)
            {
                seokeywords = Tag.GetTagsByTopicId(this.topic.ID);
            }
            meta = PageHelper.UpdateMetaInfo(meta, seokeywords, metadescritpion, this.config.Seohead);
        }

        public Topic GetTopicInfo()
        {
            string mode = DNTRequest.GetString("go").Trim().ToLower();
            if (String.IsNullOrEmpty(mode))
            {
                this.forumid = 0;
            }
            else
            {
                if (this.forumid == 0) mode = "";
            }
            //string a;
            Topic topicInfo;

            if (mode == "prev")
                topicInfo = Topic.GetTopicInfo(this.topicid, this.forumid, 1);
            else if (mode == "next")
                topicInfo = Topic.GetTopicInfo(this.topicid, this.forumid, 2);
            else
                topicInfo = Topic.FindByID(this.topicid);

            if (topicInfo == null)
            {
                if (mode == "prev")
                {
                    this.msg = "没有更旧的主题, 请返回";
                }
                else
                {
                    if (mode == "next")
                    {
                        this.msg = "没有更新的主题, 请返回";
                    }
                    else
                    {
                        this.msg = "该主题不存在";
                    }
                }
                //if (DiscuzCloud.GetCloudServiceEnableStatus("connect"))
                //{
                //    DeleteFeed deleteFeed = new DeleteFeed();
                //    deleteFeed.DeleteTopicPushedFeed(this.topicid);
                //}
                base.AddErrLine(this.msg);
                this.GetForumAds(0);
            }
            return topicInfo;
        }

        public void GetPostAds(PostpramsInfo postpramsInfo, int count)
        {
            this.inpostad = Advertisement.GetInPostAd("", this.forumid, this.templatepath, (count > postpramsInfo.Pagesize) ? postpramsInfo.Pagesize : count);
            this.quickeditorad = Advertisement.GetQuickEditorAD("", this.forumid);
            this.quickbgad = Advertisement.GetQuickEditorBgAd("", this.forumid);
            if (this.quickbgad.Length <= 1)
            {
                this.quickbgad = new string[]
                {
                    "",
                    ""
                };
            }
        }

        public void BindPageCountAndId()
        {
            this.ppp = ((this.ppp <= 0) ? this.config.Ppp : this.ppp);
            if (DNTRequest.GetInt("stand", 0) == 0)
            {
                this.postcount = Post.GetPostCountByPosterId(this.onlyauthor, this.topicid, this.topic.PosterID, this.topic.Replies);
            }
            else
            {
                this.postcount = Post.GetDebatePostCount(this.onlyauthor, this.topicid, this.topic.PosterID, DNTRequest.GetInt("stand", 0));
            }
            this.pagecount = ((this.postcount % this.ppp == 0) ? (this.postcount / this.ppp) : (this.postcount / this.ppp + 1));
            if (this.pagecount == 0)
            {
                this.pagecount = 1;
            }
            this.pageid = (DNTRequest.GetString("page").ToLower().Equals("end") ? this.pagecount : DNTRequest.GetInt("page", 1));
            if (this.postid > 0)
            {
                this.pageid = Post.GetPostCountBeforePid(this.postid, this.topicid) / this.ppp + 1;
            }
            this.pageid = ((this.pageid < 1) ? 1 : this.pageid);
            this.pageid = ((this.pageid > this.pagecount) ? this.pagecount : this.pageid);
        }

        public void IsModer()
        {
            if (useradminid != 0)
            {
                ismoder = Moderators.IsModer(useradminid, userid, forum.Fid) ? 1 : 0;
                admininfo = AdminGroup.FindByID(usergroupid);
                if (admininfo != null)
                {
                    disablepostctrl = admininfo.DisablePostctrl;
                }
            }
        }

        public void BindDownloadAttachmentTip()
        {
            if (!Scoresets.IsSetDownLoadAttachScore())
            {
                return;
            }
            float[] userExtCredits = Scoresets.GetUserExtCredits(CreditsOperationType.DownloadAttachment);
            string[] validScoreName = Scoresets.GetValidScoreName();
            string[] validScoreUnit = Scoresets.GetValidScoreUnit();
            for (int i = 0; i < userExtCredits.Length; i++)
            {
                if (userExtCredits[i] < 0f)
                {
                    this.downloadattachmenttip += string.Format("{0}:{1} {2};", validScoreName[i + 1], Math.Abs(userExtCredits[i]), validScoreUnit[i + 1]);
                }
            }
            this.downloadattachmenttip = this.downloadattachmenttip.TrimEnd(';');
        }
    }
}