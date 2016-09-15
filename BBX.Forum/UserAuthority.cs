using System;
using BBX.Common;
using BBX.Config;
using BBX.Entity;

namespace BBX.Forum
{
    public class UserAuthority
    {
        public static bool VisitAuthority(IXForum forum, UserGroup userGroupInfo, int userId, ref string msg)
        {
            if (!Forums.AllowViewByUserId(forum.Permuserlist, userId))
            {
                if (string.IsNullOrEmpty(forum.Viewperm))
                {
                    if (!userGroupInfo.AllowVisit)
                    {
                        msg = "您当前的身份 \"" + userGroupInfo.GroupTitle + "\" 没有浏览该版块的权限";
                        return false;
                    }
                }
                else
                {
                    if (!forum.AllowView(userGroupInfo.ID))
                    {
                        msg = "您没有浏览该版块的权限";
                        return false;
                    }
                }
            }
            return true;
        }
        public static bool PostAuthority(IXForum forum, UserGroup userGroupInfo, int userId, ref string msg)
        {
            if (!Forums.AllowPostByUserID(forum.Permuserlist, userId))
            {
                if (string.IsNullOrEmpty(forum.PostPerm))
                {
                    if (!userGroupInfo.AllowPost)
                    {
                        msg = "您当前的身份 \"" + userGroupInfo.GroupTitle + "\" 没有发表主题的权限";
                        return false;
                    }
                }
                else
                {
                    if (!forum.Field.AllowPost(userGroupInfo.ID))
                    {
                        msg = "您没有在该版块发表主题的权限";
                        return false;
                    }
                }
            }
            if (!forum.AllowSpecialOnly)
            {
                return true;
            }
            if (forum.AllowPostSpecial <= 0)
            {
                msg = "您没有在该版块发表特殊主题的权限";
                return false;
            }
            if ((forum.Allowpostspecial & 1) != 1 || userGroupInfo.AllowPostpoll)
            {
                return true;
            }
            msg = "您当前的身份 \"" + userGroupInfo.GroupTitle + "\" 没有发布投票的权限";
            if ((forum.Allowpostspecial & 4) != 4 || userGroupInfo.AllowBonus)
            {
                return true;
            }
            msg = "您当前的身份 \"" + userGroupInfo.GroupTitle + "\" 没有发布悬赏的权限";
            if ((forum.Allowpostspecial & 16) == 16 && !userGroupInfo.AllowDebate)
            {
                msg = "您当前的身份 \"" + userGroupInfo.GroupTitle + "\" 没有发起辩论的权限";
                return false;
            }
            return true;
        }
        public static bool PostAttachAuthority(IXForum forum, UserGroup userGroupInfo, int userId, ref string msg)
        {
            if (!Forums.AllowPostAttachByUserID(forum.Permuserlist, userId))
            {
                bool flag = forum.Field.AllowPostAttach(userGroupInfo.ID);
                if (flag && forum.PostattachPerm != "")
                {
                    return true;
                }
                if (!flag)
                {
                    msg = "您没有在该版块上传附件的权限";
                    return false;
                }
                if (!userGroupInfo.AllowPostattach)
                {
                    msg = string.Format("您当前的身份 \"{0}\" 没有上传附件的权限", userGroupInfo.GroupTitle);
                    return false;
                }
            }
            return true;
        }
        public static bool CheckUsertAttachAuthority(IXForum forum, UserGroup userGroupInfo, int userId, ref string msg)
        {
            if (!Forums.AllowGetAttachByUserID(forum.Permuserlist, userId))
            {
                if (Utils.StrIsNullOrEmpty(forum.GetattachPerm) && !userGroupInfo.AllowGetattach)
                {
                    msg = string.Format("您当前的身份 \"{0}\" 没有下载或查看附件的权限", userGroupInfo.GroupTitle);
                }
                else
                {
                    if (!forum.Field.AllowGetAttach(userGroupInfo.ID))
                    {
                        msg = "您没有在该版块下载附件的权限";
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool CheckNewbieSpan(int userId)
        {
            var config = GeneralConfigInfo.Current;
            if (config.Newbiespan > 0)
            {
                var user = User.FindByID(userId);
                if (user == null) return true;

                return DateTime.Now < user.JoinDate.AddMinutes(config.Newbiespan);
            }
            return false;
        }

        public static bool CheckPostTimeSpan(UserGroup userGroupInfo, AdminGroup admininfo, Online olUserInfo, IUser user, ref string msg)
        {
            var cfg = GeneralConfigInfo.Current;
            if (olUserInfo.AdminID != 1 && !userGroupInfo.DisablePeriodctrl)
            {
                var str = "";
                if (Scoresets.BetweenTime(cfg.Postbanperiods, out str))
                {
                    msg = "在此时间段( " + str + " )内用户不可以发帖";
                    return false;
                }
            }
            if (admininfo == null || !admininfo.DisablePostctrl)
            {
                var ts = olUserInfo.LastPostTime.AddSeconds(cfg.Postinterval) - DateTime.Now;
                if (ts.TotalSeconds > 0)
                {
                    msg = "系统规定发帖间隔为" + cfg.Postinterval + "秒, 您还需要等待 " + ts.TotalSeconds + " 秒";
                    return false;
                }
                if (olUserInfo.UserID != -1)
                {
                    if (user == null)
                    {
                        msg = "您的用户资料出现错误";
                        return false;
                    }
                    //num = Utils.StrDateDiffMinutes(text, cfg.Newbiespan);
                    ts = user.JoinDate.AddMinutes(cfg.Newbiespan) - DateTime.Now;
                    if (ts.TotalMinutes > 0)
                    {
                        msg = "系统规定新注册用户必须要在" + cfg.Newbiespan + "分钟后才可以发帖, 您还需要等待 " + ts.TotalMinutes + " 分钟";
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool PostSpecialAuthority(IXForum forum, string type, ref string msg)
        {
            if (forum.Allowpostspecial > 0)
            {
                if (type == "poll" && (forum.Allowpostspecial & 1) != 1)
                {
                    msg = string.Format("当前版块 \"{0}\" 不允许发表投票", forum.Name);
                    return false;
                }
                if (type == "bonus" && (forum.Allowpostspecial & 4) != 4)
                {
                    msg = string.Format("当前版块 \"{0}\" 不允许发表悬赏", forum.Name);
                    return false;
                }
                if (type == "debate" && (forum.Allowpostspecial & 16) != 16)
                {
                    msg = string.Format("当前版块 \"{0}\" 不允许发表辩论", forum.Name);
                    return false;
                }
            }
            return true;
        }

        public static bool PostSpecialAuthority(UserGroup usergroupinfo, string type, ref string msg)
        {
            if (type == "poll" && !usergroupinfo.AllowPostpoll)
            {
                msg = string.Format("您当前的身份 \"{0}\" 没有发布投票的权限", usergroupinfo.GroupTitle);
                return false;
            }
            if (type == "bonus" && !usergroupinfo.AllowBonus)
            {
                msg = string.Format("您当前的身份 \"{0}\" 没有发布悬赏的权限", usergroupinfo.GroupTitle);
                return false;
            }
            if (type == "debate" && !usergroupinfo.AllowDebate)
            {
                msg = string.Format("您当前的身份 \"{0}\" 没有发起辩论的权限", usergroupinfo.GroupTitle);
                return false;
            }
            return true;
        }

        public static int GetTopicPostInvisible(IXForum forum, int useradminid, int uid, UserGroup userGroup, Post postinfo)
        {
            if (useradminid == 1 || Moderators.IsModer(useradminid, uid, forum.Fid))
            {
                return 0;
            }
            if (!ForumUtils.HasAuditWord(postinfo.Message) && forum.Modnewtopics == 0 && userGroup.ModNewTopics == 0 && !Scoresets.BetweenTime(GeneralConfigInfo.Current.Postmodperiods))
            {
                return 0;
            }
            return 1;
        }

        //public static bool NeedAudit(Int32 fid, Int32 modnewposts, int useradminid, int userid, UserGroup userGroup, Topic topicInfo = null)
        //{
        //    //return useradminid != 1 && !Moderators.IsModer(useradminid, userid, fid) && (Scoresets.BetweenTime(GeneralConfigInfo.Current.Postmodperiods) || modnewposts == 1 || userGroup != null && userGroup.ModNewPosts == 1 || topicInfo != null && topicInfo.Displayorder == -2);

        //    if (useradminid == 1) return false;
        //    if (Moderators.IsModer(useradminid, userid, fid)) return false;

        //    if (Scoresets.BetweenTime(GeneralConfigInfo.Current.Postmodperiods)) return true;
        //    if (modnewposts == 1) return true;
        //    if (userGroup != null && userGroup.ModNewPosts == 1) return true;
        //    if (topicInfo != null && topicInfo.DisplayOrder == -2) return true;

        //    return false;
        //}

        public static bool NeedAudit(Int32 fid, Int32 modnewposts, int useradminid, int userid, UserGroup userGroup, Topic topicInfo = null)
        {
            //return useradminid != 1 && !Moderators.IsModer(useradminid, userid, fid) && (Scoresets.BetweenTime(GeneralConfigInfo.Current.Postmodperiods) || modnewposts == 1 || userGroup != null && userGroup.ModNewPosts == 1 || topicInfo != null && topicInfo.Displayorder == -2);

            if (useradminid == 1) return false;
            if (Moderators.IsModer(useradminid, userid, fid)) return false;

            if (Scoresets.BetweenTime(GeneralConfigInfo.Current.Postmodperiods)) return true;
            if (modnewposts == 1) return true;
            if (userGroup != null && userGroup.ModNewPosts == 1) return true;
            if (topicInfo != null && topicInfo.DisplayOrder == -2) return true;

            return false;
        }

        //public static bool NeedAudit(Int32 fid, Int32 modnewposts, int useradminid, int userid, UserGroup userGroup)
        //{
        //    return useradminid != 1 && !Moderators.IsModer(useradminid, userid, fid) && (Scoresets.BetweenTime(GeneralConfigInfo.Current.Postmodperiods) || modnewposts == 1 || userGroup != null && userGroup.ModNewTopics == 1);

        //    if (useradminid == 1) return false;
        //    if (Moderators.IsModer(useradminid, userid, fid)) return false;

        //    if (Scoresets.BetweenTime(GeneralConfigInfo.Current.Postmodperiods)) return true;
        //    if (modnewposts == 1) return true;
        //    if (userGroup != null && userGroup.ModNewPosts == 1) return true;

        //    return false;
        //}

        //public static bool PostReply(String permuserlist, String replyperm, int userid, UserGroup usergroupinfo, TopicInfo topic)
        //{
        //    bool result = usergroupinfo.Is管理团队;
        //    if (topic.Closed == 0)
        //    {
        //        if (userid > -1 && Forums.AllowReplyByUserID(permuserlist, userid)) return true;

        //        if (Utils.StrIsNullOrEmpty(replyperm))
        //        {
        //            if (usergroupinfo.AllowReply) return true;
        //        }
        //        else
        //        {
        //            if (Forums.AllowReply(replyperm, usergroupinfo.ID)) return true;
        //        }
        //    }
        //    return result;
        //}

        public static bool PostReply(IXForum fi, int userid, UserGroup usergroupinfo, Topic topic)
        {
            bool result = usergroupinfo.Is管理团队;
            if (topic.Closed == 0)
            {
                if (userid > -1 && Forums.AllowReplyByUserID(fi.Permuserlist, userid)) return true;

                if (Utils.StrIsNullOrEmpty(fi.ReplyPerm))
                {
                    if (usergroupinfo.AllowReply) return true;
                }
                else
                {
                    if (fi.Field.AllowReply(usergroupinfo.ID)) return true;
                }
            }
            return result;
        }

        public static bool DownloadAttachment(IXForum forum, int userid, UserGroup usergroupinfo)
        {
            bool result = false;
            if (Forums.AllowGetAttachByUserID(forum.Permuserlist, userid)) return true;

            if (Utils.StrIsNullOrEmpty(forum.Getattachperm))
            {
                if (usergroupinfo.AllowGetattach) return true;
            }
            else
            {
                if (forum.Field.AllowGetAttach(usergroupinfo.ID)) return true;
            }

            return result;
        }

        public static bool CanEditPost(Post postInfo, int userId, int userAdminId, ref string msg)
        {
            if (postInfo.PosterID != userId && BaseConfigs.GetFounderUid != userId)
            {
                if (postInfo.PosterID == BaseConfigs.GetFounderUid)
                {
                    msg = "您无权编辑创始人的帖子";
                    return false;
                }
                if (postInfo.PosterID != -1)
                {
                    UserGroup userGroupInfo = UserGroup.FindByID(User.FindByID(postInfo.PosterID).GroupID);
                    if (userGroupInfo.Is管理团队 && userGroupInfo.RadminID < userAdminId)
                    {
                        msg = "您无权编辑更高权限人的帖子";
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool Search(UserGroup usergroupinfo, ref string msg)
        {
            if (!usergroupinfo.AllowSearch)
            {
                msg = "您当前的身份 " + usergroupinfo.GroupTitle + " 没有搜索的权限";
                return false;
            }
            //if (usergroupinfo.Allowsearch == 2 && DNTRequest.GetInt("keywordtype", 0) == 1)
            //{
            //    msg = "您当前的身份 " + usergroupinfo.GroupTitle + " 没有全文搜索的权限";
            //    return false;
            //}
            return true;
        }

        public static bool Search(int userid, DateTime lastsearchtime, int useradminid, UserGroup usergroupinfo, ref string msg)
        {
            var config = GeneralConfigInfo.Current;
            if (useradminid != 1 && DNTRequest.GetInt("keywordtype", 0) == 1 && !usergroupinfo.DisablePeriodctrl)
            {
                string str = "";
                if (Scoresets.BetweenTime(config.Searchbanperiods, out str))
                {
                    msg = "在此时间段( " + str + " )内用户不可以进行全文搜索";
                    return false;
                }
            }
            if (useradminid != 1)
            {
                if (!Statistic.CheckSearchCount(config.Maxspm))
                {
                    msg = "抱歉,系统在一分钟内搜索的次数超过了系统安全设置的上限,请稍候再试";
                    return false;
                }
                //int num = Utils.StrDateDiffSeconds(lastsearchtime, config.Searchctrl);
                var ts = lastsearchtime.AddSeconds(config.Searchctrl) - DateTime.Now;
                if (ts.TotalSeconds > 0)
                {
                    msg = "系统规定搜索间隔为" + config.Searchctrl + "秒, 您还需要等待 " + ts.TotalSeconds + " 秒";
                    return false;
                }
                if (userid != -1 && CreditsFacade.Search(userid) == -1)
                {
                    string str2 = "";
                    if (EPayments.IsOpenEPayments())
                    {
                        str2 = "<br/><span><a href=\"usercpcreditspay.aspx\">点击充值积分</a></span>";
                    }
                    msg = "您的积分不足, 不能执行搜索操作" + str2;
                    return false;
                }
            }
            return true;
        }
    }
}