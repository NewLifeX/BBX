using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using BBX.Common;
using BBX.Config;
using BBX.Entity;
using XCode;

namespace BBX.Forum
{
    public class Posts
    {
        private static Regex regexAttach = new Regex("\\[attach\\](\\d+?)\\[\\/attach\\]", RegexOptions.IgnoreCase);
        private static Regex regexHide = new Regex("\\s*\\[hide\\][\\n\\r]*([\\s\\S]+?)[\\n\\r]*\\[\\/hide\\]\\s*", RegexOptions.IgnoreCase);
        private static Regex regexAttachImg = new Regex("\\[attachimg\\](\\d+?)\\[\\/attachimg\\]", RegexOptions.IgnoreCase);
        private static object lockHelper = new object();

        public static int DeletePost(Post pi, bool reserveAttach, bool chanagePostStatistic)
        {
            if (!reserveAttach)
            {
                //Attachments.DeleteAttachmentByPid(pid);
                Attachment.FindAllByPid(pi.ID).Delete();
            }
            //RateLogs.DeleteRateLog(pid);
            //PostInfo postInfo = GetPostInfo(postTableId, pid);
            //var pi = Post.FindByID(pid);
            CreditsFacade.DeletePost(pi, reserveAttach);
            //return BBX.Data.Posts.DeletePost(postTableId, pid, chanagePostStatistic);
            //return DatabaseProvider.GetInstance().DeletePost(postTableId, pid, chanagePostStatistic);
            return pi.Delete();
        }

        public static int GetUnauditNewPostCount(string fidList, int filter)
        {
            if (!Utils.IsNumericList(fidList)) return 0;

            //return BBX.Data.Posts.GetUnauditNewPostCount(fidList, postTableId, filter);
            return Post.GetUnauditNewPostCount(fidList, filter);
        }

        public static string[] GetHiddenAttachIdList(string content, int hide)
        {
            if (hide == 0)
            {
                return new string[0];
            }
            StringBuilder stringBuilder = new StringBuilder();
            StringBuilder stringBuilder2 = new StringBuilder();
            foreach (Match match in Posts.regexHide.Matches(content))
            {
                if (hide == 1)
                {
                    stringBuilder2.Append(match.Groups[0].ToString());
                }
            }
            foreach (Match match2 in Posts.regexAttach.Matches(stringBuilder2.ToString()))
            {
                stringBuilder.Append(match2.Groups[1].ToString());
                stringBuilder.Append(",");
            }
            foreach (Match match3 in Posts.regexAttachImg.Matches(stringBuilder2.ToString()))
            {
                stringBuilder.Append(match3.Groups[1].ToString());
                stringBuilder.Append(",");
            }
            if (stringBuilder.Length == 0)
            {
                return new string[0];
            }
            return stringBuilder.Remove(stringBuilder.Length - 1, 1).ToString().Split(',');
        }

        public static EntityList<Post> GetPostTree(int tid, int hide, int userCredit)
        {
            var list = Post.GetPostTree(tid);
            foreach (var pi in list)
            {
                pi["spaces"] = Utils.GetSpacesString(pi.Layer);
                var msg = pi.Message;
                var msg2 = Utils.CutString(Utils.HtmlEncode(msg), 0, 50);
                if (hide != -1) msg2 = UBB.HideDetail(msg2, hide, userCredit);
                pi.Message = msg2;

                if (!String.IsNullOrEmpty(msg)) pi.Title = msg;
            }
            return list;
        }

        public static int UpdatePostRateTimes(int tid, string postidlist)
        {
            if (!Utils.IsNumericList(postidlist)) return 0;

            return RateLog.UpdatePostRateTimes(postidlist);
        }

