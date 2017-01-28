using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Serialization;
using BBX.Common;
using BBX.Config;
using NewLife.Collections;
using NewLife.Log;
using NewLife.Threading;
using NewLife.Web;
using NewLife.Xml;
using XCode;
using XCode.Configuration;

namespace BBX.Entity
{
    public enum UserAction
    {
        [Description("浏览首页")]
        IndexShow = 1,

        [Description("浏览论坛板块")]
        ShowForum = 2,

        [Description("浏览帖子")]
        ShowTopic = 3,

        [Description("论坛登陆")]
        Login = 4,

        [Description("发表主题")]
        PostTopic = 5,

        [Description("发表回复")]
        PostReply = 6,

        [Description("激活用户帐户")]
        ActivationUser = 7,

        [Description("注册新用户")]
        Register = 8
    }

    /// <summary>在线</summary>
    public partial class Online : EntityBase<Online>
    {
        #region 对象操作﻿
        static Online()
        {
            Meta.Factory.AdditionalFields.Add(_.Newnotices);
            Meta.Factory.AdditionalFields.Add(_.Newpms);
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            this.TrimField();
        }

        /// <summary>验证数据，通过抛出异常的方式提示验证失败。</summary>
        /// <param name="isNew"></param>
        public override void Valid(Boolean isNew)
        {
            // 这里验证参数范围，建议抛出参数异常，指定参数名，前端用户界面可以捕获参数异常并聚焦到对应的参数输入框
            //if (String.IsNullOrEmpty(Name)) throw new ArgumentNullException(_.Name, _.Name.DisplayName + "无效！");
            //if (!isNew && ID < 1) throw new ArgumentOutOfRangeException(_.ID, _.ID.DisplayName + "必须大于0！");

            // 建议先调用基类方法，基类方法会对唯一索引的数据进行验证
            base.Valid(isNew);

            // 在新插入数据或者修改了指定字段时进行唯一性验证，CheckExist内部抛出参数异常
            //if (isNew || Dirtys[_.Name]) CheckExist(_.Name);

            if (isNew || HasDirty)
            {
                //// 处理当前已登录用户信息
                //var user = ManageProvider.Provider.Current;
                //if (!Dirtys[_.UserID] && user != null) UserID = (Int32)user.ID;
                //if (!Dirtys[_.UserName] && user != null) UserName = user.Account;

                if (!Dirtys[__.LastUpdateTime]) LastUpdateTime = DateTime.Now;
                if (!Dirtys[__.IP]) IP = WebHelper.UserHost;
                if (!Dirtys[__.UserAgent]) UserAgent = HttpContext.Current.Request.UserAgent;

                // 限定UserAgent长度
                if (UserAgent != null && UserAgent.Length > _.UserAgent.Length) UserAgent = UserAgent.Substring(0, _.UserAgent.Length);
            }
        }

        /// <summary>首次连接数据库时初始化数据，仅用于实体类重载，用户不应该调用该方法</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void InitData()
        {
            base.InitData();

            // 首次使用数据库时，创建动态删除在线用户的定时器
            if (_timer == null)
            {
                var freq = GeneralConfigInfo.Current.DeletingExpiredUserFrequency;
                if (freq < 1) freq = 5;
                XTrace.WriteLine("定时清理离线用户间隔：{0}分钟", freq);
                // 一秒后开始定时清理在线用户
                _timer = new TimerX(DeleteExpiredOnlineUsers, null, 1000, freq * 60 * 1000);
            }

            if (Meta.Count > 0)
            {
                // 如果在线编号超过百万，则重置一下在线表
                var list = FindAll(null, _.ID.Desc(), null, 0, 1);
                if (list.Count > 0 && list[0].ID > 1000000)
                {
                    XTrace.WriteLine("在线编号超过百万，重置一下在线表");
                    ResetOnlineList();
                }
            }
        }

        ///// <summary>已重载。在事务保护范围内处理业务，位于Valid之后</summary>
        ///// <returns></returns>
        //protected override Int32 OnInsert()
        //{
        //    // 关闭SQL日志
        //    var f = DAL.ShowSQL;
        //    DAL.ShowSQL = false;

        //    try
        //    {
        //        return base.OnInsert();
        //    }
        //    finally
        //    {
        //        DAL.ShowSQL = f;
        //    }
        //}

        protected override int OnInsert()
        {
            // 关闭SQL日志
            var session = Meta.Session.Dal.Session;
            var f = session.ShowSQL;
            session.ShowSQL = false;

            //XTrace.WriteLine("Online.Insert {0}", SessionID);
            try
            {
                return base.OnInsert();
            }
            catch
            {
                var online = FindBySessionID(SessionID);
                XTrace.WriteLine("出错 {0}该会话", online != null ? "已找到" : "未找到");
                throw;
            }
            finally
            {
                session.ShowSQL = f;
            }
        }

