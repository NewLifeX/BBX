using System;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;
using BBX.Common;
using NewLife.Xml;

namespace BBX.Config
{
    [XmlConfigFile("config/general.config", 15000)]
    /// <summary>通用设置</summary>
    [Description("通用设置")]
    [Serializable]
    public class GeneralConfigInfo : XmlConfig2<GeneralConfigInfo>
    {
        private string m_customauthorinfo = "gender,bday,credits,posts,joindate|uid,digestposts";
        public string Customauthorinfo { get { return m_customauthorinfo; } set { m_customauthorinfo = value; } }

        private int m_debatepagesize = 5;
        public int Debatepagesize { get { return m_debatepagesize; } set { m_debatepagesize = value; } }

        /// <summary>版权信息</summary>
        [Description("版权信息")]
        [XmlIgnore]
        public string Forumcopyright
        {
            get
            {
                return "&copy; 2002-" + DateTime.Now.Year + " <a href=\"" + Utils.CompanyUrl + "\" target=\"_blank\">" + Utils.CompanyName + "</a>.";
            }
        }

        private string m_forumtitle = Utils.ProductName;
        /// <summary>论坛标题</summary>
        [Description("论坛标题")]
        public string Forumtitle { get { return m_forumtitle; } set { m_forumtitle = value; } }

        private string m_forumurl = "forumindex.aspx";
        /// <summary>论坛地址</summary>
        [Description("论坛地址")]
        public string Forumurl { get { return m_forumurl; } set { m_forumurl = value; } }

        private string m_webtitle = Utils.ProductName;
        /// <summary>网页标题</summary>
        [Description("网页标题")]
        public string Webtitle { get { return m_webtitle; } set { m_webtitle = value; } }

        private string m_weburl;
        public string Weburl { get { return m_weburl; } set { m_weburl = value; } }

        private int m_licensed = 1;
        /// <summary>授权数</summary>
        [Description("授权数")]
        public int Licensed { get { return m_licensed; } set { m_licensed = value; } }

        private string m_icp;
        /// <summary>ICP备案号</summary>
        [Description("ICP备案号")]
        public string Icp { get { return m_icp; } set { m_icp = value; } }

        private int m_closed;
        /// <summary>是否关闭</summary>
        [Description("是否关闭")]
        public int Closed { get { return m_closed; } set { m_closed = value; } }

        private string m_closedreason;
        /// <summary>关闭原因</summary>
        [Description("关闭原因")]
        public string Closedreason { get { return m_closedreason; } set { m_closedreason = value; } }

        private int m_isframeshow;
        public int Isframeshow { get { return m_isframeshow; } set { m_isframeshow = value; } }

        private int m_admintools;
        public int Admintools { get { return m_admintools; } set { m_admintools = value; } }

        private int m_indexpage;
        public int Indexpage { get { return m_indexpage; } set { m_indexpage = value; } }

        private string m_linktext = "<a href=\"" + Utils.ProductUrl + "\" title=\"The Official " + Utils.ProductName + " Site\" target=\"_blank\">" + Utils.ProductName + "</a>";
        public string Linktext { get { return m_linktext; } set { m_linktext = value; } }

        private string m_statcode;
        public string Statcode { get { return m_statcode; } set { m_statcode = value; } }

        private string m_passwordkey = "1234567890";
        public string Passwordkey { get { return m_passwordkey; } set { m_passwordkey = value; } }

        private int m_regstatus = 1;
        public int Regstatus { get { return m_regstatus; } set { m_regstatus = value; } }

        private int m_regadvance = 1;
        public int Regadvance { get { return m_regadvance; } set { m_regadvance = value; } }

        private int m_realnamesystem;
        public int Realnamesystem { get { return m_realnamesystem; } set { m_realnamesystem = value; } }

        private string m_censoruser = "admin";
        public string Censoruser { get { return m_censoruser; } set { m_censoruser = value; } }

        private int m_doublee;
        public int Doublee { get { return m_doublee; } set { m_doublee = value; } }

        private int m_regverify;
        public int Regverify { get { return m_regverify; } set { m_regverify = value; } }

        private string m_accessemail;
        public string Accessemail { get { return m_accessemail; } set { m_accessemail = value; } }