        public static List<Post> GetPostList(PostpramsInfo pi, out List<Attachment> attachList, bool isModer)
        {
            //var postList = BBX.Data.Posts.GetPostList(postpramsInfo);
            var uid = 0;
            if (!pi.Condition.IsNullOrWhiteSpace()) uid = pi.Condition.Substring(pi.Condition.IndexOf("=") + 1).ToInt();
            var postList = Post.GetPostList(pi.Tid, uid, pi.Invisible, pi.Pageindex, pi.Pagesize)
                .ToList();
            attachList = new List<Attachment>();
            if (postList.Count == 0) return postList;
            // 设置楼层
            var start = (pi.Pageindex - 1) * pi.Pagesize;
            foreach (var item in postList)
            {
                item.Id = ++start;
            }

            int inPostAdCount = Advertisement.GetInPostAdCount("", pi.Fid);
            //ForumInfo forumInfo = Forums.GetForumInfo(postList[0].Fid);
            var forumInfo = XForum.FindByID(postList[0].Fid);
            foreach (var item in postList)
            {
                item.SmileyOff = !forumInfo.AllowSmilies ? 1 : item.SmileyOff;
                item.BBCodeOff = !forumInfo.AllowBbCode ? 1 : item.BBCodeOff;
                Posts.LoadExtraPostInfo(item, inPostAdCount);
            }
            string pidListWithAttach = Posts.GetPidListWithAttach(postList);
            attachList = Attachments.GetAttachmentList(pi, pidListWithAttach);
            Posts.ParsePostListExtraInfo(pi, attachList, isModer, postList);
            return postList;
        }

        public static void ParsePostListExtraInfo(PostpramsInfo postpramsInfo, List<Attachment> attachList, bool isModer, List<Post> postList)
        {
            int hide = postpramsInfo.Hide;
            int allowGetAttachValue = Posts.GetAllowGetAttachValue(postpramsInfo);
            var guest = UserGroup.Guest;
            //var topicInfo = postpramsInfo.Topicinfo;
            //if (postpramsInfo.Topicinfo == null) topicInfo = Topic.FindByID(postpramsInfo.Tid);
            var topicInfo = Topic.FindByID(postpramsInfo.Tid);

            string stringarray = string.Empty;
            if (topicInfo.Special == 4 && !guest.AllowDiggs)
            {
                stringarray = Debatedigg.GetUesrDiggs(postpramsInfo.Tid, postpramsInfo.CurrentUserid);
            }
            foreach (var pi in postList)
            {
                Posts.LoadPostMessage(postpramsInfo, attachList, isModer, allowGetAttachValue, hide, pi);
                if (topicInfo.Special == 4)
                {
                    if (guest.AllowDiggs)
                    {
                        pi.Digged = Debates.IsDigged(pi.ID, postpramsInfo.CurrentUserid);
                    }
                    else
                    {
                        pi.Digged = Utils.InArray(pi.ID.ToString(), stringarray);
                    }
                }
            }
        }

        public static string GetPidListWithAttach(List<Post> postList)
        {
            var sb = new StringBuilder(",");
            for (int i = 0; i < postList.Count; i++)
            {
                if (postList[i].Attachment > 0)
                {
                    sb.Append(postList[i].ID);
                    sb.Append(",");
                }
            }
            return sb.ToString().Trim(',');
        }

        private static int GetAllowGetAttachValue(PostpramsInfo postpramsInfo)
        {
            if (Forums.AllowGetAttachByUserID(Forums.GetForumInfo(postpramsInfo.Fid).Permuserlist, postpramsInfo.CurrentUserid))
            {
                return 1;
            }
            int result = 0;
            if (postpramsInfo.Getattachperm.IsNullOrWhiteSpace() || postpramsInfo.Getattachperm == null)
            {
                result = postpramsInfo.CurrentUserGroup.AllowGetattach ? 1 : 0;
            }
            else
            {
                if (Forums.AllowGetAttach(postpramsInfo.Getattachperm, postpramsInfo.Usergroupid))
                {
                    result = 1;
                }
            }
            return result;
        }

        public static void LoadExtraPostInfo(Post postInfo, int adCount)
        {
            var ug = postInfo.PostUser.Group;
            Random random = new Random((int)DateTime.Now.Ticks);
            if (postInfo.Medals != string.Empty)
            {
                postInfo.Medals = Medal.GetMedalsList(postInfo.Medals);
            }
            if (ug != null)
            {
                postInfo.Stars = ug.Stars;
                if (String.IsNullOrEmpty(ug.Color))
                {
                    postInfo.Status = ug.GroupTitle;
                }
                else
                {
                    postInfo.Status = string.Format("<span style=\"color:{0}\">{1}</span>", ug.Color, ug.GroupTitle);
                }
            }
            string[] array = Utils.SplitString(GeneralConfigInfo.Current.Postnocustom, "\n");
            if (array.Length >= postInfo.Id)
            {
                postInfo.Postnocustom = array[postInfo.Id - 1];
            }
            postInfo.Adindex = random.Next(0, adCount);
        }

