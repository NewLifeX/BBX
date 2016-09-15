using BBX.Cache;
using BBX.Config;
using BBX.Entity;

namespace BBX.Forum
{
    public class AdminCaches
    {
        private static void RemoveObject(string key)
        {
            XCache.Remove(key);
        }

        //public static void ReSetAdminGroupList()
        //{
        //    AdminCaches.RemoveObject(CacheKeys.FORUM_ADMIN_GROUP_LIST);
        //}

        //public static void ReSetUserGroupList()
        //{
        //    AdminCaches.RemoveObject(CacheKeys.FORUM_USER_GROUP_LIST);
        //}

        public static void ReSetModeratorList()
        {
            AdminCaches.RemoveObject(CacheKeys.FORUM_MODERATOR_LIST);
        }

        public static void ReSetAnnouncementList()
        {
            AdminCaches.RemoveObject(CacheKeys.FORUM_ANNOUNCEMENT_LIST);
        }

        public static void ReSetSimplifiedAnnouncementList()
        {
            AdminCaches.RemoveObject(CacheKeys.FORUM_SIMPLIFIED_ANNOUNCEMENT_LIST);
        }

        public static void ReSetForumListBoxOptions()
        {
            AdminCaches.RemoveObject(CacheKeys.FORUM_UI_FORUM_LIST_BOX_OPTIONS);
        }

        public static void ReSetSmiliesList()
        {
            AdminCaches.RemoveObject(CacheKeys.FORUM_UI_SMILIES_LIST);
            AdminCaches.RemoveObject(CacheKeys.FORUM_UI_SMILIES_LIST_WITH_INFO);
        }

        public static void ReSetIconsList()
        {
            AdminCaches.RemoveObject(CacheKeys.FORUM_UI_ICONS_LIST);
        }

        public static void ReSetCustomEditButtonList()
        {
            AdminCaches.RemoveObject(CacheKeys.FORUM_UI_CUSTOM_EDIT_BUTTON_LIST);
			//AdminCaches.RemoveObject(CacheKeys.FORUM_UI_CUSTOM_EDIT_BUTTON_INFO);
        }

        public static void ReSetConfig()
        {
            AdminCaches.RemoveObject(CacheKeys.FORUM_SETTING);
        }

        public static void ReSetScoreset()
        {
            AdminCaches.RemoveObject(CacheKeys.FORUM_SCORESET);
            AdminCaches.RemoveObject(CacheKeys.FORUM_VALID_SCORE_NAME);
            AdminCaches.RemoveObject(CacheKeys.FORUM_SCORESET_CREDITS_TAX);
            AdminCaches.RemoveObject(CacheKeys.FORUM_SCORESET_CREDITS_TRANS);
            AdminCaches.RemoveObject(CacheKeys.FORUM_SCORESET_TRANSFER_MIN_CREDITS);
            AdminCaches.RemoveObject(CacheKeys.FORUM_SCORESET_EXCHANGE_MIN_CREDITS);
            AdminCaches.RemoveObject(CacheKeys.FORUM_SCORESET_MAX_INC_PER_THREAD);
            AdminCaches.RemoveObject(CacheKeys.FORUM_SCORESET_MAX_CHARGE_SPAN);
            AdminCaches.RemoveObject(CacheKeys.FORUM_VALID_SCORE_UNIT);
        }

        public static void ReSetSiteUrls()
        {
            AdminCaches.RemoveObject(CacheKeys.FORUM_URLS);
        }

        public static void ReSetStatistics()
        {
            //AdminCaches.RemoveObject(CacheKeys.FORUM_STATISTICS);
        }

        public static void ReSetAttachmentTypeArray()
        {
			//AdminCaches.RemoveObject(CacheKeys.FORUM_FORUM_SETTING_ATTACHMENT_TYPE);
        }

        public static void ReSetTemplateListBoxOptionsCache()
        {
            AdminCaches.RemoveObject(CacheKeys.FORUM_UI_TEMPLATE_LIST_BOX_OPTIONS_FOR_FORUMINDEX);
            AdminCaches.RemoveObject(CacheKeys.FORUM_UI_TEMPLATE_LIST_BOX_OPTIONS);
        }

