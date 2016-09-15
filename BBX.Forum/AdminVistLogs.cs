using System;
using System.Data;
using Discuz.Data;
using Discuz.Common;
using Discuz.Config;

namespace Discuz.Forum
{
    public class AdminVistLogs
    {
        public static bool InsertLog(int uid, string userName, int groupId, string groupTitle, string ip, string actions, string others)
        {
            bool result;
            try
            {
                AdminVisitLogs.InsertLog(uid, userName, groupId, groupTitle, ip, actions, others);
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }

        //public static bool DeleteLog()
        //{
        //    bool result;
        //    try
        //    {
        //        AdminVisitLogs.DeleteLog();
        //        result = true;
        //    }
        //    catch
        //    {
        //        result = false;
        //    }
        //    return result;
        //}

        //public static bool DeleteLog(string condition)
        //{
        //    bool result;
        //    try
        //    {
        //        AdminVisitLogs.DeleteLog(condition);
        //        result = true;
        //    }
        //    catch
        //    {
        //        result = false;
        //    }
        //    return result;
        //}

        public static DataTable LogList(int pageSize, int currentPage)
        {
            return AdminVisitLogs.LogList(pageSize, currentPage);
        }

        public static DataTable LogList(int pageSize, int currentPage, string condition)
        {
            return AdminVisitLogs.LogList(pageSize, currentPage, condition);
        }

        //public static int RecordCount()
        //{
        //    return AdminVisitLogs.RecordCount();
        //}

        //public static int RecordCount(string condition)
        //{
        //    return AdminVisitLogs.RecordCount(condition);
        //}

        //public static string DelVisitLogCondition(string deleteMod, string visitId, string deleteNum, string deleteFrom)
        //{
        //    string result = null;
        //    if (deleteMod != null)
        //    {
        //        if (!(deleteMod == "chkall"))
        //        {
        //            if (!(deleteMod == "deleteNum"))
        //            {
        //                if (deleteMod == "deleteFrom")
        //                {
        //                    if (deleteFrom != "")
        //                    {
        //                        result = " [postdatetime]<'" + deleteFrom + "'";
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                if (deleteNum != "" && Utils.IsNumeric(deleteNum))
        //                {
        //                    result = string.Format(" [visitid] NOT IN (SELECT TOP {0} [visitid] FROM [{1}adminvisitlog] ORDER BY [visitid] DESC)", deleteNum, BaseConfigInfo.Current.Tableprefix);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            if (visitId != "")
        //            {
        //                result = string.Format(" [visitid] IN ({0})", visitId);
        //            }
        //        }
        //    }
        //    return result;
        //}

        //public static string GetVisitLogCondition(DateTime postDateTimeStart, DateTime postDateTimeEnd, string userName, string others)
        //{
        //    return AdminVisitLogs.GetVisitLogCondition(postDateTimeStart, postDateTimeEnd, userName, others);
        //}
    }
}