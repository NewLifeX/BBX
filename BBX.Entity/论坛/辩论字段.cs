﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace BBX.Entity
{
    /// <summary>辩论字段</summary>
    [Serializable]
    [DataObject]
    [Description("辩论字段")]
    [BindIndex("IU_PostDebateField_tid_opinion", true, "tid,opinion")]
    [BindTable("PostDebateField", Description = "辩论字段", ConnName = "BBX", DbType = DatabaseType.SqlServer)]
    public partial class PostDebateField : IPostDebateField
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

        private Int32 _Pid;
        /// <summary>帖子编号</summary>
        [DisplayName("帖子编号")]
        [Description("帖子编号")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(3, "Pid", "帖子编号", null, "int", 10, 0, false)]
        public virtual Int32 Pid
        {
            get { return _Pid; }
            set { if (OnPropertyChanging(__.Pid, value)) { _Pid = value; OnPropertyChanged(__.Pid); } }
        }

        private Int32 _Opinion;
        /// <summary>选项</summary>
        [DisplayName("选项")]
        [Description("选项")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(4, "Opinion", "选项", null, "int", 10, 0, false)]
        public virtual Int32 Opinion
        {
            get { return _Opinion; }
            set { if (OnPropertyChanging(__.Opinion, value)) { _Opinion = value; OnPropertyChanged(__.Opinion); } }
        }

        private Int32 _Diggs;
        /// <summary>主将</summary>
        [DisplayName("主将")]
        [Description("主将")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(5, "Diggs", "主将", null, "int", 10, 0, false)]
        public virtual Int32 Diggs
        {
            get { return _Diggs; }
            set { if (OnPropertyChanging(__.Diggs, value)) { _Diggs = value; OnPropertyChanged(__.Diggs); } }
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
                    case __.Pid : return _Pid;
                    case __.Opinion : return _Opinion;
                    case __.Diggs : return _Diggs;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.ID : _ID = Convert.ToInt32(value); break;
                    case __.Tid : _Tid = Convert.ToInt32(value); break;
                    case __.Pid : _Pid = Convert.ToInt32(value); break;
                    case __.Opinion : _Opinion = Convert.ToInt32(value); break;
                    case __.Diggs : _Diggs = Convert.ToInt32(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得辩论字段字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号</summary>
            public static readonly Field ID = FindByName(__.ID);

            ///<summary>主题编号</summary>
            public static readonly Field Tid = FindByName(__.Tid);

            ///<summary>帖子编号</summary>
            public static readonly Field Pid = FindByName(__.Pid);

            ///<summary>选项</summary>
            public static readonly Field Opinion = FindByName(__.Opinion);

            ///<summary>主将</summary>
            public static readonly Field Diggs = FindByName(__.Diggs);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得辩论字段字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号</summary>
            public const String ID = "ID";

            ///<summary>主题编号</summary>
            public const String Tid = "Tid";

            ///<summary>帖子编号</summary>
            public const String Pid = "Pid";

            ///<summary>选项</summary>
            public const String Opinion = "Opinion";

            ///<summary>主将</summary>
            public const String Diggs = "Diggs";

        }
        #endregion
    }

    /// <summary>辩论字段接口</summary>
    public partial interface IPostDebateField
    {
        #region 属性
        /// <summary>编号</summary>
        Int32 ID { get; set; }

        /// <summary>主题编号</summary>
        Int32 Tid { get; set; }

        /// <summary>帖子编号</summary>
        Int32 Pid { get; set; }

        /// <summary>选项</summary>
        Int32 Opinion { get; set; }

        /// <summary>主将</summary>
        Int32 Diggs { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}