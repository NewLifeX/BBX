﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using BBX.Common;
using BBX.Config;
using NewLife.Log;
using XCode;
using XCode.Configuration;

namespace BBX.Entity
{
    /// <summary>论坛</summary>
    public partial class XForum : EntityTree<XForum>
    {
        #region 对象操作﻿
        static XForum()
        {
            var fs = Meta.Factory.AdditionalFields;
            fs.Add(__.Posts);
            fs.Add(__.TodayPosts);
            fs.Add(__.Topics);
            fs.Add(__.CurTopics);

            Setting = new ForumSetting();
        }

        class ForumSetting : EntityTreeSetting<XForum>
        {
            public override Boolean BigSort { get { return false; } }
            public override string Sort { get { return __.DisplayOrder; } }
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

            //if (!String.IsNullOrEmpty(Parentidlist)) Parentidlist = Parentidlist.Replace(" ", null);
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            this.TrimField();
        }

        /// <summary>首次连接数据库时初始化数据，仅用于实体类重载，用户不应该调用该方法</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void InitData()
        {
            base.InitData();

            // InitData一般用于当数据表没有数据时添加一些默认数据，该实体类的任何第一次数据库操作都会触发该方法，默认异步调用
            // Meta.Count是快速取得表记录数
            if (Meta.Count > 0) return;

            // 需要注意的是，如果该方法调用了其它实体类的首次数据库操作，目标实体类的数据初始化将会在同一个线程完成
            if (XTrace.Debug) XTrace.WriteLine("开始初始化{0}论坛数据……", typeof(XForum).Name);

            //INSERT INTO [dnt_forums] ([parentid], [layer],[pathlist],[parentidlist],[subforumcount],[name],[status],[colcount],[displayorder],[templateid],[topics],[curtopics],[posts],[todayposts],[lasttid],[lasttitle],[lastpost],[lastposterid],[lastposter],[allowsmilies],[allowrss],[allowhtml],[allowbbcode],[allowimgcode],[allowblog],[istrade],[alloweditrules],[allowthumbnail],[recyclebin],[modnewposts],[jammer],[disablewatermark],[inheritedmod],[autoclose]) VALUES (0, 0, '<a href="showforum-1.aspx">默认分类</a>', '0', 1, '默认分类', 1, 1, 1, 0, 0, 0, 0, 0, 0, '', '1900-1-1 0:00:00', 0, '', 1, 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            //INSERT INTO [dnt_forumfields] VALUES (1,'', '', '', '', '', '', '', '', '', '', '', '', '', '', '',0,0,0,0,'','','','');

            var entity = new XForum();
            //entity.ParentID = 0;
            //entity.Layer = 0;
            //entity.Pathlist = "<a href=\"showforum-1.aspx\">默认分类</a>";
            //entity.Parentidlist = "0";
            //entity.SubforumCount = 1;
            entity.Name = "默认分类";
            entity.Status = 1;
            entity.ColCount = 1;
            entity.DisplayOrder = 1;
            //entity.TemplateID = 0;
            //entity.Topics = 0;
            //entity.Curtopics = 0;
            //entity.Posts = 0;
            //entity.TodayPosts = 0;
            //entity.LastTID = 0;
            //entity.LastTitle = "abc";
            //entity.LastPost = "abc";
            //entity.LastPosterID = 0;
            //entity.LastPoster = "abc";
            entity.AllowSmilies = true;
            entity.AllowRss = true;
            entity.AllowHtml = false;
            entity.AllowBbCode = true;
            entity.AllowImgCode = true;
            //entity.AllowBlog = true;
            //entity.IsTrade = true;
            //entity.AllowPostSpecial = true;
            //entity.AllowSpecialOnly = true;
            //entity.AllowEditRules = true;
            //entity.AllowThumbnail = true;
            //entity.AllowTag = true;
            //entity.Recyclebin = 0;
            //entity.Modnewposts = 0;
            //entity.Modnewtopics = 0;
            //entity.Jammer = 0;
            //entity.DisableWatermark = true;
            //entity.Inheritedmod = 0;
            //entity.AutoClose = 0;
            entity.Insert();

            //INSERT INTO [dnt_forums] ([parentid], [layer],[pathlist],[parentidlist],[subforumcount],[name],[status],[colcount],[displayorder],[templateid],[topics],[curtopics],[posts],[todayposts],[lasttid],[lasttitle],[lastpost],[lastposterid],[lastposter],[allowsmilies],[allowrss],[allowhtml],[allowbbcode],[allowimgcode],[allowblog],[istrade],[allowpostspecial],[alloweditrules],[allowthumbnail],[allowtag],[recyclebin],[modnewposts],[jammer],[disablewatermark],[inheritedmod],[autoclose]) VALUES (1, 1, '<a href="showforum-1.aspx">默认分类</a><a href="showforum-2.aspx">默认版块</a>', '1', 0, '默认版块', 1, 1, 2, 0, 0, 0, 0, 0, 0, '', '1900-1-1 0:00:00', 0, '', 1, 1, 0, 1, 1, 0, 0, 21, 0, 0, 1, 0, 0, 0, 0, 0, 0);
            //INSERT INTO [dnt_forumfields] VALUES (2,'', '', '', '', '', '', '', '', '', '', '', '', '', '', '默认版块说明文字',0,0,0,0,'','','','');

            entity = new XForum();
            entity.ParentID = 1;
            //entity.Layer = 1;
            //entity.Pathlist = "<a href=\"showforum-1.aspx\">默认分类</a><a href=\"showforum-2.aspx\">默认版块</a>";
            //entity.Parentidlist = "1";
            //entity.SubforumCount = 0;
            entity.Name = "默认版块";
            entity.Status = 1;
            entity.ColCount = 1;
            entity.DisplayOrder = 2;
            entity.AllowSmilies = true;
            entity.AllowRss = true;
            entity.AllowHtml = false;
            entity.AllowBbCode = true;
            entity.AllowImgCode = true;
            entity.AllowPostSpecial = 21;
            entity.Insert();

            if (XTrace.Debug) XTrace.WriteLine("完成初始化{0}论坛数据！", typeof(XForum).Name);
        }

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
            var rs = base.OnInsert();
            Field.Fid = ID;
            return rs + Field.Insert();
        }

