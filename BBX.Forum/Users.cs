using System;
using System.Collections;
using System.Text;
using BBX.Cache;
using BBX.Common;
using BBX.Config;
using BBX.Entity;
using NewLife.Web;
using XCode;

namespace BBX.Forum
{
    public class Users
    {
        public static float GetUserExtCredit(User userInfo, int extId)
        {
            if (userInfo == null)
            {
                return 0f;
            }
            switch (extId)
            {
                case 1:
                    return userInfo.ExtCredits1;
                case 2:
                    return userInfo.ExtCredits2;
                case 3:
                    return userInfo.ExtCredits3;
                case 4:
                    return userInfo.ExtCredits4;
                case 5:
                    return userInfo.ExtCredits5;
                case 6:
                    return userInfo.ExtCredits6;
                case 7:
                    return userInfo.ExtCredits7;
                case 8:
                    return userInfo.ExtCredits8;
                default:
                    return 0f;
            }
        }

        public static User GetUserInfo(int uid)
        {
            return User.FindByID(uid);
        }

        //public static User GetShortUserInfo(int uid)
        //{
        //	return User.FindByID(uid);
        //}

        public static string CheckRegisterDateDiff(string ip)
        {
            var config = GeneralConfigInfo.Current;
            var user = User.FindByRegIP(ip);
            if (config.Regctrl > 0 && user != null && user.GroupID != 1)
            {
                //int num = Utils.StrDateDiffHours(user.JoinDate, config.Regctrl);
                var num = (Int32)(user.JoinDate.AddHours(config.Regctrl) - DateTime.Now).TotalHours;
                if (num > 0)
                {
                    return "抱歉, 系统设置了IP注册间隔限制, 您必须在 " + num + " 小时后才可以注册";
                }
            }
            if (!config.Ipregctrl.IsNullOrEmpty() && Utils.InIPArray(WebHelper.UserHost, Utils.SplitString(config.Ipregctrl, "\n")) && user != null)
            {
                //int num2 = Utils.StrDateDiffHours(user.JoinDate, 72);
                var num2 = (Int32)(user.JoinDate.AddHours(72) - DateTime.Now).TotalHours;
                if (num2 > 0)
                {
                    return "抱歉, 系统设置了特殊IP注册限制, 您必须在 " + num2 + " 小时后才可以注册";
                }
            }
            return null;
        }

        public static int GetUserId(string username)
        {
            //ShortUserInfo shortUserInfoByName = BBX.Data.Users.GetShortUserInfoByName(username);
            //if (shortUserInfoByName == null)
            //{
            //    return 0;
            //}
            //return shortUserInfoByName.Uid;
            var user = User.FindByName(username);
            return user == null ? 0 : user.ID;
        }

        //public static DataTable GetUserList(int pagesize, int pageindex, string column, string ordertype)
        //{
        //    //DataTable userList = BBX.Data.Users.GetUserList(pagesize, pageindex, column, ordertype);
        //    DataTable userList = User.GetUserList(pagesize, pageindex, column, ordertype).ToDataTable(false);
        //    if (userList == null || userList.Rows == null) return null;

        //    userList.Columns.Add("grouptitle");
        //    userList.Columns.Add("olimg");
        //    foreach (DataRow dataRow in userList.Rows)
        //    {
        //        var groupid = (Int32)dataRow["GroupID"];
        //        var userGroupInfo = UserGroup.FindByID(groupid);
        //        if (Utils.StrIsNullOrEmpty(userGroupInfo.Color))
        //        {
        //            dataRow["grouptitle"] = userGroupInfo.GroupTitle;
        //        }
        //        else
        //        {
        //            dataRow["grouptitle"] = string.Format("<font color='{1}'>{0}</font>", userGroupInfo.GroupTitle, userGroupInfo.Color);
        //        }
        //        dataRow["olimg"] = OnlineUsers.GetGroupImg(groupid);
        //    }
        //    return userList;
        //}

