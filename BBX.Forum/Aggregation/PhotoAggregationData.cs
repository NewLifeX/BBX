using System;
using System.Xml;
using Discuz.Entity;

namespace Discuz.Aggregation
{
    public class PhotoAggregationData : AggregationData
    {
        private static PhotoAggregationInfo __photoAggregationInfo;

        public override void ClearDataBind()
        {
            __photoAggregationInfo = null;
        }

        public PhotoAggregationInfo GetPhotoAggregationInfo()
        {
            if (__photoAggregationInfo == null)
            {
                return this.GetPhotoAggregationInfoFromFile();
            }
            return __photoAggregationInfo;
        }

        public PhotoAggregationInfo GetPhotoAggregationInfoFromFile()
        {
            XmlNode xmlNode = AggregationData.xmlDoc.DocumentElement.SelectNodes("/Aggregationinfo/Aggregationpage/Albumindex/Albumconfig")[0];
            __photoAggregationInfo = new PhotoAggregationInfo();
            if (xmlNode != null)
            {
                __photoAggregationInfo.Focusphotoshowtype = ((AggregationData.xmlDoc.GetSingleNodeValue(xmlNode, "Focusphotoshowtype") == null) ? 0 : Convert.ToInt32(AggregationData.xmlDoc.GetSingleNodeValue(xmlNode, "Focusphotoshowtype")));
                __photoAggregationInfo.Focusphotodays = ((AggregationData.xmlDoc.GetSingleNodeValue(xmlNode, "Focusphotodays") == null) ? 0 : Convert.ToInt32(AggregationData.xmlDoc.GetSingleNodeValue(xmlNode, "Focusphotodays")));
                __photoAggregationInfo.Focusphotocount = ((AggregationData.xmlDoc.GetSingleNodeValue(xmlNode, "Focusphotocount") == null) ? 0 : Convert.ToInt32(AggregationData.xmlDoc.GetSingleNodeValue(xmlNode, "Focusphotocount")));
                __photoAggregationInfo.Focusalbumshowtype = ((AggregationData.xmlDoc.GetSingleNodeValue(xmlNode, "Focusalbumshowtype") == null) ? 0 : Convert.ToInt32(AggregationData.xmlDoc.GetSingleNodeValue(xmlNode, "Focusalbumshowtype")));
                __photoAggregationInfo.Focusalbumdays = ((AggregationData.xmlDoc.GetSingleNodeValue(xmlNode, "Focusalbumdays") == null) ? 0 : Convert.ToInt32(AggregationData.xmlDoc.GetSingleNodeValue(xmlNode, "Focusalbumdays")));
                __photoAggregationInfo.Focusalbumcount = ((AggregationData.xmlDoc.GetSingleNodeValue(xmlNode, "Focusalbumcount") == null) ? 0 : Convert.ToInt32(AggregationData.xmlDoc.GetSingleNodeValue(xmlNode, "Focusalbumcount")));
                __photoAggregationInfo.Weekhot = ((AggregationData.xmlDoc.GetSingleNodeValue(xmlNode, "Weekhot") == null) ? 0 : Convert.ToInt32(AggregationData.xmlDoc.GetSingleNodeValue(xmlNode, "Weekhot")));
            }
            return __photoAggregationInfo;
        }

        public void SaveAggregationData(PhotoAggregationInfo photoAggregationInfo)
        {
            XmlNode xmlNode = AggregationData.xmlDoc.SelectSingleNode("/Aggregationinfo/Aggregationpage/Albumindex/Albumconfig");
            if (xmlNode != null)
            {
                xmlNode.RemoveAll();
            }
            else
            {
                xmlNode = AggregationData.xmlDoc.CreateNode("/Aggregationinfo/Aggregationdata/Space");
            }
            AggregationData.xmlDoc.AppendChildElementByNameValue(ref xmlNode, "Focusphotoshowtype", photoAggregationInfo.Focusphotoshowtype);
            AggregationData.xmlDoc.AppendChildElementByNameValue(ref xmlNode, "Focusphotodays", photoAggregationInfo.Focusphotodays);
            AggregationData.xmlDoc.AppendChildElementByNameValue(ref xmlNode, "Focusphotocount", photoAggregationInfo.Focusphotocount);
            AggregationData.xmlDoc.AppendChildElementByNameValue(ref xmlNode, "Focusalbumshowtype", photoAggregationInfo.Focusalbumshowtype);
            AggregationData.xmlDoc.AppendChildElementByNameValue(ref xmlNode, "Focusalbumdays", photoAggregationInfo.Focusalbumdays);
            AggregationData.xmlDoc.AppendChildElementByNameValue(ref xmlNode, "Focusalbumcount", photoAggregationInfo.Focusalbumcount);
            AggregationData.xmlDoc.AppendChildElementByNameValue(ref xmlNode, "Weekhot", photoAggregationInfo.Weekhot);
            AggregationData.xmlDoc.Save(AggregationData.DataFilePath);
        }
    }
}