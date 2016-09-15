﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace BBX.Entity
{
    /// <summary>搜索缓存</summary>
    [Serializable]
    [DataObject]
    [Description("搜索缓存")]
    [BindIndex("IX_SearchCache_searchstring_groupid", false, "searchstring,groupid")]
    [BindTable("SearchCache", Description = "搜索缓存", ConnName = "BBX", DbType = DatabaseType.SqlServer)]
    public partial class SearchCache : ISearchCache
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

        private String _Keywords;
        /// <summary>关键字</summary>
        [DisplayName("关键字")]
        [Description("关键字")]
        [DataObjectField(false, false, false, 255)]
        [BindColumn(2, "Keywords", "关键字", null, "nvarchar(255)", 0, 0, true)]
        public virtual String Keywords
        {
            get { return _Keywords; }
            set { if (OnPropertyChanging(__.Keywords, value)) { _Keywords = value; OnPropertyChanged(__.Keywords); } }
        }

        private String _Searchstring;
        /// <summary></summary>
        [DisplayName("Searchstring")]
        [Description("")]
        [DataObjectField(false, false, false, 255)]
        [BindColumn(3, "Searchstring", "", null, "nvarchar(255)", 0, 0, true)]
        public virtual String Searchstring
        {
            get { return _Searchstring; }
            set { if (OnPropertyChanging(__.Searchstring, value)) { _Searchstring = value; OnPropertyChanged(__.Searchstring); } }
        }

        private String _Ip;
        /// <summary>IP地址</summary>
        [DisplayName("IP地址")]
        [Description("IP地址")]
        [DataObjectField(false, false, true, 15)]
        [BindColumn(4, "Ip", "IP地址", null, "nvarchar(15)", 0, 0, true)]
        public virtual String Ip
        {
            get { return _Ip; }
            set { if (OnPropertyChanging(__.Ip, value)) { _Ip = value; OnPropertyChanged(__.Ip); } }
        }

        private Int32 _Uid;
        /// <summary>用户编号</summary>
        [DisplayName("用户编号")]
        [Description("用户编号")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(5, "Uid", "用户编号", null, "int", 10, 0, false)]
        public virtual Int32 Uid
        {
            get { return _Uid; }
            set { if (OnPropertyChanging(__.Uid, value)) { _Uid = value; OnPropertyChanged(__.Uid); } }
        }

        private Int32 _GroupID;
        /// <summary>聊天组</summary>
        [DisplayName("聊天组")]
        [Description("聊天组")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(6, "GroupID", "聊天组", null, "int", 10, 0, false)]
        public virtual Int32 GroupID
        {
            get { return _GroupID; }
            set { if (OnPropertyChanging(__.GroupID, value)) { _GroupID = value; OnPropertyChanged(__.GroupID); } }
        }

        private DateTime _PostDateTime;
        /// <summary>发送时间</summary>
        [DisplayName("发送时间")]
        [Description("发送时间")]
        [DataObjectField(false, false, true, 3)]
        [BindColumn(7, "PostDateTime", "发送时间", null, "datetime", 3, 0, false)]
        public virtual DateTime PostDateTime
        {
            get { return _PostDateTime; }
            set { if (OnPropertyChanging(__.PostDateTime, value)) { _PostDateTime = value; OnPropertyChanged(__.PostDateTime); } }
        }

        private DateTime _Expiration;
        /// <summary>过期时间</summary>
        [DisplayName("过期时间")]
        [Description("过期时间")]
        [DataObjectField(false, false, true, 3)]
        [BindColumn(8, "Expiration", "过期时间", null, "datetime", 3, 0, false)]
        public virtual DateTime Expiration
        {
            get { return _Expiration; }
            set { if (OnPropertyChanging(__.Expiration, value)) { _Expiration = value; OnPropertyChanged(__.Expiration); } }
        }

        private Int32 _Topics;
        /// <summary>主题</summary>
        [DisplayName("主题")]
        [Description("主题")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(9, "Topics", "主题", null, "int", 10, 0, false)]
        public virtual Int32 Topics
        {
            get { return _Topics; }
            set { if (OnPropertyChanging(__.Topics, value)) { _Topics = value; OnPropertyChanged(__.Topics); } }
        }

        private String _Tids;
        /// <summary></summary>
        [DisplayName("Tids")]
        [Description("")]
        [DataObjectField(false, false, false, -1)]
        [BindColumn(10, "Tids", "", null, "ntext", 0, 0, true)]
        public virtual String Tids
        {
            get { return _Tids; }
            set { if (OnPropertyChanging(__.Tids, value)) { _Tids = value; OnPropertyChanged(__.Tids); } }
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
                    case __.Keywords : return _Keywords;
                    case __.Searchstring : return _Searchstring;
                    case __.Ip : return _Ip;
                    case __.Uid : return _Uid;
                    case __.GroupID : return _GroupID;
                    case __.PostDateTime : return _PostDateTime;
                    case __.Expiration : return _Expiration;
                    case __.Topics : return _Topics;
                    case __.Tids : return _Tids;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.ID : _ID = Convert.ToInt32(value); break;
                    case __.Keywords : _Keywords = Convert.ToString(value); break;
                    case __.Searchstring : _Searchstring = Convert.ToString(value); break;
                    case __.Ip : _Ip = Convert.ToString(value); break;
                    case __.Uid : _Uid = Convert.ToInt32(value); break;
                    case __.GroupID : _GroupID = Convert.ToInt32(value); break;
                    case __.PostDateTime : _PostDateTime = Convert.ToDateTime(value); break;
                    case __.Expiration : _Expiration = Convert.ToDateTime(value); break;
                    case __.Topics : _Topics = Convert.ToInt32(value); break;
                    case __.Tids : _Tids = Convert.ToString(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得搜索缓存字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号</summary>
            public static readonly Field ID = FindByName(__.ID);

            ///<summary>关键字</summary>
            public static readonly Field Keywords = FindByName(__.Keywords);

            ///<summary></summary>
            public static readonly Field Searchstring = FindByName(__.Searchstring);

            ///<summary>IP地址</summary>
            public static readonly Field Ip = FindByName(__.Ip);

            ///<summary>用户编号</summary>
            public static readonly Field Uid = FindByName(__.Uid);

            ///<summary>聊天组</summary>
            public static readonly Field GroupID = FindByName(__.GroupID);

            ///<summary>发送时间</summary>
            public static readonly Field PostDateTime = FindByName(__.PostDateTime);

            ///<summary>过期时间</summary>
            public static readonly Field Expiration = FindByName(__.Expiration);

            ///<summary>主题</summary>
            public static readonly Field Topics = FindByName(__.Topics);

            ///<summary></summary>
            public static readonly Field Tids = FindByName(__.Tids);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得搜索缓存字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号</summary>
            public const String ID = "ID";

            ///<summary>关键字</summary>
            public const String Keywords = "Keywords";

            ///<summary></summary>
            public const String Searchstring = "Searchstring";

            ///<summary>IP地址</summary>
            public const String Ip = "Ip";

            ///<summary>用户编号</summary>
            public const String Uid = "Uid";

            ///<summary>聊天组</summary>
            public const String GroupID = "GroupID";

            ///<summary>发送时间</summary>
            public const String PostDateTime = "PostDateTime";

            ///<summary>过期时间</summary>
            public const String Expiration = "Expiration";

            ///<summary>主题</summary>
            public const String Topics = "Topics";

            ///<summary></summary>
            public const String Tids = "Tids";

        }
        #endregion
    }

    /// <summary>搜索缓存接口</summary>
    public partial interface ISearchCache
    {
        #region 属性
        /// <summary>编号</summary>
        Int32 ID { get; set; }

        /// <summary>关键字</summary>
        String Keywords { get; set; }

        /// <summary></summary>
        String Searchstring { get; set; }

        /// <summary>IP地址</summary>
        String Ip { get; set; }

        /// <summary>用户编号</summary>
        Int32 Uid { get; set; }

        /// <summary>聊天组</summary>
        Int32 GroupID { get; set; }

        /// <summary>发送时间</summary>
        DateTime PostDateTime { get; set; }

        /// <summary>过期时间</summary>
        DateTime Expiration { get; set; }

        /// <summary>主题</summary>
        Int32 Topics { get; set; }

        /// <summary></summary>
        String Tids { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}