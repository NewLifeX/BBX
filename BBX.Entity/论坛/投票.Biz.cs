﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;
using NewLife.Log;
using NewLife.Web;
using XCode;
using XCode.Configuration;

using BBX.Entity;

using BBX.Common;

namespace BBX.Entity
{
    /// <summary>投票</summary>
    public partial class Poll : EntityBase<Poll>
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
        //    if (XTrace.Debug) XTrace.WriteLine("开始初始化{0}投票数据……", typeof(Poll).Name);

        //    var entity = new Poll();
        //    entity.Tid = 0;
        //    entity.DisplayOrder = 0;
        //    entity.Multiple = 0;
        //    entity.Visible = 0;
        //    entity.AllowView = true;
        //    entity.Maxchoices = 0;
        //    entity.Expiration = "abc";
        //    entity.Uid = 0;
        //    entity.Voternames = "abc";
        //    entity.Insert();

        //    if (XTrace.Debug) XTrace.WriteLine("完成初始化{0}投票数据！", typeof(Poll).Name);
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
        /// <summary>根据T编号查找</summary>
        /// <param name="tid">T编号</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static Poll FindByTid(Int32 tid)
        {
            if (Meta.Count >= 1000)
                return Find(__.Tid, tid);
            else // 实体缓存
                return Meta.Cache.Entities.Find(__.Tid, tid);
        }

