﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace BBX.Entity
{
    /// <summary>推送日志</summary>
    [Serializable]
    [DataObject]
    [Description("推送日志")]
    [BindTable("PushfeedLog", Description = "推送日志", ConnName = "BBX", DbType = DatabaseType.SqlServer)]
    public partial class PushfeedLog : IPushfeedLog
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

        private Int32 _Tid;
        /// <summary>帖子编号</summary>
        [DisplayName("帖子编号")]
        [Description("帖子编号")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(2, "Tid", "帖子编号", null, "int", 10, 0, false)]
        public virtual Int32 Tid
        {
            get { return _Tid; }
            set { if (OnPropertyChanging(__.Tid, value)) { _Tid = value; OnPropertyChanged(__.Tid); } }
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

        private String _AuthorToken;
        /// <summary></summary>
        [DisplayName("AuthorToken")]
        [Description("")]
        [DataObjectField(false, false, true, 16)]
        [BindColumn(4, "AuthorToken", "", null, "nvarchar(16)", 0, 0, true)]
        public virtual String AuthorToken
        {
            get { return _AuthorToken; }
            set { if (OnPropertyChanging(__.AuthorToken, value)) { _AuthorToken = value; OnPropertyChanged(__.AuthorToken); } }
        }

        private String _AuthorSecret;
        /// <summary></summary>
        [DisplayName("AuthorSecret")]
        [Description("")]
        [DataObjectField(false, false, true, 16)]
        [BindColumn(5, "AuthorSecret", "", null, "nvarchar(16)", 0, 0, true)]
        public virtual String AuthorSecret
        {
            get { return _AuthorSecret; }
            set { if (OnPropertyChanging(__.AuthorSecret, value)) { _AuthorSecret = value; OnPropertyChanged(__.AuthorSecret); } }
        }

        private DateTime _PushDate;
        /// <summary></summary>
        [DisplayName("PushDate")]
        [Description("")]
        [DataObjectField(false, false, true, 3)]
        [BindColumn(6, "PushDate", "", null, "datetime", 3, 0, false)]
        public virtual DateTime PushDate
        {
            get { return _PushDate; }
            set { if (OnPropertyChanging(__.PushDate, value)) { _PushDate = value; OnPropertyChanged(__.PushDate); } }
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
                    case __.Tid : return _Tid;
                    case __.Uid : return _Uid;
                    case __.AuthorToken : return _AuthorToken;
                    case __.AuthorSecret : return _AuthorSecret;
                    case __.PushDate : return _PushDate;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.ID : _ID = Convert.ToInt32(value); break;
                    case __.Tid : _Tid = Convert.ToInt32(value); break;
                    case __.Uid : _Uid = Convert.ToInt32(value); break;
                    case __.AuthorToken : _AuthorToken = Convert.ToString(value); break;
                    case __.AuthorSecret : _AuthorSecret = Convert.ToString(value); break;
                    case __.PushDate : _PushDate = Convert.ToDateTime(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得推送日志字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号</summary>
            public static readonly Field ID = FindByName(__.ID);

            ///<summary>帖子编号</summary>
            public static readonly Field Tid = FindByName(__.Tid);

            ///<summary>用户编号</summary>
            public static readonly Field Uid = FindByName(__.Uid);

            ///<summary></summary>
            public static readonly Field AuthorToken = FindByName(__.AuthorToken);

            ///<summary></summary>
            public static readonly Field AuthorSecret = FindByName(__.AuthorSecret);

            ///<summary></summary>
            public static readonly Field PushDate = FindByName(__.PushDate);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得推送日志字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号</summary>
            public const String ID = "ID";

            ///<summary>帖子编号</summary>
            public const String Tid = "Tid";

            ///<summary>用户编号</summary>
            public const String Uid = "Uid";

            ///<summary></summary>
            public const String AuthorToken = "AuthorToken";

            ///<summary></summary>
            public const String AuthorSecret = "AuthorSecret";

            ///<summary></summary>
            public const String PushDate = "PushDate";

        }
        #endregion
    }

    /// <summary>推送日志接口</summary>
    public partial interface IPushfeedLog
    {
        #region 属性
        /// <summary>编号</summary>
        Int32 ID { get; set; }

        /// <summary>帖子编号</summary>
        Int32 Tid { get; set; }

        /// <summary>用户编号</summary>
        Int32 Uid { get; set; }

        /// <summary></summary>
        String AuthorToken { get; set; }

        /// <summary></summary>
        String AuthorSecret { get; set; }

        /// <summary></summary>
        DateTime PushDate { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}