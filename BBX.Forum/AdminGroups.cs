using Discuz.Cache;
using Discuz.Entity;

namespace Discuz.Forum
{
    public class AdminGroups
    {
        public static AdminGroupInfo[] GetAdminGroupList()
        {
            var cacheService = DNTCache.Current;
            AdminGroupInfo[] array = cacheService.RetrieveObject(CacheKeys.FORUM_ADMIN_GROUP_LIST) as AdminGroupInfo[];
            if (array == null)
            {
                array = Discuz.Data.AdminGroups.GetAdminGroupList();
                cacheService.AddObject(CacheKeys.FORUM_ADMIN_GROUP_LIST, array);
            }
            return array;
        }

        public static AdminGroupInfo GetAdminGroupInfo(int admingid)
        {
            if (admingid > 0)
            {
                AdminGroupInfo[] adminGroupList = AdminGroups.GetAdminGroupList();
                AdminGroupInfo[] array = adminGroupList;
                for (int i = 0; i < array.Length; i++)
                {
                    AdminGroupInfo adminGroupInfo = array[i];
                    if ((int)adminGroupInfo.Admingid == admingid)
                    {
                        return adminGroupInfo;
                    }
                }
            }
            return null;
        }

        public static int SetAdminGroupInfo(AdminGroupInfo admingroupsInfo, int userGroupId)
        {
            if (AdminGroups.GetAdminGroupInfo(userGroupId) != null)
            {
                return Discuz.Data.AdminGroups.SetAdminGroupInfo(admingroupsInfo);
            }
            return AdminGroups.CreateAdminGroupInfo(admingroupsInfo);
        }

        public static int CreateAdminGroupInfo(AdminGroupInfo admingroupsInfo)
        {
            DNTCache.Current.RemoveObject(CacheKeys.FORUM_USER_GROUP_LIST);
            DNTCache.Current.RemoveObject(CacheKeys.FORUM_ADMIN_GROUP_LIST);
            return Discuz.Data.AdminGroups.CreateAdminGroupInfo(admingroupsInfo);
        }

        public static int DeleteAdminGroupInfo(short admingid)
        {
            DNTCache.Current.RemoveObject(CacheKeys.FORUM_ADMIN_GROUP_LIST);
            return Discuz.Data.AdminGroups.DeleteAdminGroupInfo(admingid);
        }

        public static void ChangeUserAdminidByGroupid(int radminId, int groupId)
        {
            if (radminId > 0 && groupId > 0)
            {
                Discuz.Data.AdminGroups.ChangeUserAdminidByGroupid(radminId, groupId);
            }
        }
    }
}