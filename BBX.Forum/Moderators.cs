using BBX.Entity;

namespace BBX.Forum
{
    public class Moderators
    {
        //public static List<ModeratorInfo> GetModeratorList()
        //{
        //    var cacheService = XCache.Current;
        //    List<ModeratorInfo> list = cacheService.RetrieveObject(CacheKeys.FORUM_MODERATOR_LIST) as List<ModeratorInfo>;
        //    if (list == null)
        //    {
        //        list = BBX.Data.Moderators.GetModeratorList();
        //        XCache.Add(CacheKeys.FORUM_MODERATOR_LIST, list);
        //    }
        //    return list;
        //}

        public static bool IsModer(int adminId, int uid, int fid)
        {
            if (adminId == 0)
            {
                return false;
            }
            if (adminId == 1 || adminId == 2)
            {
                return true;
            }
            if (adminId == 3)
            {
                //foreach (ModeratorInfo current in Moderators.GetModeratorList())
                //{
                //    if (current.Uid == uid && current.Fid == fid)
                //    {
                //        return true;
                //    }
                //}
                if (Moderator.FindByUidAndFid(uid, fid) != null) return true;

                return false;
            }
            return false;
        }

        public static string GetFidListByModerator(string moderatorUserName)
        {
            string text = "";
            foreach (var current in Forums.GetForumList())
            {
                if (("," + current.Moderators + ",").Contains("," + moderatorUserName + ","))
                {
                    text = text + current.Fid + ",";
                }
            }
            return text.TrimEnd(',');
        }
    }
}