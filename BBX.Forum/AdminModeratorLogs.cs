using System;
using System.Data;
using Discuz.Data;

namespace Discuz.Forum
{
    public class AdminModeratorLogs
    {
        public static bool InsertLog(string moderatoruid, string moderatorname, string groupid, string grouptitle, string ip, string postdatetime, string fid, string fname, string tid, string title, string actions, string reason)
        {
            return ModeratorManageLog.InsertLog(moderatoruid, moderatorname, groupid, grouptitle, ip, postdatetime, fid, fname, tid, title, actions, reason);
        }

        public static bool DeleteLog(string condition)
        {
            return ModeratorManageLog.DeleteLog(condition);
        }

        public static string SearchModeratorManageLog(string keyword)
        {
            return ModeratorManageLog.SearchModeratorManageLog(keyword);
        }

        public static DataTable LogList(int pagesize, int currentpage)
        {
            return AdminModeratorLogs.LogList(pagesize, currentpage, "");
        }

        public static DataTable LogList(int pagesize, int currentpage, string condition)
        {
            return ModeratorManageLog.GetModeratorLogList(pagesize, currentpage, condition);
        }

        public static int RecordCount()
        {
            return ModeratorManageLog.RecordCount();
        }

        public static int RecordCount(string condition)
        {
            return ModeratorManageLog.RecordCount(condition);
        }

        public static string GetDeleteModeratorManageCondition(string deleteMode, string id, string deleteNum, string deleteFrom)
        {
            return ModeratorManageLog.GetDeleteModeratorManageCondition(deleteMode, id, deleteNum, deleteFrom);
        }

        public static string GetSearchModeratorManageLogCondition(DateTime postDateTimeStart, DateTime postDateTimeEnd, string userName, string others)
        {
            return ModeratorManageLog.GetSearchModeratorManageLogCondition(postDateTimeStart, postDateTimeEnd, userName, others);
        }
    }
}