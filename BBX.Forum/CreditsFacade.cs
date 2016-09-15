using System;
using System.Data;
using System.Text;
using System.Web;
using BBX.Common;
using BBX.Config;
using BBX.Entity;
using BBX.Forum.Common;

namespace BBX.Forum
{
    public class CreditsFacade
    {
        public static bool IsEnoughCreditsPM(int userId)
        {
            return CheckUserCreditsIsEnough(userId, 1, CreditsOperationType.SendMessage, -1);
        }

        public static int SendPM(int userId)
        {
            if (userId > 0)
            {
                return UpdateUserExtCredits(userId, 1, CreditsOperationType.SendMessage, 1, false);
            }
            return -1;
        }

        public static void PostTopic(int userId, IXForum forumInfo, bool isNeedAnimation)
        {
            if (userId == -1)
            {
                return;
            }
            float[] values = Forums.GetValues(forumInfo.PostcrEdits);
            if (values != null)
            {
                UpdateUserExtCredits(userId, values, false);
            }
            else
            {
                UpdateUserExtCredits(userId, 1, CreditsOperationType.PostTopic, 1, false);
            }
            if (isNeedAnimation)
            {
                WriteUpdateUserExtCreditsCookies((values != null) ? values : Scoresets.GetUserExtCredits(CreditsOperationType.PostTopic));
            }
        }

        public static void PostTopic(int userId, IXForum forumInfo)
        {
            PostTopic(userId, forumInfo, false);
        }

        public static void DeletePost(Post pi, bool reserveAttach)
        {
            var config = GeneralConfigInfo.Current;
            if (config.Losslessdel == 0 || pi.PostDateTime.AddHours(config.Losslessdel * 24) > DateTime.Now)
            {
                var type = (pi.Layer == 0) ? CreditsOperationType.PostTopic : CreditsOperationType.PostReply;
                var forumInfo = pi.Forum as IXForum;
                float[] array = Forums.GetValues((type == CreditsOperationType.PostTopic) ? forumInfo.PostcrEdits : forumInfo.ReplycrEdits);
                if (array == null)
                {
                    array = Scoresets.GetUserExtCredits(type);
                }
                UpdateUserExtCredits(pi.PosterID, array, 1, type, -1, true);
                if (!reserveAttach)
                {
                    int att = Attachment.FindCountByPid(pi.ID);
                    if (att != 0)
                    {
                        DeleteAttachments(pi.PosterID, att);
                    }
                }
            }
        }

        //public static void PostReply(int userId, IXForum forumInfo, bool isNeedAnimation = false)
        public static void PostReply(int userId, String replyCredits, bool isNeedAnimation = false)
        {
            if (userId == -1) return;

            float[] values = Forums.GetValues(replyCredits);
            if (values != null)
            {
                UpdateUserExtCredits(userId, values, false);
            }
            else
            {
                UpdateUserExtCredits(userId, 1, CreditsOperationType.PostReply, 1, false);
            }
            if (isNeedAnimation)
            {
                WriteUpdateUserExtCreditsCookies((values != null) ? values : Scoresets.GetUserExtCredits(CreditsOperationType.PostReply));
            }
        }

        //public static void PostReply(int userId, IXForum forumInfo)
        //{
        //    PostReply(userId, forumInfo, false);
        //}

        public static int UploadAttachments(int userId, int attachmentCount)
        {
            if (userId > 0 && attachmentCount > 0)
            {
                return UpdateUserExtCredits(userId, attachmentCount, CreditsOperationType.UploadAttachment, 1, false);
            }
            return 0;
        }

        public static int DeleteAttachments(int userId, int attachmentCount)
        {
            if (userId > 0 && attachmentCount > 0)
            {
                return UpdateUserExtCredits(userId, attachmentCount, CreditsOperationType.UploadAttachment, -1, true);
            }
            return 0;
        }

        public static int DowlnLoadAttachments(int userId, int attachmentCount)
        {
            if (userId > 0 && attachmentCount > 0)
            {
                return UpdateUserExtCredits(userId, attachmentCount, CreditsOperationType.DownloadAttachment, 1, false);
            }
            return -1;
        }

        public static bool IsEnoughCreditsDownloadAttachment(int userId, int attachmentCount)
        {
            return CheckUserCreditsIsEnough(userId, attachmentCount, CreditsOperationType.DownloadAttachment, -1);
        }

        public static void SetDigest(int userId)
        {
            if (userId > 0)
            {
                UpdateUserExtCredits(userId, 1, CreditsOperationType.Digest, 1, false);
            }
        }

