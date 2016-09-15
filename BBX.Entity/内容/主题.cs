﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace BBX.Entity
{
    /// <summary>主题</summary>
    [Serializable]
    [DataObject]
    [Description("主题")]
    [BindIndex("IX_Topic_fid_displayorder", false, "fid,displayorder")]
    [BindIndex("IX_Topic_fid_displayorder_lastpostid", false, "fid,displayorder,lastpostid")]
    [BindIndex("IX_Topic_fid_displayorder_postdatetime_lastpostid", false, "fid,displayorder,postdatetime,lastpostid")]
    [BindIndex("IX_Topic_fid_displayorder_postdatetime_replies", false, "fid,displayorder,postdatetime,replies")]
    [BindIndex("IX_Topic_fid_displayorder_postdatetime_views", false, "fid,displayorder,postdatetime,views")]
    [BindTable("Topic", Description = "主题", ConnName = "BBX", DbType = DatabaseType.SqlServer)]
    public partial class Topic : ITopic
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

        private Int32 _Fid;
        /// <summary>论坛</summary>
        [DisplayName("论坛")]
        [Description("论坛")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(2, "Fid", "论坛", null, "int", 10, 0, false)]
        public virtual Int32 Fid
        {
            get { return _Fid; }
            set { if (OnPropertyChanging(__.Fid, value)) { _Fid = value; OnPropertyChanged(__.Fid); } }
        }

        private Int32 _IconID;
        /// <summary>图标</summary>
        [DisplayName("图标")]
        [Description("图标")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(3, "IconID", "图标", null, "int", 10, 0, false)]
        public virtual Int32 IconID
        {
            get { return _IconID; }
            set { if (OnPropertyChanging(__.IconID, value)) { _IconID = value; OnPropertyChanged(__.IconID); } }
        }

        private Int32 _TypeID;
        /// <summary>类型</summary>
        [DisplayName("类型")]
        [Description("类型")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(4, "TypeID", "类型", null, "int", 10, 0, false)]
        public virtual Int32 TypeID
        {
            get { return _TypeID; }
            set { if (OnPropertyChanging(__.TypeID, value)) { _TypeID = value; OnPropertyChanged(__.TypeID); } }
        }

        private Int32 _ReadPerm;
        /// <summary>读取权限</summary>
        [DisplayName("读取权限")]
        [Description("读取权限")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(5, "ReadPerm", "读取权限", null, "int", 10, 0, false)]
        public virtual Int32 ReadPerm
        {
            get { return _ReadPerm; }
            set { if (OnPropertyChanging(__.ReadPerm, value)) { _ReadPerm = value; OnPropertyChanged(__.ReadPerm); } }
        }

        private Int32 _Price;
        /// <summary>价格</summary>
        [DisplayName("价格")]
        [Description("价格")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(6, "Price", "价格", null, "int", 10, 0, false)]
        public virtual Int32 Price
        {
            get { return _Price; }
            set { if (OnPropertyChanging(__.Price, value)) { _Price = value; OnPropertyChanged(__.Price); } }
        }

        private String _Poster;
        /// <summary>发帖人</summary>
        [DisplayName("发帖人")]
        [Description("发帖人")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(7, "Poster", "发帖人", null, "nvarchar(50)", 0, 0, true)]
        public virtual String Poster
        {
            get { return _Poster; }
            set { if (OnPropertyChanging(__.Poster, value)) { _Poster = value; OnPropertyChanged(__.Poster); } }
        }

        private Int32 _PosterID;
        /// <summary>发帖人ID</summary>
        [DisplayName("发帖人ID")]
        [Description("发帖人ID")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(8, "PosterID", "发帖人ID", null, "int", 10, 0, false)]
        public virtual Int32 PosterID
        {
            get { return _PosterID; }
            set { if (OnPropertyChanging(__.PosterID, value)) { _PosterID = value; OnPropertyChanged(__.PosterID); } }
        }

        private String _Title;
        /// <summary>标题</summary>
        [DisplayName("标题")]
        [Description("标题")]
        [DataObjectField(false, false, false, 60)]
        [BindColumn(9, "Title", "标题", null, "nvarchar(60)", 0, 0, true, Master=true)]
        public virtual String Title
        {
            get { return _Title; }
            set { if (OnPropertyChanging(__.Title, value)) { _Title = value; OnPropertyChanged(__.Title); } }
        }

        private Int32 _Attention;
        /// <summary>注意</summary>
        [DisplayName("注意")]
        [Description("注意")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(10, "Attention", "注意", null, "int", 10, 0, false)]
        public virtual Int32 Attention
        {
            get { return _Attention; }
            set { if (OnPropertyChanging(__.Attention, value)) { _Attention = value; OnPropertyChanged(__.Attention); } }
        }

        private DateTime _PostDateTime;
        /// <summary>发送时间</summary>
        [DisplayName("发送时间")]
        [Description("发送时间")]
        [DataObjectField(false, false, true, 3)]
        [BindColumn(11, "PostDateTime", "发送时间", null, "datetime", 3, 0, false)]
        public virtual DateTime PostDateTime
        {
            get { return _PostDateTime; }
            set { if (OnPropertyChanging(__.PostDateTime, value)) { _PostDateTime = value; OnPropertyChanged(__.PostDateTime); } }
        }

        private DateTime _LastPost;
        /// <summary>最后发表时间</summary>
        [DisplayName("最后发表时间")]
        [Description("最后发表时间")]
        [DataObjectField(false, false, true, 3)]
        [BindColumn(12, "LastPost", "最后发表时间", null, "datetime", 3, 0, false)]
        public virtual DateTime LastPost
        {
            get { return _LastPost; }
            set { if (OnPropertyChanging(__.LastPost, value)) { _LastPost = value; OnPropertyChanged(__.LastPost); } }
        }

        private Int32 _LastPostID;
        /// <summary>最后帖子ID</summary>
        [DisplayName("最后帖子ID")]
        [Description("最后帖子ID")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(13, "LastPostID", "最后帖子ID", null, "int", 10, 0, false)]
        public virtual Int32 LastPostID
        {
            get { return _LastPostID; }
            set { if (OnPropertyChanging(__.LastPostID, value)) { _LastPostID = value; OnPropertyChanged(__.LastPostID); } }
        }

        private String _LastPoster;
        /// <summary>最后帖子发表者</summary>
        [DisplayName("最后帖子发表者")]
        [Description("最后帖子发表者")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(14, "LastPoster", "最后帖子发表者", null, "nvarchar(50)", 0, 0, true)]
        public virtual String LastPoster
        {
            get { return _LastPoster; }
            set { if (OnPropertyChanging(__.LastPoster, value)) { _LastPoster = value; OnPropertyChanged(__.LastPoster); } }
        }

        private Int32 _LastPosterID;
        /// <summary>最后发帖人ID</summary>
        [DisplayName("最后发帖人ID")]
        [Description("最后发帖人ID")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(15, "LastPosterID", "最后发帖人ID", null, "int", 10, 0, false)]
        public virtual Int32 LastPosterID
        {
            get { return _LastPosterID; }
            set { if (OnPropertyChanging(__.LastPosterID, value)) { _LastPosterID = value; OnPropertyChanged(__.LastPosterID); } }
        }

        private Int32 _Views;
        /// <summary>浏览数</summary>
        [DisplayName("浏览数")]
        [Description("浏览数")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(16, "Views", "浏览数", null, "int", 10, 0, false)]
        public virtual Int32 Views
        {
            get { return _Views; }
            set { if (OnPropertyChanging(__.Views, value)) { _Views = value; OnPropertyChanged(__.Views); } }
        }

        private Int32 _Replies;
        /// <summary>答复</summary>
        [DisplayName("答复")]
        [Description("答复")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(17, "Replies", "答复", null, "int", 10, 0, false)]
        public virtual Int32 Replies
        {
            get { return _Replies; }
            set { if (OnPropertyChanging(__.Replies, value)) { _Replies = value; OnPropertyChanged(__.Replies); } }
        }

        private Int32 _DisplayOrder;
        /// <summary>显示顺序</summary>
        [DisplayName("显示顺序")]
        [Description("显示顺序")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(18, "DisplayOrder", "显示顺序", null, "int", 10, 0, false)]
        public virtual Int32 DisplayOrder
        {
            get { return _DisplayOrder; }
            set { if (OnPropertyChanging(__.DisplayOrder, value)) { _DisplayOrder = value; OnPropertyChanged(__.DisplayOrder); } }
        }

        private String _Highlight;
        /// <summary>高亮显示</summary>
        [DisplayName("高亮显示")]
        [Description("高亮显示")]
        [DataObjectField(false, false, true, 500)]
        [BindColumn(19, "Highlight", "高亮显示", null, "nvarchar(500)", 0, 0, true)]
        public virtual String Highlight
        {
            get { return _Highlight; }
            set { if (OnPropertyChanging(__.Highlight, value)) { _Highlight = value; OnPropertyChanged(__.Highlight); } }
        }

        private Int32 _Digest;
        /// <summary>精华帖</summary>
        [DisplayName("精华帖")]
        [Description("精华帖")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(20, "Digest", "精华帖", null, "int", 10, 0, false)]
        public virtual Int32 Digest
        {
            get { return _Digest; }
            set { if (OnPropertyChanging(__.Digest, value)) { _Digest = value; OnPropertyChanged(__.Digest); } }
        }

        private Int32 _Rate;
        /// <summary>评分</summary>
        [DisplayName("评分")]
        [Description("评分")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(21, "Rate", "评分", null, "int", 10, 0, false)]
        public virtual Int32 Rate
        {
            get { return _Rate; }
            set { if (OnPropertyChanging(__.Rate, value)) { _Rate = value; OnPropertyChanged(__.Rate); } }
        }

        private Int32 _Hide;
        /// <summary>隐藏</summary>
        [DisplayName("隐藏")]
        [Description("隐藏")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(22, "Hide", "隐藏", null, "int", 10, 0, false)]
        public virtual Int32 Hide
        {
            get { return _Hide; }
            set { if (OnPropertyChanging(__.Hide, value)) { _Hide = value; OnPropertyChanged(__.Hide); } }
        }

        private Int32 _Attachment;
        /// <summary>附件类型。1普通附件，2图片附件</summary>
        [DisplayName("附件类型")]
        [Description("附件类型。1普通附件，2图片附件")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(23, "Attachment", "附件类型。1普通附件，2图片附件", null, "int", 10, 0, false)]
        public virtual Int32 Attachment
        {
            get { return _Attachment; }
            set { if (OnPropertyChanging(__.Attachment, value)) { _Attachment = value; OnPropertyChanged(__.Attachment); } }
        }

        private Int32 _Moderated;
        /// <summary>放缓</summary>
        [DisplayName("放缓")]
        [Description("放缓")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(24, "Moderated", "放缓", null, "int", 10, 0, false)]
        public virtual Int32 Moderated
        {
            get { return _Moderated; }
            set { if (OnPropertyChanging(__.Moderated, value)) { _Moderated = value; OnPropertyChanged(__.Moderated); } }
        }

        private Int32 _Closed;
        /// <summary>关闭</summary>
        [DisplayName("关闭")]
        [Description("关闭")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(25, "Closed", "关闭", null, "int", 10, 0, false)]
        public virtual Int32 Closed
        {
            get { return _Closed; }
            set { if (OnPropertyChanging(__.Closed, value)) { _Closed = value; OnPropertyChanged(__.Closed); } }
        }

        private Int32 _Magic;
        /// <summary>魔术</summary>
        [DisplayName("魔术")]
        [Description("魔术")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(26, "Magic", "魔术", null, "int", 10, 0, false)]
        public virtual Int32 Magic
        {
            get { return _Magic; }
            set { if (OnPropertyChanging(__.Magic, value)) { _Magic = value; OnPropertyChanged(__.Magic); } }
        }

        private Int32 _Identify;
        /// <summary>鉴定</summary>
        [DisplayName("鉴定")]
        [Description("鉴定")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(27, "Identify", "鉴定", null, "int", 10, 0, false)]
        public virtual Int32 Identify
        {
            get { return _Identify; }
            set { if (OnPropertyChanging(__.Identify, value)) { _Identify = value; OnPropertyChanged(__.Identify); } }
        }

        private Int32 _Special;
        /// <summary>专题。1投票，2悬赏，3悬赏已结束，4辩论</summary>
        [DisplayName("专题")]
        [Description("专题。1投票，2悬赏，3悬赏已结束，4辩论")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(28, "Special", "专题。1投票，2悬赏，3悬赏已结束，4辩论", null, "int", 10, 0, false)]
        public virtual Int32 Special
        {
            get { return _Special; }
            set { if (OnPropertyChanging(__.Special, value)) { _Special = value; OnPropertyChanged(__.Special); } }
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
                    case __.IconID : return _IconID;
                    case __.TypeID : return _TypeID;
                    case __.ReadPerm : return _ReadPerm;
                    case __.Price : return _Price;
                    case __.Poster : return _Poster;
                    case __.PosterID : return _PosterID;
                    case __.Title : return _Title;
                    case __.Attention : return _Attention;
                    case __.PostDateTime : return _PostDateTime;
                    case __.LastPost : return _LastPost;
                    case __.LastPostID : return _LastPostID;
                    case __.LastPoster : return _LastPoster;
                    case __.LastPosterID : return _LastPosterID;
                    case __.Views : return _Views;
                    case __.Replies : return _Replies;
                    case __.DisplayOrder : return _DisplayOrder;
                    case __.Highlight : return _Highlight;
                    case __.Digest : return _Digest;
                    case __.Rate : return _Rate;
                    case __.Hide : return _Hide;
                    case __.Attachment : return _Attachment;
                    case __.Moderated : return _Moderated;
                    case __.Closed : return _Closed;
                    case __.Magic : return _Magic;
                    case __.Identify : return _Identify;
                    case __.Special : return _Special;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.ID : _ID = Convert.ToInt32(value); break;
                    case __.Fid : _Fid = Convert.ToInt32(value); break;
                    case __.IconID : _IconID = Convert.ToInt32(value); break;
                    case __.TypeID : _TypeID = Convert.ToInt32(value); break;
                    case __.ReadPerm : _ReadPerm = Convert.ToInt32(value); break;
                    case __.Price : _Price = Convert.ToInt32(value); break;
                    case __.Poster : _Poster = Convert.ToString(value); break;
                    case __.PosterID : _PosterID = Convert.ToInt32(value); break;
                    case __.Title : _Title = Convert.ToString(value); break;
                    case __.Attention : _Attention = Convert.ToInt32(value); break;
                    case __.PostDateTime : _PostDateTime = Convert.ToDateTime(value); break;
                    case __.LastPost : _LastPost = Convert.ToDateTime(value); break;
                    case __.LastPostID : _LastPostID = Convert.ToInt32(value); break;
                    case __.LastPoster : _LastPoster = Convert.ToString(value); break;
                    case __.LastPosterID : _LastPosterID = Convert.ToInt32(value); break;
                    case __.Views : _Views = Convert.ToInt32(value); break;
                    case __.Replies : _Replies = Convert.ToInt32(value); break;
                    case __.DisplayOrder : _DisplayOrder = Convert.ToInt32(value); break;
                    case __.Highlight : _Highlight = Convert.ToString(value); break;
                    case __.Digest : _Digest = Convert.ToInt32(value); break;
                    case __.Rate : _Rate = Convert.ToInt32(value); break;
                    case __.Hide : _Hide = Convert.ToInt32(value); break;
                    case __.Attachment : _Attachment = Convert.ToInt32(value); break;
                    case __.Moderated : _Moderated = Convert.ToInt32(value); break;
                    case __.Closed : _Closed = Convert.ToInt32(value); break;
                    case __.Magic : _Magic = Convert.ToInt32(value); break;
                    case __.Identify : _Identify = Convert.ToInt32(value); break;
                    case __.Special : _Special = Convert.ToInt32(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得主题字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号</summary>
            public static readonly Field ID = FindByName(__.ID);

            ///<summary>论坛</summary>
            public static readonly Field Fid = FindByName(__.Fid);

            ///<summary>图标</summary>
            public static readonly Field IconID = FindByName(__.IconID);

            ///<summary>类型</summary>
            public static readonly Field TypeID = FindByName(__.TypeID);

            ///<summary>读取权限</summary>
            public static readonly Field ReadPerm = FindByName(__.ReadPerm);

            ///<summary>价格</summary>
            public static readonly Field Price = FindByName(__.Price);

            ///<summary>发帖人</summary>
            public static readonly Field Poster = FindByName(__.Poster);

            ///<summary>发帖人ID</summary>
            public static readonly Field PosterID = FindByName(__.PosterID);

            ///<summary>标题</summary>
            public static readonly Field Title = FindByName(__.Title);

            ///<summary>注意</summary>
            public static readonly Field Attention = FindByName(__.Attention);

            ///<summary>发送时间</summary>
            public static readonly Field PostDateTime = FindByName(__.PostDateTime);

            ///<summary>最后发表时间</summary>
            public static readonly Field LastPost = FindByName(__.LastPost);

            ///<summary>最后帖子ID</summary>
            public static readonly Field LastPostID = FindByName(__.LastPostID);

            ///<summary>最后帖子发表者</summary>
            public static readonly Field LastPoster = FindByName(__.LastPoster);

            ///<summary>最后发帖人ID</summary>
            public static readonly Field LastPosterID = FindByName(__.LastPosterID);

            ///<summary>浏览数</summary>
            public static readonly Field Views = FindByName(__.Views);

            ///<summary>答复</summary>
            public static readonly Field Replies = FindByName(__.Replies);

            ///<summary>显示顺序</summary>
            public static readonly Field DisplayOrder = FindByName(__.DisplayOrder);

            ///<summary>高亮显示</summary>
            public static readonly Field Highlight = FindByName(__.Highlight);

            ///<summary>精华帖</summary>
            public static readonly Field Digest = FindByName(__.Digest);

            ///<summary>评分</summary>
            public static readonly Field Rate = FindByName(__.Rate);

            ///<summary>隐藏</summary>
            public static readonly Field Hide = FindByName(__.Hide);

            ///<summary>附件类型。1普通附件，2图片附件</summary>
            public static readonly Field Attachment = FindByName(__.Attachment);

            ///<summary>放缓</summary>
            public static readonly Field Moderated = FindByName(__.Moderated);

            ///<summary>关闭</summary>
            public static readonly Field Closed = FindByName(__.Closed);

            ///<summary>魔术</summary>
            public static readonly Field Magic = FindByName(__.Magic);

            ///<summary>鉴定</summary>
            public static readonly Field Identify = FindByName(__.Identify);

            ///<summary>专题。1投票，2悬赏，3悬赏已结束，4辩论</summary>
            public static readonly Field Special = FindByName(__.Special);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得主题字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号</summary>
            public const String ID = "ID";

            ///<summary>论坛</summary>
            public const String Fid = "Fid";

            ///<summary>图标</summary>
            public const String IconID = "IconID";

            ///<summary>类型</summary>
            public const String TypeID = "TypeID";

            ///<summary>读取权限</summary>
            public const String ReadPerm = "ReadPerm";

            ///<summary>价格</summary>
            public const String Price = "Price";

            ///<summary>发帖人</summary>
            public const String Poster = "Poster";

            ///<summary>发帖人ID</summary>
            public const String PosterID = "PosterID";

            ///<summary>标题</summary>
            public const String Title = "Title";

            ///<summary>注意</summary>
            public const String Attention = "Attention";

            ///<summary>发送时间</summary>
            public const String PostDateTime = "PostDateTime";

            ///<summary>最后发表时间</summary>
            public const String LastPost = "LastPost";

            ///<summary>最后帖子ID</summary>
            public const String LastPostID = "LastPostID";

            ///<summary>最后帖子发表者</summary>
            public const String LastPoster = "LastPoster";

            ///<summary>最后发帖人ID</summary>
            public const String LastPosterID = "LastPosterID";

            ///<summary>浏览数</summary>
            public const String Views = "Views";

            ///<summary>答复</summary>
            public const String Replies = "Replies";

            ///<summary>显示顺序</summary>
            public const String DisplayOrder = "DisplayOrder";

            ///<summary>高亮显示</summary>
            public const String Highlight = "Highlight";

            ///<summary>精华帖</summary>
            public const String Digest = "Digest";

            ///<summary>评分</summary>
            public const String Rate = "Rate";

            ///<summary>隐藏</summary>
            public const String Hide = "Hide";

            ///<summary>附件类型。1普通附件，2图片附件</summary>
            public const String Attachment = "Attachment";

            ///<summary>放缓</summary>
            public const String Moderated = "Moderated";

            ///<summary>关闭</summary>
            public const String Closed = "Closed";

            ///<summary>魔术</summary>
            public const String Magic = "Magic";

            ///<summary>鉴定</summary>
            public const String Identify = "Identify";

            ///<summary>专题。1投票，2悬赏，3悬赏已结束，4辩论</summary>
            public const String Special = "Special";

        }
        #endregion
    }

    /// <summary>主题接口</summary>
    public partial interface ITopic
    {
        #region 属性
        /// <summary>编号</summary>
        Int32 ID { get; set; }

        /// <summary>论坛</summary>
        Int32 Fid { get; set; }

        /// <summary>图标</summary>
        Int32 IconID { get; set; }

        /// <summary>类型</summary>
        Int32 TypeID { get; set; }

        /// <summary>读取权限</summary>
        Int32 ReadPerm { get; set; }

        /// <summary>价格</summary>
        Int32 Price { get; set; }

        /// <summary>发帖人</summary>
        String Poster { get; set; }

        /// <summary>发帖人ID</summary>
        Int32 PosterID { get; set; }

        /// <summary>标题</summary>
        String Title { get; set; }

        /// <summary>注意</summary>
        Int32 Attention { get; set; }

        /// <summary>发送时间</summary>
        DateTime PostDateTime { get; set; }

        /// <summary>最后发表时间</summary>
        DateTime LastPost { get; set; }

        /// <summary>最后帖子ID</summary>
        Int32 LastPostID { get; set; }

        /// <summary>最后帖子发表者</summary>
        String LastPoster { get; set; }

        /// <summary>最后发帖人ID</summary>
        Int32 LastPosterID { get; set; }

        /// <summary>浏览数</summary>
        Int32 Views { get; set; }

        /// <summary>答复</summary>
        Int32 Replies { get; set; }

        /// <summary>显示顺序</summary>
        Int32 DisplayOrder { get; set; }

        /// <summary>高亮显示</summary>
        String Highlight { get; set; }

        /// <summary>精华帖</summary>
        Int32 Digest { get; set; }

        /// <summary>评分</summary>
        Int32 Rate { get; set; }

        /// <summary>隐藏</summary>
        Int32 Hide { get; set; }

        /// <summary>附件类型。1普通附件，2图片附件</summary>
        Int32 Attachment { get; set; }

        /// <summary>放缓</summary>
        Int32 Moderated { get; set; }

        /// <summary>关闭</summary>
        Int32 Closed { get; set; }

        /// <summary>魔术</summary>
        Int32 Magic { get; set; }

        /// <summary>鉴定</summary>
        Int32 Identify { get; set; }

        /// <summary>专题。1投票，2悬赏，3悬赏已结束，4辩论</summary>
        Int32 Special { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}