using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Xml;
using BBX.Config;

namespace BBX.Forum
{
    /// <summary>站点映射地址</summary>
    public class SiteUrls
    {
        public class URLRewrite
        {
            private string _Name;
            public string Name { get { return this._Name; } set { this._Name = value; } }
            private string _Pattern;
            public string Pattern { get { return this._Pattern; } set { this._Pattern = value; } }
            private string _Page;
            public string Page { get { return this._Page; } set { this._Page = value; } }
            private string _QueryString;
            public string QueryString { get { return this._QueryString; } set { this._QueryString = value; } }

            public URLRewrite(string name, string pattern, string page, string querystring)
            {
                this._Name = name;
                this._Pattern = pattern;
                this._Page = page;
                this._QueryString = querystring;
            }

            public override string ToString()
            {
                return String.Format("", Name, Pattern, QueryString, Page);
            }
        }

        //private static volatile SiteUrls instance = null;
        private string SiteUrlsFile = HttpContext.Current.Server.MapPath(BaseConfigs.GetForumPath + "config/urls.config");

        private List<URLRewrite> _Urls = new List<URLRewrite>();
        public List<URLRewrite> Urls { get { return this._Urls; } set { this._Urls = value; } }

        private NameValueCollection _Paths = new NameValueCollection();
        public NameValueCollection Paths { get { return this._Paths; } set { this._Paths = value; } }

        private SiteUrls()
        {
            //this.Urls = new ArrayList();
            //Paths = new NameValueCollection();
            var doc = new XmlDocument();
            doc.Load(this.SiteUrlsFile);
            var urls = doc.SelectSingleNode("urls");
            foreach (XmlNode note in urls.ChildNodes)
            {
                if (note.NodeType != XmlNodeType.Comment && note.Name.ToLower() == "rewrite")
                {
                    var name = note.Attributes["name"];
                    var path = note.Attributes["path"];
                    var page = note.Attributes["page"];
                    var querystring = note.Attributes["querystring"];
                    var pattern = note.Attributes["pattern"];
                    if (name != null && path != null && page != null && querystring != null && pattern != null)
                    {
                        Paths.Add(name.Value, path.Value);
                        Urls.Add(new URLRewrite(name.Value, pattern.Value, page.Value.Replace("^", "&"), querystring.Value.Replace("^", "&")));
                    }
                }
            }
        }

        private static object lockHelper = new object();
        private static volatile SiteUrls _Current;
        /// <summary>当前实例</summary>
        public static SiteUrls Current
        {
            get
            {
                if (_Current != null) return _Current;
                lock (lockHelper)
                {
                    if (_Current != null) return _Current;

                    return _Current = new SiteUrls();
                }
            }
            set { _Current = value; }
        }
    }
}