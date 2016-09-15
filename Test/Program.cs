using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using BBX.Entity;
using NewLife.Threading;
using XCode.DataAccessLayer;
using System.Linq;
using XCode;
using XCode.Transform;
using NewLife.Log;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            XTrace.UseConsole();
            while (true)
            {
#if !DEBUG
                try
                {
#endif
                Test5();
#if !DEBUG
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
#endif

                Console.WriteLine("OK!");
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.C) break;
            }
        }

        static void Test1()
        {
            //var file = @"..\Src\BBX.Forum\PageBase.cs";
            var file = @"..\Src\BBX.Control\TabPage.cs";
            var txt = File.ReadAllText(file);

            var b = false;
            //b |= StrSingle(ref txt);
            //b |= StrConcat(ref txt);
            //b |= RepCacheKey(ref txt);
            b |= FixProperty(ref txt);
            if (b)
            {
                File.WriteAllText(file, txt);
            }
        }

        static void Test2()
        {
            var path = @"..\Src";
            var fs = Directory.GetFiles(path, "*.cs", SearchOption.AllDirectories);
            for (int i = 0; i < fs.Length; i++)
            {
                var file = fs[i];
                Console.WriteLine("{0}/{1} {2}", i + 1, fs.Length, file);
                if (file.EndsWith("Program.cs")) continue;

                var txt = File.ReadAllText(file);

                var b = false;
                //b |= StrSingle(ref txt);
                //b |= StrConcat(ref txt);
                //b |= RepCacheKey(ref txt);
                //b |= RepairLock(ref txt);
                b |= FixProperty(ref txt);
                if (b) File.WriteAllText(file, txt);
            }
        }

        #region String
        //static Regex _regStrSplit = new Regex(@"\.Split\(new\s*char\[\]\s*\{\s*'([^'])'\s*\}\)", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
        //static Regex _regStrTrim = new Regex(@"\.Trim\(new\s*char\[\]\s*\{\s*'([^'])'\s*\}\)", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

        static String[] _strFunc = new String[] { "Split", "Trim", "TrimStart", "TrimEnd" };

        static Boolean StrSingle(ref String content)
        {
            if (content.IsNullOrWhiteSpace()) return false;

            var b = false;
            var str = content;
            foreach (var item in _strFunc)
            {
                var reg = new Regex(@"\." + item + @"\(new\s*char\[\]\s*\{\s*'([^']+)'\s*\}\)", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
                if (reg.IsMatch(str))
                {
                    b = true;
                    str = reg.Replace(str, "." + item + "('$1')");
                }
            }
            if (!b) return false;
            content = str;

            return true;
        }
        #endregion

        #region String.Concat
        /// <summary>用于匹配string.Concat，普通单行匹配，因为有些行是字符串，本身含有括号</summary>
        static Regex _regStrConcat1 = new Regex(
@"string\.Concat\(new\s+(?:string|object)\[\]\s*\{(?:\s*([^\n]+?),\r?\n)*\s*([^\n]+?)\r?\n\s*\}\)", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
        /// <summary>用于匹配string.Concat，采用正则平衡组</summary>
        static Regex _regStrConcat2 = new Regex(
@"string\.Concat\(new\s+(?:string|object)\[\]\s+\{
(?:\s*
    (
        (?:[^\n{}()]     #不包含大小括号
        |\{(?<Open>)    #遇到左大括号Open计数加1
        |\}(?<-Open>)   #遇到右大括号Open计数减1
        |\((?<Open2>)   #同理处理小括号
        |\)(?<-Open2>)
        )+?           #任意多个符合条件的字符
    ),            #任意多个符合条件的字符
    (?(Open)(?!))    #判断是否还有'OPEN'，有则说明不配对，什么都不匹配
    (?(Open2)(?!))   #同理处理小括号
)*
\s*
((?:[^\n{}]|\{(?<Open3>)|\}(?<-Open3>))+?)(?(Open3)(?!))
\s*\}\)", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
        static Boolean StrConcat(ref String content)
        {
            if (content.IsNullOrWhiteSpace()) return false;

            if (!content.Contains("tring.Concat(")) return false;

            var str = "";
            var sb = new StringBuilder();
            var i = 0;

            foreach (Match item in _regStrConcat1.Matches(content))
            {
                // 加上前面的
                if (item.Index > i)
                {
                    sb.Append(content.Substring(i, item.Index - i));
                }

                foreach (Capture cp in item.Groups[1].Captures)
                {
                    str = cp.Value;
                    if (str.Contains(" ? ") && str.Contains(" : ") || str.Contains(" - ") || str.Contains(" + "))
                        sb.AppendFormat("({0}) + ", str);
                    else
                        sb.AppendFormat("{0} + ", str);
                }
                //sb.Append(item.Groups[2].ToString());
                str = item.Groups[2].ToString();
                if (str.Contains(" ? ") && str.Contains(" : ") || str.Contains(" - ") || str.Contains(" + "))
                    sb.AppendFormat("({0})", str);
                else
                    sb.Append(str);

                i = item.Index + item.Length;
            }
            if (i < content.Length - 1) sb.Append(content.Substring(i));
            content = sb.ToString();

            sb = new StringBuilder();
            i = 0;

            foreach (Match item in _regStrConcat2.Matches(content))
            {
                // 加上前面的
                if (item.Index > i)
                {
                    sb.Append(content.Substring(i, item.Index - i));
                }

                foreach (Capture cp in item.Groups[1].Captures)
                {
                    str = cp.Value;
                    if (str.Contains(" ? ") && str.Contains(" : ") || str.Contains(" - ") || str.Contains(" + "))
                        sb.AppendFormat("({0}) + ", str);
                    else
                        sb.AppendFormat("{0} + ", str);
                }
                //sb.Append(item.Groups[2].ToString());
                str = item.Groups[2].ToString();
                if (str.Contains(" ? ") && str.Contains(" : ") || str.Contains(" - ") || str.Contains(" + "))
                    sb.AppendFormat("({0})", str);
                else
                    sb.Append(str);

                i = item.Index + item.Length;
            }
            if (i < content.Length - 1) sb.Append(content.Substring(i));
            content = sb.ToString();

            return true;
        }
        #endregion

        #region 缓存常量
        static Dictionary<String, String> cacheKeys;
        static Boolean RepCacheKey(ref String content)
        {
            if (content.Contains("public class CacheKeys")) return false;

            if (cacheKeys == null)
            {
                cacheKeys = new Dictionary<string, string>();
                foreach (var item in File.ReadAllLines(@"..\Src\BBX.Cache\CacheKeys.cs"))
                {
                    if (String.IsNullOrEmpty(item)) continue;
                    var p = item.IndexOf("public const string");
                    if (p < 0) continue;

                    var str = item.Substring(p + "public const string".Length).Trim();
                    var ss = str.Split('=');
                    cacheKeys.Add("CacheKeys." + ss[0].Trim(), ss[1].Trim().Trim(';').Trim());
                }
            }

            var bak = content;
            foreach (var item in cacheKeys)
            {
                content = content.Replace(item.Value, item.Key);
            }

            return bak != content;
        }
        #endregion

        #region 修正lock
        static Regex _regLock = new Regex(
@"\w+\s+(\w+);\s*
Monitor\.Enter\(\1\s*=\s*([^)]+)\);(\s*)(.*?\s*)?
try\s*
 ({\s*(?:[^{}]|\{(?<Open>)|\}(?<-Open>))+?\s*})(?(Open)(?!))
\s*finally\s*{\s*Monitor\.Exit\(\1\);\s*}", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline | RegexOptions.Compiled);
        static Boolean RepairLock(ref String content)
        {
            if (content.IsNullOrWhiteSpace()) return false;

            if (!content.Contains("Monitor.Enter")) return false;

            if (!_regLock.IsMatch(content)) return false;

            content = _regLock.Replace(content, "$4lock($2)$3$5");

            return true;
        }
        #endregion

        #region 修正属性
        const String DELETED = "// 删除";
        static Regex _regProperty = new Regex(@"private (\w+) (\w+)( = [^\n]*)?;\r\n[ \t]*(.*[^\]])(\r\n[ \t]*\[[^\]]+\])*\r\n([ \t]*)((?:public|protect|internal|private) \1\s+\w+ {(?: get { return (?:this\.)?\2; })?(?: set { \2 = value; })? })", RegexOptions.Singleline | RegexOptions.Compiled);
        //static Regex _regProperty = new Regex(@"private (\w+) (\w+)( = [^\n]*)?;\r?\n[ \t]*(.*)\r?\n([ \t]*)(public \1 \w+)", RegexOptions.Singleline | RegexOptions.Compiled);

        static Boolean FixProperty(ref String txt)
        {
            if (txt.IsNullOrWhiteSpace()) return false;

            var changed = false;

            var ss = txt.Split(new String[] { Environment.NewLine }, StringSplitOptions.None);
            for (int i = 0; i < ss.Length; i++)
            {
                var item = ss[i].Trim();
                #region 处理get
                if (item == "get" && i + 3 < ss.Length)
                {
                    if (ss[i + 1].Trim() == "{" && ss[i + 3].Trim() == "}")
                    {
                        // return语句太长的不处理
                        var ret = ss[i + 2].Trim();
                        if (ret.Length <= 80)
                        {
                            if (ret.StartsWith("return this.")) ret = "return " + ret.Substring("return this.".Length);
                            ss[i] = ss[i].TrimEnd() + " { " + ret + " }";
                            ss[++i] = DELETED;
                            ss[++i] = DELETED;
                            ss[++i] = DELETED;
                            changed = true;

                            #region 如果只有get，二次折叠
                            if (ss[i + 1].Trim() == "}" && i >= 5 && ss[i - 4].Trim() == "{")
                            {
                                ss[i - 5] = ss[i - 5].TrimEnd() + " { " + ss[i - 3].TrimStart() + " } ";
                                ss[i - 4] = DELETED; // {
                                ss[i - 3] = DELETED; // get { return xxx; }
                                ss[++i] = DELETED;   // }
                            }
                            #endregion
                        }
                    }
                }
                #endregion
                #region 处理set
                else if (item == "set" && i + 3 < ss.Length)
                {
                    if (ss[i + 1].Trim() == "{" && ss[i + 3].Trim() == "}")
                    {
                        // 语句太长的不处理
                        var str = ss[i + 2].Trim();
                        if (str.Length <= 80)
                        {
                            var p = ss[i].IndexOf("set");
                            var blank = p <= 0 ? "" : ss[i].Substring(0, p);

                            if (str.StartsWith("this.")) str = str.Substring("this.".Length);
                            ss[i] = ss[i].TrimEnd() + " { " + str + " }";
                            ss[++i] = DELETED;
                            ss[++i] = DELETED;
                            ss[++i] = DELETED;
                            changed = true;

                            #region 如果只有set，二次折叠
                            if (ss[i + 1].Trim() == "}" && i >= 5 && ss[i - 4].Trim() == "{")
                            {
                                ss[i - 5] = ss[i - 5].TrimEnd() + " { " + ss[i - 3].TrimStart() + " } ";
                                ss[i - 4] = DELETED; // {
                                ss[i - 3] = DELETED; // set { xxx = value; }
                                ss[++i] = DELETED;   // }
                            }
                            #endregion
                        }
                    }
                }
                #endregion
            }

            #region 二次拆分
            if (changed)
            {
                var sb = new StringBuilder();
                foreach (var item in ss)
                {
                    if (item != DELETED) sb.AppendLine(item);
                }
                txt = sb.ToString().Trim();

                ss = txt.Split(new String[] { Environment.NewLine }, StringSplitOptions.None);
            }
            #endregion

            #region 二次折叠
            for (int i = 0; i < ss.Length; i++)
            {
                var item = ss[i].Trim();
                if (i > 0 && i + 3 < ss.Length && item == "{" && (ss[i + 2].Trim() == "}" || ss[i + 3].Trim() == "}"))
                {
                    var next = ss[i + 1].Trim();
                    if (next.Length < 80)
                    {
                        if (next.StartsWith("get") || next.StartsWith("set"))
                        {
                            var next2 = ss[i + 2].Trim();
                            if (next2 == "}")
                            {
                                ss[i - 1] = ss[i - 1].TrimEnd() + " { " + next + " }";
                                ss[i] = DELETED;    // {
                                ss[++i] = DELETED;  // get {
                                ss[++i] = DELETED;  // }

                                changed = true;
                            }
                            else if (ss[i + 3].Trim() == "}")
                            {
                                ss[i - 1] = ss[i - 1].TrimEnd() + " { " + next + " " + next2 + " }";
                                ss[i] = DELETED;    // {
                                ss[++i] = DELETED;  // get {
                                ss[++i] = DELETED;  // set {
                                ss[++i] = DELETED;  // }

                                changed = true;
                            }
                        }
                    }
                }
            }

            if (changed)
            {
                var sb = new StringBuilder();
                foreach (var item in ss)
                {
                    if (item != DELETED) sb.AppendLine(item);
                }
                txt = sb.ToString().Trim();
            }
            #endregion

            #region 字段靠近属性
            while (_regProperty.IsMatch(txt))
            {
                //txt = _regProperty.Replace(txt, "$4\r\n$6private $1 $2$3;$5\r\n$6$7");
                txt = _regProperty.Replace(txt, m =>
                {
                    var sb2 = new StringBuilder();
                    sb2.AppendLine(m.Groups[4].Value);
                    sb2.Append(m.Groups[6].Value);
                    sb2.AppendFormat("private {0} {1}{2};", m.Groups[1].Value, m.Groups[2].Value, m.Groups[3].Value);
                    foreach (var cp in m.Groups[5].Captures)
                    {
                        sb2.Append(cp.ToString());
                    }
                    sb2.AppendLine();
                    sb2.Append(m.Groups[6].Value);
                    sb2.Append(m.Groups[7].Value);

                    return sb2.ToString();
                });
                changed = true;
            }
            #endregion

            return changed;
        }
        #endregion

        static void Test3()
        {
            var file = @"..\Src\BBX.Entity\_Entities\BBX.xml";
            var tables = DAL.Import(File.ReadAllText(file));
            var infs = typeof(User).Assembly.GetTypes()
                .Where(e => !e.IsNested && e.IsPublic && e.Name.EndsWith("Info")).OrderBy(e => e.Name)
                .ToDictionary<Type, String>(e => e.Name.Substring(0, e.Name.Length - 4), StringComparer.OrdinalIgnoreCase);
            //var dds = typeof(BBX.Data.AdminGroups).Assembly.GetTypes()
            //    .Where(e => !e.IsNested && e.IsPublic).OrderBy(e => e.Name)
            //    .ToDictionary<Type, String>(e => e.Name.CutRight("s"), StringComparer.OrdinalIgnoreCase);
            //var dfs = typeof(BBX.Forum.AdminGroups).Assembly.GetTypes()
            //    .Where(e => !e.IsNested && e.IsPublic).OrderBy(e => e.Name)
            //    .ToDictionary<Type, String>(e => e.Name.CutRight("s"), StringComparer.OrdinalIgnoreCase);

            foreach (var tb in tables)
            {
                Console.WriteLine("{0}({1})", tb.Name, tb.Description);

                var name = tb.Name;
                // 有些备注变成了别名，没意思
                if (name.EqualIgnoreCase(tb.Description)) tb.Description = null;
                name = name.CutRight("s");
                //if (!infs.ContainsKey(name)) continue;

                // 采用实体类的类名作为别名
                var inf = infs.ContainsKey(name) ? infs[name] : null;
                if (inf != null) name = inf.Name;
                name = name.CutRight("Info");

                //if (name.EndsWith("log")) name = name.Substring(0, name.Length - 3) + "Log";
                name = name.EnsureEnd("log", "Log");
                name = name.EnsureEnd("tag", "Tag");
                name = name.EnsureEnd("type", "Type");
                name = name.EnsureEnd("code", "Code");
                name = name.EnsureEnd("field", "Field");
                name = name.EnsureEnd("event", "Event");
                name = name.EnsureEnd("cache", "Cache");
                name = name.EnsureEnd("list", "List");
                name = name.EnsureEnd("stat", "Stat");
                name = name.EnsureEnd("reg", "Reg");

                name = name.EnsureStart("forum", "Forum");
                name = name.EnsureStart("moderator", "Moderator");
                name = name.EnsureStart("my", "My");
                name = name.EnsureStart("online", "Online");
                name = name.EnsureStart("post", "Post");
                name = name.EnsureStart("topic", "Topic");

                // 如果被修改过为别的名字，这里别动
                if (tb.Name.EqualIgnoreCase(tb.TableName.CutLeft("dnt_"))) tb.Name = name;

                //var dd = dds[name];
                //var df = dfs[name];

                // 处理字段
                var pis = inf != null ? inf.GetProperties() : null;
                foreach (var dc in tb.Columns)
                {
                    name = dc.Name;
                    if (dc.Identity) name = "ID";

                    // 有些备注变成了别名，没意思
                    if (dc.Name.EqualIgnoreCase(dc.Description)) dc.Description = null;

                    if (pis != null)
                    {
                        var pi = pis.FirstOrDefault(e => e.Name.EqualIgnoreCase(dc.Name));
                        if (pi == null) continue;

                        if (dc.Name != "ID" || !dc.Identity) name = pi.Name;
                        dc.DataType = pi.PropertyType;
                    }

                    if (name.Length > 3) name = name.EnsureEnd("id", "ID");
                    name = name.EnsureEnd("name", "Name");
                    name = name.EnsureEnd("datetime", "DateTime");
                    name = name.EnsureEnd("time", "Time");
                    name = name.EnsureEnd("log", "Log");
                    name = name.EnsureEnd("tag", "Tag");
                    name = name.EnsureEnd("type", "Type");
                    name = name.EnsureEnd("types", "Types");
                    name = name.EnsureEnd("code", "Code");
                    name = name.EnsureEnd("ip", "IP");
                    name = name.EnsureEnd("user", "User");
                    name = name.EnsureEnd("post", "Post");
                    name = name.EnsureEnd("title", "Title");
                    name = name.EnsureEnd("order", "Order");
                    name = name.EnsureEnd("size", "Size");
                    name = name.EnsureEnd("count", "Count");
                    name = name.EnsureEnd("edits", "Edits");
                    name = name.EnsureEnd("perm", "Perm");
                    name = name.EnsureEnd("date", "Date");

                    name = name.EnsureStart("allow", "Allow");
                    name = name.EnsureStart("disable", "Disable");
                    name = name.EnsureStart("is", "Is");
                    name = name.EnsureStart("positive", "Positive");
                    name = name.EnsureStart("negative", "Negative");
                    name = name.EnsureStart("last", "Last");
                    name = name.EnsureStart("total", "Total");

                    // 如果被修改过为别的名字，这里别动
                    if (dc.Name.EqualIgnoreCase(dc.ColumnName)) dc.Name = name;

                    // 修改类型
                    if (dc.Identity) dc.DataType = typeof(Int32);
                    if (name.StartsWith("Allow") || name.StartsWith("Is") || name.StartsWith("Disable"))
                    {
                        dc.DataType = typeof(Boolean);
                        dc.RawType = "bit";
                    }
                    if (dc.DataType == typeof(String))
                    {
                        dc.RawType = dc.RawType
                            .EnsureStart("char", "nvarchar")
                            .EnsureStart("nchar", "nvarchar")
                            .EnsureStart("varchar", "nvarchar")
                            .EnsureStart("text", "ntext");
                    }
                    else if (dc.DataType == typeof(DateTime))
                    {
                        dc.RawType = dc.RawType.EnsureStart("smalldatetime", "datetime");
                    }

                    if (dc.Name == "ID")
                        dc.Description = "编号";
                    else if (dc.Name.Length == 3 && dc.Name.EndsWith("id", StringComparison.OrdinalIgnoreCase))
                        dc.Description = dc.Name[0] + "编号";
                    else if (dc.Name == "Name")
                        dc.Description = "名称";
                    else if (dc.Name == "Code")
                        dc.Description = "代码";
                    else if (dc.Name == "DisplayOrder")
                        dc.Description = "显示顺序";
                    else if (dc.Name == "Value")
                        dc.Description = "值";

                    var tc = Type.GetTypeCode(dc.DataType);
                    if (dc.DataType.IsEnum) dc.DataType = Type.GetType("System." + tc.ToString());
                }
            }

            File.WriteAllText(file, DAL.Export(tables));
        }

        static void Test4()
        {
            var file = @"..\Src\BBX.Entity\_Entities\BBX.xml";
            var tables = DAL.Import(File.ReadAllText(file));
            var table = tables.FirstOrDefault(t => t.Name.EqualIgnoreCase("ForumField"));
            var sb = new StringBuilder();
            foreach (var item in table.Columns)
            {
                //Console.WriteLine(item);
                var name = item.Name;
                name = name.Substring(0, 1).ToUpper() + name.Substring(1);

                sb.AppendFormat("{1} IForumField.{0} {{ get {{ return Field.{0}; }} set {{ Field.{0} = value; }} }}", name, item.DataType.Name);
                //Console.WriteLine();
                sb.AppendLine();
                sb.AppendLine();
            }

            var str = sb.ToString();
            Console.WriteLine(str);
        }

        static void Test5()
        {
            var et = new EntityTransform();
            et.SrcConn = "BBX";
            et.DesConn = "BBX2";
            et.AllowInsertIdentity = true;
            //et.ShowSQL = true;
            //et.Transform();

            foreach (var item in EntityFactory.LoadEntities(et.SrcConn, true))
            {
                var eop = EntityFactory.CreateOperate(item);
                et.TransformTable(eop);
            }
        }

        static void Test压力()
        {
            //var pool = ThreadPoolX.Instance;
            //pool.MaxThreads = 1000;
            //Console.WriteLine("最小:{0}", pool.MinThreads);
            //Console.WriteLine("最大:{0}", pool.MaxThreads);
            //for (int i = 0; i < 2000; i++)
            //{
            //    pool.Queue(TestItem, i);
            //}
        }

        static Regex reg = new Regex("<a\\s+href=\"?([^\">]*)\"?\\s*>", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline | RegexOptions.IgnoreCase);
        static void TestItem(Object obj)
        {
            var id = (Int32)obj;
            Console.WriteLine("启动：{0}", id);

            try
            {
                var baseurl = "http://www.newlifex.com";
                var list = new List<String>();
                list.Add(baseurl);

                var rnd = new Random((Int32)DateTime.Now.Ticks);

                var client = new WebClient();

                for (int i = 0; i < 100; i++)
                {
                    // 随机拿一个网址
                    var url = list[rnd.Next(0, list.Count)];
                    Console.WriteLine("{0,-4} {1}", id, url);

                    try
                    {
                        var html = client.DownloadString(url);
                        if (!String.IsNullOrEmpty(html))
                        {
                            var ms = reg.Matches(html);
                            foreach (Match item in ms)
                            {
                                url = item.Groups[1].Value;
                                if (url.StartsWithIgnoreCase("http://"))
                                {
                                    // 不接受外网连接
                                    if (!url.StartsWithIgnoreCase(baseurl)) continue;
                                }
                                else
                                    url = baseurl + url.EnsureStart("/");

                                if (!list.Contains(url)) list.Add(url);
                            }
                        }
                    }
                    catch { }
                }

                Console.WriteLine("完成：{0}", id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("错误：{0} {1}", id, ex.Message);
            }
        }
    }
}