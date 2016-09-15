using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml;
using Discuz.Entity;
using Discuz.Plugin.Album;

namespace Discuz.Aggregation
{
    public class AlbumAggregationData : AggregationData
    {
        private static StringBuilder __albumRotatepic = null;
        private static List<AlbumInfo> __recommandAlbumListForWebSite;
        private static List<AlbumInfo> __recommandAlbumListForSpaceIndex;
        private static List<AlbumInfo> __recommandAlbumListForAlbumIndex;
        private static List<AlbumInfo> __focusAlbumList;
        private static List<PhotoInfo> __focusPhotoList;
        private static List<PhotoInfo> __recommandPhotoList;
        private static List<PhotoInfo> __weekHotPhotoList;

        public new string GetRotatePicData()
        {
            if (__albumRotatepic != null)
            {
                return __albumRotatepic.ToString();
            }
            __albumRotatepic = new StringBuilder();
            __albumRotatepic.Append(base.GetRotatePicStr("Albumindex"));
            return __albumRotatepic.ToString();
        }

        public override void ClearDataBind()
        {
            __recommandAlbumListForWebSite = null;
            __recommandAlbumListForSpaceIndex = null;
            __recommandAlbumListForAlbumIndex = null;
            __focusAlbumList = null;
            __focusPhotoList = null;
            __recommandPhotoList = null;
            __albumRotatepic = null;
            __weekHotPhotoList = null;
        }

