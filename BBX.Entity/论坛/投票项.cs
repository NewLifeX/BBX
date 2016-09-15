﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace BBX.Entity
{
    /// <summary>投票项</summary>
    [Serializable]
    [DataObject]
    [Description("投票项")]
    [BindIndex("IX_PollOption_tid", false, "tid")]
    [BindIndex("IX_PollOption_pollid", false, "pollid")]
    [BindTable("PollOption", Description = "投票项", ConnName = "BBX", DbType = DatabaseType.SqlServer)]
    public partial class PollOption : IPollOption
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

        private Int32 _PollID;
        /// <summary></summary>
        [DisplayName("PollID")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(3, "PollID", "", null, "int", 10, 0, false)]
        public virtual Int32 PollID
        {
            get { return _PollID; }
            set { if (OnPropertyChanging(__.PollID, value)) { _PollID = value; OnPropertyChanged(__.PollID); } }
        }

        private Int32 _Votes;
        /// <summary>投票</summary>
        [DisplayName("投票")]
        [Description("投票")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(4, "Votes", "投票", null, "int", 10, 0, false)]
        public virtual Int32 Votes
        {
            get { return _Votes; }
            set { if (OnPropertyChanging(__.Votes, value)) { _Votes = value; OnPropertyChanged(__.Votes); } }
        }

        private Int32 _DisplayOrder;
        /// <summary>显示顺序</summary>
        [DisplayName("显示顺序")]
        [Description("显示顺序")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(5, "DisplayOrder", "显示顺序", null, "int", 10, 0, false)]
        public virtual Int32 DisplayOrder
        {
            get { return _DisplayOrder; }
            set { if (OnPropertyChanging(__.DisplayOrder, value)) { _DisplayOrder = value; OnPropertyChanged(__.DisplayOrder); } }
        }

        private String _Name;
        /// <summary>投票项</summary>
        [DisplayName("投票项")]
        [Description("投票项")]
        [DataObjectField(false, false, false, 80)]
        [BindColumn(6, "Name", "投票项", null, "nvarchar(80)", 0, 0, true, Master=true)]
        public virtual String Name
        {
            get { return _Name; }
            set { if (OnPropertyChanging(__.Name, value)) { _Name = value; OnPropertyChanged(__.Name); } }
        }

        private String _VoterNames;
        /// <summary>投票者</summary>
        [DisplayName("投票者")]
        [Description("投票者")]
        [DataObjectField(false, false, false, -1)]
        [BindColumn(7, "VoterNames", "投票者", null, "ntext", 0, 0, true)]
        public virtual String VoterNames
        {
            get { return _VoterNames; }
            set { if (OnPropertyChanging(__.VoterNames, value)) { _VoterNames = value; OnPropertyChanged(__.VoterNames); } }
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
                    case __.PollID : return _PollID;
                    case __.Votes : return _Votes;
                    case __.DisplayOrder : return _DisplayOrder;
                    case __.Name : return _Name;
                    case __.VoterNames : return _VoterNames;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.ID : _ID = Convert.ToInt32(value); break;
                    case __.Tid : _Tid = Convert.ToInt32(value); break;
                    case __.PollID : _PollID = Convert.ToInt32(value); break;
                    case __.Votes : _Votes = Convert.ToInt32(value); break;
                    case __.DisplayOrder : _DisplayOrder = Convert.ToInt32(value); break;
                    case __.Name : _Name = Convert.ToString(value); break;
                    case __.VoterNames : _VoterNames = Convert.ToString(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得投票项字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号</summary>
            public static readonly Field ID = FindByName(__.ID);

            ///<summary>主题编号</summary>
            public static readonly Field Tid = FindByName(__.Tid);

            ///<summary></summary>
            public static readonly Field PollID = FindByName(__.PollID);

            ///<summary>投票</summary>
            public static readonly Field Votes = FindByName(__.Votes);

            ///<summary>显示顺序</summary>
            public static readonly Field DisplayOrder = FindByName(__.DisplayOrder);

            ///<summary>投票项</summary>
            public static readonly Field Name = FindByName(__.Name);

            ///<summary>投票者</summary>
            public static readonly Field VoterNames = FindByName(__.VoterNames);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得投票项字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号</summary>
            public const String ID = "ID";

            ///<summary>主题编号</summary>
            public const String Tid = "Tid";

            ///<summary></summary>
            public const String PollID = "PollID";

            ///<summary>投票</summary>
            public const String Votes = "Votes";

            ///<summary>显示顺序</summary>
            public const String DisplayOrder = "DisplayOrder";

            ///<summary>投票项</summary>
            public const String Name = "Name";

            ///<summary>投票者</summary>
            public const String VoterNames = "VoterNames";

        }
        #endregion
    }

    /// <summary>投票项接口</summary>
    public partial interface IPollOption
    {
        #region 属性
        /// <summary>编号</summary>
        Int32 ID { get; set; }

        /// <summary>主题编号</summary>
        Int32 Tid { get; set; }

        /// <summary></summary>
        Int32 PollID { get; set; }

        /// <summary>投票</summary>
        Int32 Votes { get; set; }

        /// <summary>显示顺序</summary>
        Int32 DisplayOrder { get; set; }

        /// <summary>投票项</summary>
        String Name { get; set; }

        /// <summary>投票者</summary>
        String VoterNames { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}