        //protected override int OnDelete()
        //{
        //    XTrace.WriteLine("Online.Delete {0}", SessionID);
        //    return base.OnDelete();
        //}
        #endregion

        #region 扩展属性﻿
        /// <summary>属性说明</summary>
        public String ActionName { get { return Action == 0 ? "" : ((UserAction)Action).GetDescription(); } }

        public String Address { get { return IP.IPToAddress(); } }

        /// <summary>当前在线用户</summary>
        public static Online Current
        {
            get
            {
                var Session = HttpContext.Current.Session;
                if (Session == null)
                {
                    XTrace.WriteLine("此时获取在线用户过早，Session尚未形成！");
                    XTrace.DebugStack();
                    return null;
                }

                // 使用Session，避免SessionID丢失
                if (Session.Count < 1) Session["BBX_SID"] = 0;

                return FindBySessionID(Session.SessionID);
            }
            set
            {
                var Session = HttpContext.Current.Session;
                if (Session == null) return;

                var online = Current;
                if (online != null) online.Delete();

                Session.Abandon();
            }
        }

        /// <summary>用户</summary>
        public User User
        {
            get
            {
                if (UserID <= 0) return null;

                return User.FindByID(UserID);
            }
        }

        /// <summary>用户组</summary>
        public UserGroup Group
        {
            get
            {
                if (GroupID <= 0) return UserGroup.Guest;

                return UserGroup.FindByID(GroupID);
            }
        }
        #endregion

        #region 扩展查询﻿
        const Int32 CacheCount = 10000;

        ///// <summary>根据用户编号,外键、是否隐身、ForumID查找</summary>
        ///// <param name="userid">用户编号,外键</param>
        ///// <param name="invisible">是否隐身</param>
        ///// <param name="forumid"></param>
        ///// <returns></returns>
        //[DataObjectMethod(DataObjectMethodType.Select, false)]
        //public static EntityList<Online> FindAllByUserIDAndInvisibleAndForumID(Int32 userid, Int16 invisible, Int32 forumid)
        //{
        //    if (Meta.Count >= CacheCount)
        //        return FindAll(new String[] { _.UserID, _.Invisible, _.ForumID }, new Object[] { userid, invisible, forumid });
        //    else // 实体缓存
        //        return Meta.Cache.Entities.FindAll(e => e.UserID == userid && e.Invisible == invisible && e.ForumID == forumid);
        //}

        /// <summary>根据ForumID查找</summary>
        /// <param name="forumid"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static EntityList<Online> FindAllByForumID(Int32 forumid)
        {
            if (Meta.Count >= CacheCount)
                return FindAll(__.ForumID, forumid);
            else // 实体缓存
                return Meta.Cache.Entities.FindAll(__.ForumID, forumid);
        }

        //public static EntityList<Online> FindAllByIP(String ip)
        //{
        //    if (Meta.Count >= CacheCount)
        //        return FindAll(__.IP, ip);
        //    else // 实体缓存
        //        return Meta.Cache.Entities.FindAll(__.IP, ip);
        //}

        ///// <summary>根据用户编号,外键、是否隐身查找</summary>
        ///// <param name="userid">用户编号,外键</param>
        ///// <param name="invisible">是否隐身</param>
        ///// <returns></returns>
        //[DataObjectMethod(DataObjectMethodType.Select, false)]
        //public static EntityList<Online> FindAllByUserIDAndInvisible(Int32 userid, Int16 invisible)
        //{
        //    if (Meta.Count >= CacheCount)
        //        return FindAll(new String[] { _.UserID, _.Invisible }, new Object[] { userid, invisible });
        //    else // 实体缓存
        //        return Meta.Cache.Entities.FindAll(e => e.UserID == userid && e.Invisible == invisible);
        //}

        /// <summary>根据用户编号,外键、IP地址查找</summary>
        /// <param name="userid">用户编号,外键</param>
        /// <param name="ip">IP地址</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static Online FindByUserID(Int32 userid)
        {
            if (Meta.Count >= CacheCount)
                return Find(_.UserID, userid);
            else // 实体缓存
                return Meta.Cache.Entities.Find(e => e.UserID == userid);
        }

        public static Online FindBySessionID(String sessionid)
        {
            if (Meta.Count >= CacheCount)
                return Find(__.SessionID, sessionid);
            else // 实体缓存
                //return Meta.Cache.Entities.Find(e => e.SessionID == sessionid);
                return Meta.Cache.Entities.FindIgnoreCase(__.SessionID, sessionid);
        }

        ///// <summary>根据用户编号,外键、IP地址查找</summary>
        ///// <param name="userid">用户编号,外键</param>
        ///// <param name="ip">IP地址</param>
        ///// <returns></returns>
        //[DataObjectMethod(DataObjectMethodType.Select, false)]
        //public static Online FindByUserIDAndIP(Int32 userid, String ip)
        //{
        //    if (Meta.Count >= CacheCount)
        //        return Find(new String[] { _.UserID, _.IP }, new Object[] { userid, ip });
        //    else // 实体缓存
        //        return Meta.Cache.Entities.Find(e => e.UserID == userid && e.IP == ip);
        //}