        public static bool CheckEmailAndSecques(string username, string email, int questionid, string answer, string forumPath)
        {
            //int num = BBX.Data.Users.CheckEmailAndSecques(username, email, ForumUtils.GetUserSecques(questionid, answer));
            var user = User.CheckEmailAndSecques(username, email, questionid, answer);
            //if (num != -1)
            if (user != null)
            {
                string text = ForumUtils.CreateAuthStr(20);
                Users.UpdateAuthStr(user.ID, text, 2);

                var sb = new StringBuilder(username);
                sb.AppendFormat("您好!<br />这封信是由 {0}", GeneralConfigInfo.Current.Forumtitle);
                sb.Append(" 发送的.<br /><br />您收到这封邮件,是因为在我们的论坛上这个邮箱地址被登记为用户邮箱,且该用户请求使用 Email 密码重置功能所致.");
                sb.Append("<br /><br />----------------------------------------------------------------------");
                sb.Append("<br />重要！");
                sb.Append("<br /><br />----------------------------------------------------------------------");
                sb.Append("<br /><br />如果您没有提交密码重置的请求或不是我们论坛的注册用户,请立即忽略并删除这封邮件.只在您确认需要重置密码的情况下,才继续阅读下面的内容.");
                sb.Append("<br /><br />----------------------------------------------------------------------");
                sb.Append("<br />密码重置说明");
                sb.Append("<br /><br />----------------------------------------------------------------------");
                sb.Append("<br /><br />您只需在提交请求后的三天之内,通过点击下面的链接重置您的密码:<br /><br />");
                sb.AppendFormat("<a href={0}/setnewpassword.aspx?uid={1}&id={2} target=_blank>{0}", forumPath, user.ID, text);
                sb.AppendFormat("/setnewpassword.aspx?uid={0}&id={1}</a>", user.ID, text);
                sb.Append("<br /><br />(如果上面不是链接形式,请将地址手工粘贴到浏览器地址栏再访问)");
                sb.Append("<br /><br />上面的页面打开后,输入新的密码后提交,之后您即可使用新的密码登录论坛了.您可以在用户控制面板中随时修改您的密码.");
                sb.AppendFormat("<br /><br />本请求提交者的 IP 为 {0}<br /><br /><br /><br />", WebHelper.UserHost);
                sb.AppendFormat("<br />此致 <br /><br />{0} 管理团队.<br />{1}<br /><br />", GeneralConfigInfo.Current.Forumtitle, forumPath);
                return Emails.SendMailToUser(DNTRequest.GetString("email"), GeneralConfigInfo.Current.Forumtitle + " 取回密码说明", sb.ToString());
            }
            return false;
        }

        public static bool ValidateEmail(string email)
        {
            return GeneralConfigInfo.Current.Doublee != 0 || User.FindByEmail(email) == null;
        }

        /// <summary>检查邮箱是否被使用过</summary>
        /// <param name="email"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static bool ValidateEmail(string email, int uid)
        {
            //int num = BBX.Data.Users.FindUserEmail(email);
            //return GeneralConfigInfo.Current.Doublee != 0 || num == -1 || uid == num;

            //if (GeneralConfigInfo.Current.Doublee != 0) return true;

            var user = User.FindByEmail(email);
            if (user == null) return true;

            return user.ID == uid;
        }

        //public static int GetUserCountByAdmin(string orderby)
        //{
        //    if (orderby == "admin")
        //    {
        //        return BBX.Data.Users.GetUserCountByAdmin();
        //    }
        //    //return Users.GetUserCount("");
        //    return User.Meta.Count;
        //}

        public static void UpdateAuthStr(int uid, string authstr, int authflag)
        {
            //BBX.Data.Users.UpdateAuthStr(uid, authstr, authflag);
            var user = User.FindByID(uid);
            var uf = user.Field;
            uf.Authstr = authstr;
            uf.AuthTime = DateTime.Now;
            uf.Authflag = (SByte)authflag;
            uf.Save();
        }

        //public static bool UpdateUserForumSetting(UserInfo userinfo)
        //{
        //	if (User.FindByID(userinfo.Uid) == null)
        //	{
        //		return false;
        //	}
        //	BBX.Data.Users.UpdateUserForumSetting(userinfo);
        //	return true;
        //}

        //public static void UpdateUserExtCredits(int uid, int extid, float pos)
        //{
        //    BBX.Data.Users.UpdateUserExtCredits(uid, extid, pos);
        //}

        public static float GetUserExtCredits(int uid, int extid)
        {
            //return BBX.Data.Users.GetUserExtCredits(uid, extid);
            var user = User.FindByID(uid);
            if (user == null) return 0;

            return (float)user["ExtCredits" + extid];
        }

