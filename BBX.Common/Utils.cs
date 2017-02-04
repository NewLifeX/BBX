using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Xml;
using NewLife.Log;
using NewLife.Reflection;

namespace BBX.Common
{
    public class Utils
    {
        //public class VersionInfo
        //{
        //    public int FileMajorPart { get { return 1; } }

        //    public int FileMinorPart { get { return 0; } }

        //    public int FileBuildPart { get { return 1031; } }

        //    public string ProductName { get { return "BBX"; } }

        //    public string LegalCopyright { get { return "2012, NewLife"; } }
        //}

        //public const string ASSEMBLY_VERSION = "1.0.1031";
        //public const string ASSEMBLY_YEAR = "2012";
        //private const string unreservedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.";
        private static Regex RegexBr = new Regex("(\\r\\n)", RegexOptions.IgnoreCase);
        public static Regex RegexFont = new Regex("<font color=\".*?\">([\\s\\S]+?)</font>", GetRegexCompiledOptions());
        //static readonly VersionInfo AssemblyFileVersion = new VersionInfo();
        //private static string TemplateCookieName = string.Format("bbxtemplateid_{0}_{1}_{2}", AssemblyFileVersion.FileMajorPart, AssemblyFileVersion.FileMinorPart, AssemblyFileVersion.FileBuildPart);

        private static string[] browerNames = new string[] { "MSIE", "Firefox", "Opera", "Netscape", "Safari", "Lynx", "Konqueror", "Mozilla" };

        //public static string[] Monthes { get { return new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" }; } }

        public static String Version { get { return Assembly.GetExecutingAssembly().GetName().Version.ToString(); } }

        public static String ProductName { get { return "BBX"; } }

        public static String ProductUrl { get { return "http://www.NewLifeX.com"; } }

        public static String CompanyName { get { return "NewLife Team"; } }

        public static String CompanyUrl { get { return "http://www.NewLifeX.com"; } }

        //public static string GetAssemblyVersion()
        //{
        //    return string.Format("{0}.{1}.{2}", AssemblyFileVersion.FileMajorPart, AssemblyFileVersion.FileMinorPart, AssemblyFileVersion.FileBuildPart);
        //}

        //public static string GetAssemblyProductName()
        //{
        //    return AssemblyFileVersion.ProductName;
        //}

        public static RegexOptions GetRegexCompiledOptions() { return RegexOptions.None; }

        public static int GetStringLength(string str)
        {
            return Encoding.Default.GetBytes(str).Length;
        }

        public static bool IsEqualStr(string str, string stringarray, string strsplit)
        {
            if (str.IsNullOrWhiteSpace() || stringarray.IsNullOrWhiteSpace()) return false;

            return (strsplit + stringarray.ToLower() + strsplit).IndexOf(strsplit + str.ToLower() + strsplit) >= 0;
        }

