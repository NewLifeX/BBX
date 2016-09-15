﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace BBX.Entity
{
    /// <summary>版主管理日志</summary>
    [Serializable]
    [DataObject]
    [Description("版主管理日志")]
    [BindIndex("IX_ModeratorManageLog_tid", false, "tid")]
    [BindTable("ModeratorManageLog", Description = "版主管理日志", ConnName = "BBX", DbType = DatabaseType.SqlServer)]
    public partial class ModeratorManageLog : IModeratorManageLog
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

        private Int32 _ModeratoruID;
        /// <summary>版主编号</summary>
        [DisplayName("版主编号")]
        [Description("版主编号")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(2, "ModeratoruID", "版主编号", null, "int", 10, 0, false)]
        public virtual Int32 ModeratoruID
        {
            get { return _ModeratoruID; }
            set { if (OnPropertyChanging(__.ModeratoruID, value)) { _ModeratoruID = value; OnPropertyChanged(__.ModeratoruID); } }
        }

        private String _ModeratorName;
        /// <summary>版主名称</summary>
        [DisplayName("版主名称")]
        [Description("版主名称")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(3, "ModeratorName", "版主名称", null, "nvarchar(50)", 0, 0, true)]
        public virtual String ModeratorName
        {
            get { return _ModeratorName; }
            set { if (OnPropertyChanging(__.ModeratorName, value)) { _ModeratorName = value; OnPropertyChanged(__.ModeratorName); } }
        }

        private Int32 _GroupID;
        /// <summary>聊天组</summary>
        [DisplayName("聊天组")]
        [Description("聊天组")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(4, "GroupID", "聊天组", null, "int", 10, 0, false)]
        public virtual Int32 GroupID
        {
            get { return _GroupID; }
            set { if (OnPropertyChanging(__.GroupID, value)) { _GroupID = value; OnPropertyChanged(__.GroupID); } }
        }

        private String _GroupTitle;
        /// <summary>分组标签</summary>
        [DisplayName("分组标签")]
        [Description("分组标签")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(5, "GroupTitle", "分组标签", null, "nvarchar(50)", 0, 0, true)]
        public virtual String GroupTitle
        {
            get { return _GroupTitle; }
            set { if (OnPropertyChanging(__.GroupTitle, value)) { _GroupTitle = value; OnPropertyChanged(__.GroupTitle); } }
        }

        private String _IP;
        /// <summary>IP地址</summary>
        [DisplayName("IP地址")]
        [Description("IP地址")]
        [DataObjectField(false, false, true, 15)]
        [BindColumn(6, "IP", "IP地址", null, "nvarchar(15)", 0, 0, true)]
        public virtual String IP
        {
            get { return _IP; }
            set { if (OnPropertyChanging(__.IP, value)) { _IP = value; OnPropertyChanged(__.IP); } }
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

        private Int32 _Fid;
        /// <summary>论坛编号</summary>
        [DisplayName("论坛编号")]
        [Description("论坛编号")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(8, "Fid", "论坛编号", null, "int", 10, 0, false)]
        public virtual Int32 Fid
        {
            get { return _Fid; }
            set { if (OnPropertyChanging(__.Fid, value)) { _Fid = value; OnPropertyChanged(__.Fid); } }
        }

        private String _FName;
        /// <summary>论坛名称</summary>
        [DisplayName("论坛名称")]
        [Description("论坛名称")]
        [DataObjectField(false, false, true, 100)]
        [BindColumn(9, "FName", "论坛名称", null, "nvarchar(100)", 0, 0, true)]
        public virtual String FName
        {
            get { return _FName; }
            set { if (OnPropertyChanging(__.FName, value)) { _FName = value; OnPropertyChanged(__.FName); } }
        }

        private Int32 _Tid;
        /// <summary>主题编号</summary>
        [DisplayName("主题编号")]
        [Description("主题编号")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(10, "Tid", "主题编号", null, "int", 10, 0, false)]
        public virtual Int32 Tid
        {
            get { return _Tid; }
            set { if (OnPropertyChanging(__.Tid, value)) { _Tid = value; OnPropertyChanged(__.Tid); } }
        }

        private String _Title;
        /// <summary>事项名称</summary>
        [DisplayName("事项名称")]
        [Description("事项名称")]
        [DataObjectField(false, false, true, 200)]
        [BindColumn(11, "Title", "事项名称", null, "nvarchar(200)", 0, 0, true, Master=true)]
        public virtual String Title
        {
            get { return _Title; }
            set { if (OnPropertyChanging(__.Title, value)) { _Title = value; OnPropertyChanged(__.Title); } }
        }

        private String _Actions;
        /// <summary>行动</summary>
        [DisplayName("行动")]
        [Description("行动")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(12, "Actions", "行动", null, "nvarchar(50)", 0, 0, true)]
        public virtual String Actions
        {
            get { return _Actions; }
            set { if (OnPropertyChanging(__.Actions, value)) { _Actions = value; OnPropertyChanged(__.Actions); } }
        }

        private String _Reason;
        /// <summary>原因</summary>
        [DisplayName("原因")]
        [Description("原因")]
        [DataObjectField(false, false, true, 200)]
        [BindColumn(13, "Reason", "原因", null, "nvarchar(200)", 0, 0, true)]
        public virtual String Reason
        {
            get { return _Reason; }
            set { if (OnPropertyChanging(__.Reason, value)) { _Reason = value; OnPropertyChanged(__.Reason); } }
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
                    case __.ModeratoruID : return _ModeratoruID;
                    case __.ModeratorName : return _ModeratorName;
                    case __.GroupID : return _GroupID;
                    case __.GroupTitle : return _GroupTitle;
                    case __.IP : return _IP;
                    case __.PostDateTime : return _PostDateTime;
                    case __.Fid : return _Fid;
                    case __.FName : return _FName;
                    case __.Tid : return _Tid;
                    case __.Title : return _Title;
                    case __.Actions : return _Actions;
                    case __.Reason : return _Reason;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.ID : _ID = Convert.ToInt32(value); break;
                    case __.ModeratoruID : _ModeratoruID = Convert.ToInt32(value); break;
                    case __.ModeratorName : _ModeratorName = Convert.ToString(value); break;
                    case __.GroupID : _GroupID = Convert.ToInt32(value); break;
                    case __.GroupTitle : _GroupTitle = Convert.ToString(value); break;
                    case __.IP : _IP = Convert.ToString(value); break;
                    case __.PostDateTime : _PostDateTime = Convert.ToDateTime(value); break;
                    case __.Fid : _Fid = Convert.ToInt32(value); break;
                    case __.FName : _FName = Convert.ToString(value); break;
                    case __.Tid : _Tid = Convert.ToInt32(value); break;
                    case __.Title : _Title = Convert.ToString(value); break;
                    case __.Actions : _Actions = Convert.ToString(value); break;
                    case __.Reason : _Reason = Convert.ToString(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得版主管理日志字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号</summary>
            public static readonly Field ID = FindByName(__.ID);

            ///<summary>版主编号</summary>
            public static readonly Field ModeratoruID = FindByName(__.ModeratoruID);

            ///<summary>版主名称</summary>
            public static readonly Field ModeratorName = FindByName(__.ModeratorName);

            ///<summary>聊天组</summary>
            public static readonly Field GroupID = FindByName(__.GroupID);

            ///<summary>分组标签</summary>
            public static readonly Field GroupTitle = FindByName(__.GroupTitle);

            ///<summary>IP地址</summary>
            public static readonly Field IP = FindByName(__.IP);

            ///<summary>发送时间</summary>
            public static readonly Field PostDateTime = FindByName(__.PostDateTime);

            ///<summary>论坛编号</summary>
            public static readonly Field Fid = FindByName(__.Fid);

            ///<summary>论坛名称</summary>
            public static readonly Field FName = FindByName(__.FName);

            ///<summary>主题编号</summary>
            public static readonly Field Tid = FindByName(__.Tid);

            ///<summary>事项名称</summary>
            public static readonly Field Title = FindByName(__.Title);

            ///<summary>行动</summary>
            public static readonly Field Actions = FindByName(__.Actions);

            ///<summary>原因</summary>
            public static readonly Field Reason = FindByName(__.Reason);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得版主管理日志字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号</summary>
            public const String ID = "ID";

            ///<summary>版主编号</summary>
            public const String ModeratoruID = "ModeratoruID";

            ///<summary>版主名称</summary>
            public const String ModeratorName = "ModeratorName";

            ///<summary>聊天组</summary>
            public const String GroupID = "GroupID";

            ///<summary>分组标签</summary>
            public const String GroupTitle = "GroupTitle";

            ///<summary>IP地址</summary>
            public const String IP = "IP";

            ///<summary>发送时间</summary>
            public const String PostDateTime = "PostDateTime";

            ///<summary>论坛编号</summary>
            public const String Fid = "Fid";

            ///<summary>论坛名称</summary>
            public const String FName = "FName";

            ///<summary>主题编号</summary>
            public const String Tid = "Tid";

            ///<summary>事项名称</summary>
            public const String Title = "Title";

            ///<summary>行动</summary>
            public const String Actions = "Actions";

            ///<summary>原因</summary>
            public const String Reason = "Reason";

        }
        #endregion
    }

    /// <summary>版主管理日志接口</summary>
    public partial interface IModeratorManageLog
    {
        #region 属性
        /// <summary>编号</summary>
        Int32 ID { get; set; }

        /// <summary>版主编号</summary>
        Int32 ModeratoruID { get; set; }

        /// <summary>版主名称</summary>
        String ModeratorName { get; set; }

        /// <summary>聊天组</summary>
        Int32 GroupID { get; set; }

        /// <summary>分组标签</summary>
        String GroupTitle { get; set; }

        /// <summary>IP地址</summary>
        String IP { get; set; }

        /// <summary>发送时间</summary>
        DateTime PostDateTime { get; set; }

        /// <summary>论坛编号</summary>
        Int32 Fid { get; set; }

        /// <summary>论坛名称</summary>
        String FName { get; set; }

        /// <summary>主题编号</summary>
        Int32 Tid { get; set; }

        /// <summary>事项名称</summary>
        String Title { get; set; }

        /// <summary>行动</summary>
        String Actions { get; set; }

        /// <summary>原因</summary>
        String Reason { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}