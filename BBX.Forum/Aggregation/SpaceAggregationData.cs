using System;
using System.Data;
using System.Text;
using System.Threading;
using System.Xml;
using Discuz.Cache;
using Discuz.Common;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Plugin.Space;

namespace Discuz.Aggregation
{
    public class SpaceAggregationData : AggregationData
    {
        private static StringBuilder __spaceRotatepic = null;
        private static SpaceShortPostInfo[] __spacePostListForWebSite;
        private static SpaceShortPostInfo[] __spacePostListForSpaceIndex;
        private static SpaceConfigInfoExt[] __spaceConfigInfosForWebSite;
        private static SpaceConfigInfoExt[] __spaceConfigInfosForSpaceIndex;
        private static object lockHelper = new object();

        public new string GetRotatePicData()
        {
            if (__spaceRotatepic != null)
            {
                return __spaceRotatepic.ToString();
            }
            __spaceRotatepic = new StringBuilder(base.GetRotatePicStr("Spaceindex"));
            return __spaceRotatepic.ToString();
        }

        public override void ClearDataBind()
        {
            __spacePostListForWebSite = null;
            __spacePostListForSpaceIndex = null;
            __spaceRotatepic = null;
            __spaceConfigInfosForWebSite = null;
            __spaceConfigInfosForSpaceIndex = null;
        }

