﻿using System;
using System.Collections.Generic;
using System.Text;
using BBX.Common;
using System.Linq;
using XCode;

namespace BBX.Entity
{
    /// <summary>广告类型</summary>
    public enum AdType
    {
        HeaderAd,
        FooterAd,
        PageWordAd,
        InPostAd,
        FloatAd,
        DoubleAd,
        MediaAd,
        PostLeaderboardAd,
        InForumAd,
        QuickEditorAd,
        QuickEditorBgAd,
        WebSiteHeaderAd,
        WebSiteHotTopicAd,
        WebSiteUserPostTopAd,
        WebSiteRecForumTopAd,
        WebSiteRecForumBottomAd,
        WebSiteRecAlbumAd,
        WebSiteBottomAd,
        PageAd
    }

    /// <summary>广告</summary>
    public partial class Advertisement : EntityBase<Advertisement>
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

        protected override Advertisement CreateInstance(bool forEdit = false)
        {
            var entity = base.CreateInstance(forEdit);
            entity.DisplayOrder = -1;
            entity.Code = "暂无内容";
            entity.Parameters = "|||||||";

            return entity;
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
        //    if (XTrace.Debug) XTrace.WriteLine("开始初始化{0}广告数据……", typeof(Advertisement).Name);

        //    var entity = new Advertisement();
        //    entity.Available = 0;
        //    entity.Type = "abc";
        //    entity.DisplayOrder = 0;
        //    entity.Title = "abc";
        //    entity.Targets = "abc";
        //    entity.StartTime = DateTime.Now;
        //    entity.EndTime = DateTime.Now;
        //    entity.Code = "abc";
        //    entity.Parameters = "abc";
        //    entity.Insert();

        //    if (XTrace.Debug) XTrace.WriteLine("完成初始化{0}广告数据！", typeof(Advertisement).Name);
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
        public static Advertisement FindByID(Int32 id)
        {
            return Meta.Cache.Entities.Find(__.ID, id);
        }

        public static EntityList<Advertisement> FindAllByType(Int32 type)
        {
            if (type < 0)
                return Meta.Cache.Entities;
            else
                return Meta.Cache.Entities.FindAll(__.Type, type);
        }

        public static EntityList<Advertisement> FindAllByIDs(String ids)
        {
            //return FindAll(__.ID.In(ids), null, null, 0, 0);

            if (ids.IsNullOrWhiteSpace()) return new EntityList<Advertisement>();

            var ds = ids.Trim().Split(",").Select(e => Int32.Parse(e)).ToArray();
            var list = Meta.Cache.Entities.ToList().Where(e => Array.IndexOf(ds, e.ID) >= 0).ToList();
            return new EntityList<Advertisement>(list);
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
        //public static EntityList<Advertisement> Search(String key, String orderClause, Int32 startRowIndex, Int32 maximumRows)
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

        public static String SearchWhere(String pagename, Int32 fid, AdType adtype)
        {
            var adt = Convert.ToInt16(adtype);
            var where = "";
            if (pagename == "indexad")
                where = ",首页,";
            else if (fid > 0)
                where = "," + fid + ",";

            if (string.IsNullOrEmpty(where))
            {
                if (adtype >= AdType.WebSiteHeaderAd)
                    return _.Type == adt;
                else
                    return _.Type == adt & _.Targets.Contains("全部");
            }

            return _.Type == adt & (_.Targets.Contains(where) | _.Targets.Contains("全部"));
        }

        public static List<Advertisement> SearchWithCache(String pagename, Int32 fid, AdType adtype, Int32 count = 0)
        {
            var list = Meta.Cache.Entities.FindAll(__.Type, adtype);
            if (list.Count < 1) return list;

            // 必须是有效广告
            list = list.FindAll(__.Available, true);

            var adt = Convert.ToInt16(adtype);
            var where = "";
            if (pagename == "indexad")
                where = ",首页,";
            else if (fid > 0)
                where = "," + fid + ",";

            if (count <= 0) count = 100;

            if (string.IsNullOrEmpty(where))
            {
                if (adtype >= AdType.WebSiteHeaderAd) return list.ToList().Take(count).ToList();

                return list.ToList().Where(e => e.Targets.Contains("全部")).Take(count).ToList();
            }

            return list.ToList().Where(e => e.Targets.Contains(where) || e.Targets.Contains("全部")).Take(count).ToList();
        }
        #endregion

        #region 扩展操作
        public static void DeleteAll(String ids)
        {
            //var list = FindAll(__.ID.In(ids), null, null, 0, 0);
            var list = FindAllByIDs(ids);
            if (list.Count > 0) list.Delete();
        }
        #endregion

        #region 业务
        static Random _rnd = new Random((Int32)DateTime.Now.Ticks);

        public static string GetOneHeaderAd(string pagename, int forumid)
        {
            var list = SearchWithCache(pagename, forumid, AdType.HeaderAd);
            if (list.Count < 1) return "";

            return list[_rnd.Next(0, list.Count)].Code + "";
        }

        public static string GetOneFooterAd(string pagename, int forumid)
        {
            var list = SearchWithCache(pagename, forumid, AdType.FooterAd);
            if (list.Count < 1) return "";

            return list[_rnd.Next(0, list.Count)].Code + "";
        }

        public static int GetInPostAdCount(string pagename, int forumid)
        {
            return SearchWithCache(pagename, forumid, AdType.InPostAd).Count;
        }

        public static string[] GetPageWordAd(string pagename, int forumid)
        {
            var list = SearchWithCache(pagename, forumid, AdType.PageWordAd);
            if (list.Count < 1) return new string[0];

            return list.Select(e => e.Code).ToArray();
        }

        public static string GetDoubleAd(string pagename, int forumid)
        {
            var list = SearchWithCache(pagename, forumid, AdType.DoubleAd);
            if (list.Count < 1) return "";

            var entity = list[_rnd.Next(0, list.Count)];

            var ps = entity.Parameters.Split('|');

            var adsMsg = entity.GetAdsMsg();
            if (adsMsg.IsNullOrWhiteSpace()) return "";

            var sb = new StringBuilder();
            sb.Append("<script type=\"text/javascript\">");
            sb.AppendFormat("\ntheFloaters.addItem('coupleBannerAdv1','(document.body.clientWidth>document.documentElement.clientWidth ? document.documentElement.clientWidth :document.body.clientWidth )-{0}-90',10,'{1}<br /><img src=\"images/common/advclose.gif\" onMouseOver=\"this.style.cursor=\\'hand\\'\" onClick=\"closeBanner();\">');", String.IsNullOrEmpty(ps[2]) ? "0" : ps[2], adsMsg);
            sb.AppendFormat("\ntheFloaters.addItem('coupleBannerAdv2',10,10,'{0}<br /><img src=\"images/common/advclose.gif\" onMouseOver=\"this.style.cursor=\\'hand\\'\" onClick=\"closeBanner();\">');", adsMsg);
            sb.Append("\n</script>");
            return sb.ToString();
        }

        private string GetAdsMsg()
        {
            if (Parameters.IsNullOrWhiteSpace()) return "";

            var ps = Parameters.Split('|');
            if (ps.Length < 4 || ps[0].ToLower() != "flash") return Code;

            return String.Format("<object classid=\"clsid:D27CDB6E-AE6D-11cf-96B8-444553540000\" codebase=\"http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,19,0\" width=\"{0}\" height=\"{1}\"><param name=\"movie\" value=\"{2}\" /><param name=\"quality\" value=\"high\" /><param name=\"wmode\" value=\"opaque\">{3}<embed src=\"{2}\" wmode=\"opaque\" quality=\"high\" pluginspage=\"http://www.macromedia.com/go/getflashplayer\" type=\"application/x-shockwave-flash\" width=\"{0}\" height=\"{1}\"></embed></object>", ps[2], ps[3], ps[1], Code);
        }

        public static string GetFloatAd(string pagename, int forumid)
        {
            var list = SearchWithCache(pagename, forumid, AdType.FloatAd);
            if (list.Count < 1) return "";

            var entity = list[_rnd.Next(0, list.Count)];

            var ps = entity.Parameters.Split('|');

            var adsMsg = entity.GetAdsMsg();
            if (adsMsg.IsNullOrWhiteSpace()) return "";

            return "<script type='text/javascript'>theFloaters.addItem('floatAdv',10,'(document.body.clientHeight>document.documentElement.clientHeight ? document.documentElement.clientHeight :document.body.clientHeight)-" + (String.IsNullOrEmpty(ps[3]) ? "0" : ps[3]) + "-40','" + adsMsg + "');</script>";
        }

        public static string GetMediaAd(string templatepath, string pagename, int forumid)
        {
            var list = SearchWithCache(pagename, forumid, AdType.MediaAd, 1);
            if (list.Count < 1) return "";

            return String.Format(list[0].Code, templatepath, pagename, forumid);
        }

        public static List<String> GetPageAd(string pagename, int forumid)
        {
            var list = SearchWithCache(pagename, forumid, AdType.PageAd);
            if (list.Count < 1) return new List<String>();

            return list.Select(e => e.Code).ToList();
        }

        public static string GetQuickEditorAD(string pagename, int forumid)
        {
            var list = SearchWithCache(pagename, forumid, AdType.QuickEditorAd);
            if (list.Count < 1) return "";

            return list[_rnd.Next(0, list.Count)].Code + "";
        }

        public static string[] GetQuickEditorBgAd(string pagename, int forumid)
        {
            var arr = new string[] { "", "" };

            var list = SearchWithCache(pagename, forumid, AdType.QuickEditorBgAd);
            if (list.Count < 1) return arr;

            var ps = list[_rnd.Next(0, list.Count)].Parameters.Split('|');
            arr[1] = ps[1];
            arr[0] = ps[4];

            return arr;
        }

        public static string[] GetMediaAdParams(string pagename, int forumid)
        {
            var list = SearchWithCache(pagename, forumid, AdType.MediaAd, 1);
            if (list.Count < 1) return new string[0];

            return list[0].Parameters.Split('|');
        }

        public static string GetInForumAd(string pagename, int forumid, List<IXForum> topforum, string templatepath)
        {
            var list = SearchWithCache(pagename, forumid, AdType.InForumAd);
            if (list.Count < 1) return "";

            var entity = list[_rnd.Next(0, list.Count)];

            var sb = new StringBuilder();

            sb.Append("<div style=\"display: none\" id=\"ad_none\">\r\n");
            int i = 0;
            while (i < topforum.Count && i < list.Count)
            {
                sb.AppendFormat("<div class=\"ad_column\" id=\"ad_intercat_{0}_none\">{1}</div>\r\n", topforum[i].Fid, list[i].Code);
                i++;
            }
            sb.Append("</div><script type='text/javascript' src='javascript/template_inforumad.js'></script>\r\n");

            return sb.ToString();
        }

        public static string GetInPostAd(string pagename, int forumid, string templatepath, int count)
        {
            var list = SearchWithCache(pagename, forumid, AdType.InPostAd);
            if (list.Count < 1) return "";

            var sb = new StringBuilder();

            sb.Append("<div style=\"display: none;\" id=\"ad_none\">\r\n");
            sb.Append(GetAdShowInfo(list, count, 0));
            sb.Append(GetAdShowInfo(list, count, 1));
            sb.Append(GetAdShowInfo(list, count, 2));
            sb.Append("</div><script type='text/javascript' src='javascript/template_inforumad.js'></script>\r\n");

            return sb.ToString();
        }

        static string GetAdShowInfo(List<Advertisement> ads, int count, int inPostAdType)
        {
            var sb = new StringBuilder();
            for (int i = 1; i <= count; i++)
            {
                var list = new List<Advertisement>();
                foreach (var item in ads)
                {
                    string[] array = Utils.SplitString(item.Parameters.Trim(), "|", 9);
                    if (array[7].ToInt(-1) == inPostAdType && (Utils.InArray(i.ToString(), array[8], ",") || array[8] == "0"))
                    {
                        list.Add(item);
                    }
                }
                if (list.Count > 0)
                {
                    var ad = list[_rnd.Next(0, list.Count)];
                    switch (inPostAdType)
                    {
                        case 0:
                            sb.AppendFormat("<div class=\"ad_textlink1\" id=\"ad_thread1_{0}_none\">{1}</div>\r\n", i, ad.Code);
                            break;

                        case 1:
                            sb.AppendFormat("<div class=\"ad_textlink2\" id=\"ad_thread2_{0}_none\">{1}</div>\r\n", i, ad.Code);
                            break;

                        default:
                            sb.AppendFormat("<div class=\"ad_pip\" id=\"ad_thread3_{0}_none\">{1}</div>\r\n", i, ad.Code);
                            break;
                    }
                }
            }
            return sb.ToString();
        }

        public static string GetOnePostLeaderboardAD(string pagename, int forumid)
        {
            var list = SearchWithCache(pagename, forumid, AdType.PostLeaderboardAd, 1);
            if (list.Count < 1) return "";

            return list[_rnd.Next(0, list.Count)].Code + "";
        }

        public static string GetInPostAdXMLByFloor(string pagename, int forumid, int floor)
        {
            var list = SearchWithCache(pagename, forumid, AdType.InPostAd);
            if (list.Count < 1) return "";

            var sb = new StringBuilder();

            sb.Append(GetAdShowInfoXMLByFloor(list, floor, 0));
            sb.Append(GetAdShowInfoXMLByFloor(list, floor, 1));
            sb.Append(GetAdShowInfoXMLByFloor(list, floor, 2));

            return sb.ToString();
        }

        public static string GetAdShowInfoXMLByFloor(List<Advertisement> ads, int floor, int inPostAdType)
        {
            var sb = new StringBuilder();
            var list = new List<Advertisement>();
            foreach (var item in ads)
            {
                string[] array = Utils.SplitString(item.Parameters.ToString().Trim(), "|", 9);
                if (array[7].ToInt(-1) == inPostAdType && (Utils.InArray(floor.ToString(), array[8], ",") || array[8] == "0"))
                {
                    list.Add(item);
                }
            }
            if (list.Count > 0)
            {
                var ad = list[_rnd.Next(0, list.Count)];
                return String.Format("<ad_thread{0}><![CDATA[{1}]]></ad_thread{0}>", inPostAdType, ad.Code);
            }
            return "";
        }

        public static string GetWebSiteAd(AdType adType)
        {
            var list = SearchWithCache("", 0, AdType.InPostAd);
            if (list.Count < 1) return "";

            return list[_rnd.Next(0, list.Count)].Code + "";
        }

        public static void CreateAd(int available, string type, int displayorder, string title, string targets, string parameters, string code, string startTime, string endTime)
        {
            targets = targets.IndexOf("全部") >= 0 ? ",全部," : ("," + targets + ",");

            var entity = new Advertisement();
            entity.Available = available;
            entity.Type = type;
            entity.DisplayOrder = displayorder;
            entity.Title = title;
            entity.Targets = targets;
            entity.Parameters = parameters;
            entity.Code = code;
            if (!startTime.IsNullOrWhiteSpace() && !startTime.Contains("1900")) entity.StartTime = startTime.ToDateTime();
            if (!endTime.IsNullOrWhiteSpace() && !endTime.Contains("1900")) entity.EndTime = endTime.ToDateTime();
            entity.Save();

            //Advertisenments.CreateAd(available, type, displayorder, title, targets, parameters, code, (startTime.IndexOf("1900") >= 0) ? "1900-1-1" : startTime, (endTime.IndexOf("1900") >= 0) ? "2555-1-1" : endTime);
            //DNTCache.Current.RemoveObject(CacheKeys.FORUM_ADVERTISEMENTS);
        }

        public static void SetAvailable(String ids, Int32 avaiable)
        {
            //var list = FindAll(__.ID.In(ids), null, null, 0, 0);
            var list = FindAllByIDs(ids);
            if (list.Count > 0)
            {
                list.SetItem(_.Available, avaiable);
                list.Save();
            }
        }

        public static void SetAvailable(Int32 type, Int32 avaiable)
        {
            //var list = FindAll(__.Type == type & _.EndTime < DateTime.Now, null, null, 0, 0);
            var list = FindAllByType(type);
            list = new EntityList<Advertisement>(list.ToList().Where(e => e.EndTime < DateTime.Now));
            if (list.Count > 0)
            {
                list.SetItem(_.Available, avaiable);
                list.Save();
            }
        }
        #endregion
    }
}