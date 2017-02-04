using System;
using System.Linq;
using BBX.Common;
using BBX.Entity;

namespace BBX.Forum
{
    public class AdminForumStats
    {
        //public static int GetUserCount()
        //{
        //    //return Users.GetUserCount("");
        //    return User.Meta.Count;
        //}

        //public static int GetPostsCount()
        //{
        //    int num = 0;
        //    foreach (var tb in TableList.GetAllPostTable())
        //    {
        //        num += TableList.GetPostsCount(tb.ID);
        //    }
        //    return num;
        //}

        //public static int GetPostsCountByFid(int fid, out int todaypostcount)
        //{
        //    int count = 0;
        //    todaypostcount = 0;
        //    int maxtid = 0;
        //    int mintid = 0;
        //    //IDataReader maxAndMinTid = BBX.Data.Forums.GetMaxAndMinTid(fid);
        //    //if (maxAndMinTid != null)
        //    //{
        //    //    if (maxAndMinTid.Read())
        //    //    {
        //    //        maxtid = maxAndMinTid["maxtid"].ToInt(0);
        //    //        mintid = maxAndMinTid["mintid"].ToInt(0);
        //    //    }
        //    //    maxAndMinTid.Close();
        //    //}
        //    var arr = Post.GetMaxAndMinTidByFid(fid);
        //    maxtid = arr[0];
        //    mintid = arr[1];
        //    if (mintid + maxtid == 0)
        //    {
        //        count = Post.GetPostCount(fid);
        //        todaypostcount = Post.GetTodayPostCount(fid);
        //    }
        //    else
        //    {
        //        //string[] postTableIdArray = Posts.GetPostTableIdArray(mintid, maxtid);
        //        //string[] array = postTableIdArray;
        //        //for (int i = 0; i < array.Length; i++)
        //        //{
        //        //    string str = array[i];
        //        //    count += BBX.Data.Posts.GetPostCount(fid, BBX.Data.Posts.GetPostTableName(str.ToInt()));
        //        //    todaypostcount += BBX.Data.Posts.GetTodayPostCount(fid, BBX.Data.Posts.GetPostTableName(str.ToInt()));
        //        //}

        //        var pts = TableList.GetAllPostTableByMinAndMax(mintid, maxtid);
        //        foreach (var item in pts)
        //        {
        //            count += Post.GetPostCount(fid, item.ID);
        //            todaypostcount += Post.GetTodayPostCount(fid, item.ID);
        //        }
        //    }
        //    return count;
        //}

        //public static int GetPostsCountByFid(int fid)
        //{
        //	int num = 0;
        //	return GetPostsCountByFid(fid, out num);
        //}

        public static int GetPostsCountByUid(int uid, out int todaypostcount)
        {
            int count = 0;
            todaypostcount = 0;
            int maxtid = 0;
            int mintid = 0;
            //IDataReader maxAndMinTidByUid = BBX.Data.Topics.GetMaxAndMinTidByUid(uid);
            //if (maxAndMinTidByUid != null)
            //{
            //    if (maxAndMinTidByUid.Read())
            //    {
            //        maxtid = maxAndMinTidByUid["maxtid"].ToInt(0);
            //        mintid = maxAndMinTidByUid["mintid"].ToInt(0);
            //    }
            //    maxAndMinTidByUid.Close();
            //}
            var arr = Post.GetMaxAndMinTidByUid(uid);
            maxtid = arr[0];
            mintid = arr[1];
            if (mintid + maxtid == 0)
            {
                count = Post.GetPostCountByUid(uid);
                todaypostcount = Post.GetTodayPostCountByUid(uid);
            }
            else
            {
                //string[] postTableIdArray = Posts.GetPostTableIdArray(mintid, maxtid);
                //string[] array = postTableIdArray;
                //for (int i = 0; i < array.Length; i++)
                //{
                //    string str = array[i];
                //    count += BBX.Data.Posts.GetPostCountByUid(uid, BBX.Data.Posts.GetPostTableName(str.ToInt()));
                //    todaypostcount += BBX.Data.Posts.GetTodayPostCountByUid(uid, BBX.Data.Posts.GetPostTableName(str.ToInt()));
                //}

                //var pts = TableList.GetAllPostTableByMinAndMax(mintid, maxtid);
                //foreach (var item in pts)
                {
                    count += Post.GetPostCountByUid(uid);
                    todaypostcount += Post.GetTodayPostCountByUid(uid);
                }
            }
            return count;
        }

