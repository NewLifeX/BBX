﻿using System;
using NewLife;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;
using BBX.Common;
using NewLife.Log;
using NewLife.Web;
using XCode;
using XCode.Configuration;

namespace BBX.Entity
{
    /// <summary>奖励日志</summary>
    public partial class BonusLog : EntityBase<BonusLog>
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

        ///// <summary>首次连接数据库时初始化数据，仅用于实体类重载，用户不应该调用该方法</summary>
        //[EditorBrowsable(EditorBrowsableState.Never)]
        //protected override void InitData()
        //{
        //    base.InitData();

        //    // InitData一般用于当数据表没有数据时添加一些默认数据，该实体类的任何第一次数据库操作都会触发该方法，默认异步调用
        //    // Meta.Count是快速取得表记录数
        //    if (Meta.Count > 0) return;

        //    // 需要注意的是，如果该方法调用了其它实体类的首次数据库操作，目标实体类的数据初始化将会在同一个线程完成
        //    if (XTrace.Debug) XTrace.WriteLine("开始初始化{0}奖励日志数据……", typeof(BonusLog).Name);

        //    var entity = new BonusLog();
        //    entity.Tid = 0;
        //    entity.AuthorID = 0;
        //    entity.AnswerID = 0;
        //    entity.AnswerName = "abc";
        //    entity.Pid = 0;
        //    entity.Dateline = DateTime.Now;
        //    entity.Bonus = 0;
        //    entity.ExtID = 0;
        //    entity.IsBest = true;
        //    entity.Insert();

        //    if (XTrace.Debug) XTrace.WriteLine("完成初始化{0}奖励日志数据！", typeof(BonusLog).Name);
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
        private List<String> hasLoad = new List<String>();
        private Post _Post;
        /// <summary>帖子</summary>
        public Post Post
        {
            get
            {
                if (_Post == null && !hasLoad.Contains("Post"))
                {
                    _Post = Post.FindByID(Pid);
                    hasLoad.Add("Post");
                }
                return _Post;
            }
            set { _Post = value; }
        }

        /// <summary>帖子内容</summary>
        public String Message { get { return Post == null ? null : Post.Message; } }
        #endregion

        #region 扩展查询﻿
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
        //public static EntityList<BonusLog> Search(String key, String orderClause, Int32 startRowIndex, Int32 maximumRows)
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
        public static void CloseBonus(Topic topicinfo, int userid, int[] postIds, int[] winerIds, string[] winnerNames, string[] costBonuss, string[] valuableAnswers, int bestAnswer)
        {
            int isbest = 0;
            topicinfo.Special = 3;
            //Topics.UpdateTopic(topicinfo);
            //var tp = Topic.FindByID(topicinfo.Tid);
            var tp = topicinfo;
            tp.Special = 3;
            tp.Update();

            //var extid = Scoresets.GetBonusCreditsTrans();
            Byte extid = 0;
            for (int i = 0; i < winerIds.Length; i++)
            {
                int num = costBonuss[i].ToInt();
                if (winerIds[i] > 0 && num > 0)
                {
                    User.UpdateUserExtCredits(winerIds[i], extid, (float)num);
                }
                if (Utils.InArray(postIds[i].ToString(), valuableAnswers)) isbest = 1;
                if (postIds[i] == bestAnswer) isbest = 2;

                //BBX.Data.Bonus.AddLog(topicinfo.Tid, topicinfo.Posterid, winerIdArray[i], winnerNameArray[i], postIdArray[i], num, Scoresets.GetBonusCreditsTrans(), isbest);
                var log = new BonusLog();
                log.Tid = topicinfo.ID;
                log.AuthorID = topicinfo.PosterID;
                log.AnswerID = winerIds[i];
                log.AnswerName = winnerNames[i];
                log.Pid = postIds[i];
                log.Dateline = DateTime.Now;
                log.Bonus = num;
                log.ExtID = extid;
                log.IsBest = (Byte)isbest;
            }
        }

        public static List<BonusLog> GetLogs(Int32 topicid)
        {
            //if (topic.Tid <= 0 || topic.Special != 3) return null;

            //return BBX.Data.Bonus.GetLogs(topic.Tid, TableList.GetPostTableId(topic.Tid));

            if (topicid <= 0) return null;
            return FindAll(_.Tid == topicid & _.Bonus != 0, _.IsBest.Desc(), null, 0, 100);
        }

        public static Dictionary<int, BonusLog> GetLogsForEachPost(int tid)
        {
            //IDataReader topicBonusLogsByPost = DatabaseProvider.GetInstance().GetTopicBonusLogsByPost(tid);
            var list = FindAll(__.Tid, tid);
            var dic = new Dictionary<Int32, BonusLog>();
            foreach (var item in list)
            {
                dic.Add(item.Pid, item);
            }
            return dic;
        }
        #endregion
    }
}