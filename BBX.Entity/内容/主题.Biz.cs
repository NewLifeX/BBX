﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using NewLife.Log;
using XCode;
using XCode.Cache;
using XCode.Configuration;
using XCode.DataAccessLayer;
using XCode.Membership;

namespace BBX.Entity
{
    /// <summary>主题</summary>
    public partial class Topic : EntityBase<Topic>
    {
        #region 对象操作﻿
        static Topic()
        {
            var fs = Meta.Factory.AdditionalFields;
            fs.Add(__.Views);
            fs.Add(__.Replies);

            // 主题仅缓存短时间，否则容易丢失阅读数
            Meta.SingleCache.Expire = 10;
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

            if (!HasDirty) return;

            var user = ManageProvider.Provider.Current;
            if (isNew)
            {
                if (!Dirtys[__.PostDateTime]) PostDateTime = DateTime.Now;

                if (user != null)
                {
                    if (!Dirtys[__.PosterID]) PosterID = user.ID;
                    if (!Dirtys[__.Poster]) Poster = user.Name;
                }
            }
            //if (!Dirtys[__.LastPost]) LastPost = DateTime.Now;

            //if (user != null)
            //{
            //	if (!Dirtys[__.LastPosterID]) LastPosterID = user.ID;
            //	if (!Dirtys[__.LastPoster]) LastPoster = user.Account;
            //}
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            //if (!String.IsNullOrEmpty(Title)) Title = Title.Trim();

            this.TrimField();
        }

        protected override void InitData()
        {
            if (Meta.Count <= 0) return;

            // 首先删除最近100个非法空贴
            var list = FindAll(_.LastPostID, 0);
            if (list.Count > 0)
            {
                XTrace.WriteLine("删除{0}个LastPostID为0的非法帖子", list.Count);
                list.Delete();
            }
        }

        /// <summary>已重载。在事务保护范围内处理业务，位于Valid之后</summary>
        /// <returns></returns>
        protected override Int32 OnInsert()
        {
            switch (Special)
            {
                case 0:
                    TrendStat.Today.Topic++;
                    break;

                case 1:
                    TrendStat.Today.Poll++;
                    break;

                case 2:
                    TrendStat.Today.Bonus++;
                    break;

                case 4:
                    TrendStat.Today.Debate++;
                    break;
            }

            return base.OnInsert();
        }

        public override int Update()
        {
            if (Dirtys[__.DisplayOrder]) _tops.Clear("更新置顶");

            return base.Update();
        }

        protected override Int32 OnDelete()
        {
            _tops.Clear("删除");

            // 删除关联帖子
            var ps = Post.FindAllByTid(ID);

            return ps.Delete() + base.OnDelete();
        }
        #endregion

        #region 扩展属性﻿
        private List<String> hasLoad = new List<String>();
        private XForum _Forum;
        /// <summary>论坛</summary>
        public XForum Forum
        {
            get
            {
                if (_Forum == null && Fid > 0 && !hasLoad.Contains("Forum"))
                {
                    _Forum = XForum.FindByID(this.Fid);
                    hasLoad.Add("Forum");
                }
                return _Forum;
            }
            set { _Forum = value; }
        }

        /// <summary>论坛名称</summary>
        public String ForumName { get { return Forum == null ? null : Forum.Name; } }

        /// <summary>论坛重写名</summary>
        public String RewriteName { get { return Forum == null ? null : (Forum as IXForum).RewriteName; } }

        private Post _Post;
        /// <summary>帖子</summary>
        public Post Post
        {
            get
            {
                if (_Post == null && !hasLoad.Contains("Post"))
                {
                    _Post = Post.FindByTid(ID);
                    hasLoad.Add("Post");
                }
                return _Post;
            }
            set { _Post = value; }
        }

        private User _PostUser;
        /// <summary>发帖人</summary>
        public User PostUser
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
            set { _PostUser = value; }
        }

        /// <summary>帖子内容</summary>
        public String Message { get { return Post == null ? null : Post.Message; } }

        private String _Folder;
        /// <summary>文件夹</summary>
        public String Folder { get { return _Folder; } set { _Folder = value; } }

        private TopicType _TopicType;
        /// <summary>主题类型</summary>
        public TopicType TopicType
        {
            get
            {
                if (_TopicType == null && TypeID > 0)
                {
                    _TopicType = TopicType.FindByID(TypeID);
                }
                return _TopicType;
            }
            set { _TopicType = value; }
        }

        /// <summary>主题类型</summary>
        public String TypeName { get { return TopicType == null ? null : TopicType.Name; } }

        /// <summary>高亮标题</summary>
        public String TitleHighlight
        {
            get
            {
                if (Highlight.IsNullOrWhiteSpace()) return Title;

                return "<span style=\"" + Highlight + "\">" + Title + "</span>";
            }
        }

        /// <summary>是否已被删除</summary>
        public Boolean Deleted { get { return DisplayOrder == -1; } set { DisplayOrder = value ? -1 : 0; } }

        /// <summary>是否已被审核</summary>
        public Boolean NotAudited { get { return DisplayOrder == -2; } set { DisplayOrder = value ? -2 : 0; } }
        #endregion

        #region 扩展查询﻿
        public static EntityList<Topic> FindAllByPosterID(Int32 uid)
        {
            if (Meta.Count >= 1000)
                return FindAll(__.PosterID, uid);
            else // 实体缓存
                return Meta.Cache.Entities.FindAll(__.PosterID, uid);
        }

        public static EntityList<Topic> FindAllByPosterID(Int32 userId, Int32 pageIndex, Int32 pageSize)
        {
            return FindAll(_.PosterID == userId, null, null, (pageIndex - 1) * pageSize, pageSize);
        }

        public static EntityList<Topic> FindAllByLastPostID(Int32 uid)
        {
            // 这里是不是有可能是LastPosterID
            if (Meta.Count >= 1000)
                return FindAll(__.LastPostID, uid);
            else // 实体缓存
                return Meta.Cache.Entities.FindAll(__.LastPostID, uid);
        }