        public static int GetPostsCountByUid(int uid)
        {
            int num = 0;
            return GetPostsCountByUid(uid, out num);
        }

        //public static int GetTopicsCount()
        //{
        //    return BBX.Data.Topics.GetTopicCount();
        //}

        //public static bool GetLastUserInfo(out string lastuserid, out string lastusername)
        //{
        //    return BBX.Data.Users.GetLastUserInfo(out lastuserid, out lastusername);
        //}

        public static void ReSetStatistic()
        {
            //int userCount = GetUserCount();
            //int topicsCount = GetTopicsCount();
            //int postsCount = GetPostsCount();
            //var userCount = User.Meta.Count;
            //var topicsCount = Topic.Meta.Count;
            //var postsCount = Post.Meta.Count;
            //string lastuserid;
            //string lastusername;
            //if (!GetLastUserInfo(out lastuserid, out lastusername))
            //{
            //    lastuserid = "";
            //    lastusername = "";
            //}
            var user = User.FindLast();
            //BBX.Data.Posts.ReSetStatistic(userCount, topicsCount, postsCount, user.ID, user.Name);
            var st = Statistic.Current;
            st.TotalUsers = User.Meta.Count;
            st.TotalTopic = Topic.Meta.Count;
            st.TotalPost = Post.Meta.Count;
            st.LastUserID = user.ID;
            st.LastUserName = user.Name;
            st.Save();
        }

        public static void ReSetUserDigestPosts()
        {
            //BBX.Data.Users.ResetUserDigestPosts();
            throw new NotSupportedException("ReSetUserDigestPosts");
        }

        public static void ReSetUserDigestPosts(int start_uid, int end_uid)
        {
        }

        public static void ReSetUserPosts(int statcount, ref int lasttableid)
        {
            //if (statcount < 1 || lasttableid > TypeConverter.StrToInt(TableList.GetPostTableId()))
            //{
            //    lasttableid = -1;
            //    return;
            //}
            for (int i = 0; i < statcount; i++)
            {
                //BBX.Data.Users.UpdateAllUserPostCount(lasttableid);
                //lasttableid++;
                throw new NotSupportedException();
            }
        }

        public static void UpdateMyPost(int statcount, ref int lasttableid)
        {
            //if (statcount < 1 || lasttableid > TypeConverter.StrToInt(TableList.GetPostTableId()))
            //{
            //    lasttableid = -1;
            //    return;
            //}
            for (int i = 0; i < statcount; i++)
            {
                //BBX.Data.Users.UpdateMyPost(lasttableid);
                //lasttableid++;
                throw new NotImplementedException();
            }
        }

        //public static void UpdatePostSP(int statcount, ref int lasttableid)
        //{
        //    if (statcount < 1 || lasttableid > TypeConverter.StrToInt(TableList.GetPostTableId()))
        //    {
        //        lasttableid = -1;
        //        return;
        //    }
        //    for (int i = 0; i < statcount; i++)
        //    {
        //        BBX.Data.Databases.UpdatePostSP(lasttableid);
        //        lasttableid++;
        //    }
        //}

        public static void ReSetUserPosts(int start_uid, int end_uid)
        {
            //IDataReader users = BBX.Data.Users.GetUsers(start_uid, end_uid);
            var users = User.GetUsers(start_uid, end_uid);
            if (users.Count > 0)
            {
                foreach (var user in users)
                {
                    //int num = users["uid"].ToInt(-1);
                    int postsCountByUid = GetPostsCountByUid(user.ID);
                    //BBX.Data.Users.UpdateUserPostCount(postsCountByUid, user.ID);
                    user.Posts = postsCountByUid;
                    user.Save();
                }
                //users.Close();
            }
        }

