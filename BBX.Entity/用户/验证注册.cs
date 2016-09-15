﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace BBX.Entity
{
    /// <summary>验证注册</summary>
    [Serializable]
    [DataObject]
    [Description("验证注册")]
    [BindIndex("IX_VerifyReg_CreateTime", false, "CreateTime")]
    [BindIndex("IX_VerifyReg_Email", false, "Email")]
    [BindIndex("IU_VerifyReg_IP", true, "IP")]
    [BindIndex("IU_VerifyReg_VerifyCode", true, "VerifyCode")]
    [BindTable("VerifyReg", Description = "验证注册", ConnName = "BBX", DbType = DatabaseType.SqlServer)]
    public partial class VerifyReg : IVerifyReg
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

        private String _IP;
        /// <summary>IP地址</summary>
        [DisplayName("IP地址")]
        [Description("IP地址")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(2, "IP", "IP地址", null, "nvarchar(50)", 0, 0, true)]
        public virtual String IP
        {
            get { return _IP; }
            set { if (OnPropertyChanging(__.IP, value)) { _IP = value; OnPropertyChanged(__.IP); } }
        }

        private String _Email;
        /// <summary>邮件</summary>
        [DisplayName("邮件")]
        [Description("邮件")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(3, "Email", "邮件", null, "nvarchar(50)", 0, 0, true)]
        public virtual String Email
        {
            get { return _Email; }
            set { if (OnPropertyChanging(__.Email, value)) { _Email = value; OnPropertyChanged(__.Email); } }
        }

        private DateTime _CreateTime;
        /// <summary>可用积分</summary>
        [DisplayName("可用积分")]
        [Description("可用积分")]
        [DataObjectField(false, false, true, 3)]
        [BindColumn(4, "CreateTime", "可用积分", null, "datetime", 3, 0, false)]
        public virtual DateTime CreateTime
        {
            get { return _CreateTime; }
            set { if (OnPropertyChanging(__.CreateTime, value)) { _CreateTime = value; OnPropertyChanged(__.CreateTime); } }
        }

        private DateTime _ExpireTime;
        /// <summary>到期时间</summary>
        [DisplayName("到期时间")]
        [Description("到期时间")]
        [DataObjectField(false, false, true, 3)]
        [BindColumn(5, "ExpireTime", "到期时间", null, "datetime", 3, 0, false)]
        public virtual DateTime ExpireTime
        {
            get { return _ExpireTime; }
            set { if (OnPropertyChanging(__.ExpireTime, value)) { _ExpireTime = value; OnPropertyChanged(__.ExpireTime); } }
        }

        private String _InviteCode;
        /// <summary>邀请码表</summary>
        [DisplayName("邀请码表")]
        [Description("邀请码表")]
        [DataObjectField(false, false, false, 7)]
        [BindColumn(6, "InviteCode", "邀请码表", null, "nvarchar(7)", 0, 0, true)]
        public virtual String InviteCode
        {
            get { return _InviteCode; }
            set { if (OnPropertyChanging(__.InviteCode, value)) { _InviteCode = value; OnPropertyChanged(__.InviteCode); } }
        }

        private String _VerifyCode;
        /// <summary>12位在线验证码</summary>
        [DisplayName("12位在线验证码")]
        [Description("12位在线验证码")]
        [DataObjectField(false, false, false, 16)]
        [BindColumn(7, "VerifyCode", "12位在线验证码", null, "nvarchar(16)", 0, 0, true)]
        public virtual String VerifyCode
        {
            get { return _VerifyCode; }
            set { if (OnPropertyChanging(__.VerifyCode, value)) { _VerifyCode = value; OnPropertyChanged(__.VerifyCode); } }
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
                    case __.IP : return _IP;
                    case __.Email : return _Email;
                    case __.CreateTime : return _CreateTime;
                    case __.ExpireTime : return _ExpireTime;
                    case __.InviteCode : return _InviteCode;
                    case __.VerifyCode : return _VerifyCode;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.ID : _ID = Convert.ToInt32(value); break;
                    case __.IP : _IP = Convert.ToString(value); break;
                    case __.Email : _Email = Convert.ToString(value); break;
                    case __.CreateTime : _CreateTime = Convert.ToDateTime(value); break;
                    case __.ExpireTime : _ExpireTime = Convert.ToDateTime(value); break;
                    case __.InviteCode : _InviteCode = Convert.ToString(value); break;
                    case __.VerifyCode : _VerifyCode = Convert.ToString(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得验证注册字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号</summary>
            public static readonly Field ID = FindByName(__.ID);

            ///<summary>IP地址</summary>
            public static readonly Field IP = FindByName(__.IP);

            ///<summary>邮件</summary>
            public static readonly Field Email = FindByName(__.Email);

            ///<summary>可用积分</summary>
            public static readonly Field CreateTime = FindByName(__.CreateTime);

            ///<summary>到期时间</summary>
            public static readonly Field ExpireTime = FindByName(__.ExpireTime);

            ///<summary>邀请码表</summary>
            public static readonly Field InviteCode = FindByName(__.InviteCode);

            ///<summary>12位在线验证码</summary>
            public static readonly Field VerifyCode = FindByName(__.VerifyCode);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得验证注册字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号</summary>
            public const String ID = "ID";

            ///<summary>IP地址</summary>
            public const String IP = "IP";

            ///<summary>邮件</summary>
            public const String Email = "Email";

            ///<summary>可用积分</summary>
            public const String CreateTime = "CreateTime";

            ///<summary>到期时间</summary>
            public const String ExpireTime = "ExpireTime";

            ///<summary>邀请码表</summary>
            public const String InviteCode = "InviteCode";

            ///<summary>12位在线验证码</summary>
            public const String VerifyCode = "VerifyCode";

        }
        #endregion
    }

    /// <summary>验证注册接口</summary>
    public partial interface IVerifyReg
    {
        #region 属性
        /// <summary>编号</summary>
        Int32 ID { get; set; }

        /// <summary>IP地址</summary>
        String IP { get; set; }

        /// <summary>邮件</summary>
        String Email { get; set; }

        /// <summary>可用积分</summary>
        DateTime CreateTime { get; set; }

        /// <summary>到期时间</summary>
        DateTime ExpireTime { get; set; }

        /// <summary>邀请码表</summary>
        String InviteCode { get; set; }

        /// <summary>12位在线验证码</summary>
        String VerifyCode { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}