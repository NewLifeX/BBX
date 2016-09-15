using BBX.Cache;
using System;
using System.Linq;
using BBX.Common;
using BBX.Config;
using BBX.Entity;

namespace BBX.Forum
{
    public class AdminUsers : Users
    {
        //public static bool UpdateUserAllInfo(UserInfo userInfo)
        //{
        //    Users.UpdateUser(userInfo);
        //    if (userInfo.Adminid == 0 || userInfo.Adminid > 3)
        //    {
        //        BBX.Data.Moderators.DeleteModerator(userInfo.Uid);
        //        AdminUsers.UpdateForumsFieldModerators(userInfo.Username);
        //    }
        //    string signature = Utils.HtmlEncode(ForumUtils.BanWordFilter(userInfo.Signature));
        //    var userGroupInfo = UserGroup.FindByID(userInfo.Groupid);
        //    GeneralConfigInfo config = GeneralConfigInfo.Current;
        //    PostpramsInfo postpramsInfo = new PostpramsInfo();
        //    postpramsInfo.Usergroupid = userGroupInfo.ID;
        //    postpramsInfo.Attachimgpost = config.Attachimgpost;
        //    postpramsInfo.Showattachmentpath = config.Showattachmentpath;
        //    postpramsInfo.Hide = 0;
        //    postpramsInfo.Price = 0;
        //    postpramsInfo.Sdetail = userInfo.Signature;
        //    postpramsInfo.Smileyoff = 1;
        //    postpramsInfo.BBCode = userGroupInfo.AllowSigbbCode;
        //    postpramsInfo.Parseurloff = 1;
        //    postpramsInfo.Showimages = userGroupInfo.AllowSigimgCode ? 1 : 0;
        //    postpramsInfo.Allowhtml = 0;
        //    postpramsInfo.Smiliesinfo = Smilies.GetSmiliesListWithInfo();
        //    postpramsInfo.Customeditorbuttoninfo = Editors.GetCustomEditButtonListWithInfo();
        //    postpramsInfo.Smiliesmax = config.Smiliesmax;
        //    postpramsInfo.Signature = 1;
        //    postpramsInfo.Onlinetimeout = config.Onlinetimeout;
        //    userInfo.Signature = signature;
        //    userInfo.Authstr = ForumUtils.CreateAuthStr(20);
        //    userInfo.Sightml = UBB.UBBToHTML(postpramsInfo);
        //    Users.UpdateUser(userInfo);
        //    Users.UpdateUserForumSetting(userInfo);
        //    return true;
        //}

		//public static bool UserNameChange(UserInfo userInfo, string oldusername)
		//{
		//	BBX.Data.Topics.UpdateTopicLastPoster(userInfo.Uid, userInfo.Username);
		//	BBX.Data.Topics.UpdateTopicPoster(userInfo.Uid, userInfo.Username);
		//	BBX.Data.Posts.UpdatePostPoster(userInfo.Uid, userInfo.Username);
		//	ShortMessage.UpdatePMSenderAndReceiver(userInfo.Uid, userInfo.Username);
		//	//BBX.Data.Announcements.UpdateAnnouncementPoster(userInfo.Uid, userInfo.Username);
		//	Announcement.UpdatePoster(userInfo.Uid, userInfo.Username);
		//	//if (BBX.Data.Statistics.UpdateStatisticsLastUserName(userInfo.Uid, userInfo.Username) != 0)
		//	//{
		//	//    //XCache.Remove(CacheKeys.FORUM_STATISTICS);
		//	//}
		//	//Statistic.UpdateStatisticsLastUserName(userInfo.Uid, userInfo.Username);
		//	Statistic.Reset();
		//	Forums.UpdateModeratorName(oldusername, userInfo.Username);
		//	return true;
		//}

		//public static bool DelUserAllInf(int uid, bool delposts, bool delpms)
		//{
		//	//bool flag = BBX.Data.Users.DeleteUser(uid, delposts, delpms);
		//	var flag = User.Delete(uid, delposts, delpms);
		//	if (flag)
		//	{
		//		//XCache.Remove(CacheKeys.FORUM_STATISTICS);
		//	}
		//	return flag;
		//}

        //public static void UpdateForumsFieldModerators(string username)
        //{
        //    //Forums.UpdateModeratorName(username, "");
        //    foreach (var item in XForum.FindAllWithCache())
        //    {
        //        var fi = item.Field;
        //        if (fi.ModeratorCollection.Contains(username))
        //        {
        //            fi.ModeratorCollection.Remove(username);
        //            fi.Moderators = string.Join(",", fi.ModeratorCollection);
        //            fi.Save();
        //        }
        //    }
        //}

        public static bool CombinationUser(int srcuid, int targetuid)
        {
            bool result;
            try
            {
                User src = User.FindByID(srcuid);
                User des = User.FindByID(targetuid);
                des.Credits += src.Credits;
                des.ExtCredits1 += src.ExtCredits1;
                des.ExtCredits2 += src.ExtCredits2;
                des.ExtCredits3 += src.ExtCredits3;
                des.ExtCredits4 += src.ExtCredits4;
                des.ExtCredits5 += src.ExtCredits5;
                des.ExtCredits6 += src.ExtCredits6;
                des.ExtCredits7 += src.ExtCredits7;
                des.ExtCredits8 += src.ExtCredits8;
                //Users.UpdateUser(userInfo2);
                des.Save();
                //BBX.Data.Users.CombinationUser(TableList.CurrentTableName, userInfo2, userInfo);
                des.CombinationFrom(src);
				User.Delete(srcuid, true, true);
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }
    }
}