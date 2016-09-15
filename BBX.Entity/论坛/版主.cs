﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace BBX.Entity
{
    /// <summary>版主</summary>
    [Serializable]
    [DataObject]
    [Description("版主")]
    [BindTable("Moderator", Description = "版主", ConnName = "BBX", DbType = DatabaseType.SqlServer)]
    public partial class Moderator : IModerator
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

        private Int32 _Fid;
        /// <summary>论坛编号</summary>
        [DisplayName("论坛编号")]
        [Description("论坛编号")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(3, "Fid", "论坛编号", null, "int", 10, 0, false)]
        public virtual Int32 Fid
        {
            get { return _Fid; }
            set { if (OnPropertyChanging(__.Fid, value)) { _Fid = value; OnPropertyChanged(__.Fid); } }
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

        private Int32 _Inherited;
        /// <summary>继承</summary>
        [DisplayName("继承")]
        [Description("继承")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(5, "Inherited", "继承", null, "int", 10, 0, false)]
        public virtual Int32 Inherited
        {
            get { return _Inherited; }
            set { if (OnPropertyChanging(__.Inherited, value)) { _Inherited = value; OnPropertyChanged(__.Inherited); } }
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
                    case __.Fid : return _Fid;
                    case __.DisplayOrder : return _DisplayOrder;
                    case __.Inherited : return _Inherited;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.ID : _ID = Convert.ToInt32(value); break;
                    case __.Uid : _Uid = Convert.ToInt32(value); break;
                    case __.Fid : _Fid = Convert.ToInt32(value); break;
                    case __.DisplayOrder : _DisplayOrder = Convert.ToInt32(value); break;
                    case __.Inherited : _Inherited = Convert.ToInt32(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得版主字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号</summary>
            public static readonly Field ID = FindByName(__.ID);

            ///<summary>用户编号</summary>
            public static readonly Field Uid = FindByName(__.Uid);

            ///<summary>论坛编号</summary>
            public static readonly Field Fid = FindByName(__.Fid);

            ///<summary>显示顺序</summary>
            public static readonly Field DisplayOrder = FindByName(__.DisplayOrder);

            ///<summary>继承</summary>
            public static readonly Field Inherited = FindByName(__.Inherited);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得版主字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号</summary>
            public const String ID = "ID";

            ///<summary>用户编号</summary>
            public const String Uid = "Uid";

            ///<summary>论坛编号</summary>
            public const String Fid = "Fid";

            ///<summary>显示顺序</summary>
            public const String DisplayOrder = "DisplayOrder";

            ///<summary>继承</summary>
            public const String Inherited = "Inherited";

        }
        #endregion
    }

    /// <summary>版主接口</summary>
    public partial interface IModerator
    {
        #region 属性
        /// <summary>编号</summary>
        Int32 ID { get; set; }

        /// <summary>用户编号</summary>
        Int32 Uid { get; set; }

        /// <summary>论坛编号</summary>
        Int32 Fid { get; set; }

        /// <summary>显示顺序</summary>
        Int32 DisplayOrder { get; set; }

        /// <summary>继承</summary>
        Int32 Inherited { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}