        //public static bool UpdateUserPreference(int uid, string avatar, int avatarwidth, int avatarheight, int templateid)
        //{
        //	if (User.FindByID(uid) == null)
        //	{
        //		return false;
        //	}
        //	BBX.Data.Users.UpdateUserPreference(uid, avatar, avatarwidth, avatarheight, templateid);
        //	return true;
        //}

        public static bool UpdateUserPassword(int uid, string password, bool originalpassword)
        {
            var user = User.FindByID(uid);
            if (user == null) return false;

            //BBX.Data.Users.UpdateUserPassword(uid, password, originalpassword);

            if (originalpassword) password = password.MD5();

            user.Password = password;
            user.Save();

            return true;
        }

        public static bool UpdateUserSecques(int uid, int questionid, string answer)
        {
            var user = User.FindByID(uid);
            if (user == null) return false;

            //BBX.Data.Users.UpdateUserSecques(uid, ForumUtils.GetUserSecques(questionid, answer));
            user.Secques = ForumUtils.GetUserSecques(questionid, answer);
            user.Update();

            return true;
        }

        public static void UpdateUserCreditsAndVisit(IUser user, string ip)
        {
            CreditsFacade.UpdateUserCredits(user.ID);
            //BBX.Data.Users.UpdateUserLastvisit(uid, ip);

            user.LastIP = ip;
            user.LastVisit = DateTime.Now;
            (user as IEntity).Save();
        }

        public static void UpdateMedals(int uid, string medals, int adminUid, string adminUserName, string ip, string reason)
        {
            if (uid <= 0) return;

            //BBX.Data.Users.UpdateMedals(uid, medals);
            var user = User.FindByID(uid);
            var uf = user as IUser;
            uf.Medals = medals;
            user.Save();

            //string username = Users.GetUserInfo(uid).Name;
            string[] array = medals.Split(',');
            for (int i = 0; i < array.Length; i++)
            {
                string text = array[i];
                if (text != "")
                {
                    Medal.Award(uid, text.ToInt(), adminUid, ip, reason);

                    //if (!BBX.Data.Medals.IsExistMedalAwardRecord(int.Parse(text), uid))
                    //{
                    //    BBX.Data.Medals.CreateMedalslog(adminUid, adminUserName, ip, username, uid, "授予", int.Parse(text), reason);
                    //}
                    //else
                    //{
                    //    BBX.Data.Medals.UpdateMedalslog("授予", DateTime.Now, reason, "收回", int.Parse(text), uid);
                    //}
                }
            }
        }

        public static int UpdateUserNewPMCount(int uid, int olid)
        {
            //int newPMCount = BBX.Data.PrivateMessages.GetNewPMCount(uid);
            var newPMCount = ShortMessage.GetNewPMCount(uid);
            //OnlineUsers.UpdateNewPms(olid, newPMCount);
            var online = Online.FindByID(olid);
            if (online != null) online.UpdateNewPms(newPMCount);
            //return BBX.Data.Users.SetUserNewPMCount(uid, newPMCount);
            var user = User.FindByID(uid);
            if (user != null)
            {
                user.NewpmCount = newPMCount;
                user.Update();
            }
            return 1;
        }

        //public static bool UpdateUserSpaceId(int spaceid, int userid)
        //{
        //    if (User.FindByID(userid) == null)
        //    {
        //        return false;
        //    }
        //    BBX.Data.Users.UpdateUserSpaceId(spaceid, userid);
        //    return true;
        //}

        //public static void UpdateUserGroup(int uid, int groupId)
        //{
        //	Users.UpdateUserGroup(uid.ToString(), groupId);
        //}

        //public static void UpdateUserGroup(string uidList, int groupId)
        //{
        //	BBX.Data.Users.UpdateUserGroup(uidList, groupId);
        //}

        public static Hashtable GetReportUsers()
        {
            var cacheService = XCache.Current;
            var ht = cacheService.RetrieveObject("/Forum/ReportUsers") as Hashtable;
            if (ht == null)
            {
                ht = new Hashtable();
                string reportusergroup = GeneralConfigInfo.Current.Reportusergroup;
                if (!Utils.IsNumericList(reportusergroup))
                {
                    return ht;
                }
                //DataTable users = BBX.Data.Users.GetUsers(reportusergroup);
                //for (int i = 0; i < users.Rows.Count; i++)
                //{
                //    ht[users.Rows[i]["uid"]] = users.Rows[i]["username"];
                //}
                var list = User.FindAllByGroupID(reportusergroup.ToInt());
                foreach (var item in list)
                {
                    ht[item.ID] = item.Name;
                }
                XCache.Add("/Forum/ReportUsers", ht);
            }
            return ht;
        }