        public static void UnDigest(int userId)
        {
            if (userId > 0)
            {
                UpdateUserExtCredits(userId, 1, CreditsOperationType.Digest, -1, false);
            }
        }

        public static int Search(int userId)
        {
            return UpdateUserExtCredits(userId, 1, CreditsOperationType.Search, 1, false);
        }

        //public static int UpdateUserCreditsByTradefinished(int userId)
        //{
        //	if (userId > 0)
        //	{
        //		return UpdateUserExtCredits(userId, 1, CreditsOperationType.TradeSucceed, 1, false);
        //	}
        //	return 0;
        //}

        public static void Vote(int userId)
        {
            if (userId > 0)
            {
                UpdateUserExtCredits(userId, 1, CreditsOperationType.Vote, 1, false);
            }
        }

        //public static void Invite(int userId, int mount)
        //{
        //	if (userId > 0)
        //	{
        //		UpdateUserExtCredits(userId, mount, CreditsOperationType.Invite, 1, false);
        //	}
        //}

        public static UserGroup GetCreditsUserGroupId(float credits)
        {
            var userGroupList = UserGroup.FindAllWithCache();
            UserGroup userGroupInfo = null;
            UserGroup userGroupInfo2 = null;
            foreach (var current in userGroupList)
            {
                if (!current.Is管理团队 && current.System == 0 && credits >= (float)current.Creditshigher && credits <= (float)current.Creditslower && (userGroupInfo == null || current.Creditshigher > userGroupInfo.Creditshigher))
                {
                    userGroupInfo = current;
                }
                if (userGroupInfo2 == null || userGroupInfo2.Creditshigher < current.Creditshigher)
                {
                    userGroupInfo2 = current;
                }
            }
            if (userGroupInfo2 != null && (float)userGroupInfo2.Creditshigher < credits)
            {
                userGroupInfo = userGroupInfo2;
            }
            if (userGroupInfo != null)
            {
                return userGroupInfo;
            }
            return new UserGroup();
        }

        public static int GetUserCreditsByUserInfo(IUser shortUserInfo)
        {
            string text = Scoresets.GetScoreCalFormula();
            if (Utils.StrIsNullOrEmpty(text))
            {
                return 0;
            }
            text = text.Replace("digestposts", shortUserInfo.DigestPosts.ToString());
            text = text.Replace("posts", shortUserInfo.Posts.ToString());
            text = text.Replace("oltime", shortUserInfo.OLTime.ToString());
            text = text.Replace("pageviews", shortUserInfo.PageViews.ToString());
            text = text.Replace("extcredits1", shortUserInfo.ExtCredits1.ToString());
            text = text.Replace("extcredits2", shortUserInfo.ExtCredits2.ToString());
            text = text.Replace("extcredits3", shortUserInfo.ExtCredits3.ToString());
            text = text.Replace("extcredits4", shortUserInfo.ExtCredits4.ToString());
            text = text.Replace("extcredits5", shortUserInfo.ExtCredits5.ToString());
            text = text.Replace("extcredits6", shortUserInfo.ExtCredits6.ToString());
            text = text.Replace("extcredits7", shortUserInfo.ExtCredits7.ToString());
            text = text.Replace("extcredits8", shortUserInfo.ExtCredits8.ToString());
            object strValue = Arithmetic.ComputeExpression(text);
            return Utils.StrToInt(Math.Floor(strValue.ToDouble()), 0);
        }

        /// <summary>被扣除积分以后，可能会降低用户级别</summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static int UpdateUserCredits(int userId)
        {
            User.UpdateUserCredits(userId);
            IUser user = (userId > 0) ? User.FindByID(userId) : null;
            if (user == null) return 0;

            // 被扣除积分以后，可能会降低用户级别
            var ug = UserGroup.FindByID(user.GroupID);
            if (ug != null && ug.IsCreditUserGroup)
            {
                ug = GetCreditsUserGroupId((float)user.Credits);
                if (ug.ID != user.GroupID)
                {
                    //BBX.Data.Users.UpdateUserGroup(user.ID.ToString(), ug.ID);
                    user.GroupID = ug.ID;
                    (user as User).Save();
                    Online.UpdateGroupid(user.ID, ug.ID);
                }
            }
            var httpCookie = HttpContext.Current.Request.Cookies["bbx"];
            if (httpCookie != null && httpCookie["userid"] == userId.ToString())
            {
                ForumUtils.WriteUserCreditsCookie(user, ug.GroupTitle);
            }
            return 1;
        }

