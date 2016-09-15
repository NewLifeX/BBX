using Discuz.Common;
using Discuz.Config;
using Discuz.Entity;
using System;
namespace Discuz.Forum
{
	public class PushFeed
	{
		private delegate bool PushFeedToCloud(TopicInfo topic, PostInfo post, AttachmentInfo[] attachments, bool feedStatus);
		private PushFeed.PushFeedToCloud pushFeed_asyncCallback;
		private string rootUrl = Utils.GetRootUrl(BaseConfigs.GetForumPath);
		private string ip = DNTRequest.GetIP();
		public void TopicPushFeed(TopicInfo topic, PostInfo post, AttachmentInfo[] attachments, bool feedStatus)
		{
			this.pushFeed_asyncCallback = new PushFeed.PushFeedToCloud(this.PushFeedToDiscuzCloud);
			this.pushFeed_asyncCallback.BeginInvoke(topic, post, attachments, feedStatus, null, null);
		}
		private bool PushFeedToDiscuzCloud(TopicInfo topic, PostInfo post, AttachmentInfo[] attachments, bool feedStatus)
		{
			if (topic == null || post == null || topic.Tid < 0 || topic.Posterid < 0 || post.Tid != topic.Tid || post.Pid < 0 || topic.Displayorder < 0 || topic.Hide == 1 || topic.Price > 0 || post.Invisible != 0 || !feedStatus)
			{
				return false;
			}
			var userConnectInfo = DiscuzCloud.GetUserConnectInfo(topic.Posterid);
			if (userConnectInfo == null || !feedStatus)
			{
				return false;
			}
			userConnectInfo.AllowPushFeed = feedStatus;
			if (DiscuzCloud.PushFeedToDiscuzCloud(topic, post, attachments, userConnectInfo, this.ip, this.rootUrl))
			{
                DiscuzCloud.CreateTopicPushFeedLog(new PushfeedLog
				{
					ID = topic.Tid,
					Uid = topic.Posterid,
					AuthorToken = userConnectInfo.Token,
					AuthorSecret = userConnectInfo.Secret
				});
				return true;
			}
			return false;
		}
	}
}