        /// <summary>根据F编号、显示顺序查找</summary>
        /// <param name="fid">F编号</param>
        /// <param name="displayorder">显示顺序</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static EntityList<Topic> FindAllByFidAndDisplayOrder(Int32 fid, Int32 displayorder)
        {
            if (Meta.Count >= 1000)
                return FindAll(new String[] { _.Fid, _.DisplayOrder }, new Object[] { fid, displayorder });
            else // 实体缓存
                return Meta.Cache.Entities.FindAll(e => e.Fid == fid && e.DisplayOrder == displayorder);
        }

        /// <summary>根据F编号、LastPostID、显示顺序查找</summary>
        /// <param name="fid">F编号</param>
        /// <param name="lastpostid"></param>
        /// <param name="displayorder">显示顺序</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static Topic FindByFidAndLastPostIDAndDisplayOrder(Int32 fid, Int32 lastpostid, Int32 displayorder)
        {
            if (Meta.Count >= 1000)
                return Find(new String[] { _.Fid, _.LastPostID, _.DisplayOrder }, new Object[] { fid, lastpostid, displayorder });
            else // 实体缓存
                return Meta.Cache.Entities.Find(e => e.Fid == fid && e.LastPostID == lastpostid && e.DisplayOrder == displayorder);
        }

        ///// <summary>根据F编号、发送时间、LastPostID、显示顺序查找</summary>
        ///// <param name="fid">F编号</param>
        ///// <param name="postdatetime">发送时间</param>
        ///// <param name="lastpostid"></param>
        ///// <param name="displayorder">显示顺序</param>
        ///// <returns></returns>
        //[DataObjectMethod(DataObjectMethodType.Select, false)]
        //public static EntityList<Topic> FindAllByFidAndPostDateTimeAndLastPostIDAndDisplayOrder(Int32 fid, String postdatetime, Int32 lastpostid, Int32 displayorder)
        //{
        //    if (Meta.Count >= 1000)
        //        return FindAll(new String[] { _.Fid, _.PostDateTime, _.LastPostID, _.DisplayOrder }, new Object[] { fid, postdatetime, lastpostid, displayorder });
        //    else // 实体缓存
        //        return Meta.Cache.Entities.FindAll(e => e.Fid == fid && e.PostDateTime == postdatetime && e.LastPostID == lastpostid && e.DisplayOrder == displayorder);
        //}

        ///// <summary>根据F编号、发送时间、答复、显示顺序查找</summary>
        ///// <param name="fid">F编号</param>
        ///// <param name="postdatetime">发送时间</param>
        ///// <param name="replies">答复</param>
        ///// <param name="displayorder">显示顺序</param>
        ///// <returns></returns>
        //[DataObjectMethod(DataObjectMethodType.Select, false)]
        //public static EntityList<Topic> FindAllByFidAndPostDateTimeAndRepliesAndDisplayOrder(Int32 fid, String postdatetime, Int32 replies, Int32 displayorder)
        //{
        //    if (Meta.Count >= 1000)
        //        return FindAll(new String[] { _.Fid, _.PostDateTime, _.Replies, _.DisplayOrder }, new Object[] { fid, postdatetime, replies, displayorder });
        //    else // 实体缓存
        //        return Meta.Cache.Entities.FindAll(e => e.Fid == fid && e.PostDateTime == postdatetime && e.Replies == replies && e.DisplayOrder == displayorder);
        //}

        /// <summary>根据编号、F编号、显示顺序查找</summary>
        /// <param name="id">编号</param>
        /// <param name="fid">F编号</param>
        /// <param name="displayorder">显示顺序</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static EntityList<Topic> FindAllByIDAndFidAndDisplayOrder(Int32 id, Int32 fid, Int32 displayorder)
        {
            if (Meta.Count >= 1000)
                return FindAll(new String[] { _.ID, _.Fid, _.DisplayOrder }, new Object[] { id, fid, displayorder });
            else // 实体缓存
                return Meta.Cache.Entities.FindAll(e => e.ID == id && e.Fid == fid && e.DisplayOrder == displayorder);
        }

        public static EntityList<Topic> FindAllByTidsAndDisplayOrder(String tids, Int32 displayorder)
        {
            return FindAll(_.ID.In(tids.SplitAsInt()) & _.DisplayOrder > displayorder, null, null, 0, 0);
        }

        ///// <summary>根据F编号、发送时间、浏览数、显示顺序查找</summary>
        ///// <param name="fid">F编号</param>
        ///// <param name="postdatetime">发送时间</param>
        ///// <param name="views">浏览数</param>
        ///// <param name="displayorder">显示顺序</param>
        ///// <returns></returns>
        //[DataObjectMethod(DataObjectMethodType.Select, false)]
        //public static EntityList<Topic> FindAllByFidAndPostDateTimeAndViewsAndDisplayOrder(Int32 fid, String postdatetime, Int32 views, Int32 displayorder)
        //{
        //    if (Meta.Count >= 1000)
        //        return FindAll(new String[] { _.Fid, _.PostDateTime, _.Views, _.DisplayOrder }, new Object[] { fid, postdatetime, views, displayorder });
        //    else // 实体缓存
        //        return Meta.Cache.Entities.FindAll(e => e.Fid == fid && e.PostDateTime == postdatetime && e.Views == views && e.DisplayOrder == displayorder);
        //}

        /// <summary>根据编号查找</summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static Topic FindByID(Int32 id)
        {
            //if (Meta.Count >= 1000)
            //    return Find(_.ID, id);
            //else // 实体缓存
            //    return Meta.Cache.Entities.Find(_.ID, id);
            // 单对象缓存
            return Meta.SingleCache[id];
        }

        public static EntityList<Topic> FindAllByIDs(String ids)
        {
            return FindAll(_.ID.In(ids.SplitAsInt()), null, null, 0, 0);
        }

        public static Int32 FindCountByForumID(Int32 fid)
        {
            return FindCount(_.Fid == fid & _.DisplayOrder > 0, null, null, 0, 0);
        }
        #endregion

        #region 高级查询
        public static SelectBuilder FindIDSQLByFIDList(String fidlist)
        {
            return FindSQL(FidIn(fidlist) & _.DisplayOrder >= 0, null, _.ID.ColumnName);
        }

