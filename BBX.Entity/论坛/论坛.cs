﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace BBX.Entity
{
    /// <summary>论坛</summary>
    [Serializable]
    [DataObject]
    [Description("论坛")]
    [BindTable("Forum", Description = "论坛", ConnName = "BBX", DbType = DatabaseType.SqlServer)]
    public partial class XForum : IXForum
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

        private Int32 _ParentID;
        /// <summary>父分类</summary>
        [DisplayName("父分类")]
        [Description("父分类")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(2, "ParentID", "父分类", null, "int", 10, 0, false)]
        public virtual Int32 ParentID
        {
            get { return _ParentID; }
            set { if (OnPropertyChanging(__.ParentID, value)) { _ParentID = value; OnPropertyChanged(__.ParentID); } }
        }

        private String _Name;
        /// <summary>名称</summary>
        [DisplayName("名称")]
        [Description("名称")]
        [DataObjectField(false, false, false, 50)]
        [BindColumn(3, "Name", "名称", null, "nvarchar(50)", 0, 0, true, Master=true)]
        public virtual String Name
        {
            get { return _Name; }
            set { if (OnPropertyChanging(__.Name, value)) { _Name = value; OnPropertyChanged(__.Name); } }
        }

        private Int32 _Status;
        /// <summary>状态。0隐藏，1显示</summary>
        [DisplayName("状态")]
        [Description("状态。0隐藏，1显示")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(4, "Status", "状态。0隐藏，1显示", null, "int", 10, 0, false)]
        public virtual Int32 Status
        {
            get { return _Status; }
            set { if (OnPropertyChanging(__.Status, value)) { _Status = value; OnPropertyChanged(__.Status); } }
        }

        private Int32 _ColCount;
        /// <summary>列数</summary>
        [DisplayName("列数")]
        [Description("列数")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(5, "ColCount", "列数", null, "int", 10, 0, false)]
        public virtual Int32 ColCount
        {
            get { return _ColCount; }
            set { if (OnPropertyChanging(__.ColCount, value)) { _ColCount = value; OnPropertyChanged(__.ColCount); } }
        }

        private Int32 _DisplayOrder;
        /// <summary>显示顺序</summary>
        [DisplayName("显示顺序")]
        [Description("显示顺序")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(6, "DisplayOrder", "显示顺序", null, "int", 10, 0, false)]
        public virtual Int32 DisplayOrder
        {
            get { return _DisplayOrder; }
            set { if (OnPropertyChanging(__.DisplayOrder, value)) { _DisplayOrder = value; OnPropertyChanged(__.DisplayOrder); } }
        }

        private Int32 _TemplateID;
        /// <summary>模板Id</summary>
        [DisplayName("模板Id")]
        [Description("模板Id")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(7, "TemplateID", "模板Id", null, "int", 10, 0, false)]
        public virtual Int32 TemplateID
        {
            get { return _TemplateID; }
            set { if (OnPropertyChanging(__.TemplateID, value)) { _TemplateID = value; OnPropertyChanged(__.TemplateID); } }
        }

        private Int32 _Topics;
        /// <summary>主题数</summary>
        [DisplayName("主题数")]
        [Description("主题数")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(8, "Topics", "主题数", null, "int", 10, 0, false)]
        public virtual Int32 Topics
        {
            get { return _Topics; }
            set { if (OnPropertyChanging(__.Topics, value)) { _Topics = value; OnPropertyChanged(__.Topics); } }
        }

        private Int32 _CurTopics;
        /// <summary>当前主题数</summary>
        [DisplayName("当前主题数")]
        [Description("当前主题数")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(9, "CurTopics", "当前主题数", null, "int", 10, 0, false)]
        public virtual Int32 CurTopics
        {
            get { return _CurTopics; }
            set { if (OnPropertyChanging(__.CurTopics, value)) { _CurTopics = value; OnPropertyChanged(__.CurTopics); } }
        }

        private Int32 _Posts;
        /// <summary>帖子数</summary>
        [DisplayName("帖子数")]
        [Description("帖子数")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(10, "Posts", "帖子数", null, "int", 10, 0, false)]
        public virtual Int32 Posts
        {
            get { return _Posts; }
            set { if (OnPropertyChanging(__.Posts, value)) { _Posts = value; OnPropertyChanged(__.Posts); } }
        }

        private Int32 _TodayPosts;
        /// <summary>今天发帖数</summary>
        [DisplayName("今天发帖数")]
        [Description("今天发帖数")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(11, "TodayPosts", "今天发帖数", null, "int", 10, 0, false)]
        public virtual Int32 TodayPosts
        {
            get { return _TodayPosts; }
            set { if (OnPropertyChanging(__.TodayPosts, value)) { _TodayPosts = value; OnPropertyChanged(__.TodayPosts); } }
        }

        private Int32 _LastTID;
        /// <summary>最后主题</summary>
        [DisplayName("最后主题")]
        [Description("最后主题")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(12, "LastTID", "最后主题", null, "int", 10, 0, false)]
        public virtual Int32 LastTID
        {
            get { return _LastTID; }
            set { if (OnPropertyChanging(__.LastTID, value)) { _LastTID = value; OnPropertyChanged(__.LastTID); } }
        }

        private String _LastTitle;
        /// <summary>最后主题标题</summary>
        [DisplayName("最后主题标题")]
        [Description("最后主题标题")]
        [DataObjectField(false, false, true, 60)]
        [BindColumn(13, "LastTitle", "最后主题标题", null, "nvarchar(60)", 0, 0, true)]
        public virtual String LastTitle
        {
            get { return _LastTitle; }
            set { if (OnPropertyChanging(__.LastTitle, value)) { _LastTitle = value; OnPropertyChanged(__.LastTitle); } }
        }

        private DateTime _LastPost;
        /// <summary>最后发布时间</summary>
        [DisplayName("最后发布时间")]
        [Description("最后发布时间")]
        [DataObjectField(false, false, true, 3)]
        [BindColumn(14, "LastPost", "最后发布时间", null, "datetime", 3, 0, false)]
        public virtual DateTime LastPost
        {
            get { return _LastPost; }
            set { if (OnPropertyChanging(__.LastPost, value)) { _LastPost = value; OnPropertyChanged(__.LastPost); } }
        }

        private Int32 _LastPosterID;
        /// <summary>最后发布者编号</summary>
        [DisplayName("最后发布者编号")]
        [Description("最后发布者编号")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(15, "LastPosterID", "最后发布者编号", null, "int", 10, 0, false)]
        public virtual Int32 LastPosterID
        {
            get { return _LastPosterID; }
            set { if (OnPropertyChanging(__.LastPosterID, value)) { _LastPosterID = value; OnPropertyChanged(__.LastPosterID); } }
        }

        private String _LastPoster;
        /// <summary>最后发布者名称</summary>
        [DisplayName("最后发布者名称")]
        [Description("最后发布者名称")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(16, "LastPoster", "最后发布者名称", null, "nvarchar(50)", 0, 0, true)]
        public virtual String LastPoster
        {
            get { return _LastPoster; }
            set { if (OnPropertyChanging(__.LastPoster, value)) { _LastPoster = value; OnPropertyChanged(__.LastPoster); } }
        }

        private Boolean _AllowSmilies;
        /// <summary>允许表情</summary>
        [DisplayName("允许表情")]
        [Description("允许表情")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(17, "AllowSmilies", "允许表情", null, "bit", 0, 0, false)]
        public virtual Boolean AllowSmilies
        {
            get { return _AllowSmilies; }
            set { if (OnPropertyChanging(__.AllowSmilies, value)) { _AllowSmilies = value; OnPropertyChanged(__.AllowSmilies); } }
        }

        private Boolean _AllowRss;
        /// <summary>允许Rss</summary>
        [DisplayName("允许Rss")]
        [Description("允许Rss")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(18, "AllowRss", "允许Rss", null, "bit", 0, 0, false)]
        public virtual Boolean AllowRss
        {
            get { return _AllowRss; }
            set { if (OnPropertyChanging(__.AllowRss, value)) { _AllowRss = value; OnPropertyChanged(__.AllowRss); } }
        }

        private Boolean _AllowHtml;
        /// <summary>允许HTML</summary>
        [DisplayName("允许HTML")]
        [Description("允许HTML")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(19, "AllowHtml", "允许HTML", null, "bit", 0, 0, false)]
        public virtual Boolean AllowHtml
        {
            get { return _AllowHtml; }
            set { if (OnPropertyChanging(__.AllowHtml, value)) { _AllowHtml = value; OnPropertyChanged(__.AllowHtml); } }
        }

        private Boolean _AllowBbCode;
        /// <summary>允许BB代码</summary>
        [DisplayName("允许BB代码")]
        [Description("允许BB代码")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(20, "AllowBbCode", "允许BB代码", null, "bit", 0, 0, false)]
        public virtual Boolean AllowBbCode
        {
            get { return _AllowBbCode; }
            set { if (OnPropertyChanging(__.AllowBbCode, value)) { _AllowBbCode = value; OnPropertyChanged(__.AllowBbCode); } }
        }

        private Boolean _AllowImgCode;
        /// <summary>允许图片</summary>
        [DisplayName("允许图片")]
        [Description("允许图片")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(21, "AllowImgCode", "允许图片", null, "bit", 0, 0, false)]
        public virtual Boolean AllowImgCode
        {
            get { return _AllowImgCode; }
            set { if (OnPropertyChanging(__.AllowImgCode, value)) { _AllowImgCode = value; OnPropertyChanged(__.AllowImgCode); } }
        }

        private Boolean _AllowBlog;
        /// <summary>允许博客</summary>
        [DisplayName("允许博客")]
        [Description("允许博客")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(22, "AllowBlog", "允许博客", null, "bit", 0, 0, false)]
        public virtual Boolean AllowBlog
        {
            get { return _AllowBlog; }
            set { if (OnPropertyChanging(__.AllowBlog, value)) { _AllowBlog = value; OnPropertyChanged(__.AllowBlog); } }
        }

        private Boolean _IsTrade;
        /// <summary>是否交易</summary>
        [DisplayName("是否交易")]
        [Description("是否交易")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(23, "IsTrade", "是否交易", null, "bit", 0, 0, false)]
        public virtual Boolean IsTrade
        {
            get { return _IsTrade; }
            set { if (OnPropertyChanging(__.IsTrade, value)) { _IsTrade = value; OnPropertyChanged(__.IsTrade); } }
        }

        private Int32 _AllowPostSpecial;
        /// <summary>发表特殊主题</summary>
        [DisplayName("发表特殊主题")]
        [Description("发表特殊主题")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(24, "AllowPostSpecial", "发表特殊主题", null, "int", 10, 0, false)]
        public virtual Int32 AllowPostSpecial
        {
            get { return _AllowPostSpecial; }
            set { if (OnPropertyChanging(__.AllowPostSpecial, value)) { _AllowPostSpecial = value; OnPropertyChanged(__.AllowPostSpecial); } }
        }

        private Boolean _AllowSpecialOnly;
        /// <summary>只允许专题</summary>
        [DisplayName("只允许专题")]
        [Description("只允许专题")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(25, "AllowSpecialOnly", "只允许专题", null, "bit", 0, 0, false)]
        public virtual Boolean AllowSpecialOnly
        {
            get { return _AllowSpecialOnly; }
            set { if (OnPropertyChanging(__.AllowSpecialOnly, value)) { _AllowSpecialOnly = value; OnPropertyChanged(__.AllowSpecialOnly); } }
        }

        private Boolean _AllowEditRules;
        /// <summary>允许编辑规则</summary>
        [DisplayName("允许编辑规则")]
        [Description("允许编辑规则")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(26, "AllowEditRules", "允许编辑规则", null, "bit", 0, 0, false)]
        public virtual Boolean AllowEditRules
        {
            get { return _AllowEditRules; }
            set { if (OnPropertyChanging(__.AllowEditRules, value)) { _AllowEditRules = value; OnPropertyChanged(__.AllowEditRules); } }
        }

        private Boolean _AllowThumbnail;
        /// <summary>允许缩略图</summary>
        [DisplayName("允许缩略图")]
        [Description("允许缩略图")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(27, "AllowThumbnail", "允许缩略图", null, "bit", 0, 0, false)]
        public virtual Boolean AllowThumbnail
        {
            get { return _AllowThumbnail; }
            set { if (OnPropertyChanging(__.AllowThumbnail, value)) { _AllowThumbnail = value; OnPropertyChanged(__.AllowThumbnail); } }
        }

        private Boolean _AllowTag;
        /// <summary>允许标签</summary>
        [DisplayName("允许标签")]
        [Description("允许标签")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(28, "AllowTag", "允许标签", null, "bit", 0, 0, false)]
        public virtual Boolean AllowTag
        {
            get { return _AllowTag; }
            set { if (OnPropertyChanging(__.AllowTag, value)) { _AllowTag = value; OnPropertyChanged(__.AllowTag); } }
        }

        private Int32 _Recyclebin;
        /// <summary>回收站</summary>
        [DisplayName("回收站")]
        [Description("回收站")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(29, "Recyclebin", "回收站", null, "int", 10, 0, false)]
        public virtual Int32 Recyclebin
        {
            get { return _Recyclebin; }
            set { if (OnPropertyChanging(__.Recyclebin, value)) { _Recyclebin = value; OnPropertyChanged(__.Recyclebin); } }
        }

        private Int32 _Modnewposts;
        /// <summary>新帖子模式</summary>
        [DisplayName("新帖子模式")]
        [Description("新帖子模式")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(30, "Modnewposts", "新帖子模式", null, "int", 10, 0, false)]
        public virtual Int32 Modnewposts
        {
            get { return _Modnewposts; }
            set { if (OnPropertyChanging(__.Modnewposts, value)) { _Modnewposts = value; OnPropertyChanged(__.Modnewposts); } }
        }

        private Int32 _Modnewtopics;
        /// <summary>新主题模式</summary>
        [DisplayName("新主题模式")]
        [Description("新主题模式")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(31, "Modnewtopics", "新主题模式", null, "int", 10, 0, false)]
        public virtual Int32 Modnewtopics
        {
            get { return _Modnewtopics; }
            set { if (OnPropertyChanging(__.Modnewtopics, value)) { _Modnewtopics = value; OnPropertyChanged(__.Modnewtopics); } }
        }

        private Int32 _Jammer;
        /// <summary>干扰机</summary>
        [DisplayName("干扰机")]
        [Description("干扰机")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(32, "Jammer", "干扰机", null, "int", 10, 0, false)]
        public virtual Int32 Jammer
        {
            get { return _Jammer; }
            set { if (OnPropertyChanging(__.Jammer, value)) { _Jammer = value; OnPropertyChanged(__.Jammer); } }
        }

        private Boolean _DisableWatermark;
        /// <summary>关闭水印</summary>
        [DisplayName("关闭水印")]
        [Description("关闭水印")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(33, "DisableWatermark", "关闭水印", null, "bit", 0, 0, false)]
        public virtual Boolean DisableWatermark
        {
            get { return _DisableWatermark; }
            set { if (OnPropertyChanging(__.DisableWatermark, value)) { _DisableWatermark = value; OnPropertyChanged(__.DisableWatermark); } }
        }

        private Int32 _Inheritedmod;
        /// <summary>继承模式</summary>
        [DisplayName("继承模式")]
        [Description("继承模式")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(34, "Inheritedmod", "继承模式", null, "int", 10, 0, false)]
        public virtual Int32 Inheritedmod
        {
            get { return _Inheritedmod; }
            set { if (OnPropertyChanging(__.Inheritedmod, value)) { _Inheritedmod = value; OnPropertyChanged(__.Inheritedmod); } }
        }

        private Int32 _AutoClose;
        /// <summary>自动关闭</summary>
        [DisplayName("自动关闭")]
        [Description("自动关闭")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(35, "AutoClose", "自动关闭", null, "int", 10, 0, false)]
        public virtual Int32 AutoClose
        {
            get { return _AutoClose; }
            set { if (OnPropertyChanging(__.AutoClose, value)) { _AutoClose = value; OnPropertyChanged(__.AutoClose); } }
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
                    case __.ParentID : return _ParentID;
                    case __.Name : return _Name;
                    case __.Status : return _Status;
                    case __.ColCount : return _ColCount;
                    case __.DisplayOrder : return _DisplayOrder;
                    case __.TemplateID : return _TemplateID;
                    case __.Topics : return _Topics;
                    case __.CurTopics : return _CurTopics;
                    case __.Posts : return _Posts;
                    case __.TodayPosts : return _TodayPosts;
                    case __.LastTID : return _LastTID;
                    case __.LastTitle : return _LastTitle;
                    case __.LastPost : return _LastPost;
                    case __.LastPosterID : return _LastPosterID;
                    case __.LastPoster : return _LastPoster;
                    case __.AllowSmilies : return _AllowSmilies;
                    case __.AllowRss : return _AllowRss;
                    case __.AllowHtml : return _AllowHtml;
                    case __.AllowBbCode : return _AllowBbCode;
                    case __.AllowImgCode : return _AllowImgCode;
                    case __.AllowBlog : return _AllowBlog;
                    case __.IsTrade : return _IsTrade;
                    case __.AllowPostSpecial : return _AllowPostSpecial;
                    case __.AllowSpecialOnly : return _AllowSpecialOnly;
                    case __.AllowEditRules : return _AllowEditRules;
                    case __.AllowThumbnail : return _AllowThumbnail;
                    case __.AllowTag : return _AllowTag;
                    case __.Recyclebin : return _Recyclebin;
                    case __.Modnewposts : return _Modnewposts;
                    case __.Modnewtopics : return _Modnewtopics;
                    case __.Jammer : return _Jammer;
                    case __.DisableWatermark : return _DisableWatermark;
                    case __.Inheritedmod : return _Inheritedmod;
                    case __.AutoClose : return _AutoClose;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.ID : _ID = Convert.ToInt32(value); break;
                    case __.ParentID : _ParentID = Convert.ToInt32(value); break;
                    case __.Name : _Name = Convert.ToString(value); break;
                    case __.Status : _Status = Convert.ToInt32(value); break;
                    case __.ColCount : _ColCount = Convert.ToInt32(value); break;
                    case __.DisplayOrder : _DisplayOrder = Convert.ToInt32(value); break;
                    case __.TemplateID : _TemplateID = Convert.ToInt32(value); break;
                    case __.Topics : _Topics = Convert.ToInt32(value); break;
                    case __.CurTopics : _CurTopics = Convert.ToInt32(value); break;
                    case __.Posts : _Posts = Convert.ToInt32(value); break;
                    case __.TodayPosts : _TodayPosts = Convert.ToInt32(value); break;
                    case __.LastTID : _LastTID = Convert.ToInt32(value); break;
                    case __.LastTitle : _LastTitle = Convert.ToString(value); break;
                    case __.LastPost : _LastPost = Convert.ToDateTime(value); break;
                    case __.LastPosterID : _LastPosterID = Convert.ToInt32(value); break;
                    case __.LastPoster : _LastPoster = Convert.ToString(value); break;
                    case __.AllowSmilies : _AllowSmilies = Convert.ToBoolean(value); break;
                    case __.AllowRss : _AllowRss = Convert.ToBoolean(value); break;
                    case __.AllowHtml : _AllowHtml = Convert.ToBoolean(value); break;
                    case __.AllowBbCode : _AllowBbCode = Convert.ToBoolean(value); break;
                    case __.AllowImgCode : _AllowImgCode = Convert.ToBoolean(value); break;
                    case __.AllowBlog : _AllowBlog = Convert.ToBoolean(value); break;
                    case __.IsTrade : _IsTrade = Convert.ToBoolean(value); break;
                    case __.AllowPostSpecial : _AllowPostSpecial = Convert.ToInt32(value); break;
                    case __.AllowSpecialOnly : _AllowSpecialOnly = Convert.ToBoolean(value); break;
                    case __.AllowEditRules : _AllowEditRules = Convert.ToBoolean(value); break;
                    case __.AllowThumbnail : _AllowThumbnail = Convert.ToBoolean(value); break;
                    case __.AllowTag : _AllowTag = Convert.ToBoolean(value); break;
                    case __.Recyclebin : _Recyclebin = Convert.ToInt32(value); break;
                    case __.Modnewposts : _Modnewposts = Convert.ToInt32(value); break;
                    case __.Modnewtopics : _Modnewtopics = Convert.ToInt32(value); break;
                    case __.Jammer : _Jammer = Convert.ToInt32(value); break;
                    case __.DisableWatermark : _DisableWatermark = Convert.ToBoolean(value); break;
                    case __.Inheritedmod : _Inheritedmod = Convert.ToInt32(value); break;
                    case __.AutoClose : _AutoClose = Convert.ToInt32(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得论坛字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号</summary>
            public static readonly Field ID = FindByName(__.ID);

            ///<summary>父分类</summary>
            public static readonly Field ParentID = FindByName(__.ParentID);

            ///<summary>名称</summary>
            public static readonly Field Name = FindByName(__.Name);

            ///<summary>状态。0隐藏，1显示</summary>
            public static readonly Field Status = FindByName(__.Status);

            ///<summary>列数</summary>
            public static readonly Field ColCount = FindByName(__.ColCount);

            ///<summary>显示顺序</summary>
            public static readonly Field DisplayOrder = FindByName(__.DisplayOrder);

            ///<summary>模板Id</summary>
            public static readonly Field TemplateID = FindByName(__.TemplateID);

            ///<summary>主题数</summary>
            public static readonly Field Topics = FindByName(__.Topics);

            ///<summary>当前主题数</summary>
            public static readonly Field CurTopics = FindByName(__.CurTopics);

            ///<summary>帖子数</summary>
            public static readonly Field Posts = FindByName(__.Posts);

            ///<summary>今天发帖数</summary>
            public static readonly Field TodayPosts = FindByName(__.TodayPosts);

            ///<summary>最后主题</summary>
            public static readonly Field LastTID = FindByName(__.LastTID);

            ///<summary>最后主题标题</summary>
            public static readonly Field LastTitle = FindByName(__.LastTitle);

            ///<summary>最后发布时间</summary>
            public static readonly Field LastPost = FindByName(__.LastPost);

            ///<summary>最后发布者编号</summary>
            public static readonly Field LastPosterID = FindByName(__.LastPosterID);

            ///<summary>最后发布者名称</summary>
            public static readonly Field LastPoster = FindByName(__.LastPoster);

            ///<summary>允许表情</summary>
            public static readonly Field AllowSmilies = FindByName(__.AllowSmilies);

            ///<summary>允许Rss</summary>
            public static readonly Field AllowRss = FindByName(__.AllowRss);

            ///<summary>允许HTML</summary>
            public static readonly Field AllowHtml = FindByName(__.AllowHtml);

            ///<summary>允许BB代码</summary>
            public static readonly Field AllowBbCode = FindByName(__.AllowBbCode);

            ///<summary>允许图片</summary>
            public static readonly Field AllowImgCode = FindByName(__.AllowImgCode);

            ///<summary>允许博客</summary>
            public static readonly Field AllowBlog = FindByName(__.AllowBlog);

            ///<summary>是否交易</summary>
            public static readonly Field IsTrade = FindByName(__.IsTrade);

            ///<summary>发表特殊主题</summary>
            public static readonly Field AllowPostSpecial = FindByName(__.AllowPostSpecial);

            ///<summary>只允许专题</summary>
            public static readonly Field AllowSpecialOnly = FindByName(__.AllowSpecialOnly);

            ///<summary>允许编辑规则</summary>
            public static readonly Field AllowEditRules = FindByName(__.AllowEditRules);

            ///<summary>允许缩略图</summary>
            public static readonly Field AllowThumbnail = FindByName(__.AllowThumbnail);

            ///<summary>允许标签</summary>
            public static readonly Field AllowTag = FindByName(__.AllowTag);

            ///<summary>回收站</summary>
            public static readonly Field Recyclebin = FindByName(__.Recyclebin);

            ///<summary>新帖子模式</summary>
            public static readonly Field Modnewposts = FindByName(__.Modnewposts);

            ///<summary>新主题模式</summary>
            public static readonly Field Modnewtopics = FindByName(__.Modnewtopics);

            ///<summary>干扰机</summary>
            public static readonly Field Jammer = FindByName(__.Jammer);

            ///<summary>关闭水印</summary>
            public static readonly Field DisableWatermark = FindByName(__.DisableWatermark);

            ///<summary>继承模式</summary>
            public static readonly Field Inheritedmod = FindByName(__.Inheritedmod);

            ///<summary>自动关闭</summary>
            public static readonly Field AutoClose = FindByName(__.AutoClose);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得论坛字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号</summary>
            public const String ID = "ID";

            ///<summary>父分类</summary>
            public const String ParentID = "ParentID";

            ///<summary>名称</summary>
            public const String Name = "Name";

            ///<summary>状态。0隐藏，1显示</summary>
            public const String Status = "Status";

            ///<summary>列数</summary>
            public const String ColCount = "ColCount";

            ///<summary>显示顺序</summary>
            public const String DisplayOrder = "DisplayOrder";

            ///<summary>模板Id</summary>
            public const String TemplateID = "TemplateID";

            ///<summary>主题数</summary>
            public const String Topics = "Topics";

            ///<summary>当前主题数</summary>
            public const String CurTopics = "CurTopics";

            ///<summary>帖子数</summary>
            public const String Posts = "Posts";

            ///<summary>今天发帖数</summary>
            public const String TodayPosts = "TodayPosts";

            ///<summary>最后主题</summary>
            public const String LastTID = "LastTID";

            ///<summary>最后主题标题</summary>
            public const String LastTitle = "LastTitle";

            ///<summary>最后发布时间</summary>
            public const String LastPost = "LastPost";

            ///<summary>最后发布者编号</summary>
            public const String LastPosterID = "LastPosterID";

            ///<summary>最后发布者名称</summary>
            public const String LastPoster = "LastPoster";

            ///<summary>允许表情</summary>
            public const String AllowSmilies = "AllowSmilies";

            ///<summary>允许Rss</summary>
            public const String AllowRss = "AllowRss";

            ///<summary>允许HTML</summary>
            public const String AllowHtml = "AllowHtml";

            ///<summary>允许BB代码</summary>
            public const String AllowBbCode = "AllowBbCode";

            ///<summary>允许图片</summary>
            public const String AllowImgCode = "AllowImgCode";

            ///<summary>允许博客</summary>
            public const String AllowBlog = "AllowBlog";

            ///<summary>是否交易</summary>
            public const String IsTrade = "IsTrade";

            ///<summary>发表特殊主题</summary>
            public const String AllowPostSpecial = "AllowPostSpecial";

            ///<summary>只允许专题</summary>
            public const String AllowSpecialOnly = "AllowSpecialOnly";

            ///<summary>允许编辑规则</summary>
            public const String AllowEditRules = "AllowEditRules";

            ///<summary>允许缩略图</summary>
            public const String AllowThumbnail = "AllowThumbnail";

            ///<summary>允许标签</summary>
            public const String AllowTag = "AllowTag";

            ///<summary>回收站</summary>
            public const String Recyclebin = "Recyclebin";

            ///<summary>新帖子模式</summary>
            public const String Modnewposts = "Modnewposts";

            ///<summary>新主题模式</summary>
            public const String Modnewtopics = "Modnewtopics";

            ///<summary>干扰机</summary>
            public const String Jammer = "Jammer";

            ///<summary>关闭水印</summary>
            public const String DisableWatermark = "DisableWatermark";

            ///<summary>继承模式</summary>
            public const String Inheritedmod = "Inheritedmod";

            ///<summary>自动关闭</summary>
            public const String AutoClose = "AutoClose";

        }
        #endregion
    }

    /// <summary>论坛接口</summary>
    public partial interface IXForum
    {
        #region 属性
        /// <summary>编号</summary>
        Int32 ID { get; set; }

        /// <summary>父分类</summary>
        Int32 ParentID { get; set; }

        /// <summary>名称</summary>
        String Name { get; set; }

        /// <summary>状态。0隐藏，1显示</summary>
        Int32 Status { get; set; }

        /// <summary>列数</summary>
        Int32 ColCount { get; set; }

        /// <summary>显示顺序</summary>
        Int32 DisplayOrder { get; set; }

        /// <summary>模板Id</summary>
        Int32 TemplateID { get; set; }

        /// <summary>主题数</summary>
        Int32 Topics { get; set; }

        /// <summary>当前主题数</summary>
        Int32 CurTopics { get; set; }

        /// <summary>帖子数</summary>
        Int32 Posts { get; set; }

        /// <summary>今天发帖数</summary>
        Int32 TodayPosts { get; set; }

        /// <summary>最后主题</summary>
        Int32 LastTID { get; set; }

        /// <summary>最后主题标题</summary>
        String LastTitle { get; set; }

        /// <summary>最后发布时间</summary>
        DateTime LastPost { get; set; }

        /// <summary>最后发布者编号</summary>
        Int32 LastPosterID { get; set; }

        /// <summary>最后发布者名称</summary>
        String LastPoster { get; set; }

        /// <summary>允许表情</summary>
        Boolean AllowSmilies { get; set; }

        /// <summary>允许Rss</summary>
        Boolean AllowRss { get; set; }

        /// <summary>允许HTML</summary>
        Boolean AllowHtml { get; set; }

        /// <summary>允许BB代码</summary>
        Boolean AllowBbCode { get; set; }

        /// <summary>允许图片</summary>
        Boolean AllowImgCode { get; set; }

        /// <summary>允许博客</summary>
        Boolean AllowBlog { get; set; }

        /// <summary>是否交易</summary>
        Boolean IsTrade { get; set; }

        /// <summary>发表特殊主题</summary>
        Int32 AllowPostSpecial { get; set; }

        /// <summary>只允许专题</summary>
        Boolean AllowSpecialOnly { get; set; }

        /// <summary>允许编辑规则</summary>
        Boolean AllowEditRules { get; set; }

        /// <summary>允许缩略图</summary>
        Boolean AllowThumbnail { get; set; }

        /// <summary>允许标签</summary>
        Boolean AllowTag { get; set; }

        /// <summary>回收站</summary>
        Int32 Recyclebin { get; set; }

        /// <summary>新帖子模式</summary>
        Int32 Modnewposts { get; set; }

        /// <summary>新主题模式</summary>
        Int32 Modnewtopics { get; set; }

        /// <summary>干扰机</summary>
        Int32 Jammer { get; set; }

        /// <summary>关闭水印</summary>
        Boolean DisableWatermark { get; set; }

        /// <summary>继承模式</summary>
        Int32 Inheritedmod { get; set; }

        /// <summary>自动关闭</summary>
        Int32 AutoClose { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}