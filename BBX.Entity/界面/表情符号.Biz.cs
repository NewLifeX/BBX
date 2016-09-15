/*
 * XCoder v6.1.5277.25230
 * 作者：nnhy/X
 * 时间：2014-07-13 23:04:29
 * 版权：版权所有 (C) 新生命开发团队 2002~2014
*/
﻿using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using BBX.Cache;
using BBX.Common;
using NewLife.Log;
using XCode;

namespace BBX.Entity
{
    /// <summary>表情符号</summary>
    public partial class Smilie : Entity<Smilie>
    {
        #region 对象操作﻿
        static Smilie()
        {
            // 缓存排序
            Meta.Cache.FillListMethod = () => FindAll(null, _.Type.Asc() & _.DisplayOrder.Asc() & _.ID.Asc(), null, 0, 0);
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
            if (XTrace.Debug) XTrace.WriteLine("开始初始化{0}[{1}]数据……", typeof(Smilie).Name, Meta.Table.DataTable.DisplayName);

            #region 初始化数据
            var entity = Add(0, "默认表情", "default");

            Add(entity.ID, ":-o", "default/0.gif");
            Add(entity.ID, ":~", "default/1.gif");
            Add(entity.ID, ":-|", "default/10.gif");
            Add(entity.ID, ":@", "default/11.gif");
            Add(entity.ID, ":Z", "default/12.gif");
            Add(entity.ID, ":D", "default/13.gif");
            Add(entity.ID, ":)", "default/14.gif");
            Add(entity.ID, ":(", "default/15.gif");
            Add(entity.ID, ":+", "default/16.gif");
            Add(entity.ID, ":#", "default/17.gif");
            Add(entity.ID, ":Q", "default/18.gif");
            Add(entity.ID, ":T", "default/19.gif");
            Add(entity.ID, ":*", "default/2.gif");
            Add(entity.ID, ":P", "default/20.gif");
            Add(entity.ID, ":-D", "default/21.gif");
            Add(entity.ID, ":m", "default/22.gif");
            Add(entity.ID, ":o", "default/23.gif");
            Add(entity.ID, ":g", "default/24.gif");
            Add(entity.ID, ":|-)", "default/25.gif");
            Add(entity.ID, ":!", "default/26.gif");
            Add(entity.ID, ":L", "default/27.gif");
            Add(entity.ID, ":giggle", "default/28.gif");
            Add(entity.ID, ":smoke", "default/29.gif");
            Add(entity.ID, ":|", "default/3.gif");
            Add(entity.ID, ":f", "default/30.gif");
            Add(entity.ID, ":-S", "default/31.gif");
            Add(entity.ID, ":?", "default/32.gif");
            Add(entity.ID, ":x", "default/33.gif");
            Add(entity.ID, ":yun", "default/34.gif");
            Add(entity.ID, ":8", "default/35.gif");
            Add(entity.ID, ":!", "default/36.gif");
            Add(entity.ID, ":!!!", "default/37.gif");
            Add(entity.ID, ":xx", "default/38.gif");
            Add(entity.ID, ":bye", "default/39.gif");
            Add(entity.ID, ":8-)", "default/4.gif");
            Add(entity.ID, ":pig", "default/40.gif");
            Add(entity.ID, ":cat", "default/41.gif");
            Add(entity.ID, ":dog", "default/42.gif");
            Add(entity.ID, ":hug", "default/43.gif");
            Add(entity.ID, ":$$", "default/44.gif");
            Add(entity.ID, ":(!)", "default/45.gif");
            Add(entity.ID, ":cup", "default/46.gif");
            Add(entity.ID, ":cake", "default/47.gif");
            Add(entity.ID, ":li", "default/48.gif");
            Add(entity.ID, ":bome", "default/49.gif");
            Add(entity.ID, ":<", "default/5.gif");
            Add(entity.ID, ":kn", "default/50.gif");
            Add(entity.ID, ":football", "default/51.gif");
            Add(entity.ID, ":music", "default/52.gif");
            Add(entity.ID, ":shit", "default/53.gif");
            Add(entity.ID, ":coffee", "default/54.gif");
            Add(entity.ID, ":eat", "default/55.gif");
            Add(entity.ID, ":pill", "default/56.gif");
            Add(entity.ID, ":rose", "default/57.gif");
            Add(entity.ID, ":fade", "default/58.gif");
            Add(entity.ID, ":kiss", "default/59.gif");
            Add(entity.ID, ":$", "default/6.gif");
            Add(entity.ID, ":heart:", "default/60.gif");
            Add(entity.ID, ":break:", "default/61.gif");
            Add(entity.ID, ":metting:", "default/62.gif");
            Add(entity.ID, ":gift:", "default/63.gif");
            Add(entity.ID, ":phone:", "default/64.gif");
            Add(entity.ID, ":time:", "default/65.gif");
            Add(entity.ID, ":email:", "default/66.gif");
            Add(entity.ID, ":TV:", "default/67.gif");
            Add(entity.ID, ":sun:", "default/68.gif");
            Add(entity.ID, ":moon:", "default/69.gif");
            Add(entity.ID, ":X", "default/7.gif");
            Add(entity.ID, ":strong:", "default/70.gif");
            Add(entity.ID, ":weak:", "default/71.gif");
            Add(entity.ID, ":share:", "default/72.gif");
            Add(entity.ID, ":v:", "default/73.gif");
            Add(entity.ID, ":duoduo:", "default/74.gif");
            Add(entity.ID, ":MM:", "default/75.gif");
            Add(entity.ID, ":hh:", "default/76.gif");
            Add(entity.ID, ":mm:", "default/77.gif");
            Add(entity.ID, ":beer:", "default/78.gif");
            Add(entity.ID, ":pesi:", "default/79.gif");
            Add(entity.ID, ":Zz", "default/8.gif");
            Add(entity.ID, ":xigua:", "default/80.gif");
            Add(entity.ID, ":yu:", "default/81.gif");
            Add(entity.ID, ":duoyun:", "default/82.gif");
            Add(entity.ID, ":snowman:", "default/83.gif");
            Add(entity.ID, ":xingxing:", "default/84.gif");
            Add(entity.ID, ":male:", "default/85.gif");
            Add(entity.ID, ":female:", "default/86.gif");
            Add(entity.ID, ":t(", "default/9.gif");
            #endregion

            if (XTrace.Debug) XTrace.WriteLine("完成初始化{0}[{1}]数据！", typeof(Smilie).Name, Meta.Table.DataTable.DisplayName);
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
        #endregion

        #region 扩展查询﻿
        /// <summary>根据编号查找</summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static Smilie FindByID(Int32 id)
        {
            if (Meta.Count >= 1000)
                return Find(__.ID, id);
            else // 实体缓存
                return Meta.Cache.Entities.Find(__.ID, id);
            // 单对象缓存
            //return Meta.SingleCache[id];
        }

        public static Smilie FindByCode(String code)
        {
            if (Meta.Count >= 1000)
                return Find(__.Code, code);
            else // 实体缓存
                return Meta.Cache.Entities.Find(__.Code, code);
            // 单对象缓存
            //return Meta.SingleCache[id];
        }

        public static EntityList<Smilie> FindAllByType(Int32 type)
        {
            if (Meta.Count >= 1000)
                return FindAll(__.Type, type);
            else // 实体缓存
                return Meta.Cache.Entities.FindAll(__.Type, type);
        }

        /// <summary>根据编号、显示顺序、类型查找</summary>
        /// <param name="id">编号</param>
        /// <param name="displayorder">显示顺序</param>
        /// <param name="type">类型</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static EntityList<Smilie> FindAllByIDAndDisplayOrderAndType(Int32 id, Int32 displayorder, Int32 type)
        {
            if (Meta.Count >= 1000)
                return FindAll(new String[] { __.ID, __.DisplayOrder, __.Type }, new Object[] { id, displayorder, type });
            else // 实体缓存
                return Meta.Cache.Entities.FindAll(e => e.ID == id && e.DisplayOrder == displayorder && e.Type == type);
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
        //public static EntityList<Smilie> Search(String key, String orderClause, Int32 startRowIndex, Int32 maximumRows)
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

        public static EntityList<Smilie> GetSmiliesTypes()
        {
            return FindAllWithCache(__.Type, 0);
        }
        #endregion

        #region 扩展操作
        public static Smilie Add(Int32 type, String code, String url, Int32 order = 0)
        {
            var entity = new Smilie();
            entity.Type = type;
            entity.DisplayOrder = order;
            entity.Code = code;
            entity.Url = url;
            entity.Insert();

            return entity;
        }

        public static void DeleteSmilies(String ids)
        {
            if (String.IsNullOrEmpty(ids)) return;

            var ds = ids.Split(",");

            foreach (var item in FindAllWithCache().ToArray())
            {
                if (Array.IndexOf(ds, item.ID) >= 0) item.Delete();
            }
        }
        #endregion

        #region 业务
        /// <summary>删除空的表情分类</summary>
        /// <returns></returns>
        public static string ClearEmptySmiliesType()
        {
            var sb = new StringBuilder();
            foreach (var item in GetSmiliesTypes())
            {
                var list = FindAllByType(item.ID);
                if (list.Count <= 0)
                {
                    if (sb.Length > 0) sb.Append(",");
                    sb.Append(item.ID);

                    item.Delete();
                }
            }

            return sb.ToString();
        }

        public static Int32 GetMaxSmiliesId()
        {
            return FindAllWithCache().ToList().Max(e => e.ID);
        }

        public static string GetSmiliesCache()
        {
            string text = XCache.Retrieve<String>(CacheKeys.FORUM_UI_SMILIES_LIST);
            if (Utils.StrIsNullOrEmpty(text))
            {
                var sb = new StringBuilder();
                //DataTable smiliesListDataTable = BBX.Data.Smilies.GetSmiliesListDataTable();
                var list = Smilie.FindAllWithCache();
                foreach (var sm in list)
                {
                    if (sm.Type != 0) continue;

                    sb.AppendFormat("'{0}': [\r\n", sm.Code.Trim().Replace("'", "\\'"));
                    bool flag = false;
                    foreach (var sm2 in list)
                    {
                        if (sm2.Type == sm.ID)
                        {
                            sb.Append("{'code' : '");
                            sb.Append(sm2.Code.Trim().Replace("'", "\\'"));
                            sb.Append("', 'url' : '");
                            sb.Append(sm2.Url.Trim().Replace("'", "\\'"));
                            sb.Append("'},\r\n");
                            flag = true;
                        }
                    }
                    if (sb.Length > 0 && flag)
                    {
                        sb.Remove(sb.Length - 3, 3);
                    }
                    sb.Append("\r\n],\r\n");
                }
                sb.Remove(sb.Length - 3, 3);
                text = sb.ToString();
                XCache.Add(CacheKeys.FORUM_UI_SMILIES_LIST, text);
            }
            return text;
        }
        #endregion
    }
}