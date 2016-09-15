﻿using System;
using System.ComponentModel;
using System.Data;
using BBX.Common;
using XCode;
using System.Collections.Generic;

namespace BBX.Entity
{
    /// <summary>支付日志</summary>
    public partial class PaymentLog : EntityBase<PaymentLog>
    {
        #region 对象操作﻿

        /// <summary>验证数据，通过抛出异常的方式提示验证失败。</summary>
        /// <param name="isNew"></param>
        public override void Valid(Boolean isNew)
        {
            // 这里验证参数范围，建议抛出参数异常，指定参数名，前端用户界面可以捕获参数异常并聚焦到对应的参数输入框
            //if (String.IsNullOrEmpty(Name)) throw new ArgumentNullException(_.Name, _.Name.DisplayName + "无效！");
            //if (!isNew && ID < 1) throw new ArgumentOutOfRangeException(_.ID, _.ID.DisplayName + "必须大于0！");

            // 建议先调用基类方法，基类方法会对唯一索引的数据进行验证
            base.Valid(isNew);

            // 在新插入数据或者修改了指定字段时进行唯一性验证，CheckExist内部抛出参数异常
            //if (isNew || Dirtys[_.Name]) CheckExist(_.Name);

        }

        ///// <summary>首次连接数据库时初始化数据，仅用于实体类重载，用户不应该调用该方法</summary>
        //[EditorBrowsable(EditorBrowsableState.Never)]
        //protected override void InitData()
        //{
        //    base.InitData();

        //    // InitData一般用于当数据表没有数据时添加一些默认数据，该实体类的任何第一次数据库操作都会触发该方法，默认异步调用
        //    // Meta.Count是快速取得表记录数
        //    if (Meta.Count > 0) return;

        //    // 需要注意的是，如果该方法调用了其它实体类的首次数据库操作，目标实体类的数据初始化将会在同一个线程完成
        //    if (XTrace.Debug) XTrace.WriteLine("开始初始化{0}支付日志数据……", typeof(PaymentLog).Name);

        //    var entity = new PaymentLog();
        //    entity.Uid = 0;
        //    entity.Tid = 0;
        //    entity.AuthorID = 0;
        //    entity.BuyDate = DateTime.Now;
        //    entity.Amount = 0;
        //    entity.Netamount = 0;
        //    entity.Insert();

        //    if (XTrace.Debug) XTrace.WriteLine("完成初始化{0}支付日志数据！", typeof(PaymentLog).Name);
        //}


        ///// <summary>已重载。基类先调用Valid(true)验证数据，然后在事务保护内调用OnInsert</summary>
        ///// <returns></returns>
        //public override Int32 Insert()
        //{
        //    return base.Insert();
        //}

        ///// <summary>已重载。在事务保护范围内处理业务，位于Valid之后</summary>
        ///// <returns></returns>
        //protected override Int32 OnInsert()
        //{
        //    return base.OnInsert();
        //}
        #endregion

        #region 扩展属性﻿
        private List<String> hasLoad = new List<String>();
        private Topic _Topic;
        /// <summary>帖子</summary>
        public Topic Topic
        {
            get
            {
                if (_Topic == null && !hasLoad.Contains("Topic"))
                {
                    _Topic = Topic.FindByID(Tid);
                    hasLoad.Add("Topic");
                }
                return _Topic;
            }
            set { _Topic = value; }
        }

        public String Title { get { return Topic != null ? Topic.Title : null; } }
        public Int32 Fid { get { return Topic != null ? Topic.Fid : 0; } }
        public DateTime PostDateTime { get { return Topic != null ? Topic.PostDateTime : DateTime.MinValue; } }

        private XForum _Forum;
        /// <summary>论坛</summary>
        public XForum Forum
        {
            get
            {
                if (_Forum == null && Fid > 0 && !hasLoad.Contains("Forum"))
                {
                    _Forum = XForum.FindByID(Fid);
                    hasLoad.Add("Forum");
                }
                return _Forum;
            }
            set { _Forum = value; }
        }

        public String ForumName { get { return Forum != null ? Forum.Name : null; } }

        private User _Author;
        /// <summary>审核者</summary>
        public User Author
        {
            get
            {
                if (_Author == null && !hasLoad.Contains("Author"))
                {
                    _Author = User.FindByID(AuthorID);
                    hasLoad.Add("Author");
                }
                return _Author;
            }
            set { _Author = value; }
        }

        public String AuthorName { get { return Author != null ? Author.Name : null; } }

        private User _Poster;
        /// <summary>发帖子</summary>
        public User Poster
        {
            get
            {
                if (_Poster == null && !hasLoad.Contains("Poster"))
                {
                    _Poster = User.FindByID(Uid);
                    hasLoad.Add("Poster");
                }
                return _Poster;
            }
            set { _Poster = value; }
        }

