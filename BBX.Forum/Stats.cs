using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BBX.Common;
using BBX.Entity;

namespace BBX.Forum
{
    public class Stats
    {
        //public static int GetForumCount()
        //{
        //    return BBX.Data.Stats.GetForumCount();
        //}
        //public static int GetTopicCount() { return Statistic.Current.TotalTopic; }
        //public static int GetPostCount() { return Statistic.Current.TotalPost; }
        //public static int GetMemberCount() { return Statistic.Current.TotalUsers; }
        //public static int GetTodayPostCount()
        //{
        //    return BBX.Data.Stats.GetTodayPostCount(TableList.GetPostTableId());
        //}
        //public static int GetTodayNewMemberCount()
        //{
        //    return BBX.Data.Stats.GetTodayNewMemberCount();
        //}
        //public static int GetAdminCount()
        //{
        //    return BBX.Data.Stats.GetAdminCount();
        //}
        //public static int GetNonPostMemCount()
        //{
        //    return BBX.Data.Stats.GetNonPostMemCount();
        //}
        public static IXForum GetHotForum()
        {
            IXForum result = null;
            int num = 0;
            foreach (var item in Forums.GetForumList())
            {
                if (item.Layer > 0 && item.Visible && item.Posts > num)
                {
                    num = item.Posts;
                    result = item;
                }
            }
            if (num > 0) return result;

            foreach (var item in Forums.GetForumList())
            {
                if (item.Layer > 0 && item.Visible) return item;

            }
            return null;
        }
        //public static void GetBestMember(out string bestmem, out int bestmemposts)
        //{
        //    BBX.Data.Stats.GetBestMember(out bestmem, out bestmemposts, TableList.GetPostTableId());
        //}
        public static Hashtable GetMonthPostsStats(Hashtable monthpostsstats)
        {
            //return BBX.Data.Stats.GetMonthPostsStats(monthpostsstats, TableList.GetPostTableId());
            //var dic = Post.GetMonthPostsStats();
            // 不要啥啥的重复计算，直接从统计表拿数据即可
            var list = StatVar.FindAllByType("monthposts");
            var max = 0;
            foreach (var item in list)
            {
                //StatVar.Update("monthposts", item.Key, item.Value);

                var v = item.IntValue;
                if (v > max) max = v;
                monthpostsstats[item.Variable] = v;
            }
            monthpostsstats["maxcount"] = max;
            return monthpostsstats;
        }
        public static Hashtable GetDayPostsStats(Hashtable daypostsstats)
        {
            //return BBX.Data.Stats.GetDayPostsStats(daypostsstats, TableList.GetPostTableId());
            //var dic = Post.GetDayPostsStats();
            // 不要啥啥的重复计算，直接从统计表拿数据即可
            var list = StatVar.FindAllByType("dayposts");
            var max = 0;
            var start = DateTime.Now.Date.AddDays(-30).ToString("yyyyMMdd").ToInt();
            foreach (var item in list)
            {
                //StatVar.Update("dayposts", item.Key, item.Value);

                var v = item.IntValue;
                if (v >= start)
                {
                    if (v > max) max = v;
                    daypostsstats[item.Variable] = v;
                }
            }
            //ArrayList arrayList = new ArrayList(daypostsstats.Values);
            //arrayList.Sort(new StatVarSorter());
            //daypostsstats["maxcount"] = ((arrayList.Count < 1) ? 0 : Utils.StrToInt(arrayList[arrayList.Count - 1], 0));
            daypostsstats["maxcount"] = max;
            return daypostsstats;
        }
        public static string GetHotTopicsHtml()
        {
            var sb = new StringBuilder();
            foreach (var item in Topic.GetHotTopics(20))
            {
                sb.AppendFormat("<li><em>{0}</em><a href=\"{1}\">{2}</a>\r\n", item.Views, Urls.ShowTopicAspxRewrite(item.ID, 0), item.Title);
            }
            return sb.ToString();
        }
        public static string GetHotReplyTopicsHtml()
        {
            var sb = new StringBuilder();
            foreach (var item in Topic.GetHotReplyTopics(20))
            {
                sb.AppendFormat("<li><em>{0}</em><a href=\"{1}\">{2}</a>\r\n", item.Replies, Urls.ShowTopicAspxRewrite(item.ID, 0), item.Title);
            }
            return sb.ToString();
        }

