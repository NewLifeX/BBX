using System;
using System.ComponentModel;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace BBX.Entity
{
    /// <summary>短消息</summary>
    [Serializable]
    [DataObject]
    [Description("短消息")]
    [BindIndex("IX_ShortMessage_msgtoid", false, "msgtoid")]
    [BindTable("ShortMessage", Description = "短消息", ConnName = "BBX", DbType = DatabaseType.SqlServer)]
    public partial class ShortMessage : IShortMessage
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

        private String _Msgfrom;
        /// <summary>发送人姓名</summary>
        [DisplayName("发送人姓名")]
        [Description("发送人姓名")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(2, "Msgfrom", "发送人姓名", null, "nvarchar(50)", 0, 0, true)]
        public virtual String Msgfrom
        {
            get { return _Msgfrom; }
            set { if (OnPropertyChanging(__.Msgfrom, value)) { _Msgfrom = value; OnPropertyChanged(__.Msgfrom); } }
        }

        private Int32 _MsgfromID;
        /// <summary>发件人UID</summary>
        [DisplayName("发件人UID")]
        [Description("发件人UID")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(3, "MsgfromID", "发件人UID", null, "int", 10, 0, false)]
        public virtual Int32 MsgfromID
        {
            get { return _MsgfromID; }
            set { if (OnPropertyChanging(__.MsgfromID, value)) { _MsgfromID = value; OnPropertyChanged(__.MsgfromID); } }
        }

        private String _Msgto;
        /// <summary>收送人姓名</summary>
        [DisplayName("收送人姓名")]
        [Description("收送人姓名")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn(4, "Msgto", "收送人姓名", null, "nvarchar(50)", 0, 0, true)]
        public virtual String Msgto
        {
            get { return _Msgto; }
            set { if (OnPropertyChanging(__.Msgto, value)) { _Msgto = value; OnPropertyChanged(__.Msgto); } }
        }

        private Int32 _MsgtoID;
        /// <summary>收件人UID</summary>
        [DisplayName("收件人UID")]
        [Description("收件人UID")]
        [DataObjectField(false, false, true, 10)]
        [BindColumn(5, "MsgtoID", "收件人UID", null, "int", 10, 0, false)]
        public virtual Int32 MsgtoID
        {
            get { return _MsgtoID; }
            set { if (OnPropertyChanging(__.MsgtoID, value)) { _MsgtoID = value; OnPropertyChanged(__.MsgtoID); } }
        }

        private Int16 _Folder;
        /// <summary>文件箱</summary>
        [DisplayName("文件箱")]
        [Description("文件箱")]
        [DataObjectField(false, false, true, 5)]
        [BindColumn(6, "Folder", "文件箱", null, "smallint", 5, 0, false)]
        public virtual Int16 Folder
        {
            get { return _Folder; }
            set { if (OnPropertyChanging(__.Folder, value)) { _Folder = value; OnPropertyChanged(__.Folder); } }
        }

        private Boolean _New;
        /// <summary>未读新消息</summary>
        [DisplayName("未读新消息")]
        [Description("未读新消息")]
        [DataObjectField(false, false, true, 1)]
        [BindColumn(7, "New", "未读新消息", null, "bit", 0, 0, false)]
        public virtual Boolean New
        {
            get { return _New; }
            set { if (OnPropertyChanging(__.New, value)) { _New = value; OnPropertyChanged(__.New); } }
        }

        private String _Subject;
        /// <summary>公告主题</summary>
        [DisplayName("公告主题")]
        [Description("公告主题")]
        [DataObjectField(false, false, true, 60)]
        [BindColumn(8, "Subject", "公告主题", null, "nvarchar(60)", 0, 0, true)]
        public virtual String Subject
        {
            get { return _Subject; }
            set { if (OnPropertyChanging(__.Subject, value)) { _Subject = value; OnPropertyChanged(__.Subject); } }
        }

        private DateTime _PostDateTime;
        /// <summary>发送时间</summary>
        [DisplayName("发送时间")]
        [Description("发送时间")]
        [DataObjectField(false, false, true, 3)]
        [BindColumn(9, "PostDateTime", "发送时间", null, "datetime", 3, 0, false)]
        public virtual DateTime PostDateTime
        {
            get { return _PostDateTime; }
            set { if (OnPropertyChanging(__.PostDateTime, value)) { _PostDateTime = value; OnPropertyChanged(__.PostDateTime); } }
        }

        private String _Message;
        /// <summary>短消息内容</summary>
        [DisplayName("短消息内容")]
        [Description("短消息内容")]
        [DataObjectField(false, false, true, -1)]
        [BindColumn(10, "Message", "短消息内容", null, "ntext", 0, 0, true)]
        public virtual String Message
        {
            get { return _Message; }
            set { if (OnPropertyChanging(__.Message, value)) { _Message = value; OnPropertyChanged(__.Message); } }
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
                    case __.Msgfrom : return _Msgfrom;
                    case __.MsgfromID : return _MsgfromID;
                    case __.Msgto : return _Msgto;
                    case __.MsgtoID : return _MsgtoID;
                    case __.Folder : return _Folder;
                    case __.New : return _New;
                    case __.Subject : return _Subject;
                    case __.PostDateTime : return _PostDateTime;
                    case __.Message : return _Message;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.ID : _ID = Convert.ToInt32(value); break;
                    case __.Msgfrom : _Msgfrom = Convert.ToString(value); break;
                    case __.MsgfromID : _MsgfromID = Convert.ToInt32(value); break;
                    case __.Msgto : _Msgto = Convert.ToString(value); break;
                    case __.MsgtoID : _MsgtoID = Convert.ToInt32(value); break;
                    case __.Folder : _Folder = Convert.ToInt16(value); break;
                    case __.New : _New = Convert.ToBoolean(value); break;
                    case __.Subject : _Subject = Convert.ToString(value); break;
                    case __.PostDateTime : _PostDateTime = Convert.ToDateTime(value); break;
                    case __.Message : _Message = Convert.ToString(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得短消息字段信息的快捷方式</summary>
        public partial class _
        {
            ///<summary>编号</summary>
            public static readonly Field ID = FindByName(__.ID);

            ///<summary>发送人姓名</summary>
            public static readonly Field Msgfrom = FindByName(__.Msgfrom);

            ///<summary>发件人UID</summary>
            public static readonly Field MsgfromID = FindByName(__.MsgfromID);

            ///<summary>收送人姓名</summary>
            public static readonly Field Msgto = FindByName(__.Msgto);

            ///<summary>收件人UID</summary>
            public static readonly Field MsgtoID = FindByName(__.MsgtoID);

            ///<summary>文件箱</summary>
            public static readonly Field Folder = FindByName(__.Folder);

            ///<summary>未读新消息</summary>
            public static readonly Field New = FindByName(__.New);

            ///<summary>公告主题</summary>
            public static readonly Field Subject = FindByName(__.Subject);

            ///<summary>发送时间</summary>
            public static readonly Field PostDateTime = FindByName(__.PostDateTime);

            ///<summary>短消息内容</summary>
            public static readonly Field Message = FindByName(__.Message);

            static Field FindByName(string name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得短消息字段名称的快捷方式</summary>
        partial class __
        {
            ///<summary>编号</summary>
            public const String ID = "ID";

            ///<summary>发送人姓名</summary>
            public const String Msgfrom = "Msgfrom";

            ///<summary>发件人UID</summary>
            public const String MsgfromID = "MsgfromID";

            ///<summary>收送人姓名</summary>
            public const String Msgto = "Msgto";

            ///<summary>收件人UID</summary>
            public const String MsgtoID = "MsgtoID";

            ///<summary>文件箱</summary>
            public const String Folder = "Folder";

            ///<summary>未读新消息</summary>
            public const String New = "New";

            ///<summary>公告主题</summary>
            public const String Subject = "Subject";

            ///<summary>发送时间</summary>
            public const String PostDateTime = "PostDateTime";

            ///<summary>短消息内容</summary>
            public const String Message = "Message";

        }
        #endregion
    }

    /// <summary>短消息接口</summary>
    public partial interface IShortMessage
    {
        #region 属性
        /// <summary>编号</summary>
        Int32 ID { get; set; }

        /// <summary>发送人姓名</summary>
        String Msgfrom { get; set; }

        /// <summary>发件人UID</summary>
        Int32 MsgfromID { get; set; }

        /// <summary>收送人姓名</summary>
        String Msgto { get; set; }

        /// <summary>收件人UID</summary>
        Int32 MsgtoID { get; set; }

        /// <summary>文件箱</summary>
        Int16 Folder { get; set; }

        /// <summary>未读新消息</summary>
        Boolean New { get; set; }

        /// <summary>公告主题</summary>
        String Subject { get; set; }

        /// <summary>发送时间</summary>
        DateTime PostDateTime { get; set; }

        /// <summary>短消息内容</summary>
        String Message { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值。</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}