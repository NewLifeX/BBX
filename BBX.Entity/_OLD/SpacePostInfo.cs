using System;

namespace Discuz.Entity
{
    public class SpacePostInfo
    {
        
        private int _postid;
        public int Postid { get { return _postid; } set { _postid = value; } }

        private string _author;
        public string Author { get { return _author; } set { _author = value; } }

        private int _uid;
        public int Uid { get { return _uid; } set { _uid = value; } }

        private DateTime _postdatetime;
        public DateTime Postdatetime { get { return _postdatetime; } set { _postdatetime = value; } }

        private string _content;
        public string Content { get { return _content; } set { _content = value; } }

        private string _title;
        public string Title { get { return _title; } set { _title = value; } }

        private string _category;
        public string Category { get { return _category; } set { _category = value; } }

        private int _postStatus;
        public int PostStatus { get { return _postStatus; } set { _postStatus = value; } }

        private int _commentStatus;
        public int CommentStatus { get { return _commentStatus; } set { _commentStatus = value; } }

        private DateTime _postUpdateTime;
        public DateTime PostUpDateTime { get { return _postUpdateTime; } set { _postUpdateTime = value; } }

        private int _commentcount;
        public int Commentcount { get { return _commentcount; } set { _commentcount = value; } }

        private int _views;
        public int Views { get { return _views; } set { _views = value; } }
    }
}