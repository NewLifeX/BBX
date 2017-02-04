using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using BBX.Common;
using BBX.Entity;
using BBX.Forum;
using System.Linq;

namespace BBX.Web
{
    public class showforum : PageBase
    {
        public List<Online> onlineuserlist;
        public List<Topic> topiclist = new List<Topic>();
        public List<Topic> toptopiclist = new List<Topic>();
        public List<IXForum> subforumlist = new List<IXForum>();
        //public List<PrivateMessageInfo> pmlist;
        public string onlineiconlist = OnlineList.GetOnlineGroupIconList();
        //public DataTable announcementlist = Announcements.GetSimplifiedAnnouncementList(Utils.GetDateTime(), "2999-01-01 00:00:00");
        public List<Announcement> announcementlist = Announcement.GetAvailableList();
        public string[] pagewordad = new string[0];
        public List<string> pagead = new List<string>();
        public string doublead;
        public string floatad;
        public string mediaad;
        public string quickeditorad = "";
        public string[] quickbgad;
        public IXForum forum;
        public UserExtcreditsInfo topicextcreditsinfo = new UserExtcreditsInfo();
        public UserExtcreditsInfo bonusextcreditsinfo = new UserExtcreditsInfo();
        public int forumtotalonline;
        public int forumtotalonlineuser;
        public int forumtotalonlineguest;
        public int forumtotalonlineinvisibleuser;
        public int forumid = DNTRequest.GetInt("forumid", -1);
        public string forumnav = "";
        public int showforumlogin;
        public int pageid = DNTRequest.GetInt("page", 1);
        public int forumpageid = DNTRequest.GetInt("page", 1);
        public int topiccount;
        public int pagecount = 1;
        public string pagenumbers = "";
        //public int toptopiccount;
        public string forumlistboxoptions;
        public bool showforumonline;
        public Boolean disablepostctrl;
        public int parseurloff;
        public int smileyoff;
        public int bbcodeoff;
        public int usesig = (ForumUtils.GetCookie("sigstatus") == "0") ? 0 : 1;
        /// <summary>每页主题数</summary>
        public int tpp = ForumUtils.GetCookie("tpp").ToInt();
        /// <summary>每页帖子数</summary>
        public int ppp = ForumUtils.GetCookie("ppp").ToInt();
        public bool ismoder;
        public string topictypeselectoptions;
        public int topictypeid = DNTRequest.GetInt("typeid", -1);
        public string filter = DNTRequest.GetHtmlEncodeString("filter");
        public bool canposttopic;
        public bool canquickpost;
        public bool needlogin;
        public int order = DNTRequest.GetInt("order", 1);
        public int interval = DNTRequest.GetInt("interval", 0);
        public int direct = DNTRequest.GetInt("direct", 1);
        //public string goodscategoryfid = (GeneralConfigInfo.Current.Enablemall <= 0) ? "{}" : MallPluginProvider.GetInstance().GetGoodsCategoryWithFid();
        public string topictypeselectlink;
        public string nextpage = "";
        public string navhomemenu = "";
        public IXForum[] visitedforums = Forums.GetVisitedForums();
        public bool showvisitedforumsmenu;
        public bool isnewbie;
        private string msg = "";
        //private string condition = "";
        //private string orderStr = "";
        public int topicid;
        public bool needaudit;

