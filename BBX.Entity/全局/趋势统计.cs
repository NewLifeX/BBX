﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace BBX.Entity
{
    /// <summary>趋势统计</summary>
    [Serializable]
    [DataObject]
    [Description("趋势统计")]
    [BindIndex("IU_TrendStat_daytime", true, "daytime")]
    [BindTable("TrendStat", Description = "趋势统计", ConnName = "BBX", DbType = DatabaseType.SqlServer)]
    public partial class TrendStat : ITrendStat
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

        private Int32 _DayTime;
        /// <summary>白天</summary>
        [DisplayName("白天")]
        [Description("白天")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(2, "DayTime", "白天", null, "int", 10, 0, false)]
        public virtual Int32 DayTime
        {
            get { return _DayTime; }
            set { if (OnPropertyChanging(__.DayTime, value)) { _DayTime = value; OnPropertyChanged(__.DayTime); } }
        }

        private Int32 _Login;
        /// <summary>登录</summary>
        [DisplayName("登录")]
        [Description("登录")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(3, "Login", "登录", null, "int", 10, 0, false)]
        public virtual Int32 Login
        {
            get { return _Login; }
            set { if (OnPropertyChanging(__.Login, value)) { _Login = value; OnPropertyChanged(__.Login); } }
        }

        private Int32 _Register;
        /// <summary>登记册</summary>
        [DisplayName("登记册")]
        [Description("登记册")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(4, "Register", "登记册", null, "int", 10, 0, false)]
        public virtual Int32 Register
        {
            get { return _Register; }
            set { if (OnPropertyChanging(__.Register, value)) { _Register = value; OnPropertyChanged(__.Register); } }
        }

        private Int32 _Topic;
        /// <summary>主题</summary>
        [DisplayName("主题")]
        [Description("主题")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(5, "Topic", "主题", null, "int", 10, 0, false)]
        public virtual Int32 Topic
        {
            get { return _Topic; }
            set { if (OnPropertyChanging(__.Topic, value)) { _Topic = value; OnPropertyChanged(__.Topic); } }
        }

        private Int32 _Post;
        /// <summary>职务</summary>
        [DisplayName("职务")]
        [Description("职务")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(6, "Post", "职务", null, "int", 10, 0, false)]
        public virtual Int32 Post
        {
            get { return _Post; }
            set { if (OnPropertyChanging(__.Post, value)) { _Post = value; OnPropertyChanged(__.Post); } }
        }

        private Int32 _Poll;
        /// <summary>投票</summary>
        [DisplayName("投票")]
        [Description("投票")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(7, "Poll", "投票", null, "int", 10, 0, false)]
        public virtual Int32 Poll
        {
            get { return _Poll; }
            set { if (OnPropertyChanging(__.Poll, value)) { _Poll = value; OnPropertyChanged(__.Poll); } }
        }

        private Int32 _Debate;
        /// <summary>辩论</summary>
        [DisplayName("辩论")]
        [Description("辩论")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(8, "Debate", "辩论", null, "int", 10, 0, false)]
        public virtual Int32 Debate
        {
            get { return _Debate; }
            set { if (OnPropertyChanging(__.Debate, value)) { _Debate = value; OnPropertyChanged(__.Debate); } }
        }

        private Int32 _Bonus;
        /// <summary>奖金</summary>
        [DisplayName("奖金")]
        [Description("奖金")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(9, "Bonus", "奖金", null, "int", 10, 0, false)]
        public virtual Int32 Bonus
        {
            get { return _Bonus; }
            set { if (OnPropertyChanging(__.Bonus, value)) { _Bonus = value; OnPropertyChanged(__.Bonus); } }
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
                    case __.DayTime : return _DayTime;
                    case __.Login : return _Login;
                    case __.Register : return _Register;
                    case __.Topic : return _Topic;
                    case __.Post : return _Post;
                    case __.Poll : return _Poll;
                    case __.Debate : return _Debate;
                    case __.Bonus : return _Bonus;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.ID : _ID = Convert.ToInt32(value); break;
                    case __.DayTime : _DayTime = Convert.ToInt32(value); break;
                    case __.Login : _Login = Convert.ToInt32(value); break;
                    case __.Register : _Register = Convert.ToInt32(value); break;
                    case __.Topic : _Topic = Convert.ToInt32(value); break;
                    case __.Post : _Post = Convert.ToInt32(value); break;
                    case __.Poll : _Poll = Convert.ToInt32(value); break;
                    case __.Debate : _Debate = Convert.ToInt32(value); break;
                    case __.Bonus : _Bonus = Convert.ToInt32(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得趋势统计字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号</summary>
            public static readonly Field ID = FindByName(__.ID);

            ///<summary>白天</summary>
            public static readonly Field DayTime = FindByName(__.DayTime);

            ///<summary>登录</summary>
            public static readonly Field Login = FindByName(__.Login);

            ///<summary>登记册</summary>
            public static readonly Field Register = FindByName(__.Register);

            ///<summary>主题</summary>
            public static readonly Field Topic = FindByName(__.Topic);

            ///<summary>职务</summary>
            public static readonly Field Post = FindByName(__.Post);

            ///<summary>投票</summary>
            public static readonly Field Poll = FindByName(__.Poll);

            ///<summary>辩论</summary>
            public static readonly Field Debate = FindByName(__.Debate);

            ///<summary>奖金</summary>
            public static readonly Field Bonus = FindByName(__.Bonus);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得趋势统计字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号</summary>
            public const String ID = "ID";

            ///<summary>白天</summary>
            public const String DayTime = "DayTime";

            ///<summary>登录</summary>
            public const String Login = "Login";

            ///<summary>登记册</summary>
            public const String Register = "Register";

            ///<summary>主题</summary>
            public const String Topic = "Topic";

            ///<summary>职务</summary>
            public const String Post = "Post";

            ///<summary>投票</summary>
            public const String Poll = "Poll";

            ///<summary>辩论</summary>
            public const String Debate = "Debate";

            ///<summary>奖金</summary>
            public const String Bonus = "Bonus";

        }
        #endregion
    }

    /// <summary>趋势统计接口</summary>
    public partial interface ITrendStat
    {
        #region 属性
        /// <summary>编号</summary>
        Int32 ID { get; set; }

        /// <summary>白天</summary>
        Int32 DayTime { get; set; }

        /// <summary>登录</summary>
        Int32 Login { get; set; }

        /// <summary>登记册</summary>
        Int32 Register { get; set; }

        /// <summary>主题</summary>
        Int32 Topic { get; set; }

        /// <summary>职务</summary>
        Int32 Post { get; set; }

        /// <summary>投票</summary>
        Int32 Poll { get; set; }

        /// <summary>辩论</summary>
        Int32 Debate { get; set; }

        /// <summary>奖金</summary>
        Int32 Bonus { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}