﻿using System;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using BBX.Common;
using BBX.Config;
using NewLife.Web;
using XCode;

namespace BBX.Entity
{
    public enum SearchType
    {
        DigestTopic,
        TopicTitle,
        PostContent,
        //AlbumTitle,
        //SpacePostTitle,
        ByPoster,
        Error
    }

    /// <summary>搜索缓存</summary>
    public partial class SearchCache : EntityBase<SearchCache>
    {
        #region 对象操作﻿

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

            if (!Dirtys[_.Ip]) Ip = WebHelper.UserHost;
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
        //    if (XTrace.Debug) XTrace.WriteLine("开始初始化{0}搜索缓存数据……", typeof(SearchCache).Name);

        //    var entity = new SearchCache();
        //    entity.Keywords = "abc";
        //    entity.Searchstring = "abc";
        //    entity.Ip = "abc";
        //    entity.Uid = 0;
        //    entity.GroupID = 0;
        //    entity.PostDateTime = "abc";
        //    entity.Expiration = "abc";
        //    entity.Topics = 0;
        //    entity.Tids = "abc";
        //    entity.Insert();

        //    if (XTrace.Debug) XTrace.WriteLine("完成初始化{0}搜索缓存数据！", typeof(SearchCache).Name);
        //}


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
        #endregion

        #region 扩展查询﻿
        /// <summary>根据Searchstring、聊天组查找</summary>
        /// <param name="searchstring"></param>
        /// <param name="groupid">聊天组</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static EntityList<SearchCache> FindAllBySearchstringAndGroupID(String searchstring, Int32 groupid)
        {
            if (Meta.Count >= 1000)
                return FindAll(new String[] { _.Searchstring, _.GroupID }, new Object[] { searchstring, groupid });
            else // 实体缓存
                return Meta.Cache.Entities.FindAll(e => e.Searchstring == searchstring && e.GroupID == groupid);
        }

        /// <summary>根据编号查找</summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static SearchCache FindByID(Int32 id)
        {
            if (Meta.Count >= 1000)
                return Find(_.ID, id);
            else // 实体缓存
                return Meta.Cache.Entities.Find(__.ID, id);
            // 单对象缓存
            //return Meta.SingleCache[id];
        }
        #endregion

        #region 高级查询
        // 以下为自定义高级查询的例子

        ///// <summary>
        ///// 查询满足条件的记录集，分页、排序
        ///// </summary>
        ///// <param name="key">关键字</param>
        ///// <param name="orderClause">排序，不带Order By</param>
        ///// <param name="startRowIndex">开始行，0表示第一行</param>
        ///// <param name="maximumRows">最大返回行数，0表示所有行</param>
        ///// <returns>实体集</returns>
        //[DataObjectMethod(DataObjectMethodType.Select, true)]
        //public static EntityList<SearchCache> Search(String key, String orderClause, Int32 startRowIndex, Int32 maximumRows)
        //{
        //    return FindAll(SearchWhere(key), orderClause, null, startRowIndex, maximumRows);
        //}

        ///// <summary>
        ///// 查询满足条件的记录总数，分页和排序无效，带参数是因为ObjectDataSource要求它跟Search统一
        ///// </summary>
        ///// <param name="key">关键字</param>
        ///// <param name="orderClause">排序，不带Order By</param>
        ///// <param name="startRowIndex">开始行，0表示第一行</param>
        ///// <param name="maximumRows">最大返回行数，0表示所有行</param>
        ///// <returns>记录数</returns>
        //public static Int32 SearchCount(String key, String orderClause, Int32 startRowIndex, Int32 maximumRows)
        //{
        //    return FindCount(SearchWhere(key), null, null, 0, 0);
        //}

