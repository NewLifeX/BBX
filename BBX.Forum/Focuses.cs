using System;
using BBX.Cache;
using BBX.Entity;
using XCode;

namespace BBX.Forum
{
    public class Focuses
    {
		//public static EntityList<Topic> GetDigestTopicList(int count)
		//{
		//	return Focuses.GetTopicList(count, -1, 0, "", TopicTimeType.All, TopicOrderType.ID, true, 20, false, "");
		//}
		//public static EntityList<Topic> GetDigestTopicList(int count, int fid, TopicTimeType timetype, TopicOrderType ordertype)
		//{
		//	return Focuses.GetTopicList(count, -1, fid, "", timetype, ordertype, true, 20, false, "");
		//}
		//public static EntityList<Topic> GetHotTopicList(int count, int views)
		//{
		//	return Focuses.GetTopicList(count, views, 0, "", TopicTimeType.All, TopicOrderType.ID, false, 20, false, "");
		//}
		//public static EntityList<Topic> GetHotTopicList(int count, int views, int fid, TopicTimeType timetype, TopicOrderType ordertype)
		//{
		//	return Focuses.GetTopicList(count, views, fid, "", timetype, ordertype, false, 20, false, "");
		//}
		//public static EntityList<Topic> GetRecentTopicList(int count)
		//{
		//	return Focuses.GetTopicList(count, -1, 0, "", TopicTimeType.All, TopicOrderType.ID, false, 20, false, "");
		//}
		//public static EntityList<Topic> GetTopicList(int count, int views, int fid, string typeIdList, TopicTimeType timetype, TopicOrderType ordertype, bool isdigest, int cachetime, bool onlyimg)
		//{
		//	return Focuses.GetTopicList(count, views, fid, typeIdList, timetype, ordertype, isdigest, cachetime, onlyimg, "");
		//}

        public static EntityList<Topic> GetTopicList(int count, int views, int fid, string typeIdList, TopicTimeType timetype, Int32 ordertype, bool isdigest, int cachetime, bool onlyimg, string fidlist)
        {
            if (cachetime == 0) cachetime = 1;
            if (count > 50) count = 50;
            if (count < 1) count = 1;
            var order = Topic._FocusOrder[ordertype];

            string xpath = string.Format("/Forum/TopicList-{0}-{1}-{2}-{3}-{4}-{5}-{6}-{7}", count, views, fid, timetype, order, isdigest, onlyimg, typeIdList.Replace(',', 'd'));
            var cacheService = XCache.Current;

            var dataTable = cacheService.RetrieveObject(xpath) as EntityList<Topic>;
            if (dataTable == null)
            {
                if (String.IsNullOrEmpty(fidlist)) fidlist = Forums.GetVisibleForum();

                dataTable = Topic.GetFocusTopicList(count, views, fid, typeIdList, Focuses.GetStartDate(timetype), order, fidlist, isdigest, onlyimg);
                XCache.Add(xpath, dataTable, cachetime * 60);
            }
            return dataTable;
        }

        //public static DataTable GetUpdatedSpaces(int count, int cachetime)
        //{
        //    if (cachetime == 0)
        //    {
        //        cachetime = 1;
        //    }
        //    if (count > 50)
        //    {
        //        count = 50;
        //    }
        //    if (count < 1)
        //    {
        //        count = 1;
        //    }
        //    string xpath = "/Space/UpdatedSpace-" + count.ToString();
        //    var cacheService = DNTCache.Current;
        //    DataTable dataTable = cacheService.RetrieveObject(xpath) as DataTable;
        //    if (dataTable == null)
        //    {
        //        dataTable = SpacePluginProvider.GetInstance().GetWebSiteAggRecentUpdateSpaceList(count);
        //        XCache.Add(xpath, dataTable, cachetime * 60);
        //    }
        //    return dataTable;
        //}
        //public static DataTable GetNewSpacePosts(int count, int cachetime)
        //{
        //    if (cachetime == 0)
        //    {
        //        cachetime = 1;
        //    }
        //    if (count > 50)
        //    {
        //        count = 50;
        //    }
        //    if (count < 1)
        //    {
        //        count = 1;
        //    }
        //    string xpath = "/Space/NewSpacePosts-" + count.ToString();
        //    var cacheService = DNTCache.Current;
        //    DataTable dataTable = cacheService.RetrieveObject(xpath) as DataTable;
        //    if (dataTable == null)
        //    {
        //        dataTable = SpacePluginProvider.GetInstance().GetWebSiteAggSpacePostList(count);
        //        XCache.Add(xpath, dataTable, cachetime * 60);
        //    }
        //    return dataTable;
        //}
        public static DateTime GetStartDate(TopicTimeType type)
        {
            var now = DateTime.Now.Date;
            switch (type)
            {
                case TopicTimeType.Day:
                    return now.AddDays(-1.0);
                case TopicTimeType.ThreeDays:
                    return now.AddDays(-3.0);
                case TopicTimeType.FiveDays:
                    return now.AddDays(-5.0);
                case TopicTimeType.Week:
                    return now.AddDays(-7.0);
                case TopicTimeType.Month:
                    return now.AddDays(-30.0);
                case TopicTimeType.SixMonth:
                    return now.AddMonths(-6);
                case TopicTimeType.Year:
                    return now.AddYears(-1);
                default:
                    return new DateTime(1754, 1, 1);
            }
        }
        //public static string GetFieldName(TopicOrderType type)
        //{
        //    switch (type)
        //    {
        //        case TopicOrderType.Views:
        //            return "views";
        //        case TopicOrderType.LastPost:
        //            return "lastpost";
        //        case TopicOrderType.PostDateTime:
        //            return "postdatetime";
        //        case TopicOrderType.Digest:
        //            return "digest";
        //        case TopicOrderType.Replies:
        //            return "replies";
        //        case TopicOrderType.Rate:
        //            return "rate";
        //        default:
        //            return "id";
        //    }
        //}
    }
}