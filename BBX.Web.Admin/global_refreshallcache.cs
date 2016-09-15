using System;
using System.Web.UI;
using BBX.Common;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web.Admin
{
    public class global_refreshallcache : Page
    {
        private void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                int @int = DNTRequest.GetInt("opnumber", 0);
                int num = -1;
                switch (@int)
                {
                    case 1:

                        //Caches.ReSetAdminGroupList();
                        num = 2;
                        break;
                    case 2:
                        //Caches.ReSetUserGroupList();
                        num = 3;
                        break;
                    case 3:
                        Caches.ReSetModeratorList();
                        num = 4;
                        break;
                    case 4:
                        Caches.ReSetAnnouncementList();
                        Caches.ReSetSimplifiedAnnouncementList();
                        num = 5;
                        break;
                    case 5:
                        Caches.ReSetSimplifiedAnnouncementList();
                        num = 6;
                        break;
                    case 6:
                        Caches.ReSetForumListBoxOptions();
                        num = 7;
                        break;
                    case 7:
                        Caches.ReSetSmiliesList();
                        num = 8;
                        break;
                    case 8:
                        Caches.ReSetIconsList();
                        num = 9;
                        break;
                    case 9:
                        Caches.ReSetCustomEditButtonList();
                        num = 10;
                        break;
                    case 10:
                        num = 11;
                        break;
                    case 11:
                        Caches.ReSetScoreset();
                        num = 12;
                        break;
                    case 12:
                        Caches.ReSetSiteUrls();
                        num = 13;
                        break;
                    case 13:
                        Caches.ReSetStatistics();
                        num = 14;
                        break;
                    case 14:
                        Caches.ReSetAttachmentTypeArray();
                        num = 15;
                        break;
                    case 15:
                        Caches.ReSetTemplateListBoxOptionsCache();
                        num = 16;
                        break;
                    case 16:
                        //Caches.ReSetOnlineGroupIconList();
                        num = 17;
                        break;
                    case 17:
                        Caches.ReSetForumLinkList();
                        num = 18;
                        break;
                    case 18:
                        Caches.ReSetBanWordList();
                        num = 19;
                        break;
                    case 19:
                        Caches.ReSetForumList();
                        num = 20;
                        break;
                    case 20:
                        Caches.ReSetOnlineUserTable();
                        num = 21;
                        break;
                    case 21:
                        Caches.ReSetRss();
                        num = 22;
                        break;
                    case 22:
                        Caches.ReSetRssXml();
                        num = 23;
                        break;
                    case 23:
                        Caches.ReSetValidTemplateIDList();
                        num = 24;
                        break;
                    case 24:
                        Caches.ReSetValidScoreName();
                        num = 25;
                        break;
                    case 25:
                        //Caches.ReSetMedalsList();
                        num = 26;
                        break;
                    case 26:
                        Caches.ReSetDBlinkAndTablePrefix();
                        num = 27;
                        break;
                    case 27:
                        //Caches.ReSetAllPostTableName();
                        num = 28;
                        break;
                    case 28:
                        //Caches.ReSetLastPostTableName();
                        num = 29;
                        break;
                    case 29:
                        Caches.ReSetAdsList();
                        num = 30;
                        break;
                    case 30:
                        Caches.ReSetStatisticsSearchtime();
                        num = 31;
                        break;
                    case 31:
                        Caches.ReSetStatisticsSearchcount();
                        num = 32;
                        break;
                    case 32:
                        Caches.ReSetCommonAvatarList();
                        num = 33;
                        break;
                    case 33:
                        Caches.ReSetJammer();
                        num = 34;
                        break;
                    case 34:
                        Caches.ReSetMagicList();
                        num = 35;
                        break;
                    case 35:
                        Caches.ReSetScorePaySet();
                        num = 36;
                        break;
                    case 36:
                        //Caches.ReSetPostTableInfo();
                        num = 37;
                        break;
                    case 37:
                        Caches.ReSetDigestTopicList(16);
                        num = 38;
                        break;
                    case 38:
                        Caches.ReSetHotTopicList(16, 30);
                        num = 39;
                        break;
                    case 39:
                        Caches.ReSetRecentTopicList(16);
                        num = 40;
                        break;
                    case 40:
                        Caches.EditDntConfig();
                        num = 41;
                        break;
                    case 41:
                        Online.ResetOnlineList();
                        num = 42;
                        break;
                    case 42:
                        Caches.ReSetNavPopupMenu();
                        num = -1;
                        break;
                }
                base.Response.Write(num);
                base.Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1.0);
                base.Response.Expires = -1;
                base.Response.End();
            }
        }
    }
}