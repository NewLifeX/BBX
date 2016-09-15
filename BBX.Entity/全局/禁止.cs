﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace BBX.Entity
{
    /// <summary>禁止</summary>
    [Serializable]
    [DataObject]
    [Description("禁止")]
    [BindTable("Banned", Description = "禁止", ConnName = "BBX", DbType = DatabaseType.SqlServer)]
    public partial class Banned : IBanned
    {
        #region 属性
        private Int32 _ID;
        /// <summary>编号</summary>
        [DisplayName("编号")]
        [Description("编号")]
        [DataObjectField(true, false, false, 10)]
        [BindColumn(1, "ID", "编号", null, "int", 10, 0, false)]
        public virtual Int32 ID
        {
            get { return _ID; }
            set { if (OnPropertyChanging(__.ID, value)) { _ID = value; OnPropertyChanged(__.ID); } }
        }

        private Int16 _Ip1;
        /// <summary></summary>
        [DisplayName("Ip1")]
        [Description("")]
        [DataObjectField(false, false, false, 5)]
        [BindColumn(2, "Ip1", "", null, "smallint", 5, 0, false)]
        public virtual Int16 Ip1
        {
            get { return _Ip1; }
            set { if (OnPropertyChanging(__.Ip1, value)) { _Ip1 = value; OnPropertyChanged(__.Ip1); } }
        }

        private Int16 _Ip2;
        /// <summary></summary>
        [DisplayName("Ip2")]
        [Description("")]
        [DataObjectField(false, false, false, 5)]
        [BindColumn(3, "Ip2", "", null, "smallint", 5, 0, false)]
        public virtual Int16 Ip2
        {
            get { return _Ip2; }
            set { if (OnPropertyChanging(__.Ip2, value)) { _Ip2 = value; OnPropertyChanged(__.Ip2); } }
        }

        private Int16 _Ip3;
        /// <summary>联通</summary>
        [DisplayName("联通")]
        [Description("联通")]
        [DataObjectField(false, false, false, 5)]
        [BindColumn(4, "Ip3", "联通", null, "smallint", 5, 0, false)]
        public virtual Int16 Ip3
        {
            get { return _Ip3; }
            set { if (OnPropertyChanging(__.Ip3, value)) { _Ip3 = value; OnPropertyChanged(__.Ip3); } }
        }

        private Int16 _Ip4;
        /// <summary>IP地址4</summary>
        [DisplayName("IP地址4")]
        [Description("IP地址4")]
        [DataObjectField(false, false, false, 5)]
        [BindColumn(5, "Ip4", "IP地址4", null, "smallint", 5, 0, false)]
        public virtual Int16 Ip4
        {
            get { return _Ip4; }
            set { if (OnPropertyChanging(__.Ip4, value)) { _Ip4 = value; OnPropertyChanged(__.Ip4); } }
        }

        private String _Admin;
        /// <summary>管理员</summary>
        [DisplayName("管理员")]
        [Description("管理员")]
        [DataObjectField(false, false, false, 50)]
        [BindColumn(6, "Admin", "管理员", null, "nvarchar(50)", 0, 0, true)]
        public virtual String Admin
        {
            get { return _Admin; }
            set { if (OnPropertyChanging(__.Admin, value)) { _Admin = value; OnPropertyChanged(__.Admin); } }
        }

        private DateTime _Dateline;
        /// <summary>日界线</summary>
        [DisplayName("日界线")]
        [Description("日界线")]
        [DataObjectField(false, false, false, 3)]
        [BindColumn(7, "Dateline", "日界线", null, "datetime", 3, 0, false)]
        public virtual DateTime Dateline
        {
            get { return _Dateline; }
            set { if (OnPropertyChanging(__.Dateline, value)) { _Dateline = value; OnPropertyChanged(__.Dateline); } }
        }

        private DateTime _Expiration;
        /// <summary>过期时间</summary>
        [DisplayName("过期时间")]
        [Description("过期时间")]
        [DataObjectField(false, false, false, 3)]
        [BindColumn(8, "Expiration", "过期时间", null, "datetime", 3, 0, false)]
        public virtual DateTime Expiration
        {
            get { return _Expiration; }
            set { if (OnPropertyChanging(__.Expiration, value)) { _Expiration = value; OnPropertyChanged(__.Expiration); } }
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
                    case __.Ip1 : return _Ip1;
                    case __.Ip2 : return _Ip2;
                    case __.Ip3 : return _Ip3;
                    case __.Ip4 : return _Ip4;
                    case __.Admin : return _Admin;
                    case __.Dateline : return _Dateline;
                    case __.Expiration : return _Expiration;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.ID : _ID = Convert.ToInt32(value); break;
                    case __.Ip1 : _Ip1 = Convert.ToInt16(value); break;
                    case __.Ip2 : _Ip2 = Convert.ToInt16(value); break;
                    case __.Ip3 : _Ip3 = Convert.ToInt16(value); break;
                    case __.Ip4 : _Ip4 = Convert.ToInt16(value); break;
                    case __.Admin : _Admin = Convert.ToString(value); break;
                    case __.Dateline : _Dateline = Convert.ToDateTime(value); break;
                    case __.Expiration : _Expiration = Convert.ToDateTime(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得禁止字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号</summary>
            public static readonly Field ID = FindByName(__.ID);

            ///<summary></summary>
            public static readonly Field Ip1 = FindByName(__.Ip1);

            ///<summary></summary>
            public static readonly Field Ip2 = FindByName(__.Ip2);

            ///<summary>联通</summary>
            public static readonly Field Ip3 = FindByName(__.Ip3);

            ///<summary>IP地址4</summary>
            public static readonly Field Ip4 = FindByName(__.Ip4);

            ///<summary>管理员</summary>
            public static readonly Field Admin = FindByName(__.Admin);

            ///<summary>日界线</summary>
            public static readonly Field Dateline = FindByName(__.Dateline);

            ///<summary>过期时间</summary>
            public static readonly Field Expiration = FindByName(__.Expiration);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得禁止字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号</summary>
            public const String ID = "ID";

            ///<summary></summary>
            public const String Ip1 = "Ip1";

            ///<summary></summary>
            public const String Ip2 = "Ip2";

            ///<summary>联通</summary>
            public const String Ip3 = "Ip3";

            ///<summary>IP地址4</summary>
            public const String Ip4 = "Ip4";

            ///<summary>管理员</summary>
            public const String Admin = "Admin";

            ///<summary>日界线</summary>
            public const String Dateline = "Dateline";

            ///<summary>过期时间</summary>
            public const String Expiration = "Expiration";

        }
        #endregion
    }

    /// <summary>禁止接口</summary>
    public partial interface IBanned
    {
        #region 属性
        /// <summary>编号</summary>
        Int32 ID { get; set; }

        /// <summary></summary>
        Int16 Ip1 { get; set; }

        /// <summary></summary>
        Int16 Ip2 { get; set; }

        /// <summary>联通</summary>
        Int16 Ip3 { get; set; }

        /// <summary>IP地址4</summary>
        Int16 Ip4 { get; set; }

        /// <summary>管理员</summary>
        String Admin { get; set; }

        /// <summary>日界线</summary>
        DateTime Dateline { get; set; }

        /// <summary>过期时间</summary>
        DateTime Expiration { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}