        private string m_censoremail;
        public string Censoremail { get { return m_censoremail; } set { m_censoremail = value; } }

        private int m_hideprivate = 1;
        public int Hideprivate { get { return m_hideprivate; } set { m_hideprivate = value; } }

        private int m_regctrl;
        public int Regctrl { get { return m_regctrl; } set { m_regctrl = value; } }

        private string m_ipregctrl;
        public string Ipregctrl { get { return m_ipregctrl; } set { m_ipregctrl = value; } }

        private string m_ipdenyaccess;
        public string Ipdenyaccess { get { return m_ipdenyaccess; } set { m_ipdenyaccess = value; } }

        private string m_ipaccess;
        public string Ipaccess { get { return m_ipaccess; } set { m_ipaccess = value; } }

        private string m_adminipaccess;
        public string Adminipaccess { get { return m_adminipaccess; } set { m_adminipaccess = value; } }

        private int m_newbiespan;
        /// <summary>新注册用户发帖时间</summary>
        [Description("新注册用户发帖时间")]
        public int Newbiespan { get { return m_newbiespan; } set { m_newbiespan = value; } }

        private int m_welcomemsg = 1;
        public int Welcomemsg { get { return m_welcomemsg; } set { m_welcomemsg = value; } }

        private string m_welcomemsgtxt = "Welcome to visit this forum!";
        public string Welcomemsgtxt { get { return m_welcomemsgtxt; } set { m_welcomemsgtxt = value; } }

        private int m_rules = 1;
        public int Rules { get { return m_rules; } set { m_rules = value; } }

        private string m_rulestxt;
        public string Rulestxt { get { return m_rulestxt; } set { m_rulestxt = value; } }

        private int m_secques;
        public int Secques { get { return m_secques; } set { m_secques = value; } }

        private int m_templateid = 1;
        public int Templateid { get { return m_templateid; } set { m_templateid = value; } }

        private int m_hottopic = 20;
        public int Hottopic { get { return m_hottopic; } set { m_hottopic = value; } }

        private int m_starthreshold = 2;
        public int Starthreshold { get { return m_starthreshold; } set { m_starthreshold = value; } }

        private int m_visitedforums = 20;
        public int Visitedforums { get { return m_visitedforums; } set { m_visitedforums = value; } }

        private int m_maxsigrows;
        public int Maxsigrows { get { return m_maxsigrows; } set { m_maxsigrows = value; } }

        private int m_moddisplay;
        public int Moddisplay { get { return m_moddisplay; } set { m_moddisplay = value; } }

        private int m_subforumsindex = 1;
        public int Subforumsindex { get { return m_subforumsindex; } set { m_subforumsindex = value; } }

        private int m_stylejump = 1;
        public int Stylejump { get { return m_stylejump; } set { m_stylejump = value; } }

        private int m_fastpost = 1;
        public int Fastpost { get { return m_fastpost; } set { m_fastpost = value; } }

        private int m_showsignatures = 1;
        public int Showsignatures { get { return m_showsignatures; } set { m_showsignatures = value; } }

        private int m_showavatars = 1;
        public int Showavatars { get { return m_showavatars; } set { m_showavatars = value; } }

        private int m_showimages = 1;
        public int Showimages { get { return m_showimages; } set { m_showimages = value; } }

        private int m_smiliesmax = 30;
        public int Smiliesmax { get { return m_smiliesmax; } set { m_smiliesmax = value; } }

        private int m_archiverstatus = 1;
        public int Archiverstatus { get { return m_archiverstatus; } set { m_archiverstatus = value; } }

        private string m_seotitle;
        public string Seotitle { get { return m_seotitle; } set { m_seotitle = value; } }

        private string m_seokeywords;
        public string Seokeywords { get { return m_seokeywords; } set { m_seokeywords = value; } }

        private string m_seodescription;
        public string Seodescription { get { return m_seodescription; } set { m_seodescription = value; } }

        private string m_seohead = "<meta name=\"generator\" content=\"" + Utils.ProductName + "\" />";
        public string Seohead { get { return m_seohead; } set { m_seohead = value; } }

        private int m_rssstatus = 1;
        public int Rssstatus { get { return m_rssstatus; } set { m_rssstatus = value; } }