        public List<PhotoInfo> GetRecommandPhotoList(string nodeName)
        {
            if (__recommandPhotoList != null)
            {
                return __recommandPhotoList;
            }
            __recommandPhotoList = new List<PhotoInfo>();
            XmlNodeList xmlNodeList = AggregationData.xmlDoc.DocumentElement.SelectNodes("/Aggregationinfo/Aggregationpage/" + nodeName + "/" + nodeName + "_photolist/Photo");
            foreach (XmlNode xmlnode in xmlNodeList)
            {
                PhotoInfo photoInfo = new PhotoInfo();
                photoInfo.Photoid = ((AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "photoid") == null) ? 0 : Convert.ToInt32(AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "photoid")));
                photoInfo.Filename = ((AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "filename") == null) ? "" : AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "filename"));
                photoInfo.Attachment = ((AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "attachment") == null) ? "" : AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "attachment"));
                photoInfo.Filesize = ((AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "filesize") == null) ? 0 : Convert.ToInt32(AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "filesize")));
                photoInfo.Description = ((AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "description") == null) ? "" : AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "description"));
                photoInfo.Postdate = ((AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "postdate") == null) ? "" : AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "postdate"));
                photoInfo.Albumid = ((AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "albumid") == null) ? 0 : Convert.ToInt32(AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "albumid")));
                photoInfo.Userid = ((AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "userid") == null) ? 0 : Convert.ToInt32(AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "userid")));
                photoInfo.Title = ((AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "title") == null) ? "" : AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "title"));
                photoInfo.Views = ((AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "views") == null) ? 0 : Convert.ToInt32(AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "views")));
                photoInfo.Commentstatus = (PhotoStatus)((AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "commentstatus") == null) ? 0 : Convert.ToInt32(AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "commentstatus")));
                photoInfo.Tagstatus = (PhotoStatus)((AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "tagstatus") == null) ? 0 : Convert.ToInt32(AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "tagstatus")));
                photoInfo.Comments = ((AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "comments") == null) ? 0 : Convert.ToInt32(AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "comments")));
                photoInfo.Username = ((AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "username") == null) ? "" : AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "username"));
                __recommandPhotoList.Add(photoInfo);
            }
            return __recommandPhotoList;
        }

        public List<PhotoInfo> GetFocusPhotoList(int type, int focusPhotoCount, int vaildDays)
        {
            if (__focusPhotoList != null)
            {
                return __focusPhotoList;
            }
            __focusPhotoList = new List<PhotoInfo>();
            IDataReader focusPhotoList = AlbumPluginProvider.GetInstance().GetFocusPhotoList(type, focusPhotoCount, vaildDays);
            if (focusPhotoList != null)
            {
                while (focusPhotoList.Read())
                {
                    PhotoInfo photoEntity = AlbumPluginProvider.GetInstance().GetPhotoEntity(focusPhotoList);
                    photoEntity.Filename = AlbumPluginProvider.GetInstance().GetThumbnailImage(photoEntity.Filename);
                    photoEntity.Title = photoEntity.Title.Trim();
                    __focusPhotoList.Add(photoEntity);
                }
                focusPhotoList.Close();
            }
            return __focusPhotoList;
        }

        public List<PhotoInfo> GetWeekHotPhotoList(int focusPhotoCount)
        {
            if (__weekHotPhotoList != null)
            {
                return __weekHotPhotoList;
            }
            __weekHotPhotoList = new List<PhotoInfo>();
            IDataReader focusPhotoList = AlbumPluginProvider.GetInstance().GetFocusPhotoList(0, focusPhotoCount, 7);
            if (focusPhotoList != null)
            {
                while (focusPhotoList.Read())
                {
                    PhotoInfo photoEntity = AlbumPluginProvider.GetInstance().GetPhotoEntity(focusPhotoList);
                    photoEntity.Filename = AlbumPluginProvider.GetInstance().GetThumbnailImage(photoEntity.Filename);
                    photoEntity.Title = photoEntity.Title.Trim();
                    __weekHotPhotoList.Add(photoEntity);
                }
                focusPhotoList.Close();
            }
            return __weekHotPhotoList;
        }

        public List<AlbumInfo> GetRecommandAlbumList(string nodeName)
        {
            List<AlbumInfo> list = null;
            if (nodeName != null)
            {
                if (nodeName == "Website")
                {
                    list = __recommandAlbumListForWebSite;
                    goto IL_52;
                }
                if (nodeName == "Spaceindex")
                {
                    list = __recommandAlbumListForSpaceIndex;
                    goto IL_52;
                }
                if (nodeName == "Albumindex")
                {
                    list = __recommandAlbumListForAlbumIndex;
                    goto IL_52;
                }
            }
            list = __recommandAlbumListForWebSite;
        IL_52:
            if (list != null)
            {
                return list;
            }
            list = new List<AlbumInfo>();
            XmlNodeList xmlNodeList = AggregationData.xmlDoc.DocumentElement.SelectNodes("/Aggregationinfo/Aggregationpage/" + nodeName + "/" + nodeName + "_albumlist/Album");
            foreach (XmlNode xmlnode in xmlNodeList)
            {
                list.Add(new AlbumInfo
                {
                    Albumid = (AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "albumid") == null) ? 0 : Convert.ToInt32(AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "albumid")),
                    Userid = (AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "userid") == null) ? 0 : Convert.ToInt32(AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "userid")),
                    Username = (AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "username") == null) ? "" : AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "username"),
                    Title = (AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "title") == null) ? "" : AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "title"),
                    Description = (AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "description") == null) ? "" : AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "description"),
                    Logo = (AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "logo") == null) ? "" : AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "logo"),
                    Password = (AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "password") == null) ? "" : AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "password"),
                    Imgcount = (AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "imgcount") == null) ? 0 : Convert.ToInt32(AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "imgcount")),
                    Views = (AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "views") == null) ? 0 : Convert.ToInt32(AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "views")),
                    Type = (AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "type") == null) ? 0 : Convert.ToInt32(AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "type")),
                    Createdatetime = (AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "createdatetime") == null) ? "" : AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "createdatetime"),
                    Albumcateid = (AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "albumcateid") == null) ? 0 : Convert.ToInt32(AggregationData.xmlDoc.GetSingleNodeValue(xmlnode, "albumcateid"))
                });
            }
            if (nodeName != null)
            {
                if (nodeName == "Website")
                {
                    __recommandAlbumListForWebSite = list;
                    return list;
                }
                if (nodeName == "Spaceindex")
                {
                    __recommandAlbumListForSpaceIndex = list;
                    return list;
                }
                if (nodeName == "Albumindex")
                {
                    __recommandAlbumListForAlbumIndex = list;
                    return list;
                }
            }
            __recommandAlbumListForWebSite = list;
            return list;
        }

        public List<AlbumInfo> GetAlbumList(int type, int focusPhotoCount, int vaildDays)
        {
            if (__focusAlbumList != null)
            {
                return __focusAlbumList;
            }
            IDataReader albumListByCondition = AlbumPluginProvider.GetInstance().GetAlbumListByCondition(type, focusPhotoCount, vaildDays);
            __focusAlbumList = new List<AlbumInfo>();
            if (albumListByCondition != null)
            {
                while (albumListByCondition.Read())
                {
                    __focusAlbumList.Add(AlbumPluginProvider.GetInstance().GetAlbumEntity(albumListByCondition));
                }
                albumListByCondition.Close();
            }
            return __focusAlbumList;
        }
    }
}