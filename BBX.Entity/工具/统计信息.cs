﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace BBX.Entity
{
    /// <summary>统计信息</summary>
    [Serializable]
    [DataObject]
    [Description("统计信息")]
    [BindIndex("IU_Stat_Type_Variable", true, "Type,Variable")]
    [BindTable("Stat", Description = "统计信息", ConnName = "BBX", DbType = DatabaseType.SqlServer)]
    public partial class Stat : IStat
    {
        #region 属性
        private Int32 _ID;
        /// <summary>编号</summary>
        [DisplayName("编号")]
        [Description("编号")]
        [DataObjectField(true, true, false, 10)]
        [BindColumn(1, "ID", "编号", null, "int", 10, 0, false)]
        public virtual Int32 ID
        {
            get { return _ID; }
            set { if (OnPropertyChanging(__.ID, value)) { _ID = value; OnPropertyChanged(__.ID); } }
        }

        private String _Type;
        /// <summary>类型</summary>
        [DisplayName("类型")]
        [Description("类型")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(2, "Type", "类型", null, "nvarchar(50)", 0, 0, true)]
        public virtual String Type
        {
            get { return _Type; }
            set { if (OnPropertyChanging(__.Type, value)) { _Type = value; OnPropertyChanged(__.Type); } }
        }

        private String _Variable;
        /// <summary>变量</summary>
        [DisplayName("变量")]
        [Description("变量")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(3, "Variable", "变量", null, "nvarchar(50)", 0, 0, true)]
        public virtual String Variable
        {
            get { return _Variable; }
            set { if (OnPropertyChanging(__.Variable, value)) { _Variable = value; OnPropertyChanged(__.Variable); } }
        }

        private Int32 _Count;
        /// <summary>数量</summary>
        [DisplayName("数量")]
        [Description("数量")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(4, "Count", "数量", null, "int", 10, 0, false)]
        public virtual Int32 Count
        {
            get { return _Count; }
            set { if (OnPropertyChanging(__.Count, value)) { _Count = value; OnPropertyChanged(__.Count); } }
        }
        #endregion

        #region 获取/设置 字段值
        /// <summary>
        /// 获取/设置 字段值。
        /// 一个索引，基类使用反射实现。
        /// 派生实体类可重写该索引，以避免反射带来的性能损耗
        /// </summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        public override Object this[String name]
        {
            get
            {
                switch (name)
                {
                    case __.ID : return _ID;
                    case __.Type : return _Type;
                    case __.Variable : return _Variable;
                    case __.Count : return _Count;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.ID : _ID = Convert.ToInt32(value); break;
                    case __.Type : _Type = Convert.ToString(value); break;
                    case __.Variable : _Variable = Convert.ToString(value); break;
                    case __.Count : _Count = Convert.ToInt32(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得统计信息字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号</summary>
            public static readonly Field ID = FindByName(__.ID);

            ///<summary>类型</summary>
            public static readonly Field Type = FindByName(__.Type);

            ///<summary>变量</summary>
            public static readonly Field Variable = FindByName(__.Variable);

            ///<summary>数量</summary>
            public static readonly Field Count = FindByName(__.Count);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得统计信息字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号</summary>
            public const String ID = "ID";

            ///<summary>类型</summary>
            public const String Type = "Type";

            ///<summary>变量</summary>
            public const String Variable = "Variable";

            ///<summary>数量</summary>
            public const String Count = "Count";

        }
        #endregion
    }

    /// <summary>统计信息接口</summary>
    public partial interface IStat
    {
        #region 属性
        /// <summary>编号</summary>
        Int32 ID { get; set; }

        /// <summary>类型</summary>
        String Type { get; set; }

        /// <summary>变量</summary>
        String Variable { get; set; }

        /// <summary>数量</summary>
        Int32 Count { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}