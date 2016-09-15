﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace BBX.Entity
{
    /// <summary>附件支付日志</summary>
    [Serializable]
    [DataObject]
    [Description("附件支付日志")]
    [BindIndex("IX_AttachPaymentLog_uid_aid", false, "uid,aid")]
    [BindTable("AttachPaymentLog", Description = "附件支付日志", ConnName = "BBX", DbType = DatabaseType.SqlServer)]
    public partial class AttachPaymentLog : IAttachPaymentLog
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

        private Int32 _Aid;
        /// <summary>附件编号</summary>
        [DisplayName("附件编号")]
        [Description("附件编号")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(4, "Aid", "附件编号", null, "int", 10, 0, false)]
        public virtual Int32 Aid
        {
            get { return _Aid; }
            set { if (OnPropertyChanging(__.Aid, value)) { _Aid = value; OnPropertyChanged(__.Aid); } }
        }

        private Int32 _AuthorID;
        /// <summary>作者编号</summary>
        [DisplayName("作者编号")]
        [Description("作者编号")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(5, "AuthorID", "作者编号", null, "int", 10, 0, false)]
        public virtual Int32 AuthorID
        {
            get { return _AuthorID; }
            set { if (OnPropertyChanging(__.AuthorID, value)) { _AuthorID = value; OnPropertyChanged(__.AuthorID); } }
        }

        private DateTime _PostDateTime;
        /// <summary>发送时间</summary>
        [DisplayName("发送时间")]
        [Description("发送时间")]
        [DataObjectField(false, false, false, 3)]
        [BindColumn(6, "PostDateTime", "发送时间", null, "datetime", 3, 0, false)]
        public virtual DateTime PostDateTime
        {
            get { return _PostDateTime; }
            set { if (OnPropertyChanging(__.PostDateTime, value)) { _PostDateTime = value; OnPropertyChanged(__.PostDateTime); } }
        }

        private Int32 _Amount;
        /// <summary>充值金额</summary>
        [DisplayName("充值金额")]
        [Description("充值金额")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(7, "Amount", "充值金额", null, "int", 10, 0, false)]
        public virtual Int32 Amount
        {
            get { return _Amount; }
            set { if (OnPropertyChanging(__.Amount, value)) { _Amount = value; OnPropertyChanged(__.Amount); } }
        }

        private Int32 _NetAmount;
        /// <summary></summary>
        [DisplayName("NetAmount")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(8, "NetAmount", "", null, "int", 10, 0, false)]
        public virtual Int32 NetAmount
        {
            get { return _NetAmount; }
            set { if (OnPropertyChanging(__.NetAmount, value)) { _NetAmount = value; OnPropertyChanged(__.NetAmount); } }
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
                    case __.Aid : return _Aid;
                    case __.AuthorID : return _AuthorID;
                    case __.PostDateTime : return _PostDateTime;
                    case __.Amount : return _Amount;
                    case __.NetAmount : return _NetAmount;
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
                    case __.Aid : _Aid = Convert.ToInt32(value); break;
                    case __.AuthorID : _AuthorID = Convert.ToInt32(value); break;
                    case __.PostDateTime : _PostDateTime = Convert.ToDateTime(value); break;
                    case __.Amount : _Amount = Convert.ToInt32(value); break;
                    case __.NetAmount : _NetAmount = Convert.ToInt32(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得附件支付日志字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号</summary>
            public static readonly Field ID = FindByName(__.ID);

            ///<summary>用户编号</summary>
            public static readonly Field Uid = FindByName(__.Uid);

            ///<summary>登录账户</summary>
            public static readonly Field UserName = FindByName(__.UserName);

            ///<summary>附件编号</summary>
            public static readonly Field Aid = FindByName(__.Aid);

            ///<summary>作者编号</summary>
            public static readonly Field AuthorID = FindByName(__.AuthorID);

            ///<summary>发送时间</summary>
            public static readonly Field PostDateTime = FindByName(__.PostDateTime);

            ///<summary>充值金额</summary>
            public static readonly Field Amount = FindByName(__.Amount);

            ///<summary></summary>
            public static readonly Field NetAmount = FindByName(__.NetAmount);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得附件支付日志字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号</summary>
            public const String ID = "ID";

            ///<summary>用户编号</summary>
            public const String Uid = "Uid";

            ///<summary>登录账户</summary>
            public const String UserName = "UserName";

            ///<summary>附件编号</summary>
            public const String Aid = "Aid";

            ///<summary>作者编号</summary>
            public const String AuthorID = "AuthorID";

            ///<summary>发送时间</summary>
            public const String PostDateTime = "PostDateTime";

            ///<summary>充值金额</summary>
            public const String Amount = "Amount";

            ///<summary></summary>
            public const String NetAmount = "NetAmount";

        }
        #endregion
    }

    /// <summary>附件支付日志接口</summary>
    public partial interface IAttachPaymentLog
    {
        #region 属性
        /// <summary>编号</summary>
        Int32 ID { get; set; }

        /// <summary>用户编号</summary>
        Int32 Uid { get; set; }

        /// <summary>登录账户</summary>
        String UserName { get; set; }

        /// <summary>附件编号</summary>
        Int32 Aid { get; set; }

        /// <summary>作者编号</summary>
        Int32 AuthorID { get; set; }

        /// <summary>发送时间</summary>
        DateTime PostDateTime { get; set; }

        /// <summary>充值金额</summary>
        Int32 Amount { get; set; }

        /// <summary></summary>
        Int32 NetAmount { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}