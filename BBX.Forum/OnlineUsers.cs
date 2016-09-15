using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BBX.Cache;
using BBX.Common;
using BBX.Config;
using BBX.Entity;
using XCode;
using NewLife;

namespace BBX.Forum
{
    public class OnlineUsers
    {
        private static object SynObject = new object();

        //public static int GetOnlineAllUserCount()
        //{
        //    //int onlineUserCountCacheMinute = GeneralConfigInfo.Current.OnlineUserCountCacheMinute;
        //    //if (onlineUserCountCacheMinute == 0)
        //    {
        //        //return BBX.Data.OnlineUsers.GetOnlineAllUserCount();
        //        return Online.Meta.Count;
        //    }
        //    //var cacheService = XCache.Current;
        //    //int num = TypeConverter.ObjectToInt(cacheService.RetrieveObject("/Forum/OnlineUserCount"));
        //    //if (num != 0)
        //    //{
        //    //    return num;
        //    //}
        //    ////num = BBX.Data.OnlineUsers.GetOnlineAllUserCount();
        //    //num = Online.Meta.Count;
        //    //XCache.Add("/Forum/OnlineUserCount", num, onlineUserCountCacheMinute * 60);
        //    //return num;
        //}

        //public static int GetCacheOnlineAllUserCount()
        //{
        //    int num = TypeConverter.StrToInt(Utils.GetCookie("onlineusercount"), 0);
        //    if (num == 0)
        //    {
        //        num = Online.Meta.Count;
        //        Utils.WriteCookie("onlineusercount", num.ToString(), 3);
        //    }
        //    return num;
        //}

        //public static EntityList<Online> GetOnlineUserList(int totaluser, out int guest, out int user, out int invisibleuser)
        //{
        //    //DataTable onlineUserListTable = BBX.Data.OnlineUsers.GetOnlineUserListTable();
        //    var list = Online.FindAllWithCache();
        //    var st = Statistic.Current;
        //    //int num = Statistic.GetInt("highestonlineusercount");
        //    if (totaluser > st.HighestOnlineUserCount)
        //    {
        //        //Statistic.UpdateStatistics("highestonlineusercount", totaluser);
        //        //Statistic.UpdateStatistics("highestonlineusertime", DateTime.Now);
        //        //Statistic.ReSetStatisticsCache();

        //        st.HighestOnlineUserCount = totaluser;
        //        st.HighestOnlineUserTime = DateTime.Now;
        //        st.Update();
        //    }
        //    //user = BBX.Data.OnlineUsers.GetOnlineUserCount();
        //    user = list.Count;
        //    //if (EntLibConfigInfo.Current != null && EntLibConfigInfo.Current.Cacheonlineuser.Enable)
        //    //{
        //    //    invisibleuser = BBX.Data.OnlineUsers.GetInvisibleOnlineUserCount();
        //    //}
        //    //else
        //    {
        //        //DataRow[] array = list.Select("invisible=1");
        //        invisibleuser = list.ToList().Count(e => e.Invisible == 1);
        //    }
        //    guest = totaluser > user ? totaluser - user : 0;
        //    return list;
        //}

        //public static void UpdatePostPMTime(int olid)
        //{
        //    if (GeneralConfigInfo.Current.Onlineoptimization != 1)
        //    {
        //        BBX.Data.OnlineUsers.UpdatePostPMTime(olid);
        //    }
        //}

        //public static void UpdateInvisible(int olid, int invisible)
        //{
        //    if (GeneralConfigInfo.Current.Onlineoptimization != 1)
        //    {
        //        BBX.Data.OnlineUsers.UpdateInvisible(olid, invisible);
        //    }
        //}

        //public static void UpdatePassword(int olid, string password)
        //{
        //    BBX.Data.OnlineUsers.UpdatePassword(olid, password);
        //}

        //public static int DeleteRows(int olid)
        //{
        //    //return BBX.Data.OnlineUsers.DeleteRows(olid);
        //    var online = Online.FindByID(olid);
        //    if (online == null) return 0;