        public static List<XForum> GetForumArray(string type)
        {
            //return BBX.Data.Stats.GetForumArray(type, TableList.GetPostTableId());

            switch ((type + "").ToLower())
            {
                case "today":
                    return XForum.GetForumsByDayPostCount(20);
                case "thismonth":
                    return XForum.GetForumsByMonthPostCount(20);
                case "posts":
                    return XForum.GetForumsByPostCount(20);
                case "topics":
                default:
                    return XForum.GetForumsByTopicCount(20);
            }
        }

        public static List<XForum> GetForumsRank(Dictionary<String, String> dic, String name)
        {
            if (dic.ContainsKey(name)) return XForum.FindAllByIDs(dic[name]);

            var list = GetForumArray(name);
            StatVar.Update("forumsrank", name, list.Select(e => e.ID).Join());
            return list;
        }

        public static User[] GetUserArray(string type)
        {
            //return BBX.Data.Stats.GetUserArray(type, TableList.GetPostTableId());

            var list = User.GetUsersRank(20, 0, type);

            return list == null ? new User[0] : list.ToArray();
        }

        public static User[] GetUserOnline(Dictionary<String, String> dic, String name)
        {
            if (dic.ContainsKey(name))
            {
                var ss = dic[name].SplitAsDictionary("|");

                var us = User.FindAllByIDs(ss.Keys.Join(",")).ToArray();
                // 附上时间
                foreach (var item in us)
                {
                    item["OnlineTime"] = ss[item.ID + ""].ToInt();
                }
                return us;
            }

            var list = OnlineTime.GetUserOnlinetime(name);
            StatVar.Update("onlines", name, list.Join(",", e => "{0}|{1}".F(e.ID, e["OnlineTime"])));
            return list;
        }

        public static User[][] GetExtsRankUserArray()
        {
            List<User[]> list = new List<User[]>();
            string[] validScoreName = Scoresets.GetValidScoreName();
            for (int i = 1; i < 9; i++)
            {
                if (validScoreName[i] == string.Empty)
                {
                    list.Add(new User[0]);
                }
                else
                {
                    list.Add(Stats.GetUserArray("extcredits" + i.ToString()));
                }
            }
            return list.ToArray();
        }

        public static User[] GetCreditsRank(Dictionary<String, String> dic, String name)
        {
            if (dic.ContainsKey(name)) return User.FindAllByIDs(dic[name]).ToArray();

            var list = GetUserArray(name);
            StatVar.Update("creditsrank", name, list.Select(e => e.ID).Join());
            return list;
        }

        public static User[][] GetExtCreditsRank(Dictionary<String, String> dic, String name)
        {
            if (dic.ContainsKey(name))
            {
                if (dic[name].IsNullOrWhiteSpace()) return new User[0][];

                // 一次性查出来所有用户
                var ids = new List<Int32>();
                foreach (var item in dic[name].Split(","))
                {
                    var ds = item.SplitAsInt("|");
                    if (ds != null && ds.Length > 0) ids.AddRange(ds);
                }
                var us = User.FindAllByIDs(ids.ToArray());

                // 开始分组用户
                var list = new List<User[]>();
                foreach (var item in dic[name].Split(","))
                {
                    var list2 = new List<User>();
                    foreach (var elm in item.SplitAsInt("|"))
                    {
                        list2.Add(us.Find(User._.ID, elm));
                    }
                    list.Add(list2.ToArray());
                }
                return list.ToArray();
            }
            else
            {
                var list = GetExtsRankUserArray();
                var sb = new StringBuilder();
                foreach (var item in list)
                {
                    sb.Separate(",");
                    sb.Append(item.Select(e => e.ID).Join("|"));
                }
                StatVar.Update("creditsrank", name, sb.ToString());
                return list;
            }
        }

