using System;
using BBX.Common;
using BBX.Entity;

namespace BBX.Forum
{
    public class TopicAdmins
    {
        public static int SetTopTopicList(int fid, string topiclist, short intValue)
        {
            var list = Topic.FindAllByIDs(topiclist);
            list.ForEach(tp => tp.DisplayOrder = intValue);
            return list.Save();
            //if (list.Count > 0 && ResetTopTopicList() == 1) return 1;

            //var file = BaseConfigs.GetForumPath.CombinePath("cache/topic", fid + ".xml");
            //if (File.Exists(file)) File.Delete(file);
            //return -1;
        }

        public static int SetDigest(string topiclist, int val)
        {
            //string us = TopicAdmins.GetUserListWithDigestTopiclist(topiclist, (val > 0) ? 1 : 0);
            //int num = TopicAdmins.SetTopicStatus(topiclist, "digest", val);
            //if (num > 0)
            var list = Topic.FindAllByIDs(topiclist);
            if (list.Count > 0)
            {
                list.ToList().ForEach(e => e.Digest = val);
                list.Save();

                // 过滤列表
                if (val > 0)
                    list = list.FindAll(e => e.Digest <= 0);
                else
                    list = list.FindAll(e => e.Digest > 0);
                foreach (var item in list)
                {
                    var user = item.PostUser;
                    if (user == null) continue;

                    user.DigestPosts = Topic.GetDigestCount(user.ID);
                    user.Save();

                    if (val > 0)
                        CreditsFacade.SetDigest(user.ID);
                    else
                        CreditsFacade.UnDigest(user.ID);
                }

                //if (Utils.IsNumericList(us))
                //{
                //    // 更新用户有多少个主题被评分
                //    //BBX.Data.Users.UpdateUserDigest(us);
                //    var users = User.FindAllByIdList(us);
                //    foreach (var item in users)
                //    {
                //        item.DigestPosts = Topic.GetDigestCount(item.ID);
                //        item.Save();
                //    }
                //}
                //if (!string.IsNullOrEmpty(us) && Utils.IsNumericList(us))
                //{
                //    string[] array = Utils.SplitString(us, ",");
                //    for (int i = 0; i < array.Length; i++)
                //    {
                //        string str = array[i];
                //        if (intValue > 0)
                //        {
                //            CreditsFacade.SetDigest(str.ToInt());
                //        }
                //        else
                //        {
                //            CreditsFacade.UnDigest(str.ToInt());
                //        }
                //    }
                //}
            }
            return list.Count;
        }

        public static int DeleteTopics(string topicList, int subTractCredits, bool reserveAttach)
        {
            if (!Utils.IsNumericList(topicList)) return -1;

            //DataTable topicList2 = Topics.GetTopicList(topicList);
            //var topicList2 = Topic.FindAllByTidsAndDisplayOrder(topicList, -10).ToDataTable(false);
            //if (topicList2 == null)
            //{
            //	return -1;
            //}
            //foreach (DataRow dataRow in topicList2.Rows)
            //{
            //	if (TypeConverter.ObjectToInt(dataRow["digest"]) > 0)
            //	{
            //		CreditsFacade.UnDigest(TypeConverter.ObjectToInt(dataRow["posterid"]));
            //	}
            //}
            var list = Topic.FindAllByIDs(topicList);
            if (list.Count == 0) return -1;

            foreach (var item in list)
            {
                if (item.Digest > 0) CreditsFacade.UnDigest(item.PosterID);
            }

            //var postList = Posts.GetPostList(topicList);
            var postList = Post.FindAllByTid(topicList.SplitAsInt(","));
            if (postList != null && postList.Count != 0)
            {
                //int fid = 0;
                //IXForum forumInfo = null;
                foreach (var pt in postList)
                {
                    //if (fid != pt.Fid)
                    //{
                    //	fid = pt.Fid;
                    //	forumInfo = Forums.GetForumInfo(fid);
                    //}
                    CreditsFacade.DeletePost(pt, reserveAttach);
                }
            }
            //int num2 = 0;
            //string[] postTableIdArray = Posts.GetPostTableIdArray(topicList);
            //for (int i = 0; i < postTableIdArray.Length; i++)
            //{
            //	string postTableId = postTableIdArray[i];
            //	//num2 = BBX.Data.TopicAdmins.DeleteTopicByTidList(topicList, postTableId);
            //	num2 = DatabaseProvider.GetInstance().DeleteTopicByTidList(topicList, postTableId, true);
            //}
            if (list.Delete() > 0 && !reserveAttach)
            {
                //Attachments.DeleteAttachmentByTid(topicList);
                Attachment.FindAllByTids(topicList).Delete();
            }
            return list.Count;
        }

