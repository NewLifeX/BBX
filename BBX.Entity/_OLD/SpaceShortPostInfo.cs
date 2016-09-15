using System;

namespace Discuz.Entity
{
    public class SpaceShortPostInfo
    {
        
        private int _postID;
        public int Postid { get { return _postID; } set { _postID = value; } }

        private string _author;
        public string Author { get { return _author; } set { _author = value; } }

        private int _uid;
        public int Uid { get { return _uid; } set { _uid = value; } }

        private DateTime _postDateTime;
        public DateTime Postdatetime { get { return _postDateTime; } set { _postDateTime = value; } }

        private string _title;
        public string Title { get { return _title; } set { _title = value; } }

        private int _commentCount;
        public int Commentcount { get { return _commentCount; } set { _commentCount = value; } }

        private int _views;
        public int Views { get { return _views; } set { _views = value; } }
    }
}