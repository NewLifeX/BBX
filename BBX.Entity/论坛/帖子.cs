﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace BBX.Entity
{
    /// <summary>帖子</summary>
    [Serializable]
    [DataObject]
    [Description("帖子")]
    [BindIndex("IX_Post_tid_posterid", false, "tid,posterid")]
    [BindIndex("IX_Post_tid_invisible", false, "tid,invisible")]
    [BindTable("Post", Description = "帖子", ConnName = "BBX", DbType = DatabaseType.SqlServer)]
    public partial class Post : IPost
    {
        #region 属性
        private Int32 _ID;
        /// <summary>帖子编号</summary>
        [DisplayName("帖子编号")]
        [Description("帖子编号")]
        [DataObjectField(true, false, true, 10)]
        [BindColumn(1, "ID", "帖子编号", null, "int", 10, 0, false)]
        public virtual Int32 ID
        {
            get { return _ID; }
            set { if (OnPropertyChanging(__.ID, value)) { _ID = value; OnPropertyChanged(__.ID); } }
        }

        private Int32 _Fid;
        /// <summary>论坛编号</summary>
        [DisplayName("论坛编号")]
        [Description("论坛编号")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(2, "Fid", "论坛编号", null, "int", 10, 0, false)]
        public virtual Int32 Fid
        {
            get { return _Fid; }
            set { if (OnPropertyChanging(__.Fid, value)) { _Fid = value; OnPropertyChanged(__.Fid); } }
        }

        private Int32 _Tid;
        /// <summary>主题编号</summary>
        [DisplayName("主题编号")]
        [Description("主题编号")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(3, "Tid", "主题编号", null, "int", 10, 0, false)]
        public virtual Int32 Tid
        {
            get { return _Tid; }
            set { if (OnPropertyChanging(__.Tid, value)) { _Tid = value; OnPropertyChanged(__.Tid); } }
        }

        private Int32 _ParentID;
        /// <summary>父分类</summary>
        [DisplayName("父分类")]
        [Description("父分类")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(4, "ParentID", "父分类", null, "int", 10, 0, false)]
        public virtual Int32 ParentID
        {
            get { return _ParentID; }
            set { if (OnPropertyChanging(__.ParentID, value)) { _ParentID = value; OnPropertyChanged(__.ParentID); } }
        }

        private Int32 _Layer;
        /// <summary>层级</summary>
        [DisplayName("层级")]
        [Description("层级")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(5, "Layer", "层级", null, "int", 10, 0, false)]
        public virtual Int32 Layer
        {
            get { return _Layer; }
            set { if (OnPropertyChanging(__.Layer, value)) { _Layer = value; OnPropertyChanged(__.Layer); } }
        }

        private String _Poster;
        /// <summary>发帖人</summary>
        [DisplayName("发帖人")]
        [Description("发帖人")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(6, "Poster", "发帖人", null, "nvarchar(50)", 0, 0, true)]
        public virtual String Poster
        {
            get { return _Poster; }
            set { if (OnPropertyChanging(__.Poster, value)) { _Poster = value; OnPropertyChanged(__.Poster); } }
        }

        private Int32 _PosterID;
        /// <summary>发帖人</summary>
        [DisplayName("发帖人")]
        [Description("发帖人")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(7, "PosterID", "发帖人", null, "int", 10, 0, false)]
        public virtual Int32 PosterID
        {
            get { return _PosterID; }
            set { if (OnPropertyChanging(__.PosterID, value)) { _PosterID = value; OnPropertyChanged(__.PosterID); } }
        }

        private String _Title;
        /// <summary>标题</summary>
        [DisplayName("标题")]
        [Description("标题")]
        [DataObjectField(false, false, true, 60)]
        [BindColumn(8, "Title", "标题", null, "nvarchar(60)", 0, 0, true, Master=true)]
        public virtual String Title
        {
            get { return _Title; }
            set { if (OnPropertyChanging(__.Title, value)) { _Title = value; OnPropertyChanged(__.Title); } }
        }

        private DateTime _PostDateTime;
        /// <summary>发送时间</summary>
        [DisplayName("发送时间")]
        [Description("发送时间")]
        [DataObjectField(false, false, true, 3)]
        [BindColumn(9, "PostDateTime", "发送时间", null, "datetime", 3, 0, false)]
        public virtual DateTime PostDateTime
        {
            get { return _PostDateTime; }
            set { if (OnPropertyChanging(__.PostDateTime, value)) { _PostDateTime = value; OnPropertyChanged(__.PostDateTime); } }
        }

        private String _IP;
        /// <summary>IP地址</summary>
        [DisplayName("IP地址")]
        [Description("IP地址")]
        [DataObjectField(false, false, true, 15)]
        [BindColumn(10, "IP", "IP地址", null, "nvarchar(15)", 0, 0, true)]
        public virtual String IP
        {
            get { return _IP; }
            set { if (OnPropertyChanging(__.IP, value)) { _IP = value; OnPropertyChanged(__.IP); } }
        }

        private String _LastEdit;
        /// <summary>最后编辑</summary>
        [DisplayName("最后编辑")]
        [Description("最后编辑")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(11, "LastEdit", "最后编辑", null, "nvarchar(50)", 0, 0, true)]
        public virtual String LastEdit
        {
            get { return _LastEdit; }
            set { if (OnPropertyChanging(__.LastEdit, value)) { _LastEdit = value; OnPropertyChanged(__.LastEdit); } }
        }

        private Int32 _Invisible;
        /// <summary>是否隐身</summary>
        [DisplayName("是否隐身")]
        [Description("是否隐身")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(12, "Invisible", "是否隐身", null, "int", 10, 0, false)]
        public virtual Int32 Invisible
        {
            get { return _Invisible; }
            set { if (OnPropertyChanging(__.Invisible, value)) { _Invisible = value; OnPropertyChanged(__.Invisible); } }
        }

        private Int32 _UseSig;
        /// <summary>使用个人签名</summary>
        [DisplayName("使用个人签名")]
        [Description("使用个人签名")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(13, "UseSig", "使用个人签名", null, "int", 10, 0, false)]
        public virtual Int32 UseSig
        {
            get { return _UseSig; }
            set { if (OnPropertyChanging(__.UseSig, value)) { _UseSig = value; OnPropertyChanged(__.UseSig); } }
        }

        private Int32 _HtmlOn;
        /// <summary>开启Html</summary>
        [DisplayName("开启Html")]
        [Description("开启Html")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(14, "HtmlOn", "开启Html", null, "int", 10, 0, false)]
        public virtual Int32 HtmlOn
        {
            get { return _HtmlOn; }
            set { if (OnPropertyChanging(__.HtmlOn, value)) { _HtmlOn = value; OnPropertyChanged(__.HtmlOn); } }
        }

        private Int32 _SmileyOff;
        /// <summary>禁用表情</summary>
        [DisplayName("禁用表情")]
        [Description("禁用表情")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(15, "SmileyOff", "禁用表情", null, "int", 10, 0, false)]
        public virtual Int32 SmileyOff
        {
            get { return _SmileyOff; }
            set { if (OnPropertyChanging(__.SmileyOff, value)) { _SmileyOff = value; OnPropertyChanged(__.SmileyOff); } }
        }

        private Int32 _ParseUrlOff;
        /// <summary>禁用网址自动识别</summary>
        [DisplayName("禁用网址自动识别")]
        [Description("禁用网址自动识别")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(16, "ParseUrlOff", "禁用网址自动识别", null, "int", 10, 0, false)]
        public virtual Int32 ParseUrlOff
        {
            get { return _ParseUrlOff; }
            set { if (OnPropertyChanging(__.ParseUrlOff, value)) { _ParseUrlOff = value; OnPropertyChanged(__.ParseUrlOff); } }
        }

        private Int32 _BBCodeOff;
        /// <summary>禁用论坛代码</summary>
        [DisplayName("禁用论坛代码")]
        [Description("禁用论坛代码")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(17, "BBCodeOff", "禁用论坛代码", null, "int", 10, 0, false)]
        public virtual Int32 BBCodeOff
        {
            get { return _BBCodeOff; }
            set { if (OnPropertyChanging(__.BBCodeOff, value)) { _BBCodeOff = value; OnPropertyChanged(__.BBCodeOff); } }
        }

        private Int32 _Attachment;
        /// <summary>附件</summary>
        [DisplayName("附件")]
        [Description("附件")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(18, "Attachment", "附件", null, "int", 10, 0, false)]
        public virtual Int32 Attachment
        {
            get { return _Attachment; }
            set { if (OnPropertyChanging(__.Attachment, value)) { _Attachment = value; OnPropertyChanged(__.Attachment); } }
        }

        private Int32 _Rate;
        /// <summary>评分</summary>
        [DisplayName("评分")]
        [Description("评分")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(19, "Rate", "评分", null, "int", 10, 0, false)]
        public virtual Int32 Rate
        {
            get { return _Rate; }
            set { if (OnPropertyChanging(__.Rate, value)) { _Rate = value; OnPropertyChanged(__.Rate); } }
        }

        private Int32 _RateTimes;
        /// <summary>平分次数</summary>
        [DisplayName("平分次数")]
        [Description("平分次数")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(20, "RateTimes", "平分次数", null, "int", 10, 0, false)]
        public virtual Int32 RateTimes
        {
            get { return _RateTimes; }
            set { if (OnPropertyChanging(__.RateTimes, value)) { _RateTimes = value; OnPropertyChanged(__.RateTimes); } }
        }

        private String _Message;
        /// <summary>内容</summary>
        [DisplayName("内容")]
        [Description("内容")]
        [DataObjectField(false, false, true, -1)]
        [BindColumn(21, "Message", "内容", null, "ntext", 0, 0, true)]
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
                    case __.Fid : return _Fid;
                    case __.Tid : return _Tid;
                    case __.ParentID : return _ParentID;
                    case __.Layer : return _Layer;
                    case __.Poster : return _Poster;
                    case __.PosterID : return _PosterID;
                    case __.Title : return _Title;
                    case __.PostDateTime : return _PostDateTime;
                    case __.IP : return _IP;
                    case __.LastEdit : return _LastEdit;
                    case __.Invisible : return _Invisible;
                    case __.UseSig : return _UseSig;
                    case __.HtmlOn : return _HtmlOn;
                    case __.SmileyOff : return _SmileyOff;
                    case __.ParseUrlOff : return _ParseUrlOff;
                    case __.BBCodeOff : return _BBCodeOff;
                    case __.Attachment : return _Attachment;
                    case __.Rate : return _Rate;
                    case __.RateTimes : return _RateTimes;
                    case __.Message : return _Message;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.ID : _ID = Convert.ToInt32(value); break;
                    case __.Fid : _Fid = Convert.ToInt32(value); break;
                    case __.Tid : _Tid = Convert.ToInt32(value); break;
                    case __.ParentID : _ParentID = Convert.ToInt32(value); break;
                    case __.Layer : _Layer = Convert.ToInt32(value); break;
                    case __.Poster : _Poster = Convert.ToString(value); break;
                    case __.PosterID : _PosterID = Convert.ToInt32(value); break;
                    case __.Title : _Title = Convert.ToString(value); break;
                    case __.PostDateTime : _PostDateTime = Convert.ToDateTime(value); break;
                    case __.IP : _IP = Convert.ToString(value); break;
                    case __.LastEdit : _LastEdit = Convert.ToString(value); break;
                    case __.Invisible : _Invisible = Convert.ToInt32(value); break;
                    case __.UseSig : _UseSig = Convert.ToInt32(value); break;
                    case __.HtmlOn : _HtmlOn = Convert.ToInt32(value); break;
                    case __.SmileyOff : _SmileyOff = Convert.ToInt32(value); break;
                    case __.ParseUrlOff : _ParseUrlOff = Convert.ToInt32(value); break;
                    case __.BBCodeOff : _BBCodeOff = Convert.ToInt32(value); break;
                    case __.Attachment : _Attachment = Convert.ToInt32(value); break;
                    case __.Rate : _Rate = Convert.ToInt32(value); break;
                    case __.RateTimes : _RateTimes = Convert.ToInt32(value); break;
                    case __.Message : _Message = Convert.ToString(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得帖子字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>帖子编号</summary>
            public static readonly Field ID = FindByName(__.ID);

            ///<summary>论坛编号</summary>
            public static readonly Field Fid = FindByName(__.Fid);

            ///<summary>主题编号</summary>
            public static readonly Field Tid = FindByName(__.Tid);

            ///<summary>父分类</summary>
            public static readonly Field ParentID = FindByName(__.ParentID);

            ///<summary>层级</summary>
            public static readonly Field Layer = FindByName(__.Layer);

            ///<summary>发帖人</summary>
            public static readonly Field Poster = FindByName(__.Poster);

            ///<summary>发帖人</summary>
            public static readonly Field PosterID = FindByName(__.PosterID);

            ///<summary>标题</summary>
            public static readonly Field Title = FindByName(__.Title);

            ///<summary>发送时间</summary>
            public static readonly Field PostDateTime = FindByName(__.PostDateTime);

            ///<summary>IP地址</summary>
            public static readonly Field IP = FindByName(__.IP);

            ///<summary>最后编辑</summary>
            public static readonly Field LastEdit = FindByName(__.LastEdit);

            ///<summary>是否隐身</summary>
            public static readonly Field Invisible = FindByName(__.Invisible);

            ///<summary>使用个人签名</summary>
            public static readonly Field UseSig = FindByName(__.UseSig);

            ///<summary>开启Html</summary>
            public static readonly Field HtmlOn = FindByName(__.HtmlOn);

            ///<summary>禁用表情</summary>
            public static readonly Field SmileyOff = FindByName(__.SmileyOff);

            ///<summary>禁用网址自动识别</summary>
            public static readonly Field ParseUrlOff = FindByName(__.ParseUrlOff);

            ///<summary>禁用论坛代码</summary>
            public static readonly Field BBCodeOff = FindByName(__.BBCodeOff);

            ///<summary>附件</summary>
            public static readonly Field Attachment = FindByName(__.Attachment);

            ///<summary>评分</summary>
            public static readonly Field Rate = FindByName(__.Rate);

            ///<summary>平分次数</summary>
            public static readonly Field RateTimes = FindByName(__.RateTimes);

            ///<summary>内容</summary>
            public static readonly Field Message = FindByName(__.Message);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得帖子字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>帖子编号</summary>
            public const String ID = "ID";

            ///<summary>论坛编号</summary>
            public const String Fid = "Fid";

            ///<summary>主题编号</summary>
            public const String Tid = "Tid";

            ///<summary>父分类</summary>
            public const String ParentID = "ParentID";

            ///<summary>层级</summary>
            public const String Layer = "Layer";

            ///<summary>发帖人</summary>
            public const String Poster = "Poster";

            ///<summary>发帖人</summary>
            public const String PosterID = "PosterID";

            ///<summary>标题</summary>
            public const String Title = "Title";

            ///<summary>发送时间</summary>
            public const String PostDateTime = "PostDateTime";

            ///<summary>IP地址</summary>
            public const String IP = "IP";

            ///<summary>最后编辑</summary>
            public const String LastEdit = "LastEdit";

            ///<summary>是否隐身</summary>
            public const String Invisible = "Invisible";

            ///<summary>使用个人签名</summary>
            public const String UseSig = "UseSig";

            ///<summary>开启Html</summary>
            public const String HtmlOn = "HtmlOn";

            ///<summary>禁用表情</summary>
            public const String SmileyOff = "SmileyOff";

            ///<summary>禁用网址自动识别</summary>
            public const String ParseUrlOff = "ParseUrlOff";

            ///<summary>禁用论坛代码</summary>
            public const String BBCodeOff = "BBCodeOff";

            ///<summary>附件</summary>
            public const String Attachment = "Attachment";

            ///<summary>评分</summary>
            public const String Rate = "Rate";

            ///<summary>平分次数</summary>
            public const String RateTimes = "RateTimes";

            ///<summary>内容</summary>
            public const String Message = "Message";

        }
        #endregion
    }

    /// <summary>帖子接口</summary>
    public partial interface IPost
    {
        #region 属性
        /// <summary>帖子编号</summary>
        Int32 ID { get; set; }

        /// <summary>论坛编号</summary>
        Int32 Fid { get; set; }

        /// <summary>主题编号</summary>
        Int32 Tid { get; set; }

        /// <summary>父分类</summary>
        Int32 ParentID { get; set; }

        /// <summary>层级</summary>
        Int32 Layer { get; set; }

        /// <summary>发帖人</summary>
        String Poster { get; set; }

        /// <summary>发帖人</summary>
        Int32 PosterID { get; set; }

        /// <summary>标题</summary>
        String Title { get; set; }

        /// <summary>发送时间</summary>
        DateTime PostDateTime { get; set; }

        /// <summary>IP地址</summary>
        String IP { get; set; }

        /// <summary>最后编辑</summary>
        String LastEdit { get; set; }

        /// <summary>是否隐身</summary>
        Int32 Invisible { get; set; }

        /// <summary>使用个人签名</summary>
        Int32 UseSig { get; set; }

        /// <summary>开启Html</summary>
        Int32 HtmlOn { get; set; }

        /// <summary>禁用表情</summary>
        Int32 SmileyOff { get; set; }

        /// <summary>禁用网址自动识别</summary>
        Int32 ParseUrlOff { get; set; }

        /// <summary>禁用论坛代码</summary>
        Int32 BBCodeOff { get; set; }

        /// <summary>附件</summary>
        Int32 Attachment { get; set; }

        /// <summary>评分</summary>
        Int32 Rate { get; set; }

        /// <summary>平分次数</summary>
        Int32 RateTimes { get; set; }

        /// <summary>内容</summary>
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