        ///// <summary>根据用户编号,外键、登录密码查找</summary>
        ///// <param name="userid">用户编号,外键</param>
        ///// <param name="password">登录密码</param>
        ///// <returns></returns>
        //[DataObjectMethod(DataObjectMethodType.Select, false)]
        //public static Online FindByUserIDAndPassword(Int32 userid, String password)
        //{
        //    if (Meta.Count >= CacheCount)
        //        return Find(new String[] { __.UserID, __.Password }, new Object[] { userid, password });
        //    else // 实体缓存
        //        return Meta.Cache.Entities.Find(e => e.UserID == userid && e.Password == password);
        //}

        /// <summary>根据编号查找</summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static Online FindByID(Int32 id)
        {
            if (Meta.Count >= CacheCount)
                return Find(__.ID, id);
            else // 实体缓存
                return Meta.Cache.Entities.Find(__.ID, id);
            // 单对象缓存
            //return Meta.SingleCache[id];
        }
        #endregion

        #region 高级查询
        #endregion

        #region 扩展操作
        #endregion

        #region 删除过期用户
        private static TimerX _timer;

        private static void DeleteExpiredOnlineUsers(Object state)
        {
            // 如果没有在线用户，则跳过
            if (Meta.Count <= 0) return;

            var batSize = 100;  // 分批删除在线用户的批大小

            var timeOut = GeneralConfigInfo.Current.Onlinetimeout;
            if (timeOut < 0) timeOut = -timeOut;
            while (true)
            {
                using (var trans = Meta.CreateTrans())
                {
                    // 查出来一批过期用户
                    var list = FindAll(_.LastUpdateTime.IsNull() | _.LastUpdateTime < DateTime.Now.AddMinutes(-timeOut), null, null, 0, batSize);
                    // 遍历删除
                    foreach (var item in list)
                    {
                        // 对于会员，设定最后活跃时间
                        if (item.UserID != -1)
                        {
                            var user = User.FindByID(item.UserID);
                            if (user != null)
                            {
                                user.OnlineState = 0;
                                if (item.LastUpdateTime > user.LastActivity) user.LastActivity = item.LastUpdateTime;
                                user.Save();
                            }
                        }

                        item.Delete();
                    }

                    trans.Commit();

                    // 如果不足批大小，说明后面没有过期在线用户了，删除
                    if (list.Count < batSize) break;
                }
            }
        }

        /// <summary>清空在线数据，重置在线列表</summary>
        /// <returns></returns>
        public static int ResetOnlineList()
        {
            try
            {
                return Meta.Session.Execute(String.Format("TRUNCATE TABLE {0}", Meta.FormatName(Meta.TableName)));
            }
            catch { return -1; }
        }
        #endregion

        #region 业务
        //static DateTime _nextRemove;

        static Object syncLock = new Object();
        public int AddOnlineUser()
        {
            var user = User.FindByID(UserID);
            if (user != null)
            {
                user.OnlineState = 1;
                user.Save();
            }

            // 避免多线程重复插入
            lock (syncLock)
            {
                var online = Current;
                if (online == null)
                    Insert();
                else
                {
                    this.ID = online.ID;
                    Update();
                }
            }

            if (ID > 2147483000) ResetOnlineList();

            return ID;
        }

        public static Int32 DeleteByUserGroup(Int32 groupid)
        {
            var list = Meta.Cache.Entities.FindAll(__.GroupID, groupid);
            list.Delete();
            return list.Count;
        }

        //public static void DeleteRowsByIP(String ip)
        //{
        //    //var list = FindAll(_.IP == ip, null, null, 0, 0);
        //    var list = FindAllByIP(ip);
        //    if (list.Count <= 0) return;

        //    foreach (var item in list)
        //    {
        //        if (item.UserID > 0)
        //        {
        //            var user = User.FindByID(item.UserID);
        //            if (user != null)
        //            {
        //                user.OnlineState = 0;
        //                user.LastActivity = DateTime.Now;
        //                user.Update();
        //            }
        //            item.Delete();
        //        }
        //    }
        //}

        public static Boolean CheckUserVerifyCode(int olid, string verifycode)
        {
            var entity = FindByID(olid);
            if (entity == null) return false;

            var rs = entity.VerifyCode.EqualIgnoreCase(verifycode);

            // 用过以后更换验证码
            entity.VerifyCode = CreateAuthStr(5, false);
            entity.Save();

            return rs;
        }