        public static void ReSetTopicPosts(int statcount, ref int lasttableid)
        {
            //if (statcount < 1 || lasttableid > TypeConverter.StrToInt(TableList.GetPostTableId()))
            //{
            //    lasttableid = -1;
            //    return;
            //}
            for (int i = 0; i < statcount; i++)
            {
                //BBX.Data.Posts.ResetLastRepliesInfoOfTopics(lasttableid);
                //DatabaseProvider.GetInstance().ResetLastRepliesInfoOfTopics(lasttableid);
                // 全表统计极其的耗性能，三思呀
                //var tp = Topic.FindByID(i);
                //if (tp != null)
                //{
                //    tp.Repair();
                //    tp.Update();
                //}
                lasttableid++;
            }
        }

        public static void ResetLastRepliesInfoOfTopics(int start_tid, int end_tid)
        {
            for (int i = start_tid; i <= end_tid; i++)
            {
                //BBX.Data.Posts.ResetLastRepliesInfoOfTopics(i);
                //DatabaseProvider.GetInstance().ResetLastRepliesInfoOfTopics(i);
                var tp = Topic.FindByID(i);
                if (tp != null)
                {
                    tp.Repair();
                    tp.Update();
                }
            }
        }

        //public static void ReSetTopicPosts(int start_tid, int end_tid)
        //{
        //	IDataReader topics = BBX.Data.Topics.GetTopics(start_tid, end_tid);
        //	if (topics != null)
        //	{
        //		while (topics.Read())
        //		{
        //			int tid = topics["tid"].ToInt(-1);
        //			string postTableId = TableList.GetPostTableId(tid);
        //			int postCount = Post.GetPostCount(tid);
        //			PostInfo lastPostByTid = BBX.Data.Posts.GetLastPostByTid(tid, BaseConfigs.GetTablePrefix + "posts" + postTableId);
        //			if (lastPostByTid != null)
        //			{
        //				if (lastPostByTid.Pid.ToInt(0) != 0)
        //				{
        //					BBX.Data.Topics.UpdateTopic(tid, postCount, lastPostByTid.Pid.ToInt(0), lastPostByTid.Postdatetime.ToString(), lastPostByTid.Posterid.ToInt(0), lastPostByTid.Poster.ToString());
        //				}
        //				else
        //				{
        //					BBX.Data.Topics.UpdateTopicLastPosterId(tid);
        //				}
        //			}
        //		}
        //		topics.Close();
        //	}
        //}

        //public static void ReSetFourmTopicAPost()
        //{
        //	BBX.Data.Forums.ResetForumsPosts();
        //}

        public static void ReSetFourmTopicAPost(int start_fid, int end_fid)
        {
            //IDataReader forums = BBX.Data.Forums.GetForums(start_fid, end_fid);
            var list = XForum.Root.AllChilds.ToList().Where(e => e.ID >= start_fid && e.ID <= end_fid).ToList();
            //int num = start_fid;
            //ReSetFourmTopicPost(list, num, true);
            foreach (var item in list)
            {
                item.ResetLastPost();
            }
        }

        //private static string SubForumList(int fid)
        //{
        //	string text = fid.ToString() + ",";
        //	foreach (var current in Forums.GetForumList())
        //	{
        //		if (("," + current.Parentidlist + ",").IndexOf("," + fid + ",") >= 0)
        //		{
        //			text = text + current.Fid + ",";
        //		}
        //	}
        //	return text.TrimEnd(',');
        //}

