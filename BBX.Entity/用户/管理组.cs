﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace BBX.Entity
{
    /// <summary>管理组</summary>
    [Serializable]
    [DataObject]
    [Description("管理组")]
    [BindTable("AdminGroup", Description = "管理组", ConnName = "BBX", DbType = DatabaseType.SqlServer)]
    public partial class AdminGroup : IAdminGroup
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

        private Boolean _AllowEditPost;
        /// <summary></summary>
        [DisplayName("AllowEditPost")]
        [Description("")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(2, "AllowEditPost", "", null, "bit", 0, 0, false)]
        public virtual Boolean AllowEditPost
        {
            get { return _AllowEditPost; }
            set { if (OnPropertyChanging(__.AllowEditPost, value)) { _AllowEditPost = value; OnPropertyChanged(__.AllowEditPost); } }
        }

        private Boolean _AllowEditpoll;
        /// <summary></summary>
        [DisplayName("AllowEditpoll")]
        [Description("")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(3, "AllowEditpoll", "", null, "bit", 0, 0, false)]
        public virtual Boolean AllowEditpoll
        {
            get { return _AllowEditpoll; }
            set { if (OnPropertyChanging(__.AllowEditpoll, value)) { _AllowEditpoll = value; OnPropertyChanged(__.AllowEditpoll); } }
        }

        private Int32 _AllowStickthread;
        /// <summary></summary>
        [DisplayName("AllowStickthread")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(4, "AllowStickthread", "", null, "int", 10, 0, false)]
        public virtual Int32 AllowStickthread
        {
            get { return _AllowStickthread; }
            set { if (OnPropertyChanging(__.AllowStickthread, value)) { _AllowStickthread = value; OnPropertyChanged(__.AllowStickthread); } }
        }

        private Boolean _AllowModPost;
        /// <summary></summary>
        [DisplayName("AllowModPost")]
        [Description("")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(5, "AllowModPost", "", null, "bit", 0, 0, false)]
        public virtual Boolean AllowModPost
        {
            get { return _AllowModPost; }
            set { if (OnPropertyChanging(__.AllowModPost, value)) { _AllowModPost = value; OnPropertyChanged(__.AllowModPost); } }
        }

        private Boolean _AllowDelPost;
        /// <summary></summary>
        [DisplayName("AllowDelPost")]
        [Description("")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(6, "AllowDelPost", "", null, "bit", 0, 0, false)]
        public virtual Boolean AllowDelPost
        {
            get { return _AllowDelPost; }
            set { if (OnPropertyChanging(__.AllowDelPost, value)) { _AllowDelPost = value; OnPropertyChanged(__.AllowDelPost); } }
        }

        private Boolean _AllowMassprune;
        /// <summary></summary>
        [DisplayName("AllowMassprune")]
        [Description("")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(7, "AllowMassprune", "", null, "bit", 0, 0, false)]
        public virtual Boolean AllowMassprune
        {
            get { return _AllowMassprune; }
            set { if (OnPropertyChanging(__.AllowMassprune, value)) { _AllowMassprune = value; OnPropertyChanged(__.AllowMassprune); } }
        }

        private Boolean _AllowRefund;
        /// <summary></summary>
        [DisplayName("AllowRefund")]
        [Description("")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(8, "AllowRefund", "", null, "bit", 0, 0, false)]
        public virtual Boolean AllowRefund
        {
            get { return _AllowRefund; }
            set { if (OnPropertyChanging(__.AllowRefund, value)) { _AllowRefund = value; OnPropertyChanged(__.AllowRefund); } }
        }

        private Boolean _AllowCensorword;
        /// <summary></summary>
        [DisplayName("AllowCensorword")]
        [Description("")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(9, "AllowCensorword", "", null, "bit", 0, 0, false)]
        public virtual Boolean AllowCensorword
        {
            get { return _AllowCensorword; }
            set { if (OnPropertyChanging(__.AllowCensorword, value)) { _AllowCensorword = value; OnPropertyChanged(__.AllowCensorword); } }
        }

        private Boolean _AllowViewIP;
        /// <summary></summary>
        [DisplayName("AllowViewIP")]
        [Description("")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(10, "AllowViewIP", "", null, "bit", 0, 0, false)]
        public virtual Boolean AllowViewIP
        {
            get { return _AllowViewIP; }
            set { if (OnPropertyChanging(__.AllowViewIP, value)) { _AllowViewIP = value; OnPropertyChanged(__.AllowViewIP); } }
        }

        private Boolean _AllowBanIP;
        /// <summary></summary>
        [DisplayName("AllowBanIP")]
        [Description("")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(11, "AllowBanIP", "", null, "bit", 0, 0, false)]
        public virtual Boolean AllowBanIP
        {
            get { return _AllowBanIP; }
            set { if (OnPropertyChanging(__.AllowBanIP, value)) { _AllowBanIP = value; OnPropertyChanged(__.AllowBanIP); } }
        }

        private Boolean _AllowEditUser;
        /// <summary></summary>
        [DisplayName("AllowEditUser")]
        [Description("")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(12, "AllowEditUser", "", null, "bit", 0, 0, false)]
        public virtual Boolean AllowEditUser
        {
            get { return _AllowEditUser; }
            set { if (OnPropertyChanging(__.AllowEditUser, value)) { _AllowEditUser = value; OnPropertyChanged(__.AllowEditUser); } }
        }

        private Boolean _AllowModUser;
        /// <summary></summary>
        [DisplayName("AllowModUser")]
        [Description("")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(13, "AllowModUser", "", null, "bit", 0, 0, false)]
        public virtual Boolean AllowModUser
        {
            get { return _AllowModUser; }
            set { if (OnPropertyChanging(__.AllowModUser, value)) { _AllowModUser = value; OnPropertyChanged(__.AllowModUser); } }
        }

        private Boolean _AllowBanUser;
        /// <summary></summary>
        [DisplayName("AllowBanUser")]
        [Description("")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(14, "AllowBanUser", "", null, "bit", 0, 0, false)]
        public virtual Boolean AllowBanUser
        {
            get { return _AllowBanUser; }
            set { if (OnPropertyChanging(__.AllowBanUser, value)) { _AllowBanUser = value; OnPropertyChanged(__.AllowBanUser); } }
        }

        private Boolean _AllowPostannounce;
        /// <summary></summary>
        [DisplayName("AllowPostannounce")]
        [Description("")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(15, "AllowPostannounce", "", null, "bit", 0, 0, false)]
        public virtual Boolean AllowPostannounce
        {
            get { return _AllowPostannounce; }
            set { if (OnPropertyChanging(__.AllowPostannounce, value)) { _AllowPostannounce = value; OnPropertyChanged(__.AllowPostannounce); } }
        }

        private Boolean _AllowViewLog;
        /// <summary></summary>
        [DisplayName("AllowViewLog")]
        [Description("")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(16, "AllowViewLog", "", null, "bit", 0, 0, false)]
        public virtual Boolean AllowViewLog
        {
            get { return _AllowViewLog; }
            set { if (OnPropertyChanging(__.AllowViewLog, value)) { _AllowViewLog = value; OnPropertyChanged(__.AllowViewLog); } }
        }

        private Boolean _DisablePostctrl;
        /// <summary></summary>
        [DisplayName("DisablePostctrl")]
        [Description("")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(17, "DisablePostctrl", "", null, "bit", 0, 0, false)]
        public virtual Boolean DisablePostctrl
        {
            get { return _DisablePostctrl; }
            set { if (OnPropertyChanging(__.DisablePostctrl, value)) { _DisablePostctrl = value; OnPropertyChanged(__.DisablePostctrl); } }
        }

        private Boolean _AllowViewrealName;
        /// <summary></summary>
        [DisplayName("AllowViewrealName")]
        [Description("")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(18, "AllowViewrealName", "", null, "bit", 0, 0, false)]
        public virtual Boolean AllowViewrealName
        {
            get { return _AllowViewrealName; }
            set { if (OnPropertyChanging(__.AllowViewrealName, value)) { _AllowViewrealName = value; OnPropertyChanged(__.AllowViewrealName); } }
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
                    case __.AllowEditPost : return _AllowEditPost;
                    case __.AllowEditpoll : return _AllowEditpoll;
                    case __.AllowStickthread : return _AllowStickthread;
                    case __.AllowModPost : return _AllowModPost;
                    case __.AllowDelPost : return _AllowDelPost;
                    case __.AllowMassprune : return _AllowMassprune;
                    case __.AllowRefund : return _AllowRefund;
                    case __.AllowCensorword : return _AllowCensorword;
                    case __.AllowViewIP : return _AllowViewIP;
                    case __.AllowBanIP : return _AllowBanIP;
                    case __.AllowEditUser : return _AllowEditUser;
                    case __.AllowModUser : return _AllowModUser;
                    case __.AllowBanUser : return _AllowBanUser;
                    case __.AllowPostannounce : return _AllowPostannounce;
                    case __.AllowViewLog : return _AllowViewLog;
                    case __.DisablePostctrl : return _DisablePostctrl;
                    case __.AllowViewrealName : return _AllowViewrealName;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.ID : _ID = Convert.ToInt32(value); break;
                    case __.AllowEditPost : _AllowEditPost = Convert.ToBoolean(value); break;
                    case __.AllowEditpoll : _AllowEditpoll = Convert.ToBoolean(value); break;
                    case __.AllowStickthread : _AllowStickthread = Convert.ToInt32(value); break;
                    case __.AllowModPost : _AllowModPost = Convert.ToBoolean(value); break;
                    case __.AllowDelPost : _AllowDelPost = Convert.ToBoolean(value); break;
                    case __.AllowMassprune : _AllowMassprune = Convert.ToBoolean(value); break;
                    case __.AllowRefund : _AllowRefund = Convert.ToBoolean(value); break;
                    case __.AllowCensorword : _AllowCensorword = Convert.ToBoolean(value); break;
                    case __.AllowViewIP : _AllowViewIP = Convert.ToBoolean(value); break;
                    case __.AllowBanIP : _AllowBanIP = Convert.ToBoolean(value); break;
                    case __.AllowEditUser : _AllowEditUser = Convert.ToBoolean(value); break;
                    case __.AllowModUser : _AllowModUser = Convert.ToBoolean(value); break;
                    case __.AllowBanUser : _AllowBanUser = Convert.ToBoolean(value); break;
                    case __.AllowPostannounce : _AllowPostannounce = Convert.ToBoolean(value); break;
                    case __.AllowViewLog : _AllowViewLog = Convert.ToBoolean(value); break;
                    case __.DisablePostctrl : _DisablePostctrl = Convert.ToBoolean(value); break;
                    case __.AllowViewrealName : _AllowViewrealName = Convert.ToBoolean(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得管理组字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号</summary>
            public static readonly Field ID = FindByName(__.ID);

            ///<summary></summary>
            public static readonly Field AllowEditPost = FindByName(__.AllowEditPost);

            ///<summary></summary>
            public static readonly Field AllowEditpoll = FindByName(__.AllowEditpoll);

            ///<summary></summary>
            public static readonly Field AllowStickthread = FindByName(__.AllowStickthread);

            ///<summary></summary>
            public static readonly Field AllowModPost = FindByName(__.AllowModPost);

            ///<summary></summary>
            public static readonly Field AllowDelPost = FindByName(__.AllowDelPost);

            ///<summary></summary>
            public static readonly Field AllowMassprune = FindByName(__.AllowMassprune);

            ///<summary></summary>
            public static readonly Field AllowRefund = FindByName(__.AllowRefund);

            ///<summary></summary>
            public static readonly Field AllowCensorword = FindByName(__.AllowCensorword);

            ///<summary></summary>
            public static readonly Field AllowViewIP = FindByName(__.AllowViewIP);

            ///<summary></summary>
            public static readonly Field AllowBanIP = FindByName(__.AllowBanIP);

            ///<summary></summary>
            public static readonly Field AllowEditUser = FindByName(__.AllowEditUser);

            ///<summary></summary>
            public static readonly Field AllowModUser = FindByName(__.AllowModUser);

            ///<summary></summary>
            public static readonly Field AllowBanUser = FindByName(__.AllowBanUser);

            ///<summary></summary>
            public static readonly Field AllowPostannounce = FindByName(__.AllowPostannounce);

            ///<summary></summary>
            public static readonly Field AllowViewLog = FindByName(__.AllowViewLog);

            ///<summary></summary>
            public static readonly Field DisablePostctrl = FindByName(__.DisablePostctrl);

            ///<summary></summary>
            public static readonly Field AllowViewrealName = FindByName(__.AllowViewrealName);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得管理组字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号</summary>
            public const String ID = "ID";

            ///<summary></summary>
            public const String AllowEditPost = "AllowEditPost";

            ///<summary></summary>
            public const String AllowEditpoll = "AllowEditpoll";

            ///<summary></summary>
            public const String AllowStickthread = "AllowStickthread";

            ///<summary></summary>
            public const String AllowModPost = "AllowModPost";

            ///<summary></summary>
            public const String AllowDelPost = "AllowDelPost";

            ///<summary></summary>
            public const String AllowMassprune = "AllowMassprune";

            ///<summary></summary>
            public const String AllowRefund = "AllowRefund";

            ///<summary></summary>
            public const String AllowCensorword = "AllowCensorword";

            ///<summary></summary>
            public const String AllowViewIP = "AllowViewIP";

            ///<summary></summary>
            public const String AllowBanIP = "AllowBanIP";

            ///<summary></summary>
            public const String AllowEditUser = "AllowEditUser";

            ///<summary></summary>
            public const String AllowModUser = "AllowModUser";

            ///<summary></summary>
            public const String AllowBanUser = "AllowBanUser";

            ///<summary></summary>
            public const String AllowPostannounce = "AllowPostannounce";

            ///<summary></summary>
            public const String AllowViewLog = "AllowViewLog";

            ///<summary></summary>
            public const String DisablePostctrl = "DisablePostctrl";

            ///<summary></summary>
            public const String AllowViewrealName = "AllowViewrealName";

        }
        #endregion
    }

    /// <summary>管理组接口</summary>
    public partial interface IAdminGroup
    {
        #region 属性
        /// <summary>编号</summary>
        Int32 ID { get; set; }

        /// <summary></summary>
        Boolean AllowEditPost { get; set; }

        /// <summary></summary>
        Boolean AllowEditpoll { get; set; }

        /// <summary></summary>
        Int32 AllowStickthread { get; set; }

        /// <summary></summary>
        Boolean AllowModPost { get; set; }

        /// <summary></summary>
        Boolean AllowDelPost { get; set; }

        /// <summary></summary>
        Boolean AllowMassprune { get; set; }

        /// <summary></summary>
        Boolean AllowRefund { get; set; }

        /// <summary></summary>
        Boolean AllowCensorword { get; set; }

        /// <summary></summary>
        Boolean AllowViewIP { get; set; }

        /// <summary></summary>
        Boolean AllowBanIP { get; set; }

        /// <summary></summary>
        Boolean AllowEditUser { get; set; }

        /// <summary></summary>
        Boolean AllowModUser { get; set; }

        /// <summary></summary>
        Boolean AllowBanUser { get; set; }

        /// <summary></summary>
        Boolean AllowPostannounce { get; set; }

        /// <summary></summary>
        Boolean AllowViewLog { get; set; }

        /// <summary></summary>
        Boolean DisablePostctrl { get; set; }

        /// <summary></summary>
        Boolean AllowViewrealName { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}