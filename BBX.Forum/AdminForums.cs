using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using BBX.Cache;
using BBX.Common;
using BBX.Config;
using BBX.Entity;

namespace BBX.Forum
{
    public class AdminForums : Forums
    {
        public static string ChildNode = "0";

        //public static IXForum[] GetForumSpecialUser(string username)
        //{
        //    if (username.IsNullOrWhiteSpace()) return XForum.FindAllWithCache().ToList().Cast<IXForum>().ToArray();

        //    var list = new List<IXForum>();
        //    foreach (IXForum item in XForum.FindAllWithCache())
        //    {
        //        if (item.Permuserlist.IsNullOrWhiteSpace()) continue;

        //        var ss = item.Permuserlist.Split("|");
        //        if (ss.Any(e => e.Split(",")[0] == username)) list.Add(item);
        //    }
        //    return list.ToArray();
        //    //DataTable dataTable = new DataTable();
        //    //if (String.IsNullOrEmpty(username))
        //    //{
        //    //    dataTable = BBX.Data.Forums.GetForumTableBySpecialUser("");
        //    //}
        //    //else
        //    //{
        //    //    dataTable = BBX.Data.Forums.GetForumTableBySpecialUser(username);
        //    //}
        //    //IXForum[] array = null;
        //    //if (dataTable.Rows.Count > 0)
        //    //{
        //    //var array = new IXForum[dataTable.Rows.Count];
        //    //for (int i = 0; i < dataTable.Rows.Count; i++)
        //    //{
        //    //    array[i] = new XForum();
        //    //    if (dataTable.Rows[i]["permuserlist"].ToString() != "")
        //    //    {
        //    //        if (username != "")
        //    //        {
        //    //            string[] array2 = dataTable.Rows[i]["permuserlist"].ToString().Split('|');
        //    //            for (int j = 0; j < array2.Length; j++)
        //    //            {
        //    //                string text = array2[j];
        //    //                if (username == text.Split(',')[0])
        //    //                {
        //    //                    array[i].Permuserlist = text;
        //    //                }
        //    //            }
        //    //        }
        //    //        else
        //    //        {
        //    //            if (dataTable.Rows[i]["permuserlist"].ToString().Split('|').Length == 1)
        //    //            {
        //    //                array[i].Permuserlist = dataTable.Rows[i]["permuserlist"].ToString();
        //    //            }
        //    //            else
        //    //            {
        //    //                for (int k = 0; k < dataTable.Rows[i]["permuserlist"].ToString().Split('|').Length; k++)
        //    //                {
        //    //                    var expr_16F = array[i];
        //    //                    expr_16F.Permuserlist = expr_16F.Permuserlist + dataTable.Rows[i]["permuserlist"].ToString().Split('|')[k] + "|";
        //    //                }
        //    //                array[i].Permuserlist = array[i].Permuserlist.ToString().Substring(0, array[i].Permuserlist.ToString().Length - 1);
        //    //            }
        //    //        }
        //    //        array[i].Fid = Utils.StrToInt(dataTable.Rows[i]["fid"].ToString(), 0);
        //    //        array[i].Name = dataTable.Rows[i]["name"].ToString();
        //    //        array[i].Moderators = dataTable.Rows[i]["moderators"].ToString();
        //    //    }
        //    //}
        //    ////}
        //    //return array;
        //}