        /// <summary>构造搜索条件</summary>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        private static String SearchWhere(String key)
        {
            // WhereExpression重载&和|运算符，作为And和Or的替代


            // SearchWhereByKeys系列方法用于构建针对字符串字段的模糊搜索
            var exp = SearchWhereByKeys(key);

            // 以下仅为演示，2、3行是同一个意思的不同写法，Field（继承自FieldItem）重载了==、!=、>、<、>=、<=等运算符（第4行）
            //exp &= _.Name == "testName"
            //    & !String.IsNullOrEmpty(key) & _.Name == key
            //    .AndIf(!String.IsNullOrEmpty(key), _.Name == key)
            //    | _.ID > 0;

            return exp;
        }
        #endregion

        #region 扩展操作
        #endregion

        #region 业务
        public static void DeleteExpried()
        {
            // DELETE FROM [{0}searchcaches] WHERE [expiration]<@expiration

            // 先试试缓存
            var exp = DateTime.Now.AddMinutes(-30);
            if (Meta.Count < 1000)
            {
                var list = new EntityList<SearchCache>();
                foreach (var item in FindAllWithCache())
                {
                    if (item.Expiration < exp) list.Add(item);
                }
                list.Delete();
            }
            else
            {
                var list = FindAll(_.Expiration < exp, null, null, 0, 1000);
                list.Delete();
            }
        }

        private static Regex regexForumTopics = new Regex("<ForumTopics>([\\s\\S]+?)</ForumTopics>");

        public static int Search(int userId, int userGroupId, string keyWord, int posterId, SearchType searchType, string fids, int searchTime, int searchTimeType, int resultOrder, int resultOrderType)
        {
            //if (postTableId == 0) postTableId = TableList.GetAllPostTable()[0].ID;

            //return Search(posttableid, userid, usergroupid, keyword, posterid, searchType, searchforumid, searchtime, searchtimetype, resultorder, resultordertype);
            //DatabaseProvider.GetInstance().DeleteExpriedSearchCache();
            SearchCache.DeleteExpried();

            var sql = string.Empty;
            var sb = new StringBuilder();
            switch (searchType)
            {
                case SearchType.DigestTopic:
                    sql = Topic.GetSearchTopicTitleSQL(posterId, fids, resultOrder, resultOrderType, searchTime, searchTimeType, 1, keyWord);
                    break;

                case SearchType.TopicTitle:
                    sql = Topic.GetSearchTopicTitleSQL(posterId, fids, resultOrder, resultOrderType, searchTime, searchTimeType, 0, keyWord);
                    break;

                case SearchType.PostContent:
                    sql = Topic.GetSearchPostContentSQL(posterId, fids, resultOrder, resultOrderType, searchTime, searchTimeType, keyWord);
                    break;

                case SearchType.ByPoster:
                    sql = Topic.GetSearchByPosterSQL(posterId);
                    break;

                default:
                    sql = Topic.GetSearchTopicTitleSQL(posterId, fids, resultOrder, resultOrderType, searchTime, searchTimeType, 0, keyWord);
                    break;
            }
            if (Utils.StrIsNullOrEmpty(sql)) return -1;

            var list = SearchCache.FindAllBySearchstringAndGroupID(sql, userGroupId);
            if (list.Count > 0) return list[0].ID;

            var ds = Meta.Session.Query(sql);
            int num2 = 0;
            if (ds.Tables[0].Rows.Count > 0)
            {
                switch (searchType)
                {
                    case SearchType.DigestTopic:
                    case SearchType.TopicTitle:
                    case SearchType.PostContent:
                        sb.Append("<ForumTopics>");
                        break;

                    case SearchType.ByPoster:
                        {
                            sb = GetSearchByPosterResult(ds);
                            var sc = new SearchCache();
                            sc.Keywords = keyWord;
                            sc.Searchstring = sql;
                            sc.PostDateTime = DateTime.Now;
                            sc.Topics = num2;
                            sc.Tids = sb.ToString();
                            sc.Uid = userId;
                            sc.GroupID = userGroupId;
                            sc.Ip = WebHelper.UserHost;
                            sc.Expiration = DateTime.Now;
                            sc.Insert();
                            return sc.ID;
                        }
                }
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    sb.Append(dr[0].ToString());
                    sb.Append(",");
                    num2++;
                }
                if (num2 > 0)
                {
                    sb.Remove(sb.Length - 1, 1);
                    switch (searchType)
                    {
                        case SearchType.DigestTopic:
                        case SearchType.TopicTitle:
                        case SearchType.PostContent:
                            sb.Append("</ForumTopics>");
                            break;
                    }
                    var sc = new SearchCache();
                    sc.Keywords = keyWord;
                    sc.Searchstring = sql;
                    sc.PostDateTime = DateTime.Now;
                    sc.Topics = num2;
                    sc.Tids = sb.ToString();
                    sc.Uid = userId;
                    sc.GroupID = userGroupId;
                    sc.Ip = WebHelper.UserHost;
                    sc.Expiration = DateTime.Now;
                    sc.Insert();
                    return sc.ID;
                }
            }
            return -1;
        }

