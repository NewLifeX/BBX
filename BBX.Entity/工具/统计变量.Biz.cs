using System;
using XCode;

namespace BBX.Entity
{
    /// <summary>统计变量</summary>
    public partial class StatVar : Entity<StatVar>
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
        //    if (XTrace.Debug) XTrace.WriteLine("开始初始化{0}统计变量数据……", typeof(StatVar).Name);

        //    var entity = new StatVar();
        //    entity.Type = "abc";
        //    entity.Variable = "abc";
        //    entity.Value = "abc";
        //    entity.Insert();

        //    if (XTrace.Debug) XTrace.WriteLine("完成初始化{0}统计变量数据！", typeof(StatVar).Name);
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
        /// <summary>整型数值</summary>
        public Int32 IntValue { get { return Value.ToInt(); } set { Value = value + ""; } }
        #endregion

        #region 扩展查询﻿
        public static StatVar FindByTypeAndVariable(String type, String variable)
        {
            if (Meta.Count >= 1000)
                return Find(new String[] { __.Type, __.Variable }, new Object[] { type, variable });

            // 实体缓存
            var entity = Meta.Cache.Entities.Find(e => (e.Type + "").Trim() == type && (e.Variable + "").Trim() == variable);
            if (entity == null) entity = Find(new String[] { __.Type, __.Variable }, new Object[] { type, variable });
            return entity;
        }

        public static EntityList<StatVar> FindAllByType(String type)
        {
            if (Meta.Count >= 1000)
                return FindAll(__.Type, type);
            else
                return Meta.Cache.Entities.FindAllIgnoreCase(__.Type, type);
        }
        #endregion

        #region 高级查询
        public static EntityList<StatVar> GetAll()
        {
            return FindAll(null, _.Type.Asc() & _.Variable.Asc(), null, 0, 0);
        }
        #endregion

        #region 扩展操作
        #endregion

        #region 业务
        public static void Update(string type, string variable, Int32 value) { Update(type, variable, value + ""); }

        public static void Update(string type, string variable, string value)
        {
            var st = FindByTypeAndVariable(type, variable);
            if (st == null)
            {
                st = new StatVar();
                st.Type = type;
                st.Variable = variable;
            }
            st.Value = value;

            // 修正空格
            st.Type = st.Type.Trim();
            st.Variable = st.Variable.Trim();

            st.Save();
        }

        public static void DeleteOldDayposts()
        {
            Delete(_.Type == "dayposts" & _.Variable < DateTime.Now.AddDays(-30).ToString("yyyyMMdd"));
        }
        #endregion
    }
}