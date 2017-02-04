using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using BBX.Cache;
using BBX.Common;
using BBX.Config;
using BBX.Entity;

namespace BBX.Forum
{
    public class Caches
    {
        private static object lockHelper = new object();
        //private static Regex r = new Regex("\\{(\\d+)\\}", Utils.GetRegexCompiledOptions());

        public static string GetForumListBoxOptionsCache(bool cateUnselectable)
        {
            var cacheService = XCache.Current;
            string text = cacheService.RetrieveObject(CacheKeys.FORUM_UI_FORUM_LIST_BOX_OPTIONS) as string;
            if (text.IsNullOrEmpty())
            {
                StringBuilder stringBuilder = new StringBuilder();
                //Caches.AddOptionTree(BBX.Data.Forums.GetVisibleForumList(), "0", cateUnselectable, stringBuilder);
                Caches.AddOptionTree(XForum.GetVisibleForumList(), 0, cateUnselectable, stringBuilder);
                text = stringBuilder.ToString();
                XCache.Add(CacheKeys.FORUM_UI_FORUM_LIST_BOX_OPTIONS, text);
            }
            return text;
        }

        private static void AddOptionTree(List<IXForum> list, Int32 parentid, bool cateUnselectable, StringBuilder sb)
        {
            //DataRow[] array = list.Select("parentid=" + parentid);
            //DataRow[] array2 = array;
            //for (int i = 0; i < array2.Length; i++)
            foreach (var forum in list)
            {
                if (forum.ParentID != parentid) continue;

                //DataRow dataRow = array2[i];
                if (cateUnselectable && forum.Layer == 0)
                {
                    sb.AppendFormat("<optgroup label=\"--{0}\">", forum.Name.Trim());
                    sb.Append(Utils.GetSpacesString(forum.Layer));
                    sb.Append(forum.Name.Trim());
                }
                else
                {
                    sb.AppendFormat("<option value=\"{0}\">", forum.ID);
                    sb.Append(Utils.GetSpacesString(forum.Layer));
                    sb.Append(forum.Name.Trim());
                    sb.Append("</option>");
                }
                Caches.AddOptionTree(list, forum.ID, cateUnselectable, sb);
                if (cateUnselectable && forum.Layer == 0)
                {
                    sb.Append("</optgroup>");
                }
            }
        }

        //public static string GetForumListBoxOptionsCache()
        //{
        //	return Caches.GetForumListBoxOptionsCache(false);
        //}

        public static string GetForumListMenuDivCache(int usergroupid, int userid, string extname)
        {
            var cacheService = XCache.Current;
            string text = cacheService.RetrieveObject(CacheKeys.FORUM_FORUM_LIST_MENU_DIV) as string;
            if (text.IsNullOrEmpty())
            {
                var sb = new StringBuilder();
                var forumList = XForum.Root.Childs;
                if (forumList.Count > 0)
                {
                    sb.Append("<div class=\"popupmenu_popup\" id=\"forumlist_menu\" style=\"overflow-y: auto; display:none\">");
                    foreach (var item in forumList)
                    {
                        if (item.Visible && item.AllowView(7) && item.Layer == 0)
                        {
                            sb.AppendFormat("<dl><dt><a href=\"{0}\">{1}</a></dt><dd><ul>", BaseConfigs.GetForumPath + Urls.ShowForumAspxRewrite(item.ID, 0, item.Field.RewriteName), item.Name);
                            foreach (var elm in item.Childs)
                            {
                                if (elm.Layer == 1 && elm.Visible)
                                {
                                    sb.AppendFormat("<li><a href=\"{0}\">{1}</a></li>", BaseConfigs.GetForumPath + Urls.ShowForumAspxRewrite(elm.ID, 0, elm.Field.RewriteName), elm.Name);
                                }
                            }
                            sb.Append("</ul></dd></dl>");
                        }
                    }
                }
                sb.Append("</div>");
                text = sb.ToString().Replace("<dd><ul></ul></dd>", "");
                XCache.Add(CacheKeys.FORUM_FORUM_LIST_MENU_DIV, text);
            }
            return text;
        }

