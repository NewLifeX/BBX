﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace BBX.Entity
{
    /// <summary>威望日志</summary>
    [Serializable]
    [DataObject]
    [Description("威望日志")]
    [BindTable("CreditsLog", Description = "威望日志", ConnName = "BBX", DbType = DatabaseType.SqlServer)]
    public partial class CreditsLog : ICreditsLog
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

        private Int32 _FromTo;
        /// <summary>来去</summary>
        [DisplayName("来去")]
        [Description("来去")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(3, "FromTo", "来去", null, "int", 10, 0, false)]
        public virtual Int32 FromTo
        {
            get { return _FromTo; }
            set { if (OnPropertyChanging(__.FromTo, value)) { _FromTo = value; OnPropertyChanged(__.FromTo); } }
        }

        private Int32 _SendCredits;
        /// <summary>发送威望</summary>
        [DisplayName("发送威望")]
        [Description("发送威望")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(4, "SendCredits", "发送威望", null, "int", 10, 0, false)]
        public virtual Int32 SendCredits
        {
            get { return _SendCredits; }
            set { if (OnPropertyChanging(__.SendCredits, value)) { _SendCredits = value; OnPropertyChanged(__.SendCredits); } }
        }

        private Int32 _ReceiveCredits;
        /// <summary>接收威望</summary>
        [DisplayName("接收威望")]
        [Description("接收威望")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(5, "ReceiveCredits", "接收威望", null, "int", 10, 0, false)]
        public virtual Int32 ReceiveCredits
        {
            get { return _ReceiveCredits; }
            set { if (OnPropertyChanging(__.ReceiveCredits, value)) { _ReceiveCredits = value; OnPropertyChanged(__.ReceiveCredits); } }
        }

        private Double _Send;
        /// <summary>发送</summary>
        [DisplayName("发送")]
        [Description("发送")]
        [DataObjectField(false, false, true, 53)]
        [BindColumn(6, "Send", "发送", null, "float", 0, 0, false)]
        public virtual Double Send
        {
            get { return _Send; }
            set { if (OnPropertyChanging(__.Send, value)) { _Send = value; OnPropertyChanged(__.Send); } }
        }

        private Double _Receive;
        /// <summary>接收</summary>
        [DisplayName("接收")]
        [Description("接收")]
        [DataObjectField(false, false, true, 53)]
        [BindColumn(7, "Receive", "接收", null, "float", 0, 0, false)]
        public virtual Double Receive
        {
            get { return _Receive; }
            set { if (OnPropertyChanging(__.Receive, value)) { _Receive = value; OnPropertyChanged(__.Receive); } }
        }

        private DateTime _PayDate;
        /// <summary>支付时间</summary>
        [DisplayName("支付时间")]
        [Description("支付时间")]
        [DataObjectField(false, false, true, 3)]
        [BindColumn(8, "PayDate", "支付时间", null, "datetime", 3, 0, false)]
        public virtual DateTime PayDate
        {
            get { return _PayDate; }
            set { if (OnPropertyChanging(__.PayDate, value)) { _PayDate = value; OnPropertyChanged(__.PayDate); } }
        }

        private Int32 _Operation;
        /// <summary>操作</summary>
        [DisplayName("操作")]
        [Description("操作")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(9, "Operation", "操作", null, "int", 10, 0, false)]
        public virtual Int32 Operation
        {
            get { return _Operation; }
            set { if (OnPropertyChanging(__.Operation, value)) { _Operation = value; OnPropertyChanged(__.Operation); } }
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
                    case __.FromTo : return _FromTo;
                    case __.SendCredits : return _SendCredits;
                    case __.ReceiveCredits : return _ReceiveCredits;
                    case __.Send : return _Send;
                    case __.Receive : return _Receive;
                    case __.PayDate : return _PayDate;
                    case __.Operation : return _Operation;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.ID : _ID = Convert.ToInt32(value); break;
                    case __.Uid : _Uid = Convert.ToInt32(value); break;
                    case __.FromTo : _FromTo = Convert.ToInt32(value); break;
                    case __.SendCredits : _SendCredits = Convert.ToInt32(value); break;
                    case __.ReceiveCredits : _ReceiveCredits = Convert.ToInt32(value); break;
                    case __.Send : _Send = Convert.ToDouble(value); break;
                    case __.Receive : _Receive = Convert.ToDouble(value); break;
                    case __.PayDate : _PayDate = Convert.ToDateTime(value); break;
                    case __.Operation : _Operation = Convert.ToInt32(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得威望日志字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号</summary>
            public static readonly Field ID = FindByName(__.ID);

            ///<summary>用户编号</summary>
            public static readonly Field Uid = FindByName(__.Uid);

            ///<summary>来去</summary>
            public static readonly Field FromTo = FindByName(__.FromTo);

            ///<summary>发送威望</summary>
            public static readonly Field SendCredits = FindByName(__.SendCredits);

            ///<summary>接收威望</summary>
            public static readonly Field ReceiveCredits = FindByName(__.ReceiveCredits);

            ///<summary>发送</summary>
            public static readonly Field Send = FindByName(__.Send);

            ///<summary>接收</summary>
            public static readonly Field Receive = FindByName(__.Receive);

            ///<summary>支付时间</summary>
            public static readonly Field PayDate = FindByName(__.PayDate);

            ///<summary>操作</summary>
            public static readonly Field Operation = FindByName(__.Operation);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得威望日志字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号</summary>
            public const String ID = "ID";

            ///<summary>用户编号</summary>
            public const String Uid = "Uid";

            ///<summary>来去</summary>
            public const String FromTo = "FromTo";

            ///<summary>发送威望</summary>
            public const String SendCredits = "SendCredits";

            ///<summary>接收威望</summary>
            public const String ReceiveCredits = "ReceiveCredits";

            ///<summary>发送</summary>
            public const String Send = "Send";

            ///<summary>接收</summary>
            public const String Receive = "Receive";

            ///<summary>支付时间</summary>
            public const String PayDate = "PayDate";

            ///<summary>操作</summary>
            public const String Operation = "Operation";

        }
        #endregion
    }

    /// <summary>威望日志接口</summary>
    public partial interface ICreditsLog
    {
        #region 属性
        /// <summary>编号</summary>
        Int32 ID { get; set; }

        /// <summary>用户编号</summary>
        Int32 Uid { get; set; }

        /// <summary>来去</summary>
        Int32 FromTo { get; set; }

        /// <summary>发送威望</summary>
        Int32 SendCredits { get; set; }

        /// <summary>接收威望</summary>
        Int32 ReceiveCredits { get; set; }

        /// <summary>发送</summary>
        Double Send { get; set; }

        /// <summary>接收</summary>
        Double Receive { get; set; }

        /// <summary>支付时间</summary>
        DateTime PayDate { get; set; }

        /// <summary>操作</summary>
        Int32 Operation { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}