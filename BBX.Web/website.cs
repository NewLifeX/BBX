using System;
using System.Collections.Generic;
using System.Data;
using BBX.Aggregation;
using BBX.Entity;
using BBX.Forum;
using NewLife;
using XCode;

namespace BBX.Web
{
    public class website : PageBase
    {
        public Post[] postlist = AggregationFacade.ForumAggregation.GetPostListFromFile("Website");
        public TopicOrderType topicordertype = AggregationFacade.ForumAggregation.GetForumAggregationTopicListOrder();
        public EntityList<Topic> topiclist;
        public DataTable userlist;
        //public SpaceConfigInfoExt[] spaceconfigs = AggregationFacade.SpaceAggregation.GetSpaceListFromFile("Website");
        //public AlbumInfo[] albuminfos;
        //public SpaceShortPostInfo[] spacepostlist = AggregationFacade.SpaceAggregation.GetSpacePostList("Website");
        public DataTable recentupdatespaceList;
        public int forumlinkcount;
        public int announcementcount;
        public List<Announcement> announcementlist;
        public string rotatepicdata = AggregationFacade.BaseAggregation.GetRotatePicData();
        //public PhotoAggregationInfo photoconfig = AggregationFacade.PhotoAggregation.GetPhotoAggregationInfo();
        //public List<AlbumInfo> recommendalbumlist = AggregationFacade.AlbumAggregation.GetRecommandAlbumList("Website");
        //public List<AlbumCategoryInfo> albumcategorylist;
		//public List<PhotoInfo> photolist;
        //public List<AlbumInfo> albumlist;
        public ForumAggregationData forumagg = AggregationFacade.ForumAggregation;
        //public AlbumAggregationData albumagg = AggregationFacade.AlbumAggregation;
        //public SpaceAggregationData spaceagg = AggregationFacade.SpaceAggregation;
        //public GoodsAggregationData goodsagg = AggregationFacade.GoodsAggregation;
        //public string spacerotatepicdata = AggregationFacade.SpaceAggregation.GetRotatePicData();
        //public List<PhotoInfo> recommendphotolist = AggregationFacade.AlbumAggregation.GetRecommandPhotoList("Albumindex");
        public int totaltopic;
        public int totalpost;
        public int totalusers;
        public int todayposts;
        public int yesterdayposts;
        public int highestposts;
        public string highestpostsdate;
        public string lastusername;
        public int lastuserid;
        public int totalonline;
        public int totalonlineuser;
        public int totalonlineguest;
        public int totalonlineinvisibleuser;
        public string highestonlineusercount;
        public string highestonlineusertime;
        public DataTable topspacecomments;
        public Tag[] taglist;
		//public GoodsinfoCollection goodscoll = new GoodsinfoCollection();
        public DataTable forumlinklist = ForumLink.FindAllForShow();// = ForumLink.FindAllWithCache().ToDataTable();
        public string doublead;
        public string floatad;
        public List<Post> userPostCountInfoList = new List<Post>();
        public string score1;
        public string score2;
        public string score3;
        public string score4;
        public string score5;
        public string score6;
        public string score7;
        public string score8;
        public string[] score;

        protected override void ShowPage()
        {
            this.pagetitle = "首页";
            if (this.config.Rssstatus == 1)
            {
                base.AddLinkRss("tools/rss.aspx", "最新主题");
            }
            //this.announcementlist = Announcements.GetSimplifiedAnnouncementList(this.nowdatetime, "2999-01-01 00:00:00");
            announcementlist = Announcement.GetAvailableList();
            if (this.announcementlist != null)
            {
                this.announcementcount = this.announcementlist.Count;
            }
            this.forumlinkcount = this.forumlinklist.Rows.Count;
            Forums.GetForumIndexCollection(this.config.Hideprivate, this.usergroupid, this.config.Moddisplay, out this.totaltopic, out this.totalpost, out this.todayposts);
            this.totalusers = Statistic.Current.TotalUsers;
            this.lastusername = Statistic.Current.LastUserName;
            this.lastuserid = Statistic.Current.LastUserID;
            this.yesterdayposts = Statistic.Current.YesterdayPosts;
            this.highestposts = Statistic.Current.HighestPosts;
            this.highestpostsdate = Statistic.Current.HighestPostsDate;
            if (this.todayposts > this.highestposts)
            {
                this.highestposts = this.todayposts;
                this.highestpostsdate = DateTime.Now.ToString("yyyy-M-d");
            }
            this.totalonline = this.onlineusercount;
            //Online.GetOnlineUserCollection(out this.totalonline, out this.totalonlineguest, out this.totalonlineuser, out this.totalonlineinvisibleuser);
            var st = Online.GetStat();
            this.totalonline = st.Total;
            this.totalonlineuser = st.User;
            this.totalonlineinvisibleuser = st.Invisible;
            this.totalonlineguest = st.Guest;

            this.highestonlineusercount = Statistic.Current.HighestOnlineUserCount + "";
            this.highestonlineusertime = Statistic.Current.HighestOnlineUserTime.ToFullString();
            if (this.userid != -1)
            {
                this.score = Scoresets.GetValidScoreName();
                IUser shortUserInfo = BBX.Entity.User.FindByID(this.userid);
                this.score1 = ((decimal)shortUserInfo.ExtCredits1).ToString();
                this.score2 = ((decimal)shortUserInfo.ExtCredits2).ToString();
                this.score3 = ((decimal)shortUserInfo.ExtCredits3).ToString();
                this.score4 = ((decimal)shortUserInfo.ExtCredits4).ToString();
                this.score5 = ((decimal)shortUserInfo.ExtCredits5).ToString();
                this.score6 = ((decimal)shortUserInfo.ExtCredits6).ToString();
                this.score7 = ((decimal)shortUserInfo.ExtCredits7).ToString();
                this.score8 = ((decimal)shortUserInfo.ExtCredits8).ToString();
            }
            //if (this.config.Enablealbum == 1 && AlbumPluginProvider.GetInstance() != null)
            //{
            //    this.albumcategorylist = AlbumPluginProvider.GetInstance().GetAlbumCategory();
            //}
            //if (this.config.Enablespace == 1 && AggregationFacade.SpaceAggregation.GetSpaceTopComments() != null)
            //{
            //    this.topspacecomments = AggregationFacade.SpaceAggregation.GetSpaceTopComments();
            //}
            this.taglist = config.Enabletag ? Tag.GetHotForumTags(this.config.Hottagcount).ToArray() : new Tag[0];
            this.doublead = Advertisement.GetDoubleAd("indexad", 0);
            this.floatad = Advertisement.GetFloatAd("indexad", 0);
        }
    }
}