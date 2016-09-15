/*
 * XCoder v5.1.4974.18563
 * 作者：nnhy/X2
 * 时间：2013-08-26 18:03:30
 * 版权：版权所有 (C) 新生命开发团队 2002~2013
*/
﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using NewLife.Log;
using NewLife.Security;
using NewLife.Web;
using XCode;
using XCode.Configuration;
using XCode.Membership;

namespace BBX.Entity
{
    ///// <summary>用户。该类等同User类，仅为了规避Web中的User命名</summary>
    //public class XUser : User { }

    /// <summary>用户</summary>
    public partial class User : EntityBase<User>, IManageUser
    {
        #region 对象操作﻿
        static User()
        {
            var fs = Meta.Factory.AdditionalFields;
            fs.Add(__.Posts);
            fs.Add(__.DigestPosts);
            fs.Add(__.OLTime);
            fs.Add(__.PageViews);
            fs.Add(__.Credits);

            fs.Add(__.ExtCredits1);
            fs.Add(__.ExtCredits2);
            fs.Add(__.ExtCredits3);
            fs.Add(__.ExtCredits4);
            fs.Add(__.ExtCredits5);
            fs.Add(__.ExtCredits6);
            fs.Add(__.ExtCredits7);
            fs.Add(__.ExtCredits8);

            Meta.SingleCache.AllowNull = false;
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            this.TrimField();
        }

        protected override int OnInsert()
        {
            base.OnInsert();

            var field = Field;
            field.Uid = ID;
            return field.Insert();
        }

        protected override int OnUpdate()
        {
            Field.Update();

            return base.OnUpdate();
        }

        protected override int OnDelete()
        {
            Field.Delete();

            var ot = OnlineTime.FindByUid(ID);
            if (ot != null) ot.Delete();

            var pls = Poll.FindAllByUid(ID);
            pls.Delete();

            var favs = Favorite.FindAllByUid(ID);
            favs.Delete();

            var mods = Moderator.FindAllByUid(ID);
            mods.Delete();

            // 更新用户总数
            var st = Statistic.Current;
            st.TotalUsers--;

            // 更新最后用户
            var user = FindLast();
            if (user != null)
            {
                st.LastUserID = user.ID;
                st.LastUserName = user.Name;
            }

            st.Save();

            return base.OnDelete();
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
            //if (isNew || Dirtys[__.Name]) CheckExist(__.Name);

            if (!HasDirty) return;

            var ip = WebHelper.UserHost;
            if (isNew)
            {
                if (!Dirtys[__.RegIP] && !String.IsNullOrEmpty(ip)) RegIP = ip;
                if (!Dirtys[__.JoinDate]) JoinDate = DateTime.Now;
                // 可能有别的目的保存用户对象，所以在Update里面不更新这两个字段，但是Insert里面可以更新
                if (!Dirtys[__.LastVisit]) LastVisit = DateTime.Now;
                if (!Dirtys[__.LastActivity]) LastActivity = DateTime.Now;
                //if (!Dirtys[__.LastPost]) LastPost = DateTime.Now;
            }
            if (!Dirtys[__.LastIP] && !String.IsNullOrEmpty(ip)) LastIP = ip;
            //if (!Dirtys[__.LastVisit]) LastVisit = DateTime.Now;
            //if (!Dirtys[__.LastActivity]) LastActivity = DateTime.Now;
            //if (!Dirtys[__.LastPost]) LastPost = DateTime.Now;
        }

        //protected override void OnLoad()
        //{
        //    base.OnLoad();

        //    // 干掉两头空格
        //    if (!String.IsNullOrEmpty(Name)) Name = Name.Trim();
        //    if (!String.IsNullOrEmpty(NickName)) NickName = NickName.Trim();
        //}

        /// <summary>首次连接数据库时初始化数据，仅用于实体类重载，用户不应该调用该方法</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void InitData()
        {
            base.InitData();

            // InitData一般用于当数据表没有数据时添加一些默认数据，该实体类的任何第一次数据库操作都会触发该方法，默认异步调用
            // Meta.Count是快速取得表记录数
            if (Meta.Count > 0) return;

            // 需要注意的是，如果该方法调用了其它实体类的首次数据库操作，目标实体类的数据初始化将会在同一个线程完成
            if (XTrace.Debug) XTrace.WriteLine("开始初始化{0}[{1}]数据……", typeof(User).Name, Meta.Table.DataTable.DisplayName);

            var entity = new User();
            entity.Name = "admin";
            entity.NickName = "管理员";
            entity.Password = "admin".MD5();
            entity.AdminID = 1;
            entity.GroupID = 1;
            //entity.GroupExpiry = 0;
            entity.Insert();

            if (XTrace.Debug) XTrace.WriteLine("完成初始化{0}[{1}]数据！", typeof(User).Name, Meta.Table.DataTable.DisplayName);
        }


        ///// <summary>已重载。基类先调用Valid(true)验证数据，然后在事务保护内调用OnInsert</summary>
        ///// <returns></returns>
        //public override Int32 Insert()
        //{
        //    return base.Insert();
        //}

        ///// <summary>已重载。在事务保护范围内处理业务，位于Valid之后</summary>
        ///// <returns></returns>
        //protected override Int32 OnInsert()
        //{
        //    return base.OnInsert();
        //}
        #endregion

        #region 扩展属性﻿
        private List<String> hasLoad = new List<String>();
        private UserField _Field;
        /// <summary>扩展字段</summary>
        public UserField Field
        {
            get
            {
                if (_Field == null && !hasLoad.Contains("Field"))
                {
                    _Field = UserField.FindByUid(ID);
                    if (_Field == null) _Field = new UserField();
                    hasLoad.Add("Field");
                }
                return _Field;
            }
            set { _Field = value; }
        }

