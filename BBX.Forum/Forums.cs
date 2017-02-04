using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using BBX.Cache;
using BBX.Common;
using BBX.Config;
using BBX.Entity;

namespace BBX.Forum
{
    public class Forums
    {
        //public static List<IXForum> GetArchiverForumIndexList(int hideprivate, int usergroupid)
        //{
        //    //List<ArchiverForumInfo> archiverForumIndexList = BBX.Data.Forums.GetArchiverForumIndexList();
        //    var forumList = GetForumList();
        //    var list = new List<IXForum>();
        //    var list2 = new List<IXForum>();
        //    var list3 = new List<IXForum>();
        //    foreach (IXForum item in XForum.FindAllWithCache())
        //    {
        //        if (item.Visible)
        //        {
        //            //foreach (var current2 in forumList)
        //            //{
        //            //    if (Utils.InArray(current2.Fid.ToString(), item.Parentidlist))
        //            //    {
        //            //        int arg_79_0 = current2.Status;
        //            //    }
        //            //}
        //            if (item.Layer == 0)
        //            {
        //                if (hideprivate == 0 || item.AllowView(usergroupid))
        //                {
        //                    list.Add(item);
        //                }
        //            }
        //            else
        //            {
        //                if (hideprivate == 0 || item.AllowView(usergroupid))
        //                {
        //                    list2.Add(item);
        //                }
        //            }
        //        }
        //    }
        //    foreach (var item in list)
        //    {
        //        list3.Add(item);
        //        foreach (var elm in list2)
        //        {
        //            if (Utils.InArray(item.Fid.ToString().Trim(), elm.Parentidlist))
        //            {
        //                list3.Add(elm);
        //            }
        //        }
        //    }
        //    return list3;
        //}

        //public static DataTable GetForumList(int fid)
        //{
        //    if (fid < 0) return new DataTable();

        //    //return Forums.GetSubForumListTable(fid);
        //    var f = XForum.FindByID(fid);
        //    if (f == null) return null;

        //    return f.AllChilds.ToDataTable(false);
        //}

        //public static DataTable GetSubForumListTable(int fid)
        //{
        //	DataTable subForumTable = BBX.Data.Forums.GetSubForumTable(fid);
        //	if (subForumTable != null)
        //	{
        //		int num = 0;
        //		int num2 = 1;
        //		foreach (DataRow dataRow in subForumTable.Rows)
        //		{
        //			if (TypeConverter.ObjectToInt(dataRow["status"]) > 0)
        //			{
        //				if (num2 > 1)
        //				{
        //					dataRow["status"] = ++num;
        //					dataRow["colcount"] = num2;
        //				}
        //				else
        //				{
        //					if (TypeConverter.ObjectToInt(dataRow["subforumcount"]) > 0 && TypeConverter.ObjectToInt(dataRow["colcount"]) > 0)
        //					{
        //						num2 = dataRow["colcount"].ToString().ToInt(0);
        //						num = num2;
        //						dataRow["status"] = num + 1;
        //					}
        //				}
        //			}
        //		}
        //	}
        //	return subForumTable;
        //}

        //public static List<ForumInfo> GetSubForumList(int fid)
        //{
        //    List<ForumInfo> list = new List<ForumInfo>();
        //    foreach (ForumInfo current in BBX.Data.GetForumList())
        //    {
        //        if (current.Parentid == fid && current.Status == 1)
        //        {
        //            list.Add(current);
        //        }
        //    }
        //    return list;
        //}

        //public static bool AllowView(string viewperm, int usergroupid)
        //{
        //    return Forums.HasPerm(viewperm, usergroupid);
        //}

        //public static bool AllowPost(string postperm, int usergroupid)
        //{
        //    return Forums.HasPerm(postperm, usergroupid);
        //}

        public static bool AllowReply(string replyperm, int usergroupid)
        {
            return Forums.HasPerm(replyperm, usergroupid);
        }

        private static bool HasPerm(string perm, int usergroupid)
        {
            return perm.IsNullOrEmpty() || Utils.InArray(usergroupid.ToString(), perm);
        }

        public static bool AllowGetAttach(string getattachperm, int usergroupid)
        {
            return Forums.HasPerm(getattachperm, usergroupid);
        }

        //public static bool AllowPostAttach(string postattachperm, int usergroupid)
        //{
        //    return Forums.HasPerm(postattachperm, usergroupid);
        //}

