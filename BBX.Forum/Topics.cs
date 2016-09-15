using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using BBX.Common;
using BBX.Config;
using BBX.Entity;
using XCode;

namespace BBX.Forum
{
    public enum MagicType
    {
        HtmlTitle = 1,
        MagicTopic,
        TopicTag
    }

    public class Topics
    {
        private static object lockHelper = new object();
        //private static MemCachedConfigInfo mcci = MemCachedConfigs.GetConfig();
        //private static RedisConfigInfo rci = RedisConfigs.GetConfig();

        //public static Int32 GetTopicsCountbyUserId(Int32 userId, bool isReplyUid)
        //{
        //    if (isReplyUid)
        //    {
        //        return BBX.Data.Topics.GetTopicReplyCountbByUserId(userId);
        //    }
        //    return BBX.Data.Topics.GetTopicCountByUserId(userId);
        //}

        //public static Int32 CreateTopic(TopicInfo topicinfo)
        //{
        //    if (topicinfo == null) return 0;

        //    lock (lockHelper)
        //    {
        //        return BBX.Data.Topics.CreateTopic(topicinfo);
        //    }
        //}

        //public static void AddParentForumTopics(string fpidlist, Int32 topics)
        //{
        //	if (Utils.IsNumericList(fpidlist) && topics > 0)
        //	{
        //		DatabaseProvider.GetInstance().AddParentForumTopics(fpidlist, topics);
        //	}
        //}

        //public static TopicInfo GetTopicInfo(Int32 tid)
        //{
        //    //return Topics.GetTopicInfo(tid, 0, 0);
        //    return Topic.FindByID(tid).Cast<TopicInfo>();
        //}

        //public static TopicInfo GetTopicInfo(Int32 tid, Int32 fid, byte mode)
        //{
        //	return BBX.Data.Topics.GetTopicInfo(tid, fid, mode);
        //}

        //public static DataTable GetTopicList(string topicList)
        //{
        //	if (!Utils.IsNumericList(topicList))
        //	{
        //		return null;
        //	}
        //	return Topics.GetTopicList(topicList, -10);
        //}

        //public static DataTable GetTopicList(string topicList, Int32 displayOrder)
        //{
        //	if (!Utils.IsNumericList(topicList))
        //	{
        //		return null;
        //	}
        //	return BBX.Data.Topics.GetTopicList(topicList, displayOrder);
        //}

        //public static DataRow GetTopTopicListID(Int32 fid)
        //{
        //    DataRow dataRow = null;
        //    string mapPath = Utils.GetMapPath(BaseConfigs.GetForumPath + "cache/topic/" + fid + ".xml");
        //    if (Utils.FileExists(mapPath))
        //    {
        //        DataSet dataSet = new DataSet();
        //        dataSet.ReadXml(mapPath, XmlReadMode.ReadSchema);
        //        if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
        //        {
        //            dataRow = dataSet.Tables[0].Rows[0];
        //            if (Utils.CutString(dataRow["tid"].ToString(), 0, 1) == ",")
        //            {
        //                dataRow["tid"] = Utils.CutString(dataRow["tid"].ToString(), 1);
        //            }
        //        }
        //        dataSet.Dispose();
        //    }
        //    return dataRow;
        //}

        public static void UpdateTopicReplyCount(Int32 tid)
        {
            //BBX.Data.Topics.UpdateTopicReplyCount(tid, TableList.GetPostTableId(tid));
            var tp = Topic.FindByID(tid);
            tp.Replies = Post.GetPostCountByTid(tid) - 1;
            tp.Save();
        }

        //public static Int32 GetTopicCount(Int32 fid)
        //{
        //    return BBX.Data.Topics.GetTopicCount(fid);
        //}

        //public static Int32 GetTopicCount(Int32 fid, bool includeClosedTopic, string condition)
        //{
        //    return BBX.Data.Topics.GetTopicCount(fid, includeClosedTopic, condition);
        //}

        //public static Int32 GetTopicCount(string condition)
        //{
        //    return BBX.Data.Topics.GetTopicCount(condition);
        //}

        //public static Int32 UpdateTopicModerated(string topiclist, Int32 moderated)
        //{
        //	return BBX.Data.Topics.UpdateTopicModerated(topiclist, moderated);
        //}

