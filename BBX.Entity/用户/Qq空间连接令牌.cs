﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace BBX.Entity
{
    /// <summary>Qq空间连接令牌</summary>
    [Serializable]
    [DataObject]
    [Description("Qq空间连接令牌")]
    [BindIndex("IU_QzoneConnectToken_Uid", true, "Uid")]
    [BindIndex("IU_QzoneConnectToken_OpenId", true, "OpenId")]
    [BindTable("QzoneConnectToken", Description = "Qq空间连接令牌", ConnName = "BBX", DbType = DatabaseType.SqlServer)]
    public partial class QzoneConnectToken : IQzoneConnectToken
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
        [DataObjectField(false, false, false, 50)]
        [BindColumn(2, "OpenId", "邮箱账号", null, "nvarchar(50)", 0, 0, true)]
        public virtual String OpenId
        {
            get { return _OpenId; }
            set { if (OnPropertyChanging(__.OpenId, value)) { _OpenId = value; OnPropertyChanged(__.OpenId); } }
        }

        private Int32 _Uid;
        /// <summary>用户ID</summary>
        [DisplayName("用户ID")]
        [Description("用户ID")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(3, "Uid", "用户ID", null, "int", 10, 0, false)]
        public virtual Int32 Uid
        {
            get { return _Uid; }
            set { if (OnPropertyChanging(__.Uid, value)) { _Uid = value; OnPropertyChanged(__.Uid); } }
        }

        private String _AccessToken;
        /// <summary>访问令牌</summary>
        [DisplayName("访问令牌")]
        [Description("访问令牌")]
        [DataObjectField(false, false, false, 50)]
        [BindColumn(4, "AccessToken", "访问令牌", null, "nvarchar(50)", 0, 0, true)]
        public virtual String AccessToken
        {
            get { return _AccessToken; }
            set { if (OnPropertyChanging(__.AccessToken, value)) { _AccessToken = value; OnPropertyChanged(__.AccessToken); } }
        }

        private DateTime _ExpiresAt;
        /// <summary>过期在</summary>
        [DisplayName("过期在")]
        [Description("过期在")]
        [DataObjectField(false, false, false, 3)]
        [BindColumn(5, "ExpiresAt", "过期在", null, "datetime", 3, 0, false)]
        public virtual DateTime ExpiresAt
        {
            get { return _ExpiresAt; }
            set { if (OnPropertyChanging(__.ExpiresAt, value)) { _ExpiresAt = value; OnPropertyChanged(__.ExpiresAt); } }
        }

        private Boolean _PushToQzone;
        /// <summary>推送表自Qq空间</summary>
        [DisplayName("推送表自Qq空间")]
        [Description("推送表自Qq空间")]
        [DataObjectField(false, false, false, 1)]
        [BindColumn(6, "PushToQzone", "推送表自Qq空间", null, "bit", 0, 0, false)]
        public virtual Boolean PushToQzone
        {
            get { return _PushToQzone; }
            set { if (OnPropertyChanging(__.PushToQzone, value)) { _PushToQzone = value; OnPropertyChanged(__.PushToQzone); } }
        }

        private Boolean _PushToWeibo;
        /// <summary>推送表自微博</summary>
        [DisplayName("推送表自微博")]
        [Description("推送表自微博")]
        [DataObjectField(false, false, false, 1)]
        [BindColumn(7, "PushToWeibo", "推送表自微博", null, "bit", 0, 0, false)]
        public virtual Boolean PushToWeibo
        {
            get { return _PushToWeibo; }
            set { if (OnPropertyChanging(__.PushToWeibo, value)) { _PushToWeibo = value; OnPropertyChanged(__.PushToWeibo); } }
        }

        private Boolean _IsPure;
        /// <summary>是纯</summary>
        [DisplayName("是纯")]
        [Description("是纯")]
        [DataObjectField(false, false, false, 1)]
        [BindColumn(8, "IsPure", "是纯", null, "bit", 0, 0, false)]
        public virtual Boolean IsPure
        {
            get { return _IsPure; }
            set { if (OnPropertyChanging(__.IsPure, value)) { _IsPure = value; OnPropertyChanged(__.IsPure); } }
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
                    case __.AccessToken : return _AccessToken;
                    case __.ExpiresAt : return _ExpiresAt;
                    case __.PushToQzone : return _PushToQzone;
                    case __.PushToWeibo : return _PushToWeibo;
                    case __.IsPure : return _IsPure;
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
                    case __.AccessToken : _AccessToken = Convert.ToString(value); break;
                    case __.ExpiresAt : _ExpiresAt = Convert.ToDateTime(value); break;
                    case __.PushToQzone : _PushToQzone = Convert.ToBoolean(value); break;
                    case __.PushToWeibo : _PushToWeibo = Convert.ToBoolean(value); break;
                    case __.IsPure : _IsPure = Convert.ToBoolean(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得Qq空间连接令牌字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号</summary>
            public static readonly Field ID = FindByName(__.ID);

            ///<summary>邮箱账号</summary>
            public static readonly Field OpenId = FindByName(__.OpenId);

            ///<summary>用户ID</summary>
            public static readonly Field Uid = FindByName(__.Uid);

            ///<summary>访问令牌</summary>
            public static readonly Field AccessToken = FindByName(__.AccessToken);

            ///<summary>过期在</summary>
            public static readonly Field ExpiresAt = FindByName(__.ExpiresAt);

            ///<summary>推送表自Qq空间</summary>
            public static readonly Field PushToQzone = FindByName(__.PushToQzone);

            ///<summary>推送表自微博</summary>
            public static readonly Field PushToWeibo = FindByName(__.PushToWeibo);

            ///<summary>是纯</summary>
            public static readonly Field IsPure = FindByName(__.IsPure);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得Qq空间连接令牌字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号</summary>
            public const String ID = "ID";

            ///<summary>邮箱账号</summary>
            public const String OpenId = "OpenId";

            ///<summary>用户ID</summary>
            public const String Uid = "Uid";

            ///<summary>访问令牌</summary>
            public const String AccessToken = "AccessToken";

            ///<summary>过期在</summary>
            public const String ExpiresAt = "ExpiresAt";

            ///<summary>推送表自Qq空间</summary>
            public const String PushToQzone = "PushToQzone";

            ///<summary>推送表自微博</summary>
            public const String PushToWeibo = "PushToWeibo";

            ///<summary>是纯</summary>
            public const String IsPure = "IsPure";

        }
        #endregion
    }

    /// <summary>Qq空间连接令牌接口</summary>
    public partial interface IQzoneConnectToken
    {
        #region 属性
        /// <summary>编号</summary>
        Int32 ID { get; set; }

        /// <summary>邮箱账号</summary>
        String OpenId { get; set; }

        /// <summary>用户ID</summary>
        Int32 Uid { get; set; }

        /// <summary>访问令牌</summary>
        String AccessToken { get; set; }

        /// <summary>过期在</summary>
        DateTime ExpiresAt { get; set; }

        /// <summary>推送表自Qq空间</summary>
        Boolean PushToQzone { get; set; }

        /// <summary>推送表自微博</summary>
        Boolean PushToWeibo { get; set; }

        /// <summary>是纯</summary>
        Boolean IsPure { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}