        public static IXForum[] GetForumSpecialUser(int fid)
        {
            var f = XForum.FindByID(fid);
            if (f != null) return new IXForum[] { f };
            return null;

            //DataTable forumTableWithSpecialUser = BBX.Data.Forums.GetForumTableWithSpecialUser(fid);
            //IXForum[] array = null;
            //if (forumTableWithSpecialUser.Rows.Count > 0)
            //{
            //    array = new IXForum[forumTableWithSpecialUser.Rows.Count];
            //    for (int i = 0; i < forumTableWithSpecialUser.Rows.Count; i++)
            //    {
            //        array[i] = new XForum();
            //        if (forumTableWithSpecialUser.Rows[i]["permuserlist"].ToString() != "")
            //        {
            //            if (forumTableWithSpecialUser.Rows[i]["permuserlist"].ToString().Split('|').Length == 1)
            //            {
            //                array[i].Permuserlist = forumTableWithSpecialUser.Rows[i]["permuserlist"].ToString();
            //            }
            //            else
            //            {
            //                for (int j = 0; j < forumTableWithSpecialUser.Rows[i]["permuserlist"].ToString().Split('|').Length; j++)
            //                {
            //                    var expr_C8 = array[i];
            //                    expr_C8.Permuserlist = expr_C8.Permuserlist + forumTableWithSpecialUser.Rows[i]["permuserlist"].ToString().Split('|')[j] + "|";
            //                }
            //                array[i].Permuserlist = array[i].Permuserlist.ToString().Substring(0, array[i].Permuserlist.ToString().Length - 1);
            //            }
            //            array[i].Fid = Utils.StrToInt(forumTableWithSpecialUser.Rows[i]["fid"].ToString(), 0);
            //            array[i].Name = forumTableWithSpecialUser.Rows[i]["name"].ToString();
            //            array[i].Moderators = forumTableWithSpecialUser.Rows[i]["moderators"].ToString();
            //        }
            //    }
            //}
            //return array;
        }

        public static string UpdateForumInfo(IXForum forumInfo)
        {
            //BBX.Data.Forums.UpdateForumInfo(forumInfo);
            forumInfo.Save();

            var cache = XCache.Current;
            XCache.Remove(CacheKeys.FORUM_FORUM_LIST);
            //SetForumsPathList();
            string result = SetForumsModerators(forumInfo.ID.ToString(), forumInfo.Moderators, forumInfo.Inheritedmod);
            XCache.Remove(CacheKeys.FORUM_UI_FORUM_LIST_BOX_OPTIONS);
            XCache.Remove(CacheKeys.FORUM_FORUM_LIST);
            XCache.Remove("/Forum/TopicTypesOption" + forumInfo.ID);
            XCache.Remove("/Forum/TopicTypesLink" + forumInfo.ID);
            XCache.Remove("/Aggregation/HotForumList");
            XCache.Remove("/Aggregation/ForumHotTopicList");
            XCache.Remove("/Aggregation/ForumNewTopicList");
            return result;
        }

        public static int CreateForums(IXForum forumInfo, out string moderatorsInfo, int adminUid, string adminUserName, int adminUserGruopId, string adminUserGroupTitle, string adminIp)
        {
            //int result = BBX.Data.Forums.CreateForumInfo(forumInfo);
            var result = forumInfo.Insert();

            //SetForumsPathList();
            moderatorsInfo = SetForumsModerators(result.ToString(), forumInfo.Moderators, forumInfo.Inheritedmod).Replace("'", "’");
            var cache = XCache.Current;
            XCache.Remove(CacheKeys.FORUM_UI_FORUM_LIST_BOX_OPTIONS);
            XCache.Remove(CacheKeys.FORUM_FORUM_LIST);
            XCache.Remove("/Aggregation/HotForumList");
            XCache.Remove("/Aggregation/ForumHotTopicList");
            XCache.Remove("/Aggregation/ForumNewTopicList");
            XCache.Remove("/Forum/DropdownOptions");
            XCache.Remove(CacheKeys.FORUM_FORUM_LIST_MENU_DIV);
            AdminVisitLog.InsertLog(adminUid, adminUserName, adminUserGruopId, adminUserGroupTitle, adminIp, "添加论坛版块", "添加论坛版块,名称为:" + forumInfo.Name);
            return result;
        }

        //public static int CreateForums(ForumInfo forumInfo)
        //{
        //    string text;
        //    return CreateForums(forumInfo, out text, 0, "API", 0, "API", "");
        //}