        public static int GetInArrayID(string strSearch, string[] stringArray, bool caseInsensetive)
        {
            for (int i = 0; i < stringArray.Length; i++)
            {
                if (caseInsensetive)
                {
                    if (strSearch.ToLower() == stringArray[i].ToLower())
                    {
                        return i;
                    }
                }
                else
                {
                    if (strSearch == stringArray[i])
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        public static int GetInArrayID(string strSearch, string[] stringArray)
        {
            return GetInArrayID(strSearch, stringArray, true);
        }

        public static bool InArray(string strSearch, string[] stringArray, bool caseInsensetive)
        {
            return GetInArrayID(strSearch, stringArray, caseInsensetive) >= 0;
        }

        public static bool InArray(string str, string[] stringarray)
        {
            return InArray(str, stringarray, false);
        }

        public static bool InArray(string str, string stringarray)
        {
            return InArray(str, SplitString(stringarray, ","), false);
        }

        public static bool InArray(string str, string stringarray, string strsplit)
        {
            return InArray(str, SplitString(stringarray, strsplit), false);
        }

        //public static bool InArray(string str, string stringarray, string strsplit, bool caseInsensetive)
        //{
        //    return InArray(str, SplitString(stringarray, strsplit), caseInsensetive);
        //}

        //public static string RTrim(string str)
        //{
        //    for (int i = str.Length; i >= 0; i--)
        //    {
        //        if (str[i].Equals(" ") || str[i].Equals("\r") || str[i].Equals("\n"))
        //        {
        //            str.Remove(i, 1);
        //        }
        //    }
        //    return str;
        //}

        public static string ClearBR(string str)
        {
            Match match = RegexBr.Match(str);
            while (match.Success)
            {
                str = str.Replace(match.Groups[0].ToString(), "");
                match = match.NextMatch();
            }
            return str;
        }

        public static string CutString(string str, int startIndex, int length)
        {
            if (startIndex >= 0)
            {
                if (length < 0)
                {
                    length *= -1;
                    if (startIndex - length < 0)
                    {
                        length = startIndex;
                        startIndex = 0;
                    }
                    else
                    {
                        startIndex -= length;
                    }
                }
                if (startIndex > str.Length)
                {
                    return "";
                }
            }
            else
            {
                if (length < 0)
                {
                    return "";
                }
                if (length + startIndex <= 0)
                {
                    return "";
                }
                length += startIndex;
                startIndex = 0;
            }
            if (str.Length - startIndex < length)
            {
                length = str.Length - startIndex;
            }
            return str.Substring(startIndex, length);
        }

        public static string CutString(string str, int startIndex)
        {
            return CutString(str, startIndex, str.Length);
        }

        /// <summary>获取物理路径</summary>
        /// <param name="strPath"></param>
        /// <returns></returns>
        public static string GetMapPath(string strPath)
        {
            if (HttpContext.Current != null) return HttpContext.Current.Server.MapPath(strPath);

            strPath = strPath.Replace("/", "\\");
            if (strPath.StartsWith("\\"))
            {
                strPath = strPath.TrimStart('\\');
            }
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
        }

        public static bool FileExists(string filename)
        {
            return File.Exists(filename);
        }

        public static void ResponseFile(string filepath, string filename, string filetype)
        {
            Stream stream = null;
            byte[] buffer = new byte[10000];
            try
            {
                stream = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                long num = stream.Length;
                HttpContext.Current.Response.ContentType = filetype;
                if (HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"].IndexOf("MSIE") > -1)
                {
                    HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + UrlEncode(filename.Trim()).Replace("+", " "));
                }
                else
                {
                    HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + filename.Trim());
                }
                while (num > 0L)
                {
                    if (HttpContext.Current.Response.IsClientConnected)
                    {
                        int num2 = stream.Read(buffer, 0, 10000);
                        HttpContext.Current.Response.OutputStream.Write(buffer, 0, num2);
                        HttpContext.Current.Response.Flush();
                        buffer = new byte[10000];
                        num -= (long)num2;
                    }
                    else
                    {
                        num = -1L;
                    }
                }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Write("Error : " + ex.Message);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
            HttpContext.Current.Response.End();
        }

        //public static bool IsImgFilename(string filename)
        //{
        //    filename = filename.Trim();
        //    if (filename.EndsWith(".") || filename.IndexOf(".") == -1)
        //    {
        //        return false;
        //    }
        //    string a = filename.Substring(filename.LastIndexOf(".") + 1).ToLower();
        //    return a == "jpg" || a == "jpeg" || a == "png" || a == "bmp" || a == "gif";
        //}

        //public static string IntToStr(int intValue)
        //{
        //    return Convert.ToString(intValue);
        //}

        public static string MD5(string str)
        {
            byte[] array = Encoding.UTF8.GetBytes(str);
            array = new MD5CryptoServiceProvider().ComputeHash(array);
            string text = "";
            for (int i = 0; i < array.Length; i++)
            {
                text += array[i].ToString("x").PadLeft(2, '0');
            }
            return text;
        }

        //public static string SHA256(string str)
        //{
        //    byte[] bytes = Encoding.UTF8.GetBytes(str);
        //    SHA256Managed sHA256Managed = new SHA256Managed();
        //    byte[] inArray = sHA256Managed.ComputeHash(bytes);
        //    return Convert.ToBase64String(inArray);
        //}

        public static string GetSubString(string p_SrcString, int p_Length, string p_TailString)
        {
            return GetSubString(p_SrcString, 0, p_Length, p_TailString);
        }

        public static string GetUnicodeSubString(string str, int len, string p_TailString)
        {
            if (str.IsNullOrEmpty()) return str;

            str = str.TrimEnd(new char[0]);
            string result = string.Empty;
            int byteCount = Encoding.Default.GetByteCount(str);
            int length = str.Length;
            int num = 0;
            int num2 = 0;
            if (byteCount > len)
            {
                for (int i = 0; i < length; i++)
                {
                    if (Convert.ToInt32(str.ToCharArray()[i]) > 255)
                    {
                        num += 2;
                    }
                    else
                    {
                        num++;
                    }
                    if (num > len)
                    {
                        num2 = i;
                        break;
                    }
                    if (num == len)
                    {
                        num2 = i + 1;
                        break;
                    }
                }
                if (num2 >= 0)
                {
                    result = str.Substring(0, num2) + p_TailString;
                }
            }
            else
            {
                result = str;
            }
            return result;
        }

        public static string GetSubString(string p_SrcString, int p_StartIndex, int p_Length, string p_TailString)
        {
            string text = p_SrcString;
            byte[] bytes = Encoding.UTF8.GetBytes(p_SrcString);
            char[] chars = Encoding.UTF8.GetChars(bytes);
            for (int i = 0; i < chars.Length; i++)
            {
                char c = chars[i];
                if ((c > 'ࠀ' && c < '一') || (c > '가' && c < '힣'))
                {
                    string result;
                    if (p_StartIndex >= p_SrcString.Length)
                    {
                        result = "";
                    }
                    else
                    {
                        result = p_SrcString.Substring(p_StartIndex, (p_Length + p_StartIndex > p_SrcString.Length) ? (p_SrcString.Length - p_StartIndex) : p_Length);
                    }
                    return result;
                }
            }
            if (p_Length >= 0)
            {
                byte[] bytes2 = Encoding.Default.GetBytes(p_SrcString);
                if (bytes2.Length > p_StartIndex)
                {
                    int num = bytes2.Length;
                    if (bytes2.Length > p_StartIndex + p_Length)
                    {
                        num = p_Length + p_StartIndex;
                    }
                    else
                    {
                        p_Length = bytes2.Length - p_StartIndex;
                        p_TailString = "";
                    }
                    int num2 = p_Length;
                    int[] array = new int[p_Length];
                    int num3 = 0;
                    for (int j = p_StartIndex; j < num; j++)
                    {
                        if (bytes2[j] > 127)
                        {
                            num3++;
                            if (num3 == 3)
                            {
                                num3 = 1;
                            }
                        }
                        else
                        {
                            num3 = 0;
                        }
                        array[j] = num3;
                    }
                    if (bytes2[num - 1] > 127 && array[p_Length - 1] == 1)
                    {
                        num2 = p_Length + 1;
                    }
                    byte[] array2 = new byte[num2];
                    Array.Copy(bytes2, p_StartIndex, array2, 0, num2);
                    text = Encoding.Default.GetString(array2);
                    text += p_TailString;
                }
            }
            return text;
        }

        public static string ReplaceString(string SourceString, string SearchString, string ReplaceString, bool IsCaseInsensetive)
        {
            return Regex.Replace(SourceString, Regex.Escape(SearchString), ReplaceString, IsCaseInsensetive ? RegexOptions.IgnoreCase : RegexOptions.None);
        }

        public static string GetSpacesString(int spacesCount)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < spacesCount; i++)
            {
                stringBuilder.Append(" &nbsp;&nbsp;");
            }
            return stringBuilder.ToString();
        }

        public static bool IsValidEmail(string strEmail)
        {
            return Regex.IsMatch(strEmail, "^[\\w\\.]+([-]\\w+)*@[A-Za-z0-9-_]+[\\.][A-Za-z0-9-_]");
        }

        public static bool IsValidDoEmail(string strEmail)
        {
            return Regex.IsMatch(strEmail, "^@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([\\w-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$");
        }

        public static bool IsURL(string strUrl)
        {
            return Regex.IsMatch(strUrl, "^(http|https)\\://([a-zA-Z0-9\\.\\-]+(\\:[a-zA-Z0-9\\.&%\\$\\-]+)*@)*((25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])|localhost|([a-zA-Z0-9\\-]+\\.)*[a-zA-Z0-9\\-]+\\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{1,10}))(\\:[0-9]+)*(/($|[a-zA-Z0-9\\.\\,\\?\\'\\\\\\+&%\\$#\\=~_\\-]+))*$");
        }

        public static string GetEmailHostName(string strEmail)
        {
            if (strEmail.IndexOf("@") < 0)
            {
                return "";
            }
            return strEmail.Substring(strEmail.LastIndexOf("@")).ToLower();
        }

        public static bool IsBase64String(string str)
        {
            return Regex.IsMatch(str, "[A-Za-z0-9\\+\\/\\=]");
        }

        public static bool IsSafeSqlString(string str)
        {
            return !Regex.IsMatch(str, "[-|;|,|\\/|\\(|\\)|\\[|\\]|\\}|\\{|%|@|\\*|!|\\']");
        }

        public static bool IsSafeUserInfoString(string str)
        {
            return !Regex.IsMatch(str, "^\\s*$|^c:\\\\con\\\\con$|[%,\\*\"\\s\\t\\<\\>\\&]|游客|^Guest");
        }

        //public static string CleanInput(string strIn)
        //{
        //    return Regex.Replace(strIn.Trim(), "[^\\w\\.@-]", "");
        //}

        public static string GetFilename(string url)
        {
            if (url == null)
            {
                return "";
            }
            string[] array = url.Split('/');
            return array[array.Length - 1].Split('?')[0];
        }

        public static string StrFormat(string str)
        {
            string result;
            if (str == null)
            {
                result = "";
            }
            else
            {
                str = str.Replace("\r\n", "<br />");
                str = str.Replace("\n", "<br />");
                result = str;
            }
            return result;
        }

        //public static string GetDate()
        //{
        //    return DateTime.Now.ToString("yyyy-MM-dd");
        //}

        //public static string GetDate(string datetimestr, string replacestr)
        //{
        //    if (String.IsNullOrEmpty(datetimestr)) return replacestr;

        //    try
        //    {
        //        datetimestr = datetimestr.ToDateTime().ToString("yyyy-MM-dd").Replace("1900-01-01", replacestr);
        //    }
        //    catch
        //    {
        //        return replacestr;
        //    }
        //    return datetimestr;
        //}

        //public static string GetTime()
        //{
        //    return DateTime.Now.ToString("HH:mm:ss");
        //}

        public static string GetDateTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string GetDateTime(int relativeday)
        {
            return DateTime.Now.AddDays((double)relativeday).ToString("yyyy-MM-dd HH:mm:ss");
        }

        //public static string GetDateTimeF()
        //{
        //    return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fffffff");
        //}

        public static string GetStandardDateTime(string fDateTime, string formatStr)
        {
            if (fDateTime == "0000-0-0 0:00:00")
            {
                return fDateTime;
            }
            DateTime dateTime = new DateTime(1900, 1, 1, 0, 0, 0, 0);
            if (DateTime.TryParse(fDateTime, out dateTime))
            {
                return dateTime.ToString(formatStr);
            }
            return "N/A";
        }

        public static string GetStandardDateTime(string fDateTime)
        {
            return GetStandardDateTime(fDateTime, "yyyy-MM-dd HH:mm:ss");
        }

        //public static string GetStandardDate(string fDate)
        //{
        //    return GetStandardDateTime(fDate, "yyyy-MM-dd");
        //}

        public static bool IsTime(string timeval)
        {
            return Regex.IsMatch(timeval, "^((([0-1]?[0-9])|(2[0-3])):([0-5]?[0-9])(:[0-5]?[0-9])?)$");
        }

        //public static string GetRealIP()
        //{
        //    return WebHelper.UserHost;
        //}

        //public static string mashSQL(string str)
        //{
        //    if (str != null)
        //    {
        //        return str.Replace("'", "'");
        //    }
        //    return "";
        //}

        //public static string ChkSQL(string str)
        //{
        //    if (str != null)
        //    {
        //        return str.Replace("'", "''");
        //    }
        //    return "";
        //}

        //public void transHtml(string path, string outpath)
        //{
        //    Page page = new Page();
        //    StringWriter stringWriter = new StringWriter();
        //    page.Server.Execute(path, stringWriter);
        //    FileStream fileStream;
        //    if (File.Exists(page.Server.MapPath("") + "\\" + outpath))
        //    {
        //        File.Delete(page.Server.MapPath("") + "\\" + outpath);
        //        fileStream = File.Create(page.Server.MapPath("") + "\\" + outpath);
        //    }
        //    else
        //    {
        //        fileStream = File.Create(page.Server.MapPath("") + "\\" + outpath);
        //    }
        //    byte[] bytes = Encoding.Default.GetBytes(stringWriter.ToString());
        //    fileStream.Write(bytes, 0, bytes.Length);
        //    fileStream.Close();
        //}

        public static string[] SplitString(string str, string strSplit)
        {
            if (str.IsNullOrEmpty()) return new String[0];

            if (strSplit == "\r\n" || strSplit == "\n")
            {
                str = str.Replace("\r\n", "\n");
                strSplit = "\n";
            }
            if (strSplit == "\\n" && str.Contains("\\r"))
            {
                str = str.Replace("\\r\\n", "\\n");
            }
            if (str.IsNullOrEmpty()) return new String[0];
            if (str.IndexOf(strSplit) < 0) return new string[] { str };

            return Regex.Split(str, Regex.Escape(strSplit), RegexOptions.IgnoreCase);
        }

        public static string[] SplitString(string strContent, string strSplit, int count)
        {
            string[] array = new string[count];
            string[] array2 = SplitString(strContent, strSplit);
            for (int i = 0; i < count; i++)
            {
                if (i < array2.Length)
                {
                    array[i] = array2[i];
                }
                else
                {
                    array[i] = string.Empty;
                }
            }
            return array;
        }

        public static string[] PadStringArray(string[] strArray, int minLength, int maxLength)
        {
            if (minLength > maxLength)
            {
                int num = maxLength;
                maxLength = minLength;
                minLength = num;
            }
            int num2 = 0;
            for (int i = 0; i < strArray.Length; i++)
            {
                if (minLength > -1 && strArray[i].Length < minLength)
                {
                    strArray[i] = null;
                }
                else
                {
                    if (strArray[i].Length > maxLength)
                    {
                        strArray[i] = strArray[i].Substring(0, maxLength);
                    }
                    num2++;
                }
            }
            string[] array = new string[num2];
            int num3 = 0;
            int num4 = 0;
            while (num3 < strArray.Length && num4 < array.Length)
            {
                if (strArray[num3] != null && strArray[num3] != string.Empty)
                {
                    array[num4] = strArray[num3];
                    num4++;
                }
                num3++;
            }
            return array;
        }

        public static string[] SplitString(string strContent, string strSplit, bool ignoreRepeatItem, int maxElementLength)
        {
            string[] array = SplitString(strContent, strSplit);
            if (!ignoreRepeatItem)
            {
                return array;
            }
            return DistinctStringArray(array, maxElementLength);
        }

        public static string[] SplitString(string strContent, string strSplit, bool ignoreRepeatItem, int minElementLength, int maxElementLength)
        {
            string[] strArray = SplitString(strContent, strSplit);
            if (ignoreRepeatItem)
            {
                strArray = DistinctStringArray(strArray);
            }
            return PadStringArray(strArray, minElementLength, maxElementLength);
        }

        public static string[] SplitString(string strContent, string strSplit, bool ignoreRepeatItem)
        {
            return SplitString(strContent, strSplit, ignoreRepeatItem, 0);
        }

        public static string[] DistinctStringArray(string[] strArray, int maxElementLength)
        {
            Hashtable hashtable = new Hashtable();
            for (int i = 0; i < strArray.Length; i++)
            {
                string text = strArray[i];
                string text2 = text;
                if (maxElementLength > 0 && text2.Length > maxElementLength)
                {
                    text2 = text2.Substring(0, maxElementLength);
                }
                hashtable[text2.Trim()] = text;
            }
            string[] array = new string[hashtable.Count];
            hashtable.Keys.CopyTo(array, 0);
            return array;
        }

        public static string[] DistinctStringArray(string[] strArray)
        {
            return DistinctStringArray(strArray, 0);
        }

        public static string EncodeHtml(string strHtml)
        {
            if (String.IsNullOrEmpty(strHtml)) return String.Empty;

            strHtml = strHtml.Replace(",", "&def");
            strHtml = strHtml.Replace("'", "&dot");
            strHtml = strHtml.Replace(";", "&dec");
            return strHtml;
        }

        //public static string StrFilter(string str, string bantext)
        //{
        //    string[] array = SplitString(bantext, "\r\n");
        //    for (int i = 0; i < array.Length; i++)
        //    {
        //        string oldValue = array[i].Substring(0, array[i].IndexOf("="));
        //        string newValue = array[i].Substring(array[i].IndexOf("=") + 1);
        //        str = str.Replace(oldValue, newValue);
        //    }
        //    return str;
        //}

        public static string GetStaticPageNumbers(int curPage, int countPage, string url, string expname, int extendPage)
        {
            return GetStaticPageNumbers(curPage, countPage, url, expname, extendPage, 0);
        }

        public static string GetStaticPageNumbers(int curPage, int countPage, string url, string expname, int extendPage, int forumrewrite)
        {
            int num = 1;
            string value = "<a href=\"" + url + "-1" + expname + "\">&laquo;</a>";
            string value2 = "<a href=\"" + url + "-" + countPage + expname + "\">&raquo;</a>";
            if (forumrewrite == 1)
            {
                value = "<a href=\"" + url + "/1/list" + expname + "\">&laquo;</a>";
                value2 = "<a href=\"" + url + "/" + countPage + "/list" + expname + "\">&raquo;</a>";
            }
            if (forumrewrite == 2)
            {
                value = "<a href=\"" + url + "/\">&laquo;</a>";
                value2 = "<a href=\"" + url + "/" + countPage + "/\">&raquo;</a>";
            }
            if (countPage < 1)
            {
                countPage = 1;
            }
            if (extendPage < 3)
            {
                extendPage = 2;
            }
            int num2;
            if (countPage > extendPage)
            {
                if (curPage - extendPage / 2 > 0)
                {
                    if (curPage + extendPage / 2 < countPage)
                    {
                        num = curPage - extendPage / 2;
                        num2 = num + extendPage - 1;
                    }
                    else
                    {
                        num2 = countPage;
                        num = num2 - extendPage + 1;
                        value2 = "";
                    }
                }
                else
                {
                    num2 = extendPage;
                    value = "";
                }
            }
            else
            {
                num = 1;
                num2 = countPage;
                value = "";
                value2 = "";
            }
            StringBuilder stringBuilder = new StringBuilder("");
            stringBuilder.Append(value);
            for (int i = num; i <= num2; i++)
            {
                if (i == curPage)
                {
                    stringBuilder.Append("<span>");
                    stringBuilder.Append(i);
                    stringBuilder.Append("</span>");
                }
                else
                {
                    stringBuilder.Append("<a href=\"");
                    if (forumrewrite == 1)
                    {
                        stringBuilder.Append(url);
                        if (i != 1)
                        {
                            stringBuilder.Append("/");
                            stringBuilder.Append(i);
                        }
                        stringBuilder.Append("/list");
                        stringBuilder.Append(expname);
                    }
                    else
                    {
                        if (forumrewrite == 2)
                        {
                            stringBuilder.Append(url);
                            stringBuilder.Append("/");
                            if (i != 1)
                            {
                                stringBuilder.Append(i);
                                stringBuilder.Append("/");
                            }
                        }
                        else
                        {
                            stringBuilder.Append(url);
                            if (i != 1)
                            {
                                stringBuilder.Append("-");
                                stringBuilder.Append(i);
                            }
                            stringBuilder.Append(expname);
                        }
                    }
                    stringBuilder.Append("\">");
                    stringBuilder.Append(i);
                    stringBuilder.Append("</a>");
                }
            }
            stringBuilder.Append(value2);
            return stringBuilder.ToString();
        }

        //public static string GetPostPageNumbers(int countPage, string url, string expname, int extendPage)
        //{
        //    int num = 1;
        //    int num2 = 1;
        //    string value = "<a href=\"" + url + "-1" + expname + "\">&laquo;</a>";
        //    string value2 = "<a href=\"" + url + "-" + countPage + expname + "\">&raquo;</a>";
        //    if (countPage < 1)
        //    {
        //        countPage = 1;
        //    }
        //    if (extendPage < 3)
        //    {
        //        extendPage = 2;
        //    }
        //    int num3;
        //    if (countPage > extendPage)
        //    {
        //        if (num2 - extendPage / 2 > 0)
        //        {
        //            if (num2 + extendPage / 2 < countPage)
        //            {
        //                num = num2 - extendPage / 2;
        //                num3 = num + extendPage - 1;
        //            }
        //            else
        //            {
        //                num3 = countPage;
        //                num = num3 - extendPage + 1;
        //                value2 = "";
        //            }
        //        }
        //        else
        //        {
        //            num3 = extendPage;
        //            value = "";
        //        }
        //    }
        //    else
        //    {
        //        num = 1;
        //        num3 = countPage;
        //        value = "";
        //        value2 = "";
        //    }
        //    StringBuilder stringBuilder = new StringBuilder("");
        //    stringBuilder.Append(value);
        //    for (int i = num; i <= num3; i++)
        //    {
        //        stringBuilder.Append("<a href=\"");
        //        stringBuilder.Append(url);
        //        stringBuilder.Append("-");
        //        stringBuilder.Append(i);
        //        stringBuilder.Append(expname);
        //        stringBuilder.Append("\">");
        //        stringBuilder.Append(i);
        //        stringBuilder.Append("</a>");
        //    }
        //    stringBuilder.Append(value2);
        //    return stringBuilder.ToString();
        //}

        public static string GetPageNumbers(int curPage, int countPage, string url, int extendPage)
        {
            return GetPageNumbers(curPage, countPage, url, extendPage, "page");
        }

        public static string GetPageNumbers(int curPage, int countPage, string url, int extendPage, string pagetag)
        {
            return GetPageNumbers(curPage, countPage, url, extendPage, pagetag, null);
        }

        public static string GetPageNumbers(int curPage, int countPage, string url, int extendPage, string pagetag, string anchor)
        {
            if (String.IsNullOrEmpty(pagetag)) pagetag = "page";

            int num = 1;
            if (url.IndexOf("?") > 0)
                url += "&";
            else
                url += "?";

            string text = "<a href=\"" + url + "&" + pagetag + "=1";
            string text2 = "<a href=\"" + url + "&" + pagetag + "=" + countPage;
            if (anchor != null)
            {
                text += anchor;
                text2 += anchor;
            }
            text += "\">&laquo;</a>";
            text2 += "\">&raquo;</a>";
            if (countPage < 1)
            {
                countPage = 1;
            }
            if (extendPage < 3)
            {
                extendPage = 2;
            }
            int num2;
            if (countPage > extendPage)
            {
                if (curPage - extendPage / 2 > 0)
                {
                    if (curPage + extendPage / 2 < countPage)
                    {
                        num = curPage - extendPage / 2;
                        num2 = num + extendPage - 1;
                    }
                    else
                    {
                        num2 = countPage;
                        num = num2 - extendPage + 1;
                        text2 = "";
                    }
                }
                else
                {
                    num2 = extendPage;
                    text = "";
                }
            }
            else
            {
                num = 1;
                num2 = countPage;
                text = "";
                text2 = "";
            }
            StringBuilder stringBuilder = new StringBuilder("");
            stringBuilder.Append(text);
            for (int i = num; i <= num2; i++)
            {
                if (i == curPage)
                {
                    stringBuilder.Append("<span>");
                    stringBuilder.Append(i);
                    stringBuilder.Append("</span>");
                }
                else
                {
                    stringBuilder.Append("<a href=\"");
                    stringBuilder.Append(url);
                    stringBuilder.Append(pagetag);
                    stringBuilder.Append("=");
                    stringBuilder.Append(i);
                    if (anchor != null)
                    {
                        stringBuilder.Append(anchor);
                    }
                    stringBuilder.Append("\">");
                    stringBuilder.Append(i);
                    stringBuilder.Append("</a>");
                }
            }
            stringBuilder.Append(text2);
            return stringBuilder.ToString();
        }

        public static string HtmlEncode(string str)
        {
            return HttpUtility.HtmlEncode(str);
        }

        public static string HtmlDecode(string str)
        {
            return HttpUtility.HtmlDecode(str);
        }

        public static string UrlEncode(string str)
        {
            return HttpUtility.UrlEncode(str);
        }

        public static string UrlDecode(string str)
        {
            return HttpUtility.UrlDecode(str);
        }

        //public static string PHPUrlEncode(string oriString)
        //{
        //    StringBuilder stringBuilder = new StringBuilder();
        //    for (int i = 0; i < oriString.Length; i++)
        //    {
        //        char c = oriString[i];
        //        if ("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.".IndexOf(c) != -1)
        //        {
        //            stringBuilder.Append(c);
        //        }
        //        else
        //        {
        //            if (c == ' ')
        //            {
        //                stringBuilder.Append("+");
        //            }
        //            else
        //            {
        //                byte[] bytes = Encoding.UTF8.GetBytes(c.ToString());
        //                byte[] array = bytes;
        //                for (int j = 0; j < array.Length; j++)
        //                {
        //                    byte value = array[j];
        //                    stringBuilder.Append('%' + Convert.ToString(value, 16).ToUpper());
        //                }
        //            }
        //        }
        //    }
        //    return stringBuilder.ToString();
        //}

        //public static string[] FindNoUTF8File(string Path)
        //{
        //    StringBuilder stringBuilder = new StringBuilder();
        //    DirectoryInfo directoryInfo = new DirectoryInfo(Path);
        //    FileInfo[] files = directoryInfo.GetFiles();
        //    for (int i = 0; i < files.Length; i++)
        //    {
        //        if (files[i].Extension.ToLower().Equals(".htm"))
        //        {
        //            FileStream fileStream = new FileStream(files[i].FullName, FileMode.Open, FileAccess.Read);
        //            bool flag = IsUTF8(fileStream);
        //            fileStream.Close();
        //            if (!flag)
        //            {
        //                stringBuilder.Append(files[i].FullName);
        //                stringBuilder.Append("\r\n");
        //            }
        //        }
        //    }
        //    return SplitString(stringBuilder.ToString(), "\r\n");
        //}

        //private static bool IsUTF8(FileStream sbInputStream)
        //{
        //    bool flag = true;
        //    long length = sbInputStream.Length;
        //    byte b = 0;
        //    int num = 0;
        //    while ((long)num < length)
        //    {
        //        byte b2 = (byte)sbInputStream.ReadByte();
        //        if ((b2 & 128) != 0)
        //        {
        //            flag = false;
        //        }
        //        if (b == 0)
        //        {
        //            if (b2 >= 128)
        //            {
        //                do
        //                {
        //                    b2 = (byte)(b2 << 1);
        //                    b += 1;
        //                }
        //                while ((b2 & 128) != 0);
        //                b -= 1;
        //                if (b == 0)
        //                {
        //                    return false;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            if ((b2 & 192) != 128)
        //            {
        //                return false;
        //            }
        //            b -= 1;
        //        }
        //        num++;
        //    }
        //    return b <= 0 && !flag;
        //}

        //public static string FormatBytesStr(int bytes)
        //{
        //    if (bytes > 1073741824)
        //    {
        //        return ((double)(bytes / 1073741824)).ToString("0") + "G";
        //    }
        //    if (bytes > 1048576)
        //    {
        //        return ((double)(bytes / 1048576)).ToString("0") + "M";
        //    }
        //    if (bytes > 1024)
        //    {
        //        return ((double)(bytes / 1024)).ToString("0") + "K";
        //    }
        //    return bytes.ToString() + "Bytes";
        //}

        //public static int SafeInt32(object objNum)
        //{
        //    if (objNum == null)
        //    {
        //        return 0;
        //    }
        //    string text = objNum.ToString();
        //    if (!IsNumeric(text))
        //    {
        //        return 0;
        //    }
        //    if (text.ToString().Length <= 9)
        //    {
        //        return int.Parse(text);
        //    }
        //    if (text.StartsWith("-"))
        //    {
        //        return -2147483648;
        //    }
        //    return 2147483647;
        //}

        //public static int StrDateDiffSeconds(string time, int Sec)
        //{
        //    if (StrIsNullOrEmpty(time))
        //    {
        //        return 1;
        //    }
        //    DateTime dateTime = TypeConverter.StrToDateTime(time, DateTime.Parse("1900-01-01"));
        //    if (dateTime.ToString("yyyy-MM-dd") == "1900-01-01")
        //    {
        //        return 1;
        //    }
        //    TimeSpan timeSpan = DateTime.Now - dateTime.AddSeconds((double)Sec);
        //    if (timeSpan.TotalSeconds > 2147483647.0)
        //    {
        //        return 2147483647;
        //    }
        //    if (timeSpan.TotalSeconds < -2147483648.0)
        //    {
        //        return -2147483648;
        //    }
        //    return (int)timeSpan.TotalSeconds;
        //}

        //public static int StrDateDiffMinutes(string time, int minutes)
        //{
        //    if (StrIsNullOrEmpty(time))
        //    {
        //        return 1;
        //    }
        //    DateTime dateTime = TypeConverter.StrToDateTime(time, DateTime.Parse("1900-01-01"));
        //    if (dateTime.ToString("yyyy-MM-dd") == "1900-01-01")
        //    {
        //        return 1;
        //    }
        //    TimeSpan timeSpan = DateTime.Now - dateTime.AddMinutes((double)minutes);
        //    if (timeSpan.TotalMinutes > 2147483647.0)
        //    {
        //        return 2147483647;
        //    }
        //    if (timeSpan.TotalMinutes < -2147483648.0)
        //    {
        //        return -2147483648;
        //    }
        //    return (int)timeSpan.TotalMinutes;
        //}

        //public static int StrDateDiffHours(string time, int hours)
        //{
        //    if (StrIsNullOrEmpty(time))
        //    {
        //        return 1;
        //    }
        //    DateTime dateTime = TypeConverter.StrToDateTime(time, DateTime.Parse("1900-01-01"));
        //    if (dateTime.ToString("yyyy-MM-dd") == "1900-01-01")
        //    {
        //        return 1;
        //    }
        //    TimeSpan timeSpan = DateTime.Now - dateTime.AddHours((double)hours);
        //    if (timeSpan.TotalHours > 2147483647.0)
        //    {
        //        return 2147483647;
        //    }
        //    if (timeSpan.TotalHours < -2147483648.0)
        //    {
        //        return -2147483648;
        //    }
        //    return (int)timeSpan.TotalHours;
        //}

        public static bool CreateDir(string name)
        {
            //return MakeSureDirectoryPathExists(name);
            try
            {
                Directory.CreateDirectory(name);
                return true;
            }
            catch (Exception ex)
            {
                XTrace.WriteException(ex);
                return false;
            }
        }

        public static string ReplaceStrToScript(string str)
        {
            return str.Replace("\\", "\\\\").Replace("'", "\\'").Replace("\"", "\\\"");
        }

        public static bool IsIP(string ip)
        {
            return Regex.IsMatch(ip, "^((2[0-4]\\d|25[0-5]|[01]?\\d\\d?)\\.){3}(2[0-4]\\d|25[0-5]|[01]?\\d\\d?)$");
        }

        public static bool IsIPSect(string ip)
        {
            return Regex.IsMatch(ip, "^((2[0-4]\\d|25[0-5]|[01]?\\d\\d?)\\.){2}((2[0-4]\\d|25[0-5]|[01]?\\d\\d?|\\*)\\.)(2[0-4]\\d|25[0-5]|[01]?\\d\\d?|\\*)$");
        }

        public static bool InIPArray(string ip, string[] iparray)
        {
            string[] array = SplitString(ip, ".");
            for (int i = 0; i < iparray.Length; i++)
            {
                string[] array2 = SplitString(iparray[i], ".");
                int num = 0;
                for (int j = 0; j < array2.Length; j++)
                {
                    if (array2[j] == "*")
                    {
                        return true;
                    }
                    if (array.Length <= j || !(array2[j] == array[j]))
                    {
                        break;
                    }
                    num++;
                }
                if (num == 4)
                {
                    return true;
                }
            }
            return false;
        }

        public static string GetAssemblyCopyright()
        {
            //return AssemblyFileVersion.LegalCopyright;
            return "2012-{0}, NewLife".F(DateTime.Now.Year);
        }

        public static void WriteCookie(string strName, string strValue)
        {
            HttpCookie httpCookie = HttpContext.Current.Request.Cookies[strName];
            if (httpCookie == null)
            {
                httpCookie = new HttpCookie(strName);
            }
            httpCookie.Value = strValue;
            HttpContext.Current.Response.AppendCookie(httpCookie);
        }

        //public static void WriteCookie(string strName, string key, string strValue)
        //{
        //    HttpCookie httpCookie = HttpContext.Current.Request.Cookies[strName];
        //    if (httpCookie == null)
        //    {
        //        httpCookie = new HttpCookie(strName);
        //    }
        //    httpCookie[key] = strValue;
        //    HttpContext.Current.Response.AppendCookie(httpCookie);
        //}

        public static void WriteCookie(string strName, string strValue, int expires)
        {
            HttpCookie httpCookie = HttpContext.Current.Request.Cookies[strName];
            if (httpCookie == null)
            {
                httpCookie = new HttpCookie(strName);
            }
            httpCookie.Value = strValue;
            httpCookie.Expires = DateTime.Now.AddMinutes((double)expires);
            HttpContext.Current.Response.AppendCookie(httpCookie);
        }

        public static string GetCookie(string name)
        {
            var context = HttpContext.Current;
            if (context == null) return "";
            var req = context.Request;
            if (req == null || req.Cookies == null) return "";

            if (req.Cookies != null && req.Cookies[name] != null)
            {
                return req.Cookies[name].Value.ToString();
            }
            return "";
        }

        public static string GetCookie(string strName, string key)
        {
            var context = HttpContext.Current;
            if (context == null) return "";
            var req = context.Request;
            if (req == null || req.Cookies == null) return "";

            if (req.Cookies != null && req.Cookies[strName] != null && req.Cookies[strName][key] != null)
            {
                return req.Cookies[strName][key].ToString();
            }
            return "";
        }

        public static bool IsDateString(string str)
        {
            if (String.IsNullOrEmpty(str)) return false;

            return Regex.IsMatch(str, "(\\d{4})-(\\d{1,2})-(\\d{1,2})");
        }

        public static string RemoveHtml(string content)
        {
            if (String.IsNullOrEmpty(content)) return String.Empty;

            return Regex.Replace(content, "<[^>]*>", string.Empty, RegexOptions.IgnoreCase);
        }

        public static string RemoveUnsafeHtml(string content)
        {
            content = Regex.Replace(content, "(\\<|\\s+)o([a-z]+\\s?=)", "$1$2", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, "(script|frame|form|meta|behavior|style)([\\s|:|>])+", "$1.$2", RegexOptions.IgnoreCase);
            return content;
        }

        public static string RemoveFontTag(string title)
        {
            Match match = RegexFont.Match(title);
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            return title;
        }

        public static bool IsNumeric(object Expression)
        {
            Int32 n = 0;
            return Int32.TryParse(Expression + "", out n);
        }

        public static int StrToInt(string expression, int defValue)
        {
            return expression.ToInt(defValue);
        }

        public static bool IsRuleTip(Hashtable NewHash, string ruletype, out string key)
        {
            key = "";
            foreach (DictionaryEntry dictionaryEntry in NewHash)
            {
                try
                {
                    string[] array = SplitString(dictionaryEntry.Value.ToString(), "\r\n");
                    string[] array2 = array;
                    for (int i = 0; i < array2.Length; i++)
                    {
                        string text = array2[i];
                        string a;
                        if (text != "" && (a = ruletype.Trim().ToLower()) != null)
                        {
                            if (!(a == "email"))
                            {
                                if (!(a == "ip"))
                                {
                                    if (a == "timesect")
                                    {
                                        string[] array3 = text.Split('-');
                                        if (!IsTime(array3[1].ToString()) || !IsTime(array3[0].ToString()))
                                        {
                                            throw new Exception();
                                        }
                                    }
                                }
                                else
                                {
                                    if (!IsIPSect(text.ToString()))
                                    {
                                        throw new Exception();
                                    }
                                }
                            }
                            else
                            {
                                if (!IsValidDoEmail(text.ToString()))
                                {
                                    throw new Exception();
                                }
                            }
                        }
                    }
                }
                catch
                {
                    key = dictionaryEntry.Key.ToString();
                    return false;
                }
            }
            return true;
        }

        //public static string ClearLastChar(string str)
        //{
        //    if (String.IsNullOrEmpty(str)) return String.Empty;

        //    return str.Substring(0, str.Length - 1);
        //}

        public static bool BackupFile(string sourceFileName, string destFileName, bool overwrite)
        {
            if (!File.Exists(sourceFileName))
            {
                throw new FileNotFoundException(sourceFileName + "文件不存在！");
            }
            if (!overwrite && File.Exists(destFileName))
            {
                return false;
            }
            bool result;
            try
            {
                File.Copy(sourceFileName, destFileName, true);
                result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public static bool BackupFile(string sourceFileName, string destFileName)
        {
            return BackupFile(sourceFileName, destFileName, true);
        }

        public static bool RestoreFile(string backupFileName, string targetFileName, string backupTargetFileName)
        {
            try
            {
                if (!File.Exists(backupFileName))
                {
                    throw new FileNotFoundException(backupFileName + "文件不存在！");
                }
                if (backupTargetFileName != null)
                {
                    if (!File.Exists(targetFileName))
                    {
                        throw new FileNotFoundException(targetFileName + "文件不存在！无法备份此文件！");
                    }
                    File.Copy(targetFileName, backupTargetFileName, true);
                }
                File.Delete(targetFileName);
                File.Copy(backupFileName, targetFileName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        public static bool RestoreFile(string backupFileName, string targetFileName)
        {
            return RestoreFile(backupFileName, targetFileName, null);
        }

        public static string GetTemplateCookieName()
        {
            //return TemplateCookieName;

            var asm = Assembly.GetExecutingAssembly();
            var ver = asm.GetName().Version;
            return "bbxtemplateid_{0}_{1}_{2}_{3}".F(ver.Major, ver.Minor, ver.Build, ver.Revision);
        }

        public static string SBCCaseToNumberic(string SBCCase)
        {
            char[] array = SBCCase.ToCharArray();
            for (int i = 0; i < array.Length; i++)
            {
                byte[] bytes = Encoding.Unicode.GetBytes(array, i, 1);
                if (bytes.Length == 2 && bytes[1] == 255)
                {
                    bytes[0] += 32;
                    bytes[1] = 0;
                    array[i] = Encoding.Unicode.GetChars(bytes)[0];
                }
            }
            return new string(array);
        }

        public static Color ToColor(string color)
        {
            color = color.TrimStart('#');
            color = Regex.Replace(color.ToLower(), "[g-zG-Z]", "");
            int length = color.Length;
            char[] array;
            int red;
            int green;
            int blue;
            if (length == 3)
            {
                array = color.ToCharArray();
                red = Convert.ToInt32(array[0].ToString() + array[0].ToString(), 16);
                green = Convert.ToInt32(array[1].ToString() + array[1].ToString(), 16);
                blue = Convert.ToInt32(array[2].ToString() + array[2].ToString(), 16);
                return Color.FromArgb(red, green, blue);
            }
            if (length != 6)
            {
                return Color.FromName(color);
            }
            array = color.ToCharArray();
            red = Convert.ToInt32(array[0].ToString() + array[1].ToString(), 16);
            green = Convert.ToInt32(array[2].ToString() + array[3].ToString(), 16);
            blue = Convert.ToInt32(array[4].ToString() + array[5].ToString(), 16);
            return Color.FromArgb(red, green, blue);
        }

        public static string ConvertSimpleFileName(string fullname, string repstring, int leftnum, int rightnum, int charnum)
        {
            string fileExtName = GetFileExtName(fullname);
            if (fileExtName.IsNullOrEmpty()) return fullname;

            int num = fullname.LastIndexOf('.');
            string text = fullname.Substring(0, num);
            int length = text.Length;
            string result;
            if (num > charnum)
            {
                string text2 = text.Substring(0, leftnum);
                string text3 = text.Substring(length - rightnum, rightnum);
                if (String.IsNullOrEmpty(repstring))
                    result = text2 + text3 + "." + fileExtName;
                else
                    result = text2 + repstring + text3 + "." + fileExtName;
            }
            else
            {
                result = fullname;
            }
            return result;
        }

        public static StringBuilder DataTableToJSON(DataTable dt)
        {
            return DataTableToJson(dt, true);
        }

        public static StringBuilder DataTableToJson(DataTable dt, bool dt_dispose)
        {
            var sb = new StringBuilder();
            sb.Append("[\r\n");
            string[] array = new string[dt.Columns.Count];
            int num = 0;
            string text = "{{";
            foreach (DataColumn dataColumn in dt.Columns)
            {
                array[num] = dataColumn.Caption.ToLower().Trim();
                text = text + "'" + dataColumn.Caption.ToLower().Trim() + "':";
                string text2 = dataColumn.DataType.ToString().Trim().ToLower();
                if (text2.IndexOf("int") > 0 || text2.IndexOf("deci") > 0 || text2.IndexOf("floa") > 0 || text2.IndexOf("doub") > 0 || text2.IndexOf("bool") > 0)
                {
                    object obj = text;
                    text = obj + "{" + num + "}";
                }
                else
                {
                    object obj2 = text;
                    text = obj2 + "'{" + num + "}'";
                }
                text += ",";
                num++;
            }
            if (text.EndsWith(","))
            {
                text = text.Substring(0, text.Length - 1);
            }
            text += "}},";
            num = 0;
            object[] array2 = new object[array.Length];
            foreach (DataRow dataRow in dt.Rows)
            {
                string[] array3 = array;
                for (int i = 0; i < array3.Length; i++)
                {
                    string arg_1EF_0 = array3[i];
                    array2[num] = dataRow[array[num]].ToString().Trim().Replace("\\", "\\\\").Replace("'", "\\'");
                    string a;
                    if ((a = array2[num].ToString()) != null)
                    {
                        if (!(a == "True"))
                        {
                            if (a == "False")
                            {
                                array2[num] = "false";
                            }
                        }
                        else
                        {
                            array2[num] = "true";
                        }
                    }
                    num++;
                }
                num = 0;
                sb.Append(string.Format(text, array2));
                sb.AppendLine();
            }
            if (sb.ToString().EndsWith(",\r\n"))
            {
                sb.Remove(sb.Length - 3, 3);
            }
            if (dt_dispose)
            {
                dt.Dispose();
            }
            return sb.Append("\r\n];");
        }

        public static bool StrIsNullOrEmpty(string str)
        {
            return str == null || str.Trim() == string.Empty;
        }

        public static bool IsNumericList(string numList)
        {
            //return !StrIsNullOrEmpty(numList) && IsNumericArray(numList.Split(','));
            return !numList.IsNullOrWhiteSpace() && numList.SplitAsInt(",").Length > 0;
        }

        //public static bool CheckColorValue(string color)
        //{
        //    if (StrIsNullOrEmpty(color))
        //    {
        //        return false;
        //    }
        //    color = color.Trim().Trim('#');
        //    return (color.Length == 3 || color.Length == 6) && !Regex.IsMatch(color, "[^0-9a-f]", RegexOptions.IgnoreCase);
        //}

        public static string GetAjaxPageNumbers(int curPage, int countPage, string callback, int extendPage)
        {
            string text = "page";
            int num = 1;
            string text2 = "<a href=\"###\" onclick=\"" + string.Format(callback, "&" + text + "=1");
            string text3 = "<a href=\"###\" onclick=\"" + string.Format(callback, "&" + text + "=" + countPage);
            text2 += "\">&laquo;</a>";
            text3 += "\">&raquo;</a>";
            if (countPage < 1)
            {
                countPage = 1;
            }
            if (extendPage < 3)
            {
                extendPage = 2;
            }
            int num2;
            if (countPage > extendPage)
            {
                if (curPage - extendPage / 2 > 0)
                {
                    if (curPage + extendPage / 2 < countPage)
                    {
                        num = curPage - extendPage / 2;
                        num2 = num + extendPage - 1;
                    }
                    else
                    {
                        num2 = countPage;
                        num = num2 - extendPage + 1;
                        text3 = "";
                    }
                }
                else
                {
                    num2 = extendPage;
                    text2 = "";
                }
            }
            else
            {
                num = 1;
                num2 = countPage;
                text2 = "";
                text3 = "";
            }
            StringBuilder stringBuilder = new StringBuilder("");
            stringBuilder.Append(text2);
            for (int i = num; i <= num2; i++)
            {
                if (i == curPage)
                {
                    stringBuilder.Append("<span>");
                    stringBuilder.Append(i);
                    stringBuilder.Append("</span>");
                }
                else
                {
                    stringBuilder.Append("<a href=\"###\" onclick=\"");
                    stringBuilder.Append(string.Format(callback, text + "=" + i));
                    stringBuilder.Append("\">");
                    stringBuilder.Append(i);
                    stringBuilder.Append("</a>");
                }
            }
            stringBuilder.Append(text3);
            return stringBuilder.ToString();
        }

        public static string GetSourceTextByUrl(string url)
        {
            string result;
            try
            {
                WebRequest webRequest = WebRequest.Create(url);
                webRequest.Timeout = 20000;
                WebResponse response = webRequest.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader streamReader = new StreamReader(responseStream);
                result = streamReader.ReadToEnd();
            }
            catch
            {
                result = "";
            }
            return result;
        }

        public static double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime d = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Math.Floor((date - d).TotalSeconds);
        }

        public static string JsonCharFilter(string sourceStr)
        {
            sourceStr = sourceStr.Replace("\\", "\\\\");
            sourceStr = sourceStr.Replace("\b", "\\\b");
            sourceStr = sourceStr.Replace("\t", "\\\t");
            sourceStr = sourceStr.Replace("\n", "\\\n");
            sourceStr = sourceStr.Replace("\n", "\\\n");
            sourceStr = sourceStr.Replace("\f", "\\\f");
            sourceStr = sourceStr.Replace("\r", "\\\r");
            return sourceStr.Replace("\"", "\\\"");
        }

        //public static string MergeString(string source, string target)
        //{
        //    return MergeString(source, target, ",");
        //}

        //public static string MergeString(string source, string target, string mergechar)
        //{
        //    if (StrIsNullOrEmpty(target))
        //    {
        //        target = source;
        //    }
        //    else
        //    {
        //        target = target + mergechar + source;
        //    }
        //    return target;
        //}

        public static string ClearUBB(string sDetail)
        {
            return Regex.Replace(sDetail, "\\[[^\\]]*?\\]", string.Empty, RegexOptions.IgnoreCase);
        }

        public static string GetRootUrl(string forumPath)
        {
            return string.Format("{0}://{1}{2}", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Authority, forumPath);
        }

        public static string GetFileExtName(string fileName)
        {
            if (fileName.IsNullOrEmpty() || fileName.IndexOf('.') <= 0)
            {
                return "";
            }
            fileName = fileName.ToLower().Trim();
            return fileName.Substring(fileName.LastIndexOf('.'), fileName.Length - fileName.LastIndexOf('.'));
        }

        public static string GetHttpWebResponse(string url)
        {
            return GetHttpWebResponse(url, "POST", string.Empty);
        }

        //public static string GetHttpWebResponse(string url, string postData)
        //{
        //    return GetHttpWebResponse(url, "POST", postData);
        //}

        public static string GetHttpWebResponse(string url, string method, string postData)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Method = method;
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            httpWebRequest.ContentLength = (long)(string.IsNullOrEmpty(postData) ? 0 : postData.Length);
            httpWebRequest.Timeout = 60000;
            HttpWebResponse httpWebResponse = null;
            string result;
            try
            {
                if (!string.IsNullOrEmpty(postData))
                {
                    StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream());
                    streamWriter.Write(postData);
                    if (streamWriter != null)
                    {
                        streamWriter.Close();
                    }
                }
                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.UTF8))
                {
                    result = streamReader.ReadToEnd();
                }
            }
            catch
            {
                result = null;
            }
            finally
            {
                if (httpWebResponse != null)
                {
                    httpWebResponse.Close();
                }
            }
            return result;
        }

        //public static T GetEnum<T>(string value, T defValue)
        //{
        //    T result;
        //    try
        //    {
        //        result = (T)((object)Enum.Parse(typeof(T), value, true));
        //    }
        //    catch (ArgumentException)
        //    {
        //        result = defValue;
        //    }
        //    return result;
        //}

        public static string FormatDate(int date, bool chnType)
        {
            string text = date.ToString();
            if (date <= 0 || text.Length != 8)
            {
                return text;
            }
            if (chnType)
            {
                return text.Substring(0, 4) + "年" + text.Substring(4, 2) + "月" + text.Substring(6) + "日";
            }
            return text.Substring(0, 4) + "-" + text.Substring(4, 2) + "-" + text.Substring(6);
        }

        public static string FormatDate(int date)
        {
            return FormatDate(date, false);
        }

        public static string GetClientBrower()
        {
            string text = HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"];
            if (!string.IsNullOrEmpty(text))
            {
                string[] array = browerNames;
                for (int i = 0; i < array.Length; i++)
                {
                    string text2 = array[i];
                    if (text.Contains(text2))
                    {
                        return text2;
                    }
                }
            }
            return "Other";
        }

        public static string GetClientOS()
        {
            string result = string.Empty;
            string text = HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"];
            if (text == null)
            {
                return "Other";
            }
            if (text.IndexOf("Win") > -1)
            {
                result = "Windows";
            }
            else
            {
                if (text.IndexOf("Mac") > -1)
                {
                    result = "Mac";
                }
                else
                {
                    if (text.IndexOf("Linux") > -1)
                    {
                        result = "Linux";
                    }
                    else
                    {
                        if (text.IndexOf("FreeBSD") > -1)
                        {
                            result = "FreeBSD";
                        }
                        else
                        {
                            if (text.IndexOf("SunOS") > -1)
                            {
                                result = "SunOS";
                            }
                            else
                            {
                                if (text.IndexOf("OS/2") > -1)
                                {
                                    result = "OS/2";
                                }
                                else
                                {
                                    if (text.IndexOf("AIX") > -1)
                                    {
                                        result = "AIX";
                                    }
                                    else
                                    {
                                        if (Regex.IsMatch(text, "(Bot|Crawl|Spider)"))
                                        {
                                            result = "Spiders";
                                        }
                                        else
                                        {
                                            result = "Other";
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        public static void RestartIISProcess()
        {
            try
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.Load(GetMapPath("~/web.config"));
                var xmlTextWriter = new XmlTextWriter(GetMapPath("~/web.config"), null);
                xmlTextWriter.Formatting = Formatting.Indented;
                xmlDocument.WriteTo(xmlTextWriter);
                xmlTextWriter.Flush();
                xmlTextWriter.Close();
            }
            catch
            {
            }
        }

        //public static bool IsIE()
        //{
        //    return HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"].IndexOf("MSIE") >= 0;
        //}

        //public static int BKDEHash(string str, int hashKey)
        //{
        //    str = ((str.Length > 1000) ? str.Substring(0, 1000) : str);
        //    byte[] bytes = Encoding.Default.GetBytes(str);
        //    int num = 0;
        //    byte[] array = bytes;
        //    for (int i = 0; i < array.Length; i++)
        //    {
        //        byte value = array[i];
        //        num = num + hashKey + value.ToInt();
        //    }
        //    return num;
        //}
    }
}