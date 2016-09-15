﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;
using NewLife.Log;
using NewLife.Web;
using XCode;
using XCode.Configuration;

namespace BBX.Entity
{
    /// <summary>趋势统计</summary>
    public partial class TrendStat : Entity<TrendStat>
    {
        #region 对象操作﻿
        static TrendStat()
        {
            //AdditionalFields.Add(__.Login);
            //AdditionalFields.Add(__.Register);
            //AdditionalFields.Add(__.Topic);
            //AdditionalFields.Add(__.Post);
            //AdditionalFields.Add(__.Poll);
            //AdditionalFields.Add(__.Debate);
            //AdditionalFields.Add(__.Bonus);

            var eop = Meta.Factory;
            eop.AdditionalFields.Add(__.Login);
            eop.AdditionalFields.Add(__.Register);
            eop.AdditionalFields.Add(__.Topic);
            eop.AdditionalFields.Add(__.Post);
            eop.AdditionalFields.Add(__.Poll);
            eop.AdditionalFields.Add(__.Debate);
            eop.AdditionalFields.Add(__.Bonus);

            // 自动保存，不允许缓存空
			var sc = Meta.SingleCache;
			sc.AutoSave = true;
			sc.AllowNull = false;
			sc.FindKeyMethod = key => Find(__.DayTime, key);
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

        ///// <summary>首次连接数据库时初始化数据，仅用于实体类重载，用户不应该调用该方法</summary>
        //[EditorBrowsable(EditorBrowsableState.Never)]
        //protected override void InitData()
        //{
        //    base.InitData();

        //    // InitData一般用于当数据表没有数据时添加一些默认数据，该实体类的任何第一次数据库操作都会触发该方法，默认异步调用
        //    // Meta.Count是快速取得表记录数
        //    if (Meta.Count > 0) return;

        //    // 需要注意的是，如果该方法调用了其它实体类的首次数据库操作，目标实体类的数据初始化将会在同一个线程完成
        //    if (XTrace.Debug) XTrace.WriteLine("开始初始化{0}趋势统计数据……", typeof(TrendStat).Name);

        //    var entity = new TrendStat();
        //    entity.DayTime = 0;
        //    entity.Login = 0;
        //    entity.Register = 0;
        //    entity.Topic = 0;
        //    entity.Post = 0;
        //    entity.Poll = 0;
        //    entity.Debate = 0;
        //    entity.Bonus = 0;
        //    entity.Insert();

        //    if (XTrace.Debug) XTrace.WriteLine("完成初始化{0}趋势统计数据！", typeof(TrendStat).Name);
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
        /// <summary>今天</summary>
        public static TrendStat Today
        {
            get
            {
                var now = DateTime.Now;
                return FindByDayTime(now.Year * 10000 + now.Month * 100 + now.Day);
            }
        }
        #endregion

        #region 扩展查询﻿
        /// <summary>根据白天查找</summary>
        /// <param name="daytime">白天</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static TrendStat FindByDayTime(Int32 daytime)
        {
            //if (Meta.Count >= 1000)
            //    return Find(_.DayTime, daytime);
            //else // 实体缓存
            //    return Meta.Cache.Entities.Find(_.DayTime, daytime);
            // 单对象缓存
            var entity = Meta.SingleCache[daytime];
            if (entity == null) entity = Find(_.DayTime, daytime);
            if (entity == null)
            {
                entity = new TrendStat();
                entity.DayTime = daytime;
                entity.Save();

                entity = Meta.SingleCache[daytime];
            }

            return entity;
        }
        #endregion

        #region 高级查询
        public static EntityList<TrendStat> Search(DateTime begin, DateTime end)
        {
            return FindAll(_.DayTime.Between(begin, end), null, null, 0, 0);
        }
        #endregion

        #region 扩展操作
        #endregion

        #region 业务
        #endregion
    }
}