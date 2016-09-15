﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using NewLife.Web;
using XCode;
using XCode.Cache;
using XCode.DataAccessLayer;
using XCode.Membership;

namespace BBX.Entity
{
    public enum DateType
    {
        Minute = 1,
        Hour,
        Day,
        Week,
        Month,
        Year
    }

    /// <summary>帖子</summary>
    public partial class Post : EntityBase<Post>
    {
        #region 对象操作﻿
        /// <summary>查找主贴的缓存</summary>
        static SingleEntityCache<Int32, Post> _cache;

        static Post()
        {
            _cache = new SingleEntityCache<Int32, Post>();
            _cache.FindKeyMethod = id =>
            {
                var list = FindAll(_.Tid == id & _.Layer == 0, null, null, 0, 1);
                return list.Count > 0 ? list[0] : null;
            };
        }

        /// <summary>验证数据，通过抛出异常的方式提示验证失败。</summary>
        /// <param name="isNew"></param>
        public override void Valid(Boolean isNew)
        {
            // 这里验证参数范围，建议抛出参数异常，指定参数名，前端用户界面可以捕获参数异常并聚焦到对应的参数输入框
            //if (String.IsNullOrEmpty(Name)) throw new ArgumentNullException(_.Name, _.Name.DisplayName + "无效！");
            //if (!isNew && ID < 1) throw new ArgumentOutOfRangeException(_.ID, _.ID.DisplayName + "必须大于0！");

            // 没有修改数据时，不要乱改IP地址
            if (!HasDirty) return;

            // 建议先调用基类方法，基类方法会对唯一索引的数据进行验证
            base.Valid(isNew);

            if (isNew) FillDefault();

            if (!Dirtys[_.IP]) IP = WebHelper.UserHost;
        }

        ///// <summary>首次连接数据库时初始化数据，仅用于实体类重载，用户不应该调用该方法</summary>
        //[EditorBrowsable(EditorBrowsableState.Never)]
        //protected override void InitData()
        //{
        //    base.InitData();

        //    // InitData一般用于当数据表没有数据时添加一些默认数据，该实体类的任何第一次数据库操作都会触发该方法，默认异步调用
        //    // Meta.Count是快速取得表记录数
        //    if (Meta.Count > 0) return;

        //    // 需要注意的是，如果该方法调用了其它实体类的首次数据库操作，目标实体类的数据初始化将会在同一个线程完成
        //    if (XTrace.Debug) XTrace.WriteLine("开始初始化{0}PostS1数据……", typeof(PostS1).Name);

        //    var entity = new PostS1();
        //    entity.Pid = 0;
        //    entity.Fid = 0;
        //    entity.Tid = 0;
        //    entity.ParentID = 0;
        //    entity.Layer = 0;
        //    entity.Poster = "abc";
        //    entity.PosterID = 0;
        //    entity.Title = "abc";
        //    entity.PostDateTime = DateTime.Now;
        //    entity.Message = "abc";
        //    entity.IP = "abc";
        //    entity.LastEdit = "abc";
        //    entity.Invisible = 0;
        //    entity.Usesig = 0;
        //    entity.Htmlon = 0;
        //    entity.Smileyoff = 0;
        //    entity.Parseurloff = 0;
        //    entity.Bbcodeoff = 0;
        //    entity.Attachment = 0;
        //    entity.Rate = 0;
        //    entity.Ratetimes = 0;
        //    entity.Insert();

        //    if (XTrace.Debug) XTrace.WriteLine("完成初始化{0}PostS1数据！", typeof(PostS1).Name);
        //}


        ///// <summary>已重载。基类先调用Valid(true)验证数据，然后在事务保护内调用OnInsert</summary>
        ///// <returns></returns>
        //public override Int32 Insert()
        //{
        //    return base.Insert();
        //}

        /// <summary>已重载。在事务保护范围内处理业务，位于Valid之后</summary>
        /// <returns></returns>
        protected override Int32 OnInsert()
        {
            // 需要借助索引表来生成主键
            var pi = new PostId();
            pi.PostDateTime = PostDateTime;
            pi.Insert();

            ID = pi.ID;
            if (ParentID == 0) ParentID = ID;

            return base.OnInsert();
        }

        protected override int OnDelete()
        {
            var pi = PostId.FindByKey(ID);
            if (pi != null) pi.Delete();

            var rlog = RateLog.FindAllByPostID(ID);
            if (rlog != null) rlog.Delete();

            return base.OnDelete();
        }
        #endregion

