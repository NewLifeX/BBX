﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace BBX.Entity
{
    /// <summary>论坛链接</summary>
    [Serializable]
    [DataObject]
    [Description("论坛链接")]
    [BindTable("ForumLink", Description = "论坛链接", ConnName = "BBX", DbType = DatabaseType.SqlServer)]
    public partial class ForumLink : IForumLink
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

        private String _Name;
        /// <summary>名称</summary>
        [DisplayName("名称")]
        [Description("名称")]
        [DataObjectField(false, false, false, 100)]
        [BindColumn(3, "Name", "名称", null, "nvarchar(100)", 0, 0, true, Master=true)]
        public virtual String Name
        {
            get { return _Name; }
            set { if (OnPropertyChanging(__.Name, value)) { _Name = value; OnPropertyChanged(__.Name); } }
        }

        private String _Url;
        /// <summary>网址</summary>
        [DisplayName("网址")]
        [Description("网址")]
        [DataObjectField(false, false, false, 100)]
        [BindColumn(4, "Url", "网址", null, "nvarchar(100)", 0, 0, true)]
        public virtual String Url
        {
            get { return _Url; }
            set { if (OnPropertyChanging(__.Url, value)) { _Url = value; OnPropertyChanged(__.Url); } }
        }

        private String _Note;
        /// <summary>描述</summary>
        [DisplayName("描述")]
        [Description("描述")]
        [DataObjectField(false, false, false, 200)]
        [BindColumn(5, "Note", "描述", null, "nvarchar(200)", 0, 0, true)]
        public virtual String Note
        {
            get { return _Note; }
            set { if (OnPropertyChanging(__.Note, value)) { _Note = value; OnPropertyChanged(__.Note); } }
        }

        private String _Logo;
        /// <summary>图标</summary>
        [DisplayName("图标")]
        [Description("图标")]
        [DataObjectField(false, false, false, 100)]
        [BindColumn(6, "Logo", "图标", null, "nvarchar(100)", 0, 0, true)]
        public virtual String Logo
        {
            get { return _Logo; }
            set { if (OnPropertyChanging(__.Logo, value)) { _Logo = value; OnPropertyChanged(__.Logo); } }
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
                    case __.Name : return _Name;
                    case __.Url : return _Url;
                    case __.Note : return _Note;
                    case __.Logo : return _Logo;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.ID : _ID = Convert.ToInt32(value); break;
                    case __.DisplayOrder : _DisplayOrder = Convert.ToInt32(value); break;
                    case __.Name : _Name = Convert.ToString(value); break;
                    case __.Url : _Url = Convert.ToString(value); break;
                    case __.Note : _Note = Convert.ToString(value); break;
                    case __.Logo : _Logo = Convert.ToString(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得论坛链接字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号</summary>
            public static readonly Field ID = FindByName(__.ID);

            ///<summary>显示顺序</summary>
            public static readonly Field DisplayOrder = FindByName(__.DisplayOrder);

            ///<summary>名称</summary>
            public static readonly Field Name = FindByName(__.Name);

            ///<summary>网址</summary>
            public static readonly Field Url = FindByName(__.Url);

            ///<summary>描述</summary>
            public static readonly Field Note = FindByName(__.Note);

            ///<summary>图标</summary>
            public static readonly Field Logo = FindByName(__.Logo);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得论坛链接字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号</summary>
            public const String ID = "ID";

            ///<summary>显示顺序</summary>
            public const String DisplayOrder = "DisplayOrder";

            ///<summary>名称</summary>
            public const String Name = "Name";

            ///<summary>网址</summary>
            public const String Url = "Url";

            ///<summary>描述</summary>
            public const String Note = "Note";

            ///<summary>图标</summary>
            public const String Logo = "Logo";

        }
        #endregion
    }

    /// <summary>论坛链接接口</summary>
    public partial interface IForumLink
    {
        #region 属性
        /// <summary>编号</summary>
        Int32 ID { get; set; }

        /// <summary>显示顺序</summary>
        Int32 DisplayOrder { get; set; }

        /// <summary>名称</summary>
        String Name { get; set; }

        /// <summary>网址</summary>
        String Url { get; set; }

        /// <summary>描述</summary>
        String Note { get; set; }

        /// <summary>图标</summary>
        String Logo { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}