        //public static Int32 UpdateTopic(TopicInfo topicinfo)
        //{
        //    //if (((Topics.mcci != null && Topics.mcci.ApplyMemCached) || (Topics.rci != null && Topics.rci.ApplyRedis)) && topicinfo.Displayorder > 0)
        //    //{
        //    //    if (topicinfo.Displayorder == 1)
        //    //    {
        //    //        DNTCache.Current.RemoveObject("/Forum/ShowTopic/TopList/" + topicinfo.Fid + "/");
        //    //    }
        //    //    else
        //    //    {
        //    //        foreach (ForumInfo current in Forums.GetForumList())
        //    //        {
        //    //            if (current.Layer > 0)
        //    //            {
        //    //                DNTCache.Current.RemoveObject("/Forum/ShowTopic/TopList/" + current.Fid + "/");
        //    //            }
        //    //        }
        //    //    }
        //    //}
        //    if (topicinfo == null)
        //    {
        //        return 0;
        //    }
        //    return BBX.Data.Topics.UpdateTopic(topicinfo);
        //}

        //public static bool InSameForum(string topicidlist, Int32 fid)
        //{
        //	return Utils.SplitString(topicidlist, ",").Length == BBX.Data.Topics.GetTopicCountInForumAndTopicIdList(topicidlist, fid);
        //}

        //public static Int32 UpdateTopicHide(Int32 tid)
        //{
        //    return BBX.Data.Topics.UpdateTopicHide(tid);
        //}

        //public static DataTable GetTopicList(Int32 forumid, Int32 pageid, Int32 tpp)
        //{
        //    return BBX.Data.Topics.GetTopicList(forumid, pageid, tpp);
        //}

        //public static DataTable GetTopicTypeName(DataTable topiclist)
        //{
        //	DataColumn dataColumn = new DataColumn();
        //	dataColumn.ColumnName = "topictypename";
        //	dataColumn.DataType = typeof(String);
        //	dataColumn.DefaultValue = "";
        //	dataColumn.AllowDBNull = true;
        //	topiclist.Columns.Add(dataColumn);
        //	var topicTypeArray = TopicType.GetTopicTypeArray();
        //	foreach (DataRow dataRow in topiclist.Rows)
        //	{
        //		object obj = topicTypeArray[Int32.Parse(dataRow["typeid"].ToString())];
        //		dataRow["topictypename"] = ((obj != null && obj.ToString().IsNullOrEmpty()) ? ("[" + obj.ToString().Trim() + "]") : "");
        //	}
        //	return topiclist;
        //}

        public static List<Topic> GetTopicsByReplyUserId(Int32 userId, Int32 pageIndex, Int32 pageSize, Int32 newmin, Int32 hot)
        {
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }
            var topicListByReplyUserId = Topic.GetTopicListByReplyUserId(userId, pageIndex, pageSize);
            foreach (var current in topicListByReplyUserId)
            {
                //Topics.LoadTopicForumName(current);
                Topics.LoadTopicFolder(0, newmin, hot, current);
                //Topics.LoadTopicHighlightTitle(current);
            }
            return topicListByReplyUserId;
        }

        //public static List<TopicInfo> GetUnauditNewTopic(string forumidlist, Int32 tpp, Int32 pageid, Int32 filter)
        //{
        //    List<TopicInfo> unauditNewTopic = BBX.Data.Topics.GetUnauditNewTopic(forumidlist, tpp, pageid, filter);
        //    foreach (TopicInfo current in unauditNewTopic)
        //    {
        //        current.Forumname = Forums.GetForumInfo(current.Fid).Name;
        //    }
        //    return unauditNewTopic;
        //}

        //public static List<TopicInfo> GetMyUnauditTopic(Int32 posterId, Int32 tpp, Int32 pageId, Int32 filter)
        //{
        //	List<TopicInfo> myUnauditTopic = BBX.Data.Topics.GetMyUnauditTopic(posterId, tpp, pageId, filter);
        //	foreach (TopicInfo current in myUnauditTopic)
        //	{
        //		current.Forumname = Forums.GetForumInfo(current.Fid).Name;
        //	}
        //	return myUnauditTopic;
        //}

        //public static Int32 GetAttentionTopicCount(string fidlist, string keyword)
        //{
        //    if (!Utils.IsNumericList(fidlist))
        //    {
        //        return 0;
        //    }
        //    return BBX.Data.Topics.GetAttentionTopicCount(fidlist, keyword);
        //}

        //public static Int32 GetUnauditNewTopicCount(string fidlist, Int32 filter)
        //{
        //    if (!Utils.IsNumericList(fidlist))
        //    {
        //        return 0;
        //    }
        //    return BBX.Data.Topics.GetUnauditNewTopicCount(fidlist, filter);
        //}

