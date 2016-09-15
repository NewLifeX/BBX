﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace BBX.Entity
{
    /// <summary>用户链接</summary>
    [Serializable]
    [DataObject]
    [Description("用户链接")]
    [BindIndex("IU_UserConnect_OpenId", true, "OpenId")]
    [BindIndex("IX_UserConnect_uid", false, "uid")]
    [BindTable("UserConnect", Description = "用户链接", ConnName = "BBX", DbType = DatabaseType.SqlServer)]
    public partial class UserConnect : IUserConnect
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

        private String _OpenId;
        /// <summary>邮箱账号</summary>
        [DisplayName("邮箱账号")]
        [Description("邮箱账号")]
        [DataObjectField(false, false, false, 32)]
        [BindColumn(2, "OpenId", "邮箱账号", null, "nvarchar(32)", 0, 0, true)]
        public virtual String OpenId
        {
            get { return _OpenId; }
            set { if (OnPropertyChanging(__.OpenId, value)) { _OpenId = value; OnPropertyChanged(__.OpenId); } }
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

        private String _Token;
        /// <summary>令牌</summary>
        [DisplayName("令牌")]
        [Description("令牌")]
        [DataObjectField(false, false, true, 16)]
        [BindColumn(4, "Token", "令牌", null, "nvarchar(16)", 0, 0, true)]
        public virtual String Token
        {
            get { return _Token; }
            set { if (OnPropertyChanging(__.Token, value)) { _Token = value; OnPropertyChanged(__.Token); } }
        }

        private String _Secret;
        /// <summary>客户保密信息</summary>
        [DisplayName("客户保密信息")]
        [Description("客户保密信息")]
        [DataObjectField(false, false, true, 16)]
        [BindColumn(5, "Secret", "客户保密信息", null, "nvarchar(16)", 0, 0, true)]
        public virtual String Secret
        {
            get { return _Secret; }
            set { if (OnPropertyChanging(__.Secret, value)) { _Secret = value; OnPropertyChanged(__.Secret); } }
        }

        private Boolean _AllowVisitQQUserInfo;
        /// <summary></summary>
        [DisplayName("AllowVisitQQUserInfo")]
        [Description("")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(6, "AllowVisitQQUserInfo", "", null, "bit", 0, 0, false)]
        public virtual Boolean AllowVisitQQUserInfo
        {
            get { return _AllowVisitQQUserInfo; }
            set { if (OnPropertyChanging(__.AllowVisitQQUserInfo, value)) { _AllowVisitQQUserInfo = value; OnPropertyChanged(__.AllowVisitQQUserInfo); } }
        }

        private Boolean _AllowPushFeed;
        /// <summary></summary>
        [DisplayName("AllowPushFeed")]
        [Description("")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(7, "AllowPushFeed", "", null, "bit", 0, 0, false)]
        public virtual Boolean AllowPushFeed
        {
            get { return _AllowPushFeed; }
            set { if (OnPropertyChanging(__.AllowPushFeed, value)) { _AllowPushFeed = value; OnPropertyChanged(__.AllowPushFeed); } }
        }

        private Boolean _IsSetPassword;
        /// <summary></summary>
        [DisplayName("IsSetPassword")]
        [Description("")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(8, "IsSetPassword", "", null, "bit", 0, 0, false)]
        public virtual Boolean IsSetPassword
        {
            get { return _IsSetPassword; }
            set { if (OnPropertyChanging(__.IsSetPassword, value)) { _IsSetPassword = value; OnPropertyChanged(__.IsSetPassword); } }
        }

        private String _CallbackInfo;
        /// <summary></summary>
        [DisplayName("CallbackInfo")]
        [Description("")]
        [DataObjectField(false, false, false, 100)]
        [BindColumn(9, "CallbackInfo", "", null, "nvarchar(100)", 0, 0, true)]
        public virtual String CallbackInfo
        {
            get { return _CallbackInfo; }
            set { if (OnPropertyChanging(__.CallbackInfo, value)) { _CallbackInfo = value; OnPropertyChanged(__.CallbackInfo); } }
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
                    case __.OpenId : return _OpenId;
                    case __.Uid : return _Uid;
                    case __.Token : return _Token;
                    case __.Secret : return _Secret;
                    case __.AllowVisitQQUserInfo : return _AllowVisitQQUserInfo;
                    case __.AllowPushFeed : return _AllowPushFeed;
                    case __.IsSetPassword : return _IsSetPassword;
                    case __.CallbackInfo : return _CallbackInfo;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.ID : _ID = Convert.ToInt32(value); break;
                    case __.OpenId : _OpenId = Convert.ToString(value); break;
                    case __.Uid : _Uid = Convert.ToInt32(value); break;
                    case __.Token : _Token = Convert.ToString(value); break;
                    case __.Secret : _Secret = Convert.ToString(value); break;
                    case __.AllowVisitQQUserInfo : _AllowVisitQQUserInfo = Convert.ToBoolean(value); break;
                    case __.AllowPushFeed : _AllowPushFeed = Convert.ToBoolean(value); break;
                    case __.IsSetPassword : _IsSetPassword = Convert.ToBoolean(value); break;
                    case __.CallbackInfo : _CallbackInfo = Convert.ToString(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得用户链接字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号</summary>
            public static readonly Field ID = FindByName(__.ID);

            ///<summary>邮箱账号</summary>
            public static readonly Field OpenId = FindByName(__.OpenId);

            ///<summary>用户编号</summary>
            public static readonly Field Uid = FindByName(__.Uid);

            ///<summary>令牌</summary>
            public static readonly Field Token = FindByName(__.Token);

            ///<summary>客户保密信息</summary>
            public static readonly Field Secret = FindByName(__.Secret);

            ///<summary></summary>
            public static readonly Field AllowVisitQQUserInfo = FindByName(__.AllowVisitQQUserInfo);

            ///<summary></summary>
            public static readonly Field AllowPushFeed = FindByName(__.AllowPushFeed);

            ///<summary></summary>
            public static readonly Field IsSetPassword = FindByName(__.IsSetPassword);

            ///<summary></summary>
            public static readonly Field CallbackInfo = FindByName(__.CallbackInfo);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得用户链接字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号</summary>
            public const String ID = "ID";

            ///<summary>邮箱账号</summary>
            public const String OpenId = "OpenId";

            ///<summary>用户编号</summary>
            public const String Uid = "Uid";

            ///<summary>令牌</summary>
            public const String Token = "Token";

            ///<summary>客户保密信息</summary>
            public const String Secret = "Secret";

            ///<summary></summary>
            public const String AllowVisitQQUserInfo = "AllowVisitQQUserInfo";

            ///<summary></summary>
            public const String AllowPushFeed = "AllowPushFeed";

            ///<summary></summary>
            public const String IsSetPassword = "IsSetPassword";

            ///<summary></summary>
            public const String CallbackInfo = "CallbackInfo";

        }
        #endregion
    }

    /// <summary>用户链接接口</summary>
    public partial interface IUserConnect
    {
        #region 属性
        /// <summary>编号</summary>
        Int32 ID { get; set; }

        /// <summary>邮箱账号</summary>
        String OpenId { get; set; }

        /// <summary>用户编号</summary>
        Int32 Uid { get; set; }

        /// <summary>令牌</summary>
        String Token { get; set; }

        /// <summary>客户保密信息</summary>
        String Secret { get; set; }

        /// <summary></summary>
        Boolean AllowVisitQQUserInfo { get; set; }

        /// <summary></summary>
        Boolean AllowPushFeed { get; set; }

        /// <summary></summary>
        Boolean IsSetPassword { get; set; }

        /// <summary></summary>
        String CallbackInfo { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}