        IUserField IUser.Field { get { return Field; } }

        /// <summary>属性说明</summary>
        public String Ignorepm { get { return Field != null ? Field.Ignorepm + "" : ""; } set { if (Field != null)Field.Ignorepm = value; } }

        /// <summary>注册地址</summary>
        public String RegAddress
        {
            get
            {
                if (RegIP.IsNullOrEmpty()) return null;

                try
                {
                    return IPAddress.Parse(RegIP).GetAddress();
                }
                catch { return null; }
            }
        }
        #endregion

        #region 用户组属性
        private UserGroup _Group;
        /// <summary>用户组</summary>
        public UserGroup Group
        {
            get
            {
                if (_Group == null && !hasLoad.Contains("Group"))
                {
                    _Group = UserGroup.FindByID(GroupID);
                    hasLoad.Add("Group");
                }
                return _Group;
            }
            set { _Group = value; }
        }

        /// <summary>用户组标题，带彩色</summary>
        public String GroupTitle
        {
            get
            {
                if (Group == null) return null;

                if (String.IsNullOrEmpty(Group.Color))
                    return Group.GroupTitle;
                else
                    return String.Format("<font color='{1}'>{0}</font>", Group.GroupTitle, Group.Color);
            }
        }

        /// <summary>在线图片</summary>
        String IUser.OnlineImage { get { return Group == null ? null : Group.OnlineImage; } }
        #endregion

        #region 扩展查询﻿
        /// <summary>根据邮件查找</summary>
        /// <param name="email">邮件</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static EntityList<User> FindAllByEmail(String email)
        {
            if (Meta.Count >= 1000)
                return FindAll(__.Email, email);
            else // 实体缓存
                return Meta.Cache.Entities.FindAll(__.Email, email);
        }

        public static EntityList<User> FindAllByGroupID(Int32 groupid)
        {
            if (Meta.Count >= 1000)
                return FindAll(__.GroupID, groupid);
            else // 实体缓存
                return Meta.Cache.Entities.FindAll(__.GroupID, groupid);
        }

        public static EntityList<User> FindAllByGroupID(String gids, Int32 startid = 0, Int32 max = 0)
        {
            var exp = _.GroupID.In(gids.SplitAsInt());
            if (startid > 0) exp &= _.ID > startid;
            return FindAll(exp, _.ID.Asc(), null, 0, max);
        }

        /// <summary>根据登录账户、用户安全提问码、邮件查找</summary>
        /// <param name="name">登录账户</param>
        /// <param name="secques">用户安全提问码</param>
        /// <param name="email">邮件</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static EntityList<User> FindAllByNameAndSecquesAndEmail(String name, String secques, String email)
        {
            if (Meta.Count >= 1000)
                return FindAll(new String[] { _.Name, _.Secques, _.Email }, new Object[] { name, secques, email });
            else // 实体缓存
                return Meta.Cache.Entities.FindAll(e => e.Name == name && e.Secques == secques && e.Email == email);
        }

        /// <summary>根据操作人、用户组查找</summary>
        /// <param name="adminid">操作人</param>
        /// <param name="groupid">用户组</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static EntityList<User> FindAllByAdminIDAndGroupID(Int32 adminid, Int32 groupid)
        {
            if (Meta.Count >= 1000)
                return FindAll(new String[] { _.AdminID, _.GroupID }, new Object[] { adminid, groupid });
            else // 实体缓存
                return Meta.Cache.Entities.FindAll(e => e.AdminID == adminid && e.GroupID == groupid);
        }

        /// <summary>根据登录账户、登录密码查找</summary>
        /// <param name="name">登录账户</param>
        /// <param name="password">登录密码</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static EntityList<User> FindAllByNameAndPassword(String name, String password)
        {
            if (Meta.Count >= 1000)
                return FindAll(new String[] { _.Name, _.Password }, new Object[] { name, password });
            else // 实体缓存
                return Meta.Cache.Entities.FindAll(e => e.Name == name && e.Password == password);
        }

        /// <summary>根据编号查找</summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static User FindByID(Int32 id)
        {
            if (id <= 0) return null;

            //if (Meta.Count >= 1000)
            //    return Find(__.ID, id);
            //else // 实体缓存
            //    return Meta.Cache.Entities.Find(__.ID, id);
            // 单对象缓存
            //return Meta.SingleCache[id];
            // 临时解决单对象缓存带来的问题
            var user = Meta.SingleCache[id];
            if (user == null)
            {
                user = Find(__.ID, id);
                if (user != null) XTrace.WriteLine("单对象缓存有问题，查不到ID={0}的用户，而直接数据库查询能查到", id);
            }
            return user;
        }

        public static EntityList<User> FindAllByIDs(String ids)
        {
            return FindAll(_.ID.In(ids.SplitAsInt()), null, null, 0, 0);
        }

