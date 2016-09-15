using BBX.Plugin.Mall;

namespace BBX.Forum.ScheduledEvents
{
    public class TagsEvent : IEvent
    {
        public void Execute(object state)
        {
            //SpacePluginBase instance = SpacePluginProvider.GetInstance();
            //AlbumPluginBase instance2 = AlbumPluginProvider.GetInstance();
            //ForumTags.WriteHotTagsListForForumCacheFile(60);
            //ForumTags.WriteHotTagsListForForumJSONPCacheFile(60);
            //if (instance != null)
            //{
            //    instance.WriteHotTagsListForSpaceJSONPCacheFile(60);
            //}
            //if (instance2 != null)
            //{
            //    instance2.WriteHotTagsListForPhotoJSONPCacheFile(60);
            //}
            MallPluginBase instance3 = MallPluginProvider.GetInstance();
            if (instance3 != null)
            {
                instance3.WriteHotTagsListForGoodsJSONPCacheFile(60);
            }
        }
    }
}