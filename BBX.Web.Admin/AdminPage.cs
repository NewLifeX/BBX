using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using System.Xml;
using BBX.Common;
using BBX.Config;
using BBX.Entity;
using BBX.Forum;
using NewLife.Web;

namespace BBX.Web.Admin
{
    public class AdminPage : Page
    {
        private enum FavoriteStatus
        {
            Show,
            Hidden,
            Full,
            Exist
        }

        private const int MaxShortcutMenuCount = 15;
        protected internal string username;
        protected internal int userid;
        protected internal int usergroupid;
        protected internal short useradminid;
        protected internal string grouptitle;
        protected internal string ip;
        protected internal GeneralConfigInfo config;

        public string footer = "<div align=\"center\" style=\" padding-top:60px;font-size:11px; font-family: Arial;\">Powered by <a style=\"COLOR: #000000\" href=\"" + Utils.ProductUrl + "\" target=\"_blank\">" + Utils.ProductName + "</a> &nbsp;&copy; 2002-" + DateTime.Now.Year + ", <a style=\"COLOR: #000000;font-weight:bold\" href=\"" + Utils.CompanyUrl + "\" target=\"_blank\">" + Utils.CompanyName + "</a></div>";

        public bool AllowShowNavigation = true;
        public float m_starttick = (float)Environment.TickCount;
        public float m_processtime;

        public float Processtime { get { return m_processtime; } }

        private bool IsRestore
        {
            get
            {
                return base.Request.QueryString["IsRestore"] != null && base.Request.QueryString["IsRestore"] == "1" && base.Request.Form["__VIEWSTATE"] == null;
            }
        }

        private string RestoreKey { get { return base.Request.QueryString["key"]; } }

        private bool saveState;

        public bool SavePageState { get { return saveState; } set { saveState = value; } }