        //public static void ReSetOnlineGroupIconList()
        //{
        //    AdminCaches.RemoveObject(CacheKeys.FORUM_UI_ONLINE_ICON_LIST);
        //    //AdminCaches.RemoveObject(CacheKeys.FORUM_ONLINE_ICON_TABLE);
        //}

        public static void ReSetForumLinkList()
        {
            AdminCaches.RemoveObject(CacheKeys.FORUM_FORUM_LINK_LIST);
        }

        public static void ReSetBanWordList()
        {
            //AdminCaches.RemoveObject(CacheKeys.FORUM_BAN_WORD_LIST);
        }

        public static void ReSetForumList()
        {
            AdminCaches.RemoveObject(CacheKeys.FORUM_FORUM_LIST);
        }

        public static void ReSetOnlineUserTable()
        {
        }

        public static void ReSetRss()
        {
            AdminCaches.RemoveObject(CacheKeys.FORUM_RSS);
        }

        public static void ReSetForumRssXml(int fid)
        {
            AdminCaches.RemoveObject(string.Format(CacheKeys.FORUM_RSS_FORUM, fid));
        }

        public static void ReSetRssXml()
        {
            AdminCaches.RemoveObject(CacheKeys.FORUM_RSS_INDEX);
        }

        public static void ReSetValidTemplateIDList()
        {
            AdminCaches.RemoveObject(CacheKeys.FORUM_TEMPLATE_ID_LIST);
        }

        public static void ReSetValidScoreName()
        {
            AdminCaches.RemoveObject(CacheKeys.FORUM_VALID_SCORE_NAME);
        }

        //public static void ReSetMedalsList()
        //{
        //    AdminCaches.RemoveObject(CacheKeys.FORUM_UI_MEDALS_LIST);
        //}

        public static void ReSetDBlinkAndTablePrefix()
        {
            AdminCaches.RemoveObject(CacheKeys.FORUM_BASE_SETTING_DBCONNECTSTRING);
            AdminCaches.RemoveObject(CacheKeys.FORUM_BASE_SETTING_TABLE_PREFIX);
        }

        public static void ReSetLastPostTableName()
        {
            AdminCaches.RemoveObject(CacheKeys.FORUM_LAST_POST_TABLE_NAME);
        }

        public static void ReSetAllPostTableName()
        {
            AdminCaches.RemoveObject(CacheKeys.FORUM_POST_TABLE_NAME);
        }

        public static void ReSetAdsList()
        {
            AdminCaches.RemoveObject(CacheKeys.FORUM_ADVERTISEMENTS);
        }

        public static void ReSetStatisticsSearchtime()
        {
            //AdminCaches.RemoveObject(CacheKeys.FORUM_STATISTICS_SEARCHTIME);
        }

        public static void ReSetStatisticsSearchcount()
        {
            //AdminCaches.RemoveObject(CacheKeys.FORUM_STATISTICS_SEARCHCOUNT);
        }

        public static void ReSetCommonAvatarList()
        {
            AdminCaches.RemoveObject(CacheKeys.FORUM_COMMON_AVATAR_LIST);
        }

        public static void ReSetJammer()
        {
            AdminCaches.RemoveObject(CacheKeys.FORUM_UI_JAMMER);
        }

        public static void ReSetMagicList()
        {
            AdminCaches.RemoveObject(CacheKeys.FORUM_MAGIC_LIST);
        }

        public static void ReSetScorePaySet()
        {
            AdminCaches.RemoveObject(CacheKeys.FORUM_SCORE_PAY_SET);
        }

        public static void ReSetPostTableInfo()
        {
            AdminCaches.RemoveObject(CacheKeys.FORUM_POST_TABLE_NAME);
            AdminCaches.RemoveObject(CacheKeys.FORUM_LAST_POST_TABLE_NAME);
        }

        public static void ReSetTopiclistByFid(string fid)
        {
            AdminCaches.RemoveObject(string.Format(CacheKeys.FORUM_TOPIC_LIST_FID, fid));
        }

