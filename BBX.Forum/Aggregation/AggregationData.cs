using System;
using System.IO;
using System.Text;
using System.Web;
using System.Xml;
using BBX.Cache;
using BBX.Common;
using BBX.Common.Xml;
using BBX.Config;

namespace BBX.Aggregation
{
    /// <summary>聚合数据</summary>
    public class AggregationData
    {
        protected static XmlDocumentExtender xmlDoc;

        private static string filepath;
        /// <summary>数据文件</summary>
        public static string DataFile { get { return filepath; } }

        static AggregationData()
        {
            filepath = HttpContext.Current.Server.MapPath(BaseConfigs.GetForumPath + "config/aggregation.config");
            //picRotateData = null;
            xmlDoc = new XmlDocumentExtender();
            xmlDoc.Load(filepath);
        }

        public static void ReadAggregationConfig()
        {
            xmlDoc = new XmlDocumentExtender();
            xmlDoc.Load(filepath);
        }

        private static String picRotateData;
        public string GetRotatePicData()
        {
            return picRotateData ?? (picRotateData = GetRotatePicStr("Website"));
        }

        protected string GetRotatePicStr(string nodeName)
        {
            xmlDoc.Load(filepath);
            var xmlNodeList = xmlDoc.DocumentElement.SelectNodes("/Aggregationinfo/Aggregationpage/" + nodeName + "/" + nodeName + "_rotatepiclist/" + nodeName + "_rotatepic");
            var sb = new StringBuilder();
            for (int i = 0; i < xmlNodeList.Count; i++)
            {
                sb.Append("data[\"-1_" + (i + 1) + "\"] = \"img: " + xmlDoc.GetSingleNodeValue(xmlNodeList[i], "img").Replace("\"", "\\\"") + "; url: " + xmlDoc.GetSingleNodeValue(xmlNodeList[i], "url").Replace("\"", "\\\"") + "; target: _blank; alt:" + xmlDoc.GetSingleNodeValue(xmlNodeList[i], "titlecontent").Replace("\"", "\\\"") + " ; titlecontent: " + xmlDoc.GetSingleNodeValue(xmlNodeList[i], "titlecontent").Replace("\"", "\\\"") + ";\"\r\n");
            }
            return sb.ToString().Trim();
        }

        public virtual void ClearDataBind()
        {
            picRotateData = null;
        }

        public void ClearAllDataBind()
        {
            this.ClearDataBind();
            AggregationFacade.ForumAggregation.ClearDataBind();
            //AggregationFacade.AlbumAggregation.ClearDataBind();
            //AggregationFacade.PhotoAggregation.ClearDataBind();
            //AggregationFacade.SpaceAggregation.ClearDataBind();
            var cache = XCache.Current;
            XCache.Remove("/Aggregation/HotForumList");
            XCache.Remove("/Aggregation/ForumNewTopicList");
            XCache.Remove("/Aggregation/ForumHotTopicList");
            //cache.RemoveObject("/Space/RecentUpdateSpaceAggregationList");
            //cache.RemoveObject("/Space/ToppostcountSpaceList");
            //cache.RemoveObject("/Space/TopcommentcountSpaceList");
            //cache.RemoveObject("/Space/TopvisitedtimesSpaceList");
            //cache.RemoveObject("/Space/TopcommentcountPostList");
            //cache.RemoveObject("/Space/TopviewsPostList");
            //cache.RemoveObject("/Space/SpaceTopNewComments");
            var fileInfo = new FileInfo(filepath);
            fileInfo.LastWriteTime = DateTime.Now;
            ReadAggregationConfig();
        }
    }
}