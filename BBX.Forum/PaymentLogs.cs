using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Discuz.Common;
using Discuz.Data;
using Discuz.Entity;

namespace Discuz.Forum
{
    public class PaymentLogs
    {
        public static int BuyTopic(int uid, int tid, int posterid, int price, float netamount)
        {
            if (price > Scoresets.GetMaxIncPerTopic()) Scoresets.GetMaxIncPerTopic();

            var user = User.FindByID(uid);
            if (user == null) return -2;

            if (PaymentLogs.GetUserExtCredits(user, Scoresets.GetTopicAttachCreditsTrans()) < (float)price) return -1;

            Discuz.Data.Users.BuyTopic(uid, tid, posterid, price, netamount, Scoresets.GetTopicAttachCreditsTrans());
            CreditsFacade.UpdateUserCredits(uid);
            CreditsFacade.UpdateUserCredits(posterid);
            return Discuz.Data.PaymentLogs.CreatePaymentLog(uid, tid, posterid, price, netamount);
        }
        private static float GetUserExtCredits(IUser userInfo, int extCreditsId)
        {
            switch (extCreditsId)
            {
                case 1:
                    return userInfo.ExtCredits1;
                case 2:
                    return userInfo.ExtCredits2;
                case 3:
                    return userInfo.ExtCredits3;
                case 4:
                    return userInfo.ExtCredits4;
                case 5:
                    return userInfo.ExtCredits5;
                case 6:
                    return userInfo.ExtCredits6;
                case 7:
                    return userInfo.ExtCredits7;
                case 8:
                    return userInfo.ExtCredits8;
                default:
                    return 0f;
            }
        }
        public static bool IsBuyer(int tid, int uid)
        {
            return Discuz.Data.PaymentLogs.IsBuyer(tid, uid);
        }
        public static DataTable GetPayLogInList(int pagesize, int currentpage, int uid)
        {
            return PaymentLogs.LoadPayLogForumName(Discuz.Data.PaymentLogs.GetPayLogInList(pagesize, currentpage, uid));
        }
        public static int GetPaymentLogInRecordCount(int uid)
        {
            return Discuz.Data.PaymentLogs.GetPaymentLogInRecordCount(uid);
        }
        public static DataTable GetPayLogOutList(int pagesize, int currentpage, int uid)
        {
            return PaymentLogs.LoadPayLogForumName(DatabaseProvider.GetInstance().GetPayLogOutList(pagesize, currentpage, uid));
        }
        private static DataTable LoadPayLogForumName(DataTable dt)
        {
            if (dt != null)
            {
                DataColumn dataColumn = new DataColumn("forumname", typeof(String));
                dataColumn.DefaultValue = "";
                dataColumn.AllowDBNull = false;
                dt.Columns.Add(dataColumn);

                foreach (DataRow dr in dt.Rows)
                {
                    var fid = TypeConverter.ObjectToInt(dr["fid"].ToString().Trim());

                    var fm = XForum.FindByID(fid);
                    if (fm != null) dr["forumname"] = fm.Name;
                }

                //var forumList = Discuz.Data.Forums.GetForumList();
                //IEnumerator enumerator = dt.Rows.GetEnumerator();
                //try
                //{
                //    DataRow dr;
                //    while (enumerator.MoveNext())
                //    {
                //        dr = (DataRow)enumerator.Current;
                //        if (!Utils.StrIsNullOrEmpty(dr["fid"].ToString().Trim()))
                //        {
                //            //Predicate<IXForum> match = (IXForum forumInfo) => forumInfo.ParentID == TypeConverter.ObjectToInt(dr["fid"]);
                //            //using (List<ForumInfo>.Enumerator enumerator2 = forumList.FindAll(match).GetEnumerator())
                //            //{
                //            //    if (enumerator2.MoveNext())
                //            //    {
                //            //        ForumInfo current = enumerator2.Current;
                //            //        dr["forumname"] = current.Name;
                //            //    }
                //            //}
                //            var fid = TypeConverter.ObjectToInt(dr["fid"].ToString().Trim());
                //            foreach (var item in forumList.FindAll(f => f.ParentID == fid))
                //            {
                //                dr["forumname"] = item.Name;
                //                break;
                //            }
                //        }
                //    }
                //}
                //finally
                //{
                //    IDisposable disposable = enumerator as IDisposable;
                //    if (disposable != null)
                //    {
                //        disposable.Dispose();
                //    }
                //}
            }
            return dt;
        }
        public static int GetPaymentLogOutRecordCount(int uid)
        {
            if (uid <= 0)
            {
                return 0;
            }
            return Discuz.Data.PaymentLogs.GetPaymentLogOutRecordCount(uid);
        }
        public static DataTable GetPaymentLogByTid(int pagesize, int currentpage, int tid)
        {
            if (tid <= 0 || currentpage <= 0)
            {
                return new DataTable();
            }
            return Discuz.Data.PaymentLogs.GetPaymentLogByTid(pagesize, currentpage, tid);
        }
        public static int GetPaymentLogByTidCount(int tid)
        {
            if (tid <= 0)
            {
                return 0;
            }
            return Discuz.Data.PaymentLogs.GetPaymentLogByTidCount(tid);
        }
    }
}