        public static int UpdateUserExtCredits(int uid, float[] values, bool allowMinus)
        {
            if (uid < 1 || User.FindByID(uid) == null)
            {
                return 0;
            }
            if (values.Length < 8)
            {
                return -1;
            }
            if (!allowMinus && !User.CheckUserCreditsIsEnough(uid, values))
            {
                return -1;
            }
            User.UpdateUserExtCredits(uid, values);
            UpdateUserCredits(uid);
            for (int i = 0; i < values.Length; i++)
            {
                if ((double)values[i] != 0.0)
                {
                    Sync.UpdateCredits(uid, i + 1, values[i].ToString(), "");
                }
            }
            return 1;
        }

        public static int UpdateUserExtCredits(string uidlist, float[] values)
        {
            int num = -1;
            if (Utils.IsNumericList(uidlist))
            {
                num = 0;
                string[] array = Utils.SplitString(uidlist, ",");
                for (int i = 0; i < array.Length; i++)
                {
                    var uid = array[i].ToInt();
                    if (uid > 0)
                    {
                        num += UpdateUserExtCredits(uid, values, true);
                    }
                }
            }
            return num;
        }

        private static int UpdateUserExtCredits(int uid, int mount, CreditsOperationType creditsOperationType, int pos, bool allowMinus)
        {
            return UpdateUserExtCredits(uid, Scoresets.GetUserExtCredits(creditsOperationType), mount, creditsOperationType, pos, allowMinus);
        }

        private static void WriteUpdateUserExtCreditsCookies(float[] values)
        {
            StringBuilder stringBuilder = new StringBuilder("");
            stringBuilder.Append("0,");
            for (int i = 0; i < values.Length; i++)
            {
                stringBuilder.Append(values[i].ToString());
                stringBuilder.Append(",");
            }
            HttpCookie httpCookie = HttpContext.Current.Request.Cookies["bbx_creditnotice"];
            if (httpCookie == null)
            {
                httpCookie = new HttpCookie("bbx_creditnotice");
            }
            httpCookie.Value = stringBuilder.ToString().TrimEnd(',');
            httpCookie.Expires = DateTime.Now.AddMinutes(36000.0);
            httpCookie.Path = BaseConfigs.GetForumPath;
            HttpContext.Current.Response.AppendCookie(httpCookie);
        }

        private static bool CheckUserCreditsIsEnough(int uid, int mount, CreditsOperationType creditsOperationType, int pos)
        {
            DataTable scoreSet = Scoresets.GetScoreSet();
            scoreSet.PrimaryKey = new DataColumn[]
            {
                scoreSet.Columns["id"]
            };
            float[] array = new float[8];
            for (int i = 0; i < 8; i++)
            {
                array[i] = (Single)scoreSet.Rows[(int)creditsOperationType]["extcredits" + (i + 1)].ToDouble();
            }
            if (pos < 0)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (array[j].ToDouble() < 0f)
                    {
                        return User.CheckUserCreditsIsEnough(uid, array, pos, mount);
                    }
                }
            }
            return true;
        }

        private static int UpdateUserExtCredits(int uid, float[] extCredits, int mount, CreditsOperationType creditsOperationType, int pos, bool allowMinus)
        {
            if (uid == -1) return -1;

            float num = 0f;
            for (int i = 0; i < extCredits.Length; i++)
            {
                float num2 = extCredits[i];
                if (num2 != 0f)
                {
                    num = num2;
                    break;
                }
            }
            if (num == 0f) return 1;

            if (pos < 0)
            {
                if (creditsOperationType != CreditsOperationType.PostTopic &&
                    creditsOperationType != CreditsOperationType.PostReply && !allowMinus &&
                    !User.CheckUserCreditsIsEnough(uid, extCredits, pos, mount))
                {
                    return -1;
                }
            }
            else
            {
                if ((creditsOperationType == CreditsOperationType.DownloadAttachment ||
                    creditsOperationType == CreditsOperationType.Search) && !allowMinus &&
                    !User.CheckUserCreditsIsEnough(uid, extCredits, -1, mount))
                {
                    return -1;
                }
            }
            User.UpdateUserExtCredits(uid, extCredits, pos, mount);
            for (int j = 0; j < extCredits.Length; j++)
            {
                if ((double)extCredits[j] != 0.0)
                {
                    Sync.UpdateCredits(uid, j + 1, extCredits[j].ToString(), "");
                }
            }
            UpdateUserCredits(uid);
            return 1;
        }
    }
}