        private int m_rssttl = 60;
        public int Rssttl { get { return m_rssttl; } set { m_rssttl = value; } }

        private int m_sitemapstatus = 1;
        public int Sitemapstatus { get { return m_sitemapstatus; } set { m_sitemapstatus = value; } }

        private int m_sitemapttl = 12;
        public int Sitemapttl { get { return m_sitemapttl; } set { m_sitemapttl = value; } }

        private int m_nocacheheaders;
        public int Nocacheheaders { get { return m_nocacheheaders; } set { m_nocacheheaders = value; } }

        private int m_fullmytopics = 1;
        public int Fullmytopics { get { return m_fullmytopics; } set { m_fullmytopics = value; } }

        private int m_debug = 1;
        public int Debug { get { return m_debug; } set { m_debug = value; } }

        private string m_rewriteurl;
        public string Rewriteurl { get { return m_rewriteurl; } set { m_rewriteurl = value; } }

        private string m_extname = ".aspx";
        public string Extname { get { return m_extname; } set { m_extname = value; } }

        private int m_whosonlinestatus = 3;
        /// <summary>显示在线用户。0不显示1首页2分论坛3首页和分论坛</summary>
        [Description("显示在线用户。0不显示1首页2分论坛3首页和分论坛")]
        public int Whosonlinestatus { get { return m_whosonlinestatus; } set { m_whosonlinestatus = value; } }

        private int m_maxonlinelist = 300;
        /// <summary>最多显示在线人数</summary>
        [Description("最多显示在线人数，默认300")]
        public int Maxonlinelist { get { return m_maxonlinelist; } set { m_maxonlinelist = value; } }

        private int m_userstatusby = 1;
        public int Userstatusby { get { return m_userstatusby; } set { m_userstatusby = value; } }

        private int m_forumjump = 1;
        public int Forumjump { get { return m_forumjump; } set { m_forumjump = value; } }

        private int m_modworkstatus = 1;
        public int Modworkstatus { get { return m_modworkstatus; } set { m_modworkstatus = value; } }

        private int m_maxmodworksmonths = 3;
        public int Maxmodworksmonths { get { return m_maxmodworksmonths; } set { m_maxmodworksmonths = value; } }

        private string m_seccodestatus = "register.aspx,login.aspx";
        public string Seccodestatus { get { return m_seccodestatus; } set { m_seccodestatus = value; } }

        private int m_guestcachepagetimeout;
        public int Guestcachepagetimeout { get { return m_guestcachepagetimeout; } set { m_guestcachepagetimeout = value; } }

        private int m_topiccachemark;
        public int Topiccachemark { get { return m_topiccachemark; } set { m_topiccachemark = value; } }

        private int m_maxonlines = 5000;
        public int Maxonlines { get { return m_maxonlines; } set { m_maxonlines = value; } }

        private int m_postinterval;
        /// <summary>发帖间隔。秒</summary>
        [Description("发帖间隔。秒")]
        public int Postinterval { get { return m_postinterval; } set { m_postinterval = value; } }

        private int m_searchctrl;
        /// <summary>搜索间隔。秒</summary>
        [Description("搜索间隔。秒")]
        public int Searchctrl { get { return m_searchctrl; } set { m_searchctrl = value; } }

        private int m_maxspm = 5;
        /// <summary>系统在一分钟内搜索的最大次数</summary>
        [Description("系统在一分钟内搜索的最大次数")]
        public int Maxspm { get { return m_maxspm; } set { m_maxspm = value; } }

        private string m_visitbanperiods;
        public string Visitbanperiods { get { return m_visitbanperiods; } set { m_visitbanperiods = value; } }

        private string m_postbanperiods;
        /// <summary>禁止发帖时间段</summary>
        [Description("禁止发帖时间段")]
        public string Postbanperiods { get { return m_postbanperiods; } set { m_postbanperiods = value; } }

        private string m_postmodperiods;
        public string Postmodperiods { get { return m_postmodperiods; } set { m_postmodperiods = value; } }

        private string m_attachbanperiods;
        public string Attachbanperiods { get { return m_attachbanperiods; } set { m_attachbanperiods = value; } }

        private string m_searchbanperiods;
        public string Searchbanperiods { get { return m_searchbanperiods; } set { m_searchbanperiods = value; } }

