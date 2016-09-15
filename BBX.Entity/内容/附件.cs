﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace BBX.Entity
{
    /// <summary>附件</summary>
    [Serializable]
    [DataObject]
    [Description("附件")]
    [BindIndex("IX_Attachment_pid", false, "pid")]
    [BindIndex("IX_Attachment_tid", false, "tid")]
    [BindIndex("IX_Attachment_uid", false, "uid")]
    [BindTable("Attachment", Description = "附件", ConnName = "BBX", DbType = DatabaseType.SqlServer)]
    public partial class Attachment : IAttachment
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

        private Int32 _Uid;
        /// <summary>用户编号</summary>
        [DisplayName("用户编号")]
        [Description("用户编号")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(2, "Uid", "用户编号", null, "int", 10, 0, false)]
        public virtual Int32 Uid
        {
            get { return _Uid; }
            set { if (OnPropertyChanging(__.Uid, value)) { _Uid = value; OnPropertyChanged(__.Uid); } }
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

        private Int32 _Pid;
        /// <summary>帖子编号</summary>
        [DisplayName("帖子编号")]
        [Description("帖子编号")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(4, "Pid", "帖子编号", null, "int", 10, 0, false)]
        public virtual Int32 Pid
        {
            get { return _Pid; }
            set { if (OnPropertyChanging(__.Pid, value)) { _Pid = value; OnPropertyChanged(__.Pid); } }
        }

        private DateTime _PostDateTime;
        /// <summary>发送时间</summary>
        [DisplayName("发送时间")]
        [Description("发送时间")]
        [DataObjectField(false, false, true, 3)]
        [BindColumn(5, "PostDateTime", "发送时间", null, "datetime", 3, 0, false)]
        public virtual DateTime PostDateTime
        {
            get { return _PostDateTime; }
            set { if (OnPropertyChanging(__.PostDateTime, value)) { _PostDateTime = value; OnPropertyChanged(__.PostDateTime); } }
        }

        private Int32 _ReadPerm;
        /// <summary>阅读权限</summary>
        [DisplayName("阅读权限")]
        [Description("阅读权限")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(6, "ReadPerm", "阅读权限", null, "int", 10, 0, false)]
        public virtual Int32 ReadPerm
        {
            get { return _ReadPerm; }
            set { if (OnPropertyChanging(__.ReadPerm, value)) { _ReadPerm = value; OnPropertyChanged(__.ReadPerm); } }
        }

        private String _FileName;
        /// <summary>Word文件名</summary>
        [DisplayName("Word文件名")]
        [Description("Word文件名")]
        [DataObjectField(false, false, true, 100)]
        [BindColumn(7, "FileName", "Word文件名", null, "nvarchar(100)", 0, 0, true)]
        public virtual String FileName
        {
            get { return _FileName; }
            set { if (OnPropertyChanging(__.FileName, value)) { _FileName = value; OnPropertyChanged(__.FileName); } }
        }

        private String _Description;
        /// <summary>描述</summary>
        [DisplayName("描述")]
        [Description("描述")]
        [DataObjectField(false, false, true, 100)]
        [BindColumn(8, "Description", "描述", null, "nvarchar(100)", 0, 0, true)]
        public virtual String Description
        {
            get { return _Description; }
            set { if (OnPropertyChanging(__.Description, value)) { _Description = value; OnPropertyChanged(__.Description); } }
        }

        private String _FileType;
        /// <summary>生成文件扩展名</summary>
        [DisplayName("生成文件扩展名")]
        [Description("生成文件扩展名")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(9, "FileType", "生成文件扩展名", null, "nvarchar(50)", 0, 0, true)]
        public virtual String FileType
        {
            get { return _FileType; }
            set { if (OnPropertyChanging(__.FileType, value)) { _FileType = value; OnPropertyChanged(__.FileType); } }
        }

        private Int32 _FileSize;
        /// <summary>文件大小</summary>
        [DisplayName("文件大小")]
        [Description("文件大小")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(10, "FileSize", "文件大小", null, "int", 10, 0, false)]
        public virtual Int32 FileSize
        {
            get { return _FileSize; }
            set { if (OnPropertyChanging(__.FileSize, value)) { _FileSize = value; OnPropertyChanged(__.FileSize); } }
        }

        private String _Name;
        /// <summary>名称</summary>
        [DisplayName("名称")]
        [Description("名称")]
        [DataObjectField(false, false, true, 255)]
        [BindColumn(11, "Name", "名称", null, "nvarchar(255)", 0, 0, true, Master=true)]
        public virtual String Name
        {
            get { return _Name; }
            set { if (OnPropertyChanging(__.Name, value)) { _Name = value; OnPropertyChanged(__.Name); } }
        }

        private Int32 _Downloads;
        /// <summary>下载次数</summary>
        [DisplayName("下载次数")]
        [Description("下载次数")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(12, "Downloads", "下载次数", null, "int", 10, 0, false)]
        public virtual Int32 Downloads
        {
            get { return _Downloads; }
            set { if (OnPropertyChanging(__.Downloads, value)) { _Downloads = value; OnPropertyChanged(__.Downloads); } }
        }

        private Int32 _Width;
        /// <summary>广告宽度</summary>
        [DisplayName("广告宽度")]
        [Description("广告宽度")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(13, "Width", "广告宽度", null, "int", 10, 0, false)]
        public virtual Int32 Width
        {
            get { return _Width; }
            set { if (OnPropertyChanging(__.Width, value)) { _Width = value; OnPropertyChanged(__.Width); } }
        }

        private Int32 _Height;
        /// <summary>广告高度</summary>
        [DisplayName("广告高度")]
        [Description("广告高度")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(14, "Height", "广告高度", null, "int", 10, 0, false)]
        public virtual Int32 Height
        {
            get { return _Height; }
            set { if (OnPropertyChanging(__.Height, value)) { _Height = value; OnPropertyChanged(__.Height); } }
        }

        private Int32 _AttachPrice;
        /// <summary>售价</summary>
        [DisplayName("售价")]
        [Description("售价")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(15, "AttachPrice", "售价", null, "int", 10, 0, false)]
        public virtual Int32 AttachPrice
        {
            get { return _AttachPrice; }
            set { if (OnPropertyChanging(__.AttachPrice, value)) { _AttachPrice = value; OnPropertyChanged(__.AttachPrice); } }
        }

        private Boolean _IsImage;
        /// <summary>1—为有缩略图；0—为没有缩略图</summary>
        [DisplayName("1—为有缩略图；0—为没有缩略图")]
        [Description("1—为有缩略图；0—为没有缩略图")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(16, "IsImage", "1—为有缩略图；0—为没有缩略图", null, "bit", 0, 0, false)]
        public virtual Boolean IsImage
        {
            get { return _IsImage; }
            set { if (OnPropertyChanging(__.IsImage, value)) { _IsImage = value; OnPropertyChanged(__.IsImage); } }
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
                    case __.Uid : return _Uid;
                    case __.Tid : return _Tid;
                    case __.Pid : return _Pid;
                    case __.PostDateTime : return _PostDateTime;
                    case __.ReadPerm : return _ReadPerm;
                    case __.FileName : return _FileName;
                    case __.Description : return _Description;
                    case __.FileType : return _FileType;
                    case __.FileSize : return _FileSize;
                    case __.Name : return _Name;
                    case __.Downloads : return _Downloads;
                    case __.Width : return _Width;
                    case __.Height : return _Height;
                    case __.AttachPrice : return _AttachPrice;
                    case __.IsImage : return _IsImage;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.ID : _ID = Convert.ToInt32(value); break;
                    case __.Uid : _Uid = Convert.ToInt32(value); break;
                    case __.Tid : _Tid = Convert.ToInt32(value); break;
                    case __.Pid : _Pid = Convert.ToInt32(value); break;
                    case __.PostDateTime : _PostDateTime = Convert.ToDateTime(value); break;
                    case __.ReadPerm : _ReadPerm = Convert.ToInt32(value); break;
                    case __.FileName : _FileName = Convert.ToString(value); break;
                    case __.Description : _Description = Convert.ToString(value); break;
                    case __.FileType : _FileType = Convert.ToString(value); break;
                    case __.FileSize : _FileSize = Convert.ToInt32(value); break;
                    case __.Name : _Name = Convert.ToString(value); break;
                    case __.Downloads : _Downloads = Convert.ToInt32(value); break;
                    case __.Width : _Width = Convert.ToInt32(value); break;
                    case __.Height : _Height = Convert.ToInt32(value); break;
                    case __.AttachPrice : _AttachPrice = Convert.ToInt32(value); break;
                    case __.IsImage : _IsImage = Convert.ToBoolean(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得附件字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号</summary>
            public static readonly Field ID = FindByName(__.ID);

            ///<summary>用户编号</summary>
            public static readonly Field Uid = FindByName(__.Uid);

            ///<summary>主题编号</summary>
            public static readonly Field Tid = FindByName(__.Tid);

            ///<summary>帖子编号</summary>
            public static readonly Field Pid = FindByName(__.Pid);

            ///<summary>发送时间</summary>
            public static readonly Field PostDateTime = FindByName(__.PostDateTime);

            ///<summary>阅读权限</summary>
            public static readonly Field ReadPerm = FindByName(__.ReadPerm);

            ///<summary>Word文件名</summary>
            public static readonly Field FileName = FindByName(__.FileName);

            ///<summary>描述</summary>
            public static readonly Field Description = FindByName(__.Description);

            ///<summary>生成文件扩展名</summary>
            public static readonly Field FileType = FindByName(__.FileType);

            ///<summary>文件大小</summary>
            public static readonly Field FileSize = FindByName(__.FileSize);

            ///<summary>名称</summary>
            public static readonly Field Name = FindByName(__.Name);

            ///<summary>下载次数</summary>
            public static readonly Field Downloads = FindByName(__.Downloads);

            ///<summary>广告宽度</summary>
            public static readonly Field Width = FindByName(__.Width);

            ///<summary>广告高度</summary>
            public static readonly Field Height = FindByName(__.Height);

            ///<summary>售价</summary>
            public static readonly Field AttachPrice = FindByName(__.AttachPrice);

            ///<summary>1—为有缩略图；0—为没有缩略图</summary>
            public static readonly Field IsImage = FindByName(__.IsImage);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得附件字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号</summary>
            public const String ID = "ID";

            ///<summary>用户编号</summary>
            public const String Uid = "Uid";

            ///<summary>主题编号</summary>
            public const String Tid = "Tid";

            ///<summary>帖子编号</summary>
            public const String Pid = "Pid";

            ///<summary>发送时间</summary>
            public const String PostDateTime = "PostDateTime";

            ///<summary>阅读权限</summary>
            public const String ReadPerm = "ReadPerm";

            ///<summary>Word文件名</summary>
            public const String FileName = "FileName";

            ///<summary>描述</summary>
            public const String Description = "Description";

            ///<summary>生成文件扩展名</summary>
            public const String FileType = "FileType";

            ///<summary>文件大小</summary>
            public const String FileSize = "FileSize";

            ///<summary>名称</summary>
            public const String Name = "Name";

            ///<summary>下载次数</summary>
            public const String Downloads = "Downloads";

            ///<summary>广告宽度</summary>
            public const String Width = "Width";

            ///<summary>广告高度</summary>
            public const String Height = "Height";

            ///<summary>售价</summary>
            public const String AttachPrice = "AttachPrice";

            ///<summary>1—为有缩略图；0—为没有缩略图</summary>
            public const String IsImage = "IsImage";

        }
        #endregion
    }

    /// <summary>附件接口</summary>
    public partial interface IAttachment
    {
        #region 属性
        /// <summary>编号</summary>
        Int32 ID { get; set; }

        /// <summary>用户编号</summary>
        Int32 Uid { get; set; }

        /// <summary>主题编号</summary>
        Int32 Tid { get; set; }

        /// <summary>帖子编号</summary>
        Int32 Pid { get; set; }

        /// <summary>发送时间</summary>
        DateTime PostDateTime { get; set; }

        /// <summary>阅读权限</summary>
        Int32 ReadPerm { get; set; }

        /// <summary>Word文件名</summary>
        String FileName { get; set; }

        /// <summary>描述</summary>
        String Description { get; set; }

        /// <summary>生成文件扩展名</summary>
        String FileType { get; set; }

        /// <summary>文件大小</summary>
        Int32 FileSize { get; set; }

        /// <summary>名称</summary>
        String Name { get; set; }

        /// <summary>下载次数</summary>
        Int32 Downloads { get; set; }

        /// <summary>广告宽度</summary>
        Int32 Width { get; set; }

        /// <summary>广告高度</summary>
        Int32 Height { get; set; }

        /// <summary>售价</summary>
        Int32 AttachPrice { get; set; }

        /// <summary>1—为有缩略图；0—为没有缩略图</summary>
        Boolean IsImage { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}