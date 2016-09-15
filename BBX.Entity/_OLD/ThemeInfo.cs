using System;

namespace Discuz.Entity
{
    [Serializable]
    public class ThemeInfo
    {
        
        private int _themeId;
        public int ThemeId { get { return _themeId; } set { _themeId = value; } }

        private string _directory;
        public string Directory { get { return _directory; } set { _directory = value; } }

        private string _name;
        public string Name { get { return _name; } set { _name = value; } }

        private int _type;
        public int Type { get { return _type; } set { _type = value; } }

        private string _author;
        public string Author { get { return _author; } set { _author = value; } }

        private string _createDate;
        public string CreateDate { get { return _createDate; } set { _createDate = value; } }

        private string _copyRight;
        public string CopyRight { get { return _copyRight; } set { _copyRight = value; } }
    }
}