        //public static int GetUserIdByRewriteName(string rewritename)
        //{
        //	return BBX.Data.Users.GetUserIdByRewriteName(rewritename);
        //}

        //public static void UpdateUserPMSetting(UserInfo user)
        //{
        //    BBX.Data.Users.UpdateUserPMSetting(user);
        //}

        public static void UpdateBanUser(int groupid, string groupexpiry, int uid)
        {
            //BBX.Data.Users.UpdateBanUser(groupid, groupexpiry, uid);
            var user = User.FindByID(uid);
            if (user != null)
            {
                user.GroupID = groupid;
                user.GroupExpiry = groupexpiry.ToInt();
                user.Save();
            }
            var online = Online.FindByUserID(uid);
            if (online != null)
            {
                online.GroupID = groupid;
                online.Update();
            }

            var notice = new Notice();
            notice.New = 1;
            if (groupid == 4) notice.Type = (Int32)NoticeType.BanPostNotice;
            if (groupid == 5) notice.Type = (Int32)NoticeType.BanVisitNotice;

            notice.PostDateTime = DateTime.Now;
            notice.Poster = DNTRequest.GetFormString("uname");
            notice.PosterID = uid;
            notice.Uid = uid;
            notice.Note = DNTRequest.GetFormString("reason") + "截至到" + groupexpiry + "到期";
            //Notices.CreateNoticeInfo(noticeInfo);
            notice.Insert();
        }

        //public static string SearchSpecialUser(int fid)
        //{
        //    DataTable dataTable = BBX.Data.Users.SearchSpecialUser(fid);
        //    if (dataTable.Rows.Count <= 0)
        //    {
        //        return null;
        //    }
        //    return dataTable.Rows[0]["permuserlist"].ToString();
        //}

        //public static void UpdateSpecialUser(string permuserlist, int fid)
        //{
        //    BBX.Data.Users.UpdateSpecialUser(permuserlist, fid);
        //}

        //public static int GetUserExtCreditsByUserid(int uid, int extnumber)
        //{
        //    if (extnumber > 0 && extnumber <= 8)
        //    {
        //        return BBX.Data.Users.GetUserExtCreditsByUserid(uid, extnumber);
        //    }
        //    return 0;
        //}

        public static void UpdateUserAdminIdByGroupId(int adminId, int groupId)
        {
            //BBX.Data.Users.UpdateUserAdminIdByGroupId(adminId, groupId);

            var list = User.FindAllByGroupID(groupId);
            list.ForEach(e => e.AdminID = adminId);
            list.Save();
        }

        public static void UpdateUserToStopTalkGroup(string uidList)
        {
            //BBX.Data.Users.UpdateUserToStopTalkGroup(uidList);
            var list = User.FindAllByIDs(uidList);
            list.ForEach(e =>
            {
                e.GroupID = 4;
                e.AdminID = 0;
                e.GroupExpiry = 0;
            });
            list.Save();
        }

        public static void UpdateEmailValidateInfo(string authstr, DateTime authTime, int uid)
        {
            //BBX.Data.Users.UpdateEmailValidateInfo(authstr, authTime, uid);

            var user = User.FindByID(uid);
            var uf = user.Field;
            uf.Authstr = authstr;
            uf.AuthTime = authTime;
            uf.Authflag = 1;
            uf.Save();
        }

        public static int UpdateUserCredits(string credits, int startuid)
        {
            //return BBX.Data.Users.UpdateUserCredits(credits, startuid);

            var cr = credits.ToInt();
            var list = User.FindAll(User._.ID > startuid, User._.ID.Asc(), null, 0, 100);
            list.ForEach(e => e.Credits = cr);
            return list.Save();
        }

        //public static DataTable GetUserListByGroupidList(string groupIdList)
        //{
        //    return BBX.Data.Users.GetUserListByGroupid(groupIdList);
        //}

