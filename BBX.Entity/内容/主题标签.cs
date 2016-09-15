﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace BBX.Entity
{
    /// <summary>主题标签</summary>
    [Serializable]
    [DataObject]
    [Description("主题标签")]
    [BindTable("TopicTag", Description = "主题标签", ConnName = "BBX", DbType = DatabaseType.SqlServer)]
    public partial class TopicTag : ITopicTag
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

        private Int32 _TagID;
        /// <summary>标签编号</summary>
        [DisplayName("标签编号")]
        [Description("标签编号")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(2, "TagID", "标签编号", null, "int", 10, 0, false)]
        public virtual Int32 TagID
        {
            get { return _TagID; }
            set { if (OnPropertyChanging(__.TagID, value)) { _TagID = value; OnPropertyChanged(__.TagID); } }
        }

        private Int32 _Tid;
        /// <summary>帖子编号</summary>
        [DisplayName("帖子编号")]
        [Description("帖子编号")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(3, "Tid", "帖子编号", null, "int", 10, 0, false)]
        public virtual Int32 Tid
        {
            get { return _Tid; }
            set { if (OnPropertyChanging(__.Tid, value)) { _Tid = value; OnPropertyChanged(__.Tid); } }
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
                    case __.TagID : return _TagID;
                    case __.Tid : return _Tid;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.ID : _ID = Convert.ToInt32(value); break;
                    case __.TagID : _TagID = Convert.ToInt32(value); break;
                    case __.Tid : _Tid = Convert.ToInt32(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得主题标签字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号</summary>
            public static readonly Field ID = FindByName(__.ID);

            ///<summary>标签编号</summary>
            public static readonly Field TagID = FindByName(__.TagID);

            ///<summary>帖子编号</summary>
            public static readonly Field Tid = FindByName(__.Tid);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得主题标签字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号</summary>
            public const String ID = "ID";

            ///<summary>标签编号</summary>
            public const String TagID = "TagID";

            ///<summary>帖子编号</summary>
            public const String Tid = "Tid";

        }
        #endregion
    }

    /// <summary>主题标签接口</summary>
    public partial interface ITopicTag
    {
        #region 属性
        /// <summary>编号</summary>
        Int32 ID { get; set; }

        /// <summary>标签编号</summary>
        Int32 TagID { get; set; }

        /// <summary>帖子编号</summary>
        Int32 Tid { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}