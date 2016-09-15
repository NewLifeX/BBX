using System;
using System.Text;
using System.Web.UI;
using BBX.Common;
using BBX.Config;
using BBX.Entity;
using BBX.Forum;
using NewLife.Web;

namespace BBX.Web.UI
{
    public class ArchiverPage : Page
    {
        protected internal UserGroup usergroupinfo;
        protected internal int userid;
        protected internal int useradminid;
        protected internal GeneralConfigInfo config = GeneralConfigInfo.Current;

        //public ArchiverPage() { }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (this.config.Archiverstatus == 2 && IsSearchEnginesGet())
            {
                Response.Redirect(this.OrganizeURL(Request.Url));
            }
            if (this.config.Archiverstatus == 3 && IsBrowserGet())
            {
                Response.Redirect(this.OrganizeURL(Request.Url));
            }
            if (Online.Meta.Count >= this.config.Maxonlines)
            {
                this.ShowError("抱歉,目前访问人数太多,你暂时无法访问论坛.", 0);
            }
            if (this.config.Nocacheheaders == 1)
            {
                Response.Buffer = true;
                Response.ExpiresAbsolute = DateTime.Now.AddDays(-1.0);
                Response.Cache.SetExpires(DateTime.Now.AddDays(-1.0));
                Response.Expires = 0;
                Response.CacheControl = "no-cache";
                Response.Cache.SetNoStore();
            }
            var onlineUserInfo = Online.UpdateInfo();
            this.userid = onlineUserInfo.UserID;
            this.useradminid = (int)onlineUserInfo.AdminID;
            if (this.config.Closed == 1 && onlineUserInfo.AdminID != 1)
            {
                this.ShowError("", 1);
            }
            this.usergroupinfo = onlineUserInfo.Group;
            if (!this.usergroupinfo.AllowVisit)
            {
                this.ShowError("抱歉, 您所在的用户组 \"" + usergroupinfo.GroupTitle + "\" 不允许访问论坛", 2);
            }
            if (!config.Ipaccess.IsNullOrWhiteSpace() && !Utils.InIPArray(WebHelper.UserHost, Utils.SplitString(config.Ipaccess, "\n")))
            {
                this.ShowError("抱歉, 系统设置了IP访问列表限制, 您无法访问本论坛", 0);
                return;
            }
            if (!config.Ipdenyaccess.IsNullOrWhiteSpace() && Utils.InIPArray(WebHelper.UserHost, Utils.SplitString(config.Ipdenyaccess, "\n")))
            {
                this.ShowError("由于您严重违反了论坛的相关规定, 已被禁止访问.", 2);
                return;
            }
            if (onlineUserInfo.AdminID != 1 && DNTRequest.GetPageName() != "login.aspx" && Scoresets.BetweenTime(this.config.Visitbanperiods))
            {
                this.ShowError("在此时间段内不允许访问本论坛", 2);
                return;
            }
            Response.Write("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.1//EN\" \"http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd\">\r\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head>\r\n <meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />\r\n    ");
            if (!String.IsNullOrEmpty(config.Seokeywords))
            {
                Response.Write("<meta name=\"keywords\" content=\"" + this.config.Seokeywords + "\" />\r\n");
            }
            if (!String.IsNullOrEmpty(config.Seodescription))
            {
                Response.Write("<meta name=\"description\" content=\"" + this.config.Seodescription + "\" />\r\n");
            }
            Response.Write(this.config.Seohead.Trim());
            Response.Write("\r\n<link href=\"dntarchiver.css\" rel=\"stylesheet\" type=\"text/css\" />");
            if (this.config.Archiverstatus == 0)
            {
                this.ShowError("系统禁止使用Archiver", 3);
                Response.End();
            }
        }

