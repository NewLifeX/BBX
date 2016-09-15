﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace BBX.Entity
{
    /// <summary>统计</summary>
    [Serializable]
    [DataObject]
    [Description("统计")]
    [BindTable("Statistic", Description = "统计", ConnName = "BBX", DbType = DatabaseType.SqlServer)]
    public partial class Statistic : IStatistic
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

        private Int32 _TotalTopic;
        /// <summary></summary>
        [DisplayName("TotalTopic")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(2, "TotalTopic", "", null, "int", 10, 0, false)]
        public virtual Int32 TotalTopic
        {
            get { return _TotalTopic; }
            set { if (OnPropertyChanging(__.TotalTopic, value)) { _TotalTopic = value; OnPropertyChanged(__.TotalTopic); } }
        }

        private Int32 _TotalPost;
        /// <summary></summary>
        [DisplayName("TotalPost")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(3, "TotalPost", "", null, "int", 10, 0, false)]
        public virtual Int32 TotalPost
        {
            get { return _TotalPost; }
            set { if (OnPropertyChanging(__.TotalPost, value)) { _TotalPost = value; OnPropertyChanged(__.TotalPost); } }
        }

        private Int32 _TotalUsers;
        /// <summary></summary>
        [DisplayName("TotalUsers")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(4, "TotalUsers", "", null, "int", 10, 0, false)]
        public virtual Int32 TotalUsers
        {
            get { return _TotalUsers; }
            set { if (OnPropertyChanging(__.TotalUsers, value)) { _TotalUsers = value; OnPropertyChanged(__.TotalUsers); } }
        }

        private String _LastUserName;
        /// <summary></summary>
        [DisplayName("LastUserName")]
        [Description("")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(5, "LastUserName", "", null, "nvarchar(50)", 0, 0, true)]
        public virtual String LastUserName
        {
            get { return _LastUserName; }
            set { if (OnPropertyChanging(__.LastUserName, value)) { _LastUserName = value; OnPropertyChanged(__.LastUserName); } }
        }

        private Int32 _LastUserID;
        /// <summary></summary>
        [DisplayName("LastUserID")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(6, "LastUserID", "", null, "int", 10, 0, false)]
        public virtual Int32 LastUserID
        {
            get { return _LastUserID; }
            set { if (OnPropertyChanging(__.LastUserID, value)) { _LastUserID = value; OnPropertyChanged(__.LastUserID); } }
        }

        private Int32 _HighestOnlineUserCount;
        /// <summary></summary>
        [DisplayName("HighestOnlineUserCount")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(7, "HighestOnlineUserCount", "", null, "int", 10, 0, false)]
        public virtual Int32 HighestOnlineUserCount
        {
            get { return _HighestOnlineUserCount; }
            set { if (OnPropertyChanging(__.HighestOnlineUserCount, value)) { _HighestOnlineUserCount = value; OnPropertyChanged(__.HighestOnlineUserCount); } }
        }

        private DateTime _HighestOnlineUserTime;
        /// <summary></summary>
        [DisplayName("HighestOnlineUserTime")]
        [Description("")]
        [DataObjectField(false, false, true, 3)]
        [BindColumn(8, "HighestOnlineUserTime", "", null, "datetime", 3, 0, false)]
        public virtual DateTime HighestOnlineUserTime
        {
            get { return _HighestOnlineUserTime; }
            set { if (OnPropertyChanging(__.HighestOnlineUserTime, value)) { _HighestOnlineUserTime = value; OnPropertyChanged(__.HighestOnlineUserTime); } }
        }

        private Int32 _YesterdayPosts;
        /// <summary></summary>
        [DisplayName("YesterdayPosts")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(9, "YesterdayPosts", "", null, "int", 10, 0, false)]
        public virtual Int32 YesterdayPosts
        {
            get { return _YesterdayPosts; }
            set { if (OnPropertyChanging(__.YesterdayPosts, value)) { _YesterdayPosts = value; OnPropertyChanged(__.YesterdayPosts); } }
        }

        private Int32 _HighestPosts;
        /// <summary></summary>
        [DisplayName("HighestPosts")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(10, "HighestPosts", "", null, "int", 10, 0, false)]
        public virtual Int32 HighestPosts
        {
            get { return _HighestPosts; }
            set { if (OnPropertyChanging(__.HighestPosts, value)) { _HighestPosts = value; OnPropertyChanged(__.HighestPosts); } }
        }

        private String _HighestPostsDate;
        /// <summary></summary>
        [DisplayName("HighestPostsDate")]
        [Description("")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(11, "HighestPostsDate", "", null, "nvarchar(50)", 0, 0, true)]
        public virtual String HighestPostsDate
        {
            get { return _HighestPostsDate; }
            set { if (OnPropertyChanging(__.HighestPostsDate, value)) { _HighestPostsDate = value; OnPropertyChanged(__.HighestPostsDate); } }
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
                    case __.TotalTopic : return _TotalTopic;
                    case __.TotalPost : return _TotalPost;
                    case __.TotalUsers : return _TotalUsers;
                    case __.LastUserName : return _LastUserName;
                    case __.LastUserID : return _LastUserID;
                    case __.HighestOnlineUserCount : return _HighestOnlineUserCount;
                    case __.HighestOnlineUserTime : return _HighestOnlineUserTime;
                    case __.YesterdayPosts : return _YesterdayPosts;
                    case __.HighestPosts : return _HighestPosts;
                    case __.HighestPostsDate : return _HighestPostsDate;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.ID : _ID = Convert.ToInt32(value); break;
                    case __.TotalTopic : _TotalTopic = Convert.ToInt32(value); break;
                    case __.TotalPost : _TotalPost = Convert.ToInt32(value); break;
                    case __.TotalUsers : _TotalUsers = Convert.ToInt32(value); break;
                    case __.LastUserName : _LastUserName = Convert.ToString(value); break;
                    case __.LastUserID : _LastUserID = Convert.ToInt32(value); break;
                    case __.HighestOnlineUserCount : _HighestOnlineUserCount = Convert.ToInt32(value); break;
                    case __.HighestOnlineUserTime : _HighestOnlineUserTime = Convert.ToDateTime(value); break;
                    case __.YesterdayPosts : _YesterdayPosts = Convert.ToInt32(value); break;
                    case __.HighestPosts : _HighestPosts = Convert.ToInt32(value); break;
                    case __.HighestPostsDate : _HighestPostsDate = Convert.ToString(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得统计字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号</summary>
            public static readonly Field ID = FindByName(__.ID);

            ///<summary></summary>
            public static readonly Field TotalTopic = FindByName(__.TotalTopic);

            ///<summary></summary>
            public static readonly Field TotalPost = FindByName(__.TotalPost);

            ///<summary></summary>
            public static readonly Field TotalUsers = FindByName(__.TotalUsers);

            ///<summary></summary>
            public static readonly Field LastUserName = FindByName(__.LastUserName);

            ///<summary></summary>
            public static readonly Field LastUserID = FindByName(__.LastUserID);

            ///<summary></summary>
            public static readonly Field HighestOnlineUserCount = FindByName(__.HighestOnlineUserCount);

            ///<summary></summary>
            public static readonly Field HighestOnlineUserTime = FindByName(__.HighestOnlineUserTime);

            ///<summary></summary>
            public static readonly Field YesterdayPosts = FindByName(__.YesterdayPosts);

            ///<summary></summary>
            public static readonly Field HighestPosts = FindByName(__.HighestPosts);

            ///<summary></summary>
            public static readonly Field HighestPostsDate = FindByName(__.HighestPostsDate);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得统计字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号</summary>
            public const String ID = "ID";

            ///<summary></summary>
            public const String TotalTopic = "TotalTopic";

            ///<summary></summary>
            public const String TotalPost = "TotalPost";

            ///<summary></summary>
            public const String TotalUsers = "TotalUsers";

            ///<summary></summary>
            public const String LastUserName = "LastUserName";

            ///<summary></summary>
            public const String LastUserID = "LastUserID";

            ///<summary></summary>
            public const String HighestOnlineUserCount = "HighestOnlineUserCount";

            ///<summary></summary>
            public const String HighestOnlineUserTime = "HighestOnlineUserTime";

            ///<summary></summary>
            public const String YesterdayPosts = "YesterdayPosts";

            ///<summary></summary>
            public const String HighestPosts = "HighestPosts";

            ///<summary></summary>
            public const String HighestPostsDate = "HighestPostsDate";

        }
        #endregion
    }

    /// <summary>统计接口</summary>
    public partial interface IStatistic
    {
        #region 属性
        /// <summary>编号</summary>
        Int32 ID { get; set; }

        /// <summary></summary>
        Int32 TotalTopic { get; set; }

        /// <summary></summary>
        Int32 TotalPost { get; set; }

        /// <summary></summary>
        Int32 TotalUsers { get; set; }

        /// <summary></summary>
        String LastUserName { get; set; }

        /// <summary></summary>
        Int32 LastUserID { get; set; }

        /// <summary></summary>
        Int32 HighestOnlineUserCount { get; set; }

        /// <summary></summary>
        DateTime HighestOnlineUserTime { get; set; }

        /// <summary></summary>
        Int32 YesterdayPosts { get; set; }

        /// <summary></summary>
        Int32 HighestPosts { get; set; }

        /// <summary></summary>
        String HighestPostsDate { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}