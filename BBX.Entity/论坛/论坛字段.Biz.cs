﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;
using NewLife.Log;
using NewLife.Web;
using XCode;
using XCode.Configuration;
using System.Linq;

namespace BBX.Entity
{
    /// <summary>论坛字段</summary>
    public partial class ForumField : EntityBase<ForumField>
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

        protected override void OnLoad()
        {
            base.OnLoad();

            var ss = (Moderators + "").Split(",");
            _ModeratorsCollection = new HashSet<String>(ss, StringComparer.OrdinalIgnoreCase);
        }
        #endregion

        #region 扩展属性﻿
        private ICollection<String> _ModeratorsCollection;
        /// <summary>论坛版主</summary>
        public ICollection<String> ModeratorCollection { get { return _ModeratorsCollection; } set { _ModeratorsCollection = value; } }
        #endregion

        #region 扩展查询﻿
        /// <summary>根据编号查找</summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static ForumField FindByID(Int32 id)
        {
            if (Meta.Count >= 1000)
                return Find(_.Fid, id);
            else // 实体缓存
                return Meta.Cache.Entities.Find(__.Fid, id);
            // 单对象缓存
            //return Meta.SingleCache[id];
        }

        ///// <summary>根据F编号查找</summary>
        ///// <param name="fid">F编号</param>
        ///// <returns></returns>
        //[DataObjectMethod(DataObjectMethodType.Select, false)]
        //public static EntityList<ForumField> FindAllByFid(Int32 fid)
        //{
        //    if (Meta.Count >= 1000)
        //        return FindAll(__.Fid, fid);
        //    else // 实体缓存
        //        return Meta.Cache.Entities.FindAll(__.Fid, fid);
        //}
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
        //public static EntityList<ForumField> Search(String key, String orderClause, Int32 startRowIndex, Int32 maximumRows)
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
        public static Boolean CheckRewriteNameInvalid(String rewriteName)
        {
            var entity = new ForumField();
            entity.RewriteName = rewriteName;
            return !entity.Exist(false, rewriteName);
        }

        public Boolean AllowView(Int32 gid = 7)
        {
            if (ViewPerm.IsNullOrWhiteSpace()) return true;

            return ViewPerm.SplitAsInt().Contains(gid);
        }

        public Boolean AllowPost(int usergroupid)
        {
            return PostPerm.IsNullOrWhiteSpace() || PostPerm.SplitAsInt().Contains(usergroupid);
        }

        public Boolean AllowReply(int usergroupid)
        {
            return ReplyPerm.IsNullOrWhiteSpace() || ReplyPerm.SplitAsInt().Contains(usergroupid);
        }

        public Boolean AllowGetAttach(int usergroupid)
        {
            return GetattachPerm.IsNullOrWhiteSpace() || GetattachPerm.SplitAsInt().Contains(usergroupid);
        }

        public Boolean AllowPostAttach(int usergroupid)
        {
            return PostattachPerm.IsNullOrWhiteSpace() || PostattachPerm.SplitAsInt().Contains(usergroupid);
        }
        #endregion
    }
}