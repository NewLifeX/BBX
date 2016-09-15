﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace BBX.Entity
{
    /// <summary>计划任务</summary>
    [Serializable]
    [DataObject]
    [Description("计划任务")]
    [BindTable("ScheduledEvent", Description = "计划任务", ConnName = "BBX", DbType = DatabaseType.SqlServer)]
    public partial class ScheduledEvent : IScheduledEvent
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

        private String _Key;
        /// <summary>事件名</summary>
        [DisplayName("事件名")]
        [Description("事件名")]
        [DataObjectField(false, false, false, 50)]
        [BindColumn(2, "Key", "事件名", null, "nvarchar(50)", 0, 0, true)]
        public virtual String Key
        {
            get { return _Key; }
            set { if (OnPropertyChanging(__.Key, value)) { _Key = value; OnPropertyChanged(__.Key); } }
        }

        private String _ServerName;
        /// <summary>服务器名</summary>
        [DisplayName("服务器名")]
        [Description("服务器名")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(3, "ServerName", "服务器名", null, "nvarchar(50)", 0, 0, true)]
        public virtual String ServerName
        {
            get { return _ServerName; }
            set { if (OnPropertyChanging(__.ServerName, value)) { _ServerName = value; OnPropertyChanged(__.ServerName); } }
        }

        private DateTime _LastExecuted;
        /// <summary>最后执行</summary>
        [DisplayName("最后执行")]
        [Description("最后执行")]
        [DataObjectField(false, false, true, 3)]
        [BindColumn(4, "LastExecuted", "最后执行", null, "datetime", 3, 0, false)]
        public virtual DateTime LastExecuted
        {
            get { return _LastExecuted; }
            set { if (OnPropertyChanging(__.LastExecuted, value)) { _LastExecuted = value; OnPropertyChanged(__.LastExecuted); } }
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
                    case __.Key : return _Key;
                    case __.ServerName : return _ServerName;
                    case __.LastExecuted : return _LastExecuted;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.ID : _ID = Convert.ToInt32(value); break;
                    case __.Key : _Key = Convert.ToString(value); break;
                    case __.ServerName : _ServerName = Convert.ToString(value); break;
                    case __.LastExecuted : _LastExecuted = Convert.ToDateTime(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得计划任务字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号</summary>
            public static readonly Field ID = FindByName(__.ID);

            ///<summary>事件名</summary>
            public static readonly Field Key = FindByName(__.Key);

            ///<summary>服务器名</summary>
            public static readonly Field ServerName = FindByName(__.ServerName);

            ///<summary>最后执行</summary>
            public static readonly Field LastExecuted = FindByName(__.LastExecuted);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得计划任务字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号</summary>
            public const String ID = "ID";

            ///<summary>事件名</summary>
            public const String Key = "Key";

            ///<summary>服务器名</summary>
            public const String ServerName = "ServerName";

            ///<summary>最后执行</summary>
            public const String LastExecuted = "LastExecuted";

        }
        #endregion
    }

    /// <summary>计划任务接口</summary>
    public partial interface IScheduledEvent
    {
        #region 属性
        /// <summary>编号</summary>
        Int32 ID { get; set; }

        /// <summary>事件名</summary>
        String Key { get; set; }

        /// <summary>服务器名</summary>
        String ServerName { get; set; }

        /// <summary>最后执行</summary>
        DateTime LastExecuted { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}