        public static int DeleteTopicsWithoutChangingCredits(string topiclist, bool reserveAttach)
        {
            if (!Utils.IsNumericList(topiclist))
            {
                return -1;
            }
            //int num = -1;
            //string[] postTableIdArray = Posts.GetPostTableIdArray(topiclist);
            //for (int i = 0; i < postTableIdArray.Length; i++)
            //{
            //	string postTableId = postTableIdArray[i];
            //	//num = BBX.Data.TopicAdmins.DeleteTopicByTidList(topiclist, postTableId);
            //	num = DatabaseProvider.GetInstance().DeleteTopicByTidList(topiclist, postTableId, true);
            //}
            var list = Topic.FindAllByIDs(topiclist);
            if (list.Delete() > 0 && !reserveAttach)
            {
                //Attachments.DeleteAttachmentByTid(topiclist);
                Attachment.FindAllByTids(topiclist).Delete();
            }
            return list.Count;
        }

        public static int DeleteTopics(string topiclist, bool reserveAttach)
        {
            return DeleteTopics(topiclist, 1, reserveAttach);
        }

        public static int DeleteTopics(string topiclist, Boolean toDustbin, bool reserveAttach)
        {
            if (toDustbin)
            {
                //return TopicAdmins.SetTopicStatus(topiclist, "displayorder", -1);
                var list = Topic.FindAllByIDs(topiclist);
                list.ForEach(e => e.Deleted = true);
                return list.Save();
            }
            else
            {
                return DeleteTopics(topiclist, reserveAttach);
            }
        }

        public static int MoveTopics(string topiclist, int fid, int oldfid, bool savelink, int topicType)
        {
            if (!Utils.IsNumericList(topiclist))
            {
                return -1;
            }
            string text = "";
            //DataTable topicList = Topics.GetTopicList(topiclist);
            //foreach (DataRow dataRow in topicList.Rows)
            //{
            //	if (TypeConverter.ObjectToInt(dataRow["closed"]) <= 1 || TypeConverter.ObjectToInt(dataRow["fid"]) != oldfid)
            //	{
            //		text = text + dataRow["tid"].ToString() + ",";
            //	}
            //}
            var topicList = Topic.FindAllByTidsAndDisplayOrder(topiclist, -10);
            foreach (var tp in topicList)
            {
                if (tp.Closed <= 1 || tp.Fid != oldfid)
                {
                    text = text + tp.Fid + ",";
                }
            }
            text = text.TrimEnd(',');
            if (string.IsNullOrEmpty(text)) return -1;

            //BBX.Data.TopicAdmins.DeleteClosedTopics(fid, text);
            Topic.DeleteClosedTopics(fid, text);
            TopicAdmins.MoveTopics(text, fid, oldfid, topicType);
            if (savelink)
            {
                //if (BBX.Data.TopicAdmins.CopyTopicLink(oldfid, text) <= 0)
                //if (DatabaseProvider.GetInstance().CopyTopicLink(oldfid, text) <= 0)
                //{
                //    return -2;
                //}
                // 复制帖子链接
                foreach (var tp in topicList)
                {
                    tp.Fid = oldfid;
                    tp.Closed = tp.ID;
                    tp.ID = 0;
                }
                topicList.Insert();
                //AdminForumStats.ReSetFourmTopicAPost(oldfid);
                //XForum.SetRealCurrentTopics(oldfid);
                var f = XForum.FindByID(oldfid);
                f.ResetLastPost();
            }
            return 1;
        }

