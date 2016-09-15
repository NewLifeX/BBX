﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace BBX.Entity
{
    /// <summary>单词</summary>
    [Serializable]
    [DataObject]
    [Description("单词")]
    [BindTable("Word", Description = "单词", ConnName = "BBX", DbType = DatabaseType.SqlServer)]
    public partial class Word : IWord
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

        private String _Admin;
        /// <summary>管理员</summary>
        [DisplayName("管理员")]
        [Description("管理员")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(2, "Admin", "管理员", null, "nvarchar(50)", 0, 0, true)]
        public virtual String Admin
        {
            get { return _Admin; }
            set { if (OnPropertyChanging(__.Admin, value)) { _Admin = value; OnPropertyChanged(__.Admin); } }
        }

        private String _Key;
        /// <summary>查找</summary>
        [DisplayName("查找")]
        [Description("查找")]
        [DataObjectField(false, false, false, 255)]
        [BindColumn(3, "Key", "查找", null, "nvarchar(255)", 0, 0, true)]
        public virtual String Key
        {
            get { return _Key; }
            set { if (OnPropertyChanging(__.Key, value)) { _Key = value; OnPropertyChanged(__.Key); } }
        }

        private String _Replacement;
        /// <summary>更换</summary>
        [DisplayName("更换")]
        [Description("更换")]
        [DataObjectField(false, false, false, 255)]
        [BindColumn(4, "Replacement", "更换", null, "nvarchar(255)", 0, 0, true)]
        public virtual String Replacement
        {
            get { return _Replacement; }
            set { if (OnPropertyChanging(__.Replacement, value)) { _Replacement = value; OnPropertyChanged(__.Replacement); } }
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
                    case __.Admin : return _Admin;
                    case __.Key : return _Key;
                    case __.Replacement : return _Replacement;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.ID : _ID = Convert.ToInt32(value); break;
                    case __.Admin : _Admin = Convert.ToString(value); break;
                    case __.Key : _Key = Convert.ToString(value); break;
                    case __.Replacement : _Replacement = Convert.ToString(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得单词字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号</summary>
            public static readonly Field ID = FindByName(__.ID);

            ///<summary>管理员</summary>
            public static readonly Field Admin = FindByName(__.Admin);

            ///<summary>查找</summary>
            public static readonly Field Key = FindByName(__.Key);

            ///<summary>更换</summary>
            public static readonly Field Replacement = FindByName(__.Replacement);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得单词字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号</summary>
            public const String ID = "ID";

            ///<summary>管理员</summary>
            public const String Admin = "Admin";

            ///<summary>查找</summary>
            public const String Key = "Key";

            ///<summary>更换</summary>
            public const String Replacement = "Replacement";

        }
        #endregion
    }

    /// <summary>单词接口</summary>
    public partial interface IWord
    {
        #region 属性
        /// <summary>编号</summary>
        Int32 ID { get; set; }

        /// <summary>管理员</summary>
        String Admin { get; set; }

        /// <summary>查找</summary>
        String Key { get; set; }

        /// <summary>更换</summary>
        String Replacement { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}