        static Dictionary<Int32, Int32> _News;
        static DateTime _NextGetNew;

        /// <summary>检测论坛板块的新帖子数</summary>
        /// <param name="fids"></param>
        /// <returns></returns>
        public static Dictionary<Int32, Int32> GetHasNewByFids(Int32[] fids)
        {
            if (_NextGetNew > DateTime.Now) return _News;

            // 每个论坛60分钟或120分钟内的最新帖子
            var now = DateTime.Now;
            // 去掉分秒，减少不同的查询SQL
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0);

            var exp = FidIn(fids);
            exp &= _.LastPost > now.AddMinutes(-600);
            var list = FindAll(exp + " Group By " + __.Fid, null, String.Format("{0}, max({1}) as {1}", __.Fid, _.ID.ColumnName), 0, 0);
            var dic = new Dictionary<Int32, Int32>();
            foreach (var item in list)
            {
                dic.Add(item.Fid, item.ID);
            }

            // 缓存一分钟
            _NextGetNew = DateTime.Now.AddMinutes(10);

            return _News = dic;
        }

        //static Int32 _Count;
        //static DateTime _NextGetCount;

        public static Int32 GetTopicCount(Int32 fid, bool includeClosedTopic, String condition)
        {
            //if (_NextGetCount > DateTime.Now) return _Count;

            //var exp = _.Fid == fid & _.DisplayOrder > -1 & _.Closed <= 1;
            var exp = _.DisplayOrder >= 0;
            if (fid > 0) exp &= _.Fid == fid;
            if (!String.IsNullOrEmpty(condition))
            {
                condition = condition.Trim();
                if (condition.StartsWithIgnoreCase("and")) condition = condition.Substring(3).Trim();
                if (condition.StartsWithIgnoreCase("or")) condition = condition.Substring(2).Trim();
                exp &= condition;
            }

            if (!includeClosedTopic)
                exp &= _.Closed == 0;
            else
                exp &= _.Closed <= 1;

            //// 缓存一分钟
            //_NextGetCount = DateTime.Now.AddMinutes(1);

            //return _Count = FindCount(exp, null, null, 0, 0);
            return FindCount(exp, null, null, 0, 0);
        }

        public static EntityList<Topic> GetHotTopicsList(Int32 pageSize, Int32 pageIndex, Int32 fid, String showType, Int32 timeBetween)
        {
            var exp = new WhereExpression();
            if (fid > 0) exp &= _.Fid == fid;
            if (timeBetween > 0) exp &= _.PostDateTime >= DateTime.Now.AddDays(-timeBetween);
            if (!String.IsNullOrEmpty(showType)) showType += " Desc";

            return FindAll(exp, showType, null, (pageIndex - 1) * pageSize, pageSize);
        }

        public static Int32 GetHotTopicsCount(Int32 fid, Int32 timeBetween)
        {
            var exp = new WhereExpression();
            if (fid > 0) exp &= _.Fid == fid;
            if (timeBetween > 0) exp &= _.PostDateTime >= DateTime.Now.AddDays(-timeBetween);
            return FindCount(exp, null, null, 0, 0);
        }

        public static EntityList<Topic> GetTopicList(Int32 forumId, String posterList, String keyList, DateTime startDate, DateTime endDate, Int32 pageSize, Int32 currentPage)
        {
            return FindAll(SearchWhere(forumId, posterList, keyList, startDate, endDate), _.ID.Desc(), null, (currentPage - 1) * pageSize, pageSize);
        }

        public static Int32 GetTopicListCount(Int32 forumId, String posterList, String keyList, DateTime startDate, DateTime endDate)
        {
            return FindCount(SearchWhere(forumId, posterList, keyList, startDate, endDate), null, null, 0, 0);
        }

        static String SearchWhere(Int32 forumId, String posterList, String keyList, DateTime startDate, DateTime endDate)
        {
            var forum = XForum.FindByID(forumId);
            var exp = new Expression();

            var fids = forum.GetVisibleFids();
            if (fids == null || fids.Length == 0) return "1<>0";

            // 如果要过滤的论坛数就是论坛总数，那么直接不需要过滤
            if (fids.Length != XForum.Meta.Count && fids.Length != XForum.GetForumCountNoTop()) exp &= _.Fid.In(fids);

            //exp &= _.ID >= Post.GetMinPostTableTid(postName);
            //exp &= _.ID <= Post.GetMaxPostTableTid(postName);
            exp &= _.Closed != 1;

            if (!String.IsNullOrEmpty(posterList))
            {
                exp &= _.Poster.In(posterList.Split(','));
            }

            if (!String.IsNullOrEmpty(keyList))
            {
                var exp2 = new WhereExpression();
                foreach (var item in keyList.Split(','))
                {
                    exp2 |= _.Title.Contains(item);
                }
                exp &= exp2;
            }
            if (startDate > DateTime.MinValue) exp &= _.PostDateTime >= startDate.Date;
            if (endDate > DateTime.MinValue) exp &= _.PostDateTime < endDate.Date.AddDays(1);

            return exp;
        }

        /// <summary>查询</summary>
        /// <param name="fids"></param>
        /// <param name="keyword"></param>
        /// <param name="start"></param>
        /// <param name="max"></param>
        /// <param name="count">传入负数表示仅查询记录数，正数表示仅查询对象列表，0表示两者都查</param>
        /// <returns></returns>
        public static EntityList<Topic> GetAttentionTopics(Int32[] fids, String keyword, Int32 start, Int32 max, ref Int32 count)
        {
            //return BBX.Data.Topics.GetAttentionTopics(fidList, tpp, pageIndex, keyword);
            var exp = _.Attention == 1 & _.DisplayOrder >= 0;
            if (fids != null && fids.Length > 0 && fids[0] > 0) exp &= FidIn(fids);
            if (!String.IsNullOrEmpty(keyword))
            {
                var exp2 = _.Title.Contains(keyword);
                exp2 |= _.Poster.Contains(keyword);
                exp &= exp2;
            }

            // 有时候可能只要数量
            if (count <= 0)
            {
                var onlyCount = count < 0;
                count = FindCount(exp, null, null, 0, 0);
                // 负数只查记录数
                if (onlyCount) return null;

                if (count <= 0) return new EntityList<Topic>();
            }

            return FindAll(exp, _.LastPost.Desc(), null, start, max);
        }

        public static EntityList<Topic> GetUnauditNewTopic(Int32[] fids, Int32 displayorder, Int32 pageindex, Int32 pagesize)
        {
            var exp = _.DisplayOrder == displayorder;
            if (fids != null && fids.Length > 0 && fids[0] > 0) exp &= FidIn(fids);

            return FindAll(exp, null, null, (pageindex - 1) * pagesize, pagesize);
        }

        public static Int32 GetUnauditNewTopicCount(Int32[] fids, Int32 displayorder)
        {
            var exp = _.DisplayOrder == displayorder;
            if (fids != null && fids.Length > 0 && fids[0] > 0) exp &= FidIn(fids);

            return FindCount(exp, null, null, 0, 0);
        }

        public static Int32 GetMyUnauditTopicCount(Int32 posterid, Int32 displayorder)
        {
            return FindCount(_.PosterID == posterid & _.DisplayOrder == displayorder);
        }

        public static EntityList<Topic> GetTopTopicList(Int32 fid, Int32 pageSize, Int32 pageIndex, String tids)
        {
            if (pageIndex < 1) pageIndex = 1;

            //var list = BBX.Data.Topics.GetTopTopicList(fid, pageSize, pageIndex, tids);
            //Topics.LoadTopTopicListExtraInfo(topicTypePrefix, list);

            var exp = _.DisplayOrder > 0;
            if (fid > 0) exp &= _.Fid == fid;
            if (!tids.IsNullOrWhiteSpace()) exp &= _.ID.In(tids.SplitAsInt());

            return FindAll(exp, _.DisplayOrder.Desc() & _.LastPost.Desc(), null, (pageIndex - 1) * pageSize, pageSize);
        }

        public static EntityList<Topic> GetTopicListByReplyUserId(Int32 userId, Int32 pageIndex, Int32 pageSize)
        {
            return FindAll(_.PosterID == userId, _.ID.Desc(), null, (pageIndex - 1) * pageSize, pageSize);
        }

        public static EntityList<Topic> GetTopicList(Int32 fid, Int32 pageSize, Int32 pageIndex, Int32 startNumber, String condition)
        {
            if (pageIndex < 1) pageIndex = 1;

            var exp = _.DisplayOrder >= 0;
            if (fid > 0) exp &= _.Fid == fid;
            if (!condition.IsNullOrWhiteSpace()) exp &= condition;

            return FindAll(exp, _.LastPostID.Desc(), null, (pageIndex - 1) * pageSize - startNumber, pageSize);
        }

        public static EntityList<Topic> Search(Int32 fid, Int32 typeid, Int32 days, String filter, Int32 order, Boolean isDesc, Int32 start, Int32 max)
        {
            // 只查询普通帖子
            var exp = _.DisplayOrder == 0;
            if (fid > 0) exp &= _.Fid == fid;
            if (typeid > 0) exp &= _.TypeID == typeid;
            if (days > 0) exp &= _.PostDateTime >= DateTime.Now.AddDays(-days);

            switch ((filter + "").ToLower().Trim())
            {
                case "poll":
                    exp &= _.Special == 1;
                    break;
                case "reward":
                    exp &= (_.Special == 2 | _.Special == 3);
                    break;
                case "rewarded":
                    exp &= _.Special == 3;
                    break;
                case "rewarding":
                    exp &= _.Special == 2;
                    break;
                case "debate":
                    exp &= _.Special == 4;
                    break;
                case "digest":
                    exp &= _.Digest > 0;
                    break;
            }
            var ody = _.LastPostID;
            switch (order)
            {
                case 2:
                    ody = _.ID;
                    break;
                case 3:
                    ody = _.Views;
                    break;
                case 4:
                    ody = _.Replies;
                    break;
            }

            return FindAll(exp, isDesc ? ody.Desc() : ody.Asc(), null, start, max);
        }

        public static Int32 SearchCount(Int32 fid, Int32 typeid, Int32 days, String filter)
        {
            var exp = _.DisplayOrder >= 0;
            if (fid > 0) exp &= _.Fid == fid;
            if (typeid > 0) exp &= _.TypeID == typeid;
            if (days > 0) exp &= _.PostDateTime >= DateTime.Now.AddDays(-days);

            switch ((filter + "").ToLower().Trim())
            {
                case "poll":
                    exp &= _.Special == 1;
                    break;
                case "reward":
                    exp &= (_.Special == 2 | _.Special == 3);
                    break;
                case "rewarded":
                    exp &= _.Special == 3;
                    break;
                case "rewarding":
                    exp &= _.Special == 2;
                    break;
                case "debate":
                    exp &= _.Special == 4;
                    break;
                case "digest":
                    exp &= _.Digest > 0;
                    break;
            }

            return FindCount(exp);
        }

        public static EntityList<Topic> GetTopicListByViewsOrReplies(Int32 fid, Int32 pageSize, Int32 pageIndex, Int32 startNumber, String condition, String orderFields, Int32 sortType)
        {
            if (pageIndex < 1) pageIndex = 1;

            var exp = _.DisplayOrder >= 0;
            if (fid > 0) exp &= _.Fid == fid;
            if (!condition.IsNullOrWhiteSpace()) exp &= condition;
            var order = _.LastPostID.Desc().ToString();
            if (!String.IsNullOrEmpty(orderFields)) order = orderFields + " " + (sortType == 0 ? "Asc" : "Desc");

            return FindAll(exp, order, null, (pageIndex - 1) * pageSize - startNumber, pageSize);
        }

        public static EntityList<Topic> GetTopicListByDate(Int32 fid, Int32 pageSize, Int32 pageIndex, Int32 startNumber, String condition, String orderFields, Int32 sortType)
        {
            if (pageIndex < 1) pageIndex = 1;

            var exp = _.DisplayOrder >= 0;
            if (fid > 0) exp &= _.Fid == fid;
            if (!condition.IsNullOrWhiteSpace()) exp &= condition;
            var order = _.LastPostID.Desc().ToString();
            if (!String.IsNullOrEmpty(orderFields)) order = orderFields + " " + (sortType == 0 ? "Asc" : "Desc");

            return FindAll(exp, order, null, (pageIndex - 1) * pageSize - startNumber, pageSize);
        }

        public static EntityList<Topic> GetTopicListByCondition(Int32 pageSize, Int32 pageIndex, Int32 startNumber, String condition, String orderFields, Int32 sortType)
        {
            if (pageIndex < 1) pageIndex = 1;

            var exp = _.DisplayOrder >= 0;
            if (!condition.IsNullOrWhiteSpace()) exp &= condition;
            var order = _.LastPostID.Desc().ToString();
            if (!String.IsNullOrEmpty(orderFields)) order = orderFields + " " + (sortType == 0 ? "Asc" : "Desc");

            return FindAll(exp, order, null, (pageIndex - 1) * pageSize - startNumber, pageSize);
        }

        public static EntityList<Topic> GetTopicListByTagId(Int32 tagId, Int32 pageIndex, Int32 pageSize)
        {
            var exp = _.DisplayOrder >= 0;
            exp &= _.ID.In(TopicTag.FindSQLWithKey(TopicTag._.TagID == tagId));
            return FindAll(exp, null, null, (pageIndex - 1) * pageSize, pageSize);
        }

        public static EntityList<Topic> GetMyUnauditTopic(Int32 posterId, Int32 displayorder, Int32 pageindex, Int32 pagesize)
        {
            var exp = _.PosterID == posterId & _.DisplayOrder == displayorder;
            return FindAll(exp, null, null, (pageindex - 1) * pagesize, pagesize);
        }

        public static Int32 GetDisplayorder(String topiclist)
        {
            //return TopicAdmins.GetTopicStatus(topiclist, "displayorder");
            var list = FindAll(_.ID.In(topiclist.SplitAsInt()), _.DisplayOrder.Desc(), _.DisplayOrder.Sum(), 0, 0);
            return list.Count > 0 ? list[0].DisplayOrder : 0;
        }

        public static Int32 GetDigest(String topiclist)
        {
            //return TopicAdmins.GetTopicStatus(topiclist, "digest");
            var list = FindAll(_.ID.In(topiclist.SplitAsInt()), _.Digest.Desc(), _.Digest.Sum(), 0, 0);
            return list.Count > 0 ? list[0].Digest : 0;
        }

        /// <summary>查询用户有多少个被评分主题</summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static Int32 GetDigestCount(Int32 uid)
        {
            return FindCount(_.PosterID == uid & _.Digest > 0);
        }

        public static String GetTopicCountCondition(String getType, int getNewTopic, Int32 fid, String fs)
        {
            var exp = new WhereExpression();
            if (getType == "digest")
                exp &= _.Digest > 0;
            else if (getType == "newtopic")
                exp &= _.LastPost > DateTime.Now.AddMinutes(-getNewTopic);

            if (fid > 0)
                exp &= _.Fid == fid;
            else if (!fs.IsNullOrWhiteSpace())
                exp &= FidIn(fs);

            return exp;
        }

        public static String SearchWhere(int fid, String keyWord, String displayOrder, String digest, String attachment, String poster, Int32 viewsMin, Int32 viewsMax, Int32 repliesMax, Int32 repliesMin, Int32 rate, Int32 lastPost, DateTime postDateTimeStart, DateTime postDateTimeEnd)
        {
            var exp = new WhereExpression();
            if (fid > 0) exp &= _.Fid == fid;
            // 偷懒，不做关键字拆分
            if (!keyWord.IsNullOrWhiteSpace()) exp &= _.Title.Contains(keyWord);

            if (displayOrder == "1")
                exp &= _.DisplayOrder > 0;
            else if (displayOrder == "2")
                exp &= _.DisplayOrder <= 0;

            if (digest == "1")
                exp &= _.Digest >= 1;
            else if (digest == "2")
                exp &= _.Digest < 1;

            if (attachment == "1")
                exp &= _.Attachment > 0;
            else if (attachment == "2")
                exp &= _.Attachment <= 0;

            // 偷懒，不做关键字拆分
            if (!poster.IsNullOrWhiteSpace()) exp &= _.Poster == poster;

            if (viewsMax > 0) exp &= _.Views > viewsMax;
            if (viewsMin > 0) exp &= _.Views < viewsMin;
            if (repliesMax > 0) exp &= _.Replies > repliesMax;
            if (repliesMin > 0) exp &= _.Replies < repliesMin;
            if (rate > 0) exp &= _.Rate > rate;
            if (lastPost > 0) exp &= _.LastPost > DateTime.Now.AddDays(-lastPost);

            exp &= _.PostDateTime.Between(postDateTimeStart, postDateTimeEnd);

            return exp;
        }

        public static EntityList<Topic> Search(Int32 recycleDay)
        {
            var exp = _.DisplayOrder == -1 & _.PostDateTime > DateTime.Now.AddDays(-recycleDay);
            return FindAll(exp, null, null, 0, 0);
        }

        public static String SearchTopicAudit(int fid, String poster, String title, String moderatorName, DateTime postDateTimeStart, DateTime postDateTimeEnd, DateTime delDateTimeStart, DateTime delDateTimeEnd)
        {
            var exp = new WhereExpression();
            if (fid > 0) exp &= _.Fid == fid;
            if (!poster.IsNullOrWhiteSpace()) exp &= _.Poster == poster;
            if (!title.IsNullOrWhiteSpace()) exp &= _.Title.Contains(title);
            if (!moderatorName.IsNullOrWhiteSpace()) exp &= _.ID.In(ModeratorManageLog.FindSQLWithTidByName(moderatorName));
            exp &= _.PostDateTime.Between(postDateTimeStart, postDateTimeEnd);
            exp &= _.ID.In(ModeratorManageLog.FindSQLWithTidByTime(delDateTimeStart, delDateTimeEnd));

            return exp;
        }

        public static EntityList<Topic> GetHotDebatesList(String hotField, int defHotCount, int getCount)
        {
            var exp = _.Special == 4;
            var fi = Meta.Table.FindByName(hotField);
            exp &= fi >= defHotCount;

            return FindAll(exp, null, null, 0, getCount);
        }

        public static EntityList<Topic> GetRecommendDebates(String tidList)
        {
            var exp = _.ID.In(tidList.SplitAsInt());
            exp &= _.Special == 4;

            return FindAll(exp, null, null, 0, 0);
        }

        public static SelectBuilder FindSQLWithKeyByFid(Int32 fid)
        {
            var fi = XForum.FindByID(fid);
            var vs = XForum.GetVisibleForumList().Select(e => e.ID);
            var ids = fi.AllChilds.ToList().Where(e => vs.Contains(e.ID)).Select(e => e.ID).ToArray();

            return FindSQLWithKey(_.DisplayOrder >= 0 & _.Closed != 1 & FidIn(ids));
        }
        #endregion

        #region 扩展操作
        public static void DeleteClosedTopics(Int32 fid, String topicIdList)
        {
            var list = FindAll(_.Fid == fid & _.Closed.In(topicIdList.SplitAsInt()), null, null, 0, 0);
            list.Delete();
        }
        #endregion

        #region 业务
        public static void UpdateViewCount(Int32 tid, Int32 viewcount)
        {
            var t = FindByID(tid);
            if (t != null)
            {
                t.Views += viewcount;
                // 不再实时保存浏览数，让单对象缓存自动保存
                //t.Save();
            }
        }

        /// <summary>获取指定板块最后发布的主题数，包括子版块</summary>
        /// <param name="fid"></param>
        /// <param name="visibleForums"></param>
        /// <returns></returns>
        public static Int32 GetLastPostTid(Int32 fid, String visibleForums)
        {
            //IF @visibleforums=''
            //  SELECT TOP 1 [tid] FROM [dnt_topics] AS t LEFT JOIN [dnt_forums] AS f  ON [t].[fid] = [f].[fid] 
            //  WHERE [t].[closed]<>1 AND  [t].[displayorder] >=0  AND ([t].[fid] = @fid 
            //  OR CHARINDEX(',' + CONVERT(NVARCHAR(10), @fid) + ',' , ',' + RTRIM([f].[parentidlist]) + ',') > 0 )  
            //  ORDER BY [t].[lastpost] DESC
            //ELSE
            //  EXEC('SELECT TOP 1 [tid] FROM [dnt_topics] AS t LEFT JOIN [dnt_forums] AS f  ON [t].[fid] = [f].[fid] 
            //  WHERE [t].[closed]<>1 AND  [t].[displayorder] >=0  AND ([t].[fid] = ' + @fid +
            //  'OR CHARINDEX('','' + CONVERT(NVARCHAR(10), ' + @fid + ') + '','' , '','' + RTRIM([f].[parentidlist]) + '','') > 0 )  
            //  AND [t].[fid] IN ('+@visibleforums+')  ORDER BY [t].[lastpost] DESC')

            var exp = _.Closed != 1 & _.DisplayOrder >= 0;

            if (fid > 0)
            {
                var xf = XForum.FindByID(fid);
                if (xf != null) exp &= (_.Fid == fid | _.Fid.In(xf.AllChildKeys));
            }

            if (!String.IsNullOrEmpty(visibleForums)) exp &= FidIn(visibleForums);

            var list = FindAll(exp, _.LastPost.Desc(), null, 0, 1);
            return list.Count > 0 ? list[0].ID : 0;
        }

        //public static Int32 GetTopicCount(Int32 fid)
        //{
        //	var f = XForum.FindByID(fid);
        //	if (f == null) return 0;

        //	return f.CurTopics;
        //}

        public static Int32 GetTopicCount(params Int32[] fids)
        {
            var exp = FidIn(fids) & _.DisplayOrder >= 0;
            return FindCount(exp);
        }

        public static EntityList<Topic> GetTopicList(Int32 forumid, Int32 pageindex, Int32 pagesize)
        {
            var exp = _.DisplayOrder >= 0;
            if (forumid > 0) exp &= _.Fid == forumid;
            return FindAll(exp, _.LastPostID.Desc(), null, (pageindex - 1) * pagesize, pagesize);
        }

        public static Int32 GetMaxPostTableTid()
        {
            var list = FindAll(null, _.ID.Desc(), null, 0, 1);
            return list.Count > 0 ? list[0].ID : 0;
        }

        /// <summary>获取焦点主题列表</summary>
        /// <param name="count"></param>
        /// <param name="views"></param>
        /// <param name="fid"></param>
        /// <param name="typeIdList"></param>
        /// <param name="startTime"></param>
        /// <param name="orderFieldName"></param>
        /// <param name="visibleForum"></param>
        /// <param name="isDigest"></param>
        /// <param name="onlyImg"></param>
        /// <returns></returns>
        public static EntityList<Topic> GetFocusTopicList(Int32 count, Int32 views, Int32 fid, String typeIdList, DateTime startTime, String orderFieldName, String visibleForum, bool isDigest, bool onlyImg)
        {
            var exp = _.Closed.IsFalse(false) & _.DisplayOrder >= 0;
            if (views > 0) exp &= _.Views > views;
            if (startTime.Year > 2000) exp &= _.PostDateTime > startTime;

            if (isDigest) exp &= _.Digest > 0;
            if (fid > 0) exp &= _.Fid.In(XForum.GetSubForumList(fid, 0).Where(e => String.IsNullOrEmpty(e.Password)).Select(e => e.ID));
            if (onlyImg) exp &= _.Attachment == 2;
            // 仅计算有效的论坛版面
            if (!String.IsNullOrEmpty(visibleForum)) exp &= FidIn(visibleForum);
            if (!String.IsNullOrEmpty(typeIdList)) exp &= _.TypeID.In(typeIdList.SplitAsInt());

            // 限定排序可用字段。这里跟前台数字对应，必须小心顺序
            var fi = Meta.Table.FindByName(orderFieldName);
            if ((fi as FieldItem) == null || !_FocusOrder.Contains(fi.Name)) fi = _.ID;

            return FindAll(exp, fi.Desc(), null, 0, count);
        }

        /// <summary>限定排序可用字段。这里跟前台数字对应，必须小心顺序</summary>
        public static String[] _FocusOrder = new String[] { __.ID, __.Views, __.LastPost, __.PostDateTime, __.Digest, __.Replies, __.Rate };
        //public static EntityList<Topic> GetFocusTopicList(Int32 count, Int32 views, Int32 fid, String typeIdList, DateTime startTime, Int32 orderType, String visibleForum, bool isDigest, bool onlyImg)
        //{
        //    return GetFocusTopicList(count, views, fid, typeIdList, startTime, _FocusOrder[orderType], visibleForum, isDigest, onlyImg);
        //}

        public static EntityList<Topic> GetNewTopics(Int32[] fids)
        {
            var exp = _.DisplayOrder >= 0;
            if (fids != null && fids.Length > 0) exp &= FidIn(fids);
            var list = FindAll(exp, _.ID.Desc(), null, 0, 20);
            return list;
        }

        public static EntityList<Topic> GetSitemapNewTopics(String forumIdList)
        {
            return FindAll(_.Fid.NotIn(forumIdList.SplitAsInt()), _.ID.Desc(), null, 0, 20);
        }

        public static void UpdateTopicAttentionByTidList(Int32 attention, params Int32[] tids)
        {
            var exp = new WhereExpression();
            if (tids.Length == 1)
                exp &= _.ID == tids[0];
            else
                exp &= _.ID.In(tids);

            var list = FindAll(exp, null, null, 0, 0);
            if (list.Count < 1) return;

            foreach (var item in list)
            {
                item.Attention = attention;
            }
            list.Save();
        }

        public static Int32 GetAttentionTopicCount(Int32[] fids, String keyword)
        {
            var exp = _.Attention == 1 & _.DisplayOrder > 0;
            if (fids != null && fids.Length > 0 && fids[0] > 0) exp &= FidIn(fids);
            if (!String.IsNullOrEmpty(keyword))
            {
                var exp2 = _.Title.Contains(keyword);
                exp2 |= _.Poster.Contains(keyword);
                exp &= exp2;
            }

            return FindCount(exp, null, null, 0, 0);
        }

        public static void UpdateTopicAttentionByFidList(Int32[] fids, Int32 attention, Int32 days)
        {
            var exp = _.PostDateTime < DateTime.Now.AddDays(-days);
            if (fids != null && fids.Length > 0 && fids[0] > 0) exp &= FidIn(fids);

            var list = FindAll(exp, null, null, 0, 0);
            if (list.Count < 1) return;

            foreach (var item in list)
            {
                item.Attention = attention;
            }
            list.Save();
        }

        public static Topic GetTopicInfo(Int32 tid, Int32 fid, byte mode)
        {
            var tp = FindByID(tid);
            if (tp == null) return null;

            var exp = _.Fid == fid & _.DisplayOrder >= 0;
            switch (mode)
            {
                case 1:
                    {
                        exp &= _.LastPostID > tp.LastPostID;

                        var list = FindAll(exp, _.LastPostID.Asc(), null, 0, 1);
                        return list.Count > 0 ? list[0] : null;
                    }
                case 2:
                    {
                        exp &= _.LastPostID < tp.LastPostID;

                        var list = FindAll(exp, _.LastPostID.Desc(), null, 0, 1);
                        return list.Count > 0 ? list[0] : null;
                    }
                default:
                    return tp;
            }
        }

        public static bool InSameForum(String topicidlist, Int32 fid)
        {
            var ss = topicidlist.Split(",");
            var exp = _.Fid == fid & _.ID.In(topicidlist.SplitAsInt());
            return ss.Length == FindCount(exp);
        }
        public static Topic GetForumsLastPostTid(Int32[] fids)
        {
            //String commandText = String.Format("SELECT [tid] FROM [{0}topics] WHERE [lastpostid] = (SELECT MAX([lastpostid]) FROM [{0}topics] WHERE [fid] IN({1}) AND [displayorder]>-1 AND [closed]=0)", TablePrefix, fidList);
            //return DbHelper.ExecuteScalar<Int32>(commandText);

            var exp = FidIn(fids) & _.DisplayOrder >= 0 & _.Closed == 0;
            var list = FindAll(exp, _.LastPostID.Desc(), null, 0, 1);
            return list.Count > 0 ? list[0] : null;
        }

        static EntityCache<Topic> _tops = new EntityCache<Topic>
        {
            FillListMethod = () => FindAll(_.DisplayOrder > 0, _.Fid.Asc(), null, 0, 0)
        };
        /// <summary>取得所有置顶帖子。并不多所以不需要分页</summary>
        /// <returns></returns>
        public static EntityList<Topic> GetAllTop()
        {
            //return FindAll(_.DisplayOrder > 0, _.Fid.Asc(), null, 0, 0);
            return _tops.Entities;
        }

        public static List<Topic> GetTop(Int32 fid)
        {
            var list = GetAllTop();
            if (list.Count == 0) return list;

            // 保留全局置顶以及本版面置顶
            var fi = XForum.FindByID(fid);
            var ps = fi.AllParents.ToList().Select(e => e.ID).ToList();
            if (!ps.Contains(fi.ID)) ps.Add(fi.ID);
            var ts = list.ToList().Where(
                e => ps.Contains(e.Fid) ||  // 当前版面的父级
                    e.DisplayOrder >= 3 ||  // 全局置顶
                    e.DisplayOrder == 2 && e.Forum.AllParents.Count > 0 && ps.Contains(e.Forum.AllParents[0].ID)    // 本分类置顶
                )
                .OrderByDescending(e => e.LastPost)
                .OrderByDescending(e => e.DisplayOrder)
                .ToList();

            return ts;
        }

        public static EntityList<Topic> GetHotReplyTopics(Int32 count)
        {
            return FindAll(_.DisplayOrder >= 0, _.Replies.Desc(), null, 0, count);
        }

        public static EntityList<Topic> GetHotTopics(Int32 count)
        {
            return FindAll(_.DisplayOrder >= 0, _.Views.Desc(), null, 0, count);
        }
        #endregion

        #region 帖子逻辑
        /// <summary>发帖主函数，涉及所有数据库操作，事务保护</summary>
        public void Create(Post pi)
        {
            using (var trans = Meta.CreateTrans())
            {
                this.Insert();

                pi.Tid = ID;
                pi.Create();

                if (DisplayOrder == 0)
                {
                    var st = Statistic.Current;
                    st.TotalTopic++;
                    st.Save();
                }

                // 所有上级论坛帖子数增加
                var ff = Forum;
                while (ff != null && ff.ID != 0)
                {
                    ff.Topics++;
                    ff.CurTopics++;

                    ff.Save();

                    ff = ff.Parent;
                }

                // 插入我的帖子
                if (PosterID != -1)
                {
                    var my = new MyTopic();
                    my.Uid = PosterID;
                    my.Tid = ID;
                    my.Dateline = PostDateTime;
                    my.Insert();
                }

                trans.Commit();
            }
        }

        //public Post CreatePost()
        //{
        //	var pi = new Post();
        //	pi.CopyFrom(this);
        //	pi.Tid = ID;
        //	//pi.Title

        //	return pi;
        //}

        public Int32 Repair()
        {
            //Int32 result = DatabaseProvider.GetInstance().RepairTopics(tid, posttable);

            var pt = Post.FindLastByTid(ID);
            if (pt == null) return 0;

            LastPost = pt.PostDateTime;
            LastPostID = pt.ID;
            LastPosterID = pt.PosterID;
            LastPoster = pt.Poster;
            Replies = Post.GetReplies(ID);

            return Save();
        }

        public static Int32 ChangeFid(Int32 srcFid, Int32 targetFid)
        {
            return Update(_.Fid == targetFid, _.Fid == srcFid);
        }
        #endregion

        #region 审核
        public static void PassAuditNewTopic(String tidList)
        {
            var list = FindAllByIDs(tidList);
            foreach (var item in list)
            {
                item.Post.Invisible = 0;
                item.Post.Save();
            }
        }
        #endregion

        #region 搜索主题
        public static String GetSearchTopicTitleSQL(int posterId, String fids, int resultOrder, int resultOrderType, int searchTime, int searchTimeType, int digest, String keyWord)
        {
            //var TablePrefix = BaseConfigInfo.Current.Tableprefix;
            var where = SearchWhere(posterId, fids, searchTime, searchTimeType, digest, keyWord);

            return FindSQL(where, GetOrder(resultOrder, resultOrderType), __.ID);
        }

        static String SearchWhere(int posterId, String fids, int searchTime, int searchTimeType, int digest, String keyWord)
        {
            keyWord = Regex.Replace(keyWord, "--|;|'|\"", "", RegexOptions.Multiline | RegexOptions.Compiled);
            var sb = new StringBuilder(keyWord);
            sb.Replace("'", "''");
            sb.Replace("%", "[%]");
            sb.Replace("_", "[_]");
            sb.Replace("[", "[[]");
            keyWord = sb.ToString();

            var exp = _.DisplayOrder >= 0;
            if (posterId > 0) exp &= _.PosterID == posterId;
            if (digest > 0) exp &= _.Digest > digest;
            if (searchTime != 0)
            {
                if (searchTimeType == 1)
                    exp &= _.PostDateTime < DateTime.Now.Date.AddDays(searchTime);
                else
                    exp &= _.PostDateTime > DateTime.Now.Date.AddDays(searchTime);
            }
            if (!fids.IsNullOrWhiteSpace()) exp &= FidIn(fids);
            if (!keyWord.IsNullOrWhiteSpace())
            {
                // 暂时不处理多关键字
                exp &= _.Title.Contains(keyWord);
            }

            return exp;
        }

        static String GetOrder(int resultOrder, int resultOrderType)
        {
            var sort = _.LastPostID;
            switch (resultOrder)
            {
                case 1:
                    sort = _.ID;
                    break;
                case 2:
                    sort = _.Replies;
                    break;
                case 3:
                    sort = _.Views;
                    break;
            }

            return resultOrderType == 1 ? sort.Asc() : sort.Desc();
        }

        public static String GetSearchPostContentSQL(int posterId, String fids, int resultOrder, int resultOrderType, int searchTime, int searchTimeType, String keyWord)
        {
            var where = Post.SearchWhere(posterId, fids, searchTime, searchTimeType, keyWord);

            return FindSQL(_.DisplayOrder >= 0 & where, GetOrder(resultOrder, resultOrderType), __.ID);
        }

        public static String GetSearchByPosterSQL(int posterId)
        {
            //if (posterId > 0)
            //{
            //    var TablePrefix = BaseConfigInfo.Current.Tableprefix;

            //    return String.Format("SELECT DISTINCT [tid], 'forum' AS [datafrom] FROM [{0}post] WHERE [posterid]={1} AND [tid] NOT IN (SELECT [tid] FROM [{0}topic] WHERE [posterid]={1} AND [displayorder]<0)", TablePrefix, posterId);
            //}
            //return "";

            if (posterId <= 0) return "";

            var exp = _.DisplayOrder < 0;
            if (posterId > 0) exp &= _.PosterID == posterId;
            //return exp;

            return FindSQLWithKey(exp);
        }
        #endregion

        #region 辅助
        /// <summary>构造Fid In(xx,xx,xx)，检查论坛版面ID有效性</summary>
        /// <param name="fids"></param>
        /// <returns></returns>
        static Expression FidIn(String fids) { return FidIn(fids.SplitAsInt()); }

        static Expression FidIn(Int32[] fids)
        {
            // 如果要过滤的论坛数就是论坛总数，那么直接不需要过滤
            var count = fids.Distinct().Count();
            if (count == XForum.Meta.Count || count == XForum.GetForumCountNoTop()) return new Expression();

            var fs = XForum.Root.AllChilds.ToList().Select(e => e.ID).ToArray();
            // 计算两个数组的交集
            return _.Fid.In(fids.Intersect(fs).ToArray());
        }
        #endregion
    }
}