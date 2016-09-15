﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace BBX.Entity
{
    /// <summary>链接绑定日志</summary>
    [Serializable]
    [DataObject]
    [Description("链接绑定日志")]
    [BindIndex("IX_ConnectbindLog_OpenID", false, "OpenID")]
    [BindIndex("IX_ConnectbindLog_uid", false, "uid")]
    [BindTable("ConnectbindLog", Description = "链接绑定日志", ConnName = "BBX", DbType = DatabaseType.SqlServer)]
    public partial class ConnectbindLog : IConnectbindLog
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

        private String _OpenID;
        /// <summary>邮箱账号</summary>
        [DisplayName("邮箱账号")]
        [Description("邮箱账号")]
        [DataObjectField(false, false, false, 32)]
        [BindColumn(2, "OpenID", "邮箱账号", null, "nvarchar(32)", 0, 0, true)]
        public virtual String OpenID
        {
            get { return _OpenID; }
            set { if (OnPropertyChanging(__.OpenID, value)) { _OpenID = value; OnPropertyChanged(__.OpenID); } }
        }

        private Int32 _Uid;
        /// <summary>用户编号</summary>
        [DisplayName("用户编号")]
        [Description("用户编号")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(3, "Uid", "用户编号", null, "int", 10, 0, false)]
        public virtual Int32 Uid
        {
            get { return _Uid; }
            set { if (OnPropertyChanging(__.Uid, value)) { _Uid = value; OnPropertyChanged(__.Uid); } }
        }

        private Int16 _Type;
        /// <summary>类型</summary>
        [DisplayName("类型")]
        [Description("类型")]
        [DataObjectField(false, false, false, 5)]
        [BindColumn(4, "Type", "类型", null, "smallint", 5, 0, false)]
        public virtual Int16 Type
        {
            get { return _Type; }
            set { if (OnPropertyChanging(__.Type, value)) { _Type = value; OnPropertyChanged(__.Type); } }
        }

        private Int16 _BindCount;
        /// <summary></summary>
        [DisplayName("BindCount")]
        [Description("")]
        [DataObjectField(false, false, true, 5)]
        [BindColumn(5, "BindCount", "", null, "smallint", 5, 0, false)]
        public virtual Int16 BindCount
        {
            get { return _BindCount; }
            set { if (OnPropertyChanging(__.BindCount, value)) { _BindCount = value; OnPropertyChanged(__.BindCount); } }
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
                    case __.OpenID : return _OpenID;
                    case __.Uid : return _Uid;
                    case __.Type : return _Type;
                    case __.BindCount : return _BindCount;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.ID : _ID = Convert.ToInt32(value); break;
                    case __.OpenID : _OpenID = Convert.ToString(value); break;
                    case __.Uid : _Uid = Convert.ToInt32(value); break;
                    case __.Type : _Type = Convert.ToInt16(value); break;
                    case __.BindCount : _BindCount = Convert.ToInt16(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得链接绑定日志字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号</summary>
            public static readonly Field ID = FindByName(__.ID);

            ///<summary>邮箱账号</summary>
            public static readonly Field OpenID = FindByName(__.OpenID);

            ///<summary>用户编号</summary>
            public static readonly Field Uid = FindByName(__.Uid);

            ///<summary>类型</summary>
            public static readonly Field Type = FindByName(__.Type);

            ///<summary></summary>
            public static readonly Field BindCount = FindByName(__.BindCount);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得链接绑定日志字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号</summary>
            public const String ID = "ID";

            ///<summary>邮箱账号</summary>
            public const String OpenID = "OpenID";

            ///<summary>用户编号</summary>
            public const String Uid = "Uid";

            ///<summary>类型</summary>
            public const String Type = "Type";

            ///<summary></summary>
            public const String BindCount = "BindCount";

        }
        #endregion
    }

    /// <summary>链接绑定日志接口</summary>
    public partial interface IConnectbindLog
    {
        #region 属性
        /// <summary>编号</summary>
        Int32 ID { get; set; }

        /// <summary>邮箱账号</summary>
        String OpenID { get; set; }

        /// <summary>用户编号</summary>
        Int32 Uid { get; set; }

        /// <summary>类型</summary>
        Int16 Type { get; set; }

        /// <summary></summary>
        Int16 BindCount { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}