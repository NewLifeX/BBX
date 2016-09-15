﻿using System;
using System.ComponentModel;
using NewLife.Web;
using XCode;
using XCode.Membership;

namespace BBX.Entity
{
    /// <summary>勋章日志</summary>
    public partial class MedalsLog : EntityBase<MedalsLog>
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

            if (!Dirtys[_.IP]) IP = WebHelper.UserHost;
            // 处理当前已登录用户信息
            if (!Dirtys[_.UserName] && ManageProvider.Provider.Current != null) UserName = ManageProvider.Provider.Current.Name;
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
        //    if (XTrace.Debug) XTrace.WriteLine("开始初始化{0}勋章日志数据……", typeof(MedalsLog).Name);

        //    var entity = new MedalsLog();
        //    entity.AdminName = "abc";
        //    entity.AdminID = 0;
        //    entity.IP = "abc";
        //    entity.PostDateTime = DateTime.Now;
        //    entity.UserName = "abc";
        //    entity.Uid = 0;
        //    entity.Actions = "abc";
        //    entity.Medals = 0;
        //    entity.Reason = "abc";
        //    entity.Insert();

        //    if (XTrace.Debug) XTrace.WriteLine("完成初始化{0}勋章日志数据！", typeof(MedalsLog).Name);
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
        public static MedalsLog FindByID(Int32 id)
        {
            if (Meta.Count >= 1000)
                return Find(_.ID, id);
            else // 实体缓存
                return Meta.Cache.Entities.Find(__.ID, id);
            // 单对象缓存
            //return Meta.SingleCache[id];
        }

        public static MedalsLog FindByUidAndMid(Int32 uid, Int32 mid)
        {
            if (Meta.Count >= 1000)
                return Find(new String[] { __.Uid, __.Medals }, new Object[] { uid, mid });
            else // 实体缓存
                return Meta.Cache.Entities.FindAll(__.Uid, uid).Find(__.Medals, mid);
        }
        #endregion

        #region 高级查询
        // 以下为自定义高级查询的例子

        /// <summary>
        /// 查询满足条件的记录集，分页、排序
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="orderClause">排序，不带Order By</param>
        /// <param name="startRowIndex">开始行，0表示第一行</param>
        /// <param name="maximumRows">最大返回行数，0表示所有行</param>
        /// <returns>实体集</returns>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static EntityList<MedalsLog> Search(DateTime postDateTimeStart, DateTime postDateTimeEnd, string userName, String reason, Int32 startRowIndex, Int32 maximumRows)
        {
            return FindAll(SearchWhere(postDateTimeStart, postDateTimeEnd, userName, reason), null, null, startRowIndex, maximumRows);
        }

        public static EntityList<MedalsLog> Search(Int32 pagesize, Int32 pageindex, String condition = null)
        {
            return Search(condition, null, (pageindex - 1) * pagesize, pagesize);
        }

        /// <summary>
        /// 查询满足条件的记录总数，分页和排序无效，带参数是因为ObjectDataSource要求它跟Search统一
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="orderClause">排序，不带Order By</param>
        /// <param name="startRowIndex">开始行，0表示第一行</param>
        /// <param name="maximumRows">最大返回行数，0表示所有行</param>
        /// <returns>记录数</returns>
        public static Int32 SearchCount(DateTime postDateTimeStart, DateTime postDateTimeEnd, string userName, String reason, Int32 startRowIndex, Int32 maximumRows)
        {
            return FindCount(SearchWhere(postDateTimeStart, postDateTimeEnd, userName, reason), null, null, 0, 0);
        }

        public static Int32 SearchCount(String condition = null)
        {
            return FindCount(condition, null, null, 0, 0);
        }

        /// <summary>构造搜索条件</summary>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        private static String SearchWhere(DateTime postDateTimeStart, DateTime postDateTimeEnd, string userName, String reason)
        {
            var exp = _.PostDateTime.Between(postDateTimeStart, postDateTimeEnd);
            if (!String.IsNullOrEmpty(reason)) exp &= _.Reason.Contains(reason);
            if (!String.IsNullOrEmpty(userName))
            {
                var ss = userName.Split(",");
                var exp2 = new WhereExpression();
                foreach (var item in ss)
                {
                    exp2 |= _.UserName.Contains(item);
                }
                exp &= exp2;
            }

            return exp;
        }
        #endregion

        #region 扩展操作
        public static Int32 Delete(string deleteMode, string id, Int32 deleteNum, DateTime deleteFrom)
        {
            if (deleteMode.IsNullOrWhiteSpace()) return -1;

            switch (deleteMode)
            {
                case "chkall":
                    {
                        var list = FindAll(_.ID.In(id.SplitAsInt()), null, null, 0, 0);
                        return list.Delete();
                    }
                    //break;
                case "deleteNum":
                    {
                        var list = FindAll(null, _.ID.Desc(), null, deleteNum, 0);
                        return list.Delete();
                    }
                    //break;
                case "deleteFrom":
                    {
                        var list = FindAll(_.PostDateTime < deleteFrom, null, null, 0, 0);
                        return list.Delete();
                    }
                    //break;
                default:
                    return 0;
                //break;
            }
        }
        #endregion

        #region 业务
        public static MedalsLog Create()
        {
            return null;
        }
        #endregion
    }
}