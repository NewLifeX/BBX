using System.Collections.Generic;
using System.Data;

using Discuz.Entity;

namespace Discuz.Plugin.Space
{
    public abstract class SpacePluginBase : PluginBase
    {
        public abstract string SpaceHotTagJSONPCacheFileName { get; }

        public abstract SpacePostInfo GetSpacepostsInfo(int blogid);

        public abstract void WriteHotTagsListForSpaceJSONPCacheFile(int count);

        public abstract void GetSpacePostTagsCacheFile(int postid);

        public abstract int CheckSpaceRewriteNameAvailable(string rewriteName);

        public abstract int GetSpacePostCountWithSameTag(int tagid);

        public abstract List<SpacePostInfo> GetSpacePostsWithSameTag(int tagid, int pageid, int tpp);

        public abstract DataTable GetWebSiteAggRecentUpdateSpaceList(int count);

        public abstract DataTable GetWebSiteAggTopSpaceList(string orderby, int topnumber);

        public abstract string[] GetSpaceLastPostInfo(int userid);

        public abstract DataTable GetWebSiteAggTopSpacePostList(string orderby, int topnumber);

        public abstract DataTable GetWebSiteAggSpacePostsList(int pageSize, int currentPage);

        public abstract int GetWebSiteAggSpacePostsCount();

        public abstract DataTable GetWebSiteAggSpaceTopComments(int topnumber);

        public abstract DataTable GetWebSiteAggSpacePostList(int count);
    }
}