        #region 扩展属性﻿
        private List<String> hasLoad = new List<String>();
        private XForum _Forum;
        /// <summary>论坛</summary>
        public IXForum Forum
        {
            get
            {
                if (_Forum == null && !hasLoad.Contains("Forum"))
                {
                    _Forum = XForum.FindByID(this.Fid);
                    hasLoad.Add("Forum");
                }
                return _Forum;
            }
            set { _Forum = value as XForum; }
        }

        /// <summary>论坛名称</summary>
        public String ForumName { get { return Forum == null ? null : Forum.Name; } }

        private Topic _Topic;
        /// <summary>所属主题</summary>
        public Topic Topic
        {
            get
            {
                if (_Topic == null && !hasLoad.Contains("Topic"))
                {
                    _Topic = Topic.FindByID(Tid);
                    hasLoad.Add("Topic");
                }
                return _Topic;
            }
            set { _Topic = value; }
        }

        private String _TopicTitle;
        /// <summary>主题标题</summary>
        public String TopicTitle
        {
            get
            {
                if (_TopicTitle == null && Topic != null) _TopicTitle = Topic.Title;
                return _TopicTitle;
            }
            set { _TopicTitle = value; }
        }

        private User _PostUser;
        /// <summary>发帖人</summary>
        public IUser PostUser
        {
            get
            {
                if (_PostUser == null && !hasLoad.Contains("PostUser"))
                {
                    _PostUser = User.FindByID(PosterID);
                    hasLoad.Add("PostUser");
                }
                return _PostUser;
            }
            set { _PostUser = value as User; }
        }

        private EntityList<Attachment> _Attachs;
        public EntityList<Attachment> Attachs
        {
            get
            {
                if (_Attachs == null && !Dirtys.ContainsKey("Attachs"))
                {
                    _Attachs = BBX.Entity.Attachment.FindAllByPid(ID);
                    Dirtys.Add("Attachs", true);
                }
                return _Attachs;
            }
        }

        public Int32 AttachCount { get { return Attachs.Count; } }

        private String _Html;
        /// <summary>Html表示</summary>
        public String Html { get { return _Html; } set { _Html = value; } }

        private Int32 _Id;
        /// <summary>用于显示用的楼层</summary>
        public Int32 Id { get { return _Id; } set { _Id = value; } }

        private Int32 _Diggs;
        /// <summary>支持数</summary>
        public Int32 Diggs { get { return _Diggs; } set { _Diggs = value; } }

        private Boolean _Digged;
        /// <summary>属性说明</summary>
        public Boolean Digged { get { return _Digged; } set { _Digged = value; } }

        private String _Medals;
        /// <summary>属性说明</summary>
        public String Medals { get { return _Medals; } set { _Medals = value; } }

        private Int32 _Stars;
        /// <summary>属性说明</summary>
        public Int32 Stars { get { return _Stars; } set { _Stars = value; } }

        private String _Status;
        /// <summary>属性说明</summary>
        public String Status { get { return _Status; } set { _Status = value; } }

        private String _Postnocustom;
        /// <summary>属性说明</summary>
        public String Postnocustom { get { return _Postnocustom; } set { _Postnocustom = value; } }

        private Int32 _Adindex;
        /// <summary>属性说明</summary>
        public Int32 Adindex { get { return _Adindex; } set { _Adindex = value; } }

        private Int32 _Debateopinion;
        /// <summary>属性说明</summary>
        public Int32 Debateopinion { get { return _Debateopinion; } set { _Debateopinion = value; } }
        #endregion

        #region 扩展查询﻿
        /// <summary>根据F编号、T编号、PosterID查找</summary>
        /// <param name="fid">F编号</param>
        /// <param name="tid">T编号</param>
        /// <param name="posterid"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static EntityList<Post> FindAllByFidAndTidAndPosterID(Int32 fid, Int32 tid, Int32 posterid)
        {
            if (Meta.Count >= 1000)
                return FindAll(new String[] { _.Fid, _.Tid, _.PosterID }, new Object[] { fid, tid, posterid });
            else // 实体缓存
                return Meta.Cache.Entities.FindAll(e => e.Fid == fid && e.Tid == tid && e.PosterID == posterid);
        }

        /// <summary>根据P编号、T编号、PosterID查找</summary>
        /// <param name="pid">P编号</param>
        /// <param name="tid">T编号</param>
        /// <param name="posterid"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static EntityList<Post> FindAllByPidAndTidAndPosterID(Int32 pid, Int32 tid, Int32 posterid)
        {
            if (Meta.Count >= 1000)
                return FindAll(new String[] { _.ID, _.Tid, _.PosterID }, new Object[] { pid, tid, posterid });
            else // 实体缓存
                return Meta.Cache.Entities.FindAll(e => e.ID == pid && e.Tid == tid && e.PosterID == posterid);
        }

