﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace BBX.Entity
{
    /// <summary>管理日志</summary>
    [Serializable]
    [DataObject]
    [Description("管理日志")]
    [BindTable("AdminVisitLog", Description = "管理日志", ConnName = "BBX", DbType = DatabaseType.SqlServer)]
    public partial class AdminVisitLog : IAdminVisitLog
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

        private Int32 _Uid;
        /// <summary>用户编号</summary>
        [DisplayName("用户编号")]
        [Description("用户编号")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(2, "Uid", "用户编号", null, "int", 10, 0, false)]
        public virtual Int32 Uid
        {
            get { return _Uid; }
            set { if (OnPropertyChanging(__.Uid, value)) { _Uid = value; OnPropertyChanged(__.Uid); } }
        }

        private String _UserName;
        /// <summary>登录账户</summary>
        [DisplayName("登录账户")]
        [Description("登录账户")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(3, "UserName", "登录账户", null, "nvarchar(50)", 0, 0, true)]
        public virtual String UserName
        {
            get { return _UserName; }
            set { if (OnPropertyChanging(__.UserName, value)) { _UserName = value; OnPropertyChanged(__.UserName); } }
        }

        private Int32 _GroupID;
        /// <summary>聊天组</summary>
        [DisplayName("聊天组")]
        [Description("聊天组")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(4, "GroupID", "聊天组", null, "int", 10, 0, false)]
        public virtual Int32 GroupID
        {
            get { return _GroupID; }
            set { if (OnPropertyChanging(__.GroupID, value)) { _GroupID = value; OnPropertyChanged(__.GroupID); } }
        }

        private String _GroupTitle;
        /// <summary>分组标签</summary>
        [DisplayName("分组标签")]
        [Description("分组标签")]
        [DataObjectField(false, false, false, 50)]
        [BindColumn(5, "GroupTitle", "分组标签", null, "nvarchar(50)", 0, 0, true)]
        public virtual String GroupTitle
        {
            get { return _GroupTitle; }
            set { if (OnPropertyChanging(__.GroupTitle, value)) { _GroupTitle = value; OnPropertyChanged(__.GroupTitle); } }
        }

        private String _IP;
        /// <summary>IP地址</summary>
        [DisplayName("IP地址")]
        [Description("IP地址")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(6, "IP", "IP地址", null, "nvarchar(50)", 0, 0, true)]
        public virtual String IP
        {
            get { return _IP; }
            set { if (OnPropertyChanging(__.IP, value)) { _IP = value; OnPropertyChanged(__.IP); } }
        }

        private DateTime _PostDateTime;
        /// <summary>发送时间</summary>
        [DisplayName("发送时间")]
        [Description("发送时间")]
        [DataObjectField(false, false, true, 3)]
        [BindColumn(7, "PostDateTime", "发送时间", null, "datetime", 3, 0, false)]
        public virtual DateTime PostDateTime
        {
            get { return _PostDateTime; }
            set { if (OnPropertyChanging(__.PostDateTime, value)) { _PostDateTime = value; OnPropertyChanged(__.PostDateTime); } }
        }

        private String _Actions;
        /// <summary>行动</summary>
        [DisplayName("行动")]
        [Description("行动")]
        [DataObjectField(false, false, false, 100)]
        [BindColumn(8, "Actions", "行动", null, "nvarchar(100)", 0, 0, true)]
        public virtual String Actions
        {
            get { return _Actions; }
            set { if (OnPropertyChanging(__.Actions, value)) { _Actions = value; OnPropertyChanged(__.Actions); } }
        }

        private String _Others;
        /// <summary>其他费用</summary>
        [DisplayName("其他费用")]
        [Description("其他费用")]
        [DataObjectField(false, false, false, 200)]
        [BindColumn(9, "Others", "其他费用", null, "nvarchar(200)", 0, 0, true)]
        public virtual String Others
        {
            get { return _Others; }
            set { if (OnPropertyChanging(__.Others, value)) { _Others = value; OnPropertyChanged(__.Others); } }
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
                    case __.Uid : return _Uid;
                    case __.UserName : return _UserName;
                    case __.GroupID : return _GroupID;
                    case __.GroupTitle : return _GroupTitle;
                    case __.IP : return _IP;
                    case __.PostDateTime : return _PostDateTime;
                    case __.Actions : return _Actions;
                    case __.Others : return _Others;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.ID : _ID = Convert.ToInt32(value); break;
                    case __.Uid : _Uid = Convert.ToInt32(value); break;
                    case __.UserName : _UserName = Convert.ToString(value); break;
                    case __.GroupID : _GroupID = Convert.ToInt32(value); break;
                    case __.GroupTitle : _GroupTitle = Convert.ToString(value); break;
                    case __.IP : _IP = Convert.ToString(value); break;
                    case __.PostDateTime : _PostDateTime = Convert.ToDateTime(value); break;
                    case __.Actions : _Actions = Convert.ToString(value); break;
                    case __.Others : _Others = Convert.ToString(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得管理日志字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号</summary>
            public static readonly Field ID = FindByName(__.ID);

            ///<summary>用户编号</summary>
            public static readonly Field Uid = FindByName(__.Uid);

            ///<summary>登录账户</summary>
            public static readonly Field UserName = FindByName(__.UserName);

            ///<summary>聊天组</summary>
            public static readonly Field GroupID = FindByName(__.GroupID);

            ///<summary>分组标签</summary>
            public static readonly Field GroupTitle = FindByName(__.GroupTitle);

            ///<summary>IP地址</summary>
            public static readonly Field IP = FindByName(__.IP);

            ///<summary>发送时间</summary>
            public static readonly Field PostDateTime = FindByName(__.PostDateTime);

            ///<summary>行动</summary>
            public static readonly Field Actions = FindByName(__.Actions);

            ///<summary>其他费用</summary>
            public static readonly Field Others = FindByName(__.Others);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得管理日志字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号</summary>
            public const String ID = "ID";

            ///<summary>用户编号</summary>
            public const String Uid = "Uid";

            ///<summary>登录账户</summary>
            public const String UserName = "UserName";

            ///<summary>聊天组</summary>
            public const String GroupID = "GroupID";

            ///<summary>分组标签</summary>
            public const String GroupTitle = "GroupTitle";

            ///<summary>IP地址</summary>
            public const String IP = "IP";

            ///<summary>发送时间</summary>
            public const String PostDateTime = "PostDateTime";

            ///<summary>行动</summary>
            public const String Actions = "Actions";

            ///<summary>其他费用</summary>
            public const String Others = "Others";

        }
        #endregion
    }

    /// <summary>管理日志接口</summary>
    public partial interface IAdminVisitLog
    {
        #region 属性
        /// <summary>编号</summary>
        Int32 ID { get; set; }

        /// <summary>用户编号</summary>
        Int32 Uid { get; set; }

        /// <summary>登录账户</summary>
        String UserName { get; set; }

        /// <summary>聊天组</summary>
        Int32 GroupID { get; set; }

        /// <summary>分组标签</summary>
        String GroupTitle { get; set; }

        /// <summary>IP地址</summary>
        String IP { get; set; }

        /// <summary>发送时间</summary>
        DateTime PostDateTime { get; set; }

        /// <summary>行动</summary>
        String Actions { get; set; }

        /// <summary>其他费用</summary>
        String Others { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}