        private NameValueCollection postData = new NameValueCollection();
        private NameValueCollection PostData
        {
            get
            {
                if (this.IsRestore) return this.postData;

                return base.Request.Form;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            this.m_processtime = ((float)Environment.TickCount - this.m_starttick) / 1000f;
            base.OnInit(e);
        }

        public AdminPage()
        {
            if (!this.Page.IsPostBack)
            {
                this.RegisterAdminPageClientScriptBlock();
            }
            config = GeneralConfigInfo.Current;
            if (!config.Adminipaccess.IsNullOrWhiteSpace())
            {
                string[] iparray = Utils.SplitString(this.config.Adminipaccess, "\n");
                if (!Utils.InIPArray(WebHelper.UserHost, iparray))
                {
                    this.Context.Response.Redirect(BaseConfigs.GetForumPath + "admin/syslogin.aspx");
                    return;
                }
            }
        }

        protected override void OnPreLoad(EventArgs e)
        {
            base.OnPreLoad(e);

            var online = Online.UpdateInfo();
            //var gp = UserGroup.FindByID((int)online.GroupID);
            var gp = online.Group;
            if (online.UserID <= 0 || !gp.Is管理员)
            {
                this.Context.Response.Redirect(BaseConfigs.GetForumPath + "admin/syslogin.aspx");
                return;
            }
            string secques = Users.GetUserInfo(online.UserID).Secques;
            if (this.Context.Request.Cookies["bbx_admin"] == null || this.Context.Request.Cookies["bbx_admin"]["key"] == null || ForumUtils.GetCookiePassword(this.Context.Request.Cookies["bbx_admin"]["key"].ToString(), this.config.Passwordkey) != online.Password + secques + online.UserID.ToString())
            {
                this.Context.Response.Redirect(BaseConfigs.GetForumPath + "admin/syslogin.aspx");
                return;
            }
            HttpCookie httpCookie = HttpContext.Current.Request.Cookies["bbx_admin"];
            httpCookie.Values["key"] = ForumUtils.SetCookiePassword(online.Password + secques + online.UserID.ToString(), this.config.Passwordkey);
            httpCookie.Values["userid"] = online.UserID.ToString();
            httpCookie.Expires = DateTime.Now.AddMinutes(30.0);
            HttpContext.Current.Response.AppendCookie(httpCookie);
            this.userid = online.UserID;
            this.username = online.UserName;
            this.usergroupid = (int)online.GroupID;
            this.useradminid = (short)gp.RadminID;
            this.grouptitle = gp.GroupTitle;
            this.ip = WebHelper.UserHost;
        }

        public void RegisterAdminPageClientScriptBlock()
        {
            string script = "<div id=\"success\" style=\"position:absolute;z-index:300;height:120px;width:284px;left:50%;top:50%;margin-left:-150px;margin-top:-80px;\">\r\n\t<div id=\"Layer2\" style=\"position:absolute;z-index:300;width:270px;height:90px;background-color: #FFFFFF;border:solid #000000 1px;font-size:14px;\">\r\n\t\t<div id=\"Layer4\" style=\"height:26px;background:#f1f1f1;line-height:26px;padding:0px 3px 0px 3px;font-weight:bolder;\">操作提示</div>\r\n\t\t<div id=\"Layer5\" style=\"height:64px;line-height:150%;padding:0px 3px 0px 3px;\" align=\"center\"><BR /><table><tr><td valign=top><img border=\"0\" src=\"../images/ajax_loading.gif\"  /></td><td valign=middle style=\"font-size: 14px;\" >正在执行当前操作, 请稍等...<BR /></td></tr></table><BR /></div>\r\n\t</div>\r\n\t<div id=\"Layer3\" style=\"position:absolute;width:270px;height:90px;z-index:299;left:4px;top:5px;background-color: #E8E8E8;\"></div>\r\n</div>\r\n<script> \r\ndocument.getElementById('success').style.display = \"none\"; \r\n</script> \r\n<script type=\"text/javascript\" src=\"../js/divcover.js\"></script>\r\n";
            base.ClientScript.RegisterClientScriptBlock(base.GetType(), "Page", script);
        }

        public new void RegisterStartupScript(string key, string scriptstr)
        {
            key = key.ToLower();
            if (key == "pagetemplate" || key == "page")
            {
                string script = "";
                if (key == "page")
                {
                    script = "<script> \r\nvar bar=0;\r\ndocument.getElementById('success').style.display = \"block\";  \r\ndocument.getElementById('Layer5').innerHTML ='<BR>操作成功执行<BR>';  \r\ncount() ; \r\nfunction count(){ \r\nbar=bar+4; \r\nif (bar<99) \r\n{setTimeout(\"count()\",100);} \r\nelse { \r\ndocument.getElementById('success').style.display = \"none\";HideOverSels('success'); \r\n" + scriptstr + "} \r\n} \r\n</script> \r\n<script> window.onload = function(){HideOverSels('success')};</script>\r\n";
                }
                if (key == "pagetemplate")
                {
                    script = "<script> \r\nvar bar=0;\r\n success.style.display = \"block\";  \r\ndocument.getElementById('Layer5').innerHTML = '<BR>" + scriptstr + "<BR>';  \r\ncount() ; \r\nfunction count(){ \r\nbar=bar+4; \r\nif (bar<99) \r\n{setTimeout(\"count()\",100);} \r\nelse { \r\ndocument.getElementById('success').style.display = \"none\";HideOverSels('success'); \r\n}} \r\n</script> \r\n<script> window.onload = function(){HideOverSels('success')};</script>\r\n";
                }
                base.ClientScript.RegisterStartupScript(base.GetType(), key, script);
                return;
            }
            base.ClientScript.RegisterStartupScript(base.GetType(), key, scriptstr);
        }

        public void CallBaseRegisterStartupScript(string key, string scriptstr)
        {
            base.ClientScript.RegisterStartupScript(base.GetType(), key, scriptstr);
        }

        public bool CheckCookie()
        {
            config = GeneralConfigInfo.Current;
            if (!config.Adminipaccess.IsNullOrWhiteSpace())
            {
                string[] iparray = Utils.SplitString(this.config.Adminipaccess, "\n");
                if (!Utils.InIPArray(WebHelper.UserHost, iparray))
                {
                    this.Context.Response.Redirect(BaseConfigs.GetForumPath + "admin/syslogin.aspx");
                    return false;
                }
            }
            var online = Online.UpdateInfo();
            //var gp = UserGroup.FindByID((int)online.GroupID);
            if (online.UserID <= 0 || !online.Group.Is管理员)
            {
                this.Context.Response.Redirect(BaseConfigs.GetForumPath + "admin/syslogin.aspx");
                return false;
            }
            string secques = Users.GetUserInfo(online.UserID).Secques;
            if (this.Context.Request.Cookies["bbx_admin"] == null || this.Context.Request.Cookies["bbx_admin"]["key"] == null || ForumUtils.GetCookiePassword(this.Context.Request.Cookies["bbx_admin"]["key"].ToString(), this.config.Passwordkey) != online.Password + secques + online.UserID)
            {
                this.Context.Response.Redirect(BaseConfigs.GetForumPath + "admin/syslogin.aspx");
                return false;
            }
            HttpCookie httpCookie = HttpContext.Current.Request.Cookies["bbx_admin"];
            httpCookie.Values["key"] = ForumUtils.SetCookiePassword(online.Password + secques + online.UserID, this.config.Passwordkey);
            httpCookie.Expires = DateTime.Now.AddMinutes(30.0);
            HttpContext.Current.Response.AppendCookie(httpCookie);
            return true;
        }

        public bool IsFounderUid(int uid)
        {
            return BaseConfigInfo.Current.Founderuid == 0 || BaseConfigInfo.Current.Founderuid == uid;
        }

        protected string GetShowMessage()
        {
            string str = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">";
            str += "<html xmlns=\"http://www.w3.org/1999/xhtml\"><head><title>您没有权限运行当前程序!</title><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">";
            str += "<link href=\"../styles/default.css\" type=\"text/css\" rel=\"stylesheet\"></head><body><br><br><div style=\"width:100%\" align=\"center\">";
            str += "<div align=\"center\" style=\"width:660px; border:1px dotted #FF6600; background-color:#FFFCEC; margin:auto; padding:20px;\"><img src=\"../images/hint.gif\" border=\"0\" alt=\"提示:\" align=\"absmiddle\" width=\"11\" height=\"13\" /> &nbsp;";
            return str + "您没有权限运行当前程序,请您以论坛创始人身份登陆后台进行操作!</div></div></body></html>";
        }

        public void LoadRegisterStartupScript(string key, string scriptstr)
        {
            string text = "程序执行中... <BR /> 当前操作可能要运行一段时间.<BR />您可在此期间进行其它操作<BR /><BR />";
            string scriptstr2 = "<script> \r\nvar bar=0;\r\n success.style.display = \"block\";  \r\ndocument.getElementById('Layer5').innerHTML ='" + text + "';  \r\ncount() ; \r\nfunction count(){ \r\nbar=bar+2; \r\nif (bar<99) \r\n{setTimeout(\"count()\",100);} \r\nelse { \r\n\tdocument.getElementById('success').style.display = \"none\";HideOverSels('success'); \r\n" + scriptstr + "} \r\n} \r\n</script> \r\n<script> window.onload = function(){HideOverSels('success')};</script>\r\n";
            this.CallBaseRegisterStartupScript(key, scriptstr2);
        }

        protected override void OnLoad(EventArgs e)
        {
            if (this.IsRestore)
            {
                ArrayList arrayList = new ArrayList();
                string[] allKeys = this.PostData.AllKeys;
                for (int i = 0; i < allKeys.Length; i++)
                {
                    string text = allKeys[i];
                    var control = this.FindControl(text);
                    if (control is IPostBackDataHandler && ((IPostBackDataHandler)control).LoadPostData(text, this.PostData))
                    {
                        arrayList.Add(control);
                    }
                }
                foreach (IPostBackDataHandler postBackDataHandler in arrayList)
                {
                    postBackDataHandler.RaisePostDataChangedEvent();
                }
            }
            base.OnLoad(e);
            string text2 = "<script type=\"text/javascript\" src=\"../js/AjaxHelper.js\"></script><script type='text/javascript'>\nfunction ResetShortcutMenu(){window.parent.LoadShortcutMenu();}\nfunction FavoriteFunction(url){\nAjaxHelper.Updater('../UserControls/favoritefunction','resultmessage','url='+url,ResetShortcutMenu);\n}\n</script>\n";
            text2 += "<div align='right' style=''>";
            FavoriteStatus favoriteStatus = this.GetFavoriteStatus();
            if (favoriteStatus != FavoriteStatus.Hidden)
            {
                if (favoriteStatus == FavoriteStatus.Exist)
                {
                    text2 += text2 + "<span id='resultmessage' title='已经将该页面加入到快捷操作菜单中'><img src='../images/existmenu.gif' style='vertical-align:middle' /> 已经收藏</span>";
                }
                else
                {
                    if (favoriteStatus == FavoriteStatus.Full)
                    {
                        string arg_12B_0 = text2;
                        object obj = text2;
                        text2 = arg_12B_0 + obj + "<span id='resultmessage' title='快捷操作菜单最大收藏数为" + 15 + "项'><img src='../images/fullmenu.gif' style='vertical-align:middle' /> 收藏已满</span>\n</b>";
                    }
                    else
                    {
                        if (favoriteStatus == FavoriteStatus.Show)
                        {
                            text2 = text2 + "<span align='right' id='resultmessage'>\n<a href='javascript:void(0);' title='将该页面加入快捷操作菜单' onclick='FavoriteFunction(window.location.pathname.toLowerCase().replace(\"" + BaseConfigs.GetForumPath + "admin/\",\"\") + window.location.search.toLowerCase());' style='text-decoration:none;color:#333;' onfocus=\"this.blur();\"><img src='../images/addmenu.gif' align='absmiddle' /> 加入常用功能</a>\n</span>";
                        }
                    }
                }
            }
            if (this.AllowShowNavigation)
            {
                text2 += "<span><a href='javascript:void(0);' onclick='window.parent.showNavigation()' title='按ESC键或点击链接显示导航菜单' style='text-decoration:none;color:#333;'><img src='../images/navigation.gif' style='vertical-align:middle'> 管理导航</a></span>";
            }
            text2 += "</div>";
            base.ClientScript.RegisterClientScriptBlock(base.GetType(), "Form1", text2);
            base.ClientScript.RegisterClientScriptBlock(base.GetType(), "Navigation", "<script type='text/javascript'>if(document.documentElement.addEventListener){document.documentElement.addEventListener('keydown', window.parent.resetEscAndF5, false);}else if(document.documentElement.attachEvent){document.documentElement.attachEvent('onkeydown', window.parent.resetEscAndF5);}</script>");
        }

        private FavoriteStatus GetFavoriteStatus()
        {
            var mapPath = Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "admin/xml/navmenu.config");
            var url = Request.Url.ToString().ToLower();
            url = url.Substring(url.LastIndexOf('/') + 1);

            var doc = new XmlDocument();
            doc.Load(mapPath);
            var xmlNodeList = doc.SelectNodes("/dataset/submain");
            bool flag = false;
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                if (xmlNode.SelectSingleNode("link").InnerText.IndexOf('/') != -1 && xmlNode.SelectSingleNode("link").InnerText.Split('/')[1].ToLower() == url)
                {
                    flag = true;
                }
            }
            if (!flag)
            {
                return FavoriteStatus.Hidden;
            }
            var xmlNodeList2 = doc.SelectNodes("/dataset/shortcut");
            foreach (XmlNode xmlNode2 in xmlNodeList2)
            {
                if (xmlNode2.SelectSingleNode("link").InnerText.IndexOf(url) != -1)
                {
                    FavoriteStatus result = FavoriteStatus.Exist;
                    return result;
                }
            }
            if (xmlNodeList2.Count >= 15)
            {
                return FavoriteStatus.Full;
            }
            return FavoriteStatus.Show;
        }