        public static int SendPMByGroupidList(string groupidlist, int topnumber, ref int start_uid, string msgfrom, int msguid, int folder, string subject, string postdatetime, string message)
        {
            //DataTable userListByGroupid = BBX.Data.Users.GetUserListByGroupid(groupidlist, topnumber, start_uid);
            //foreach (DataRow dataRow in userListByGroupid.Rows)
            var list = User.FindAllByGroupID(groupidlist, start_uid, topnumber);
            foreach (var item in list)
            {
                var msg = new ShortMessage();
                msg.Msgfrom = msgfrom.Replace("'", "''");
                msg.MsgfromID = msguid;
                msg.Msgto = item.Name.Replace("'", "''");
                msg.MsgtoID = item.ID;
                msg.Folder = (Int16)folder;
                msg.Subject = subject;
                msg.PostDateTime = postdatetime.ToDateTime();
                msg.Message = message;
                msg.Create(false);
                start_uid = msg.MsgtoID;

                var entity = Online.FindByUserID(msg.MsgtoID);
                //if (OnlineUsers.GetOlidByUid(msg.MsgtoID) > 0)
                if (entity != null)
                {
                    Users.UpdateUserNewPMCount(msg.MsgtoID, entity.ID);
                }
            }
            return list.Count;
        }

        public static int SendEmailByGroupidList(string groupidlist, int topnumber, ref int start_uid, string subject, string body)
        {
            //DataTable userListByGroupid = BBX.Data.Users.GetUserListByGroupid(groupidlist, topnumber, start_uid);
            //foreach (DataRow dataRow in userListByGroupid.Rows)
            var list = User.FindAllByGroupID(groupidlist, start_uid, topnumber);
            foreach (var item in list)
            {
                if (!item.Email.IsNullOrWhiteSpace())
                {
                    //EmailMultiThread em = new EmailMultiThread(dataRow["UserName"].ToString().Trim(), dataRow["Email"].ToString().Trim(), subject, body);
                    //new Thread(new ThreadStart(em.Send)).Start();
                    Emails.SendAsync(item.Name, item.Email, subject, body);
                }
                start_uid = item.ID;
            }
            return list.Count;
        }

        //public static DataTable GetUserListByGroupid(int groupId)
        //{
        //    return Users.GetUserListByGroupidList(groupId.ToString());
        //}

        //public static DataTable GetEmailListByUserNameList(string userNameList)
        //{
        //    return BBX.Data.Users.GetEmailListByUserNameList(userNameList);
        //}

        //public static DataTable GetEmailListByGroupidList(string groupidList)
        //{
        //    return BBX.Data.Users.GetEmailListByGroupidList(groupidList);
        //}

        //public static void UpdateUserGroupByUidList(int groupid, string uidList)
        //{
        //	BBX.Data.Users.UpdateUserGroupByUidList(groupid, uidList);
        //}

        //public static void DeleteUsers(string uidList)
        //{
        //	if (!String.IsNullOrEmpty(uidList))
        //	{
        //		BBX.Data.Users.DeleteUsers(uidList);
        //	}
        //}

        //public static void ClearUsersAuthstr(string uidList)
        //{
        //    if (!String.IsNullOrEmpty(uidList))
        //    {
        //        BBX.Data.Users.ClearUsersAuthstr(uidList);
        //    }

        //}

        //public static void ClearUsersAuthstrByUncheckedUserGroup()
        //{
        //    Users.ClearUsersAuthstr(Users.GetUidListByUserGroupId(8));
        //}

        //private static string GetUidListByUserGroupId(int userGroupId)
        //{
        //    string text = "";
        //    foreach (DataRow dataRow in Users.GetUserListByGroupid(userGroupId).Rows)
        //    {
        //        text = text + dataRow["uid"].ToString() + ",";
        //    }
        //    return text.TrimEnd(',');
        //}

        //public static void DeleteAuditUser()
        //{
        //    Users.DeleteUsers(Users.GetUidListByUserGroupId(8));
        //}

        //public static DataTable AuditNewUserClear(string searchUserName, string regBefore, string regIp)
        //{
        //    return BBX.Data.Users.AuditNewUserClear(searchUserName, regBefore, regIp);
        //}

