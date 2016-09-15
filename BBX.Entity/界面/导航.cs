﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace BBX.Entity
{
    /// <summary>导航</summary>
    [Serializable]
    [DataObject]
    [Description("导航")]
    [BindTable("Nav", Description = "导航", ConnName = "BBX", DbType = DatabaseType.SqlServer)]
    public partial class Nav : INav
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

        private Int32 _ParentID;
        /// <summary>父分类</summary>
        [DisplayName("父分类")]
        [Description("父分类")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(2, "ParentID", "父分类", null, "int", 10, 0, false)]
        public virtual Int32 ParentID
        {
            get { return _ParentID; }
            set { if (OnPropertyChanging(__.ParentID, value)) { _ParentID = value; OnPropertyChanged(__.ParentID); } }
        }

        private String _Name;
        /// <summary>名称</summary>
        [DisplayName("名称")]
        [Description("名称")]
        [DataObjectField(false, false, false, 50)]
        [BindColumn(3, "Name", "名称", null, "nvarchar(50)", 0, 0, true, Master=true)]
        public virtual String Name
        {
            get { return _Name; }
            set { if (OnPropertyChanging(__.Name, value)) { _Name = value; OnPropertyChanged(__.Name); } }
        }

        private String _Title;
        /// <summary>标题</summary>
        [DisplayName("标题")]
        [Description("标题")]
        [DataObjectField(false, false, false, 255)]
        [BindColumn(4, "Title", "标题", null, "nvarchar(255)", 0, 0, true)]
        public virtual String Title
        {
            get { return _Title; }
            set { if (OnPropertyChanging(__.Title, value)) { _Title = value; OnPropertyChanged(__.Title); } }
        }

        private String _Url;
        /// <summary>网址</summary>
        [DisplayName("网址")]
        [Description("网址")]
        [DataObjectField(false, false, false, 500)]
        [BindColumn(5, "Url", "网址", null, "nvarchar(500)", 0, 0, true)]
        public virtual String Url
        {
            get { return _Url; }
            set { if (OnPropertyChanging(__.Url, value)) { _Url = value; OnPropertyChanged(__.Url); } }
        }

        private Int32 _Target;
        /// <summary>打开位置</summary>
        [DisplayName("打开位置")]
        [Description("打开位置")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(6, "Target", "打开位置", null, "int", 10, 0, false)]
        public virtual Int32 Target
        {
            get { return _Target; }
            set { if (OnPropertyChanging(__.Target, value)) { _Target = value; OnPropertyChanged(__.Target); } }
        }

        private Int32 _Type;
        /// <summary>类型</summary>
        [DisplayName("类型")]
        [Description("类型")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(7, "Type", "类型", null, "int", 10, 0, false)]
        public virtual Int32 Type
        {
            get { return _Type; }
            set { if (OnPropertyChanging(__.Type, value)) { _Type = value; OnPropertyChanged(__.Type); } }
        }

        private Int32 _Available;
        /// <summary>是否可用</summary>
        [DisplayName("是否可用")]
        [Description("是否可用")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(8, "Available", "是否可用", null, "int", 10, 0, false)]
        public virtual Int32 Available
        {
            get { return _Available; }
            set { if (OnPropertyChanging(__.Available, value)) { _Available = value; OnPropertyChanged(__.Available); } }
        }

        private Int32 _DisplayOrder;
        /// <summary>显示顺序</summary>
        [DisplayName("显示顺序")]
        [Description("显示顺序")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(9, "DisplayOrder", "显示顺序", null, "int", 10, 0, false)]
        public virtual Int32 DisplayOrder
        {
            get { return _DisplayOrder; }
            set { if (OnPropertyChanging(__.DisplayOrder, value)) { _DisplayOrder = value; OnPropertyChanged(__.DisplayOrder); } }
        }

        private SByte _Highlight;
        /// <summary>高亮</summary>
        [DisplayName("高亮")]
        [Description("高亮")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(10, "Highlight", "高亮", null, "tinyint", 0, 0, false)]
        public virtual SByte Highlight
        {
            get { return _Highlight; }
            set { if (OnPropertyChanging(__.Highlight, value)) { _Highlight = value; OnPropertyChanged(__.Highlight); } }
        }

        private Int32 _Level;
        /// <summary>级别</summary>
        [DisplayName("级别")]
        [Description("级别")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(11, "Level", "级别", null, "int", 10, 0, false)]
        public virtual Int32 Level
        {
            get { return _Level; }
            set { if (OnPropertyChanging(__.Level, value)) { _Level = value; OnPropertyChanged(__.Level); } }
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
                    case __.ParentID : return _ParentID;
                    case __.Name : return _Name;
                    case __.Title : return _Title;
                    case __.Url : return _Url;
                    case __.Target : return _Target;
                    case __.Type : return _Type;
                    case __.Available : return _Available;
                    case __.DisplayOrder : return _DisplayOrder;
                    case __.Highlight : return _Highlight;
                    case __.Level : return _Level;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.ID : _ID = Convert.ToInt32(value); break;
                    case __.ParentID : _ParentID = Convert.ToInt32(value); break;
                    case __.Name : _Name = Convert.ToString(value); break;
                    case __.Title : _Title = Convert.ToString(value); break;
                    case __.Url : _Url = Convert.ToString(value); break;
                    case __.Target : _Target = Convert.ToInt32(value); break;
                    case __.Type : _Type = Convert.ToInt32(value); break;
                    case __.Available : _Available = Convert.ToInt32(value); break;
                    case __.DisplayOrder : _DisplayOrder = Convert.ToInt32(value); break;
                    case __.Highlight : _Highlight = Convert.ToSByte(value); break;
                    case __.Level : _Level = Convert.ToInt32(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得导航字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号</summary>
            public static readonly Field ID = FindByName(__.ID);

            ///<summary>父分类</summary>
            public static readonly Field ParentID = FindByName(__.ParentID);

            ///<summary>名称</summary>
            public static readonly Field Name = FindByName(__.Name);

            ///<summary>标题</summary>
            public static readonly Field Title = FindByName(__.Title);

            ///<summary>网址</summary>
            public static readonly Field Url = FindByName(__.Url);

            ///<summary>打开位置</summary>
            public static readonly Field Target = FindByName(__.Target);

            ///<summary>类型</summary>
            public static readonly Field Type = FindByName(__.Type);

            ///<summary>是否可用</summary>
            public static readonly Field Available = FindByName(__.Available);

            ///<summary>显示顺序</summary>
            public static readonly Field DisplayOrder = FindByName(__.DisplayOrder);

            ///<summary>高亮</summary>
            public static readonly Field Highlight = FindByName(__.Highlight);

            ///<summary>级别</summary>
            public static readonly Field Level = FindByName(__.Level);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得导航字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号</summary>
            public const String ID = "ID";

            ///<summary>父分类</summary>
            public const String ParentID = "ParentID";

            ///<summary>名称</summary>
            public const String Name = "Name";

            ///<summary>标题</summary>
            public const String Title = "Title";

            ///<summary>网址</summary>
            public const String Url = "Url";

            ///<summary>打开位置</summary>
            public const String Target = "Target";

            ///<summary>类型</summary>
            public const String Type = "Type";

            ///<summary>是否可用</summary>
            public const String Available = "Available";

            ///<summary>显示顺序</summary>
            public const String DisplayOrder = "DisplayOrder";

            ///<summary>高亮</summary>
            public const String Highlight = "Highlight";

            ///<summary>级别</summary>
            public const String Level = "Level";

        }
        #endregion
    }

    /// <summary>导航接口</summary>
    public partial interface INav
    {
        #region 属性
        /// <summary>编号</summary>
        Int32 ID { get; set; }

        /// <summary>父分类</summary>
        Int32 ParentID { get; set; }

        /// <summary>名称</summary>
        String Name { get; set; }

        /// <summary>标题</summary>
        String Title { get; set; }

        /// <summary>网址</summary>
        String Url { get; set; }

        /// <summary>打开位置</summary>
        Int32 Target { get; set; }

        /// <summary>类型</summary>
        Int32 Type { get; set; }

        /// <summary>是否可用</summary>
        Int32 Available { get; set; }

        /// <summary>显示顺序</summary>
        Int32 DisplayOrder { get; set; }

        /// <summary>高亮</summary>
        SByte Highlight { get; set; }

        /// <summary>级别</summary>
        Int32 Level { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}