        protected override void ShowPage()
        {
            this.GetPostAds(this.forumid);
            if (this.userid > 0 && this.useradminid > 0)
            {
                var adminGroupInfo = AdminGroup.FindByID(this.usergroupid);
                if (adminGroupInfo != null)
                {
                    this.disablepostctrl = adminGroupInfo.DisablePostctrl;
                }
            }
            if (this.forumid == -1)
            {
                base.AddLinkRss(this.forumpath + "tools/rss.aspx", "最新主题");
                base.AddErrLine("无效的版块ID");
                return;
            }
            this.forum = Forums.GetForumInfo(this.forumid);
            if (this.forum == null || this.forum.Fid < 1)
            {
                if (this.config.Rssstatus == 1)
                {
                    base.AddLinkRss(this.forumpath + "tools/rss.aspx", Utils.EncodeHtml(this.config.Forumtitle) + " 最新主题");
                }
                base.AddErrLine("不存在的版块ID");
                return;
            }
            if (this.config.Rssstatus == 1)
            {
                base.AddLinkRss(this.forumpath + "tools/" + Urls.RssAspxRewrite(this.forum.Fid), Utils.EncodeHtml(this.forum.Name) + " 最新主题");
            }
            if (this.JumpUrl(this.forum))
            {
                return;
            }
            this.needaudit = UserAuthority.NeedAudit(forum.Fid, forum.Modnewposts, this.useradminid, this.userid, this.usergroupinfo);
            if (this.useradminid > 0)
            {
                this.ismoder = Moderators.IsModer(this.useradminid, this.userid, this.forumid);
            }
            //this.SetSearchCondition();
            this.showforumlogin = this.IsShowForumLogin(this.forum);
            this.pagetitle = Utils.RemoveHtml(this.forum.Name);
            this.navhomemenu = Caches.GetForumListMenuDivCache(this.usergroupid, this.userid, this.config.Extname);
            this.forumnav = base.ShowForumAspxRewrite(ForumUtils.UpdatePathListExtname(this.forum.Pathlist.Trim(), this.config.Extname).Replace("\"showforum", "\"" + this.forumurl + "showforum"), this.forumid, this.pageid);
            this.topicextcreditsinfo = Scoresets.GetScoreSet(Scoresets.GetTopicAttachCreditsTrans());
            this.bonusextcreditsinfo = Scoresets.GetScoreSet(Scoresets.GetBonusCreditsTrans());
            if (this.forum.ApplytopicType == 1)
            {
                this.topictypeselectoptions = Forums.GetCurrentTopicTypesOption(this.forum.Fid, this.forum.Topictypes);
            }
            if (this.forum.ApplytopicType == 1)
            {
                this.topictypeselectlink = Forums.GetCurrentTopicTypesLink(this.forum.Fid, this.forum.Topictypes, this.forumurl + "showforum.aspx");
            }
            meta = PageHelper.UpdateMetaInfo(meta, this.forum.Seokeywords.IsNullOrEmpty() ? this.config.Seokeywords : this.forum.Seokeywords, this.forum.Seodescription.IsNullOrEmpty() ? this.forum.Description : this.forum.Seodescription, this.config.Seohead);
            this.SetEditorState();
            if (!UserAuthority.VisitAuthority(this.forum, this.usergroupinfo, this.userid, ref this.msg))
            {
                base.AddErrLine(this.msg);
                this.needlogin = (this.userid == -1);
                return;
            }
            this.canposttopic = UserAuthority.PostAuthority(this.forum, this.usergroupinfo, this.userid, ref this.msg);
            if (this.useradminid != 1 && !this.usergroupinfo.DisablePeriodctrl)
            {
                string text = "";
                if (this.canposttopic && Scoresets.BetweenTime(this.config.Postbanperiods, out text))
                {
                    this.canposttopic = false;
                }
                this.isnewbie = UserAuthority.CheckNewbieSpan(this.userid);
            }
            if ((this.config.Fastpost == 1 || this.config.Fastpost == 3) && !this.forum.AllowSpecialOnly && (this.userid < 0 || (this.canposttopic && !this.isnewbie)))
            {
                this.canquickpost = true;
            }
            if ((forum as XForum).Childs.Count > 0)
            {
                this.subforumlist = Forums.GetSubForumCollection(this.forumid, this.forum.ColCount, this.config.Hideprivate, this.usergroupid, this.config.Moddisplay);
            }
            if (!this.forum.Rules.IsNullOrWhiteSpace())
            {
                this.forum.Rules = UBB.ParseSimpleUBB(this.forum.Rules);
            }
            // 满足条件的主题数
            this.topiccount = Topic.SearchCount(forumid, topictypeid, interval, filter);
            if (tpp <= 0) tpp = this.config.Tpp;
            if (this.ppp <= 0) this.ppp = this.config.Ppp;
            if (this.pageid < 1) this.pageid = 1;

            // 内层板块，需要显示主题列表
            if (this.forum.Layer > 0)
            {
                // 置顶主题列表
                var tops = Topic.GetTop(forumid);
                if (tops.Count > 0)
                {
                    // 加上非本板块的主题数
                    topiccount += tops.Count(e => e.Fid != forumid);
                }

                // 根据主题总数计算分页
                //pagecount = ((topiccount % tpp == 0) ? (topiccount / tpp) : (topiccount / tpp + 1));
                pagecount = topiccount / tpp;
                if (topiccount % tpp > 0) pagecount++;
                if (pagecount == 0) pagecount = 1;
                if (pageid > pagecount) pageid = pagecount;

                // 如果有置顶主题，则先读取置顶主题，再读取部分普通主题以填满一页
                if (tops.Count > 0)
                {
                    // 置顶主题数
                    var toptopiccount = tops.Count;
                    // 置顶主题构成的完整页数，不包括半截
                    int topPageCount = toptopiccount / tpp;
                    // 置顶主题可能满好几页，如果分到当前页还有主题，则需要处理
                    if (toptopiccount > tpp * (pageid - 1))
                    {
                        // 取出本页所属的置顶主题
                        toptopiclist = Topics.GetTopTopicList(tops, tpp, pageid, forum.AutoClose, forum.Topictypeprefix);

                        if (toptopiclist.Count < tpp)
                            // 本页有一部分置顶主题，要注意计算本页普通主题的数量（总页大小-置顶帖在本页的大小），开始行很简单从0开始即可（因为是第一页普通主题）
                            topiclist = GetTopicInfoList(tpp - toptopiclist.Count, pageid - topPageCount, 0);
                    }
                    else
                        // 已经没有置顶主题，取满一页普通主题，注意开始行的计算
                        topiclist = GetTopicInfoList(tpp, pageid - topPageCount, toptopiccount % tpp);
                }
                else
                {
                    // 如果没有置顶主题，则直接读取一页普通主题
                    topiclist = GetTopicInfoList(tpp, pageid, 0);
                }
                // 如果没有主题，或者实际主题列表大于主题总数，那么更新论坛的主题数
                // 在XCode支持下，即使有缓存，这种可能性也很小很小
                if (topiclist == null || topiclist.Count == 0 || topiclist.Count > topiccount)
                {
                    XForum.SetRealCurrentTopics(this.forum.Fid);
                }
                this.SetPageNumber();

                var vs = visitedforums;
                this.showvisitedforumsmenu = (vs != null && ((vs.Length == 1 && vs[0].Fid != forumid) || vs.Length > 1));
                this.SetVisitedForumsCookie();
                Utils.WriteCookie("forumpageid", this.pageid.ToString(), 30);
                this.IsGuestCachePage();
            }
            this.forum.Description = UBB.ParseSimpleUBB(this.forum.Description);
            Online.UpdateAction(this.olid, UserAction.ShowForum, this.forumid, this.forum.Name, -1, "");
            if ((this.forumtotalonline < this.config.Maxonlinelist && (this.config.Whosonlinestatus == 2 || this.config.Whosonlinestatus == 3)) || DNTRequest.GetString("showonline") == "yes")
            {
                this.showforumonline = true;
                this.onlineuserlist = Online.GetList(this.forumid, Online._.UserID, true);

                var st = Online.GetStat();
                this.forumtotalonline = st.Total;
                this.forumtotalonlineuser = st.User;
                this.forumtotalonlineinvisibleuser = st.Invisible;
                this.forumtotalonlineguest = st.Guest;
            }
            if (DNTRequest.GetString("showonline") == "no")
            {
                this.showforumonline = false;
            }
            // 这里会导致版主列表不断增大，直到最后内存溢出，临时解决
            if (!forum.Moderators.IsNullOrWhiteSpace() && (forum.ModeratorsHtml.IsNullOrWhiteSpace() || !forum.ModeratorsHtml.Contains("href")))
            {
                string text2 = string.Empty;
                string[] array = this.forum.Moderators.Split(',');
                for (int i = 0; i < array.Length; i++)
                {
                    text2 += string.Format("<a href=\"{0}userinfo.aspx?username={1}\">{2}</a>,", this.forumpath, Utils.UrlEncode(array[i]), array[i]);
                }
                this.forum.ModeratorsHtml = text2.TrimEnd(',');
            }
            ForumUtils.UpdateVisitedForumsOptions(this.forumid);
        }

