namespace Discuz.Entity
{
    public class TabInfo
    {
        
        private int _tabID;
        public int TabID { get { return _tabID; } set { _tabID = value; } }

        private int _userID;
        public int UserID { get { return _userID; } set { _userID = value; } }

        private string _tabName;
        public string TabName { get { return _tabName; } set { _tabName = value; } }

        private string _iconFile;
        public string IconFile { get { return _iconFile; } set { _iconFile = value; } }

        private int _displayOrder;
        public int DisplayOrder { get { return _displayOrder; } set { _displayOrder = value; } }

        private string _template;
        public string Template { get { return _template; } set { _template = value; } }
    }
}