        public static EntityList<User> FindAllByIDs(Int32[] ids)
        {
            return FindAll(_.ID.In(ids), null, null, 0, 0);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static User FindByName(String name)
        {
            if (Meta.Count >= 1000)
                return Find(__.Name, name);
            else // 实体缓存
                return Meta.Cache.Entities.Find(__.Name, name);
        }

        public static EntityList<User> FindAllByNames(String[] names)
        {
            return FindAll(_.Name.In(names), null, null, 0, 0);
        }

        public static User FindByEmail(String email)
        {
            if (Meta.Count >= 1000)
                return Find(__.Email, email);
            else // 实体缓存
                return Meta.Cache.Entities.Find(__.Email, email);
        }

        /// <summary>根据登录账户、登录密码、用户安全提问码查找</summary>
        /// <param name="name">登录账户</param>
        /// <param name="password">登录密码</param>
        /// <param name="secques">用户安全提问码</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static EntityList<User> FindAllByNameAndPasswordAndSecques(String name, String password, String secques)
        {
            if (Meta.Count >= 1000)
                return FindAll(new String[] { _.Name, _.Password, _.Secques }, new Object[] { name, password, secques });
            else // 实体缓存
                return Meta.Cache.Entities.FindAll(e => e.Name == name && e.Password == password && e.Secques == secques);
        }

        /// <summary>根据注册IP查找</summary>
        /// <param name="regip">注册IP</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static EntityList<User> FindAllByRegIP(String regip)
        {
            if (Meta.Count >= 1000)
                return FindAll(__.RegIP, regip);
            else // 实体缓存
                return Meta.Cache.Entities.FindAll(__.RegIP, regip);
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static User FindByRegIP(String regip)
        {
            if (Meta.Count >= 1000)
                return Find(_.RegIP, regip);
            else // 实体缓存
                return Meta.Cache.Entities.Find(__.RegIP, regip);
        }

        /// <summary>根据登录账户查找</summary>
        /// <param name="name">登录账户</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static EntityList<User> FindAllByName(String name)
        {
            if (Meta.Count >= 1000)
                return FindAll(__.Name, name);
            else // 实体缓存
                return Meta.Cache.Entities.FindAll(__.Name, name);
        }

        public static EntityList<User> FindAllByIdList(String idlist)
        {
            return FindAll(_.ID.In(idlist.SplitAsInt()), null, null, 0, 0);
        }

        /// <summary>最后一个用户</summary>
        /// <returns></returns>
        public static User FindLast()
        {
            var list = FindAll(null, null, null, 0, 1);
            return list.Count > 0 ? list[0] : null;
        }

        public static EntityList<User> GetUsers(int start_uid, int end_uid)
        {
            //return DatabaseProvider.GetInstance().GetUsers(start_uid, end_uid);
            return FindAll(_.ID >= start_uid & _.ID <= end_uid, null, null, 0, 0);
        }
        #endregion

        #region 高级查询
        public static EntityList<User> Search(String userNames, Int32 start = 0, Int32 max = 0)
        {
            var exp = new WhereExpression();
            var arr = userNames.Split(",");
            foreach (var item in arr)
            {
                exp |= _.Name.Contains(item);
            }

            return FindAll(exp, null, null, start, max);
        }

        public static EntityList<User> GetUsersRank(int count, Int32 postTableId, string type)
        {
            if (postTableId <= 0) postTableId = 1;

            if (type == null) return null;

            //string commandText;
            switch (type)
            {
                case "posts":
                    //commandText = string.Format("SELECT TOP {0} [username], [uid], [posts] FROM [{1}users] WHERE [posts]>0 ORDER BY [posts] DESC, [uid]", count, TablePrefix);
                    return FindAll(_.Posts > 0, _.Posts.Desc(), null, 0, count);

                case "digestposts":
                    //commandText = string.Format("SELECT TOP {0} [username], [uid], [digestposts] FROM [{1}users] WHERE [digestposts]>0 ORDER BY [digestposts] DESC, [uid]", count, TablePrefix);
                    return FindAll(_.DigestPosts > 0, _.DigestPosts.Desc(), null, 0, count);

                case "thismonth":
                    //commandText = string.Format("SELECT DISTINCT TOP {0} [poster] AS [username], [posterid] AS [uid], COUNT(pid) AS [posts] FROM [{1}posts{2}] WHERE [postdatetime]>='{3}' AND [invisible]=0 AND [posterid]>0 GROUP BY [poster], [posterid] ORDER BY [posts] DESC", new object[]
                    //    {
                    //        count,
                    //        TablePrefix,
                    //        postTableId,
                    //        DateTime.Now.AddDays(-30.0).ToString("yyyy-MM-dd")
                    //    });
                    return GroupByTime(postTableId, DateTime.Now.Date.AddDays(-30), count);

                case "thisweek":
                    //commandText = string.Format("SELECT DISTINCT TOP {0} [poster] AS [username], [posterid] AS [uid], COUNT(pid) AS [posts] FROM [{1}posts{2}] WHERE [postdatetime]>='{3}' AND [invisible]=0 AND [posterid]>0 GROUP BY [poster], [posterid] ORDER BY [posts] DESC", new object[]
                    //    {
                    //        count,
                    //        TablePrefix,
                    //        postTableId,
                    //        DateTime.Now.AddDays(-7.0).ToString("yyyy-MM-dd")
                    //    });
                    //break;
                    return GroupByTime(postTableId, DateTime.Now.Date.AddDays(-7), count);

                case "today":
                    //commandText = string.Format("SELECT DISTINCT TOP {0} [poster] AS [username], [posterid] AS [uid], COUNT(pid) AS [posts] FROM [{1}posts{2}] WHERE [postdatetime]>='{3}' AND [invisible]=0 AND [posterid]>0 GROUP BY [poster], [posterid] ORDER BY [posts] DESC", new object[]
                    //    {
                    //        count,
                    //        TablePrefix,
                    //        postTableId,
                    //        DateTime.Now.ToString("yyyy-MM-dd")
                    //    });
                    //break;
                    return GroupByTime(postTableId, DateTime.Now.Date, count);

                case "credits":
                    return FindAll(null, _.Credits.Desc(), null, 0, count);

                case "extcredits1":
                    return FindAll(null, _.ExtCredits1.Desc(), null, 0, count);

                case "extcredits2":
                    return FindAll(null, _.ExtCredits2.Desc(), null, 0, count);

                case "extcredits3":
                    return FindAll(null, _.ExtCredits3.Desc(), null, 0, count);

                case "extcredits4":
                    return FindAll(null, _.ExtCredits4.Desc(), null, 0, count);

                case "extcredits5":
                    return FindAll(null, _.ExtCredits5.Desc(), null, 0, count);

                case "extcredits6":
                    return FindAll(null, _.ExtCredits6.Desc(), null, 0, count);

                case "extcredits7":
                    return FindAll(null, _.ExtCredits7.Desc(), null, 0, count);

                case "extcredits8":
                    //commandText = string.Format("SELECT TOP {0} [username], [uid], [extcredits8] FROM [{1}users] ORDER BY [extcredits8] DESC, [uid]", count, TablePrefix);
                    return FindAll(null, _.ExtCredits8.Desc(), null, 0, count);

                default:
                    return null;
            }
        }

        public static EntityList<User> GroupByTime(Int32 postTableId, DateTime start, Int32 count)
        {
            var us = new EntityList<User>();

            var ps = Post.GetPosterIDStat(start, count);
            if (ps.Count < 1) return us;

            //foreach (var p in ps)
            //{
            //    us.Add(new User { ID = p.PosterID, Name = p.Poster, Posts = p.ID });
            //}
            us = FindAll(_.ID.In(ps.Keys), null, null, 0, 0);
            foreach (var item in us)
            {
                var posts = 0;
                if (ps.TryGetValue(item.ID, out posts))
                    item.Posts = posts;
                else
                    item.Posts = 0;

                // 取消所有脏数据，避免数据被错误保存
                item.SetDirty(false);
            }

            return us;
        }

        public static List<IUser> GetUserList(int pagesize, int pageindex, string column, string ordertype)
        {
            string[] array = new string[]
            {
                "name",
                "credits",
                "posts",
                "admin",
                "lastactivity",
                "joindate",
                "oltime"
            };
            int num = Array.IndexOf<string>(array, column);
            column = (num > 6 || num < 0) ? "id" : array[num];

            return FindAll(null, column + " " + ordertype, null, (pageindex - 1) * pagesize, pagesize).Cast<IUser>().ToList();
        }

        public static int GetTodayNewMemberCount()
        {
            //SELECT COUNT(1) FROM [{0}users] WHERE [joindate]>='{1}'
            return FindCount(_.JoinDate > DateTime.Now.Date, null, null, 0, 0);
        }

        public static int GetAdminCount()
        {
            //SELECT COUNT(1) FROM [{0}users] WHERE [adminid]>0
            return FindCount(_.AdminID > 0, null, null, 0, 0);
        }

        public static int GetNonPostMemCount()
        {
            //SELECT COUNT(1) FROM [{0}users] WHERE [posts]=0
            return FindCount(_.Posts == 0, null, null, 0, 0);
        }

        public static EntityList<User> GetEmailListByGroupidList(Int32[] gids)
        {
            return FindAll(_.GroupID.In(gids) & _.Email.NotIsNullOrEmpty(), null, null, 0, 0);
        }

        public static EntityList<User> AuditNewUserClear(string searchUser, Int32 regBefore, string regIp)
        {
            var exp = new WhereExpression();
            if (!searchUser.IsNullOrWhiteSpace()) exp &= _.Name.Contains(searchUser);
            if (!regIp.IsNullOrWhiteSpace()) exp &= _.RegIP.Contains(regIp);
            if (regBefore > 0) exp &= _.JoinDate >= DateTime.Now.AddDays(-regBefore);

            return FindAll(exp, null, null, 0, 0);
        }

        public static String SearchWhere(bool isLike, bool isPostDateTime, string userName, string nickName, Int32 userGroup, string email, Int32 creditsStart, Int32 creditsEnd, string lastIp, Int32 posts, Int32 digestPosts, string uid, DateTime joinDateStart, DateTime joinDateEnd)
        {
            var exp = new WhereExpression();
            if (isLike)
            {
                if (!userName.IsNullOrWhiteSpace()) exp &= _.Name.Contains(userName);
                if (!nickName.IsNullOrWhiteSpace()) exp &= _.NickName.Contains(nickName);
            }
            else
            {
                if (!userName.IsNullOrWhiteSpace()) exp &= _.Name == userName;
                if (!nickName.IsNullOrWhiteSpace()) exp &= _.NickName == nickName;
            }

            if (userGroup > 0) exp &= _.GroupID == userGroup;
            if (!email.IsNullOrWhiteSpace()) exp &= _.Email.Contains(email);
            if (creditsStart > 0) exp &= _.Credits >= creditsStart;
            if (creditsEnd > 0) exp &= _.Credits <= creditsEnd;
            if (!lastIp.IsNullOrWhiteSpace()) exp &= _.LastIP.Contains(lastIp);
            if (posts > 0) exp &= _.Posts >= posts;
            if (digestPosts > 0) exp &= _.DigestPosts >= digestPosts;

            if (!uid.IsNullOrWhiteSpace()) exp &= _.ID.In(uid.SplitAsInt());

            if (isPostDateTime) exp &= _.JoinDate.Between(joinDateStart, joinDateEnd);

            return exp;
        }
        #endregion

        #region 扩展操作
        public static void DeleteAuditUser()
        {
            //Users.DeleteUsers(Users.GetUidListByUserGroupId(8));
            var list = FindAllByGroupID(8);
            list.Delete();
        }
        #endregion

        #region 业务
        public User CombinationFrom(User user)
        {
            if (user == null) return this;

            throw new NotImplementedException("从用户user合并到当前用户，未实现！");

            //return this;
        }

        /// <summary>删除用户</summary>
        /// <param name="uid"></param>
        /// <param name="delposts"></param>
        /// <param name="delpms"></param>
        /// <returns></returns>
        public Boolean Delete(Boolean delposts, Boolean delpms)
        {
            var user = this;

            using (var tran = new EntityTransaction<User>())
            {
                user.Delete();

                var tps = Topic.FindAllByPosterID(user.ID);
                var lps = Topic.FindAllByLastPostID(user.ID);
                if (delposts)
                {
                    tps.Delete();
                    lps.Delete();

                    var ps = Post.FindAllByPosterID(user.ID);
                    ps.Delete();
                }
                else
                {
                    tps.SetItem(Topic._.Poster, "该用户已被删除");
                    lps.SetItem(Topic._.LastPoster, "该用户已被删除");

                    tps.Save();
                    lps.Save();
                }

                var pms1 = ShortMessage.FindAllByMsgtoID(user.ID);
                var pms2 = ShortMessage.FindAllByMsgfromID(user.ID);
                if (delpms)
                {
                    pms1.Delete();
                    pms2.Delete();
                }
                else
                {
                    pms1.SetItem(ShortMessage._.Msgto, "该用户已被删除");
                    pms2.SetItem(ShortMessage._.Msgfrom, "该用户已被删除");

                    pms1.Save();
                    pms2.Save();
                }

                tran.Commit();
            }

            return true;
        }

        public static int SetUserOnlineState(Int32 uid, Boolean online)
        {
            //string commandText = string.Format("UPDATE [{0}users] SET [onlinestate]={1},[lastactivity]=GETDATE(),[lastvisit]=GETDATE() WHERE [uid]={2}", TablePrefix, onlineState, uid);
            //return DbHelper.ExecuteNonQuery(CommandType.Text, commandText);

            var user = FindByID(uid);
            if (user == null) return -1;

            user.OnlineState = online ? 1 : 0;
            user.LastActivity = DateTime.Now;
            user.LastVisit = DateTime.Now;
            user.LastIP = WebHelper.UserHost;
            return user.Save();
        }

        public static void ClearUsersAuthstr(string uidList)
        {
            if (String.IsNullOrEmpty(uidList)) return;

            var list = FindAll(_.ID.In(uidList.SplitAsInt()), null, null, 0, 0);
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    item.Field.Authstr = null;
                    item.Field.Update();
                }
            }
        }

