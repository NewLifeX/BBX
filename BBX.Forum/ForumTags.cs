using System;
using System.Collections.Generic;
using System.Text;
using BBX.Cache;
using BBX.Common;

using BBX.Config;
using BBX.Data;
using BBX.Entity;
using BBX.Forum.ScheduledEvents;

namespace BBX.Forum
{
    public class ForumTags
    {
        public const string ForumHotTagJSONCacheFileName = "cache\\tag\\hottags_forum_cache_json.txt";
        public const string ForumHotTagJSONPCacheFileName = "cache\\tag\\hottags_forum_cache_jsonp.txt";

        public static void WriteHotTagsListForForumCacheFile(int count)
        {
            string filename = EventManager.RootPath + "cache\\tag\\hottags_forum_cache_json.txt";
            List<TagInfo> hotTagsListForForum = BBX.Data.ForumTags.GetHotTagsListForForum(count);
            Tags.WriteTagsCacheFile(filename, hotTagsListForForum, string.Empty, true);
        }

        public static void WriteHotTagsListForForumJSONPCacheFile(int count)
        {
            string filename = EventManager.RootPath + "cache\\tag\\hottags_forum_cache_jsonp.txt";
            List<TagInfo> hotTagsListForForum = BBX.Data.ForumTags.GetHotTagsListForForum(count);
            Tags.WriteTagsCacheFile(filename, hotTagsListForForum, "forumhottag_callback", true);
        }

        public static void WriteTopicTagsCacheFile(int topicid)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(BaseConfigs.GetForumPath);
            stringBuilder.Append("cache/topic/magic/");
            stringBuilder.Append((topicid / 1000 + 1).ToString());
            stringBuilder.Append("/");
            string mapPath = Utils.GetMapPath(stringBuilder.ToString() + topicid.ToString() + "_tags.config");
            List<TagInfo> tagsListByTopic = ForumTags.GetTagsListByTopic(topicid);
            Tags.WriteTagsCacheFile(mapPath, tagsListByTopic, string.Empty, false);
        }

        public static List<TagInfo> GetTagsListByTopic(int topicid)
        {
            List<TagInfo> list;
            //if ((MemCachedConfigs.GetConfig() != null && MemCachedConfigs.GetConfig().ApplyMemCached) || (RedisConfigs.GetConfig() != null && RedisConfigs.GetConfig().ApplyRedis))
            //{
            //    var cacheService = DNTCache.Current;
            //    list = (cacheService.RetrieveObject("/Forum/ShowTopic/Tag/" + topicid + "/") as List<TagInfo>);
            //    if (list == null)
            //    {
            //        list = BBX.Data.ForumTags.GetTagsListByTopic(topicid);
            //        XCache.Add("/Forum/ShowTopic/Tag/" + topicid + "/", list);
            //    }
            //}
            //else
            {
                list = BBX.Data.ForumTags.GetTagsListByTopic(topicid);
            }
            return list;
        }

        public static string GetTagsByTopicId(int topicid)
        {
            List<TagInfo> tagsListByTopic = ForumTags.GetTagsListByTopic(topicid);
            string text = "";
            foreach (TagInfo current in tagsListByTopic)
            {
                if (Utils.StrIsNullOrEmpty(text))
                {
                    text = current.Tagname;
                }
                else
                {
                    text = text + "," + current.Tagname;
                }
            }
            return text;
        }

        public static TagInfo[] GetCachedHotForumTags(int count)
        {
            var cacheService = XCache.Current;
            TagInfo[] array = cacheService.RetrieveObject("/Forum/Tag/Hot-" + count) as TagInfo[];
            if (array == null)
            {
                array = BBX.Data.ForumTags.GetCachedHotForumTags(count);
                XCache.Add("/Forum/Tag/Hot-" + count, array, 21600);
            }
            return array;
        }

        public static void DeleteTopicTags(int topicid)
        {
            BBX.Data.ForumTags.DeleteTopicTags(topicid);
            XCache.Remove("/Forum/ShowTopic/Tag/" + topicid + "/");
        }

        public static void CreateTopicTags(string[] tagArray, int topicId, int userId, string currentDateTime)
        {
            BBX.Data.ForumTags.CreateTopicTags(string.Join(" ", tagArray), topicId, userId, currentDateTime);
            ForumTags.WriteTopicTagsCacheFile(topicId);
            XCache.Remove("/Forum/ShowTopic/Tag/" + topicId + "/");
        }
    }
}