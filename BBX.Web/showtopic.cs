using System;
using System.Collections.Generic;
using System.Web;
using BBX.Common;
using BBX.Config;
using BBX.Entity;
using BBX.Forum;
using BBX.Web.UI;

namespace BBX.Web
{
    public class showtopic : TopicPage
    {
        public List<Post> postlist = new List<Post>();
        //public PollInfo pollinfo = new PollInfo();
        public Poll pollinfo = new Poll();
        public bool showpollresult = true;
        public Dictionary<int, int> debateList = new Dictionary<int, int>();
        public Debate debateexpand = new Debate();
        //public string goodscategoryfid = (GeneralConfigInfo.Current.Enablemall <= 0) ? "{}" : MallPluginProvider.GetInstance().GetGoodsCategoryWithFid();
        public UserExtcreditsInfo userextcreditsinfo = new UserExtcreditsInfo();
        //public List<PrivateMessageInfo> pmlist = new List<PrivateMessageInfo>();
        public List<BonusLog> bonuslogs = new List<BonusLog>();
        public int posterid = DNTRequest.GetInt("posterid", 0);
        public string nextpage = "";
        public string listlink = "";
        public string navhomemenu = "";
        public IXForum[] visitedforums = Forums.GetVisitedForums();
        public bool showvisitedforumsmenu;
        private IUser userInfo;
        public int hide;
        public int typeid = DNTRequest.GetInt("typeid", -1);
        public int stand = DNTRequest.GetInt("stand", 0);
        public int invisible = DNTRequest.GetInt("invisible", 0);
        public string[] postleftshow;
        public string[] userfaceshow;

        protected override void ShowPage()
        {
            this.topic = base.GetTopicInfo();
            if (this.topic == null)
            {
                return;
            }
            this.topicid = this.topic.ID;
            this.forumid = this.topic.Fid;
            this.forum = Forums.GetForumInfo(this.forumid);
            if (this.forum == null)
            {
                base.AddErrLine("不存在的版块ID");
                return;
            }
            if (!base.ValidateInfo() || base.IsErr())
            {
                return;
            }
            base.IsModer();
            int topicPrice = this.GetTopicPrice(this.topic);
            if (this.topic.Special == 0 && topicPrice > 0)
            {
                HttpContext.Current.Response.Redirect(this.forumpath + "buytopic.aspx?topicid=" + this.topic.ID);
                return;
            }
            if (this.postid > 0 && Post.FindByID(this.postid) == null)
            {
                base.AddErrLine("该帖可能已被删除 " + string.Format("<a href=\"{0}\">[返回主题]</a>", base.ShowTopicAspxRewrite(this.topicid, 1)));
                return;
            }
            ForumUtils.SetVisitedForumsCookie(this.forumid.ToString());
            if (this.userid > 0)
            {
                this.userInfo = BBX.Entity.User.FindByID(this.userid);
            }
            if (this.topic.Identify > 0)
            {
                this.topicidentify = TopicIdentify.FindByID(this.topic.Identify);
            }
            this.pagetitle = string.Format("{0} - {1}", this.topic.Title, Utils.RemoveHtml(this.forum.Name));
            base.GetForumAds(this.forum.Fid);
            TopicType.GetTopicTypeArray().TryGetValue(this.topic.TypeID, out this.topictypes);
            this.topictypes = (Utils.StrIsNullOrEmpty(this.topictypes) ? "" : ("[" + this.topictypes + "]"));
            this.userextcreditsinfo = Scoresets.GetScoreSet(Scoresets.GetTopicAttachCreditsTrans());
            this.score = Scoresets.GetValidScoreName();
            this.scoreunit = Scoresets.GetValidScoreUnit();
            this.navhomemenu = Caches.GetForumListMenuDivCache(this.usergroupid, this.userid, this.config.Extname);
            base.EditorState();
            string[] array = this.config.Customauthorinfo.Split('|');
            this.postleftshow = array[0].Split(',');
            this.userfaceshow = array[1].Split(',');
            this.onlyauthor = ((this.onlyauthor == "1" || this.onlyauthor == "2") ? this.onlyauthor : "0");
            this.BindPageCountAndId();
            base.GetPostAds(this.GetPostPramsInfo(topicPrice), this.postlist.Count);
            //this.bonuslogs = Bonus.GetLogs(this.topic);
            if (this.topic.Special == 3) this.bonuslogs = BonusLog.GetLogs(this.topic.ID);
            if (this.topic.Special == 1)
            {
                this.GetPollInfo();
            }
            if (this.topic.Special == 4)
            {
                this.GetDebateInfo();
            }
            this.enabletag = config.Enabletag && forum.AllowTag;
            if (this.postlist != null && this.postlist.Count > 0)
            {
                base.UpdateMetaInfo(Utils.RemoveHtml(this.postlist[0].Message));
            }
            this.IsGuestCachePage();
            //Topic.UpdateViewCount(this.topicid, 1);
            topic.Views++;
            Topics.MarkOldTopic(this.topic);
            this.topicviews = this.topic.Views;
            Online.UpdateAction(this.olid, UserAction.ShowTopic, this.forumid, this.forum.Name, this.topicid, this.topic.Title);
            if (DNTRequest.GetInt("fromfav", 0) > 0)
            {
                //Favorites.UpdateUserFavoriteViewTime(this.userid, this.topicid);
                var fav = Favorite.FindByUidAndTid(userid, topicid);
                if (fav != null)
                {
                    fav.ViewTime = DateTime.Now;
                    fav.Update();
                }
            }
        }