        public static string GetTemplateListBoxOptionsCache(bool topMenu)
        {
            lock (lockHelper)
            {
                var key1 = CacheKeys.FORUM_UI_TEMPLATE_LIST_BOX_OPTIONS_FOR_FORUMINDEX;
                var key2 = CacheKeys.FORUM_UI_TEMPLATE_LIST_BOX_OPTIONS;
                string text = topMenu ? XCache.Retrieve<String>(key1) : XCache.Retrieve<String>(key2);
                if (text.IsNullOrEmpty())
                {
                    var sb = new StringBuilder();
                    //var validTemplateList = Templates.GetValidTemplateList();
                    foreach (var tmp in Template.GetValids())
                    {
                        if (topMenu)
                        {
                            sb.AppendFormat("<li><a onclick=\"window.location.href='{0}showtemplate.aspx?templateid={1}';return false;\" href=\"###\"><i style=\"background: url(&quot;templates/{2}/about.png&quot;) no-repeat scroll 0% 0% transparent;\">&nbsp;</i><span>{3}</span><em></em></a></li>", BaseConfigs.GetForumPath, tmp.ID, tmp.Directory, tmp.Name.Trim());
                        }
                        else
                        {
                            sb.AppendFormat("<li><a onclick=\"window.location.href='{0}showtemplate.aspx?templateid={1}';return false;\" href=\"###\">{2}</a></li>", BaseConfigs.GetForumPath, tmp.ID, tmp.Name.Trim());
                        }
                    }
                    text = sb.ToString();
                    XCache.Add(topMenu ? key1 : key2, text);
                    //validTemplateList.Dispose();
                }
                return text;
            }
        }

        public static string GetTemplateListBoxOptionsCache()
        {
            return Caches.GetTemplateListBoxOptionsCache(false);
        }

        //public static string GetSmiliesCache()
        //{
        //    string text = XCache.Retrieve<String>(CacheKeys.FORUM_UI_SMILIES_LIST);
        //    if (text.IsNullOrEmpty())
        //    {
        //        var sb = new StringBuilder();
        //        //DataTable smiliesListDataTable = BBX.Data.Smilies.GetSmiliesListDataTable();
        //        var list = Smilie.FindAllWithCache();
        //        foreach (var sm in list)
        //        {
        //            if (sm.Type != 0) continue;

        //            sb.AppendFormat("'{0}': [\r\n", sm.Code.Trim().Replace("'", "\\'"));
        //            bool flag = false;
        //            foreach (var sm2 in list)
        //            {
        //                if (sm2.Type == sm.ID)
        //                {
        //                    sb.Append("{'code' : '");
        //                    sb.Append(sm2.Code.Trim().Replace("'", "\\'"));
        //                    sb.Append("', 'url' : '");
        //                    sb.Append(sm2.Url.Trim().Replace("'", "\\'"));
        //                    sb.Append("'},\r\n");
        //                    flag = true;
        //                }
        //            }
        //            if (sb.Length > 0 && flag)
        //            {
        //                sb.Remove(sb.Length - 3, 3);
        //            }
        //            sb.Append("\r\n],\r\n");
        //        }
        //        sb.Remove(sb.Length - 3, 3);
        //        text = sb.ToString();
        //        XCache.Add(CacheKeys.FORUM_UI_SMILIES_LIST, text);
        //    }
        //    return text;
        //}

        //public static string GetSmiliesFirstPageCache()
        //{
        //	string text = XCache.Retrieve<String>("/Forum/UI/SmiliesListFirstPage");
        //	if (text.IsNullOrEmpty())
        //	{
        //		var sb = new StringBuilder();
        //		//DataTable smiliesListDataTable = BBX.Data.Smilies.GetSmiliesListDataTable();
        //		var list = Smilie.FindAllWithCache();
        //		foreach (var sm in list)
        //		{
        //			if (sm.Type != 0) continue;