        static Random _rnd;
        static String _Range = "123456789abcdefghjkmnpqrstuvwxy";
        public static string CreateAuthStr(int len, bool OnlyNum)
        {
            if (_rnd == null) _rnd = new Random((Int32)DateTime.Now.Ticks);
            var sb = new StringBuilder();
            for (int i = 0; i < len; i++)
            {
                int num;
                if (!OnlyNum)
                {
                    num = _rnd.Next(0, _Range.Length);
                }
                else
                {
                    num = _rnd.Next(0, 10 - 1);
                }
                sb.Append(_Range[num]);
            }
            return sb.ToString();
        }

        public void UpdateNewNotices(Int32 pluscount)
        {
            if (pluscount > 0)
            {
                Newnotices += pluscount;
                Update();
            }
            else
            {
                var count = Notice.FindCountByUidAndNew(UserID, 1);
                Newnotices = count;
                Update();
            }
        }

        public void UpdateNewPms(Int32 count)
        {
            this.Newpms += count;
            Update();
        }
        #endregion

        #region 创建在线用户
        //private static EntityList<OnlineList> GetOnlineGroupIconTable()
        //{
        //    var cacheService = XCache.Current;
        //    var list = cacheService.RetrieveObject(CacheKeys.FORUM_ONLINE_ICON_TABLE) as EntityList<OnlineList>;
        //    if (list == null)
        //    {
        //        list = OnlineList.GetGroupIcons();
        //        XCache.Add(CacheKeys.FORUM_ONLINE_ICON_TABLE, list);
        //    }
        //    return list;
        //}

