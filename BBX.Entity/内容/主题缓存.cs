﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace BBX.Entity
{
    /// <summary>主题缓存</summary>
    [Serializable]
    [DataObject]
    [Description("主题缓存")]
    [BindIndex("IX_TopicTagCache_Tid", false, "Tid")]
    [BindTable("TopicTagCache", Description = "主题缓存", ConnName = "BBX", DbType = DatabaseType.SqlServer)]
    public partial class TopicTagCache : ITopicTagCache
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
        /// <summary>主题编号</summary>
        [DisplayName("主题编号")]
        [Description("主题编号")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(2, "Tid", "主题编号", null, "int", 10, 0, false)]
        public virtual Int32 Tid
        {
            get { return _Tid; }
            set { if (OnPropertyChanging(__.Tid, value)) { _Tid = value; OnPropertyChanged(__.Tid); } }
        }

        private Int32 _LinktID;
        /// <summary></summary>
        [DisplayName("LinktID")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(3, "LinktID", "", null, "int", 10, 0, false)]
        public virtual Int32 LinktID
        {
            get { return _LinktID; }
            set { if (OnPropertyChanging(__.LinktID, value)) { _LinktID = value; OnPropertyChanged(__.LinktID); } }
        }

        private String _LinkTitle;
        /// <summary></summary>
        [DisplayName("LinkTitle")]
        [Description("")]
        [DataObjectField(false, false, true, 60)]
        [BindColumn(4, "LinkTitle", "", null, "nvarchar(60)", 0, 0, true)]
        public virtual String LinkTitle
        {
            get { return _LinkTitle; }
            set { if (OnPropertyChanging(__.LinkTitle, value)) { _LinkTitle = value; OnPropertyChanged(__.LinkTitle); } }
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
                    case __.LinktID : return _LinktID;
                    case __.LinkTitle : return _LinkTitle;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.ID : _ID = Convert.ToInt32(value); break;
                    case __.Tid : _Tid = Convert.ToInt32(value); break;
                    case __.LinktID : _LinktID = Convert.ToInt32(value); break;
                    case __.LinkTitle : _LinkTitle = Convert.ToString(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得主题缓存字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号</summary>
            public static readonly Field ID = FindByName(__.ID);

            ///<summary>主题编号</summary>
            public static readonly Field Tid = FindByName(__.Tid);

            ///<summary></summary>
            public static readonly Field LinktID = FindByName(__.LinktID);

            ///<summary></summary>
            public static readonly Field LinkTitle = FindByName(__.LinkTitle);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得主题缓存字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号</summary>
            public const String ID = "ID";

            ///<summary>主题编号</summary>
            public const String Tid = "Tid";

            ///<summary></summary>
            public const String LinktID = "LinktID";

            ///<summary></summary>
            public const String LinkTitle = "LinkTitle";

        }
        #endregion
    }

    /// <summary>主题缓存接口</summary>
    public partial interface ITopicTagCache
    {
        #region 属性
        /// <summary>编号</summary>
        Int32 ID { get; set; }

        /// <summary>主题编号</summary>
        Int32 Tid { get; set; }

        /// <summary></summary>
        Int32 LinktID { get; set; }

        /// <summary></summary>
        String LinkTitle { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}