        public static List<IXForum> GetForumList()
        {
            var cacheService = XCache.Current;
            var list = cacheService.RetrieveObject(CacheKeys.FORUM_FORUM_LIST) as List<IXForum>;
            if (list == null || list.Count == 0)
            {
                //list = BBX.Data.GetForumList();
                list = XForum.Root.AllChilds.Cast<IXForum>().ToList();
                XCache.Add(CacheKeys.FORUM_FORUM_LIST, list);
            }
            return list;
        }

        public static DataTable GetForumListForDataTable()
        {
            //return BBX.Data.Forums.GetForumListForDataTable();
            var dt = XForum.Root.AllChilds.ToDataTable(false);
            if (dt != null && dt.Columns.Contains("ID"))
            {
                //2014-1-20 在XCode中的实体将ID替换为fid（数据库中仍然存的是fid），在XCode中将实体转换位DataTable时使用的是ID，所以下面的访问要将ID列名改成fid
                //因为fid用的太多，所以还是将ID改成fid
                dt.Columns["ID"].ColumnName = "fid";
            }
            return dt;
        }

        public static List<IXForum> GetForumList(string fidList)
        {
            var list = new List<IXForum>();
            foreach (var current in GetForumList())
            {
                string[] array = fidList.Split(',');
                for (int i = 0; i < array.Length; i++)
                {
                    string a = array[i];
                    if (a == current.Fid.ToString())
                    {
                        list.Add(current);
                    }
                }
            }
            return list;
        }

        public static IXForum GetForumInfo(int fid)
        {
            //return Forums.GetForumInfo(fid, true);
            return XForum.FindByID(fid);
        }

        //public static IXForum GetForumInfo(int fid, bool clone)
        //{
        //    if (fid < 1)
        //    {
        //        return null;
        //    }
        //    var forumList = GetForumList();
        //    if (forumList == null) return null;

        //    foreach (var item in forumList)
        //    {
        //        if (item.Fid == fid)
        //        {
        //            item.Pathlist = item.Pathlist.Replace("a><a", "a> &raquo; <a");
        //            return clone ? item.Clone() : item;
        //        }
        //    }
        //    return null;
        //}

        //public static int SetRealCurrentTopics(int fid)
        //{
        //    return BBX.Data.Forums.SetRealCurrentTopics(fid);
        //}

        public static string GetVisibleForum()
        {
            var sb = new StringBuilder();
            var visibleForumList = Forums.GetVisibleForumList();
            if (visibleForumList == null) return "";

            foreach (var current in visibleForumList)
            {
                sb.AppendFormat(",{0}", current.Fid);
            }
            if (sb.Length <= 0)
            {
                return "";
            }
            return sb.Remove(0, 1).ToString();
        }

        public static List<IXForum> GetVisibleForumList()
        {
            var forumList = GetForumList();
            var list = new List<IXForum>();
            var guest = UserGroup.Guest;
            foreach (var item in forumList)
            {
                if (item.Visible)
                {
                    if (item.Layer <= 0 || list.Find(info => info.Fid == item.ParentID) != null)
                    {
                        if (item.Viewperm.IsNullOrEmpty())
                        {
                            if (!guest.AllowVisit) continue;
                        }
                        else
                        {
                            if (!item.AllowView(7)) continue;
                        }
                        if (item.Password.IsNullOrEmpty())
                        {
                            list.Add(item);
                        }
                    }
                }
            }
            return list;
        }

        public static string GetCurrentTopicTypesOption(int fid, string topictypes)
        {
            if (topictypes.IsNullOrEmpty() || topictypes == "|") return "";

            var cacheService = XCache.Current;
            string text = cacheService.RetrieveObject("/Forum/TopicTypesOption" + fid) as string;
            if (text == null)
            {
                var stringBuilder = new StringBuilder("<option value=\"0\">分类</option>");
                string[] array = topictypes.Replace("\r\n", "").Split('|');
                for (int i = 0; i < array.Length; i++)
                {
                    string text2 = array[i];
                    if (!Utils.StrIsNullOrEmpty(text2.Trim()))
                    {
                        stringBuilder.AppendFormat("<option value=\"{0}\">{1}</option>", text2.Split(',')[0], text2.Split(',')[1]);
                    }
                }
                text = stringBuilder.ToString();
                XCache.Add("/Forum/TopicTypesOption" + fid, text);
            }
            return text;
        }

