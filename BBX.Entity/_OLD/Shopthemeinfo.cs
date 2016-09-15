namespace Discuz.Entity
{
    public class Shopthemeinfo
    {
        
        private int _themeid;
        public int Themeid { get { return _themeid; } set { _themeid = value; } }

        private string _directory;
        public string Directory { get { return _directory; } set { _directory = value; } }

        private string _name;
        public string Name { get { return _name; } set { _name = value; } }

        private string _author;
        public string Author { get { return _author; } set { _author = value; } }

        private string _createdate;
        public string Createdate { get { return _createdate; } set { _createdate = value; } }

        private string _copyright;
        public string Copyright { get { return _copyright; } set { _copyright = value; } }
    }
}