        private bool JumpUrl(IXForum forumInfo)
        {
            if (!forumInfo.Redirect.IsNullOrEmpty())
            {
                HttpContext.Current.Response.Redirect(forumInfo.Redirect);
                return true;
            }
            //if (this.config.Enablemall == 1 && forumInfo.IsTrade)
            //{
            //	MallPluginBase instance = MallPluginProvider.GetInstance();
            //	int goodsCategoryIdByFid = instance.GetGoodsCategoryIdByFid(forumInfo.ID);
            //	if (goodsCategoryIdByFid > 0)
            //	{
            //		HttpContext.Current.Response.Redirect(BaseConfigs.GetForumPath + Urls.ShowGoodsListAspxRewrite(goodsCategoryIdByFid, 1));
            //		return true;
            //	}
            //}
            return false;
        }

        public List<Topic> GetTopicInfoList(int pageSize, int pageIndex, int startNumber)
        {
            return Topics.GetTopicList(forumid, topictypeid, interval, filter, order, direct != 0, pageSize, pageIndex, startNumber, 600, config.Hottopic, forum.AutoClose, forum.Topictypeprefix);
        }

        private void SetPageNumber()
        {
            if (String.IsNullOrEmpty(DNTRequest.GetString("search")))
            {
                if (this.topictypeid == -1)
                {
                    if (this.config.Aspxrewrite == 1)
                    {
                        if (this.filter.IsNullOrEmpty())
                        {
                            if (this.config.Iisurlrewrite == 0)
                            {
                                this.pagenumbers = Utils.GetStaticPageNumbers(this.pageid, this.pagecount, this.forum.Rewritename.IsNullOrEmpty() ? ("showforum-" + this.forumid) : (this.forumpath + this.forum.Rewritename), this.config.Extname, 8, (!this.forum.Rewritename.IsNullOrEmpty()) ? 1 : 0);
                            }
                            else
                            {
                                this.pagenumbers = Utils.GetStaticPageNumbers(this.pageid, this.pagecount, this.forum.Rewritename.IsNullOrEmpty() ? ("showforum-" + this.forumid) : (this.forumpath + this.forum.Rewritename), this.config.Extname, 8, (!this.forum.Rewritename.IsNullOrEmpty()) ? 2 : 0);
                            }
                            if (this.pageid < this.pagecount)
                            {
                                this.nextpage = string.Format("<a href=\"{0}{1}\" class=\"next\">下一页</a>", this.forumpath, Urls.ShowForumAspxRewrite(this.forumid, this.pageid + 1, this.forum.Rewritename));
                                return;
                            }
                        }
                        else
                        {
                            this.pagenumbers = Utils.GetPageNumbers(this.pageid, this.pagecount, string.Format("{0}showforum.aspx?forumid={1}&filter={2}", this.forumpath, this.forumid, this.filter), 8);
                            if (this.pageid < this.pagecount)
                            {
                                this.nextpage = string.Format("<a href=\"{0}showforum.aspx?forumid={1}&filter={2}&page={3}\" class=\"next\">下一页</a>", new object[]
                                {
                                    this.forumpath,
                                    this.forumid,
                                    this.filter,
                                    this.pageid + 1
                                });
                                return;
                            }
                        }
                    }
                    else
                    {
                        this.pagenumbers = Utils.GetPageNumbers(this.pageid, this.pagecount, string.Format("{0}showforum.aspx?forumid={1}{2}", this.forumpath, this.forumid, this.filter.IsNullOrEmpty() ? "" : ("&filter=" + this.filter)), 8);
                        if (this.pageid < this.pagecount)
                        {
                            this.nextpage = string.Format("<a href=\"{0}showforum.aspx?forumid={1}{2}&page={3}\" class=\"next\">下一页</a>", new object[]
                            {
                                this.forumpath,
                                this.forumid,
                                this.filter.IsNullOrEmpty() ? "" : ("&filter=" + this.filter),
                                this.pageid + 1
                            });
                            return;
                        }
                    }
                }
                else
                {
                    this.pagenumbers = Utils.GetPageNumbers(this.pageid, this.pagecount, string.Format("{0}showforum.aspx?forumid={1}&typeid={2}{3}", new object[]
                    {
                        this.forumpath,
                        this.forumid,
                        this.topictypeid,
                        this.filter.IsNullOrEmpty() ? "" : ("&filter=" + this.filter)
                    }), 8);
                    if (this.pageid < this.pagecount)
                    {
                        this.nextpage = string.Format("<a href=\"{0}showforum.aspx?forumid={1}&typeid={2}{3}&page={4}\" class=\"next\">下一页</a>", new object[]
                        {
                            this.forumpath,
                            this.forumid,
                            this.topictypeid,
                            this.filter.IsNullOrEmpty() ? "" : ("&filter=" + this.filter),
                            this.pageid + 1
                        });
                        return;
                    }
                }
            }
            else
            {
                this.pagenumbers = Utils.GetPageNumbers(this.pageid, this.pagecount, string.Format("{0}showforum.aspx?search=1&cond={1}&order={2}&direct={3}&forumid={4}&interval={5}&typeid={6}{7}", new object[]
                {
                    this.forumpath,
                    DNTRequest.GetHtmlEncodeString("cond").Trim(),
                    this.order,
                    this.direct,
                    this.forumid,
                    this.interval,
                    this.topictypeid,
                    this.filter.IsNullOrEmpty() ? "" : ("&filter=" + this.filter)
                }), 8);
                if (this.pageid < this.pagecount)
                {
                    this.nextpage = string.Format("<a href=\"{0}showforum.aspx?search=1&cond={1}&order={2}&direct={3}&forumid={4}&interval={5}&typeid={6}{7}&page={8}\" class=\"next\">下一页</a>", new object[]
                    {
                        this.forumpath,
                        DNTRequest.GetHtmlEncodeString("cond").Trim(),
                        this.order,
                        this.direct,
                        this.forumid,
                        this.interval,
                        this.topictypeid,
                        this.filter.IsNullOrEmpty() ? "" : ("&filter=" + this.filter),
                        this.pageid + 1
                    });
                }
            }
        }

