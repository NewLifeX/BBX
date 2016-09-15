﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace BBX.Entity
{
    /// <summary>收藏</summary>
    [Serializable]
    [DataObject]
    [Description("收藏")]
    [BindTable("Favorite", Description = "收藏", ConnName = "BBX", DbType = DatabaseType.SqlServer)]
    public partial class Favorite : IFavorite
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

        private DateTime _FavTime;
        /// <summary>收藏时间</summary>
        [DisplayName("收藏时间")]
        [Description("收藏时间")]
        [DataObjectField(false, false, true, 3)]
        [BindColumn(5, "FavTime", "收藏时间", null, "datetime", 3, 0, false)]
        public virtual DateTime FavTime
        {
            get { return _FavTime; }
            set { if (OnPropertyChanging(__.FavTime, value)) { _FavTime = value; OnPropertyChanged(__.FavTime); } }
        }

        private DateTime _ViewTime;
        /// <summary>查看时间</summary>
        [DisplayName("查看时间")]
        [Description("查看时间")]
        [DataObjectField(false, false, true, 3)]
        [BindColumn(6, "ViewTime", "查看时间", null, "datetime", 3, 0, false)]
        public virtual DateTime ViewTime
        {
            get { return _ViewTime; }
            set { if (OnPropertyChanging(__.ViewTime, value)) { _ViewTime = value; OnPropertyChanged(__.ViewTime); } }
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
                    case __.TypeID : return _TypeID;
                    case __.FavTime : return _FavTime;
                    case __.ViewTime : return _ViewTime;
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
                    case __.TypeID : _TypeID = Convert.ToInt32(value); break;
                    case __.FavTime : _FavTime = Convert.ToDateTime(value); break;
                    case __.ViewTime : _ViewTime = Convert.ToDateTime(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得收藏字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号</summary>
            public static readonly Field ID = FindByName(__.ID);

            ///<summary>用户编号</summary>
            public static readonly Field Uid = FindByName(__.Uid);

            ///<summary>主题编号</summary>
            public static readonly Field Tid = FindByName(__.Tid);

            ///<summary>类型</summary>
            public static readonly Field TypeID = FindByName(__.TypeID);

            ///<summary>收藏时间</summary>
            public static readonly Field FavTime = FindByName(__.FavTime);

            ///<summary>查看时间</summary>
            public static readonly Field ViewTime = FindByName(__.ViewTime);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得收藏字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号</summary>
            public const String ID = "ID";

            ///<summary>用户编号</summary>
            public const String Uid = "Uid";

            ///<summary>主题编号</summary>
            public const String Tid = "Tid";

            ///<summary>类型</summary>
            public const String TypeID = "TypeID";

            ///<summary>收藏时间</summary>
            public const String FavTime = "FavTime";

            ///<summary>查看时间</summary>
            public const String ViewTime = "ViewTime";

        }
        #endregion
    }

    /// <summary>收藏接口</summary>
    public partial interface IFavorite
    {
        #region 属性
        /// <summary>编号</summary>
        Int32 ID { get; set; }

        /// <summary>用户编号</summary>
        Int32 Uid { get; set; }

        /// <summary>主题编号</summary>
        Int32 Tid { get; set; }

        /// <summary>类型</summary>
        Int32 TypeID { get; set; }

        /// <summary>收藏时间</summary>
        DateTime FavTime { get; set; }

        /// <summary>查看时间</summary>
        DateTime ViewTime { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}