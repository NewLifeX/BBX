﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace BBX.Entity
{
    /// <summary>表情符号</summary>
    [Serializable]
    [DataObject]
    [Description("表情符号")]
    [BindTable("Smilie", Description = "表情符号", ConnName = "BBX", DbType = DatabaseType.SqlServer)]
    public partial class Smilie : ISmilie
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

        private Int32 _DisplayOrder;
        /// <summary>显示顺序</summary>
        [DisplayName("显示顺序")]
        [Description("显示顺序")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(2, "DisplayOrder", "显示顺序", null, "int", 10, 0, false)]
        public virtual Int32 DisplayOrder
        {
            get { return _DisplayOrder; }
            set { if (OnPropertyChanging(__.DisplayOrder, value)) { _DisplayOrder = value; OnPropertyChanged(__.DisplayOrder); } }
        }

        private Int32 _Type;
        /// <summary>类型</summary>
        [DisplayName("类型")]
        [Description("类型")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(3, "Type", "类型", null, "int", 10, 0, false)]
        public virtual Int32 Type
        {
            get { return _Type; }
            set { if (OnPropertyChanging(__.Type, value)) { _Type = value; OnPropertyChanged(__.Type); } }
        }

        private String _Code;
        /// <summary>代码</summary>
        [DisplayName("代码")]
        [Description("代码")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(4, "Code", "代码", null, "nvarchar(50)", 0, 0, true)]
        public virtual String Code
        {
            get { return _Code; }
            set { if (OnPropertyChanging(__.Code, value)) { _Code = value; OnPropertyChanged(__.Code); } }
        }

        private String _Url;
        /// <summary>网站</summary>
        [DisplayName("网站")]
        [Description("网站")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(5, "Url", "网站", null, "nvarchar(50)", 0, 0, true)]
        public virtual String Url
        {
            get { return _Url; }
            set { if (OnPropertyChanging(__.Url, value)) { _Url = value; OnPropertyChanged(__.Url); } }
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
                    case __.DisplayOrder : return _DisplayOrder;
                    case __.Type : return _Type;
                    case __.Code : return _Code;
                    case __.Url : return _Url;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.ID : _ID = Convert.ToInt32(value); break;
                    case __.DisplayOrder : _DisplayOrder = Convert.ToInt32(value); break;
                    case __.Type : _Type = Convert.ToInt32(value); break;
                    case __.Code : _Code = Convert.ToString(value); break;
                    case __.Url : _Url = Convert.ToString(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得表情符号字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号</summary>
            public static readonly Field ID = FindByName(__.ID);

            ///<summary>显示顺序</summary>
            public static readonly Field DisplayOrder = FindByName(__.DisplayOrder);

            ///<summary>类型</summary>
            public static readonly Field Type = FindByName(__.Type);

            ///<summary>代码</summary>
            public static readonly Field Code = FindByName(__.Code);

            ///<summary>网站</summary>
            public static readonly Field Url = FindByName(__.Url);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得表情符号字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号</summary>
            public const String ID = "ID";

            ///<summary>显示顺序</summary>
            public const String DisplayOrder = "DisplayOrder";

            ///<summary>类型</summary>
            public const String Type = "Type";

            ///<summary>代码</summary>
            public const String Code = "Code";

            ///<summary>网站</summary>
            public const String Url = "Url";

        }
        #endregion
    }

    /// <summary>表情符号接口</summary>
    public partial interface ISmilie
    {
        #region 属性
        /// <summary>编号</summary>
        Int32 ID { get; set; }

        /// <summary>显示顺序</summary>
        Int32 DisplayOrder { get; set; }

        /// <summary>类型</summary>
        Int32 Type { get; set; }

        /// <summary>代码</summary>
        String Code { get; set; }

        /// <summary>网站</summary>
        String Url { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}