        public String UserName { get { return Poster != null ? Poster.Name : null; } }
        #endregion

        #region 扩展查询﻿
        /// <summary>根据编号查找</summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static PaymentLog FindByID(Int32 id)
        {
            if (Meta.Count >= 1000)
                return Find(_.ID, id);
            else // 实体缓存
                return Meta.Cache.Entities.Find(__.ID, id);
            // 单对象缓存
            //return Meta.SingleCache[id];
        }
        #endregion

        #region 高级查询
        // 以下为自定义高级查询的例子

        ///// <summary>
        ///// 查询满足条件的记录集，分页、排序
        ///// </summary>
        ///// <param name="key">关键字</param>
        ///// <param name="orderClause">排序，不带Order By</param>
        ///// <param name="startRowIndex">开始行，0表示第一行</param>
        ///// <param name="maximumRows">最大返回行数，0表示所有行</param>
        ///// <returns>实体集</returns>
        //[DataObjectMethod(DataObjectMethodType.Select, true)]
        //public static EntityList<PaymentLog> Search(String key, String orderClause, Int32 startRowIndex, Int32 maximumRows)
        //{
        //    return FindAll(SearchWhere(key), orderClause, null, startRowIndex, maximumRows);
        //}

        ///// <summary>
        ///// 查询满足条件的记录总数，分页和排序无效，带参数是因为ObjectDataSource要求它跟Search统一
        ///// </summary>
        ///// <param name="key">关键字</param>
        ///// <param name="orderClause">排序，不带Order By</param>
        ///// <param name="startRowIndex">开始行，0表示第一行</param>
        ///// <param name="maximumRows">最大返回行数，0表示所有行</param>
        ///// <returns>记录数</returns>
        //public static Int32 SearchCount(String key, String orderClause, Int32 startRowIndex, Int32 maximumRows)
        //{
        //    return FindCount(SearchWhere(key), null, null, 0, 0);
        //}

        /// <summary>构造搜索条件</summary>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        private static String SearchWhere(String key)
        {
            // WhereExpression重载&和|运算符，作为And和Or的替代


            // SearchWhereByKeys系列方法用于构建针对字符串字段的模糊搜索
            var exp = SearchWhereByKeys(key);

            // 以下仅为演示，2、3行是同一个意思的不同写法，Field（继承自FieldItem）重载了==、!=、>、<、>=、<=等运算符（第4行）
            //exp &= _.Name == "testName"
            //    & !String.IsNullOrEmpty(key) & _.Name == key
            //    .AndIf(!String.IsNullOrEmpty(key), _.Name == key)
            //    | _.ID > 0;

            return exp;
        }
        #endregion

        #region 扩展操作
        #endregion

        #region 业务
        public static bool DeleteLog()
        {
            //return BBX.Data.PaymentLogs.DeleteLog();
            return Delete("") > 0;
        }

        public static bool DeleteLog(string condition)
        {
            //return BBX.Data.PaymentLogs.DeleteLog(condition);
            return Delete(condition) > 0;
        }

        //public static DataTable LogList(int pagesize, int currentpage)
        //{
        //    DataTable paymentLogList = BBX.Data.PaymentLogs.GetPaymentLogList(pagesize, currentpage);
        //    if (paymentLogList != null)
        //    {
        //        DataColumn dataColumn = new DataColumn();
        //        dataColumn.ColumnName = "forumname";
        //        dataColumn.DataType = typeof(String);
        //        dataColumn.DefaultValue = "";
        //        dataColumn.AllowDBNull = false;
        //        paymentLogList.Columns.Add(dataColumn);
        //        DataTable forumListForDataTable = Forums.GetForumListForDataTable();
        //        foreach (DataRow dataRow in paymentLogList.Rows)
        //        {
        //            if (dataRow["fid"].ToString().IsNullOrEmpty())
        //            {
        //                DataRow[] array = forumListForDataTable.Select("fid=" + dataRow["fid"].ToString());
        //                int num = 0;
        //                if (num < array.Length)
        //                {
        //                    DataRow dataRow2 = array[num];
        //                    dataRow["forumname"] = dataRow2["name"].ToString();
        //                }
        //            }
        //        }
        //    }
        //    return paymentLogList;
        //}

