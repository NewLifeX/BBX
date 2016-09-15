﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using BBX.Common;
using BBX.Config;
using NewLife.Log;
using XCode;

namespace BBX.Entity
{
    /// <summary>导航</summary>
    public partial class Nav : EntityTree<Nav>
    {
        #region 对象操作﻿
        static Nav()
        {
            // 注册新的实体树操作
            Setting = new NavSetting();
        }

        class NavSetting : EntityTreeSetting<Nav>
        {
            /// <summary>已重载。</summary>
            public override string Key { get { return __.ID; } }

            /// <summary>关联父键名，一般是Parent加主键，如ParentID</summary>
            public override string Parent { get { return __.ParentID; } }

            public override string Sort { get { return __.DisplayOrder; } }

            public override string Name { get { return __.Title; } }

            public override Boolean BigSort { get { return false; } }
        }

        //protected override string SortingKeyName { get { return _.DisplayOrder; } }

        //protected override bool BigSort { get { return false; } }

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
            if (XTrace.Debug) XTrace.WriteLine("开始初始化{0}导航数据……", typeof(Nav).Name);

            var i = 0;
            Add("论坛", "index.aspx", i++);
            Add("标签", "tags.aspx", i++);
            Add("会员", "showuser.aspx", i++);
            Add("搜索", "search.aspx", i++);
            Add("帮助", "help.aspx", i++);

            if (XTrace.Debug) XTrace.WriteLine("完成初始化{0}导航数据！", typeof(Nav).Name);
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
        //private String _nav;
        /// <summary>属性说明</summary>
        public String Content
        {
            get
            {
                if (Childs.Count > 0)
                {
                    var sb = new StringBuilder();
                    sb.AppendFormat("<li><a id=\"menu_{0}\" onMouseOver=\"showMenu({{'ctrlid':this.id}});\" href=\"", ID);
                    if (!Url.StartsWith("http://") && !Url.StartsWith("/")) sb.Append(BaseConfigs.GetForumPath);
                    sb.Append(Url.Trim() + "\"");
                    if (Target != 0) sb.Append(" target=\"_blank\"");

                    sb.AppendFormat(" title=\"{0}\">", !Title.IsNullOrEmpty() ? Title.Trim() : Name.Trim());
                    //if (Utils.InArray(ID.ToString(), GetMainNavigationHasSub()))
                    //    return sb.Append("<span class=\"drop\">" + Name.Trim() + "</span></a></li>").ToString();
                    //else
                    //    return sb.Append(Name.Trim() + "</a></li>").ToString();
                    return sb.Append("<span class=\"drop\">" + Name.Trim() + "</span></a></li>").ToString();
                }
                else
                {
                    var sb = new StringBuilder();
                    sb.Append("<li><a href=\"");
                    if (!Url.StartsWith("http://") && !Url.StartsWith("/")) sb.Append(BaseConfigs.GetForumPath);
                    sb.Append(Url.Trim());
                    if (Target != 0) sb.Append("\" target=\"_blank");
                    sb.AppendFormat("\">{0}</a></li>", Name.Trim());
                    return sb.ToString();
                }
            }
            set { }
        }
        #endregion

        #region 扩展查询﻿
        /// <summary>根据编号查找</summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static Nav FindByID(Int32 id)
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
        //public static EntityList<Nav> Search(String key, String orderClause, Int32 startRowIndex, Int32 maximumRows)
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
        static void Add(String name, String url, Int32 order = 0, Int32 target = 0)
        {
            var nav = new Nav();
            nav.Name = name;
            nav.Title = name;
            nav.Url = url;
            nav.DisplayOrder = order;
            nav.Target = target;
            nav.Insert();
        }
        #endregion

        #region 业务  @宁波-小董 2012-11-14修改
        //很坑爹，先保证能运行吧。大部分方法都是直接从原来代码copy过来的。稍微修改，没有过多考虑逻辑进行优化
        //考虑用缓存，缓存中FindAll，杂排序？
        public static EntityList<Nav> GetNavigation(bool getAll)
        {
            //原接口中有多个字段的排序
            // string commandText = string.Format("SELECT {0} FROM [{1}navs] {2} ORDER BY [parentid],[displayorder],[id]", "[id],[parentid],[name],[title],[url],[target],[type],[available],[displayorder],[highlight],[level]", BaseConfigs.GetTablePrefix, getAllNavigation ? "" : " WHERE [available]=1 ");
            //return getAll ?
            //    FindAll(null, _.ParentID + "," + _.DisplayOrder + "," + _.ID, null, 0, 0) :
            //    FindAll(__.Available.Equal(true), _.ParentID + "," + _.DisplayOrder + "," + _.ID, null, 0, 0);

            var list = Meta.Cache.Entities;
            if (!getAll) list = list.FindAll(__.Available, 1);
            return list.Sort(new String[] { _.ParentID, _.DisplayOrder, _.ID }, new Boolean[] { false, false, false });
        }

        public static String[] GetMainNavigationHasSub()
        {
            //string text = "";
            //foreach (var current in Root.Childs)
            //{
            //    text = text + current.ParentID + ",";
            //    text.Remove(text.Length - 1, 1);
            //}
            //return text.Split(',');

            return Root.Childs.ToList().Select(e => e.ID + "").ToArray();
        }

