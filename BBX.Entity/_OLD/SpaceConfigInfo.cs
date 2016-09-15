using System;

namespace Discuz.Entity
{
    public class SpaceConfigInfo
    {
        private string _rewritename;
        
        private int _spaceID;
        public int SpaceID { get { return _spaceID; } set { _spaceID = value; } }

        private int _userID;
        public int UserID { get { return _userID; } set { _userID = value; } }

        private string _spaceTitle;
        public string Spacetitle { get { return _spaceTitle; } set { _spaceTitle = value; } }

        private string __description;
        public string Description { get { return __description; } set { __description = value; } }

        private int __blogDispMode;
        public int BlogDispMode { get { return __blogDispMode; } set { __blogDispMode = value; } }

        private int _bpp;
        public int Bpp { get { return _bpp; } set { _bpp = value; } }

        private int _commentPref;
        public int Commentpref { get { return _commentPref; } set { _commentPref = value; } }

        private int _messagePref;
        public int MessagePref { get { return _messagePref; } set { _messagePref = value; } }

        public string Rewritename { get { return _rewritename; } set { _rewritename = value.Trim(); } }

        private int _themeID;
        public int ThemeID { get { return _themeID; } set { _themeID = value; } }

        private string _themePath;
        public string ThemePath { get { return _themePath; } set { _themePath = value; } }

        private int _postCount;
        public int PostCount { get { return _postCount; } set { _postCount = value; } }

        private int _commentCount;
        public int CommentCount { get { return _commentCount; } set { _commentCount = value; } }

        private int _visitedTimes;
        public int VisitedTimes { get { return _visitedTimes; } set { _visitedTimes = value; } }

        private DateTime _createDateTime;
        public DateTime CreateDateTime { get { return _createDateTime; } set { _createDateTime = value; } }

        private DateTime _updateDateTime;
        public DateTime UpdateDateTime { get { return _updateDateTime; } set { _updateDateTime = value; } }

        private int _defaultTab;
        public int DefaultTab { get { return _defaultTab; } set { _defaultTab = value; } }

        private SpaceStatusType _status;
        public SpaceStatusType Status { get { return _status; } set { _status = value; } }
    }
}