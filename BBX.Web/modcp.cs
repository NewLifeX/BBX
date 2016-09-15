using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using BBX.Cache;
using BBX.Common;
using BBX.Config;
using BBX.Entity;
using BBX.Forum;
using NewLife.Web;

namespace BBX.Web
{
    public class modcp : PageBase
    {
        public DataTable ModeratorLogs = new DataTable();
        public DataTable permuserlist = new DataTable();
        public List<Banned> showbannediplist = new List<Banned>();
        public List<IXForum> showforumlist = new List<IXForum>();
        public List<Announcement> announcementlist = new List<Announcement>();
        public List<IXForum> foruminfolist = new List<IXForum>();
        public DataTable moderatorLogs = new DataTable();
        public List<IXForum> forumslist = new List<IXForum>();
        public List<Topic> topiclist = new List<Topic>();
        public List<Post> postlist = new List<Post>();
        public IXForum foruminfo;
        //protected AdminUserGroups adminusergroup;
        public AdminGroup admingroupinfo;
        public string subject = "";
        public string displayorder = "";
        public string message = "";
        public string tip = "";
        public string forumname = "";
        public int mpp = 16;
        public int pageid = DNTRequest.GetInt("page", 1);
        public int pagecount;
        public int counts;
        public int id = -1;
        public int uid = -1;
        public int banuid = -1;
        public int groupid = -1;
        public int groupexpiry;
        public int forumid = DNTRequest.GetInt("forumid", 0);
        public int fid = DNTRequest.GetInt("forumid", 0);
        public string uname = "";
        public string location = "";
        public string bio = "";
        public string signature = "";
        public string grouptitle = "";
        public string curstatus = "";
        public string reason = "";
        public string operation = DNTRequest.GetString("operation");
        public string pagenumbers = "";
        public string starttime = Utils.GetDateTime();
        public string endtime = Utils.GetDateTime(1);
        public string op = DNTRequest.GetString("op");
        //public List<TableList> posttablelist = new List<TableList>();
        //public int tableid = DNTRequest.GetInt("tablelist", Utils.StrToInt(TableList.GetPostTableId(), 1));
        public string forumliststr = "";
        public bool banusersubmit;
        public bool editusersubmit;
        public bool editannouncement;
        public bool deleteannoucement;
        public string forumnav = "";
        public Post showtopicpagepostinfo;
        public bool ismoder;
        public int filter;
        public bool needshowlogin;
        public bool alloweditrules = true;
        public bool allowdeleteavatar;
        public string about = "";
        public int last;
        public int auditTopicCount;
        public int auditPostCount;

        protected override void ShowPage()
        {
            this.pagetitle = "管理面板";
            this.about = DNTRequest.GetString("about");
            //this.auditTopicCount = Topics.GetUnauditNewTopicCount(DNTRequest.GetString("forumid"), -2);
            this.auditTopicCount = Topic.GetUnauditNewTopicCount(new Int32[] { DNTRequest.GetInt("forumid") }, -2);
            this.auditPostCount = Posts.GetUnauditNewPostCount(DNTRequest.GetString("forumid"), 1);

            if (this.useradminid < 1 || this.useradminid > 3)
            {
                base.AddErrLine(string.Format("您当前的身份 \"{0}\" 没有管理权限", this.usergroupinfo.GroupTitle));
                return;
            }
            if (Utils.StrIsNullOrEmpty(Utils.GetCookie("cplogincookie")))
            {
                if (this.operation != "login")
                {
                    Utils.WriteCookie("reurl", Request.RawUrl);
                    this.Context.Response.Redirect(BaseConfigs.GetForumPath + "modcp.aspx?operation=login&forumid=" + this.forumid);
                    return;
                }
                this.needshowlogin = true;
            }
            Utils.WriteCookie("cplogincookie", Utils.GetCookie("cplogincookie"), 20);
            this.ismoder = Moderators.IsModer(this.useradminid, this.userid, this.forumid);
            //this.admingroupinfo = AdminUserGroups.AdminGetAdminGroupInfo(this.usergroupid);
            admingroupinfo = AdminGroup.FindByID(usergroupid);
            if (this.admingroupinfo == null)
            {
                base.AddErrLine("您所在的管理组不存在");
                return;
            }
            if (this.admingroupinfo.AllowPostannounce && Utils.InArray(this.operation.ToLower(), "addannouncements,list,manage,add,editannouncements,updateannouncements"))
            {
                string a;
                if ((a = this.operation.ToLower()) != null)
                {
                    if (a == "addannouncements")
                    {
                        this.AddAnnouncements();
                        return;
                    }
                    if (a == "list")
                    {
                        this.ShowAnnouncements();
                        return;
                    }
                    if (a == "manage")
                    {
                        this.ManageAnnouncements();
                        return;
                    }
                    if (a == "add")
                    {
                        this.AddAnnouncements();
                        return;
                    }
                    if (a == "editannouncements")
                    {
                        this.EditAnnouncements();
                        return;
                    }
                    if (!(a == "updateannouncements"))
                    {
                        return;
                    }
                    this.UpdateAnnouncements();
                }
                return;
            }
            string key;
            switch (key = this.operation.ToLower())
            {
                case "edituser":
                    if (admingroupinfo.AllowEditUser)
                    {
                        this.EditUser();
                        return;
                    }
                    break;
                case "updateuser":
                    if (admingroupinfo.AllowEditUser)
                    {
                        this.UpdateUser();
                        return;
                    }
                    break;
                case "banusersearch":
                    if (this.admingroupinfo.AllowBanUser)
                    {
                        this.BanUserSearch();
                        return;
                    }
                    break;
                case "banuser":
                    if (this.admingroupinfo.AllowBanUser)
                    {
                        this.UpdateBanUser();
                        return;
                    }
                    break;
                case "ipban":
                    if (this.admingroupinfo.AllowBanIP)
                    {
                        string text = DNTRequest.GetInt("ip1new", 0) + "." + DNTRequest.GetInt("ip2new", 0) + "." + DNTRequest.GetInt("ip3new", 0) + "." + DNTRequest.GetInt("ip4new", 0);
                        if (text == "0.0.0.0" && Utils.StrIsNullOrEmpty(DNTRequest.GetFormString("chkbanip")))
                        {
                            this.EditBanIp();
                            return;
                        }
                        if (!this.VertifyIp(text))
                        {
                            return;
                        }
                        this.BanIp(text);
                        this.DelBanIp();
                        return;
                    }
                    break;
                case "showbannedlist":
                    this.ShowBannedList();
                    return;
                case "forumaccesslist":
                    this.SetDropdownOptions();
                    this.SearchForumSpecialUser();
                    if (DNTRequest.GetString("op") == "access_successful")
                    {
                        this.tip = "access_successful";
                        return;
                    }
                    break;
                case "forumaccessupdate":
                    this.UpdatePermuserListUser();
                    return;
                case "editforum":
                    this.SetDropdownOptions();
                    this.GetForumInfo();
                    return;
                case "updateforum":
                    this.UpdateForum();
                    return;
                case "audittopic":
                    if (this.admingroupinfo.AllowModPost)
                    {
                        this.SetDropdownOptions();
                        //this.posttablelist = TableList.GetAllPostTable();
                        this.GetTopicList();
                        this.AuditNewTopic();
                        return;
                    }
                    break;
                case "auditpost":
                    if (this.admingroupinfo.AllowModPost)
                    {
                        this.SetDropdownOptions();
                        //this.posttablelist = TableList.GetAllPostTable();
                        this.AuditPost();
                        this.GetPostList();
                        return;
                    }
                    break;
                case "attention":
                    this.SetDropdownOptions();
                    this.GetAttentionTopics();
                    return;
                case "userout":
                    this.UserOut();
                    return;
                case "login":
                    this.Login();
                    return;
                case "logs":
                    this.GetLogs();
                    return;
                case "deleteuserpost":
                    this.DelUserPost();
                    break;

                //return;
            }
        }

