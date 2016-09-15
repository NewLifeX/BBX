﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace BBX.Entity
{
    /// <summary>订单</summary>
    [Serializable]
    [DataObject]
    [Description("订单")]
    [BindIndex("IU_Order_Code", true, "Code")]
    [BindTable("Order", Description = "订单", ConnName = "BBX", DbType = DatabaseType.SqlServer)]
    public partial class Order : IOrder
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

        private String _Code;
        /// <summary>代码</summary>
        [DisplayName("代码")]
        [Description("代码")]
        [DataObjectField(false, false, false, 32)]
        [BindColumn(2, "Code", "代码", null, "nvarchar(32)", 0, 0, true)]
        public virtual String Code
        {
            get { return _Code; }
            set { if (OnPropertyChanging(__.Code, value)) { _Code = value; OnPropertyChanged(__.Code); } }
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

        private String _Buyer;
        /// <summary>购买者</summary>
        [DisplayName("购买者")]
        [Description("购买者")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(4, "Buyer", "购买者", null, "nvarchar(50)", 0, 0, true)]
        public virtual String Buyer
        {
            get { return _Buyer; }
            set { if (OnPropertyChanging(__.Buyer, value)) { _Buyer = value; OnPropertyChanged(__.Buyer); } }
        }

        private Int32 _PayType;
        /// <summary>付款类型</summary>
        [DisplayName("付款类型")]
        [Description("付款类型")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(5, "PayType", "付款类型", null, "int", 10, 0, false)]
        public virtual Int32 PayType
        {
            get { return _PayType; }
            set { if (OnPropertyChanging(__.PayType, value)) { _PayType = value; OnPropertyChanged(__.PayType); } }
        }

        private String _TradeNo;
        /// <summary>交易号</summary>
        [DisplayName("交易号")]
        [Description("交易号")]
        [DataObjectField(false, false, true, 32)]
        [BindColumn(6, "TradeNo", "交易号", null, "nvarchar(32)", 0, 0, true)]
        public virtual String TradeNo
        {
            get { return _TradeNo; }
            set { if (OnPropertyChanging(__.TradeNo, value)) { _TradeNo = value; OnPropertyChanged(__.TradeNo); } }
        }

        private Decimal _Price;
        /// <summary>价格</summary>
        [DisplayName("价格")]
        [Description("价格")]
        [DataObjectField(false, false, true, 19)]
        [BindColumn(7, "Price", "价格", null, "money", 0, 0, false)]
        public virtual Decimal Price
        {
            get { return _Price; }
            set { if (OnPropertyChanging(__.Price, value)) { _Price = value; OnPropertyChanged(__.Price); } }
        }

        private Int32 _Status;
        /// <summary>订单状态1成功0未结算2失败</summary>
        [DisplayName("订单状态1成功0未结算2失败")]
        [Description("订单状态1成功0未结算2失败")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(8, "Status", "订单状态1成功0未结算2失败", null, "int", 10, 0, false)]
        public virtual Int32 Status
        {
            get { return _Status; }
            set { if (OnPropertyChanging(__.Status, value)) { _Status = value; OnPropertyChanged(__.Status); } }
        }

        private DateTime _CreatedTime;
        /// <summary>创建时间</summary>
        [DisplayName("创建时间")]
        [Description("创建时间")]
        [DataObjectField(false, false, true, 3)]
        [BindColumn(9, "CreatedTime", "创建时间", null, "datetime", 3, 0, false)]
        public virtual DateTime CreatedTime
        {
            get { return _CreatedTime; }
            set { if (OnPropertyChanging(__.CreatedTime, value)) { _CreatedTime = value; OnPropertyChanged(__.CreatedTime); } }
        }

        private DateTime _ConfirmedTime;
        /// <summary>确认时间</summary>
        [DisplayName("确认时间")]
        [Description("确认时间")]
        [DataObjectField(false, false, true, 3)]
        [BindColumn(10, "ConfirmedTime", "确认时间", null, "datetime", 3, 0, false)]
        public virtual DateTime ConfirmedTime
        {
            get { return _ConfirmedTime; }
            set { if (OnPropertyChanging(__.ConfirmedTime, value)) { _ConfirmedTime = value; OnPropertyChanged(__.ConfirmedTime); } }
        }

        private Int32 _Credit;
        /// <summary>信用值</summary>
        [DisplayName("信用值")]
        [Description("信用值")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(11, "Credit", "信用值", null, "int", 10, 0, false)]
        public virtual Int32 Credit
        {
            get { return _Credit; }
            set { if (OnPropertyChanging(__.Credit, value)) { _Credit = value; OnPropertyChanged(__.Credit); } }
        }

        private Int32 _Amount;
        /// <summary>充值金额</summary>
        [DisplayName("充值金额")]
        [Description("充值金额")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(12, "Amount", "充值金额", null, "int", 10, 0, false)]
        public virtual Int32 Amount
        {
            get { return _Amount; }
            set { if (OnPropertyChanging(__.Amount, value)) { _Amount = value; OnPropertyChanged(__.Amount); } }
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
                    case __.Code : return _Code;
                    case __.Uid : return _Uid;
                    case __.Buyer : return _Buyer;
                    case __.PayType : return _PayType;
                    case __.TradeNo : return _TradeNo;
                    case __.Price : return _Price;
                    case __.Status : return _Status;
                    case __.CreatedTime : return _CreatedTime;
                    case __.ConfirmedTime : return _ConfirmedTime;
                    case __.Credit : return _Credit;
                    case __.Amount : return _Amount;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.ID : _ID = Convert.ToInt32(value); break;
                    case __.Code : _Code = Convert.ToString(value); break;
                    case __.Uid : _Uid = Convert.ToInt32(value); break;
                    case __.Buyer : _Buyer = Convert.ToString(value); break;
                    case __.PayType : _PayType = Convert.ToInt32(value); break;
                    case __.TradeNo : _TradeNo = Convert.ToString(value); break;
                    case __.Price : _Price = Convert.ToDecimal(value); break;
                    case __.Status : _Status = Convert.ToInt32(value); break;
                    case __.CreatedTime : _CreatedTime = Convert.ToDateTime(value); break;
                    case __.ConfirmedTime : _ConfirmedTime = Convert.ToDateTime(value); break;
                    case __.Credit : _Credit = Convert.ToInt32(value); break;
                    case __.Amount : _Amount = Convert.ToInt32(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得订单字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号</summary>
            public static readonly Field ID = FindByName(__.ID);

            ///<summary>代码</summary>
            public static readonly Field Code = FindByName(__.Code);

            ///<summary>用户编号</summary>
            public static readonly Field Uid = FindByName(__.Uid);

            ///<summary>购买者</summary>
            public static readonly Field Buyer = FindByName(__.Buyer);

            ///<summary>付款类型</summary>
            public static readonly Field PayType = FindByName(__.PayType);

            ///<summary>交易号</summary>
            public static readonly Field TradeNo = FindByName(__.TradeNo);

            ///<summary>价格</summary>
            public static readonly Field Price = FindByName(__.Price);

            ///<summary>订单状态1成功0未结算2失败</summary>
            public static readonly Field Status = FindByName(__.Status);

            ///<summary>创建时间</summary>
            public static readonly Field CreatedTime = FindByName(__.CreatedTime);

            ///<summary>确认时间</summary>
            public static readonly Field ConfirmedTime = FindByName(__.ConfirmedTime);

            ///<summary>信用值</summary>
            public static readonly Field Credit = FindByName(__.Credit);

            ///<summary>充值金额</summary>
            public static readonly Field Amount = FindByName(__.Amount);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得订单字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号</summary>
            public const String ID = "ID";

            ///<summary>代码</summary>
            public const String Code = "Code";

            ///<summary>用户编号</summary>
            public const String Uid = "Uid";

            ///<summary>购买者</summary>
            public const String Buyer = "Buyer";

            ///<summary>付款类型</summary>
            public const String PayType = "PayType";

            ///<summary>交易号</summary>
            public const String TradeNo = "TradeNo";

            ///<summary>价格</summary>
            public const String Price = "Price";

            ///<summary>订单状态1成功0未结算2失败</summary>
            public const String Status = "Status";

            ///<summary>创建时间</summary>
            public const String CreatedTime = "CreatedTime";

            ///<summary>确认时间</summary>
            public const String ConfirmedTime = "ConfirmedTime";

            ///<summary>信用值</summary>
            public const String Credit = "Credit";

            ///<summary>充值金额</summary>
            public const String Amount = "Amount";

        }
        #endregion
    }

    /// <summary>订单接口</summary>
    public partial interface IOrder
    {
        #region 属性
        /// <summary>编号</summary>
        Int32 ID { get; set; }

        /// <summary>代码</summary>
        String Code { get; set; }

        /// <summary>用户编号</summary>
        Int32 Uid { get; set; }

        /// <summary>购买者</summary>
        String Buyer { get; set; }

        /// <summary>付款类型</summary>
        Int32 PayType { get; set; }

        /// <summary>交易号</summary>
        String TradeNo { get; set; }

        /// <summary>价格</summary>
        Decimal Price { get; set; }

        /// <summary>订单状态1成功0未结算2失败</summary>
        Int32 Status { get; set; }

        /// <summary>创建时间</summary>
        DateTime CreatedTime { get; set; }

        /// <summary>确认时间</summary>
        DateTime ConfirmedTime { get; set; }

        /// <summary>信用值</summary>
        Int32 Credit { get; set; }

        /// <summary>充值金额</summary>
        Int32 Amount { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}