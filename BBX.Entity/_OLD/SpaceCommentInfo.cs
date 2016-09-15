using System;

namespace Discuz.Entity
{
    public class SpaceCommentInfo
    {
        
        private int _commentID;
        public int CommentID { get { return _commentID; } set { _commentID = value; } }

        private int _postID;
        public int PostID { get { return _postID; } set { _postID = value; } }

        private string _author;
        public string Author { get { return _author; } set { _author = value; } }

        private string _email;
        public string Email { get { return _email; } set { _email = value; } }

        private string _url;
        public string Url { get { return _url; } set { _url = value; } }

        private string _ip;
        public string Ip { get { return _ip; } set { _ip = value; } }

        private DateTime _postDateTime;
        public DateTime PostDateTime { get { return _postDateTime; } set { _postDateTime = value; } }

        private string _content;
        public string Content { get { return _content; } set { _content = value; } }

        private int _parentID;
        public int ParentID { get { return _parentID; } set { _parentID = value; } }

        private int _uid;
        public int Uid { get { return _uid; } set { _uid = value; } }

        private string _posttitle;
        public string PostTitle { get { return _posttitle; } set { _posttitle = value; } }
    }
}