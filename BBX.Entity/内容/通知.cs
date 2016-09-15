﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace BBX.Entity
{
    /// <summary>通知</summary>
    [Serializable]
    [DataObject]
    [Description("通知")]
    [BindTable("Notice", Description = "通知", ConnName = "BBX", DbType = DatabaseType.SqlServer)]
    public partial class Notice : INotice
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

        private Int32 _Uid;
        /// <summary>用户编号</summary>
        [DisplayName("用户编号")]
        [Description("用户编号")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(2, "Uid", "用户编号", null, "int", 10, 0, false)]
        public virtual Int32 Uid
        {
            get { return _Uid; }
            set { if (OnPropertyChanging(__.Uid, value)) { _Uid = value; OnPropertyChanged(__.Uid); } }
        }

        private Int32 _Type;
        /// <summary>类型</summary>
        [DisplayName("类型")]
        [Description("类型")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(3, "Type", "类型", null, "int", 10, 0, false)]
        public virtual Int32 Type
        {
            get { return _Type; }
            set { if (OnPropertyChanging(__.Type, value)) { _Type = value; OnPropertyChanged(__.Type); } }
        }

        private Int32 _New;
        /// <summary>新增功能</summary>
        [DisplayName("新增功能")]
        [Description("新增功能")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(4, "New", "新增功能", null, "int", 10, 0, false)]
        public virtual Int32 New
        {
            get { return _New; }
            set { if (OnPropertyChanging(__.New, value)) { _New = value; OnPropertyChanged(__.New); } }
        }

        private Int32 _PosterID;
        /// <summary>发送人</summary>
        [DisplayName("发送人")]
        [Description("发送人")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(5, "PosterID", "发送人", null, "int", 10, 0, false)]
        public virtual Int32 PosterID
        {
            get { return _PosterID; }
            set { if (OnPropertyChanging(__.PosterID, value)) { _PosterID = value; OnPropertyChanged(__.PosterID); } }
        }

        private String _Poster;
        /// <summary>发送人</summary>
        [DisplayName("发送人")]
        [Description("发送人")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(6, "Poster", "发送人", null, "nvarchar(50)", 0, 0, true)]
        public virtual String Poster
        {
            get { return _Poster; }
            set { if (OnPropertyChanging(__.Poster, value)) { _Poster = value; OnPropertyChanged(__.Poster); } }
        }

        private String _Note;
        /// <summary>描述</summary>
        [DisplayName("描述")]
        [Description("描述")]
        [DataObjectField(false, false, false, -1)]
        [BindColumn(7, "Note", "描述", null, "ntext", 0, 0, true)]
        public virtual String Note
        {
            get { return _Note; }
            set { if (OnPropertyChanging(__.Note, value)) { _Note = value; OnPropertyChanged(__.Note); } }
        }

        private DateTime _PostDateTime;
        /// <summary>发送时间</summary>
        [DisplayName("发送时间")]
        [Description("发送时间")]
        [DataObjectField(false, false, true, 3)]
        [BindColumn(8, "PostDateTime", "发送时间", null, "datetime", 3, 0, false)]
        public virtual DateTime PostDateTime
        {
            get { return _PostDateTime; }
            set { if (OnPropertyChanging(__.PostDateTime, value)) { _PostDateTime = value; OnPropertyChanged(__.PostDateTime); } }
        }

        private Int32 _FromID;
        /// <summary>从</summary>
        [DisplayName("从")]
        [Description("从")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(9, "FromID", "从", null, "int", 10, 0, false)]
        public virtual Int32 FromID
        {
            get { return _FromID; }
            set { if (OnPropertyChanging(__.FromID, value)) { _FromID = value; OnPropertyChanged(__.FromID); } }
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
                    case __.Uid : return _Uid;
                    case __.Type : return _Type;
                    case __.New : return _New;
                    case __.PosterID : return _PosterID;
                    case __.Poster : return _Poster;
                    case __.Note : return _Note;
                    case __.PostDateTime : return _PostDateTime;
                    case __.FromID : return _FromID;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.ID : _ID = Convert.ToInt32(value); break;
                    case __.Uid : _Uid = Convert.ToInt32(value); break;
                    case __.Type : _Type = Convert.ToInt32(value); break;
                    case __.New : _New = Convert.ToInt32(value); break;
                    case __.PosterID : _PosterID = Convert.ToInt32(value); break;
                    case __.Poster : _Poster = Convert.ToString(value); break;
                    case __.Note : _Note = Convert.ToString(value); break;
                    case __.PostDateTime : _PostDateTime = Convert.ToDateTime(value); break;
                    case __.FromID : _FromID = Convert.ToInt32(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得通知字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号</summary>
            public static readonly Field ID = FindByName(__.ID);

            ///<summary>用户编号</summary>
            public static readonly Field Uid = FindByName(__.Uid);

            ///<summary>类型</summary>
            public static readonly Field Type = FindByName(__.Type);

            ///<summary>新增功能</summary>
            public static readonly Field New = FindByName(__.New);

            ///<summary>发送人</summary>
            public static readonly Field PosterID = FindByName(__.PosterID);

            ///<summary>发送人</summary>
            public static readonly Field Poster = FindByName(__.Poster);

            ///<summary>描述</summary>
            public static readonly Field Note = FindByName(__.Note);

            ///<summary>发送时间</summary>
            public static readonly Field PostDateTime = FindByName(__.PostDateTime);

            ///<summary>从</summary>
            public static readonly Field FromID = FindByName(__.FromID);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得通知字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号</summary>
            public const String ID = "ID";

            ///<summary>用户编号</summary>
            public const String Uid = "Uid";

            ///<summary>类型</summary>
            public const String Type = "Type";

            ///<summary>新增功能</summary>
            public const String New = "New";

            ///<summary>发送人</summary>
            public const String PosterID = "PosterID";

            ///<summary>发送人</summary>
            public const String Poster = "Poster";

            ///<summary>描述</summary>
            public const String Note = "Note";

            ///<summary>发送时间</summary>
            public const String PostDateTime = "PostDateTime";

            ///<summary>从</summary>
            public const String FromID = "FromID";

        }
        #endregion
    }

    /// <summary>通知接口</summary>
    public partial interface INotice
    {
        #region 属性
        /// <summary>编号</summary>
        Int32 ID { get; set; }

        /// <summary>用户编号</summary>
        Int32 Uid { get; set; }

        /// <summary>类型</summary>
        Int32 Type { get; set; }

        /// <summary>新增功能</summary>
        Int32 New { get; set; }

        /// <summary>发送人</summary>
        Int32 PosterID { get; set; }

        /// <summary>发送人</summary>
        String Poster { get; set; }

        /// <summary>描述</summary>
        String Note { get; set; }

        /// <summary>发送时间</summary>
        DateTime PostDateTime { get; set; }

        /// <summary>从</summary>
        Int32 FromID { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}