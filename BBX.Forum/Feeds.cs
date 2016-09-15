using System;
using System.Collections.Generic;
using System.Text;
using BBX.Cache;
using BBX.Common;
using BBX.Config;
using BBX.Entity;

namespace BBX.Forum
{
    public class Feeds
    {
        public static string GetRssXml(int ttl)
        {
            var cacheService = XCache.Current;
            string text = cacheService.RetrieveObject(CacheKeys.FORUM_RSS_INDEX) as string;
            if (text == null)
            {
                var guest = UserGroup.Guest;
                //var sb = new StringBuilder();
                // 查找支持RSS或者匿名访客浏览的论坛版面
                var fids = new List<Int32>();
                foreach (var item in Forums.GetForumList())
                {
                    if (item.AllowRss)
                    {
                        if (!Utils.StrIsNullOrEmpty(item.Viewperm))
                        {
                            if (Utils.InArray("7", item.Viewperm, ","))
                            {
                                //sb.AppendFormat(",{0}", current.Fid);
                                fids.Add(item.Fid);
                            }
                        }
                        else
                        {
                            if (guest.AllowVisit)
                            {
                                //sb.AppendFormat(",{0}", current.Fid);
                                fids.Add(item.Fid);
                            }
                        }
                    }
                }
                //text = BBX.Data.Feeds.BuildRssOutput(ttl, sb.ToString().Trim(','), "");
                text = BuildRssOutput(ttl, fids.ToArray(), "");
                XCache.Add(CacheKeys.FORUM_RSS_INDEX, text, ttl * 60);
            }
            return text;
        }
        public static string GetForumRssXml(int ttl, int fid)
        {
            var cacheService = XCache.Current;
            string text = cacheService.RetrieveObject("/Forum/RSS/Forum" + fid) as string;
            if (text == null)
            {
                var forumInfo = Forums.GetForumInfo(fid);
                if (forumInfo == null)
                {
                    return "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<Rss>Specified forum not found</Rss>\r\n";
                }
                if (!forumInfo.AllowView(7))
                {
                    return "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<Rss>Guest Denied</Rss>\r\n";
                }
                if (!UserGroup.Guest.AllowVisit)
                {
                    return "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<Rss>Guest Denied</Rss>\r\n";
                }
                text = BuildRssOutput(ttl, new Int32[] { fid }, forumInfo.Name);
                XCache.Add("/Forum/RSS/Forum" + fid, text, ttl * 60);
            }
            return text;
        }
        public static string GetBaiduSitemap(int ttl)
        {
            var cacheService = XCache.Current;
            string text = cacheService.RetrieveObject("/Forum/Sitemap/Baidu") as string;
            if (text == null)
            {
                var guest = UserGroup.Guest;
                var sb = new StringBuilder();
                foreach (var item in Forums.GetForumList())
                {
                    if (!item.AllowRss)
                    {
                        sb.AppendFormat(",{0}", item.Fid);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(item.ViewPerm))
                        {
                            if (!guest.AllowVisit)
                            {
                                sb.AppendFormat(",{0}", item.Fid);
                            }
                        }
                        else
                        {
                            if (!Utils.InArray("7", item.ViewPerm, ","))
                            {
                                sb.AppendFormat(",{0}", item.Fid);
                            }
                        }
                    }
                }
                sb = ((sb.Length > 0) ? sb.Remove(0, 1) : sb);
                var user = User.FindByID(BaseConfigs.GetFounderUid);
                text = GetBaiduSitemap(sb.ToString(), user);
                XCache.Add("/Forum/Sitemap/Baidu", text, ttl * 60);
            }
            return text;
        }

        private static string forumurl = "http://" + DNTRequest.GetCurrentFullHost() + BaseConfigs.GetForumPath;