        public static string GetGroupImg(int groupid)
        {
            string text = "";
            var list = OnlineList.GetGroupIcons();
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    if (item.GroupID == 0 && String.IsNullOrEmpty(text) || item.GroupID == groupid)
                    {
                        text = "<img src=\"" + BaseConfigs.GetForumPath + "images/groupicons/" + item.Img + "\" />";
                    }
                }
            }
            return text;
        }

        void SetUser(User user)
        {
            var online = this;

            online.UserID = user.ID;
            online.UserName = (user.Name + "").Trim();
            online.NickName = (user.NickName + "").Trim();
            online.Password = (user.Password + "").Trim();
            online.GroupID = user.GroupID;
            online.Olimg = GetGroupImg(user.GroupID);
            online.AdminID = user.AdminID;
            online.Invisible = user.Invisible;
            online.LastPostTime = user.LastPost;
        }

        void SetGuest()
        {
            var online = this;

            var kind = UserGroupKinds.游客;
            online.UserID = -1;
            online.UserName = kind.ToString();
            online.NickName = kind.ToString();
            online.Password = "";
            online.GroupID = (Int16)kind;
            online.Olimg = GetGroupImg((Int32)kind);
            online.AdminID = 0;
            online.Invisible = false;
            //online.LastPostTime = DateTime.MinValue;
        }

        public static Online CreateUser(int uid)
        {
            var Request = HttpContext.Current.Request;

            if (uid <= 0) return CreateGuestUser();

            var user = User.FindByID(uid);
            if (user == null) return CreateGuestUser();

            var online = new Online();
            online.SetUser(user);
            online.SessionID = HttpContext.Current.Session.SessionID;
            online.IP = WebHelper.UserHost;
            online.UserAgent = HttpContext.Current.Request.UserAgent;
            //在页面上貌似没看到有显示这些时间的地方，因为要多查询几张表来查询时间，暂时先不做处理
            //online.Lastpostpmtime = "1900-1-1 00:00:00";
            //online.Lastsearchtime = "1900-1-1 00:00:00";
            //online.LastUpdateTime = DateTime.Now;
            //online.Action = 0;
            //online.Lastactivity = 0;
            online.VerifyCode = CreateAuthStr(5, false);
            //int privateMessageCount = PrivateMessages.GetPrivateMessageCount(uid, 0, 1);
            var privateMessageCount = ShortMessage.GetPrivateMessageCount(uid, 0, 1);
            //int newNoticeCountByUid = Notices.GetNewNoticeCountByUid(uid);
            int newNoticeCountByUid = Notice.FindCountByUidAndNew(uid, 1);
            online.Newpms = (Int16)(privateMessageCount > 1000 ? 1000 : privateMessageCount);
            online.Newnotices = (Int16)(newNoticeCountByUid > 1000 ? 1000 : newNoticeCountByUid);
            //online.Olid = BBX.Data.OnlineUsers.CreateOnlineUserInfo(online, timeout);
            online.AddOnlineUser();
            if (user.AdminID > 0 && user.AdminID < 4 && Notice.ReNewNotice(NoticeType.AttentionNotice, user.ID) == 0)
            {
                Notice.Create(user.ID, NoticeType.AttentionNotice, "请及时查看<a href=\"modcp.aspx?operation=attention&forumid=0\">需要关注的主题</a>");
            }
            User.SetUserOnlineState(uid, true);
            var cookie = HttpContext.Current.Request.Cookies["bbx"];
            if (cookie != null)
            {
                cookie["tpp"] = user.Tpp.ToString();
                cookie["ppp"] = user.Ppp.ToString();
                int num = cookie["expires"].ToInt(0);
                if (num > 0) cookie.Expires = DateTime.Now.AddMinutes(num);
            }
            var domain = GeneralConfigInfo.Current.CookieDomain;
            if (!domain.IsNullOrEmpty() && Request.Url.Host.IndexOf(domain) > -1 && IsValidDomain(Request.Url.Host))
            {
                cookie.Domain = domain;
            }
            HttpContext.Current.Response.AppendCookie(cookie);

            return online;
        }

        static bool IsValidDomain(string host)
        {
            return host.IndexOf(".") != -1 && !new Regex("^\\d+$").IsMatch(host.Replace(".", string.Empty));
        }

        public static Online CreateGuestUser()
        {
            var online = new Online();
            online.SetGuest();
            online.SessionID = HttpContext.Current.Session.SessionID;
            //onlineUserInfo.Adminid = 0;
            //onlineUserInfo.Invisible = 0;
            online.IP = WebHelper.UserHost;
            online.UserAgent = HttpContext.Current.Request.UserAgent;
            online.TagGuest();  // 标记公开的游客
            //onlineUserInfo.Lastposttime = "1900-1-1 00:00:00";
            //onlineUserInfo.Lastpostpmtime = "1900-1-1 00:00:00";
            //onlineUserInfo.Lastsearchtime = "1900-1-1 00:00:00";
            //onlineUserInfo.Lastupdatetime = Utils.GetDateTime();
            //online.LastUpdateTime = DateTime.Now;
            //online.Action = 0;
            //onlineUserInfo.Lastactivity = 0;
            online.VerifyCode = CreateAuthStr(5, false);
            //onlineUserInfo.Olid = BBX.Data.OnlineUsers.CreateOnlineUserInfo(onlineUserInfo, timeout);

            online.AddOnlineUser();

            return online;
        }

        public static Online UpdateInfo(int uid = -1, string passwd = "")
        {
            var cfg = GeneralConfigInfo.Current;
            var passwordkey = cfg.Passwordkey;

            string iP = WebHelper.UserHost;
            int userid = GetCookie("userid").ToInt(uid);
            string pass = Utils.StrIsNullOrEmpty(passwd) ? GetCookiePassword(passwordkey) : GetCookiePassword(passwd, passwordkey);
            if (pass.Length == 0 || !Utils.IsBase64String(pass)) userid = -1;

            var online = Current;

            if (userid > 0)
            {
                // 根据用户ID和密码来验证在线信息
                //var online = Online.GetOnlineUser(userid, pass);
                if (!DNTRequest.GetPageName().EndsWith("ajax.ashx") && cfg.Statstatus)
                {
                    Stat.IncCount(false, online != null);
                }
                if (online != null)
                {
                    if (online.UserID != userid)
                    {
                        var user = User.Check(userid, pass, false);
                        if (user != null)
                            online.SetUser(user);
                        else
                        {
                            XTrace.WriteLine("用户ID={0} Pass={1}验证失败 当前登录{2}", userid, pass, online.UserID);
                            online.SetGuest();
                        }
                    }

                    // 如果IP变了，更换一下
                    online.IP = iP;
                    online.Save();
                    return online;
                }
                else
                {
                    CheckIp(iP);
                    // 如果找不到在线信息，则准备登录
                    var user = User.Check(userid, pass, false);
                    if (user != null) return Online.CreateUser(userid);

                    XTrace.WriteLine("用户ID={0} Pass={1}验证失败", userid, pass);
                    return CreateGuestUser();
                }
            }
            else
            {
                //var online = Online.FindByUserIDAndIP(-1, iP);
                if (!DNTRequest.GetPageName().EndsWith("ajax.ashx") && cfg.Statstatus)
                {
                    Stat.IncCount(true, online != null);
                }
                if (online == null) return CreateGuestUser();
                return online;
            }
        }

        private static void CheckIp(string ip)
        {
            string text = "";
            //var bannedIpList = Caches.GetBannedIpList();
            var bannedIpList = Banned.FindAllWithCache();
            var ips = ip.Split('.');
            foreach (var item in bannedIpList)
            {
                if (ip == String.Format("{0}.{1}.{2}.{3}", item.Ip1, item.Ip2, item.Ip3, item.Ip4))
                {
                    text = "您的ip被封,于" + item.Expiration + "后解禁";
                    break;
                }
                if (item.Ip4 == 0 && ips[0].ToInt(-1) == item.Ip1 && ips[1].ToInt(-1) == item.Ip2 && ips[2].ToInt(-1) == item.Ip3)
                {
                    text = "您所在的ip段被封,于" + item.Expiration + "后解禁";
                    break;
                }
            }
            if (text != string.Empty)
            {
                HttpContext.Current.Response.Redirect(BaseConfigs.GetForumPath + "tools/error.htm?forumpath=" + BaseConfigs.GetForumPath + "&templatepath=default&msg=" + Utils.UrlEncode(text));
            }
        }

        public static string GetCookie(string strName)
        {
            if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies["bbx"] != null && HttpContext.Current.Request.Cookies["bbx"][strName] != null)
            {
                return Utils.UrlDecode(HttpContext.Current.Request.Cookies["bbx"][strName].ToString());
            }
            return "";
        }

        public static string GetCookiePassword(string key)
        {
            return DES.Decode(GetCookie("password"), key).Trim();
        }

        public static string GetCookiePassword(string password, string key)
        {
            return DES.Decode(password, key);
        }
        #endregion

        #region 在线列表

        //public static EntityList<Online> GetOnlineUserList(int totaluser, String order, Boolean isdesc, out int guest, out int user, out int invisibleuser)
        //{
        //    var list = Online.FindAllWithCache();
        //    if (!String.IsNullOrEmpty(order))
        //    {
        //        FieldItem fi = Meta.Table.FindByName(order);
        //        if (fi != null) list = list.Sort(fi.Name, isdesc);
        //    }

        //    var st = Statistic.Current;
        //    if (totaluser > st.HighestOnlineUserCount)
        //    {
        //        st.HighestOnlineUserCount = totaluser;
        //        st.HighestOnlineUserTime = DateTime.Now;
        //        st.Update();
        //    }
        //    user = list.Count;
        //    invisibleuser = list.ToList().Count(e => e.Invisible == 1);
        //    guest = totaluser > user ? totaluser - user : 0;
        //    return list;
        //}

        public static EntityList<Online> GetList(Int32 forumid = 0, String order = null, Boolean isdesc = false, Int32 start = 0, Int32 max = 200)
        {
            if (Meta.Count < CacheCount)
            {
                var list = Online.FindAllWithCache().Clone();
                if (forumid > 0) list = list.FindAll(__.ForumID, forumid);
                // 如果隐藏游客
                if (GeneralConfigInfo.Current.WhosOnlineContract) list = list.FindAll(e => e.UserID > 0);

                // 排序
                if (!String.IsNullOrEmpty(order))
                {
                    FieldItem fi = Meta.Table.FindByName(order);
                    if (fi != null) list = list.Sort(fi.Name, isdesc);
                }

                return list.Page(start, max);
            }
            else
            {
                // 最多返回1000行
                var exp = new WhereExpression();
                if (forumid > 0) exp &= _.ForumID == forumid;
                if (GeneralConfigInfo.Current.WhosOnlineContract) exp &= _.UserID > 0;

                // 排序
                ConcatExpression dexp = null;
                if (!String.IsNullOrEmpty(order))
                {
                    FieldItem fi = Meta.Table.FindByName(order);
                    if (fi != null)
                    {
                        if (isdesc)
                            dexp = fi.Desc();
                        else
                            dexp = fi.Asc();
                    }
                }

                return FindAll(null, dexp, null, start, max);
            }
        }

        //static Statistics _stat;
        //static DateTime _nextGetStat;

        static DictionaryCache<Int32, Statistics> _StatCache = new DictionaryCache<int, Statistics> { Expire = 10 };

        /// <summary>获取在线用户数，带缓存</summary>
        /// <returns></returns>
        public static Statistics GetStat(Int32 forumid = 0)
        {
            return _StatCache.GetItem(forumid, fid =>
            {
                var st = new Statistics();
                st.Total = Meta.Count;
                if (st.Total < CacheCount)
                {
                    //_nextGetStat = now.AddSeconds(3);

                    var list = Online.FindAllWithCache();
                    if (fid > 0) list = list.FindAll(__.ForumID, fid);

                    st.Total = list.Count;
                    var user = 0;
                    var invisibleuser = 0;
                    foreach (var item in list.ToArray())
                    {
                        if (item.UserID > 0) user++;
                        if (item.Invisible) invisibleuser++;
                    }
                    st.User = user;
                    st.Invisible = invisibleuser;
                }
                else
                {
                    //_nextGetStat = now.AddSeconds(10);

                    var exp = new WhereExpression();
                    if (fid > 0) exp &= _.ForumID == fid;
                    st.Total = FindCount(exp, null, null, 0, 0);
                    exp &= _.UserID > 0;
                    st.User = FindCount(exp, null, null, 0, 0);
                    exp &= _.Invisible == 1;
                    st.Invisible = FindCount(exp, null, null, 0, 0);
                }
                st.Guest = st.Total - st.User;

                // 更新最高在线用户
                var stc = Statistic.Current;
                if (st.Total > stc.HighestOnlineUserCount)
                {
                    stc.HighestOnlineUserCount = st.Total;
                    stc.HighestOnlineUserTime = DateTime.Now;
                    stc.Update();
                }

                return st;
            });
        }

        /// <summary>在线用户统计</summary>
        public class Statistics
        {
            /// <summary>总在线</summary>
            public Int32 Total;

            /// <summary>在线会员</summary>
            public Int32 User;

            /// <summary>在线访客</summary>
            public Int32 Guest;

            /// <summary>隐身用户</summary>
            public Int32 Invisible;
        }
        #endregion

        #region 更新动作
        public static void UpdateAction(int olid, UserAction action, int inid, int timeout)
        {
            var tick = Environment.TickCount;
            if (timeout < 0 && tick - Utils.GetCookie("lastolupdate").ToInt(tick) < 300000)
            {
                Utils.WriteCookie("lastolupdate", tick.ToString());
                return;
            }
            UpdateAction(olid, action, inid);
        }

        public static void UpdateAction(int olid, UserAction action, int inid)
        {
            if (GeneralConfigInfo.Current.Onlineoptimization != 1)
            {
                //BBX.Data.Online.UpdateAction(olid, action, inid);
                var entity = FindByID(olid);
                if (entity != null)
                {
                    entity.Action = (Int32)action;
                    entity.ForumID = inid;
                    entity.TitleID = inid;
                    entity.Save();
                }
            }
        }

        public static void UpdateAction(int olid, UserAction action, int fid, string forumname, int tid, string topictitle)
        {
            bool flag = false;
            if (forumname != null && forumname.Length > 40) forumname = forumname.Substring(0, 37) + "...";
            if (topictitle != null && topictitle.Length > 40) topictitle = topictitle.Substring(0, 37) + "...";
            var tick = Environment.TickCount;
            var config = GeneralConfigInfo.Current;
            var act = (UserAction)action;
            if (act == UserAction.PostReply || act == UserAction.PostTopic)
            {
                if (config.PostTimeStorageMedia == 0 || String.IsNullOrEmpty(Utils.GetCookie("lastposttime")))
                    flag = true;
                else
                    Utils.WriteCookie("lastposttime", Utils.GetDateTime());
            }
            else
            {
                if (config.Onlineoptimization != 1 &&
                    tick - Utils.GetCookie("lastolupdate").ToInt(tick) >= 300000 &&
                    (act == UserAction.ShowForum || act == UserAction.ShowTopic || act == UserAction.ShowTopic || act == UserAction.PostReply))
                {
                    flag = true;
                }
            }
            if (flag)
            {
                //BBX.Data.Online.UpdateAction(olid, action, fid, forumname, tid, topictitle);
                var entity = FindByID(olid);
                if (entity != null)
                {
                    entity.Action = (Int32)action;
                    entity.ForumID = fid;
                    entity.ForumName = forumname;
                    entity.TitleID = tid;
                    entity.Title = topictitle;
                    entity.Save();
                }

                Utils.WriteCookie("lastolupdate", tick.ToString());
                Utils.WriteCookie("lastposttime", Utils.GetDateTime());
            }
        }

        public static void UpdatePostPMTime(int olid)
        {
            if (GeneralConfigInfo.Current.Onlineoptimization != 1)
            {
                //BBX.Data.OnlineUsers.UpdatePostPMTime(olid);
                var entity = FindByID(olid);
                if (entity != null)
                {
                    entity.LastPostpmTime = DateTime.Now;
                    entity.Save();
                }
            }
        }

        public static void UpdateInvisible(int olid, Boolean invisible)
        {
            if (GeneralConfigInfo.Current.Onlineoptimization != 1)
            {
                //BBX.Data.OnlineUsers.UpdateInvisible(olid, invisible);
                var entity = FindByID(olid);
                if (entity != null)
                {
                    entity.Invisible = invisible;
                    entity.Save();
                }
            }
        }

        public static void UpdatePassword(int olid, string password)
        {
            //BBX.Data.OnlineUsers.UpdatePassword(olid, password);
            var entity = FindByID(olid);
            if (entity != null)
            {
                entity.Password = password;
                entity.Save();
            }
        }

        public static int DeleteUserByUid(int uid)
        {
            //return DeleteRows(GetOlidByUid(uid));
            var entity = Online.FindByUserID(uid);
            if (entity != null) return entity.Delete();
            return 0;
        }

        public static Int32 LastOnlineTime
        {
            get { return WebHelper.ReadCookie("lastactivity", "onlinetime").ToInt(Environment.TickCount); }
            set { WebHelper.WriteCookie("lastactivity", "onlinetime", value + ""); }
        }

        public static Int32 LastOLTime
        {
            get { return WebHelper.ReadCookie("lastactivity", "oltime").ToInt(Environment.TickCount); }
            set { WebHelper.WriteCookie("lastactivity", "oltime", value + ""); }
        }

        public static void UpdateOnlineTime(int oltimespan, int uid)
        {
            if (oltimespan != 0)
            {
                if (LastOnlineTime == 0) LastOnlineTime = Environment.TickCount;

                int num = Environment.TickCount - LastOnlineTime;
                if (num <= 0 || num >= oltimespan * 60 * 1000)
                {
                    //BBX.Data.OnlineUsers.UpdateOnlineTime(oltimespan, uid);
                    OnlineTime.UpdateOnlineTime(oltimespan, uid);
                    //WebHelper.WriteCookie("lastactivity", "onlinetime", Environment.TickCount.ToString());
                    LastOnlineTime = Environment.TickCount;
                    num = Environment.TickCount - LastOLTime;
                    if (LastOLTime == 0 || num <= 0 || num >= 2 * oltimespan * 60 * 1000)
                    {
                        //BBX.Data.OnlineUsers.SynchronizeOnlineTime(uid);
                        OnlineTime.SynchronizeOnlineTime(uid);
                        //WebHelper.WriteCookie("lastactivity", "oltime", Environment.TickCount.ToString());
                        LastOLTime = Environment.TickCount;
                    }
                }
            }
        }

        public static void UpdateGroupid(int userid, int groupid)
        {
            //if (OnlineUsers.appDBCache)
            //{
            //    OnlineUsers.IOnlineUserService.UpdateGroupid(userid, groupid);
            //    return;
            //}
            //DatabaseProvider.GetInstance().UpdateGroupid(userid, groupid);
            var entity = Online.FindByUserID(userid);
            if (entity != null)
            {
                entity.GroupID = groupid;
                entity.Save();
            }
        }
        #endregion

        #region 公开特殊游客名称
        public void TagGuest()
        {
            if (NickName != UserGroupKinds.游客.ToString()) return;

            // 根据Agent识别
            var name = AgentConfig.Current.GetName(UserAgent);
            // 根据物理地址识别
            if (String.IsNullOrEmpty(name))
            {
                var addr = Address;
                if (!String.IsNullOrEmpty(addr))
                {
                    var p = addr.IndexOf(' ');
                    if (p >= 0) addr = addr.Substring(0, p).Trim();
                    addr = addr.Trim().TrimEnd("省", "自治区", "市");
                    name = addr + "访客";
                }
            }
            else
            {
                // 爬虫
                IsBot = true;
            }
            if (!String.IsNullOrEmpty(name))
            {
                UserName = name;
                NickName = name;
            }
        }
        #endregion
    }

    [XmlConfigFile("config/Agent.config", 150000)]
    [Description("代理匹配配置")]
    public class AgentConfig : XmlConfig2<AgentConfig>
    {
        private List<Item> _Agents;
        /// <summary>匹配代理集合</summary>
        [Description("Url为匹配请求地址的正则表达式，Target为要重写的目标地址，可以使用$1等匹配项，Name只是标识作用，不参与业务处理")]
        public List<Item> Agents { get { return _Agents; } set { _Agents = value; } }

        public String GetName(String agent)
        {
            if (String.IsNullOrEmpty(agent)) return null;
            if (Agents == null || Agents.Count < 1) return null;

            foreach (var item in Agents)
            {
                var name = item.GetName(agent);
                if (!String.IsNullOrEmpty(name)) return name;
            }
            return null;
        }

        protected override void OnLoaded()
        {
            base.OnLoaded();

            if (Agents == null) Agents = new List<Item>();
            if (Agents.Count < 1)
            {
                this.Add("google", "谷歌爬虫")
                    .Add("bing", "Bing爬虫")
                    .Add("msn", "MSN爬虫")
                    .Add("baidu", "百度爬虫")
                    .Add("DNSPod", "DNSPod监控")
                    .Add("360Spider", "360爬虫")
                    .Add("baidu", "百度爬虫")
                    .Add("AhrefsBot", "荷兰Ahrefs爬虫")
                    .Add("youdao", "有道订阅")
                    .Add("", "")
                    .Add("", "");

                this.Save();
            }
        }

        AgentConfig Add(String key, String name)
        {
            if (!String.IsNullOrEmpty(key)) Agents.Add(new Item { Agent = "\\b" + key + "\\b", Name = name });

            return this;
        }

        /// <summary>Url重写地址配置项</summary>
        public class Item
        {
            private String _Agent;
            /// <summary>名称</summary>
            [XmlAttribute]
            public String Agent { get { return _Agent; } set { _Agent = value; } }

            private String _Name;
            /// <summary>名称</summary>
            [XmlAttribute]
            public String Name { get { return _Name; } set { _Name = value; } }

            private Regex _reg;
            /// <summary>获取指定输入的重写Url</summary>
            /// <param name="input"></param>
            /// <returns></returns>
            public String GetName(String input)
            {
                if (String.IsNullOrEmpty(input)) return input;
                if (String.IsNullOrEmpty(Agent)) return null;
                if (String.IsNullOrEmpty(Name)) return null;

                if (_reg == null) _reg = new Regex(Agent, RegexOptions.IgnoreCase);

                var m = _reg.Match(input);
                if (m == null || !m.Success) return null;

                return m.Result(Name);
            }
        }
    }
}