        private void Login()
        {
            if (!this.needshowlogin)
            {
                this.Context.Response.Redirect(BaseConfigs.GetForumPath + "modcp.aspx?operation=attention&forumid=87&forumid=" + this.forumid);
                return;
            }
            string cpname = DNTRequest.GetFormString("cpname");
            string cppwd = DNTRequest.GetFormString("cppwd");
            if (Utils.StrIsNullOrEmpty(cpname) || Utils.StrIsNullOrEmpty(cppwd)) return;

            var userInfo = BBX.Entity.User.Login(cpname, cppwd);
            if (userInfo == null)
            {
                base.AddErrLine("用户名或密码错误");
                return;
            }

            if (userInfo.AdminID > 3 || userInfo.AdminID < 1)
            {
                base.AddErrLine("您当前的身份没有管理权限");
                return;
            }
            Utils.WriteCookie("cplogincookie", userInfo.Name, 20);
            this.Context.Response.Redirect(Utils.GetCookie("reurl"));
        }

        public void AddAnnouncements()
        {
            if (this.ispost)
            {
                string subject = DNTRequest.GetString("subject").Trim();
                string message = DNTRequest.GetString("message").Trim();
                if (Utils.StrIsNullOrEmpty(subject) || Utils.StrIsNullOrEmpty(message))
                {
                    base.AddErrLine("主题或内容不能为空");
                    return;
                }
                DateTime starttime;
                DateTime.TryParse(DNTRequest.GetString("starttime"), out starttime);
                DateTime endtime;
                DateTime.TryParse(DNTRequest.GetString("endtime"), out endtime);
                if (starttime >= endtime)
                {
                    base.AddErrLine("开始日期或结束日期非法,或者是开始日期与结束日期倒置");
                    return;
                }
                //Announcements.CreateAnnouncement(this.username, this.userid, subject, 0, starttime.ToString(), endtime.ToString(), message);
                Announcement.Create(userid, username, subject, message, starttime, endtime);
                XCache.Remove(CacheKeys.FORUM_ANNOUNCEMENT_LIST);
                XCache.Remove(CacheKeys.FORUM_SIMPLIFIED_ANNOUNCEMENT_LIST);
                AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.GetGroupTile(), WebHelper.UserHost, "添加公告", "添加公告");
                //this.nowdatetime = DateTime.Now.ToShortDateString();
                this.Context.Response.Redirect(BaseConfigs.GetForumPath + "modcp.aspx?operation=list&op=add&forumid=" + DNTRequest.GetFormInt("fid", 0));
            }
        }

        public void ShowAnnouncements()
        {
            if (Utils.InArray(DNTRequest.GetString("op"), "add,delsuccessful"))
            {
                this.tip = DNTRequest.GetString("op");
            }
            //this.counts = Announcements.GetAnnouncementList().Rows.Count;
            counts = Announcement.Meta.Count;
            this.pagecount = ((this.counts % 5 == 0) ? (this.counts / 5) : (this.counts / 5 + 1));
            this.pagecount = ((this.pagecount == 0) ? 1 : this.pagecount);
            this.pageid = ((DNTRequest.GetInt("page", 1) > this.pagecount) ? this.pagecount : DNTRequest.GetInt("page", 1));
            //this.announcementlist = Announcements.GetAnnouncementList(5, this.pageid);
            announcementlist = Announcement.FindAll(null, null, null, (pageid - 1) * 5, 5);
            this.pagenumbers = Utils.GetPageNumbers(this.pageid, this.pagecount, "modcp.aspx?operation=list", 5);
        }

        public void ManageAnnouncements()
        {
            this.UpdateDisplayOrder();
            if (!Utils.StrIsNullOrEmpty(DNTRequest.GetFormString("aidlist")))
            {
                this.DeleteAnnouncements(DNTRequest.GetFormString("aidlist"));
            }
        }

        public void UpdateDisplayOrder()
        {
            var ids = DNTRequest.GetFormString("hid");
            this.displayorder = DNTRequest.GetFormString("displayorder");
            //this.announcementlist = Announcement.FindAllWithCache();
            if (this.announcementlist.Count == 0)
            {
                base.AddErrLine("当前没有任何公告");
                return;
            }
            //Announcements.UpdateAnnouncementDisplayOrder(this.displayorder, hid, this.userid, this.useradminid);
            Announcement.UpdateDisplayOrder(ids, displayorder);
        }