        public void GetPostAds(int forumid)
        {
            this.headerad = Advertisement.GetOneHeaderAd("", forumid);
            this.footerad = Advertisement.GetOneFooterAd("", forumid);
            this.pagewordad = Advertisement.GetPageWordAd("", forumid);
            this.pagead = Advertisement.GetPageAd("", forumid);
            this.doublead = Advertisement.GetDoubleAd("", forumid);
            this.floatad = Advertisement.GetFloatAd("", forumid);
            this.mediaad = Advertisement.GetMediaAd(this.templatepath, "", forumid);
            this.quickeditorad = Advertisement.GetQuickEditorAD("", forumid);
            this.quickbgad = Advertisement.GetQuickEditorBgAd("", forumid);
            if (this.quickbgad.Length <= 1)
            {
                this.quickbgad = new string[]
                {
                    "",
                    ""
                };
            }
        }

        private void SetEditorState()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("var Allowhtml=1;\r\n");
            this.smileyoff = forum.AllowSmilies ? 0 : 1;
            stringBuilder.Append("var Allowsmilies=" + (1 - this.smileyoff) + ";\r\n");
            this.bbcodeoff = ((this.forum.Allowbbcode == 1 && this.usergroupinfo.AllowCusbbCode) ? 0 : 1);
            stringBuilder.Append("var Allowbbcode=" + (1 - this.bbcodeoff) + ";\r\n");
            stringBuilder.Append("var Allowimgcode=" + this.forum.Allowimgcode + ";\r\n");
            base.AddScript(stringBuilder.ToString());
        }

        private int IsShowForumLogin(IXForum forum)
        {
            int result = 1;
            if (forum.Password.IsNullOrEmpty())
            {
                result = 0;
            }
            else
            {
                if (Utils.MD5(forum.Password) == ForumUtils.GetCookie("forum" + this.forumid + "password"))
                {
                    result = 0;
                }
                else
                {
                    if (forum.Password == DNTRequest.GetString("forumpassword"))
                    {
                        ForumUtils.WriteCookie("forum" + forum.Fid + "password", Utils.MD5(forum.Password));
                        result = 0;
                    }
                }
            }
            return result;
        }

        private void SetVisitedForumsCookie()
        {
            if (this.forum.Layer > 0)
            {
                ForumUtils.SetVisitedForumsCookie(this.forum.Fid.ToString());
            }
        }

        public void IsGuestCachePage()
        {
            if (this.userid == -1 && this.pageid > 0 && this.pageid < this.pagecount && ForumUtils.IsGuestCachePage(this.pageid, "showforum"))
            {
                this.isguestcachepage = 1;
            }
        }
    }
}