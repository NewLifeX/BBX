﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace BBX.Entity
{
    /// <summary>论坛字段</summary>
    [Serializable]
    [DataObject]
    [Description("论坛字段")]
    [BindTable("ForumField", Description = "论坛字段", ConnName = "BBX", DbType = DatabaseType.SqlServer)]
    public partial class ForumField : IForumField
    {
        #region 属性
        private Int32 _Fid;
        /// <summary>论坛编号</summary>
        [DisplayName("论坛编号")]
        [Description("论坛编号")]
        [DataObjectField(true, false, false, 10)]
        [BindColumn(1, "Fid", "论坛编号", null, "int", 10, 0, false)]
        public virtual Int32 Fid
        {
            get { return _Fid; }
            set { if (OnPropertyChanging(__.Fid, value)) { _Fid = value; OnPropertyChanged(__.Fid); } }
        }

        private String _Password;
        /// <summary>登录密码</summary>
        [DisplayName("登录密码")]
        [Description("登录密码")]
        [DataObjectField(false, false, false, 16)]
        [BindColumn(2, "Password", "登录密码", null, "nvarchar(16)", 0, 0, true)]
        public virtual String Password
        {
            get { return _Password; }
            set { if (OnPropertyChanging(__.Password, value)) { _Password = value; OnPropertyChanged(__.Password); } }
        }

        private String _Icon;
        /// <summary>图标</summary>
        [DisplayName("图标")]
        [Description("图标")]
        [DataObjectField(false, false, true, 255)]
        [BindColumn(3, "Icon", "图标", null, "nvarchar(255)", 0, 0, true)]
        public virtual String Icon
        {
            get { return _Icon; }
            set { if (OnPropertyChanging(__.Icon, value)) { _Icon = value; OnPropertyChanged(__.Icon); } }
        }

        private String _PostcrEdits;
        /// <summary></summary>
        [DisplayName("PostcrEdits")]
        [Description("")]
        [DataObjectField(false, false, true, 255)]
        [BindColumn(4, "PostcrEdits", "", null, "nvarchar(255)", 0, 0, true)]
        public virtual String PostcrEdits
        {
            get { return _PostcrEdits; }
            set { if (OnPropertyChanging(__.PostcrEdits, value)) { _PostcrEdits = value; OnPropertyChanged(__.PostcrEdits); } }
        }

        private String _ReplycrEdits;
        /// <summary></summary>
        [DisplayName("ReplycrEdits")]
        [Description("")]
        [DataObjectField(false, false, true, 255)]
        [BindColumn(5, "ReplycrEdits", "", null, "nvarchar(255)", 0, 0, true)]
        public virtual String ReplycrEdits
        {
            get { return _ReplycrEdits; }
            set { if (OnPropertyChanging(__.ReplycrEdits, value)) { _ReplycrEdits = value; OnPropertyChanged(__.ReplycrEdits); } }
        }

        private String _Redirect;
        /// <summary>重定向</summary>
        [DisplayName("重定向")]
        [Description("重定向")]
        [DataObjectField(false, false, true, 255)]
        [BindColumn(6, "Redirect", "重定向", null, "nvarchar(255)", 0, 0, true)]
        public virtual String Redirect
        {
            get { return _Redirect; }
            set { if (OnPropertyChanging(__.Redirect, value)) { _Redirect = value; OnPropertyChanged(__.Redirect); } }
        }

        private String _Attachextensions;
        /// <summary></summary>
        [DisplayName("Attachextensions")]
        [Description("")]
        [DataObjectField(false, false, true, 255)]
        [BindColumn(7, "Attachextensions", "", null, "nvarchar(255)", 0, 0, true)]
        public virtual String Attachextensions
        {
            get { return _Attachextensions; }
            set { if (OnPropertyChanging(__.Attachextensions, value)) { _Attachextensions = value; OnPropertyChanged(__.Attachextensions); } }
        }

        private String _Rules;
        /// <summary>编码规则</summary>
        [DisplayName("编码规则")]
        [Description("编码规则")]
        [DataObjectField(false, false, true, -1)]
        [BindColumn(8, "Rules", "编码规则", null, "ntext", 0, 0, true)]
        public virtual String Rules
        {
            get { return _Rules; }
            set { if (OnPropertyChanging(__.Rules, value)) { _Rules = value; OnPropertyChanged(__.Rules); } }
        }

        private String _TopicTypes;
        /// <summary></summary>
        [DisplayName("TopicTypes")]
        [Description("")]
        [DataObjectField(false, false, true, -1)]
        [BindColumn(9, "TopicTypes", "", null, "ntext", 0, 0, true)]
        public virtual String TopicTypes
        {
            get { return _TopicTypes; }
            set { if (OnPropertyChanging(__.TopicTypes, value)) { _TopicTypes = value; OnPropertyChanged(__.TopicTypes); } }
        }

        private String _ViewPerm;
        /// <summary></summary>
        [DisplayName("ViewPerm")]
        [Description("")]
        [DataObjectField(false, false, true, -1)]
        [BindColumn(10, "ViewPerm", "", null, "ntext", 0, 0, true)]
        public virtual String ViewPerm
        {
            get { return _ViewPerm; }
            set { if (OnPropertyChanging(__.ViewPerm, value)) { _ViewPerm = value; OnPropertyChanged(__.ViewPerm); } }
        }

        private String _PostPerm;
        /// <summary></summary>
        [DisplayName("PostPerm")]
        [Description("")]
        [DataObjectField(false, false, true, -1)]
        [BindColumn(11, "PostPerm", "", null, "ntext", 0, 0, true)]
        public virtual String PostPerm
        {
            get { return _PostPerm; }
            set { if (OnPropertyChanging(__.PostPerm, value)) { _PostPerm = value; OnPropertyChanged(__.PostPerm); } }
        }

        private String _ReplyPerm;
        /// <summary></summary>
        [DisplayName("ReplyPerm")]
        [Description("")]
        [DataObjectField(false, false, true, -1)]
        [BindColumn(12, "ReplyPerm", "", null, "ntext", 0, 0, true)]
        public virtual String ReplyPerm
        {
            get { return _ReplyPerm; }
            set { if (OnPropertyChanging(__.ReplyPerm, value)) { _ReplyPerm = value; OnPropertyChanged(__.ReplyPerm); } }
        }

        private String _GetattachPerm;
        /// <summary></summary>
        [DisplayName("GetattachPerm")]
        [Description("")]
        [DataObjectField(false, false, true, -1)]
        [BindColumn(13, "GetattachPerm", "", null, "ntext", 0, 0, true)]
        public virtual String GetattachPerm
        {
            get { return _GetattachPerm; }
            set { if (OnPropertyChanging(__.GetattachPerm, value)) { _GetattachPerm = value; OnPropertyChanged(__.GetattachPerm); } }
        }

        private String _PostattachPerm;
        /// <summary></summary>
        [DisplayName("PostattachPerm")]
        [Description("")]
        [DataObjectField(false, false, true, -1)]
        [BindColumn(14, "PostattachPerm", "", null, "ntext", 0, 0, true)]
        public virtual String PostattachPerm
        {
            get { return _PostattachPerm; }
            set { if (OnPropertyChanging(__.PostattachPerm, value)) { _PostattachPerm = value; OnPropertyChanged(__.PostattachPerm); } }
        }

        private String _Moderators;
        /// <summary>版主</summary>
        [DisplayName("版主")]
        [Description("版主")]
        [DataObjectField(false, false, true, -1)]
        [BindColumn(15, "Moderators", "版主", null, "ntext", 0, 0, true)]
        public virtual String Moderators
        {
            get { return _Moderators; }
            set { if (OnPropertyChanging(__.Moderators, value)) { _Moderators = value; OnPropertyChanged(__.Moderators); } }
        }

        private String _Description;
        /// <summary>描述</summary>
        [DisplayName("描述")]
        [Description("描述")]
        [DataObjectField(false, false, true, -1)]
        [BindColumn(16, "Description", "描述", null, "ntext", 0, 0, true)]
        public virtual String Description
        {
            get { return _Description; }
            set { if (OnPropertyChanging(__.Description, value)) { _Description = value; OnPropertyChanged(__.Description); } }
        }

        private SByte _ApplytopicType;
        /// <summary></summary>
        [DisplayName("ApplytopicType")]
        [Description("")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(17, "ApplytopicType", "", null, "tinyint", 0, 0, false)]
        public virtual SByte ApplytopicType
        {
            get { return _ApplytopicType; }
            set { if (OnPropertyChanging(__.ApplytopicType, value)) { _ApplytopicType = value; OnPropertyChanged(__.ApplytopicType); } }
        }

        private SByte _PostbytopicType;
        /// <summary></summary>
        [DisplayName("PostbytopicType")]
        [Description("")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(18, "PostbytopicType", "", null, "tinyint", 0, 0, false)]
        public virtual SByte PostbytopicType
        {
            get { return _PostbytopicType; }
            set { if (OnPropertyChanging(__.PostbytopicType, value)) { _PostbytopicType = value; OnPropertyChanged(__.PostbytopicType); } }
        }

        private SByte _ViewbytopicType;
        /// <summary></summary>
        [DisplayName("ViewbytopicType")]
        [Description("")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(19, "ViewbytopicType", "", null, "tinyint", 0, 0, false)]
        public virtual SByte ViewbytopicType
        {
            get { return _ViewbytopicType; }
            set { if (OnPropertyChanging(__.ViewbytopicType, value)) { _ViewbytopicType = value; OnPropertyChanged(__.ViewbytopicType); } }
        }

        private SByte _Topictypeprefix;
        /// <summary></summary>
        [DisplayName("Topictypeprefix")]
        [Description("")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(20, "Topictypeprefix", "", null, "tinyint", 0, 0, false)]
        public virtual SByte Topictypeprefix
        {
            get { return _Topictypeprefix; }
            set { if (OnPropertyChanging(__.Topictypeprefix, value)) { _Topictypeprefix = value; OnPropertyChanged(__.Topictypeprefix); } }
        }

        private String _Permuserlist;
        /// <summary></summary>
        [DisplayName("Permuserlist")]
        [Description("")]
        [DataObjectField(false, false, true, -1)]
        [BindColumn(21, "Permuserlist", "", null, "ntext", 0, 0, true)]
        public virtual String Permuserlist
        {
            get { return _Permuserlist; }
            set { if (OnPropertyChanging(__.Permuserlist, value)) { _Permuserlist = value; OnPropertyChanged(__.Permuserlist); } }
        }

        private String _Seokeywords;
        /// <summary>SEO关键字</summary>
        [DisplayName("SEO关键字")]
        [Description("SEO关键字")]
        [DataObjectField(false, false, true, 500)]
        [BindColumn(22, "Seokeywords", "SEO关键字", null, "nvarchar(500)", 0, 0, true)]
        public virtual String Seokeywords
        {
            get { return _Seokeywords; }
            set { if (OnPropertyChanging(__.Seokeywords, value)) { _Seokeywords = value; OnPropertyChanged(__.Seokeywords); } }
        }

        private String _Seodescription;
        /// <summary>SEO描述</summary>
        [DisplayName("SEO描述")]
        [Description("SEO描述")]
        [DataObjectField(false, false, true, 500)]
        [BindColumn(23, "Seodescription", "SEO描述", null, "nvarchar(500)", 0, 0, true)]
        public virtual String Seodescription
        {
            get { return _Seodescription; }
            set { if (OnPropertyChanging(__.Seodescription, value)) { _Seodescription = value; OnPropertyChanged(__.Seodescription); } }
        }

        private String _RewriteName;
        /// <summary>URL重写名称</summary>
        [DisplayName("URL重写名称")]
        [Description("URL重写名称")]
        [DataObjectField(false, false, true, 20)]
        [BindColumn(24, "RewriteName", "URL重写名称", null, "nvarchar(20)", 0, 0, true)]
        public virtual String RewriteName
        {
            get { return _RewriteName; }
            set { if (OnPropertyChanging(__.RewriteName, value)) { _RewriteName = value; OnPropertyChanged(__.RewriteName); } }
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
                    case __.Fid : return _Fid;
                    case __.Password : return _Password;
                    case __.Icon : return _Icon;
                    case __.PostcrEdits : return _PostcrEdits;
                    case __.ReplycrEdits : return _ReplycrEdits;
                    case __.Redirect : return _Redirect;
                    case __.Attachextensions : return _Attachextensions;
                    case __.Rules : return _Rules;
                    case __.TopicTypes : return _TopicTypes;
                    case __.ViewPerm : return _ViewPerm;
                    case __.PostPerm : return _PostPerm;
                    case __.ReplyPerm : return _ReplyPerm;
                    case __.GetattachPerm : return _GetattachPerm;
                    case __.PostattachPerm : return _PostattachPerm;
                    case __.Moderators : return _Moderators;
                    case __.Description : return _Description;
                    case __.ApplytopicType : return _ApplytopicType;
                    case __.PostbytopicType : return _PostbytopicType;
                    case __.ViewbytopicType : return _ViewbytopicType;
                    case __.Topictypeprefix : return _Topictypeprefix;
                    case __.Permuserlist : return _Permuserlist;
                    case __.Seokeywords : return _Seokeywords;
                    case __.Seodescription : return _Seodescription;
                    case __.RewriteName : return _RewriteName;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.Fid : _Fid = Convert.ToInt32(value); break;
                    case __.Password : _Password = Convert.ToString(value); break;
                    case __.Icon : _Icon = Convert.ToString(value); break;
                    case __.PostcrEdits : _PostcrEdits = Convert.ToString(value); break;
                    case __.ReplycrEdits : _ReplycrEdits = Convert.ToString(value); break;
                    case __.Redirect : _Redirect = Convert.ToString(value); break;
                    case __.Attachextensions : _Attachextensions = Convert.ToString(value); break;
                    case __.Rules : _Rules = Convert.ToString(value); break;
                    case __.TopicTypes : _TopicTypes = Convert.ToString(value); break;
                    case __.ViewPerm : _ViewPerm = Convert.ToString(value); break;
                    case __.PostPerm : _PostPerm = Convert.ToString(value); break;
                    case __.ReplyPerm : _ReplyPerm = Convert.ToString(value); break;
                    case __.GetattachPerm : _GetattachPerm = Convert.ToString(value); break;
                    case __.PostattachPerm : _PostattachPerm = Convert.ToString(value); break;
                    case __.Moderators : _Moderators = Convert.ToString(value); break;
                    case __.Description : _Description = Convert.ToString(value); break;
                    case __.ApplytopicType : _ApplytopicType = Convert.ToSByte(value); break;
                    case __.PostbytopicType : _PostbytopicType = Convert.ToSByte(value); break;
                    case __.ViewbytopicType : _ViewbytopicType = Convert.ToSByte(value); break;
                    case __.Topictypeprefix : _Topictypeprefix = Convert.ToSByte(value); break;
                    case __.Permuserlist : _Permuserlist = Convert.ToString(value); break;
                    case __.Seokeywords : _Seokeywords = Convert.ToString(value); break;
                    case __.Seodescription : _Seodescription = Convert.ToString(value); break;
                    case __.RewriteName : _RewriteName = Convert.ToString(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得论坛字段字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>论坛编号</summary>
            public static readonly Field Fid = FindByName(__.Fid);

            ///<summary>登录密码</summary>
            public static readonly Field Password = FindByName(__.Password);

            ///<summary>图标</summary>
            public static readonly Field Icon = FindByName(__.Icon);

            ///<summary></summary>
            public static readonly Field PostcrEdits = FindByName(__.PostcrEdits);

            ///<summary></summary>
            public static readonly Field ReplycrEdits = FindByName(__.ReplycrEdits);

            ///<summary>重定向</summary>
            public static readonly Field Redirect = FindByName(__.Redirect);

            ///<summary></summary>
            public static readonly Field Attachextensions = FindByName(__.Attachextensions);

            ///<summary>编码规则</summary>
            public static readonly Field Rules = FindByName(__.Rules);

            ///<summary></summary>
            public static readonly Field TopicTypes = FindByName(__.TopicTypes);

            ///<summary></summary>
            public static readonly Field ViewPerm = FindByName(__.ViewPerm);

            ///<summary></summary>
            public static readonly Field PostPerm = FindByName(__.PostPerm);

            ///<summary></summary>
            public static readonly Field ReplyPerm = FindByName(__.ReplyPerm);

            ///<summary></summary>
            public static readonly Field GetattachPerm = FindByName(__.GetattachPerm);

            ///<summary></summary>
            public static readonly Field PostattachPerm = FindByName(__.PostattachPerm);

            ///<summary>版主</summary>
            public static readonly Field Moderators = FindByName(__.Moderators);

            ///<summary>描述</summary>
            public static readonly Field Description = FindByName(__.Description);

            ///<summary></summary>
            public static readonly Field ApplytopicType = FindByName(__.ApplytopicType);

            ///<summary></summary>
            public static readonly Field PostbytopicType = FindByName(__.PostbytopicType);

            ///<summary></summary>
            public static readonly Field ViewbytopicType = FindByName(__.ViewbytopicType);

            ///<summary></summary>
            public static readonly Field Topictypeprefix = FindByName(__.Topictypeprefix);

            ///<summary></summary>
            public static readonly Field Permuserlist = FindByName(__.Permuserlist);

            ///<summary>SEO关键字</summary>
            public static readonly Field Seokeywords = FindByName(__.Seokeywords);

            ///<summary>SEO描述</summary>
            public static readonly Field Seodescription = FindByName(__.Seodescription);

            ///<summary>URL重写名称</summary>
            public static readonly Field RewriteName = FindByName(__.RewriteName);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得论坛字段字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>论坛编号</summary>
            public const String Fid = "Fid";

            ///<summary>登录密码</summary>
            public const String Password = "Password";

            ///<summary>图标</summary>
            public const String Icon = "Icon";

            ///<summary></summary>
            public const String PostcrEdits = "PostcrEdits";

            ///<summary></summary>
            public const String ReplycrEdits = "ReplycrEdits";

            ///<summary>重定向</summary>
            public const String Redirect = "Redirect";

            ///<summary></summary>
            public const String Attachextensions = "Attachextensions";

            ///<summary>编码规则</summary>
            public const String Rules = "Rules";

            ///<summary></summary>
            public const String TopicTypes = "TopicTypes";

            ///<summary></summary>
            public const String ViewPerm = "ViewPerm";

            ///<summary></summary>
            public const String PostPerm = "PostPerm";

            ///<summary></summary>
            public const String ReplyPerm = "ReplyPerm";

            ///<summary></summary>
            public const String GetattachPerm = "GetattachPerm";

            ///<summary></summary>
            public const String PostattachPerm = "PostattachPerm";

            ///<summary>版主</summary>
            public const String Moderators = "Moderators";

            ///<summary>描述</summary>
            public const String Description = "Description";

            ///<summary></summary>
            public const String ApplytopicType = "ApplytopicType";

            ///<summary></summary>
            public const String PostbytopicType = "PostbytopicType";

            ///<summary></summary>
            public const String ViewbytopicType = "ViewbytopicType";

            ///<summary></summary>
            public const String Topictypeprefix = "Topictypeprefix";

            ///<summary></summary>
            public const String Permuserlist = "Permuserlist";

            ///<summary>SEO关键字</summary>
            public const String Seokeywords = "Seokeywords";

            ///<summary>SEO描述</summary>
            public const String Seodescription = "Seodescription";

            ///<summary>URL重写名称</summary>
            public const String RewriteName = "RewriteName";

        }
        #endregion
    }

    /// <summary>论坛字段接口</summary>
    public partial interface IForumField
    {
        #region 属性
        /// <summary>论坛编号</summary>
        Int32 Fid { get; set; }

        /// <summary>登录密码</summary>
        String Password { get; set; }

        /// <summary>图标</summary>
        String Icon { get; set; }

        /// <summary></summary>
        String PostcrEdits { get; set; }

        /// <summary></summary>
        String ReplycrEdits { get; set; }

        /// <summary>重定向</summary>
        String Redirect { get; set; }

        /// <summary></summary>
        String Attachextensions { get; set; }

        /// <summary>编码规则</summary>
        String Rules { get; set; }

        /// <summary></summary>
        String TopicTypes { get; set; }

        /// <summary></summary>
        String ViewPerm { get; set; }

        /// <summary></summary>
        String PostPerm { get; set; }

        /// <summary></summary>
        String ReplyPerm { get; set; }

        /// <summary></summary>
        String GetattachPerm { get; set; }

        /// <summary></summary>
        String PostattachPerm { get; set; }

        /// <summary>版主</summary>
        String Moderators { get; set; }

        /// <summary>描述</summary>
        String Description { get; set; }

        /// <summary></summary>
        SByte ApplytopicType { get; set; }

        /// <summary></summary>
        SByte PostbytopicType { get; set; }

        /// <summary></summary>
        SByte ViewbytopicType { get; set; }

        /// <summary></summary>
        SByte Topictypeprefix { get; set; }

        /// <summary></summary>
        String Permuserlist { get; set; }

        /// <summary>SEO关键字</summary>
        String Seokeywords { get; set; }

        /// <summary>SEO描述</summary>
        String Seodescription { get; set; }

        /// <summary>URL重写名称</summary>
        String RewriteName { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}