using System;

namespace BBX.Entity
{
    [Serializable]
    public class TopicView
    {
        
        private int _topicID;
        public int TopicID { get { return _topicID; } set { _topicID = value; } }

        private int _viewCount;
        public int ViewCount { get { return _viewCount; } set { _viewCount = value; } }

        public TopicView()
        {
        }

        public TopicView(int tid, int viewcount)
        {
            this._topicID = tid;
            this._viewCount = viewcount;
        }
    }
}