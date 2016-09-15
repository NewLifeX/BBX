﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace BBX.Entity
{
    /// <summary>在线列表</summary>
    [Serializable]
    [DataObject]
    [Description("在线列表")]
    [BindTable("OnlineList", Description = "在线列表", ConnName = "BBX", DbType = DatabaseType.SqlServer)]
    public partial class OnlineList : IOnlineList
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

        private Int32 _GroupID;
        /// <summary>用户组</summary>
        [DisplayName("用户组")]
        [Description("用户组")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(2, "GroupID", "用户组", null, "int", 10, 0, false)]
        public virtual Int32 GroupID
        {
            get { return _GroupID; }
            set { if (OnPropertyChanging(__.GroupID, value)) { _GroupID = value; OnPropertyChanged(__.GroupID); } }
        }

        private Int32 _DisplayOrder;
        /// <summary>显示顺序</summary>
        [DisplayName("显示顺序")]
        [Description("显示顺序")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(3, "DisplayOrder", "显示顺序", null, "int", 10, 0, false)]
        public virtual Int32 DisplayOrder
        {
            get { return _DisplayOrder; }
            set { if (OnPropertyChanging(__.DisplayOrder, value)) { _DisplayOrder = value; OnPropertyChanged(__.DisplayOrder); } }
        }

        private String _Title;
        /// <summary>事项名称</summary>
        [DisplayName("事项名称")]
        [Description("事项名称")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(4, "Title", "事项名称", null, "nvarchar(50)", 0, 0, true, Master=true)]
        public virtual String Title
        {
            get { return _Title; }
            set { if (OnPropertyChanging(__.Title, value)) { _Title = value; OnPropertyChanged(__.Title); } }
        }

        private String _Img;
        /// <summary>图片</summary>
        [DisplayName("图片")]
        [Description("图片")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(5, "Img", "图片", null, "nvarchar(50)", 0, 0, true)]
        public virtual String Img
        {
            get { return _Img; }
            set { if (OnPropertyChanging(__.Img, value)) { _Img = value; OnPropertyChanged(__.Img); } }
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
                    case __.GroupID : return _GroupID;
                    case __.DisplayOrder : return _DisplayOrder;
                    case __.Title : return _Title;
                    case __.Img : return _Img;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.ID : _ID = Convert.ToInt32(value); break;
                    case __.GroupID : _GroupID = Convert.ToInt32(value); break;
                    case __.DisplayOrder : _DisplayOrder = Convert.ToInt32(value); break;
                    case __.Title : _Title = Convert.ToString(value); break;
                    case __.Img : _Img = Convert.ToString(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得在线列表字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号</summary>
            public static readonly Field ID = FindByName(__.ID);

            ///<summary>用户组</summary>
            public static readonly Field GroupID = FindByName(__.GroupID);

            ///<summary>显示顺序</summary>
            public static readonly Field DisplayOrder = FindByName(__.DisplayOrder);

            ///<summary>事项名称</summary>
            public static readonly Field Title = FindByName(__.Title);

            ///<summary>图片</summary>
            public static readonly Field Img = FindByName(__.Img);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得在线列表字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号</summary>
            public const String ID = "ID";

            ///<summary>用户组</summary>
            public const String GroupID = "GroupID";

            ///<summary>显示顺序</summary>
            public const String DisplayOrder = "DisplayOrder";

            ///<summary>事项名称</summary>
            public const String Title = "Title";

            ///<summary>图片</summary>
            public const String Img = "Img";

        }
        #endregion
    }

    /// <summary>在线列表接口</summary>
    public partial interface IOnlineList
    {
        #region 属性
        /// <summary>编号</summary>
        Int32 ID { get; set; }

        /// <summary>用户组</summary>
        Int32 GroupID { get; set; }

        /// <summary>显示顺序</summary>
        Int32 DisplayOrder { get; set; }

        /// <summary>事项名称</summary>
        String Title { get; set; }

        /// <summary>图片</summary>
        String Img { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}