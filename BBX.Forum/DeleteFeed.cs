using Discuz.Common;
using Discuz.Entity;

namespace Discuz.Forum
{
    public class DeleteFeed
    {
        private delegate bool DeletePushedFeed(int tid);

        private DeleteFeed.DeletePushedFeed deleteFeed_asyncCallback;
        private string ip = DNTRequest.GetIP();

        public void DeleteTopicPushedFeed(int tid)
        {
            this.deleteFeed_asyncCallback = new DeleteFeed.DeletePushedFeed(this.DeletePushedFeedInCloud);
            this.deleteFeed_asyncCallback.BeginInvoke(tid, null, null);
        }

        private bool DeletePushedFeedInCloud(int tid)
        {
            if (tid < 0)
            {
                return false;
            }
            PushfeedLog topicPushFeedLog = DiscuzCloud.GetTopicPushFeedLog(tid);
            if (topicPushFeedLog == null)
            {
                return false;
            }
            bool flag = DiscuzCloud.DeletePushedFeedInDiscuzCloud(topicPushFeedLog, this.ip);
            if (flag)
            {
                DiscuzCloud.DeleteTopicPushFeedLog(tid);
            }
            return true;
        }
    }
}