        /// <summary>根据P编号查找</summary>
        /// <param name="pid">P编号</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static Post FindByID(Int32 pid)
        {
            //if (Meta.Count >= 1000)
            //    return Find(__.ID, pid);
            //else // 实体缓存
            //    return Meta.Cache.Entities.Find(__.ID, pid);
            // 单对象缓存
            return Meta.SingleCache[pid];
        }

        public static EntityList<Post> FindAllByIDs(String ids)
        {
            return FindAll(_.ID.In(ids.SplitAsInt(",")), null, null, 0, 0);
        }

        /// <summary>根据标题Tid查找层次为0的帖子，也就是主贴</summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        public static Post FindByTid(Int32 tid)
        {
            //if (Meta.Count >= 1000)
            //    return Find(__.Tid, tid);
            //else // 实体缓存
            //    return Meta.Cache.Entities.Find(__.Tid, tid);
            // 单对象缓存
            //return Meta.SingleCache[tid];
            return _cache[tid];
        }

        public static EntityList<Post> FindAllByTid(params Int32[] tids)
        {
            return FindAll(_.Tid.In(tids), null, null, 0, 0);
        }

        public static Post FindLastByTid(Int32 tid)
        {
            var list = FindAll(_.Tid == tid, _.ID.Desc(), null, 0, 1);
            return list.Count > 0 ? list[0] : null;
        }

        public static Post FindLastByFids(params Int32[] fids)
        {
            var list = FindAll(_.Fid.In(fids), _.ID.Desc(), null, 0, 1);
            return list.Count > 0 ? list[0] : null;
        }

        public static EntityList<Post> FindAllByPosterID(Int32 uid)
        {
            if (Meta.Count >= 1000)
                return FindAll(__.PosterID, uid);
            else // 实体缓存
                return Meta.Cache.Entities.FindAll(__.PosterID, uid);
            // 单对象缓存
            //return Meta.SingleCache[pid];
        }

        /// <summary>根据P编号、T编号、是否隐身查找</summary>
        /// <param name="pid">P编号</param>
        /// <param name="tid">T编号</param>
        /// <param name="invisible">是否隐身</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static Post FindByPidAndTidAndInvisible(Int32 pid, Int32 tid, Int32 invisible)
        {
            if (Meta.Count >= 1000)
                return Find(new String[] { _.ID, _.Tid, _.Invisible }, new Object[] { pid, tid, invisible });
            else // 实体缓存
                return Meta.Cache.Entities.Find(e => e.ID == pid && e.Tid == tid && e.Invisible == invisible);
        }
        #endregion

        #region 高级查询
        /// <summary>昨天发帖总数</summary>
        /// <param name="postTableId">帖子表编号</param>
        /// <returns></returns>
        public static Int32 FindCountForYesterday()
        {
            //return (Int32)ProcessWithTable(postTableId, () =>
            //{
            var yes = DateTime.Now.Date.AddDays(-1);
            return FindCount(_.PostDateTime.Between(yes, yes), null, null, 0, 0);
            //});
        }

        public static int GetTodayPostCount(Int32 postTableId)
        {
            //SELECT COUNT(1) FROM [{0}posts{1}] WHERE [postdatetime]>='{2}' AND [invisible]=0
            //if (postTableId <= 0) postTableId = TableList.Current.ID;
            return FindCount(_.PostDateTime >= DateTime.Now & _.Invisible == 0, null, null, 0, 0);
        }

        public static int GetPostCount(Int32 fid, Int32 postTableID = 0)
        {
            //return DatabaseProvider.GetInstance().GetPostCount(fid, postTableName);
            return FindCount(_.Fid == fid, null, null, 0, 0);
        }

        public static int GetTodayPostCount(Int32 fid, Int32 postTableID = 0)
        {
            //return DatabaseProvider.GetInstance().GetTodayPostCount(fid, postTableName);
            return FindCount(_.Fid == fid & _.PostDateTime >= DateTime.Now.Date, null, null, 0, 0);
        }

        public static int GetPostCountByTid(Int32 tid, Int32 postTableID = 0)
        {
            //string commandText = string.Format("SELECT COUNT([pid]) AS [postcount] FROM [{0}] WHERE [tid] = @tid AND [layer] <> 0", postTableName);
            //return DbHelper.ExecuteScalar<Int32>(commandText, DbHelper.MakeInParam("@tid", DbType.Double, 4, tid));

            //return (Int32)ProcessWithTable(postTableID, () => FindCount(_.Tid == tid & _.Layer != 0, null, null, 0, 0));
            return FindCount(__.Tid, tid);
        }