        public static int MoveTopics(string topiclist, int fid, int oldfid, int topicType)
        {
            if (!Utils.IsNumericList(topiclist)) return -1;

            //string[] array = topiclist.Split(',');
            //for (int i = 0; i < array.Length; i++)
            //{
            //    string str = array[i];
            //    DatabaseProvider.GetInstance().UpdatePost(topiclist, fid, TableList.GetPostTableName(str.ToInt()));
            //}
            var ps = Post.FindAllByTid(topiclist.SplitAsInt(","));
            ps.ForEach(p => p.Fid = fid);
            ps.Save();
            //int num = BBX.Data.Topics.UpdateTopic(topiclist, fid, topicType);
            var list = Topic.FindAllByIDs(topiclist);
            list.ForEach(tp => { tp.Fid = fid; tp.TypeID = topicType; });
            var num = list.Save();
            if (num > 0)
            {
                //AdminForumStats.ReSetFourmTopicAPost(fid);
                //AdminForumStats.ReSetFourmTopicAPost(oldfid);
                //XForum.SetRealCurrentTopics(fid);
                //XForum.SetRealCurrentTopics(oldfid);
                XForum.FindByID(fid).ResetLastPost();
                XForum.FindByID(oldfid).ResetLastPost();
            }
            //TopicAdmins.ResetTopTopicList();
            return num;
        }

        public static int CopyTopics(string topiclist, int fid)
        {
            if (!Utils.IsNumericList(topiclist)) return -1;

            int num = 0;
            string[] array = topiclist.Split(',');
            for (int i = 0; i < array.Length; i++)
            {
                string str = array[i];
                var tp_ = Topic.FindByID(str.ToInt());
                if (tp_ != null)
                {
                    var tp = new Topic();
                    tp.CopyFrom(tp_);
                    tp.Fid = fid;
                    tp.ReadPerm = 0;
                    tp.Price = 0;
                    //tp.Postdatetime = Utils.GetDateTime();
                    //tp.Lastpost = Utils.GetDateTime();
                    //tp.Lastposter = Utils.GetDateTime();
                    tp.Views = 0;
                    tp.Replies = 0;
                    tp.DisplayOrder = 0;
                    tp.Highlight = "";
                    tp.Digest = 0;
                    tp.Rate = 0;
                    tp.Hide = 0;
                    tp.Special = 0;
                    tp.Attachment = 0;
                    tp.Moderated = 0;
                    tp.Closed = 0;

                    var pi_ = Post.FindByTid(tp.ID);
                    var pi = new Post();
                    pi.CopyFrom(pi_);
                    pi.Fid = tp.Fid;
                    //pi.Tid = num2;
                    pi.ParentID = 0;
                    pi.Layer = 0;
                    //pi.Postdatetime = Utils.GetDateTime();
                    pi.Invisible = 0;
                    pi.Attachment = 0;
                    pi.Rate = 0;
                    pi.RateTimes = 0;
                    pi.Message = UBB.ClearAttachUBB(pi.Message);
                    pi.Title = tp.Title;

                    tp.Create(pi);
                }
            }
            return num;
        }

        public static int RepairTopicList(string topicList)
        {
            if (!Utils.IsNumericList(topicList)) return 0;

            int num = 0;
            //string[] postTableIdArray = Posts.GetPostTableIdArray(topicList);
            string[] array = topicList.Split(',');
            for (int i = 0; i < array.Length; i++)
            {
                //int num2 = TopicAdmins.RepairTopics(array[i], BaseConfigs.GetTablePrefix + "posts" + postTableIdArray[i].ToInt());
                var tp = Topic.FindByID(array[i].ToInt());
                int num2 = tp.Repair();
                if (num2 > 0)
                {
                    num = num2 + num;
                    Attachments.UpdateTopicAttachment(topicList);
                }
            }
            return num;
        }

