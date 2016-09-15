using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml;
using BBX.Cache;
using BBX.Common;
using BBX.Common.Xml;
using BBX.Config;
using BBX.Data;
using BBX.Entity;
using BBX.Forum;
using XCode;

namespace BBX.Aggregation
{
    public class ForumAggregationData : AggregationData
    {
        private static Post[] postInfos;
        private static DataTable topicList;
        private static StringBuilder topicJson;
        private static int[] recommendForumidArray;
        private static TopicOrderType? topicOrderType;

        public override void ClearDataBind()
        {
            postInfos = null;
            topicList = null;
            topicJson = null;
            recommendForumidArray = null;
            topicOrderType = null;
        }

        public int[] GetRecommendForumID()
        {
            if (recommendForumidArray != null) return recommendForumidArray;

            var doc = xmlDoc;
            var xmlNodeList = doc.DocumentElement.SelectNodes("/Aggregationinfo/Aggregationpage/Website/Website_forumrecomend");
            if (xmlNodeList.Count > 0)
            {
                string text = (doc.GetSingleNodeValue(xmlNodeList[0], "fidlist") == null) ? "" : doc.GetSingleNodeValue(xmlNodeList[0], "fidlist");
                if (!Utils.StrIsNullOrEmpty(text))
                {
                    string[] array = text.Split(',');
                    if (Utils.IsNumericArray(array))
                    {
                        recommendForumidArray = new int[array.Length];
                        int num = 0;
                        string[] array2 = array;
                        for (int i = 0; i < array2.Length; i++)
                        {
                            string text2 = array2[i];
                            if (text2 != "")
                            {
                                recommendForumidArray[num] = Convert.ToInt32(text2);
                                num++;
                            }
                        }
                    }
                }
            }
            if (recommendForumidArray != null)
            {
                return recommendForumidArray;
            }
            return new int[1];
        }

        public TopicOrderType GetForumAggregationTopicListOrder()
        {
            if (!topicOrderType.HasValue)
            {
                var doc = new XmlDocumentExtender();
                doc.Load(Utils.GetMapPath(BaseConfigs.GetForumPath + "config/aggregation.config"));
                topicOrderType = new TopicOrderType?((TopicOrderType)TypeConverter.ObjectToInt(doc.GetSingleNodeValue(doc.SelectNodes("/Aggregationinfo/Aggregationpage/Website/Forum")[0], "Bbs/Showtype")));
            }
            return topicOrderType.Value;
        }

        public Post[] GetPostListFromFile(string nodeName)
        {
            if (postInfos != null) return postInfos;

            var doc = xmlDoc;
            var xmlNodeList = doc.DocumentElement.SelectNodes("/Aggregationinfo/Aggregationpage/" + nodeName + "/Forum/Topiclist/Topic");
            postInfos = new Post[xmlNodeList.Count];
            int num = 0;
            foreach (XmlNode xmlnode in xmlNodeList)
            {
                postInfos[num] = new Post();
                postInfos[num].Tid = TypeConverter.ObjectToInt(doc.GetSingleNodeValue(xmlnode, "topicid"));
                postInfos[num].Title = ((doc.GetSingleNodeValue(xmlnode, "title") == null) ? "" : doc.GetSingleNodeValue(xmlnode, "title"));
                postInfos[num].Poster = ((doc.GetSingleNodeValue(xmlnode, "poster") == null) ? "" : doc.GetSingleNodeValue(xmlnode, "poster"));
                postInfos[num].PosterID = TypeConverter.ObjectToInt(doc.GetSingleNodeValue(xmlnode, "posterid"));
                postInfos[num].PostDateTime = (doc.GetSingleNodeValue(xmlnode, "postdatetime") == null) ? DateTime.MinValue : doc.GetSingleNodeValue(xmlnode, "postdatetime").ToDateTime();
                postInfos[num].Message = ((doc.GetSingleNodeValue(xmlnode, "shortdescription") == null) ? "" : doc.GetSingleNodeValue(xmlnode, "shortdescription"));
                postInfos[num].Fid = TypeConverter.ObjectToInt(doc.GetSingleNodeValue(xmlnode, "fid"));
                //postInfos[num].Forumname = ((doc.GetSingleNodeValue(xmlnode, "forumname") == null) ? "" : doc.GetSingleNodeValue(xmlnode, "forumname"));
                //postInfos[num].ForumRewriteName = ((doc.GetSingleNodeValue(xmlnode, "forumrewritename") == null) ? "" : doc.GetSingleNodeValue(xmlnode, "forumrewritename"));
                num++;
            }
            return postInfos;
        }