        public static int GetPostCountByUid(Int32 uid, Int32 postTableID = 0)
        {
            //return DatabaseProvider.GetInstance().GetPostCountByUid(uid, postTableName);
            return FindCount(_.PosterID == uid, null, null, 0, 0);
        }

        public static int GetTodayPostCountByUid(Int32 uid, Int32 postTableID = 0)
        {
            //return DatabaseProvider.GetInstance().GetTodayPostCountByUid(uid, postTableName);
            return FindCount(_.PosterID == uid & _.PostDateTime > DateTime.Now.Date, null, null, 0, 0);
        }

        public static int GetPostCountByPosterId(string onlyauthor, int topicId, int posterId, int replies)
        {
            if (String.IsNullOrEmpty(onlyauthor) || onlyauthor == "0") return replies + 1;

            //return BBX.Data.Posts.GetPostCountByPosterId(TableList.GetPostTableId(topicId), topicId, posterId);
            //SELECT COUNT(pid) FROM [dnt_posts1] WHERE [tid] = @tid AND [posterid] = @posterid  AND [layer]>=0
            return FindCount(_.Tid == topicId & _.PosterID == posterId & _.Layer >= 0, null, null, 0, 0);
        }

        public static Int32[] GetMaxAndMinTidByUid(int uid)
        {
            //return DatabaseProvider.GetInstance().GetMaxAndMinTid(uid);

            var arr = new Int32[2];
            var list = FindAll(_.PosterID == uid, null, _.ID.Max() & _.ID.Min(__.PosterID), 0, 0);
            if (list.Count > 0)
            {
                arr[0] = list[0].ID;
                arr[1] = list[0].PosterID;
            }
            return arr;
        }

        public static Int32[] GetMaxAndMinTidByFid(int fid)
        {
            var arr = new Int32[2];
            var list = FindAll(_.Fid.In(XForum.FindByID(fid).AllChildKeys), null, _.ID.Max() & _.ID.Min(__.PosterID), 0, 0);
            if (list.Count > 0)
            {
                arr[0] = list[0].ID;
                arr[1] = list[0].PosterID;
            }
            return arr;
        }

        /// <summary>获取主题的帖子列表</summary>
        /// <param name="tid"></param>
        /// <param name="posterid"></param>
        /// <param name="invisible"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public static EntityList<Post> GetPostList(Int32 tid, Int32 posterid, Int32 invisible, Int32 pageindex, Int32 pagesize)
        {
            var exp = _.Tid == tid;
            if (posterid > 0) exp &= _.PosterID == posterid;
            if (invisible > 0) exp &= _.Invisible > invisible;

            return FindAll(exp, _.ID.Asc(), null, (pageindex - 1) * pagesize, pagesize);
        }

        public static EntityList<Post> Search(Int32 days)
        {
            return FindAll(_.PostDateTime >= DateTime.Now.AddDays(-days), null, null, 0, 0);
        }

        public static SelectBuilder FindSQLByFid(Int32 fid)
        {
            return FindSQLWithKey(_.Fid == fid);
        }

        public static SelectBuilder FindSQLByPoster(String poster)
        {
            return FindSQLWithKey(_.Poster == poster);
        }

        public static EntityList<Post> SearchDebate(Int32 tid, Int32 opinion, Int32 start, Int32 max)
        {
            var exp = _.Invisible == 0;
            if (tid > 0) exp &= _.Tid == tid & _.Layer == 0;
            if (opinion > 0) exp |= _.ID.In(PostDebateField.FindSQLByTidAndOpinion(tid, opinion));

            return FindAll(exp, null, null, start, max);
        }

        public static Int32 SearchDebateCount(Int32 tid, Int32 opinion)
        {
            var exp = _.Invisible == 0;
            if (tid > 0) exp &= _.Tid == tid & _.Layer == 0;
            if (opinion > 0) exp |= _.ID.In(PostDebateField.FindSQLByTidAndOpinion(tid, opinion));

            return FindCount(exp);
        }

        public static Int32 GetPostCountBeforePid(int pid, int tid)
        {
            var exp = _.Tid == tid & _.ID < pid;
            exp &= (_.Invisible == 0 | _.Invisible == -2);

            return FindCount(exp);
        }