        private static void LoadPostMessage(PostpramsInfo postpramsInfo, List<Attachment> attachList, bool isModer, int allowGetAttach, int originalHideStatus, Post pi)
        {
            bool flag = !Utils.InArray(pi.PostUser.GroupID + "", "4,5,6") && pi.Invisible == 0;
            var msg = pi.Message;
            if (pi.Invisible == 1)
            {
                msg = "<div class='hintinfo'>该帖子尚未通过审核, 您是发帖者, 以下是帖子内容</div>" + msg;
            }
            else if (!flag)
            {
                if (isModer)
                {
                    msg = "<div class='hintinfo'>该用户帖子内容已被屏蔽, 您拥有管理权限, 以下是帖子内容</div>" + msg;
                }
                else
                {
                    msg = "<div class='hintinfo'>该用户帖子内容已被屏蔽</div>";
                    var list = new List<Attachment>();
                    foreach (var att in attachList)
                    {
                        if (att.Pid == pi.ID)
                        {
                            list.Add(att);
                        }
                    }
                    foreach (var current2 in list)
                    {
                        attachList.Remove(current2);
                    }
                }
            }
            if (flag || isModer)
            {
                postpramsInfo.Smileyoff = pi.SmileyOff;
                postpramsInfo.BBCode = pi.BBCodeOff == 0;
                postpramsInfo.Parseurloff = pi.ParseUrlOff;
                postpramsInfo.Allowhtml = pi.HtmlOn;
                postpramsInfo.Sdetail = msg;
                postpramsInfo.Pid = pi.ID;
                var user = pi.PostUser;
                if (user != null && user.Group != null && !user.Group.AllowHideCode)
                    postpramsInfo.Hide = 0;

                if (!postpramsInfo.Ubbmode)
                    msg = UBB.UBBToHTML(postpramsInfo);
                else
                    msg = Utils.HtmlEncode(msg);

                if (postpramsInfo.Jammer == 1) msg = ForumUtils.AddJammer(msg);

                string text = msg;
                if (pi.Attachment > 0 || Posts.regexAttach.IsMatch(text) || Posts.regexAttachImg.IsMatch(text))
                {
                    string[] hiddenAttachIdList = Posts.GetHiddenAttachIdList(postpramsInfo.Sdetail, postpramsInfo.Hide);
                    var list2 = new List<Attachment>();
                    foreach (var item in attachList)
                    {
                        text = Attachments.GetMessageWithAttachInfo(postpramsInfo, allowGetAttach, hiddenAttachIdList, pi.ID, pi.PosterID, item, text);
                        if (item.Inserted || Utils.InArray(item.ID.ToString(), hiddenAttachIdList))
                        {
                            list2.Add(item);
                        }
                    }
                    foreach (var item in list2)
                    {
                        attachList.Remove(item);
                    }
                }
                msg = text;
                postpramsInfo.Hide = originalHideStatus;
            }
            pi.Html = msg;
        }

        public static bool IsAD(string regular, string title, string message)
        {
            return !(String.IsNullOrEmpty(regular.Trim())) && (Regex.IsMatch(title, regular) || Regex.IsMatch(ForumUtils.RemoveSpecialChars(message, GeneralConfigInfo.Current.Antispamreplacement), regular));
        }

        public static void AuditPost(string validate, string delete, string ignore, string fidlist)
        {
            //BBX.Data.Posts.AuditPost(postTableId, validate, delete, ignore, fidlist);
            if (!delete.IsNullOrWhiteSpace())
            {
                var list = Post.FindAllByIDs(delete);
                list.Delete();
            }
            if (!ignore.IsNullOrWhiteSpace())
            {
                var list = Post.FindAllByIDs(ignore);
                list.ForEach(e => e.Invisible = -3);
                list.Update();
            }
            if (!string.IsNullOrEmpty(validate))
            {
                Post.Pass(validate);
                var list = Post.FindAllByIDs(validate);
                foreach (var item in list)
                {
                    CreditsFacade.PostReply(item.PosterID, item.Forum.ReplycrEdits);
                }
                //foreach (DataRow dataRow in Posts.GetPostListByCondition(BaseConfigs.GetTablePrefix + "posts" + postTableId, "[pid] IN (" + validate + ")").Rows)
                //{
                //    IXForum fm = XForum.FindByID((Int32)dataRow["fid"]);
                //    CreditsFacade.PostReply((Int32)dataRow["posterid"], fm.ReplycrEdits);
                //}
            }
        }

