﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace BBX.Entity
{
    /// <summary>邀请</summary>
    [Serializable]
    [DataObject]
    [Description("邀请")]
    [BindIndex("IX_Invitation_Code", false, "Code")]
    [BindIndex("IX_Invitation_CreatorID", false, "CreatorID")]
    [BindIndex("IX_Invitation_InviteType", false, "InviteType")]
    [BindTable("Invitation", Description = "邀请", ConnName = "BBX", DbType = DatabaseType.SqlServer)]
    public partial class Invitation : IInvitation
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

        private String _Code;
        /// <summary>邀请码</summary>
        [DisplayName("邀请码")]
        [Description("邀请码")]
        [DataObjectField(false, false, false, 7)]
        [BindColumn(2, "Code", "邀请码", null, "nvarchar(7)", 0, 0, true)]
        public virtual String Code
        {
            get { return _Code; }
            set { if (OnPropertyChanging(__.Code, value)) { _Code = value; OnPropertyChanged(__.Code); } }
        }

        private Int32 _CreatorID;
        /// <summary>作者</summary>
        [DisplayName("作者")]
        [Description("作者")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(3, "CreatorID", "作者", null, "int", 10, 0, false)]
        public virtual Int32 CreatorID
        {
            get { return _CreatorID; }
            set { if (OnPropertyChanging(__.CreatorID, value)) { _CreatorID = value; OnPropertyChanged(__.CreatorID); } }
        }

        private String _Creator;
        /// <summary>作者</summary>
        [DisplayName("作者")]
        [Description("作者")]
        [DataObjectField(false, false, true, 20)]
        [BindColumn(4, "Creator", "作者", null, "nvarchar(20)", 0, 0, true)]
        public virtual String Creator
        {
            get { return _Creator; }
            set { if (OnPropertyChanging(__.Creator, value)) { _Creator = value; OnPropertyChanged(__.Creator); } }
        }

        private Int32 _SuccessCount;
        /// <summary>成功数</summary>
        [DisplayName("成功数")]
        [Description("成功数")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(5, "SuccessCount", "成功数", null, "int", 10, 0, false)]
        public virtual Int32 SuccessCount
        {
            get { return _SuccessCount; }
            set { if (OnPropertyChanging(__.SuccessCount, value)) { _SuccessCount = value; OnPropertyChanged(__.SuccessCount); } }
        }

        private DateTime _CreateTime;
        /// <summary>创建时间</summary>
        [DisplayName("创建时间")]
        [Description("创建时间")]
        [DataObjectField(false, false, true, 3)]
        [BindColumn(6, "CreateTime", "创建时间", null, "datetime", 3, 0, false)]
        public virtual DateTime CreateTime
        {
            get { return _CreateTime; }
            set { if (OnPropertyChanging(__.CreateTime, value)) { _CreateTime = value; OnPropertyChanged(__.CreateTime); } }
        }

        private DateTime _ExpireTime;
        /// <summary>到期时间</summary>
        [DisplayName("到期时间")]
        [Description("到期时间")]
        [DataObjectField(false, false, true, 3)]
        [BindColumn(7, "ExpireTime", "到期时间", null, "datetime", 3, 0, false)]
        public virtual DateTime ExpireTime
        {
            get { return _ExpireTime; }
            set { if (OnPropertyChanging(__.ExpireTime, value)) { _ExpireTime = value; OnPropertyChanged(__.ExpireTime); } }
        }

        private Int32 _MaxCount;
        /// <summary>最大数</summary>
        [DisplayName("最大数")]
        [Description("最大数")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(8, "MaxCount", "最大数", null, "int", 10, 0, false)]
        public virtual Int32 MaxCount
        {
            get { return _MaxCount; }
            set { if (OnPropertyChanging(__.MaxCount, value)) { _MaxCount = value; OnPropertyChanged(__.MaxCount); } }
        }

        private Int32 _InviteType;
        /// <summary>邀请类型</summary>
        [DisplayName("邀请类型")]
        [Description("邀请类型")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(9, "InviteType", "邀请类型", null, "int", 10, 0, false)]
        public virtual Int32 InviteType
        {
            get { return _InviteType; }
            set { if (OnPropertyChanging(__.InviteType, value)) { _InviteType = value; OnPropertyChanged(__.InviteType); } }
        }

        private Boolean _IsDeleted;
        /// <summary>是否审核</summary>
        [DisplayName("是否审核")]
        [Description("是否审核")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(10, "IsDeleted", "是否审核", null, "bit", 0, 0, false)]
        public virtual Boolean IsDeleted
        {
            get { return _IsDeleted; }
            set { if (OnPropertyChanging(__.IsDeleted, value)) { _IsDeleted = value; OnPropertyChanged(__.IsDeleted); } }
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
                    case __.Code : return _Code;
                    case __.CreatorID : return _CreatorID;
                    case __.Creator : return _Creator;
                    case __.SuccessCount : return _SuccessCount;
                    case __.CreateTime : return _CreateTime;
                    case __.ExpireTime : return _ExpireTime;
                    case __.MaxCount : return _MaxCount;
                    case __.InviteType : return _InviteType;
                    case __.IsDeleted : return _IsDeleted;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.ID : _ID = Convert.ToInt32(value); break;
                    case __.Code : _Code = Convert.ToString(value); break;
                    case __.CreatorID : _CreatorID = Convert.ToInt32(value); break;
                    case __.Creator : _Creator = Convert.ToString(value); break;
                    case __.SuccessCount : _SuccessCount = Convert.ToInt32(value); break;
                    case __.CreateTime : _CreateTime = Convert.ToDateTime(value); break;
                    case __.ExpireTime : _ExpireTime = Convert.ToDateTime(value); break;
                    case __.MaxCount : _MaxCount = Convert.ToInt32(value); break;
                    case __.InviteType : _InviteType = Convert.ToInt32(value); break;
                    case __.IsDeleted : _IsDeleted = Convert.ToBoolean(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得邀请字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号</summary>
            public static readonly Field ID = FindByName(__.ID);

            ///<summary>邀请码</summary>
            public static readonly Field Code = FindByName(__.Code);

            ///<summary>作者</summary>
            public static readonly Field CreatorID = FindByName(__.CreatorID);

            ///<summary>作者</summary>
            public static readonly Field Creator = FindByName(__.Creator);

            ///<summary>成功数</summary>
            public static readonly Field SuccessCount = FindByName(__.SuccessCount);

            ///<summary>创建时间</summary>
            public static readonly Field CreateTime = FindByName(__.CreateTime);

            ///<summary>到期时间</summary>
            public static readonly Field ExpireTime = FindByName(__.ExpireTime);

            ///<summary>最大数</summary>
            public static readonly Field MaxCount = FindByName(__.MaxCount);

            ///<summary>邀请类型</summary>
            public static readonly Field InviteType = FindByName(__.InviteType);

            ///<summary>是否审核</summary>
            public static readonly Field IsDeleted = FindByName(__.IsDeleted);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得邀请字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号</summary>
            public const String ID = "ID";

            ///<summary>邀请码</summary>
            public const String Code = "Code";

            ///<summary>作者</summary>
            public const String CreatorID = "CreatorID";

            ///<summary>作者</summary>
            public const String Creator = "Creator";

            ///<summary>成功数</summary>
            public const String SuccessCount = "SuccessCount";

            ///<summary>创建时间</summary>
            public const String CreateTime = "CreateTime";

            ///<summary>到期时间</summary>
            public const String ExpireTime = "ExpireTime";

            ///<summary>最大数</summary>
            public const String MaxCount = "MaxCount";

            ///<summary>邀请类型</summary>
            public const String InviteType = "InviteType";

            ///<summary>是否审核</summary>
            public const String IsDeleted = "IsDeleted";

        }
        #endregion
    }

    /// <summary>邀请接口</summary>
    public partial interface IInvitation
    {
        #region 属性
        /// <summary>编号</summary>
        Int32 ID { get; set; }

        /// <summary>邀请码</summary>
        String Code { get; set; }

        /// <summary>作者</summary>
        Int32 CreatorID { get; set; }

        /// <summary>作者</summary>
        String Creator { get; set; }

        /// <summary>成功数</summary>
        Int32 SuccessCount { get; set; }

        /// <summary>创建时间</summary>
        DateTime CreateTime { get; set; }

        /// <summary>到期时间</summary>
        DateTime ExpireTime { get; set; }

        /// <summary>最大数</summary>
        Int32 MaxCount { get; set; }

        /// <summary>邀请类型</summary>
        Int32 InviteType { get; set; }

        /// <summary>是否审核</summary>
        Boolean IsDeleted { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}