        public static List<Post> GetUnauditPost(string fidList, int filter, int pageIndex, int pagesize)
        {
            //return Posts.LoadPostInfoList(DatabaseProvider.GetInstance().GetUnauditNewPost(fidList, ppp, pageIndex, postTableId, filter));

            var exp = _.Layer > 0;
            if (!fidList.IsNullOrWhiteSpace()) exp &= _.Fid.In(fidList.SplitAsInt());
            exp &= _.Invisible == filter;

            return FindAll(exp, null, null, (pageIndex - 1) * pagesize, pagesize);
        }

        public static int GetUnauditNewPostCount(string fidList, int filter)
        {
            //return DatabaseProvider.GetInstance().GetUnauditNewPostCount(fidList, postTableId, filter);

            var exp = _.Invisible == filter & _.Layer > 0;
            if (!fidList.IsNullOrWhiteSpace()) exp &= _.Fid.In(fidList.SplitAsInt());

            return FindCount(exp);
        }

        public static EntityList<Post> GetPostListByCondition(String condition)
        {
            return FindAll(condition, null, null, 0, 0);
        }

        public static EntityList<Post> GetPagedLastPost(PostpramsInfo ppi)
        {
            //return DatabaseProvider.GetInstance().GetPagedLastPostList(postpramsInfo, TableList.GetPostTableName(postpramsInfo.Tid));
            //if (postParmsInfo.Pageindex > 1)
            //{
            //    text = string.Format(" AND p.[pid] < (SELECT MIN([pid]) FROM (SELECT TOP {0} [{1}].[pid] FROM [{1}] WHERE [{1}].[tid]=@tid AND [{1}].[invisible]<=0 AND [{1}].layer<>0 ORDER BY [{1}].[pid] DESC) AS tblTmp)", (postParmsInfo.Pageindex - 1) * postParmsInfo.Pagesize, postTableName, TablePrefix);
            //}
            //string commandText = string.Format("SELECT TOP {0} p.[pid], p.[fid], p.[layer], p.[posterid], p.[title], p.[message], p.[postdatetime], p.[attachment], p.[poster], p.[posterid], p.[invisible], p.[usesig], p.[htmlon], p.[smileyoff], p.[parseurloff], p.[bbcodeoff], p.[rate], p.[ratetimes], u.[username], u.[email], u.[showemail], uf.[avatar], uf.[avatarwidth], uf.[avatarheight], uf.[sightml] AS [signature], uf.[location], uf.[customstatus] FROM [{1}] p LEFT JOIN [{2}users] u ON u.[uid]=p.[posterid] LEFT JOIN [{2}userfields] uf ON uf.[uid]=u.[uid] WHERE p.[tid]=@tid AND p.[invisible]=0 AND p.layer<>0 {3} ORDER BY p.[pid] DESC", new object[]
            //{
            //    postParmsInfo.Pagesize,
            //    postTableName,
            //    TablePrefix,
            //    text
            //});

            var exp = _.Tid == ppi.Tid & _.Invisible == 0 & _.Layer != 0;

            return FindAll(exp, null, null, (ppi.Pageindex - 1) * ppi.Pagesize, ppi.Pagesize);
        }

        public static EntityList<Post> GetPostTree(Int32 tid)
        {
            return FindAll(_.Tid == tid & _.Invisible == 0, _.ParentID.Asc(), null, 0, 0);
        }

        public static Int32 GetModPostCountByPidList(string fidList, string pidList)
        {
            return FindCount(_.Fid.In(fidList.SplitAsInt()) & _.ID.In(pidList.SplitAsInt()));
        }

        public static String SearchSQL(int forumId, DateTime start, DateTime end, string poster, bool lowerUpper, string ip, string message)
        {
            var exp = new WhereExpression();
            if (forumId > 0) exp &= _.Fid == forumId;
            exp &= _.PostDateTime.Between(start, end);
            if (!poster.IsNullOrWhiteSpace())
            {
                if (lowerUpper)
                    exp &= _.Poster == poster;
                else
                    exp &= _.Poster.Contains(poster);
            }
            if (!ip.IsNullOrWhiteSpace()) exp &= _.Poster.Contains(ip);
            if (!message.IsNullOrWhiteSpace()) exp &= _.Poster.Contains(message);

            return exp;
        }

        public static Int32 GetDebatePostCount(string onlyauthor, int tid, int posterId, int stand)
        {
            var exp = _.Tid == tid & _.Layer > 0;
            exp &= _.ID.In(PostDebateField.FindSQLWithPidByTidAndOpinion(tid, stand));
            if (!onlyauthor.IsNullOrWhiteSpace() && onlyauthor != "0") exp &= _.PosterID == posterId;

            return FindCount(exp);
        }

