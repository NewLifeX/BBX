using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using BBX.Common;
using BBX.Config;
using BBX.Entity;
using NewLife.Log;
using NewLife.Threading;

namespace BBX.Forum
{
    public class HttpModule : IHttpModule
    {
        //private static Timer eventTimer;

        public void Init(HttpApplication context)
        {
            // 记录页面开始时间
            context.Context.Items["StartTime"] = DateTime.Now;

            context.BeginRequest += ReUrl_BeginRequest;
            //if (eventTimer == null && ScheduleConfigInfo.Current.Enabled)
            //{
            //    EventManager.RootPath = Utils.GetMapPath(BaseConfigs.GetForumPath);

            //    // 一分钟后，指定时间间隔（分钟）执行事件处理
            //    eventTimer = new Timer(ScheduledEventWorkCallback, context.Context, 60000, EventManager.TimerMinutesInterval * 60000);
            //}
        }

        //private void ScheduledEventWorkCallback(object sender)
        //{
        //    try
        //    {
        //        if (ScheduleConfigInfo.Current.Enabled)
        //        {
        //            EventManager.Execute();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        XTrace.WriteException(ex);
        //    }
        //}

        public void Dispose()
        {
            //eventTimer = null;
        }

        private void ReUrl_BeginRequest(object sender, EventArgs e)
        {
            var baseConfigInfo = BaseConfigInfo.Current;
            if (baseConfigInfo == null) return;

            var config = GeneralConfigInfo.Current;
            var context = ((HttpApplication)sender).Context;

            #region Forcewww
            var request = context.Request;
            var url = request.Url;
            if (config.Forcewww == 1 && url.Host.Contains(".") && !url.Host.StartsWith("www.", StringComparison.OrdinalIgnoreCase))
                context.Response.Redirect(url.Scheme + "://www." + url.Authority + url.PathAndQuery);
            #endregion

            var forumPath = baseConfigInfo.Forumpath.ToLower();
            var myPath = request.Path.ToLower();
            if (!myPath.StartsWith(forumPath) || !myPath.EndsWith(".aspx") || this.IgnorePathContains(myPath, forumPath))
                return;

            // 获取模版信息
            var templateID = config.Templateid;
            var cookie = Utils.GetCookie(Utils.GetTemplateCookieName());
            if (Template.Has(cookie)) templateID = cookie.ToInt();
            var tmp = Template.FindByID(templateID);

            #region 主题列表页 list.aspx
            if ((config.Iisurlrewrite == 1 || config.Aspxrewrite == 1) && myPath.EndsWith("/list.aspx"))
            {
                myPath = (myPath.StartsWith("/") ? myPath : ("/" + myPath));
                var array = myPath.Replace(forumPath, "/").Split('/');
                if (array.Length > 1 && !Utils.StrIsNullOrEmpty(array[1]))
                {
                    var num = 0;
                    foreach (var fi in Forums.GetForumList())
                    {
                        if (array[1].EqualIgnoreCase(fi.RewriteName))
                        {
                            num = fi.Fid;
                            break;
                        }
                    }
                    if (num > 0)
                    {
                        var text4 = "forumid=" + num;
                        if (array.Length > 2 && Utils.IsNumeric(array[2]))
                        {
                            text4 = text4 + "&page=" + array[2];
                        }
                        if (config.Specifytemplate > 0)
                        {
                            tmp = this.SelectTemplate(tmp, "showforum.aspx", text4);
                        }
                        this.CreatePage("showforum.aspx", forumPath, tmp);
                        context.RewritePath(forumPath + "aspx/" + tmp.Name + "/showforum.aspx", String.Empty, text4 + "&selectedtemplateid=" + tmp.ID);
                        return;
                    }
                    context.RewritePath(myPath.Replace("list.aspx", String.Empty), String.Empty, String.Empty);
                    return;
                }
            }
            #endregion

            #region 子目录
            if (myPath.Substring(forumPath.Length).IndexOf("/") != -1)
            {
                if (config.Aspxrewrite == 1)
                {
                    if (myPath.StartsWith(forumPath + "archiver/"))
                    {
                        var input = myPath.Substring(forumPath.Length + 8);
                        foreach (var item in SiteUrls.Current.Urls)
                        {
                            if (Regex.IsMatch(input, item.Pattern, Utils.GetRegexCompiledOptions() | RegexOptions.IgnoreCase))
                            {
                                var queryString = Regex.Replace(input, item.Pattern, item.QueryString, Utils.GetRegexCompiledOptions() | RegexOptions.IgnoreCase);
                                context.RewritePath(forumPath + "archiver" + item.Page, String.Empty, queryString);
                                return;
                            }
                        }
                    }
                    if (myPath.StartsWith(forumPath + "tools/"))
                    {
                        var input2 = myPath.Substring(forumPath.Length + 5);
                        foreach (var item in SiteUrls.Current.Urls)
                        {
                            if (Regex.IsMatch(input2, item.Pattern, Utils.GetRegexCompiledOptions() | RegexOptions.IgnoreCase))
                            {
                                var queryString2 = Regex.Replace(input2, item.Pattern, Utils.UrlDecode(item.QueryString), Utils.GetRegexCompiledOptions() | RegexOptions.IgnoreCase);
                                context.RewritePath(forumPath + "tools" + item.Page, String.Empty, queryString2);
                                return;
                            }
                        }
                    }
                }
                if (myPath.StartsWith(forumPath + "upload/") || myPath.StartsWith(forumPath + "space/upload/") || myPath.StartsWith(forumPath + "avatars/upload/"))
                {
                    context.RewritePath(forumPath + "index.aspx");
                }
                return;
            }
            #endregion

            if (myPath.EndsWith("/index.aspx") || myPath.EndsWith("/default.aspx"))
            {
                var pageName = (config.Indexpage == 0) ? "forumindex.aspx" : "website.aspx";
                CreatePage(pageName, forumPath, tmp);
                context.RewritePath(forumPath + "aspx/" + tmp.Name + "/" + pageName);
                return;
            }
            var fileAndQuery = myPath.Substring(request.Path.LastIndexOf("/"));
            var query = request.QueryString.ToString();
            // 根据重写规则进行匹配
            if (config.Aspxrewrite == 1)
            {
                foreach (var item in SiteUrls.Current.Urls)
                {
                    if (Regex.IsMatch(myPath, item.Pattern, Utils.GetRegexCompiledOptions() | RegexOptions.IgnoreCase))
                    {
                        var text7 = Regex.Replace(fileAndQuery, item.Pattern, item.QueryString, Utils.GetRegexCompiledOptions() | RegexOptions.IgnoreCase);
                        CreatePage(item.Page.Replace("/", ""), forumPath, tmp);
                        if (config.Specifytemplate > 0) tmp = SelectTemplate(tmp, item.Page, text7);
                        context.RewritePath(forumPath + "aspx/" + tmp.Name + item.Page, String.Empty, text7 + "&selectedtemplateid=" + tmp.ID + (String.IsNullOrEmpty(query) ? "" : ("&" + query)));
                        return;
                    }
                }
            }
            CreatePage(fileAndQuery.Replace("/", ""), forumPath, tmp);
            if (config.Specifytemplate > 0) tmp = SelectTemplate(tmp, myPath, query);
            context.RewritePath(forumPath + "aspx/" + tmp.Name + fileAndQuery, String.Empty, query + "&selectedtemplateid=" + tmp.ID);
        }

