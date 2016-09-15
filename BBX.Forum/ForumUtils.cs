using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using BBX.Cache;
using BBX.Common;
using BBX.Config;
using BBX.Entity;

namespace BBX.Forum
{
    public class ForumUtils
    {
        private static string[] verifycodeRange;
        private static Random verifycodeRandom;
        private static Regex r_word;
        private static RegexOptions options;
        public static Regex[] r;

        static ForumUtils()
        {
            verifycodeRange = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f", "g", "h", "j", "k", "m", "n", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y" };
            verifycodeRandom = new Random((Int32)DateTime.Now.Ticks);
            options = RegexOptions.IgnoreCase;
            r = new Regex[5];
            //mcci = MemCachedConfigs.GetConfig();
            //rci = RedisConfigs.GetConfig();
            r[0] = new Regex("(\\r\\n)", options);
            r[1] = new Regex("(\\n)", options);
            r[2] = new Regex("(\\r)", options);
            r[3] = new Regex("(<br( *)(/?)>)", options);
            r[4] = new Regex("(</p>)", options);
            string verifycode = GeneralConfigInfo.Current.Verifycode;
            if (!verifycode.IsNullOrWhiteSpace())
            {
                char[] array = verifycode.ToCharArray();
                var list = new List<string>();
                char[] array2 = array;
                for (int i = 0; i < array2.Length; i++)
                {
                    char c = array2[i];
                    list.Add(c.ToString());
                }
                verifycodeRange = list.ToArray();
            }
        }

        public static string GetCookiePassword(string password, string key)
        {
            return DES.Decode(password, key);
        }

        public static string SetCookiePassword(string password, string key)
        {
            return DES.Encode(password, key);
        }

        public static string GetUserSecques(int questionid, string answer)
        {
            if (questionid > 0)
            {
                return Utils.MD5(answer + Utils.MD5(questionid.ToString())).Substring(15, 8);
            }
            return "";
        }

