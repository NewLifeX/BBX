﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Xml;
using BBX.Cache;
using BBX.Common;
using BBX.Config;
using NewLife.Log;
using XCode;

namespace BBX.Entity
{
    /// <summary>模板</summary>
    public partial class Template : EntityBase<Template>
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

        }

        /// <summary>首次连接数据库时初始化数据，仅用于实体类重载，用户不应该调用该方法</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void InitData()
        {
            base.InitData();

            if (Meta.Count > 0) return;

            // 需要注意的是，如果该方法调用了其它实体类的首次数据库操作，目标实体类的数据初始化将会在同一个线程完成
            if (XTrace.Debug) XTrace.WriteLine("开始初始化{0}模板数据……", typeof(Template).Name);

            var entity = new Template();
            entity.Name = "Default";
            entity.Directory = "Default";
            entity.Author = "BBX";
            entity.Createdate = "2002-1-1";
            entity.Ver = "1.0";
            entity.Fordntver = "1.0";
            entity.Import(null, entity.Directory);
            entity.Copyright = "Copyright 2002~2015 NewLife Tearm.";
            //entity.Url = "http://www.NewLifeX.com";
            entity.Enable = true;
            entity.Insert();

            var path = BaseConfigs.GetForumPath.CombinePath("templates").GetFullPath();
            Import(path);

            if (XTrace.Debug) XTrace.WriteLine("完成初始化{0}模板数据！", typeof(Template).Name);
        }

        /// <summary>已重载。基类先调用Valid(true)验证数据，然后在事务保护内调用OnInsert</summary>
        /// <returns></returns>
        public override Int32 Insert()
        {
            return base.Insert();
        }

        /// <summary>已重载。在事务保护范围内处理业务，位于Valid之后</summary>
        /// <returns></returns>
        protected override Int32 OnInsert()
        {
            return base.OnInsert();
        }
        #endregion

        #region 扩展属性﻿
        //private Boolean _Valid;
        ///// <summary>是否有效</summary>
        //public Boolean IsValid { get { return _Valid; } set { _Valid = value; } }
        #endregion

        #region 扩展查询﻿
        /// <summary>根据编号查找</summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static Template FindByID(Int32 id)
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
        //public static EntityList<Template> Search(String key, String orderClause, Int32 startRowIndex, Int32 maximumRows)
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
        public static Boolean Has(String tid)
        {
            var id = 0;
            if (!Int32.TryParse(tid, out id)) return false;
            return Has(id);
        }

        public static Boolean Has(Int32 tid) { return FindByID(tid) != null; }

        public static EntityList<Template> GetValids() { return FindAllWithCache(__.Enable, true); }

        public static Template FindByPath(String path)
        {
            return Meta.Session.Cache.Entities.FindIgnoreCase(__.Directory, path);
        }

        //public static String Join(IEnumerable<Int32> ids)
        //{
        //    var sb = new StringBuilder();
        //    foreach (var item in ids)
        //    {
        //        if (sb.Length > 0) sb.Append(",");
        //        sb.Append(item + "");
        //    }
        //    return sb.ToString();
        //}

        public static Template CreateTemplate(string name, string directory, string copyright, string author, string createdate, string ver, string fordntver)
        {
            var tmp = new Template();
            tmp.Name = name;
            tmp.Directory = directory;
            tmp.Copyright = copyright;
            tmp.Author = author;
            tmp.Createdate = createdate;
            tmp.Ver = ver;
            tmp.Fordntver = fordntver;

            tmp.Save();

            return tmp;
        }

        public static Int32 Delete(IEnumerable<Int32> ids)
        {
            var count = 0;
            foreach (var item in ids)
            {
                var tmp = FindByID(item);
                if (tmp != null)
                {
                    tmp.Delete();
                    count++;
                }
            }

            return count;
        }

        public static void Delete(Int32[] ids, int uid, string userName, int groupId, string groupTitle, string ip)
        {
            //AdminTemplates.RemoveTemplateInDB(templateIdList, uid, userName, groupId, groupTitle, ip);
            var config = GeneralConfigInfo.Current;
            for (int i = 0; i < ids.Length; i++)
            {
                var name = DNTRequest.GetString("temp" + ids[i]);
                var tmp = FindByID(ids[i]);
                if (tmp != null)
                {
                    if (config.Templateid == tmp.ID)
                    {
                        config.Templateid = 1;
                        config.Save();
                    }
                    // 还是按照数据库里面的名字吧
                    name = tmp.Name;
                    tmp.Delete();
                }
                if (!String.IsNullOrEmpty(name))
                {
                    var path = BaseConfigs.GetForumPath.CombinePath("templates").GetFullPath();
                    var di = path.CombinePath(name).AsDirectory();
                    if (di.Exists) di.Delete(true);
                    di = path.CombinePath("..\\aspx\\" + ids[i]).AsDirectory();
                    if (di.Exists) di.Delete(true);
                }
            }
            AdminVisitLog.InsertLog(uid, userName, groupId, groupTitle, ip, "从模板库中删除模板文件", "ID为:" + ids.Join());
        }

        public static int GetWidth(string templatePath)
        {
            //var cacheService = XCache.Current;
            //string text = cacheService.RetrieveObject("/Forum/TemplateWidth/" + templatePath) as string;
            //if (text == null)
            //{
            //    text = AboutInfo.Load(Utils.GetMapPath(BaseConfigs.GetForumPath + "templates/" + templatePath + "/")).width;
            //    XCache.Add("/Forum/TemplateWidth/" + templatePath, text);
            //}
            //return Int32.Parse(text);

            var tmp = FindByPath(templatePath);
            return tmp != null && tmp.Width > 0 ? tmp.Width : 750;
        }

        public static EntityList<Template> Import(String path)
        {
            var di = path.AsDirectory();
            if (!di.Exists) return null;

            var list = FindAllWithCache();
            var ps = new EntityList<Template>();
            foreach (var item in path.AsDirectory().GetDirectories())
            {
                // 跳过已有
                if (list.FindIgnoreCase(__.Name, item.Name) != null || Find(__.Name, item.Name) != null) continue;

                // 跳过无效
                var xml = item.FullName.CombinePath("about.xml");
                if (!File.Exists(xml)) continue;

                //var ai = AboutInfo.Load(xml);
                var tmp = new Template();
                tmp.Import(item.Name, item.Name);
                tmp.Insert();

                ps.Add(tmp);
            }
            return ps;
        }

        public void Import(String name, String dir)
        {
            var xml = "templates".CombinePath(dir, "about.xml").GetFullPath();
            if (!File.Exists(xml)) return;

            var ai = AboutInfo.Load(xml);
            var tmp = this;
            if (!name.IsNullOrEmpty())
            {
                tmp.Name = name;
                tmp.Directory = name;
            }
            tmp.Author = ai.author;
            tmp.Copyright = ai.copyright;
            tmp.Ver = ai.ver;
            tmp.Fordntver = ai.fordntver;
            tmp.Createdate = ai.createdate;
            tmp.Width = ai.width.ToInt(750);
            tmp.Enable = true;
        }
        #endregion

        #region 模版about文件
        class AboutInfo
        {
            public string name = "";
            public string author = "";
            public string createdate = "";
            public string ver = "";
            public string fordntver = "";
            public string copyright = "";
            public string width = "600";

            public static AboutInfo Load(String xmlPath)
            {
                var info = new AboutInfo();

                var doc = new XmlDocument();
                doc.Load(xmlPath);
                var xmlNode = doc.SelectSingleNode("about");
                foreach (XmlNode node in xmlNode.ChildNodes)
                {
                    if (node.NodeType != XmlNodeType.Comment && node.Name.ToLower() == "template")
                    {
                        var ns = node.Attributes;
                        info.name = ((ns["name"] != null) ? ns["name"].Value : "");
                        info.author = ((ns["author"] != null) ? ns["author"].Value : "");
                        info.createdate = ((ns["createdate"] != null) ? ns["createdate"].Value : "");
                        info.ver = ((ns["ver"] != null) ? ns["ver"].Value : "");
                        info.fordntver = ((ns["fordntver"] != null) ? ns["fordntver"].Value : "");
                        info.copyright = ((ns["copyright"] != null) ? ns["copyright"].Value : "");
                        info.width = ((ns["width"] != null) ? ns["width"].Value : "600");
                    }
                }

                return info;
            }
        }

        #endregion
    }
}