namespace Discuz.Entity
{
    public class TopicPushFeedInfo
    {
        
        private int topicId;
        public int TopicId { get { return topicId; } set { topicId = value; } }

        private int uid;
        public int Uid { get { return uid; } set { uid = value; } }

        private string authorToken;
        public string AuthorToken { get { return authorToken; } set { authorToken = value; } }

        private string authorSecret;
        public string AuthorSecret { get { return authorSecret; } set { authorSecret = value; } }
    }
}