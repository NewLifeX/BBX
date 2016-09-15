﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace BBX.Entity
{
    /// <summary>位置</summary>
    [Serializable]
    [DataObject]
    [Description("位置")]
    [BindTable("Location", Description = "位置", ConnName = "BBX", DbType = DatabaseType.SqlServer)]
    public partial class Location : ILocation
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

        private String _City;
        /// <summary>收货人的城市</summary>
        [DisplayName("收货人的城市")]
        [Description("收货人的城市")]
        [DataObjectField(false, false, false, 50)]
        [BindColumn(2, "City", "收货人的城市", null, "nvarchar(50)", 0, 0, true)]
        public virtual String City
        {
            get { return _City; }
            set { if (OnPropertyChanging(__.City, value)) { _City = value; OnPropertyChanged(__.City); } }
        }

        private String _State;
        /// <summary>结算状态</summary>
        [DisplayName("结算状态")]
        [Description("结算状态")]
        [DataObjectField(false, false, false, 50)]
        [BindColumn(3, "State", "结算状态", null, "nvarchar(50)", 0, 0, true)]
        public virtual String State
        {
            get { return _State; }
            set { if (OnPropertyChanging(__.State, value)) { _State = value; OnPropertyChanged(__.State); } }
        }

        private String _Country;
        /// <summary>国家</summary>
        [DisplayName("国家")]
        [Description("国家")]
        [DataObjectField(false, false, false, 50)]
        [BindColumn(4, "Country", "国家", null, "nvarchar(50)", 0, 0, true)]
        public virtual String Country
        {
            get { return _Country; }
            set { if (OnPropertyChanging(__.Country, value)) { _Country = value; OnPropertyChanged(__.Country); } }
        }

        private String _ZipCode;
        /// <summary>邮编</summary>
        [DisplayName("邮编")]
        [Description("邮编")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(5, "ZipCode", "邮编", null, "nvarchar(50)", 0, 0, true)]
        public virtual String ZipCode
        {
            get { return _ZipCode; }
            set { if (OnPropertyChanging(__.ZipCode, value)) { _ZipCode = value; OnPropertyChanged(__.ZipCode); } }
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
                    case __.City : return _City;
                    case __.State : return _State;
                    case __.Country : return _Country;
                    case __.ZipCode : return _ZipCode;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.ID : _ID = Convert.ToInt32(value); break;
                    case __.City : _City = Convert.ToString(value); break;
                    case __.State : _State = Convert.ToString(value); break;
                    case __.Country : _Country = Convert.ToString(value); break;
                    case __.ZipCode : _ZipCode = Convert.ToString(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得位置字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号</summary>
            public static readonly Field ID = FindByName(__.ID);

            ///<summary>收货人的城市</summary>
            public static readonly Field City = FindByName(__.City);

            ///<summary>结算状态</summary>
            public static readonly Field State = FindByName(__.State);

            ///<summary>国家</summary>
            public static readonly Field Country = FindByName(__.Country);

            ///<summary>邮编</summary>
            public static readonly Field ZipCode = FindByName(__.ZipCode);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得位置字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号</summary>
            public const String ID = "ID";

            ///<summary>收货人的城市</summary>
            public const String City = "City";

            ///<summary>结算状态</summary>
            public const String State = "State";

            ///<summary>国家</summary>
            public const String Country = "Country";

            ///<summary>邮编</summary>
            public const String ZipCode = "ZipCode";

        }
        #endregion
    }

    /// <summary>位置接口</summary>
    public partial interface ILocation
    {
        #region 属性
        /// <summary>编号</summary>
        Int32 ID { get; set; }

        /// <summary>收货人的城市</summary>
        String City { get; set; }

        /// <summary>结算状态</summary>
        String State { get; set; }

        /// <summary>国家</summary>
        String Country { get; set; }

        /// <summary>邮编</summary>
        String ZipCode { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}