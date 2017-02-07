using System;
using System.ComponentModel;
using XCode;

namespace BBX.Entity
{
    /// <summary>趋势统计</summary>
    public partial class TrendStat : Entity<TrendStat>
    {
        #region 对象操作﻿
        static TrendStat()
        {
            var df = Meta.Factory.AdditionalFields;
            df.Add(__.Login);
            df.Add(__.Register);
            df.Add(__.Topic);
            df.Add(__.Post);
            df.Add(__.Poll);
            df.Add(__.Debate);
            df.Add(__.Bonus);

            // 自动保存，不允许缓存空
			var sc = Meta.SingleCache;
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