        public string GetTopicJsonFromFile()
        {
            if (topicJson != null) return topicJson.ToString();

            topicJson = new StringBuilder();
            topicJson.Append("[");
            var doc = xmlDoc;
            var xmlNodeList = doc.DocumentElement.SelectNodes("/Aggregationinfo/Aggregationpage/Website/Website_forumrecomendtopiclist/Website_forumrecomendtopic");
            int num = 1;
            foreach (XmlNode xmlnode in xmlNodeList)
            {
                topicJson.Append(string.Format("{{'id' : {0}, 'title' : '{1}', 'fid' : {2}, 'img' : '{3}', 'tid' : {4}}},", new object[]
                {
                    num,
                    (doc.GetSingleNodeValue(xmlnode, "title") == null) ? "" : doc.GetSingleNodeValue(xmlnode, "title"),
                    (doc.GetSingleNodeValue(xmlnode, "fid") == null) ? 0 : Convert.ToInt32(doc.GetSingleNodeValue(xmlnode, "fid")),
                    (doc.GetSingleNodeValue(xmlnode, "img") == null) ? "" : doc.GetSingleNodeValue(xmlnode, "img"),
                    (doc.GetSingleNodeValue(xmlnode, "tid") == null) ? 0 : Convert.ToInt32(doc.GetSingleNodeValue(xmlnode, "tid"))
                }));
                num++;
            }
            if (topicJson.ToString().EndsWith(","))
            {
                topicJson.Remove(topicJson.Length - 1, 1);
            }
            topicJson.Append("]");
            return topicJson.ToString();
        }

        public DataTable GetForumTopicList()
        {
            if (topicList != null) return topicList;

            int topNumber = 10;
            var doc = xmlDoc;
            XmlNode xmlnode = doc.DocumentElement.SelectNodes("/Aggregationinfo/Aggregationpage/Website/Forum/Bbs").Item(0);
            if (doc.GetSingleNodeValue(xmlnode, "Topnumber") != null)
            {
                try
                {
                    topNumber = Convert.ToInt32(doc.GetSingleNodeValue(xmlnode, "Topnumber").ToLower());
                }
                catch
                {
                    topNumber = 10;
                }
            }
            if (topicOrderType.HasValue)
            {
                topicList = DatabaseProvider.GetInstance().GetWebSiteAggForumTopicList(doc.GetSingleNodeValue(xmlnode, "Showtype").ToLower(), topNumber);
            }
            else
            {
                topicList = DatabaseProvider.GetInstance().GetWebSiteAggForumTopicList("3", topNumber);
            }
            return topicList;
        }

        public List<IXForum> GetHotForumList(int topNumber, string orderby, int fid)
        {
            orderby = ((String.IsNullOrEmpty(orderby)) ? "posts" : orderby);
            var cacheService = XCache.Current;
            var list = XCache.Retrieve<List<IXForum>>("/Aggregation/HotForumList");
            if (list == null)
            {
                list = XForum.GetWebSiteAggHotForumList((topNumber <= 0) ? 10 : topNumber, orderby, fid);
                XCache.Add("/Aggregation/HotForumList", list, 300);
            }
            return list;
        }

        public EntityList<Topic> GetForumTopicList(int count, int views, int forumid, TopicTimeType timeType, TopicOrderType orderType, bool isDigest, bool onlyImg)
        {
            return Focuses.GetTopicList(count, views, forumid, "", timeType, orderType, isDigest, 5, onlyImg, "");
        }

        //public DataTable GetUserList(int topNumber, string orderBy)
        //{
        //    var cacheService = XCache.Current;
        //    DataTable dataTable = XCache.Retrieve<DataTable>("/Aggregation/Users_" + orderBy + "List");
        //    if (dataTable == null)
        //    {
        //        dataTable = BBX.Forum.Users.GetUserList(topNumber, 1, orderBy, "desc");
        //        XCache.Add("/Aggregation/Users_" + orderBy + "List", dataTable, 300);
        //    }
        //    return dataTable;
        //}

        public EntityList<Post> GetLastPostList(int fid, int count)
        {
            var cacheService = XCache.Current;
            var list = XCache.Retrieve<EntityList<Post>>("/Aggregation/lastpostList_" + fid);
            if (list == null)
            {
                //list = DatabaseProvider.GetInstance().GetLastPostList(fid, count, TableList.CurrentTableName, BBX.Forum.Forums.GetVisibleForum());
                list = Post.GetLastPostList(fid, count);
                XCache.Add("/Aggregation/lastpostList_" + fid, list, 300);
            }
            return list;
        }

        public List<Post> GetUserPostCountList(int topNumber, DateType dateType, int dateNum)
        {
            var list = XCache.Retrieve<List<Post>>("/Aggregation/UserPostCountList");
            if (list == null)
            {
                list = Post.GetUserPostCountList(topNumber, dateType, (dateNum > 1) ? dateNum : 1);
                XCache.Add("/UserPostCountList", list, 120);
            }
            return list;
        }
    }
}