        public static void ReSetDigestTopicList(int count)
        {
            AdminCaches.ReSetFocusTopicList(count, -1, 0, TopicTimeType.All, TopicOrderType.ID, true);
        }

        public static void ReSetDigestTopicList(int count, int fid, TopicTimeType timetype, TopicOrderType ordertype)
        {
            AdminCaches.ReSetFocusTopicList(count, -1, fid, timetype, ordertype, true);
        }

        public static void ReSetHotTopicList(int count, int views)
        {
            AdminCaches.ReSetFocusTopicList(count, views, 0, TopicTimeType.All, TopicOrderType.ID, false);
        }

        public static void ReSetHotTopicList(int count, int views, int fid, TopicTimeType timetype, TopicOrderType ordertype)
        {
            AdminCaches.ReSetFocusTopicList(count, views, fid, timetype, ordertype, false);
        }

        public static void ReSetRecentTopicList(int count)
        {
            AdminCaches.ReSetFocusTopicList(count, -1, 0, TopicTimeType.All, TopicOrderType.ID, false);
        }

        private static void ReSetFocusTopicList(int count, int views, int fid, TopicTimeType timetype, TopicOrderType ordertype, bool isdigest)
        {
            string key = string.Format(CacheKeys.FORUM_TOPIC_LIST_FORMAT, new object[]
            {
                count,
                views,
                fid,
                timetype,
                ordertype,
                isdigest
            });
            AdminCaches.RemoveObject(key);
        }

        public static void ResetAlbumCategory()
        {
            AdminCaches.RemoveObject(CacheKeys.SPACE_ALBUM_CATEGORY);
        }

        public static void ReSetNavPopupMenu()
        {
            AdminCaches.RemoveObject(CacheKeys.FORUM_FORUM_LIST_MENU_DIV);
        }

        public static void ReSetAllCache()
        {
            //AdminCaches.ReSetAdminGroupList();
            //AdminCaches.ReSetUserGroupList();
            AdminCaches.ReSetModeratorList();
            AdminCaches.ReSetAnnouncementList();
            AdminCaches.ReSetSimplifiedAnnouncementList();
            AdminCaches.ReSetForumListBoxOptions();
            AdminCaches.ReSetSmiliesList();
            AdminCaches.ReSetIconsList();
            AdminCaches.ReSetCustomEditButtonList();
            AdminCaches.ReSetConfig();
            AdminCaches.ReSetScoreset();
            AdminCaches.ReSetSiteUrls();
            AdminCaches.ReSetStatistics();
            AdminCaches.ReSetAttachmentTypeArray();
            AdminCaches.ReSetTemplateListBoxOptionsCache();
            //AdminCaches.ReSetOnlineGroupIconList();
            AdminCaches.ReSetForumLinkList();
            AdminCaches.ReSetBanWordList();
            AdminCaches.ReSetForumList();
            AdminCaches.ReSetRss();
            AdminCaches.ReSetRssXml();
            AdminCaches.ReSetValidTemplateIDList();
            AdminCaches.ReSetValidScoreName();
            //AdminCaches.ReSetMedalsList();
            AdminCaches.ReSetDBlinkAndTablePrefix();
            AdminCaches.ReSetAllPostTableName();
            AdminCaches.ReSetLastPostTableName();
            AdminCaches.ReSetAdsList();
            AdminCaches.ReSetStatisticsSearchtime();
            AdminCaches.ReSetStatisticsSearchcount();
            AdminCaches.ReSetCommonAvatarList();
            AdminCaches.ReSetJammer();
            AdminCaches.ReSetMagicList();
            AdminCaches.ReSetScorePaySet();
            AdminCaches.ReSetPostTableInfo();
            AdminCaches.ReSetDigestTopicList(16);
            AdminCaches.ReSetHotTopicList(16, 30);
            AdminCaches.ReSetRecentTopicList(16);
            AdminCaches.ResetAlbumCategory();
            AdminCaches.EditDntConfig();

            Online.ResetOnlineList();
        }

        public static bool EditDntConfig()
        {
            var config = BaseConfigInfo.Current;

            return true;
        }
    }
}