        //    return online.Delete();
        //}

        //public static List<Online> GetForumOnlineUserCollection(int forumid, out int totaluser, out int guest, out int user, out int invisibleuser)
        //{
        //    var list = Online.FindAllByForumID(forumid);
        //    guest = 0;
        //    invisibleuser = 0;
        //    totaluser = list.Count;
        //    foreach (var item in list)
        //    {
        //        if (item.UserID == -1)
        //        {
        //            guest++;
        //        }
        //        if (item.Invisible == 1)
        //        {
        //            invisibleuser++;
        //        }
        //    }
        //    user = totaluser - guest;

        //    return list;
        //}

        //public static List<Online> GetOnlineUserCollection(out int totaluser, out int guest, out int user, out int invisibleuser)
        //{
        //    var list = Online.FindAllWithCache();
        //    user = 0;
        //    invisibleuser = 0;
        //    if (GeneralConfigInfo.Current.Whosonlinecontract == 0)
        //        totaluser = list.Count;
        //    else
        //        totaluser = Online.Meta.Count;

        //    foreach (var item in list)
        //    {
        //        if (item.UserID > 0) user++;
        //        if (item.Invisible == 1) invisibleuser++;
        //    }
        //    var st = Statistic.Current;
        //    if (totaluser > st.HighestOnlineUserCount)
        //    {
        //        st.HighestOnlineUserCount = totaluser;
        //        st.HighestOnlineUserTime = DateTime.Now;
        //        st.Update();
        //    }
        //    guest = totaluser > user ? (totaluser - user) : 0;

        //    return list;
        //}

        //public static void UpdateOnlineTime(int oltimespan, int uid)
        //{
        //    if (oltimespan != 0)
        //    {
        //        if (Utils.StrIsNullOrEmpty(Utils.GetCookie("lastactivity", "onlinetime")))
        //        {
        //            Utils.WriteCookie("lastactivity", "onlinetime", Environment.TickCount.ToString());
        //        }
        //        int num = Environment.TickCount - TypeConverter.StrToInt(Utils.GetCookie("lastactivity", "onlinetime"), Environment.TickCount);
        //        if (num <= 0 || num >= oltimespan * 60 * 1000)
        //        {
        //            //BBX.Data.OnlineUsers.UpdateOnlineTime(oltimespan, uid);
        //            OnlineTime.UpdateOnlineTime(oltimespan, uid);
        //            Utils.WriteCookie("lastactivity", "onlinetime", Environment.TickCount.ToString());
        //            num = Environment.TickCount - TypeConverter.StrToInt(Utils.GetCookie("lastactivity", "oltime"), Environment.TickCount);
        //            if (Utils.StrIsNullOrEmpty(Utils.GetCookie("lastactivity", "oltime")) || num <= 0 || num >= 2 * oltimespan * 60 * 1000)
        //            {
        //                //BBX.Data.OnlineUsers.SynchronizeOnlineTime(uid);
        //                OnlineTime.SynchronizeOnlineTime(uid);
        //                Utils.WriteCookie("lastactivity", "oltime", Environment.TickCount.ToString());
        //            }
        //        }
        //    }
        //}

        public static int GetOlidByUid(int uid)
        {
            //return BBX.Data.OnlineUsers.GetOlidByUid(uid);
            if (uid <= 0) return uid;
            var online = Online.FindByUserID(uid);
            return online != null ? online.ID : -1;
        }

        //public static int DeleteUserByUid(int uid)
        //{
        //    return DeleteRows(GetOlidByUid(uid));
        //}

        //public static int UpdateNewPms(int olid, int count)
        //{
        //    return BBX.Data.OnlineUsers.UpdateNewPms(olid, count);
        //}

        //public static int UpdateNewNotices(int olid, int pluscount)
        //{
        //    return BBX.Data.OnlineUsers.UpdateNewNotices(olid, pluscount);
        //}

        //public static int UpdateNewNotices(int olid)
        //{
        //    return BBX.Data.OnlineUsers.UpdateNewNotices(olid, 0);
        //}
    }
}