using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using BBX.Common;
using BBX.Entity;
using BBX.Forum;
using NewLife.Xml;
using System.Linq;
using XUser = BBX.Entity.User;

namespace BBX.Web
{
    public class stats : PageBase
    {
        public Hashtable totalstats = new Hashtable();
        public Hashtable osstats = new Hashtable();
        public Hashtable browserstats = new Hashtable();
        public Hashtable monthstats = new Hashtable();
        public Hashtable weekstats = new Hashtable();
        public Hashtable hourstats = new Hashtable();
        public Dictionary<String, String> mainstats = new Dictionary<String, String>();
        public Hashtable daypostsstats = new Hashtable();
        public Hashtable monthpostsstats = new Hashtable();
        public Hashtable forumsrankstats = new Hashtable();
        public Hashtable onlinesstats = new Hashtable();
        public Hashtable postsrankstats = new Hashtable();
        public Hashtable teamstats = new Hashtable();
        public Hashtable creditsrankstats = new Hashtable();
        public Hashtable tradestats = new Hashtable();
        public string lastupdate = "";
        public string nextupdate = "";
        public string type = "";
        public string statuspara = "";
        public string primarybegin = (String.IsNullOrEmpty(DNTRequest.GetString("primarybegin"))) ? DateTime.Now.AddMonths(-1).AddDays(1.0).ToString("yyyy-MM-dd") : DNTRequest.GetString("primarybegin");
        public string primaryend = (String.IsNullOrEmpty(DNTRequest.GetString("primaryend"))) ? DateTime.Now.ToString("yyyy-MM-dd") : DNTRequest.GetString("primaryend");
        public int members;
        public int mempost;
        public string admins;
        public int memnonpost;
        public string lastmember;
        public double mempostpercent;
        public string bestmem;
        public int bestmemposts;
        public int forums;
        public double mempostavg;
        public double postsaddavg;
        public double membersaddavg;
        public double topicreplyavg;
        public double pageviewavg;
        public IXForum hotforum;
        public int topics;
        public int posts;
        public string postsaddtoday;
        public string membersaddtoday;
        public string activeindex;
        public bool statstatus;
        public string monthpostsofstatsbar = "";
        public string daypostsofstatsbar = "";
        public string monthofstatsbar = "";
        public int runtime;
        public string weekofstatsbar = string.Empty;
        public string hourofstatsbar = string.Empty;
        public string browserofstatsbar = string.Empty;
        public string osofstatsbar = string.Empty;
        public string hotreplytopics;
        public string hottopics;
        public string postsrank;
        public string digestpostsrank;
        public string thismonthpostsrank;
        public string todaypostsrank;
        public string topicsforumsrank;
        public string postsforumsrank;
        public string thismonthforumsrank;
        public string todayforumsrank;
        public string[] score;
        public string creditsrank;
        public string extcreditsrank1;
        public string extcreditsrank2;
        public string extcreditsrank3;
        public string extcreditsrank4;
        public string extcreditsrank5;
        public string extcreditsrank6;
        public string extcreditsrank7;
        public string extcreditsrank8;
        public string totalonlinerank;
        public string thismonthonlinerank;
        public int maxos;
        public int maxbrowser;
        public int maxmonth;
        public int yearofmaxmonth;
        public int monthofmaxmonth;
        public int maxweek;
        public string dayofmaxweek;
        public int maxhour;
        public int maxhourfrom;
        public int maxhourto;
        public int maxmonthposts;
        public int maxdayposts;
        public int statscachelife = 120;
        private Dictionary<string, string> statvars = new Dictionary<string, string>();
        public bool needlogin;

