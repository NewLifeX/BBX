using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Web;
using BBX.Common;
using BBX.Entity;
using BBX.Forum;
using BBX.Web.UI;

namespace BBX.Web.Archiver
{
    public class showforum : ArchiverPage
    {
        private string FORUM_LINK = "<a href=\"showforum-{0}{1}\">{2}</a>";

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            //}
            //public showforum()
            //{
            if (this.config.Aspxrewrite != 1)
            {
                this.FORUM_LINK = "<a href=\"showforum{1}?forumid={0}\">{2}</a>";
            }
            int fid = DNTRequest.GetInt("forumid", -1);
            if (fid == -1)
            {
                base.ShowMsg("无效的版块ID");
                return;
            }
            var forumInfo = Forums.GetForumInfo(fid);
            if (forumInfo == null)
            {
                base.ShowMsg("不存在的版块ID");
                return;
            }
            if (!forumInfo.AllowView(this.usergroupinfo.ID))
            {
                base.ShowMsg("您没有浏览该版块的权限");
                return;
            }
            if (!String.IsNullOrEmpty(forumInfo.Password))
            {
                base.ShowMsg("简洁版本无法浏览设置了密码的版块");
                return;
            }
            base.ShowTitle(Utils.RemoveHtml(forumInfo.Name) + " - ");
            base.ShowBody();
            HttpContext.Current.Response.Write("<h1>" + this.config.Forumtitle + "</h1>");
            HttpContext.Current.Response.Write("<div class=\"forumnav\">");
            HttpContext.Current.Response.Write("<a href=\"index.aspx\">首页</a> &raquo; ");
            string[] array = forumInfo.Pathlist.Trim().Replace("&raquo; ", ",").Split(',');
            //string[] array2 = forumInfo.Parentidlist.Trim().Split(',');
            var ps = (forumInfo as XForum).AllParents;
            for (int i = 0; i < array.Length - 1; i++)
            {
                //RegexOptions options = RegexOptions.None;
                var regex = new Regex("\"/.*/list\\.aspx", RegexOptions.None);
                array[i] = regex.Replace(array[i], string.Format("\"showforum-{0}.aspx", ps[i].ID));
            }
            array[array.Length - 1] = string.Format("<a href=\"showforum-{0}.aspx\">{1}</a>", forumInfo.ID, forumInfo.Name);
            string pathlist = string.Join(" &raquo; ", array);
            HttpContext.Current.Response.Write(ForumUtils.UpdatePathListExtname(pathlist, this.config.Extname));
            HttpContext.Current.Response.Write("</div>\r\n");
            HttpContext.Current.Response.Write("<div id=\"wrap\">");
            int pageindex = DNTRequest.GetInt("page", 1);
            //int topicCount = Topics.GetTopicCount(fid);
            //var topicCount = Topic.GetTopicCount(fid);
            var topicCount = forumInfo.CurTopics;
            int pagesize = 50;
            int pagecount = (topicCount % pagesize == 0) ? (topicCount / pagesize) : (topicCount / pagesize + 1);
            if (pagecount == 0) pagecount = 1;
            if (pageindex < 1) pageindex = 1;
            if (pageindex > pagecount) pageindex = pagecount;

            if (forumInfo.Layer == 0)
            {
                string arg = (this.config.Aspxrewrite == 1) ? this.config.Extname : ".aspx";
                //List<ForumInfo> subForumList = Forums.GetSubForumList(forumInfo.ID);
                HttpContext.Current.Response.Write("<ol>");
                foreach (var item in XForum.FindAllByParent(forumInfo.ID))
                {
                    HttpContext.Current.Response.Write("<div class=\"forumitem\"><h3>");
                    HttpContext.Current.Response.Write(Utils.GetSpacesString(item.Layer));
                    HttpContext.Current.Response.Write(string.Format(this.FORUM_LINK, item.ID, arg, Utils.HtmlDecode(item.Name)));
                    HttpContext.Current.Response.Write("</h3></div>\r\n");
                }
                HttpContext.Current.Response.Write("</ol>");
            }
            HttpContext.Current.Response.Write("<ol>");
            //DataTable topicList = Topics.GetTopicList(fid, pageindex, pagesize);
            var topicList = Topic.GetTopicList(fid, pageindex, pagesize);
            foreach (var tp in topicList)
            {
                if (this.config.Aspxrewrite == 1)
                {
                    HttpContext.Current.Response.Write(String.Format("<li><a href=\"showtopic-{0}{1}\">{2}</a>  &nbsp; ({3} 篇回复)</li>", tp.ID, this.config.Extname, tp.Title.Trim(), tp.Replies));
                }
                else
                {
                    HttpContext.Current.Response.Write(String.Format("<li><a href=\"showtopic.aspx?topicid={0}\">{1}</a>  &nbsp; ({2} 篇回复)</li>", tp.ID, tp.Title.Trim(), tp.Replies));
                }
            }
            HttpContext.Current.Response.Write("</ol>");
            HttpContext.Current.Response.Write("</div>");
            HttpContext.Current.Response.Write("<div class=\"pagenumbers\">");
            if (this.config.Aspxrewrite == 1)
            {
                HttpContext.Current.Response.Write(Utils.GetStaticPageNumbers(pageindex, pagecount, "showforum-" + fid, this.config.Extname, 8));
                HttpContext.Current.Response.Write("</div>");
                HttpContext.Current.Response.Write(string.Format("<div class=\"fullversion\">查看完整版本: <a href=\"../showforum-{0}{1}\">{2}</a></div>\r\n", fid, this.config.Extname, forumInfo.Name));
            }
            else
            {
                HttpContext.Current.Response.Write(Utils.GetPageNumbers(pageindex, pagecount, "showforum.aspx", 8));
                HttpContext.Current.Response.Write("</div>");
                HttpContext.Current.Response.Write(string.Format("<div class=\"fullversion\">查看完整版本: <a href=\"../showforum.aspx?forumid={0}\">{1}</a></div>\r\n", fid, forumInfo.Name));
            }
            base.ShowFooter();
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
    }
}