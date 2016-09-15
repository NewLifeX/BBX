﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace BBX.Entity
{
    /// <summary>模板</summary>
    [Serializable]
    [DataObject]
    [Description("模板")]
    [BindTable("Template", Description = "模板", ConnName = "BBX", DbType = DatabaseType.SqlServer)]
    public partial class Template : ITemplate
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

        private String _Directory;
        /// <summary>目录</summary>
        [DisplayName("目录")]
        [Description("目录")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(2, "Directory", "目录", null, "nvarchar(50)", 0, 0, true)]
        public virtual String Directory
        {
            get { return _Directory; }
            set { if (OnPropertyChanging(__.Directory, value)) { _Directory = value; OnPropertyChanged(__.Directory); } }
        }

        private String _Name;
        /// <summary>名称</summary>
        [DisplayName("名称")]
        [Description("名称")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(3, "Name", "名称", null, "nvarchar(50)", 0, 0, true, Master=true)]
        public virtual String Name
        {
            get { return _Name; }
            set { if (OnPropertyChanging(__.Name, value)) { _Name = value; OnPropertyChanged(__.Name); } }
        }

        private String _Author;
        /// <summary>授权人</summary>
        [DisplayName("授权人")]
        [Description("授权人")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(4, "Author", "授权人", null, "nvarchar(50)", 0, 0, true)]
        public virtual String Author
        {
            get { return _Author; }
            set { if (OnPropertyChanging(__.Author, value)) { _Author = value; OnPropertyChanged(__.Author); } }
        }

        private String _Createdate;
        /// <summary>创建时间</summary>
        [DisplayName("创建时间")]
        [Description("创建时间")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(5, "Createdate", "创建时间", null, "nvarchar(50)", 0, 0, true)]
        public virtual String Createdate
        {
            get { return _Createdate; }
            set { if (OnPropertyChanging(__.Createdate, value)) { _Createdate = value; OnPropertyChanged(__.Createdate); } }
        }

        private String _Ver;
        /// <summary>版本</summary>
        [DisplayName("版本")]
        [Description("版本")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(6, "Ver", "版本", null, "nvarchar(50)", 0, 0, true)]
        public virtual String Ver
        {
            get { return _Ver; }
            set { if (OnPropertyChanging(__.Ver, value)) { _Ver = value; OnPropertyChanged(__.Ver); } }
        }

        private String _Fordntver;
        /// <summary>版本</summary>
        [DisplayName("版本")]
        [Description("版本")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(7, "Fordntver", "版本", null, "nvarchar(50)", 0, 0, true)]
        public virtual String Fordntver
        {
            get { return _Fordntver; }
            set { if (OnPropertyChanging(__.Fordntver, value)) { _Fordntver = value; OnPropertyChanged(__.Fordntver); } }
        }

        private String _Copyright;
        /// <summary>版权信息</summary>
        [DisplayName("版权信息")]
        [Description("版权信息")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(8, "Copyright", "版权信息", null, "nvarchar(50)", 0, 0, true)]
        public virtual String Copyright
        {
            get { return _Copyright; }
            set { if (OnPropertyChanging(__.Copyright, value)) { _Copyright = value; OnPropertyChanged(__.Copyright); } }
        }

        private String _Url;
        /// <summary>地址</summary>
        [DisplayName("地址")]
        [Description("地址")]
        [DataObjectField(false, false, true, 100)]
        [BindColumn(9, "Url", "地址", null, "nvarchar(100)", 0, 0, true)]
        public virtual String Url
        {
            get { return _Url; }
            set { if (OnPropertyChanging(__.Url, value)) { _Url = value; OnPropertyChanged(__.Url); } }
        }

        private Int32 _Width;
        /// <summary>宽度</summary>
        [DisplayName("宽度")]
        [Description("宽度")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(10, "Width", "宽度", null, "int", 10, 0, false)]
        public virtual Int32 Width
        {
            get { return _Width; }
            set { if (OnPropertyChanging(__.Width, value)) { _Width = value; OnPropertyChanged(__.Width); } }
        }

        private Boolean _Enable;
        /// <summary>是否有效</summary>
        [DisplayName("是否有效")]
        [Description("是否有效")]
        [DataObjectField(false, false, true, 100)]
        [BindColumn(11, "Enable", "是否有效", null, "bit", 0, 0, false)]
        public virtual Boolean Enable
        {
            get { return _Enable; }
            set { if (OnPropertyChanging(__.Enable, value)) { _Enable = value; OnPropertyChanged(__.Enable); } }
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
                    case __.Directory : return _Directory;
                    case __.Name : return _Name;
                    case __.Author : return _Author;
                    case __.Createdate : return _Createdate;
                    case __.Ver : return _Ver;
                    case __.Fordntver : return _Fordntver;
                    case __.Copyright : return _Copyright;
                    case __.Url : return _Url;
                    case __.Width : return _Width;
                    case __.Enable : return _Enable;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.ID : _ID = Convert.ToInt32(value); break;
                    case __.Directory : _Directory = Convert.ToString(value); break;
                    case __.Name : _Name = Convert.ToString(value); break;
                    case __.Author : _Author = Convert.ToString(value); break;
                    case __.Createdate : _Createdate = Convert.ToString(value); break;
                    case __.Ver : _Ver = Convert.ToString(value); break;
                    case __.Fordntver : _Fordntver = Convert.ToString(value); break;
                    case __.Copyright : _Copyright = Convert.ToString(value); break;
                    case __.Url : _Url = Convert.ToString(value); break;
                    case __.Width : _Width = Convert.ToInt32(value); break;
                    case __.Enable : _Enable = Convert.ToBoolean(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得模板字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号</summary>
            public static readonly Field ID = FindByName(__.ID);

            ///<summary>目录</summary>
            public static readonly Field Directory = FindByName(__.Directory);

            ///<summary>名称</summary>
            public static readonly Field Name = FindByName(__.Name);

            ///<summary>授权人</summary>
            public static readonly Field Author = FindByName(__.Author);

            ///<summary>创建时间</summary>
            public static readonly Field Createdate = FindByName(__.Createdate);

            ///<summary>版本</summary>
            public static readonly Field Ver = FindByName(__.Ver);

            ///<summary>版本</summary>
            public static readonly Field Fordntver = FindByName(__.Fordntver);

            ///<summary>版权信息</summary>
            public static readonly Field Copyright = FindByName(__.Copyright);

            ///<summary>地址</summary>
            public static readonly Field Url = FindByName(__.Url);

            ///<summary>宽度</summary>
            public static readonly Field Width = FindByName(__.Width);

            ///<summary>是否有效</summary>
            public static readonly Field Enable = FindByName(__.Enable);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得模板字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号</summary>
            public const String ID = "ID";

            ///<summary>目录</summary>
            public const String Directory = "Directory";

            ///<summary>名称</summary>
            public const String Name = "Name";

            ///<summary>授权人</summary>
            public const String Author = "Author";

            ///<summary>创建时间</summary>
            public const String Createdate = "Createdate";

            ///<summary>版本</summary>
            public const String Ver = "Ver";

            ///<summary>版本</summary>
            public const String Fordntver = "Fordntver";

            ///<summary>版权信息</summary>
            public const String Copyright = "Copyright";

            ///<summary>地址</summary>
            public const String Url = "Url";

            ///<summary>宽度</summary>
            public const String Width = "Width";

            ///<summary>是否有效</summary>
            public const String Enable = "Enable";

        }
        #endregion
    }

    /// <summary>模板接口</summary>
    public partial interface ITemplate
    {
        #region 属性
        /// <summary>编号</summary>
        Int32 ID { get; set; }

        /// <summary>目录</summary>
        String Directory { get; set; }

        /// <summary>名称</summary>
        String Name { get; set; }

        /// <summary>授权人</summary>
        String Author { get; set; }

        /// <summary>创建时间</summary>
        String Createdate { get; set; }

        /// <summary>版本</summary>
        String Ver { get; set; }

        /// <summary>版本</summary>
        String Fordntver { get; set; }

        /// <summary>版权信息</summary>
        String Copyright { get; set; }

        /// <summary>地址</summary>
        String Url { get; set; }

        /// <summary>宽度</summary>
        Int32 Width { get; set; }

        /// <summary>是否有效</summary>
        Boolean Enable { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}