        public void IsGuestCachePage()
        {
            if (this.userid == -1 && this.pageid > 0 && this.pageid < this.pagecount && ForumUtils.IsGuestCachePage(this.pageid, "showtopic"))
            {
                this.isguestcachepage = 1;
            }
        }

        public void GetPollInfo()
        {
            var pi = Poll.FindByTid(this.topic.ID);
            if (pi == null) return;

            this.pollinfo = pi;

            this.voters = misc.GetVoters(this.topicid, this.userid, this.username, out this.allowvote);
            if (this.pollinfo.Uid != this.userid && this.useradminid != 1 && this.pollinfo.Visible == 1 && (this.allowvote || (this.userid == -1 && !Utils.InArray(this.topicid.ToString(), ForumUtils.GetCookie("polled")))))
            {
                this.showpollresult = false;
            }
            if (pollinfo.Expiration <= DateTime.MinValue)
            {
                this.pollinfo.Expiration = DateTime.Now;
            }
            if (pollinfo.Expiration < DateTime.Now)
            {
                this.allowvote = false;
            }
        }

        public void GetDebateInfo()
        {
            this.debateexpand = Debate.FindByTid(this.topicid);
            this.debateList = Debates.GetPostDebateList(this.topicid);
            if (this.debateexpand.Terminaled)
            {
                this.isenddebate = true;
                if (this.isenddebate)
                {
                    this.canreply = false;
                }
            }
            foreach (var item in this.postlist)
            {
                if (this.debateList != null && this.debateList.ContainsKey(item.ID))
                {
                    item.Debateopinion = this.debateList[item.ID];
                }
            }
        }

        public int GetTopicPrice(Topic topicInfo)
        {
            int result = 0;
            if (topicInfo.Special == 0 && topicInfo.Price > 0 && this.userid != topicInfo.PosterID && this.ismoder != 1)
            {
                result = topicInfo.Price;
                var max = Scoresets.GetMaxChargeSpan();
                if (PaymentLog.IsBuyer(topicInfo.ID, this.userid) || max != 0 && topicInfo.PostDateTime.AddHours(max) < DateTime.Now)
                {
                    result = -1;
                }
            }
            return result;
        }

        public PostpramsInfo GetPostPramsInfo(int price)
        {
            var pi = new PostpramsInfo();
            pi.Fid = this.forum.Fid;
            pi.Tid = this.topicid;
            pi.Jammer = this.forum.Jammer;
            pi.Pagesize = this.ppp;
            pi.Pageindex = this.pageid;
            pi.Getattachperm = this.forum.Getattachperm;
            pi.Usergroupid = this.usergroupid;
            pi.Attachimgpost = this.config.Attachimgpost;
            pi.Showattachmentpath = this.config.Showattachmentpath;
            pi.Price = price;
            pi.Usergroupreadaccess = ((this.ismoder == 1) ? 0x7FFFFFFF : this.usergroupinfo.Readaccess);
            pi.CurrentUserid = this.userid;
            pi.Showimages = this.forum.Allowimgcode;
            pi.Smiliesinfo = Smilies.GetSmiliesListWithInfo();
            pi.Customeditorbuttoninfo = Editors.GetCustomEditButtonListWithInfo();
            pi.Smiliesmax = this.config.Smiliesmax;
            pi.Bbcodemode = this.config.Bbcodemode;
            pi.CurrentUserGroup = this.usergroupinfo;
            //pi.Topicinfo = this.topic.Cast<TopicInfo>();
            pi.Hide = ((this.topic.Hide >= 1) ? ((this.ismoder == 1 || Post.IsReplier(this.topicid, this.userid)) ? -1 : this.topic.Hide) : this.topic.Hide);
            pi.Hide = ((this.topic.PosterID == this.userid) ? -2 : pi.Hide);
            this.hide = pi.Hide;
            pi.Condition = Posts.GetPostPramsInfoCondition(this.onlyauthor, this.topicid, this.posterid);
            pi.TemplateWidth = Template.GetWidth(this.templatepath);
            pi.Usercredits = ((this.userInfo == null) ? 0 : this.userInfo.Credits);
            pi.Invisible = this.invisible;
            switch (this.stand)
            {
                case 0:
                    this.postlist = Posts.GetPostList(pi, out this.attachmentlist, this.ismoder == 1);
                    break;

                case 1:
                    this.postlist = Debates.GetPositivePostList(pi, out this.attachmentlist, this.ismoder == 1);
                    break;

                case 2:
                    this.postlist = Debates.GetNegativePostList(pi, out this.attachmentlist, this.ismoder == 1);
                    break;
            }
            if (this.topic.Special == 4)
            {
                string text = "";
                foreach (var item in this.postlist)
                {
                    text = text + item.ID + ",";
                }
                var postDiggs = Debates.GetPostDiggs(text.Trim(','));
                foreach (var item in this.postlist)
                {
                    if (postDiggs.ContainsKey(item.ID))
                    {
                        item.Diggs = postDiggs[item.ID];
                    }
                }
            }
            if (this.postlist.Count == 0)
            {
                TopicAdmins.RepairTopicList(this.topicid.ToString());
                this.topic = base.GetTopicInfo();
                this.BindPageCountAndId();
                pi.Pageindex = this.pagecount;
                this.postlist = Posts.GetPostList(pi, out this.attachmentlist, this.ismoder == 1);
            }
            foreach (var current3 in this.attachmentlist)
            {
                if (Forums.AllowGetAttachByUserID(this.forum.Permuserlist, this.userid))
                {
                    current3.Getattachperm = 1;
                    current3.AllowRead = true;
                }
            }
            base.BindDownloadAttachmentTip();
            return pi;
        }

