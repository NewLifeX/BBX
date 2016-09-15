﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace BBX.Entity
{
    /// <summary>表列表</summary>
    [Serializable]
    [DataObject]
    [Description("表列表")]
    [BindTable("TableList", Description = "表列表", ConnName = "BBX", DbType = DatabaseType.SqlServer)]
    public partial class TableList : ITableList
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

        private DateTime _CreateDateTime;
        /// <summary>创建时间</summary>
        [DisplayName("创建时间")]
        [Description("创建时间")]
        [DataObjectField(false, false, true, 3)]
        [BindColumn(2, "CreateDateTime", "创建时间", null, "datetime", 3, 0, false)]
        public virtual DateTime CreateDateTime
        {
            get { return _CreateDateTime; }
            set { if (OnPropertyChanging(__.CreateDateTime, value)) { _CreateDateTime = value; OnPropertyChanged(__.CreateDateTime); } }
        }

        private String _Description;
        /// <summary>描述</summary>
        [DisplayName("描述")]
        [Description("描述")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(3, "Description", "描述", null, "nvarchar(50)", 0, 0, true)]
        public virtual String Description
        {
            get { return _Description; }
            set { if (OnPropertyChanging(__.Description, value)) { _Description = value; OnPropertyChanged(__.Description); } }
        }

        private Int32 _MintID;
        /// <summary>最小编号</summary>
        [DisplayName("最小编号")]
        [Description("最小编号")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(4, "MintID", "最小编号", null, "int", 10, 0, false)]
        public virtual Int32 MintID
        {
            get { return _MintID; }
            set { if (OnPropertyChanging(__.MintID, value)) { _MintID = value; OnPropertyChanged(__.MintID); } }
        }

        private Int32 _MaxtID;
        /// <summary>最大编号</summary>
        [DisplayName("最大编号")]
        [Description("最大编号")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(5, "MaxtID", "最大编号", null, "int", 10, 0, false)]
        public virtual Int32 MaxtID
        {
            get { return _MaxtID; }
            set { if (OnPropertyChanging(__.MaxtID, value)) { _MaxtID = value; OnPropertyChanged(__.MaxtID); } }
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
                    case __.CreateDateTime : return _CreateDateTime;
                    case __.Description : return _Description;
                    case __.MintID : return _MintID;
                    case __.MaxtID : return _MaxtID;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.ID : _ID = Convert.ToInt32(value); break;
                    case __.CreateDateTime : _CreateDateTime = Convert.ToDateTime(value); break;
                    case __.Description : _Description = Convert.ToString(value); break;
                    case __.MintID : _MintID = Convert.ToInt32(value); break;
                    case __.MaxtID : _MaxtID = Convert.ToInt32(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得表列表字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号</summary>
            public static readonly Field ID = FindByName(__.ID);

            ///<summary>创建时间</summary>
            public static readonly Field CreateDateTime = FindByName(__.CreateDateTime);

            ///<summary>描述</summary>
            public static readonly Field Description = FindByName(__.Description);

            ///<summary>最小编号</summary>
            public static readonly Field MintID = FindByName(__.MintID);

            ///<summary>最大编号</summary>
            public static readonly Field MaxtID = FindByName(__.MaxtID);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得表列表字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号</summary>
            public const String ID = "ID";

            ///<summary>创建时间</summary>
            public const String CreateDateTime = "CreateDateTime";

            ///<summary>描述</summary>
            public const String Description = "Description";

            ///<summary>最小编号</summary>
            public const String MintID = "MintID";

            ///<summary>最大编号</summary>
            public const String MaxtID = "MaxtID";

        }
        #endregion
    }

    /// <summary>表列表接口</summary>
    public partial interface ITableList
    {
        #region 属性
        /// <summary>编号</summary>
        Int32 ID { get; set; }

        /// <summary>创建时间</summary>
        DateTime CreateDateTime { get; set; }

        /// <summary>描述</summary>
        String Description { get; set; }

        /// <summary>最小编号</summary>
        Int32 MintID { get; set; }

        /// <summary>最大编号</summary>
        Int32 MaxtID { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}