        //public static void SetForumsPathList()
        //{
        //    var generalConfigInfo = GeneralConfigInfo.Current;
        //    SetForumsPathList(generalConfigInfo.Aspxrewrite == 1, generalConfigInfo.Extname);
        //}

        //public static void SetForumsPathList(bool isaspxrewrite, string extname)
        //{
        //    DataTable forumListForDataTable = Forums.GetForumListForDataTable();
        //    string getForumPath = BaseConfigs.GetForumPath;
        //    foreach (DataRow dataRow in forumListForDataTable.Rows)
        //    {
        //        //2014-2-11 rewritename不知道是什么，没有找到这个字段，先注释掉
        //        string text = "";
        //        if (dataRow["parentidlist"].ToString().Trim() == "0")
        //        {
        //            //text = "<a href=\"" + ((dataRow["rewritename"].ToString().Trim() == string.Empty) ? string.Empty : getForumPath) + Urls.ShowForumAspxRewrite(Utils.StrToInt(dataRow["fid"], 0), 0, dataRow["rewritename"].ToString()) + "\">" + dataRow["name"].ToString().Trim() + "</a>";
        //            text = "<a href=\"" + (string.Empty) + Urls.ShowForumAspxRewrite(Utils.StrToInt(dataRow["fid"], 0), 0, "") + "\">" + dataRow["name"].ToString().Trim() + "</a>";
        //        }
        //        else
        //        {
        //            string[] array = dataRow["parentidlist"].ToString().Trim().Split(',');
        //            for (int i = 0; i < array.Length; i++)
        //            {
        //                string text2 = array[i];
        //                if (text2.IsNullOrEmpty())
        //                {
        //                    DataRow[] array2 = forumListForDataTable.Select("[fid]=" + text2);
        //                    if (array2.Length > 0)
        //                    {
        //                        string text3 = text;
        //                        text = text3 + "<a href=\"" + (string.Empty) + Urls.ShowForumAspxRewrite(Utils.StrToInt(array2[0]["fid"], 0), 0, "") + "\">" + array2[0]["name"].ToString().Trim() + "</a>";
        //                    }
        //                }
        //            }
        //            //Urls.ShowForumAspxRewrite(Utils.StrToInt(dataRow["fid"], 0), 0, dataRow["rewritename"].ToString());
        //            Urls.ShowForumAspxRewrite(Utils.StrToInt(dataRow["fid"], 0), 0, "");
        //            string text4 = text;
        //            text = text4 + "<a href=\"" + ("") + Urls.ShowForumAspxRewrite(Utils.StrToInt(dataRow["fid"], 0), 0, "") + "\">" + dataRow["name"].ToString().Trim() + "</a>";
        //        }
        //        //foreach (var item in BBX.Entity.Forum.FindAllWithCache())
        //        //{
        //        //    if (item.ID == int.Parse(dataRow["fid"].ToString()))
        //        //    {
        //        //        item.Pathlist = text;
        //        //        //BBX.Data.Forums.UpdateForumInfo(item);
        //        //        item.Save();
        //        //    }
        //        //}
        //        var f = BBX.Entity.XForum.FindByID(int.Parse(dataRow["fid"].ToString()));
        //        if (f != null)
        //        {
        //            f.Pathlist = text;
        //            f.Save();
        //        }
        //    }
        //}

