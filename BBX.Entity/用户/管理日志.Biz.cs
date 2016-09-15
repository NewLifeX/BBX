﻿using System;
using System.ComponentModel;
using NewLife.Web;
using XCode;
using XCode.Membership;

namespace BBX.Entity
{
    /// <summary>管理日志</summary>
    public partial class AdminVisitLog : EntityBase<AdminVisitLog>
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

            // 处理当前已登录用户信息
            if (!Dirtys[_.UserName] && ManageProvider.Provider.Current != null) UserName = ManageProvider.Provider.Current.Name;
            if (!Dirtys[_.IP]) IP = WebHelper.UserHost;
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
        //    if (XTrace.Debug) XTrace.WriteLine("开始初始化{0}管理日志数据……", typeof(AdminvisitLog).Name);

        //    var entity = new AdminvisitLog();
        //    entity.Uid = 0;
        //    entity.UserName = "abc";
        //    entity.GroupID = 0;
        //    entity.GroupTitle = "abc";
        //    entity.IP = "abc";
        //    entity.PostDateTime = DateTime.Now;
        //    entity.Actions = "abc";
        //    entity.Others = "abc";
        //    entity.Insert();

        //    if (XTrace.Debug) XTrace.WriteLine("完成初始化{0}管理日志数据！", typeof(AdminvisitLog).Name);
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
        #endregion

        #region 高级查询
        /// <summary>
        /// 查询满足条件的记录集，分页、排序
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="orderClause">排序，不带Order By</param>
        /// <param name="startRowIndex">开始行，0表示第一行</param>
        /// <param name="maximumRows">最大返回行数，0表示所有行</param>
        /// <returns>实体集</returns>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static EntityList<AdminVisitLog> Search(String key, String users, DateTime start, DateTime end, String orderClause, Int32 startRowIndex, Int32 maximumRows)
        {
            return FindAll(SearchWhere(key, users, start, end), orderClause, null, startRowIndex, maximumRows);
        }

        /// <summary>
        /// 查询满足条件的记录总数，分页和排序无效，带参数是因为ObjectDataSource要求它跟Search统一
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="orderClause">排序，不带Order By</param>
        /// <param name="startRowIndex">开始行，0表示第一行</param>
        /// <param name="maximumRows">最大返回行数，0表示所有行</param>
        /// <returns>记录数</returns>
        public static Int32 SearchCount(String key, String users, DateTime start, DateTime end, String orderClause, Int32 startRowIndex, Int32 maximumRows)
        {
            return FindCount(SearchWhere(key, users, start, end), null, null, 0, 0);
        }

        /// <summary>构造搜索条件</summary>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        private static String SearchWhere(String key, String users, DateTime start, DateTime end)
        {
            // WhereExpression重载&和|运算符，作为And和Or的替代
            var exp = SearchWhereByKeys(key);

            // SearchWhereByKeys系列方法用于构建针对字符串字段的模糊搜索
            if (!String.IsNullOrEmpty(key)) exp &= _.Others.Contains(key);
            if (!users.IsNullOrWhiteSpace()) exp &= _.UserName.In(users.Split(","));
            if (start > DateTime.MinValue) exp &= _.PostDateTime >= start.Date;
            if (end > DateTime.MinValue) exp &= _.PostDateTime < end.Date.AddDays(1);

            return exp;
        }
        #endregion

        #region 扩展操作
        public static Int32 Del(string deleteMod, string visitId, Int32 deleteNum, DateTime deleteFrom)
        {
            if (deleteMod == null) return 0;

            if (deleteMod == "chkall")
            {
                if (visitId != "") return Delete(_.ID.In(visitId.SplitAsInt()));
                return 0;
            }

            if (deleteMod == "deleteNum")
            {
                if (deleteNum > 0)
                {
                    //result = String.Format(" [visitid] NOT IN (SELECT TOP {0} [visitid] FROM [{1}adminvisitlog] ORDER BY [visitid] DESC)", deleteNum, BaseConfigInfo.Current.Tableprefix);

                    return Delete(_.ID.In(FindSQL(null, _.ID.Desc(), _.ID, 0, deleteNum)));
                }
            }

            if (deleteMod == "deleteFrom")
            {
                if (deleteFrom > DateTime.MinValue) return Delete(_.PostDateTime < deleteFrom);
            }
            return 0;
        }
        #endregion

        #region 业务
        public static bool InsertLog(int uid, string userName, int groupId, string groupTitle, string ip, string actions, string others)
        {
            try
            {
                var log = new AdminVisitLog();
                log.Uid = uid;
                log.UserName = userName;
                log.GroupID = groupId;
                log.GroupTitle = groupTitle;
                log.IP = ip;
                log.Actions = actions;
                log.Others = others;
                log.Insert();

                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}