        private int m_memliststatus = 1;
        public int Memliststatus { get { return m_memliststatus; } set { m_memliststatus = value; } }

        private int m_dupkarmarate;
        public int Dupkarmarate { get { return m_dupkarmarate; } set { m_dupkarmarate = value; } }

        private int m_minpostsize = 1;
        public int Minpostsize { get { return m_minpostsize; } set { m_minpostsize = value; } }

        private int m_maxpostsize = 10000;
        public int Maxpostsize { get { return m_maxpostsize; } set { m_maxpostsize = value; } }

        private int m_tpp = 26;
        public int Tpp { get { return m_tpp; } set { m_tpp = value; } }

        private int m_ppp = 16;
        public int Ppp { get { return m_ppp; } set { m_ppp = value; } }

        private int m_maxfavorites = 30;
        public int Maxfavorites { get { return m_maxfavorites; } set { m_maxfavorites = value; } }

        private int m_maxpolloptions = 10;
        public int Maxpolloptions { get { return m_maxpolloptions; } set { m_maxpolloptions = value; } }

        private int m_maxattachments = 10;
        public int Maxattachments { get { return m_maxattachments; } set { m_maxattachments = value; } }

        private int m_attachimgpost = 1;
        public int Attachimgpost { get { return m_attachimgpost; } set { m_attachimgpost = value; } }

        private int m_attachrefcheck = 1;
        public int Attachrefcheck { get { return m_attachrefcheck; } set { m_attachrefcheck = value; } }

        private int m_attachsave;
        public int Attachsave { get { return m_attachsave; } set { m_attachsave = value; } }

        private int m_watermarkstatus = 3;
        public int Watermarkstatus { get { return m_watermarkstatus; } set { m_watermarkstatus = value; } }

        private int m_watermarktype;
        public int Watermarktype { get { return m_watermarktype; } set { m_watermarktype = value; } }

        private int m_watermarktransparency = 5;
        public int Watermarktransparency { get { return m_watermarktransparency; } set { m_watermarktransparency = value; } }

        private string m_watermarktext = Utils.ProductName;
        public string Watermarktext { get { return m_watermarktext; } set { m_watermarktext = value; } }

        private string m_watermarkpic = "watermark.gif";
        public string Watermarkpic { get { return m_watermarkpic; } set { m_watermarkpic = value; } }

        private string m_watermarkfontname = "Tahoma";
        public string Watermarkfontname { get { return m_watermarkfontname; } set { m_watermarkfontname = value; } }

        private int m_watermarkfontsize = 12;
        public int Watermarkfontsize { get { return m_watermarkfontsize; } set { m_watermarkfontsize = value; } }

        private int m_showattachmentpath;
        public int Showattachmentpath { get { return m_showattachmentpath; } set { m_showattachmentpath = value; } }

        private int m_attachimgquality = 80;
        public int Attachimgquality { get { return m_attachimgquality; } set { m_attachimgquality = value; } }

        private int m_attachimgmaxheight;
        public int Attachimgmaxheight { get { return m_attachimgmaxheight; } set { m_attachimgmaxheight = value; } }

        private int m_attachimgmaxwidth;
        public int Attachimgmaxwidth { get { return m_attachimgmaxwidth; } set { m_attachimgmaxwidth = value; } }

        private int m_reasonpm;
        public int Reasonpm { get { return m_reasonpm; } set { m_reasonpm = value; } }

        private int m_moderactions = 1;
        public int Moderactions { get { return m_moderactions; } set { m_moderactions = value; } }

        private int m_karmaratelimit = 4;
        public int Karmaratelimit { get { return m_karmaratelimit; } set { m_karmaratelimit = value; } }

        private int m_losslessdel = 200;
        public int Losslessdel { get { return m_losslessdel; } set { m_losslessdel = value; } }

        private int m_edittimelimit = 10;
        public int Edittimelimit { get { return m_edittimelimit; } set { m_edittimelimit = value; } }

        private int m_editedby = 1;
        public int Editedby { get { return m_editedby; } set { m_editedby = value; } }

        private int m_defaulteditormode = 1;
        /// <summary>默认编辑器模式，0为论坛代码，1为可视化（默认）</summary>
        [Description("默认编辑器模式，0为论坛代码，1为可视化（默认）")]
        public int Defaulteditormode { get { return m_defaulteditormode; } set { m_defaulteditormode = value; } }