        //			sb.AppendFormat("'{0}': [\r\n", sm.Code.Trim().Replace("'", "\\'"));
        //			bool flag = false;
        //			int num = 0;
        //			foreach (var sm2 in list)
        //			{
        //				if (sm2.Type == sm.ID && num < 16)
        //				{
        //					sb.Append("{'code' : '");
        //					sb.Append(sm2.Code.Trim().Replace("'", "\\'"));
        //					sb.Append("', 'url' : '");
        //					sb.Append(sm2.Url.Trim().Replace("'", "\\'"));
        //					sb.Append("'},\r\n");
        //					flag = true;
        //					num++;
        //				}
        //			}
        //			if (sb.Length > 0 && flag)
        //			{
        //				sb.Remove(sb.Length - 3, 3);
        //			}
        //			sb.Append("\r\n],\r\n");
        //			break;
        //		}
        //		sb.Remove(sb.Length - 3, 3);
        //		text = sb.ToString();
        //		XCache.Add("/Forum/UI/SmiliesListFirstPage", text);
        //	}
        //	return text;
        //}

        //public static DataTable GetSmilieTypesCache()
        //{
        //    DataTable dataTable = XCache.Retrieve<DataTable>("/Forum/UI/SmiliesTypeList");
        //    if (dataTable == null || dataTable.Rows.Count == 0)
        //    {
        //        dataTable = BBX.Data.Smilies.GetSmiliesTypes();
        //        XCache.Add("/Forum/UI/SmiliesTypeList", dataTable);
        //    }
        //    return dataTable;
        //}

        public static string GetCustomEditButtonList()
        {
            string result;
            lock (lockHelper)
            {
                string text = XCache.Retrieve<String>(CacheKeys.FORUM_UI_CUSTOM_EDIT_BUTTON_LIST);
                if (text == null)
                {
                    var sb = new StringBuilder();
                    //var customEditButtonList = DatabaseProvider.GetInstance().GetCustomEditButtonList();
                    var customEditButtonList = BbCode.FindAllByAvailable(1);
                    //try
                    //{
                    foreach (var bb in customEditButtonList)
                    //while (customEditButtonList.Read())
                    {
                        sb.AppendFormat(",'{0}':['", Utils.ReplaceStrToScript(bb.Tag));
                        sb.Append(Utils.ReplaceStrToScript(bb.Tag));
                        sb.Append("','");
                        sb.Append(Utils.ReplaceStrToScript(bb.Icon));
                        sb.Append("','");
                        sb.Append(Utils.ReplaceStrToScript(bb.Explanation));
                        sb.Append("',['");
                        sb.Append(Utils.ReplaceStrToScript(bb.ParamsDescript).Replace(",", "','"));
                        sb.Append("'],['");
                        sb.Append(Utils.ReplaceStrToScript(bb.ParamsDefValue).Replace(",", "','"));
                        sb.Append("'],");
                        sb.Append(Utils.ReplaceStrToScript(bb.Params + ""));
                        sb.Append("]");
                    }
                    if (sb.Length > 0)
                    {
                        sb.Remove(0, 1);
                    }
                    text = Utils.ClearBR(sb.ToString());
                    XCache.Add(CacheKeys.FORUM_UI_CUSTOM_EDIT_BUTTON_LIST, text);
                    //}
                    //finally
                    //{
                    //	customEditButtonList.Close();
                    //}
                }
                result = text;
            }
            return result;
        }

