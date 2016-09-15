using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using BBX.Common;
using BBX.Config;
using BBX.Entity;
using XCode;

namespace BBX.Forum
{
    public class Debates
    {
        public static Dictionary<int, int> GetPostDebateList(int tid)
        {
            if (tid <= 0) return null;

            //return BBX.Data.Debates.GetPostDebateList(tid);
            return PostDebateField.FindAllByTid(tid).ToList().ToDictionary(e => e.Pid, e => e.Opinion);
        }

        //public static DebateInfo GetDebateTopic(int tid)
        //{
        //    if (tid <= 0)
        //    {
        //        return null;
        //    }
        //    return BBX.Data.Debates.GetDebateTopic(tid);
        //}

        //public static bool UpdateDebateTopic(DebateInfo debateInfo)
        //{
        //    return debateInfo.Tid > 0 && BBX.Data.Debates.UpdateDebateTopic(debateInfo);
        //}

        public static string GetDebatesJsonList(string callback, string tidlist)
        {
            if (callback == null) return "0";

            if (callback == "recommenddebates")
            {
                if (Utils.IsNumericList(tidlist))
                {
                    if (Utils.StrIsNullOrEmpty(tidlist))
                    {
                        tidlist = GeneralConfigInfo.Current.Recommenddebates;
                    }
                    //return BBX.Data.Debates.GetRecommendDebates(callback, tidlist);
                    var list = Topic.GetRecommendDebates(tidlist);
                    return DebateTopicJson(callback, list);
                }
            }
            else if (callback == "gethotdebatetopic")
            {
                string[] array = Utils.StrIsNullOrEmpty(tidlist) ? new string[0] : tidlist.Split(',');
                if (array.Length >= 3 && (!(array[0] != "views") || !(array[0] != "replies") || !Utils.IsNumeric(array[1]) || !Utils.IsNumeric(array[2])))
                {
                    var list = Topic.GetHotDebatesList(array[0], array[1].ToInt(), array[2].ToInt());
                    return DebateTopicJson(callback, list);
                }
            }

            return "0";
        }

        public static StringBuilder CommentDabetas(int tid, string message, bool ispost)
        {
            var sb = Debates.IsValidDebates(tid, message, ispost);
            if (!sb.ToString().Contains("<error>"))
            {
                sb.Append("<message>" + message + "</message>");
                //BBX.Data.Debates.CommentDabetas(tid, TypeConverter.ObjectToInt(TableList.GetPostTableId(tid)), Utils.HtmlEncode(ForumUtils.BanWordFilter(message)));
                var pi = Post.FindByTid(tid);
                pi.Message += Utils.HtmlEncode(ForumUtils.BanWordFilter(message));
                pi.Update();
            }
            return sb;
        }

        private static string DebateTopicJson(string callback, EntityList<Topic> list)
        {
            var sb = new StringBuilder();
            sb.Append(callback);
            sb.Append("([");
            //while (reader.Read())
            foreach (var tp in list)
            {
                sb.Append(string.Format("{{'title':'{0}','tid','{1}'}},", tp.Title, tp.ID));
            }
            //reader.Close();
            if (sb.ToString().IndexOf("title") < 0)
            {
                return "0";
            }
            return sb.ToString().Remove(sb.Length - 1) + "])";
        }

        private static StringBuilder IsValidDebates(int tid, string message, bool ispost)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");
            if (!ispost || ForumUtils.IsCrossSitePost())
            {
                stringBuilder.Append("<error>您的请求来路不正确，无法提交。如果您安装了某种默认屏蔽来路信息的个人防火墙软件(如 Norton Internet Security)，请设置其不要禁止来路信息后再试。</error>");
                return stringBuilder;
            }
            Regex regex = new Regex("\\[area=([\\s\\S]+?)\\]([\\s\\S]+?)\\[/area\\]", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            MatchCollection matchCollection = regex.Matches(message);
            if (matchCollection.Count == 0)
            {
                stringBuilder.Append("<error>评论内容不能为空</error>");
                return stringBuilder;
            }
            var topicInfo = Topic.FindByID(tid);
            if (tid == 0 || topicInfo.Special != 4)
            {
                stringBuilder.Append("<error>本主题不是辩论帖，无法点评</error>");
                return stringBuilder;
            }
            var db = Debate.FindByTid(tid);
            if (db.TerminalTime > DateTime.Now)
            {
                stringBuilder.Append("<error>本辩论帖结束时间未到，无法点评</error>");
                return stringBuilder;
            }
            return stringBuilder;
        }

        public static bool AllowDiggs(int userid)
        {
            if (!UserGroup.Guest.AllowDiggs && userid == -1)
            {
                return false;
            }
            //var userGroupInfo = UserGroup.FindByID(BBX.Data.Users.GetUserInfo(userid).Groupid);
            var userGroupInfo = UserGroup.FindByID(User.FindByID(userid).GroupID);
            return userGroupInfo.AllowDiggs;
        }

        public static void AddDebateDigg(int tid, int pid, int type, int userid)
        {
            if (userid < 0) return;

            var userInfo = User.FindByID(userid);
            if (userInfo == null) return;

            //BBX.Data.Debates.AddDebateDigg(tid, pid, type, Utils.GetRealIP(), userInfo);
            throw new NotImplementedException();
        }