        public static string GetCurrentTopicTypesLink(int fid, string topictypes, string fullpagename)
        {
            if (topictypes.IsNullOrEmpty()) return "";

            var cacheService = XCache.Current;
            string text = cacheService.RetrieveObject("/Forum/TopicTypesLink" + fid) as string;
            if (text == null)
            {
                var stringBuilder = new StringBuilder();
                var stringBuilder2 = new StringBuilder();
                string[] array = topictypes.Split('|');
                for (int i = 0; i < array.Length; i++)
                {
                    string text2 = array[i];
                    if (!Utils.StrIsNullOrEmpty(text2.Trim()))
                    {
                        if (text2.Split(',')[2] == "0")
                        {
                            stringBuilder.AppendFormat("<a href=\"{0}?forumid={1}&typeid={2}\">{3}</a>", new object[]
                            {
                                fullpagename,
                                fid,
                                text2.Split(',')[0],
                                text2.Split(',')[1]
                            });
                        }
                        else
                        {
                            stringBuilder2.AppendFormat("<a href=\"{0}?forumid={1}&typeid={2}\">{3}</a>", new object[]
                            {
                                fullpagename,
                                fid,
                                text2.Split(',')[0],
                                text2.Split(',')[1]
                            });
                        }
                    }
                }
                stringBuilder.AppendFormat("<a href=\"{0}?forumid={1}&typeid={2}\">{3}</a>", new object[]
                {
                    fullpagename,
                    fid,
                    0,
                    "未分类"
                });
                if (!Utils.StrIsNullOrEmpty(stringBuilder2.ToString()))
                {
                    stringBuilder.Append("<a id=\"topictypedrop\" onmouseover=\"showMenu(this.id, true);\">更多分类...</a>");
                    stringBuilder.Append("<ul class=\"p_pop\" id=\"topictypedrop_menu\" style=\"display: none\">");
                    stringBuilder.AppendFormat("<li class='topictype'>{0}</li>", stringBuilder2.ToString().Trim());
                    stringBuilder.Append("</ul>");
                }
                text = stringBuilder.ToString();
                XCache.Add("/Forum/TopicTypesLink" + fid, text);
            }
            return text;
        }

        private static int GetForumSpecialUserPower(string Permuserlist, int userid)
        {
            string[] array = Permuserlist.Split('|');
            for (int i = 0; i < array.Length; i++)
            {
                string text = array[i];
                if (!text.IsNullOrEmpty() && text.Split(',')[1] == userid.ToString())
                {
                    return text.Split(',')[2].ToInt();
                }
            }
            return 0;
        }