        //public static string GetOnlineGroupIconList()
        //{
        //    var cacheService = XCache.Current;
        //    string text = cacheService.RetrieveObject(CacheKeys.FORUM_UI_ONLINE_ICON_LIST) as string;
        //    if (text.IsNullOrEmpty())
        //    {
        //        StringBuilder stringBuilder = new StringBuilder();
        //        IDataReader onlineGroupIconList = DatabaseProvider.GetInstance().GetOnlineGroupIconList();
        //        string getForumPath = BaseConfigs.GetForumPath;
        //        try
        //        {
        //            while (onlineGroupIconList.Read())
        //            {
        //                stringBuilder.AppendFormat("<img src=\"{0}images/groupicons/{1}\" /> {2} &nbsp; &nbsp; &nbsp; ", getForumPath, onlineGroupIconList["img"], onlineGroupIconList["title"]);
        //            }
        //            text = stringBuilder.ToString();
        //            XCache.Add(CacheKeys.FORUM_UI_ONLINE_ICON_LIST, text);
        //        }
        //        finally
        //        {
        //            onlineGroupIconList.Close();
        //        }
        //    }
        //    return text;
        //}

        //public static string[,] GetBanWordList()
        //{
        //    var cacheService = XCache.Current;
        //    string[,] array = cacheService.RetrieveObject(CacheKeys.FORUM_BAN_WORD_LIST) as string[,];
        //    if (array == null)
        //    {
        //        //DataTable banWordList = DatabaseProvider.GetInstance().GetBanWordList();
        //        var banWordList = Word.FindAllWithCache();
        //        array = new string[banWordList.Count, 2];
        //        string text = "";
        //        var i = 0;
        //        foreach (var item in banWordList)
        //        {
        //            text = item.Key.Trim();
        //            foreach (Match match in Caches.r.Matches(text))
        //            {
        //                text = text.Replace(match.Groups[0].ToString(), match.Groups[0].ToString().Replace("{", ".{0,"));
        //            }
        //            array[i, 0] = Word.ConvertRegexCode(text);
        //            array[i, 1] = item.Replacement.Trim();
        //            i++;
        //        }
        //        XCache.Add(CacheKeys.FORUM_BAN_WORD_LIST, array);
        //        //banWordList.Dispose();
        //    }
        //    return array;
        //}

        //public static DataTable GetAvatarList()
        //{
        //	var cacheService = XCache.Current;
        //	DataTable dataTable = cacheService.RetrieveObject(CacheKeys.FORUM_COMMON_AVATAR_LIST) as DataTable;
        //	if (dataTable == null)
        //	{
        //		dataTable = new DataTable();
        //		dataTable.Columns.Add("filename", typeof(String));
        //		DirectoryInfo directoryInfo = new DirectoryInfo(Utils.GetMapPath(BaseConfigs.GetForumPath + "avatars/common/"));
        //		FileSystemInfo[] fileSystemInfos = directoryInfo.GetFileSystemInfos();
        //		for (int i = 0; i < fileSystemInfos.Length; i++)
        //		{
        //			FileSystemInfo fileSystemInfo = fileSystemInfos[i];
        //			if (fileSystemInfo != null)
        //			{
        //				string text = fileSystemInfo.Extension.ToLower();
        //				if (text.Equals(".jpg") || text.Equals(".gif") || text.Equals(".png"))
        //				{
        //					DataRow dataRow = dataTable.NewRow();
        //					dataRow["filename"] = "avatars/common/" + fileSystemInfo.Name;
        //					dataTable.Rows.Add(dataRow);
        //				}
        //			}
        //		}
        //		XCache.Add(CacheKeys.FORUM_COMMON_AVATAR_LIST, dataTable);
        //	}
        //	return dataTable;
        //}

        public static string GetJammer()
        {
            var cacheService = XCache.Current;
            string text = cacheService.RetrieveObject(CacheKeys.FORUM_UI_JAMMER) as string;
            if (text == null)
            {
                Random random = new Random((int)DateTime.Now.Ticks);
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < 10; i++)
                {
                    stringBuilder.Append(Convert.ToChar(random.Next(1, 256)));
                }
                stringBuilder.Append(HttpContext.Current.Request.Url.Authority);
                for (int j = 0; j < 10; j++)
                {
                    stringBuilder.Append(Convert.ToChar(random.Next(1, 256)));
                }
                text = stringBuilder.ToString();
                text = Utils.HtmlEncode(text);
                if (stringBuilder.Length > 0)
                {
                    stringBuilder.Remove(0, stringBuilder.Length);
                }
                text = stringBuilder.AppendFormat("<span class=\"jammer\">{0}</span>", text).ToString();
                XCache.Add(CacheKeys.FORUM_UI_JAMMER, text, 720);
            }
            return text;
        }

