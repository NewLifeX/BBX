using System;
using Discuz.Cache;
using Discuz.Common;

namespace Discuz.Forum
{
    public class Statistics
    {
        //public static DataRow GetStatisticsRow()
        //{
        //    var cacheService = XCache.Current;
        //    DataTable dataTable = cacheService.RetrieveObject(CacheKeys.FORUM_STATISTICS) as DataTable;
        //    if (dataTable == null)
        //    {
        //        dataTable = Discuz.Data.Statistics.GetStatisticsRow();
        //        XCache.Add(CacheKeys.FORUM_STATISTICS, dataTable);
        //    }
        //    return dataTable.Rows[0];
        //}
        //public static void GetPostCountFromForum(int fid, out int topiccount, out int postcount, out int todaypostcount)
        //{
        //    if (fid == 0)
        //    {
        //        Discuz.Data.Statistics.GetAllForumStatistics(out topiccount, out postcount, out todaypostcount);
        //        return;
        //    }
        //    Discuz.Data.Statistics.GetForumStatistics(fid, out topiccount, out postcount, out todaypostcount);
        //}
        //public static string GetStatisticsRowItem(string param)
        //{
        //    return Statistics.GetStatisticsRow()[param].ToString();
        //}
        public static string GetStatisticsSearchtime()
        {
            var cacheService = XCache.Current;
            string text = cacheService.RetrieveObject(CacheKeys.FORUM_STATISTICS_SEARCHTIME) as string;
            if (text == null)
            {
                text = DateTime.Now.ToString();
                XCache.Add(CacheKeys.FORUM_STATISTICS_SEARCHTIME, text);
            }
            return text;
        }
        public static int GetStatisticsSearchcount()
        {
            var cacheService = XCache.Current;
            int num = Utils.StrToInt(cacheService.RetrieveObject(CacheKeys.FORUM_STATISTICS_SEARCHCOUNT), 0);
            if (num == 0)
            {
                num = 1;
                XCache.Add(CacheKeys.FORUM_STATISTICS_SEARCHCOUNT, num);
            }
            return num;
        }
        public static void SetStatisticsSearchtime(string searchtime)
        {
            XCache.Remove(CacheKeys.FORUM_STATISTICS_SEARCHTIME);
            XCache.Add(CacheKeys.FORUM_STATISTICS_SEARCHTIME, searchtime);
        }
        public static void SetStatisticsSearchcount(int searchcount)
        {
            XCache.Remove(CacheKeys.FORUM_STATISTICS_SEARCHCOUNT);
            XCache.Add(CacheKeys.FORUM_STATISTICS_SEARCHCOUNT, searchcount);
        }
        //public static int UpdateStatistics(string param, string strValue)
        //{
        //    return Discuz.Data.Statistics.UpdateStatistics(param, strValue);
        //}
        public static bool CheckSearchCount(int maxspm)
        {
            if (maxspm == 0) return true;

            int statisticsSearchcount = GetStatisticsSearchcount();
            if (Utils.StrDateDiffSeconds(GetStatisticsSearchtime(), 60) > 0)
            {
                SetStatisticsSearchtime(DateTime.Now.ToString());
                SetStatisticsSearchcount(1);
            }
            if (statisticsSearchcount > maxspm) return false;

            SetStatisticsSearchcount(statisticsSearchcount + 1);
            return true;
        }
        //public static void ReSetStatisticsCache()
        //{
        //    XCache.Remove(CacheKeys.FORUM_STATISTICS);
        //    XCache.Add(CacheKeys.FORUM_STATISTICS, Discuz.Data.Statistics.GetStatisticsRow());
        //}

        //public static void UpdateStatisticsLastUserName(int uid, string newUserName)
        //{
        //    Data.Statistics.UpdateStatisticsLastUserName(uid, newUserName);
        //}
    }
}
