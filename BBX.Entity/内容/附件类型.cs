﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace BBX.Entity
{
    /// <summary>附件类型</summary>
    [Serializable]
    [DataObject]
    [Description("附件类型")]
    [BindTable("AttachType", Description = "附件类型", ConnName = "BBX", DbType = DatabaseType.SqlServer)]
    public partial class AttachType : IAttachType
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

        private String _Extension;
        /// <summary>扩展名</summary>
        [DisplayName("扩展名")]
        [Description("扩展名")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(2, "Extension", "扩展名", null, "nvarchar(50)", 0, 0, true)]
        public virtual String Extension
        {
            get { return _Extension; }
            set { if (OnPropertyChanging(__.Extension, value)) { _Extension = value; OnPropertyChanged(__.Extension); } }
        }

        private Int32 _MaxSize;
        /// <summary>最大尺寸</summary>
        [DisplayName("最大尺寸")]
        [Description("最大尺寸")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(3, "MaxSize", "最大尺寸", null, "int", 10, 0, false)]
        public virtual Int32 MaxSize
        {
            get { return _MaxSize; }
            set { if (OnPropertyChanging(__.MaxSize, value)) { _MaxSize = value; OnPropertyChanged(__.MaxSize); } }
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
                    case __.Extension : return _Extension;
                    case __.MaxSize : return _MaxSize;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.ID : _ID = Convert.ToInt32(value); break;
                    case __.Extension : _Extension = Convert.ToString(value); break;
                    case __.MaxSize : _MaxSize = Convert.ToInt32(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得附件类型字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号</summary>
            public static readonly Field ID = FindByName(__.ID);

            ///<summary>扩展名</summary>
            public static readonly Field Extension = FindByName(__.Extension);

            ///<summary>最大尺寸</summary>
            public static readonly Field MaxSize = FindByName(__.MaxSize);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得附件类型字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号</summary>
            public const String ID = "ID";

            ///<summary>扩展名</summary>
            public const String Extension = "Extension";

            ///<summary>最大尺寸</summary>
            public const String MaxSize = "MaxSize";

        }
        #endregion
    }

    /// <summary>附件类型接口</summary>
    public partial interface IAttachType
    {
        #region 属性
        /// <summary>编号</summary>
        Int32 ID { get; set; }

        /// <summary>扩展名</summary>
        String Extension { get; set; }

        /// <summary>最大尺寸</summary>
        Int32 MaxSize { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}