        //public static void SetForumslayer()
        //{
        //    foreach (var item in Forums.GetForumList())
        //    {
        //        Int16 num = 0;
        //        string text = "";
        //        int parentid = item.ParentID;
        //        if (parentid == 0)
        //        {
        //            var forumInfo = Forums.GetForumInfo(item.Fid);
        //            if (forumInfo.Layer != num)
        //            {
        //                forumInfo.Layer = num;
        //                forumInfo.Parentidlist = "0";
        //                UpdateForumInfo(forumInfo);
        //            }
        //        }
        //        else
        //        {
        //            int num2;
        //            while (true)
        //            {
        //                num2 = parentid;
        //                parentid = Forums.GetForumInfo(parentid).ParentID;
        //                num++;
        //                if (parentid == 0)
        //                {
        //                    break;
        //                }
        //                text = num2 + "," + text;
        //            }
        //            text = (num2 + "," + text).TrimEnd(',');
        //            var forumInfo2 = Forums.GetForumInfo(item.Fid);
        //            if (forumInfo2.Layer != num || forumInfo2.Parentidlist != text)
        //            {
        //                forumInfo2.Layer = num;
        //                forumInfo2.Parentidlist = text;
        //                UpdateForumInfo(forumInfo2);
        //            }
        //        }
        //    }
        //}

        public static bool MovingForumsPos(Int32 currentfid, Int32 targetfid, bool isaschildnode)
        {
            string extname = GeneralConfigInfo.Current.Extname;
            //BBX.Data.Forums.MovingForumsPos(currentfid, targetfid, isaschildnode, extname);
            var src = XForum.FindByID(currentfid);
            var tar = XForum.FindByID(targetfid);

            if (!isaschildnode)
            {
                // 仅仅移动顺序
                src.DisplayOrder = tar.DisplayOrder - 1;
                src.Save();
            }
            else
            {
                // 作为子版块，仅更新部分数据，其它数据在一段时间后将会得以自动更新，这里不再处理
                src.ParentID = tar.ID;
                src.Save();
            }

            //SetForumslayer();
            SetForumsSubForumCountAndDispalyorder();
            //SetForumsPathList();
            XCache.Remove(CacheKeys.FORUM_UI_FORUM_LIST_BOX_OPTIONS);
            XCache.Remove(CacheKeys.FORUM_FORUM_LIST);
            return true;
        }

        public static string SetForumsModerators(string sfid, string moderators, int inherited)
        {
            //BBX.Data.Moderators.DeleteModeratorByFid(int.Parse(fid));
            var fid = Int32.Parse(sfid);
            var f = XForum.FindByID(fid);
            Moderator.DeleteByFid(fid);
            if (inherited == 1)
            {
                string fids = "-1";
                //while (true)
                //{
                //	var parentIdByFid = BBX.Data.Forums.GetParentIdByFid(fid);
                //	if (parentIdByFid.Rows.Count <= 0)
                //	{
                //		break;
                //	}
                //	var text = parentIdByFid.Rows[0][0].ToString();
                //	if (text == "0" || String.IsNullOrEmpty(text)) break;

                //	fids = fids + "," + text;
                //}
                if (f.AllChilds.Count > 0) fids = f.AllChildKeyString;
                int num = 1;
                //foreach (DataRow dataRow in BBX.Data.Moderators.GetUidModeratorByFid(fids).Rows)
                //{
                //    BBX.Data.Moderators.AddModerator(int.Parse(dataRow[0].ToString()), int.Parse(sfid), num, 1);
                //    num++;
                //}
                foreach (var item in Moderator.FindAllIDsByFids(fids))
                {
                    var entity = new Moderator();
                    entity.Uid = item;
                    entity.Fid = fid;
                    entity.DisplayOrder = num++;
                    entity.Inherited = 1;
                    entity.Insert();
                }
            }
            InsertForumsModerators(sfid, moderators, 1, 0);
            return UpdateUserInfoWithModerator(moderators);
        }

