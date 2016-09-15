using System;

namespace Discuz.Entity
{
    public class PhotoCommentInfo
    {
        
        private int _commentID;
        public int Commentid { get { return _commentID; } set { _commentID = value; } }

        private int _postID;
        public int Photoid { get { return _postID; } set { _postID = value; } }

        private string _username;
        public string Username { get { return _username; } set { _username = value; } }

        private int _userid;
        public int Userid { get { return _userid; } set { _userid = value; } }

        private string _ip;
        public string Ip { get { return _ip; } set { _ip = value; } }

        private DateTime _postDateTime;
        public DateTime Postdatetime { get { return _postDateTime; } set { _postDateTime = value; } }

        private string _content;
        public string Content { get { return _content; } set { _content = value; } }

        private int _parentID;
        public int Parentid { get { return _parentID; } set { _parentID = value; } }
    }
}