        public static string GetUidListByUserGroupId(int userGroupId)
        {
            //string text = "";
            //foreach (DataRow dataRow in Users.GetUserListByGroupid(userGroupId).Rows)
            //{
            //    text = text + dataRow["uid"].ToString() + ",";
            //}
            //return text.TrimEnd(',');
            var list = FindAll(__.GroupID, userGroupId);
            var sb = new StringBuilder();
            foreach (var item in list)
            {
                if (sb.Length > 0) sb.Append(",");
                sb.Append(item.ID);
            }
            return sb.ToString();
        }
        #endregion

        #region 登录
        public static User Login(String username, String password, bool originalpassword = true, int questionid = 0, String answer = null)
        {
            //return BBX.Data.Users.CheckPasswordAndSecques(username, password, originalpassword, ForumUtils.GetUserSecques(questionid, answer));

            var user = Check(username, password, originalpassword);
            if (user == null) return null;

            if (questionid > 0 && user.Secques.EqualIgnoreCase(GetUserSecques(questionid, answer))) return null;

            // 在登录时修正以前的QQ#用户名
            if (user.Name.StartsWith("QQ#") && !String.IsNullOrEmpty(user.NickName))
            {
                if (FindCount(__.Name, user.NickName) <= 0)
                {
                    user.Name = user.NickName;
                }
            }

            return user;
        }