        public static void SendEmailForAccountCreateSucceed(string uidList)
        {
            //foreach (DataRow dataRow in BBX.Data.Users.GetUsersByUidlLst(uidList).Rows)
            //{
            //    Emails.SendRegMail(dataRow["username"].ToString().Trim(), dataRow["email"].ToString().Trim(), "");
            //}
            foreach (var item in User.FindAllByIDs(uidList))
            {
                if (!item.Email.IsNullOrWhiteSpace())
                    Emails.SendRegMail(item.Name, item.Email, "");
            }
        }

        public static string GetModerators(int fid)
        {
            //string text = "";
            //foreach (DataRow dataRow in BBX.Data.Users.GetModerators(fid).Rows)
            //{
            //	text = text + dataRow["username"].ToString().Trim() + ",";
            //}
            //return text.TrimEnd(',');

            var list = Moderator.FindAllByFid(fid);
            var sb = new StringBuilder();
            foreach (var item in list)
            {
                if (item.Inherited != 1) continue;

                var user = User.FindByID(item.Uid);
                if (sb.Length > 0) sb.Append(",");
                sb.Append(user.Name);
            }
            return sb.ToString();
        }

        public static string GetSearchUserList(string userNameList)
        {
            var sb = new StringBuilder();
            //IDataReader userListByUserName = BBX.Data.Users.GetUserListByUserName(userNameList);
            var list = User.FindAllByNames(userNameList.Split(","));
            int num = 0;
            bool flag = false;
            sb.Append("<table width=\"100%\" style=\"align:center\"><tr>");
            //while (userListByUserName.Read())
            foreach (var user in list)
            {
                flag = true;
                if (num >= 3)
                {
                    num = 0;
                    sb.Append("</tr><tr>");
                }
                num++;
                sb.Append("<td width=\"33%\" style=\"align:left\"><a href=\"#\" onclick=\"javascript:resetindexmenu('7','3','7,8','user/edituser.aspx?uid=" + user.ID + "');\">" + user.Name + "</a></td>");
            }
            //userListByUserName.Close();
            if (!flag)
            {
                sb.Append("没有找到相匹配的结果");
            }
            sb.Append("</tr></table>");
            return sb.ToString();
        }

        //public static string GetUsersSearchCondition(bool isLike, bool isPostDateTime, string userName, string nickName, string userGroup, string email, string credits_Start, string credits_End, string lastIp, string posts, string digestPosts, string uid, string joindateStart, string joindateEnd)
        //{
        //    return BBX.Data.Users.GetUsersSearchCondition(isLike, isPostDateTime, userName, nickName, userGroup, email, credits_Start, credits_End, lastIp, posts, digestPosts, uid, joindateStart, joindateEnd);
        //}

        public static void SendEmailForUncheckedUserGroup()
        {
            //foreach (DataRow dataRow in Users.GetUserListByGroupid(8).Rows)
            foreach (var user in User.FindAllByGroupID(8))
            {
                Emails.SendRegMail(user.Name, user.Email, user.Password);
            }
        }

        public static bool ResetPassword(User userInfo)
        {
            //if (GeneralConfigInfo.Current.Passwordmode > 1 && PasswordModeProvider.GetInstance() != null)
            //{
            //    //PasswordModeProvider.GetInstance().SaveUserInfo(userInfo);
            //    throw new NotImplementedException();
            //}
            //else
            {
                userInfo.Password = Utils.MD5(userInfo.Password);
                //Users.UpdateUser(userInfo);
                userInfo.Save();
            }
            return true;
        }

        public static VerifyReg CreateVerifyRegisterInfo(string email, string inviteCode)
        {
            if (!Utils.IsValidEmail(email)) return null;

            var vi = new VerifyReg();
            vi.IP = WebHelper.UserHost;
            vi.Email = email;
            vi.CreateTime = DateTime.Now;
            vi.ExpireTime = DateTime.Now.AddDays(GeneralConfigInfo.Current.Verifyregisterexpired);
            vi.InviteCode = inviteCode;
            vi.VerifyCode = ForumUtils.CreateAuthStr(16);
            //if (BBX.Data.Users.CreateVerifyRegisterInfo(vi) <= 0)
            //{
            //    return null;
            //}
            return vi;
        }

        //public static VerifyRegisterInfo GetVerifyRegisterInfo(string verifyCode)
        //{
        //    if (verifyCode.Length != 16)
        //    {
        //        return null;
        //    }
        //    return BBX.Data.Users.GetVerifyRegisterInfo(verifyCode, "code");
        //}