        protected override int OnUpdate()
        {
            return base.OnUpdate() + Field.Update();
        }

        protected override int OnDelete()
        {
            return Field.Delete() + base.OnDelete();
        }
        #endregion

        #region 扩展属性﻿
        private List<String> hasLoad = new List<String>();
        private ForumField _Field;
        /// <summary>扩展字段</summary>
        public ForumField Field
        {
            get
            {
                if (_Field == null && !hasLoad.Contains("Field"))
                {
                    _Field = ForumField.FindByID(ID);
                    if (_Field == null) _Field = new ForumField { Fid = ID };
                    hasLoad.Add("Field");
                }
                return _Field;
            }
            set { _Field = value; }
        }

        //IForumField IXForum.Field { get { return Field; } }

        IXForum IXForum.Parent { get { return Parent; } }

        private String _ModeratorsHtml;
        /// <summary>版主列表</summary>
        public String ModeratorsHtml { get { return _ModeratorsHtml; } set { _ModeratorsHtml = value; } }

        /// <summary>论坛是否可见，不可见的论坛为隐藏论坛</summary>
        public Boolean Visible { get { return Status != 0; } set { Status = value ? 1 : 0; } }

        /// <summary>层次深度</summary>
        public Int32 Layer { get { return Deepth - 1; } }

        ///// <summary>是否允许任何游客访问，要求论坛可见，授权为空或者允许游客</summary>
        //public Boolean AllowAny
        //{
        //    get
        //    {
        //        if (!Visible) return false;

        //        if (Field != null && !Field.AllowView(7)) return false;

        //        return true;
        //    }
        //}

        private String _Pathlist;
        /// <summary>论坛路径，用于导航链接</summary>
        public String Pathlist
        {
            get
            {
                if (_Pathlist == null)
                {
                    //_Pathlist = "<a href=\"showforum-1.aspx\">默认分类</a><a href=\"showforum-2.aspx\">默认版块</a>";
                    var config = GeneralConfigInfo.Current;
                    var sb = new StringBuilder();
                    foreach (var item in FindAllParents(this, true))
                    {
                        sb.Separate("  &raquo; ");
                        if (config.Aspxrewrite == 1)
                            sb.AppendFormat("<a href=\"showforum-{0}.aspx\">{1}</a>", item.ID, item.Name);
                        else
                            sb.AppendFormat("<a href=\"showforum.aspx?forumid={0}.aspx\">{1}</a>", item.ID, item.Name);
                    }
                    _Pathlist = sb.ToString() + "";
                }
                return _Pathlist;
            }
        }

        /// <summary>父级的子论坛列数</summary>
        public Int32 ParentColCount { get { return Parent == null ? 1 : Parent.ColCount; } }
        #endregion

        #region 扩展查询﻿
        ///// <summary>根据父分类、层次、状态等查找</summary>
        ///// <param name="parentid">父分类</param>
        ///// <param name="layer">层次</param>
        ///// <param name="status">状态等</param>
        ///// <returns></returns>
        //[DataObjectMethod(DataObjectMethodType.Select, false)]
        //public static EntityList<XForum> FindAllByParentIDAndLayerAndStatus(Int32 parentid, Int32 layer, Int32 status)
        //{
        //    if (Meta.Count >= 1000)
        //        return FindAll(new String[] { _.ParentID, _.Layer, _.Status }, new Object[] { parentid, layer, status });
        //    else // 实体缓存
        //        return Meta.Cache.Entities.FindAll(e => e.ParentID == parentid && e.Layer == layer && e.Status == status);
        //}

        /// <summary>根据编号查找</summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static XForum FindByID(Int32 id)
        {
            if (id <= 0) return Root;

            if (Meta.Count >= 1000)
                return Find(_.ID, id);
            else // 实体缓存
                return Meta.Cache.Entities.Find(__.ID, id);
            // 单对象缓存
            //return Meta.SingleCache[id];
        }