        public static User Check(String username, String password, Boolean originalpassword = true)
        {
            var user = FindByName(username);
            if (user == null) return null;

            if (originalpassword) password = password.MD5();
            if (!user.Password.EqualIgnoreCase(password)) return null;

            return user;
        }

        public static User Check(Int32 uid, String password, Boolean originalpassword = true)
        {
            var user = FindByID(uid);
            if (user == null)
            {
                XTrace.WriteLine("找不到ID={0}的用户，可能是单对象缓存有问题", uid);
                return null;
            }

            if (originalpassword) password = password.MD5();
            if (!user.Password.EqualIgnoreCase(password)) return null;

            return user;
        }

        public static string GetUserSecques(int questionid, string answer)
        {
            if (questionid <= 0) return "";

            return (answer + questionid.ToString().MD5()).MD5().Substring(15, 8);
        }
        #endregion

        #region 密码取回
        public static User CheckEmailAndSecques(string username, string email, int questionid, string answer)
        {
            var user = FindByName(username);
            if (user == null) return null;

            if (!user.Email.EqualIgnoreCase(email)) return null;

            if (questionid > 0 && user.Secques != GetUserSecques(questionid, answer)) return null;

            return user;
        }
        #endregion

        #region 积分
        /// <summary>更新指定积分组的积分</summary>
        /// <param name="uid"></param>
        /// <param name="extid"></param>
        /// <param name="pos"></param>
        public static void UpdateUserExtCredits(int uid, int extid, float pos)
        {
            //UPDATE [{0}users] SET [extcredits{1}]=[extcredits{1}] + {2} WHERE [uid]={3}
            var user = FindByID(uid);
            if (user != null)
            {
                FieldItem fi = Meta.Table.FindByName("ExtCredits" + extid);
                if (fi != null)
                {
                    user.SetItem(fi.Name, pos + (float)user[fi.Name]);
                    user.Update();
                }
            }
        }