        public static int RatePosts(int tid, string postidlist, string score, string extcredits, int userid, string username, string reason)
        {
            if (!Utils.IsNumericList(postidlist)) return 0;

            float[] array = new float[8];
            float[] array2 = array;
            string[] scores = Utils.SplitString(score, ",");
            string[] extcreditses = Utils.SplitString(extcredits, ",");
            //string postTableId = TableList.GetPostTableId(tid);
            for (int i = 0; i < extcreditses.Length; i++)
            {
                int extid = extcreditses[i].ToInt(-1);
                if (extid > 0 && extid < array2.Length)
                {
                    array2[extid - 1] = (float)scores[i].ToInt();
                    //string[] array5 = Utils.SplitString(postidlist, ",");
                    //for (int j = 0; j < array5.Length; j++)
                    //{
                    //	string text = array5[j];
                    //	if (text.Trim() != string.Empty)
                    //	{
                    //		TopicAdmins.SetPostRate(postTableId, text.ToInt(), extcreditses[i].ToInt(), TypeConverter.StrToFloat(scores[i]), true);
                    //	}
                    //}
                    //AdminRateLogs.InsertLog(postidlist, userid, username, extid, TypeConverter.StrToFloat(scores[i]), reason);
                    var arr = postidlist.SplitAsInt();
                    for (int k = 0; k < arr.Length; k++)
                    {
                        SetPostRate(arr[i], extcreditses[i].ToInt(), scores[i].ToDouble(), true);
                        var log = RateLog.Create(arr[i], userid, username, extid, scores[i].ToInt(), reason);
                    }
                }
            }
            //return CreditsFacade.UpdateUserExtCredits(TopicAdmins.GetUserListWithPostlist(tid, postidlist), array2);
            var ps = Post.FindAllByIDs(postidlist);
            var uids = ps.Join(Post._.PosterID, ",");
            return CreditsFacade.UpdateUserExtCredits(uids, array2);
        }

        public static void SetPostRate(int postid, int extid, Double score, bool israte)
        {
            if (score == 0f) return;

            var rate = israte ? score : (-1f * score);
            //BBX.Data.Posts.UpdatePostRate(postid, rate, posttableid);
            //PostInfo postInfo = BBX.Data.Posts.GetPostInfo(posttableid, postid);
            var pi = Post.FindByID(postid);
            if (pi != null)
            {
                pi.Rate += (Int32)rate;
                pi.Save();

                if (pi.Layer == 0)
                {
                    //BBX.Data.TopicAdmins.SetTopicStatus(pi.Tid.ToString(), "rate", pi.Rate.ToString());
                    pi.Topic.Rate = pi.Rate;
                    pi.Topic.Save();
                }
            }
        }

        public static string CheckRateState(string postidlist, int userid)
        {
            if (!Utils.IsNumericList(postidlist))
            {
                return "";
            }
            string text = "";
            string[] array = Utils.SplitString(postidlist, ",");
            for (int i = 0; i < array.Length; i++)
            {
                string pid = array[i];
                //string text2 = BBX.Data.TopicAdmins.CheckRateState(userid, pid);
                var rate = RateLog.FindByPidAndUid(pid.ToInt(), userid);
                var text2 = rate != null ? rate.Pid + "" : "";
                if (!text2.IsNullOrEmpty())
                {
                    if (!text.IsNullOrEmpty())
                    {
                        text += ",";
                    }
                    text += text2;
                }
            }
            return text;
        }

