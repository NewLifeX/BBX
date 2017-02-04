using System;
using NewLife.Reflection;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using BBX.Cache;
using BBX.Common;
using BBX.Config;
using BBX.Entity;
using NewLife.Log;
using NewLife.Web;
using XCode.DataAccessLayer;

namespace BBX.Forum
{
    public class PageBase : Page
    {
        #region 重要基础信息
        protected internal GeneralConfigInfo config = GeneralConfigInfo.Current;
        protected internal string pagetitle = "页面";
        public string pagename = DNTRequest.GetPageName();
        #endregion

        #region 重要用户信息
        protected internal UserGroup usergroupinfo;
        protected internal Online oluserinfo;
        protected internal string username;
        protected internal string password;
        protected internal string userkey;
        protected internal int userid;
        protected internal int olid;
        protected internal int usergroupid;
        protected internal int useradminid;
        #endregion

        #region 模版相关
        // 前台模板是否能够显示。专门解决无权访问页面，但是前台模板还继续呈现的问题
        public Boolean CanShow = false;

        protected internal StringBuilder templateBuilder = new StringBuilder();
        protected internal int templateid;
        protected internal string templatelistboxoptions = "";
        protected internal string templatepath = "";

        public string forumpath = BaseConfigs.GetForumPath;
        public string rooturl = Utils.GetRootUrl(BaseConfigs.GetForumPath);
        public string imagedir = "default/images";
        public string cssdir = "default";
        public string jsdir = "javascript";
        public string topicidentifydir = "images/identify/";
        public string posticondir = "images/posticons/";

        public string forumurl = "";
        #endregion

        protected internal DateTime lastposttime;
        protected internal DateTime lastpostpmtime;
        protected internal DateTime lastsearchtime;
        protected internal int pmsound;
        protected internal bool ispost = DNTRequest.IsPost();
        protected internal int newpmcount;
        protected internal int realnewpmcount;
        protected internal string meta = "";
        protected internal string link = "";
        protected internal string script = "";
        protected internal int page_err;
        protected internal string msgbox_text = "";
        protected internal string usercpmsgbox_text = "";

        protected internal int newtopicminute = 120;
        protected internal int onlineusercount = 1;
        protected internal string headerad = "";
        protected internal string footerad = "";
        protected bool isseccode = true;
        protected int isguestcachepage;
        protected string mainnavigation;
        protected List<Nav> subnavigation;
        protected Int32[] mainnavigationhassub;
        private DateTime m_starttick = DateTime.Now;
        public string useravatar = "";
        public int topicattachscorefield = Scoresets.GetTopicAttachCreditsTrans();
        public int inajax = DNTRequest.GetInt("inajax", 0);
        public int infloat = DNTRequest.GetInt("infloat", 0);
        public string userinfotips = "";
        public bool isLoginCode = true;
        public bool isnarrowpage;
        public int question = DNTRequest.GetQueryInt("question", 0);
        public bool isopenconnect = QzoneConnectConfigInfo.Current.EnableConnect;
        public bool isbindconnect;
        //private static bool recordPageView = LoadBalanceConfigInfo.Current != null && LoadBalanceConfigInfo.Current.AppLoadBalance && LoadBalanceConfigInfo.Current.RecordPageView;
        protected string aspxrewriteurl = "";

        //private static Dictionary<string, int> pageViewStatistic = new Dictionary<string, int>();
        //public static Dictionary<string, int> PageViewSatisticInfo { get { return pageViewStatistic; } set { pageViewStatistic = value; } }

        #region 执行时间和次数
        //private double m_processtime;
        public Int32 Processtime { get { return (Int32)(DateTime.Now - m_starttick).TotalMilliseconds; } }

        private Int32 _querycount;
        /// <summary>查询次数</summary>
        public Int32 querycount { get { return DAL.QueryTimes - _querycount; } set { _querycount = value; } }

        private Int32 _ExecuteCount;
        /// <summary>执行次数</summary>
        public Int32 ExecuteCount { get { return DAL.ExecuteTimes - _ExecuteCount; } set { _ExecuteCount = value; } }
        #endregion