        private int m_allowswitcheditor = 1;
        /// <summary>是否允许切换编辑器模式，1为允许（默认）</summary>
        [Description("是否允许切换编辑器模式，1为允许（默认）")]
        public int Allowswitcheditor { get { return m_allowswitcheditor; } set { m_allowswitcheditor = value; } }

        private int m_smileyinsert = 1;
        public int Smileyinsert { get { return m_smileyinsert; } set { m_smileyinsert = value; } }

        private string m_cookiedomain;
        public string CookieDomain { get { return m_cookiedomain; } set { m_cookiedomain = value; } }

        private int m_passwordmode;
        public int Passwordmode { get { return m_passwordmode; } set { m_passwordmode = value; } }

        private int m_bbcodemode;
        public int Bbcodemode { get { return m_bbcodemode; } set { m_bbcodemode = value; } }

        private int m_fulltextsearch;
        public int Fulltextsearch { get { return m_fulltextsearch; } set { m_fulltextsearch = value; } }

        private int m_cachelog;
        public int Cachelog { get { return m_cachelog; } set { m_cachelog = value; } }

        private int m_onlinetimeout = 10;
        /// <summary>无动作离线时间，默认10分钟</summary>
        [Description("无动作离线时间，默认10分钟")]
        public int Onlinetimeout { get { return m_onlinetimeout; } set { m_onlinetimeout = value; } }

        //private int m_topicqueuestats;
        //public int TopicQueueStats { get { return m_topicqueuestats; } set { m_topicqueuestats = value; } }

        //private int m_topicqueuestatscount = 20;
        //public int TopicQueueStatsCount { get { return m_topicqueuestatscount; } set { m_topicqueuestatscount = value; } }

        private int m_displayratecount = 100;
        public int DisplayRateCount { get { return m_displayratecount; } set { m_displayratecount = value; } }

        private string m_reportusergroup = "1";
        public string Reportusergroup { get { return m_reportusergroup; } set { m_reportusergroup = value; } }

        private string m_photomangegroups;
        public string Photomangegroups { get { return m_photomangegroups; } set { m_photomangegroups = value; } }

        //private int m_silverlight;
        //public int Silverlight { get { return m_silverlight; } set { m_silverlight = value; } }

        private int m_browsecreatetemplate;
        /// <summary>浏览自动生成</summary>
        [Description("浏览自动生成")]
        public int BrowseCreateTemplate { get { return m_browsecreatetemplate; } set { m_browsecreatetemplate = value; } }

        private string m_ratevalveset = "1,50,200,600,800";
        public string Ratevalveset { get { return m_ratevalveset; } set { m_ratevalveset = value; } }

        private int m_topictoblog = 1;
        public int Topictoblog { get { return m_topictoblog; } set { m_topictoblog = value; } }

        private int m_aspxrewrite = 1;
        public int Aspxrewrite { get { return m_aspxrewrite; } set { m_aspxrewrite = value; } }

        private int m_viewnewtopicminute = 120;
        public int Viewnewtopicminute { get { return m_viewnewtopicminute; } set { m_viewnewtopicminute = value; } }

        private int m_specifytemplate;
        public int Specifytemplate { get { return m_specifytemplate; } set { m_specifytemplate = value; } }

        private string m_verifyimageassemly;
        public string VerifyImageAssemly { get { return m_verifyimageassemly; } set { m_verifyimageassemly = value; } }

        private int m_mytopicsavetime = 30;
        public int Mytopicsavetime { get { return m_mytopicsavetime; } set { m_mytopicsavetime = value; } }

        private int m_mypostsavetime = 30;
        public int Mypostsavetime { get { return m_mypostsavetime; } set { m_mypostsavetime = value; } }

        private int m_myattachmentsavetime = 30;
        public int Myattachmentsavetime { get { return m_myattachmentsavetime; } set { m_myattachmentsavetime = value; } }

        private Boolean m_enabletag;
        /// <summary>是否允许使用标签(Tag)功能</summary>
        [Description("是否允许使用标签(Tag)功能")]
        public Boolean Enabletag { get { return m_enabletag; } set { m_enabletag = value; } }