        public static EntityList<Post> GetLastPostList(Int32 fid, Int32 count)
        {

            var exp = _.Layer > 0 & _.Invisible != 1;
            exp &= _.Tid.In(Topic.FindSQLWithKeyByFid(fid));

            return FindAll(exp, null, null, 0, count);
        }
        #endregion

        #region 扩展操作
        /// <summary>获取回帖数</summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        public static Int32 GetReplies(Int32 tid)
        {
            var exp = _.Tid == tid & _.Layer > 0;
            exp &= (_.Invisible == 0 | _.Invisible == -2);
            return FindCount(exp);
        }

        /// <summary>审核通过</summary>
        /// <param name="ids"></param>
        public static void Pass(String ids)
        {
            var list = FindAllByIDs(ids);
            list.ForEach(e => e.Invisible = 0);
            list.Update();

            var last = list[list.Count - 1];
            last.UpdateStatistic(list.Count);
        }
        #endregion

        #region 统计
        //public static EntityList<Post> GroupByTime(Int32 postTableId, DateTime time, Int32 count)
        //{
        //    //commandText = string.Format("SELECT DISTINCT TOP {0} [poster] AS [username], [posterid] AS [uid], COUNT(pid) AS [posts] FROM [{1}posts{2}] WHERE [postdatetime]>='{3}' AND [invisible]=0 AND [posterid]>0 GROUP BY [poster], [posterid] ORDER BY [posts] DESC", new object[]
        //    //            {
        //    //                count,
        //    //                TablePrefix,
        //    //                postTableId,
        //    //                DateTime.Now.AddDays(-30.0).ToString("yyyy-MM-dd")
        //    //            });

        //    var exp = _.PostDateTime > time;
        //    exp &= _.Invisible == 0;
        //    exp &= _.PosterID > 0;

        //    //var where = String.Format("", _.PosterID);
        //    //var selects = String.Format("{0},Max({1}) as {1}, COUNT(*) AS {2}", _.PosterID, _.Poster, _.ID);

        //    return FindAll(exp + _.PosterID.GroupBy(), _.ID.Desc(), _.Poster.Max() & _.ID.Count() & _.PosterID, 0, count);
        //}

        public static Dictionary<Int32, Int32> GetPostCountByForums(DateTime start, Int32 postTableId)
        {
            var exp = _.Invisible == 0 & _.PosterID > 0;
            if (start > DateTime.MinValue) exp &= _.PostDateTime >= start;
            var list = FindAll(exp & _.Fid.GroupBy(), null, _.ID.Count() & _.Fid, 0, 0);

            var dic = new Dictionary<Int32, Int32>();
            foreach (var item in list)
            {
                dic.Add(item.Fid, item.ID);
            }

            return dic;
        }

        public static void GetBestMember(out string bestmem, out int bestmemposts)
        {
            // SELECT TOP 1 [poster], COUNT(1) AS [posts] FROM [{0}posts{1}] WHERE [postdatetime]>='{2}' AND [invisible]=0 AND [posterid]>0 GROUP BY [poster] ORDER BY [posts] DESC
            bestmem = "";
            bestmemposts = 0;

            var exp = _.PostDateTime > DateTime.Now.Date & _.Invisible == 0 & _.PosterID > 0;
            var list = FindAll(exp & _.Poster.GroupBy(), _.ID.Desc(), _.ID.Count() & _.Poster, 0, 1);

            if (list.Count > 0)
            {
                bestmem = list[0].Poster;
                bestmemposts = list[0].ID;
            }
        }

        /// <summary>找到发帖最多的用户</summary>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static EntityList<Post> GetPostStatByUser(DateTime start, Int32 count)
        {
            var exp = _.Invisible == 0 & _.PosterID > 0;
            if (start > DateTime.MinValue) exp &= _.PostDateTime >= start;

            return FindAll(exp & _.PosterID.GroupBy(), _.ID.Desc(), _.ID.Count() & _.PosterID, 0, count);
        }

        public static Dictionary<Int32, Int32> GetPosterIDStat(DateTime start, Int32 count)
        {
            var list = GetPostStatByUser(start, count);
            if (list.Count < 1) return new Dictionary<int, int>();

            return list.ToList().ToDictionary(e => e.PosterID, e => e.ID);
        }