        #region 缓存页
        private int GetCachePage(string pagename)
        {
            var cacheService = XCache.Current;
            string text = cacheService.RetrieveObject("/Forum/GuestCachePage/" + pagename) as string;
            if (text != null && text.Length > 1)
            {
                Response.Write(text);
                Response.End();
                return 2;
            }
            return 1;
        }

        private bool GetUserCachePage(string pagename)
        {
            var req = HttpContext.Current.Request;
            var pcount = req.Form.Count + req.QueryString.Count;
            switch (pagename)
            {
                case "website.aspx":
                    this.isguestcachepage = this.GetCachePage(pagename);
                    break;

                case "forumindex.aspx":
                    this.isguestcachepage = this.GetCachePage(pagename);
                    break;

                case "showtopic.aspx":
                    {
                        int page = DNTRequest.GetQueryInt("page", 1);
                        int topicid = DNTRequest.GetQueryInt("topicid", 0);
                        if ((pcount == 2 || pcount == 3) && topicid > 0 && ForumUtils.ResponseShowTopicCacheFile(topicid, page))
                        {
                            Topic.UpdateViewCount(topicid, 1);
                            return true;
                        }
                        break;
                    }
                case "showforum.aspx":
                    {
                        int page = DNTRequest.GetQueryInt("page", 1);
                        int forumid = DNTRequest.GetQueryInt("forumid", 0);
                        if ((pcount == 2 || pcount == 3) && forumid > 0 && ForumUtils.ResponseShowForumCacheFile(forumid, page))
                        {
                            return true;
                        }
                        break;
                    }
            }
            return false;
        }
        #endregion

        private bool ValidateUserPermission()
        {
            if (this.onlineusercount >= this.config.Maxonlines && this.useradminid != 1 && this.pagename != "login.aspx" && this.pagename != "logout.aspx")
            {
                this.ShowMessage("抱歉,目前访问人数太多,你暂时无法访问论坛.", 0);
                return false;
            }
            if (!this.usergroupinfo.AllowVisit && this.useradminid != 1 && this.pagename != "login.aspx" && this.pagename != "register.aspx" && this.pagename != "logout.aspx" && this.pagename != "activationuser.aspx" && this.pagename != "getpassword.aspx")
            {
                this.ShowMessage("抱歉, 您所在的用户组 \"" + usergroupinfo.GroupTitle + "\" 不允许访问论坛", 2);
                return false;
            }
            if (!this.config.Ipaccess.IsNullOrWhiteSpace())
            {
                string[] iparray = Utils.SplitString(this.config.Ipaccess, "\n");
                if (!Utils.InIPArray(WebHelper.UserHost, iparray))
                {
                    this.ShowMessage("抱歉, 系统设置了IP访问列表限制, 您无法访问本论坛", 0);
                    return false;
                }
            }
            if (!this.config.Ipdenyaccess.IsNullOrWhiteSpace())
            {
                string[] iparray2 = Utils.SplitString(this.config.Ipdenyaccess, "\n");
                if (Utils.InIPArray(WebHelper.UserHost, iparray2))
                {
                    this.ShowMessage("由于您严重违反了论坛的相关规定, 已被禁止访问.", 2);
                    return false;
                }
            }
            if (this.useradminid != 1 && this.pagename != "login.aspx" && this.pagename != "logout.aspx" && !this.usergroupinfo.DisablePeriodctrl && Scoresets.BetweenTime(this.config.Visitbanperiods))
            {
                this.ShowMessage("在此时间段内不允许访问本论坛", 2);
                return false;
            }
            return true;
        }