        //public static DataTable GetMedalsList()
        //{
        //    var cacheService = XCache.Current;
        //    DataTable dataTable = cacheService.RetrieveObject(CacheKeys.FORUM_UI_MEDALS_LIST) as DataTable;
        //    if (dataTable == null)
        //    {
        //        dataTable = DatabaseProvider.GetInstance().GetMedalsList();
        //        string forumpath = BaseConfigInfo.Current.Forumpath;
        //        foreach (DataRow dataRow in dataTable.Rows)
        //        {
        //            if (dataRow["available"].ToString() == "1")
        //            {
        //                if (!Utils.StrIsNullOrEmpty(dataRow["image"].ToString()))
        //                {
        //                    //if (EntLibConfigInfo.Current != null && !EntLibConfigInfo.Current.Medaldir.IsNullOrEmpty())
        //                    //{
        //                    //    dataRow["image"] = "<img border=\"0\" src=\"" + EntLibConfigInfo.Current.Medaldir + dataRow["image"] + "\" alt=\"" + dataRow["name"] + "\" title=\"" + dataRow["name"] + "\" />";
        //                    //}
        //                    //else
        //                    {
        //                        dataRow["image"] = "<img border=\"0\" src=\"" + forumpath + "images/medals/" + dataRow["image"] + "\" alt=\"" + dataRow["name"] + "\" title=\"" + dataRow["name"] + "\" />";
        //                    }
        //                }
        //                else
        //                {
        //                    dataRow["image"] = "";
        //                }
        //            }
        //            else
        //            {
        //                dataRow["image"] = "";
        //            }
        //        }
        //        XCache.Add(CacheKeys.FORUM_UI_MEDALS_LIST, dataTable);
        //    }
        //    return dataTable;
        //}

        //public static string GetMedalsList(string mdealList)
        //{
        //    var medalsList = Caches.GetMedalsList();
        //    var array = Utils.SplitString(mdealList, ",");
        //    var stringBuilder = new StringBuilder();
        //    for (int i = 0; i < array.Length; i++)
        //    {
        //        var id = TypeConverter.StrToInt(array[i], 1) - 1;
        //        if (id >= 0 && id < medalsList.Rows.Count) stringBuilder.Append(medalsList.Rows[id]["image"]);
        //    }
        //    return stringBuilder.ToString();
        //}

        //public static TopicIdentify GetTopicIdentify(int identifyid)
        //{
        //    foreach (TopicIdentify current in Caches.GetTopicIdentifyCollection())
        //    {
        //        if (current.Identifyid == identifyid)
        //        {
        //            return current;
        //        }
        //    }
        //    return new TopicIdentify();
        //}

        //public static string GetTopicIdentifyFileNameJsArray()
        //{
        //    var cacheService = DNTCache.Current;
        //    string text = cacheService.RetrieveObject("/Forum/TopicIndentifysJsArray") as string;
        //    if (text.IsNullOrEmpty())
        //    {
        //        Caches.GetTopicIdentifyCollection();
        //        text = (cacheService.RetrieveObject("/Forum/TopicIndentifysJsArray") as string);
        //    }
        //    return text;
        //}

        //public static List<IpInfo> GetBannedIpList()
        //{
        //    var list = XCache.Current.RetrieveObject("/Forum/BannedIp") as List<IpInfo>;
        //    if (list == null)
        //    {
        //        list = Ips.GetBannedIpList();
        //        XCache.Add("/Forum/BannedIp", list);
        //    }
        //    return list;
        //}