        private static bool ValidateSpecialUserPerm(string permUserList, int userId, ForumSpecialUserPower forumSpecialUserPower)
        {
            if (!permUserList.IsNullOrEmpty())
            {
                ForumSpecialUserPower forumSpecialUserPower2 = (ForumSpecialUserPower)Forums.GetForumSpecialUserPower(permUserList, userId);
                if ((forumSpecialUserPower2 & forumSpecialUserPower) > (ForumSpecialUserPower)0)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool AllowViewByUserId(string permUserList, int userId)
        {
            return Forums.ValidateSpecialUserPerm(permUserList, userId, ForumSpecialUserPower.ViewByUser);
        }

        public static bool AllowPostByUserID(string permUserList, int userId)
        {
            return Forums.ValidateSpecialUserPerm(permUserList, userId, ForumSpecialUserPower.PostByUser);
        }

        public static bool AllowReplyByUserID(string permUserList, int userId)
        {
            return Forums.ValidateSpecialUserPerm(permUserList, userId, ForumSpecialUserPower.ReplyByUser);
        }

        public static bool AllowGetAttachByUserID(string permUserList, int userId)
        {
            return Forums.ValidateSpecialUserPerm(permUserList, userId, ForumSpecialUserPower.DownloadAttachByUser);
        }

        public static bool AllowPostAttachByUserID(string permUserList, int userId)
        {
            return Forums.ValidateSpecialUserPerm(permUserList, userId, ForumSpecialUserPower.PostAttachByUser);
        }

        public static bool IsCurrentForumTopicType(Int32 typeid, string topictypes)
        {
            if (topictypes.IsNullOrEmpty()) return true;

            string[] array = topictypes.Split('|');
            for (int i = 0; i < array.Length; i++)
            {
                var tids = array[i].SplitAsInt(",");
                if (tids[0] == typeid) return true;
            }
            return false;
        }

        private static List<IXForum> GetRealForumIndexCollection(List<IXForum> forums)
        {
            var list = new List<IXForum>();
            var list2 = new List<IXForum>();
            var list3 = new List<IXForum>();
            foreach (var item in forums)
            {
                if (item.ParentID == 0)
                    list.Add(item);
                else
                    list2.Add(item);
            }
            foreach (var item in list)
            {
                list3.Add(item);
                foreach (var elm in list2)
                {
                    if (elm.ParentID == item.Fid)
                    {
                        //elm.ColCount = item.ColCount;
                        list3.Add(elm);
                    }
                }
            }
            return list3;
        }

        public static List<IXForum> GetForumIndexCollection(int hidePrivate, int userGroupId, int moderStyle, out int topicCount, out int postCount, out int todayCount)
        {
            var list = new List<IXForum>();
            topicCount = 0;
            postCount = 0;
            todayCount = 0;
            //foreach (var current in BBX.Data.Forums.GetForumIndexList())
            foreach (var frm in XForum.GetForumIndexList())
            {
                if (!frm.Viewperm.IsNullOrEmpty() && !Utils.InArray(userGroupId.ToString(), frm.Viewperm))
                {
                    if (hidePrivate != 0) continue;

                    //frm.LastTitle = "";
                    //frm.LastPoster = "";
                    //frm.Status = -1;
                }
                if (frm.Layer == 0 && Utils.GetCookie("bbx_collapse").IndexOf("_category_" + frm.Fid + "_") > -1)
                {
                    frm.Collapse = "display: none;";
                }
                //if (frm.Status > 0)
                //{
                //    if (frm.ParentID == 0 && frm.SubforumCount > 0)
                //    {
                //        frm.Status = frm.ColCount + 1;
                //    }
                //    else
                //    {
                //        frm.Status = 1;
                //        //frm.ColCount = 1;
                //    }
                //}
                //current.Moderators = Forums.GetModerators(current, moderStyle);
                if (frm.ModeratorsHtml == null) frm.ModeratorsHtml = frm.GetModerators(moderStyle);
                if (frm.LastPost <= DateTime.MinValue || frm.LastPost.Date != DateTime.Now.Date)
                {
                    frm.TodayPosts = 0;
                }
                //if (!frm.Viewperm.IsNullOrEmpty() && !Utils.InArray(userGroupId.ToString(), frm.Viewperm) && hidePrivate == 0)
                //{
                //    frm.LastTitle = "";
                //    frm.LastPoster = "";
                //    frm.Status = -1;
                //}
                if (frm.Layer > 0)
                {
                    //var forumInfo = Forums.GetForumInfo(frm.Fid, false);
                    //if (forumInfo != null)
                    //{
                    //    forumInfo.Topics = frm.Topics;
                    //    forumInfo.Posts = frm.Posts;
                    //    forumInfo.TodayPosts = frm.TodayPosts;
                    //}
                    topicCount += frm.Topics;
                    postCount += frm.Posts;
                    todayCount += frm.TodayPosts;
                }
                list.Add(frm);
            }
            return Forums.GetRealForumIndexCollection(list);
        }

        public static List<IXForum> GetSubForumCollection(int fid, int colcount, int hideprivate, int usergroupid, int moderstyle)
        {
            var list = new List<IXForum>();
            if (fid > 0)
            {
                //list = BBX.Data.Forums.GetSubForumList(fid, colcount);
                list = XForum.GetSubForumList(fid, colcount);
                foreach (var forum in list)
                {
                    forum.Description = UBB.ParseSimpleUBB(forum.Description);
                    //forum.Moderators = Forums.GetModerators(forum, moderstyle);
                    if (forum.ModeratorsHtml == null) forum.ModeratorsHtml = forum.GetModerators(moderstyle);
                    if (forum.LastPost <= DateTime.MinValue || forum.LastPost.Date != DateTime.Now.Date)
                    {
                        forum.TodayPosts = 0;
                    }
                    //if (!forum.Viewperm.IsNullOrEmpty() && !Utils.InArray(usergroupid.ToString(), forum.Viewperm) && hideprivate == 0)
                    //{
                    //    forum.LastTitle = "";
                    //    forum.LastPoster = "";
                    //    forum.Status = -1;
                    //}
                }
            }
            return list;
        }

        public static string GetDropdownOptions()
        {
            var cacheService = XCache.Current;
            string text = cacheService.RetrieveObject("/Forum/DropdownOptions") as string;
            if (text == null)
            {
                var sb = new StringBuilder();
                //DataTable shortForumList = BBX.Data.Forums.GetShortForumList();
                //var shortForumList = XForum.Root.AllChilds.ToDataTable(false);
                string blank = Utils.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;");
                //DataRow[] array = shortForumList.Select("parentid=0");
                //for (int i = 0; i < array.Length; i++)
                //{
                //    DataRow dataRow = array[i];
                //    sb.AppendFormat("<option value=\"{0}\" disabled='true'>{1}</option>", dataRow[0].ToString().Trim(), dataRow[1].ToString().Trim());
                //    sb.Append(Forums.BindNode(dataRow[0].ToString().Trim(), shortForumList, blank));
                //}
                foreach (var item in XForum.Root.Childs)
                {
                    sb.AppendFormat("<option value=\"{0}\" disabled='true'>{1}</option>", item.ID, item.Name);
                    sb.Append(BindNode(item, blank));
                }
                text = sb.ToString();
                XCache.Add("/Forum/DropdownOptions", text);
            }
            return text;
        }

        public static string GetModerDropdownOptions(int uid)
        {
            var sb = new StringBuilder();
            //foreach (DataRow dataRow in BBX.Data.Forums.GetModerForums(uid).Rows)
            //{
            //	sb.AppendFormat("<option value=\"{0}\">{1}</option>", dataRow["fid"], dataRow["name"]);
            //}
            var list = Moderator.FindAllByUid(uid).ToList();
            foreach (var item in XForum.Root.AllChilds)
            {
                if (!list.Any(e => e.Fid == item.ID)) continue;

                sb.AppendFormat("<option value=\"{0}\">{1}</option>", item.ID, item.Name);
            }
            return sb.ToString();
        }

        private static string BindNode(XForum fi, string blank)
        {
            var sb = new StringBuilder();
            //DataRow[] array = dt.Select("parentid=" + parentid);
            //for (int i = 0; i < array.Length; i++)
            //{
            //    DataRow dataRow = array[i];
            //    sb.AppendFormat("<option value=\"{0}\">{1}{2}</option>", dataRow[0].ToString().Trim(), blank, dataRow[1].ToString().Trim());
            //    sb.Append(Forums.BindNode(dataRow[0].ToString().Trim(), dt, Utils.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;") + blank));
            //}
            foreach (var item in fi.Childs)
            {
                sb.AppendFormat("<option value=\"{0}\">{1}{2}</option>", item.ID, blank, item.Name);
                sb.Append(BindNode(item, Utils.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;") + blank));
            }
            return sb.ToString();
        }

        //public static DataTable GetOpenForumList()
        //{
        //	return BBX.Data.Forums.GetOpenForumList();
        //}

        //public static void UpdateFourmsDisplayOrder(int minDisplayOrder)
        //{
        //	BBX.Data.Forums.UpdateFourmsDisplayOrder(minDisplayOrder);
        //}

        public static int GetSpecifyForumTemplateCount()
        {
            int num = 0;
            foreach (var current in GetForumList())
            {
                if (current.TemplateID != 0 && current.TemplateID != GeneralConfigInfo.Current.Templateid)
                {
                    num++;
                }
            }
            return num;
        }

        //public static int UpdateForumField(int fid, string fieldname, string fieldvalue)
        //{
        //	return BBX.Data.Forums.UpdateForumField(fid, fieldname, fieldvalue);
        //}

        //public static void UpdateModeratorName(string oldName, string newName)
        //{
        //    BBX.Data.Forums.UpdateModeratorName(oldName, newName);
        //}

        public static string GetForumLinkOfAssociatedTopicType(int topicTypeId)
        {
            string text = "";
            //DataTable forumNameIncludeTopicType = BBX.Data.Forums.GetForumNameIncludeTopicType();
            //foreach (DataRow dataRow in forumNameIncludeTopicType.Rows)
            foreach (var item in XForum.Root.AllChilds)
            {
                string[] array = (item.Field.TopicTypes + "").Split('|');
                for (int i = 0; i < array.Length; i++)
                {
                    string text2 = array[i];
                    if (text2.IndexOf(topicTypeId + ",") == 0)
                    {
                        object obj = text;
                        text = obj + "<a href='" + BaseConfigs.GetForumPath + "showforum.aspx?forumid=" + item.ID + "&typeid=" + topicTypeId + "&search=1' target='_blank'>" + item.Name + "</a>";
                        object obj2 = text;
                        text = obj2 + "[<a href='forum_editforums.aspx?fid=" + item.ID + "&tabindex=4'>编辑</a>],";
                        break;
                    }
                }
            }
            return text.TrimEnd(',');
        }

        //public static DataTable GetExistTopicTypeOfForum()
        //{
        //    return BBX.Data.Forums.GetExistTopicTypeOfForum();
        //}

        //public static bool DeleteForum(Int32 fid)
        //{
        //	//if (BBX.Data.Forums.IsExistSubForum(int.Parse(fid)))
        //	//{
        //	//	return false;
        //	//}
        //	////BBX.Data.Forums.DeleteForum(TableList.CurrentTableName, fid);
        //	//DatabaseProvider.GetInstance().DeleteForumsByFid(TableList.CurrentTableName, fid);

        //	var f = XForum.FindByID(fid);
        //	if (f == null) return false;
        //	f.Delete();

        //	XCache.Remove(CacheKeys.FORUM_UI_FORUM_LIST_BOX_OPTIONS);
        //	XCache.Remove(CacheKeys.FORUM_FORUM_LIST);
        //	return true;
        //}

        //public static void UpdateSubForumCount(int fid)
        //{
        //	BBX.Data.Forums.UpdateSubForumCount(fid);
        //}

        //public static DataTable GetForumField(int fid, string fieldname)
        //{
        //	return BBX.Data.Forums.GetForumField(fid, fieldname);
        //}

        public static float[] GetValues(string credits)
        {
            float[] array = new float[8];
            if (!credits.IsNullOrEmpty())
            {
                int num = 0;
                string[] array2 = Utils.SplitString(credits, ",");
                for (int i = 0; i < array2.Length; i++)
                {
                    string text = array2[i];
                    if (num == 0)
                    {
                        if (!text.Equals("True"))
                        {
                            array = null;
                            break;
                        }
                        num++;
                    }
                    else
                    {
                        array[num - 1] = (Single)text.ToDouble();
                        num++;
                        if (num > 8)
                        {
                            break;
                        }
                    }
                }
                return array;
            }
            return null;
        }

        public static IXForum[] GetVisitedForums()
        {
            string cookie = Utils.GetCookie("visitedforums");
            if (String.IsNullOrEmpty(cookie)) return new IXForum[0];

            var list = new List<IXForum>();
            string[] array = cookie.Split(',');
            for (int i = 0; i < array.Length; i++)
            {
                string b = array[i];
                foreach (var current in GetForumList())
                {
                    if (current.Fid.ToString() == b)
                    {
                        //list.Add(new SimpleForumInfo
                        //{
                        //    Fid = current.Fid,
                        //    Name = Utils.RemoveHtml(current.Name),
                        //    Url = Urls.ShowForumAspxRewrite(current.Fid, 1, current.Rewritename),
                        //    Postbytopictype = current.PostbytopicType,
                        //    Topictypes = current.Topictypes
                        //});
                        list.Add(current);
                        break;
                    }
                }
            }
            return list.ToArray();
        }

        //public static SimpleForumInfo GetLastPostedForum()
        //{
        //	string cookie = Utils.GetCookie("lastpostedforum");
        //	if (String.IsNullOrEmpty(cookie))
        //	{
        //		return null;
        //	}
        //	foreach (var current in GetForumList())
        //	{
        //		if (current.Fid.ToString() == cookie)
        //		{
        //			return new SimpleForumInfo
        //			{
        //				Fid = current.Fid,
        //				Name = Utils.RemoveHtml(current.Name),
        //				Url = Urls.ShowForumAspxRewrite(current.Fid, 1, current.Rewritename),
        //				Postbytopictype = current.PostbytopicType,
        //				Topictypes = current.Topictypes
        //			};
        //		}
        //	}
        //	return null;
        //}

        //public static string ShowForumCondition(int sqlid, int cond)
        //{
        //    return DatabaseProvider.GetInstance().ShowForumCondition(sqlid, cond);
        //}

        //public static void ResetForumsTopics()
        //{
        //	BBX.Data.Forums.ResetForumsTopics();
        //}

        //public static void ResetTodayPosts()
        //{
        //	BBX.Data.Forums.ResetTodayPosts();
        //}

        //public static int GetFirstFourmID()
        //{
        //    return BBX.Data.Forums.GetFirstFourmID();
        //}
    }
}