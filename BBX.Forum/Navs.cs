using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using Discuz.Common;

using Discuz.Config;
using Discuz.Data;
using Discuz.Entity;

namespace Discuz.Forum
{
    public class Navs
    {
        private static DataTable mainNavigation;
        private static DataTable subNavigation;
        private static string[] mainNavigationHasSub;
        private static List<NavInfo> navigationList;
        private static Predicate<NavInfo> matchParent;

        static Navs()
        {
            Navs.navigationList = Discuz.Data.Navs.GetNavigation();
            Navs.matchParent = ((NavInfo navInfo) => navInfo.Parentid == 0);
            Navs.InitNavigation();
        }

        private static List<NavInfo> GetNavigation()
        {
            if (Navs.navigationList == null)
            {
                Navs.navigationList = Discuz.Data.Navs.GetNavigation();
            }
            return Navs.navigationList;
        }

        public static DataTable GetMainNavigation()
        {
            if (Navs.mainNavigation == null)
            {
                Navs.mainNavigation = new DataTable();
                Navs.mainNavigation.Columns.Add("id", typeof(Int32));
                Navs.mainNavigation.Columns.Add("nav", typeof(String));
                Navs.mainNavigation.Columns.Add("level", typeof(Int32));
                Navs.mainNavigation.Columns.Add("url", typeof(String));
                Navs.mainNavigation.Columns.Add("name", typeof(String));
                foreach (NavInfo current in Discuz.Data.Navs.GetNavigation().FindAll(Navs.matchParent))
                {
                    if (current.Parentid == 0)
                    {
                        DataRow dataRow = Navs.mainNavigation.NewRow();
                        StringBuilder stringBuilder = new StringBuilder();
                        stringBuilder.AppendFormat("<li><a id=\"menu_{0}\" onMouseOver=\"showMenu({{'ctrlid':this.id}});\" href=\"", current.Id);
                        if (!current.Url.StartsWith("http://") && !current.Url.StartsWith("/"))
                        {
                            stringBuilder.Append(BaseConfigs.GetForumPath);
                        }
                        stringBuilder.Append(current.Url.Trim() + "\"");
                        if (current.Target != 0)
                        {
                            stringBuilder.Append(" target=\"_blank\"");
                        }
                        stringBuilder.AppendFormat(" title=\"{0}\">", (current.Title.Trim() != "") ? current.Title.Trim() : current.Name.Trim());
                        if (Utils.InArray(current.Id.ToString(), Navs.GetMainNavigationHasSub()))
                        {
                            dataRow["nav"] = stringBuilder.Append("<span class=\"drop\">" + current.Name.Trim() + "</span></a></li>").ToString();
                        }
                        else
                        {
                            dataRow["nav"] = stringBuilder.Append(current.Name.Trim() + "</a></li>").ToString();
                        }
                        dataRow["id"] = current.Id;
                        dataRow["url"] = current.Url.Trim();
                        dataRow["level"] = current.Level;
                        dataRow["name"] = current.Name;
                        Navs.mainNavigation.Rows.Add(dataRow);
                    }
                }
            }
            return Navs.mainNavigation;
        }

        public static DataTable GetSubNavigation()
        {
            if (Navs.subNavigation == null)
            {
                Navs.subNavigation = new DataTable();
                Navs.subNavigation.Columns.Add("id", typeof(Int32));
                Navs.subNavigation.Columns.Add("nav", typeof(String));
                Navs.subNavigation.Columns.Add("level", typeof(Int32));
                Navs.subNavigation.Columns.Add("parentid", typeof(Int32));
                List<NavInfo> navigation = Discuz.Data.Navs.GetNavigation();
                foreach (NavInfo current in navigation.FindAll(Navs.matchParent))
                {
                    foreach (NavInfo current2 in navigation)
                    {
                        if (current2.Parentid == current.Id)
                        {
                            StringBuilder stringBuilder = new StringBuilder();
                            DataRow dataRow = Navs.subNavigation.NewRow();
                            stringBuilder.Append("<li><a href=\"");
                            if (!current2.Url.StartsWith("http://") && !current2.Url.StartsWith("/"))
                            {
                                stringBuilder.Append(BaseConfigs.GetForumPath);
                            }
                            stringBuilder.Append(current2.Url.Trim());
                            if (current2.Target != 0)
                            {
                                stringBuilder.Append("\" target=\"_blank");
                            }
                            stringBuilder.AppendFormat("\">{0}</a></li>", current2.Name.Trim());
                            dataRow["nav"] = stringBuilder.ToString();
                            dataRow["id"] = current2.Id;
                            dataRow["parentid"] = current2.Parentid;
                            dataRow["level"] = current2.Level;
                            Navs.subNavigation.Rows.Add(dataRow);
                        }
                    }
                }
            }
            return Navs.subNavigation;
        }

