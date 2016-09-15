/*
 * XCoder v5.1.4817.26372
 * 作者：nnhy/X
 * 时间：2013-08-25 19:14:30
 * 版权：版权所有 (C) 新生命开发团队 2002~2013
*/
﻿using System;
using System.ComponentModel;
using System.Linq;
using XCode;

namespace BBX.Entity
{
    /// <summary>公告</summary>
    public partial class Announcement : EntityBase<Announcement>
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
        //    if (XTrace.Debug) XTrace.WriteLine("开始初始化{0}[{1}]数据……", typeof(Announcement).Name, Meta.Table.DataTable.DisplayName);

        //    var entity = new Announcement();
        //    entity.Poster = "abc";
        //    entity.PosterID = 0;
        //    entity.Title = "abc";
        //    entity.DisplayOrder = 0;
        //    entity.StartTime = DateTime.Now;
        //    entity.EndTime = DateTime.Now;
        //    entity.Message = "abc";
        //    entity.Insert();

        //    if (XTrace.Debug) XTrace.WriteLine("完成初始化{0}[{1}]数据！", typeof(Announcement).Name, Meta.Table.DataTable.DisplayName);
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
        public static Announcement FindByID(Int32 id)
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
        /// <summary>获取有效的广告列表</summary>
        /// <returns></returns>
        public static EntityList<Announcement> GetAvailableList()
        {
            //return FindAll(_.StartTime < DateTime.Now & _.EndTime > DateTime.Now, null, null, 0, 0);
            var now = DateTime.Now;
            return Search(null, null, now, now);
        }

        /// <summary>查询符合条件的公告</summary>
        /// <param name="poster">发布者</param>
        /// <param name="title">标题</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <returns></returns>
        public static EntityList<Announcement> Search(String poster, String title, DateTime start, DateTime end)
        {
            if (Meta.Count <= 0) return new EntityList<Announcement>();

            // 公告的总数一般不多，可以使用实体缓存
            if (Meta.Count < 1000)
            {
                var list = Meta.Cache.Entities.ToList().AsEnumerable();

                // 使用Linq从实体缓存里面过滤需要的数据
                if (!poster.IsNullOrWhiteSpace()) list = list.Where(e => e.Poster.Contains(poster));
                if (!title.IsNullOrWhiteSpace()) list = list.Where(e => e.Title.Contains(title));
                if (start > DateTime.MinValue) list = list.Where(e => e.StartTime > start);
                if (end > DateTime.MinValue) list = list.Where(e => e.EndTime < end);

                return new EntityList<Announcement>(list);
            }

            var exp = new WhereExpression();

            // 使用条件表达式构建查询SQL语句
            if (!poster.IsNullOrWhiteSpace()) exp &= _.Poster.Contains(poster);
            if (!title.IsNullOrWhiteSpace()) exp &= _.Title.Contains(title);
            if (start > DateTime.MinValue) exp &= _.StartTime > start;
            if (end > DateTime.MinValue) exp &= _.EndTime < end;

            //var exp = _.Poster.Contains(poster) & _.Title.Contains(title) & _.StartTime > start & _.EndTime < end;

            return FindAll(exp, null, null, 0, 0);
        }
        #endregion

        #region 扩展操作
        #endregion

        #region 业务
        public static Int32 UpdatePoster(Int32 uid, String username)
        {
            return Update(_.Poster == username, _.PosterID == uid);
        }

        public static Announcement Create(Int32 uid, String username, String subject, String message, DateTime start, DateTime end, Int32 order = 0)
        {
            var entity = new Announcement();
            entity.PosterID = uid;
            entity.Poster = username;
            entity.Title = subject;
            entity.Message = message;
            entity.StartTime = start;
            entity.EndTime = end;
            entity.DisplayOrder = order;
            entity.Insert();

            return entity;
        }

        public static void UpdateDisplayOrder(String ids, String orders)
        {
            if (String.IsNullOrEmpty(ids) || String.IsNullOrEmpty(orders)) return;

            var arr1 = ids.Split(',');
            var arr2 = orders.Split(',');
            if (arr1.Length != arr2.Length) return;

            for (int i = 0; i < arr1.Length; i++)
            {
                Update(_.DisplayOrder == arr2[i], _.ID == arr1[i]);
            }
        }

        public static Int32 DeleteList(String adlist)
        {
            return Delete(_.ID.In(adlist.SplitAsInt()));
        }
        #endregion
    }
}