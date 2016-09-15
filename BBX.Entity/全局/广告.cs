﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace BBX.Entity
{
    /// <summary>广告</summary>
    [Serializable]
    [DataObject]
    [Description("广告")]
    [BindTable("Advertisement", Description = "广告", ConnName = "BBX", DbType = DatabaseType.SqlServer)]
    public partial class Advertisement : IAdvertisement
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

        private Int32 _Available;
        /// <summary>是否可用</summary>
        [DisplayName("是否可用")]
        [Description("是否可用")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(2, "Available", "是否可用", null, "int", 10, 0, false)]
        public virtual Int32 Available
        {
            get { return _Available; }
            set { if (OnPropertyChanging(__.Available, value)) { _Available = value; OnPropertyChanged(__.Available); } }
        }

        private String _Type;
        /// <summary>类型</summary>
        [DisplayName("类型")]
        [Description("类型")]
        [DataObjectField(false, false, false, 50)]
        [BindColumn(3, "Type", "类型", null, "nvarchar(50)", 0, 0, true)]
        public virtual String Type
        {
            get { return _Type; }
            set { if (OnPropertyChanging(__.Type, value)) { _Type = value; OnPropertyChanged(__.Type); } }
        }

        private Int32 _DisplayOrder;
        /// <summary>显示顺序</summary>
        [DisplayName("显示顺序")]
        [Description("显示顺序")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(4, "DisplayOrder", "显示顺序", null, "int", 10, 0, false)]
        public virtual Int32 DisplayOrder
        {
            get { return _DisplayOrder; }
            set { if (OnPropertyChanging(__.DisplayOrder, value)) { _DisplayOrder = value; OnPropertyChanged(__.DisplayOrder); } }
        }

        private String _Title;
        /// <summary>事项名称</summary>
        [DisplayName("事项名称")]
        [Description("事项名称")]
        [DataObjectField(false, false, false, 50)]
        [BindColumn(5, "Title", "事项名称", null, "nvarchar(50)", 0, 0, true, Master=true)]
        public virtual String Title
        {
            get { return _Title; }
            set { if (OnPropertyChanging(__.Title, value)) { _Title = value; OnPropertyChanged(__.Title); } }
        }

        private String _Targets;
        /// <summary>目标</summary>
        [DisplayName("目标")]
        [Description("目标")]
        [DataObjectField(false, false, false, 255)]
        [BindColumn(6, "Targets", "目标", null, "nvarchar(255)", 0, 0, true)]
        public virtual String Targets
        {
            get { return _Targets; }
            set { if (OnPropertyChanging(__.Targets, value)) { _Targets = value; OnPropertyChanged(__.Targets); } }
        }

        private DateTime _StartTime;
        /// <summary>政策文件生效日期</summary>
        [DisplayName("政策文件生效日期")]
        [Description("政策文件生效日期")]
        [DataObjectField(false, false, false, 3)]
        [BindColumn(7, "StartTime", "政策文件生效日期", null, "datetime", 3, 0, false)]
        public virtual DateTime StartTime
        {
            get { return _StartTime; }
            set { if (OnPropertyChanging(__.StartTime, value)) { _StartTime = value; OnPropertyChanged(__.StartTime); } }
        }

        private DateTime _EndTime;
        /// <summary>过期时间</summary>
        [DisplayName("过期时间")]
        [Description("过期时间")]
        [DataObjectField(false, false, false, 3)]
        [BindColumn(8, "EndTime", "过期时间", null, "datetime", 3, 0, false)]
        public virtual DateTime EndTime
        {
            get { return _EndTime; }
            set { if (OnPropertyChanging(__.EndTime, value)) { _EndTime = value; OnPropertyChanged(__.EndTime); } }
        }

        private String _Code;
        /// <summary>代码</summary>
        [DisplayName("代码")]
        [Description("代码")]
        [DataObjectField(false, false, false, -1)]
        [BindColumn(9, "Code", "代码", null, "ntext", 0, 0, true)]
        public virtual String Code
        {
            get { return _Code; }
            set { if (OnPropertyChanging(__.Code, value)) { _Code = value; OnPropertyChanged(__.Code); } }
        }

        private String _Parameters;
        /// <summary>参数</summary>
        [DisplayName("参数")]
        [Description("参数")]
        [DataObjectField(false, false, false, -1)]
        [BindColumn(10, "Parameters", "参数", null, "ntext", 0, 0, true)]
        public virtual String Parameters
        {
            get { return _Parameters; }
            set { if (OnPropertyChanging(__.Parameters, value)) { _Parameters = value; OnPropertyChanged(__.Parameters); } }
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
                    case __.Available : return _Available;
                    case __.Type : return _Type;
                    case __.DisplayOrder : return _DisplayOrder;
                    case __.Title : return _Title;
                    case __.Targets : return _Targets;
                    case __.StartTime : return _StartTime;
                    case __.EndTime : return _EndTime;
                    case __.Code : return _Code;
                    case __.Parameters : return _Parameters;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.ID : _ID = Convert.ToInt32(value); break;
                    case __.Available : _Available = Convert.ToInt32(value); break;
                    case __.Type : _Type = Convert.ToString(value); break;
                    case __.DisplayOrder : _DisplayOrder = Convert.ToInt32(value); break;
                    case __.Title : _Title = Convert.ToString(value); break;
                    case __.Targets : _Targets = Convert.ToString(value); break;
                    case __.StartTime : _StartTime = Convert.ToDateTime(value); break;
                    case __.EndTime : _EndTime = Convert.ToDateTime(value); break;
                    case __.Code : _Code = Convert.ToString(value); break;
                    case __.Parameters : _Parameters = Convert.ToString(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得广告字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号</summary>
            public static readonly Field ID = FindByName(__.ID);

            ///<summary>是否可用</summary>
            public static readonly Field Available = FindByName(__.Available);

            ///<summary>类型</summary>
            public static readonly Field Type = FindByName(__.Type);

            ///<summary>显示顺序</summary>
            public static readonly Field DisplayOrder = FindByName(__.DisplayOrder);

            ///<summary>事项名称</summary>
            public static readonly Field Title = FindByName(__.Title);

            ///<summary>目标</summary>
            public static readonly Field Targets = FindByName(__.Targets);

            ///<summary>政策文件生效日期</summary>
            public static readonly Field StartTime = FindByName(__.StartTime);

            ///<summary>过期时间</summary>
            public static readonly Field EndTime = FindByName(__.EndTime);

            ///<summary>代码</summary>
            public static readonly Field Code = FindByName(__.Code);

            ///<summary>参数</summary>
            public static readonly Field Parameters = FindByName(__.Parameters);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得广告字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号</summary>
            public const String ID = "ID";

            ///<summary>是否可用</summary>
            public const String Available = "Available";

            ///<summary>类型</summary>
            public const String Type = "Type";

            ///<summary>显示顺序</summary>
            public const String DisplayOrder = "DisplayOrder";

            ///<summary>事项名称</summary>
            public const String Title = "Title";

            ///<summary>目标</summary>
            public const String Targets = "Targets";

            ///<summary>政策文件生效日期</summary>
            public const String StartTime = "StartTime";

            ///<summary>过期时间</summary>
            public const String EndTime = "EndTime";

            ///<summary>代码</summary>
            public const String Code = "Code";

            ///<summary>参数</summary>
            public const String Parameters = "Parameters";

        }
        #endregion
    }

    /// <summary>广告接口</summary>
    public partial interface IAdvertisement
    {
        #region 属性
        /// <summary>编号</summary>
        Int32 ID { get; set; }

        /// <summary>是否可用</summary>
        Int32 Available { get; set; }

        /// <summary>类型</summary>
        String Type { get; set; }

        /// <summary>显示顺序</summary>
        Int32 DisplayOrder { get; set; }

        /// <summary>事项名称</summary>
        String Title { get; set; }

        /// <summary>目标</summary>
        String Targets { get; set; }

        /// <summary>政策文件生效日期</summary>
        DateTime StartTime { get; set; }

        /// <summary>过期时间</summary>
        DateTime EndTime { get; set; }

        /// <summary>代码</summary>
        String Code { get; set; }

        /// <summary>参数</summary>
        String Parameters { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}