        public static string[] GetMainNavigationHasSub()
        {
            if (Navs.mainNavigationHasSub == null)
            {
                string text = "";
                foreach (NavInfo current in Discuz.Data.Navs.GetNavigationHasSub())
                {
                    text = text + current.Parentid + ",";
                    text.Remove(text.Length - 1, 1);
                }
                Navs.mainNavigationHasSub = text.Split(',');
            }
            return Navs.mainNavigationHasSub;
        }

        public static void InitNavigation()
        {
            Navs.GetMainNavigation();
            Navs.GetSubNavigation();
            Navs.GetMainNavigationHasSub();
        }

        public static void ClearNavigation()
        {
            Navs.mainNavigation = null;
            Navs.subNavigation = null;
            Navs.mainNavigationHasSub = null;
            Navs.navigationList = null;
        }

        public static string GetNavigationString(int userid, int useradminid)
        {
            List<string> list = new List<string>();
            DataTable dataTable = Navs.GetMainNavigation();
            if (dataTable != null)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    string value = Navs.ChangeStyleForCurrentUrl(dataRow);
                    switch (Utils.StrToInt(dataRow["level"].ToString(), 4))
                    {
                        case 0:
                            list.Add(value);
                            break;

                        case 1:
                            if (userid != -1)
                            {
                                list.Add(value);
                            }
                            break;

                        case 2:
                            if (useradminid == 3 || useradminid == 1 || useradminid == 2)
                            {
                                list.Add(value);
                            }
                            break;

                        case 3:
                            if (useradminid == 1)
                            {
                                list.Add(value);
                            }
                            break;
                    }
                }
            }
            return string.Join(string.Empty, list.ToArray());
        }

        private static string ChangeStyleForCurrentUrl(DataRow dr)
        {
            string text = dr["nav"].ToString();
            string text2 = dr["url"].ToString().Trim();
            if (Utils.InArray(text2, "/,index.aspx") && DNTRequest.GetPageName() == ((GeneralConfigInfo.Current.Indexpage == 0) ? "forumindex.aspx" : "website.aspx"))
            {
                return Navs.ReplaceCurrentCssClass(text);
            }
            if (!Utils.StrIsNullOrEmpty(text2) && HttpContext.Current.Request.RawUrl.ToString().Contains(text2))
            {
                return Navs.ReplaceCurrentCssClass(text);
            }
            return text;
        }

        private static string ReplaceCurrentCssClass(string nav)
        {
            return nav.Replace("<a", "<a class=\"current\"");
        }

        public static void DeleteNavigation(int id)
        {
            Discuz.Data.Navs.DeleteNavigation(id);
            Navs.ClearNavigation();
            Navs.InitNavigation();
        }

        public static void InsertNavigation(NavInfo nav)
        {
            Discuz.Data.Navs.InsertNavigation(nav);
            Navs.ClearNavigation();
            Navs.InitNavigation();
        }

        public static void UpdateNavigation(NavInfo nav)
        {
            Discuz.Data.Navs.UpdateNavigation(nav);
            Navs.ClearNavigation();
            Navs.InitNavigation();
        }

        public static DataTable GetNavigation(bool getAll)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("id", typeof(Int32));
            dataTable.Columns.Add("parentid", typeof(Int32));
            dataTable.Columns.Add("name", typeof(String));
            dataTable.Columns.Add("title", typeof(String));
            dataTable.Columns.Add("url", typeof(String));
            dataTable.Columns.Add("target", typeof(Int16));
            dataTable.Columns.Add("type", typeof(Int16));
            dataTable.Columns.Add("available", typeof(Int16));
            dataTable.Columns.Add("displayorder", typeof(Int32));
            dataTable.Columns.Add("highlight", typeof(Int16));
            dataTable.Columns.Add("level", typeof(Int32));
            IDataReader navigationData = Discuz.Data.Navs.GetNavigationData(true);
            while (navigationData.Read())
            {
                DataRow dataRow = dataTable.NewRow();
                dataRow["id"] = Utils.StrToInt(navigationData["id"], 0);
                dataRow["parentid"] = Utils.StrToInt(navigationData["parentid"], 0);
                dataRow["name"] = navigationData["name"].ToString().Trim();
                dataRow["title"] = navigationData["title"].ToString().Trim();
                dataRow["url"] = navigationData["url"].ToString().Trim();
                dataRow["target"] = Utils.StrToInt(navigationData["target"], 0);
                dataRow["type"] = Utils.StrToInt(navigationData["type"], 0);
                dataRow["available"] = Utils.StrToInt(navigationData["available"], 0);
                dataRow["displayorder"] = Utils.StrToInt(navigationData["displayorder"], 0);
                dataRow["highlight"] = Utils.StrToInt(navigationData["highlight"], 0);
                dataRow["level"] = Utils.StrToInt(navigationData["level"], 0);
                dataTable.Rows.Add(dataRow);
            }
            navigationData.Close();
            return dataTable;
        }

        public static NavInfo GetNavigation(int id)
        {
            foreach (NavInfo current in Discuz.Data.Navs.GetNavigation(true))
            {
                if (current.Id == id)
                {
                    return current;
                }
            }
            return null;
        }
    }
}