        public static EntityList<Post> GetUserPostCountList(int topNumber, DateType dateType, int dateNum)
        {
            var exp = _.PosterID > 0 & _.Invisible <= 0;
            var dt = DateTime.Now;
            switch (dateType)
            {
                case DateType.Minute:
                    dt = dt.AddMinutes(dateNum);
                    break;
                case DateType.Hour:
                    dt = dt.AddHours(dateNum);
                    break;
                case DateType.Day:
                    dt = dt.AddDays(dateNum);
                    break;
                case DateType.Week:
                    dt = dt.AddDays(7 * dateNum);
                    break;
                case DateType.Month:
                    dt = dt.AddMonths(dateNum);
                    break;
                case DateType.Year:
                    dt = dt.AddYears(dateNum);
                    break;
                default:
                    break;
            }
            exp &= _.PostDateTime >= dt;

            return FindAll(exp & _.PosterID.GroupBy() & _.Poster, _.ID.Desc(), _.ID.Count() & _.PosterID & _.Poster, 0, topNumber);
        }
        #endregion

        #region 业务
        //public static TResult ProcessWithTable<TResult>(Func<Object> func)
        //{
        //    var tableName = TableList.Current.TableName;
        //    return (TResult)Meta.ProcessWithSplit(Meta.ConnName, tableName, func);
        //}

        //public static Object ProcessWithTable(Int32 postTableId, Func<Object> func)
        //{
        //    return ProcessWithTable(TableList.FormatPostTableName(postTableId), func);
        //}

        //public static Object ProcessWithTable(String tableName, Func<Object> func)
        //{
        //    return Meta.ProcessWithSplit(Meta.ConnName, tableName, func);
        //}

        //public static int GetMaxPostTableTid(string postTableName)
        //{
        //    //string commandText = string.Format("SELECT ISNULL(MAX([tid]), 0) FROM [{0}]", postTableName);
        //    //return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.Text, commandText)) + 1;

        //    return (Int32)ProcessWithTable(postTableName, () =>
        //    {
        //        var list = FindAll(null, _.ID.Desc(), null, 0, 1);
        //        return list.Count > 0 ? list[0].ID : 0;
        //    });
        //}

        //public static int GetMinPostTableTid(string postTableName)
        //{
        //    //string commandText = string.Format("SELECT ISNULL(MIN([tid]), 0) FROM [{0}]", postTableName);
        //    //return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.Text, commandText)) + 1;

        //    return (Int32)ProcessWithTable(postTableName, () =>
        //    {
        //        var list = FindAll(null, _.ID.Asc(), null, 0, 1);
        //        return list.Count > 0 ? list[0].ID : 0;
        //    });
        //}

        public static bool IsReplier(int tid, int uid)
        {
            //return tid > 0 && uid > 0 && BBX.Data.Posts.IsReplier(tid, uid);
            if (tid <= 0 || uid <= 0) return false;

            return FindCount(_.Tid == tid & _.PosterID == uid) > 0;
        }

        public static Dictionary<String, Int32> GetDayPostsStats()
        {
            // 按照年月日 yyyyMMdd 为键，统计每天的帖子数，更新到哈希表里面去
            // 打算增加PostDate字段，专门存储日期，然后按照它统计
            var list = FindAll(_.PostDateTime >= DateTime.Now.Date.AddDays(-30), null, null, 0, 0);
            var dic = new Dictionary<String, Int32>();
            foreach (var item in list)
            {
                var key = item.PostDateTime.ToString("yyyyMMdd");
                if (dic.ContainsKey(key))
                    dic[key] += 1;
                else
                    dic.Add(key, 1);
            }
            return dic;
        }

        //public static Dictionary<String, Int32> GetMonthPostsStats()
        //{
        //    // 按照年月日 yyyyMM 为键，统计每月的帖子数，更新到哈希表里面去
        //    // 打算增加PostDate字段，专门存储日期，然后按照它统计
        //    var list = FindAll(null, null, null, 0, 0);
        //    var dic = new Dictionary<String, Int32>();
        //    foreach (var item in list)
        //    {
        //        var key = item.PostDateTime.ToString("yyyyMM");
        //        if (dic.ContainsKey(key))
        //            dic[key] += 1;
        //        else
        //            dic.Add(key, 1);
        //    }
        //    return dic;
        //}
        #endregion