        public static EntityList<Poll> FindAllByUid(Int32 uid)
        {
            if (Meta.Count >= 1000)
                return FindAll(__.Uid, uid);
            else // 实体缓存
                return Meta.Cache.Entities.FindAll(__.Uid, uid);
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
        //public static EntityList<Poll> Search(String key, String orderClause, Int32 startRowIndex, Int32 maximumRows)
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

        #region 业务  投票相关的业务类，@宁波-小董 2012-11-13

        public static string GetPollEnddatetime(int tid)
        {
            //return Utils.GetDate(Poll.Find(__.Tid, tid).Expiration, Utils.GetDate());           
            var entity = FindByTid(tid);
            if (entity == null) return null;

            if (entity.Expiration <= DateTime.MinValue) return null;

            return entity.Expiration.ToString("yyyy-MM-dd");
        }
        //获取投票的用户名列表
        public string GetPollUserNameList(int tid)
        {
            return FindByTid(tid).Voternames;
        }

        #region  创建投票
        //创建投票
        public static bool CreatePoll(int tid, int multiple, int itemcount, string itemnamelist, string itemvaluelist, DateTime enddatetime, int userid, int maxchoices, int visible, bool allowview)
        {
            string[] array = Utils.SplitString(itemnamelist, "\r\n");
            if (array.Length != itemcount || Utils.SplitString(itemvaluelist, "\r\n").Length != itemcount)
            {
                return false;
            }
            int num = new Poll()
            {
                DisplayOrder = 0,
                Expiration = enddatetime,
                MaxChoices = (Int16)maxchoices,
                Multiple = multiple,
                Tid = tid,
                Uid = userid,
                Visible = visible,
                AllowView = allowview
            }.Insert();
            if (num > 0)
            {
                for (int i = 0; i < itemcount; i++)
                {
                    new PollOption()
                    {
                        DisplayOrder = i + 1,
                        PollID = num,
                        Name = Utils.GetSubString(array[i], 80, ""),
                        Tid = tid,
                        VoterNames = "",
                        Votes = 0
                    }.Insert();
                }
                return true;
            }
            return false;
        }

        //创建投票
        public static string CreatePoll(string pollItemName, int multiple, int maxchoices, int visiblepoll, bool allowview, DateTime enddatetime, int tid, string[] pollitem, int userid)
        {
            string result = null;
            StringBuilder stringBuilder = new StringBuilder("");
            for (int i = 0; i < pollitem.Length; i++)
            {
                stringBuilder.Append("0\r\n");
            }
            string text = Utils.HtmlEncode(pollItemName);
            if (text != "")
            {
                if (multiple <= 0)
                {
                    multiple = 0;
                }
                if (multiple == 1 && maxchoices > pollitem.Length)
                {
                    maxchoices = pollitem.Length;
                }
                if (!CreatePoll(tid, multiple, pollitem.Length, text.Trim(), stringBuilder.ToString().Trim(), enddatetime, userid, maxchoices, visiblepoll, allowview))
                {
                    result = "投票错误";
                }
            }
            else
            {
                result = "投票项为空";
            }
            return result;
        }
        #endregion

        #region 更新投票
        public static bool UpdatePoll(int tid, int multiple, int itemcount, string polloptionidlist, string itemnamelist, string itemdisplayorderlist, DateTime enddatetime, int maxchoices, int visible, bool allowview)
        {
            string[] array = Utils.SplitString(itemnamelist, "\r\n");
            string[] array2 = Utils.SplitString(itemdisplayorderlist, "\r\n");
            string[] array3 = Utils.SplitString(polloptionidlist, "\r\n");
            if (array.Length != itemcount || array2.Length != itemcount)
            {
                return false;
            }

            var pollInfo = FindByTid(tid);
            pollInfo.Expiration = enddatetime;
            pollInfo.MaxChoices = (Int16)maxchoices;
            pollInfo.Multiple = multiple;
            pollInfo.Tid = tid;
            pollInfo.Visible = visible;
            pollInfo.AllowView = allowview;
            bool flag = false;
            if (pollInfo.ID > 0)
            {
                flag = pollInfo.Update() > 1 ? true : false;
            }
            if (flag)
            {
                List<PollOption> pollOptionInfoCollection = PollOption.FindAll(PollOption._.Tid, pollInfo.Tid);
                int num = 0;
                string[] array4 = array3;
                for (int i = 0; i < array4.Length; i++)
                {
                    string a = array4[i];
                    bool flag2 = false;
                    foreach (PollOption current in pollOptionInfoCollection)
                    {
                        if (a == current.ID.ToString())
                        {
                            current.PollID = pollInfo.ID;
                            current.Name = Utils.GetSubString(array[num], 80, "");
                            current.DisplayOrder = (Utils.StrIsNullOrEmpty(array2[num]) ? (num + 1) : Utils.StrToInt(array2[num], 0));
                            //BBX.Data.Polls.UpdatePollOption(current);
                            current.Update();
                            flag2 = true;
                            break;
                        }
                    }
                    if (!flag2)
                    {
                        new PollOption()
                        {
                            DisplayOrder = Utils.StrIsNullOrEmpty(array2[num]) ? (num + 1) : Utils.StrToInt(array2[num], 0),
                            PollID = pollInfo.ID,
                            Name = Utils.GetSubString(array[num], 80, ""),
                            Tid = tid,
                            VoterNames = "",
                            Votes = 0
                        }.Insert();
                    }
                    num++;
                }
                foreach (PollOption current2 in pollOptionInfoCollection)
                {
                    if (("\r\n" + polloptionidlist + "\r\n").IndexOf("\r\n" + current2.ID + "\r\n") < 0)
                    {
                        current2.Delete();
                    }
                }
                return true;
            }
            return false;
        }

        public static int UpdatePoll(int tid, string selitemidlist, string username)
        {
            if (Utils.StrIsNullOrEmpty(username))
            {
                return -1;
            }
            string[] array = Utils.SplitString(selitemidlist, ",");
            string[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                string expression = array2[i];
                if (Utils.StrToInt(expression, -1) == -1)
                {
                    return -1;
                }
            }
            var pollInfo = FindByTid(tid);
            if (pollInfo.ID < 1)
            {
                return -3;
            }
            if (Utils.StrIsNullOrEmpty(pollInfo.Voternames))
            {
                pollInfo.Voternames = username;
            }
            else
            {
                pollInfo.Voternames = pollInfo.Voternames + "\r\n" + username;
            }
            pollInfo.Update();
            //List<PollOption > pollOptionInfoCollection = BBX.Data.Polls.GetPollOptionInfoCollection(pollInfo.Tid);
            List<PollOption> pollOptionInfoCollection = PollOption.FindAll(PollOption._.Tid, pollInfo.Tid);
            string[] array3 = array;
            for (int j = 0; j < array3.Length; j++)
            {
                string a = array3[j];
                foreach (PollOption current in pollOptionInfoCollection)
                {
                    if (a == current.ID.ToString())
                    {
                        if (Utils.StrIsNullOrEmpty(current.VoterNames))
                        {
                            current.VoterNames = username;
                        }
                        else
                        {
                            current.VoterNames = current.VoterNames + "\r\n" + username;
                        }
                        current.Votes++;
                        current.Update();
                    }
                }
            }
            return 0;
        }
        #endregion

        #region 判断用户名是否能够投票
        public static bool AllowVote(int tid, string username)
        {
            if (Utils.StrIsNullOrEmpty(username)) return false;

            string pollUserNameList = FindByTid(tid).Voternames;
            if (Utils.StrIsNullOrEmpty(pollUserNameList)) return true;

            string[] array = Utils.SplitString(pollUserNameList.Trim(), "\r\n");
            for (int i = 0; i < array.Length; i++)
            {
                string text = array[i];
                if (text.Trim() == username)
                {
                    return false;
                }
            }
            return true;
        }
        #endregion

        #endregion
    }
}