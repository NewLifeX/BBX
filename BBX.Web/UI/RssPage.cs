using System.Web;
using BBX.Common;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web.UI
{
    public class RssPage : PageBase
    {
        public RssPage()
        {
            HttpContext.Current.Response.ContentType = "application/xml";
            if (this.config.Rssstatus == 1)
            {
                if (DNTRequest.GetInt("forumid", -1) == -1)
                {
                    HttpContext.Current.Response.Write(Feeds.GetRssXml(this.config.Rssttl));
                    HttpContext.Current.Response.End();
                    return;
                }
                var forumInfo = Forums.GetForumInfo(DNTRequest.GetInt("forumid", -1));
                if (forumInfo != null && forumInfo.AllowRss)
                {
                    HttpContext.Current.Response.Write(Feeds.GetForumRssXml(this.config.Rssttl, DNTRequest.GetInt("forumid", -1)));
                    HttpContext.Current.Response.End();
                    return;
                }
            }
            HttpContext.Current.Response.Write("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n");
            HttpContext.Current.Response.Write("<Rss>Error</Rss>\r\n");
            HttpContext.Current.Response.End();
        }
    }
}