        //public static int DeleteVerifyRegisterInfo(int regId)
        //{
        //    return BBX.Data.Users.DeleteVerifyRegisterInfo(regId);
        //}

        //public static VerifyRegisterInfo GetVerifyRegisterInfoByIp(string ip)
        //{
        //    return BBX.Data.Users.GetVerifyRegisterInfo(ip, "ip");
        //}

        //public static VerifyRegisterInfo GetVerifyRegisterInfoByEmail(string email)
        //{
        //    return BBX.Data.Users.GetVerifyRegisterInfo(email, "email");
        //}

        public static bool PageValidateUserName(string userName, out string errorMessage, Boolean checkExists = true)
        {
            if (string.IsNullOrEmpty(userName))
            {
                errorMessage = "用户名不能为空";
                return false;
            }
            if (Utils.GetStringLength(userName) > 20)
            {
                errorMessage = "用户名不得超过20个字符";
                return false;
            }
            if (Utils.GetStringLength(userName) < 3)
            {
                errorMessage = "用户名不得小于3个字符";
                return false;
            }
            if (userName.IndexOf("\u3000") != -1 || userName.IndexOf("") != -1 || userName.IndexOf("") != -1 || userName.IndexOf("") != -1 || userName.IndexOf("") != -1 || userName.IndexOf("") != -1 || userName.IndexOf("") != -1 || userName.IndexOf("") != -1 || userName.IndexOf("") != -1 || userName.IndexOf("") != -1 || userName.IndexOf("") != -1)
            {
                errorMessage = "用户名中不允许包含全角空格符";
                return false;
            }
            if (userName.IndexOf(" ") != -1)
            {
                errorMessage = "用户名中不允许包含空格";
                return false;
            }
            if (userName.IndexOf(":") != -1)
            {
                errorMessage = "用户名中不允许包含冒号";
                return false;
            }
            if (!Utils.IsSafeSqlString(userName) || !Utils.IsSafeUserInfoString(userName))
            {
                errorMessage = "用户名中存在非法字符";
                return false;
            }
            if (userName.Trim() == "系统" || ForumUtils.IsBanUsername(userName, GeneralConfigInfo.Current.Censoruser))
            {
                errorMessage = "用户名 \"" + userName + "\" 不允许在本论坛使用";
                return false;
            }
            if (checkExists && Users.GetUserId(userName) > 0)
            {
                errorMessage = "该用户名已存在";
                return false;
            }
            errorMessage = "";
            return true;
        }

        public static bool PageValidateEmail(string email, bool checkVerifyRegister, out string errorMessage)
        {
            if (string.IsNullOrEmpty(email))
            {
                errorMessage = "Email不能为空";
                return false;
            }
            if (!Utils.IsValidEmail(email))
            {
                errorMessage = "Email格式不正确";
                return false;
            }
            if (!Users.ValidateEmail(email))
            {
                errorMessage = "Email: \"" + email + "\" 已经被其它用户注册使用";
                return false;
            }
            if (checkVerifyRegister)
            {
                //var vi = Users.GetVerifyRegisterInfoByEmail(email);
                var vi = VerifyReg.FindByEmail(email);
                if (vi != null && vi.CreateTime > DateTime.Now.AddMinutes(-5.0))
                {
                    errorMessage = "Email:\"" + email + "\" 在五分钟内已经发送过一次注册请求,请耐心等待";
                    return false;
                }
            }
            var config = GeneralConfigInfo.Current;
            string emailHostName = Utils.GetEmailHostName(email);
            if (!config.Accessemail.IsNullOrEmpty() && !Utils.InArray(emailHostName, config.Accessemail.Replace("\r\n", "\n"), "\n"))
            {
                errorMessage = "Email: \"" + email + "\" 不在本论坛允许范围之类, 本论坛只允许用户使用这些Email地址注册: " + config.Accessemail.Replace("\n", ",").Replace("\r", "");
                return false;
            }
            if (!config.Censoremail.IsNullOrEmpty() && Utils.InArray(emailHostName, config.Censoremail.Replace("\r\n", "\n"), "\n"))
            {
                errorMessage = "Email: \"" + email + "\" 不允许在本论坛使用, 本论坛不允许用户使用的Email地址包括: " + config.Censoremail.Replace("\n", ",").Replace("\r", "");
                return false;
            }
            errorMessage = "";
            return true;
        }
    }
}