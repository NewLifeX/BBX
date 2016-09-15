﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace BBX.Entity
{
    /// <summary>标签</summary>
    [Serializable]
    [DataObject]
    [Description("标签")]
    [BindTable("Tag", Description = "标签", ConnName = "BBX", DbType = DatabaseType.SqlServer)]
    public partial class Tag : ITag
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

        private String _Name;
        /// <summary>名称</summary>
        [DisplayName("名称")]
        [Description("名称")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(2, "Name", "名称", null, "nvarchar(50)", 0, 0, true, Master=true)]
        public virtual String Name
        {
            get { return _Name; }
            set { if (OnPropertyChanging(__.Name, value)) { _Name = value; OnPropertyChanged(__.Name); } }
        }

        private Int32 _UserID;
        /// <summary>用户编号</summary>
        [DisplayName("用户编号")]
        [Description("用户编号")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(3, "UserID", "用户编号", null, "int", 10, 0, false)]
        public virtual Int32 UserID
        {
            get { return _UserID; }
            set { if (OnPropertyChanging(__.UserID, value)) { _UserID = value; OnPropertyChanged(__.UserID); } }
        }

        private DateTime _PostDateTime;
        /// <summary>发送时间</summary>
        [DisplayName("发送时间")]
        [Description("发送时间")]
        [DataObjectField(false, false, true, 3)]
        [BindColumn(4, "PostDateTime", "发送时间", null, "datetime", 3, 0, false)]
        public virtual DateTime PostDateTime
        {
            get { return _PostDateTime; }
            set { if (OnPropertyChanging(__.PostDateTime, value)) { _PostDateTime = value; OnPropertyChanged(__.PostDateTime); } }
        }

        private Int32 _OrderID;
        /// <summary>排序</summary>
        [DisplayName("排序")]
        [Description("排序")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(5, "OrderID", "排序", null, "int", 10, 0, false)]
        public virtual Int32 OrderID
        {
            get { return _OrderID; }
            set { if (OnPropertyChanging(__.OrderID, value)) { _OrderID = value; OnPropertyChanged(__.OrderID); } }
        }

        private String _Color;
        /// <summary>颜色</summary>
        [DisplayName("颜色")]
        [Description("颜色")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(6, "Color", "颜色", null, "nvarchar(50)", 0, 0, true)]
        public virtual String Color
        {
            get { return _Color; }
            set { if (OnPropertyChanging(__.Color, value)) { _Color = value; OnPropertyChanged(__.Color); } }
        }

        private Int32 _Count;
        /// <summary>数量</summary>
        [DisplayName("数量")]
        [Description("数量")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(7, "Count", "数量", null, "int", 10, 0, false)]
        public virtual Int32 Count
        {
            get { return _Count; }
            set { if (OnPropertyChanging(__.Count, value)) { _Count = value; OnPropertyChanged(__.Count); } }
        }

        private Int32 _FCount;
        /// <summary></summary>
        [DisplayName("FCount")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(8, "FCount", "", null, "int", 10, 0, false)]
        public virtual Int32 FCount
        {
            get { return _FCount; }
            set { if (OnPropertyChanging(__.FCount, value)) { _FCount = value; OnPropertyChanged(__.FCount); } }
        }

        private Int32 _PCount;
        /// <summary></summary>
        [DisplayName("PCount")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(9, "PCount", "", null, "int", 10, 0, false)]
        public virtual Int32 PCount
        {
            get { return _PCount; }
            set { if (OnPropertyChanging(__.PCount, value)) { _PCount = value; OnPropertyChanged(__.PCount); } }
        }

        private Int32 _SCount;
        /// <summary></summary>
        [DisplayName("SCount")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(10, "SCount", "", null, "int", 10, 0, false)]
        public virtual Int32 SCount
        {
            get { return _SCount; }
            set { if (OnPropertyChanging(__.SCount, value)) { _SCount = value; OnPropertyChanged(__.SCount); } }
        }

        private Int32 _VCount;
        /// <summary></summary>
        [DisplayName("VCount")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(11, "VCount", "", null, "int", 10, 0, false)]
        public virtual Int32 VCount
        {
            get { return _VCount; }
            set { if (OnPropertyChanging(__.VCount, value)) { _VCount = value; OnPropertyChanged(__.VCount); } }
        }

        private Int32 _GCount;
        /// <summary></summary>
        [DisplayName("GCount")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(12, "GCount", "", null, "int", 10, 0, false)]
        public virtual Int32 GCount
        {
            get { return _GCount; }
            set { if (OnPropertyChanging(__.GCount, value)) { _GCount = value; OnPropertyChanged(__.GCount); } }
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
                    case __.Name : return _Name;
                    case __.UserID : return _UserID;
                    case __.PostDateTime : return _PostDateTime;
                    case __.OrderID : return _OrderID;
                    case __.Color : return _Color;
                    case __.Count : return _Count;
                    case __.FCount : return _FCount;
                    case __.PCount : return _PCount;
                    case __.SCount : return _SCount;
                    case __.VCount : return _VCount;
                    case __.GCount : return _GCount;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.ID : _ID = Convert.ToInt32(value); break;
                    case __.Name : _Name = Convert.ToString(value); break;
                    case __.UserID : _UserID = Convert.ToInt32(value); break;
                    case __.PostDateTime : _PostDateTime = Convert.ToDateTime(value); break;
                    case __.OrderID : _OrderID = Convert.ToInt32(value); break;
                    case __.Color : _Color = Convert.ToString(value); break;
                    case __.Count : _Count = Convert.ToInt32(value); break;
                    case __.FCount : _FCount = Convert.ToInt32(value); break;
                    case __.PCount : _PCount = Convert.ToInt32(value); break;
                    case __.SCount : _SCount = Convert.ToInt32(value); break;
                    case __.VCount : _VCount = Convert.ToInt32(value); break;
                    case __.GCount : _GCount = Convert.ToInt32(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得标签字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号</summary>
            public static readonly Field ID = FindByName(__.ID);

            ///<summary>名称</summary>
            public static readonly Field Name = FindByName(__.Name);

            ///<summary>用户编号</summary>
            public static readonly Field UserID = FindByName(__.UserID);

            ///<summary>发送时间</summary>
            public static readonly Field PostDateTime = FindByName(__.PostDateTime);

            ///<summary>排序</summary>
            public static readonly Field OrderID = FindByName(__.OrderID);

            ///<summary>颜色</summary>
            public static readonly Field Color = FindByName(__.Color);

            ///<summary>数量</summary>
            public static readonly Field Count = FindByName(__.Count);

            ///<summary></summary>
            public static readonly Field FCount = FindByName(__.FCount);

            ///<summary></summary>
            public static readonly Field PCount = FindByName(__.PCount);

            ///<summary></summary>
            public static readonly Field SCount = FindByName(__.SCount);

            ///<summary></summary>
            public static readonly Field VCount = FindByName(__.VCount);

            ///<summary></summary>
            public static readonly Field GCount = FindByName(__.GCount);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得标签字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号</summary>
            public const String ID = "ID";

            ///<summary>名称</summary>
            public const String Name = "Name";

            ///<summary>用户编号</summary>
            public const String UserID = "UserID";

            ///<summary>发送时间</summary>
            public const String PostDateTime = "PostDateTime";

            ///<summary>排序</summary>
            public const String OrderID = "OrderID";

            ///<summary>颜色</summary>
            public const String Color = "Color";

            ///<summary>数量</summary>
            public const String Count = "Count";

            ///<summary></summary>
            public const String FCount = "FCount";

            ///<summary></summary>
            public const String PCount = "PCount";

            ///<summary></summary>
            public const String SCount = "SCount";

            ///<summary></summary>
            public const String VCount = "VCount";

            ///<summary></summary>
            public const String GCount = "GCount";

        }
        #endregion
    }

    /// <summary>标签接口</summary>
    public partial interface ITag
    {
        #region 属性
        /// <summary>编号</summary>
        Int32 ID { get; set; }

        /// <summary>名称</summary>
        String Name { get; set; }

        /// <summary>用户编号</summary>
        Int32 UserID { get; set; }

        /// <summary>发送时间</summary>
        DateTime PostDateTime { get; set; }

        /// <summary>排序</summary>
        Int32 OrderID { get; set; }

        /// <summary>颜色</summary>
        String Color { get; set; }

        /// <summary>数量</summary>
        Int32 Count { get; set; }

        /// <summary></summary>
        Int32 FCount { get; set; }

        /// <summary></summary>
        Int32 PCount { get; set; }

        /// <summary></summary>
        Int32 SCount { get; set; }

        /// <summary></summary>
        Int32 VCount { get; set; }

        /// <summary></summary>
        Int32 GCount { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}