        /// <summary>生成Rss输出。主要控制论坛版面</summary>
        /// <param name="ttl"></param>
        /// <param name="fids"></param>
        /// <param name="forumname"></param>
        /// <returns></returns>
        public static string BuildRssOutput(int ttl, Int32[] fids, string forumname)
        {
            var config = GeneralConfigInfo.Current;

            var sb = new StringBuilder("<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n");
            // 这些论坛版面的最新帖子
            var newTopics = Topic.GetNewTopics(fids);
            sb.Append("<?xml-stylesheet type=\"text/xsl\" href=\"rss.xsl\" media=\"screen\"?>\r\n");
            sb.Append("<rss version=\"2.0\">\r\n");
            sb.Append("  <channel>\r\n");
            sb.AppendFormat("    <title>{0}{1}</title>\r\n", Utils.HtmlEncode(config.Forumtitle), (!Utils.StrIsNullOrEmpty(forumname)) ? (" - " + Utils.HtmlEncode(forumname)) : "");
            sb.AppendFormat("    <link>{0}", forumurl);
            // 如果只有一个论坛版面，则显示完整路径
            if (fids.Length == 1)
            {
                if (config.Aspxrewrite == 1)
                    sb.AppendFormat("showforum-{0}{1}", fids[0], config.Extname);
                else
                    sb.AppendFormat("showforum.aspx?forumid={0}", fids[0]);
            }
            sb.Append("</link>\r\n");
            sb.Append("    <description>Latest 20 threads</description>\r\n");
            sb.AppendFormat("    <copyright>Copyright (c) {0}</copyright>\r\n", Utils.HtmlEncode(config.Forumtitle));
            sb.AppendFormat("    <generator>{0}</generator>\r\n", Utils.ProductName);
            sb.AppendFormat("    <pubDate>{0}</pubDate>\r\n", DateTime.Now.ToString("r"));
            sb.AppendFormat("    <ttl>{0}</ttl>\r\n", ttl);
            if (newTopics != null)
            {
                foreach (var tp in newTopics)
                {
                    sb.Append("    <item>\r\n");
                    sb.AppendFormat("      <title>{0}</title>\r\n", Utils.HtmlEncode(tp.Title));
                    sb.Append("    <description><![CDATA[");
                    if (!String.IsNullOrEmpty(tp.Message))
                    {
                        if (tp.Message.IndexOf("[hide]") > -1)
                        {
                            sb.Append("***内容隐藏***");
                        }
                        else
                        {
                            sb.Append(Utils.HtmlEncode(Utils.GetSubString(Utils.ClearUBB(tp.Message), 200, "......")));
                        }
                    }
                    sb.Append("]]></description>\r\n");
                    sb.AppendFormat("      <link>{0}", Utils.HtmlEncode(forumurl));
                    if (config.Aspxrewrite == 1)
                    {
                        sb.AppendFormat("showtopic-{0}{1}", tp.ID, config.Extname);
                    }
                    else
                    {
                        sb.AppendFormat("showtopic.aspx?topicid={0}", tp.ID);
                    }
                    sb.Append("</link>\r\n");
                    // 如果不止一个论坛版面，则需要显示分类名
                    if (fids.Length != 1)
                    {
                        sb.AppendFormat("      <category>{0}</category>\r\n", Utils.HtmlEncode(tp.ForumName));
                    }
                    sb.AppendFormat("      <author>{0}</author>\r\n", Utils.HtmlEncode(tp.Poster));
                    sb.AppendFormat("      <pubDate>{0}</pubDate>\r\n", Utils.HtmlEncode(tp.PostDateTime.ToString("r")));
                    sb.Append("    </item>\r\n");
                }
            }
            return sb.Append("  </channel>\r\n</rss>").ToString();
        }

        public static string GetBaiduSitemap(string sbforumlist, IUser master)
        {
            var config = GeneralConfigInfo.Current;

            var sb = new StringBuilder("<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n");
            //IDataReader sitemapNewTopics = DatabaseProvider.GetInstance().GetSitemapNewTopics(sbforumlist.ToString());
            var sitemapNewTopics = Topic.GetSitemapNewTopics(sbforumlist);
            sb.Append("<document xmlns:bbs=\"http://www.baidu.com/search/bbs_sitemap.xsd\">\r\n");
            sb.AppendFormat("  <webSite>{0}</webSite>\r\n", forumurl);
            sb.AppendFormat("  <webMaster>{0}</webMaster>\r\n", (master != null) ? master.Email : "");
            sb.AppendFormat("  <updatePeri>{0}</updatePeri>\r\n", config.Sitemapttl);
            sb.AppendFormat("  <updatetime>{0}</updatetime>\r\n", DateTime.Now.ToString("r"));
            sb.AppendFormat("  <version>{0} {1}</version>\r\n", Utils.ProductName, Utils.Version);
            if (sitemapNewTopics != null)
            {
                foreach (var tp in sitemapNewTopics)
                {
                    sb.Append("    <item>\r\n");
                    sb.AppendFormat("      <link>{0}", Utils.HtmlEncode(forumurl));
                    if (config.Aspxrewrite == 1)
                    {
                        sb.AppendFormat("showtopic-{0}{1}", tp.ID, config.Extname);
                    }
                    else
                    {
                        sb.AppendFormat("showtopic-{0}", tp.ID);
                    }
                    sb.Append("      </link>\r\n");
                    sb.AppendFormat("      <title>{0}</title>\r\n", Utils.HtmlEncode(tp.Title));
                    sb.AppendFormat("      <pubDate>{0}</pubDate>\r\n", Utils.HtmlEncode(tp.PostDateTime.ToFullString()));
                    sb.AppendFormat("      <bbs:lastDate>{0}</bbs:lastDate>\r\n", tp.LastPost);
                    sb.AppendFormat("      <bbs:reply>{0}</bbs:reply>\r\n", tp.Replies);
                    sb.AppendFormat("      <bbs:hit>{0}</bbs:hit>\r\n", tp.Views);
                    sb.AppendFormat("      <bbs:boardid>{0}</bbs:boardid>\r\n", tp.Fid);
                    sb.AppendFormat("      <bbs:pick>{0}</bbs:pick>\r\n", tp.Digest);
                    sb.Append("    </item>\r\n");
                }
                //sitemapNewTopics.Close();
                sb.Append("</document>");
            }
            else
            {
                sb.Length = 0;
                sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n");
                sb.Append("<document>Error</document>\r\n");
            }
            return sb.ToString();
        }
    }
}