        //public static SortedList<int, string> GetTopicTypeArray()
        //{
        //    var cacheService = XCache.Current;
        //    SortedList<int, string> sortedList = cacheService.RetrieveObject("/Forum/TopicTypes") as SortedList<int, string>;
        //    if (sortedList == null)
        //    {
        //        sortedList = new SortedList<int, string>();
        //        DataTable topicTypeList = DatabaseProvider.GetInstance().GetTopicTypeList();
        //        if (topicTypeList.Rows.Count > 0)
        //        {
        //            foreach (DataRow dataRow in topicTypeList.Rows)
        //            {
        //                if (!Utils.StrIsNullOrEmpty(dataRow["typeid"].ToString()) && !Utils.StrIsNullOrEmpty(dataRow["name"].ToString()))
        //                {
        //                    sortedList.Add(TypeConverter.ObjectToInt(dataRow["typeid"]), dataRow["name"].ToString());
        //                }
        //            }
        //        }
        //        XCache.Add("/Forum/TopicTypes", sortedList);
        //    }
        //    return sortedList;
        //}

        //public static List<TopicIdentify> GetTopicIdentifyCollection()
        //{
        //    var cacheService = DNTCache.Current;
        //    List<TopicIdentify> list = cacheService.RetrieveObject("/Forum/TopicIdentifys") as List<TopicIdentify>;
        //    if (list == null)
        //    {
        //        list = new List<TopicIdentify>();
        //        IDataReader topicsIdentifyItem = DatabaseProvider.GetInstance().GetTopicsIdentifyItem();
        //        StringBuilder stringBuilder = new StringBuilder("<script type='text/javascript'>var topicidentify = { ");
        //        while (topicsIdentifyItem.Read())
        //        {
        //            list.Add(new TopicIdentify
        //            {
        //                Identifyid = TypeConverter.ObjectToInt(topicsIdentifyItem["identifyid"]),
        //                Name = topicsIdentifyItem["name"].ToString(),
        //                Filename = topicsIdentifyItem["filename"].ToString()
        //            });
        //            stringBuilder.AppendFormat("'{0}':'{1}',", topicsIdentifyItem["identifyid"], topicsIdentifyItem["filename"]);
        //        }
        //        topicsIdentifyItem.Close();
        //        stringBuilder.Remove(stringBuilder.Length - 1, 1);
        //        stringBuilder.Append("};</script>");
        //        XCache.Add("/Forum/TopicIdentifys", list);
        //        XCache.Add("/Forum/TopicIndentifysJsArray", stringBuilder.ToString());
        //    }
        //    return list;
        //}

        private static void RemoveObject(string key)
        {
            XCache.Remove(key);
        }

        //public static void ReSetAdminGroupList()
        //{
        //    Caches.RemoveObject(CacheKeys.FORUM_ADMIN_GROUP_LIST);
        //}

        //public static void ReSetUserGroupList()
        //{
        //    Caches.RemoveObject(CacheKeys.FORUM_USER_GROUP_LIST);
        //}

        public static void ReSetModeratorList()
        {
            Caches.RemoveObject(CacheKeys.FORUM_MODERATOR_LIST);
        }

        public static void ReSetAnnouncementList()
        {
            Caches.RemoveObject(CacheKeys.FORUM_ANNOUNCEMENT_LIST);
        }

        public static void ReSetSimplifiedAnnouncementList()
        {
            Caches.RemoveObject(CacheKeys.FORUM_SIMPLIFIED_ANNOUNCEMENT_LIST);
        }

        public static void ReSetForumListBoxOptions()
        {
            Caches.RemoveObject(CacheKeys.FORUM_UI_FORUM_LIST_BOX_OPTIONS);
        }

        public static void ReSetSmiliesList()
        {
            Caches.RemoveObject(CacheKeys.FORUM_UI_SMILIES_LIST);
            Caches.RemoveObject(CacheKeys.FORUM_UI_SMILIES_LIST_WITH_INFO);
        }

        public static void ReSetIconsList()
        {
            Caches.RemoveObject(CacheKeys.FORUM_UI_ICONS_LIST);
        }