        public static void UpdateUserCredits(int uid)
        {
            //UPDATE [dnt_users] SET [credits] = extcredits1 + posts + digestposts* 5 WHERE [uid] = @uid
            var user = FindByID(uid);
            if (user == null) return;

            user.Credits = (Int32)user.ExtCredits1 + user.Posts + user.DigestPosts * 5;
            user.Update();
        }

        //public static void UpdateUserExtCredits(int uid, float[] values)
        //{
        //    //UPDATE [{0}users] SET
        //    //  [extcredits1]=[extcredits1] + @extcredits1, 
        //    //  [extcredits2]=[extcredits2] + @extcredits2, 
        //    //  [extcredits3]=[extcredits3] + @extcredits3, 
        //    //  [extcredits4]=[extcredits4] + @extcredits4, 
        //    //  [extcredits5]=[extcredits5] + @extcredits5, 
        //    //  [extcredits6]=[extcredits6] + @extcredits6, 
        //    //  [extcredits7]=[extcredits7] + @extcredits7, 
        //    //  [extcredits8]=[extcredits8] + @extcredits8  WHERE [uid]=@uid

        //    var user = FindByID(uid);
        //    if (user == null) return;

        //    for (int i = 0; i < values.Length; i++)
        //    {
        //        var name = "ExtCredits" + (i + 1);
        //        user.SetItem(name, values[i] + (float)user[name]);
        //    }
        //    user.Update();
        //}

        public static void UpdateUserExtCredits(int uid, float[] values, int pos = 1, int mount = 1)
        {
            if (uid <= 0) return;
            if (values == null || values.Length <= 0) return;

            if (pos != 1 || mount != 1)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = values[i] * pos * mount;
                }
            }

            //UPDATE [{0}users] SET
            //  [extcredits1]=[extcredits1] + @extcredits1, 
            //  [extcredits2]=[extcredits2] + @extcredits2, 
            //  [extcredits3]=[extcredits3] + @extcredits3, 
            //  [extcredits4]=[extcredits4] + @extcredits4, 
            //  [extcredits5]=[extcredits5] + @extcredits5, 
            //  [extcredits6]=[extcredits6] + @extcredits6, 
            //  [extcredits7]=[extcredits7] + @extcredits7, 
            //  [extcredits8]=[extcredits8] + @extcredits8  WHERE [uid]=@uid

            //UpdateUserExtCredits(uid, values);

            var user = FindByID(uid);
            if (user == null) return;

