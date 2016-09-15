using System;
using System.Web;
using System.Web.UI;
using BBX.Config;
using BBX.Forum;

namespace BBX.Web.UI
{
    public class SitemapPage : Page
    {
        public SitemapPage()
        {
            if (GeneralConfigInfo.Current.Sitemapstatus == 1)
            {
                HttpContext.Current.Response.ContentType = "application/xml";
                HttpContext.Current.Response.AppendHeader("Last-Modified", DateTime.Now.ToString("r"));
                HttpContext.Current.Response.Write(Feeds.GetBaiduSitemap(GeneralConfigInfo.Current.Sitemapttl));
                HttpContext.Current.Response.End();
            }
            HttpContext.Current.Response.Write("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n");
            HttpContext.Current.Response.Write("<Document>Sitemap is forbidden</Document>\r\n");
            HttpContext.Current.Response.End();
        }
    }
}