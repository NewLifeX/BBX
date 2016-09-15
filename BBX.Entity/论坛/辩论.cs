﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace BBX.Entity
{
    /// <summary>辩论</summary>
    [Serializable]
    [DataObject]
    [Description("辩论")]
    [BindIndex("IU_Debate_Tid", true, "Tid")]
    [BindTable("Debate", Description = "辩论", ConnName = "BBX", DbType = DatabaseType.SqlServer)]
    public partial class Debate : IDebate
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
        [DataObjectField(false, false, false, 10)]
        [BindColumn(2, "Tid", "主题编号", null, "int", 10, 0, false)]
        public virtual Int32 Tid
        {
            get { return _Tid; }
            set { if (OnPropertyChanging(__.Tid, value)) { _Tid = value; OnPropertyChanged(__.Tid); } }
        }

        private String _PositiveOpinion;
        /// <summary></summary>
        [DisplayName("PositiveOpinion")]
        [Description("")]
        [DataObjectField(false, false, false, 200)]
        [BindColumn(3, "PositiveOpinion", "", null, "nvarchar(200)", 0, 0, true)]
        public virtual String PositiveOpinion
        {
            get { return _PositiveOpinion; }
            set { if (OnPropertyChanging(__.PositiveOpinion, value)) { _PositiveOpinion = value; OnPropertyChanged(__.PositiveOpinion); } }
        }

        private String _NegativeOpinion;
        /// <summary></summary>
        [DisplayName("NegativeOpinion")]
        [Description("")]
        [DataObjectField(false, false, false, 200)]
        [BindColumn(4, "NegativeOpinion", "", null, "nvarchar(200)", 0, 0, true)]
        public virtual String NegativeOpinion
        {
            get { return _NegativeOpinion; }
            set { if (OnPropertyChanging(__.NegativeOpinion, value)) { _NegativeOpinion = value; OnPropertyChanged(__.NegativeOpinion); } }
        }

        private DateTime _TerminalTime;
        /// <summary></summary>
        [DisplayName("TerminalTime")]
        [Description("")]
        [DataObjectField(false, false, false, 3)]
        [BindColumn(5, "TerminalTime", "", null, "datetime", 3, 0, false)]
        public virtual DateTime TerminalTime
        {
            get { return _TerminalTime; }
            set { if (OnPropertyChanging(__.TerminalTime, value)) { _TerminalTime = value; OnPropertyChanged(__.TerminalTime); } }
        }

        private Int32 _PositiveDiggs;
        /// <summary></summary>
        [DisplayName("PositiveDiggs")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(6, "PositiveDiggs", "", null, "int", 10, 0, false)]
        public virtual Int32 PositiveDiggs
        {
            get { return _PositiveDiggs; }
            set { if (OnPropertyChanging(__.PositiveDiggs, value)) { _PositiveDiggs = value; OnPropertyChanged(__.PositiveDiggs); } }
        }

        private Int32 _NegativeDiggs;
        /// <summary></summary>
        [DisplayName("NegativeDiggs")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(7, "NegativeDiggs", "", null, "int", 10, 0, false)]
        public virtual Int32 NegativeDiggs
        {
            get { return _NegativeDiggs; }
            set { if (OnPropertyChanging(__.NegativeDiggs, value)) { _NegativeDiggs = value; OnPropertyChanged(__.NegativeDiggs); } }
        }

        private Int32 _PositiveVote;
        /// <summary></summary>
        [DisplayName("PositiveVote")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(8, "PositiveVote", "", null, "int", 10, 0, false)]
        public virtual Int32 PositiveVote
        {
            get { return _PositiveVote; }
            set { if (OnPropertyChanging(__.PositiveVote, value)) { _PositiveVote = value; OnPropertyChanged(__.PositiveVote); } }
        }

        private String _PositiveVoterids;
        /// <summary></summary>
        [DisplayName("PositiveVoterids")]
        [Description("")]
        [DataObjectField(false, false, false, -1)]
        [BindColumn(9, "PositiveVoterids", "", null, "ntext", 0, 0, true)]
        public virtual String PositiveVoterids
        {
            get { return _PositiveVoterids; }
            set { if (OnPropertyChanging(__.PositiveVoterids, value)) { _PositiveVoterids = value; OnPropertyChanged(__.PositiveVoterids); } }
        }

        private Int32 _NegativeVote;
        /// <summary>否决提案</summary>
        [DisplayName("否决提案")]
        [Description("否决提案")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(10, "NegativeVote", "否决提案", null, "int", 10, 0, false)]
        public virtual Int32 NegativeVote
        {
            get { return _NegativeVote; }
            set { if (OnPropertyChanging(__.NegativeVote, value)) { _NegativeVote = value; OnPropertyChanged(__.NegativeVote); } }
        }

        private String _NegativeVoterids;
        /// <summary></summary>
        [DisplayName("NegativeVoterids")]
        [Description("")]
        [DataObjectField(false, false, false, -1)]
        [BindColumn(11, "NegativeVoterids", "", null, "ntext", 0, 0, true)]
        public virtual String NegativeVoterids
        {
            get { return _NegativeVoterids; }
            set { if (OnPropertyChanging(__.NegativeVoterids, value)) { _NegativeVoterids = value; OnPropertyChanged(__.NegativeVoterids); } }
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
                    case __.PositiveOpinion : return _PositiveOpinion;
                    case __.NegativeOpinion : return _NegativeOpinion;
                    case __.TerminalTime : return _TerminalTime;
                    case __.PositiveDiggs : return _PositiveDiggs;
                    case __.NegativeDiggs : return _NegativeDiggs;
                    case __.PositiveVote : return _PositiveVote;
                    case __.PositiveVoterids : return _PositiveVoterids;
                    case __.NegativeVote : return _NegativeVote;
                    case __.NegativeVoterids : return _NegativeVoterids;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.ID : _ID = Convert.ToInt32(value); break;
                    case __.Tid : _Tid = Convert.ToInt32(value); break;
                    case __.PositiveOpinion : _PositiveOpinion = Convert.ToString(value); break;
                    case __.NegativeOpinion : _NegativeOpinion = Convert.ToString(value); break;
                    case __.TerminalTime : _TerminalTime = Convert.ToDateTime(value); break;
                    case __.PositiveDiggs : _PositiveDiggs = Convert.ToInt32(value); break;
                    case __.NegativeDiggs : _NegativeDiggs = Convert.ToInt32(value); break;
                    case __.PositiveVote : _PositiveVote = Convert.ToInt32(value); break;
                    case __.PositiveVoterids : _PositiveVoterids = Convert.ToString(value); break;
                    case __.NegativeVote : _NegativeVote = Convert.ToInt32(value); break;
                    case __.NegativeVoterids : _NegativeVoterids = Convert.ToString(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得辩论字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号</summary>
            public static readonly Field ID = FindByName(__.ID);

            ///<summary>主题编号</summary>
            public static readonly Field Tid = FindByName(__.Tid);

            ///<summary></summary>
            public static readonly Field PositiveOpinion = FindByName(__.PositiveOpinion);

            ///<summary></summary>
            public static readonly Field NegativeOpinion = FindByName(__.NegativeOpinion);

            ///<summary></summary>
            public static readonly Field TerminalTime = FindByName(__.TerminalTime);

            ///<summary></summary>
            public static readonly Field PositiveDiggs = FindByName(__.PositiveDiggs);

            ///<summary></summary>
            public static readonly Field NegativeDiggs = FindByName(__.NegativeDiggs);

            ///<summary></summary>
            public static readonly Field PositiveVote = FindByName(__.PositiveVote);

            ///<summary></summary>
            public static readonly Field PositiveVoterids = FindByName(__.PositiveVoterids);

            ///<summary>否决提案</summary>
            public static readonly Field NegativeVote = FindByName(__.NegativeVote);

            ///<summary></summary>
            public static readonly Field NegativeVoterids = FindByName(__.NegativeVoterids);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得辩论字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号</summary>
            public const String ID = "ID";

            ///<summary>主题编号</summary>
            public const String Tid = "Tid";

            ///<summary></summary>
            public const String PositiveOpinion = "PositiveOpinion";

            ///<summary></summary>
            public const String NegativeOpinion = "NegativeOpinion";

            ///<summary></summary>
            public const String TerminalTime = "TerminalTime";

            ///<summary></summary>
            public const String PositiveDiggs = "PositiveDiggs";

            ///<summary></summary>
            public const String NegativeDiggs = "NegativeDiggs";

            ///<summary></summary>
            public const String PositiveVote = "PositiveVote";

            ///<summary></summary>
            public const String PositiveVoterids = "PositiveVoterids";

            ///<summary>否决提案</summary>
            public const String NegativeVote = "NegativeVote";

            ///<summary></summary>
            public const String NegativeVoterids = "NegativeVoterids";

        }
        #endregion
    }

    /// <summary>辩论接口</summary>
    public partial interface IDebate
    {
        #region 属性
        /// <summary>编号</summary>
        Int32 ID { get; set; }

        /// <summary>主题编号</summary>
        Int32 Tid { get; set; }

        /// <summary></summary>
        String PositiveOpinion { get; set; }

        /// <summary></summary>
        String NegativeOpinion { get; set; }

        /// <summary></summary>
        DateTime TerminalTime { get; set; }

        /// <summary></summary>
        Int32 PositiveDiggs { get; set; }

        /// <summary></summary>
        Int32 NegativeDiggs { get; set; }

        /// <summary></summary>
        Int32 PositiveVote { get; set; }

        /// <summary></summary>
        String PositiveVoterids { get; set; }

        /// <summary>否决提案</summary>
        Int32 NegativeVote { get; set; }

        /// <summary></summary>
        String NegativeVoterids { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}