        public static bool IsDigged(int pid, int userid)
        {
            if (!UserGroup.Guest.AllowDiggs)
            {
                //return !DatabaseProvider.GetInstance().AllowDiggs(pid, userid);
                return Debatedigg.FindByPidAndUid(pid, userid) != null;
            }
            if (Utils.StrIsNullOrEmpty(Utils.GetCookie("debatedigged")))
            {
                return false;
            }
            string[] array = Utils.GetCookie("debatedigged").Split(',');
            for (int i = 0; i < array.Length; i++)
            {
                string expression = array[i];
                if (pid == Utils.StrToInt(expression, 0))
                {
                    return true;
                }
            }
            return false;
        }

        public static void WriteCookies(int pid)
        {
            if (Utils.StrIsNullOrEmpty(Utils.GetCookie("debatedigged")))
            {
                Utils.WriteCookie("debatedigged", pid.ToString(), 1440);
                return;
            }
            Utils.WriteCookie("debatedigged", Utils.GetCookie("debatedigged") + "," + pid, 1440);
        }

        //public static int GetDebatesPostCount(PostpramsInfo postpramsInfo, int debateOpinion)
        //{
        //    return BBX.Data.Debates.GetDebatesPostCount(postpramsInfo.Tid, debateOpinion);
        //}

        public static List<Post> GetPositivePostList(PostpramsInfo postpramsInfo, out List<Attachment> attachmentlist, bool ismoder)
        {
            return GetDebatePostList(postpramsInfo, out attachmentlist, ismoder, 1, new PostOrderType());
        }

        public static List<Post> GetNegativePostList(PostpramsInfo postpramsInfo, out List<Attachment> attachmentlist, bool ismoder)
        {
            return GetDebatePostList(postpramsInfo, out attachmentlist, ismoder, 2, new PostOrderType());
        }

        private static List<Post> GetDebatePostList(PostpramsInfo pi, out List<Attachment> attachList, bool isModer, int debateOpinion, PostOrderType postOrderType)
        {
            var sb = new StringBuilder();
            //var list = BBX.Data.Debates.GetDebatePostList(pi, debateOpinion, postOrderType);
            var list = Post.SearchDebate(pi.Tid, debateOpinion, (pi.Pageindex - 1) * pi.Pagesize, pi.Pagesize);
            if (list.Count == 0 && pi.Pageindex > 1)
            {
                //var count = BBX.Data.Debates.GetRealDebatePostCount(pi.Tid, debateOpinion);
                var count = Post.SearchDebateCount(pi.Tid, debateOpinion);
                pi.Pageindex = ((count % pi.Pagesize == 0) ? (count / pi.Pagesize) : (count / pi.Pagesize + 1));
                //list = BBX.Data.Debates.GetDebatePostList(pi, debateOpinion, postOrderType);
                list = Post.SearchDebate(pi.Tid, debateOpinion, (pi.Pageindex - 1) * pi.Pagesize, pi.Pagesize);
            }
            var list2 = list.ToList();
            // 设置楼层
            var start = (pi.Pageindex - 1) * pi.Pagesize;
            var db = Debate.FindByTid(pi.Tid);
            foreach (var item in list2)
            {
                item.Id = ++start;

                if (db != null) db.CastTo(item);
            }

            var sb2 = new StringBuilder();
            int inPostAdCount = Advertisement.GetInPostAdCount("", pi.Fid);
            foreach (var item in list2)
            {
                Posts.LoadExtraPostInfo(item, inPostAdCount);
                sb.AppendFormat("{0},", item.ID);
                if (item.Attachment > 0)
                {
                    sb2.AppendFormat("{0},", item.ID);
                }
            }
            attachList = Attachments.GetAttachmentList(pi, sb2.ToString().TrimEnd(','));
            var postDiggs = Debates.GetPostDiggs(sb.ToString().Trim(','));
            foreach (var item in list2)
            {
                if (postDiggs.ContainsKey(item.ID))
                {
                    item.Diggs = postDiggs[item.ID];
                }
            }
            Posts.ParsePostListExtraInfo(pi, attachList, isModer, list2);
            return list2;
        }

        public static Dictionary<int, int> GetPostDiggs(string pidlist)
        {
            if (!Utils.IsNumericList(pidlist)) return new Dictionary<int, int>();

            //return BBX.Data.Debates.GetPostDiggs(pidlist);
            var list = PostDebateField.FindAllByPids(pidlist.SplitAsInt());
            return list.ToList().ToDictionary(e => e.Pid, e => e.Diggs);
        }

        //public static void DeleteDebatePost(int tid, int opinion, int pid)
        //{
        //    switch (opinion)
        //    {
        //        case 1:
        //            //BBX.Data.Debates.DeleteDebatePost(tid, "positivediggs", pid);
        //            Debate.DeleteDebatePost(tid, 1, pid);
        //            return;

        //        case 2:
        //            BBX.Data.Debates.DeleteDebatePost(tid, "negativediggs", pid);
        //            return;

        //        default:
        //            return;
        //    }
        //}

        //public static string CheckDebateVoter(int tid, int stand)
        //{
        //    return BBX.Data.Debates.CheckDebateVoter(tid, stand);
        //}

        //public static void InsertDebateVoter(int tid, string uidList, int stand)
        //{
        //    BBX.Data.Debates.InsertDebateVoter(tid, uidList, stand);
        //}

        //public static int GetDebatePostCountByPosterId(string onlyauthor, int topicId, int posterId, int replies, int stand)
        //{
        //    return BBX.Data.Debates.GetDebatePostCountByPosterId(onlyauthor, TableList.GetPostTableId(topicId), topicId, posterId, stand);
        //}
    }
}