﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace BBX.Entity
{
    /// <summary>支付日志</summary>
    [Serializable]
    [DataObject]
    [Description("支付日志")]
    [BindTable("PaymentLog", Description = "支付日志", ConnName = "BBX", DbType = DatabaseType.SqlServer)]
    public partial class PaymentLog : IPaymentLog
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

        private Int32 _Tid;
        /// <summary>主题编号</summary>
        [DisplayName("主题编号")]
        [Description("主题编号")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(3, "Tid", "主题编号", null, "int", 10, 0, false)]
        public virtual Int32 Tid
        {
            get { return _Tid; }
            set { if (OnPropertyChanging(__.Tid, value)) { _Tid = value; OnPropertyChanged(__.Tid); } }
        }

        private Int32 _AuthorID;
        /// <summary>作者编号</summary>
        [DisplayName("作者编号")]
        [Description("作者编号")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(4, "AuthorID", "作者编号", null, "int", 10, 0, false)]
        public virtual Int32 AuthorID
        {
            get { return _AuthorID; }
            set { if (OnPropertyChanging(__.AuthorID, value)) { _AuthorID = value; OnPropertyChanged(__.AuthorID); } }
        }

        private DateTime _BuyDate;
        /// <summary>购买时间</summary>
        [DisplayName("购买时间")]
        [Description("购买时间")]
        [DataObjectField(false, false, false, 3)]
        [BindColumn(5, "BuyDate", "购买时间", null, "datetime", 3, 0, false)]
        public virtual DateTime BuyDate
        {
            get { return _BuyDate; }
            set { if (OnPropertyChanging(__.BuyDate, value)) { _BuyDate = value; OnPropertyChanged(__.BuyDate); } }
        }

        private Int16 _Amount;
        /// <summary>充值金额</summary>
        [DisplayName("充值金额")]
        [Description("充值金额")]
        [DataObjectField(false, false, false, 5)]
        [BindColumn(6, "Amount", "充值金额", null, "smallint", 5, 0, false)]
        public virtual Int16 Amount
        {
            get { return _Amount; }
            set { if (OnPropertyChanging(__.Amount, value)) { _Amount = value; OnPropertyChanged(__.Amount); } }
        }

        private Int16 _Netamount;
        /// <summary></summary>
        [DisplayName("Netamount")]
        [Description("")]
        [DataObjectField(false, false, false, 5)]
        [BindColumn(7, "Netamount", "", null, "smallint", 5, 0, false)]
        public virtual Int16 Netamount
        {
            get { return _Netamount; }
            set { if (OnPropertyChanging(__.Netamount, value)) { _Netamount = value; OnPropertyChanged(__.Netamount); } }
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
                    case __.Tid : return _Tid;
                    case __.AuthorID : return _AuthorID;
                    case __.BuyDate : return _BuyDate;
                    case __.Amount : return _Amount;
                    case __.Netamount : return _Netamount;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.ID : _ID = Convert.ToInt32(value); break;
                    case __.Uid : _Uid = Convert.ToInt32(value); break;
                    case __.Tid : _Tid = Convert.ToInt32(value); break;
                    case __.AuthorID : _AuthorID = Convert.ToInt32(value); break;
                    case __.BuyDate : _BuyDate = Convert.ToDateTime(value); break;
                    case __.Amount : _Amount = Convert.ToInt16(value); break;
                    case __.Netamount : _Netamount = Convert.ToInt16(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得支付日志字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号</summary>
            public static readonly Field ID = FindByName(__.ID);

            ///<summary>用户编号</summary>
            public static readonly Field Uid = FindByName(__.Uid);

            ///<summary>主题编号</summary>
            public static readonly Field Tid = FindByName(__.Tid);

            ///<summary>作者编号</summary>
            public static readonly Field AuthorID = FindByName(__.AuthorID);

            ///<summary>购买时间</summary>
            public static readonly Field BuyDate = FindByName(__.BuyDate);

            ///<summary>充值金额</summary>
            public static readonly Field Amount = FindByName(__.Amount);

            ///<summary></summary>
            public static readonly Field Netamount = FindByName(__.Netamount);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得支付日志字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号</summary>
            public const String ID = "ID";

            ///<summary>用户编号</summary>
            public const String Uid = "Uid";

            ///<summary>主题编号</summary>
            public const String Tid = "Tid";

            ///<summary>作者编号</summary>
            public const String AuthorID = "AuthorID";

            ///<summary>购买时间</summary>
            public const String BuyDate = "BuyDate";

            ///<summary>充值金额</summary>
            public const String Amount = "Amount";

            ///<summary></summary>
            public const String Netamount = "Netamount";

        }
        #endregion
    }

    /// <summary>支付日志接口</summary>
    public partial interface IPaymentLog
    {
        #region 属性
        /// <summary>编号</summary>
        Int32 ID { get; set; }

        /// <summary>用户编号</summary>
        Int32 Uid { get; set; }

        /// <summary>主题编号</summary>
        Int32 Tid { get; set; }

        /// <summary>作者编号</summary>
        Int32 AuthorID { get; set; }

        /// <summary>购买时间</summary>
        DateTime BuyDate { get; set; }

        /// <summary>充值金额</summary>
        Int16 Amount { get; set; }

        /// <summary></summary>
        Int16 Netamount { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}