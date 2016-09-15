using System;
using System.Data;
using BBX.Common;
using BBX.Data;
using BBX.Entity;

namespace BBX.Forum
{
    public class AdminRateLogs
    {
		//public static int InsertLog(string postidlist, int userid, string username, int extid, float score, string reason)
		//{
		//	int num = 0;
		//	string[] array = Utils.SplitString(postidlist, ",");
		//	for (int i = 0; i < array.Length; i++)
		//	{
		//		string expression = array[i];
		//		num += RateLogs.CreateRateLog(Utils.StrToInt(expression, 0), userid, username, extid, score, reason);
		//	}
		//	return num;
		//}

		//public static bool DeleteLog()
		//{
		//	return RateLogs.DeleteRateLog();
		//}

		//public static bool DeleteLog(string condition)
		//{
		//	return RateLogs.DeleteRateLog(condition);
		//}

        public static DataTable GetRateLogList(int pagesize, int currentpage)
        {
            return RateLogs.GetRateLogList(pagesize, currentpage, TableList.CurrentTableName);
        }

		//public static DataTable LogList(int pagesize, int currentpage, string condition)
		//{
		//	return RateLogs.GetRateLogList(pagesize, currentpage, TableList.CurrentTableName, condition);
		//}

		//public static int RecordCount()
		//{
		//	return RateLogs.GetRateLogCount();
		//}

		//public static int RecordCount(string condition)
		//{
		//	return RateLogs.GetRateLogCount(condition);
		//}

		//public static string GetSearchRateLogCondition(DateTime postDateTimeStart, DateTime postDateTimeEnd, string userName, string others)
		//{
		//	return RateLogs.GetSearchRateLogCondition(postDateTimeStart, postDateTimeEnd, userName, others);
		//}

		//public static string GetRateLogCountCondition(int userid, string postidlist)
		//{
		//	return RateLogs.GetRateLogCountCondition(userid, postidlist);
		//}
    }
}