        //private static void ReSetFourmTopicPost(List<XForum> fs, int fid, bool fixTopicCount)
        //{
        //	//if (reader != null)
        //	//{
        //	int todaypostcount = 0;
        //	int lasttid = 0;
        //	string lasttitle = "";
        //	DateTime lastpost = DateTime.MinValue;
        //	int lastposterid = 0;
        //	string lastposter = "";
        //	foreach (var f in fs)
        //	{
        //		//}
        //		//while (reader.Read())
        //		//{
        //		//fid = reader["fid"].ToInt(-1);
        //		int tcount = BBX.Data.Topics.GetTopicCountOfForumWithSub(SubForumList(fid));
        //		int pcount = GetPostsCountByFid(fid, out todaypostcount);
        //		if (fixTopicCount)
        //		{
        //			//XForum.SetRealCurrentTopics(fid);
        //			f.SetRealCurrentTopics();
        //		}
        //		else
        //		{
        //			//lasttid = 0;
        //			//lasttitle = "";
        //			//lastpost = "1900-1-1";
        //			//lastposterid = 0;
        //			//lastposter = "";
        //		}
        //		// 取子孙论坛的最新帖子
        //		//var forumLastPost = BBX.Data.Posts.GetForumLastPost(fid, TableList.CurrentTableName, tcount, pcount, lasttid, lasttitle, lastpost, lastposterid, lastposter, todaypostcount);
        //		var post = Post.FindLastByFids(f.AllChildKeys.ToArray());
        //		//if (forumLastPost.Read())
        //		if (post != null && post.Topic != null)
        //		{
        //			lasttid = post.Tid;
        //			lasttitle = post.TopicTitle;
        //			lastpost = post.PostDateTime;
        //			lastposterid = post.PosterID;
        //			lastposter = post.Poster;
        //		}
        //		//forumLastPost.Close();
        //		//BBX.Data.Forums.UpdateForum(fid, tcount, pcount, lasttid, lasttitle, lastpost, lastposterid, lastposter, todaypostcount);
        //		f.Update(tcount, pcount, lasttid, lasttitle, lastpost, lastposterid, lastposter, todaypostcount);
        //	}
        //	//	reader.Close();
        //	//}
        //}

        //public static void ReSetFourmTopicAPost(int fid)
        //{
        //	if (fid < 1) return;

        //	var f = XForum.FindByID(fid);

        //	int lasttid = 0;
        //	string lasttitle = "";
        //	DateTime lastpost = DateTime.MinValue;
        //	int lastposterid = 0;
        //	string lastposter = "";
        //	int todaypostcount = 0;
        //	int tcount = BBX.Data.Topics.GetTopicCountOfForumWithSub(SubForumList(fid));
        //	int pcount = GetPostsCountByFid(fid, out todaypostcount);
        //	// 查找本论坛最后一个帖子
        //	//IDataReader lastPostByFid = BBX.Data.Posts.GetLastPostByFid(fid, TableList.CurrentTableName);
        //	//if (lastPostByFid.Read())
        //	var post = Post.FindLastByFids(fid);
        //	if (post != null)
        //	{
        //		lasttid = post.Tid;
        //		lasttitle = post.TopicTitle;
        //		lastpost = post.PostDateTime;
        //		lastposterid = post.PosterID;
        //		lastposter = post.Poster;
        //	}
        //	//lastPostByFid.Close();
        //	//BBX.Data.Forums.UpdateForum(fid, tcount, pcount, num, lasttitle, lastpost, lastposterid, lastposter, todaypostcount);
        //	f.Update(tcount, pcount, lasttid, lasttitle, lastpost, lastposterid, lastposter, todaypostcount);
        //}

        //public static void ReSetFourmTopicAPost(int fid, out int topiccount, out int postcount, out int lasttid, out string lasttitle, out DateTime lastpost, out int lastposterid, out string lastposter, out int todaypostcount)
        //{
        //	topiccount = 0;
        //	postcount = 0;
        //	lasttid = 0;
        //	lasttitle = "";
        //	lastpost = DateTime.MinValue;
        //	lastposterid = 0;
        //	lastposter = "";
        //	todaypostcount = 0;
        //	if (fid < 1) return;

        //	topiccount = BBX.Data.Topics.GetTopicCountOfForumWithSub(SubForumList(fid));
        //	postcount = GetPostsCountByFid(fid, out todaypostcount);
        //	//IDataReader lastPostByFid = BBX.Data.Posts.GetLastPostByFid(fid, TableList.CurrentTableName);
        //	//if (lastPostByFid.Read())
        //	var post = Post.FindLastByFids(fid);
        //	if (post != null)
        //	{
        //		lasttid = post.Tid;
        //		lasttitle = post.TopicTitle;
        //		lastpost = post.PostDateTime;
        //		lastposterid = post.PosterID;
        //		lastposter = post.Poster;
        //	}
        //	//lastPostByFid.Close();
        //}

        //public static void ReSetClearMove()
        //{
        //	DatabaseProvider.GetInstance().ReSetClearMove();
        //	//BBX.Data.Forums.ReSetClearMove();
        //}
    }
}