        protected override void ShowPage()
        {
            this.pagetitle = "统计";
            if (!this.usergroupinfo.AllowViewstats && (!(DNTRequest.GetString("type") == "trend") || !(DNTRequest.GetString("xml") == "1")))
            {
                base.AddErrLine("您所在的用户组 ( <b>" + this.usergroupinfo.GroupTitle + "</b> ) 没有查看统计信息的权限");
                this.needlogin = (this.userid < 1);
                return;
            }
            this.statscachelife = ((this.statscachelife <= 0) ? this.statscachelife : this.config.Statscachelife);

            var allStats = Stat.GetAll();
            this.statstatus = this.config.Statstatus;
            this.totalstats["hits"] = 0;
            this.totalstats["maxmonth"] = 0;
            this.totalstats["guests"] = 0;
            this.totalstats["visitors"] = 0;
            foreach (Stat item in allStats)
            {
                string a;
                if ((a = item.Type) != null)
                {
                    if (!(a == "total"))
                    {
                        if (!(a == "os"))
                        {
                            if (!(a == "browser"))
                            {
                                if (!(a == "month"))
                                {
                                    if (!(a == "week"))
                                    {
                                        if (a == "hour")
                                        {
                                            this.SetValue(item, this.hourstats);
                                            if (item.Count > this.maxhour)
                                            {
                                                this.maxhour = item.Count;
                                                this.maxhourfrom = item.Variable.ToInt(0);
                                                this.maxhourto = this.maxhourfrom + 1;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        this.SetValue(item, this.weekstats);
                                        if (item.Count > this.maxweek)
                                        {
                                            this.maxweek = item.Count;
                                            this.dayofmaxweek = item.Variable;
                                        }
                                    }
                                }
                                else
                                {
                                    this.SetValue(item, this.monthstats);
                                    if (item.Count > this.maxmonth)
                                    {
                                        this.maxmonth = item.Count;
                                        this.yearofmaxmonth = item.Variable.ToInt(0) / 100;
                                        this.monthofmaxmonth = item.Variable.ToInt(0) - this.yearofmaxmonth * 100;
                                    }
                                }
                            }
                            else
                            {
                                this.SetValue(item, this.browserstats);
                                this.maxbrowser = ((item.Count > this.maxbrowser) ? item.Count : this.maxbrowser);
                            }
                        }
                        else
                        {
                            this.SetValue(item, this.osstats);
                            this.maxos = ((item.Count > this.maxos) ? item.Count : this.maxos);
                        }
                    }
                    else
                    {
                        this.SetValue(item, this.totalstats);
                    }
                }
            }
            //List<StatVarInfo> allStatVars = Stats.GetAllStatVars();
            //var allStatVars = StatVar.GetAll();
            foreach (var item in StatVar.GetAll())
            {
                if ((item.Variable != "lastupdate" || !Utils.IsNumeric(item.Value)) && item.Type != null)
                {
                    switch (item.Type)
                    {
                        case "dayposts":
                            this.SetValue(item, this.daypostsstats);
                            break;

                        case "creditsrank":
                            this.SetValue(item, this.creditsrankstats);
                            break;

                        case "forumsrank":
                            this.SetValue(item, this.forumsrankstats);
                            break;

                        case "postsrank":
                            this.SetValue(item, this.postsrankstats);
                            break;

                        case "main":
                            //this.SetValue(item, this.mainstats);
                            mainstats[item.Variable] = item.Value;
                            break;

                        case "monthposts":
                            this.SetValue(item, this.monthpostsstats);
                            break;

                        case "onlines":
                            this.SetValue(item, this.onlinesstats);
                            break;

                        case "team":
                            this.SetValue(item, this.teamstats);
                            break;

                        case "trade":
                            this.SetValue(item, this.tradestats);
                            break;
                    }
                }
            }
            this.type = DNTRequest.GetString("type");
            if ((String.IsNullOrEmpty(this.type) && !this.statstatus) || this.type == "posts")
            {
                StatVar.DeleteOldDayposts();
                this.monthpostsstats = Stats.GetMonthPostsStats(this.monthpostsstats);
                this.maxmonthposts = (int)this.monthpostsstats["maxcount"];
                this.monthpostsstats.Remove("maxcount");
                this.daypostsstats = Stats.GetDayPostsStats(this.daypostsstats);
                this.maxdayposts = (int)this.daypostsstats["maxcount"];
                this.daypostsstats.Remove("maxcount");
            }
            string key2;
            switch (key2 = this.type)
            {
                case "views":
                    this.GetViews();
                    return;

                case "client":
                    this.GetClient();
                    return;

                case "posts":
                    this.GetPosts();
                    return;

                case "forumsrank":
                    this.GetForumsRank();
                    return;

                case "topicsrank":
                    this.GetTopicsRank();
                    return;

                case "postsrank":
                    this.GetPostsRank();
                    return;

                case "creditsrank":
                    this.GetCreditsRank();
                    return;

                case "trade":
                    this.GetTrade();
                    return;

                case "onlinetime":
                    this.GetOnlinetime();
                    return;

                case "team":
                    this.GetTeam();
                    return;

                case "modworks":
                    this.GetModWorks();
                    return;

                case "trend":
                    this.GetTrend();
                    return;

                case "":
                    this.Default();
                    return;
            }
            base.AddErrLine("未定义操作请返回");
            base.SetShowBackLink(false);
        }

        private void GetTrend()
        {
            string text = Utils.MD5(this.config.Passwordkey + "\t" + DateTime.Now.Day);
            if (text != DNTRequest.GetString("hash") && DNTRequest.GetString("xml") != "")
            {
                base.AddErrLine("参数验证不正确");
                return;
            }
            if (!Utils.IsDateString(this.primarybegin))
            {
                base.AddErrLine("起始日期格式不正确");
                return;
            }
            if (!Utils.IsDateString(this.primaryend))
            {
                base.AddErrLine("结束日期格式不正确");
                return;
            }
            string text2 = (String.IsNullOrEmpty(DNTRequest.GetString("types"))) ? "all" : DNTRequest.GetString("types");
            string text3 = (text2 == "all") ? "login,register,topic,poll,bonus,debate,post" : text2;
            string[] array = text3.Split(',');
            for (int i = 0; i < array.Length; i++)
            {
                string str = array[i];
                if (!Utils.InArray(str, "all,login,register,topic,poll,bonus,debate,post"))
                {
                    base.AddErrLine("参数验证不正确");
                    return;
                }
            }
            DateTime t = this.primarybegin.ToDateTime(DateTime.Now.AddMonths(-1).AddDays(1.0));
            DateTime t2 = this.primaryend.ToDateTime();
            if (t > t2)
            {
                base.AddErrLine("统计开始日期不能小于结束日期");
                return;
            }
            this.primarybegin = t.ToString("yyyy-MM-dd");
            this.primaryend = t2.ToString("yyyy-MM-dd");
            if (DNTRequest.GetString("xml") != "")
            {
                Hashtable hashtable = new Hashtable();
                string field = "*";
                if (DNTRequest.GetString("merge") == "1")
                {
                    field = "[daytime],[" + text3.Replace(",", "]+[") + "] AS [statistic]";
                    text3 = "statistic";
                }
                Hashtable hashtable2 = new Hashtable();
                hashtable2.Add("login", "登录用户");
                hashtable2.Add("register", "新注册用户");
                hashtable2.Add("topic", "主题");
                hashtable2.Add("poll", "投票");
                hashtable2.Add("bonus", "悬赏");
                hashtable2.Add("debate", "辩论");
                hashtable2.Add("post", "主题回帖");
                string trendGraph = Stats.GetTrendGraph(hashtable, field, t.ToString("yyyyMMdd"), t2.ToString("yyyyMMdd"), text3);
                StringBuilder stringBuilder = new StringBuilder(2048);
                stringBuilder.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                stringBuilder.AppendFormat("<chart><xaxis>{0}</xaxis><graphs>", trendGraph);
                int num = 0;
                foreach (string key in hashtable.Keys)
                {
                    stringBuilder.AppendFormat("<graph gid=\"{0}\" title=\"{1}\">{2}</graph>", num, hashtable2[key], hashtable[key]);
                    num++;
                }
                stringBuilder.Append("</graphs></chart>");
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Expires = -1;
                HttpContext.Current.Response.ContentType = "application/xml";
                HttpContext.Current.Response.Write(stringBuilder.ToString());
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                return;
            }
            this.statuspara = "path=&settings_file=config/stat_setting.xml&data_file=" + Utils.UrlEncode("stats.aspx?type=trend&xml=1&types=" + text2 + "&primarybegin=" + this.primarybegin + "&primaryend=" + this.primaryend + "&merge=" + DNTRequest.GetInt("merge", 0) + "&hash=" + text);
        }

        private void SetValue(Stat stat, Hashtable ht)
        {
            ht[stat.Variable] = stat.Count;
        }

        private void SetValue(StatVar statvar, Hashtable ht)
        {
            ht[statvar.Variable] = statvar.Value;
        }

        void CheckLastUpdate(String type)
        {
            if (!this.statvars.ContainsKey("lastupdate") || (DateTime.Now - statvars["lastupdate"].ToDateTime()).TotalMinutes > (double)this.statscachelife)
            {
                var now = DateTime.Now.ToFullString();
                this.statvars.Clear();
                this.statvars["lastupdate"] = now;
                StatVar.Update(type, "lastupdate", now);
            }
        }

        private void Default()
        {
            this.lastmember = Statistic.Current.LastUserName;
            foreach (var item in mainstats)
            {
                statvars[item.Key] = item.Value;
            }
            CheckLastUpdate("main");
            this.forums = XForum.GetForumCount();
            var st = Statistic.Current;
            this.topics = st.TotalTopic;
            this.posts = st.TotalPost;
            this.members = st.TotalUsers;
            if (this.statvars.ContainsKey("runtime"))
            {
                this.runtime = this.statvars["runtime"].ToInt(0);
            }
            else
            {
                this.runtime = (DateTime.Now - Convert.ToDateTime(this.monthpostsstats["starttime"])).Days;
                StatVar.Update("main", "runtime", this.runtime);
            }
            if (this.statvars.ContainsKey("postsaddtoday"))
            {
                this.postsaddtoday = this.statvars["postsaddtoday"];
            }
            else
            {
                this.postsaddtoday = Post.GetTodayPostCount(0).ToString();
                StatVar.Update("main", "postsaddtoday", this.postsaddtoday);
            }
            if (this.statvars.ContainsKey("membersaddtoday"))
            {
                this.membersaddtoday = this.statvars["membersaddtoday"];
            }
            else
            {
                this.membersaddtoday = BBX.Entity.User.GetTodayNewMemberCount().ToString();
                StatVar.Update("main", "membersaddtoday", this.membersaddtoday);
            }
            if (this.statvars.ContainsKey("admins"))
            {
                this.admins = this.statvars["admins"];
            }
            else
            {
                this.admins = BBX.Entity.User.GetAdminCount().ToString();
                StatVar.Update("main", "admins", this.admins);
            }
            if (this.statvars.ContainsKey("memnonpost"))
            {
                this.memnonpost = this.statvars["memnonpost"].ToInt(0);
            }
            else
            {
                this.memnonpost = BBX.Entity.User.GetNonPostMemCount();
                StatVar.Update("main", "memnonpost", this.memnonpost);
            }
            if (this.statvars.ContainsKey("hotforum"))
            {
                this.hotforum = XForum.FindByID(this.statvars["hotforum"].ToInt());
            }
            else
            {
                this.hotforum = Stats.GetHotForum();
                StatVar.Update("main", "hotforum", this.hotforum.ID + "");
            }
            if (this.statvars.ContainsKey("bestmem") && this.statvars.ContainsKey("bestmemposts"))
            {
                this.bestmem = this.statvars["bestmem"];
                this.bestmemposts = this.statvars["bestmemposts"].ToInt(0);
            }
            else
            {
                Post.GetBestMember(out this.bestmem, out this.bestmemposts);
                StatVar.Update("main", "bestmem", this.bestmem);
                StatVar.Update("main", "bestmemposts", this.bestmemposts);
            }
            this.mempost = this.members - this.memnonpost;
            this.mempostavg = Math.Round((double)this.posts / (double)this.members, 2);
            this.topicreplyavg = Math.Round((double)(this.posts - this.topics) / (double)this.topics, 2);
            this.mempostpercent = Math.Round((double)(this.mempost * 100) / (double)this.members, 2);
            this.postsaddavg = Math.Round((double)this.posts / (double)this.runtime, 2);
            this.membersaddavg = (double)(this.members / this.runtime);
            int num = totalstats["members"].ToInt() + totalstats["guests"].ToInt();
            this.totalstats["visitors"] = num;
            this.pageviewavg = Math.Round((double)totalstats["hits"].ToInt() / (double)((num == 0) ? 1 : num), 2);
            this.activeindex = ((Math.Round(this.membersaddavg / (double)((this.members == 0) ? 1 : this.members), 2) + Math.Round(this.postsaddavg / (double)((this.posts == 0) ? 1 : this.posts), 2)) * 1500.0 + this.topicreplyavg * 10.0 + this.mempostavg + Math.Round(this.mempostpercent / 10.0, 2) + this.pageviewavg).ToString();
            if (this.statstatus)
            {
                this.monthofstatsbar = Stats.GetStatsDataHtml("month", this.monthstats, this.maxmonth);
            }
            else
            {
                this.monthpostsofstatsbar = Stats.GetStatsDataHtml("monthposts", this.monthpostsstats, this.maxmonthposts);
                this.daypostsofstatsbar = Stats.GetStatsDataHtml("dayposts", this.daypostsstats, this.maxdayposts);
            }
            this.lastupdate = this.statvars["lastupdate"];
            this.nextupdate = Utility.ToDateTime(this.statvars["lastupdate"]).AddMinutes((double)this.statscachelife).ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void GetModWorks() { }

        private void GetTeam()
        {
            foreach (string key in this.teamstats.Keys)
            {
                this.statvars[key] = this.teamstats[key] + "";
            }
            CheckLastUpdate("team");
            this.lastupdate = this.statvars["lastupdate"];
            this.nextupdate = Utility.ToDateTime(this.statvars["lastupdate"]).AddMinutes((double)this.statscachelife).ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void GetOnlinetime()
        {
            if (this.config.Oltimespan == 0)
            {
                this.totalonlinerank = "<li>未开启在线时长统计</li>";
                this.thismonthonlinerank = "<li></li>";
                return;
            }
            foreach (string key in this.onlinesstats.Keys)
            {
                this.statvars[key] = this.onlinesstats[key] + "";
            }
            CheckLastUpdate("onlines");
            var total = Stats.GetUserOnline(statvars, "total");
            var thismonth = Stats.GetUserOnline(statvars, "thismonth");
            int maxrows = Math.Max(total.Length, thismonth.Length);
            this.totalonlinerank = Stats.GetUserRankHtml(total, "onlinetime", maxrows);
            this.thismonthonlinerank = Stats.GetUserRankHtml(thismonth, "onlinetime", maxrows);
            this.lastupdate = this.statvars["lastupdate"];
            this.nextupdate = Utility.ToDateTime(this.statvars["lastupdate"]).AddMinutes((double)this.statscachelife).ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void GetTrade() { }

        private void GetCreditsRank()
        {
            this.score = Scoresets.GetValidScoreName();
            foreach (string key in this.creditsrankstats.Keys)
            {
                this.statvars[key] = this.creditsrankstats[key] + "";
            }
            CheckLastUpdate("creditsrank");
            var credits = Stats.GetCreditsRank(statvars, "credits");
            var extendedcredits = Stats.GetExtCreditsRank(statvars, "extendedcredits");
            int num = credits.Length;
            for (int i = 1; i < 8; i++)
            {
                num = Math.Max(extendedcredits[i].Length, num);
            }
            this.creditsrank = Stats.GetUserRankHtml(credits, "credits", num);
            this.extcreditsrank1 = Stats.GetUserRankHtml(extendedcredits[0], "extcredits1", num);
            this.extcreditsrank2 = Stats.GetUserRankHtml(extendedcredits[1], "extcredits2", num);
            this.extcreditsrank3 = Stats.GetUserRankHtml(extendedcredits[2], "extcredits3", num);
            this.extcreditsrank4 = Stats.GetUserRankHtml(extendedcredits[3], "extcredits4", num);
            this.extcreditsrank5 = Stats.GetUserRankHtml(extendedcredits[4], "extcredits5", num);
            this.extcreditsrank6 = Stats.GetUserRankHtml(extendedcredits[5], "extcredits6", num);
            this.extcreditsrank7 = Stats.GetUserRankHtml(extendedcredits[6], "extcredits7", num);
            this.extcreditsrank8 = Stats.GetUserRankHtml(extendedcredits[7], "extcredits8", num);
            this.lastupdate = this.statvars["lastupdate"];
            this.nextupdate = Utility.ToDateTime(this.statvars["lastupdate"]).AddMinutes((double)this.statscachelife).ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void GetPostsRank()
        {
            foreach (string key in this.postsrankstats.Keys)
            {
                this.statvars[key] = this.postsrankstats[key] + "";
            }
            CheckLastUpdate("postsrank");
            XUser[] array;
            if (this.statvars.ContainsKey("posts"))
            {
                array = this.statvars["posts"].ToXmlEntity<XUser[]>();
            }
            else
            {
                array = Stats.GetUserArray("posts");
                StatVar.Update("postsrank", "posts", array.ToXml());
            }
            XUser[] array2;
            if (this.statvars.ContainsKey("digestposts"))
            {
                array2 = this.statvars["digestposts"].ToXmlEntity<XUser[]>();
            }
            else
            {
                array2 = Stats.GetUserArray("digestposts");
                StatVar.Update("postsrank", "digestposts", array2.ToXml());
            }
            XUser[] array3;
            if (this.statvars.ContainsKey("thismonth"))
            {
                array3 = this.statvars["thismonth"].ToXmlEntity<XUser[]>();
            }
            else
            {
                array3 = Stats.GetUserArray("thismonth");
                StatVar.Update("postsrank", "thismonth", array3.ToXml());
            }
            XUser[] array4;
            if (this.statvars.ContainsKey("today"))
            {
                array4 = this.statvars["today"].ToXmlEntity<XUser[]>();
            }
            else
            {
                array4 = Stats.GetUserArray("today");
                StatVar.Update("postsrank", "today", array4.ToXml());
            }
            int num = array.Length;
            num = Math.Max(array2.Length, num);
            num = Math.Max(array3.Length, num);
            num = Math.Max(array4.Length, num);
            this.postsrank = Stats.GetUserRankHtml(array, "posts", num);
            this.digestpostsrank = Stats.GetUserRankHtml(array2, "digestposts", num);
            this.thismonthpostsrank = Stats.GetUserRankHtml(array3, "thismonth", num);
            this.todaypostsrank = Stats.GetUserRankHtml(array4, "today", num);
            this.lastupdate = this.statvars["lastupdate"];
            this.nextupdate = Utility.ToDateTime(this.statvars["lastupdate"]).AddMinutes((double)this.statscachelife).ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void GetTopicsRank()
        {
            this.hottopics = Stats.GetHotTopicsHtml();
            this.hotreplytopics = Stats.GetHotReplyTopicsHtml();
        }

        private void GetForumsRank()
        {
            foreach (string key in this.forumsrankstats.Keys)
            {
                this.statvars[key] = this.forumsrankstats[key] + "";
            }
            CheckLastUpdate("forumsrank");

            var list = Stats.GetForumsRank(statvars, "topics");
            var list2 = Stats.GetForumsRank(statvars, "posts");
            var list3 = Stats.GetForumsRank(statvars, "thismonth");
            var list4 = Stats.GetForumsRank(statvars, "today");
            int num = list.Count;
            num = Math.Max(list2.Count, num);
            num = Math.Max(list3.Count, num);
            num = Math.Max(list4.Count, num);
            this.topicsforumsrank = Stats.GetForumsRankHtml(list, "topics", num);
            this.postsforumsrank = Stats.GetForumsRankHtml(list2, "posts", num);
            this.thismonthforumsrank = Stats.GetForumsRankHtml(list3, "thismonth", num);
            this.todayforumsrank = Stats.GetForumsRankHtml(list4, "today", num);
            this.lastupdate = this.statvars["lastupdate"];
            this.nextupdate = Utility.ToDateTime(this.statvars["lastupdate"]).AddMinutes((double)this.statscachelife).ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void GetPosts()
        {
            this.monthpostsofstatsbar = Stats.GetStatsDataHtml("monthposts", this.monthpostsstats, this.maxmonthposts);
            this.daypostsofstatsbar = Stats.GetStatsDataHtml("dayposts", this.daypostsstats, this.maxdayposts);
        }

        private void GetClient()
        {
            if (!this.statstatus) return;

            this.browserofstatsbar = Stats.GetStatsDataHtml("browser", this.browserstats, this.maxbrowser);
            this.osofstatsbar = Stats.GetStatsDataHtml("os", this.osstats, this.maxos);
        }

        private void GetViews()
        {
            if (!this.statstatus) return;

            this.weekofstatsbar = Stats.GetStatsDataHtml("week", this.weekstats, this.maxweek);
            this.hourofstatsbar = Stats.GetStatsDataHtml("hour", this.hourstats, this.maxhour);
        }

        public string IsChecked(string op)
        {
            if (op == "merge")
            {
                if (DNTRequest.GetString("merge") == "1") return " checked=\"checked\"";

                return "";
            }
            else
            {
                if (!DNTRequest.GetString("types").Contains(op)) return "";

                return " checked=\"checked\"";
            }
        }
    }
}