        private void RegisterMessage(string scriptstr, bool autoHidd, string autoJumpUrl)
        {
            string text = "<script type='text/javascript'>\r\nvar bar=0;\r\ndocument.getElementById('success').style.display = \"block\";\r\ndocument.getElementById('Layer5').innerHTML = '<BR>" + scriptstr + "<BR>';\r\n";
            if (autoHidd)
            {
                text += "count();\r\nfunction count()\r\n{\r\n\tbar=bar+4;\r\n\tif (bar<99)\r\n\t{\r\n\t\tsetTimeout(\"count()\",200);\r\n\t}\r\n\telse\r\n\t{\r\n";
                if (String.IsNullOrEmpty(autoJumpUrl))
                {
                    text += "\t\tdocument.getElementById('success').style.display = \"none\";HideOverSels('success');\r\n";
                }
                else
                {
                    text = text + "\t\twindow.location='" + autoJumpUrl + "';\r\n";
                }
                text += "\t}\r\n}\r\n";
            }
            text += "</script>\r\n<script> window.onload = function(){HideOverSels('success')};</script>\r\n";
            base.ClientScript.RegisterStartupScript(base.GetType(), "resultMessage", text);
        }

        protected void RegisterMessage(string scriptstr, string autoJumpUrl)
        {
            this.RegisterMessage(scriptstr, true, autoJumpUrl);
        }

