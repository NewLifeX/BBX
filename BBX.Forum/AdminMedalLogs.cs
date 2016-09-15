using System;
using System.Data;

namespace BBX.Forum
{
    public class AdminMedalLogs
    {
        public static bool DeleteLog(string condition)
        {
            return BBX.Data.Medals.DeleteLog(condition);
        }

        public static DataTable LogList(int pagesize, int currentpage)
        {
            return BBX.Data.Medals.LogList(pagesize, currentpage);
        }

        public static DataTable LogList(int pagesize, int currentpage, string condition)
        {
            return BBX.Data.Medals.LogList(pagesize, currentpage, condition);
        }

        public static int RecordCount()
        {
            return BBX.Data.Medals.RecordCount();
        }

        public static int RecordCount(string condition)
        {
            return BBX.Data.Medals.RecordCount(condition);
        }

        public static string GetDelMedalLogCondition(string deleteMode, string id, string deleteNum, string deleteFrom)
        {
            return BBX.Data.Medals.GetDelMedalLogCondition(deleteMode, id, deleteNum, deleteFrom);
        }

        public static string GetSearchMedalLogCondition(DateTime postDateTimeStart, DateTime postDateTimeEnd, string userName, string reason)
        {
            return BBX.Data.Medals.GetSearchMedalLogCondition(postDateTimeStart, postDateTimeEnd, userName, reason);
        }
    }
}