        private bool ValidateVerifyCode()
        {
            if (String.IsNullOrEmpty(DNTRequest.GetString("vcode")))
            {
                if (!(this.pagename == "showforum.aspx"))
                {
                    if (this.pagename.EndsWith("ajax.ashx"))
                    {
                        if (DNTRequest.GetString("t") == "quickreply")
                        {
                            ResponseAjaxVcodeError();
                            return false;
                        }
                    }
                    else
                    {
                        if ((!(DNTRequest.GetString("loginsubmit") == "true") || !(this.pagename == "login.aspx")) && (!(DNTRequest.GetFormString("agree") == "true") || !(this.pagename == "register.aspx")))
                        {
                            this.AddErrLine("验证码错误");
                            return false;
                        }
                    }
                }
            }
            else
            {
                if (!Online.CheckUserVerifyCode(this.olid, DNTRequest.GetString("vcode")))
                {
                    if (this.pagename.EndsWith("ajax.ashx"))
                    {
                        ResponseAjaxVcodeError();
                        return false;
                    }
                    this.AddErrLine("验证码错误");
                    return false;
                }
            }
            return true;
        }

        private void LoadUrlConfig()
        {
            this.forumurl = this.config.Forumurl.ToLower();
            if (this.forumurl.IndexOf("http://") == 0)
            {
                if (this.forumurl.EndsWith("aspx") || this.forumurl.EndsWith("htm") || this.forumurl.EndsWith("html"))
                {
                    this.forumurl = this.forumurl.Substring(0, this.forumurl.LastIndexOf('/')) + "/";
                    return;
                }
                if (!this.forumurl.EndsWith("/"))
                {
                    this.forumurl += "/";
                    return;
                }
            }
            else
            {
                this.forumurl = BaseConfigs.GetForumPath;
            }
        }

        //public static void PageViewStatistic(string pagename)
        //{
        //    if (pageViewStatistic.Count == 0)
        //    {
        //        pageViewStatistic.Add(Utils.GetDateTime(), -1);
        //    }
        //    if (pageViewStatistic.ContainsKey(pagename))
        //    {
        //        pageViewStatistic[pagename] = pageViewStatistic[pagename] + 1;
        //        return;
        //    }
        //    pageViewStatistic.Add(pagename, 1);
        //}

        #region 页面核心流程
        static PageBase()
        {
            // 为了引发IManageProvider接口的初始化
            var provider = XCode.Membership.ManageProvider.Provider;
        }