        public static string UpdateUserInfoWithModerator(string moderators)
        {
            moderators = ((moderators == null) ? "" : moderators);
            string text = "";
            string[] array = moderators.Split(',');
            for (int i = 0; i < array.Length; i++)
            {
                string name = array[i];
                if (name != "" && !("系统" == name))
                {
                    var user = User.FindByName(name);
                    if (user != null && user.GroupID != 7 && user.GroupID != 8)
                    {
                        if (user.GroupID > 3 || user.GroupID <= 0)
                        {
                            //int radminid = UserGroup.FindByID(userInfo.GroupID).RadminID;
                            var ug = UserGroup.FindByID(user.GroupID);
                            if (ug != null && ug.Is管理团队)
                            {
                                //BBX.Data.Users.SetUserToModerator(name);
                                user.AdminID = 3;
                                user.GroupID = 3;
                                user.Save();
                            }
                        }
                    }
                    else
                    {
                        text = text + name + ",";
                    }
                }
            }
            Caches.ReSetModeratorList();
            return text;
        }

        public static void InsertForumsModerators(string fid, string moderators, int displayorder, int inherited)
        {
            moderators = ((moderators == null) ? "" : moderators);
            //BBX.Data.Moderators.InsertForumsModerators(fid, moderators, displayorder, inherited);
            Moderator.Adds(Int32.Parse(fid), moderators, displayorder, inherited);

            Caches.ReSetModeratorList();
        }

        public static void CompareOldAndNewModerator(string oldmoderators, string newmoderators, int currentfid)
        {
            if (oldmoderators == null || String.IsNullOrEmpty(oldmoderators))
            {
                return;
            }
            string[] array = oldmoderators.Split(',');
            for (int i = 0; i < array.Length; i++)
            {
                string name = array[i];
                if (name != "" && ("," + newmoderators + ",").IndexOf("," + name + ",") < 0)
                {
                    var user = User.FindByName(name);
                    if (user != null)
                    {
                        int uid = user.ID;
                        int adminid = user.AdminID;
                        //DataTable uidInModeratorsByUid = BBX.Data.Moderators.GetUidInModeratorsByUid(currentfid, uid);
                        //if (uidInModeratorsByUid.Rows.Count == 0 && adminid != 1)
                        if (adminid != 1 && Moderator.FindByUidAndFid(uid, currentfid) == null)
                        {
                            var ug = CreditsFacade.GetCreditsUserGroupId((float)user.Credits);
                            //BBX.Data.Users.UpdateUserOnlineInfo(ug.ID, uid);
                            //BBX.Data.Users.UpdateUserOtherInfo(ug.ID, uid);
                            user.AdminID = 0;
                            user.GroupID = ug.ID;
                            user.Save();

                            var online = Online.FindByUserID(user.ID);
                            if (online != null)
                            {
                                online.AdminID = 0;
                                online.GroupID = ug.ID;
                                online.Save();
                            }
                        }
                    }
                }
            }
        }

        //public static string FindChildNode(string correntfid)
        //{
        //	string childNode2;
        //	lock (ChildNode)
        //	{
        //		DataTable forumByParentid = BBX.Data.Forums.GetForumByParentid(int.Parse(correntfid));
        //		ChildNode = ChildNode + "," + correntfid;
        //		if (forumByParentid.Rows.Count > 0)
        //		{
        //			foreach (DataRow dataRow in forumByParentid.Rows)
        //			{
        //				FindChildNode(dataRow["fid"].ToString());
        //			}
        //		}
        //		forumByParentid.Dispose();
        //		childNode2 = ChildNode;
        //	}
        //	return childNode2;
        //}

        //public static bool CombinationForums(Int32 sourcefid, Int32 targetfid)
        //{
        //	//if (BBX.Data.Forums.IsExistSubForum(int.Parse(sourcefid)))
        //	//{
        //	//	return false;
        //	//}

        //	var f = XForum.FindByID(sourcefid);
        //	if (f == null) return false;

        //	ChildNode = "0";
        //	string fidlist = ("," + FindChildNode(targetfid)).Replace(",0,", "");
        //	BBX.Data.Forums.CombinationForums(sourcefid, targetfid, fidlist);
        //	XCache.Remove(CacheKeys.FORUM_UI_FORUM_LIST_BOX_OPTIONS);
        //	XCache.Remove(CacheKeys.FORUM_FORUM_LIST);
        //	return true;
        //}