        private static string RegEsc(string str)
        {
            string[] array = new string[] { "%", "_", "'" };
            string[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                string text = array2[i];
                string a;
                if ((a = text) != null)
                {
                    if (!(a == "%"))
                    {
                        if (!(a == "_"))
                        {
                            if (a == "'")
                            {
                                str = str.Replace(text, "['']");
                            }
                        }
                        else
                        {
                            str = str.Replace(text, "[_]");
                        }
                    }
                    else
                    {
                        str = str.Replace(text, "[%]");
                    }
                }
            }
            return str;
        }

        static StringBuilder GetSearchByPosterResult(DataSet ds)
        {
            var sb = new StringBuilder("<ForumTopics>");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                sb.AppendFormat("{0},", dr[0].ToString());
            }
            if (sb.ToString().EndsWith(","))
            {
                sb.Length--;
            }
            return sb.Append("</ForumTopics>");
        }

        public static EntityList<Topic> GetSearchCacheList(int searchid, int pagesize, int pageindex, out int topiccount, SearchType searchType)
        {
            var list = new EntityList<Topic>();

            topiccount = 0;
            //DataTable searchCache = BBX.Data.Searches.GetSearchCache(searchid);
            var searchCache = SearchCache.FindByID(searchid);
            if (searchCache == null) return list;

            var input = searchCache.Tids;

            var match = regexForumTopics.Match(input);
            if (match.Success)
            {
                string tids = GetCurrentPageTids(match.Groups[1].Value, out topiccount, pagesize, pageindex);
                if (Utils.StrIsNullOrEmpty(tids)) return list;

                if (searchType == SearchType.DigestTopic)
                    return GetSearchDigestTopicsList(pagesize, tids);

                return GetSearchTopicsList(pagesize, tids);
            }
            return list;
        }

        static EntityList<Topic> GetSearchDigestTopicsList(int pageSize, string strTids)
        {
            return Topic.FindAll(Topic._.ID.In(strTids.SplitAsInt()), null, null, 0, pageSize);
        }

        static EntityList<Topic> GetSearchTopicsList(int pageSize, string strTids)
        {
            return Topic.FindAll(Topic._.ID.In(strTids.SplitAsInt()), null, null, 0, pageSize);
        }

        private static string GetCurrentPageTids(string tids, out int topiccount, int pagesize, int pageindex)
        {
            var arr = Utils.SplitString(tids, ",");
            topiccount = arr.Length;
            int page = (topiccount % pagesize == 0) ? (topiccount / pagesize) : (topiccount / pagesize + 1);
            if (page < 1) page = 1;

            if (pageindex > page) pageindex = page;

            int start = pagesize * (pageindex - 1);
            var sb = new StringBuilder();
            int k = start;
            while (k < topiccount && k <= start + pagesize)
            {
                sb.Append(arr[k]);
                sb.Append(",");
                k++;
            }
            return sb.Remove(sb.Length - 1, 1).ToString();
        }

        //public static int GetPostsCount(Int32 postTableid)
        //{
        //    var old = Topic.Meta.ConnName;
        //    Topic.Meta.ConnName = "dnt_posts" + postTableid;
        //    try
        //    {
        //        return Topic.Meta.Count;
        //    }
        //    finally
        //    {
        //        Topic.Meta.ConnName = old;
        //    }
        //}
        #endregion
    }
}