        //public static IUser[] GetUserOnlinetime(string field)
        //{
        //    return BBX.Data.Stats.GetUserOnlinetime(field);
        //}
        public static string GetTrendGraph(Hashtable graph, string field, string begin, string end, string type)
        {
            var sb = new StringBuilder(2048);
            //IDataReader trendGraph = DatabaseProvider.GetInstance().GetTrendGraph(field, begin, end);
            var list = TrendStat.Search(begin.ToDateTime(), end.ToDateTime());
            int num = 1;
            //while (trendGraph.Read())
            foreach (var item in list)
            {
                sb.AppendFormat("<value xid=\"{0}\">{1}</value>", num, item.DayTime.ToString("MMdd"));
                if (type == "statistic")
                {
                    if (graph.ContainsKey(type))
                        graph[type] += string.Format("<value xid=\"{0}\">{1}</value>", num, item[type]);
                    else
                        graph.Add(type, string.Format("<value xid=\"{0}\">{1}</value>", num, item[type]));
                }
                else
                {
                    string[] array = type.Split(',');
                    for (int i = 0; i < array.Length; i++)
                    {
                        string text = array[i];
                        if (graph.ContainsKey(text))
                            graph[text] += string.Format("<value xid=\"{0}\">{1}</value>", num, item[text]);
                        else
                            graph.Add(text, string.Format("<value xid=\"{0}\">{1}</value>", num, item[text]));
                    }
                }
                num++;
            }
            return sb.ToString();
        }

