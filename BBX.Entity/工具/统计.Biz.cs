﻿using System;
using NewLife;
using XCode;

namespace BBX.Entity
{
    /// <summary>统计</summary>
    public partial class Statistic : Entity<Statistic>
    {
        #region 对象操作﻿
        static Statistic()
        {
            //AdditionalFields.Add(__.TotalTopic);
            //AdditionalFields.Add(__.TotalPost);
            //AdditionalFields.Add(__.TotalUsers);

            var fs = Meta.Factory.AdditionalFields;
            fs.Add(__.TotalTopic);
            fs.Add(__.TotalPost);
            fs.Add(__.TotalUsers);
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
        //    if (XTrace.Debug) XTrace.WriteLine("开始初始化{0}统计数据……", typeof(Statistic).Name);

        //    var entity = new Statistic();
        //    entity.TotalTopic = 0;
        //    entity.TotalPost = 0;
        //    entity.TotalUsers = 0;
        //    entity.LastUserName = "abc";
        //    entity.LastUserID = 0;
        //    entity.HighestonlineuserCount = 0;
        //    entity.HighestonlineuserTime = DateTime.Now;
        //    entity.Yesterdayposts = 0;
        //    entity.Highestposts = 0;
        //    entity.HighestpostsDate = "abc";
        //    entity.Insert();

        //    if (XTrace.Debug) XTrace.WriteLine("完成初始化{0}统计数据！", typeof(Statistic).Name);
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
        private static Statistic _Current;
        /// <summary>当前统计</summary>
        public static Statistic Current
        {
            get
            {
                if (_Current == null)
                {
                    var list = FindAll(null, _.ID.Desc(), null, 0, 1);
                    if (list.Count > 0)
                        _Current = list[0];
                    else
                    {
                        _Current = new Statistic();
                        _Current.Save();
                    }
                }
                return _Current;
            }
        }
        #endregion

        #region 扩展查询﻿
        #endregion

        #region 高级查询
        #endregion

        #region 扩展操作
        #endregion

        #region 业务
        public static int UpdateStatisticsLastUserName(int uid, string newUserName)
        {
            //return DatabaseProvider.GetInstance().UpdateStatisticsLastUserName(uid, newUserName);
            var st = Find(_.LastUserID == uid);
            //if (st == null) return 0;
            if (st == null) st = Current;

            st.LastUserID = uid;
            st.LastUserName = newUserName;
            return st.Update();
        }

        public static void Reset()
        {
            var user = User.FindLast();

            var st = Current;
            st.LastUserID = user.ID;
            st.LastUserName = user.Name;
            st.TotalUsers = User.Meta.Count;

            st.TotalTopic = Topic.Meta.Count;
            st.TotalPost = Post.Meta.Count;

            st.Update();
        }

        public static void UpdateYesterdayPosts()
        {
            var count = Post.FindCountForYesterday();
            StatVar.Update("dayposts", DateTime.Now.ToString("yyyyMMdd"), count);
            var st = Current;
            st.YesterdayPosts = count;
            if (count > Statistic.Current.HighestPosts)
            {
                st.HighestPosts = count;
                st.HighestPostsDate = DateTime.Now.Date.AddDays(-1).ToFullString();
            }
            st.Update();
        }

        static Int32 _count;
        static DateTime _next;
        public static bool CheckSearchCount(int maxspm)
        {
            if (_next < DateTime.Now)
            {
                _count = 0;
                _next = DateTime.Now;
            }
            if (_count > maxspm) return false;

            _count++;
            return true;
        }
        #endregion
    }
}