﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace BBX.Entity
{
    /// <summary>勋章日志</summary>
    [Serializable]
    [DataObject]
    [Description("勋章日志")]
    [BindTable("MedalsLog", Description = "勋章日志", ConnName = "BBX", DbType = DatabaseType.SqlServer)]
    public partial class MedalsLog : IMedalsLog
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

        private String _AdminName;
        /// <summary>管理员</summary>
        [DisplayName("管理员")]
        [Description("管理员")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(2, "AdminName", "管理员", null, "nvarchar(50)", 0, 0, true)]
        public virtual String AdminName
        {
            get { return _AdminName; }
            set { if (OnPropertyChanging(__.AdminName, value)) { _AdminName = value; OnPropertyChanged(__.AdminName); } }
        }

        private Int32 _AdminID;
        /// <summary>操作人</summary>
        [DisplayName("操作人")]
        [Description("操作人")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(3, "AdminID", "操作人", null, "int", 10, 0, false)]
        public virtual Int32 AdminID
        {
            get { return _AdminID; }
            set { if (OnPropertyChanging(__.AdminID, value)) { _AdminID = value; OnPropertyChanged(__.AdminID); } }
        }

        private String _IP;
        /// <summary>IP地址</summary>
        [DisplayName("IP地址")]
        [Description("IP地址")]
        [DataObjectField(false, false, true, 15)]
        [BindColumn(4, "IP", "IP地址", null, "nvarchar(15)", 0, 0, true)]
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
        [BindColumn(5, "PostDateTime", "发送时间", null, "datetime", 3, 0, false)]
        public virtual DateTime PostDateTime
        {
            get { return _PostDateTime; }
            set { if (OnPropertyChanging(__.PostDateTime, value)) { _PostDateTime = value; OnPropertyChanged(__.PostDateTime); } }
        }

        private String _UserName;
        /// <summary>登录账户</summary>
        [DisplayName("登录账户")]
        [Description("登录账户")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(6, "UserName", "登录账户", null, "nvarchar(50)", 0, 0, true)]
        public virtual String UserName
        {
            get { return _UserName; }
            set { if (OnPropertyChanging(__.UserName, value)) { _UserName = value; OnPropertyChanged(__.UserName); } }
        }

        private Int32 _Uid;
        /// <summary>用户编号</summary>
        [DisplayName("用户编号")]
        [Description("用户编号")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(7, "Uid", "用户编号", null, "int", 10, 0, false)]
        public virtual Int32 Uid
        {
            get { return _Uid; }
            set { if (OnPropertyChanging(__.Uid, value)) { _Uid = value; OnPropertyChanged(__.Uid); } }
        }

        private String _Actions;
        /// <summary>动作</summary>
        [DisplayName("动作")]
        [Description("动作")]
        [DataObjectField(false, false, true, 100)]
        [BindColumn(8, "Actions", "动作", null, "nvarchar(100)", 0, 0, true)]
        public virtual String Actions
        {
            get { return _Actions; }
            set { if (OnPropertyChanging(__.Actions, value)) { _Actions = value; OnPropertyChanged(__.Actions); } }
        }

        private Int32 _Medals;
        /// <summary>奖牌</summary>
        [DisplayName("奖牌")]
        [Description("奖牌")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(9, "Medals", "奖牌", null, "int", 10, 0, false)]
        public virtual Int32 Medals
        {
            get { return _Medals; }
            set { if (OnPropertyChanging(__.Medals, value)) { _Medals = value; OnPropertyChanged(__.Medals); } }
        }

        private String _Reason;
        /// <summary>原因</summary>
        [DisplayName("原因")]
        [Description("原因")]
        [DataObjectField(false, false, true, 100)]
        [BindColumn(10, "Reason", "原因", null, "nvarchar(100)", 0, 0, true)]
        public virtual String Reason
        {
            get { return _Reason; }
            set { if (OnPropertyChanging(__.Reason, value)) { _Reason = value; OnPropertyChanged(__.Reason); } }
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
                    case __.AdminName : return _AdminName;
                    case __.AdminID : return _AdminID;
                    case __.IP : return _IP;
                    case __.PostDateTime : return _PostDateTime;
                    case __.UserName : return _UserName;
                    case __.Uid : return _Uid;
                    case __.Actions : return _Actions;
                    case __.Medals : return _Medals;
                    case __.Reason : return _Reason;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.ID : _ID = Convert.ToInt32(value); break;
                    case __.AdminName : _AdminName = Convert.ToString(value); break;
                    case __.AdminID : _AdminID = Convert.ToInt32(value); break;
                    case __.IP : _IP = Convert.ToString(value); break;
                    case __.PostDateTime : _PostDateTime = Convert.ToDateTime(value); break;
                    case __.UserName : _UserName = Convert.ToString(value); break;
                    case __.Uid : _Uid = Convert.ToInt32(value); break;
                    case __.Actions : _Actions = Convert.ToString(value); break;
                    case __.Medals : _Medals = Convert.ToInt32(value); break;
                    case __.Reason : _Reason = Convert.ToString(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得勋章日志字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号</summary>
            public static readonly Field ID = FindByName(__.ID);

            ///<summary>管理员</summary>
            public static readonly Field AdminName = FindByName(__.AdminName);

            ///<summary>操作人</summary>
            public static readonly Field AdminID = FindByName(__.AdminID);

            ///<summary>IP地址</summary>
            public static readonly Field IP = FindByName(__.IP);

            ///<summary>发送时间</summary>
            public static readonly Field PostDateTime = FindByName(__.PostDateTime);

            ///<summary>登录账户</summary>
            public static readonly Field UserName = FindByName(__.UserName);

            ///<summary>用户编号</summary>
            public static readonly Field Uid = FindByName(__.Uid);

            ///<summary>动作</summary>
            public static readonly Field Actions = FindByName(__.Actions);

            ///<summary>奖牌</summary>
            public static readonly Field Medals = FindByName(__.Medals);

            ///<summary>原因</summary>
            public static readonly Field Reason = FindByName(__.Reason);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得勋章日志字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号</summary>
            public const String ID = "ID";

            ///<summary>管理员</summary>
            public const String AdminName = "AdminName";

            ///<summary>操作人</summary>
            public const String AdminID = "AdminID";

            ///<summary>IP地址</summary>
            public const String IP = "IP";

            ///<summary>发送时间</summary>
            public const String PostDateTime = "PostDateTime";

            ///<summary>登录账户</summary>
            public const String UserName = "UserName";

            ///<summary>用户编号</summary>
            public const String Uid = "Uid";

            ///<summary>动作</summary>
            public const String Actions = "Actions";

            ///<summary>奖牌</summary>
            public const String Medals = "Medals";

            ///<summary>原因</summary>
            public const String Reason = "Reason";

        }
        #endregion
    }

    /// <summary>勋章日志接口</summary>
    public partial interface IMedalsLog
    {
        #region 属性
        /// <summary>编号</summary>
        Int32 ID { get; set; }

        /// <summary>管理员</summary>
        String AdminName { get; set; }

        /// <summary>操作人</summary>
        Int32 AdminID { get; set; }

        /// <summary>IP地址</summary>
        String IP { get; set; }

        /// <summary>发送时间</summary>
        DateTime PostDateTime { get; set; }

        /// <summary>登录账户</summary>
        String UserName { get; set; }

        /// <summary>用户编号</summary>
        Int32 Uid { get; set; }

        /// <summary>动作</summary>
        String Actions { get; set; }

        /// <summary>奖牌</summary>
        Int32 Medals { get; set; }

        /// <summary>原因</summary>
        String Reason { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}