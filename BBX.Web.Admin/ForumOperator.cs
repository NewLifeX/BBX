using BBX.Cache;

namespace BBX.Web.Admin
{
    public class ForumOperator
    {
        public static void RefreshForumCache()
        {
            XCache.Remove("/Forum/DropdownOptions");
            XCache.Remove(CacheKeys.FORUM_FORUM_LIST);
            XCache.Remove(CacheKeys.FORUM_FORUM_LIST_MENU_DIV);
        }
    }
}