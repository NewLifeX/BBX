using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using NewLife.Xml;

namespace BBX.Config
{
    [XmlConfigFile("config/forumhotsetting.config", 15000)]
    /// <summary>论坛热帖配置</summary>
    [Description("论坛热帖配置")]
    [Serializable]
    public class ForumHotConfigInfo : XmlConfig2<ForumHotConfigInfo>
    {
        private bool _enable = true;
        /// <summary>启用</summary>
        [Description("启用")]
        public bool Enable { get { return _enable; } set { _enable = value; } }

        private List<ForumHotItemInfo> _forumHotCollection = new List<ForumHotItemInfo>();
        /// <summary>集合</summary>
        [Description("集合")]
        public List<ForumHotItemInfo> ForumHotCollection { get { return _forumHotCollection; } set { _forumHotCollection = value; } }

        static List<ForumHotItemInfo> _defs;

        //public ForumHotConfigInfo()
        //{
        //    if (_defs != null) _forumHotCollection.AddRange(_defs);
        //}

        protected override void OnLoaded()
        {
            base.OnLoaded();

            if (_forumHotCollection == null) _forumHotCollection = new List<ForumHotItemInfo>();
            if (_forumHotCollection.Count < 1) _forumHotCollection.AddRange(_defs);
        }

        static ForumHotConfigInfo()
        {
            var list = new List<ForumHotItemInfo>();
            _defs = list;

            var forumHotItemInfo = new ForumHotItemInfo();
            forumHotItemInfo.Id = 1;
            forumHotItemInfo.Name = "最新主题";
            forumHotItemInfo.Datatype = "topics";
            forumHotItemInfo.Sorttype = "PostDateTime";
            forumHotItemInfo.Topictitlelength = 30;
            forumHotItemInfo.Forumnamelength = 10;
            forumHotItemInfo.Dataitemcount = 13;
            list.Add(forumHotItemInfo);

            var forumHotItemInfo2 = new ForumHotItemInfo();
            forumHotItemInfo2.Id = 2;
            forumHotItemInfo2.Name = "热门主题";
            forumHotItemInfo2.Datatype = "topics";
            forumHotItemInfo2.Sorttype = "Views";
            forumHotItemInfo2.Topictitlelength = 30;
            forumHotItemInfo2.Forumnamelength = 10;
            forumHotItemInfo2.Dataitemcount = 13;
            list.Add(forumHotItemInfo2);

            var forumHotItemInfo3 = new ForumHotItemInfo();
            forumHotItemInfo3.Id = 3;
            forumHotItemInfo3.Name = "精华主题";
            forumHotItemInfo3.Datatype = "topics";
            forumHotItemInfo3.Sorttype = "Digest";
            forumHotItemInfo3.Topictitlelength = 30;
            forumHotItemInfo3.Forumnamelength = 10;
            forumHotItemInfo3.Dataitemcount = 13;
            list.Add(forumHotItemInfo3);

            var forumHotItemInfo4 = new ForumHotItemInfo();
            forumHotItemInfo4.Id = 4;
            forumHotItemInfo4.Name = "用户发帖排行";
            forumHotItemInfo4.Datatype = "users";
            forumHotItemInfo4.Sorttype = "posts";
            forumHotItemInfo4.Dataitemcount = 20;
            forumHotItemInfo4.Datatimetype = "Month";
            list.Add(forumHotItemInfo4);

            var forumHotItemInfo5 = new ForumHotItemInfo();
            forumHotItemInfo5.Id = 5;
            forumHotItemInfo5.Name = "版块发帖排行";
            forumHotItemInfo5.Datatype = "forums";
            forumHotItemInfo5.Sorttype = "posts";
            forumHotItemInfo5.Forumnamelength = 20;
            forumHotItemInfo5.Dataitemcount = 20;
            list.Add(forumHotItemInfo5);

            var forumHotItemInfo6 = new ForumHotItemInfo();
            forumHotItemInfo6.Id = 6;
            forumHotItemInfo6.Name = "热点图片";
            forumHotItemInfo6.Datatype = "pictures";
            forumHotItemInfo6.Sorttype = "ID";
            forumHotItemInfo6.Dataitemcount = 5;
            list.Add(forumHotItemInfo6);
        }
    }

    /// <summary>论坛热点</summary>
    [Description("论坛热点")]
    [Serializable]
    public class ForumHotItemInfo
    {
        private int id;
        [XmlAttribute]
        public int Id { get { return id; } set { id = value; } }

        private int enabled = 1;
        [XmlAttribute]
        public int Enabled { get { return enabled; } set { enabled = value; } }

        private string name = "";
        [XmlAttribute]
        public string Name { get { return name; } set { name = value; } }

        private string datatype = "";
        [XmlAttribute]
        public string Datatype { get { return datatype; } set { datatype = value; } }

        private string sorttype = "";
        [XmlAttribute]
        public string Sorttype { get { return sorttype; } set { sorttype = value; } }

        private string forumlist = "";
        [XmlAttribute]
        public string Forumlist { get { return forumlist; } set { forumlist = value; } }

        private int dataitemcount;
        [XmlAttribute]
        public int Dataitemcount { get { return dataitemcount; } set { dataitemcount = value; } }

        private int forumnamelength;
        [XmlAttribute]
        public int Forumnamelength { get { return forumnamelength; } set { forumnamelength = value; } }

        private int topictitlelength;
        [XmlAttribute]
        public int Topictitlelength { get { return topictitlelength; } set { topictitlelength = value; } }

        private int cachetimeout;
        /// <summary>缓存超时时间</summary>
        [XmlAttribute]
        public int Cachetimeout { get { return cachetimeout; } set { cachetimeout = value; } }

        private string datatimetype = "All";
        [XmlAttribute]
        public string Datatimetype { get { return datatimetype; } set { datatimetype = value; } }
    }
}