﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace BBX.Entity
{
    /// <summary>奖励日志</summary>
    [Serializable]
    [DataObject]
    [Description("奖励日志")]
    [BindTable("BonusLog", Description = "奖励日志", ConnName = "BBX", DbType = DatabaseType.SqlServer)]
    public partial class BonusLog : IBonusLog
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

        private Int32 _AuthorID;
        /// <summary>作者编号</summary>
        [DisplayName("作者编号")]
        [Description("作者编号")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(3, "AuthorID", "作者编号", null, "int", 10, 0, false)]
        public virtual Int32 AuthorID
        {
            get { return _AuthorID; }
            set { if (OnPropertyChanging(__.AuthorID, value)) { _AuthorID = value; OnPropertyChanged(__.AuthorID); } }
        }

        private Int32 _AnswerID;
        /// <summary>挑战编号</summary>
        [DisplayName("挑战编号")]
        [Description("挑战编号")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(4, "AnswerID", "挑战编号", null, "int", 10, 0, false)]
        public virtual Int32 AnswerID
        {
            get { return _AnswerID; }
            set { if (OnPropertyChanging(__.AnswerID, value)) { _AnswerID = value; OnPropertyChanged(__.AnswerID); } }
        }

        private String _AnswerName;
        /// <summary>挑战名称</summary>
        [DisplayName("挑战名称")]
        [Description("挑战名称")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(5, "AnswerName", "挑战名称", null, "nvarchar(50)", 0, 0, true)]
        public virtual String AnswerName
        {
            get { return _AnswerName; }
            set { if (OnPropertyChanging(__.AnswerName, value)) { _AnswerName = value; OnPropertyChanged(__.AnswerName); } }
        }

        private Int32 _Pid;
        /// <summary>帖子编号</summary>
        [DisplayName("帖子编号")]
        [Description("帖子编号")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(6, "Pid", "帖子编号", null, "int", 10, 0, false)]
        public virtual Int32 Pid
        {
            get { return _Pid; }
            set { if (OnPropertyChanging(__.Pid, value)) { _Pid = value; OnPropertyChanged(__.Pid); } }
        }

        private DateTime _Dateline;
        /// <summary>时间日期</summary>
        [DisplayName("时间日期")]
        [Description("时间日期")]
        [DataObjectField(false, false, false, 3)]
        [BindColumn(7, "Dateline", "时间日期", null, "datetime", 3, 0, false)]
        public virtual DateTime Dateline
        {
            get { return _Dateline; }
            set { if (OnPropertyChanging(__.Dateline, value)) { _Dateline = value; OnPropertyChanged(__.Dateline); } }
        }

        private Int32 _Bonus;
        /// <summary>奖金</summary>
        [DisplayName("奖金")]
        [Description("奖金")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(8, "Bonus", "奖金", null, "int", 10, 0, false)]
        public virtual Int32 Bonus
        {
            get { return _Bonus; }
            set { if (OnPropertyChanging(__.Bonus, value)) { _Bonus = value; OnPropertyChanged(__.Bonus); } }
        }

        private Byte _ExtID;
        /// <summary>奖励积分分组</summary>
        [DisplayName("奖励积分分组")]
        [Description("奖励积分分组")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(9, "ExtID", "奖励积分分组", null, "tinyint", 0, 0, false)]
        public virtual Byte ExtID
        {
            get { return _ExtID; }
            set { if (OnPropertyChanging(__.ExtID, value)) { _ExtID = value; OnPropertyChanged(__.ExtID); } }
        }

        private Byte _IsBest;
        /// <summary>是否最佳</summary>
        [DisplayName("是否最佳")]
        [Description("是否最佳")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(10, "IsBest", "是否最佳", null, "tinyint", 0, 0, false)]
        public virtual Byte IsBest
        {
            get { return _IsBest; }
            set { if (OnPropertyChanging(__.IsBest, value)) { _IsBest = value; OnPropertyChanged(__.IsBest); } }
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
                    case __.AuthorID : return _AuthorID;
                    case __.AnswerID : return _AnswerID;
                    case __.AnswerName : return _AnswerName;
                    case __.Pid : return _Pid;
                    case __.Dateline : return _Dateline;
                    case __.Bonus : return _Bonus;
                    case __.ExtID : return _ExtID;
                    case __.IsBest : return _IsBest;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.ID : _ID = Convert.ToInt32(value); break;
                    case __.Tid : _Tid = Convert.ToInt32(value); break;
                    case __.AuthorID : _AuthorID = Convert.ToInt32(value); break;
                    case __.AnswerID : _AnswerID = Convert.ToInt32(value); break;
                    case __.AnswerName : _AnswerName = Convert.ToString(value); break;
                    case __.Pid : _Pid = Convert.ToInt32(value); break;
                    case __.Dateline : _Dateline = Convert.ToDateTime(value); break;
                    case __.Bonus : _Bonus = Convert.ToInt32(value); break;
                    case __.ExtID : _ExtID = Convert.ToByte(value); break;
                    case __.IsBest : _IsBest = Convert.ToByte(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得奖励日志字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号</summary>
            public static readonly Field ID = FindByName(__.ID);

            ///<summary>主题编号</summary>
            public static readonly Field Tid = FindByName(__.Tid);

            ///<summary>作者编号</summary>
            public static readonly Field AuthorID = FindByName(__.AuthorID);

            ///<summary>挑战编号</summary>
            public static readonly Field AnswerID = FindByName(__.AnswerID);

            ///<summary>挑战名称</summary>
            public static readonly Field AnswerName = FindByName(__.AnswerName);

            ///<summary>帖子编号</summary>
            public static readonly Field Pid = FindByName(__.Pid);

            ///<summary>时间日期</summary>
            public static readonly Field Dateline = FindByName(__.Dateline);

            ///<summary>奖金</summary>
            public static readonly Field Bonus = FindByName(__.Bonus);

            ///<summary>奖励积分分组</summary>
            public static readonly Field ExtID = FindByName(__.ExtID);

            ///<summary>是否最佳</summary>
            public static readonly Field IsBest = FindByName(__.IsBest);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得奖励日志字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号</summary>
            public const String ID = "ID";

            ///<summary>主题编号</summary>
            public const String Tid = "Tid";

            ///<summary>作者编号</summary>
            public const String AuthorID = "AuthorID";

            ///<summary>挑战编号</summary>
            public const String AnswerID = "AnswerID";

            ///<summary>挑战名称</summary>
            public const String AnswerName = "AnswerName";

            ///<summary>帖子编号</summary>
            public const String Pid = "Pid";

            ///<summary>时间日期</summary>
            public const String Dateline = "Dateline";

            ///<summary>奖金</summary>
            public const String Bonus = "Bonus";

            ///<summary>奖励积分分组</summary>
            public const String ExtID = "ExtID";

            ///<summary>是否最佳</summary>
            public const String IsBest = "IsBest";

        }
        #endregion
    }

    /// <summary>奖励日志接口</summary>
    public partial interface IBonusLog
    {
        #region 属性
        /// <summary>编号</summary>
        Int32 ID { get; set; }

        /// <summary>主题编号</summary>
        Int32 Tid { get; set; }

        /// <summary>作者编号</summary>
        Int32 AuthorID { get; set; }

        /// <summary>挑战编号</summary>
        Int32 AnswerID { get; set; }

        /// <summary>挑战名称</summary>
        String AnswerName { get; set; }

        /// <summary>帖子编号</summary>
        Int32 Pid { get; set; }

        /// <summary>时间日期</summary>
        DateTime Dateline { get; set; }

        /// <summary>奖金</summary>
        Int32 Bonus { get; set; }

        /// <summary>奖励积分分组</summary>
        Byte ExtID { get; set; }

        /// <summary>是否最佳</summary>
        Byte IsBest { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}