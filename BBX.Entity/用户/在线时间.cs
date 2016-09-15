﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace BBX.Entity
{
    /// <summary>在线时间</summary>
    [Serializable]
    [DataObject]
    [Description("在线时间")]
    [BindIndex("IU_OnlineTime_Uid", true, "Uid")]
    [BindTable("OnlineTime", Description = "在线时间", ConnName = "BBX", DbType = DatabaseType.SqlServer)]
    public partial class OnlineTime : IOnlineTime
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
        [DataObjectField(false, false, false, 10)]
        [BindColumn(2, "Uid", "用户编号", null, "int", 10, 0, false)]
        public virtual Int32 Uid
        {
            get { return _Uid; }
            set { if (OnPropertyChanging(__.Uid, value)) { _Uid = value; OnPropertyChanged(__.Uid); } }
        }

        private Int32 _Thismonth;
        /// <summary>本月</summary>
        [DisplayName("本月")]
        [Description("本月")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(3, "Thismonth", "本月", null, "int", 10, 0, false)]
        public virtual Int32 Thismonth
        {
            get { return _Thismonth; }
            set { if (OnPropertyChanging(__.Thismonth, value)) { _Thismonth = value; OnPropertyChanged(__.Thismonth); } }
        }

        private Int32 _Total;
        /// <summary>总数</summary>
        [DisplayName("总数")]
        [Description("总数")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(4, "Total", "总数", null, "int", 10, 0, false)]
        public virtual Int32 Total
        {
            get { return _Total; }
            set { if (OnPropertyChanging(__.Total, value)) { _Total = value; OnPropertyChanged(__.Total); } }
        }

        private DateTime _LastUpdate;
        /// <summary>最后更新时间</summary>
        [DisplayName("最后更新时间")]
        [Description("最后更新时间")]
        [DataObjectField(false, false, true, 3)]
        [BindColumn(5, "LastUpdate", "最后更新时间", null, "datetime", 3, 0, false)]
        public virtual DateTime LastUpdate
        {
            get { return _LastUpdate; }
            set { if (OnPropertyChanging(__.LastUpdate, value)) { _LastUpdate = value; OnPropertyChanged(__.LastUpdate); } }
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
                    case __.Thismonth : return _Thismonth;
                    case __.Total : return _Total;
                    case __.LastUpdate : return _LastUpdate;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.ID : _ID = Convert.ToInt32(value); break;
                    case __.Uid : _Uid = Convert.ToInt32(value); break;
                    case __.Thismonth : _Thismonth = Convert.ToInt32(value); break;
                    case __.Total : _Total = Convert.ToInt32(value); break;
                    case __.LastUpdate : _LastUpdate = Convert.ToDateTime(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得在线时间字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号</summary>
            public static readonly Field ID = FindByName(__.ID);

            ///<summary>用户编号</summary>
            public static readonly Field Uid = FindByName(__.Uid);

            ///<summary>本月</summary>
            public static readonly Field Thismonth = FindByName(__.Thismonth);

            ///<summary>总数</summary>
            public static readonly Field Total = FindByName(__.Total);

            ///<summary>最后更新时间</summary>
            public static readonly Field LastUpdate = FindByName(__.LastUpdate);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得在线时间字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号</summary>
            public const String ID = "ID";

            ///<summary>用户编号</summary>
            public const String Uid = "Uid";

            ///<summary>本月</summary>
            public const String Thismonth = "Thismonth";

            ///<summary>总数</summary>
            public const String Total = "Total";

            ///<summary>最后更新时间</summary>
            public const String LastUpdate = "LastUpdate";

        }
        #endregion
    }

    /// <summary>在线时间接口</summary>
    public partial interface IOnlineTime
    {
        #region 属性
        /// <summary>编号</summary>
        Int32 ID { get; set; }

        /// <summary>用户编号</summary>
        Int32 Uid { get; set; }

        /// <summary>本月</summary>
        Int32 Thismonth { get; set; }

        /// <summary>总数</summary>
        Int32 Total { get; set; }

        /// <summary>最后更新时间</summary>
        DateTime LastUpdate { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}