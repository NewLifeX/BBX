namespace BBX.Entity
{
    public class PhotoInfo
    {
        
        private int _photoid;
        public int Photoid { get { return _photoid; } set { _photoid = value; } }

        private int _albumid;
        public int Albumid { get { return _albumid; } set { _albumid = value; } }

        private int _userid;
        public int Userid { get { return _userid; } set { _userid = value; } }

        private string _filename;
        public string Filename { get { return _filename; } set { _filename = value; } }

        private string _attachment;
        public string Attachment { get { return _attachment; } set { _attachment = value; } }

        private int _filesize;
        public int Filesize { get { return _filesize; } set { _filesize = value; } }

        private string _description;
        public string Description { get { return _description; } set { _description = value; } }

        private string _postdate;
        public string Postdate { get { return _postdate; } set { _postdate = value; } }

        private string _title;
        public string Title { get { return _title; } set { _title = value; } }

        private int _views;
        public int Views { get { return _views; } set { _views = value; } }

        private PhotoStatus _commentstatus = PhotoStatus.RegisteredUser;
        public PhotoStatus Commentstatus { get { return _commentstatus; } set { _commentstatus = value; } }

        private PhotoStatus _tagstatus = PhotoStatus.Buddy;
        public PhotoStatus Tagstatus { get { return _tagstatus; } set { _tagstatus = value; } }

        private int _comments;
        public int Comments { get { return _comments; } set { _comments = value; } }

        private string _username;
        public string Username { get { return _username; } set { _username = value; } }

        private int _isattachment;
        public int IsAttachment { get { return _isattachment; } set { _isattachment = value; } }
    }
}