        #region 帖子逻辑
        public Int32 Create()
        {
            var tp = Topic.FindByID(Tid);
            if (tp == null) throw new ArgumentNullException("Tid", "找不到ID=" + Tid + "的主题");

            using (var trans = Meta.CreateTrans())
            {
                FillDefault();

                // 需要借助索引表来生成主键
                var pi = new PostId();
                pi.PostDateTime = PostDateTime;
                pi.Insert();

                ID = pi.ID;
                if (ParentID == 0) ParentID = ID;

                var rs = base.OnInsert();

                if (Invisible == 0) UpdateStatistic(1);

                bool flag = Regex.IsMatch(Message, "\\s*(\\[hide\\][\\n\\r]*([\\s\\S]+?)[\\n\\r]*\\[\\/hide\\]|\\[hide=(\\d+?)\\][\\n\\r]*([\\s\\S]+?)[\\n\\r]*\\[\\/hide\\])\\s*", RegexOptions.IgnoreCase);
                if (tp != null)
                {
                    // 如果这个帖子是主贴，有隐藏内容，则隐藏主题
                    if (flag && Layer <= 0) tp.Hide = 1;

                    tp.Save();
                }

                // 插入我的帖子
                if (PosterID != -1)
                {
                    var my = new MyPost();
                    my.Uid = PosterID;
                    my.Tid = Tid;
                    my.Pid = ID;
                    my.Dateline = PostDateTime;
                    my.Insert();
                }

                trans.Commit();

                return rs;
            }
        }

        void UpdateStatistic(Int32 count)
        {
            // 更新各地统计信息
            var st = Statistic.Current;
            st.TotalPost += count;
            st.Save();

            // 更新论坛统计
            var fi = XForum.FindByID(Fid);
            if (fi == null) throw new ArgumentNullException("Fid", "找不到ID=" + Fid + "的论坛");

            // 所有上级论坛帖子数增加
            while (fi != null && fi.ID != 0)
            {
                fi.Posts += count;
                if (fi.LastPost.Date == DateTime.Now.Date)
                    fi.TodayPosts += count;
                else
                    fi.TodayPosts = 1;

                fi.LastTID = Tid;
                fi.LastTitle = TopicTitle;
                fi.LastPost = PostDateTime;
                fi.LastPoster = Poster;
                fi.LastPosterID = PosterID;

                fi.Save();

                fi = fi.Parent;
            }

            // 更新用户统计
            var user = User.FindByID(PosterID);
            if (user == null) throw new ArgumentNullException("PosterID", "找不到ID=" + PosterID + "的发帖者");
            {
                user.LastPost = PostDateTime;
                user.LastPostID = ID;
                user.LastPostTitle = TopicTitle;
                user.Posts += count;
                user.LastActivity = DateTime.Now;

                user.Save();
            }

            var tp = Topic.FindByID(Tid);
            if (Invisible == 0 && Layer > 0)
                tp.Replies += count;
            else if (Layer <= 0)
                tp.Replies = 0;

            tp.LastPost = PostDateTime;
            tp.LastPoster = Poster;
            tp.LastPosterID = PosterID;
            tp.LastPostID = ID;
            tp.Save();
        }

        /// <summary>填充默认值</summary>
        void FillDefault()
        {
            if (!Dirtys[__.PostDateTime]) PostDateTime = DateTime.Now;
            if (!Dirtys[_.IP]) IP = WebHelper.UserHost;

            var user = ManageProvider.Provider.Current;
            if (user != null)
            {
                if (!Dirtys[__.PosterID]) PosterID = user.ID;
                if (!Dirtys[__.Poster]) Poster = user.Name;
            }
        }

        public static Int32 ChangeFid(Int32 srcFid, Int32 targetFid)
        {
            return Update(_.Fid == targetFid, _.Fid == srcFid);
        }

        public override void CastTo(object entity)
        {
            if (PostUser != null)
            {
                var user = PostUser as User;
                user.CastTo(entity);
                if (user.Field != null) user.Field.CastTo(entity);
            }
            // 最后拷贝自己，避免ID被覆盖
            base.CastTo(entity);
        }
        #endregion

        #region 搜索帖子
        public static String SearchWhere(int posterId, string fids, int searchTime, int searchTimeType, string keyWord)
        {
            keyWord = Regex.Replace(keyWord, "--|;|'|\"", "", RegexOptions.Multiline | RegexOptions.Compiled);
            var sb = new StringBuilder(keyWord);
            sb.Replace("'", "''");
            sb.Replace("%", "[%]");
            sb.Replace("_", "[_]");
            sb.Replace("[", "[[]");
            keyWord = sb.ToString();

            var exp = new WhereExpression();
            if (posterId > 0) exp &= _.PosterID == posterId;
            if (searchTime != 0)
            {
                if (searchTimeType == 1)
                    exp &= _.PostDateTime < DateTime.Now.Date.AddDays(searchTime);
                else
                    exp &= _.PostDateTime > DateTime.Now.Date.AddDays(searchTime);
            }
            if (!fids.IsNullOrWhiteSpace()) exp &= _.Fid.In(fids.SplitAsInt());
            if (!keyWord.IsNullOrWhiteSpace())
            {
                // 暂时不处理多关键字
                exp &= _.Title.Contains(keyWord);
            }

            return exp;
        }
        #endregion
    }
}