        private string OrganizeURL(Uri requestURL)
        {
            string[] array = requestURL.AbsolutePath.Replace("archiver/", "").Split('/');
            string text = array[array.Length - 1].ToLower();
            string a;
            if ((a = text) != null)
            {
                if (!(a == "showforum.aspx"))
                {
                    if (a == "showtopic.aspx")
                    {
                        if (this.config.Aspxrewrite == 1)
                        {
                            var sb = new StringBuilder();
                            sb.Append("../" + text.Substring(0, text.IndexOf('.')));
                            var topicid = Request["topicid"].ToInt();
                            if (topicid != 0)
                            {
                                sb.Append("-" + topicid);
                                var page = Request["page"].ToInt();
                                if (page != 0) sb.Append("-" + page);
                            }
                            return sb.Append(GeneralConfigInfo.Current.Extname).ToString();
                        }
                        return requestURL.PathAndQuery.Replace("archiver/", "../");
                    }
                }
                else
                {
                    if (this.config.Aspxrewrite == 1)
                    {
                        var sb = new StringBuilder();
                        sb.Append("../" + text.Substring(0, text.IndexOf('.')));
                        var forumid = Request["forumid"].ToInt();
                        if (forumid != 0)
                        {
                            sb.Append("-" + forumid);
                            var page = Request["page"].ToInt();
                            if (page != 0) sb.Append("-" + page);
                        }
                        return sb.Append(GeneralConfigInfo.Current.Extname).ToString();
                    }
                    return requestURL.PathAndQuery.Replace("archiver/", "../");
                }
            }
            return "../index.aspx";
        }

        public void ShowTitle(string title)
        {
            Response.Write(string.Format("\r\n<title>{0}{1}{2} - Powered by " + Utils.ProductName + " Archiver</title>\r\n", Utils.HtmlEncode(title), Utils.HtmlEncode(this.config.Seotitle.Trim()), this.config.Seotitle.IsNullOrWhiteSpace() ? "" : " "));
        }

        public void ShowBody()
        {
            Response.Write("\r\n</head>\r\n\r\n<body>\r\n");
        }

        public void ShowMsg(string msg)
        {
            this.ShowBody();
            Response.Write("<div class=\"msg\">" + Utils.HtmlEncode(msg) + "</div>");
            this.ShowFooter();
        }

        public void ShowFooter()
        {
            Response.Write(string.Format("<div class=\"copyright\" align=\"center\">Powered by <a href=\"{0}\">{1}</a> Archiver {2} 2001-{3} <a href=\"{4}\" target=\"_blank\" style=\"color:#000000\">{5}</a></div>{6}\r\n</body>\r\n</html>", Utils.ProductUrl, Utils.ProductName, Utils.Version, DateTime.Now.Year, Utils.CompanyUrl, Utils.CompanyName, GeneralConfigInfo.Current.Statcode));
        }

        public void ShowError(string hint, byte mode)
        {
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
            Response.Write("<style type=\"text/css\"><!-- body { margin: 20px; font-family: Tahoma, Verdana; font-size: 14px; color: #333333; background-color: #FFFFFF; }a {color: #1F4881;text-decoration: none;}--></style></head><body><div style=\"border: #cccccc solid 1px; padding: 20px; width: 500px; margin:auto\" align=\"center\">");
            Response.Write(s2);
            Response.Write("</div></body></html>");
            Response.End();
        }

        public bool IsBrowserGet()
        {
            string[] array = new string[]
            {
                "ie",
                "opera",
                "netscape",
                "mozilla",
                "konqueror",
                "firefox"
            };
            string text = Request.Browser.Type.ToLower();
            for (int i = 0; i < array.Length; i++)
            {
                if (text.IndexOf(array[i]) >= 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsSearchEnginesGet()
        {
            if (Request.UrlReferrer == null) return false;

            string[] array = new string[]
            {
                "google",
                "yahoo",
                "msn",
                "baidu",
                "sogou",
                "sohu",
                "sina",
                "163",
                "lycos",
                "tom",
                "yisou",
                "iask",
                "soso",
                "gougou",
                "zhongsou"
            };
            string text = Request.UrlReferrer.ToString().ToLower();
            for (int i = 0; i < array.Length; i++)
            {
                if (text.IndexOf(array[i]) >= 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}