        public static EntityList<XForum> FindAllByIDs(String ids)
        {
            var fs = ids.SplitAsInt().Distinct();
            return Meta.Cache.Entities.FindAll(e => fs.Contains(e.ID));
        }

        //public static XForum GetFirst()
        //{
        //    var list = FindAll(null, _.ID.Asc(), null, 0, 1);
        //    return list.Count > 0 ? list[0] : null;
        //}
        #endregion

        #region 高级查询
        public static List<IXForum> GetForumIndexList()
        {
            var list = Root.AllChilds.ToList()
                .Where(e => e.Visible && e.Layer <= 1 && !(e.Parent != null && !e.Parent.Visible && e.Parent.Layer == 0))
                .OrderBy(e => e.DisplayOrder)
                .Cast<IXForum>().ToList();

            var dic = Topic.GetHasNewByFids(list.Select(e => e.ID).ToArray());
            foreach (var item in list)
            {
                var id = 0;
                if (dic.TryGetValue(item.Fid, out id) && id > 0)
                    item.Havenew = "new";
                else
                    item.Havenew = "old";

                if (!item.Icon.IsNullOrWhiteSpace())
                {
                    if (!item.Icon.ToLower().StartsWith("http://")) item.Icon = BaseConfigs.GetForumPath + item.Icon;
                }
            }

            return list;
        }

        public static List<IXForum> GetSubForumList(int fid, int colcount)
        {
            var xf = FindByID(fid);
            if (xf == null) return null;

            var list = xf.Childs.ToList()
                .Where(e => e.Visible && e.Layer <= 1 && !(e.Parent != null && !e.Parent.Visible && e.Parent.Layer == 0))
                .OrderBy(e => e.DisplayOrder)
                .Cast<IXForum>().ToList();

            var dic = Topic.GetHasNewByFids(list.Select(e => e.ID).ToArray());
            foreach (var item in list)
            {
                var id = 0;
                if (dic.TryGetValue(item.Fid, out id) && id > 0)
                    item.Havenew = "new";
                else
                    item.Havenew = "old";

                //if (colcount > 0) item.ColCount = colcount;

                if (!item.Icon.IsNullOrWhiteSpace())
                {
                    if (!item.Icon.ToLower().StartsWith("http://")) item.Icon = BaseConfigs.GetForumPath + item.Icon;
                }
            }

            return list;
        }

        public static Int32 GetForumCount()
        {
            return FindAllWithCache().ToList().Count(e => e.Layer > 0 && e.Visible);
        }

        /// <summary>获取匿名可见子论坛</summary>
        /// <returns></returns>
        public List<IXForum> GetVisibles()
        {
            var list = AllChilds.ToList().Cast<IXForum>().ToList();
            // 权限
            list = list.Where(e => e.AllowView(7)).ToList();
            // 密码
            list = list.Where(e => String.IsNullOrEmpty(e.Password)).ToList();
            // 状态
            list = list.Where(e => e.Visible).ToList();
            return list;
        }

        /// <summary>获取匿名可见子论坛的编号，包括自己</summary>
        /// <returns></returns>
        public Int32[] GetVisibleFids()
        {
            var list = GetVisibles().Select(e => e.Fid).ToList();
            if (ID > 0) list.Insert(0, ID);
            return list.ToArray();
        }

        public static List<IXForum> GetVisibleForumList()
        {
            //string commandText = string.Format("SELECT [name], [fid], [layer],[parentid] FROM [{0}forums] WHERE [parentid] NOT IN (SELECT fid FROM [{0}forums] WHERE [status] < 1 AND [layer] = 0) AND [status] > 0 AND [displayorder] >=0 ORDER BY [displayorder]", TablePrefix);

            var list = FindAllWithCache().ToList().Cast<IXForum>();
            list = list.Where(e => e.Visible && e.DisplayOrder >= 0);
            list = list.Where(e => e.Parent == null || e.Parent.ID == 0 || !(!e.Parent.Visible && e.Parent.Layer == 0));
            return list.OrderBy(e => e.DisplayOrder).ToList();
        }

        //public static List<IXForum> GetOpenForumList()
        //{
        //    var list = FindAllWithCache().ToList().Cast<IXForum>();
        //    list = list.Where(e => e.Visible
        //        && e.Permuserlist.IsNullOrWhiteSpace()
        //        && (e.AllowView(7))
        //        );
        //    return list.OrderBy(e => e.DisplayOrder).ToList();
        //}

        //public static List<IXForum> GetWebSiteAggHotForumList(int topNumber, string orderby, int fid)
        //{
        //    //var exp = _.Status == 1 & _.Layer > 0;
        //    var list = FindAllWithCache().ToList().Cast<IXForum>();
        //    list = list.Where(e => e.Visible
        //        && e.Layer > 0
        //        && e.Password.IsNullOrWhiteSpace()
        //        );
        //    if (fid > 0) list = list.Where(e => e.ID == fid);
        //    var fi = Meta.Table.FindByName(orderby) as FieldItem;
        //    if (fi != null) list = list.OrderBy(e => e.DisplayOrder);