        public static void ReSetCustomEditButtonList()
        {
            Caches.RemoveObject(CacheKeys.FORUM_UI_CUSTOM_EDIT_BUTTON_LIST);
            //Caches.RemoveObject(CacheKeys.FORUM_UI_CUSTOM_EDIT_BUTTON_INFO);
        }

        public static void ReSetConfig()
        {
            Caches.RemoveObject(CacheKeys.FORUM_SETTING);
        }

        public static void ReSetScoreset()
        {
            Caches.RemoveObject(CacheKeys.FORUM_SCORESET);
            Caches.RemoveObject(CacheKeys.FORUM_VALID_SCORE_NAME);
            Caches.RemoveObject(CacheKeys.FORUM_SCORESET_CREDITS_TAX);
            Caches.RemoveObject(CacheKeys.FORUM_SCORESET_CREDITS_TRANS);
            Caches.RemoveObject(CacheKeys.FORUM_SCORESET_TRANSFER_MIN_CREDITS);
            Caches.RemoveObject(CacheKeys.FORUM_SCORESET_EXCHANGE_MIN_CREDITS);
            Caches.RemoveObject(CacheKeys.FORUM_SCORESET_MAX_INC_PER_THREAD);
            Caches.RemoveObject(CacheKeys.FORUM_SCORESET_MAX_CHARGE_SPAN);
            Caches.RemoveObject(CacheKeys.FORUM_VALID_SCORE_UNIT);
        }

        public static void ReSetSiteUrls()
        {
            Caches.RemoveObject(CacheKeys.FORUM_URLS);
        }

        public static void ReSetStatistics()
        {
            //Caches.RemoveObject(CacheKeys.FORUM_STATISTICS);
        }

        public static void ReSetAttachmentTypeArray()
        {
            //Caches.RemoveObject(CacheKeys.FORUM_FORUM_SETTING_ATTACHMENT_TYPE);
        }

        public static void ReSetTemplateListBoxOptionsCache()
        {
            Caches.RemoveObject(CacheKeys.FORUM_UI_TEMPLATE_LIST_BOX_OPTIONS_FOR_FORUMINDEX);
            Caches.RemoveObject(CacheKeys.FORUM_UI_TEMPLATE_LIST_BOX_OPTIONS);
        }

        //public static void ReSetOnlineGroupIconList()
        //{
        //    Caches.RemoveObject(CacheKeys.FORUM_UI_ONLINE_ICON_LIST);
        //    //Caches.RemoveObject(CacheKeys.FORUM_ONLINE_ICON_TABLE);
        //}

        public static void ReSetForumLinkList()
        {
            Caches.RemoveObject(CacheKeys.FORUM_FORUM_LINK_LIST);
        }

        public static void ReSetBanWordList()
        {
            //Caches.RemoveObject(CacheKeys.FORUM_BAN_WORD_LIST);
        }

        public static void ReSetForumList()
        {
            Caches.RemoveObject(CacheKeys.FORUM_FORUM_LIST);
        }

        public static void ReSetOnlineUserTable()
        {
        }

        public static void ReSetRss()
        {
            Caches.RemoveObject(CacheKeys.FORUM_RSS);
        }

        public static void ReSetForumRssXml(int fid)
        {
            Caches.RemoveObject(string.Format(CacheKeys.FORUM_RSS_FORUM, fid));
        }

        public static void ReSetRssXml()
        {
            Caches.RemoveObject(CacheKeys.FORUM_RSS_INDEX);
        }

        public static void ReSetValidTemplateIDList()
        {
            Caches.RemoveObject(CacheKeys.FORUM_TEMPLATE_ID_LIST);
        }

        public static void ReSetValidScoreName()
        {
            Caches.RemoveObject(CacheKeys.FORUM_VALID_SCORE_NAME);
        }

        //public static void ReSetMedalsList()
        //{
        //    Caches.RemoveObject(CacheKeys.FORUM_UI_MEDALS_LIST);
        //}