        //private int m_enablemall = 1;
        //public int Enablemall { get { return m_enablemall; } set { m_enablemall = value; } }

        private int m_statscachelife = 120;
        public int Statscachelife { get { return m_statscachelife; } set { m_statscachelife = value; } }

        private int m_forcewww;
        public int Forcewww { get { return m_forcewww; } set { m_forcewww = value; } }

        private Boolean m_statstatus;
        /// <summary>使用论坛流量统计</summary>
        [Description("使用论坛流量统计")]
        public Boolean Statstatus { get { return m_statstatus; } set { m_statstatus = value; } }

        private int m_pvfrequence = 60;
        public int Pvfrequence { get { return m_pvfrequence; } set { m_pvfrequence = value; } }

        private int m_oltimespan = 20;
        public int Oltimespan { get { return m_oltimespan; } set { m_oltimespan = value; } }

        private string m_recommenddebates;
        public string Recommenddebates { get { return m_recommenddebates; } set { m_recommenddebates = value; } }

        private int m_gpp = 16;
        public int Gpp { get { return m_gpp; } set { m_gpp = value; } }

        private int m_hottagcount = 10;
        public int Hottagcount { get { return m_hottagcount; } set { m_hottagcount = value; } }

        private Boolean m_disablepostad;
        /// <summary>新用户广告强力屏蔽</summary>
        [Description("新用户广告强力屏蔽")]
        public Boolean DisablePostAD { get { return m_disablepostad; } set { m_disablepostad = value; } }

        private int m_disablepostadregminute = 60;
        /// <summary>新用户广告强力屏蔽</summary>
        [Description("新用户广告强力屏蔽注册时间")]
        public int DisablePostADRegMinute { get { return m_disablepostadregminute; } set { m_disablepostadregminute = value; } }

        private int m_disablepostadpostcount = 5;
        /// <summary>新用户广告强力屏蔽发帖数</summary>
        [Description("新用户广告强力屏蔽发帖数")]
        public int DisablePostADPostCount { get { return m_disablepostadpostcount; } set { m_disablepostadpostcount = value; } }

        private string m_disablepostadregular = "((\\d{4}|\\d{4}-)?(\\d(?:\\s*)){7})|((\\d{3}|\\d{3}-)?\\d{8})|(1(?:\\s*)[35](?:\\s*)[0123456789](?:\\s*)(\\d(?:\\s*)){8})\r\n[qQ](.+?)(\\d(?:\\s*)){7}";
        /// <summary>新用户广告强力屏蔽正则表达式</summary>
        [Description("新用户广告强力屏蔽正则表达式")]
        public string DisablePostADRegular { get { return m_disablepostadregular; } set { m_disablepostadregular = value; } }

        private Boolean m_whosonlinecontract;
        /// <summary>在线列表是否隐藏游客</summary>
        [Description("在线列表是否隐藏游客")]
        public Boolean WhosOnlineContract { get { return m_whosonlinecontract; } set { m_whosonlinecontract = value; } }

        private string m_postnocustom;
        public string Postnocustom { get { return m_postnocustom; } set { m_postnocustom = value; } }

        private int m_iisurlrewrite;
        public int Iisurlrewrite { get { return m_iisurlrewrite; } set { m_iisurlrewrite = value; } }

        private int m_notificationreserveddays = 7;
        public int Notificationreserveddays { get { return m_notificationreserveddays; } set { m_notificationreserveddays = value; } }

        private int m_maxindexsubforumcount;
        public int Maxindexsubforumcount { get { return m_maxindexsubforumcount; } set { m_maxindexsubforumcount = value; } }

        private int _Deletingexpireduserfrequency = 5;
        /// <summary>删除离线用户频率，建议2~10分钟</summary>
        [Description("删除离线用户频率，建议2~10分钟")]
        public int DeletingExpiredUserFrequency
        {
            get
            {
                if (_Deletingexpireduserfrequency < 1) _Deletingexpireduserfrequency = 5;

                return _Deletingexpireduserfrequency;
            }
            set { _Deletingexpireduserfrequency = value; }
        }

        private int m_replynotificationstatus = 1;
        public int Replynotificationstatus { get { return m_replynotificationstatus; } set { m_replynotificationstatus = value; } }