        public static void WriteCookie(string strName, string strValue)
        {
            var cookie = HttpContext.Current.Request.Cookies["bbx"];
            if (cookie == null)
            {
                cookie = new HttpCookie("bbx");
                cookie[strName] = Utils.UrlEncode(strValue);
            }
            else
            {
                cookie[strName] = Utils.UrlEncode(strValue);
                int num = cookie["expires"].ToInt();
                if (num > 0)
                {
                    cookie.Expires = DateTime.Now.AddMinutes(num);
                }
            }
            string domain = GeneralConfigInfo.Current.CookieDomain;
            if (domain != null && HttpContext.Current.Request.Url.Host.IndexOf(domain.TrimStart('.')) > -1 && IsValidDomain(HttpContext.Current.Request.Url.Host))
            {
                cookie.Domain = domain;
            }
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        public static void WriteUserCookie(int uid, int expires, string passwordkey, int templateid, int invisible)
        {
            User userInfo = User.FindByID(uid);
            WriteUserCookie(userInfo, expires, passwordkey, templateid, invisible);
        }

        public static void WriteUserCookie(User userinfo, int expires, string passwordkey, int templateid, int invisible)
        {
            if (userinfo == null) return;

            var httpCookie = new HttpCookie("bbx");
            httpCookie.Values["userid"] = userinfo.ID.ToString();
            httpCookie.Values["password"] = Utils.UrlEncode(SetCookiePassword(userinfo.Password, passwordkey));
            // 如果模版ID无效，则使用用户ID
            if (Template.FindByID(templateid) == null)
            {
                templateid = 0;
                //string[] array = Utils.SplitString(Templates.GetValidTemplateIDList(), ",");
                //for (int i = 0; i < array.Length; i++)
                //{
                //    string text = array[i];
                //    if (text.Equals(userinfo.TemplateID.ToString()))
                //    {
                //        templateid = userinfo.TemplateID;
                //        break;
                //    }
                //}
                if (Template.Has(userinfo.TemplateID)) templateid = userinfo.TemplateID;
            }
            httpCookie.Values["tpp"] = userinfo.Tpp.ToString();
            httpCookie.Values["ppp"] = userinfo.Ppp.ToString();
            httpCookie.Values["pmsound"] = userinfo.Pmsound.ToString();
            if (invisible != 0 || invisible != 1)
            {
                invisible = userinfo.Invisible ? 1 : 0;
            }
            httpCookie.Values["invisible"] = invisible.ToString();
            httpCookie.Values["referer"] = "index.aspx";
            httpCookie.Values["sigstatus"] = userinfo.Sigstatus.ToString();
            httpCookie.Values["expires"] = expires.ToString();
            httpCookie.Values["userinfotips"] = Utils.GetCookie("bbx", "userinfotips");
            if (expires > 0)
            {
                httpCookie.Expires = DateTime.Now.AddMinutes((double)expires);
            }
            string domain = GeneralConfigInfo.Current.CookieDomain;
            if (domain != null && HttpContext.Current.Request.Url.Host.IndexOf(domain.TrimStart('.')) > -1 && IsValidDomain(HttpContext.Current.Request.Url.Host))
            {
                httpCookie.Domain = domain;
            }
            HttpContext.Current.Response.AppendCookie(httpCookie);
            if (templateid > 0)
            {
                Utils.WriteCookie(Utils.GetTemplateCookieName(), templateid.ToString(), 999999);
            }
        }

        public static void WriteUserCookie(int uid, int expires, string passwordkey)
        {
            WriteUserCookie(uid, expires, passwordkey, 0, -1);
        }

        public static void WriteUserCookie(User userinfo, int expires, string passwordkey)
        {
            WriteUserCookie(userinfo, expires, passwordkey, 0, -1);
        }

        /// <summary>获取Cookie值</summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetCookie(string name)
        {
            var context = HttpContext.Current;
            if (context == null) return "";
            var req = context.Request;
            if (req == null || req.Cookies == null) return "";

            var cookie = req.Cookies["bbx"];
            if (cookie == null) return "";

            var value = cookie[name];
            if (value == null) return "";

            return Utils.UrlDecode(value);
        }

        public static void ClearUserCookie(string cookieName = "bbx")
        {
            var httpCookie = new HttpCookie(cookieName);
            httpCookie.Values.Clear();
            httpCookie.Expires = DateTime.Now.AddYears(-1);
            var domain = GeneralConfigInfo.Current.CookieDomain;
            var context = HttpContext.Current;
            if (!domain.IsNullOrWhiteSpace())
            {
                if (context != null && context.Request.Url.Host.IndexOf(domain.TrimStart('.')) > -1 && IsValidDomain(context.Request.Url.Host))
                {
                    httpCookie.Domain = domain;
                }
            }
            if (context != null) context.Response.AppendCookie(httpCookie);
        }

        public static string CreateAuthStr(int len)
        {
            var sb = new StringBuilder();
            var random = new Random();
            for (int i = 0; i < len; i++)
            {
                int num = random.Next();
                if (num % 2 == 0)
                {
                    sb.Append((char)(48 + (ushort)(num % 10)));
                }
                else
                {
                    sb.Append((char)(65 + (ushort)(num % 26)));
                }
            }
            return sb.ToString();
        }

        public static bool IsGuestCachePage(int pageid, string pagename)
        {
            if (pagename == "showtopic")
            {
                if (GeneralConfigInfo.Current.Guestcachepagetimeout > 0 && pageid == 1) return true;
            }
            return false;
        }

        public static bool ResponseShowTopicCacheFile(int tid, int pageid)
        {
            if (!IsGuestCachePage(pageid, "showtopic")) return false;

            var cacheService = XCache.Current;
            string text = cacheService.RetrieveObject("/Forum/ShowTopicGuestCachePage/Tid_" + tid + "/Page_" + pageid + "/") as string;
            if (string.IsNullOrEmpty(text)) return false;

            HttpContext.Current.Response.Write(text);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
            return true;
        }

        public static bool ResponseShowForumCacheFile(int fid, int pageid)
        {
            if (!IsGuestCachePage(pageid, "showforum")) return false;

            var cacheService = XCache.Current;
            string text = cacheService.RetrieveObject("/Forum/ShowForumGuestCachePage/Fid_" + fid + "/Page_" + pageid + "/") as string;
            if (string.IsNullOrEmpty(text)) return false;

            HttpContext.Current.Response.Write(text);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
            return true;
        }

        public static bool CreateShowTopicCacheFile(int tid, int pageid, string pagetext)
        {
            if (IsGuestCachePage(pageid, "showtopic"))
            {
                var cacheService = XCache.Current;
                if (!string.IsNullOrEmpty(pagetext))
                {
                    pagetext = pagetext + "\r\n<!-- " + Utils.ProductName + " CachedPage (Created: " + Utils.GetDateTime() + ") -->\r\n";
                    XCache.Add("/Forum/ShowTopicGuestCachePage/Tid_" + tid + "/Page_" + pageid + "/", pagetext, GeneralConfigInfo.Current.Guestcachepagetimeout * 60);
                }
                return true;
            }
            return false;
        }

        public static bool CreateShowForumCacheFile(int fid, int pageid, string pagetext)
        {
            if (IsGuestCachePage(pageid, "showforum"))
            {
                var cacheService = XCache.Current;
                if (!string.IsNullOrEmpty(pagetext))
                {
                    pagetext = pagetext + "\r\n<!-- " + Utils.ProductName + " CachedPage (Created: " + Utils.GetDateTime() + ") -->";
                    //XCache.Add("/Forum/ShowForumGuestCachePage/Fid_" + fid + "/Page_" + pageid + "/", pagetext, mcci.ApplyMemCached ? (mcci.CacheShowForumCacheTime * 60) : (rci.CacheShowForumCacheTime * 60));
                    XCache.Add("/Forum/ShowForumGuestCachePage/Fid_" + fid + "/Page_" + pageid + "/", pagetext, 0);
                    return true;
                }
            }
            return false;
        }

        public static bool DeleteTopicCacheFile(int tid)
        {
            int num = 1;
            //if (mcci != null && mcci.ApplyMemCached)
            //{
            //    num = mcci.CacheShowTopicPageNumber;
            //}
            //else
            //{
            //    if (rci != null && rci.ApplyRedis)
            //    {
            //        num = rci.CacheShowTopicPageNumber;
            //    }
            //}
            for (int i = 1; i < num; i++)
            {
                XCache.Remove("/Forum/ShowTopicGuestCachePage/Tid_" + tid + "/Page_" + i + "/");
            }
            return true;
        }

        public static int DeleteTopicCacheFile(string tidlist)
        {
            int num = 0;
            string[] array = Utils.SplitString(tidlist, ",");
            string[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                string text = array2[i];
                if (Utils.IsNumeric(text) && DeleteTopicCacheFile(int.Parse(text)))
                {
                    num++;
                }
            }
            return num;
        }

        public static bool IsPostFile()
        {
            for (int i = 0; i < HttpContext.Current.Request.Files.Count; i++)
            {
                if (HttpContext.Current.Request.Files[i].FileName != "")
                {
                    return true;
                }
            }
            return false;
        }

        public static void UpdateVisitedForumsOptions(int fid)
        {
            if (String.IsNullOrEmpty(GetCookie("visitedforums").Trim()))
            {
                WriteCookie("visitedforums", fid.ToString());
                return;
            }
            bool flag = false;
            string[] array = Utils.SplitString(GetCookie("visitedforums"), ",");
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == fid.ToString())
                {
                    flag = true;
                }
            }
            if (!flag)
            {
                WriteCookie("visitedforums", fid + "," + GetCookie("visitedforums"));
            }
        }

        public static string BanWordFilter(string text)
        {
            StringBuilder stringBuilder = new StringBuilder(text);
            string[,] banWordList = Word.GetBanWordList();
            int num = banWordList.Length / 2;
            for (int i = 0; i < num; i++)
            {
                if (banWordList[i, 1] != "{BANNED}" && banWordList[i, 1] != "{MOD}")
                {
                    stringBuilder = new StringBuilder().Append(Regex.Replace(stringBuilder.ToString(), banWordList[i, 0], banWordList[i, 1], Utils.GetRegexCompiledOptions()));
                }
            }
            return stringBuilder.ToString();
        }

        public static bool InBanWordArray(string text)
        {
            string[,] banWordList = Word.GetBanWordList();
            for (int i = 0; i < banWordList.Length / 2; i++)
            {
                r_word = new Regex(banWordList[i, 0], Utils.GetRegexCompiledOptions());
                IEnumerator enumerator = r_word.Matches(text).GetEnumerator();
                try
                {
                    if (enumerator.MoveNext())
                    {
                        Match arg_3F_0 = (Match)enumerator.Current;
                        return true;
                    }
                }
                finally
                {
                    IDisposable disposable = enumerator as IDisposable;
                    if (disposable != null)
                    {
                        disposable.Dispose();
                    }
                }
            }
            return false;
        }

        public static bool HasBannedWord(string text)
        {
            string[,] banWordList = Word.GetBanWordList();
            text = RemoveSpecialChars(text, GeneralConfigInfo.Current.Antispamreplacement);
            for (int i = 0; i < banWordList.Length / 2; i++)
            {
                if (banWordList[i, 1] == "{BANNED}")
                {
                    r_word = new Regex(banWordList[i, 0], Utils.GetRegexCompiledOptions());
                    IEnumerator enumerator = r_word.Matches(text).GetEnumerator();
                    try
                    {
                        if (enumerator.MoveNext())
                        {
                            Match arg_65_0 = (Match)enumerator.Current;
                            return true;
                        }
                    }
                    finally
                    {
                        IDisposable disposable = enumerator as IDisposable;
                        if (disposable != null)
                        {
                            disposable.Dispose();
                        }
                    }
                }
            }
            return false;
        }

        public static string GetBannedWord(string text)
        {
            string[,] banWordList = Word.GetBanWordList();
            text = RemoveSpecialChars(text, GeneralConfigInfo.Current.Antispamreplacement);
            for (int i = 0; i < banWordList.Length / 2; i++)
            {
                if (banWordList[i, 1] == "{BANNED}")
                {
                    r_word = new Regex(banWordList[i, 0], Utils.GetRegexCompiledOptions());
                    IEnumerator enumerator = r_word.Matches(text).GetEnumerator();
                    try
                    {
                        if (enumerator.MoveNext())
                        {
                            Match match = (Match)enumerator.Current;
                            return match.Groups[0].ToString();
                        }
                    }
                    finally
                    {
                        IDisposable disposable = enumerator as IDisposable;
                        if (disposable != null)
                        {
                            disposable.Dispose();
                        }
                    }
                }
            }
            return string.Empty;
        }

        public static bool HasAuditWord(string text)
        {
            string[,] banWordList = Word.GetBanWordList();
            text = RemoveSpecialChars(text, GeneralConfigInfo.Current.Antispamreplacement);
            for (int i = 0; i < banWordList.Length / 2; i++)
            {
                if (banWordList[i, 1] == "{MOD}")
                {
                    r_word = new Regex(banWordList[i, 0], Utils.GetRegexCompiledOptions());
                    IEnumerator enumerator = r_word.Matches(text).GetEnumerator();
                    try
                    {
                        if (enumerator.MoveNext())
                        {
                            Match arg_65_0 = (Match)enumerator.Current;
                            return true;
                        }
                    }
                    finally
                    {
                        IDisposable disposable = enumerator as IDisposable;
                        if (disposable != null)
                        {
                            disposable.Dispose();
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>是否跨站点提交</summary>
        /// <returns></returns>
        public static bool IsCrossSitePost()
        {
            //    return !DNTRequest.IsPost() || IsCrossSitePost(DNTRequest.GetUrlReferrer(), DNTRequest.GetHost());
            //}

            //public static bool IsCrossSitePost(string url, string host)
            //{
            var req = HttpContext.Current.Request;
            //if (!req.HttpMethod.Equals("POST")) return true;

            var url = req.UrlReferrer;
            if (url == null || url.ToString().Length < 7) return true;

            return url.Host != req.Url.Host;
        }

        public static bool IsHidePost(string str)
        {
            var regex = new Regex("\\[hide(=\\d+)?\\]");
            return regex.IsMatch(str) && str.IndexOf("[/hide]") > 0;
        }

        public static string AddJammer(string message)
        {
            string jammer = Caches.GetJammer();
            Match match = r[0].Match(message);
            if (match.Success)
            {
                message = message.Replace(match.Groups[0].ToString(), jammer + "\r\n");
            }
            match = r[1].Match(message);
            if (match.Success)
            {
                message = message.Replace(match.Groups[0].ToString(), jammer + "\n");
            }
            match = r[2].Match(message);
            if (match.Success)
            {
                message = message.Replace(match.Groups[0].ToString(), jammer + "\r");
            }
            match = r[3].Match(message);
            if (match.Success)
            {
                message = message.Replace(match.Groups[0].ToString(), jammer + "<br />");
            }
            match = r[4].Match(message);
            if (match.Success)
            {
                message = message.Replace(match.Groups[0].ToString(), jammer + "</p>");
            }
            return message + jammer;
        }

        public static bool IsBanUsername(string str, string stringarray)
        {
            if (Utils.StrIsNullOrEmpty(stringarray)) return false;

            stringarray = Regex.Escape(stringarray).Replace("\\*", "[\\s\\S]*");
            string[] array = Utils.SplitString(stringarray, "\\n");
            for (int i = 0; i < array.Length; i++)
            {
                string text = array[i];
                var regex = new Regex(string.Format("^{0}$", text), RegexOptions.IgnoreCase);
                if (regex.IsMatch(str) && !text.IsNullOrWhiteSpace())
                {
                    return true;
                }
            }
            return false;
        }

        public static string GetReUrl()
        {
            if (!DNTRequest.GetString("reurl").IsNullOrEmpty())
            {
                Utils.WriteCookie("reurl", DNTRequest.GetString("reurl").Trim());
                return DNTRequest.GetString("reurl").Trim();
            }
            if (String.IsNullOrEmpty(Utils.GetCookie("reurl")))
            {
                return "index.aspx";
            }
            return Utils.GetCookie("reurl");
        }

        public static bool IsValidDomain(string host)
        {
            return host.IndexOf(".") != -1 && !new Regex("^\\d+$").IsMatch(host.Replace(".", string.Empty));
        }

        public static string UpdatePathListExtname(string pathlist, string extname)
        {
            return pathlist.Replace("{extname}", extname);
        }

        public static string ConvertDateTime(string date, DateTime currentDateTime)
        {
            if (Utils.StrIsNullOrEmpty(date))
            {
                return "";
            }
            DateTime startDate;
            if (!DateTime.TryParse(date, out startDate))
            {
                return "";
            }
            if (GeneralConfigInfo.Current.DateDiff == 0)
            {
                return Convert.ToDateTime(date).ToString("yyyy-MM-dd HH:mm");
            }

            return ConvertDateTime(startDate, currentDateTime);
        }

        public static string ConvertDateTime(DateTime startDate, DateTime currentDateTime)
        {
            string result;
            if (currentDateTime.Year == startDate.Year && currentDateTime.Month == startDate.Month)
            {
                if (DateDiff("hour", startDate, currentDateTime) <= 10L)
                {
                    if (DateDiff("hour", startDate, currentDateTime) > 0L)
                    {
                        return DateDiff("hour", startDate, currentDateTime) + "小时前";
                    }
                    if (DateDiff("minute", startDate, currentDateTime) > 0L)
                    {
                        return DateDiff("minute", startDate, currentDateTime) + "分钟前";
                    }
                    if (DateDiff("second", startDate, currentDateTime) >= 0L)
                    {
                        return DateDiff("second", startDate, currentDateTime) + "秒前";
                    }
                    return "刚才";
                }
                else
                {
                    switch (currentDateTime.Day - startDate.Day)
                    {
                        case 0:
                            result = "今天 " + startDate.ToString("HH") + ":" + startDate.ToString("mm");
                            break;

                        case 1:
                            result = "昨天 " + startDate.ToString("HH") + ":" + startDate.ToString("mm");
                            break;

                        case 2:
                            result = "前天 " + startDate.ToString("HH") + ":" + startDate.ToString("mm");
                            break;

                        default:
                            result = startDate.ToString("yyyy-MM-dd HH:mm");
                            break;
                    }
                }
            }
            else
            {
                result = startDate.ToString("yyyy-MM-dd HH:mm");
            }
            return result;
        }

        public static long DateDiff(string Interval, DateTime StartDate, DateTime EndDate)
        {
            long result = 0L;
            TimeSpan timeSpan = new TimeSpan(EndDate.Ticks - StartDate.Ticks);
            switch (Interval)
            {
                case "second":
                    result = (long)timeSpan.TotalSeconds;
                    break;

                case "minute":
                    result = (long)timeSpan.TotalMinutes;
                    break;

                case "hour":
                    result = (long)timeSpan.TotalHours;
                    break;

                case "day":
                    result = (long)timeSpan.Days;
                    break;

                case "week":
                    result = (long)(timeSpan.Days / 7);
                    break;

                case "month":
                    result = (long)(timeSpan.Days / 30);
                    break;

                case "quarter":
                    result = (long)(timeSpan.Days / 30 / 3);
                    break;

                case "year":
                    result = (long)(timeSpan.Days / 365);
                    break;
            }
            return result;
        }

        public static string ConvertDateTime(string date)
        {
            return ConvertDateTime(date, DateTime.Now);
        }

        public static string ConvertDateTime(DateTime date)
        {
            return ConvertDateTime(date, DateTime.Now);
        }

        public static string ConvertCreditAndAmountToWord(int credit, int amount)
        {
            if (credit < 1 || credit > 8)
            {
                return "0";
            }
            string[] validScoreName = Scoresets.GetValidScoreName();
            string[] validScoreUnit = Scoresets.GetValidScoreUnit();
            return string.Format("{0}:{1}{2}", validScoreName[credit], amount, validScoreUnit[credit]);
        }

        public static string RemoveSpecialChars(string content, string keyCharString)
        {
            char[] array = keyCharString.ToCharArray();
            for (int i = 0; i < array.Length; i++)
            {
                content = content.Replace(Convert.ToString(array[i]), string.Empty);
            }
            return content.Replace(" ", "");
        }

        public static void WriteUserCreditsCookie(IUser userInfo, string groupTitle)
        {
            if (userInfo == null)
            {
                return;
            }
            string[] validScoreName = Scoresets.GetValidScoreName();
            string[] validScoreUnit = Scoresets.GetValidScoreUnit();
            string text = "积分:" + userInfo.Credits + ",";
            text = text + "用户组:" + groupTitle + ",";
            for (int i = 0; i < validScoreName.Length; i++)
            {
                string key;
                if (!Utils.StrIsNullOrEmpty(validScoreName[i]) && (key = "Extcredits" + i) != null)
                {
                    switch (key)
                    {
                        case "Extcredits1":
                            {
                                object obj = text;
                                text = obj + validScoreName[i] + ": " + userInfo.ExtCredits1 + validScoreUnit[1] + ",";
                                break;
                            }
                        case "Extcredits2":
                            {
                                object obj2 = text;
                                text = obj2 + validScoreName[i] + ": " + userInfo.ExtCredits2 + validScoreUnit[2] + ",";
                                break;
                            }
                        case "Extcredits3":
                            {
                                object obj3 = text;
                                text = obj3 + validScoreName[i] + ": " + userInfo.ExtCredits3 + validScoreUnit[3] + ",";
                                break;
                            }
                        case "Extcredits4":
                            {
                                object obj4 = text;
                                text = obj4 + validScoreName[i] + ": " + userInfo.ExtCredits4 + validScoreUnit[4] + ",";
                                break;
                            }
                        case "Extcredits5":
                            {
                                object obj5 = text;
                                text = obj5 + validScoreName[i] + ": " + userInfo.ExtCredits5 + validScoreUnit[5] + ",";
                                break;
                            }
                        case "Extcredits6":
                            {
                                object obj6 = text;
                                text = obj6 + validScoreName[i] + ": " + userInfo.ExtCredits6 + validScoreUnit[6] + ",";
                                break;
                            }
                        case "Extcredits7":
                            {
                                object obj7 = text;
                                text = obj7 + validScoreName[i] + ": " + userInfo.ExtCredits7 + validScoreUnit[7] + ",";
                                break;
                            }
                        case "Extcredits8":
                            {
                                object obj8 = text;
                                text = obj8 + validScoreName[i] + ": " + userInfo.ExtCredits8 + validScoreUnit[8] + ",";
                                break;
                            }
                    }
                }
            }
            HttpCookie httpCookie = HttpContext.Current.Request.Cookies["dntusertips"];
            if (httpCookie == null)
            {
                httpCookie = new HttpCookie("dntusertips");
            }
            httpCookie.Values["userinfotips"] = Utils.UrlEncode(text.TrimEnd(','));
            httpCookie.Expires = DateTime.Now.AddMinutes(5.0);
            HttpContext.Current.Response.AppendCookie(httpCookie);
        }

        public static string GetUserCreditsCookie(int uId, string groupTitle)
        {
            if (uId == -1)
            {
                return "";
            }
            string text = Utils.UrlDecode(Utils.GetCookie("dntusertips", "userinfotips"));
            if (!string.IsNullOrEmpty(text))
            {
                return text;
            }
            WriteUserCreditsCookie(User.FindByID(uId), groupTitle);
            return Utils.UrlDecode(Utils.GetCookie("dntusertips", "userinfotips"));
        }

        public static void SetVisitedForumsCookie(string fid)
        {
            string text = Utils.GetCookie("visitedforums");
            if (String.IsNullOrEmpty(text))
            {
                text = fid;
            }
            else
            {
                int num = ("," + text + ",").IndexOf("," + fid + ",");
                if (num == -1)
                {
                    text = fid + "," + text;
                }
                if (num > 0)
                {
                    text = fid + "," + text.Replace("," + fid, "");
                }
                if (text.Split(',').Length > 10)
                {
                    text = text.Substring(0, text.LastIndexOf(","));
                }
            }
            Utils.WriteCookie("visitedforums", text, 43200);
        }
    }
}