        public static bool GetModPostCountByPidList(string moderatorUserName, string pidList)
        {
            string mods = Moderators.GetFidListByModerator(moderatorUserName);
            return !(String.IsNullOrEmpty(mods)) && pidList.Split(',').Length == Post.GetModPostCountByPidList(mods, pidList);
        }

        public static string GetPostPramsInfoCondition(string onlyauthor, int topicid, int posterid)
        {
            if (!onlyauthor.IsNullOrEmpty() && !onlyauthor.Equals("0"))
            {
                return string.Format(" posterid={0}", posterid);
            }
            return "";
        }

        public static EntityList<Post> GetPagedLastPost(PostpramsInfo ppi)
        {
            var list = Post.GetPagedLastPost(ppi);
            var random = new Random((int)DateTime.Now.Ticks);
            int inPostAdCount = Advertisement.GetInPostAdCount("", ppi.Fid);
            foreach (var pi in list)
            {
                ppi.Smileyoff = pi.SmileyOff;
                ppi.BBCode = pi.BBCodeOff < 1;
                ppi.Parseurloff = pi.ParseUrlOff;
                ppi.Allowhtml = pi.HtmlOn;
                ppi.Pid = pi.ID;

                var msg = pi.Message;
                ppi.Sdetail = msg;
                if (ppi.Price > 0 && pi.Layer == 0)
                {
                    var ue = Scoresets.GetScoreSet(Scoresets.GetTopicAttachCreditsTrans());
                    msg = string.Format("<div class=\"paystyle\">此帖为交易帖,要付 {0} <span class=\"bold\">{1}</span>{2} 才可查看</div>", ue.Name, ppi.Price, ue.Unit);
                }
                else
                {
                    if (!ppi.Ubbmode)
                        msg = UBB.UBBToHTML(ppi);
                    else
                        msg = Utils.HtmlEncode(msg);
                }
                pi.Adindex = random.Next(0, inPostAdCount);
                if (ppi.Jammer == 1) msg = ForumUtils.AddJammer(msg);
                pi.Html = msg;

                if (!pi["showemail"].ToBoolean())
                {
                    pi["email"] = "";
                }
            }
            return list;
        }

        public static string GetPostMessage(UserGroup usergroupinfo, AdminGroup adminGroupInfo, string postmessage, bool ishtmlon)
        {
            string result;
            if (adminGroupInfo != null && adminGroupInfo.ID == 1)
            {
                if (!usergroupinfo.AllowHtml)
                {
                    result = Utils.HtmlEncode(postmessage);
                }
                else
                {
                    result = (ishtmlon ? postmessage : Utils.HtmlEncode(postmessage));
                }
            }
            else
            {
                if (!usergroupinfo.AllowHtml)
                {
                    result = Utils.HtmlEncode(ForumUtils.BanWordFilter(postmessage));
                }
                else
                {
                    result = (ishtmlon ? ForumUtils.BanWordFilter(postmessage) : Utils.HtmlEncode(ForumUtils.BanWordFilter(postmessage)));
                }
            }
            return result;
        }

        public static void ClearPosts(int uid, int days)
        {
            if (days == 0)
            {
                //foreach (var item in TableList.GetAllPostTable())
                //{
                //    //if (dataRow["id"].ToString() != "")
                //    {
                //        BBX.Data.Posts.DeletePostByPosterid(item.ID, uid);
                //    }
                //}
                //BBX.Data.Topics.DeleteTopicByPosterid(uid);
                var list = Topic.FindAllByPosterID(uid);
                list.Delete();
                //BBX.Data.Users.ClearPosts(uid);
                var user = User.FindByID(uid);
                user.DigestPosts = 0;
                user.Posts = 0;
                user.Save();
            }
            else
            {
                //BBX.Data.Posts.DeletePostByUidAndDays(uid, days);
                var ps = Post.Search(days);
                ps.Delete();
            }
            //BBX.Data.Attachments.DeleteAttachmentByUid(uid, days);
            Attachment.SearchByUidAndDays(uid, days).Delete();
        }
    }
}