            // 批量更新积分
            for (int i = 1; i <= 8 && i <= values.Length; i++)
            {
                FieldItem fi = Meta.Table.FindByName("ExtCredits" + i);
                if (fi != null)
                {
                    user.SetItem(fi.Name, values[i - 1] + (float)user[fi.Name]);
                }
            }
            user.Update();
        }

        public static bool CheckUserCreditsIsEnough(int uid, float[] values)
        {
            var user = FindByID(uid);
            if (user == null) return false;

            for (int i = 0; i < values.Length; i++)
            {
                var name = "ExtCredits" + (i + 1);
                var f = values[i];
                if (f < 0) f = -f;
                if (((float)user[name]) < f) return false;
            }
            return true;
        }

        public static bool CheckUserCreditsIsEnough(int uid, float[] values, int pos, int mount)
        {
            if (pos >= 0) return true;

            var user = FindByID(uid);
            if (user == null) return false;

            for (int i = 0; i < values.Length; i++)
            {
                var name = "ExtCredits" + (i + 1);
                var f = values[i] * mount;
                if (f < 0)
                    f = -f;
                else
                    f = 0;

                if (((float)user[name]) < f) return false;
            }
            return true;
        }
        #endregion

        #region 用户扩展字段接口
        Int32 IUser.Uid { get { return Field.Uid; } set { Field.Uid = value; } }

        String IUser.Website { get { return Field.Website; } set { Field.Website = value; } }

        String IUser.Icq { get { return Field.Icq; } set { Field.Icq = value; } }

        String IUser.Qq { get { return Field.qq; } set { Field.qq = value; } }

        String IUser.Yahoo { get { return Field.Yahoo; } set { Field.Yahoo = value; } }

        String IUser.Msn { get { return Field.Msn; } set { Field.Msn = value; } }

        String IUser.Skype { get { return Field.Skype; } set { Field.Skype = value; } }

        String IUser.Location { get { return Field.Location; } set { Field.Location = value; } }

        String IUser.Customstatus { get { return Field.Customstatus; } set { Field.Customstatus = value; } }

        String IUser.Avatar { get { return Field.Avatar; } set { Field.Avatar = value; } }

        Int32 IUser.Avatarwidth { get { return Field.Avatarwidth; } set { Field.Avatarwidth = value; } }

        Int32 IUser.Avatarheight { get { return Field.Avatarheight; } set { Field.Avatarheight = value; } }

        String IUser.Medals { get { return Field.Medals; } set { Field.Medals = value; } }

        String IUser.Bio { get { return Field.Bio; } set { Field.Bio = value; } }

        String IUser.Signature { get { return Field.Signature; } set { Field.Signature = value; } }

        String IUser.Sightml { get { return Field.Sightml; } set { Field.Sightml = value; } }

        String IUser.Authstr { get { return Field.Authstr; } set { Field.Authstr = value; } }

        DateTime IUser.AuthTime { get { return Field.AuthTime; } set { Field.AuthTime = value; } }

        SByte IUser.Authflag { get { return Field.Authflag; } set { Field.Authflag = value; } }

        String IUser.RealName { get { return Field.RealName; } set { Field.RealName = value; } }

        String IUser.Idcard { get { return Field.Idcard; } set { Field.Idcard = value; } }

        String IUser.Mobile { get { return Field.Mobile; } set { Field.Mobile = value; } }

        String IUser.Phone { get { return Field.Phone; } set { Field.Phone = value; } }

        String IUser.Ignorepm { get { return Field.Ignorepm; } set { Field.Ignorepm = value; } }
        #endregion

        #region 统计

        //public static EntityList<User> GetUsersRank(int count, Int32 postTableId, string type)
        //{
        //    if (String.IsNullOrEmpty(type)) return null;

        //    // 时间类统计
        //    var start = DateTime.MinValue;
        //    switch (type)
        //    {
        //        case "thismonth":
        //            start = DateTime.Now.Date.AddDays(-30);
        //            break;

        //        case "thisweek":
        //            start = DateTime.Now.Date.AddDays(-7);
        //            break;

        //        case "today":
        //            start = DateTime.Now.Date;
        //            break;
        //    }
        //    if (start > DateTime.MinValue)
        //    {
        //        var ps = Post.GetPosterIDStat(start, count);
        //        var list = FindAll(_.ID.In(ps.Keys), null, null, 0, 0);
        //        foreach (var item in list)
        //        {
        //            var posts = 0;
        //            if (ps.TryGetValue(item.ID, out posts))
        //                item.Posts = posts;
        //            else
        //                item.Posts = 0;

        //            // 取消所有脏数据，避免数据被错误保存
        //            item.SetDirty(false);
        //        }

        //        return list;
        //    }

        //    //string commandText;
        //    WhereExpression exp = null;
        //    ConcatExpression order = null;
        //    switch (type)
        //    {
        //        case "posts":
        //            //commandText = string.Format("SELECT TOP {0} [username], [uid], [posts] FROM [{1}users] WHERE [posts]>0 ORDER BY [posts] DESC, [uid]", count, TablePrefix);
        //            exp = _.Posts > 0;
        //            order = _.Posts.Desc() & _.ID.Asc();
        //            break;

        //        case "digestposts":
        //            //commandText = string.Format("SELECT TOP {0} [username], [uid], [digestposts] FROM [{1}users] WHERE [digestposts]>0 ORDER BY [digestposts] DESC, [uid]", count, TablePrefix);
        //            exp = _.DigestPosts > 0;
        //            order = _.Posts.Desc() & _.ID.Asc();
        //            break;

        //        case "thismonth":
        //            /*commandText = string.Format("SELECT DISTINCT TOP {0} [poster] AS [username], [posterid] AS [uid], COUNT(pid) AS [posts] FROM [{1}posts{2}] WHERE [postdatetime]>='{3}' AND [invisible]=0 AND [posterid]>0 GROUP BY [poster], [posterid] ORDER BY [posts] DESC", new object[]
        //                {
        //                    count,
        //                    TablePrefix,
        //                    postTableId,
        //                    DateTime.Now.AddDays(-30.0).ToString("yyyy-MM-dd")
        //                });*/
        //            exp = _.Posts > 0;
        //            order = _.Posts.Desc() & _.ID.Asc();
        //            break;

        //        case "thisweek":
        //            /*commandText = string.Format("SELECT DISTINCT TOP {0} [poster] AS [username], [posterid] AS [uid], COUNT(pid) AS [posts] FROM [{1}posts{2}] WHERE [postdatetime]>='{3}' AND [invisible]=0 AND [posterid]>0 GROUP BY [poster], [posterid] ORDER BY [posts] DESC", new object[]
        //                {
        //                    count,
        //                    TablePrefix,
        //                    postTableId,
        //                    DateTime.Now.AddDays(-7.0).ToString("yyyy-MM-dd")
        //                });*/
        //            exp = _.Posts > 0;
        //            order = _.Posts.Desc() & _.ID.Asc();
        //            break;

        //        case "today":
        //            /*commandText = string.Format("SELECT DISTINCT TOP {0} [poster] AS [username], [posterid] AS [uid], COUNT(pid) AS [posts] FROM [{1}posts{2}] WHERE [postdatetime]>='{3}' AND [invisible]=0 AND [posterid]>0 GROUP BY [poster], [posterid] ORDER BY [posts] DESC", new object[]
        //                {
        //                    count,
        //                    TablePrefix,
        //                    postTableId,
        //                    DateTime.Now.ToString("yyyy-MM-dd")
        //                });*/
        //            exp = _.Posts > 0;
        //            order = _.Posts.Desc() & _.ID.Asc();
        //            break;

        //        case "credits":
        //            commandText = string.Format("SELECT TOP {0} [username], [uid], [credits] FROM [{1}users] ORDER BY [credits] DESC, [uid]", count, TablePrefix);
        //            exp = _.Posts > 0;
        //            order = _.Posts.Desc() & _.ID.Asc();
        //            break;

        //        case "extcredits1":
        //            //commandText = string.Format("SELECT TOP {0} [username], [uid], [extcredits1] FROM [{1}users] ORDER BY [extcredits1] DESC, [uid]", count, TablePrefix);
        //            exp = _.Posts > 0;
        //            order = _.Posts.Desc() & _.ID.Asc();
        //            break;

        //        case "extcredits2":
        //            commandText = string.Format("SELECT TOP {0} [username], [uid], [extcredits2] FROM [{1}users] ORDER BY [extcredits2] DESC, [uid]", count, TablePrefix);
        //            exp = _.Posts > 0;
        //            order = _.Posts.Desc() & _.ID.Asc();
        //            break;

        //        case "extcredits3":
        //            commandText = string.Format("SELECT TOP {0} [username], [uid], [extcredits3] FROM [{1}users] ORDER BY [extcredits3] DESC, [uid]", count, TablePrefix);
        //            exp = _.Posts > 0;
        //            order = _.Posts.Desc() & _.ID.Asc();
        //            break;

        //        case "extcredits4":
        //            commandText = string.Format("SELECT TOP {0} [username], [uid], [extcredits4] FROM [{1}users] ORDER BY [extcredits4] DESC, [uid]", count, TablePrefix);
        //            exp = _.Posts > 0;
        //            order = _.Posts.Desc() & _.ID.Asc();
        //            break;

        //        case "extcredits5":
        //            commandText = string.Format("SELECT TOP {0} [username], [uid], [extcredits5] FROM [{1}users] ORDER BY [extcredits5] DESC, [uid]", count, TablePrefix);
        //            break;

        //        case "extcredits6":
        //            commandText = string.Format("SELECT TOP {0} [username], [uid], [extcredits6] FROM [{1}users] ORDER BY [extcredits6] DESC, [uid]", count, TablePrefix);
        //            break;

        //        case "extcredits7":
        //            commandText = string.Format("SELECT TOP {0} [username], [uid], [extcredits7] FROM [{1}users] ORDER BY [extcredits7] DESC, [uid]", count, TablePrefix);
        //            break;

        //        case "extcredits8":
        //            commandText = string.Format("SELECT TOP {0} [username], [uid], [extcredits8] FROM [{1}users] ORDER BY [extcredits8] DESC, [uid]", count, TablePrefix);
        //            break;

        //        default:
        //            return null;
        //    }
        //    return DbHelper.ExecuteReader(CommandType.Text, commandText);
        //}
        #endregion

        #region IManageUser 成员
        /// <summary>编号</summary>
        object IManageUser.Uid { get { return ID; } }

        ///// <summary>账号</summary>
        //string IManageUser.Account { get { return Name; } set { Name = value; } }

        Boolean IManageUser.Enable { get { return true; } set { } }

        Boolean IManageUser.IsAdmin { get { return Group != null && Group.Is管理员; } set { } }

        ///// <summary>密码</summary>
        //string IManageUser.Password { get { return Password; } set { Password = value; } }

        //[NonSerialized]
        //IDictionary<String, Object> _Properties;
        ///// <summary>属性集合</summary>
        //IDictionary<String, Object> IManageUser.Properties
        //{
        //    get
        //    {
        //        if (_Properties == null)
        //        {
        //            var dic = new Dictionary<String, Object>();
        //            foreach (var item in Meta.FieldNames)
        //            {
        //                dic[item] = this[item];
        //            }
        //            foreach (var item in Extends)
        //            {
        //                dic[item.Key] = item.Value;
        //            }

        //            _Properties = dic;
        //        }
        //        return _Properties;
        //    }
        //}
        #endregion
    }

    public partial interface IUser
    {
        /// <summary>用户组</summary>
        UserGroup Group { get; }
        IUserField Field { get; }

        /// <summary>注册地址</summary>
        String RegAddress { get; }

        #region 属性
        /// <summary>U编号</summary>
        Int32 Uid { get; set; }

        /// <summary>公司网址</summary>
        String Website { get; set; }

        /// <summary>ICQ号码</summary>
        String Icq { get; set; }

        /// <summary>QQ号码（多个以空格隔开）</summary>
        String Qq { get; set; }

        /// <summary>雅虎通帐号</summary>
        String Yahoo { get; set; }

        /// <summary>MSN帐号</summary>
        String Msn { get; set; }

        /// <summary></summary>
        String Skype { get; set; }

        /// <summary>办理地点</summary>
        String Location { get; set; }

        /// <summary></summary>
        String Customstatus { get; set; }

        /// <summary>会员头像</summary>
        String Avatar { get; set; }

        /// <summary></summary>
        Int32 Avatarwidth { get; set; }

        /// <summary></summary>
        Int32 Avatarheight { get; set; }

        /// <summary>奖牌</summary>
        String Medals { get; set; }

        /// <summary>生物</summary>
        String Bio { get; set; }

        /// <summary>签字</summary>
        String Signature { get; set; }

        /// <summary></summary>
        String Sightml { get; set; }

        /// <summary></summary>
        String Authstr { get; set; }

        /// <summary></summary>
        DateTime AuthTime { get; set; }

        /// <summary></summary>
        SByte Authflag { get; set; }

        /// <summary>真实姓名</summary>
        String RealName { get; set; }

        /// <summary>身份证号码</summary>
        String Idcard { get; set; }

        /// <summary>手机号码</summary>
        String Mobile { get; set; }

        /// <summary>手机</summary>
        String Phone { get; set; }

        /// <summary></summary>
        String Ignorepm { get; set; }
        #endregion

        #region 用户组属性
        /// <summary>用户组标题，带彩色</summary>
        String GroupTitle { get; }

        /// <summary>在线图片</summary>
        String OnlineImage { get; }
        #endregion
    }
}