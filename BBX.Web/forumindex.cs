using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using BBX.Common;
using BBX.Config;
using BBX.Entity;
using BBX.Forum;
using NewLife;

namespace BBX.Web
{
    public class forumindex : PageBase
    {
        public List<IXForum> forumlist = new List<IXForum>();
        public List<Online> onlineuserlist = new List<Online>();
        //public List<PrivateMessageInfo> pmlist;
        public string lastvisit = "未知";
        public DataTable forumlinklist = ForumLink.FindAllWithCache().ToDataTable();
        public List<Announcement> announcementlist = new List<Announcement>();
        public string[] pagewordad = new string[0];
        public string doublead;
        public string floatad;
        public string mediaad;
        public string inforumad;
        public List<string> pagead = new List<string>();
        public int announcementcount;
        public string onlineiconlist = "";
        public IUser userinfo;
        public int totaltopic;
        public int totalpost;
        public int totalusers;
        public int todayposts;
        public int yesterdayposts;
        public int highestposts;
        public string highestpostsdate = "";
        public int forumlinkcount;
        public string lastusername = "";
        public int lastuserid;
        public int totalonline;
        public int totalonlineuser;
        public int totalonlineguest;
        public int totalonlineinvisibleuser;
        public string highestonlineusercount = "";
        public string highestonlineusertime = "";
        public bool showforumonline;
        public bool isactivespace;
        public bool isallowapply;
        public string[] score = Scoresets.GetValidScoreName();
        public string navhomemenu = "";
        public bool showpmhint;
        public Tag[] taglist;
        public int maxsubcount = GeneralConfigInfo.Current.Maxindexsubforumcount;
        public string templatelistboxoptionsforforumindex = Caches.GetTemplateListBoxOptionsCache(true);
        public ForumHotConfigInfo forumhotconfiginfo = ForumHotConfigInfo.Current;
        public Boolean disablepostctrl;

        protected override void ShowPage()
        {
            this.pagetitle = "首页";
            if (this.userid > 0 && this.useradminid > 0)
            {
                var adminGroupInfo = AdminGroup.FindByID(this.usergroupid);
                if (adminGroupInfo != null)
                {
                    this.disablepostctrl = adminGroupInfo.DisablePostctrl;
                }
            }
            int num = DNTRequest.GetInt("f", 1);
            if (num == 0)
            {
                ForumUtils.WriteCookie("isframe", "1");
            }
            else
            {
                num = ForumUtils.GetCookie("isframe").ToInt(-1);
                if (num == -1) num = this.config.Isframeshow;
            }
            if (num == 2)
            {
                HttpContext.Current.Response.Redirect(BaseConfigs.GetForumPath + "frame.aspx");
                HttpContext.Current.Response.End();
                return;
            }
            if (this.config.Rssstatus == 1)
            {
                base.AddLinkRss("tools/rss.aspx", "最新主题");
            }
            Online.UpdateAction(this.olid, UserAction.IndexShow, 0, this.config.Onlinetimeout);
            if (this.userid != -1)
            {
                this.userinfo = BBX.Entity.User.FindByID(this.userid);
                if (this.userinfo == null)
                {
                    this.userid = -1;
                    ForumUtils.ClearUserCookie();
                }
                else
                {
                    //this.newpmcount = !userinfo.Newpm ? 0 : this.newpmcount;
                    if (!userinfo.Newpm) newpmcount = 0;
                    this.lastvisit = this.userinfo.LastVisit.ToString();
                    this.showpmhint = (this.userinfo.NewsLetter.ToInt() > 4);
                }
            }
            this.navhomemenu = Caches.GetForumListMenuDivCache(this.usergroupid, this.userid, this.config.Extname);
            this.forumlist = Forums.GetForumIndexCollection(this.config.Hideprivate, this.usergroupid, this.config.Moddisplay, out this.totaltopic, out this.totalpost, out this.todayposts);
            this.forumlinkcount = this.forumlinklist.Rows.Count;
            //if (this.config.Enablespace == 1)
            //{
            //    this.GetSpacePerm();
            //}
            this.totalusers = Statistic.Current.TotalUsers;
            this.lastusername = Statistic.Current.LastUserName + "";
            this.lastuserid = Statistic.Current.LastUserID;
            this.yesterdayposts = Statistic.Current.YesterdayPosts;
            this.highestposts = Statistic.Current.HighestPosts;
            this.highestpostsdate = Statistic.Current.HighestPostsDate + "";
            if (this.todayposts > this.highestposts)
            {
                this.highestposts = this.todayposts;
                this.highestpostsdate = DateTime.Now.ToString("yyyy-M-d");
            }
            this.totalonline = this.onlineusercount;
            this.showforumonline = false;
            this.onlineiconlist = OnlineList.GetOnlineGroupIconList();
            if (this.totalonline < this.config.Maxonlinelist || DNTRequest.GetString("showonline") == "yes")
            {
                this.showforumonline = true;
                var list = Online.GetList(0, Online._.UserID, true);
                // 根据活跃时间降序
                list.Sort(Online._.LastActivity, true);
                this.onlineuserlist = list;

                var st = Online.GetStat();
                this.totalonline = st.Total;
                this.totalonlineuser = st.User;
                this.totalonlineinvisibleuser = st.Invisible;
                this.totalonlineguest = st.Guest;
            }
            if (DNTRequest.GetString("showonline") == "no")
            {
                this.showforumonline = false;
            }
            this.highestonlineusercount = Statistic.Current.HighestOnlineUserCount + "";
            this.highestonlineusertime = Statistic.Current.HighestOnlineUserTime.ToString("yyyy-MM-dd HH:mm");
            //this.announcementlist = Announcements.GetSimplifiedAnnouncementList(this.nowdatetime, "2999-01-01 00:00:00");
            announcementlist = Announcement.GetAvailableList();
            this.announcementcount = this.announcementlist != null ? this.announcementlist.Count : 0;
            var fs = new List<IXForum>();
            foreach (var current in this.forumlist)
            {
                current.Description = UBB.ParseSimpleUBB(current.Description);
                if (current.Layer == 0)
                {
                    fs.Add(current);
                }
            }
            this.taglist = config.Enabletag ? Tag.GetHotForumTags(config.Hottagcount).ToArray() : new Tag[0];
            this.headerad = Advertisement.GetOneHeaderAd("indexad", 0);
            this.footerad = Advertisement.GetOneFooterAd("indexad", 0);
            this.inforumad = Advertisement.GetInForumAd("indexad", 0, fs, this.templatepath);
            this.pagewordad = Advertisement.GetPageWordAd("indexad", 0);
            this.doublead = Advertisement.GetDoubleAd("indexad", 0);
            this.floatad = Advertisement.GetFloatAd("indexad", 0);
            this.mediaad = Advertisement.GetMediaAd(this.templatepath, "indexad", 0);
            this.pagead = Advertisement.GetPageAd("indexad", 0);
            if (this.userid > 0 && this.oluserinfo.Newpms < 0)
            {
                Users.UpdateUserNewPMCount(this.userid, this.olid);
            }
        }
    }
}