        public static string GetStatsDataHtml(string type, Hashtable statht, int max)
        {
            var dic = new Dictionary<String, Int32>();
            foreach (DictionaryEntry item in statht)
            {
                dic.Add(item.Key + "", item.Value.ToInt());
            }

            var list = new List<String>();
            if (type == "os" || type == "browser")
            {
                foreach (var item in dic.OrderByDescending(e => e.Value))
                {
                    list.Add(item.Key);
                }
                //ArrayList arrayList = new ArrayList(statht);
                //arrayList.Sort(new BBX.Data.Stats.StatSorter());
                //arrayList.Reverse();
                //list = new ArrayList();
                //IEnumerator enumerator = arrayList.GetEnumerator();
                //try
                //{
                //    while (enumerator.MoveNext())
                //    {
                //        DictionaryEntry dictionaryEntry = (DictionaryEntry)enumerator.Current;
                //        list.Add(dictionaryEntry.Key);
                //    }
                //    goto IL_98;
                //}
                //finally
                //{
                //    IDisposable disposable = enumerator as IDisposable;
                //    if (disposable != null)
                //    {
                //        disposable.Dispose();
                //    }
                //}
            }
            else
            {
                foreach (DictionaryEntry item in statht)
                {
                    list.Add(item.Key + "");
                }
                list.Sort();
            }
            var total = 0;
            foreach (var key in list)
            {
                total += Utils.StrToInt(statht[key], 0);
            }
            var sb = new StringBuilder();
            foreach (var key in list)
            {
                var txt = "";
                switch (type)
                {
                    case "week":
                        #region 周
                        if (key == null) continue;
                        switch (key)
                        {
                            case "0":
                                txt = "星期日";
                                break;
                            case "1":
                                txt = "星期一";
                                break;
                            case "2":
                                txt = "星期二";
                                break;
                            case "3":
                                txt = "星期三";
                                break;
                            case "4":
                                txt = "星期四";
                                break;
                            case "5":
                                txt = "星期五";
                                break;
                            case "6":
                                txt = "星期六";
                                break;
                            default:
                                continue;
                        }
                        #endregion
                        break;
                    case "dayposts":
                        txt = key.Substring(0, 4) + "-" + key.Substring(4, 2) + "-" + key.Substring(6);
                        break;
                    case "month":
                    case "monthposts":
                        if (key.Length >= 4)
                        {
                            txt = key.Substring(0, 4) + "-" + key.Substring(4);
                        }
                        break;
                    case "hour":
                        txt = key;
                        break;
                    default:
                        txt = "<img src='images/stats/" + key.Replace("/", "") + ".gif ' border='0' alt='" + key + "' title='" + key + "' />&nbsp;" + key;
                        break;
                }
                var count = dic[key];
                var width = (int)(370.0 * ((max == 0) ? 0.0 : ((double)count / (double)max)));
                double num5 = Math.Round((double)count * 100.0 / (double)((total == 0) ? 1 : total), 2);
                if (width <= 0) width = 2;

                txt = ((count == max) ? ("<strong>" + txt + "</strong>") : txt);
                string text4 = "<div class='optionbar left'><div class='pollcolor5' style='width: " + width + "px'>&nbsp;</div></div>&nbsp;<strong>" + count + "</strong> (" + num5 + "%)";
                sb.Append("<tr><th width=\"100\">" + txt + "</th><td style='text-align:left'>" + text4 + "</td></tr>\r\n");
            }
            return sb.ToString();
        }
        public static string GetForumsRankHtml(List<XForum> forums, string type, int maxrows)
        {
            var sb = new StringBuilder();
            foreach (IXForum current in forums)
            {
                sb.AppendFormat("<li><em>{0}</em><a href=\"{1}\" target=\"_blank\">{2}</a></li>", (type == "topics") ? current.Topics : current.Posts, Urls.ShowForumAspxRewrite(current.Fid, 0), current.Name);
                maxrows--;
            }
            for (int i = 0; i < maxrows; i++)
            {
                sb.Append("<li>&nbsp;</li>");
            }
            return sb.ToString();
        }
        public static string GetUserRankHtml(IUser[] users, string type, int maxrows)
        {
            var sb = new StringBuilder();
            string text = "";
            int num = maxrows;
            //int i = 0;
            //while (i < users.Length)
            for (int i = 0; i < users.Length; i++, num--)
            {
                var user = users[i];
                if (user == null) continue;

                string str = string.Empty;
                if (type != null)
                {
                    switch (type)
                    {
                        case "credits":
                            str = user.Credits + "";
                            break;
                        case "extcredits1":
                            str = user.ExtCredits1 + "";
                            text = Scoresets.GetValidScoreUnit()[1];
                            break;
                        case "extcredits2":
                            str = user.ExtCredits2 + "";
                            text = Scoresets.GetValidScoreUnit()[2];
                            break;
                        case "extcredits3":
                            str = user.ExtCredits3 + "";
                            text = Scoresets.GetValidScoreUnit()[3];
                            break;
                        case "extcredits4":
                            str = user.ExtCredits4 + "";
                            text = Scoresets.GetValidScoreUnit()[4];
                            break;
                        case "extcredits5":
                            str = user.ExtCredits5 + "";
                            text = Scoresets.GetValidScoreUnit()[5];
                            break;
                        case "extcredits6":
                            str = user.ExtCredits6 + "";
                            text = Scoresets.GetValidScoreUnit()[6];
                            break;
                        case "extcredits7":
                            str = user.ExtCredits7 + "";
                            text = Scoresets.GetValidScoreUnit()[7];
                            break;
                        case "extcredits8":
                            str = user.ExtCredits8 + "";
                            text = Scoresets.GetValidScoreUnit()[8];
                            break;
                        case "digestposts":
                            str = user.DigestPosts + "";
                            break;
                        case "onlinetime":
                            str = Math.Round(user["OnlineTime"].ToDouble() / 60.0, 2).ToString();
                            text = "小时";
                            break;
                        default:
                            //goto IL_259;
                            break;
                    }
                }
                //IL_259:
                str = user.Posts.ToString();
                //    goto IL_26A;
                //IL_26A:
                sb.AppendFormat("<li><em>{0}</em><a href=\"{1}\" target=\"_blank\">{2}</a></li>", str + ((text == string.Empty) ? string.Empty : (" " + text)), Urls.UserInfoAspxRewrite(user.ID), user.Name);
                //num--;
                //i++;
                //continue;
            }
            for (int j = 0; j < num; j++)
            {
                sb.Append("<li>&nbsp;</li>");
            }
            return sb.ToString();
        }
    }
}