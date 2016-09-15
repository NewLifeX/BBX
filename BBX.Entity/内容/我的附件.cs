﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace BBX.Entity
{
    /// <summary>我的附件</summary>
    [Serializable]
    [DataObject]
    [Description("我的附件")]
    [BindIndex("IX_MyAttachment_uid_extname", false, "uid,extname")]
    [BindTable("MyAttachment", Description = "我的附件", ConnName = "BBX", DbType = DatabaseType.SqlServer)]
    public partial class MyAttachment : IMyAttachment
    {
        #region 属性
        private Int32 _ID;
        /// <summary>附件编号</summary>
        [DisplayName("附件编号")]
        [Description("附件编号")]
        [DataObjectField(true, false, false, 10)]
        [BindColumn(1, "ID", "附件编号", null, "int", 10, 0, false)]
        public virtual Int32 ID
        {
            get { return _ID; }
            set { if (OnPropertyChanging(__.ID, value)) { _ID = value; OnPropertyChanged(__.ID); } }
        }

        private Int32 _Uid;
        /// <summary>用户编号</summary>
        [DisplayName("用户编号")]
        [Description("用户编号")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn(2, "Uid", "用户编号", null, "int", 10, 0, false)]
        public virtual Int32 Uid
        {
            get { return _Uid; }
            set { if (OnPropertyChanging(__.Uid, value)) { _Uid = value; OnPropertyChanged(__.Uid); } }
        }

        private String _Name;
        /// <summary>名称</summary>
        [DisplayName("名称")]
        [Description("名称")]
        [DataObjectField(false, false, true, 100)]
        [BindColumn(3, "Name", "名称", null, "nvarchar(100)", 0, 0, true, Master=true)]
        public virtual String Name
        {
            get { return _Name; }
            set { if (OnPropertyChanging(__.Name, value)) { _Name = value; OnPropertyChanged(__.Name); } }
        }

        private String _Description;
        /// <summary>描述</summary>
        [DisplayName("描述")]
        [Description("描述")]
        [DataObjectField(false, false, false, 100)]
        [BindColumn(4, "Description", "描述", null, "nvarchar(100)", 0, 0, true)]
        public virtual String Description
        {
            get { return _Description; }
            set { if (OnPropertyChanging(__.Description, value)) { _Description = value; OnPropertyChanged(__.Description); } }
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

        private Int32 _Downloads;
        /// <summary>下载次数</summary>
        [DisplayName("下载次数")]
        [Description("下载次数")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(6, "Downloads", "下载次数", null, "int", 10, 0, false)]
        public virtual Int32 Downloads
        {
            get { return _Downloads; }
            set { if (OnPropertyChanging(__.Downloads, value)) { _Downloads = value; OnPropertyChanged(__.Downloads); } }
        }

        private String _FileName;
        /// <summary>文件名</summary>
        [DisplayName("文件名")]
        [Description("文件名")]
        [DataObjectField(false, false, false, 100)]
        [BindColumn(7, "FileName", "文件名", null, "nvarchar(100)", 0, 0, true)]
        public virtual String FileName
        {
            get { return _FileName; }
            set { if (OnPropertyChanging(__.FileName, value)) { _FileName = value; OnPropertyChanged(__.FileName); } }
        }

        private Int32 _Pid;
        /// <summary>帖子编号</summary>
        [DisplayName("帖子编号")]
        [Description("帖子编号")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(8, "Pid", "帖子编号", null, "int", 10, 0, false)]
        public virtual Int32 Pid
        {
            get { return _Pid; }
            set { if (OnPropertyChanging(__.Pid, value)) { _Pid = value; OnPropertyChanged(__.Pid); } }
        }

        private Int32 _Tid;
        /// <summary>主题编号</summary>
        [DisplayName("主题编号")]
        [Description("主题编号")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(9, "Tid", "主题编号", null, "int", 10, 0, false)]
        public virtual Int32 Tid
        {
            get { return _Tid; }
            set { if (OnPropertyChanging(__.Tid, value)) { _Tid = value; OnPropertyChanged(__.Tid); } }
        }

        private String _Extname;
        /// <summary>扩展名</summary>
        [DisplayName("扩展名")]
        [Description("扩展名")]
        [DataObjectField(false, false, false, 50)]
        [BindColumn(10, "Extname", "扩展名", null, "nvarchar(50)", 0, 0, true)]
        public virtual String Extname
        {
            get { return _Extname; }
            set { if (OnPropertyChanging(__.Extname, value)) { _Extname = value; OnPropertyChanged(__.Extname); } }
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
                    case __.Name : return _Name;
                    case __.Description : return _Description;
                    case __.PostDateTime : return _PostDateTime;
                    case __.Downloads : return _Downloads;
                    case __.FileName : return _FileName;
                    case __.Pid : return _Pid;
                    case __.Tid : return _Tid;
                    case __.Extname : return _Extname;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.ID : _ID = Convert.ToInt32(value); break;
                    case __.Uid : _Uid = Convert.ToInt32(value); break;
                    case __.Name : _Name = Convert.ToString(value); break;
                    case __.Description : _Description = Convert.ToString(value); break;
                    case __.PostDateTime : _PostDateTime = Convert.ToDateTime(value); break;
                    case __.Downloads : _Downloads = Convert.ToInt32(value); break;
                    case __.FileName : _FileName = Convert.ToString(value); break;
                    case __.Pid : _Pid = Convert.ToInt32(value); break;
                    case __.Tid : _Tid = Convert.ToInt32(value); break;
                    case __.Extname : _Extname = Convert.ToString(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得我的附件字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>附件编号</summary>
            public static readonly Field ID = FindByName(__.ID);

            ///<summary>用户编号</summary>
            public static readonly Field Uid = FindByName(__.Uid);

            ///<summary>名称</summary>
            public static readonly Field Name = FindByName(__.Name);

            ///<summary>描述</summary>
            public static readonly Field Description = FindByName(__.Description);

            ///<summary>发送时间</summary>
            public static readonly Field PostDateTime = FindByName(__.PostDateTime);

            ///<summary>下载次数</summary>
            public static readonly Field Downloads = FindByName(__.Downloads);

            ///<summary>文件名</summary>
            public static readonly Field FileName = FindByName(__.FileName);

            ///<summary>帖子编号</summary>
            public static readonly Field Pid = FindByName(__.Pid);

            ///<summary>主题编号</summary>
            public static readonly Field Tid = FindByName(__.Tid);

            ///<summary>扩展名</summary>
            public static readonly Field Extname = FindByName(__.Extname);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得我的附件字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>附件编号</summary>
            public const String ID = "ID";

            ///<summary>用户编号</summary>
            public const String Uid = "Uid";

            ///<summary>名称</summary>
            public const String Name = "Name";

            ///<summary>描述</summary>
            public const String Description = "Description";

            ///<summary>发送时间</summary>
            public const String PostDateTime = "PostDateTime";

            ///<summary>下载次数</summary>
            public const String Downloads = "Downloads";

            ///<summary>文件名</summary>
            public const String FileName = "FileName";

            ///<summary>帖子编号</summary>
            public const String Pid = "Pid";

            ///<summary>主题编号</summary>
            public const String Tid = "Tid";

            ///<summary>扩展名</summary>
            public const String Extname = "Extname";

        }
        #endregion
    }

    /// <summary>我的附件接口</summary>
    public partial interface IMyAttachment
    {
        #region 属性
        /// <summary>附件编号</summary>
        Int32 ID { get; set; }

        /// <summary>用户编号</summary>
        Int32 Uid { get; set; }

        /// <summary>名称</summary>
        String Name { get; set; }

        /// <summary>描述</summary>
        String Description { get; set; }

        /// <summary>发送时间</summary>
        DateTime PostDateTime { get; set; }

        /// <summary>下载次数</summary>
        Int32 Downloads { get; set; }

        /// <summary>文件名</summary>
        String FileName { get; set; }

        /// <summary>帖子编号</summary>
        Int32 Pid { get; set; }

        /// <summary>主题编号</summary>
        Int32 Tid { get; set; }

        /// <summary>扩展名</summary>
        String Extname { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}