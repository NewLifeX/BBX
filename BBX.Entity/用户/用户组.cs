﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace BBX.Entity
{
    /// <summary>用户组</summary>
    [Serializable]
    [DataObject]
    [Description("用户组")]
    [BindTable("UserGroup", Description = "用户组", ConnName = "BBX", DbType = DatabaseType.SqlServer)]
    public partial class UserGroup : IUserGroup
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

        private Int32 _RadminID;
        /// <summary></summary>
        [DisplayName("RadminID")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(2, "RadminID", "", null, "int", 10, 0, false)]
        public virtual Int32 RadminID
        {
            get { return _RadminID; }
            set { if (OnPropertyChanging(__.RadminID, value)) { _RadminID = value; OnPropertyChanged(__.RadminID); } }
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

        private Int16 _System;
        /// <summary>系统</summary>
        [DisplayName("系统")]
        [Description("系统")]
        [DataObjectField(false, false, true, 5)]
        [BindColumn(4, "System", "系统", null, "smallint", 5, 0, false)]
        public virtual Int16 System
        {
            get { return _System; }
            set { if (OnPropertyChanging(__.System, value)) { _System = value; OnPropertyChanged(__.System); } }
        }

        private String _GroupTitle;
        /// <summary>分组标签</summary>
        [DisplayName("分组标签")]
        [Description("分组标签")]
        [DataObjectField(false, false, false, 50)]
        [BindColumn(5, "GroupTitle", "分组标签", null, "nvarchar(50)", 0, 0, true)]
        public virtual String GroupTitle
        {
            get { return _GroupTitle; }
            set { if (OnPropertyChanging(__.GroupTitle, value)) { _GroupTitle = value; OnPropertyChanged(__.GroupTitle); } }
        }

        private Int32 _Creditshigher;
        /// <summary></summary>
        [DisplayName("Creditshigher")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(6, "Creditshigher", "", null, "int", 10, 0, false)]
        public virtual Int32 Creditshigher
        {
            get { return _Creditshigher; }
            set { if (OnPropertyChanging(__.Creditshigher, value)) { _Creditshigher = value; OnPropertyChanged(__.Creditshigher); } }
        }

        private Int32 _Creditslower;
        /// <summary></summary>
        [DisplayName("Creditslower")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(7, "Creditslower", "", null, "int", 10, 0, false)]
        public virtual Int32 Creditslower
        {
            get { return _Creditslower; }
            set { if (OnPropertyChanging(__.Creditslower, value)) { _Creditslower = value; OnPropertyChanged(__.Creditslower); } }
        }

        private Int32 _Stars;
        /// <summary>评分等级</summary>
        [DisplayName("评分等级")]
        [Description("评分等级")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(8, "Stars", "评分等级", null, "int", 10, 0, false)]
        public virtual Int32 Stars
        {
            get { return _Stars; }
            set { if (OnPropertyChanging(__.Stars, value)) { _Stars = value; OnPropertyChanged(__.Stars); } }
        }

        private String _Color;
        /// <summary>颜色</summary>
        [DisplayName("颜色")]
        [Description("颜色")]
        [DataObjectField(false, false, false, 7)]
        [BindColumn(9, "Color", "颜色", null, "nvarchar(7)", 0, 0, true)]
        public virtual String Color
        {
            get { return _Color; }
            set { if (OnPropertyChanging(__.Color, value)) { _Color = value; OnPropertyChanged(__.Color); } }
        }

        private String _Groupavatar;
        /// <summary></summary>
        [DisplayName("Groupavatar")]
        [Description("")]
        [DataObjectField(false, false, false, 60)]
        [BindColumn(10, "Groupavatar", "", null, "nvarchar(60)", 0, 0, true)]
        public virtual String Groupavatar
        {
            get { return _Groupavatar; }
            set { if (OnPropertyChanging(__.Groupavatar, value)) { _Groupavatar = value; OnPropertyChanged(__.Groupavatar); } }
        }

        private Int32 _Readaccess;
        /// <summary></summary>
        [DisplayName("Readaccess")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(11, "Readaccess", "", null, "int", 10, 0, false)]
        public virtual Int32 Readaccess
        {
            get { return _Readaccess; }
            set { if (OnPropertyChanging(__.Readaccess, value)) { _Readaccess = value; OnPropertyChanged(__.Readaccess); } }
        }

        private Boolean _AllowVisit;
        /// <summary></summary>
        [DisplayName("AllowVisit")]
        [Description("")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(12, "AllowVisit", "", null, "bit", 0, 0, false)]
        public virtual Boolean AllowVisit
        {
            get { return _AllowVisit; }
            set { if (OnPropertyChanging(__.AllowVisit, value)) { _AllowVisit = value; OnPropertyChanged(__.AllowVisit); } }
        }

        private Boolean _AllowPost;
        /// <summary></summary>
        [DisplayName("AllowPost")]
        [Description("")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(13, "AllowPost", "", null, "bit", 0, 0, false)]
        public virtual Boolean AllowPost
        {
            get { return _AllowPost; }
            set { if (OnPropertyChanging(__.AllowPost, value)) { _AllowPost = value; OnPropertyChanged(__.AllowPost); } }
        }

        private Boolean _AllowReply;
        /// <summary></summary>
        [DisplayName("AllowReply")]
        [Description("")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(14, "AllowReply", "", null, "bit", 0, 0, false)]
        public virtual Boolean AllowReply
        {
            get { return _AllowReply; }
            set { if (OnPropertyChanging(__.AllowReply, value)) { _AllowReply = value; OnPropertyChanged(__.AllowReply); } }
        }

        private Boolean _AllowPostpoll;
        /// <summary></summary>
        [DisplayName("AllowPostpoll")]
        [Description("")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(15, "AllowPostpoll", "", null, "bit", 0, 0, false)]
        public virtual Boolean AllowPostpoll
        {
            get { return _AllowPostpoll; }
            set { if (OnPropertyChanging(__.AllowPostpoll, value)) { _AllowPostpoll = value; OnPropertyChanged(__.AllowPostpoll); } }
        }

        private Boolean _AllowDirectPost;
        /// <summary></summary>
        [DisplayName("AllowDirectPost")]
        [Description("")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(16, "AllowDirectPost", "", null, "bit", 0, 0, false)]
        public virtual Boolean AllowDirectPost
        {
            get { return _AllowDirectPost; }
            set { if (OnPropertyChanging(__.AllowDirectPost, value)) { _AllowDirectPost = value; OnPropertyChanged(__.AllowDirectPost); } }
        }

        private Boolean _AllowGetattach;
        /// <summary></summary>
        [DisplayName("AllowGetattach")]
        [Description("")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(17, "AllowGetattach", "", null, "bit", 0, 0, false)]
        public virtual Boolean AllowGetattach
        {
            get { return _AllowGetattach; }
            set { if (OnPropertyChanging(__.AllowGetattach, value)) { _AllowGetattach = value; OnPropertyChanged(__.AllowGetattach); } }
        }

        private Boolean _AllowPostattach;
        /// <summary></summary>
        [DisplayName("AllowPostattach")]
        [Description("")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(18, "AllowPostattach", "", null, "bit", 0, 0, false)]
        public virtual Boolean AllowPostattach
        {
            get { return _AllowPostattach; }
            set { if (OnPropertyChanging(__.AllowPostattach, value)) { _AllowPostattach = value; OnPropertyChanged(__.AllowPostattach); } }
        }

        private Boolean _AllowVote;
        /// <summary></summary>
        [DisplayName("AllowVote")]
        [Description("")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(19, "AllowVote", "", null, "bit", 0, 0, false)]
        public virtual Boolean AllowVote
        {
            get { return _AllowVote; }
            set { if (OnPropertyChanging(__.AllowVote, value)) { _AllowVote = value; OnPropertyChanged(__.AllowVote); } }
        }

        private Boolean _AllowMultigroups;
        /// <summary></summary>
        [DisplayName("AllowMultigroups")]
        [Description("")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(20, "AllowMultigroups", "", null, "bit", 0, 0, false)]
        public virtual Boolean AllowMultigroups
        {
            get { return _AllowMultigroups; }
            set { if (OnPropertyChanging(__.AllowMultigroups, value)) { _AllowMultigroups = value; OnPropertyChanged(__.AllowMultigroups); } }
        }

        private Boolean _AllowSearch;
        /// <summary></summary>
        [DisplayName("AllowSearch")]
        [Description("")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(21, "AllowSearch", "", null, "bit", 0, 0, false)]
        public virtual Boolean AllowSearch
        {
            get { return _AllowSearch; }
            set { if (OnPropertyChanging(__.AllowSearch, value)) { _AllowSearch = value; OnPropertyChanged(__.AllowSearch); } }
        }

        private Boolean _AllowAvatar;
        /// <summary></summary>
        [DisplayName("AllowAvatar")]
        [Description("")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(22, "AllowAvatar", "", null, "bit", 0, 0, false)]
        public virtual Boolean AllowAvatar
        {
            get { return _AllowAvatar; }
            set { if (OnPropertyChanging(__.AllowAvatar, value)) { _AllowAvatar = value; OnPropertyChanged(__.AllowAvatar); } }
        }

        private Boolean _AllowCstatus;
        /// <summary></summary>
        [DisplayName("AllowCstatus")]
        [Description("")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(23, "AllowCstatus", "", null, "bit", 0, 0, false)]
        public virtual Boolean AllowCstatus
        {
            get { return _AllowCstatus; }
            set { if (OnPropertyChanging(__.AllowCstatus, value)) { _AllowCstatus = value; OnPropertyChanged(__.AllowCstatus); } }
        }

        private Boolean _AllowUsebLog;
        /// <summary></summary>
        [DisplayName("AllowUsebLog")]
        [Description("")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(24, "AllowUsebLog", "", null, "bit", 0, 0, false)]
        public virtual Boolean AllowUsebLog
        {
            get { return _AllowUsebLog; }
            set { if (OnPropertyChanging(__.AllowUsebLog, value)) { _AllowUsebLog = value; OnPropertyChanged(__.AllowUsebLog); } }
        }

        private Boolean _AllowInvisible;
        /// <summary></summary>
        [DisplayName("AllowInvisible")]
        [Description("")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(25, "AllowInvisible", "", null, "bit", 0, 0, false)]
        public virtual Boolean AllowInvisible
        {
            get { return _AllowInvisible; }
            set { if (OnPropertyChanging(__.AllowInvisible, value)) { _AllowInvisible = value; OnPropertyChanged(__.AllowInvisible); } }
        }

        private Boolean _AllowTransfer;
        /// <summary></summary>
        [DisplayName("AllowTransfer")]
        [Description("")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(26, "AllowTransfer", "", null, "bit", 0, 0, false)]
        public virtual Boolean AllowTransfer
        {
            get { return _AllowTransfer; }
            set { if (OnPropertyChanging(__.AllowTransfer, value)) { _AllowTransfer = value; OnPropertyChanged(__.AllowTransfer); } }
        }

        private Boolean _AllowSetreadPerm;
        /// <summary></summary>
        [DisplayName("AllowSetreadPerm")]
        [Description("")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(27, "AllowSetreadPerm", "", null, "bit", 0, 0, false)]
        public virtual Boolean AllowSetreadPerm
        {
            get { return _AllowSetreadPerm; }
            set { if (OnPropertyChanging(__.AllowSetreadPerm, value)) { _AllowSetreadPerm = value; OnPropertyChanged(__.AllowSetreadPerm); } }
        }

        private Boolean _AllowSetattachPerm;
        /// <summary></summary>
        [DisplayName("AllowSetattachPerm")]
        [Description("")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(28, "AllowSetattachPerm", "", null, "bit", 0, 0, false)]
        public virtual Boolean AllowSetattachPerm
        {
            get { return _AllowSetattachPerm; }
            set { if (OnPropertyChanging(__.AllowSetattachPerm, value)) { _AllowSetattachPerm = value; OnPropertyChanged(__.AllowSetattachPerm); } }
        }

        private Boolean _AllowHideCode;
        /// <summary></summary>
        [DisplayName("AllowHideCode")]
        [Description("")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(29, "AllowHideCode", "", null, "bit", 0, 0, false)]
        public virtual Boolean AllowHideCode
        {
            get { return _AllowHideCode; }
            set { if (OnPropertyChanging(__.AllowHideCode, value)) { _AllowHideCode = value; OnPropertyChanged(__.AllowHideCode); } }
        }

        private Boolean _AllowHtml;
        /// <summary></summary>
        [DisplayName("AllowHtml")]
        [Description("")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(30, "AllowHtml", "", null, "bit", 0, 0, false)]
        public virtual Boolean AllowHtml
        {
            get { return _AllowHtml; }
            set { if (OnPropertyChanging(__.AllowHtml, value)) { _AllowHtml = value; OnPropertyChanged(__.AllowHtml); } }
        }

        private Boolean _AllowHtmlTitle;
        /// <summary></summary>
        [DisplayName("AllowHtmlTitle")]
        [Description("")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(31, "AllowHtmlTitle", "", null, "bit", 0, 0, false)]
        public virtual Boolean AllowHtmlTitle
        {
            get { return _AllowHtmlTitle; }
            set { if (OnPropertyChanging(__.AllowHtmlTitle, value)) { _AllowHtmlTitle = value; OnPropertyChanged(__.AllowHtmlTitle); } }
        }

        private Boolean _AllowCusbbCode;
        /// <summary></summary>
        [DisplayName("AllowCusbbCode")]
        [Description("")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(32, "AllowCusbbCode", "", null, "bit", 0, 0, false)]
        public virtual Boolean AllowCusbbCode
        {
            get { return _AllowCusbbCode; }
            set { if (OnPropertyChanging(__.AllowCusbbCode, value)) { _AllowCusbbCode = value; OnPropertyChanged(__.AllowCusbbCode); } }
        }

        private Boolean _AllowNickName;
        /// <summary></summary>
        [DisplayName("AllowNickName")]
        [Description("")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(33, "AllowNickName", "", null, "bit", 0, 0, false)]
        public virtual Boolean AllowNickName
        {
            get { return _AllowNickName; }
            set { if (OnPropertyChanging(__.AllowNickName, value)) { _AllowNickName = value; OnPropertyChanged(__.AllowNickName); } }
        }

        private Boolean _AllowSigbbCode;
        /// <summary></summary>
        [DisplayName("AllowSigbbCode")]
        [Description("")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(34, "AllowSigbbCode", "", null, "bit", 0, 0, false)]
        public virtual Boolean AllowSigbbCode
        {
            get { return _AllowSigbbCode; }
            set { if (OnPropertyChanging(__.AllowSigbbCode, value)) { _AllowSigbbCode = value; OnPropertyChanged(__.AllowSigbbCode); } }
        }

        private Boolean _AllowSigimgCode;
        /// <summary></summary>
        [DisplayName("AllowSigimgCode")]
        [Description("")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(35, "AllowSigimgCode", "", null, "bit", 0, 0, false)]
        public virtual Boolean AllowSigimgCode
        {
            get { return _AllowSigimgCode; }
            set { if (OnPropertyChanging(__.AllowSigimgCode, value)) { _AllowSigimgCode = value; OnPropertyChanged(__.AllowSigimgCode); } }
        }

        private Boolean _AllowViewpro;
        /// <summary></summary>
        [DisplayName("AllowViewpro")]
        [Description("")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(36, "AllowViewpro", "", null, "bit", 0, 0, false)]
        public virtual Boolean AllowViewpro
        {
            get { return _AllowViewpro; }
            set { if (OnPropertyChanging(__.AllowViewpro, value)) { _AllowViewpro = value; OnPropertyChanged(__.AllowViewpro); } }
        }

        private Boolean _AllowViewstats;
        /// <summary></summary>
        [DisplayName("AllowViewstats")]
        [Description("")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(37, "AllowViewstats", "", null, "bit", 0, 0, false)]
        public virtual Boolean AllowViewstats
        {
            get { return _AllowViewstats; }
            set { if (OnPropertyChanging(__.AllowViewstats, value)) { _AllowViewstats = value; OnPropertyChanged(__.AllowViewstats); } }
        }

        private Boolean _DisablePeriodctrl;
        /// <summary></summary>
        [DisplayName("DisablePeriodctrl")]
        [Description("")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(38, "DisablePeriodctrl", "", null, "bit", 0, 0, false)]
        public virtual Boolean DisablePeriodctrl
        {
            get { return _DisablePeriodctrl; }
            set { if (OnPropertyChanging(__.DisablePeriodctrl, value)) { _DisablePeriodctrl = value; OnPropertyChanged(__.DisablePeriodctrl); } }
        }

        private Int32 _ReasonPm;
        /// <summary></summary>
        [DisplayName("ReasonPm")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(39, "ReasonPm", "", null, "int", 10, 0, false)]
        public virtual Int32 ReasonPm
        {
            get { return _ReasonPm; }
            set { if (OnPropertyChanging(__.ReasonPm, value)) { _ReasonPm = value; OnPropertyChanged(__.ReasonPm); } }
        }

        private Int32 _MaxPrice;
        /// <summary></summary>
        [DisplayName("MaxPrice")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(40, "MaxPrice", "", null, "int", 10, 0, false)]
        public virtual Int32 MaxPrice
        {
            get { return _MaxPrice; }
            set { if (OnPropertyChanging(__.MaxPrice, value)) { _MaxPrice = value; OnPropertyChanged(__.MaxPrice); } }
        }

        private Int32 _MaxPmNum;
        /// <summary></summary>
        [DisplayName("MaxPmNum")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(41, "MaxPmNum", "", null, "int", 10, 0, false)]
        public virtual Int32 MaxPmNum
        {
            get { return _MaxPmNum; }
            set { if (OnPropertyChanging(__.MaxPmNum, value)) { _MaxPmNum = value; OnPropertyChanged(__.MaxPmNum); } }
        }

        private Int32 _MaxSigSize;
        /// <summary></summary>
        [DisplayName("MaxSigSize")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(42, "MaxSigSize", "", null, "int", 10, 0, false)]
        public virtual Int32 MaxSigSize
        {
            get { return _MaxSigSize; }
            set { if (OnPropertyChanging(__.MaxSigSize, value)) { _MaxSigSize = value; OnPropertyChanged(__.MaxSigSize); } }
        }

        private Int32 _MaxAttachSize;
        /// <summary></summary>
        [DisplayName("MaxAttachSize")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(43, "MaxAttachSize", "", null, "int", 10, 0, false)]
        public virtual Int32 MaxAttachSize
        {
            get { return _MaxAttachSize; }
            set { if (OnPropertyChanging(__.MaxAttachSize, value)) { _MaxAttachSize = value; OnPropertyChanged(__.MaxAttachSize); } }
        }

        private Int32 _MaxSizeperday;
        /// <summary></summary>
        [DisplayName("MaxSizeperday")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(44, "MaxSizeperday", "", null, "int", 10, 0, false)]
        public virtual Int32 MaxSizeperday
        {
            get { return _MaxSizeperday; }
            set { if (OnPropertyChanging(__.MaxSizeperday, value)) { _MaxSizeperday = value; OnPropertyChanged(__.MaxSizeperday); } }
        }

        private String _AttachExtensions;
        /// <summary></summary>
        [DisplayName("AttachExtensions")]
        [Description("")]
        [DataObjectField(false, false, true, 100)]
        [BindColumn(45, "AttachExtensions", "", null, "nvarchar(100)", 0, 0, true)]
        public virtual String AttachExtensions
        {
            get { return _AttachExtensions; }
            set { if (OnPropertyChanging(__.AttachExtensions, value)) { _AttachExtensions = value; OnPropertyChanged(__.AttachExtensions); } }
        }

        private String _Raterange;
        /// <summary></summary>
        [DisplayName("Raterange")]
        [Description("")]
        [DataObjectField(false, false, false, 500)]
        [BindColumn(46, "Raterange", "", null, "nvarchar(500)", 0, 0, true)]
        public virtual String Raterange
        {
            get { return _Raterange; }
            set { if (OnPropertyChanging(__.Raterange, value)) { _Raterange = value; OnPropertyChanged(__.Raterange); } }
        }

        private Boolean _AllowSpace;
        /// <summary></summary>
        [DisplayName("AllowSpace")]
        [Description("")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(47, "AllowSpace", "", null, "bit", 0, 0, false)]
        public virtual Boolean AllowSpace
        {
            get { return _AllowSpace; }
            set { if (OnPropertyChanging(__.AllowSpace, value)) { _AllowSpace = value; OnPropertyChanged(__.AllowSpace); } }
        }

        private Int32 _MaxSpaceattachSize;
        /// <summary></summary>
        [DisplayName("MaxSpaceattachSize")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(48, "MaxSpaceattachSize", "", null, "int", 10, 0, false)]
        public virtual Int32 MaxSpaceattachSize
        {
            get { return _MaxSpaceattachSize; }
            set { if (OnPropertyChanging(__.MaxSpaceattachSize, value)) { _MaxSpaceattachSize = value; OnPropertyChanged(__.MaxSpaceattachSize); } }
        }

        private Int32 _MaxSpacephotoSize;
        /// <summary></summary>
        [DisplayName("MaxSpacephotoSize")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(49, "MaxSpacephotoSize", "", null, "int", 10, 0, false)]
        public virtual Int32 MaxSpacephotoSize
        {
            get { return _MaxSpacephotoSize; }
            set { if (OnPropertyChanging(__.MaxSpacephotoSize, value)) { _MaxSpacephotoSize = value; OnPropertyChanged(__.MaxSpacephotoSize); } }
        }

        private Boolean _AllowDebate;
        /// <summary></summary>
        [DisplayName("AllowDebate")]
        [Description("")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(50, "AllowDebate", "", null, "bit", 0, 0, false)]
        public virtual Boolean AllowDebate
        {
            get { return _AllowDebate; }
            set { if (OnPropertyChanging(__.AllowDebate, value)) { _AllowDebate = value; OnPropertyChanged(__.AllowDebate); } }
        }

        private Boolean _AllowBonus;
        /// <summary></summary>
        [DisplayName("AllowBonus")]
        [Description("")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(51, "AllowBonus", "", null, "bit", 0, 0, false)]
        public virtual Boolean AllowBonus
        {
            get { return _AllowBonus; }
            set { if (OnPropertyChanging(__.AllowBonus, value)) { _AllowBonus = value; OnPropertyChanged(__.AllowBonus); } }
        }

        private Int16 _MinBonusprice;
        /// <summary></summary>
        [DisplayName("MinBonusprice")]
        [Description("")]
        [DataObjectField(false, false, true, 5)]
        [BindColumn(52, "MinBonusprice", "", null, "smallint", 5, 0, false)]
        public virtual Int16 MinBonusprice
        {
            get { return _MinBonusprice; }
            set { if (OnPropertyChanging(__.MinBonusprice, value)) { _MinBonusprice = value; OnPropertyChanged(__.MinBonusprice); } }
        }

        private Int16 _MaxBonusprice;
        /// <summary></summary>
        [DisplayName("MaxBonusprice")]
        [Description("")]
        [DataObjectField(false, false, true, 5)]
        [BindColumn(53, "MaxBonusprice", "", null, "smallint", 5, 0, false)]
        public virtual Int16 MaxBonusprice
        {
            get { return _MaxBonusprice; }
            set { if (OnPropertyChanging(__.MaxBonusprice, value)) { _MaxBonusprice = value; OnPropertyChanged(__.MaxBonusprice); } }
        }

        private Boolean _AllowTrade;
        /// <summary></summary>
        [DisplayName("AllowTrade")]
        [Description("")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(54, "AllowTrade", "", null, "bit", 0, 0, false)]
        public virtual Boolean AllowTrade
        {
            get { return _AllowTrade; }
            set { if (OnPropertyChanging(__.AllowTrade, value)) { _AllowTrade = value; OnPropertyChanged(__.AllowTrade); } }
        }

        private Boolean _AllowDiggs;
        /// <summary></summary>
        [DisplayName("AllowDiggs")]
        [Description("")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(55, "AllowDiggs", "", null, "bit", 0, 0, false)]
        public virtual Boolean AllowDiggs
        {
            get { return _AllowDiggs; }
            set { if (OnPropertyChanging(__.AllowDiggs, value)) { _AllowDiggs = value; OnPropertyChanged(__.AllowDiggs); } }
        }

        private Int16 _ModNewTopics;
        /// <summary></summary>
        [DisplayName("ModNewTopics")]
        [Description("")]
        [DataObjectField(false, false, true, 5)]
        [BindColumn(56, "ModNewTopics", "", null, "smallint", 5, 0, false)]
        public virtual Int16 ModNewTopics
        {
            get { return _ModNewTopics; }
            set { if (OnPropertyChanging(__.ModNewTopics, value)) { _ModNewTopics = value; OnPropertyChanged(__.ModNewTopics); } }
        }

        private Int16 _ModNewPosts;
        /// <summary></summary>
        [DisplayName("ModNewPosts")]
        [Description("")]
        [DataObjectField(false, false, true, 5)]
        [BindColumn(57, "ModNewPosts", "", null, "smallint", 5, 0, false)]
        public virtual Int16 ModNewPosts
        {
            get { return _ModNewPosts; }
            set { if (OnPropertyChanging(__.ModNewPosts, value)) { _ModNewPosts = value; OnPropertyChanged(__.ModNewPosts); } }
        }

        private Int32 _IgnoresecCode;
        /// <summary></summary>
        [DisplayName("IgnoresecCode")]
        [Description("")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(58, "IgnoresecCode", "", null, "int", 10, 0, false)]
        public virtual Int32 IgnoresecCode
        {
            get { return _IgnoresecCode; }
            set { if (OnPropertyChanging(__.IgnoresecCode, value)) { _IgnoresecCode = value; OnPropertyChanged(__.IgnoresecCode); } }
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
                    case __.RadminID : return _RadminID;
                    case __.Type : return _Type;
                    case __.System : return _System;
                    case __.GroupTitle : return _GroupTitle;
                    case __.Creditshigher : return _Creditshigher;
                    case __.Creditslower : return _Creditslower;
                    case __.Stars : return _Stars;
                    case __.Color : return _Color;
                    case __.Groupavatar : return _Groupavatar;
                    case __.Readaccess : return _Readaccess;
                    case __.AllowVisit : return _AllowVisit;
                    case __.AllowPost : return _AllowPost;
                    case __.AllowReply : return _AllowReply;
                    case __.AllowPostpoll : return _AllowPostpoll;
                    case __.AllowDirectPost : return _AllowDirectPost;
                    case __.AllowGetattach : return _AllowGetattach;
                    case __.AllowPostattach : return _AllowPostattach;
                    case __.AllowVote : return _AllowVote;
                    case __.AllowMultigroups : return _AllowMultigroups;
                    case __.AllowSearch : return _AllowSearch;
                    case __.AllowAvatar : return _AllowAvatar;
                    case __.AllowCstatus : return _AllowCstatus;
                    case __.AllowUsebLog : return _AllowUsebLog;
                    case __.AllowInvisible : return _AllowInvisible;
                    case __.AllowTransfer : return _AllowTransfer;
                    case __.AllowSetreadPerm : return _AllowSetreadPerm;
                    case __.AllowSetattachPerm : return _AllowSetattachPerm;
                    case __.AllowHideCode : return _AllowHideCode;
                    case __.AllowHtml : return _AllowHtml;
                    case __.AllowHtmlTitle : return _AllowHtmlTitle;
                    case __.AllowCusbbCode : return _AllowCusbbCode;
                    case __.AllowNickName : return _AllowNickName;
                    case __.AllowSigbbCode : return _AllowSigbbCode;
                    case __.AllowSigimgCode : return _AllowSigimgCode;
                    case __.AllowViewpro : return _AllowViewpro;
                    case __.AllowViewstats : return _AllowViewstats;
                    case __.DisablePeriodctrl : return _DisablePeriodctrl;
                    case __.ReasonPm : return _ReasonPm;
                    case __.MaxPrice : return _MaxPrice;
                    case __.MaxPmNum : return _MaxPmNum;
                    case __.MaxSigSize : return _MaxSigSize;
                    case __.MaxAttachSize : return _MaxAttachSize;
                    case __.MaxSizeperday : return _MaxSizeperday;
                    case __.AttachExtensions : return _AttachExtensions;
                    case __.Raterange : return _Raterange;
                    case __.AllowSpace : return _AllowSpace;
                    case __.MaxSpaceattachSize : return _MaxSpaceattachSize;
                    case __.MaxSpacephotoSize : return _MaxSpacephotoSize;
                    case __.AllowDebate : return _AllowDebate;
                    case __.AllowBonus : return _AllowBonus;
                    case __.MinBonusprice : return _MinBonusprice;
                    case __.MaxBonusprice : return _MaxBonusprice;
                    case __.AllowTrade : return _AllowTrade;
                    case __.AllowDiggs : return _AllowDiggs;
                    case __.ModNewTopics : return _ModNewTopics;
                    case __.ModNewPosts : return _ModNewPosts;
                    case __.IgnoresecCode : return _IgnoresecCode;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.ID : _ID = Convert.ToInt32(value); break;
                    case __.RadminID : _RadminID = Convert.ToInt32(value); break;
                    case __.Type : _Type = Convert.ToInt32(value); break;
                    case __.System : _System = Convert.ToInt16(value); break;
                    case __.GroupTitle : _GroupTitle = Convert.ToString(value); break;
                    case __.Creditshigher : _Creditshigher = Convert.ToInt32(value); break;
                    case __.Creditslower : _Creditslower = Convert.ToInt32(value); break;
                    case __.Stars : _Stars = Convert.ToInt32(value); break;
                    case __.Color : _Color = Convert.ToString(value); break;
                    case __.Groupavatar : _Groupavatar = Convert.ToString(value); break;
                    case __.Readaccess : _Readaccess = Convert.ToInt32(value); break;
                    case __.AllowVisit : _AllowVisit = Convert.ToBoolean(value); break;
                    case __.AllowPost : _AllowPost = Convert.ToBoolean(value); break;
                    case __.AllowReply : _AllowReply = Convert.ToBoolean(value); break;
                    case __.AllowPostpoll : _AllowPostpoll = Convert.ToBoolean(value); break;
                    case __.AllowDirectPost : _AllowDirectPost = Convert.ToBoolean(value); break;
                    case __.AllowGetattach : _AllowGetattach = Convert.ToBoolean(value); break;
                    case __.AllowPostattach : _AllowPostattach = Convert.ToBoolean(value); break;
                    case __.AllowVote : _AllowVote = Convert.ToBoolean(value); break;
                    case __.AllowMultigroups : _AllowMultigroups = Convert.ToBoolean(value); break;
                    case __.AllowSearch : _AllowSearch = Convert.ToBoolean(value); break;
                    case __.AllowAvatar : _AllowAvatar = Convert.ToBoolean(value); break;
                    case __.AllowCstatus : _AllowCstatus = Convert.ToBoolean(value); break;
                    case __.AllowUsebLog : _AllowUsebLog = Convert.ToBoolean(value); break;
                    case __.AllowInvisible : _AllowInvisible = Convert.ToBoolean(value); break;
                    case __.AllowTransfer : _AllowTransfer = Convert.ToBoolean(value); break;
                    case __.AllowSetreadPerm : _AllowSetreadPerm = Convert.ToBoolean(value); break;
                    case __.AllowSetattachPerm : _AllowSetattachPerm = Convert.ToBoolean(value); break;
                    case __.AllowHideCode : _AllowHideCode = Convert.ToBoolean(value); break;
                    case __.AllowHtml : _AllowHtml = Convert.ToBoolean(value); break;
                    case __.AllowHtmlTitle : _AllowHtmlTitle = Convert.ToBoolean(value); break;
                    case __.AllowCusbbCode : _AllowCusbbCode = Convert.ToBoolean(value); break;
                    case __.AllowNickName : _AllowNickName = Convert.ToBoolean(value); break;
                    case __.AllowSigbbCode : _AllowSigbbCode = Convert.ToBoolean(value); break;
                    case __.AllowSigimgCode : _AllowSigimgCode = Convert.ToBoolean(value); break;
                    case __.AllowViewpro : _AllowViewpro = Convert.ToBoolean(value); break;
                    case __.AllowViewstats : _AllowViewstats = Convert.ToBoolean(value); break;
                    case __.DisablePeriodctrl : _DisablePeriodctrl = Convert.ToBoolean(value); break;
                    case __.ReasonPm : _ReasonPm = Convert.ToInt32(value); break;
                    case __.MaxPrice : _MaxPrice = Convert.ToInt32(value); break;
                    case __.MaxPmNum : _MaxPmNum = Convert.ToInt32(value); break;
                    case __.MaxSigSize : _MaxSigSize = Convert.ToInt32(value); break;
                    case __.MaxAttachSize : _MaxAttachSize = Convert.ToInt32(value); break;
                    case __.MaxSizeperday : _MaxSizeperday = Convert.ToInt32(value); break;
                    case __.AttachExtensions : _AttachExtensions = Convert.ToString(value); break;
                    case __.Raterange : _Raterange = Convert.ToString(value); break;
                    case __.AllowSpace : _AllowSpace = Convert.ToBoolean(value); break;
                    case __.MaxSpaceattachSize : _MaxSpaceattachSize = Convert.ToInt32(value); break;
                    case __.MaxSpacephotoSize : _MaxSpacephotoSize = Convert.ToInt32(value); break;
                    case __.AllowDebate : _AllowDebate = Convert.ToBoolean(value); break;
                    case __.AllowBonus : _AllowBonus = Convert.ToBoolean(value); break;
                    case __.MinBonusprice : _MinBonusprice = Convert.ToInt16(value); break;
                    case __.MaxBonusprice : _MaxBonusprice = Convert.ToInt16(value); break;
                    case __.AllowTrade : _AllowTrade = Convert.ToBoolean(value); break;
                    case __.AllowDiggs : _AllowDiggs = Convert.ToBoolean(value); break;
                    case __.ModNewTopics : _ModNewTopics = Convert.ToInt16(value); break;
                    case __.ModNewPosts : _ModNewPosts = Convert.ToInt16(value); break;
                    case __.IgnoresecCode : _IgnoresecCode = Convert.ToInt32(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得用户组字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号</summary>
            public static readonly Field ID = FindByName(__.ID);

            ///<summary></summary>
            public static readonly Field RadminID = FindByName(__.RadminID);

            ///<summary>类型</summary>
            public static readonly Field Type = FindByName(__.Type);

            ///<summary>系统</summary>
            public static readonly Field System = FindByName(__.System);

            ///<summary>分组标签</summary>
            public static readonly Field GroupTitle = FindByName(__.GroupTitle);

            ///<summary></summary>
            public static readonly Field Creditshigher = FindByName(__.Creditshigher);

            ///<summary></summary>
            public static readonly Field Creditslower = FindByName(__.Creditslower);

            ///<summary>评分等级</summary>
            public static readonly Field Stars = FindByName(__.Stars);

            ///<summary>颜色</summary>
            public static readonly Field Color = FindByName(__.Color);

            ///<summary></summary>
            public static readonly Field Groupavatar = FindByName(__.Groupavatar);

            ///<summary></summary>
            public static readonly Field Readaccess = FindByName(__.Readaccess);

            ///<summary></summary>
            public static readonly Field AllowVisit = FindByName(__.AllowVisit);

            ///<summary></summary>
            public static readonly Field AllowPost = FindByName(__.AllowPost);

            ///<summary></summary>
            public static readonly Field AllowReply = FindByName(__.AllowReply);

            ///<summary></summary>
            public static readonly Field AllowPostpoll = FindByName(__.AllowPostpoll);

            ///<summary></summary>
            public static readonly Field AllowDirectPost = FindByName(__.AllowDirectPost);

            ///<summary></summary>
            public static readonly Field AllowGetattach = FindByName(__.AllowGetattach);

            ///<summary></summary>
            public static readonly Field AllowPostattach = FindByName(__.AllowPostattach);

            ///<summary></summary>
            public static readonly Field AllowVote = FindByName(__.AllowVote);

            ///<summary></summary>
            public static readonly Field AllowMultigroups = FindByName(__.AllowMultigroups);

            ///<summary></summary>
            public static readonly Field AllowSearch = FindByName(__.AllowSearch);

            ///<summary></summary>
            public static readonly Field AllowAvatar = FindByName(__.AllowAvatar);

            ///<summary></summary>
            public static readonly Field AllowCstatus = FindByName(__.AllowCstatus);

            ///<summary></summary>
            public static readonly Field AllowUsebLog = FindByName(__.AllowUsebLog);

            ///<summary></summary>
            public static readonly Field AllowInvisible = FindByName(__.AllowInvisible);

            ///<summary></summary>
            public static readonly Field AllowTransfer = FindByName(__.AllowTransfer);

            ///<summary></summary>
            public static readonly Field AllowSetreadPerm = FindByName(__.AllowSetreadPerm);

            ///<summary></summary>
            public static readonly Field AllowSetattachPerm = FindByName(__.AllowSetattachPerm);

            ///<summary></summary>
            public static readonly Field AllowHideCode = FindByName(__.AllowHideCode);

            ///<summary></summary>
            public static readonly Field AllowHtml = FindByName(__.AllowHtml);

            ///<summary></summary>
            public static readonly Field AllowHtmlTitle = FindByName(__.AllowHtmlTitle);

            ///<summary></summary>
            public static readonly Field AllowCusbbCode = FindByName(__.AllowCusbbCode);

            ///<summary></summary>
            public static readonly Field AllowNickName = FindByName(__.AllowNickName);

            ///<summary></summary>
            public static readonly Field AllowSigbbCode = FindByName(__.AllowSigbbCode);

            ///<summary></summary>
            public static readonly Field AllowSigimgCode = FindByName(__.AllowSigimgCode);

            ///<summary></summary>
            public static readonly Field AllowViewpro = FindByName(__.AllowViewpro);

            ///<summary></summary>
            public static readonly Field AllowViewstats = FindByName(__.AllowViewstats);

            ///<summary></summary>
            public static readonly Field DisablePeriodctrl = FindByName(__.DisablePeriodctrl);

            ///<summary></summary>
            public static readonly Field ReasonPm = FindByName(__.ReasonPm);

            ///<summary></summary>
            public static readonly Field MaxPrice = FindByName(__.MaxPrice);

            ///<summary></summary>
            public static readonly Field MaxPmNum = FindByName(__.MaxPmNum);

            ///<summary></summary>
            public static readonly Field MaxSigSize = FindByName(__.MaxSigSize);

            ///<summary></summary>
            public static readonly Field MaxAttachSize = FindByName(__.MaxAttachSize);

            ///<summary></summary>
            public static readonly Field MaxSizeperday = FindByName(__.MaxSizeperday);

            ///<summary></summary>
            public static readonly Field AttachExtensions = FindByName(__.AttachExtensions);

            ///<summary></summary>
            public static readonly Field Raterange = FindByName(__.Raterange);

            ///<summary></summary>
            public static readonly Field AllowSpace = FindByName(__.AllowSpace);

            ///<summary></summary>
            public static readonly Field MaxSpaceattachSize = FindByName(__.MaxSpaceattachSize);

            ///<summary></summary>
            public static readonly Field MaxSpacephotoSize = FindByName(__.MaxSpacephotoSize);

            ///<summary></summary>
            public static readonly Field AllowDebate = FindByName(__.AllowDebate);

            ///<summary></summary>
            public static readonly Field AllowBonus = FindByName(__.AllowBonus);

            ///<summary></summary>
            public static readonly Field MinBonusprice = FindByName(__.MinBonusprice);

            ///<summary></summary>
            public static readonly Field MaxBonusprice = FindByName(__.MaxBonusprice);

            ///<summary></summary>
            public static readonly Field AllowTrade = FindByName(__.AllowTrade);

            ///<summary></summary>
            public static readonly Field AllowDiggs = FindByName(__.AllowDiggs);

            ///<summary></summary>
            public static readonly Field ModNewTopics = FindByName(__.ModNewTopics);

            ///<summary></summary>
            public static readonly Field ModNewPosts = FindByName(__.ModNewPosts);

            ///<summary></summary>
            public static readonly Field IgnoresecCode = FindByName(__.IgnoresecCode);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得用户组字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号</summary>
            public const String ID = "ID";

            ///<summary></summary>
            public const String RadminID = "RadminID";

            ///<summary>类型</summary>
            public const String Type = "Type";

            ///<summary>系统</summary>
            public const String System = "System";

            ///<summary>分组标签</summary>
            public const String GroupTitle = "GroupTitle";

            ///<summary></summary>
            public const String Creditshigher = "Creditshigher";

            ///<summary></summary>
            public const String Creditslower = "Creditslower";

            ///<summary>评分等级</summary>
            public const String Stars = "Stars";

            ///<summary>颜色</summary>
            public const String Color = "Color";

            ///<summary></summary>
            public const String Groupavatar = "Groupavatar";

            ///<summary></summary>
            public const String Readaccess = "Readaccess";

            ///<summary></summary>
            public const String AllowVisit = "AllowVisit";

            ///<summary></summary>
            public const String AllowPost = "AllowPost";

            ///<summary></summary>
            public const String AllowReply = "AllowReply";

            ///<summary></summary>
            public const String AllowPostpoll = "AllowPostpoll";

            ///<summary></summary>
            public const String AllowDirectPost = "AllowDirectPost";

            ///<summary></summary>
            public const String AllowGetattach = "AllowGetattach";

            ///<summary></summary>
            public const String AllowPostattach = "AllowPostattach";

            ///<summary></summary>
            public const String AllowVote = "AllowVote";

            ///<summary></summary>
            public const String AllowMultigroups = "AllowMultigroups";

            ///<summary></summary>
            public const String AllowSearch = "AllowSearch";

            ///<summary></summary>
            public const String AllowAvatar = "AllowAvatar";

            ///<summary></summary>
            public const String AllowCstatus = "AllowCstatus";

            ///<summary></summary>
            public const String AllowUsebLog = "AllowUsebLog";

            ///<summary></summary>
            public const String AllowInvisible = "AllowInvisible";

            ///<summary></summary>
            public const String AllowTransfer = "AllowTransfer";

            ///<summary></summary>
            public const String AllowSetreadPerm = "AllowSetreadPerm";

            ///<summary></summary>
            public const String AllowSetattachPerm = "AllowSetattachPerm";

            ///<summary></summary>
            public const String AllowHideCode = "AllowHideCode";

            ///<summary></summary>
            public const String AllowHtml = "AllowHtml";

            ///<summary></summary>
            public const String AllowHtmlTitle = "AllowHtmlTitle";

            ///<summary></summary>
            public const String AllowCusbbCode = "AllowCusbbCode";

            ///<summary></summary>
            public const String AllowNickName = "AllowNickName";

            ///<summary></summary>
            public const String AllowSigbbCode = "AllowSigbbCode";

            ///<summary></summary>
            public const String AllowSigimgCode = "AllowSigimgCode";

            ///<summary></summary>
            public const String AllowViewpro = "AllowViewpro";

            ///<summary></summary>
            public const String AllowViewstats = "AllowViewstats";

            ///<summary></summary>
            public const String DisablePeriodctrl = "DisablePeriodctrl";

            ///<summary></summary>
            public const String ReasonPm = "ReasonPm";

            ///<summary></summary>
            public const String MaxPrice = "MaxPrice";

            ///<summary></summary>
            public const String MaxPmNum = "MaxPmNum";

            ///<summary></summary>
            public const String MaxSigSize = "MaxSigSize";

            ///<summary></summary>
            public const String MaxAttachSize = "MaxAttachSize";

            ///<summary></summary>
            public const String MaxSizeperday = "MaxSizeperday";

            ///<summary></summary>
            public const String AttachExtensions = "AttachExtensions";

            ///<summary></summary>
            public const String Raterange = "Raterange";

            ///<summary></summary>
            public const String AllowSpace = "AllowSpace";

            ///<summary></summary>
            public const String MaxSpaceattachSize = "MaxSpaceattachSize";

            ///<summary></summary>
            public const String MaxSpacephotoSize = "MaxSpacephotoSize";

            ///<summary></summary>
            public const String AllowDebate = "AllowDebate";

            ///<summary></summary>
            public const String AllowBonus = "AllowBonus";

            ///<summary></summary>
            public const String MinBonusprice = "MinBonusprice";

            ///<summary></summary>
            public const String MaxBonusprice = "MaxBonusprice";

            ///<summary></summary>
            public const String AllowTrade = "AllowTrade";

            ///<summary></summary>
            public const String AllowDiggs = "AllowDiggs";

            ///<summary></summary>
            public const String ModNewTopics = "ModNewTopics";

            ///<summary></summary>
            public const String ModNewPosts = "ModNewPosts";

            ///<summary></summary>
            public const String IgnoresecCode = "IgnoresecCode";

        }
        #endregion
    }

    /// <summary>用户组接口</summary>
    public partial interface IUserGroup
    {
        #region 属性
        /// <summary>编号</summary>
        Int32 ID { get; set; }

        /// <summary></summary>
        Int32 RadminID { get; set; }

        /// <summary>类型</summary>
        Int32 Type { get; set; }

        /// <summary>系统</summary>
        Int16 System { get; set; }

        /// <summary>分组标签</summary>
        String GroupTitle { get; set; }

        /// <summary></summary>
        Int32 Creditshigher { get; set; }

        /// <summary></summary>
        Int32 Creditslower { get; set; }

        /// <summary>评分等级</summary>
        Int32 Stars { get; set; }

        /// <summary>颜色</summary>
        String Color { get; set; }

        /// <summary></summary>
        String Groupavatar { get; set; }

        /// <summary></summary>
        Int32 Readaccess { get; set; }

        /// <summary></summary>
        Boolean AllowVisit { get; set; }

        /// <summary></summary>
        Boolean AllowPost { get; set; }

        /// <summary></summary>
        Boolean AllowReply { get; set; }

        /// <summary></summary>
        Boolean AllowPostpoll { get; set; }

        /// <summary></summary>
        Boolean AllowDirectPost { get; set; }

        /// <summary></summary>
        Boolean AllowGetattach { get; set; }

        /// <summary></summary>
        Boolean AllowPostattach { get; set; }

        /// <summary></summary>
        Boolean AllowVote { get; set; }

        /// <summary></summary>
        Boolean AllowMultigroups { get; set; }

        /// <summary></summary>
        Boolean AllowSearch { get; set; }

        /// <summary></summary>
        Boolean AllowAvatar { get; set; }

        /// <summary></summary>
        Boolean AllowCstatus { get; set; }

        /// <summary></summary>
        Boolean AllowUsebLog { get; set; }

        /// <summary></summary>
        Boolean AllowInvisible { get; set; }

        /// <summary></summary>
        Boolean AllowTransfer { get; set; }

        /// <summary></summary>
        Boolean AllowSetreadPerm { get; set; }

        /// <summary></summary>
        Boolean AllowSetattachPerm { get; set; }

        /// <summary></summary>
        Boolean AllowHideCode { get; set; }

        /// <summary></summary>
        Boolean AllowHtml { get; set; }

        /// <summary></summary>
        Boolean AllowHtmlTitle { get; set; }

        /// <summary></summary>
        Boolean AllowCusbbCode { get; set; }

        /// <summary></summary>
        Boolean AllowNickName { get; set; }

        /// <summary></summary>
        Boolean AllowSigbbCode { get; set; }

        /// <summary></summary>
        Boolean AllowSigimgCode { get; set; }

        /// <summary></summary>
        Boolean AllowViewpro { get; set; }

        /// <summary></summary>
        Boolean AllowViewstats { get; set; }

        /// <summary></summary>
        Boolean DisablePeriodctrl { get; set; }

        /// <summary></summary>
        Int32 ReasonPm { get; set; }

        /// <summary></summary>
        Int32 MaxPrice { get; set; }

        /// <summary></summary>
        Int32 MaxPmNum { get; set; }

        /// <summary></summary>
        Int32 MaxSigSize { get; set; }

        /// <summary></summary>
        Int32 MaxAttachSize { get; set; }

        /// <summary></summary>
        Int32 MaxSizeperday { get; set; }

        /// <summary></summary>
        String AttachExtensions { get; set; }

        /// <summary></summary>
        String Raterange { get; set; }

        /// <summary></summary>
        Boolean AllowSpace { get; set; }

        /// <summary></summary>
        Int32 MaxSpaceattachSize { get; set; }

        /// <summary></summary>
        Int32 MaxSpacephotoSize { get; set; }

        /// <summary></summary>
        Boolean AllowDebate { get; set; }

        /// <summary></summary>
        Boolean AllowBonus { get; set; }

        /// <summary></summary>
        Int16 MinBonusprice { get; set; }

        /// <summary></summary>
        Int16 MaxBonusprice { get; set; }

        /// <summary></summary>
        Boolean AllowTrade { get; set; }

        /// <summary></summary>
        Boolean AllowDiggs { get; set; }

        /// <summary></summary>
        Int16 ModNewTopics { get; set; }

        /// <summary></summary>
        Int16 ModNewPosts { get; set; }

        /// <summary></summary>
        Int32 IgnoresecCode { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}