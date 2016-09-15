﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace BBX.Entity
{
    /// <summary>在线</summary>
    [Serializable]
    [DataObject]
    [Description("在线")]
    [BindIndex("IX_Online_LastUpdateTime", false, "LastUpdateTime")]
    [BindIndex("IX_Online_ForumID", false, "ForumID")]
    [BindTable("Online", Description = "在线", ConnName = "BBX_Online", DbType = DatabaseType.SqlServer)]
    public partial class Online : IOnline
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

        private String _SessionID;
        /// <summary>会话标识</summary>
        [DisplayName("会话标识")]
        [Description("会话标识")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(2, "SessionID", "会话标识", null, "nvarchar(50)", 0, 0, true)]
        public virtual String SessionID
        {
            get { return _SessionID; }
            set { if (OnPropertyChanging(__.SessionID, value)) { _SessionID = value; OnPropertyChanged(__.SessionID); } }
        }

        private Int32 _UserID;
        /// <summary>用户编号</summary>
        [DisplayName("用户编号")]
        [Description("用户编号")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(3, "UserID", "用户编号", null, "int", 10, 0, false)]
        public virtual Int32 UserID
        {
            get { return _UserID; }
            set { if (OnPropertyChanging(__.UserID, value)) { _UserID = value; OnPropertyChanged(__.UserID); } }
        }

        private String _IP;
        /// <summary>IP地址</summary>
        [DisplayName("IP地址")]
        [Description("IP地址")]
        [DataObjectField(false, false, true, 15)]
        [BindColumn(4, "IP", "IP地址", null, "nvarchar(15)", 0, 0, true)]
        public virtual String IP
        {
            get { return _IP; }
            set { if (OnPropertyChanging(__.IP, value)) { _IP = value; OnPropertyChanged(__.IP); } }
        }

        private String _UserName;
        /// <summary>登录账户</summary>
        [DisplayName("登录账户")]
        [Description("登录账户")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(5, "UserName", "登录账户", null, "nvarchar(50)", 0, 0, true)]
        public virtual String UserName
        {
            get { return _UserName; }
            set { if (OnPropertyChanging(__.UserName, value)) { _UserName = value; OnPropertyChanged(__.UserName); } }
        }

        private String _NickName;
        /// <summary>昵称。如果为空,则显示UserName</summary>
        [DisplayName("昵称")]
        [Description("昵称。如果为空,则显示UserName")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(6, "NickName", "昵称。如果为空,则显示UserName", null, "nvarchar(50)", 0, 0, true)]
        public virtual String NickName
        {
            get { return _NickName; }
            set { if (OnPropertyChanging(__.NickName, value)) { _NickName = value; OnPropertyChanged(__.NickName); } }
        }

        private String _Password;
        /// <summary>登录密码</summary>
        [DisplayName("登录密码")]
        [Description("登录密码")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(7, "Password", "登录密码", null, "nvarchar(50)", 0, 0, true)]
        public virtual String Password
        {
            get { return _Password; }
            set { if (OnPropertyChanging(__.Password, value)) { _Password = value; OnPropertyChanged(__.Password); } }
        }

        private Int32 _GroupID;
        /// <summary>聊天组</summary>
        [DisplayName("聊天组")]
        [Description("聊天组")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(8, "GroupID", "聊天组", null, "int", 10, 0, false)]
        public virtual Int32 GroupID
        {
            get { return _GroupID; }
            set { if (OnPropertyChanging(__.GroupID, value)) { _GroupID = value; OnPropertyChanged(__.GroupID); } }
        }

        private String _Olimg;
        /// <summary>在线图标</summary>
        [DisplayName("在线图标")]
        [Description("在线图标")]
        [DataObjectField(false, false, true, 80)]
        [BindColumn(9, "Olimg", "在线图标", null, "nvarchar(80)", 0, 0, true)]
        public virtual String Olimg
        {
            get { return _Olimg; }
            set { if (OnPropertyChanging(__.Olimg, value)) { _Olimg = value; OnPropertyChanged(__.Olimg); } }
        }

        private Int32 _AdminID;
        /// <summary>操作人</summary>
        [DisplayName("操作人")]
        [Description("操作人")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(10, "AdminID", "操作人", null, "int", 10, 0, false)]
        public virtual Int32 AdminID
        {
            get { return _AdminID; }
            set { if (OnPropertyChanging(__.AdminID, value)) { _AdminID = value; OnPropertyChanged(__.AdminID); } }
        }

        private Boolean _Invisible;
        /// <summary>是否隐身</summary>
        [DisplayName("是否隐身")]
        [Description("是否隐身")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(11, "Invisible", "是否隐身", null, "bit", 0, 0, false)]
        public virtual Boolean Invisible
        {
            get { return _Invisible; }
            set { if (OnPropertyChanging(__.Invisible, value)) { _Invisible = value; OnPropertyChanged(__.Invisible); } }
        }

        private Int32 _Action;
        /// <summary>动作</summary>
        [DisplayName("动作")]
        [Description("动作")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(12, "Action", "动作", null, "int", 10, 0, false)]
        public virtual Int32 Action
        {
            get { return _Action; }
            set { if (OnPropertyChanging(__.Action, value)) { _Action = value; OnPropertyChanged(__.Action); } }
        }

        private Int32 _LastActivity;
        /// <summary>最后活动时间</summary>
        [DisplayName("最后活动时间")]
        [Description("最后活动时间")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(13, "LastActivity", "最后活动时间", null, "int", 10, 0, false)]
        public virtual Int32 LastActivity
        {
            get { return _LastActivity; }
            set { if (OnPropertyChanging(__.LastActivity, value)) { _LastActivity = value; OnPropertyChanged(__.LastActivity); } }
        }

        private DateTime _LastPostTime;
        /// <summary>最后发帖时间</summary>
        [DisplayName("最后发帖时间")]
        [Description("最后发帖时间")]
        [DataObjectField(false, false, true, 3)]
        [BindColumn(14, "LastPostTime", "最后发帖时间", null, "datetime", 3, 0, false)]
        public virtual DateTime LastPostTime
        {
            get { return _LastPostTime; }
            set { if (OnPropertyChanging(__.LastPostTime, value)) { _LastPostTime = value; OnPropertyChanged(__.LastPostTime); } }
        }

        private DateTime _LastPostpmTime;
        /// <summary>最后私信时间</summary>
        [DisplayName("最后私信时间")]
        [Description("最后私信时间")]
        [DataObjectField(false, false, true, 3)]
        [BindColumn(15, "LastPostpmTime", "最后私信时间", null, "datetime", 3, 0, false)]
        public virtual DateTime LastPostpmTime
        {
            get { return _LastPostpmTime; }
            set { if (OnPropertyChanging(__.LastPostpmTime, value)) { _LastPostpmTime = value; OnPropertyChanged(__.LastPostpmTime); } }
        }

        private DateTime _LastSearchTime;
        /// <summary>最后搜索时间</summary>
        [DisplayName("最后搜索时间")]
        [Description("最后搜索时间")]
        [DataObjectField(false, false, true, 3)]
        [BindColumn(16, "LastSearchTime", "最后搜索时间", null, "datetime", 3, 0, false)]
        public virtual DateTime LastSearchTime
        {
            get { return _LastSearchTime; }
            set { if (OnPropertyChanging(__.LastSearchTime, value)) { _LastSearchTime = value; OnPropertyChanged(__.LastSearchTime); } }
        }

        private DateTime _LastUpdateTime;
        /// <summary>最后更新时间</summary>
        [DisplayName("最后更新时间")]
        [Description("最后更新时间")]
        [DataObjectField(false, false, true, 3)]
        [BindColumn(17, "LastUpdateTime", "最后更新时间", null, "datetime", 3, 0, false)]
        public virtual DateTime LastUpdateTime
        {
            get { return _LastUpdateTime; }
            set { if (OnPropertyChanging(__.LastUpdateTime, value)) { _LastUpdateTime = value; OnPropertyChanged(__.LastUpdateTime); } }
        }

        private Int32 _ForumID;
        /// <summary>论坛编号</summary>
        [DisplayName("论坛编号")]
        [Description("论坛编号")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(18, "ForumID", "论坛编号", null, "int", 10, 0, false)]
        public virtual Int32 ForumID
        {
            get { return _ForumID; }
            set { if (OnPropertyChanging(__.ForumID, value)) { _ForumID = value; OnPropertyChanged(__.ForumID); } }
        }

        private String _ForumName;
        /// <summary>论坛名称</summary>
        [DisplayName("论坛名称")]
        [Description("论坛名称")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(19, "ForumName", "论坛名称", null, "nvarchar(50)", 0, 0, true)]
        public virtual String ForumName
        {
            get { return _ForumName; }
            set { if (OnPropertyChanging(__.ForumName, value)) { _ForumName = value; OnPropertyChanged(__.ForumName); } }
        }

        private Int32 _TitleID;
        /// <summary>帖子编号</summary>
        [DisplayName("帖子编号")]
        [Description("帖子编号")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(20, "TitleID", "帖子编号", null, "int", 10, 0, false)]
        public virtual Int32 TitleID
        {
            get { return _TitleID; }
            set { if (OnPropertyChanging(__.TitleID, value)) { _TitleID = value; OnPropertyChanged(__.TitleID); } }
        }

        private String _Title;
        /// <summary>帖子</summary>
        [DisplayName("帖子")]
        [Description("帖子")]
        [DataObjectField(false, false, true, 80)]
        [BindColumn(21, "Title", "帖子", null, "nvarchar(80)", 0, 0, true, Master=true)]
        public virtual String Title
        {
            get { return _Title; }
            set { if (OnPropertyChanging(__.Title, value)) { _Title = value; OnPropertyChanged(__.Title); } }
        }

        private String _VerifyCode;
        /// <summary>验证码</summary>
        [DisplayName("验证码")]
        [Description("验证码")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(22, "VerifyCode", "验证码", null, "nvarchar(10)", 0, 0, true)]
        public virtual String VerifyCode
        {
            get { return _VerifyCode; }
            set { if (OnPropertyChanging(__.VerifyCode, value)) { _VerifyCode = value; OnPropertyChanged(__.VerifyCode); } }
        }

        private Int32 _Newpms;
        /// <summary>私信数量</summary>
        [DisplayName("私信数量")]
        [Description("私信数量")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(23, "Newpms", "私信数量", null, "int", 10, 0, false)]
        public virtual Int32 Newpms
        {
            get { return _Newpms; }
            set { if (OnPropertyChanging(__.Newpms, value)) { _Newpms = value; OnPropertyChanged(__.Newpms); } }
        }

        private Int32 _Newnotices;
        /// <summary>通知数量</summary>
        [DisplayName("通知数量")]
        [Description("通知数量")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(24, "Newnotices", "通知数量", null, "int", 10, 0, false)]
        public virtual Int32 Newnotices
        {
            get { return _Newnotices; }
            set { if (OnPropertyChanging(__.Newnotices, value)) { _Newnotices = value; OnPropertyChanged(__.Newnotices); } }
        }

        private String _UserAgent;
        /// <summary>用户终端</summary>
        [DisplayName("用户终端")]
        [Description("用户终端")]
        [DataObjectField(false, false, true, 250)]
        [BindColumn(25, "UserAgent", "用户终端", null, "nvarchar(250)", 0, 0, true)]
        public virtual String UserAgent
        {
            get { return _UserAgent; }
            set { if (OnPropertyChanging(__.UserAgent, value)) { _UserAgent = value; OnPropertyChanged(__.UserAgent); } }
        }

        private Boolean _IsBot;
        /// <summary>是否爬虫</summary>
        [DisplayName("是否爬虫")]
        [Description("是否爬虫")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(26, "IsBot", "是否爬虫", null, "bit", 0, 0, false)]
        public virtual Boolean IsBot
        {
            get { return _IsBot; }
            set { if (OnPropertyChanging(__.IsBot, value)) { _IsBot = value; OnPropertyChanged(__.IsBot); } }
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
                    case __.SessionID : return _SessionID;
                    case __.UserID : return _UserID;
                    case __.IP : return _IP;
                    case __.UserName : return _UserName;
                    case __.NickName : return _NickName;
                    case __.Password : return _Password;
                    case __.GroupID : return _GroupID;
                    case __.Olimg : return _Olimg;
                    case __.AdminID : return _AdminID;
                    case __.Invisible : return _Invisible;
                    case __.Action : return _Action;
                    case __.LastActivity : return _LastActivity;
                    case __.LastPostTime : return _LastPostTime;
                    case __.LastPostpmTime : return _LastPostpmTime;
                    case __.LastSearchTime : return _LastSearchTime;
                    case __.LastUpdateTime : return _LastUpdateTime;
                    case __.ForumID : return _ForumID;
                    case __.ForumName : return _ForumName;
                    case __.TitleID : return _TitleID;
                    case __.Title : return _Title;
                    case __.VerifyCode : return _VerifyCode;
                    case __.Newpms : return _Newpms;
                    case __.Newnotices : return _Newnotices;
                    case __.UserAgent : return _UserAgent;
                    case __.IsBot : return _IsBot;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.ID : _ID = Convert.ToInt32(value); break;
                    case __.SessionID : _SessionID = Convert.ToString(value); break;
                    case __.UserID : _UserID = Convert.ToInt32(value); break;
                    case __.IP : _IP = Convert.ToString(value); break;
                    case __.UserName : _UserName = Convert.ToString(value); break;
                    case __.NickName : _NickName = Convert.ToString(value); break;
                    case __.Password : _Password = Convert.ToString(value); break;
                    case __.GroupID : _GroupID = Convert.ToInt32(value); break;
                    case __.Olimg : _Olimg = Convert.ToString(value); break;
                    case __.AdminID : _AdminID = Convert.ToInt32(value); break;
                    case __.Invisible : _Invisible = Convert.ToBoolean(value); break;
                    case __.Action : _Action = Convert.ToInt32(value); break;
                    case __.LastActivity : _LastActivity = Convert.ToInt32(value); break;
                    case __.LastPostTime : _LastPostTime = Convert.ToDateTime(value); break;
                    case __.LastPostpmTime : _LastPostpmTime = Convert.ToDateTime(value); break;
                    case __.LastSearchTime : _LastSearchTime = Convert.ToDateTime(value); break;
                    case __.LastUpdateTime : _LastUpdateTime = Convert.ToDateTime(value); break;
                    case __.ForumID : _ForumID = Convert.ToInt32(value); break;
                    case __.ForumName : _ForumName = Convert.ToString(value); break;
                    case __.TitleID : _TitleID = Convert.ToInt32(value); break;
                    case __.Title : _Title = Convert.ToString(value); break;
                    case __.VerifyCode : _VerifyCode = Convert.ToString(value); break;
                    case __.Newpms : _Newpms = Convert.ToInt32(value); break;
                    case __.Newnotices : _Newnotices = Convert.ToInt32(value); break;
                    case __.UserAgent : _UserAgent = Convert.ToString(value); break;
                    case __.IsBot : _IsBot = Convert.ToBoolean(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得在线字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号</summary>
            public static readonly Field ID = FindByName(__.ID);

            ///<summary>会话标识</summary>
            public static readonly Field SessionID = FindByName(__.SessionID);

            ///<summary>用户编号</summary>
            public static readonly Field UserID = FindByName(__.UserID);

            ///<summary>IP地址</summary>
            public static readonly Field IP = FindByName(__.IP);

            ///<summary>登录账户</summary>
            public static readonly Field UserName = FindByName(__.UserName);

            ///<summary>昵称。如果为空,则显示UserName</summary>
            public static readonly Field NickName = FindByName(__.NickName);

            ///<summary>登录密码</summary>
            public static readonly Field Password = FindByName(__.Password);

            ///<summary>聊天组</summary>
            public static readonly Field GroupID = FindByName(__.GroupID);

            ///<summary>在线图标</summary>
            public static readonly Field Olimg = FindByName(__.Olimg);

            ///<summary>操作人</summary>
            public static readonly Field AdminID = FindByName(__.AdminID);

            ///<summary>是否隐身</summary>
            public static readonly Field Invisible = FindByName(__.Invisible);

            ///<summary>动作</summary>
            public static readonly Field Action = FindByName(__.Action);

            ///<summary>最后活动时间</summary>
            public static readonly Field LastActivity = FindByName(__.LastActivity);

            ///<summary>最后发帖时间</summary>
            public static readonly Field LastPostTime = FindByName(__.LastPostTime);

            ///<summary>最后私信时间</summary>
            public static readonly Field LastPostpmTime = FindByName(__.LastPostpmTime);

            ///<summary>最后搜索时间</summary>
            public static readonly Field LastSearchTime = FindByName(__.LastSearchTime);

            ///<summary>最后更新时间</summary>
            public static readonly Field LastUpdateTime = FindByName(__.LastUpdateTime);

            ///<summary>论坛编号</summary>
            public static readonly Field ForumID = FindByName(__.ForumID);

            ///<summary>论坛名称</summary>
            public static readonly Field ForumName = FindByName(__.ForumName);

            ///<summary>帖子编号</summary>
            public static readonly Field TitleID = FindByName(__.TitleID);

            ///<summary>帖子</summary>
            public static readonly Field Title = FindByName(__.Title);

            ///<summary>验证码</summary>
            public static readonly Field VerifyCode = FindByName(__.VerifyCode);

            ///<summary>私信数量</summary>
            public static readonly Field Newpms = FindByName(__.Newpms);

            ///<summary>通知数量</summary>
            public static readonly Field Newnotices = FindByName(__.Newnotices);

            ///<summary>用户终端</summary>
            public static readonly Field UserAgent = FindByName(__.UserAgent);

            ///<summary>是否爬虫</summary>
            public static readonly Field IsBot = FindByName(__.IsBot);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得在线字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号</summary>
            public const String ID = "ID";

            ///<summary>会话标识</summary>
            public const String SessionID = "SessionID";

            ///<summary>用户编号</summary>
            public const String UserID = "UserID";

            ///<summary>IP地址</summary>
            public const String IP = "IP";

            ///<summary>登录账户</summary>
            public const String UserName = "UserName";

            ///<summary>昵称。如果为空,则显示UserName</summary>
            public const String NickName = "NickName";

            ///<summary>登录密码</summary>
            public const String Password = "Password";

            ///<summary>聊天组</summary>
            public const String GroupID = "GroupID";

            ///<summary>在线图标</summary>
            public const String Olimg = "Olimg";

            ///<summary>操作人</summary>
            public const String AdminID = "AdminID";

            ///<summary>是否隐身</summary>
            public const String Invisible = "Invisible";

            ///<summary>动作</summary>
            public const String Action = "Action";

            ///<summary>最后活动时间</summary>
            public const String LastActivity = "LastActivity";

            ///<summary>最后发帖时间</summary>
            public const String LastPostTime = "LastPostTime";

            ///<summary>最后私信时间</summary>
            public const String LastPostpmTime = "LastPostpmTime";

            ///<summary>最后搜索时间</summary>
            public const String LastSearchTime = "LastSearchTime";

            ///<summary>最后更新时间</summary>
            public const String LastUpdateTime = "LastUpdateTime";

            ///<summary>论坛编号</summary>
            public const String ForumID = "ForumID";

            ///<summary>论坛名称</summary>
            public const String ForumName = "ForumName";

            ///<summary>帖子编号</summary>
            public const String TitleID = "TitleID";

            ///<summary>帖子</summary>
            public const String Title = "Title";

            ///<summary>验证码</summary>
            public const String VerifyCode = "VerifyCode";

            ///<summary>私信数量</summary>
            public const String Newpms = "Newpms";

            ///<summary>通知数量</summary>
            public const String Newnotices = "Newnotices";

            ///<summary>用户终端</summary>
            public const String UserAgent = "UserAgent";

            ///<summary>是否爬虫</summary>
            public const String IsBot = "IsBot";

        }
        #endregion
    }

    /// <summary>在线接口</summary>
    public partial interface IOnline
    {
        #region 属性
        /// <summary>编号</summary>
        Int32 ID { get; set; }

        /// <summary>会话标识</summary>
        String SessionID { get; set; }

        /// <summary>用户编号</summary>
        Int32 UserID { get; set; }

        /// <summary>IP地址</summary>
        String IP { get; set; }

        /// <summary>登录账户</summary>
        String UserName { get; set; }

        /// <summary>昵称。如果为空,则显示UserName</summary>
        String NickName { get; set; }

        /// <summary>登录密码</summary>
        String Password { get; set; }

        /// <summary>聊天组</summary>
        Int32 GroupID { get; set; }

        /// <summary>在线图标</summary>
        String Olimg { get; set; }

        /// <summary>操作人</summary>
        Int32 AdminID { get; set; }

        /// <summary>是否隐身</summary>
        Boolean Invisible { get; set; }

        /// <summary>动作</summary>
        Int32 Action { get; set; }

        /// <summary>最后活动时间</summary>
        Int32 LastActivity { get; set; }

        /// <summary>最后发帖时间</summary>
        DateTime LastPostTime { get; set; }

        /// <summary>最后私信时间</summary>
        DateTime LastPostpmTime { get; set; }

        /// <summary>最后搜索时间</summary>
        DateTime LastSearchTime { get; set; }

        /// <summary>最后更新时间</summary>
        DateTime LastUpdateTime { get; set; }

        /// <summary>论坛编号</summary>
        Int32 ForumID { get; set; }

        /// <summary>论坛名称</summary>
        String ForumName { get; set; }

        /// <summary>帖子编号</summary>
        Int32 TitleID { get; set; }

        /// <summary>帖子</summary>
        String Title { get; set; }

        /// <summary>验证码</summary>
        String VerifyCode { get; set; }

        /// <summary>私信数量</summary>
        Int32 Newpms { get; set; }

        /// <summary>通知数量</summary>
        Int32 Newnotices { get; set; }

        /// <summary>用户终端</summary>
        String UserAgent { get; set; }

        /// <summary>是否爬虫</summary>
        Boolean IsBot { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}