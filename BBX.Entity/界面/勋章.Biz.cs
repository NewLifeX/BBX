/*
 * XCoder v6.1.5312.20840
 * 作者：nnhy/X2
 * 时间：2014-08-01 13:50:05
 * 版权：版权所有 (C) 新生命开发团队 2002~2014
*/
﻿using System;
using System.ComponentModel;
using System.Linq;
using BBX.Config;
using NewLife;
using NewLife.Log;
using XCode;

namespace BBX.Entity
{
    /// <summary>勋章</summary>
    public partial class Medal : Entity<Medal>
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
            //if (isNew || Dirtys[__.Name]) CheckExist(__.Name);

        }

        /// <summary>首次连接数据库时初始化数据，仅用于实体类重载，用户不应该调用该方法</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void InitData()
        {
            base.InitData();

            // InitData一般用于当数据表没有数据时添加一些默认数据，该实体类的任何第一次数据库操作都会触发该方法，默认异步调用
            // Meta.Count是快速取得表记录数
            if (Meta.Count > 0) return;

            // 需要注意的是，如果该方法调用了其它实体类的首次数据库操作，目标实体类的数据初始化将会在同一个线程完成
            if (XTrace.Debug) XTrace.WriteLine("开始初始化{0}[{1}]数据……", typeof(Medal).Name, Meta.Table.DataTable.DisplayName);

            Add("Medal No.1", true, "Medal1.gif");
            Add("Medal No.2", true, "Medal2.gif");
            Add("Medal No.3", true, "Medal3.gif");
            Add("Medal No.4", true, "Medal4.gif");
            Add("Medal No.5", true, "Medal5.gif");
            Add("Medal No.6", true, "Medal6.gif");
            Add("Medal No.7", true, "Medal7.gif");
            Add("Medal No.8", true, "Medal8.gif");
            Add("Medal No.9", true, "Medal9.gif");
            Add("Medal No.10", true, "Medal10.gif");

            if (XTrace.Debug) XTrace.WriteLine("完成初始化{0}[{1}]数据！", typeof(Medal).Name, Meta.Table.DataTable.DisplayName);
        }

        public static Medal Add(String name, Boolean available, String ico)
        {
            var entity = new Medal();
            //entity.ID = 0;
            entity.Name = name;
            entity.Available = available;
            entity.Image = ico;
            entity.Insert();

            return entity;
        }

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
        public String ImagePath
        {
            get
            {
                if (!Available) return "";

                return "<img border=\"0\" src=\"" + BaseConfigInfo.Current.Forumpath + "images/medals/" + Image + "\" alt=\"" + Name + "\" title=\"" + Name + "\" />";
            }
        }
        #endregion

        #region 扩展查询﻿
        /// <summary>根据编号查找</summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static Medal FindByID(Int32 id)
        {
            if (Meta.Count >= 1000)
                return Find(_.ID, id);
            else // 实体缓存
                return Meta.Cache.Entities.Find(__.ID, id);
            // 单对象缓存
            //return Meta.SingleCache[id];
        }

        public static Medal FindByFile(String file)
        {
            return Meta.Cache.Entities.FindIgnoreCase(__.Name, file);
        }

        public static string GetMedalsList(string medalList)
        {
            var ms = medalList.Split(",");
            var arr = FindAllWithCache().ToList().Where(e => ms.Contains(e.ID + "")).Select(e => e.Image).ToArray();
            return String.Join("", arr);
            //var medalsList = Caches.GetMedalsList();
            //var array = Utils.SplitString(mdealList, ",");
            //var stringBuilder = new StringBuilder();
            //for (int i = 0; i < array.Length; i++)
            //{
            //    var id = TypeConverter.StrToInt(array[i], 1) - 1;
            //    if (id >= 0 && id < medalsList.Rows.Count) stringBuilder.Append(medalsList.Rows[id]["image"]);
            //}
            //return stringBuilder.ToString();
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
        //public static EntityList<Medal> Search(String key, String orderClause, Int32 startRowIndex, Int32 maximumRows)
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
            var exp = SearchWhereByKeys(key, null);

            // 以下仅为演示，Field（继承自FieldItem）重载了==、!=、>、<、>=、<=等运算符（第4行）
            //if (userid > 0) exp &= _.OperatorID == userid;
            //if (isSign != null) exp &= _.IsSign == isSign.Value;
            //if (start > DateTime.MinValue) exp &= _.OccurTime >= start;
            //if (end > DateTime.MinValue) exp &= _.OccurTime < end.AddDays(1).Date;

            return exp;
        }

        public static EntityList<Medal> GetAvailable()
        {
            return Meta.Cache.Entities.FindAll(__.Available, true);
        }
        #endregion

        #region 扩展操作
        //public static Medal Add(string medalName, Boolean available, string image)
        //{
        //    var entity = new Medal();
        //    entity.Name = medalName;
        //    entity.Available = available;
        //    entity.Image = image;
        //    entity.Insert();

        //    return entity;
        //}

        public static void SetAvailable(String mids, Boolean available)
        {
            var ids = mids.Split(',');
            foreach (var item in ids)
            {
                var md = FindByID(item.ToInt());
                if (md != null)
                {
                    md.Available = available;
                    md.Save();
                }
            }
        }
        #endregion

        #region 业务
        public static MedalsLog Award(Int32 uid, Int32 medalid, Int32 adminuid, String ip, String reason)
        {
            var log = MedalsLog.FindByUidAndMid(uid, medalid);
            if (log == null) log = new MedalsLog();

            log.Uid = uid;
            log.Medals = medalid;
            log.AdminID = adminuid;
            log.IP = ip;
            log.Actions = "授予";
            log.Reason = reason;
            log.PostDateTime = DateTime.Now;

            var user = User.FindByID(uid);
            if (user != null) log.UserName = user.ToString();

            user = User.FindByID(adminuid);
            if (user != null) log.AdminName = user.ToString();

            log.Insert();

            return log;
        }

        public static void Import(String[] files)
        {
            var max = FindAllWithCache().ToList().Max(e => e.ID);
            foreach (var item in files)
            {
                if (item.EqualIgnoreCase("thumbs.db")) continue;

                var medal = FindByFile(item);
                if (medal == null)
                {
                    medal = new Medal();
                    medal.Name = "Medal No." + ++max;
                    medal.Image = item;
                    medal.Insert();
                }
            }
        }
        #endregion
    }
}