        public static void ReSetDBlinkAndTablePrefix()
        {
            Caches.RemoveObject(CacheKeys.FORUM_BASE_SETTING_DBCONNECTSTRING);
            Caches.RemoveObject(CacheKeys.FORUM_BASE_SETTING_TABLE_PREFIX);
        }

        //public static void ReSetLastPostTableName()
        //{
        //    Caches.RemoveObject(CacheKeys.FORUM_LAST_POST_TABLE_NAME);
        //}

        //public static void ReSetAllPostTableName()
        //{
        //    Caches.RemoveObject(CacheKeys.FORUM_POST_TABLE_NAME);
        //}

        public static void ReSetAdsList()
        {
            Caches.RemoveObject(CacheKeys.FORUM_ADVERTISEMENTS);
        }

        public static void ReSetStatisticsSearchtime()
        {
            //Caches.RemoveObject(CacheKeys.FORUM_STATISTICS_SEARCHTIME);
        }

        public static void ReSetStatisticsSearchcount()
        {
            //Caches.RemoveObject(CacheKeys.FORUM_STATISTICS_SEARCHCOUNT);
        }

        public static void ReSetCommonAvatarList()
        {
            Caches.RemoveObject(CacheKeys.FORUM_COMMON_AVATAR_LIST);
        }

        public static void ReSetJammer()
        {
            Caches.RemoveObject(CacheKeys.FORUM_UI_JAMMER);
        }

        public static void ReSetMagicList()
        {
            Caches.RemoveObject(CacheKeys.FORUM_MAGIC_LIST);
        }

        public static void ReSetScorePaySet()
        {
            Caches.RemoveObject(CacheKeys.FORUM_SCORE_PAY_SET);
        }

        //public static void ReSetPostTableInfo()
        //{
        //    Caches.RemoveObject(CacheKeys.FORUM_POST_TABLE_NAME);
        //    Caches.RemoveObject(CacheKeys.FORUM_LAST_POST_TABLE_NAME);
        //    //TableList.ResetPostTables();
        //}

        public static void ReSetTopiclistByFid(string fid)
        {
            Caches.RemoveObject(string.Format(CacheKeys.FORUM_TOPIC_LIST_FID, fid));
        }

        public static void ReSetDigestTopicList(int count)
        {
            Caches.ReSetFocusTopicList(count, -1, 0, TopicTimeType.All, "ID", true);
        }

        //public static void ReSetDigestTopicList(int count, int fid, TopicTimeType timetype, TopicOrderType ordertype)
        //{
        //	Caches.ReSetFocusTopicList(count, -1, fid, timetype, ordertype, true);
        //}

        public static void ReSetHotTopicList(int count, int views)
        {
            Caches.ReSetFocusTopicList(count, views, 0, TopicTimeType.All, "ID", false);
        }

        //public static void ReSetHotTopicList(int count, int views, int fid, TopicTimeType timetype, TopicOrderType ordertype)
        //{
        //	Caches.ReSetFocusTopicList(count, views, fid, timetype, ordertype, false);
        //}

        public static void ReSetRecentTopicList(int count)
        {
            Caches.ReSetFocusTopicList(count, -1, 0, TopicTimeType.All, "ID", false);
        }

        private static void ReSetFocusTopicList(int count, int views, int fid, TopicTimeType timetype, String ordertype, bool isdigest)
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
            Caches.RemoveObject(key);
        }

        public static void ReSetNavPopupMenu()
        {
            Caches.RemoveObject(CacheKeys.FORUM_FORUM_LIST_MENU_DIV);
        }

        public static void ReSetAllCache()
        {
            XCache.Current.FlushAll();
            Caches.EditDntConfig();
            //BBX.Data.OnlineUsers.CreateOnlineTable();
            Online.ResetOnlineList();
        }

        public static bool EditDntConfig()
        {
            var config = BaseConfigInfo.Current;
            return true;
        }
    }
}