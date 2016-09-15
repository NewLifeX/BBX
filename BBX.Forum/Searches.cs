using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using Discuz.Common;
using Discuz.Entity;

namespace Discuz.Forum
{
    public class Searches
    {
        private static Regex regexSpacePost = new Regex("<SpacePosts>([\\s\\S]+?)</SpacePosts>");
        private static Regex regexAlbum = new Regex("<Albums>([\\s\\S]+?)</Albums>");
        private static Regex regexForumTopics = new Regex("<ForumTopics>([\\s\\S]+?)</ForumTopics>");
        public static int Search(int posttableid, int userid, int usergroupid, string keyword, int posterid, SearchType searchType, string searchforumid, int searchtime, int searchtimetype, int resultorder, int resultordertype)
        {
            bool spaceenabled = false;
            bool albumenable = false;
            if (posttableid == 0)
            {
                posttableid = TypeConverter.StrToInt(TableList.GetPostTableId(), 1);
            }
            //if (GeneralConfigInfo.Current.Enablespace == 1 && SpacePluginProvider.GetInstance() != null)
            //{
            //    spaceenabled = true;
            //}
            //if (GeneralConfigInfo.Current.Enablealbum == 1 && AlbumPluginProvider.GetInstance() != null)
            //{
            //    albumenable = true;
            //}
            return Discuz.Data.Searches.Search(posttableid, userid, usergroupid, keyword, posterid, searchType, searchforumid, searchtime, searchtimetype, resultorder, resultordertype);
        }

        public static DataTable GetSearchCacheList(int posttableid, int searchid, int pagesize, int pageindex, out int topiccount, SearchType searchType)
        {
            topiccount = 0;
            //DataTable searchCache = Discuz.Data.Searches.GetSearchCache(searchid);
            var searchCache = SearchCache.FindByID(searchid);
            if (searchCache == null) return new DataTable();

            //string input = searchCache.Rows[0][0].ToString();
            var input = searchCache.Keywords;
            switch (searchType)
            {
                //case SearchType.AlbumTitle:
                //{
                //    Match match = Searches.regexAlbum.Match(input);
                //    if (match.Success)
                //    {
                //        string currentPageTids = Searches.GetCurrentPageTids(match.Groups[1].Value, out topiccount, pagesize, pageindex);
                //        if (Utils.StrIsNullOrEmpty(currentPageTids))
                //        {
                //            return new DataTable();
                //        }
                //        //if (AlbumPluginProvider.GetInstance() != null)
                //        //{
                //        //    return AlbumPluginProvider.GetInstance().GetResult(pagesize, currentPageTids);
                //        //}
                //        return new DataTable();
                //    }
                //    break;
                //}
                //case SearchType.SpacePostTitle:
                //{
                //    Match match = Searches.regexSpacePost.Match(input);
                //    if (match.Success)
                //    {
                //        string currentPageTids2 = Searches.GetCurrentPageTids(match.Groups[1].Value, out topiccount, pagesize, pageindex);
                //        if (Utils.StrIsNullOrEmpty(currentPageTids2))
                //        {
                //            return new DataTable();
                //        }
                //        //if (SpacePluginProvider.GetInstance() != null)
                //        //{
                //        //    return SpacePluginProvider.GetInstance().GetResult(pagesize, currentPageTids2);
                //        //}
                //        return new DataTable();
                //    }
                //    break;
                //}
                default:
                    {
                        var match = Searches.regexForumTopics.Match(input);
                        if (match.Success)
                        {
                            string currentPageTids3 = Searches.GetCurrentPageTids(match.Groups[1].Value, out topiccount, pagesize, pageindex);
                            if (Utils.StrIsNullOrEmpty(currentPageTids3)) return new DataTable();

                            if (searchType == SearchType.DigestTopic)
                                return Discuz.Data.Searches.GetSearchDigestTopicsList(pagesize, currentPageTids3);

                            return Discuz.Data.Searches.GetSearchTopicsList(pagesize, currentPageTids3);
                        }
                        break;
                    }
            }
            return new DataTable();
        }

        private static string GetCurrentPageTids(string tids, out int topiccount, int pagesize, int pageindex)
        {
            var arr = Utils.SplitString(tids, ",");
            topiccount = arr.Length;
            int page = (topiccount % pagesize == 0) ? (topiccount / pagesize) : (topiccount / pagesize + 1);
            if (page < 1) page = 1;

            if (pageindex > page) pageindex = page;

            int start = pagesize * (pageindex - 1);
            var sb = new StringBuilder();
            int k = start;
            while (k < topiccount && k <= start + pagesize)
            {
                sb.Append(arr[k]);
                sb.Append(",");
                k++;
            }
            return sb.Remove(sb.Length - 1, 1).ToString();
        }
    }
}