﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace BBX.Entity
{
    /// <summary>等级日志</summary>
    [Serializable]
    [DataObject]
    [Description("等级日志")]
    [BindTable("RateLog", Description = "等级日志", ConnName = "BBX", DbType = DatabaseType.SqlServer)]
    public partial class RateLog : IRateLog
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

        private Int32 _Pid;
        /// <summary>帖子编号</summary>
        [DisplayName("帖子编号")]
        [Description("帖子编号")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(2, "Pid", "帖子编号", null, "int", 10, 0, false)]
        public virtual Int32 Pid
        {
            get { return _Pid; }
            set { if (OnPropertyChanging(__.Pid, value)) { _Pid = value; OnPropertyChanged(__.Pid); } }
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

        private String _UserName;
        /// <summary>登录账户</summary>
        [DisplayName("登录账户")]
        [Description("登录账户")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(4, "UserName", "登录账户", null, "nvarchar(50)", 0, 0, true)]
        public virtual String UserName
        {
            get { return _UserName; }
            set { if (OnPropertyChanging(__.UserName, value)) { _UserName = value; OnPropertyChanged(__.UserName); } }
        }

        private Int32 _ExtCredits;
        /// <summary></summary>
        [DisplayName("ExtCredits")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(5, "ExtCredits", "", null, "int", 10, 0, false)]
        public virtual Int32 ExtCredits
        {
            get { return _ExtCredits; }
            set { if (OnPropertyChanging(__.ExtCredits, value)) { _ExtCredits = value; OnPropertyChanged(__.ExtCredits); } }
        }

        private DateTime _PostDateTime;
        /// <summary>发送时间</summary>
        [DisplayName("发送时间")]
        [Description("发送时间")]
        [DataObjectField(false, false, true, 3)]
        [BindColumn(6, "PostDateTime", "发送时间", null, "datetime", 3, 0, false)]
        public virtual DateTime PostDateTime
        {
            get { return _PostDateTime; }
            set { if (OnPropertyChanging(__.PostDateTime, value)) { _PostDateTime = value; OnPropertyChanged(__.PostDateTime); } }
        }

        private Int32 _Score;
        /// <summary>评分</summary>
        [DisplayName("评分")]
        [Description("评分")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(7, "Score", "评分", null, "int", 10, 0, false)]
        public virtual Int32 Score
        {
            get { return _Score; }
            set { if (OnPropertyChanging(__.Score, value)) { _Score = value; OnPropertyChanged(__.Score); } }
        }

        private String _Reason;
        /// <summary>原因</summary>
        [DisplayName("原因")]
        [Description("原因")]
        [DataObjectField(false, false, false, 50)]
        [BindColumn(8, "Reason", "原因", null, "nvarchar(50)", 0, 0, true)]
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
                    case __.Pid : return _Pid;
                    case __.Uid : return _Uid;
                    case __.UserName : return _UserName;
                    case __.ExtCredits : return _ExtCredits;
                    case __.PostDateTime : return _PostDateTime;
                    case __.Score : return _Score;
                    case __.Reason : return _Reason;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.ID : _ID = Convert.ToInt32(value); break;
                    case __.Pid : _Pid = Convert.ToInt32(value); break;
                    case __.Uid : _Uid = Convert.ToInt32(value); break;
                    case __.UserName : _UserName = Convert.ToString(value); break;
                    case __.ExtCredits : _ExtCredits = Convert.ToInt32(value); break;
                    case __.PostDateTime : _PostDateTime = Convert.ToDateTime(value); break;
                    case __.Score : _Score = Convert.ToInt32(value); break;
                    case __.Reason : _Reason = Convert.ToString(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得等级日志字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号</summary>
            public static readonly Field ID = FindByName(__.ID);

            ///<summary>帖子编号</summary>
            public static readonly Field Pid = FindByName(__.Pid);

            ///<summary>用户编号</summary>
            public static readonly Field Uid = FindByName(__.Uid);

            ///<summary>登录账户</summary>
            public static readonly Field UserName = FindByName(__.UserName);

            ///<summary></summary>
            public static readonly Field ExtCredits = FindByName(__.ExtCredits);

            ///<summary>发送时间</summary>
            public static readonly Field PostDateTime = FindByName(__.PostDateTime);

            ///<summary>评分</summary>
            public static readonly Field Score = FindByName(__.Score);

            ///<summary>原因</summary>
            public static readonly Field Reason = FindByName(__.Reason);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得等级日志字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号</summary>
            public const String ID = "ID";

            ///<summary>帖子编号</summary>
            public const String Pid = "Pid";

            ///<summary>用户编号</summary>
            public const String Uid = "Uid";

            ///<summary>登录账户</summary>
            public const String UserName = "UserName";

            ///<summary></summary>
            public const String ExtCredits = "ExtCredits";

            ///<summary>发送时间</summary>
            public const String PostDateTime = "PostDateTime";

            ///<summary>评分</summary>
            public const String Score = "Score";

            ///<summary>原因</summary>
            public const String Reason = "Reason";

        }
        #endregion
    }

    /// <summary>等级日志接口</summary>
    public partial interface IRateLog
    {
        #region 属性
        /// <summary>编号</summary>
        Int32 ID { get; set; }

        /// <summary>帖子编号</summary>
        Int32 Pid { get; set; }

        /// <summary>用户编号</summary>
        Int32 Uid { get; set; }

        /// <summary>登录账户</summary>
        String UserName { get; set; }

        /// <summary></summary>
        Int32 ExtCredits { get; set; }

        /// <summary>发送时间</summary>
        DateTime PostDateTime { get; set; }

        /// <summary>评分</summary>
        Int32 Score { get; set; }

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