        public void DeleteAnnouncements(string aidlist)
        {
            if (aidlist != "")
            {
                this.DelAnnouncementOperation(aidlist);
                this.tip = "delsuccessful";
                AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.GetGroupTile(), WebHelper.UserHost, "删除公告", "删除公告：" + aidlist);
                this.Context.Response.Redirect(BaseConfigs.GetForumPath + "modcp.aspx?operation=list&op=delsuccessful&forumid=" + DNTRequest.GetFormInt("fid", 0));
            }
        }

        private string GetGroupTile()
        {
            var userGroupInfo = UserGroup.FindByID(BBX.Entity.User.FindByID(this.userid).GroupID);
            if (userGroupInfo != null)
            {
                return userGroupInfo.GroupTitle;
            }
            return "";
        }

        private void DelAnnouncementOperation(string aidlist)
        {
            //Announcements.DeleteAnnouncements(aidlist);
            Announcement.DeleteList(aidlist);
            AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.GetGroupTile(), WebHelper.UserHost, "删除公告", "删除公告,公告ID为: " + DNTRequest.GetString("id"));
            XCache.Remove(CacheKeys.FORUM_ANNOUNCEMENT_LIST);
            XCache.Remove(CacheKeys.FORUM_SIMPLIFIED_ANNOUNCEMENT_LIST);
        }

        public void EditAnnouncements()
        {
            this.id = DNTRequest.GetInt("id", 0);
            if (!this.ispost && id != 0)
            {
                //AnnouncementInfo announcement = Announcements.GetAnnouncement(DNTRequest.GetInt("id", 0));
                var announcement = Announcement.FindByID(id);
                //this.id = DNTRequest.GetInt("id", 0);
                if (this.useradminid == 1 || (announcement.PosterID == this.userid && announcement.PosterID > 0))
                {
                    this.GetOneAnnouncement(announcement);
                    return;
                }
                this.editannouncement = true;
                base.AddMsgLine("该公告已经删除或您没有权利编辑它，请返回");
                base.SetUrl("modcp.aspx?operation=list");
                base.SetMetaRefresh();
            }
        }

        private void GetOneAnnouncement(Announcement announcementInfo)
        {
            this.subject = announcementInfo.Title;
            this.displayorder = announcementInfo.DisplayOrder.ToString();
            this.starttime = announcementInfo.StartTime.ToString();
            this.endtime = announcementInfo.EndTime.ToString();
            this.message = announcementInfo.Message;
        }

        public void UpdateAnnouncements()
        {
            //int num = TypeConverter.StrToInt(DNTRequest.GetFormString("id"));
            var aid = DNTRequest.GetInt("id", 0);
            if (aid != 0)
            {
                //int posterid = Announcements.GetAnnouncement(TypeConverter.StrToInt(DNTRequest.GetString("id"))).Posterid;
                var an = Announcement.FindByID(aid);
                if ((this.useradminid == 1 || (an.PosterID == this.userid && an.PosterID > 0)) && !this.UpdateAnnouncementOperation(an))
                {
                    XCache.Remove(CacheKeys.FORUM_ANNOUNCEMENT_LIST);
                    XCache.Remove(CacheKeys.FORUM_SIMPLIFIED_ANNOUNCEMENT_LIST);
                    return;
                }
            }
            this.Context.Response.Redirect(BaseConfigs.GetForumPath + "modcp.aspx?operation=list");
        }

        private bool UpdateAnnouncementOperation(Announcement an)
        {
            string subject = DNTRequest.GetString("subject").Trim();
            string message = DNTRequest.GetString("message").Trim();
            if (Utils.StrIsNullOrEmpty(subject) || Utils.StrIsNullOrEmpty(message))
            {
                base.AddErrLine("主题或内容不能为空");
                return false;
            }
            DateTime starttime;
            DateTime.TryParse(DNTRequest.GetString("starttime"), out starttime);
            DateTime endtime;
            DateTime.TryParse(DNTRequest.GetString("endtime"), out endtime);
            if (starttime >= endtime)
            {
                base.AddErrLine("开始日期或结束日期非法,或者是开始日期与结束日期倒置");
                return false;
            }
            //Announcements.UpdateAnnouncement(new AnnouncementInfo
            //{
            //    Id = id,
            //    Poster = this.username,
            //    Title = subject,
            //    Displayorder = TypeConverter.StrToInt(DNTRequest.GetString("displayorder")),
            //    Starttime = starttime,
            //    Endtime = endtime,
            //    Message = message
            //});
            an.ID = id;
            an.Poster = username;
            an.Title = subject;
            an.Message = message;
            an.DisplayOrder = DNTRequest.GetInt("displayorder");
            an.StartTime = starttime;
            an.EndTime = endtime;
            an.Update();

            AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.GetGroupTile(), WebHelper.UserHost, "更新公告", "更新公告,标题为:" + DNTRequest.GetString("title"));
            return true;
        }

        public void ShowBannedList()
        {
            //int num = 0;
            this.pageid = DNTRequest.GetInt("page", 1);
            //this.showbannediplist = Ips.GetBannedIpList(5, this.pageid, out num);
            showbannediplist = Banned.FindAll(null, null, null, (pageid - 1) * 5, 5);
            this.counts = Banned.Meta.Count;
            this.pagecount = ((this.counts % 5 == 0) ? (this.counts / 5) : (this.counts / 5 + 1));
            this.pagecount = ((this.pagecount == 0) ? 1 : this.pagecount);
            this.pageid = ((this.pageid > this.pagecount) ? this.pagecount : this.pageid);
            this.pagenumbers = Utils.GetPageNumbers(this.pageid, this.pagecount, "modcp.aspx?operation=showbannedlist&forumid=" + this.forumid, 5);
        }

        public void DelBanIp()
        {
            if (Utils.StrIsNullOrEmpty(DNTRequest.GetFormString("chkbanip")))
            {
                base.AddErrLine("请选择要删除的IP");
                return;
            }
            //Ips.DelBanIp(DNTRequest.GetFormString("chkbanip"));
            Int32 banip = WebHelper.RequestInt("chkbanip");
            Banned ban = Banned.FindByID((Int16)banip);
            if (ban != null) ban.Delete();
            AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.GetGroupTile(), WebHelper.UserHost, "删除被禁止访问的ip", "");
        }

        private void EditBanIp()
        {
            string[] array = DNTRequest.GetFormString("expiration").Split(',');
            string[] array2 = DNTRequest.GetFormString("hiddenexpiration").Split(',');
            string[] hiddenid = DNTRequest.GetFormString("hiddenid").Split(',');
            if (array.Length != array2.Length) return;

            Banned.EditBanIp(array, array2, hiddenid, this.useradminid, this.userid);
            this.Context.Response.Redirect(BaseConfigs.GetForumPath + "modcp.aspx?operation=showbannedlist");
        }

        private bool VertifyIp(string ipkey)
        {
            Hashtable newHash = new Hashtable();
            if (!Utils.IsRuleTip(newHash, "ip", out ipkey))
            {
                base.AddErrLine("格式有错误");
                return false;
            }
            return true;
        }

        public void BanIp(string ipkey)
        {
            if (this.VertifyIp(ipkey))
            {
                //Ips.AddBannedIp(ipkey, (double)DNTRequest.GetFormInt("validitynew", 30), this.username);
                Banned.Add(ipkey, DNTRequest.GetFormInt("validitynew", 30), this.username);
                AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.GetGroupTile(), WebHelper.UserHost, "添加被禁止访问的ip", "");
            }
            this.Context.Response.Redirect(BaseConfigs.GetForumPath + "modcp.aspx?operation=showbannedlist&forumid=" + DNTRequest.GetFormInt("fid", 0));
        }

        public void EditUser()
        {
            this.uname = DNTRequest.GetFormString("username");
            User userInfo = (this.uname != "") ? Users.GetUserInfo(Users.GetUserId(this.uname)) : Users.GetUserInfo(DNTRequest.GetInt("uid", 0));
            if (userInfo == null)
            {
                return;
            }
            if (userInfo.AdminID >= 1 && userInfo.AdminID <= 3 && userInfo.AdminID <= this.useradminid)
            {
                base.AddErrLine("您无权编辑此用户信息");
                return;
            }
            this.uid = userInfo.ID;
            this.uname = userInfo.Name;
            this.location = userInfo.Field.Location;
            this.bio = userInfo.Field.Bio;
            this.signature = userInfo.Field.Signature;
            this.allowdeleteavatar = Avatars.ExistAvatar(this.uid.ToString());
        }

        public void UpdateUser()
        {
            this.editusersubmit = true;
            User userInfo = Users.GetUserInfo(DNTRequest.GetFormInt("uid", 0));
            if (userInfo.AdminID >= 1 && userInfo.AdminID <= 3 && userInfo.AdminID <= this.useradminid)
            {
                base.AddErrLine("您无权编辑此用户信息");
                return;
            }
            if (DNTRequest.GetString("bio").Length > 500)
            {
                base.AddErrLine("自我介绍不得超过500个字符");
                return;
            }
            if (DNTRequest.GetString("signature").Length > 500)
            {
                base.AddErrLine("签名不得超过500个字符");
                return;
            }

            var uf = userInfo.Field;
            uf.Location = Utils.HtmlEncode(DNTRequest.GetFormString("locationnew"));
            uf.Bio = Utils.HtmlEncode(DNTRequest.GetFormString("bionew"));
            if (uf.Signature != Utils.HtmlEncode(DNTRequest.GetFormString("signaturenew")))
            {
                uf.Signature = Utils.HtmlEncode(DNTRequest.GetFormString("signaturenew"));
                uf.Sightml = UBB.UBBToHTML(new PostpramsInfo
                {
                    Usergroupid = this.usergroupid,
                    Attachimgpost = this.config.Attachimgpost,
                    Showattachmentpath = this.config.Showattachmentpath,
                    Hide = 0,
                    Price = 0,
                    Sdetail = Utils.HtmlEncode(ForumUtils.BanWordFilter(DNTRequest.GetString("signaturenew"))),
                    Smileyoff = 1,
                    BBCode = this.usergroupinfo.AllowSigbbCode,
                    Parseurloff = 1,
                    Showimages = this.usergroupinfo.AllowSigimgCode ? 1 : 0,
                    Allowhtml = 0,
                    Signature = 1,
                    Smiliesinfo = Smilies.GetSmiliesListWithInfo(),
                    Customeditorbuttoninfo = null,
                    Smiliesmax = this.config.Smiliesmax,
                    //Signature = 1
                });
            }
            if (DNTRequest.GetString("delavatar") == "1")
            {
                Avatars.DeleteAvatar(userInfo.ID.ToString());
            }
            //if (!Users.UpdateUser(userInfo))
            if (userInfo.Save() < 1)
            {
                base.AddErrLine("用户未更新成功");
                return;
            }
            AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.GetGroupTile(), WebHelper.UserHost, "更新编辑用户", "");
            if (DNTRequest.GetFormString("operation") == "edituser")
            {
                this.op = "updateuser";
                base.SetUrl("modcp.aspx?operation=edituser&forumid=" + this.fid);
                base.MsgForward("modcp_succeed");
                base.AddMsgLine("用户资料成功更新");
                base.SetMetaRefresh();
            }
        }

        public void BanUserSearch()
        {
            this.uname = DNTRequest.GetFormString("username");
            this.uid = DNTRequest.GetFormInt("uid", 0);
            User userInfo = (this.uname != "") ? Users.GetUserInfo(Users.GetUserId(this.uname)) : Users.GetUserInfo(this.uid);
            if (userInfo == null)
            {
                return;
            }
            if (userInfo.AdminID >= 1 && userInfo.AdminID <= 3 && userInfo.AdminID <= this.useradminid)
            {
                base.AddErrLine("您无权编辑此用户信息");
                return;
            }
            this.uid = userInfo.ID;
            this.uname = userInfo.Name;
            this.groupid = userInfo.AdminID;
            this.groupexpiry = userInfo.GroupExpiry;
            this.grouptitle = UserGroup.FindByID(userInfo.GroupID).GroupTitle;
            this.curstatus = this.grouptitle;
        }

        public void UpdateBanUser()
        {
            int formInt = DNTRequest.GetFormInt("banexpirynew", -1);
            string text = (formInt == 0) ? "29990101" : string.Format("{0:yyyyMMdd}", DateTime.Now.AddDays((double)formInt));
            this.uid = DNTRequest.GetFormString("uid").ToInt();
            IUser shortUserInfo = BBX.Entity.User.FindByID(this.uid);
            if (shortUserInfo.AdminID >= 1 && shortUserInfo.AdminID <= 3 && shortUserInfo.AdminID <= this.useradminid)
            {
                base.AddErrLine("您无权编辑此用户信息");
                return;
            }
            Users.UpdateBanUser(DNTRequest.GetFormString("bannew").ToInt(), text, this.uid);
            AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.GetGroupTile(), WebHelper.UserHost, "更新被禁止访问或发帖的用户", "");
            this.banusersubmit = true;
            base.SetUrl("modcp.aspx?operation=banusersearch");
            base.MsgForward("modcp_succeed");
            base.AddMsgLine("用户资料成功更新");
            base.SetMetaRefresh();
        }

        protected void SearchForumSpecialUser()
        {
            int formInt = DNTRequest.GetFormInt("forumid", 0);
            if (DNTRequest.GetFormString("suser") != "" && Users.GetUserId(DNTRequest.GetFormString("suser")) == -1)
            {
                this.forumliststr = this.forumliststr.Replace("value=\"" + formInt + "\"", "value=\"" + formInt + "\" selected");
                return;
            }
            if (formInt == 0)
            {
                this.GetAllSpecialUser(DNTRequest.GetFormString("suser"));
            }
            else
            {
                if (formInt != -1 && formInt != 0)
                {
                    this.GetAllSpecialUser(formInt);
                }
            }
            this.forumliststr = this.forumliststr.Replace("value=\"" + formInt + "\"", "value=\"" + formInt + "\" selected");
        }

        private string GetAccess(string permuserlist, int sid, string filter)
        {
            string text = permuserlist;
            for (int i = 0; i < text.Split('|').Length; i++)
            {
                this.uid = Utils.StrToInt(text.Split('|')[i].Split(',')[1].ToString(), 0);
                if (this.uid == sid)
                {
                    string newValue = text.Split('|')[i].Split(',')[0] + "," + this.uid + "," + this.Cheked_Access(Utils.StrToInt(text.Split('|')[i].Split(',')[2], 0)).ToString();
                    if (filter == "updatepermuserlistuser")
                    {
                        text = text.Replace(text.Split('|')[i], newValue);
                        return text;
                    }
                    if (!(filter == "del"))
                    {
                        return text.Split('|')[i];
                    }
                    ArrayList arrayList = new ArrayList(permuserlist.Split('|'));
                    string[] array = text.Split('|');
                    for (int j = 0; j < array.Length; j++)
                    {
                        string text2 = array[j];
                        if (Utils.StrToInt(text2.Split(',')[1], 0) == sid)
                        {
                            arrayList.Remove(text2);
                        }
                    }
                    if (arrayList.Count == 0)
                    {
                        return text = "";
                    }
                    foreach (string str in arrayList)
                    {
                        text = "";
                        text = text + str + "|";
                    }
                    if (text != "")
                    {
                        if (text.Contains("||"))
                        {
                            text = text.Replace("||", "|");
                        }
                        text = text.Substring(0, text.Length - 1);
                    }
                }
            }
            return text;
        }

        protected void UpdatePermuserListUser()
        {
            string formString = DNTRequest.GetFormString("new_user");
            int userId = Users.GetUserId(formString);
            if (userId <= 0)
            {
                base.AddErrLine("该用户不存在");
                return;
            }
            User userInfo = Users.GetUserInfo(userId);
            if (userInfo.ID == this.userid && userInfo.AdminID >= 1 && userInfo.AdminID <= 3 && userInfo.AdminID <= this.useradminid)
            {
                base.AddErrLine("您无权变更管理员或此特殊用户权限或您自己的特殊权限");
                return;
            }
            this.GetAllSpecialUser("");
            int forumid = DNTRequest.GetFormInt("forumid", -1);
            //string perm = Users.SearchSpecialUser(forumid);
            var xf = XForum.FindByID(forumid) as IXForum;
            var perm = xf.Permuserlist;
            if (!Utils.StrIsNullOrEmpty(perm))
            {
                if (perm.Contains(formString))
                {
                    string access = this.GetAccess(perm, userId, (DNTRequest.GetFormInt("deleteaccess", -1) == 0) ? "del" : "updatepermuserlistuser");
                    //Users.UpdateSpecialUser(access, formInt);
                    var forum = XForum.FindByID(forumid);
                    (forum as IXForum).Permuserlist = access;
                    forum.Update();
                }
                this.Context.Response.Redirect(BaseConfigs.GetForumPath + "modcp.aspx?operation=forumaccesslist&op=access_successful");
                return;
            }
            AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.GetGroupTile(), WebHelper.UserHost, "更新用户权限", "");
            this.Context.Response.Redirect(BaseConfigs.GetForumPath + "modcp.aspx?operation=forumaccesslist&forumid=" + DNTRequest.GetFormInt("fid", 0));
        }

        public void GetAllSpecialUser(string uname)
        {
            //var forumSpecialUser = AdminForums.GetForumSpecialUser(uname);
            //if (forumSpecialUser == null) return;

            //if (forumSpecialUser.Length > 0)
            //{
            //    var array = forumSpecialUser;
            //    for (int i = 0; i < array.Length; i++)
            //    {
            //        var item = array[i];
            //        this.foruminfolist.Add(item);
            //    }
            //}
            foreach (IXForum item in XForum.Root.AllChilds)
            {
                if (uname.IsNullOrWhiteSpace())
                    foruminfolist.Add(item);
                else if (!item.Permuserlist.IsNullOrWhiteSpace())
                {
                    var ss = item.Permuserlist.Split("|");
                    if (ss.Any(e => e.Split(",")[0] == username)) foruminfolist.Add(item);
                }
            }
        }

        public void GetAllSpecialUser(int forumid)
        {
            var forumSpecialUser = AdminForums.GetForumSpecialUser(forumid);
            this.foruminfolist = new List<IXForum>();
            if (forumSpecialUser != null)
            {
                var array = forumSpecialUser;
                for (int i = 0; i < array.Length; i++)
                {
                    var item = array[i];
                    this.foruminfolist.Add(item);
                }
            }
        }

        protected string GetPowerImg(int power, ForumSpecialUserPower thePower)
        {
            if (!this.IsPower(power, thePower))
            {
                return "access_normal.gif";
            }
            return "access_allow.gif";
        }

        protected bool IsPower(int power, ForumSpecialUserPower thePower)
        {
            return (power & (int)thePower) != 0;
        }

        public int Cheked_Access(int special_value)
        {
            int num = 0;
            if (DNTRequest.GetFormInt("new_view", 0) != 0)
            {
                num = this.GetPower(DNTRequest.GetFormInt("new_view", 0), ForumSpecialUserPower.ViewByUser);
            }
            if (DNTRequest.GetFormInt("new_post", 0) != 0)
            {
                num = this.GetPower(DNTRequest.GetFormInt("new_post", 0), ForumSpecialUserPower.PostByUser);
            }
            if (DNTRequest.GetFormInt("new_reply", 0) != 0)
            {
                num = this.GetPower(DNTRequest.GetFormInt("new_reply", 0), ForumSpecialUserPower.ReplyByUser);
            }
            if (DNTRequest.GetFormInt("new_postattach", 0) != 0)
            {
                num = this.GetPower(DNTRequest.GetFormInt("new_postattach", 0), ForumSpecialUserPower.DownloadAttachByUser);
            }
            if (DNTRequest.GetFormInt("new_getattach", 0) != 0)
            {
                num = this.GetPower(DNTRequest.GetFormInt("new_getattach", 0), ForumSpecialUserPower.PostAttachByUser);
            }
            if (special_value >= 0 && special_value >= num)
            {
                return special_value - num;
            }
            return num;
        }

        protected int GetPower(int power, ForumSpecialUserPower thePower)
        {
            return power |= (int)thePower;
        }

        public List<IXForum> GetForumList()
        {
            foreach (var current in Forums.GetForumList())
            {
                if (this.useradminid == 3 && Utils.InArray(this.username, current.Moderators))
                {
                    this.forumslist.Add(current);
                }
            }
            return this.forumslist;
        }

        public void GetForumInfo()
        {
            if (DNTRequest.GetInt("forumid", 0) == 0)
            {
                //this.forumid = Forums.GetFirstFourmID();
                //this.foruminfo = Forums.GetForumInfo(this.forumid);
                var xf = XForum.Root.AllChilds.ToList().FirstOrDefault();
                this.forumid = xf.ID;
                this.foruminfo = xf;
            }
            else
            {
                this.foruminfo = Forums.GetForumInfo(DNTRequest.GetInt("forumid", 0));
            }
            if (this.foruminfo != null)
            {
                this.alloweditrules = (this.foruminfo.AllowEditRules || this.useradminid != 3);
                return;
            }
            base.AddErrLine("参数错误");
        }

        public void SetDropdownOptions()
        {
            this.forumliststr = ((this.useradminid == 3) ? Forums.GetModerDropdownOptions(this.userid) : Forums.GetDropdownOptions());
            this.forumliststr = this.forumliststr.Replace("value=\"" + DNTRequest.GetInt("forumid", 0) + "\"", "value=\"" + DNTRequest.GetInt("forumid", 0) + "\" selected");
        }

        public void UpdateForum()
        {
            this.forumid = DNTRequest.GetFormInt("forumid", 0);
            if (this.forumid == 0)
            {
                return;
            }
            var forumInfo = Forums.GetForumInfo(this.forumid);
            if (this.useradminid != 3 || forumInfo.AllowEditRules)
            {
                forumInfo.Rules = DNTRequest.GetString("rulesmessage");
            }
            forumInfo.Description = DNTRequest.GetString("descriptionmessage");
            AdminForums.UpdateForumInfo(forumInfo);
            this.Context.Response.Redirect(BaseConfigs.GetForumPath + "modcp.aspx?operation=editforum&forumid=" + this.forumid, false);
        }

        private void UpdateUserCredits(string tidlist)
        {
            string[] array = tidlist.Split(',');
            for (int i = 0; i < array.Length; i++)
            {
                string s = array[i];
                var topicInfo = Topic.FindByID(int.Parse(s));
                CreditsFacade.PostTopic(topicInfo.PosterID, topicInfo.Forum);
            }
        }

        private void UpdateUserCredits(int postTableId, string pidlist)
        {
            string[] array = pidlist.Split(',');
            for (int i = 0; i < array.Length; i++)
            {
                //string s = array[i];
                var postInfo = Post.FindByID(array[i].ToInt());
                CreditsFacade.PostReply(postInfo.PosterID, Forums.GetForumInfo(postInfo.Fid).ReplycrEdits);
            }
        }

        public void GetTopicList()
        {
            this.filter = DNTRequest.GetInt("filter", 0);
            if (this.filter != -2 && this.filter != -3)
            {
                return;
            }
            this.forumid = DNTRequest.GetInt("forumid", 0);
            this.foruminfo = Forums.GetForumInfo(this.forumid);
            StringBuilder stringBuilder = new StringBuilder();
            //string text;
            var fids = new List<Int32>();
            if (this.forumid == 0 && !Utils.InArray(this.useradminid.ToString(), "1,2"))
            {
                foreach (var current in this.GetForumList())
                {
                    //stringBuilder.Append(current.Fid + ",");
                    fids.Add(current.Fid);
                }
                //text = stringBuilder.ToString().TrimEnd(',');
            }
            else
            {
                //text = this.forumid.ToString();
                fids.Add(this.forumid);
            }
            this.forumname = ((this.foruminfo != null) ? this.foruminfo.Name : "");
            int num = DNTRequest.GetInt("page", 1);
            //this.topiclist = Topic.GetUnauditNewTopic(fids.ToArray(), this.filter, num, 16);
            //this.counts = Topics.GetUnauditNewTopicCount(text, this.filter);
            this.topiclist = Topic.GetUnauditNewTopic(fids.ToArray(), this.filter, num, 16);
            this.counts = Topic.GetUnauditNewTopicCount(fids.ToArray(), this.filter);
            this.last = ((this.counts >= 16) ? 15 : this.counts);
            this.pagecount = ((this.counts % 16 == 0) ? (this.counts / 16) : (this.counts / 16 + 1));
            this.pagecount = ((this.pagecount == 0) ? 1 : this.pagecount);
            num = ((num > this.pagecount) ? this.pagecount : num);
            this.pagenumbers = Utils.GetPageNumbers(num, this.pagecount, "modcp.aspx?operation=audittopic&forumid=" + this.forumid + "&filter=" + this.filter, 8);
        }

        public void GetPostList()
        {
            this.filter = DNTRequest.GetInt("filter", 0);
            if (this.filter != 1 && this.filter != -3)
            {
                return;
            }
            this.forumid = DNTRequest.GetInt("forumid", 0);
            this.foruminfo = Forums.GetForumInfo(this.forumid);
            this.pageid = DNTRequest.GetInt("page", 1);
            StringBuilder stringBuilder = new StringBuilder();
            string fidList;
            if (this.forumid == 0 && !Utils.InArray(this.useradminid.ToString(), "1,2"))
            {
                foreach (var current in this.GetForumList())
                {
                    stringBuilder.Append(current.Fid + ",");
                }
                fidList = stringBuilder.ToString();
            }
            else
            {
                fidList = this.forumid.ToString();
            }
            this.forumname = ((this.foruminfo != null) ? this.foruminfo.Name : "");
            if (this.forumid != 0)
            {
                this.forumnav = base.ShowForumAspxRewrite(this.foruminfo.Pathlist.Trim(), this.forumid, 1);
            }
            //this.tableid = DNTRequest.GetInt("tablelist", Utils.StrToInt(TableList.GetPostTableId(), 1));
            this.postlist = Post.GetUnauditPost(fidList, this.filter, this.pageid, 16);
            this.counts = Posts.GetUnauditNewPostCount(fidList, this.filter);
            this.last = ((this.counts >= 16) ? 15 : this.counts);
            this.pagecount = ((this.counts % 16 == 0) ? (this.counts / 16) : (this.counts / 16 + 1));
            this.pagecount = ((this.pagecount == 0) ? 1 : this.pagecount);
            this.pageid = ((this.pageid > this.pagecount) ? this.pagecount : this.pageid);
            this.pagenumbers = Utils.GetPageNumbers(this.pageid, this.pagecount, string.Format("modcp.aspx?operation=auditpost&forumid={0}&filter={1}", this.forumid, this.filter), 8);
        }

        public string GetPostForumNav(int fId)
        {
            if (fId <= 0)
            {
                return "";
            }
            return base.ShowForumAspxRewrite(Forums.GetForumInfo(fId).Pathlist.Trim(), fId, 1) + " » ";
        }

        public void AuditNewTopic()
        {
            string tids = DNTRequest.GetString("topicidlist");
            var fids = DNTRequest.GetString("fidlist").SplitAsInt(",");
            //string[] array = Utils.SplitString(fids, ",", true);
            //if (!Utils.IsNumericArray(array)) return;
            if (fids.Length == 0) return;

            if (this.useradminid == 3)
            {
                for (int i = 0; i < fids.Length; i++)
                {
                    if (!Moderators.IsModer(3, this.userid, fids[i]))
                    {
                        return;
                    }
                }
            }
            //Posts.GetAllPostTableName();
            var dic = new Dictionary<string, string>();
            //var dic2 = new Dictionary<string, string>();
            if (tids != "")
            {
                //string[] array2 = tids.Split(',');
                //for (int j = 0; j < array2.Length; j++)
                //{
                //    string text = array2[j];
                //    //string postTableId = TableList.GetPostTableId(Utils.StrToInt(text, 0));
                //    dic2[postTableId] = ((!dic2.ContainsKey(postTableId)) ? "" : dic2[postTableId].ToString()) + text + ",";
                //}
                //foreach (var item in dic2)
                {
                    string validate = string.Empty;
                    string delete = string.Empty;
                    string ignore = string.Empty;
                    string[] array3 = tids.Split(',');
                    for (int k = 0; k < array3.Length; k++)
                    {
                        string text5 = array3[k];
                        string act = DNTRequest.GetString("mod_" + text5).ToLower();
                        if (tids.Contains(text5) && act != null)
                        {
                            if (!(act == "validate"))
                            {
                                if (!(act == "delete"))
                                {
                                    if (act == "ignore")
                                    {
                                        dic.Add("帖子ID为" + text5 + "  " + DNTRequest.GetString("pm_" + text5), "忽略未审核主题");
                                        ignore = ignore + "," + text5.ToString();
                                    }
                                }
                                else
                                {
                                    dic.Add("帖子ID为" + text5 + "  " + DNTRequest.GetString("pm_" + text5), "删除未审核帖子");
                                    delete = delete + "," + text5.ToString();
                                }
                            }
                            else
                            {
                                dic.Add("帖子ID为<a href=\"" + this.config.Forumurl + "showtopic.aspx?tid=" + text5 + "\">" + text5 + "</a>  " + (DNTRequest.GetString("pm_" + text5)), "审核通过帖子");
                                validate = validate + "," + text5.ToString();
                            }
                        }
                    }
                    if (delete != "")
                    {
                        delete = delete.Remove(0, 1);
                    }
                    if (ignore != "")
                    {
                        ignore = ignore.Remove(0, 1);
                    }
                    if (validate != "")
                    {
                        validate = validate.Remove(0, 1);
                    }
                    if (Utils.SplitString(delete, ",", true).Length > 1 && !admingroupinfo.AllowMassprune)
                    {
                        base.AddErrLine("您所在的用户组无法批量删帖");
                        return;
                    }
                    Topics.PassAuditNewTopic(ignore, validate, delete, fids);
                }
                foreach (var item in dic)
                {
                    AdminVisitLog.InsertLog(this.uid, this.username, this.usergroupid, this.GetGroupTile(), WebHelper.UserHost, item.Value.ToString(), item.Key.ToString());
                }
                base.SetUrl(BaseConfigs.GetForumPath + "modcp.aspx?operation=audittopic&forumid=" + this.forumid);
                base.SetMetaRefresh(0);
            }
        }

        public void AuditPost()
        {
            if (!Utils.StrIsNullOrEmpty(DNTRequest.GetString("pidlist")) && this.ismoder)
            {
                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                string text = "";
                string text2 = "";
                string text3 = "";
                //this.tableid = DNTRequest.GetInt("tableid", 0);
                string[] array = DNTRequest.GetString("pidlist").Split(',');
                string[] array2 = DNTRequest.GetString("tidlist").Split(',');
                var fids = DNTRequest.GetString("fidlist").SplitAsInt(",");
                //if (!Utils.IsNumericArray(fids))
                //{
                //    return;
                //}
                if (fids.Length == 0) return;
                if (this.useradminid == 3)
                {
                    for (int i = 0; i < fids.Length; i++)
                    {
                        if (!Moderators.IsModer(3, this.userid, fids[i]))
                        {
                            return;
                        }
                    }
                }
                for (int j = 0; j < array.Length; j++)
                {
                    string a;
                    if ((a = DNTRequest.GetString("mod_" + array[j]).ToLower()) != null)
                    {
                        if (!(a == "validate"))
                        {
                            if (!(a == "delete"))
                            {
                                if (a == "ignore")
                                {
                                    dictionary.Add("帖子ID为" + array[j] + "  " + DNTRequest.GetString("pm_" + array[j]), "忽略未审核帖子");
                                    text3 = text3 + "," + array[j];
                                }
                            }
                            else
                            {
                                dictionary.Add("帖子ID为" + array[j] + "  " + DNTRequest.GetString("pm_" + array[j]), "删除未审核帖子");
                                text2 = text2 + "," + array[j];
                            }
                        }
                        else
                        {
                            dictionary.Add("帖子ID为<A href=\"" + this.config.Forumurl + "showtopic.aspx?pid=" + array[j] + "\">" + array[j] + "</a>  " + (DNTRequest.GetString("pm_" + array[j])), "审核通过帖子");
                            text = text + "," + array[j];
                            this.UpdateUserCredits(Utils.StrToInt(array2[j], 0), array[j]);
                        }
                    }
                }
                if (text2 != "")
                {
                    text2 = text2.Remove(0, 1);
                }
                if (text != "")
                {
                    text = text.Remove(0, 1);
                }
                if (text3 != "")
                {
                    text3 = text3.Remove(0, 1);
                }
                if (Utils.SplitString(text2, ",", true).Length > 1 && !admingroupinfo.AllowMassprune)
                {
                    base.AddErrLine("您所在的用户组无法批量删帖");
                    return;
                }
                Posts.AuditPost(text, text2, text3, DNTRequest.GetString("fidlist"));
                foreach (KeyValuePair<string, string> current in dictionary)
                {
                    AdminVisitLog.InsertLog(this.uid, this.username, this.usergroupid, this.GetGroupTile(), WebHelper.UserHost, current.Value.ToString(), current.Key.ToString());
                }
                base.SetUrl(BaseConfigs.GetForumPath + "modcp.aspx?operation=auditpost&forumid=" + this.forumid);
                base.SetMetaRefresh(0);
            }
        }

        public void GetLogs()
        {
            int size = DNTRequest.GetInt("lpp", 20);
            int page = DNTRequest.GetInt("page", 1);
            string keyword = DNTRequest.GetString("keyword");
            if (String.IsNullOrEmpty(keyword))
            {
                this.moderatorLogs = ModeratorManageLog.FindAll(null, null, null, (page - 1) * size, size).ToDataTable();
                this.counts = ModeratorManageLog.Meta.Count;
            }
            else
            {
                this.moderatorLogs = ModeratorManageLog.Search(keyword, null, (page - 1) * size, size).ToDataTable();
                this.counts = ModeratorManageLog.SearchCount(keyword, null, (page - 1) * size, size);
            }
            this.pagecount = ((this.counts % size == 0) ? (this.counts / size) : (this.counts / size + 1));
            this.pagecount = ((this.pagecount == 0) ? 1 : this.pagecount);
            page = ((page > this.pagecount) ? this.pagecount : page);
            this.pagenumbers = Utils.GetPageNumbers(page, this.pagecount, "modcp.aspx?operation=logs&keyword=" + keyword + "&lpp=" + size, 8);
        }

        public void UserOut()
        {
            ForumUtils.ClearUserCookie("cplogincookie");
            if (DNTRequest.GetInt("forumid", 0) == 0)
            {
                this.Context.Response.Redirect(BaseConfigs.GetForumPath + "index.aspx");
                return;
            }
            this.Context.Response.Redirect(BaseConfigs.GetForumPath + "showforum-" + DNTRequest.GetInt("forumid", 0) + this.config.Extname);
        }

        public void GetAttentionTopics()
        {
            int num = DNTRequest.GetInt("page", 1);
            string key = DNTRequest.GetString("keyword");
            string text = "modcp.aspx?operation=attention&keyword=" + key + "&forumid=" + this.forumid;
            //string text2 = "0";
            //var sb = new StringBuilder();
            var fids = new List<Int32>();
            if (this.forumid == 0 && !Utils.InArray(this.useradminid.ToString(), "1,2"))
            {
                foreach (var current in this.GetForumList())
                {
                    //sb.Append(current.Fid + ",");
                    fids.Add(current.Fid);
                }
                //if (sb.Length > 0)
                //{
                //    text2 = sb.ToString().Remove(sb.ToString().Length - 1);
                //}
            }
            else
            {
                //text2 = this.forumid.ToString();
                fids.Add(this.forumid);
            }
            int formInt = DNTRequest.GetFormInt("disattentiontype", -1);
            if (formInt != -1 && this.ispost)
            {
                int num2 = formInt;
                if (num2 != 0)
                {
                    Topic.UpdateTopicAttentionByFidList(fids.ToArray(), 0, Math.Abs(formInt));
                    this.Context.Response.Redirect(BaseConfigs.GetForumPath + text);
                    return;
                }
                if (DNTRequest.GetFormString("topicid") != "")
                {
                    //Topics.UpdateTopicAttentionByTidList(DNTRequest.GetFormString("topicid"), 0);
                    var ss = DNTRequest.GetFormString("topicid").SplitAsInt(",");
                    Topic.UpdateTopicAttentionByTidList(0, ss);
                    this.Context.Response.Redirect(BaseConfigs.GetForumPath + text);
                    return;
                }
            }
            else
            {
                //this.topiclist = Topics.GetAttentionTopics(text2, 16, num, key);
                //this.counts = Topics.GetAttentionTopicCount(text2, key);
                var count = 0;
                this.topiclist = Topic.GetAttentionTopics(fids.ToArray(), key, (num - 1) * 16, 16, ref count);
                this.counts = count;
                this.pagecount = ((this.counts % 16 == 0) ? (this.counts / 16) : (this.counts / 16 + 1));
                this.pagecount = ((this.pagecount == 0) ? 1 : this.pagecount);
                num = ((num > this.pagecount) ? this.pagecount : num);
                this.pagenumbers = Utils.GetPageNumbers(num, this.pagecount, text, 8);
            }
        }

        public void DelUserPost()
        {
            string formString;
            if ((formString = DNTRequest.GetFormString("deletetype")) != null && !(formString == "topics"))
            {
                //formString == "posts";
                formString = "posts";
            }
        }

        //public void GetAuditTopicCountByFid(string fid)
        //{
        //    //this.counts = Topics.GetUnauditNewTopicCount(fid, this.filter);
        //    var count = -1;
        //    Topic.GetUnauditNewTopic(new Int32[] { fid }, filter, 0, 0, ref count);
        //    this.counts = count;
        //}
    }
}