        //public static Int32 GetMyUnauditTopicCount(Int32 posterid, Int32 filter)
        //{
        //    return BBX.Data.Topics.GetMyUnauditTopicCount(posterid, filter);
        //}

        public static List<Topic> GetTopicsByUserId(Int32 userId, Int32 pageIndex, Int32 pageSize, Int32 newmin, Int32 hot)
        {
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }
            //var list = BBX.Data.Topics.GetTopicsByUserId(userId, pageIndex, pageSize);
            var list = Topic.FindAllByPosterID(userId, pageIndex, pageSize);
            foreach (var current in list)
            {
                //Topics.LoadTopicForumName(current);
                //Topics.LoadTopicHighlightTitle(current);
                Topics.LoadTopicFolder(0, newmin, hot, current);
            }
            return list;
        }

        //public static List<Topic> GetTopTopicList(Int32 fid, Int32 pageSize, Int32 pageIndex, string tids, Int32 autocloseTime, Int32 topicTypePrefix)
        //{
        //    if (pageIndex < 1) pageIndex = 1;

        //    var list = Topic.GetTopTopicList(fid, pageSize, pageIndex, tids);
        //    LoadTopTopicListExtraInfo(topicTypePrefix, list);

        //    return list;
        //}

        public static List<Topic> GetTopTopicList(List<Topic> list, Int32 pageSize, Int32 pageIndex, Int32 autocloseTime, Int32 topicTypePrefix)
        {
            if (pageIndex < 1) pageIndex = 1;

            list = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            LoadTopTopicListExtraInfo(topicTypePrefix, list);

            return list;
        }

        //public static List<Topic> GetTopicList(Int32 fid, Int32 pageSize, Int32 pageIndex, Int32 startNumber, Int32 newMinutes, Int32 hotReplyNumber, Int32 autocloseTime, Int32 topicTypePrefix, string condition)
        //{
        //    if (pageIndex < 1)
        //    {
        //        pageIndex = 1;
        //    }
        //    var topicList = Topic.GetTopicList(fid, pageSize, pageIndex, startNumber, condition);
        //    LoadTopicListExtraInfo(autocloseTime, topicTypePrefix, topicList);
        //    return topicList;
        //}

        public static List<Topic> GetTopicList(Int32 fid, Int32 typeid, Int32 days, String filter, Int32 order, Boolean isDesc, Int32 pageSize, Int32 pageIndex, Int32 startNumber, Int32 newMinutes, Int32 hotReplyNumber, Int32 autocloseTime, Int32 topicTypePrefix)
        {
            if (pageIndex < 1) pageIndex = 1;

            //List<Topic> list = null;
            //if (Utils.InArray(orderby, "views,replies"))
            //{
            //    list = Topic.GetTopicListByViewsOrReplies(fid, pageSize, pageIndex, startNumber, condition, orderby, ascdesc);
            //}
            //else
            //{
            //    list = Topic.GetTopicListByDate(fid, pageSize, pageIndex, startNumber, condition, orderby, ascdesc);
            //}
            var start = (pageIndex - 1) * pageSize - startNumber;
            var list = Topic.Search(fid, typeid, days, filter, order, isDesc, start, pageSize);
            LoadTopicListExtraInfo(autocloseTime, topicTypePrefix, newMinutes, hotReplyNumber, list);
            return list;
        }

        //public static List<Topic> GetTopicList(Int32 fid, Int32 pageSize, Int32 pageIndex, Int32 startNumber, Int32 newMinutes, Int32 hotReplyNumber, Int32 autocloseTime, Int32 topicTypePrefix, string condition, string orderby, Int32 ascdesc)
        //{
        //    if (pageIndex < 1) pageIndex = 1;

        //    List<Topic> list = null;
        //    if (Utils.InArray(orderby, "views,replies"))
        //    {
        //        list = Topic.GetTopicListByViewsOrReplies(fid, pageSize, pageIndex, startNumber, condition, orderby, ascdesc);
        //    }
        //    else
        //    {
        //        list = Topic.GetTopicListByDate(fid, pageSize, pageIndex, startNumber, condition, orderby, ascdesc);
        //    }
        //    Topics.LoadTopicListExtraInfo(autocloseTime, topicTypePrefix, newMinutes, hotReplyNumber, list);
        //    return list;
        //}

        public static List<Topic> GetTopicListByCondition(Int32 pageSize, Int32 pageIndex, Int32 startNumber, Int32 newMinutes, Int32 hotReplyNumber, Int32 autocloseTime, Int32 topicTypePrefix, string condition, string orderby, Int32 ascdesc)
        {
            if (pageIndex < 1) pageIndex = 1;

            var list = Topic.GetTopicListByCondition(pageSize, pageIndex, startNumber, condition, orderby, ascdesc);
            Topics.LoadTopicListExtraInfo(autocloseTime, topicTypePrefix, newMinutes, hotReplyNumber, list);
            return list;
        }

        public static void WriteHtmlTitleFile(string htmltitle, Int32 topicid)
        {
            var sb = new StringBuilder();
            sb.Append(BaseConfigs.GetForumPath);
            sb.Append("cache/topic/magic/");
            if (!Directory.Exists(Utils.GetMapPath(sb.ToString())))
            {
                Utils.CreateDir(Utils.GetMapPath(sb.ToString()));
            }
            sb.Append(topicid / 1000 + 1);
            sb.Append("/");
            if (!Directory.Exists(Utils.GetMapPath(sb.ToString())))
            {
                Utils.CreateDir(Utils.GetMapPath(sb.ToString()));
            }
            string mapPath = Utils.GetMapPath(sb.ToString() + topicid.ToString() + "_htmltitle.config");
            try
            {
                using (FileStream fileStream = new FileStream(mapPath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(Utils.RemoveUnsafeHtml(htmltitle));
                    fileStream.Write(bytes, 0, bytes.Length);
                    fileStream.Close();
                }
            }
            catch
            {
            }
        }

        public static Int32 GetMagicValue(Int32 magic, MagicType magicType)
        {
            if (magic == 0)
            {
                return 0;
            }
            string text = magic.ToString();
            switch (magicType)
            {
                case MagicType.HtmlTitle:
                    if (text.Length >= 2)
                    {
                        return text.Substring(1, 1).ToInt();
                    }
                    break;

                case MagicType.MagicTopic:
                    if (text.Length >= 5)
                    {
                        return text.Substring(2, 3).ToInt();
                    }
                    break;

                case MagicType.TopicTag:
                    if (text.Length >= 6)
                    {
                        return text.Substring(5, 1).ToInt();
                    }
                    break;
            }
            return 0;
        }

        public static Int32 SetMagicValue(Int32 magic, MagicType magicType, Int32 newmagicvalue)
        {
            string[] array = Utils.SplitString(magic.ToString(), "");
            switch (magicType)
            {
                case MagicType.HtmlTitle:
                    if (array.Length >= 2)
                    {
                        array[1] = newmagicvalue.ToString().Substring(0, 1);
                        return String.Join("", array).ToInt(magic);
                    }
                    return String.Format("1{0}", newmagicvalue.ToString().Substring(0, 1)).ToInt(magic);
                case MagicType.MagicTopic:
                    if (array.Length >= 5)
                    {
                        string[] array2 = Utils.SplitString(newmagicvalue.ToString().PadLeft(3, '0'), "");
                        array[2] = array2[0];
                        array[3] = array2[1];
                        array[4] = array2[2];
                        return String.Join("", array).ToInt(magic);
                    }
                    return String.Format("1{0}{1}", Topics.GetMagicValue(magic, MagicType.HtmlTitle), newmagicvalue.ToString().PadLeft(3, '0').Substring(0, 3)).ToInt(magic);
                case MagicType.TopicTag:
                    if (array.Length >= 6)
                    {
                        array[5] = newmagicvalue.ToString().Substring(0, 1);
                        return String.Join("", array).ToInt(magic);
                    }
                    return String.Format("1{0}{1}{2}", Topics.GetMagicValue(magic, MagicType.HtmlTitle), Topics.GetMagicValue(magic, MagicType.MagicTopic).ToString("000"), newmagicvalue.ToString().Substring(0, 1)).ToInt(magic);
                default:
                    return magic;
            }
        }

        public static string GetHtmlTitle(Int32 topicid)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(BaseConfigs.GetForumPath);
            stringBuilder.Append("cache/topic/magic/");
            stringBuilder.Append(topicid / 1000 + 1);
            stringBuilder.Append("/");
            string mapPath = Utils.GetMapPath(stringBuilder.ToString() + topicid.ToString() + "_htmltitle.config");
            if (!File.Exists(mapPath))
            {
                return "";
            }
            string result;
            using (FileStream fileStream = new FileStream(mapPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    result = streamReader.ReadToEnd();
                }
            }
            return result;
        }

        //public static List<TopicInfo> GetRelatedTopicList(Int32 topicId, Int32 count)
        //{
        //	if (topicId <= 0)
        //	{
        //		return null;
        //	}
        //	return TopicTagCaches.GetRelatedTopicList(topicId, count);
        //}

        //public static Int32 GetTopicCountByTagId(Int32 tagId)
        //{
        //    if (tagId <= 0)
        //    {
        //        return 0;
        //    }
        //    return BBX.Data.Topics.GetTopicCountByTagId(tagId);
        //}

        //public static List<TopicInfo> GetTopicsWithSameTag(Int32 tagid, Int32 count)
        //{
        //	return Topics.GetTopicListByTagId(tagid, 1, count);
        //}

        public static List<Topic> GetTopicListByTagId(Int32 tagId, Int32 pageIndex, Int32 pageSize)
        {
            if (pageIndex < 1) pageIndex = 1;

            //var topicListByTagId = BBX.Data.Topics.GetTopicListByTagId(tagId, pageIndex, pageSize);
            var topicListByTagId = Topic.GetTopicListByTagId(tagId, pageIndex, pageSize);
            //foreach (TopicInfo current in topicListByTagId)
            //{
            //	Topics.LoadTopicForumName(current);
            //}
            return topicListByTagId;
        }

        public static void NeatenRelateTopics()
        {
            // 暂时不知道如何改进，可搜索存储过程[dnt_neatenrelatetopic]
            //TopicTagCaches.NeatenRelateTopics();
        }

        //public static void DeleteRelatedTopics(Int32 topicId)
        //{
        //	TopicTagCaches.DeleteRelatedTopics(topicId);
        //}

        //public static void CreateDebateTopic(DebateInfo debateTopic)
        //{
        //    BBX.Data.Debates.CreateDebateTopic(debateTopic);
        //}

        public static void PassAuditNewTopic(string ignore, string validate, string delete, Int32[] fids)
        {
            if ((!string.IsNullOrEmpty(ignore) && !Utils.IsNumericList(ignore)) || (!string.IsNullOrEmpty(validate) && !Utils.IsNumericList(validate)) || (!string.IsNullOrEmpty(delete) && !Utils.IsNumericList(delete)) || fids == null || fids.Length == 0)
            {
                return;
            }
            //BBX.Data.Topics.PassAuditNewTopic(postTableId, ignore, validate, delete, fidlist);
            if (!ignore.IsNullOrWhiteSpace())
            {
                var list = Topic.FindAllByIDs(ignore);
                list.ForEach(e => e.DisplayOrder = -3);
                list.Save();
            }
            if (!delete.IsNullOrWhiteSpace())
            {
                var list = Topic.FindAllByIDs(delete);
                list.Delete();
            }
            if (!string.IsNullOrEmpty(validate))
            {
                //foreach (DataRow dataRow in Topics.GetTopicList(validate).Rows)
                //{
                //	CreditsFacade.PostTopic(TypeConverter.ObjectToInt(dataRow["posterid"]), Forums.GetForumInfo(TypeConverter.ObjectToInt(dataRow["fid"])));
                //}
                //foreach (var tp in Topic.FindAllByTidsAndDisplayOrder(validate, -10))
                //{
                //    CreditsFacade.PostTopic(tp.ID, Forums.GetForumInfo(tp.Fid));
                //}
                var list = Topic.FindAllByIDs(validate);
                foreach (var item in list)
                {
                    item.Post.Invisible = 0;
                    item.Post.Save();

                    if (item.DisplayOrder > -10)
                        CreditsFacade.PostTopic(item.ID, item.Forum);
                }
            }
        }

        public static bool GetModTopicCountByTidList(string moderatorUserName, string tidList)
        {
            var fids = Moderators.GetFidListByModerator(moderatorUserName).SplitAsInt();
            if (fids.Length <= 0 || fids[0] == 0) return false;

            //return !(String.IsNullOrEmpty(fids)) && tidList.Split(',').Length == BBX.Data.Topics.GetModTopicCountByTidList(fids.TrimEnd(','), tidList);
            var list = Topic.FindAllByIDs(tidList);
            var count = list.ToList().Count(e => fids.Contains(e.Fid));
            return list.Count == count;
        }

        //public static List<TopicInfo> GetAttentionTopics(string fidList, Int32 tpp, Int32 pageIndex, string keyword)
        //{
        //    return BBX.Data.Topics.GetAttentionTopics(fidList, tpp, pageIndex, keyword);
        //}

        //public static void UpdateTopicAttentionByTidList(string tidList, Int32 attention)
        //{
        //    BBX.Data.Topics.UpdateTopicAttentionByTidList(tidList, attention);
        //}

        //public static void UpdateTopicAttentionByFidList(string fidList, Int32 datetime)
        //{
        //    BBX.Data.Topics.UpdateTopicAttentionByFidList(fidList, 0, DateTime.Now.AddDays((double)(-(double)datetime)).ToString());
        //}

        /// <summary>标记为已浏览，保存在Cookie中</summary>
        /// <param name="topic"></param>
        public static void MarkOldTopic(Topic topic)
        {
            var ids = ForumUtils.GetCookie("oldtopic").SplitAsInt().ToList();
            if (!ids.Contains(topic.ID) && DateTime.Now.AddMinutes(-600.0) < topic.LastPost)
            {
                ids.Add(topic.ID);
                // 如果超过最大数，干掉最小的那个
                if (ids.Count > 100) ids.RemoveAt(0);
                ForumUtils.WriteCookie("oldtopic", ids.Join());
            }
        }

        //public static string GetTopicCountCondition(out string type, string gettype, Int32 getnewtopic)
        //{
        //    return BBX.Data.Topics.GetTopicCountCondition(out type, gettype, getnewtopic);
        //}

        //public static Int32 GetTopicListCount(string postName, Int32 forumId, string posterList, string keyList, string startDate, string endDate)
        //{
        //    return BBX.Data.Topics.GetTopicListCount(postName, forumId, posterList, keyList, startDate, endDate);
        //}

        //public static Int32 GetHotTopicsCount(Int32 fid, Int32 timeBetween)
        //{
        //    return BBX.Data.Topics.GetHotTopicsCount(fid, timeBetween);
        //}

        //public static DataTable GetTopicList(string postName, Int32 forumId, string posterList, string keyList, string startDate, string endDate, Int32 pageSize, Int32 currentPage)
        //{
        //    return BBX.Data.Topics.GetTopicList(postName, forumId, posterList, keyList, startDate, endDate, pageSize, currentPage);
        //}

        //public static DataTable GetHotTopicsList(Int32 pageSize, Int32 pageIndex, Int32 fid, string showType, Int32 timeBetween)
        //{
        //    return BBX.Data.Topics.GetHotTopicsList(pageSize, pageIndex, fid, showType, timeBetween);
        //}

        //public static void PassAuditNewTopic(string tidList)
        //{
        //    string[] array = tidList.Split(',');
        //    for (Int32 i = 0; i < array.Length; i++)
        //    {
        //        string s = array[i];
        //        TopicInfo topicInfo = Topics.GetTopicInfo(Int32.Parse(s));
        //        CreditsFacade.PostTopic(topicInfo.Posterid, Forums.GetForumInfo(topicInfo.Fid));
        //    }
        //    BBX.Data.Topics.PassAuditNewTopic(TableList.GetPostTableId(), tidList);
        //}

        //public static void UpdateMyTopic()
        //{
        //    BBX.Data.Topics.UpdateMyTopic();
        //}

        //public static Int32 GetTitleDisplayOrder(UserGroup usergroupinfo, Int32 useradminid, IXForum forum, TopicInfo tp, string message, Boolean disablepost)
        //{
        //    if (useradminid == 1 || Moderators.IsModer(useradminid, tp.Posterid, forum.Fid))
        //    {
        //        return tp.Displayorder;
        //    }
        //    if (forum.Modnewtopics == 1 || usergroupinfo.ModNewTopics == 1 || (Scoresets.BetweenTime(GeneralConfigInfo.Current.Postmodperiods) && !disablepost) || ForumUtils.HasAuditWord(tp.Title) || ForumUtils.HasAuditWord(message))
        //    {
        //        return -2;
        //    }
        //    return tp.Displayorder;
        //}

        public static Int32 GetTitleDisplayOrder(UserGroup usergroupinfo, Int32 useradminid, IXForum forum, Topic tp, string message, Boolean disablepost)
        {
            if (useradminid == 1 || Moderators.IsModer(useradminid, tp.PosterID, forum.Fid))
            {
                return tp.DisplayOrder;
            }
            if (forum.Modnewtopics == 1 || usergroupinfo.ModNewTopics == 1 || (Scoresets.BetweenTime(GeneralConfigInfo.Current.Postmodperiods) && !disablepost) || ForumUtils.HasAuditWord(tp.Title) || ForumUtils.HasAuditWord(message))
            {
                return -2;
            }
            return tp.DisplayOrder;
        }

        //public static DataTable GetAuditTopicList(string condition)
        //{
        //    return BBX.Data.Topics.GetAuditTopicList(condition);
        //}

        //public static string GetSearchTopicsCondition(Int32 fid, string keyWord, string displayOrder, string digest, string attachment, string poster, bool lowerUpper, string viewsMin, string viewsMax, string repliesMax, string repliesMin, string rate, string lastPost, DateTime postDateTimeStart, DateTime postDateTimeEnd)
        //{
        //    return BBX.Data.Topics.GetSearchTopicsCondition(fid, keyWord, displayOrder, digest, attachment, poster, lowerUpper, viewsMin, viewsMax, repliesMax, repliesMin, rate, lastPost, postDateTimeStart, postDateTimeEnd);
        //}

        //public static DataTable GetTopicNumber(string tagname, Int32 from, Int32 end, Int32 type)
        //{
        //    return BBX.Data.Topics.GetTopicNumber(tagname, from, end, type);
        //}

        //public static void DeleteRecycleTopic(Int32 recycleDay)
        //{
        //    string text = "";
        //    DataTable tidForModeratorManageLogByPostDateTime = BBX.Data.Topics.GetTidForModeratorManageLogByPostDateTime(DateTime.Now.AddDays((double)(-(double)recycleDay)));
        //    if (tidForModeratorManageLogByPostDateTime.Rows.Count > 0)
        //    {
        //        foreach (DataRow dataRow in tidForModeratorManageLogByPostDateTime.Rows)
        //        {
        //            text = text + dataRow["tid"].ToString() + ",";
        //        }
        //        TopicAdmins.DeleteTopics(text.Trim(','), 0, false);
        //    }
        //}

        //public static DataTable GetTopicsByCondition(string condition)
        //{
        //    return BBX.Data.Topics.GetTopicsByCondition(condition);
        //}

        //public static string GetTopicFilterCondition(string filter)
        //{
        //    return DatabaseProvider.GetInstance().GetTopicFilterCondition(filter);
        //}

        //private static void LoadTopicForumName(TopicInfo topicInfo)
        //{
        //	var forumInfo = Forums.GetForumInfo(topicInfo.Fid);
        //	topicInfo.Forumname = ((forumInfo == null) ? "" : forumInfo.Name);
        //}

        //private static void LoadTopicFolder(Int32 autocloseTime, Int32 newMinutes, Int32 hotReplyNumber, TopicInfo topicInfo)
        //{
        //    if (topicInfo.Closed == 0)
        //    {
        //        string text = ForumUtils.GetCookie("oldtopic") + "D";
        //        if (newMinutes > 0 && text.IndexOf("D" + topicInfo.Tid.ToString() + "D") == -1 && DateTime.Now.AddMinutes((double)(-1 * newMinutes)) < TypeConverter.StrToDateTime(topicInfo.Lastpost))
        //        {
        //            topicInfo.Folder = "new";
        //        }
        //        else
        //        {
        //            topicInfo.Folder = "old";
        //        }
        //        if (hotReplyNumber > 0 && topicInfo.Replies >= hotReplyNumber)
        //        {
        //            topicInfo.Folder += "hot";
        //        }
        //        if (autocloseTime > 0 && Utils.StrDateDiffHours(topicInfo.Postdatetime, autocloseTime * 24) > 0)
        //        {
        //            topicInfo.Closed = 1;
        //            topicInfo.Folder = "closed";
        //            return;
        //        }
        //    }
        //    else
        //    {
        //        topicInfo.Folder = "closed";
        //        if (topicInfo.Closed > 1)
        //        {
        //            Int32 tid = topicInfo.Tid;
        //            topicInfo.Tid = topicInfo.Closed;
        //            topicInfo.Closed = tid;
        //            topicInfo.Folder = "move";
        //        }
        //    }
        //}

        private static void LoadTopicFolder(Int32 autocloseTime, Int32 newMinutes, Int32 hotReplyNumber, Topic tp)
        {
            if (tp.Closed == 0)
            {
                string text = ForumUtils.GetCookie("oldtopic") + "D";
                if (newMinutes > 0 && text.IndexOf("D" + tp.ID + "D") == -1 && DateTime.Now < tp.LastPost.AddMinutes(newMinutes))
                {
                    tp.Folder = "new";
                }
                else
                {
                    tp.Folder = "old";
                }
                if (hotReplyNumber > 0 && tp.Replies >= hotReplyNumber)
                {
                    tp.Folder += "hot";
                }
                if (autocloseTime > 0 && tp.PostDateTime.AddHours(autocloseTime * 24) < DateTime.Now)
                {
                    tp.Closed = 1;
                    tp.Folder = "closed";
                    return;
                }
            }
            else
            {
                tp.Folder = "closed";
                if (tp.Closed > 1)
                {
                    Int32 tid = tp.ID;
                    // 下面这一行没有搞明白，主键被修改是非常危险的事情
                    //tp.Tid = tp.Closed;
                    tp.Closed = tid;
                    tp.Folder = "move";
                }
            }
        }

        //private static void LoadTopicHighlightTitle(Topic topicInfo)
        //{
        //	if (topicInfo.Highlight != "")
        //	{
        //		topicInfo.Title = "<span style=\"" + topicInfo.Highlight + "\">" + topicInfo.Title + "</span>";
        //	}
        //}

        //private static void LoadTopicType(Int32 topicTypePrefix, IDictionary<Int32, string> topicTypeList, Topic topicinfo)
        //{
        //	if (topicTypePrefix > 0 && topicinfo.TypeID > 0)
        //	{
        //		string text = "";
        //		topicTypeList.TryGetValue(topicinfo.TypeID, out text);
        //		topicinfo.Topictypename = text.Trim();
        //	}
        //}

        private static void LoadTopicListExtraInfo(Int32 autoCloseTime, Int32 topicTypePrefix, Int32 newMinutes, Int32 hotReplyNumber, List<Topic> list)
        {
            //var topicTypeArray = TopicType.GetTopicTypeArray();
            //var sb = new StringBuilder();
            foreach (var tp in list)
            {
                if (tp.Closed == 0 && autoCloseTime > 0 && tp.PostDateTime.AddHours(autoCloseTime * 24) < DateTime.Now)
                {
                    //sb.Append(tp.ID);
                    //sb.Append(",");
                    tp.Closed = 1;
                    tp.Update();
                }
                LoadTopicFolder(autoCloseTime, newMinutes, hotReplyNumber, tp);
                //Topics.LoadTopicHighlightTitle(current);
                //Topics.LoadTopicType(topicTypePrefix, topicTypeArray, current);
            }
            //if (sb.Length > 0)
            //{
            //    TopicAdmins.SetClose(sb.ToString().TrimEnd(','), 1);
            //}
        }

        private static void LoadTopTopicListExtraInfo(Int32 topicTypePrefix, Int32 newMinutes, Int32 hotReplyNumber, List<Topic> list)
        {
            //var topicTypeArray = TopicType.GetTopicTypeArray();
            //var stringBuilder = new StringBuilder();
            foreach (var tp in list)
            {
                var forumInfo = Forums.GetForumInfo(tp.Fid);
                if (tp.Closed == 0 && forumInfo.AutoClose > 0 && tp.PostDateTime.AddHours(forumInfo.AutoClose * 24) < DateTime.Now)
                {
                    //stringBuilder.Append(tp.ID.ToString());
                    //stringBuilder.Append(",");
                    tp.Closed = 1;
                    tp.Update();
                }
                LoadTopicFolder(forumInfo.AutoClose, newMinutes, hotReplyNumber, tp);
                //LoadTopicHighlightTitle(tp);
                //LoadTopicType(topicTypePrefix, topicTypeArray, tp);
            }
            //if (stringBuilder.Length > 0)
            //{
            //    TopicAdmins.SetClose(stringBuilder.ToString().TrimEnd(','), 1);
            //}
        }

        private static void LoadTopTopicListExtraInfo(Int32 topicTypePrefix, List<Topic> list)
        {
            Topics.LoadTopTopicListExtraInfo(topicTypePrefix, 600, 0, list);
        }

        //private static void LoadTopicListExtraInfo(Int32 autoCloseTime, Int32 topicTypePrefix, List<Topic> list)
        //{
        //    LoadTopicListExtraInfo(autoCloseTime, topicTypePrefix, 600, 0, list);
        //}

        //private static Int32 CompareDisplayOrder(TopicInfo x, TopicInfo y)
        //{
        //	return new CaseInsensitiveComparer().Compare(x.Displayorder, y.Displayorder);
        //}
    }
}