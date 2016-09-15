﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace BBX.Entity
{
    /// <summary>用户字段</summary>
    [Serializable]
    [DataObject]
    [Description("用户字段")]
    [BindTable("UserField", Description = "用户字段", ConnName = "BBX", DbType = DatabaseType.SqlServer)]
    public partial class UserField : IUserField
    {
        #region 属性
        private Int32 _Uid;
        /// <summary>用户编号</summary>
        [DisplayName("用户编号")]
        [Description("用户编号")]
        [DataObjectField(true, false, false, 10)]
        [BindColumn(1, "Uid", "用户编号", null, "int", 10, 0, false)]
        public virtual Int32 Uid
        {
            get { return _Uid; }
            set { if (OnPropertyChanging(__.Uid, value)) { _Uid = value; OnPropertyChanged(__.Uid); } }
        }

        private String _Website;
        /// <summary>公司网址</summary>
        [DisplayName("公司网址")]
        [Description("公司网址")]
        [DataObjectField(false, false, false, 80)]
        [BindColumn(2, "Website", "公司网址", null, "nvarchar(80)", 0, 0, true)]
        public virtual String Website
        {
            get { return _Website; }
            set { if (OnPropertyChanging(__.Website, value)) { _Website = value; OnPropertyChanged(__.Website); } }
        }

        private String _Icq;
        /// <summary>ICQ号码</summary>
        [DisplayName("ICQ号码")]
        [Description("ICQ号码")]
        [DataObjectField(false, false, false, 12)]
        [BindColumn(3, "Icq", "ICQ号码", null, "nvarchar(12)", 0, 0, true)]
        public virtual String Icq
        {
            get { return _Icq; }
            set { if (OnPropertyChanging(__.Icq, value)) { _Icq = value; OnPropertyChanged(__.Icq); } }
        }

        private String _qq;
        /// <summary>QQ号码</summary>
        [DisplayName("QQ号码")]
        [Description("QQ号码")]
        [DataObjectField(false, false, false, 12)]
        [BindColumn(4, "qq", "QQ号码", null, "nvarchar(12)", 0, 0, true)]
        public virtual String qq
        {
            get { return _qq; }
            set { if (OnPropertyChanging(__.qq, value)) { _qq = value; OnPropertyChanged(__.qq); } }
        }

        private String _Yahoo;
        /// <summary>雅虎通帐号</summary>
        [DisplayName("雅虎通帐号")]
        [Description("雅虎通帐号")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(5, "Yahoo", "雅虎通帐号", null, "nvarchar(50)", 0, 0, true)]
        public virtual String Yahoo
        {
            get { return _Yahoo; }
            set { if (OnPropertyChanging(__.Yahoo, value)) { _Yahoo = value; OnPropertyChanged(__.Yahoo); } }
        }

        private String _Msn;
        /// <summary>MSN帐号</summary>
        [DisplayName("MSN帐号")]
        [Description("MSN帐号")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(6, "Msn", "MSN帐号", null, "nvarchar(50)", 0, 0, true)]
        public virtual String Msn
        {
            get { return _Msn; }
            set { if (OnPropertyChanging(__.Msn, value)) { _Msn = value; OnPropertyChanged(__.Msn); } }
        }

        private String _Skype;
        /// <summary></summary>
        [DisplayName("Skype")]
        [Description("")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(7, "Skype", "", null, "nvarchar(50)", 0, 0, true)]
        public virtual String Skype
        {
            get { return _Skype; }
            set { if (OnPropertyChanging(__.Skype, value)) { _Skype = value; OnPropertyChanged(__.Skype); } }
        }

        private String _Location;
        /// <summary>办理地点</summary>
        [DisplayName("办理地点")]
        [Description("办理地点")]
        [DataObjectField(false, false, false, 50)]
        [BindColumn(8, "Location", "办理地点", null, "nvarchar(50)", 0, 0, true)]
        public virtual String Location
        {
            get { return _Location; }
            set { if (OnPropertyChanging(__.Location, value)) { _Location = value; OnPropertyChanged(__.Location); } }
        }

        private String _Customstatus;
        /// <summary></summary>
        [DisplayName("Customstatus")]
        [Description("")]
        [DataObjectField(false, false, false, 50)]
        [BindColumn(9, "Customstatus", "", null, "nvarchar(50)", 0, 0, true)]
        public virtual String Customstatus
        {
            get { return _Customstatus; }
            set { if (OnPropertyChanging(__.Customstatus, value)) { _Customstatus = value; OnPropertyChanged(__.Customstatus); } }
        }

        private String _Avatar;
        /// <summary>会员头像</summary>
        [DisplayName("会员头像")]
        [Description("会员头像")]
        [DataObjectField(false, false, false, 255)]
        [BindColumn(10, "Avatar", "会员头像", null, "nvarchar(255)", 0, 0, true)]
        public virtual String Avatar
        {
            get { return _Avatar; }
            set { if (OnPropertyChanging(__.Avatar, value)) { _Avatar = value; OnPropertyChanged(__.Avatar); } }
        }

        private Int32 _Avatarwidth;
        /// <summary></summary>
        [DisplayName("Avatarwidth")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(11, "Avatarwidth", "", null, "int", 10, 0, false)]
        public virtual Int32 Avatarwidth
        {
            get { return _Avatarwidth; }
            set { if (OnPropertyChanging(__.Avatarwidth, value)) { _Avatarwidth = value; OnPropertyChanged(__.Avatarwidth); } }
        }

        private Int32 _Avatarheight;
        /// <summary></summary>
        [DisplayName("Avatarheight")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(12, "Avatarheight", "", null, "int", 10, 0, false)]
        public virtual Int32 Avatarheight
        {
            get { return _Avatarheight; }
            set { if (OnPropertyChanging(__.Avatarheight, value)) { _Avatarheight = value; OnPropertyChanged(__.Avatarheight); } }
        }

        private String _Medals;
        /// <summary>奖牌</summary>
        [DisplayName("奖牌")]
        [Description("奖牌")]
        [DataObjectField(false, false, false, 300)]
        [BindColumn(13, "Medals", "奖牌", null, "nvarchar(300)", 0, 0, true)]
        public virtual String Medals
        {
            get { return _Medals; }
            set { if (OnPropertyChanging(__.Medals, value)) { _Medals = value; OnPropertyChanged(__.Medals); } }
        }

        private String _Bio;
        /// <summary>生物</summary>
        [DisplayName("生物")]
        [Description("生物")]
        [DataObjectField(false, false, false, 500)]
        [BindColumn(14, "Bio", "生物", null, "nvarchar(500)", 0, 0, true)]
        public virtual String Bio
        {
            get { return _Bio; }
            set { if (OnPropertyChanging(__.Bio, value)) { _Bio = value; OnPropertyChanged(__.Bio); } }
        }

        private String _Signature;
        /// <summary>签字</summary>
        [DisplayName("签字")]
        [Description("签字")]
        [DataObjectField(false, false, false, 500)]
        [BindColumn(15, "Signature", "签字", null, "nvarchar(500)", 0, 0, true)]
        public virtual String Signature
        {
            get { return _Signature; }
            set { if (OnPropertyChanging(__.Signature, value)) { _Signature = value; OnPropertyChanged(__.Signature); } }
        }

        private String _Sightml;
        /// <summary></summary>
        [DisplayName("Sightml")]
        [Description("")]
        [DataObjectField(false, false, false, 1000)]
        [BindColumn(16, "Sightml", "", null, "nvarchar(1000)", 0, 0, true)]
        public virtual String Sightml
        {
            get { return _Sightml; }
            set { if (OnPropertyChanging(__.Sightml, value)) { _Sightml = value; OnPropertyChanged(__.Sightml); } }
        }

        private String _Authstr;
        /// <summary></summary>
        [DisplayName("Authstr")]
        [Description("")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(17, "Authstr", "", null, "nvarchar(50)", 0, 0, true)]
        public virtual String Authstr
        {
            get { return _Authstr; }
            set { if (OnPropertyChanging(__.Authstr, value)) { _Authstr = value; OnPropertyChanged(__.Authstr); } }
        }

        private DateTime _AuthTime;
        /// <summary></summary>
        [DisplayName("AuthTime")]
        [Description("")]
        [DataObjectField(false, false, true, 3)]
        [BindColumn(18, "AuthTime", "", null, "datetime", 3, 0, false)]
        public virtual DateTime AuthTime
        {
            get { return _AuthTime; }
            set { if (OnPropertyChanging(__.AuthTime, value)) { _AuthTime = value; OnPropertyChanged(__.AuthTime); } }
        }

        private SByte _Authflag;
        /// <summary></summary>
        [DisplayName("Authflag")]
        [Description("")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(19, "Authflag", "", null, "tinyint", 0, 0, false)]
        public virtual SByte Authflag
        {
            get { return _Authflag; }
            set { if (OnPropertyChanging(__.Authflag, value)) { _Authflag = value; OnPropertyChanged(__.Authflag); } }
        }

        private String _RealName;
        /// <summary>真实姓名</summary>
        [DisplayName("真实姓名")]
        [Description("真实姓名")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(20, "RealName", "真实姓名", null, "nvarchar(10)", 0, 0, true)]
        public virtual String RealName
        {
            get { return _RealName; }
            set { if (OnPropertyChanging(__.RealName, value)) { _RealName = value; OnPropertyChanged(__.RealName); } }
        }

        private String _Idcard;
        /// <summary>身份证号码</summary>
        [DisplayName("身份证号码")]
        [Description("身份证号码")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(21, "Idcard", "身份证号码", null, "nvarchar(50)", 0, 0, true)]
        public virtual String Idcard
        {
            get { return _Idcard; }
            set { if (OnPropertyChanging(__.Idcard, value)) { _Idcard = value; OnPropertyChanged(__.Idcard); } }
        }

        private String _Mobile;
        /// <summary>手机号码</summary>
        [DisplayName("手机号码")]
        [Description("手机号码")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(22, "Mobile", "手机号码", null, "nvarchar(50)", 0, 0, true)]
        public virtual String Mobile
        {
            get { return _Mobile; }
            set { if (OnPropertyChanging(__.Mobile, value)) { _Mobile = value; OnPropertyChanged(__.Mobile); } }
        }

        private String _Phone;
        /// <summary>手机</summary>
        [DisplayName("手机")]
        [Description("手机")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(23, "Phone", "手机", null, "nvarchar(50)", 0, 0, true)]
        public virtual String Phone
        {
            get { return _Phone; }
            set { if (OnPropertyChanging(__.Phone, value)) { _Phone = value; OnPropertyChanged(__.Phone); } }
        }

        private String _Ignorepm;
        /// <summary></summary>
        [DisplayName("Ignorepm")]
        [Description("")]
        [DataObjectField(false, false, false, 1000)]
        [BindColumn(24, "Ignorepm", "", null, "nvarchar(1000)", 0, 0, true)]
        public virtual String Ignorepm
        {
            get { return _Ignorepm; }
            set { if (OnPropertyChanging(__.Ignorepm, value)) { _Ignorepm = value; OnPropertyChanged(__.Ignorepm); } }
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
                    case __.Uid : return _Uid;
                    case __.Website : return _Website;
                    case __.Icq : return _Icq;
                    case __.qq : return _qq;
                    case __.Yahoo : return _Yahoo;
                    case __.Msn : return _Msn;
                    case __.Skype : return _Skype;
                    case __.Location : return _Location;
                    case __.Customstatus : return _Customstatus;
                    case __.Avatar : return _Avatar;
                    case __.Avatarwidth : return _Avatarwidth;
                    case __.Avatarheight : return _Avatarheight;
                    case __.Medals : return _Medals;
                    case __.Bio : return _Bio;
                    case __.Signature : return _Signature;
                    case __.Sightml : return _Sightml;
                    case __.Authstr : return _Authstr;
                    case __.AuthTime : return _AuthTime;
                    case __.Authflag : return _Authflag;
                    case __.RealName : return _RealName;
                    case __.Idcard : return _Idcard;
                    case __.Mobile : return _Mobile;
                    case __.Phone : return _Phone;
                    case __.Ignorepm : return _Ignorepm;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.Uid : _Uid = Convert.ToInt32(value); break;
                    case __.Website : _Website = Convert.ToString(value); break;
                    case __.Icq : _Icq = Convert.ToString(value); break;
                    case __.qq : _qq = Convert.ToString(value); break;
                    case __.Yahoo : _Yahoo = Convert.ToString(value); break;
                    case __.Msn : _Msn = Convert.ToString(value); break;
                    case __.Skype : _Skype = Convert.ToString(value); break;
                    case __.Location : _Location = Convert.ToString(value); break;
                    case __.Customstatus : _Customstatus = Convert.ToString(value); break;
                    case __.Avatar : _Avatar = Convert.ToString(value); break;
                    case __.Avatarwidth : _Avatarwidth = Convert.ToInt32(value); break;
                    case __.Avatarheight : _Avatarheight = Convert.ToInt32(value); break;
                    case __.Medals : _Medals = Convert.ToString(value); break;
                    case __.Bio : _Bio = Convert.ToString(value); break;
                    case __.Signature : _Signature = Convert.ToString(value); break;
                    case __.Sightml : _Sightml = Convert.ToString(value); break;
                    case __.Authstr : _Authstr = Convert.ToString(value); break;
                    case __.AuthTime : _AuthTime = Convert.ToDateTime(value); break;
                    case __.Authflag : _Authflag = Convert.ToSByte(value); break;
                    case __.RealName : _RealName = Convert.ToString(value); break;
                    case __.Idcard : _Idcard = Convert.ToString(value); break;
                    case __.Mobile : _Mobile = Convert.ToString(value); break;
                    case __.Phone : _Phone = Convert.ToString(value); break;
                    case __.Ignorepm : _Ignorepm = Convert.ToString(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得用户字段字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>用户编号</summary>
            public static readonly Field Uid = FindByName(__.Uid);

            ///<summary>公司网址</summary>
            public static readonly Field Website = FindByName(__.Website);

            ///<summary>ICQ号码</summary>
            public static readonly Field Icq = FindByName(__.Icq);

            ///<summary>QQ号码</summary>
            public static readonly Field qq = FindByName(__.qq);

            ///<summary>雅虎通帐号</summary>
            public static readonly Field Yahoo = FindByName(__.Yahoo);

            ///<summary>MSN帐号</summary>
            public static readonly Field Msn = FindByName(__.Msn);

            ///<summary></summary>
            public static readonly Field Skype = FindByName(__.Skype);

            ///<summary>办理地点</summary>
            public static readonly Field Location = FindByName(__.Location);

            ///<summary></summary>
            public static readonly Field Customstatus = FindByName(__.Customstatus);

            ///<summary>会员头像</summary>
            public static readonly Field Avatar = FindByName(__.Avatar);

            ///<summary></summary>
            public static readonly Field Avatarwidth = FindByName(__.Avatarwidth);

            ///<summary></summary>
            public static readonly Field Avatarheight = FindByName(__.Avatarheight);

            ///<summary>奖牌</summary>
            public static readonly Field Medals = FindByName(__.Medals);

            ///<summary>生物</summary>
            public static readonly Field Bio = FindByName(__.Bio);

            ///<summary>签字</summary>
            public static readonly Field Signature = FindByName(__.Signature);

            ///<summary></summary>
            public static readonly Field Sightml = FindByName(__.Sightml);

            ///<summary></summary>
            public static readonly Field Authstr = FindByName(__.Authstr);

            ///<summary></summary>
            public static readonly Field AuthTime = FindByName(__.AuthTime);

            ///<summary></summary>
            public static readonly Field Authflag = FindByName(__.Authflag);

            ///<summary>真实姓名</summary>
            public static readonly Field RealName = FindByName(__.RealName);

            ///<summary>身份证号码</summary>
            public static readonly Field Idcard = FindByName(__.Idcard);

            ///<summary>手机号码</summary>
            public static readonly Field Mobile = FindByName(__.Mobile);

            ///<summary>手机</summary>
            public static readonly Field Phone = FindByName(__.Phone);

            ///<summary></summary>
            public static readonly Field Ignorepm = FindByName(__.Ignorepm);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得用户字段字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>用户编号</summary>
            public const String Uid = "Uid";

            ///<summary>公司网址</summary>
            public const String Website = "Website";

            ///<summary>ICQ号码</summary>
            public const String Icq = "Icq";

            ///<summary>QQ号码</summary>
            public const String qq = "qq";

            ///<summary>雅虎通帐号</summary>
            public const String Yahoo = "Yahoo";

            ///<summary>MSN帐号</summary>
            public const String Msn = "Msn";

            ///<summary></summary>
            public const String Skype = "Skype";

            ///<summary>办理地点</summary>
            public const String Location = "Location";

            ///<summary></summary>
            public const String Customstatus = "Customstatus";

            ///<summary>会员头像</summary>
            public const String Avatar = "Avatar";

            ///<summary></summary>
            public const String Avatarwidth = "Avatarwidth";

            ///<summary></summary>
            public const String Avatarheight = "Avatarheight";

            ///<summary>奖牌</summary>
            public const String Medals = "Medals";

            ///<summary>生物</summary>
            public const String Bio = "Bio";

            ///<summary>签字</summary>
            public const String Signature = "Signature";

            ///<summary></summary>
            public const String Sightml = "Sightml";

            ///<summary></summary>
            public const String Authstr = "Authstr";

            ///<summary></summary>
            public const String AuthTime = "AuthTime";

            ///<summary></summary>
            public const String Authflag = "Authflag";

            ///<summary>真实姓名</summary>
            public const String RealName = "RealName";

            ///<summary>身份证号码</summary>
            public const String Idcard = "Idcard";

            ///<summary>手机号码</summary>
            public const String Mobile = "Mobile";

            ///<summary>手机</summary>
            public const String Phone = "Phone";

            ///<summary></summary>
            public const String Ignorepm = "Ignorepm";

        }
        #endregion
    }

    /// <summary>用户字段接口</summary>
    public partial interface IUserField
    {
        #region 属性
        /// <summary>用户编号</summary>
        Int32 Uid { get; set; }

        /// <summary>公司网址</summary>
        String Website { get; set; }

        /// <summary>ICQ号码</summary>
        String Icq { get; set; }

        /// <summary>QQ号码</summary>
        String qq { get; set; }

        /// <summary>雅虎通帐号</summary>
        String Yahoo { get; set; }

        /// <summary>MSN帐号</summary>
        String Msn { get; set; }

        /// <summary></summary>
        String Skype { get; set; }

        /// <summary>办理地点</summary>
        String Location { get; set; }

        /// <summary></summary>
        String Customstatus { get; set; }

        /// <summary>会员头像</summary>
        String Avatar { get; set; }

        /// <summary></summary>
        Int32 Avatarwidth { get; set; }

        /// <summary></summary>
        Int32 Avatarheight { get; set; }

        /// <summary>奖牌</summary>
        String Medals { get; set; }

        /// <summary>生物</summary>
        String Bio { get; set; }

        /// <summary>签字</summary>
        String Signature { get; set; }

        /// <summary></summary>
        String Sightml { get; set; }

        /// <summary></summary>
        String Authstr { get; set; }

        /// <summary></summary>
        DateTime AuthTime { get; set; }

        /// <summary></summary>
        SByte Authflag { get; set; }

        /// <summary>真实姓名</summary>
        String RealName { get; set; }

        /// <summary>身份证号码</summary>
        String Idcard { get; set; }

        /// <summary>手机号码</summary>
        String Mobile { get; set; }

        /// <summary>手机</summary>
        String Phone { get; set; }

        /// <summary></summary>
        String Ignorepm { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}