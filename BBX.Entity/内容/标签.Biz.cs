/*
 * XCoder v6.1.5277.25230
 * 作者：nnhy/X
 * 时间：2014-07-13 23:26:14
 * 版权：版权所有 (C) 新生命开发团队 2002~2014
*/
﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using XCode;
using XCode.Membership;

namespace BBX.Entity
{
    /// <summary>标签</summary>
    public partial class Tag : Entity<Tag>
    {
        #region 对象操作﻿
        static Tag()
        {
            var fs = Meta.Factory.AdditionalFields;
            fs.Add(__.Count);
            fs.Add(__.PCount);
            fs.Add(__.FCount);
            fs.Add(__.VCount);
            fs.Add(__.GCount);
        }

        /// <summary>验证数据，通过抛出异常的方式提示验证失败。</summary>
        /// <param name="isNew"></param>
        public override void Valid(Boolean isNew)
        {
            if (!HasDirty) return;

            // 建议先调用基类方法，基类方法会对唯一索引的数据进行验证
            base.Valid(isNew);

            // 在新插入数据或者修改了指定字段时进行唯一性验证，CheckExist内部抛出参数异常
            //if (isNew || Dirtys[__.Name]) CheckExist(__.Name);

            // 处理当前已登录用户信息
            if (!Dirtys[__.UserID] && ManageProvider.Provider.Current != null) UserID = (Int32)ManageProvider.Provider.Current.ID;
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
        //    if (XTrace.Debug) XTrace.WriteLine("开始初始化{0}[{1}]数据……", typeof(Tag).Name, Meta.Table.DataTable.DisplayName);

        //    var entity = new Tag();
        //    entity.Name = "abc";
        //    entity.UserID = 0;
        //    entity.PostDateTime = DateTime.Now;
        //    entity.OrderID = 0;
        //    entity.Color = "abc";
        //    entity.Count = 0;
        //    entity.FCount = 0;
        //    entity.PCount = 0;
        //    entity.SCount = 0;
        //    entity.VCount = 0;
        //    entity.GCount = 0;
        //    entity.Insert();

        //    if (XTrace.Debug) XTrace.WriteLine("完成初始化{0}[{1}]数据！", typeof(Tag).Name, Meta.Table.DataTable.DisplayName);
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
        /// <summary>根据编号查找</summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static Tag FindByID(Int32 id)
        {
            //if (Meta.Count >= 1000)
            //    return Find(__.ID, id);

            // 实体缓存
            if (Meta.Count < 1000) return Meta.Cache.Entities.Find(__.ID, id);

            // 单对象缓存
            return Meta.SingleCache[id];
        }

        public static EntityList<Tag> FindAllByIDs(params Int32[] ids)
        {
            // 实体缓存
            if (Meta.Count < 1000) return Meta.Cache.Entities.FindAll(e => ids.Contains(e.ID));

            // 单对象缓存，数量不多的时候可以考虑从单对象缓存里面获取
            if (ids.Length <= 20)
            {
                var list = new EntityList<Tag>();
                foreach (var item in ids.Distinct())
                {
                    var entity = Meta.SingleCache[item];
                    if (entity != null) list.Add(entity);
                }
                return list;
            }

            return FindAll(_.ID.In(ids), null, null, 0, 0);
        }

        public static Tag FindByName(String name)
        {
            if (Meta.Count >= 1000)
                return Find(__.Name, name);
            else // 实体缓存
                return Meta.Cache.Entities.Find(__.Name, name);
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
        //public static EntityList<Tag> Search(String key, String orderClause, Int32 startRowIndex, Int32 maximumRows)
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
            var exp = SearchWhereByKeys(key, null);

            // 以下仅为演示，Field（继承自FieldItem）重载了==、!=、>、<、>=、<=等运算符（第4行）
            //if (userid > 0) exp &= _.OperatorID == userid;
            //if (isSign != null) exp &= _.IsSign == isSign.Value;
            //if (start > DateTime.MinValue) exp &= _.OccurTime >= start;
            //if (end > DateTime.MinValue) exp &= _.OccurTime < end.AddDays(1).Date;

            return exp;
        }
        #endregion

        #region 扩展操作
        #endregion

        #region 业务

        public static EntityList<Tag> GetForumTags(string tagName, int type, Int32 fcountbegin = 0, Int32 fcountend = 0, Int32 start = 0, Int32 max = 0)
        {
            //string text = string.Format("SELECT {0} FROM [{1}tags]  {2} ", "[tagid],[tagname],[userid],[postdatetime],[orderid],[color],[count],[fcount],[pcount],[scount],[vcount],[gcount]", TablePrefix, (!Utils.StrIsNullOrEmpty(tagName)) ? (" WHERE [tagname] LIKE '%" + RegEsc(tagName) + "%'") : "");
            //if (type == 1)
            //{
            //    text += ((!Utils.StrIsNullOrEmpty(tagName)) ? " AND [orderid] < 0 " : " WHERE [orderid] < 0 ");
            //}
            //else
            //{
            //    if (type == 2)
            //    {
            //        text += ((!Utils.StrIsNullOrEmpty(tagName)) ? " AND [orderid] >= 0" : " WHERE [orderid] >= 0 ");
            //    }
            //}
            //text += " ORDER BY [fcount] DESC";
            //return DbHelper.ExecuteDataset(CommandType.Text, text).Tables[0];

            var exp = new WhereExpression();
            if (!String.IsNullOrEmpty(tagName)) exp &= _.Name.Contains(tagName);
            if (type == 1)
                exp &= _.OrderID < 0;
            else if (type == 2)
                exp &= _.OrderID >= 0;

            if (fcountbegin > 0) exp &= _.FCount > fcountbegin;
            if (fcountend > 0) exp &= _.FCount < fcountend;

            return FindAll(exp, _.FCount.Desc(), null, start, max);
        }

        public static bool UpdateForumTags(int tagid, int orderid, string color)
        {
            var regex = new Regex("^#?([0-9|A-F]){6}$");
            if (color != "" && !regex.IsMatch(color)) return false;

            //BBX.Data.Tags.UpdateForumTags(tagid, orderid, color.Replace("#", ""));
            var tag = FindByID(tagid);
            if (tag == null) return false;

            tag.OrderID = orderid;
            tag.Color = color;
            tag.Update();

            return true;
        }

        public static void DeleteTopicTags(int topicid)
        {
            //UPDATE [dnt_tags] SET [count]=[count]-1,[fcount]=[fcount]-1 
            //WHERE EXISTS (SELECT [tagid] FROM [dnt_topictags] WHERE [tid] = @tid AND [tagid] = [dnt_tags].[tagid])

            //DELETE FROM [dnt_topictags] WHERE [tid] = @tid	

            var list = TopicTag.FindAllByTid(topicid);
            if (list.Count <= 0) return;

            using (var trans = Meta.CreateTrans())
            {
                foreach (var tp in list)
                {
                    tp.Delete();

                    var tag = FindByID(tp.TagID);
                    if (tag == null) continue;

                    tag.Count--;
                    tag.FCount--;
                    tag.Update();
                }
            }
        }

        public static EntityList<Tag> GetTagsListByTopic(int topicid)
        {
            var list = TopicTag.FindAllByTid(topicid);
            if (list.Count <= 0) return new EntityList<Tag>();

            //return FindAll(_.ID.In(list.GetItem<Int32>(TopicTag._.TagID)), null, null, 0, 0);
            return FindAllByIDs(list.GetItem<Int32>(TopicTag._.TagID).ToArray());
        }

        public static String GetTagsByTopicId(Int32 topicid)
        {
            var list = GetTagsListByTopic(topicid);
            if (list.Count <= 0) return "";

            return list.Join(__.Name, ",");
        }

        public static EntityList<Tag> GetHotForumTags(int count)
        {
            //SELECT TOP {0} {1} FROM [{2}tags] WHERE [fcount] > 0 AND [orderid] > -1 ORDER BY [orderid], [fcount] DESC

            // 增加实体缓存支持
            if (Meta.Count < 1000)
            {
                var list = Meta.Cache.Entities.ToList().AsEnumerable();
                list = list.Where(e => e.FCount > 0 && e.OrderID > -1);
                list = list.OrderByDescending(e => e.FCount);
                list = list.OrderBy(e => e.OrderID);
                return new EntityList<Tag>(list);
            }

            return FindAll(_.FCount > 0 & _.OrderID > -1, _.OrderID.Asc() & _.FCount.Desc(), null, 0, count);
        }

        public static void CreateTopicTags(string[] tagArray, int topicId, int userId, string currentDateTime)
        {
            //EXEC [dnt_createtags] @tags, @userid, @postdatetime
            //INSERT INTO [dnt_tags]([tagname], [userid], [postdatetime], [orderid], [color], [count], [fcount], [pcount], [scount], [vcount]) 
            //    SELECT [item], @userid, @postdatetime, 0, '', 0, 0, 0, 0, 0 FROM [dnt_split](@tags, ' ') AS [newtags] 
            //    WHERE NOT EXISTS (SELECT [tagname] FROM [dnt_tags] WHERE [newtags].[item] = [tagname])

            //UPDATE [dnt_tags] SET [fcount]=[fcount]+1,[count]=[count]+1
            //WHERE EXISTS (SELECT [item] FROM [dnt_split](@tags, ' ') AS [newtags] WHERE [newtags].[item] = [tagname])

            //INSERT INTO [dnt_topictags] (tagid, tid)
            //SELECT tagid, @tid FROM [dnt_tags] WHERE EXISTS (SELECT [item] FROM [dnt_split](@tags, ' ') WHERE [item] = [dnt_tags].[tagname])

            foreach (var item in tagArray)
            {
                var tag = FindByName(item);
                if (tag == null)
                {
                    tag = new Tag();
                    tag.Name = item;
                    tag.UserID = userId;
                    tag.PostDateTime = currentDateTime.ToDateTime();
                }

                tag.Count++;
                tag.FCount++;
                tag.Save();

                var tp = TopicTag.FindByTidAndTagID(topicId, tag.ID);
                if (tp == null)
                {
                    tp = new TopicTag();
                    tp.Tid = topicId;
                    tp.TagID = tag.ID;
                    tp.Insert();
                }
            }
        }

        public static String GetHotTagsListForForum(int count)
        {
            var hotTagsListForForum = GetHotForumTags(count);
            return GetTags(hotTagsListForForum, string.Empty, true);
        }

        public static String GetHotTagsListForForumJSONP(int count)
        {
            var hotTagsListForForum = GetHotForumTags(count);
            return GetTags(hotTagsListForForum, "forumhottag_callback", true);
        }

        public static String GetTopicTags(int topicid)
        {
            //var sb = new StringBuilder();
            //sb.Append(BaseConfigs.GetForumPath);
            //sb.Append("cache/topic/magic/");
            //sb.Append((topicid / 1000 + 1).ToString());
            //sb.Append("/");
            //string mapPath = Utils.GetMapPath(sb.ToString() + topicid.ToString() + "_tags.config");
            //List<TagInfo> tagsListByTopic = ForumTags.GetTagsListByTopic(topicid);
            //Tags.WriteTagsCacheFile(mapPath, tagsListByTopic, string.Empty, false);

            var list = GetTagsListByTopic(topicid);
            return GetTags(list, "", false);
        }

        static String GetTags(List<Tag> tags, string jsonp_callback, bool outputcountfield)
        {
            if (tags.Count <= 0) return null;

            var sb = new StringBuilder();
            if (!String.IsNullOrEmpty(jsonp_callback))
            {
                sb.Append(jsonp_callback);
                sb.Append("(");
            }
            sb.Append("[\r\n  ");
            foreach (var tag in tags)
            {
                if (outputcountfield)
                    sb.AppendFormat("{{'tagid' : '{0}', 'tagname' : '{1}', 'fcount' : '{2}', 'pcount' : '{3}', 'scount' : '{4}', 'vcount' : '{5}', 'gcount' : '{6}'}}, ", tag.ID, tag.Name, tag.FCount, tag.PCount, tag.SCount, tag.VCount, tag.GCount);
                else
                    sb.AppendFormat("{{'tagid' : '{0}', 'tagname' : '{1}'}}, ", tag.ID, tag.Name);
            }
            sb.Append("\r\n]");
            if (!String.IsNullOrEmpty(jsonp_callback)) sb.Append(")");

            return sb.ToString();
        }
        #endregion
    }
}