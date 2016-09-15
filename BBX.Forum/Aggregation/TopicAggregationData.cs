using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using BBX.Cache;
using BBX.Config;

namespace BBX.Aggregation
{
    public class TopicAggregationData
    {
        public static DataTable GetForumAggregationTopic(int fid)
        {
            var cacheService = XCache.Current;
            var dataTable = cacheService.RetrieveObject("/Aggregation/TopicByForumId_" + fid) as DataTable;
            if (dataTable != null) return dataTable;

            string text = HttpContext.Current.Server.MapPath(BaseConfigs.GetForumPath + "config/agg_" + fid + ".config");
            if (!File.Exists(text)) return new DataTable();

            var xmlDocument = new XmlDocument();
            try
            {
                xmlDocument.Load(text);
                var xmlNode = xmlDocument.SelectSingleNode("/Aggregationinfo/Forum");
                if (xmlNode != null)
                {
                    var dataSet = new DataSet();
                    using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(Regex.Replace(xmlNode.InnerXml, "[\0-\b|\v-\f|\u000e-\u001f]", ""))))
                    {
                        dataSet.ReadXml(memoryStream);
                    }
                    if (dataSet.Tables.Count != 0)
                    {
                        dataTable = dataSet.Tables[0];
                    }
                    else
                    {
                        dataTable = new DataTable();
                    }
                }
                else
                {
                    dataTable = new DataTable();
                }
            }
            catch
            {
                dataTable = new DataTable();
            }
            XCache.Add("/Aggregation/TopicByForumId_" + fid, dataTable, 300);
            return dataTable;
        }

        public static DataTable GetForumAggerationHotTopics()
        {
            var cacheService = XCache.Current;
            DataTable dataTable = cacheService.RetrieveObject("/Aggregation/Hottopiclist") as DataTable;
            if (dataTable != null)
            {
                return dataTable;
            }
            string text = HttpContext.Current.Server.MapPath(BaseConfigs.GetForumPath + "config/agg_hottopics.config");
            if (!File.Exists(text))
            {
                return new DataTable();
            }
            XmlDocument xmlDocument = new XmlDocument();
            try
            {
                xmlDocument.Load(text);
                XmlNode xmlNode = xmlDocument.SelectSingleNode("/Aggregationinfo/Forum");
                if (xmlNode != null)
                {
                    DataSet dataSet = new DataSet();
                    using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(Regex.Replace(xmlNode.InnerXml, "[\0-\b|\v-\f|\u000e-\u001f]", ""))))
                    {
                        dataSet.ReadXml(memoryStream);
                    }
                    if (dataSet.Tables.Count != 0)
                    {
                        dataTable = dataSet.Tables[0];
                    }
                    else
                    {
                        dataTable = new DataTable();
                    }
                }
                else
                {
                    dataTable = new DataTable();
                }
            }
            catch
            {
                dataTable = new DataTable();
            }
            XCache.Add("/Aggregation/Hottopiclist", dataTable, 300);
            return dataTable;
        }
    }
}