        public bool IgnorePathContains(String path, String forumPath)
        {
            var arr = new String[] { "admin/", "aspx/" };
            for (var i = 0; i < arr.Length; i++)
            {
                if (path.IndexOf(forumPath + arr[i]) > -1) return true;
            }
            return false;
        }

        public void CreatePage(String pageName, String forumPath, Template tmp)
        {
            var config = GeneralConfigInfo.Current;
            if (config.BrowseCreateTemplate == 1)
            {
                if (pageName == "showtemplate.aspx")
                    CreateTemplate(forumPath, Template.FindByID(DNTRequest.GetInt("templateid", 1)), (config.Indexpage == 0) ? "forumindex.aspx" : "website.aspx");

                CreateTemplate(forumPath, tmp, pageName);
            }
        }

        public Template SelectTemplate(Template tmp, String pagename, String newUrl)
        {
            var pages = "showforum,showtopic,showdebate,showbonus,posttopic,postreply,showtree,editpost,delpost,topicadmin";
            var forumid = 0;
            var num2 = pagename.LastIndexOf("/") + 1;
            var num3 = pagename.LastIndexOf(".") - num2;
            if (num3 > 0 && Utils.InArray(pagename.Substring(num2, num3), pages))
            {
                var config = GeneralConfigInfo.Current;
                var cookie = Utils.GetCookie(Utils.GetTemplateCookieName());
                var deftid = 0;
                //if (Utils.InArray(cookie, Templates.GetValidTemplateIDList()))
                if (Template.Has(cookie))
                    deftid = Utils.StrToInt(cookie, config.Templateid);

                var array = newUrl.Split('&');
                for (int i = 0; i < array.Length; i++)
                {
                    var item = array[i];
                    var value = item.Split('=')[1];
                    if (item.IndexOf("forumid=") >= 0 && value != "")
                    {
                        forumid = Utils.StrToInt(value, 0);
                    }
                    else
                    {
                        if (item.IndexOf("topicid=") >= 0 && value != "")
                        {
                            var topicInfo = Topic.FindByID(Utils.StrToInt(value, 0));
                            if (topicInfo != null) forumid = topicInfo.Fid;
                        }
                        else
                        {
                            forumid = DNTRequest.GetInt("forumid", 0);
                        }
                    }
                    if (forumid > 0)
                    {
                        var forumInfo = Forums.GetForumInfo(forumid);
                        var tid = (forumInfo == null) ? 0 : forumInfo.TemplateID;
                        if (tid <= 0)
                        {
                            tid = deftid;
                            if (tid == 0) tid = config.Templateid;
                        }
                        if (tid > 0 && tmp.ID != tid) tmp = Template.FindByID(tid);
                        break;
                    }
                }
            }
            return tmp;
        }

        private void CreateTemplate(String forumpath, Template tmp, String pagename)
        {
            var path = String.Format("{0}aspx/{1}/{2}", forumpath, tmp.Name, pagename).GetFullPath();
            if (!File.Exists(path))
            {
                var forumPageTemplate = new ForumPageTemplate();
                var rs = forumPageTemplate.GetTemplate(forumpath, tmp.Directory, pagename.Split('.')[0], 1, tmp.Name);

                // 异步生成该风格所有模版
                if (!rs.IsNullOrEmpty())
                {
                    XTrace.WriteLine("异步生成该风格{0}所有模版", tmp.Name);
                    ThreadPoolX.QueueUserWorkItem(() => ForumPageTemplate.BuildTemplate(tmp));
                }
            }
        }
    }
}