using System.Data;
using BBX.Common;
using BBX.Entity;

namespace BBX.Forum
{
    public class AdminTopics : TopicAdmins
    {
        //public static bool UpdateTopicAllInfo(TopicInfo topicinfo)
        //{
        //	bool result;
        //	try
        //	{
        //		Topics.UpdateTopic(topicinfo);
        //		result = true;
        //	}
        //	catch
        //	{
        //		result = false;
        //	}
        //	return result;
        //}

        //public static bool DeleteTopicByTid(int tid)
        //{
        //	return BBX.Data.Posts.DeleteTopicByTid(tid, TableList.CurrentTableName);
        //}

        //public static bool SetTypeid(string topiclist, int value)
        //{
        //    return BBX.Data.Topics.SetTypeid(topiclist, value);
        //}

        //public static DataSet AdminGetPostList(int tid, int pagesize, int pageindex)
        //{
        //    DataSet dataSet = BBX.Data.Posts.GetPosts(tid, pagesize, pageindex, TableList.GetPostTableId(tid));
        //    if (dataSet == null)
        //    {
        //        dataSet = new DataSet();
        //        dataSet.Tables.Add("post");
        //        dataSet.Tables.Add();
        //        return dataSet;
        //    }
        //    dataSet.Tables[0].TableName = "post";
        //    foreach (DataRow dataRow in dataSet.Tables[0].Rows)
        //    {
        //        if (dataRow["attachment"].ToString().Equals("1"))
        //        {
        //            dataRow["attachment"] = Attachment.FindCountByPid(dataRow["pid"].ToInt(0));
        //        }
        //    }
        //    return dataSet;
        //}

        public static void BatchMoveTopics(string tidList, int targetForumId, int adminUid, string adminUserName, int adminUserGroupId, string adminUserGroupTitle, string adminIp)
        {
            //foreach (DataRow dataRow in BBX.Data.Topics.GetTopicFidByTid(tidList).Rows)
            //{
            //    string text = "0";
            //    foreach (DataRow dataRow2 in BBX.Data.Topics.GetTopicTidByFid(tidList, int.Parse(dataRow["fid"].ToString())).Rows)
            //    {
            //        text = text + "," + dataRow2["tid"].ToString();
            //    }
            //    TopicAdmins.MoveTopics(text, targetForumId, (int)Convert.ToInt16(dataRow["fid"].ToString()), 0);
            //}
            var list = Topic.FindAllByIDs(tidList);
            foreach (var tp in list)
            {
                //tp.Fid = targetForumId;
                //tp.TypeID = 0;
                //tp.Save();
                TopicAdmins.MoveTopics(tp.ID + "", targetForumId, tp.Fid, 0);
            }
            AdminVisitLog.InsertLog(adminUid, adminUserName, adminUserGroupId, adminUserGroupTitle, adminIp, "批量移动主题", "主题ID:" + tidList + " <br />目标论坛fid:" + targetForumId);
        }

        public static void BatchDeleteTopics(string tidList, bool isChagePostNumAndCredits, int adminUid, string adminUserName, int adminUserGroupId, string adminUserGroupTitle, string adminIp)
        {
            TopicAdmins.DeleteTopics(tidList, isChagePostNumAndCredits ? 1 : 0, false);
            Attachments.UpdateTopicAttachment(tidList);
            AdminVisitLog.InsertLog(adminUid, adminUserName, adminUserGroupId, adminUserGroupTitle, adminIp, "批量删除主题", "主题ID:" + tidList);
        }

        public static void BatchChangeTopicsDisplayOrderLevel(string tidList, int displayOrderLevel, int adminUid, string adminUserName, int adminUserGroupId, string adminUserGroupTitle, string adminIp)
        {
            //BBX.Data.Topics.SetDisplayorder(tidList, displayOrderLevel);
            var list = Topic.FindAllByIDs(tidList);
            list.ForEach(e => e.DisplayOrder = displayOrderLevel);
            list.Save();
            AdminVisitLog.InsertLog(adminUid, adminUserName, adminUserGroupId, adminUserGroupTitle, adminIp, "批量置顶主题", "主题ID:" + tidList + "<br /> 置顶级为:" + displayOrderLevel);
        }

        public static void BatchChangeTopicsDigest(string tidList, int digestLevel, int adminUid, string adminUserName, int adminUserGroupId, string adminUserGroupTitle, string adminIp)
        {
            TopicAdmins.SetDigest(tidList, digestLevel);
            AdminVisitLog.InsertLog(adminUid, adminUserName, adminUserGroupId, adminUserGroupTitle, adminIp, "批量加精主题", "主题ID:" + tidList + "<br /> 加精级为:" + digestLevel);
        }

        public static void BatchDeleteTopicAttachs(string tidList, int adminUid, string adminUserName, int adminUserGroupId, string adminUserGroupTitle, string adminIp)
        {
            //Attachments.DeleteAttachmentByTid(tidList);
            Attachment.FindAllByTids(tidList).Delete();
            AdminVisitLog.InsertLog(adminUid, adminUserName, adminUserGroupId, adminUserGroupTitle, adminIp, "删除主题中的附件", "主题ID:" + tidList);
        }
    }
}