        public static void SetForumsSubForumCountAndDispalyorder()
        {
            //DataTable forumListForDataTable = Forums.GetForumListForDataTable();
            //foreach (DataRow dataRow in forumListForDataTable.Rows)
            //{
            //	BBX.Data.Forums.UpdateSubForumCount(int.Parse(forumListForDataTable.Select("parentid=" + dataRow["fid"].ToString()).Length.ToString()), int.Parse(dataRow["fid"].ToString()));
            //}
            //if (forumListForDataTable.Rows.Count == 1)
            //{
            //	return;
            //}
            //int num = 1;
            //DataRow[] array = forumListForDataTable.Select("parentid=0");
            //for (int i = 0; i < array.Length; i++)
            //{
            //	DataRow dataRow2 = array[i];
            //	if (dataRow2["parentid"].ToString() == "0")
            //	{
            //		ChildNode = "0";
            //		string text = ("," + FindChildNode(dataRow2["fid"].ToString())).Replace(",0,", "");
            //		string[] array2 = text.Split(',');
            //		for (int j = 0; j < array2.Length; j++)
            //		{
            //			string str = array2[j];
            //			BBX.Data.Forums.UpdateDisplayorderInForumByFid(num, str.ToInt());
            //			num++;
            //		}
            //	}
            //}

            // 树根下面的列表是一个顺序层次列表
            var list = XForum.Root.AllChilds.ToList();
            // 为了从底层往上层更新数据，我们需要倒序
            list.Reverse();
            var count = list.Count;
            foreach (var item in list)
            {
                //item.SubforumCount = item.Childs.Count;
                item.DisplayOrder = count--;
                item.Update();
            }
        }

        //public static void SetForumsStatus()
        //{
        //    //DataTable mainForum = BBX.Data.Forums.GetMainForum();
        //    //foreach (DataRow dataRow in mainForum.Rows)
        //    foreach (var f in XForum.FindAllWithCache().ToList().OrderBy(e => e.DisplayOrder))
        //    {
        //        if (f.Layer != 0) continue;

        //        ChildNode = "0";
        //        //string fidlist = ("," + FindChildNode(f.ID)).Replace(",0,", "");
        //        if (!f.Visible)
        //        {
        //            // 设置子孙状态都是0
        //            //BBX.Data.Forums.UpdateStatusByFidlist(fidlist);
        //            foreach (var item in f.AllChilds)
        //            {
        //                item.Visible = false;
        //                item.Update();
        //            }
        //        }
        //        else //if (f.Status == 1)
        //        {
        //            // 设置子孙状态都是1
        //            //BBX.Data.Forums.UpdateStatusByFidlistOther(fidlist);
        //            foreach (var item in f.AllChilds)
        //            {
        //                item.Visible = true;
        //                item.Update();
        //            }
        //        }
        //        //else
        //        //{
        //        //    //BBX.Data.Forums.SetStatusInForum(4, f.ID);
        //        //    f.Status = 4;
        //        //    f.Update();

        //        //    int num = 5;
        //        //    foreach (var item in f.AllChilds)
        //        //    {
        //        //        item.Status = num++;
        //        //        item.Update();
        //        //    }
        //        //}
        //    }
        //}

        //public static bool BatchSetForumInf(IXForum forumInfo, BatchSetParams bsp, string fidlist)
        //{
        //    return BBX.Data.Forums.BatchSetForumInf(forumInfo, bsp, fidlist);
        //}

        //public static DataTable GetMaxDisplayOrder(int fid)
        //{
        //	return BBX.Data.Forums.GetMaxDisplayOrder(fid);
        //}

        //public static int GetMaxDisplayOrder()
        //{
        //	return BBX.Data.Forums.GetMaxDisplayOrder();
        //}