        public SpaceConfigInfoExt[] GetSpaceListFromFile(string nodeName)
        {
            SpaceConfigInfoExt[] array = (nodeName == "Website") ? __spaceConfigInfosForWebSite : __spaceConfigInfosForSpaceIndex;
            if (array != null)
            {
                return array;
            }
            XmlNodeList xmlNodeList = AggregationData.xmlDoc.DocumentElement.SelectNodes("/Aggregationinfo/Aggregationpage/" + nodeName + "/" + nodeName + "_spacelist/Space");
            array = new SpaceConfigInfoExt[xmlNodeList.Count];
            int num = 0;
            foreach (XmlNode xmlnode in xmlNodeList)
            {
                array[num] = new SpaceConfigInfoExt();
                array[num].Spaceid = ((AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "spaceid") == null) ? 0 : Convert.ToInt32(AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "spaceid")));
                array[num].Userid = ((AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "userid") == null) ? 0 : Convert.ToInt32(AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "userid").Trim()));
                array[num].Spacetitle = ((AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "title") == null) ? "" : Utils.RemoveHtml(AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "title").Trim()));
                array[num].Description = ((AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "description") == null) ? "" : Utils.RemoveHtml(AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "description").Trim()));
                array[num].Postcount = ((AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "postcount") == null) ? 0 : Convert.ToInt32(AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "postcount")));
                array[num].Spacepic = ((AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "pic") == null) ? "" : AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "pic").Trim());
                array[num].Albumcount = ((AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "albumcount") == null) ? 0 : Convert.ToInt32(AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "albumcount")));
                array[num].Postid = ((AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "postid") == null) ? 0 : Convert.ToInt32(AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "postid")));
                array[num].Posttitle = ((AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "posttitle") == null) ? "" : Utils.RemoveHtml(AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "posttitle")));
                num++;
            }
            if (nodeName == "Website")
            {
                __spaceConfigInfosForWebSite = array;
            }
            else
            {
                __spaceConfigInfosForSpaceIndex = array;
            }
            return array;
        }

        public SpaceShortPostInfo[] GetSpacePostList(string nodeName)
        {
            SpaceShortPostInfo[] array = (nodeName == "Website") ? __spacePostListForWebSite : __spacePostListForSpaceIndex;
            if (array != null)
            {
                return array;
            }
            XmlNodeList xmlNodeList = AggregationData.xmlDoc.DocumentElement.SelectNodes("/Aggregationinfo/Aggregationpage/" + nodeName + "/" + nodeName + "_spacearticlelist/Article");
            array = new SpaceShortPostInfo[xmlNodeList.Count];
            int num = 0;
            foreach (XmlNode xmlnode in xmlNodeList)
            {
                array[num] = new SpaceShortPostInfo();
                array[num].Postid = ((AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "postid") == null) ? 0 : Convert.ToInt32(AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "postid")));
                array[num].Author = ((AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "author") == null) ? "" : AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "author").Trim());
                array[num].Uid = ((AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "uid") == null) ? 0 : Convert.ToInt32(AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "uid").Trim()));
                array[num].Postdatetime = ((AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "postdatetime") == null) ? DateTime.Now : Convert.ToDateTime(AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "postdatetime").Trim()));
                array[num].Title = ((AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "title") == null) ? "" : Utils.RemoveHtml(AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "title")));
                array[num].Views = ((AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "views") == null) ? 0 : Convert.ToInt32(AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "views").Trim()));
                array[num].Commentcount = ((AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "commentcount") == null) ? 0 : Convert.ToInt32(AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "commentcount")));
                num++;
            }
            if (nodeName == "Website")
            {
                __spacePostListForWebSite = array;
            }
            else
            {
                __spacePostListForSpaceIndex = array;
            }
            return array;
        }

        public DataTable GetRecentUpdateSpaceList(int count)
        {
            var cacheService = DNTCache.Current;
            DataTable dataTable = cacheService.RetrieveObject("/Space/RecentUpdateSpaceAggregationList") as DataTable;
            if (dataTable == null)
            {
                dataTable = SpacePluginProvider.GetInstance().GetWebSiteAggRecentUpdateSpaceList(count);
                cacheService.AddObject("/Space/RecentUpdateSpaceAggregationList", dataTable, AggregationConfigInfo.Current.RecentUpdateSpaceAggregationListTimeout * 60);
            }
            cacheService.LoadDefaultCacheStrategy();
            return dataTable;
        }

        public DataTable GetTopSpaceList(string orderBy, int topNumber)
        {
            DataTable webSiteAggTopSpaceList = SpacePluginProvider.GetInstance().GetWebSiteAggTopSpaceList(orderBy, topNumber);
            webSiteAggTopSpaceList.Columns.Add("postid", typeof(string));
            webSiteAggTopSpaceList.Columns.Add("posttitle", typeof(string));
            foreach (DataRow dataRow in webSiteAggTopSpaceList.Rows)
            {
                string[] spaceLastPostInfo = SpacePluginProvider.GetInstance().GetSpaceLastPostInfo(int.Parse(dataRow["userid"].ToString()));
                dataRow["postid"] = spaceLastPostInfo[0];
                dataRow["posttitle"] = Utils.RemoveHtml(spaceLastPostInfo[1].ToString());
            }
            return webSiteAggTopSpaceList;
        }

        public DataTable GetTopSpaceListFromCache(string orderBy)
        {
            var cacheService = DNTCache.Current;
            DataTable dataTable = cacheService.RetrieveObject("/Space/Top" + orderBy + "SpaceList") as DataTable;
            if (dataTable == null && orderBy != null)
            {
                if (!(orderBy == "commentcount"))
                {
                    if (orderBy == "visitedtimes")
                    {
                        dataTable = this.GetTopSpaceList(orderBy, AggregationConfigInfo.Current.TopvisitedtimesSpaceListCount);
                        cacheService.AddObject("/Space/Top" + orderBy + "SpaceList", dataTable, AggregationConfigInfo.Current.TopvisitedtimesSpaceListTimeout * 60);
                    }
                }
                else
                {
                    dataTable = this.GetTopSpaceList(orderBy, AggregationConfigInfo.Current.TopcommentcountSpaceListCount);
                    cacheService.AddObject("/Space/Top" + orderBy + "SpaceList", dataTable, AggregationConfigInfo.Current.TopcommentcountPostListTimeout * 60);
                }
            }
            return dataTable;
        }

        public DataTable GetTopSpacePostList(string orderBy, int topNumber)
        {
            return SpacePluginProvider.GetInstance().GetWebSiteAggTopSpacePostList(orderBy, topNumber);
        }

        public DataTable GetTopSpacePostListFromCache(string orderBy)
        {
            DataTable result;
            lock(lockHelper)
            {
                var cacheService = DNTCache.Current;
                DataTable dataTable = cacheService.RetrieveObject("/Space/Top" + orderBy + "PostList") as DataTable;
                if (dataTable == null)
                {
                    string a;
                    if ((a = orderBy) != null)
                    {
                        if (a == "commentcount")
                        {
                            dataTable = this.GetTopSpacePostList(orderBy, AggregationConfigInfo.Current.TopcommentcountPostListCount);
                            cacheService.AddObject("/Space/Top" + orderBy + "PostList", dataTable, AggregationConfigInfo.Current.TopcommentcountPostListTimeout * 60);
                            goto IL_102;
                        }
                        if (a == "views")
                        {
                            dataTable = this.GetTopSpacePostList(orderBy, AggregationConfigInfo.Current.TopviewsPostListCount);
                            cacheService.AddObject("/Space/Top" + orderBy + "PostList", dataTable, AggregationConfigInfo.Current.TopviewsPostListTimeout * 60);
                            goto IL_102;
                        }
                    }
                    orderBy = "commentcount";
                    dataTable = this.GetTopSpacePostList(orderBy, 1);
                    dataTable.Rows.Clear();
                    cacheService.AddObject("/Space/Top" + orderBy + "PostList", dataTable, 300);
                }
            IL_102:
                result = dataTable;
            }
            return result;
        }

        public DataTable SpacePostsList(int pageSize, int currentPage)
        {
            DataTable webSiteAggSpacePostsList = SpacePluginProvider.GetInstance().GetWebSiteAggSpacePostsList(pageSize, currentPage);
            foreach (DataRow dataRow in webSiteAggSpacePostsList.Rows)
            {
                dataRow["content"] = Utils.RemoveHtml(dataRow["content"].ToString());
            }
            return webSiteAggSpacePostsList;
        }

        public int GetSpacePostsCount()
        {
            return SpacePluginProvider.GetInstance().GetWebSiteAggSpacePostsCount();
        }

        public DataTable GetSpaceTopComments(int topNumber)
        {
            if (SpacePluginProvider.GetInstance() == null)
            {
                return new DataTable();
            }
            return SpacePluginProvider.GetInstance().GetWebSiteAggSpaceTopComments(topNumber);
        }

        public DataTable GetSpaceTopComments()
        {
            var cacheService = DNTCache.Current;
            DataTable dataTable = cacheService.RetrieveObject("/Space/SpaceTopNewComments") as DataTable;
            if (dataTable == null)
            {
                dataTable = this.GetSpaceTopComments(AggregationConfigInfo.Current.SpaceTopNewCommentsCount);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    dataRow["author"] = Utils.HtmlEncode(dataRow["author"].ToString().Trim());
                    dataRow["posttitle"] = Utils.HtmlEncode(dataRow["posttitle"].ToString().Trim());
                    dataRow["content"] = Utils.HtmlEncode(dataRow["content"].ToString().Trim());
                }
                cacheService.AddObject("/Space/SpaceTopNewComments", dataTable, AggregationConfigInfo.Current.SpaceTopNewCommentsTimeout * 60);
            }
            return dataTable;
        }
    }
}