﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace BBX.Entity
{
    /// <summary>帮助</summary>
    [Serializable]
    [DataObject]
    [Description("帮助")]
    [BindTable("Help", Description = "帮助", ConnName = "BBX", DbType = DatabaseType.SqlServer)]
    public partial class Help : IHelp
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

        private String _Title;
        /// <summary>事项名称</summary>
        [DisplayName("事项名称")]
        [Description("事项名称")]
        [DataObjectField(false, false, false, 100)]
        [BindColumn(2, "Title", "事项名称", null, "nvarchar(100)", 0, 0, true, Master=true)]
        public virtual String Title
        {
            get { return _Title; }
            set { if (OnPropertyChanging(__.Title, value)) { _Title = value; OnPropertyChanged(__.Title); } }
        }

        private String _Message;
        /// <summary>短消息内容</summary>
        [DisplayName("短消息内容")]
        [Description("短消息内容")]
        [DataObjectField(false, false, true, -1)]
        [BindColumn(3, "Message", "短消息内容", null, "ntext", 0, 0, true)]
        public virtual String Message
        {
            get { return _Message; }
            set { if (OnPropertyChanging(__.Message, value)) { _Message = value; OnPropertyChanged(__.Message); } }
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

        private Int32 _Orderby;
        /// <summary>排序</summary>
        [DisplayName("排序")]
        [Description("排序")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(5, "Orderby", "排序", null, "int", 10, 0, false)]
        public virtual Int32 Orderby
        {
            get { return _Orderby; }
            set { if (OnPropertyChanging(__.Orderby, value)) { _Orderby = value; OnPropertyChanged(__.Orderby); } }
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
                    case __.Title : return _Title;
                    case __.Message : return _Message;
                    case __.Pid : return _Pid;
                    case __.Orderby : return _Orderby;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.ID : _ID = Convert.ToInt32(value); break;
                    case __.Title : _Title = Convert.ToString(value); break;
                    case __.Message : _Message = Convert.ToString(value); break;
                    case __.Pid : _Pid = Convert.ToInt32(value); break;
                    case __.Orderby : _Orderby = Convert.ToInt32(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得帮助字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号</summary>
            public static readonly Field ID = FindByName(__.ID);

            ///<summary>事项名称</summary>
            public static readonly Field Title = FindByName(__.Title);

            ///<summary>短消息内容</summary>
            public static readonly Field Message = FindByName(__.Message);

            ///<summary>帖子编号</summary>
            public static readonly Field Pid = FindByName(__.Pid);

            ///<summary>排序</summary>
            public static readonly Field Orderby = FindByName(__.Orderby);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得帮助字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号</summary>
            public const String ID = "ID";

            ///<summary>事项名称</summary>
            public const String Title = "Title";

            ///<summary>短消息内容</summary>
            public const String Message = "Message";

            ///<summary>帖子编号</summary>
            public const String Pid = "Pid";

            ///<summary>排序</summary>
            public const String Orderby = "Orderby";

        }
        #endregion
    }

    /// <summary>帮助接口</summary>
    public partial interface IHelp
    {
        #region 属性
        /// <summary>编号</summary>
        Int32 ID { get; set; }

        /// <summary>事项名称</summary>
        String Title { get; set; }

        /// <summary>短消息内容</summary>
        String Message { get; set; }

        /// <summary>帖子编号</summary>
        Int32 Pid { get; set; }

        /// <summary>排序</summary>
        Int32 Orderby { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}