        //    return list.Take(topNumber).ToList();
        //}
        #endregion

        #region 扩展操作
        public static int SetRealCurrentTopics(int fid)
        {
            //DbParameter[] commandParameters = new DbParameter[]
            //{
            //    DbHelper.MakeInParam("@fid", DbType.Double, 4, fid)
            //};
            //return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, string.Format("{0}setcurrenttopics", TablePrefix), commandParameters);
            //UPDATE [dnt_forums] SET [curtopics] = (SELECT COUNT(tid) FROM [dnt_topics] WHERE [displayorder] >= 0 AND [fid]=@fid) WHERE [fid]=@fid

            var f = FindByID(fid);
            //f.Topics = Topic.FindCountByForumID(fid);
            return f.SetRealCurrentTopics();
        }

        public Int32 SetRealCurrentTopics()
        {
            Topics = Topic.FindCountByForumID(ID);
            return Update();
        }

        public Int32 Update(int topiccount, int postcount, int lasttid, string lasttitle, DateTime lastpost, int lastposterid, string lastposter, int todaypostcount)
        {
            //DatabaseProvider.GetInstance().UpdateForum(fid, topiccount, postcount, lasttid, lasttitle, lastpost, lastposterid, lastposter, todaypostcount);

            var f = this;
            f.Topics = topiccount;
            f.Posts = postcount;
            f.LastTID = lasttid;
            f.LastTitle = lasttitle;
            f.LastPost = lastpost;
            f.LastPosterID = lastposterid;
            f.LastPoster = lastposter;
            f.TodayPosts = todaypostcount;
            return f.Update();
        }

        public static Boolean CheckRewriteNameInvalid(string rewriteName)
        {
            string[] array = "admin,aspx,tools,archive".Split(',');
            for (int i = 0; i < array.Length; i++)
            {
                string value = array[i];
                if (rewriteName.IndexOf(value) != -1)
                {
                    return true;
                }
            }
            //return !Regex.IsMatch(rewriteName, "([\\w|\\-|_])+") || BBX.Data.Forums.CheckRewriteNameInvalid(rewriteName);
            if (!Regex.IsMatch(rewriteName, "([\\w|\\-|_])+")) return true;

            return ForumField.CheckRewriteNameInvalid(rewriteName);
        }

        public static void UpdateForumsFieldModerators(string username)
        {
            //Forums.UpdateModeratorName(username, "");
            foreach (var item in XForum.Root.AllChilds)
            {
                var fi = item.Field;
                if (fi.ModeratorCollection.Contains(username))
                {
                    fi.ModeratorCollection.Remove(username);
                    fi.Moderators = string.Join(",", fi.ModeratorCollection.ToArray());
                    fi.Save();
                }
            }
        }
        #endregion

        #region 业务
        public string GetModerators(int moderStyle)
        {
            var sb = new StringBuilder();
            var sb2 = new StringBuilder();

            var info = this as IXForum;
            int num = info.Layer <= 0 ? 3 : 6;
            int num2 = 0;
            //string[] array = Utils.SplitString(info.Moderators, ",");
            var mods = info.Moderators.Split(",");
            for (int i = 0; i < mods.Length; i++)
            {
                var mod = mods[i];
                if (!mod.Trim().IsNullOrWhiteSpace())
                {
                    if (moderStyle == 0)
                    {
                        string text2 = string.Format("<a href=\"{0}userinfo.aspx?username={1}\" target=\"_blank\">{2}</a>,", BaseConfigs.GetForumPath, Utils.UrlEncode(mod.Trim()), mod.Trim());
                        if (num2++ < num)
                            sb.Append(text2);
                        else
                            sb2.AppendFormat("<li>{0}</li>", text2.TrimEnd(','));
                    }
                    else
                    {
                        sb.AppendFormat("<option value=\"{0}\">{1}</option>", mod.Trim(), mod.Trim());
                    }
                }
            }
            if (sb.Length > 0 && moderStyle == 1)
            {
                sb.Insert(0, string.Format("<select style=\"width: 100px;\" onchange=\"window.open('{0}userinfo.aspx?username=' + escape(this.value));\">", BaseConfigs.GetForumPath));
                sb.Append("</select>");
            }
            if (sb2.Length > 0)
            {
                sb2.Insert(0, string.Format("<a id=\"forum{0}_submoderators\" href=\"###\" onclick=\"showMenu({{'ctrlid':this.id, 'pos':'21'}})\">......</a><ul id=\"forum{0}_submoderators_menu\" class=\"p_pop moders\" style=\"position: absolute; z-index: 301; left: 998.5px; top: 93px; display: none;\">", info.Fid));
                sb2.Append("</ul>");
                sb.Append(sb2);
            }
            return sb.ToString().TrimEnd(',');
        }

        //public IDataReader GetAllForumStatistics()
        //{
        //    //string commandText = string.Format("SELECT SUM([topics]) AS [topiccount],SUM([posts]) AS [postcount],
        //    //SUM([todayposts])-(
        //    //  SELECT SUM([todayposts]) FROM [{0}forums] 
        //    //  WHERE [lastpost] < CONVERT(CHAR(12),GETDATE(),101) AND [layer]=1
        //    //) AS [todaypostcount] 
        //    //FROM [{0}forums] WHERE [layer]=1", TablePrefix);
        //    //return DbHelper.ExecuteReader(CommandType.Text, commandText);
        //}