        public static string GetTopicListModeratorLog(int tid)
        {
            //return BBX.Data.TopicAdmins.GetTopicListModeratorLog(tid);

            string result = "";
            var log = ModeratorManageLog.FindLastByTid(tid);
            if (log != null)
            {
                result = "本主题由 " + log.GroupTitle + " " + log.ModeratorName + " 于 " + log.PostDateTime.ToFullString() + " 执行 " + log.Actions + " 操作";
                //log.Close();
            }
            return result;
        }

        public static void CancelRatePosts(string ratelogidlist, int tid, string pid, int userid, string username, int groupid, string grouptitle, int forumid, string forumname, string reason)
        {
            if (!Utils.IsNumeric(pid)) return;

            //var postid = pid.ToInt();

            //int posterid = Posts.GetPostInfo(tid, postid).Posterid;
            //if (posterid <= 0) return;
            var post = Post.FindByID(pid.ToInt());
            if (post == null) return;

            //string postTableId = TableList.GetPostTableId(tid);
            //DataTable dataTable = AdminRateLogs.LogList(ratelogidlist.Split(',').Length, 1, "id IN(" + ratelogidlist + ")");
            //foreach (DataRow dataRow in dataTable.Rows)
            //{
            //	TopicAdmins.SetPostRate(postTableId, pid.ToInt(), TypeConverter.ObjectToInt(dataRow["extcredits"]), (float)TypeConverter.ObjectToInt(dataRow["score"]), false);
            //	User.UpdateUserExtCredits(posterid, TypeConverter.ObjectToInt(dataRow["extcredits"]), -1f * TypeConverter.ObjectToFloat(dataRow["score"]));
            //}
            //AdminRateLogs.DeleteLog("[id] IN(" + ratelogidlist + ")");
            var list = RateLog.FindAllByIDs(ratelogidlist);
            foreach (var item in list)
            {
                SetPostRate(post.ID, item.ExtCredits, (float)item.Score, false);
                User.UpdateUserExtCredits(post.PosterID, item.ExtCredits, -1f * item.Score);
            }
            list.Delete();
            //if (AdminRateLogs.LogList(1, 1, "pid = " + pid).Rows.Count == 0)
            if (RateLog.SearchByPid(post.ID, 0, 1) == null)
            {
                //BBX.Data.Posts.CancelPostRate(pid, postTableId);
                post.Rate = 0;
                post.RateTimes = 0;
                post.Update();
            }
            var topicInfo = Topic.FindByID(tid);
            ModeratorManageLog.Add(userid, username, groupid, grouptitle, forumid, forumname, tid, (topicInfo == null) ? "暂无标题" : topicInfo.Title, "撤消评分", reason);
        }

        public static void BumpTopics(string topiclist, int bumptype)
        {
            if (!Utils.IsNumericList(topiclist)) return;

            var list = Topic.FindAllByIDs(topiclist);
            if (bumptype == 1)
            {
                //string[] array = topiclist.Split(',');
                //for (int i = 0; i < array.Length; i++)
                //{
                //    string tidList = array[i];
                //    BBX.Data.TopicAdmins.SetTopicsBump(tidList, BBX.Data.TopicAdmins.GetPostId());
                //}
                //return;
                var fid = list[0].Fid;
                var max = Topic.FindMax(Topic._.LastPostID, Topic._.Fid == fid);
                foreach (var item in list)
                {
                    if (item.Fid != fid) max = Topic.FindMax(Topic._.LastPostID, Topic._.Fid == fid);

                    item.LastPostID = max + 1;
                    item.Save();
                }
            }
            else
            {
                //BBX.Data.TopicAdmins.SetTopicsBump(topiclist, 0);
                // 找到本版面最小的LastPostID，强制把当前帖子改为最小减一
                var fid = list[0].Fid;
                var min = Topic.FindMin(Topic._.LastPostID, Topic._.Fid == fid);
                foreach (var item in list)
                {
                    if (item.Fid != fid) min = Topic.FindMin(Topic._.LastPostID, Topic._.Fid == fid);

                    item.LastPostID = min - 1;
                    item.Save();
                }
            }
        }
    }
}