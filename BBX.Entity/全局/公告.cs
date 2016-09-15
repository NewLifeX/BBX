﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace BBX.Entity
{
    /// <summary>公告</summary>
    [Serializable]
    [DataObject]
    [Description("公告")]
    [BindTable("Announcement", Description = "公告", ConnName = "BBX", DbType = DatabaseType.SqlServer)]
    public partial class Announcement : IAnnouncement
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

        private String _Poster;
        /// <summary>发布人</summary>
        [DisplayName("发布人")]
        [Description("发布人")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(2, "Poster", "发布人", null, "nvarchar(50)", 0, 0, true)]
        public virtual String Poster
        {
            get { return _Poster; }
            set { if (OnPropertyChanging(__.Poster, value)) { _Poster = value; OnPropertyChanged(__.Poster); } }
        }

        private Int32 _PosterID;
        /// <summary>发布人</summary>
        [DisplayName("发布人")]
        [Description("发布人")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(3, "PosterID", "发布人", null, "int", 10, 0, false)]
        public virtual Int32 PosterID
        {
            get { return _PosterID; }
            set { if (OnPropertyChanging(__.PosterID, value)) { _PosterID = value; OnPropertyChanged(__.PosterID); } }
        }

        private String _Title;
        /// <summary>事项名称</summary>
        [DisplayName("事项名称")]
        [Description("事项名称")]
        [DataObjectField(false, false, false, 250)]
        [BindColumn(4, "Title", "事项名称", null, "nvarchar(250)", 0, 0, true, Master=true)]
        public virtual String Title
        {
            get { return _Title; }
            set { if (OnPropertyChanging(__.Title, value)) { _Title = value; OnPropertyChanged(__.Title); } }
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

        private DateTime _StartTime;
        /// <summary>开始时间</summary>
        [DisplayName("开始时间")]
        [Description("开始时间")]
        [DataObjectField(false, false, false, 3)]
        [BindColumn(6, "StartTime", "开始时间", null, "datetime", 3, 0, false)]
        public virtual DateTime StartTime
        {
            get { return _StartTime; }
            set { if (OnPropertyChanging(__.StartTime, value)) { _StartTime = value; OnPropertyChanged(__.StartTime); } }
        }

        private DateTime _EndTime;
        /// <summary>结束时间</summary>
        [DisplayName("结束时间")]
        [Description("结束时间")]
        [DataObjectField(false, false, false, 3)]
        [BindColumn(7, "EndTime", "结束时间", null, "datetime", 3, 0, false)]
        public virtual DateTime EndTime
        {
            get { return _EndTime; }
            set { if (OnPropertyChanging(__.EndTime, value)) { _EndTime = value; OnPropertyChanged(__.EndTime); } }
        }

        private String _Message;
        /// <summary>短消息内容</summary>
        [DisplayName("短消息内容")]
        [Description("短消息内容")]
        [DataObjectField(false, false, false, -1)]
        [BindColumn(8, "Message", "短消息内容", null, "ntext", 0, 0, true)]
        public virtual String Message
        {
            get { return _Message; }
            set { if (OnPropertyChanging(__.Message, value)) { _Message = value; OnPropertyChanged(__.Message); } }
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
                    case __.Poster : return _Poster;
                    case __.PosterID : return _PosterID;
                    case __.Title : return _Title;
                    case __.DisplayOrder : return _DisplayOrder;
                    case __.StartTime : return _StartTime;
                    case __.EndTime : return _EndTime;
                    case __.Message : return _Message;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.ID : _ID = Convert.ToInt32(value); break;
                    case __.Poster : _Poster = Convert.ToString(value); break;
                    case __.PosterID : _PosterID = Convert.ToInt32(value); break;
                    case __.Title : _Title = Convert.ToString(value); break;
                    case __.DisplayOrder : _DisplayOrder = Convert.ToInt32(value); break;
                    case __.StartTime : _StartTime = Convert.ToDateTime(value); break;
                    case __.EndTime : _EndTime = Convert.ToDateTime(value); break;
                    case __.Message : _Message = Convert.ToString(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得公告字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号</summary>
            public static readonly Field ID = FindByName(__.ID);

            ///<summary>发布人</summary>
            public static readonly Field Poster = FindByName(__.Poster);

            ///<summary>发布人</summary>
            public static readonly Field PosterID = FindByName(__.PosterID);

            ///<summary>事项名称</summary>
            public static readonly Field Title = FindByName(__.Title);

            ///<summary>显示顺序</summary>
            public static readonly Field DisplayOrder = FindByName(__.DisplayOrder);

            ///<summary>开始时间</summary>
            public static readonly Field StartTime = FindByName(__.StartTime);

            ///<summary>结束时间</summary>
            public static readonly Field EndTime = FindByName(__.EndTime);

            ///<summary>短消息内容</summary>
            public static readonly Field Message = FindByName(__.Message);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得公告字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号</summary>
            public const String ID = "ID";

            ///<summary>发布人</summary>
            public const String Poster = "Poster";

            ///<summary>发布人</summary>
            public const String PosterID = "PosterID";

            ///<summary>事项名称</summary>
            public const String Title = "Title";

            ///<summary>显示顺序</summary>
            public const String DisplayOrder = "DisplayOrder";

            ///<summary>开始时间</summary>
            public const String StartTime = "StartTime";

            ///<summary>结束时间</summary>
            public const String EndTime = "EndTime";

            ///<summary>短消息内容</summary>
            public const String Message = "Message";

        }
        #endregion
    }

    /// <summary>公告接口</summary>
    public partial interface IAnnouncement
    {
        #region 属性
        /// <summary>编号</summary>
        Int32 ID { get; set; }

        /// <summary>发布人</summary>
        String Poster { get; set; }

        /// <summary>发布人</summary>
        Int32 PosterID { get; set; }

        /// <summary>事项名称</summary>
        String Title { get; set; }

        /// <summary>显示顺序</summary>
        Int32 DisplayOrder { get; set; }

        /// <summary>开始时间</summary>
        DateTime StartTime { get; set; }

        /// <summary>结束时间</summary>
        DateTime EndTime { get; set; }

        /// <summary>短消息内容</summary>
        String Message { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}