        //public IXForum GetForumStatistics(int fid = 0)
        //{
        //    //string commandText = string.Format("SELECT SUM([topics]) AS [topiccount],SUM([posts]) AS [postcount],
        //    //SUM([todayposts])-(
        //    //  SELECT SUM([todayposts]) FROM [{0}forums] 
        //    //  WHERE [lastpost] < CONVERT(CHAR(12),GETDATE(),101) AND [layer]=1 AND [fid] = @fid
        //    //) AS [todaypostcount] 
        //    //FROM [{0}forums] WHERE [fid] = {1} AND [layer]=1", TablePrefix, fid);
        //    //return DbHelper.ExecuteReader(CommandType.Text, commandText);

        //    var exp = _.Layer == 1;
        //    if (fid > 0) exp &= _.ID == fid;
        //    var list = FindAll(exp, null, _.Topics.Sum() & _.Posts.Sum() & _.TodayPosts.Sum(), 0, 0);
        //    var e = list[0];
        //    exp &= _.LastPost < DateTime.Now.Date;
        //    list = FindAll(exp, null, _.Topics.Sum() & _.Posts.Sum() & _.TodayPosts.Sum(), 0, 0);
        //    e.TodayPosts -= list[0].TodayPosts;
        //    return e;
        //}

        ///// <summary>获取排序的论坛列表</summary>
        ///// <returns></returns>
        //public static EntityList<XForum> GetShortForums()
        //{
        //    var list = FindAllWithCache();
        //    list = new EntityList<XForum>(list);
        //    list.Sort(__.ID, true);
        //    return list;
        //}

        /// <summary>批量设置论坛</summary>
        /// <param name="forumInfo"></param>
        /// <param name="bsp"></param>
        /// <param name="fidList"></param>
        /// <returns></returns>
        public static Boolean BatchSet(IXForum forumInfo, BatchSetParams bsp, string fidList)
        {
            throw new NotImplementedException("未实现批量设置论坛");

            //var sb = new StringBuilder();
            //var sb2 = new StringBuilder();
            //sb.AppendFormat("UPDATE [{0}forums] SET ", TablePrefix);
            //if (bsp.SetSetting)
            //{
            //    sb.AppendFormat("[Allowsmilies]='{0}' ,", forumInfo.AllowSmilies);
            //    sb.AppendFormat("[Allowrss]='{0}' ,", forumInfo.AllowRss);
            //    sb.AppendFormat("[Allowhtml]='{0}' ,", forumInfo.AllowHtml);
            //    sb.AppendFormat("[Allowbbcode]='{0}' ,", forumInfo.AllowBbCode);
            //    sb.AppendFormat("[Allowimgcode]='{0}' ,", forumInfo.AllowImgCode);
            //    sb.AppendFormat("[Allowblog]='{0}' ,", forumInfo.AllowBlog);
            //    sb.AppendFormat("[istrade]='{0}' ,", forumInfo.IsTrade);
            //    sb.AppendFormat("[allowpostspecial]='{0}' ,", forumInfo.AllowPostSpecial);
            //    sb.AppendFormat("[allowspecialonly]='{0}' ,", forumInfo.AllowSpecialOnly);
            //    sb.AppendFormat("[Alloweditrules]='{0}' ,", forumInfo.AllowEditRules);
            //    sb.AppendFormat("[allowthumbnail]='{0}' ,", forumInfo.AllowThumbnail);
            //    sb.AppendFormat("[Recyclebin]='{0}' ,", forumInfo.Recyclebin);
            //    sb.AppendFormat("[Modnewposts]='{0}' ,", forumInfo.Modnewposts);
            //    sb.AppendFormat("[Modnewtopics]='{0}' ,", forumInfo.Modnewtopics);
            //    sb.AppendFormat("[Jammer]='{0}' ,", forumInfo.Jammer);
            //    sb.AppendFormat("[Disablewatermark]='{0}' ,", forumInfo.DisableWatermark);
            //    sb.AppendFormat("[Inheritedmod]='{0}' ,", forumInfo.Inheritedmod);
            //    sb.AppendFormat("[allowtag]='{0}' ,", forumInfo.AllowTag);
            //}
            //if (sb.ToString().EndsWith(","))
            //{
            //    sb.Remove(sb.Length - 1, 1);
            //}
            //sb.AppendFormat("WHERE [fid] IN ({0})", fidList);

            var list = FindAll(_.ID.In(fidList.SplitAsInt()), null, null, 0, 0);


            //sb2.AppendFormat("UPDATE [{0}forumfields] SET ", TablePrefix);
            //if (bsp.SetPassWord)
            //{
            //    sb2.AppendFormat("[password]='{0}' ,", forumInfo.Password);
            //}
            //if (bsp.SetAttachExtensions)
            //{
            //    sb2.AppendFormat("[attachextensions]='{0}' ,", forumInfo.Attachextensions);
            //}
            //if (bsp.SetPostCredits)
            //{
            //    sb2.AppendFormat("[postcredits]='{0}' ,", forumInfo.PostcrEdits);
            //}
            //if (bsp.SetReplyCredits)
            //{
            //    sb2.AppendFormat("[replycredits]='{0}' ,", forumInfo.ReplycrEdits);
            //}
            //if (bsp.SetViewperm)
            //{
            //    sb2.AppendFormat("[Viewperm]='{0}' ,", forumInfo.ViewPerm);
            //}
            //if (bsp.SetPostperm)
            //{
            //    sb2.AppendFormat("[PostPerm]='{0}' ,", forumInfo.PostPerm);
            //}
            //if (bsp.SetReplyperm)
            //{
            //    sb2.AppendFormat("[Replyperm]='{0}' ,", forumInfo.ReplyPerm);
            //}
            //if (bsp.SetGetattachperm)
            //{
            //    sb2.AppendFormat("[Getattachperm]='{0}' ,", forumInfo.GetattachPerm);
            //}
            //if (bsp.SetPostattachperm)
            //{
            //    sb2.AppendFormat("[Postattachperm]='{0}' ,", forumInfo.PostattachPerm);
            //}
            //if (sb2.ToString().EndsWith(","))
            //{
            //    sb2.Remove(sb2.Length - 1, 1);
            //}
            //sb2.AppendFormat("WHERE [fid] IN ({0})", fidList);

            //if (sb.ToString().IndexOf("SET WHERE") < 0)
            //{
            //    DbHelper.ExecuteNonQuery(CommandType.Text, sb.ToString());
            //}
            //if (sb2.ToString().IndexOf("SET WHERE") < 0)
            //{
            //    DbHelper.ExecuteNonQuery(CommandType.Text, sb2.ToString());
            //}

            return true;
        }

