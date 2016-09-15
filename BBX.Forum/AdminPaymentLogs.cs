using System;
using System.Data;
using Discuz.Common;

namespace Discuz.Forum
{
    public class AdminPaymentLogs
    {
        public static bool DeleteLog()
        {
            return Discuz.Data.PaymentLogs.DeleteLog();
        }

        public static bool DeleteLog(string condition)
        {
            return Discuz.Data.PaymentLogs.DeleteLog(condition);
        }

        public static DataTable LogList(int pagesize, int currentpage)
        {
            DataTable paymentLogList = Discuz.Data.PaymentLogs.GetPaymentLogList(pagesize, currentpage);
            if (paymentLogList != null)
            {
                DataColumn dataColumn = new DataColumn();
                dataColumn.ColumnName = "forumname";
                dataColumn.DataType = typeof(String);
                dataColumn.DefaultValue = "";
                dataColumn.AllowDBNull = false;
                paymentLogList.Columns.Add(dataColumn);
                DataTable forumListForDataTable = Forums.GetForumListForDataTable();
                foreach (DataRow dataRow in paymentLogList.Rows)
                {
                    if (dataRow["fid"].ToString().Trim() != "")
                    {
                        DataRow[] array = forumListForDataTable.Select("fid=" + dataRow["fid"].ToString());
                        int num = 0;
                        if (num < array.Length)
                        {
                            DataRow dataRow2 = array[num];
                            dataRow["forumname"] = dataRow2["name"].ToString();
                        }
                    }
                }
            }
            return paymentLogList;
        }

        public static DataTable LogList(int pagesize, int currentpage, string condition)
        {
            DataTable paymentLogList = Discuz.Data.PaymentLogs.GetPaymentLogList(pagesize, currentpage, condition);
            if (paymentLogList != null)
            {
                DataColumn dataColumn = new DataColumn();
                dataColumn.ColumnName = "forumname";
                dataColumn.DataType = typeof(String);
                dataColumn.DefaultValue = "";
                dataColumn.AllowDBNull = false;
                paymentLogList.Columns.Add(dataColumn);
                DataTable forumListForDataTable = Forums.GetForumListForDataTable();
                foreach (DataRow dataRow in paymentLogList.Rows)
                {
                    if (dataRow["fid"].ToString().Trim() != "")
                    {
                        DataRow[] array = forumListForDataTable.Select("fid=" + dataRow["fid"].ToString());
                        int num = 0;
                        if (num < array.Length)
                        {
                            DataRow dataRow2 = array[num];
                            dataRow["forumname"] = dataRow2["name"].ToString();
                        }
                    }
                }
            }
            return paymentLogList;
        }

        public static int RecordCount(string condition)
        {
            if (!Utils.StrIsNullOrEmpty(condition))
            {
                return Discuz.Data.PaymentLogs.RecordCount(condition);
            }
            return Discuz.Data.PaymentLogs.RecordCount();
        }

        public static string GetSearchPaymentLogCondition(DateTime postDateTimeStart, DateTime postDateTimeEnd, string userName)
        {
            return Discuz.Data.PaymentLogs.GetSearchPaymentLogCondition(postDateTimeStart, postDateTimeEnd, userName);
        }
    }
}