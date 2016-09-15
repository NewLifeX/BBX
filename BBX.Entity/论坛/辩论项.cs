﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace BBX.Entity
{
    /// <summary>辩论项</summary>
    [Serializable]
    [DataObject]
    [Description("辩论项")]
    [BindTable("Debatedigg", Description = "辩论项", ConnName = "BBX", DbType = DatabaseType.SqlServer)]
    public partial class Debatedigg : IDebatedigg
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

        private String _Digger;
        /// <summary>挖掘机</summary>
        [DisplayName("挖掘机")]
        [Description("挖掘机")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(4, "Digger", "挖掘机", null, "nvarchar(50)", 0, 0, true)]
        public virtual String Digger
        {
            get { return _Digger; }
            set { if (OnPropertyChanging(__.Digger, value)) { _Digger = value; OnPropertyChanged(__.Digger); } }
        }

        private Int32 _DiggerID;
        /// <summary></summary>
        [DisplayName("DiggerID")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(5, "DiggerID", "", null, "int", 10, 0, false)]
        public virtual Int32 DiggerID
        {
            get { return _DiggerID; }
            set { if (OnPropertyChanging(__.DiggerID, value)) { _DiggerID = value; OnPropertyChanged(__.DiggerID); } }
        }

        private String _DiggerIP;
        /// <summary></summary>
        [DisplayName("DiggerIP")]
        [Description("")]
        [DataObjectField(false, false, false, 15)]
        [BindColumn(6, "DiggerIP", "", null, "nvarchar(15)", 0, 0, true)]
        public virtual String DiggerIP
        {
            get { return _DiggerIP; }
            set { if (OnPropertyChanging(__.DiggerIP, value)) { _DiggerIP = value; OnPropertyChanged(__.DiggerIP); } }
        }

        private DateTime _DiggDateTime;
        /// <summary></summary>
        [DisplayName("DiggDateTime")]
        [Description("")]
        [DataObjectField(false, false, false, 3)]
        [BindColumn(7, "DiggDateTime", "", null, "datetime", 3, 0, false)]
        public virtual DateTime DiggDateTime
        {
            get { return _DiggDateTime; }
            set { if (OnPropertyChanging(__.DiggDateTime, value)) { _DiggDateTime = value; OnPropertyChanged(__.DiggDateTime); } }
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
                    case __.Digger : return _Digger;
                    case __.DiggerID : return _DiggerID;
                    case __.DiggerIP : return _DiggerIP;
                    case __.DiggDateTime : return _DiggDateTime;
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
                    case __.Digger : _Digger = Convert.ToString(value); break;
                    case __.DiggerID : _DiggerID = Convert.ToInt32(value); break;
                    case __.DiggerIP : _DiggerIP = Convert.ToString(value); break;
                    case __.DiggDateTime : _DiggDateTime = Convert.ToDateTime(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得辩论项字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号</summary>
            public static readonly Field ID = FindByName(__.ID);

            ///<summary>主题编号</summary>
            public static readonly Field Tid = FindByName(__.Tid);

            ///<summary>帖子编号</summary>
            public static readonly Field Pid = FindByName(__.Pid);

            ///<summary>挖掘机</summary>
            public static readonly Field Digger = FindByName(__.Digger);

            ///<summary></summary>
            public static readonly Field DiggerID = FindByName(__.DiggerID);

            ///<summary></summary>
            public static readonly Field DiggerIP = FindByName(__.DiggerIP);

            ///<summary></summary>
            public static readonly Field DiggDateTime = FindByName(__.DiggDateTime);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得辩论项字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号</summary>
            public const String ID = "ID";

            ///<summary>主题编号</summary>
            public const String Tid = "Tid";

            ///<summary>帖子编号</summary>
            public const String Pid = "Pid";

            ///<summary>挖掘机</summary>
            public const String Digger = "Digger";

            ///<summary></summary>
            public const String DiggerID = "DiggerID";

            ///<summary></summary>
            public const String DiggerIP = "DiggerIP";

            ///<summary></summary>
            public const String DiggDateTime = "DiggDateTime";

        }
        #endregion
    }

    /// <summary>辩论项接口</summary>
    public partial interface IDebatedigg
    {
        #region 属性
        /// <summary>编号</summary>
        Int32 ID { get; set; }

        /// <summary>主题编号</summary>
        Int32 Tid { get; set; }

        /// <summary>帖子编号</summary>
        Int32 Pid { get; set; }

        /// <summary>挖掘机</summary>
        String Digger { get; set; }

        /// <summary></summary>
        Int32 DiggerID { get; set; }

        /// <summary></summary>
        String DiggerIP { get; set; }

        /// <summary></summary>
        DateTime DiggDateTime { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}