        //public static DataTable GetMainNavigation()
        //{
        //    var dt = Root.Childs.ToDataTable();
        //    var dc = new DataColumn();
        //    dc.ColumnName = "nav";
        //    dc.DataType = typeof(String);
        //    dt.Columns.Add(dc);

        //    return dt;

        //    //var list = Nav.Root.Childs;
        //    //var table = new DataTable();
        //    //table.Columns.Add("id", typeof(Int32));
        //    //table.Columns.Add("nav", typeof(String));
        //    //table.Columns.Add("level", typeof(Int32));
        //    //table.Columns.Add("url", typeof(String));
        //    //table.Columns.Add("name", typeof(String));
        //    //foreach (var current in list)
        //    //{
        //    //    var dataRow = table.NewRow();
        //    //    var sb = new StringBuilder();
        //    //    sb.AppendFormat("<li><a id=\"menu_{0}\" onMouseOver=\"showMenu({{'ctrlid':this.id}});\" href=\"", current.ID);
        //    //    if (!current.Url.StartsWith("http://") && !current.Url.StartsWith("/"))
        //    //    {
        //    //        sb.Append(BaseConfigs.GetForumPath);
        //    //    }
        //    //    sb.Append(current.Url.Trim() + "\"");
        //    //    if (current.Target != 0)
        //    //    {
        //    //        sb.Append(" target=\"_blank\"");
        //    //    }
        //    //    sb.AppendFormat(" title=\"{0}\">", (current.Title.IsNullOrEmpty()) ? current.Title.Trim() : current.Name.Trim());
        //    //    if (Utils.InArray(current.ID.ToString(), GetMainNavigationHasSub()))
        //    //    {
        //    //        dataRow["nav"] = sb.Append("<span class=\"drop\">" + current.Name.Trim() + "</span></a></li>").ToString();
        //    //    }
        //    //    else
        //    //    {
        //    //        dataRow["nav"] = sb.Append(current.Name.Trim() + "</a></li>").ToString();
        //    //    }
        //    //    dataRow["id"] = current.ID;
        //    //    dataRow["url"] = current.Url.Trim();
        //    //    dataRow["level"] = current.Level;
        //    //    dataRow["name"] = current.Name;
        //    //    table.Rows.Add(dataRow);
        //    //}
        //    //return table;
        //}

        public static EntityList<Nav> GetSubNavigation()
        {
            var list = new EntityList<Nav>();
            foreach (var item in Root.Childs)
            {
                if (item.Available != 1) continue;

                //list.Add(item);
                list.AddRange(item.Childs.FindAll(__.Available, 1));
            }

            return list;

            //var table = new DataTable();
            //table.Columns.Add("id", typeof(Int32));
            //table.Columns.Add("nav", typeof(String));
            //table.Columns.Add("level", typeof(Int32));
            //table.Columns.Add("parentid", typeof(Int32));
            //var navigation = GetNavigation();
            //foreach (var current in Root.Childs)
            //{
            //    foreach (var current2 in navigation)
            //    {
            //        if (current2.ParentID == current.ID)
            //        {
            //            var sb = new StringBuilder();
            //            var dataRow = table.NewRow();
            //            sb.Append("<li><a href=\"");
            //            if (!current2.Url.StartsWith("http://") && !current2.Url.StartsWith("/"))
            //            {
            //                sb.Append(BaseConfigs.GetForumPath);
            //            }
            //            sb.Append(current2.Url.Trim());
            //            if (current2.Target != 0)
            //            {
            //                sb.Append("\" target=\"_blank");
            //            }
            //            sb.AppendFormat("\">{0}</a></li>", current2.Name.Trim());
            //            dataRow["nav"] = sb.ToString();
            //            dataRow["id"] = current2.ID;
            //            dataRow["parentid"] = current2.ParentID;
            //            dataRow["level"] = current2.Level;
            //            table.Rows.Add(dataRow);
            //        }
            //    }
            //}
            //return table;
        }

        public static String GetNavigationString(int userid, int useradminid)
        {
            var list = new List<string>();
            foreach (var item in Root.Childs)
            {
                var value = item.ChangeStyleForCurrentUrl();
                switch (item.Level)
                {
                    case 0:
                        list.Add(value);
                        break;
                    case 1:
                        if (userid != -1) list.Add(value);
                        break;
                    case 2:
                        if (useradminid == 3 || useradminid == 1 || useradminid == 2) list.Add(value);
                        break;
                    case 3:
                        if (useradminid == 1) list.Add(value);
                        break;
                }
            }
            return String.Join(String.Empty, list.ToArray());
        }

        private string ChangeStyleForCurrentUrl()
        {
            var text = Content;
            var url = Url.Trim();
            if (Utils.InArray(url, "/,index.aspx") && DNTRequest.GetPageName() == (GeneralConfigInfo.Current.Indexpage == 0 ? "forumindex.aspx" : "website.aspx"))
            {
                return ReplaceCurrentCssClass(text);
            }
            if (!Utils.StrIsNullOrEmpty(url) && HttpContext.Current.Request.RawUrl.ToString().Contains(url))
            {
                return ReplaceCurrentCssClass(text);
            }
            return text;
        }

        private static string ReplaceCurrentCssClass(string nav) { return nav.Replace("<a", "<a class=\"current\""); }
        #endregion
    }
}