        private int m_replyemailstatus = 1;
        public int Replyemailstatus { get { return m_replyemailstatus; } set { m_replyemailstatus = value; } }

        private string m_disallowfloatwin;
        public string Disallowfloatwin { get { return m_disallowfloatwin; } set { m_disallowfloatwin = value; } }

        private int m_allwoforumindexpost = 1;
        public int Allwoforumindexpost { get { return m_allwoforumindexpost; } set { m_allwoforumindexpost = value; } }

        private int m_onlineoptimization;
        /// <summary>用户在线表性能优化开关</summary>
        [Description("用户在线表性能优化开关")]
        public int Onlineoptimization { get { return m_onlineoptimization; } set { m_onlineoptimization = value; } }

        private int m_onlineusercountcacheminute;
        /// <summary>在线用户数统计缓存时间(分钟)</summary>
        [Description("在线用户数统计缓存时间(分钟)")]
        public int OnlineUserCountCacheMinute { get { return m_onlineusercountcacheminute; } set { m_onlineusercountcacheminute = value; } }

        private Boolean m_avatarstatic = false;
        /// <summary>使用动态=false/静态=true地址调用头像</summary>
        [Description("使用动态=false/静态=true地址调用头像")]
        public Boolean AvatarStatic { get { return m_avatarstatic; } set { m_avatarstatic = value; } }

        private string m_msgforwardlist = "posttopic_succeed,editpost_succeed,postreply_succeed";
        public string Msgforwardlist { get { return m_msgforwardlist; } set { m_msgforwardlist = value; } }

        private int m_quickforward = 1;
        public int Quickforward { get { return m_quickforward; } set { m_quickforward = value; } }

        private int m_posttimestoragemedia;
        public int PostTimeStorageMedia { get { return m_posttimestoragemedia; } set { m_posttimestoragemedia = value; } }

        private int m_disableshare = 1;
        public int Disableshare { get { return m_disableshare; } set { m_disableshare = value; } }

        private string m_sharelist = "0|kaixin001|开心|1,1|sina|新浪微博|1,2|renren|人人|1,3|douban|豆瓣|1,4|sohu|白社会|1,5|qq|qq书签|1,6|google|google书签|1,7|vivi|爱问收藏|1,8|live|live收藏|1,9|favorite|收藏夹|1,10|baidu|百度收藏|1";
        public string Sharelist { get { return m_sharelist; } set { m_sharelist = value; } }

        private string m_alipayaccout;
        public string Alipayaccout { get { return m_alipayaccout; } set { m_alipayaccout = value; } }

        private string m_alipaypartnercheckkey;
        public string Alipaypartnercheckkey { get { return m_alipaypartnercheckkey; } set { m_alipaypartnercheckkey = value; } }

        private string m_alipaypartnerid;
        public string Alipaypartnerid { get { return m_alipaypartnerid; } set { m_alipaypartnerid = value; } }

        private string m_tenpayaccout;
        public string Tenpayaccout { get { return m_tenpayaccout; } set { m_tenpayaccout = value; } }

        private string m_tenpaysecretkey;
        public string Tenpaysecretkey { get { return m_tenpaysecretkey; } set { m_tenpaysecretkey = value; } }

        private int m_usealipaycustompartnerid;
        public int Usealipaycustompartnerid { get { return m_usealipaycustompartnerid; } set { m_usealipaycustompartnerid = value; } }

        private int m_usealipayinstantpay;
        public int Usealipayinstantpay { get { return m_usealipayinstantpay; } set { m_usealipayinstantpay = value; } }

        private int m_cashtocreditrate = 1;
        public int Cashtocreditrate { get { return m_cashtocreditrate; } set { m_cashtocreditrate = value; } }

        private int m_mincreditstobuy;
        public int Mincreditstobuy { get { return m_mincreditstobuy; } set { m_mincreditstobuy = value; } }

        private int m_maxcreditstobuy = 1000;
        public int Maxcreditstobuy { get { return m_maxcreditstobuy; } set { m_maxcreditstobuy = value; } }

        private int m_userbuycreditscountperday = 15;
        public int Userbuycreditscountperday { get { return m_userbuycreditscountperday; } set { m_userbuycreditscountperday = value; } }