        public static Int32 ReSetClearMove()
        {
            var list = Topic.FindAll(Topic._.Closed > 1, null, null, 0, 0);
            return list.Delete();
        }

        /// <summary>重置最后发帖信息</summary>
        public void ResetLastPost()
        {
            var post = Post.FindLastByFids(AllChildKeys.ToArray());
            // 没有帖子就不更新了，对于没有帖子的论坛版面，数据错一点也没有关系
            if (post == null) return;

            var f = this;
            f.Topics = Topic.GetTopicCount(ID);
            f.Posts = Post.GetPostCount(ID);
            f.TodayPosts = Post.GetTodayPostCount(ID);

            f.LastTID = post.Tid;
            f.LastTitle = post.TopicTitle;
            f.LastPost = post.PostDateTime;
            f.LastPosterID = post.PosterID;
            f.LastPoster = post.Poster;
            f.Update();
        }

        /// <summary>合并论坛</summary>
        /// <param name="sourcefid"></param>
        /// <param name="targetfid"></param>
        /// <returns></returns>
        public static Boolean Combination(Int32 sourcefid, Int32 targetfid)
        {
            //if (BBX.Data.Forums.IsExistSubForum(int.Parse(sourcefid)))
            //{
            //	return false;
            //}

            var f = XForum.FindByID(sourcefid);
            if (f == null) return false;

            // 修改主题表
            // 修改帖子表
            // 更新发帖统计
            // 父论坛的子论坛数减一
            // 删除论坛

            using (var trans = Meta.CreateTrans())
            {
                Topic.ChangeFid(sourcefid, targetfid);
                Post.ChangeFid(sourcefid, targetfid);

                f.ResetLastPost();

                //foreach (var item in f.AllParents)
                //{
                //    item.SubforumCount--;
                //    item.Update();
                //}

                f.Delete();
            }

            return true;
        }

        /// <summary>是否运行指定用户组访问，默认7表示游客</summary>
        /// <param name="gid"></param>
        /// <returns></returns>
        public Boolean AllowView(Int32 gid = 7) { return Field.AllowView(gid); }
        #endregion

        #region 辅助函数
        public override string ToString()
        {
            return Name;
        }
        #endregion

        #region 扩展字段接口实现
        Int32 IXForum.Fid { get { return ID; } set { ID = value; } }

        String IXForum.Password { get { return Field.Password; } set { Field.Password = value; } }

        String IXForum.Icon { get { return Field.Icon; } set { Field.Icon = value; } }

        String IXForum.PostcrEdits { get { return Field.PostcrEdits; } set { Field.PostcrEdits = value; } }

        String IXForum.ReplycrEdits { get { return Field.ReplycrEdits; } set { Field.ReplycrEdits = value; } }

        String IXForum.Redirect { get { return Field.Redirect; } set { Field.Redirect = value; } }

        String IXForum.Attachextensions { get { return Field.Attachextensions; } set { Field.Attachextensions = value; } }

        String IXForum.Rules { get { return Field.Rules; } set { Field.Rules = value; } }

        String IXForum.TopicTypes { get { return Field.TopicTypes; } set { Field.TopicTypes = value; } }

