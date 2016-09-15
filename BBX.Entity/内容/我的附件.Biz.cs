/*
 * XCoder v5.1.5002.17097
 * 作者：nnhy/X
 * 时间：2013-09-11 09:31:01
 * 版权：版权所有 (C) 新生命开发团队 2002~2013
*/
﻿using System;
using System.ComponentModel;
using System.Linq;
using BBX.Common;
using BBX.Config;
using XCode;

namespace BBX.Entity
{
    /// <summary>我的附件</summary>
    public partial class MyAttachment : EntityBase<MyAttachment>
    {
        #region 对象操作﻿
        static MyAttachment()
        {
            var df = Meta.Factory.AdditionalFields;
            df.Add(__.Downloads);
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

        protected override void OnLoad()
        {
            base.OnLoad();

            this.TrimField();
        }
        #endregion

        #region 扩展属性﻿
        /// <summary>简单名称</summary>
        public String SimpleName { get { return Utils.ConvertSimpleFileName(Name, "...", 6, 3, 15); } }
        #endregion

        #region 扩展查询﻿
        /// <summary>根据编号查找</summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static MyAttachment FindByID(Int32 id)
        {
            //if (Meta.Count >= 1000)
            //    return Find(_.ID, id);
            //else // 实体缓存
            //    return Meta.Cache.Entities.Find(__.ID, id);
            // 单对象缓存
            return Meta.SingleCache[id];
        }

        /// <summary>根据U编号、扩展名查找</summary>
        /// <param name="uid">U编号</param>
        /// <param name="extname">扩展名</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static EntityList<MyAttachment> FindAllByUidAndExtname(Int32 uid, String extname)
        {
            if (Meta.Count >= 1000)
                return FindAll(new String[] { _.Uid, _.Extname }, new Object[] { uid, extname });
            else // 实体缓存
                return Meta.Cache.Entities.FindAll(e => e.Uid == uid && e.Extname == extname);
        }

        public static Int32 FindCountByUidAndExtNames(Int32 uid, String extnames)
        {
            var exp = new WhereExpression();
            if (uid > 0) exp &= _.Uid == uid;
            if (extnames != null && extnames.Length > 0) exp &= _.Extname.In(extnames.Split(","));
            return FindCount(exp);
        }
        #endregion

        #region 高级查询
        public static EntityList<MyAttachment> Search(Int32 uid, Int32 typeid, String orderClause, Int32 startRowIndex, Int32 maximumRows)
        {
            return FindAll(SearchWhere(uid, typeid), orderClause, null, startRowIndex, maximumRows);
        }

        public static Int32 SearchCount(Int32 uid, Int32 typeid, String orderClause = null, Int32 startRowIndex = 0, Int32 maximumRows = -1)
        {
            return FindCount(SearchWhere(uid, typeid), null, null, 0, 0);
        }

        private static String SearchWhere(Int32 uid, Int32 typeid)
        {
            var exp = new WhereExpression();
            if (uid > 0) exp &= _.Uid == uid;
            if (typeid > 0)
            {
                var ts = MyAttachmentsTypeConfigInfo.Current.AttachmentType;
                if (ts == null || ts.Length <= 0) return "1=0";

                var exts = ts.Where(e => e.TypeId == typeid).Select(e => e.ExtName.EnsureStart(".")).ToArray();
                if (exts.Length == 0) return "1=0";

                exp &= _.Extname.In(exts);
            }

            return exp;
        }

        public static EntityList<MyAttachment> FindAllByAIDs(String aids)
        {
            if (String.IsNullOrEmpty(aids)) return new EntityList<MyAttachment>();

            return FindAll(_.ID.In(aids.SplitAsInt()), null, null, 0, 0);
        }
        #endregion

        #region 扩展操作
        #endregion

        #region 业务
        public static AttachmentType[] AttachTypeList()
        {
            return MyAttachmentsTypeConfigInfo.Current.AttachmentType;
        }
        #endregion
    }
}