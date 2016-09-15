﻿using System;
using System.ComponentModel;
using NewLife.Log;
using XCode.Cache;

namespace BBX.Entity
{
    /// <summary>计划任务</summary>
    public partial class ScheduledEvent : EntityBase<ScheduledEvent>
    {
        #region 对象操作﻿
        static ScheduledEvent()
        {
            //Meta.Session.HoldCache = true;
            //Meta.Cache.Expriod = 10 * 60;
        }

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

        /// <summary>首次连接数据库时初始化数据，仅用于实体类重载，用户不应该调用该方法</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void InitData()
        {
            base.InitData();

            if (Meta.Count > 0)
            {
                // 干掉七天前非本机的数据
                var svr = Environment.MachineName;
                var last = DateTime.Now.AddDays(-7);
                var list = FindAllWithCache().FindAll(e => !e.ServerName.EqualIgnoreCase(svr) && e.LastExecuted < last);
                //var list = FindAll(_.ServerName == svr & _.LastExecuted < last, null, null, 0, 0);
                if (list.Count > 0)
                {
                    XTrace.WriteLine("干掉七天前非本机的计划任务数据{0}行", list.Count);
                    list.Delete();
                }
            }
        }
        #endregion

        #region 扩展属性﻿
        #endregion

        #region 扩展查询﻿
        //static SingleEntityCache<String, ScheduledEvent> ks;

        public static ScheduledEvent FindByKeyAndServer(String key, String server)
        {
            if (Meta.Count >= 1000)
                return Find(new String[] { __.Key, __.ServerName }, new Object[] { key, server });
            else // 实体缓存。
                return Meta.Cache.Entities.FindAll(__.Key, key).Find(_.ServerName, server);

            //// 频繁查询和更新的表，不适合使用实体缓存，改用单对象缓存
            //if (ks == null)
            //{
            //    ks = new SingleEntityCache<string, ScheduledEvent>();
            //    ks.AutoSave = true;
            //    ks.FindKeyMethod = k =>
            //    {
            //        var p = k.IndexOf("#");
            //        return Find(new String[] { __.Key, __.ServerName }, new Object[] { k.Substring(0, p), k.Substring(p + 1) });
            //    };
            //}

            //return ks[key + "#" + server];
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
        //public static EntityList<ScheduledEvent> Search(String key, String orderClause, Int32 startRowIndex, Int32 maximumRows)
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
        public static DateTime GetLast(String key, String server)
        {
            var entity = FindByKeyAndServer(key, server);
            if (entity == null) return DateTime.MinValue;

            return entity.LastExecuted;
        }

        public static ScheduledEvent SetLast(String key, String server, DateTime last)
        {
            var entity = FindByKeyAndServer(key, server);
            if (entity == null) entity = new ScheduledEvent { Key = key, ServerName = server };

            entity.LastExecuted = last;
            //if (entity.ID == 0) 
            entity.Save();

            return entity;
        }
        #endregion
    }
}