        String IXForum.ViewPerm { get { return Field.ViewPerm; } set { Field.ViewPerm = value; } }

        String IXForum.PostPerm { get { return Field.PostPerm; } set { Field.PostPerm = value; } }

        String IXForum.ReplyPerm { get { return Field.ReplyPerm; } set { Field.ReplyPerm = value; } }

        String IXForum.GetattachPerm { get { return Field.GetattachPerm; } set { Field.GetattachPerm = value; } }

        String IXForum.PostattachPerm { get { return Field.PostattachPerm; } set { Field.PostattachPerm = value; } }

        String IXForum.Moderators { get { return Field.Moderators; } set { Field.Moderators = value; } }

        String IXForum.Description { get { return Field.Description; } set { Field.Description = value; } }

        SByte IXForum.ApplytopicType { get { return Field.ApplytopicType; } set { Field.ApplytopicType = value; } }

        SByte IXForum.PostbytopicType { get { return Field.PostbytopicType; } set { Field.PostbytopicType = value; } }

        SByte IXForum.ViewbytopicType { get { return Field.ViewbytopicType; } set { Field.ViewbytopicType = value; } }

        SByte IXForum.Topictypeprefix { get { return Field.Topictypeprefix; } set { Field.Topictypeprefix = value; } }

        String IXForum.Permuserlist { get { return Field.Permuserlist; } set { Field.Permuserlist = value; } }

        String IXForum.Seokeywords { get { return Field.Seokeywords; } set { Field.Seokeywords = value; } }

        String IXForum.Seodescription { get { return Field.Seodescription; } set { Field.Seodescription = value; } }

        String IXForum.RewriteName { get { return Field.RewriteName; } set { Field.RewriteName = value; } }
        #endregion

        #region 兼容
        String IXForum.Rewritename { get { return Field.RewriteName; } }

        String IXForum.Viewperm { get { return Field.ViewPerm; } }
        String IXForum.Replyperm { get { return Field.ReplyPerm; } }
        String IXForum.Getattachperm { get { return Field.GetattachPerm; } }
        String IXForum.Topictypes { get { return Field.TopicTypes; } }

        //Int32 IXForum.Allowsmilies { get { return AllowSmilies ? 1 : 0; } }
        //Int32 IXForum.Allowtag { get { return AllowTag ? 1 : 0; } }
        Int32 IXForum.Allowbbcode { get { return AllowBbCode ? 1 : 0; } }
        Int32 IXForum.Allowimgcode { get { return AllowImgCode ? 1 : 0; } }

        Int32 IXForum.Allowpostspecial { get { return AllowPostSpecial; } }

        IXForum IXForum.Clone() { return this; }

        private string _havenew;
        public string Havenew { get { return _havenew; } set { _havenew = value; } }

        private string _collapse = string.Empty;
        public string Collapse { get { return _collapse; } set { _collapse = value; } }
        #endregion

        #region 统计
        /// <summary>获取可发帖论坛数，实际上就是非顶层论坛数</summary>
        /// <returns></returns>
        public static Int32 GetForumCountNoTop()
        {
            return Root.AllChilds.ToList().Count(e => e.Deepth > 1);
        }

        public static EntityList<XForum> GetForumsByPostCount(int count)
        {
            // SELECT TOP {0} [fid], [name], [posts] FROM [{1}forums] WHERE [status]>0 AND [layer]>0 ORDER BY [posts] DESC", count, TablePrefix

            var list = Meta.Cache.Entities.ToList().AsEnumerable();
            list = list.Where(e => e.Visible && e.Layer > 0);
            list = list.OrderByDescending(e => e.Posts).Take(count);
            return new EntityList<XForum>(list);
        }

        /// <summary>今日发帖数排行</summary>
        /// <param name="count"></param>
        /// <param name="postTableId"></param>
        /// <returns></returns>
        public static EntityList<XForum> GetForumsByDayPostCount(int count, Int32 postTableId = 0)
        {
            /*if (!Utils.IsNumeric(postTableId))
            {
                postTableId = "1";
            }
            string commandText = string.Format("SELECT DISTINCT TOP {0} [p].[fid], [f].[name], COUNT([pid]) AS [posts] FROM [{1}posts{2}] [p] LEFT JOIN [{1}forums] [f] ON [p].[fid]=[f].[fid] WHERE [postdatetime]>='{3}' AND [invisible]=0 AND [posterid]>0 GROUP BY [p].[fid], [f].[name] ORDER BY [posts] DESC", new object[]
            {
                count,
                TablePrefix,
                postTableId,
                DateTime.Now.ToString("yyyy-MM-dd")
            });
            return DbHelper.ExecuteReader(CommandType.Text, commandText);*/

            var list = Meta.Cache.Entities.ToList().AsEnumerable();
            list = list.Where(e => e.Visible && e.Layer > 0);

            // 今日发帖数
            var ps = Post.GetPostCountByForums(DateTime.Now.Date, postTableId);
            foreach (var item in list)
            {
                if (ps.ContainsKey(item.ID))
                    item.Posts = ps[item.ID];
                else
                    item.Posts = 0;
            }

            list = list.OrderByDescending(e => e.Posts).Take(count);
            return new EntityList<XForum>(list);
        }

