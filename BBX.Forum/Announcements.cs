using System;
using System.Data;
using Discuz.Cache;
using Discuz.Common;
using Discuz.Entity;

namespace Discuz.Forum
{
    public class Announcements
    {
        public static DataTable GetAnnouncementList(string starttime, string endtime)
        {
            var cacheService = DNTCache.Current;
            DataTable dataTable = cacheService.RetrieveObject(CacheKeys.FORUM_ANNOUNCEMENT_LIST) as DataTable;
            if (dataTable == null)
            {
                dataTable = Discuz.Data.Announcements.GetAnnouncementList();
                cacheService.AddObject(CacheKeys.FORUM_ANNOUNCEMENT_LIST, dataTable);
            }
            return dataTable;
        }

        public static DataTable GetSimplifiedAnnouncementList(string starttime, string endtime)
        {
            return Announcements.GetSpecificAnnouncementList(starttime, endtime, -1);
        }

        public static DataTable GetSimplifiedAnnouncementList(string starttime)
        {
            return Announcements.GetSpecificAnnouncementList(starttime, "2999-01-01 00:00:00", -1);
        }

        public static DataTable GetSpecificAnnouncementList(string starttime, string endtime, int maxcount)
        {
            var cacheService = DNTCache.Current;
            DataTable dataTable = cacheService.RetrieveObject(CacheKeys.FORUM_SIMPLIFIED_ANNOUNCEMENT_LIST) as DataTable;
            if (dataTable == null)
            {
                dataTable = Discuz.Data.Announcements.GetAnnouncementList(maxcount);
                cacheService.AddObject(CacheKeys.FORUM_SIMPLIFIED_ANNOUNCEMENT_LIST, dataTable);
            }
            return dataTable;
        }

        public static DataTable GetAnnouncementList()
        {
            return Discuz.Data.Announcements.GetAnnouncementList();
        }

        public static DataTable GetAnnouncementList(int num, int pageid)
        {
            if (num <= 0 || pageid <= 0)
            {
                return new DataTable();
            }
            return Discuz.Data.Announcements.GetAnnouncementList(num, pageid);
        }

        public static void UpdateAnnouncementDisplayOrder(string displayorder, string[] hid, int userid, int useradminid)
        {
            DataTable announcementList = Announcements.GetAnnouncementList();
            for (int i = 0; i < displayorder.Split(',').Length; i++)
            {
                AnnouncementInfo announcement = Announcements.GetAnnouncement(TypeConverter.StrToInt(hid[i]));
                if (((announcement.Posterid > 0 && announcement.Posterid == userid) || useradminid == 1) && announcement.Displayorder.ToString() != displayorder[i].ToString())
                {
                    Discuz.Data.Announcements.UpdateAnnouncementDisplayOrder(TypeConverter.StrToInt(displayorder.Split(',')[i]), TypeConverter.ObjectToInt(announcementList.Rows[i]["id"]));
                }
            }
        }

        public static void CreateAnnouncement(string username, int userid, string subject, int displayorder, string starttime, string endtime, string message)
        {
            AnnouncementInfo announcementInfo = new AnnouncementInfo();
            announcementInfo.Title = subject;
            announcementInfo.Poster = username;
            announcementInfo.Posterid = userid;
            announcementInfo.Displayorder = displayorder;
            DateTime dateTime;
            DateTime.TryParse(starttime, out dateTime);
            announcementInfo.Starttime = dateTime;
            DateTime.TryParse(endtime, out dateTime);
            announcementInfo.Endtime = dateTime;
            announcementInfo.Message = message;
            Discuz.Data.Announcements.CreateAnnouncement(announcementInfo);
        }

        public static AnnouncementInfo GetAnnouncement(int aid)
        {
            if (aid <= 0)
            {
                return null;
            }
            return Discuz.Data.Announcements.GetAnnouncement(aid);
        }

        public static void UpdateAnnouncement(AnnouncementInfo announcementInfo)
        {
            if (announcementInfo.Id > 0)
            {
                Discuz.Data.Announcements.UpdateAnnouncement(announcementInfo);
            }
        }

        public static void DeleteAnnouncements(string aidlist)
        {
            Discuz.Data.Announcements.DeleteAnnouncements(aidlist);
            DNTCache.Current.RemoveObject(CacheKeys.FORUM_ANNOUNCEMENT_LIST);
            DNTCache.Current.RemoveObject(CacheKeys.FORUM_SIMPLIFIED_ANNOUNCEMENT_LIST);
        }

        public static DataTable GetAnnouncementsByCondition(string condition)
        {
            return Discuz.Data.Announcements.GetAnnouncementsByCondition(condition);
        }
    }
}