        protected void RegisterMessage(string scriptstr, bool autoHidd)
        {
            this.RegisterMessage(scriptstr, autoHidd, "");
        }

        protected void RegisterMessage(string scriptstr)
        {
            this.RegisterMessage(scriptstr, false);
        }

        //protected void MySavePageState(object viewState)
        //{
        //    string componentName = this.userid + "_" + base.GetType().Name.Trim();
        //    var container = MyControlContainer.GetContainer();
        //    container.AddNormalComponent(componentName, viewState);
        //}

        //protected object MyLoadPageState()
        //{
        //    string componentName = this.userid + "_" + base.GetType().Name.Trim();
        //    var container = MyControlContainer.GetContainer();
        //    object normalComponentDataObject = container.GetNormalComponentDataObject(componentName);
        //    container.RemoveComponentByName(componentName);
        //    return normalComponentDataObject;
        //}

        //protected override void SavePageStateToPersistenceMedium(object viewState)
        //{
        //    base.SavePageStateToPersistenceMedium(viewState);
        //}

        //protected override object LoadPageStateFromPersistenceMedium()
        //{
        //    object result = new object();
        //    try
        //    {
        //        result = base.LoadPageStateFromPersistenceMedium();
        //    }
        //    catch
        //    {
        //        result = null;
        //    }
        //    return result;
        //}
    }
}