        //public static void UpdateForumsDisplayOrder(int currentdisplayorder)
        //{
        //	BBX.Data.Forums.UpdateForumsDisplayOrder(currentdisplayorder);
        //}

        //public static void SetSubForumCount(int fid)
        //{
        //	BBX.Data.Forums.SetSubForumCount(fid);
        //}

        public static void CreateSmilies(int displayOrder, int type, string code, string url, int adminUid, string adminUserName, int adminUserGruopId, string adminUserGroupTitle, string adminIp)
        {
            Smilie.Add(type, code, url, displayOrder);
            ResetCacheObjectAboutSmilies();
            AdminVisitLog.InsertLog(adminUid, adminUserName, adminUserGruopId, adminUserGroupTitle, adminIp, "表情文件添加", code);
        }

        public static void UpdateSmilies(int id, int displayOrder, int type, string code, string url, int adminUid, string adminUserName, int adminUserGruopId, string adminUserGroupTitle, string adminIp)
        {
            //BBX.Data.Smilies.UpdateSmilies(id, displayOrder, type, code);
            var sm = Smilie.FindByID(id);
            sm.DisplayOrder = displayOrder;
            sm.Type = type;
            sm.Code = code;
            sm.Update();

            ResetCacheObjectAboutSmilies();
            AdminVisitLog.InsertLog(adminUid, adminUserName, adminUserGruopId, adminUserGroupTitle, adminIp, "表情文件修改", code);
        }

        public static void DeleteSmilies(string idList, int adminUid, string adminUserName, int adminUserGruopId, string adminUserGroupTitle, string adminIp)
        {
            Smilie.DeleteSmilies(idList);
            ResetCacheObjectAboutSmilies();
            AdminVisitLog.InsertLog(adminUid, adminUserName, adminUserGruopId, adminUserGroupTitle, adminIp, "表情文件删除", "ID:" + idList);
        }

        private static void ResetCacheObjectAboutSmilies()
        {
            var cache = XCache.Current;
            XCache.Remove(CacheKeys.FORUM_UI_SMILIES_LIST);
            XCache.Remove("/Forum/UI/SmiliesListFirstPage");
            XCache.Remove(CacheKeys.FORUM_UI_SMILIES_LIST_WITH_INFO);
            //XCache.Remove("/Forum/UI/SmiliesTypeList");
        }

        //public static bool CreateUpdateUserCreditsProcedure(string creditExpression, bool testCreditExpression)
        //{
        //    return BBX.Data.Forums.CreateUpdateUserCreditsProcedure(creditExpression, testCreditExpression);
        //}

        //public static bool CreateUpdateUserCreditsProcedure(string creditExpression)
        //{
        //    return CreateUpdateUserCreditsProcedure(creditExpression, true);
        //}

        public static string UploadForumIcon(int fId)
        {
            if (fId <= 0)
            {
                return "";
            }
            string text = Path.GetFileName(HttpContext.Current.Request.Files[0].FileName);
            string arg = Utils.CutString(text, text.LastIndexOf(".") + 1).ToLower();
            if (!Attachment.IsImgFilename(text))
            {
                return "";
            }
            text = string.Format("forumicon_{0}.{1}", fId, arg);
            string mapPath = Utils.GetMapPath(BaseConfigs.GetForumPath + "upload/forumicons/");
            if (!Directory.Exists(mapPath))
            {
                Utils.CreateDir(mapPath);
            }
            HttpContext.Current.Request.Files[0].SaveAs(mapPath + text);
            return "upload/forumicons/" + text;
        }

        //public static int UpdateForumTemplateID(IXForum forumInfo)
        //{
        //	string text = FindChildNode(forumInfo.ID.ToString());
        //	if (Utils.IsNumericList(text) && forumInfo.TemplateID >= 0)
        //	{
        //		return BBX.Data.Forums.UpdateForumTemplateID(forumInfo.TemplateID, text);
        //	}
        //	return 0;
        //}
    }
}