        public new void BindPageCountAndId()
        {
            base.BindPageCountAndId();
            string text = (this.typeid == -1) ? "" : ("&typeid=" + this.typeid);
            string text2 = (this.stand == 0) ? "" : ("&stand=" + this.stand);
            string text3 = (string.IsNullOrEmpty(this.onlyauthor) || this.onlyauthor == "0") ? "" : "onlyauthor=" + this.onlyauthor + "&posterid=" + this.posterid;
            if (Utils.StrIsNullOrEmpty(this.onlyauthor) || this.onlyauthor == "0")
            {
                if (this.config.Aspxrewrite == 1 && this.typeid <= -1 && this.stand == 0)
                {
                    this.pagenumbers = Utils.GetStaticPageNumbers(this.pageid, this.pagecount, "showtopic-" + this.topicid, this.config.Extname, 8);
                }
                else
                {
                    this.pagenumbers = Utils.GetPageNumbers(this.pageid, this.pagecount, string.Format("showtopic.aspx?topicid={0}{1}{2}", this.topicid, text, text2), 8);
                }
            }
            else
            {
                this.pagenumbers = Utils.GetPageNumbers(this.pageid, this.pagecount, string.Format("showtopic.aspx?topicid={0}&{1}{2}", this.topicid, text3, text), 8);
            }
            if (this.pageid != this.pagecount)
            {
                if (this.stand != 0)
                {
                    this.nextpage = string.Format("<a href=\"showtopic.aspx?topicid={0}{1}{2}&page={3}\" class=\"next\">下一页</a>", new object[]
                    {
                        this.topicid,
                        text,
                        text2,
                        this.pageid + 1
                    });
                }
                else
                {
                    string text4 = Urls.ShowTopicAspxRewrite(this.topicid, this.pageid + 1, this.typeid);
                    string text5 = (String.IsNullOrEmpty(text3)) ? "" : ((text4.IndexOf("?") == -1) ? "?" : "&");
                    this.nextpage = "<a href=\"" + text4 + text5 + text3 + "\" class=\"next\">下一页</a>";
                }
            }
            this.showvisitedforumsmenu = (this.visitedforums != null && ((this.visitedforums.Length == 1 && this.visitedforums[0].Fid != this.forumid) || this.visitedforums.Length > 1));
            if (this.typeid < 0)
            {
                this.listlink = "<a id=\"visitedforums\" href=\"" + Urls.ShowForumAspxRewrite(this.forumid, this.forumpid, this.forum.Rewritename) + "\"";
            }
            else
            {
                this.listlink = "<a id='visitedforums' href=showforum.aspx?forumid=" + this.forumid + "&page=" + this.forumpid + "&typeid=" + this.typeid;
            }
            if (this.showvisitedforumsmenu)
            {
                this.listlink += " onmouseover=\"$('visitedforums').id = 'visitedforumstmp';this.id = 'visitedforums';showMenu({'ctrlid':this.id, 'pos':'34'})\";";
            }
            this.listlink += ">返回列表</a>";
            if (String.IsNullOrEmpty(this.onlyauthor) || this.onlyauthor == "0")
            {
                ForumUtils.WriteCookie("referer", string.Format("showtopic.aspx?topicid={0}&page={1}", this.topicid, this.pageid));
                return;
            }
            ForumUtils.WriteCookie("referer", "showtopic.aspx?&topicid=" + this.topicid + "&" + text3 + "&page=" + this.pageid);
        }
    }
}