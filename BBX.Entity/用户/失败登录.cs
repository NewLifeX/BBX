﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace BBX.Entity
{
    /// <summary>失败登录</summary>
    [Serializable]
    [DataObject]
    [Description("失败登录")]
    [BindIndex("IU_Failedlogin_IP", true, "IP")]
    [BindTable("Failedlogin", Description = "失败登录", ConnName = "BBX", DbType = DatabaseType.SqlServer)]
    public partial class Failedlogin : IFailedlogin
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

        private String _IP;
        /// <summary>IP地址</summary>
        [DisplayName("IP地址")]
        [Description("IP地址")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(2, "IP", "IP地址", null, "nvarchar(50)", 0, 0, true)]
        public virtual String IP
        {
            get { return _IP; }
            set { if (OnPropertyChanging(__.IP, value)) { _IP = value; OnPropertyChanged(__.IP); } }
        }

        private Int32 _ErrCount;
        /// <summary>错误数</summary>
        [DisplayName("错误数")]
        [Description("错误数")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(3, "ErrCount", "错误数", null, "int", 10, 0, false)]
        public virtual Int32 ErrCount
        {
            get { return _ErrCount; }
            set { if (OnPropertyChanging(__.ErrCount, value)) { _ErrCount = value; OnPropertyChanged(__.ErrCount); } }
        }

        private DateTime _LastUpdate;
        /// <summary>最后更新时间</summary>
        [DisplayName("最后更新时间")]
        [Description("最后更新时间")]
        [DataObjectField(false, false, true, 3)]
        [BindColumn(4, "LastUpdate", "最后更新时间", null, "datetime", 3, 0, false)]
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
                    case __.IP : return _IP;
                    case __.ErrCount : return _ErrCount;
                    case __.LastUpdate : return _LastUpdate;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.ID : _ID = Convert.ToInt32(value); break;
                    case __.IP : _IP = Convert.ToString(value); break;
                    case __.ErrCount : _ErrCount = Convert.ToInt32(value); break;
                    case __.LastUpdate : _LastUpdate = Convert.ToDateTime(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得失败登录字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号</summary>
            public static readonly Field ID = FindByName(__.ID);

            ///<summary>IP地址</summary>
            public static readonly Field IP = FindByName(__.IP);

            ///<summary>错误数</summary>
            public static readonly Field ErrCount = FindByName(__.ErrCount);

            ///<summary>最后更新时间</summary>
            public static readonly Field LastUpdate = FindByName(__.LastUpdate);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得失败登录字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号</summary>
            public const String ID = "ID";

            ///<summary>IP地址</summary>
            public const String IP = "IP";

            ///<summary>错误数</summary>
            public const String ErrCount = "ErrCount";

            ///<summary>最后更新时间</summary>
            public const String LastUpdate = "LastUpdate";

        }
        #endregion
    }

    /// <summary>失败登录接口</summary>
    public partial interface IFailedlogin
    {
        #region 属性
        /// <summary>编号</summary>
        Int32 ID { get; set; }

        /// <summary>IP地址</summary>
        String IP { get; set; }

        /// <summary>错误数</summary>
        Int32 ErrCount { get; set; }

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