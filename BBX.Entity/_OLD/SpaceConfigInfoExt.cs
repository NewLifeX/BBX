using System;

namespace Discuz.Entity
{
    public class SpaceConfigInfoExt
    {
        
        private int _spaceID;
        public int Spaceid { get { return _spaceID; } set { _spaceID = value; } }

        private int _userID;
        public int Userid { get { return _userID; } set { _userID = value; } }

        private string _spaceTitle;
        public string Spacetitle { get { return _spaceTitle; } set { _spaceTitle = value; } }

        private string __description;
        public string Description { get { return __description; } set { __description = value; } }

        private int __blogDispMode;
        public int Blogdispmode { get { return __blogDispMode; } set { __blogDispMode = value; } }

        private int _bpp;
        public int Bpp { get { return _bpp; } set { _bpp = value; } }

        private int _commentPref;
        public int Commentpref { get { return _commentPref; } set { _commentPref = value; } }

        private int _messagePref;
        public int Messagepref { get { return _messagePref; } set { _messagePref = value; } }

        private string _rewritename;
        public string Rewritename { get { return _rewritename; } set { _rewritename = value; } }

        private int _themeID;
        public int Themeid { get { return _themeID; } set { _themeID = value; } }

        private string _themePath;
        public string Themepath { get { return _themePath; } set { _themePath = value; } }

        private int _postCount;
        public int Postcount { get { return _postCount; } set { _postCount = value; } }

        private int _commentCount;
        public int Commentcount { get { return _commentCount; } set { _commentCount = value; } }

        private int _visitedTimes;
        public int Visitedtimes { get { return _visitedTimes; } set { _visitedTimes = value; } }

        private DateTime _createDateTime;
        public DateTime Createdatetime { get { return _createDateTime; } set { _createDateTime = value; } }

        private DateTime _updateDateTime;
        public DateTime Updatedatetime { get { return _updateDateTime; } set { _updateDateTime = value; } }

        private int _defaultTab;
        public int Defaulttab { get { return _defaultTab; } set { _defaultTab = value; } }

        private SpaceStatusType _status;
        public SpaceStatusType Status { get { return _status; } set { _status = value; } }

        private string _spacePic;
        public string Spacepic { get { return _spacePic; } set { _spacePic = value; } }

        private int _albumCount;
        public int Albumcount { get { return _albumCount; } set { _albumCount = value; } }

        private int _postID;
        public int Postid { get { return _postID; } set { _postID = value; } }

        private string _postTitle;
        public string Posttitle { get { return _postTitle; } set { _postTitle = value; } }
    }
}