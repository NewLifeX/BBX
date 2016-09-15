﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace BBX.Entity
{
    /// <summary>用户</summary>
    [Serializable]
    [DataObject]
    [Description("用户")]
    [BindIndex("IX_User_email", false, "email")]
    [BindIndex("IX_User_groupid_adminid", false, "groupid,adminid")]
    [BindIndex("IX_User_regip", false, "regip")]
    [BindIndex("IX_User_username", false, "username")]
    [BindTable("User", Description = "用户", ConnName = "BBX", DbType = DatabaseType.SqlServer)]
    public partial class User : IUser
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

        private String _Name;
        /// <summary>登录账户</summary>
        [DisplayName("登录账户")]
        [Description("登录账户")]
        [DataObjectField(false, false, false, 50)]
        [BindColumn(2, "Name", "登录账户", null, "nvarchar(50)", 0, 0, true, Master=true)]
        public virtual String Name
        {
            get { return _Name; }
            set { if (OnPropertyChanging(__.Name, value)) { _Name = value; OnPropertyChanged(__.Name); } }
        }

        private String _NickName;
        /// <summary>昵称</summary>
        [DisplayName("昵称")]
        [Description("昵称")]
        [DataObjectField(false, false, false, 50)]
        [BindColumn(3, "NickName", "昵称", null, "nvarchar(50)", 0, 0, true)]
        public virtual String NickName
        {
            get { return _NickName; }
            set { if (OnPropertyChanging(__.NickName, value)) { _NickName = value; OnPropertyChanged(__.NickName); } }
        }

        private String _Password;
        /// <summary>登录密码</summary>
        [DisplayName("登录密码")]
        [Description("登录密码")]
        [DataObjectField(false, false, false, 50)]
        [BindColumn(4, "Password", "登录密码", null, "nvarchar(50)", 0, 0, true)]
        public virtual String Password
        {
            get { return _Password; }
            set { if (OnPropertyChanging(__.Password, value)) { _Password = value; OnPropertyChanged(__.Password); } }
        }

        private String _Secques;
        /// <summary>用户安全提问码</summary>
        [DisplayName("用户安全提问码")]
        [Description("用户安全提问码")]
        [DataObjectField(false, false, false, 8)]
        [BindColumn(5, "Secques", "用户安全提问码", null, "nvarchar(8)", 0, 0, true)]
        public virtual String Secques
        {
            get { return _Secques; }
            set { if (OnPropertyChanging(__.Secques, value)) { _Secques = value; OnPropertyChanged(__.Secques); } }
        }

        private Int32 _SpaceID;
        /// <summary>空间</summary>
        [DisplayName("空间")]
        [Description("空间")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(6, "SpaceID", "空间", null, "int", 10, 0, false)]
        public virtual Int32 SpaceID
        {
            get { return _SpaceID; }
            set { if (OnPropertyChanging(__.SpaceID, value)) { _SpaceID = value; OnPropertyChanged(__.SpaceID); } }
        }

        private Int32 _Gender;
        /// <summary>性别</summary>
        [DisplayName("性别")]
        [Description("性别")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(7, "Gender", "性别", null, "int", 10, 0, false)]
        public virtual Int32 Gender
        {
            get { return _Gender; }
            set { if (OnPropertyChanging(__.Gender, value)) { _Gender = value; OnPropertyChanged(__.Gender); } }
        }

        private Int32 _AdminID;
        /// <summary>管理权限</summary>
        [DisplayName("管理权限")]
        [Description("管理权限")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(8, "AdminID", "管理权限", null, "int", 10, 0, false)]
        public virtual Int32 AdminID
        {
            get { return _AdminID; }
            set { if (OnPropertyChanging(__.AdminID, value)) { _AdminID = value; OnPropertyChanged(__.AdminID); } }
        }

        private Int32 _GroupID;
        /// <summary>用户组</summary>
        [DisplayName("用户组")]
        [Description("用户组")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(9, "GroupID", "用户组", null, "int", 10, 0, false)]
        public virtual Int32 GroupID
        {
            get { return _GroupID; }
            set { if (OnPropertyChanging(__.GroupID, value)) { _GroupID = value; OnPropertyChanged(__.GroupID); } }
        }

        private Int32 _GroupExpiry;
        /// <summary>组过期时间</summary>
        [DisplayName("组过期时间")]
        [Description("组过期时间")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(10, "GroupExpiry", "组过期时间", null, "int", 10, 0, false)]
        public virtual Int32 GroupExpiry
        {
            get { return _GroupExpiry; }
            set { if (OnPropertyChanging(__.GroupExpiry, value)) { _GroupExpiry = value; OnPropertyChanged(__.GroupExpiry); } }
        }

        private String _ExtGroupIds;
        /// <summary>扩展用户组</summary>
        [DisplayName("扩展用户组")]
        [Description("扩展用户组")]
        [DataObjectField(false, false, false, 50)]
        [BindColumn(11, "ExtGroupIds", "扩展用户组", null, "nvarchar(50)", 0, 0, true)]
        public virtual String ExtGroupIds
        {
            get { return _ExtGroupIds; }
            set { if (OnPropertyChanging(__.ExtGroupIds, value)) { _ExtGroupIds = value; OnPropertyChanged(__.ExtGroupIds); } }
        }

        private String _RegIP;
        /// <summary>注册IP</summary>
        [DisplayName("注册IP")]
        [Description("注册IP")]
        [DataObjectField(false, false, false, 50)]
        [BindColumn(12, "RegIP", "注册IP", null, "nvarchar(50)", 0, 0, true)]
        public virtual String RegIP
        {
            get { return _RegIP; }
            set { if (OnPropertyChanging(__.RegIP, value)) { _RegIP = value; OnPropertyChanged(__.RegIP); } }
        }

        private DateTime _JoinDate;
        /// <summary>注册时间</summary>
        [DisplayName("注册时间")]
        [Description("注册时间")]
        [DataObjectField(false, false, true, 3)]
        [BindColumn(13, "JoinDate", "注册时间", null, "datetime", 3, 0, false)]
        public virtual DateTime JoinDate
        {
            get { return _JoinDate; }
            set { if (OnPropertyChanging(__.JoinDate, value)) { _JoinDate = value; OnPropertyChanged(__.JoinDate); } }
        }

        private String _LastIP;
        /// <summary>上次登录IP</summary>
        [DisplayName("上次登录IP")]
        [Description("上次登录IP")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(14, "LastIP", "上次登录IP", null, "nvarchar(50)", 0, 0, true)]
        public virtual String LastIP
        {
            get { return _LastIP; }
            set { if (OnPropertyChanging(__.LastIP, value)) { _LastIP = value; OnPropertyChanged(__.LastIP); } }
        }

        private DateTime _LastVisit;
        /// <summary>上次访问时间</summary>
        [DisplayName("上次访问时间")]
        [Description("上次访问时间")]
        [DataObjectField(false, false, true, 3)]
        [BindColumn(15, "LastVisit", "上次访问时间", null, "datetime", 3, 0, false)]
        public virtual DateTime LastVisit
        {
            get { return _LastVisit; }
            set { if (OnPropertyChanging(__.LastVisit, value)) { _LastVisit = value; OnPropertyChanged(__.LastVisit); } }
        }

        private DateTime _LastActivity;
        /// <summary>最后活动时间</summary>
        [DisplayName("最后活动时间")]
        [Description("最后活动时间")]
        [DataObjectField(false, false, true, 3)]
        [BindColumn(16, "LastActivity", "最后活动时间", null, "datetime", 3, 0, false)]
        public virtual DateTime LastActivity
        {
            get { return _LastActivity; }
            set { if (OnPropertyChanging(__.LastActivity, value)) { _LastActivity = value; OnPropertyChanged(__.LastActivity); } }
        }

        private DateTime _LastPost;
        /// <summary>最后发帖时间</summary>
        [DisplayName("最后发帖时间")]
        [Description("最后发帖时间")]
        [DataObjectField(false, false, true, 3)]
        [BindColumn(17, "LastPost", "最后发帖时间", null, "datetime", 3, 0, false)]
        public virtual DateTime LastPost
        {
            get { return _LastPost; }
            set { if (OnPropertyChanging(__.LastPost, value)) { _LastPost = value; OnPropertyChanged(__.LastPost); } }
        }

        private Int32 _LastPostID;
        /// <summary>最后帖子编号</summary>
        [DisplayName("最后帖子编号")]
        [Description("最后帖子编号")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(18, "LastPostID", "最后帖子编号", null, "int", 10, 0, false)]
        public virtual Int32 LastPostID
        {
            get { return _LastPostID; }
            set { if (OnPropertyChanging(__.LastPostID, value)) { _LastPostID = value; OnPropertyChanged(__.LastPostID); } }
        }

        private String _LastPostTitle;
        /// <summary>最后帖子标题</summary>
        [DisplayName("最后帖子标题")]
        [Description("最后帖子标题")]
        [DataObjectField(false, false, false, 60)]
        [BindColumn(19, "LastPostTitle", "最后帖子标题", null, "nvarchar(60)", 0, 0, true)]
        public virtual String LastPostTitle
        {
            get { return _LastPostTitle; }
            set { if (OnPropertyChanging(__.LastPostTitle, value)) { _LastPostTitle = value; OnPropertyChanged(__.LastPostTitle); } }
        }

        private Int32 _Posts;
        /// <summary>发帖数</summary>
        [DisplayName("发帖数")]
        [Description("发帖数")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(20, "Posts", "发帖数", null, "int", 10, 0, false)]
        public virtual Int32 Posts
        {
            get { return _Posts; }
            set { if (OnPropertyChanging(__.Posts, value)) { _Posts = value; OnPropertyChanged(__.Posts); } }
        }

        private Int32 _DigestPosts;
        /// <summary>精华帖数</summary>
        [DisplayName("精华帖数")]
        [Description("精华帖数")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(21, "DigestPosts", "精华帖数", null, "int", 10, 0, false)]
        public virtual Int32 DigestPosts
        {
            get { return _DigestPosts; }
            set { if (OnPropertyChanging(__.DigestPosts, value)) { _DigestPosts = value; OnPropertyChanged(__.DigestPosts); } }
        }

        private Int32 _OLTime;
        /// <summary>在线时间</summary>
        [DisplayName("在线时间")]
        [Description("在线时间")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(22, "OLTime", "在线时间", null, "int", 10, 0, false)]
        public virtual Int32 OLTime
        {
            get { return _OLTime; }
            set { if (OnPropertyChanging(__.OLTime, value)) { _OLTime = value; OnPropertyChanged(__.OLTime); } }
        }

        private Int32 _PageViews;
        /// <summary>页面访问量</summary>
        [DisplayName("页面访问量")]
        [Description("页面访问量")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(23, "PageViews", "页面访问量", null, "int", 10, 0, false)]
        public virtual Int32 PageViews
        {
            get { return _PageViews; }
            set { if (OnPropertyChanging(__.PageViews, value)) { _PageViews = value; OnPropertyChanged(__.PageViews); } }
        }

        private Int32 _Credits;
        /// <summary>积分数</summary>
        [DisplayName("积分数")]
        [Description("积分数")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(24, "Credits", "积分数", null, "int", 10, 0, false)]
        public virtual Int32 Credits
        {
            get { return _Credits; }
            set { if (OnPropertyChanging(__.Credits, value)) { _Credits = value; OnPropertyChanged(__.Credits); } }
        }

        private Single _ExtCredits1;
        /// <summary>扩展积分</summary>
        [DisplayName("扩展积分")]
        [Description("扩展积分")]
        [DataObjectField(false, false, true, 7)]
        [BindColumn(25, "ExtCredits1", "扩展积分", null, "real", 0, 0, false)]
        public virtual Single ExtCredits1
        {
            get { return _ExtCredits1; }
            set { if (OnPropertyChanging(__.ExtCredits1, value)) { _ExtCredits1 = value; OnPropertyChanged(__.ExtCredits1); } }
        }

        private Single _ExtCredits2;
        /// <summary>扩展积分</summary>
        [DisplayName("扩展积分")]
        [Description("扩展积分")]
        [DataObjectField(false, false, true, 7)]
        [BindColumn(26, "ExtCredits2", "扩展积分", null, "real", 0, 0, false)]
        public virtual Single ExtCredits2
        {
            get { return _ExtCredits2; }
            set { if (OnPropertyChanging(__.ExtCredits2, value)) { _ExtCredits2 = value; OnPropertyChanged(__.ExtCredits2); } }
        }

        private Single _ExtCredits3;
        /// <summary>扩展积分</summary>
        [DisplayName("扩展积分")]
        [Description("扩展积分")]
        [DataObjectField(false, false, true, 7)]
        [BindColumn(27, "ExtCredits3", "扩展积分", null, "real", 0, 0, false)]
        public virtual Single ExtCredits3
        {
            get { return _ExtCredits3; }
            set { if (OnPropertyChanging(__.ExtCredits3, value)) { _ExtCredits3 = value; OnPropertyChanged(__.ExtCredits3); } }
        }

        private Single _ExtCredits4;
        /// <summary>扩展积分</summary>
        [DisplayName("扩展积分")]
        [Description("扩展积分")]
        [DataObjectField(false, false, true, 7)]
        [BindColumn(28, "ExtCredits4", "扩展积分", null, "real", 0, 0, false)]
        public virtual Single ExtCredits4
        {
            get { return _ExtCredits4; }
            set { if (OnPropertyChanging(__.ExtCredits4, value)) { _ExtCredits4 = value; OnPropertyChanged(__.ExtCredits4); } }
        }

        private Single _ExtCredits5;
        /// <summary>扩展积分</summary>
        [DisplayName("扩展积分")]
        [Description("扩展积分")]
        [DataObjectField(false, false, true, 7)]
        [BindColumn(29, "ExtCredits5", "扩展积分", null, "real", 0, 0, false)]
        public virtual Single ExtCredits5
        {
            get { return _ExtCredits5; }
            set { if (OnPropertyChanging(__.ExtCredits5, value)) { _ExtCredits5 = value; OnPropertyChanged(__.ExtCredits5); } }
        }

        private Single _ExtCredits6;
        /// <summary>扩展积分</summary>
        [DisplayName("扩展积分")]
        [Description("扩展积分")]
        [DataObjectField(false, false, true, 7)]
        [BindColumn(30, "ExtCredits6", "扩展积分", null, "real", 0, 0, false)]
        public virtual Single ExtCredits6
        {
            get { return _ExtCredits6; }
            set { if (OnPropertyChanging(__.ExtCredits6, value)) { _ExtCredits6 = value; OnPropertyChanged(__.ExtCredits6); } }
        }

        private Single _ExtCredits7;
        /// <summary>扩展积分</summary>
        [DisplayName("扩展积分")]
        [Description("扩展积分")]
        [DataObjectField(false, false, true, 7)]
        [BindColumn(31, "ExtCredits7", "扩展积分", null, "real", 0, 0, false)]
        public virtual Single ExtCredits7
        {
            get { return _ExtCredits7; }
            set { if (OnPropertyChanging(__.ExtCredits7, value)) { _ExtCredits7 = value; OnPropertyChanged(__.ExtCredits7); } }
        }

        private Single _ExtCredits8;
        /// <summary>扩展积分</summary>
        [DisplayName("扩展积分")]
        [Description("扩展积分")]
        [DataObjectField(false, false, true, 7)]
        [BindColumn(32, "ExtCredits8", "扩展积分", null, "real", 0, 0, false)]
        public virtual Single ExtCredits8
        {
            get { return _ExtCredits8; }
            set { if (OnPropertyChanging(__.ExtCredits8, value)) { _ExtCredits8 = value; OnPropertyChanged(__.ExtCredits8); } }
        }

        private Int32 _AvatarShowId;
        /// <summary>头像</summary>
        [DisplayName("头像")]
        [Description("头像")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(33, "AvatarShowId", "头像", null, "int", 10, 0, false)]
        public virtual Int32 AvatarShowId
        {
            get { return _AvatarShowId; }
            set { if (OnPropertyChanging(__.AvatarShowId, value)) { _AvatarShowId = value; OnPropertyChanged(__.AvatarShowId); } }
        }

        private String _Email;
        /// <summary>邮件</summary>
        [DisplayName("邮件")]
        [Description("邮件")]
        [DataObjectField(false, false, false, 50)]
        [BindColumn(34, "Email", "邮件", null, "nvarchar(50)", 0, 0, true)]
        public virtual String Email
        {
            get { return _Email; }
            set { if (OnPropertyChanging(__.Email, value)) { _Email = value; OnPropertyChanged(__.Email); } }
        }

        private String _Bday;
        /// <summary>生日</summary>
        [DisplayName("生日")]
        [Description("生日")]
        [DataObjectField(false, false, false, 50)]
        [BindColumn(35, "Bday", "生日", null, "nvarchar(50)", 0, 0, true)]
        public virtual String Bday
        {
            get { return _Bday; }
            set { if (OnPropertyChanging(__.Bday, value)) { _Bday = value; OnPropertyChanged(__.Bday); } }
        }

        private Int32 _Sigstatus;
        /// <summary>是否签名</summary>
        [DisplayName("是否签名")]
        [Description("是否签名")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(36, "Sigstatus", "是否签名", null, "int", 10, 0, false)]
        public virtual Int32 Sigstatus
        {
            get { return _Sigstatus; }
            set { if (OnPropertyChanging(__.Sigstatus, value)) { _Sigstatus = value; OnPropertyChanged(__.Sigstatus); } }
        }

        private Int32 _Tpp;
        /// <summary></summary>
        [DisplayName("Tpp")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(37, "Tpp", "", null, "int", 10, 0, false)]
        public virtual Int32 Tpp
        {
            get { return _Tpp; }
            set { if (OnPropertyChanging(__.Tpp, value)) { _Tpp = value; OnPropertyChanged(__.Tpp); } }
        }

        private Int32 _Ppp;
        /// <summary></summary>
        [DisplayName("Ppp")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(38, "Ppp", "", null, "int", 10, 0, false)]
        public virtual Int32 Ppp
        {
            get { return _Ppp; }
            set { if (OnPropertyChanging(__.Ppp, value)) { _Ppp = value; OnPropertyChanged(__.Ppp); } }
        }

        private Int32 _TemplateID;
        /// <summary>模板Id</summary>
        [DisplayName("模板Id")]
        [Description("模板Id")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(39, "TemplateID", "模板Id", null, "int", 10, 0, false)]
        public virtual Int32 TemplateID
        {
            get { return _TemplateID; }
            set { if (OnPropertyChanging(__.TemplateID, value)) { _TemplateID = value; OnPropertyChanged(__.TemplateID); } }
        }

        private Int32 _Pmsound;
        /// <summary>短消息铃声</summary>
        [DisplayName("短消息铃声")]
        [Description("短消息铃声")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(40, "Pmsound", "短消息铃声", null, "int", 10, 0, false)]
        public virtual Int32 Pmsound
        {
            get { return _Pmsound; }
            set { if (OnPropertyChanging(__.Pmsound, value)) { _Pmsound = value; OnPropertyChanged(__.Pmsound); } }
        }

        private Boolean _ShowEmail;
        /// <summary>是否显示邮箱</summary>
        [DisplayName("是否显示邮箱")]
        [Description("是否显示邮箱")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(41, "ShowEmail", "是否显示邮箱", null, "bit", 0, 0, false)]
        public virtual Boolean ShowEmail
        {
            get { return _ShowEmail; }
            set { if (OnPropertyChanging(__.ShowEmail, value)) { _ShowEmail = value; OnPropertyChanged(__.ShowEmail); } }
        }

        private Boolean _Invisible;
        /// <summary>是否隐身</summary>
        [DisplayName("是否隐身")]
        [Description("是否隐身")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(42, "Invisible", "是否隐身", null, "bit", 0, 0, false)]
        public virtual Boolean Invisible
        {
            get { return _Invisible; }
            set { if (OnPropertyChanging(__.Invisible, value)) { _Invisible = value; OnPropertyChanged(__.Invisible); } }
        }

        private Boolean _Newpm;
        /// <summary>是否有新消息</summary>
        [DisplayName("是否有新消息")]
        [Description("是否有新消息")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(43, "Newpm", "是否有新消息", null, "bit", 0, 0, false)]
        public virtual Boolean Newpm
        {
            get { return _Newpm; }
            set { if (OnPropertyChanging(__.Newpm, value)) { _Newpm = value; OnPropertyChanged(__.Newpm); } }
        }

        private Int32 _NewpmCount;
        /// <summary>新短消息数量</summary>
        [DisplayName("新短消息数量")]
        [Description("新短消息数量")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(44, "NewpmCount", "新短消息数量", null, "int", 10, 0, false)]
        public virtual Int32 NewpmCount
        {
            get { return _NewpmCount; }
            set { if (OnPropertyChanging(__.NewpmCount, value)) { _NewpmCount = value; OnPropertyChanged(__.NewpmCount); } }
        }

        private Int32 _AccessMasks;
        /// <summary></summary>
        [DisplayName("AccessMasks")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(45, "AccessMasks", "", null, "int", 10, 0, false)]
        public virtual Int32 AccessMasks
        {
            get { return _AccessMasks; }
            set { if (OnPropertyChanging(__.AccessMasks, value)) { _AccessMasks = value; OnPropertyChanged(__.AccessMasks); } }
        }

        private Int32 _OnlineState;
        /// <summary>在线状态,1为在线,0为不在线</summary>
        [DisplayName("在线状态,1为在线,0为不在线")]
        [Description("在线状态,1为在线,0为不在线")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(46, "OnlineState", "在线状态,1为在线,0为不在线", null, "int", 10, 0, false)]
        public virtual Int32 OnlineState
        {
            get { return _OnlineState; }
            set { if (OnPropertyChanging(__.OnlineState, value)) { _OnlineState = value; OnPropertyChanged(__.OnlineState); } }
        }

        private Int32 _NewsLetter;
        /// <summary>是否接收论坛通知</summary>
        [DisplayName("是否接收论坛通知")]
        [Description("是否接收论坛通知")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(47, "NewsLetter", "是否接收论坛通知", null, "int", 10, 0, false)]
        public virtual Int32 NewsLetter
        {
            get { return _NewsLetter; }
            set { if (OnPropertyChanging(__.NewsLetter, value)) { _NewsLetter = value; OnPropertyChanged(__.NewsLetter); } }
        }

        private String _Salt;
        /// <summary>MD5加密盐</summary>
        [DisplayName("MD5加密盐")]
        [Description("MD5加密盐")]
        [DataObjectField(false, false, false, 6)]
        [BindColumn(48, "Salt", "MD5加密盐", null, "nvarchar(6)", 0, 0, true)]
        public virtual String Salt
        {
            get { return _Salt; }
            set { if (OnPropertyChanging(__.Salt, value)) { _Salt = value; OnPropertyChanged(__.Salt); } }
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
                    case __.Name : return _Name;
                    case __.NickName : return _NickName;
                    case __.Password : return _Password;
                    case __.Secques : return _Secques;
                    case __.SpaceID : return _SpaceID;
                    case __.Gender : return _Gender;
                    case __.AdminID : return _AdminID;
                    case __.GroupID : return _GroupID;
                    case __.GroupExpiry : return _GroupExpiry;
                    case __.ExtGroupIds : return _ExtGroupIds;
                    case __.RegIP : return _RegIP;
                    case __.JoinDate : return _JoinDate;
                    case __.LastIP : return _LastIP;
                    case __.LastVisit : return _LastVisit;
                    case __.LastActivity : return _LastActivity;
                    case __.LastPost : return _LastPost;
                    case __.LastPostID : return _LastPostID;
                    case __.LastPostTitle : return _LastPostTitle;
                    case __.Posts : return _Posts;
                    case __.DigestPosts : return _DigestPosts;
                    case __.OLTime : return _OLTime;
                    case __.PageViews : return _PageViews;
                    case __.Credits : return _Credits;
                    case __.ExtCredits1 : return _ExtCredits1;
                    case __.ExtCredits2 : return _ExtCredits2;
                    case __.ExtCredits3 : return _ExtCredits3;
                    case __.ExtCredits4 : return _ExtCredits4;
                    case __.ExtCredits5 : return _ExtCredits5;
                    case __.ExtCredits6 : return _ExtCredits6;
                    case __.ExtCredits7 : return _ExtCredits7;
                    case __.ExtCredits8 : return _ExtCredits8;
                    case __.AvatarShowId : return _AvatarShowId;
                    case __.Email : return _Email;
                    case __.Bday : return _Bday;
                    case __.Sigstatus : return _Sigstatus;
                    case __.Tpp : return _Tpp;
                    case __.Ppp : return _Ppp;
                    case __.TemplateID : return _TemplateID;
                    case __.Pmsound : return _Pmsound;
                    case __.ShowEmail : return _ShowEmail;
                    case __.Invisible : return _Invisible;
                    case __.Newpm : return _Newpm;
                    case __.NewpmCount : return _NewpmCount;
                    case __.AccessMasks : return _AccessMasks;
                    case __.OnlineState : return _OnlineState;
                    case __.NewsLetter : return _NewsLetter;
                    case __.Salt : return _Salt;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.ID : _ID = Convert.ToInt32(value); break;
                    case __.Name : _Name = Convert.ToString(value); break;
                    case __.NickName : _NickName = Convert.ToString(value); break;
                    case __.Password : _Password = Convert.ToString(value); break;
                    case __.Secques : _Secques = Convert.ToString(value); break;
                    case __.SpaceID : _SpaceID = Convert.ToInt32(value); break;
                    case __.Gender : _Gender = Convert.ToInt32(value); break;
                    case __.AdminID : _AdminID = Convert.ToInt32(value); break;
                    case __.GroupID : _GroupID = Convert.ToInt32(value); break;
                    case __.GroupExpiry : _GroupExpiry = Convert.ToInt32(value); break;
                    case __.ExtGroupIds : _ExtGroupIds = Convert.ToString(value); break;
                    case __.RegIP : _RegIP = Convert.ToString(value); break;
                    case __.JoinDate : _JoinDate = Convert.ToDateTime(value); break;
                    case __.LastIP : _LastIP = Convert.ToString(value); break;
                    case __.LastVisit : _LastVisit = Convert.ToDateTime(value); break;
                    case __.LastActivity : _LastActivity = Convert.ToDateTime(value); break;
                    case __.LastPost : _LastPost = Convert.ToDateTime(value); break;
                    case __.LastPostID : _LastPostID = Convert.ToInt32(value); break;
                    case __.LastPostTitle : _LastPostTitle = Convert.ToString(value); break;
                    case __.Posts : _Posts = Convert.ToInt32(value); break;
                    case __.DigestPosts : _DigestPosts = Convert.ToInt32(value); break;
                    case __.OLTime : _OLTime = Convert.ToInt32(value); break;
                    case __.PageViews : _PageViews = Convert.ToInt32(value); break;
                    case __.Credits : _Credits = Convert.ToInt32(value); break;
                    case __.ExtCredits1 : _ExtCredits1 = Convert.ToSingle(value); break;
                    case __.ExtCredits2 : _ExtCredits2 = Convert.ToSingle(value); break;
                    case __.ExtCredits3 : _ExtCredits3 = Convert.ToSingle(value); break;
                    case __.ExtCredits4 : _ExtCredits4 = Convert.ToSingle(value); break;
                    case __.ExtCredits5 : _ExtCredits5 = Convert.ToSingle(value); break;
                    case __.ExtCredits6 : _ExtCredits6 = Convert.ToSingle(value); break;
                    case __.ExtCredits7 : _ExtCredits7 = Convert.ToSingle(value); break;
                    case __.ExtCredits8 : _ExtCredits8 = Convert.ToSingle(value); break;
                    case __.AvatarShowId : _AvatarShowId = Convert.ToInt32(value); break;
                    case __.Email : _Email = Convert.ToString(value); break;
                    case __.Bday : _Bday = Convert.ToString(value); break;
                    case __.Sigstatus : _Sigstatus = Convert.ToInt32(value); break;
                    case __.Tpp : _Tpp = Convert.ToInt32(value); break;
                    case __.Ppp : _Ppp = Convert.ToInt32(value); break;
                    case __.TemplateID : _TemplateID = Convert.ToInt32(value); break;
                    case __.Pmsound : _Pmsound = Convert.ToInt32(value); break;
                    case __.ShowEmail : _ShowEmail = Convert.ToBoolean(value); break;
                    case __.Invisible : _Invisible = Convert.ToBoolean(value); break;
                    case __.Newpm : _Newpm = Convert.ToBoolean(value); break;
                    case __.NewpmCount : _NewpmCount = Convert.ToInt32(value); break;
                    case __.AccessMasks : _AccessMasks = Convert.ToInt32(value); break;
                    case __.OnlineState : _OnlineState = Convert.ToInt32(value); break;
                    case __.NewsLetter : _NewsLetter = Convert.ToInt32(value); break;
                    case __.Salt : _Salt = Convert.ToString(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得用户字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号</summary>
            public static readonly Field ID = FindByName(__.ID);

            ///<summary>登录账户</summary>
            public static readonly Field Name = FindByName(__.Name);

            ///<summary>昵称</summary>
            public static readonly Field NickName = FindByName(__.NickName);

            ///<summary>登录密码</summary>
            public static readonly Field Password = FindByName(__.Password);

            ///<summary>用户安全提问码</summary>
            public static readonly Field Secques = FindByName(__.Secques);

            ///<summary>空间</summary>
            public static readonly Field SpaceID = FindByName(__.SpaceID);

            ///<summary>性别</summary>
            public static readonly Field Gender = FindByName(__.Gender);

            ///<summary>管理权限</summary>
            public static readonly Field AdminID = FindByName(__.AdminID);

            ///<summary>用户组</summary>
            public static readonly Field GroupID = FindByName(__.GroupID);

            ///<summary>组过期时间</summary>
            public static readonly Field GroupExpiry = FindByName(__.GroupExpiry);

            ///<summary>扩展用户组</summary>
            public static readonly Field ExtGroupIds = FindByName(__.ExtGroupIds);

            ///<summary>注册IP</summary>
            public static readonly Field RegIP = FindByName(__.RegIP);

            ///<summary>注册时间</summary>
            public static readonly Field JoinDate = FindByName(__.JoinDate);

            ///<summary>上次登录IP</summary>
            public static readonly Field LastIP = FindByName(__.LastIP);

            ///<summary>上次访问时间</summary>
            public static readonly Field LastVisit = FindByName(__.LastVisit);

            ///<summary>最后活动时间</summary>
            public static readonly Field LastActivity = FindByName(__.LastActivity);

            ///<summary>最后发帖时间</summary>
            public static readonly Field LastPost = FindByName(__.LastPost);

            ///<summary>最后帖子编号</summary>
            public static readonly Field LastPostID = FindByName(__.LastPostID);

            ///<summary>最后帖子标题</summary>
            public static readonly Field LastPostTitle = FindByName(__.LastPostTitle);

            ///<summary>发帖数</summary>
            public static readonly Field Posts = FindByName(__.Posts);

            ///<summary>精华帖数</summary>
            public static readonly Field DigestPosts = FindByName(__.DigestPosts);

            ///<summary>在线时间</summary>
            public static readonly Field OLTime = FindByName(__.OLTime);

            ///<summary>页面访问量</summary>
            public static readonly Field PageViews = FindByName(__.PageViews);

            ///<summary>积分数</summary>
            public static readonly Field Credits = FindByName(__.Credits);

            ///<summary>扩展积分</summary>
            public static readonly Field ExtCredits1 = FindByName(__.ExtCredits1);

            ///<summary>扩展积分</summary>
            public static readonly Field ExtCredits2 = FindByName(__.ExtCredits2);

            ///<summary>扩展积分</summary>
            public static readonly Field ExtCredits3 = FindByName(__.ExtCredits3);

            ///<summary>扩展积分</summary>
            public static readonly Field ExtCredits4 = FindByName(__.ExtCredits4);

            ///<summary>扩展积分</summary>
            public static readonly Field ExtCredits5 = FindByName(__.ExtCredits5);

            ///<summary>扩展积分</summary>
            public static readonly Field ExtCredits6 = FindByName(__.ExtCredits6);

            ///<summary>扩展积分</summary>
            public static readonly Field ExtCredits7 = FindByName(__.ExtCredits7);

            ///<summary>扩展积分</summary>
            public static readonly Field ExtCredits8 = FindByName(__.ExtCredits8);

            ///<summary>头像</summary>
            public static readonly Field AvatarShowId = FindByName(__.AvatarShowId);

            ///<summary>邮件</summary>
            public static readonly Field Email = FindByName(__.Email);

            ///<summary>生日</summary>
            public static readonly Field Bday = FindByName(__.Bday);

            ///<summary>是否签名</summary>
            public static readonly Field Sigstatus = FindByName(__.Sigstatus);

            ///<summary></summary>
            public static readonly Field Tpp = FindByName(__.Tpp);

            ///<summary></summary>
            public static readonly Field Ppp = FindByName(__.Ppp);

            ///<summary>模板Id</summary>
            public static readonly Field TemplateID = FindByName(__.TemplateID);

            ///<summary>短消息铃声</summary>
            public static readonly Field Pmsound = FindByName(__.Pmsound);

            ///<summary>是否显示邮箱</summary>
            public static readonly Field ShowEmail = FindByName(__.ShowEmail);

            ///<summary>是否隐身</summary>
            public static readonly Field Invisible = FindByName(__.Invisible);

            ///<summary>是否有新消息</summary>
            public static readonly Field Newpm = FindByName(__.Newpm);

            ///<summary>新短消息数量</summary>
            public static readonly Field NewpmCount = FindByName(__.NewpmCount);

            ///<summary></summary>
            public static readonly Field AccessMasks = FindByName(__.AccessMasks);

            ///<summary>在线状态,1为在线,0为不在线</summary>
            public static readonly Field OnlineState = FindByName(__.OnlineState);

            ///<summary>是否接收论坛通知</summary>
            public static readonly Field NewsLetter = FindByName(__.NewsLetter);

            ///<summary>MD5加密盐</summary>
            public static readonly Field Salt = FindByName(__.Salt);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得用户字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号</summary>
            public const String ID = "ID";

            ///<summary>登录账户</summary>
            public const String Name = "Name";

            ///<summary>昵称</summary>
            public const String NickName = "NickName";

            ///<summary>登录密码</summary>
            public const String Password = "Password";

            ///<summary>用户安全提问码</summary>
            public const String Secques = "Secques";

            ///<summary>空间</summary>
            public const String SpaceID = "SpaceID";

            ///<summary>性别</summary>
            public const String Gender = "Gender";

            ///<summary>管理权限</summary>
            public const String AdminID = "AdminID";

            ///<summary>用户组</summary>
            public const String GroupID = "GroupID";

            ///<summary>组过期时间</summary>
            public const String GroupExpiry = "GroupExpiry";

            ///<summary>扩展用户组</summary>
            public const String ExtGroupIds = "ExtGroupIds";

            ///<summary>注册IP</summary>
            public const String RegIP = "RegIP";

            ///<summary>注册时间</summary>
            public const String JoinDate = "JoinDate";

            ///<summary>上次登录IP</summary>
            public const String LastIP = "LastIP";

            ///<summary>上次访问时间</summary>
            public const String LastVisit = "LastVisit";

            ///<summary>最后活动时间</summary>
            public const String LastActivity = "LastActivity";

            ///<summary>最后发帖时间</summary>
            public const String LastPost = "LastPost";

            ///<summary>最后帖子编号</summary>
            public const String LastPostID = "LastPostID";

            ///<summary>最后帖子标题</summary>
            public const String LastPostTitle = "LastPostTitle";

            ///<summary>发帖数</summary>
            public const String Posts = "Posts";

            ///<summary>精华帖数</summary>
            public const String DigestPosts = "DigestPosts";

            ///<summary>在线时间</summary>
            public const String OLTime = "OLTime";

            ///<summary>页面访问量</summary>
            public const String PageViews = "PageViews";

            ///<summary>积分数</summary>
            public const String Credits = "Credits";

            ///<summary>扩展积分</summary>
            public const String ExtCredits1 = "ExtCredits1";

            ///<summary>扩展积分</summary>
            public const String ExtCredits2 = "ExtCredits2";

            ///<summary>扩展积分</summary>
            public const String ExtCredits3 = "ExtCredits3";

            ///<summary>扩展积分</summary>
            public const String ExtCredits4 = "ExtCredits4";

            ///<summary>扩展积分</summary>
            public const String ExtCredits5 = "ExtCredits5";

            ///<summary>扩展积分</summary>
            public const String ExtCredits6 = "ExtCredits6";

            ///<summary>扩展积分</summary>
            public const String ExtCredits7 = "ExtCredits7";

            ///<summary>扩展积分</summary>
            public const String ExtCredits8 = "ExtCredits8";

            ///<summary>头像</summary>
            public const String AvatarShowId = "AvatarShowId";

            ///<summary>邮件</summary>
            public const String Email = "Email";

            ///<summary>生日</summary>
            public const String Bday = "Bday";

            ///<summary>是否签名</summary>
            public const String Sigstatus = "Sigstatus";

            ///<summary></summary>
            public const String Tpp = "Tpp";

            ///<summary></summary>
            public const String Ppp = "Ppp";

            ///<summary>模板Id</summary>
            public const String TemplateID = "TemplateID";

            ///<summary>短消息铃声</summary>
            public const String Pmsound = "Pmsound";

            ///<summary>是否显示邮箱</summary>
            public const String ShowEmail = "ShowEmail";

            ///<summary>是否隐身</summary>
            public const String Invisible = "Invisible";

            ///<summary>是否有新消息</summary>
            public const String Newpm = "Newpm";

            ///<summary>新短消息数量</summary>
            public const String NewpmCount = "NewpmCount";

            ///<summary></summary>
            public const String AccessMasks = "AccessMasks";

            ///<summary>在线状态,1为在线,0为不在线</summary>
            public const String OnlineState = "OnlineState";

            ///<summary>是否接收论坛通知</summary>
            public const String NewsLetter = "NewsLetter";

            ///<summary>MD5加密盐</summary>
            public const String Salt = "Salt";

        }
        #endregion
    }

    /// <summary>用户接口</summary>
    public partial interface IUser
    {
        #region 属性
        /// <summary>编号</summary>
        Int32 ID { get; set; }

        /// <summary>登录账户</summary>
        String Name { get; set; }

        /// <summary>昵称</summary>
        String NickName { get; set; }

        /// <summary>登录密码</summary>
        String Password { get; set; }

        /// <summary>用户安全提问码</summary>
        String Secques { get; set; }

        /// <summary>空间</summary>
        Int32 SpaceID { get; set; }

        /// <summary>性别</summary>
        Int32 Gender { get; set; }

        /// <summary>管理权限</summary>
        Int32 AdminID { get; set; }

        /// <summary>用户组</summary>
        Int32 GroupID { get; set; }

        /// <summary>组过期时间</summary>
        Int32 GroupExpiry { get; set; }

        /// <summary>扩展用户组</summary>
        String ExtGroupIds { get; set; }

        /// <summary>注册IP</summary>
        String RegIP { get; set; }

        /// <summary>注册时间</summary>
        DateTime JoinDate { get; set; }

        /// <summary>上次登录IP</summary>
        String LastIP { get; set; }

        /// <summary>上次访问时间</summary>
        DateTime LastVisit { get; set; }

        /// <summary>最后活动时间</summary>
        DateTime LastActivity { get; set; }

        /// <summary>最后发帖时间</summary>
        DateTime LastPost { get; set; }

        /// <summary>最后帖子编号</summary>
        Int32 LastPostID { get; set; }

        /// <summary>最后帖子标题</summary>
        String LastPostTitle { get; set; }

        /// <summary>发帖数</summary>
        Int32 Posts { get; set; }

        /// <summary>精华帖数</summary>
        Int32 DigestPosts { get; set; }

        /// <summary>在线时间</summary>
        Int32 OLTime { get; set; }

        /// <summary>页面访问量</summary>
        Int32 PageViews { get; set; }

        /// <summary>积分数</summary>
        Int32 Credits { get; set; }

        /// <summary>扩展积分</summary>
        Single ExtCredits1 { get; set; }

        /// <summary>扩展积分</summary>
        Single ExtCredits2 { get; set; }

        /// <summary>扩展积分</summary>
        Single ExtCredits3 { get; set; }

        /// <summary>扩展积分</summary>
        Single ExtCredits4 { get; set; }

        /// <summary>扩展积分</summary>
        Single ExtCredits5 { get; set; }

        /// <summary>扩展积分</summary>
        Single ExtCredits6 { get; set; }

        /// <summary>扩展积分</summary>
        Single ExtCredits7 { get; set; }

        /// <summary>扩展积分</summary>
        Single ExtCredits8 { get; set; }

        /// <summary>头像</summary>
        Int32 AvatarShowId { get; set; }

        /// <summary>邮件</summary>
        String Email { get; set; }

        /// <summary>生日</summary>
        String Bday { get; set; }

        /// <summary>是否签名</summary>
        Int32 Sigstatus { get; set; }

        /// <summary></summary>
        Int32 Tpp { get; set; }

        /// <summary></summary>
        Int32 Ppp { get; set; }

        /// <summary>模板Id</summary>
        Int32 TemplateID { get; set; }

        /// <summary>短消息铃声</summary>
        Int32 Pmsound { get; set; }

        /// <summary>是否显示邮箱</summary>
        Boolean ShowEmail { get; set; }

        /// <summary>是否隐身</summary>
        Boolean Invisible { get; set; }

        /// <summary>是否有新消息</summary>
        Boolean Newpm { get; set; }

        /// <summary>新短消息数量</summary>
        Int32 NewpmCount { get; set; }

        /// <summary></summary>
        Int32 AccessMasks { get; set; }

        /// <summary>在线状态,1为在线,0为不在线</summary>
        Int32 OnlineState { get; set; }

        /// <summary>是否接收论坛通知</summary>
        Int32 NewsLetter { get; set; }

        /// <summary>MD5加密盐</summary>
        String Salt { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}