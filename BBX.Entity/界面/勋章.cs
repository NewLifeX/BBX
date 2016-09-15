﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace BBX.Entity
{
    /// <summary>勋章</summary>
    [Serializable]
    [DataObject]
    [Description("勋章")]
    [BindTable("Medal", Description = "勋章", ConnName = "BBX", DbType = DatabaseType.SqlServer)]
    public partial class Medal : IMedal
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
        [DataObjectField(false, false, false, 50)]
        [BindColumn(2, "Name", "名称", null, "nvarchar(50)", 0, 0, true, Master=true)]
        public virtual String Name
        {
            get { return _Name; }
            set { if (OnPropertyChanging(__.Name, value)) { _Name = value; OnPropertyChanged(__.Name); } }
        }

        private Boolean _Available;
        /// <summary>是否可用</summary>
        [DisplayName("是否可用")]
        [Description("是否可用")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(3, "Available", "是否可用", null, "bit", 0, 0, false)]
        public virtual Boolean Available
        {
            get { return _Available; }
            set { if (OnPropertyChanging(__.Available, value)) { _Available = value; OnPropertyChanged(__.Available); } }
        }

        private String _Image;
        /// <summary>图片</summary>
        [DisplayName("图片")]
        [Description("图片")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(4, "Image", "图片", null, "nvarchar(50)", 0, 0, true)]
        public virtual String Image
        {
            get { return _Image; }
            set { if (OnPropertyChanging(__.Image, value)) { _Image = value; OnPropertyChanged(__.Image); } }
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
                    case __.Available : return _Available;
                    case __.Image : return _Image;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.ID : _ID = Convert.ToInt32(value); break;
                    case __.Name : _Name = Convert.ToString(value); break;
                    case __.Available : _Available = Convert.ToBoolean(value); break;
                    case __.Image : _Image = Convert.ToString(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得勋章字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号</summary>
            public static readonly Field ID = FindByName(__.ID);

            ///<summary>名称</summary>
            public static readonly Field Name = FindByName(__.Name);

            ///<summary>是否可用</summary>
            public static readonly Field Available = FindByName(__.Available);

            ///<summary>图片</summary>
            public static readonly Field Image = FindByName(__.Image);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得勋章字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号</summary>
            public const String ID = "ID";

            ///<summary>名称</summary>
            public const String Name = "Name";

            ///<summary>是否可用</summary>
            public const String Available = "Available";

            ///<summary>图片</summary>
            public const String Image = "Image";

        }
        #endregion
    }

    /// <summary>勋章接口</summary>
    public partial interface IMedal
    {
        #region 属性
        /// <summary>编号</summary>
        Int32 ID { get; set; }

        /// <summary>名称</summary>
        String Name { get; set; }

        /// <summary>是否可用</summary>
        Boolean Available { get; set; }

        /// <summary>图片</summary>
        String Image { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}