using System.Collections.Generic;

namespace Discuz.Entity
{
    public class ModulePref
    {
        private List<ModuleRequire> _requires;

        private string _title = string.Empty;
        public string Title { get { return _title; } set { _title = value; } }

        private string _description = string.Empty;
        public string Description { get { return _description; } set { _description = value; } }

        private string _author = string.Empty;
        public string Author { get { return _author; } set { _author = value; } }

        private string _authorEmail = string.Empty;
        public string AuthorEmail { get { return _authorEmail; } set { _authorEmail = value; } }

        private string _authorAffiliation = string.Empty;
        public string AuthorAffiliation { get { return _authorAffiliation; } set { _authorAffiliation = value; } }

        private string _authorLocation = string.Empty;
        public string AuthorLocation { get { return _authorLocation; } set { _authorLocation = value; } }

        private string _titleUrl = string.Empty;
        public string TitleUrl { get { return _titleUrl; } set { _titleUrl = value; } }

        private string _renderInline = string.Empty;
        public string RenderInline { get { return _renderInline; } set { _renderInline = value; } }

        private string _screenshot = string.Empty;
        public string Screenshot { get { return _screenshot; } set { _screenshot = value; } }

        private string _thumbnail = string.Empty;
        public string Thumbnail { get { return _thumbnail; } set { _thumbnail = value; } }

        private string _category = string.Empty;
        public string Category { get { return _category; } set { _category = value; } }

        private string _category2 = string.Empty;
        public string Category2 { get { return _category2; } set { _category2 = value; } }

        private string _directoryTitle = string.Empty;
        public string DirectoryTitle { get { return _directoryTitle; } set { _directoryTitle = value; } }

        private int _height = 200;
        public int Height { get { return _height; } set { _height = value; } }

        private int _width = 320;
        public int Width { get { return _width; } set { _width = value; } }

        private bool _scaling = true;
        public bool Scaling { get { return _scaling; } set { _scaling = value; } }

        private bool _scrolling;
        public bool Scrolling { get { return _scrolling; } set { _scrolling = value; } }

        private bool _singleton = true;
        public bool Singleton { get { return _singleton; } set { _singleton = value; } }

        private string _authorPhoto = string.Empty;
        public string AuthorPhoto { get { return _authorPhoto; } set { _authorPhoto = value; } }

        private string _authorAboutMe = string.Empty;
        public string AuthorAboutMe { get { return _authorAboutMe; } set { _authorAboutMe = value; } }

        private string _authorLink = string.Empty;
        public string AuthorLink { get { return _authorLink; } set { _authorLink = value; } }

        private string _authorQuote = string.Empty;
        public string AuthorQuote { get { return _authorQuote; } set { _authorQuote = value; } }

        private string _controller = string.Empty;
        public string Controller { get { return _controller; } set { _controller = value; } }

        public List<ModuleRequire> Requires { get { return _requires; } set { _requires = value; } }
    }
}