        public PageBase()
        {
            LoadUrlConfig();
            userid = ForumUtils.GetCookie("userid").ToInt(-1);
            if (this.userid == -1 && this.config.Guestcachepagetimeout > 0 && this.GetUserCachePage(this.pagename))
            {
                return;
            }
            meta += PageHelper.AddMetaInfo(this.config.Seokeywords, this.config.Seodescription, this.config.Seohead);
            if (this.config.Nocacheheaders == 1)
            {
                Response.BufferOutput = false;
                Response.ExpiresAbsolute = DateTime.Now.AddDays(-1.0);
                Response.Cache.SetExpires(DateTime.Now.AddDays(-1.0));
                Response.Expires = 0;
                Response.CacheControl = "no-cache";
                Response.Cache.SetNoStore();
            }

            //  反射给所有字符串字段复制空字符串，页面模板因为这个问题已经故障数十次
            foreach (var fi in this.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
            {
                if (fi.FieldType == typeof(String))
                {
                    var str = (String)this.GetValue(fi);
                    if (str == null) this.SetValue(fi, "");
                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            LoadTemplate();
            base.OnLoad(e);

            if (this.pagename != "forumlist.aspx" && this.pagename != "forumindex.aspx")
            {
                this.oluserinfo = Online.UpdateInfo();
            }
            else
            {
                try
                {
                    this.oluserinfo = Online.UpdateInfo();
                }
                catch
                {
                    Thread.Sleep(2000);
                    this.oluserinfo = Online.UpdateInfo();
                }
            }
            if (this.config.PostTimeStorageMedia == 1 && Utils.GetCookie("lastposttime") != "")
            {
                var lptime = DateTime.MinValue;
                if (DateTime.TryParse(Utils.GetCookie("lastposttime"), out lptime)) this.oluserinfo.LastPostTime = lptime;
            }
            if (userid > 0)
            {
                if (oluserinfo == null)
                {
                    XTrace.WriteLine("发现Cookie记录ID={0}已登录，但是未检测到登录对象", userid);
                }
                else if (userid != oluserinfo.UserID)
                {
                    XTrace.WriteLine("发现Cookie记录ID={0}已登录，实际登录对象ID={1}", userid, oluserinfo.UserID);
                }
            }
            this.userid = this.oluserinfo.UserID;
            this.usergroupid = (int)this.oluserinfo.GroupID;
            this.username = this.oluserinfo.UserName;
            this.password = this.oluserinfo.Password;
            //2014-1-20 增加判断password是否为null，初次使用时使用QQ登陆，密码会为空
            this.userkey = password != null ? (this.password.Length > 16) ? this.password.Substring(4, 8).Trim() : "" : "";
            this.lastposttime = this.oluserinfo.LastPostTime;
            this.lastpostpmtime = this.oluserinfo.LastPostpmTime;
            this.lastsearchtime = this.oluserinfo.LastSearchTime;
            this.olid = this.oluserinfo.ID;
            //this.isopenconnect = DiscuzCloud.GetCloudServiceEnableStatus("connect");
            if (this.userid > 0)
            {
                this.useravatar = Avatars.GetAvatarUrl(this.userid, AvatarSize.Small);
            }

            this.pmsound = ForumUtils.GetCookie("pmsound").ToInt(0);
            if (this.usergroupid == 4 || this.usergroupid == 5)
            {
                var user = BBX.Entity.User.FindByID(this.userid);
                //var user2 = user as IUser;
                if (user.GroupExpiry != 0 && user.GroupExpiry <= DateTime.Now.ToString("yyyyMMdd").ToInt(0))
                {
                    var creditsUserGroupId = CreditsFacade.GetCreditsUserGroupId((float)user.Credits);
                    this.usergroupid = ((creditsUserGroupId.ID != 0) ? creditsUserGroupId.ID : this.usergroupid);
                    //Users.UpdateUserGroup(this.userid, this.usergroupid);
                    user.GroupID = usergroupid;
                    user.Save();
                }
            }
            this.usergroupinfo = UserGroup.FindByID(this.usergroupid);
            this.useradminid = this.usergroupinfo.RadminID;
            string userCreditsCookie = ForumUtils.GetUserCreditsCookie(this.userid, this.usergroupinfo.GroupTitle);
            if (userCreditsCookie != "")
            {
                string[] array = userCreditsCookie.Split(',');
                this.userinfotips = "<p><a class=\"drop\" onmouseover=\"showMenu(this.id);\" href=\"" + BaseConfigs.GetForumPath + "usercpcreditspay.aspx\" id=\"extcreditmenu\">" + array[0] + "</a> ";
                string text = this.userinfotips;
                this.userinfotips = text + "<span class=\"pipe\">|</span>用户组: <a class=\"xi2\" id=\"g_upmine\" href=\"" + BaseConfigs.GetForumPath + "usercp.aspx\">" + array[1].Split(':')[1] + "</a></p>";
                this.userinfotips += "<ul id=\"extcreditmenu_menu\" class=\"p_pop\" style=\"display:none;\">";
                for (int i = 2; i < array.Length; i++)
                {
                    this.userinfotips += string.Format("<li><a> {0}</a></li>", array[i]);
                }
                this.userinfotips += "</ul>";
            }
            this.mainnavigation = Nav.GetNavigationString(this.userid, this.useradminid);
            this.subnavigation = Nav.GetSubNavigation();
            this.mainnavigationhassub = Nav.Root.Childs.GetItem<Int32>(Nav._.ID).ToArray();
            if (this.config.Closed == 1 && this.pagename != "login.aspx" && this.pagename != "logout.aspx" && this.pagename != "register.aspx" && this.useradminid != 1)
            {
                this.ShowMessage(1);
                return;
            }
            if (!Utils.InArray(this.pagename, "attachment.aspx"))
            {
                //this.onlineusercount = Online.Meta.Count;
                var st = Online.GetStat();
                this.onlineusercount = st.Total;
            }
            if (!this.ValidateUserPermission()) return;

            if (this.userid != -1 && !Utils.InArray(this.pagename, "attachment.aspx"))
            {
                Online.UpdateOnlineTime(this.config.Oltimespan, this.userid);
            }
            var tmp = Template.FindByID(this.templateid);
            this.templatepath = tmp.Directory;
            if (!String.IsNullOrEmpty(tmp.Url) && tmp.Url.StartsWithIgnoreCase("http://"))
            {
                imagedir = tmp.Url.TrimEnd('/') + "/images";
                cssdir = tmp.Url.TrimEnd('/');
            }
            else
            {
                imagedir = forumpath + "templates/" + tmp.Directory + "/images";
                cssdir = forumpath + "templates/" + tmp.Directory;
            }
            if (!config.ImageServer.IsNullOrEmpty()) imagedir = config.ImageServer.TrimEnd("/") + imagedir;
            if (!config.CssServer.IsNullOrEmpty()) cssdir = config.CssServer.TrimEnd("/") + cssdir;

            this.topicidentifydir = this.forumpath + "images/identify";
            this.posticondir = this.forumpath + "images/posticons";
            this.jsdir = "javascript";
            if (!config.JsServer.IsNullOrEmpty())
                jsdir = config.JsServer.EnsureEnd("/") + jsdir;
            else
                jsdir = this.rooturl + jsdir;

            //this.nowdatetime = Utils.GetDateTime();
            //this.ispost = DNTRequest.IsPost();
            //this.isget = DNTRequest.IsGet();
            //this.link = "";
            //this.script = "";
            this.templatelistboxoptions = Caches.GetTemplateListBoxOptionsCache();
            string oldValue = string.Format("<li><a href=\"###\" onclick=\"window.location.href='{0}showtemplate.aspx?templateid={1}'\">", "", BaseConfigs.GetForumPath, this.templateid);
            string newValue = string.Format("<li class=\"current\"><a href=\"###\" onclick=\"window.location.href='{0}showtemplate.aspx?templateid={1}'\">", BaseConfigs.GetForumPath, this.templateid);
            this.templatelistboxoptions = this.templatelistboxoptions.Replace(oldValue, newValue);
            this.isLoginCode = this.config.Seccodestatus.Contains("login.aspx");
            this.isseccode = (Utils.InArray(this.pagename, this.config.Seccodestatus) && this.usergroupinfo.IgnoresecCode == 0);
            this.headerad = Advertisement.GetOneHeaderAd("", 0);
            this.footerad = Advertisement.GetOneFooterAd("", 0);
            if (this.config.Allowchangewidth == 0) Utils.WriteCookie("allowchangewidth", "");

            if (this.pagename != "website.aspx" && (Utils.GetCookie("allowchangewidth") == "0" || (string.IsNullOrEmpty(Utils.GetCookie("allowchangewidth")) && this.config.Showwidthmode == 1)))
            {
                this.isnarrowpage = true;
            }
            if (this.isseccode && this.ispost && !this.ValidateVerifyCode()) return;

            this.newtopicminute = this.config.Viewnewtopicminute;

            CanShow = true;

            this.ShowPage();
        }

        void LoadTemplate()
        {
            var sid = DNTRequest.GetInt("selectedtemplateid", 0);
            if (!Template.Has(sid))
            {
                var cookie = Utils.GetCookie(Utils.GetTemplateCookieName());
                if (Template.Has(cookie))
                {
                    sid = cookie.ToInt(this.config.Templateid);
                }
            }
            if (sid == 0) sid = this.config.Templateid;
            this.templateid = sid;
        }

        protected virtual void ShowPage() { }

        protected override void OnUnload(EventArgs e)
        {
            var pname = this.pagename;
            var req = HttpContext.Current.Request;
            var pcount = req.Form.Count + req.QueryString.Count;
            if (this.isguestcachepage == 1 && pname != null)
            {
                int page = DNTRequest.GetQueryInt("page", 1);
                switch (pname)
                {
                    case "index.aspx":
                        var cacheService = XCache.Current;
                        if (!(cacheService.RetrieveObject("/Forum/GuestCachePage/" + this.pagename) is string) && this.templateBuilder.Length > 1 && this.templateid == this.config.Templateid)
                        {
                            this.templateBuilder.Append("\r\n\r\n<!-- " + Utils.ProductName + " CachedPage (Created: " + Utils.GetDateTime() + ") -->");
                            XCache.Add("/Forum/GuestCachePage/" + this.pagename, this.templateBuilder.ToString());
                        }
                        break;
                    case "showtopic.aspx":
                        int topicid = DNTRequest.GetQueryInt("topicid", 0);
                        if ((pcount == 2 || pcount == 3) && topicid > 0 && this.templateid == this.config.Templateid)
                        {
                            ForumUtils.CreateShowTopicCacheFile(topicid, page, this.templateBuilder.ToString());
                        }
                        break;
                    case "showforum.aspx":
                        int forumid = DNTRequest.GetQueryInt("forumid", 0);
                        if ((pcount == 2 || pcount == 3) && forumid > 0 && this.templateid == this.config.Templateid)
                        {
                            ForumUtils.CreateShowForumCacheFile(forumid, page, this.templateBuilder.ToString());
                        }
                        break;
                }
            }
            base.OnUnload(e);
        }

        protected override void OnInit(EventArgs e)
        {
            //if (this.isguestcachepage == 1) this.m_processtime = 0.0;
            this.querycount = DAL.QueryTimes;
            this.ExecuteCount = DAL.ExecuteTimes;

            //this.m_starttick = DateTime.Now;
            var time = Context.Items["StartTime"].ToDateTime();
            if (time > DateTime.MinValue && time < m_starttick) m_starttick = time;
            this.isbindconnect = (this.isopenconnect && QzoneConnectContext.Current.IsOnlineUserBindConnect);
            //this.ShowPage();

            base.OnInit(e);
        }

        //protected override void OnPreLoad(EventArgs e)
        //{
        //    base.OnPreLoad(e);
        //}

        //protected override void OnPreRender(EventArgs e)
        //{
        //    //this.m_processtime = (DateTime.Now - m_starttick).TotalMilliseconds;
        //    //this.querycount = DbHelper.QueryCount + (DAL.QueryTimes - this.querycount);
        //    //this.ExecuteCount = DAL.ExecuteTimes - this.ExecuteCount;
        //    DbHelper.QueryCount = 0;

        //    base.OnPreRender(e);
        //}
        #endregion

        #region 页头辅助处理
        private void ResponseAjaxVcodeError()
        {
            Response.Clear();
            Response.ContentType = "Text/XML";
            Response.Expires = 0;
            Response.Cache.SetNoStore();
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n");
            stringBuilder.Append("<error>验证码错误</error>");
            Response.Write(stringBuilder.ToString());
            Response.End();
        }

        //public void SetMetaRefresh()
        //{
        //    this.SetMetaRefresh(2, this.msgbox_url);
        //}

        //public void SetMetaRefresh(int sec)
        //{
        //    this.SetMetaRefresh(sec, this.msgbox_url);
        //}

        /// <summary>通过元数据Meta设置刷新</summary>
        /// <param name="sec"></param>
        /// <param name="url"></param>
        public void SetMetaRefresh(int sec = 2, string url = null)
        {
            if (url == null) url = this.msgbox_url;
            if (this.infloat != 1) meta += PageHelper.SetMetaRefresh(sec, url);
        }

        //public void AddLinkCss(string url)
        //{
        //    this.link = this.link + "\r\n<link href=\"" + url + "\" rel=\"stylesheet\" type=\"text/css\" />";
        //}

        public void AddLinkRss(string url, string title)
        {
            this.link = this.link + "\r\n<link rel=\"alternate\" type=\"application/rss+xml\" title=\"" + Utils.RemoveHtml(title) + "\" href=\"" + url + "\" />";
        }

        public void AddLinkCss(string url, string linkid)
        {
            this.link = this.link + "\r\n<link href=\"" + url + "\" rel=\"stylesheet\" type=\"text/css\" id=\"" + linkid + "\" />";
        }

        //public void AddScript(string scriptstr)
        //{
        //    this.AddScript(scriptstr, "javascript");
        //}

        public void AddScript(string scriptstr, string scripttype = "javascript")
        {
            if (!scripttype.EqualIgnoreCase("vbscript") && !scripttype.EqualIgnoreCase("vbscript"))
            {
                scripttype = "javascript";
            }
            this.script = this.script + "\r\n<script type=\"text/" + scripttype + "\">" + scriptstr + "</script>";
        }

        public string FormatBytes(double bytes)
        {
            if (bytes > 1073741824.0)
            {
                return (Math.Round(bytes / 1073741824.0 * 100.0) / 100.0).ToString() + " G";
            }
            if (bytes > 1048576.0)
            {
                return (Math.Round(bytes / 1048576.0 * 100.0) / 100.0).ToString() + " M";
            }
            if (bytes > 1024.0)
            {
                return (Math.Round(bytes / 1024.0 * 100.0) / 100.0).ToString() + " K";
            }
            return bytes.ToString() + " Bytes";
        }

        //public string FormatBytes(string byteStr)
        //{
        //    return this.FormatBytes((double)byteStr.ToInt());
        //}

        public bool IsErr()
        {
            return this.page_err > 0;
        }
        #endregion

        #region 消息框
        protected internal string msgbox_showbacklink = "true";
        protected internal string msgbox_backlink = "javascript:history.back();";
        protected internal string msgbox_url = "";

        /// <summary>设置消息框要跳转的链接</summary>
        /// <param name="strurl"></param>
        public void SetUrl(string strurl)
        {
            this.msgbox_url = strurl;
        }

        /// <summary>设置后退的链接脚本</summary>
        /// <param name="strback"></param>
        public void SetBackLink(string strback)
        {
            this.msgbox_backlink = strback;
        }

        /// <summary>是否显示后退链接。默认显示</summary>
        /// <param name="link"></param>
        public void SetShowBackLink(bool link)
        {
            this.msgbox_showbacklink = link.ToString().ToLower();
        }

        /// <summary>添加错误行</summary>
        /// <param name="errinfo"></param>
        public void AddErrLine(string errinfo)
        {
            AddMsgLine(errinfo);

            this.page_err++;
        }

        /// <summary>添加消息行</summary>
        /// <param name="strinfo"></param>
        public void AddMsgLine(string strinfo)
        {
            if (!String.IsNullOrEmpty(msgbox_text)) msgbox_text += "<br />";
            msgbox_text += strinfo;
        }

        public void MsgForward(string forwardName, bool spJump = false)
        {
            if (this.config.Quickforward == 1 && this.infloat == 0 && Utils.InArray(forwardName, this.config.Msgforwardlist))
            {
                Response.Redirect(spJump ? this.msgbox_url : (this.forumpath + this.msgbox_url), true);
            }
        }

        //public void MsgForward(string forwardName)
        //{
        //    this.MsgForward(forwardName, false);
        //}
        #endregion

        #region 显示信息
        public void ShowMessage(byte mode)
        {
            this.ShowMessage("", mode);
        }

        public void ShowMessage(string hint, byte mode)
        {
            //var Response = Response;

            Response.Clear();
            Response.Write("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\r\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head><title>");
            string s;
            string s2;
            switch (mode)
            {
                case 1:
                    s = "论坛已关闭";
                    s2 = this.config.Closedreason;
                    break;

                case 2:
                    s = "禁止访问";
                    s2 = hint;
                    break;

                default:
                    s = "提示";
                    s2 = hint;
                    break;
            }
            Response.Write(s);
            Response.Write(" - ");
            Response.Write(this.config.Forumtitle);
            Response.Write(" - Powered by " + Utils.ProductName + "</title><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
            Response.Write(this.meta);
            Response.Write("<style type=\"text/css\"><!-- body { margin: 20px; font-family: Tahoma, Verdana; font-size: 14px; color: #333333; background-color: #FFFFFF; }a {color: #1F4881;text-decoration: none;}--></style></head><body><div style=\"border: #cccccc solid 1px; padding: 20px; width: 500px; margin:auto\" align=\"center\">");
            Response.Write(s2);
            Response.Write("</div><br /><br /><br /><div style=\"border: 0px; padding: 0px; width: 500px; margin:auto\"><strong>当前服务器时间:</strong> ");
            Response.Write(Utils.GetDateTime());
            Response.Write("<br /><strong>当前页面</strong> ");
            Response.Write(this.pagename);
            Response.Write("<br /><strong>可选择操作:</strong> ");
            if (String.IsNullOrEmpty(this.userkey))
            {
                Response.Write("<a href=");
                Response.Write(this.forumpath);
                Response.Write("login.aspx>登录</a> | <a href=");
                Response.Write(this.forumpath);
                Response.Write("register.aspx>注册</a>");
            }
            else
            {
                Response.Write("<a href=\"logout.aspx?userkey=" + this.userkey + "\">退出</a>");
                if (this.useradminid == 1)
                {
                    Response.Write(" | <a href=\"logout.aspx?userkey=" + this.userkey + "\">系统管理</a>");
                }
            }
            Response.Write("</div></body></html>");
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        #endregion

        #region 页面重写
        protected string ShowForumAspxRewrite(string forumid, int pageid)
        {
            return this.ShowForumAspxRewrite(forumid.ToInt(0), (pageid <= 0) ? 0 : pageid);
        }

        protected string ShowForumAspxRewrite(int forumid, int pageid)
        {
            return Urls.ShowForumAspxRewrite(forumid, pageid);
        }

        protected string ShowForumAspxRewrite(string pathlist, int forumid, int pageid)
        {
            return Urls.ShowForumAspxRewrite(pathlist, forumid, pageid);
        }

        protected string ShowForumAspxRewrite(int forumid, int pageid, string rewritename)
        {
            return Urls.ShowForumAspxRewrite(forumid, pageid, rewritename);
        }

        protected string ShowTopicAspxRewrite(string topicid, int pageid)
        {
            return this.ShowTopicAspxRewrite(topicid.ToInt(0), (pageid <= 0) ? 0 : pageid);
        }

        protected string ShowTopicAspxRewrite(int topicid, int pageid)
        {
            return Urls.ShowTopicAspxRewrite(topicid, pageid);
        }

        //protected string ShowDebateAspxRewrite(string topicid)
        //{
        //    return this.ShowDebateAspxRewrite(topicid.ToInt(0));
        //}

        //protected string ShowDebateAspxRewrite(int topicid)
        //{
        //    return Urls.ShowDebateAspxRewrite(topicid);
        //}

        //protected string ShowBonusAspxRewrite(string topicid, int pageid)
        //{
        //    return this.ShowBonusAspxRewrite(topicid.ToInt(0), (pageid <= 0) ? 0 : pageid);
        //}

        //protected string ShowBonusAspxRewrite(int topicid, int pageid)
        //{
        //    return Urls.ShowBonusAspxRewrite(topicid, pageid);
        //}

        protected string UserInfoAspxRewrite(int userid)
        {
            return Urls.UserInfoAspxRewrite(userid);
        }

        protected string UserInfoAspxRewrite(string userid)
        {
            return this.UserInfoAspxRewrite(userid.ToInt(0));
        }

        //protected string RssAspxRewrite(int forumid)
        //{
        //    return Urls.RssAspxRewrite(forumid);
        //}

        //protected string RssAspxRewrite(string forumid)
        //{
        //    return this.RssAspxRewrite(forumid.ToInt(0));
        //}

        //protected string ShowGoodsAspxRewrite(string goodsid)
        //{
        //    return this.ShowGoodsAspxRewrite(goodsid.ToInt(0));
        //}

        //protected string ShowGoodsAspxRewrite(int goodsid)
        //{
        //    return Urls.ShowGoodsAspxRewrite(goodsid);
        //}

        //protected string ShowGoodsListAspxRewrite(string categoryid, int pageid)
        //{
        //    return this.ShowGoodsListAspxRewrite(categoryid.ToInt(0), (pageid <= 0) ? 0 : pageid);
        //}

        //protected string ShowGoodsListAspxRewrite(int categoryid, int pageid)
        //{
        //    return Urls.ShowGoodsListAspxRewrite(categoryid, pageid);
        //}
        #endregion
    }
}