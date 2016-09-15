﻿using System;
using System.ComponentModel;
using XCode;
using XCode.Membership;

namespace BBX.Entity
{
    /// <summary>等级日志</summary>
    public partial class RateLog : EntityBase<RateLog>
    {
        #region 对象操作﻿

        /// <summary>验证数据，通过抛出异常的方式提示验证失败。</summary>
        /// <param name="isNew"></param>
        public override void Valid(Boolean isNew)
        {
            // 这里验证参数范围，建议抛出参数异常，指定参数名，前端用户界面可以捕获参数异常并聚焦到对应的参数输入框
            //if (String.IsNullOrEmpty(Name)) throw new ArgumentNullException(_.Name, _.Name.DisplayName + "无效！");
            //if (!isNew && ID < 1) throw new ArgumentOutOfRangeException(_.ID, _.ID.DisplayName + "必须大于0！");

            if (!HasDirty) return;

            // 建议先调用基类方法，基类方法会对唯一索引的数据进行验证
            base.Valid(isNew);

            // 在新插入数据或者修改了指定字段时进行唯一性验证，CheckExist内部抛出参数异常
            //if (isNew || Dirtys[_.Name]) CheckExist(_.Name);

            // 处理当前已登录用户信息
            if (!Dirtys[_.UserName] && ManageProvider.Provider.Current != null) UserName = ManageProvider.Provider.Current.Name;

            if (isNew && !Dirtys[__.PostDateTime]) PostDateTime = DateTime.Now;
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
        //    if (XTrace.Debug) XTrace.WriteLine("开始初始化{0}等级日志数据……", typeof(RateLog).Name);

        //    var entity = new RateLog();
        //    entity.Pid = 0;
        //    entity.Uid = 0;
        //    entity.UserName = "abc";
        //    entity.ExtCrEdits = 0;
        //    entity.PostDateTime = "abc";
        //    entity.Score = 0;
        //    entity.Reason = "abc";
        //    entity.Insert();

        //    if (XTrace.Debug) XTrace.WriteLine("完成初始化{0}等级日志数据！", typeof(RateLog).Name);
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
        #endregion

        #region 扩展查询﻿
        /// <summary>根据编号查找</summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static RateLog FindByID(Int32 id)
        {
            if (Meta.Count >= 1000)
                return Find(_.ID, id);
            else // 实体缓存
                return Meta.Cache.Entities.Find(__.ID, id);
            // 单对象缓存
            //return Meta.SingleCache[id];
        }

        public static EntityList<RateLog> FindAllByIDs(String ids)
        {
            if (ids.IsNullOrWhiteSpace()) return new EntityList<RateLog>();

            return FindAll(_.ID.In(ids.SplitAsInt()), null, null, 0, 0);
        }

        public static EntityList<RateLog> FindAllByPostID(Int32 pid)
        {
            return FindAll(__.Pid, pid);
        }

        public static RateLog FindByPidAndUid(Int32 pid, Int32 uid)
        {
            return Find(_.Pid == pid & _.Uid == uid);
        }
        #endregion

        #region 高级查询
        // 以下为自定义高级查询的例子

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static EntityList<RateLog> Search(String username, DateTime start, DateTime end, String key, String orderClause, Int32 startRowIndex, Int32 maximumRows)
        {
            return FindAll(SearchWhere(username, start, end, key), orderClause, null, startRowIndex, maximumRows);
        }

        public static Int32 SearchCount(String username, DateTime start, DateTime end, String key, String orderClause = null, Int32 startRowIndex = 0, Int32 maximumRows = 0)
        {
            return FindCount(SearchWhere(username, start, end, key), null, null, 0, 0);
        }

        /// <summary>构造搜索条件</summary>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        private static String SearchWhere(String username, DateTime start, DateTime end, String key)
        {
            //var exp = SearchWhereByKeys(key);

            var exp = new WhereExpression();
            if (!String.IsNullOrEmpty(username)) exp &= _.UserName.Contains(username);
            if (start > DateTime.MinValue) exp &= _.PostDateTime >= start;
            if (end > DateTime.MinValue) exp &= _.PostDateTime < end.AddDays(1).Date;
            if (!String.IsNullOrEmpty(key)) exp &= _.Reason.Contains(key);

            return exp;
        }

        public static EntityList<RateLog> SearchByPid(Int32 pid, Int32 start, Int32 max)
        {
            return FindAll(_.Pid == pid, null, null, start, max);
        }

        public static EntityList<RateLog> Search(Int32 uid, Int32 pid, Int32 startRowIndex = 0, Int32 maximumRows = 0)
        {
            var exp = new WhereExpression();
            if (uid > 0) exp &= _.Uid == uid;
            if (pid > 0) exp &= _.Pid == pid;

            return FindAll(exp, null, null, startRowIndex, maximumRows);
        }

        public static Int32 SearchCount(Int32 uid, Int32 pid)
        {
            var exp = new WhereExpression();
            if (uid > 0) exp &= _.Uid == uid;
            if (pid > 0) exp &= _.Pid == pid;

            return FindCount(exp);
        }
        #endregion

        #region 扩展操作
        public static RateLog Create(int pid, int userid, string userName, int extid, Int32 score, string reason)
        {
            var entity = new RateLog();
            entity.Pid = pid;
            entity.Uid = userid;
            entity.UserName = userName;
            entity.ExtCredits = extid;
            entity.Score = score;
            entity.Reason = reason;
            entity.PostDateTime = DateTime.Now;

            return entity;
        }

        public static int UpdatePostRateTimes(string postidlist)
        {
            //var list = FindAll(_.Pid.In(postidlist.SplitAsInt(",")), null, null, 0, 0);
            var pids = postidlist.SplitAsInt(",");
            var rs = 0;
            foreach (var pid in pids)
            {
                // 有多少个用户给这个帖子评分
                var list = FindAll(_.Pid == pid, null, _.Uid.Count(), 0, 0);
                var count = list.Count > 0 ? list[0].Uid : 0;

                var pi = Post.FindByID(pid);
                pi.RateTimes = count;
                rs += pi.Update();
            }

            return rs;
        }
        #endregion

        #region 业务
        public static int[] GroupParticipateScore(int uid)
        {
            // SELECT [extcredits], SUM(ABS([score])) AS [todayrate] FROM [{0}ratelog] WHERE DATEDIFF(d,[postdatetime],getdate()) = 0 AND [uid] = {1} GROUP BY [extcredits]

            var exp = _.Uid == uid & _.PostDateTime >= DateTime.Now.Date;
            var list = FindAll(exp & _.ExtCredits.GroupBy(), null, _.Score.Sum() & _.ExtCredits, 0, 0);
            var arr = new Int32[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                arr[list[0].ExtCredits] = list[0].Score;
            }

            return arr;
        }
        #endregion
    }
}