        private int m_shownewposticon = 1;
        public int Shownewposticon { get { return m_shownewposticon; } set { m_shownewposticon = value; } }

        private string _Jqueryurl;
        public string Jqueryurl
        {
            get
            {
                if (String.IsNullOrEmpty(_Jqueryurl)) _Jqueryurl = "javascript/jquery.js";

                return _Jqueryurl;
            }
            set { _Jqueryurl = value; }
        }
        public String GetJqueryUrl() { return Path.Combine(BaseConfigs.GetForumPath, Jqueryurl.TrimStart('/')); }

        private String _ImageServer;
        /// <summary>图片服务器地址，设置不同地址用于加快浏览器加载页面速度</summary>
        [Description("图片服务器地址，设置不同地址用于加快浏览器加载页面速度")]
        public String ImageServer { get { return _ImageServer; } set { _ImageServer = value; } }

        private String _JsServer;
        /// <summary>Js服务器地址，设置不同地址用于加快浏览器加载页面速度</summary>
        [Description("Js服务器地址，设置不同地址用于加快浏览器加载页面速度")]
        public String JsServer { get { return _JsServer; } set { _JsServer = value; } }

        private String _CssServer;
        /// <summary>Css服务器地址，设置不同地址用于加快浏览器加载页面速度</summary>
        [Description("Css服务器地址，设置不同地址用于加快浏览器加载页面速度")]
        public String CssServer { get { return _CssServer; } set { _CssServer = value; } }

        private string m_antispamregisterusername = "username";
        public string Antispamregisterusername { get { return m_antispamregisterusername; } set { m_antispamregisterusername = value; } }

        private string m_antispamregisteremail = "email";
        public string Antispamregisteremail { get { return m_antispamregisteremail; } set { m_antispamregisteremail = value; } }

        private string m_antispamposttitle = "title";
        public string Antispamposttitle { get { return m_antispamposttitle; } set { m_antispamposttitle = value; } }

        private string m_antispampostmessage = "message";
        public string Antispampostmessage { get { return m_antispampostmessage; } set { m_antispampostmessage = value; } }

        private string m_antispamreplacement = string.Empty;
        public string Antispamreplacement { get { return m_antispamreplacement; } set { m_antispamreplacement = value; } }

        private string m_verifycode;
        public string Verifycode { get { return m_verifycode; } set { m_verifycode = value; } }

        //private int m_installation = 1;
        //public int Installation { get { return m_installation; } set { m_installation = value; } }

        private int m_emaillogin = 1;
        public int Emaillogin { get { return m_emaillogin; } set { m_emaillogin = value; } }

        private int m_ratelisttype;
        public int Ratelisttype { get { return m_ratelisttype; } set { m_ratelisttype = value; } }

        private int m_swfupload = 1;
        public int Swfupload { get { return m_swfupload; } set { m_swfupload = value; } }

        private int m_showimgattachmode;
        public int Showimgattachmode { get { return m_showimgattachmode; } set { m_showimgattachmode = value; } }

        private int m_webgarden = 1;
        public int Webgarden { get { return m_webgarden; } set { m_webgarden = value; } }

        private int m_allowchangewidth = 1;
        public int Allowchangewidth { get { return m_allowchangewidth; } set { m_allowchangewidth = value; } }

        private int m_showwidthmode;
        public int Showwidthmode { get { return m_showwidthmode; } set { m_showwidthmode = value; } }

        private int m_deletetimelimit = 10;
        public int Deletetimelimit { get { return m_deletetimelimit; } set { m_deletetimelimit = value; } }

        private int m_datediff = 1;
        public int DateDiff { get { return m_datediff; } set { m_datediff = value; } }

        private int m_verifyregisterexpired;
        public int Verifyregisterexpired { get { return m_verifyregisterexpired; } set { m_verifyregisterexpired = value; } }

        private string m_verifyregisteremailtemp = "尊敬的{0},您在我站提交的注册请求已收到,请点击 <a href=\"{1}\">安全注册链接</a> 完成最后注册.<br/>如无法点击链接,请复制{1} 在浏览器中打开";
        public string Verifyregisteremailtemp { get { return m_verifyregisteremailtemp; } set { m_verifyregisteremailtemp = value; } }
    }
}