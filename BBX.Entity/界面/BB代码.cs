﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace BBX.Entity
{
    /// <summary>BB代码</summary>
    [Serializable]
    [DataObject]
    [Description("BB代码")]
    [BindTable("BbCode", Description = "BB代码", ConnName = "BBX", DbType = DatabaseType.SqlServer)]
    public partial class BbCode : IBbCode
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

        private Int32 _Available;
        /// <summary>是否可用</summary>
        [DisplayName("是否可用")]
        [Description("是否可用")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(2, "Available", "是否可用", null, "int", 10, 0, false)]
        public virtual Int32 Available
        {
            get { return _Available; }
            set { if (OnPropertyChanging(__.Available, value)) { _Available = value; OnPropertyChanged(__.Available); } }
        }

        private String _Tag;
        /// <summary>标签</summary>
        [DisplayName("标签")]
        [Description("标签")]
        [DataObjectField(false, false, false, 100)]
        [BindColumn(3, "Tag", "标签", null, "nvarchar(100)", 0, 0, true)]
        public virtual String Tag
        {
            get { return _Tag; }
            set { if (OnPropertyChanging(__.Tag, value)) { _Tag = value; OnPropertyChanged(__.Tag); } }
        }

        private String _Icon;
        /// <summary>图标</summary>
        [DisplayName("图标")]
        [Description("图标")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(4, "Icon", "图标", null, "nvarchar(50)", 0, 0, true)]
        public virtual String Icon
        {
            get { return _Icon; }
            set { if (OnPropertyChanging(__.Icon, value)) { _Icon = value; OnPropertyChanged(__.Icon); } }
        }

        private String _Example;
        /// <summary>示例</summary>
        [DisplayName("示例")]
        [Description("示例")]
        [DataObjectField(false, false, false, 255)]
        [BindColumn(5, "Example", "示例", null, "nvarchar(255)", 0, 0, true)]
        public virtual String Example
        {
            get { return _Example; }
            set { if (OnPropertyChanging(__.Example, value)) { _Example = value; OnPropertyChanged(__.Example); } }
        }

        private Int32 _Params;
        /// <summary>参数</summary>
        [DisplayName("参数")]
        [Description("参数")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(6, "Params", "参数", null, "int", 10, 0, false)]
        public virtual Int32 Params
        {
            get { return _Params; }
            set { if (OnPropertyChanging(__.Params, value)) { _Params = value; OnPropertyChanged(__.Params); } }
        }

        private Int32 _Nest;
        /// <summary>鸟巢</summary>
        [DisplayName("鸟巢")]
        [Description("鸟巢")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(7, "Nest", "鸟巢", null, "int", 10, 0, false)]
        public virtual Int32 Nest
        {
            get { return _Nest; }
            set { if (OnPropertyChanging(__.Nest, value)) { _Nest = value; OnPropertyChanged(__.Nest); } }
        }

        private String _Explanation;
        /// <summary>解释</summary>
        [DisplayName("解释")]
        [Description("解释")]
        [DataObjectField(false, false, true, -1)]
        [BindColumn(8, "Explanation", "解释", null, "ntext", 0, 0, true)]
        public virtual String Explanation
        {
            get { return _Explanation; }
            set { if (OnPropertyChanging(__.Explanation, value)) { _Explanation = value; OnPropertyChanged(__.Explanation); } }
        }

        private String _Replacement;
        /// <summary>更换</summary>
        [DisplayName("更换")]
        [Description("更换")]
        [DataObjectField(false, false, true, -1)]
        [BindColumn(9, "Replacement", "更换", null, "ntext", 0, 0, true)]
        public virtual String Replacement
        {
            get { return _Replacement; }
            set { if (OnPropertyChanging(__.Replacement, value)) { _Replacement = value; OnPropertyChanged(__.Replacement); } }
        }

        private String _ParamsDescript;
        /// <summary>参数描述</summary>
        [DisplayName("参数描述")]
        [Description("参数描述")]
        [DataObjectField(false, false, true, -1)]
        [BindColumn(10, "ParamsDescript", "参数描述", null, "ntext", 0, 0, true)]
        public virtual String ParamsDescript
        {
            get { return _ParamsDescript; }
            set { if (OnPropertyChanging(__.ParamsDescript, value)) { _ParamsDescript = value; OnPropertyChanged(__.ParamsDescript); } }
        }

        private String _ParamsDefValue;
        /// <summary>参数默认值</summary>
        [DisplayName("参数默认值")]
        [Description("参数默认值")]
        [DataObjectField(false, false, true, -1)]
        [BindColumn(11, "ParamsDefValue", "参数默认值", null, "ntext", 0, 0, true)]
        public virtual String ParamsDefValue
        {
            get { return _ParamsDefValue; }
            set { if (OnPropertyChanging(__.ParamsDefValue, value)) { _ParamsDefValue = value; OnPropertyChanged(__.ParamsDefValue); } }
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
                    case __.Available : return _Available;
                    case __.Tag : return _Tag;
                    case __.Icon : return _Icon;
                    case __.Example : return _Example;
                    case __.Params : return _Params;
                    case __.Nest : return _Nest;
                    case __.Explanation : return _Explanation;
                    case __.Replacement : return _Replacement;
                    case __.ParamsDescript : return _ParamsDescript;
                    case __.ParamsDefValue : return _ParamsDefValue;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.ID : _ID = Convert.ToInt32(value); break;
                    case __.Available : _Available = Convert.ToInt32(value); break;
                    case __.Tag : _Tag = Convert.ToString(value); break;
                    case __.Icon : _Icon = Convert.ToString(value); break;
                    case __.Example : _Example = Convert.ToString(value); break;
                    case __.Params : _Params = Convert.ToInt32(value); break;
                    case __.Nest : _Nest = Convert.ToInt32(value); break;
                    case __.Explanation : _Explanation = Convert.ToString(value); break;
                    case __.Replacement : _Replacement = Convert.ToString(value); break;
                    case __.ParamsDescript : _ParamsDescript = Convert.ToString(value); break;
                    case __.ParamsDefValue : _ParamsDefValue = Convert.ToString(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得BB代码字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号</summary>
            public static readonly Field ID = FindByName(__.ID);

            ///<summary>是否可用</summary>
            public static readonly Field Available = FindByName(__.Available);

            ///<summary>标签</summary>
            public static readonly Field Tag = FindByName(__.Tag);

            ///<summary>图标</summary>
            public static readonly Field Icon = FindByName(__.Icon);

            ///<summary>示例</summary>
            public static readonly Field Example = FindByName(__.Example);

            ///<summary>参数</summary>
            public static readonly Field Params = FindByName(__.Params);

            ///<summary>鸟巢</summary>
            public static readonly Field Nest = FindByName(__.Nest);

            ///<summary>解释</summary>
            public static readonly Field Explanation = FindByName(__.Explanation);

            ///<summary>更换</summary>
            public static readonly Field Replacement = FindByName(__.Replacement);

            ///<summary>参数描述</summary>
            public static readonly Field ParamsDescript = FindByName(__.ParamsDescript);

            ///<summary>参数默认值</summary>
            public static readonly Field ParamsDefValue = FindByName(__.ParamsDefValue);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得BB代码字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号</summary>
            public const String ID = "ID";

            ///<summary>是否可用</summary>
            public const String Available = "Available";

            ///<summary>标签</summary>
            public const String Tag = "Tag";

            ///<summary>图标</summary>
            public const String Icon = "Icon";

            ///<summary>示例</summary>
            public const String Example = "Example";

            ///<summary>参数</summary>
            public const String Params = "Params";

            ///<summary>鸟巢</summary>
            public const String Nest = "Nest";

            ///<summary>解释</summary>
            public const String Explanation = "Explanation";

            ///<summary>更换</summary>
            public const String Replacement = "Replacement";

            ///<summary>参数描述</summary>
            public const String ParamsDescript = "ParamsDescript";

            ///<summary>参数默认值</summary>
            public const String ParamsDefValue = "ParamsDefValue";

        }
        #endregion
    }

    /// <summary>BB代码接口</summary>
    public partial interface IBbCode
    {
        #region 属性
        /// <summary>编号</summary>
        Int32 ID { get; set; }

        /// <summary>是否可用</summary>
        Int32 Available { get; set; }

        /// <summary>标签</summary>
        String Tag { get; set; }

        /// <summary>图标</summary>
        String Icon { get; set; }

        /// <summary>示例</summary>
        String Example { get; set; }

        /// <summary>参数</summary>
        Int32 Params { get; set; }

        /// <summary>鸟巢</summary>
        Int32 Nest { get; set; }

        /// <summary>解释</summary>
        String Explanation { get; set; }

        /// <summary>更换</summary>
        String Replacement { get; set; }

        /// <summary>参数描述</summary>
        String ParamsDescript { get; set; }

        /// <summary>参数默认值</summary>
        String ParamsDefValue { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}