        public static EntityList<XForum> GetForumsByMonthPostCount(int count, Int32 postTableId = 0)
        {
            /*if (!Utils.IsNumeric(postTableId))
            {
                postTableId = "1";
            }
            string commandText = string.Format("SELECT DISTINCT TOP {0} [p].[fid], [f].[name], COUNT([pid]) AS [posts] FROM [{1}posts{2}] [p] LEFT JOIN [{1}forums] [f] ON [p].[fid]=[f].[fid] WHERE [postdatetime]>='{3}' AND [invisible]=0 AND [posterid]>0 GROUP BY [p].[fid], [f].[name] ORDER BY [posts] DESC", new object[]
            {
                count,
                TablePrefix,
                postTableId,
                DateTime.Now.AddDays(-30.0).ToString("yyyy-MM-dd")
            });
            return DbHelper.ExecuteReader(CommandType.Text, commandText);*/

            var list = Meta.Cache.Entities.ToList().AsEnumerable();
            list = list.Where(e => e.Visible && e.Layer > 0);

            // 本月发帖数
            var ps = Post.GetPostCountByForums(DateTime.Now.Date.AddDays(-30), postTableId);
            foreach (var item in list)
            {
                if (ps.ContainsKey(item.ID))
                    item.Posts = ps[item.ID];
                else
                    item.Posts = 0;
            }

            list = list.OrderByDescending(e => e.Posts).Take(count);
            return new EntityList<XForum>(list);
        }

        public static EntityList<XForum> GetForumsByTopicCount(int count)
        {
            /*string commandText = string.Format("SELECT TOP {0} [fid], [name], [topics] FROM [{1}forums] WHERE [status]>0 AND [layer]>0 ORDER BY [topics] DESC", count, TablePrefix);
            return DbHelper.ExecuteReader(CommandType.Text, commandText);*/

            var list = Meta.Cache.Entities.ToList().AsEnumerable();
            list = list.Where(e => e.Visible && e.Layer > 0);
            list = list.OrderByDescending(e => e.Topics).Take(count);
            return new EntityList<XForum>(list);
        }
        #endregion
    }

    public partial interface IXForum : IEntity
    {
        ForumField Field { get; }
        IXForum Parent { get; }

        #region 属性
        /// <summary>F编号</summary>
        Int32 Fid { get; set; }

        /// <summary>登录密码</summary>
        String Password { get; set; }

        /// <summary>IOC图标</summary>
        String Icon { get; set; }

        /// <summary></summary>
        String PostcrEdits { get; set; }

        /// <summary></summary>
        String ReplycrEdits { get; set; }

        /// <summary>重定向</summary>
        String Redirect { get; set; }

        /// <summary></summary>
        String Attachextensions { get; set; }

        /// <summary>编码规则</summary>
        String Rules { get; set; }

        /// <summary></summary>
        String TopicTypes { get; set; }

        /// <summary></summary>
        String ViewPerm { get; set; }

        /// <summary></summary>
        String PostPerm { get; set; }

        /// <summary></summary>
        String ReplyPerm { get; set; }

        /// <summary></summary>
        String GetattachPerm { get; set; }

        /// <summary></summary>
        String PostattachPerm { get; set; }

        /// <summary>版主</summary>
        String Moderators { get; set; }

        /// <summary>描述</summary>
        String Description { get; set; }

        /// <summary></summary>
        SByte ApplytopicType { get; set; }

        /// <summary></summary>
        SByte PostbytopicType { get; set; }

        /// <summary></summary>
        SByte ViewbytopicType { get; set; }

        /// <summary></summary>
        SByte Topictypeprefix { get; set; }

        /// <summary></summary>
        String Permuserlist { get; set; }

        /// <summary></summary>
        String Seokeywords { get; set; }

        /// <summary></summary>
        String Seodescription { get; set; }

        /// <summary>URL重写名称</summary>
        String RewriteName { get; set; }
        #endregion

        #region 兼容，以后要干掉这些代码
        String Rewritename { get; }

        String Viewperm { get; }
        String Replyperm { get; }
        String Getattachperm { get; }
        String Topictypes { get; }

        //Int32 Allowsmilies { get; }
        //Int32 Allowtag { get; }
        Int32 Allowbbcode { get; }
        Int32 Allowimgcode { get; }

        Int32 Allowpostspecial { get; }

        IXForum Clone();

        String Havenew { get; set; }
        String Collapse { get; set; }
        #endregion

        string GetModerators(int moderStyle);
        String ModeratorsHtml { get; set; }

        Boolean Visible { get; set; }

        /// <summary>层次深度</summary>
        Int32 Layer { get; }

        /// <summary>论坛路径，用于导航链接</summary>
        String Pathlist { get; }

        /// <summary>父级的子论坛列数</summary>
        Int32 ParentColCount { get; }

        /// <summary>是否运行指定用户组访问，默认7表示游客</summary>
        /// <param name="gid"></param>
        /// <returns></returns>
        Boolean AllowView(Int32 gid = 7);
    }
}