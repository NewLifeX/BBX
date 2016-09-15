/*
 * XCoder v5.1.5002.17097
 * 作者：nnhy/X
 * 时间：2013-09-11 09:31:00
 * 版权：版权所有 (C) 新生命开发团队 2002~2013
*/
﻿using System;
using System.ComponentModel;
using BBX.Common;
using NewLife.Log;
using XCode;

namespace BBX.Entity
{
    /// <summary>BB代码</summary>
	public partial class BbCode : EntityBase<BbCode>
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
            if (XTrace.Debug) XTrace.WriteLine("开始初始化{0}[{1}]数据……", typeof(BbCode).Name, Meta.Table.DataTable.DisplayName);

            //INSERT INTO [dnt_bbcodes] VALUES (0,'fly','fly.gif','[fly]示例文字[/fly]',1,1,'在帖子中插入滚动文字','<marquee width="90%" behavior="alternate" scrollamount="3">{1}</marquee>','请输入滚动文字','滚动文字');
            //INSERT INTO [dnt_bbcodes] VALUES (0,'silverlight','silverlight.gif','[silverlight]http://localhost/123.wmv[/silverlight]',3,1,'在帖子中使用Silverlight播放器', '<script type="text/javascript" src="silverlight/player/showtopiccontainer.js"></script><div id="divPlayer_{RANDOM}"></div><script>Silverlight.InstallAndCreateSilverlight("1.0",document.getElementById("divPlayer_{RANDOM}"),installExperienceHTML,"InstallPromptDiv",function(){new StartPlayer_0("divPlayer_{RANDOM}", parseInt("{2}"), parseInt("{3}"), "{1}", forumpath)})</script>', '请输入音频或视频的地址,请输入音频或视频的宽度,请输入视频的高度(音频无效)', 'http://,400,300');

            var entity = new BbCode();
            entity.Available = 0;
            entity.Tag = "fly";
            entity.Icon = "fly.gif";
            entity.Example = "[fly]示例文字[/fly]";
            entity.Params = 1;
            entity.Nest = 1;
            entity.Explanation = "在帖子中插入滚动文字";
            entity.Replacement = "<marquee width=\"90%\" behavior=\"alternate\" scrollamount=\"3\">{1}</marquee>";
            entity.ParamsDescript = "请输入滚动文字";
            entity.ParamsDefValue = "滚动文字";
            entity.Insert();

            entity.Available = 0;
            entity.Tag = "silverlight";
            entity.Icon = "silverlight.gif";
            entity.Example = "[silverlight]http://localhost/123.wmv[/silverlight]";
            entity.Params = 3;
            entity.Nest = 1;
            entity.Explanation = "在帖子中使用Silverlight播放器";
            entity.Replacement = "<script type=\"text/javascript\" src=\"silverlight/player/showtopiccontainer.js\"></script><div id=\"divPlayer_{RANDOM}\"></div><script>Silverlight.InstallAndCreateSilverlight(\"1.0\",document.getElementById(\"divPlayer_{RANDOM}\"),installExperienceHTML,\"InstallPromptDiv\",function(){new StartPlayer_0(\"divPlayer_{RANDOM}\", parseInt(\"{2}\"), parseInt(\"{3}\"), \"{1}\", forumpath)})</script>";
            entity.ParamsDescript = "请输入音频或视频的地址,请输入音频或视频的宽度,请输入视频的高度(音频无效)";
            entity.ParamsDefValue = "http://,400,300";
            entity.Insert();

            if (XTrace.Debug) XTrace.WriteLine("完成初始化{0}[{1}]数据！", typeof(BbCode).Name, Meta.Table.DataTable.DisplayName);
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
        /// <summary>根据是否可用查找</summary>
        /// <param name="available">是否可用</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static EntityList<BbCode> FindAllByAvailable(Int32 available)
        {
            if (Meta.Count >= 1000)
                return FindAll(_.Available, available);
            else // 实体缓存
                return Meta.Cache.Entities.FindAll(__.Available, available);
        }

        /// <summary>根据编号查找</summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static BbCode FindByID(Int32 id)
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
        //public static EntityList<BbCode> Search(String key, String orderClause, Int32 startRowIndex, Int32 maximumRows)
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
        public static void UpdateBBCode(int available, string tag, string icon, string replacement, string example, string explanation, string param, string nest, string paramsDescription, string paramsDefaultValue, int id)
        {
            var code = FindByID(id);
            if (code != null)
            {
                code.Available = available;
                code.Tag = tag;
                code.Replacement = replacement;
                code.Example = example;
                code.Explanation = explanation;
                code.Params = Int32.Parse(param);
                code.Nest = Int32.Parse(nest);
                code.ParamsDescript = paramsDescription;
                code.ParamsDefValue = paramsDefaultValue;
                code.Update();
            }
        }

        public static void DeleteBBCode(string idList)
        {
            var list = FindAll(_.ID.In(idList.SplitAsInt()), null, null, 0, 0);
            list.Delete();
        }

        public static void BatchUpdateAvailable(int status, string idList)
        {
            if (Utils.IsNumericList(idList))
            {
                var list = FindAll(_.ID.In(idList.SplitAsInt()), null, null, 0, 0);
                list.ForEach(e => e.Available = status);
                list.Update();
            }
        }

        public static void CreateBBCCode(int available, string tag, string icon, string replacement, string example, string explanation, string param, string nest, string paramsDescript, string paramsDefvalue)
        {
            var code = new BbCode();
            code.Available = available;
            code.Tag = tag;
            code.Replacement = replacement;
            code.Example = example;
            code.Explanation = explanation;
            code.Params = Int32.Parse(param);
            code.Nest = Int32.Parse(nest);
            code.ParamsDescript = paramsDescript;
            code.ParamsDefValue = paramsDefvalue;
            code.Insert();
        }
        #endregion
    }
}