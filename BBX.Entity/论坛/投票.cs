﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace BBX.Entity
{
    /// <summary>投票</summary>
    [Serializable]
    [DataObject]
    [Description("投票")]
    [BindIndex("IU_Poll_tid", true, "tid")]
    [BindTable("Poll", Description = "投票", ConnName = "BBX", DbType = DatabaseType.SqlServer)]
    public partial class Poll : IPoll
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

        private Int32 _Tid;
        /// <summary>主题编号</summary>
        [DisplayName("主题编号")]
        [Description("主题编号")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(2, "Tid", "主题编号", null, "int", 10, 0, false)]
        public virtual Int32 Tid
        {
            get { return _Tid; }
            set { if (OnPropertyChanging(__.Tid, value)) { _Tid = value; OnPropertyChanged(__.Tid); } }
        }

        private Int32 _DisplayOrder;
        /// <summary>显示顺序</summary>
        [DisplayName("显示顺序")]
        [Description("显示顺序")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(3, "DisplayOrder", "显示顺序", null, "int", 10, 0, false)]
        public virtual Int32 DisplayOrder
        {
            get { return _DisplayOrder; }
            set { if (OnPropertyChanging(__.DisplayOrder, value)) { _DisplayOrder = value; OnPropertyChanged(__.DisplayOrder); } }
        }

        private Int32 _Multiple;
        /// <summary>多个</summary>
        [DisplayName("多个")]
        [Description("多个")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(4, "Multiple", "多个", null, "int", 10, 0, false)]
        public virtual Int32 Multiple
        {
            get { return _Multiple; }
            set { if (OnPropertyChanging(__.Multiple, value)) { _Multiple = value; OnPropertyChanged(__.Multiple); } }
        }

        private Int32 _Visible;
        /// <summary>可见</summary>
        [DisplayName("可见")]
        [Description("可见")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(5, "Visible", "可见", null, "int", 10, 0, false)]
        public virtual Int32 Visible
        {
            get { return _Visible; }
            set { if (OnPropertyChanging(__.Visible, value)) { _Visible = value; OnPropertyChanged(__.Visible); } }
        }

        private Boolean _AllowView;
        /// <summary>允许查看</summary>
        [DisplayName("允许查看")]
        [Description("允许查看")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(6, "AllowView", "允许查看", null, "bit", 0, 0, false)]
        public virtual Boolean AllowView
        {
            get { return _AllowView; }
            set { if (OnPropertyChanging(__.AllowView, value)) { _AllowView = value; OnPropertyChanged(__.AllowView); } }
        }

        private Int16 _MaxChoices;
        /// <summary>最大选项</summary>
        [DisplayName("最大选项")]
        [Description("最大选项")]
        [DataObjectField(false, false, true, 5)]
        [BindColumn(7, "MaxChoices", "最大选项", null, "smallint", 5, 0, false)]
        public virtual Int16 MaxChoices
        {
            get { return _MaxChoices; }
            set { if (OnPropertyChanging(__.MaxChoices, value)) { _MaxChoices = value; OnPropertyChanged(__.MaxChoices); } }
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

        private Int32 _Uid;
        /// <summary>用户编号</summary>
        [DisplayName("用户编号")]
        [Description("用户编号")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(9, "Uid", "用户编号", null, "int", 10, 0, false)]
        public virtual Int32 Uid
        {
            get { return _Uid; }
            set { if (OnPropertyChanging(__.Uid, value)) { _Uid = value; OnPropertyChanged(__.Uid); } }
        }

        private String _Voternames;
        /// <summary>投票者</summary>
        [DisplayName("投票者")]
        [Description("投票者")]
        [DataObjectField(false, false, true, -1)]
        [BindColumn(10, "Voternames", "投票者", null, "ntext", 0, 0, true)]
        public virtual String Voternames
        {
            get { return _Voternames; }
            set { if (OnPropertyChanging(__.Voternames, value)) { _Voternames = value; OnPropertyChanged(__.Voternames); } }
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
                    case __.Tid : return _Tid;
                    case __.DisplayOrder : return _DisplayOrder;
                    case __.Multiple : return _Multiple;
                    case __.Visible : return _Visible;
                    case __.AllowView : return _AllowView;
                    case __.MaxChoices : return _MaxChoices;
                    case __.Expiration : return _Expiration;
                    case __.Uid : return _Uid;
                    case __.Voternames : return _Voternames;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.ID : _ID = Convert.ToInt32(value); break;
                    case __.Tid : _Tid = Convert.ToInt32(value); break;
                    case __.DisplayOrder : _DisplayOrder = Convert.ToInt32(value); break;
                    case __.Multiple : _Multiple = Convert.ToInt32(value); break;
                    case __.Visible : _Visible = Convert.ToInt32(value); break;
                    case __.AllowView : _AllowView = Convert.ToBoolean(value); break;
                    case __.MaxChoices : _MaxChoices = Convert.ToInt16(value); break;
                    case __.Expiration : _Expiration = Convert.ToDateTime(value); break;
                    case __.Uid : _Uid = Convert.ToInt32(value); break;
                    case __.Voternames : _Voternames = Convert.ToString(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得投票字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号</summary>
            public static readonly Field ID = FindByName(__.ID);

            ///<summary>主题编号</summary>
            public static readonly Field Tid = FindByName(__.Tid);

            ///<summary>显示顺序</summary>
            public static readonly Field DisplayOrder = FindByName(__.DisplayOrder);

            ///<summary>多个</summary>
            public static readonly Field Multiple = FindByName(__.Multiple);

            ///<summary>可见</summary>
            public static readonly Field Visible = FindByName(__.Visible);

            ///<summary>允许查看</summary>
            public static readonly Field AllowView = FindByName(__.AllowView);

            ///<summary>最大选项</summary>
            public static readonly Field MaxChoices = FindByName(__.MaxChoices);

            ///<summary>过期时间</summary>
            public static readonly Field Expiration = FindByName(__.Expiration);

            ///<summary>用户编号</summary>
            public static readonly Field Uid = FindByName(__.Uid);

            ///<summary>投票者</summary>
            public static readonly Field Voternames = FindByName(__.Voternames);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得投票字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号</summary>
            public const String ID = "ID";

            ///<summary>主题编号</summary>
            public const String Tid = "Tid";

            ///<summary>显示顺序</summary>
            public const String DisplayOrder = "DisplayOrder";

            ///<summary>多个</summary>
            public const String Multiple = "Multiple";

            ///<summary>可见</summary>
            public const String Visible = "Visible";

            ///<summary>允许查看</summary>
            public const String AllowView = "AllowView";

            ///<summary>最大选项</summary>
            public const String MaxChoices = "MaxChoices";

            ///<summary>过期时间</summary>
            public const String Expiration = "Expiration";

            ///<summary>用户编号</summary>
            public const String Uid = "Uid";

            ///<summary>投票者</summary>
            public const String Voternames = "Voternames";

        }
        #endregion
    }

    /// <summary>投票接口</summary>
    public partial interface IPoll
    {
        #region 属性
        /// <summary>编号</summary>
        Int32 ID { get; set; }

        /// <summary>主题编号</summary>
        Int32 Tid { get; set; }

        /// <summary>显示顺序</summary>
        Int32 DisplayOrder { get; set; }

        /// <summary>多个</summary>
        Int32 Multiple { get; set; }

        /// <summary>可见</summary>
        Int32 Visible { get; set; }

        /// <summary>允许查看</summary>
        Boolean AllowView { get; set; }

        /// <summary>最大选项</summary>
        Int16 MaxChoices { get; set; }

        /// <summary>过期时间</summary>
        DateTime Expiration { get; set; }

        /// <summary>用户编号</summary>
        Int32 Uid { get; set; }

        /// <summary>投票者</summary>
        String Voternames { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}