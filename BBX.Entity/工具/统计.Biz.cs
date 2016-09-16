using System;
using System.Threading.Tasks;
using NewLife;
using NewLife.Threading;
using XCode;

namespace BBX.Entity
{
    /// <summary>统计</summary>
    public partial class Statistic : Entity<Statistic>
    {
        #region 对象操作﻿
        static Statistic()
        {
            var fs = Meta.Factory.AdditionalFields;
            fs.Add(__.TotalTopic);
            fs.Add(__.TotalPost);
            fs.Add(__.TotalUsers);

            new TimerX(Execute, null, 5000, 3600 * 1000);
        }
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

        static void Execute(Object state)
        {
            Statistic.UpdateYesterdayPosts();
            Statistic.Reset();
            if (DateTime.Today.Day == 1)
            {
                OnlineTime.ResetThismonthOnlineTime();
                StatVar.Update("onlines", "lastupdate", "0");
            }
        }
        #endregion
    }
}