        public static EntityList<PaymentLog> LogList(int pagesize, int currentpage, string condition = null)
        {
            //DataTable paymentLogList = BBX.Data.PaymentLogs.GetPaymentLogList(pagesize, currentpage, condition);
            //if (paymentLogList != null)
            //{
            //    DataColumn dataColumn = new DataColumn();
            //    dataColumn.ColumnName = "forumname";
            //    dataColumn.DataType = typeof(String);
            //    dataColumn.DefaultValue = "";
            //    dataColumn.AllowDBNull = false;
            //    paymentLogList.Columns.Add(dataColumn);
            //    DataTable forumListForDataTable = Forums.GetForumListForDataTable();
            //    foreach (DataRow dataRow in paymentLogList.Rows)
            //    {
            //        if (dataRow["fid"].ToString().IsNullOrEmpty())
            //        {
            //            DataRow[] array = forumListForDataTable.Select("fid=" + dataRow["fid"].ToString());
            //            int num = 0;
            //            if (num < array.Length)
            //            {
            //                DataRow dataRow2 = array[num];
            //                dataRow["forumname"] = dataRow2["name"].ToString();
            //            }
            //        }
            //    }
            //}
            //return paymentLogList;

            return FindAll(condition, _.ID.Desc(), null, (currentpage - 1) * pagesize, pagesize);
        }

        public static int RecordCount(string condition)
        {
            //if (!Utils.StrIsNullOrEmpty(condition))
            //{
            //    return BBX.Data.PaymentLogs.RecordCount(condition);
            //}
            //return BBX.Data.PaymentLogs.RecordCount();

            return FindCount(condition, null, null, 0, 0);
        }

        public static string GetSearchPaymentLogCondition(DateTime postDateTimeStart, DateTime postDateTimeEnd, string userName)
        {
            //return BBX.Data.PaymentLogs.GetSearchPaymentLogCondition(postDateTimeStart, postDateTimeEnd, userName);

            var exp = _.BuyDate.Between(postDateTimeStart, postDateTimeEnd);
            if (!String.IsNullOrEmpty(userName))
            {
                //exp&=_.Uid.In(User.FindSQLWithKey(userName));
                var list = User.Search(userName);
                if (list.Count > 0)
                {
                    var arr = list.GetItem<Int32>(__.ID);
                    exp &= _.Uid.In(arr);
                }
            }

            return exp;
        }

        public static int BuyTopic(int uid, int tid, int posterid, int price, float netamount)
        {
            //if (price > Scoresets.GetMaxIncPerTopic()) Scoresets.GetMaxIncPerTopic();

            var user = User.FindByID(uid);
            if (user == null) return -2;

            //if (GetUserExtCredits(user, Scoresets.GetTopicAttachCreditsTrans()) < (float)price) return -1;

            //BBX.Data.Users.BuyTopic(uid, tid, posterid, price, netamount, Scoresets.GetTopicAttachCreditsTrans());
            //CreditsFacade.UpdateUserCredits(uid);
            //CreditsFacade.UpdateUserCredits(posterid);
            //return BBX.Data.PaymentLogs.CreatePaymentLog(uid, tid, posterid, price, netamount);

            var log = new PaymentLog();
            log.Uid = uid;
            log.Tid = tid;
            log.AuthorID = posterid;
            log.BuyDate = DateTime.Now;
            log.Amount = (short)price;
            log.Netamount = (short)netamount;
            log.Insert();

            throw new Exception("移植未完成！");
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
            //return BBX.Data.PaymentLogs.IsBuyer(tid, uid);

            return Find(_.Uid == uid & _.Tid == tid) != null;
        }

        public static EntityList<PaymentLog> GetPayLogInList(int pagesize, int currentpage, int uid)
        {
            //return PaymentLogs.LoadPayLogForumName(BBX.Data.PaymentLogs.GetPayLogInList(pagesize, currentpage, uid));
            return FindAll(_.AuthorID == uid, _.ID.Desc(), null, (currentpage - 1) * pagesize, pagesize);
        }

        public static int GetPaymentLogInRecordCount(int uid)
        {
            //return BBX.Data.PaymentLogs.GetPaymentLogInRecordCount(uid);
            return FindCount(_.AuthorID == uid, null, null, 0, 0);
        }

        public static EntityList<PaymentLog> GetPayLogOutList(int pagesize, int currentpage, int uid)
        {
            return FindAll(_.Uid == uid, _.ID.Desc(), null, (currentpage - 1) * pagesize, pagesize);
        }

        public static int GetPaymentLogOutRecordCount(int uid)
        {
            if (uid <= 0) return 0;

            return FindCount(_.Uid == uid, null, null, 0, 0);
        }

        public static EntityList<PaymentLog> GetPaymentLogByTid(int pagesize, int currentpage, int tid)
        {
            //if (tid <= 0 || currentpage <= 0)
            //{
            //    return new DataTable();
            //}
            //return BBX.Data.PaymentLogs.GetPaymentLogByTid(pagesize, currentpage, tid);

            return FindAll(_.Tid == tid, _.ID.Desc(), null, (currentpage - 1) * pagesize, pagesize);
        }

        public static int GetPaymentLogByTidCount(int tid)
        {
            if (tid <= 0) return 0;

            //return BBX.Data.PaymentLogs.GetPaymentLogByTidCount(tid);

            return FindCount(_.Tid == tid, null, null, 0, 0);
        }
        #endregion
    }
}