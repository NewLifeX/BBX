using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using NewLife.Log;

namespace BBX.Common
{
    /// <summary>页面模版</summary>
    public abstract class PageTemplate
    {
        public static Regex[] r;
        private Dictionary<String, String> headerTemplateCache = new Dictionary<String, String>();

        public abstract String ReplaceSpecialTemplate(String forumPath, String skinName, String strTemplate);

        static PageTemplate()
        {
            var option = RegexOptions.Compiled;

            r = new Regex[25];
            r[0] = new Regex("<%template ([^\\[\\]\\{\\}\\s]+)%>", option);
            r[1] = new Regex("<%loop ((\\(([a-zA-Z]+)\\) )?)([^\\[\\]\\{\\}\\s]+) ([^\\[\\]\\{\\}\\s]+)%>", option);
            r[2] = new Regex("<%\\/loop%>", option);
            r[3] = new Regex("<%while ([^\\[\\]\\{\\}\\s]+)%>", option);
            r[4] = new Regex("<%\\/while ([^\\[\\]\\{\\}\\s]+)%>", option);
            r[5] = new Regex("<%if (?:\\s*)(([^\\s]+)((?:\\s*)(\\|\\||\\&\\&)(?:\\s*)([^\\s]+))?)(?:\\s*)%>", option);
            r[6] = new Regex("<%else(( (?:\\s*)if (?:\\s*)(([^\\s]+)((?:\\s*)(\\|\\||\\&\\&)(?:\\s*)([^\\s]+))?))?)(?:\\s*)%>", option);
            r[7] = new Regex("<%\\/if%>", option);
            r[8] = new Regex("(\\{strtoint\\(([^\\s]+?)\\)\\})", option);
            r[9] = new Regex("(<%urlencode\\(([^\\s]+?)\\)%>)", option);
            r[10] = new Regex("(<%Format\\(([^\\s]+?),(.*?)\\)%>)", option);
            r[11] = new Regex("(\\{([^\\.\\[\\]\\{\\}\\s]+)\\.([^\\[\\]\\{\\}\\s]+)\\})", option); // 对于 {xxx.yyy} 的格式，把第一个y变为大写
            r[12] = new Regex("(\\{request\\[([^\\[\\]\\{\\}\\s]+)\\]\\})", option);
            r[13] = new Regex("(\\{([^\\[\\]\\{\\}\\s]+)\\[([^\\[\\]\\{\\}\\s]+)\\]\\})", option);
            r[14] = new Regex("({([^\\[\\]/\\{\\}='\\s]+)})", option);
            r[15] = new Regex("({([^\\[\\]/\\{\\}='\\s]+)})", option);
            r[16] = new Regex("(([=|>|<|!]=)\\\\\"([^\\s]*)\\\\\")", option);
            r[17] = new Regex("<%namespace (?:\"?)([\\s\\S]+?)(?:\"?)%>", option);
            r[18] = new Regex("<%csharp%>([\\s\\S]+?)<%/csharp%>", option);
            r[19] = new Regex("<%set ((\\(([a-zA-Z]+)\\))?)(?:\\s*)\\{([^\\s]+)\\}(?:\\s*)=(?:\\s*)(.*?)(?:\\s*)%>", option);
            r[20] = new Regex("(<%getsubstring\\(([^\\s]+?),(.\\d*?),([^\\s]+?)\\)%>)", option);
            r[21] = new Regex("<%repeat\\(([^\\s]+?)(?:\\s*),(?:\\s*)([^\\s]+?)\\)%>", option);
            r[22] = new Regex("<%inherits (?:\"?)([\\s\\S]+?)(?:\"?)%>", option);
            r[23] = new Regex("<%continue%>");
            r[24] = new Regex("<%break%>");
        }

        public virtual String GetTemplate(String forumPath, String skinName, String pname, int nest, String tempname)
        {
            return GetTemplate(forumPath, skinName, pname, "", nest, tempname);
        }

        public virtual String GetTemplate(String forumPath, String skinName, String pname, String subdir, int nest, String tempname)
        {
            var key = forumPath + skinName + "/" + pname;
            if (nest == 2 && headerTemplateCache.ContainsKey(key)) return headerTemplateCache[key];

            if (nest > 5) return "";
            if (nest < 1) nest = 1;

            var sw = new Stopwatch();
            sw.Start();

            var sb = new StringBuilder(220000);

            var text = "";
            if (subdir != String.Empty && !subdir.EndsWith("\\")) subdir += "\\";

            var name = "BBX.Web." + pname;

            var path = GetTempFile(forumPath, skinName, pname, subdir);
            using (var streamReader = new StreamReader(path, Encoding.UTF8))
            {
                var sb2 = new StringBuilder(70000);
                sb2.Append(streamReader.ReadToEnd());
                streamReader.Close();
                if (nest == 1)
                {
                    foreach (Match m in r[17].Matches(sb2.ToString()))
                    {
                        text = text + "\r\n<%@ Import namespace=\"" + m.Groups[1] + "\" %>";
                        sb2.Replace(m.Groups[0].ToString(), String.Empty);
                    }
                    foreach (Match m in r[22].Matches(sb2.ToString()))
                    {
                        name = m.Groups[1].ToString();
                        sb2.Replace(m.Groups[0].ToString(), String.Empty);
                    }
                    if ("\"".Equals(name)) name = "BBX.Forum.PageBase";
                }
                foreach (Match m in r[18].Matches(sb2.ToString()))
                {
                    sb2.Replace(m.Groups[0].ToString(), m.Groups[0].ToString().Replace("\r\n", "\r\t\r"));
                }
                sb2.Replace("\r\n<%", "<%");
                sb2.Replace("\r\n", "\r\r\r");
                sb2.Replace("<%", "\r\r\n<%");
                sb2.Replace("%>", "%>\r\r\n");
                sb2.Replace("<%csharp%>\r\r\n", "<%csharp%>").Replace("\r\r\n<%/csharp%>", "<%/csharp%>");
                var array = Utils.SplitString(sb2.ToString(), "\r\r\n");
                foreach (var item in array)
                {
                    if (!String.IsNullOrEmpty(item)) sb.Append(ConvertTags(nest, forumPath, skinName, subdir, item, tempname));
                }
            }
            if (nest == 1)
            {
                var sb2 = new StringBuilder();
                sb2.AppendFormat("<%@ Page language=\"c#\" AutoEventWireup=\"false\" EnableViewState=\"false\" Inherits=\"{0}\" %>", name);
                sb2.AppendLine();
                sb2.AppendLine("<%@ Import namespace=\"System.Data\" %>");
                sb2.AppendLine("<%@ Import namespace=\"BBX.Common\" %>");
                sb2.AppendLine("<%@ Import namespace=\"BBX.Forum\" %>");
                sb2.AppendLine("<%@ Import namespace=\"BBX.Entity\" %>");
                sb2.AppendLine("<%@ Import namespace=\"BBX.Config\" %>");
                sb2.AppendLine(text);
                sb2.AppendLine("<script runat=\"server\">");
                sb2.AppendLine("override protected void OnLoad(EventArgs e)");
                sb2.AppendLine("{");
                sb2.AppendLine("base.OnLoad(e);");
                sb2.AppendLine("if (!CanShow) return;");
                sb2.AppendFormat("templateBuilder.Capacity = {0};", sb.Capacity);
                sb2.AppendLine(Regex.Replace(sb.ToString(), "\\r\\n\\s*templateBuilder\\.Append\\(\"\"\\);", ""));
                sb2.AppendLine("Response.Write(templateBuilder.ToString());");
                sb2.AppendLine("}");
                sb2.AppendLine("</script>");
                //var content = String.Format("templateBuilder.Capacity = {3};\r\n{4}\r\n\tResponse.Write(templateBuilder.ToString());\r\n}}\r\n</script>\r\n", name,
                //    text, DateTime.Now, sb.Capacity, Regex.Replace(sb.ToString(), "\\r\\n\\s*templateBuilder\\.Append\\(\"\"\\);", ""));
                var content = sb2.ToString();
                var mapPath = Utils.GetMapPath(forumPath + "aspx\\" + tempname + "\\");
                if (!Directory.Exists(mapPath)) Utils.CreateDir(mapPath);

                var path2 = mapPath + pname + ".aspx";
                if (!File.Exists(path2) || File.ReadAllText(path2) != content)
                {
                    File.WriteAllText(path2, content, Encoding.UTF8);
                    //using (var fileStream = new FileStream(path2, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                    //{
                    //    var bytes = Encoding.UTF8.GetBytes(content);
                    //    fileStream.Write(bytes, 0, bytes.Length);
                    //    fileStream.Close();
                    //}
                }
            }
            if (nest == 2) headerTemplateCache.Add(key, sb.ToString());

            sw.Stop();
            XTrace.WriteLine("生成模版 {0}\\{1,-20} 耗时{2}", skinName, pname, sw.Elapsed);

            return sb.ToString();
        }

        String GetTempFile(String forumPath, String skinName, String pname, String subdir)
        {
            var tempPath = Utils.GetMapPath(forumPath + "templates");

            var format = "{0}\\{1}\\{2}{3}.config";
            var path = String.Format(format, tempPath, skinName, subdir, pname);
            if (File.Exists(path)) return path;

            path = String.Format(format, tempPath, skinName, "", pname);
            if (File.Exists(path)) return path;

            path = String.Format(format, tempPath, "default", subdir, pname);
            if (File.Exists(path)) return path;

            path = String.Format(format, tempPath, "default", "", pname);
            if (File.Exists(path)) return path;

            format = "{0}\\{1}\\{2}{3}.htm";
            path = String.Format(format, tempPath, skinName, subdir, pname);
            if (File.Exists(path)) return path;

            path = String.Format(format, tempPath, skinName, "", pname);
            if (File.Exists(path)) return path;

            path = String.Format(format, tempPath, "default", subdir, pname);
            if (File.Exists(path)) return path;

            path = String.Format(format, tempPath, "default", "", pname);
            if (File.Exists(path)) return path;

            return "";
        }

        private String ConvertTags(int nest, String forumPath, String skinName, String subdir, String inputStr, String tempname)
        {
            var text = "";
            var str = inputStr.Replace("\\", "\\\\");
            str = str.Replace("\"", "\\\"");
            str = str.Replace("</script>", "</\");\r\n\ttemplateBuilder.Append(\"script>");
            bool flag = false;
            foreach (Match m in r[0].Matches(str))
            {
                flag = true;
                str = str.Replace(m.Groups[0].ToString(), "\r\n" + GetTemplate(forumPath, skinName, m.Groups[1].ToString(), subdir, nest + 1, tempname) + "\r\n");
            }
            foreach (Match m in r[1].Matches(str))
            {
                flag = true;
                if (String.IsNullOrEmpty(m.Groups[3].ToString()))
                {
                    str = str.Replace(m.Groups[0].ToString(), String.Format("\r\n\tint {0}__loop__id=0;\r\n\tforeach(DataRow {0} in {1}.Rows)\r\n\t{{\r\n\t\t{0}__loop__id++;\r\n", m.Groups[4], m.Groups[5]));
                }
                else
                {
                    str = str.Replace(m.Groups[0].ToString(), String.Format("\r\n\tint {1}__loop__id=0;\r\n\tforeach({0} {1} in {2})\r\n\t{{\r\n\t\t{1}__loop__id++;\r\n", m.Groups[3], m.Groups[4], m.Groups[5]));
                }
            }
            foreach (Match m in r[2].Matches(str))
            {
                flag = true;
                str = str.Replace(m.Groups[0].ToString(), "\r\n\t}\t//end loop\r\n");
            }
            foreach (Match m in r[3].Matches(str))
            {
                flag = true;
                str = str.Replace(m.Groups[0].ToString(), String.Format("\r\n\tint {0}__loop__id=0;\r\nwhile({0}.Read())\r\n\t{{\r\n{0}__loop__id++;\r\n", m.Groups[1]));
            }
            foreach (Match m in r[4].Matches(str))
            {
                flag = true;
                str = str.Replace(m.Groups[0].ToString(), "\r\n\t}\t//end while\r\n" + m.Groups[1] + ".Close();\r\n");
            }
            foreach (Match m in r[5].Matches(str))
            {
                flag = true;
                str = str.Replace(m.Groups[0].ToString(), "\r\n\tif (" + m.Groups[1].ToString().Replace("\\\"", "\"") + ")\r\n\t{\r\n");
            }
            foreach (Match m in r[6].Matches(str))
            {
                flag = true;
                if (m.Groups[1].ToString() == String.Empty)
                {
                    str = str.Replace(m.Groups[0].ToString(), "\r\n\t}\r\n\telse\r\n\t{\r\n");
                }
                else
                {
                    str = str.Replace(m.Groups[0].ToString(), "\r\n\t}\r\n\telse if (" + m.Groups[3].ToString().Replace("\\\"", "\"") + ")\r\n\t{\r\n");
                }
            }
            foreach (Match m in r[7].Matches(str))
            {
                flag = true;
                str = str.Replace(m.Groups[0].ToString(), "\r\n\t}\t//end if\r\n");
            }
            foreach (Match m in r[19].Matches(str))
            {
                flag = true;
                String arg = "";
                if (m.Groups[3].ToString() != String.Empty)
                {
                    arg = m.Groups[3].ToString();
                }
                str = str.Replace(m.Groups[0].ToString(), String.Format("\t{0} {1} = {2};\r\n\t", arg, m.Groups[4], m.Groups[5]).Replace("\\\"", "\""));
            }
            foreach (Match m in r[21].Matches(str))
            {
                flag = true;
                str = str.Replace(m.Groups[0].ToString(), "\tfor (int i = 0; i < " + m.Groups[2] + "; i++)\r\n\t{\r\n\t\ttemplateBuilder.Append(" + m.Groups[1].ToString().Replace("\\\"", "\"").Replace("\\\\", "\\") + ");\r\n\t}\r\n");
            }
            foreach (Match m in r[23].Matches(str))
            {
                flag = true;
                str = str.Replace(m.Groups[0].ToString(), "\tcontinue;\r\n");
            }
            foreach (Match m in r[24].Matches(str))
            {
                flag = true;
                str = str.Replace(m.Groups[0].ToString(), "\tbreak;\r\n");
            }
            foreach (Match m in r[8].Matches(str))
            {
                str = str.Replace(m.Groups[0].ToString(), "" + m.Groups[2] + ".ToInt(0)");
            }
            foreach (Match m in r[9].Matches(str))
            {
                flag = true;
                str = str.Replace(m.Groups[0].ToString(), "templateBuilder.Append(Utils.UrlEncode(" + m.Groups[2] + "));");
            }
            foreach (Match m in r[10].Matches(str))
            {
                flag = true;
                str = str.Replace(m.Groups[0].ToString(), String.Format("\ttemplateBuilder.Append({0}.ToString(\"{1}\"));", m.Groups[2], m.Groups[3].ToString().Replace("\\\"", String.Empty)));
            }
            foreach (Match m in r[20].Matches(str))
            {
                flag = true;
                str = str.Replace(m.Groups[0].ToString(), String.Format("\ttemplateBuilder.Append(Utils.GetUnicodeSubString({0},{1},\"{2}\"));", m.Groups[2], m.Groups[3], m.Groups[4].ToString().Replace("\\\"", String.Empty)));
            }

            // xx.yy
            // new Regex("(\\{([^\\.\\[\\]\\{\\}\\s]+)\\.([^\\[\\]\\{\\}\\s]+)\\})", option)
            // (\{([^\.\[\]\{\}\s]+)\.([^\[\]\{\}\s]+)\})
            foreach (Match m in r[11].Matches(str))
            {
                var key = m.Groups[3].ToString();
                if (flag)
                {
                    str = str.Replace(m.Groups[0].ToString(), String.Format("{0}.{1}{2}", m.Groups[2], key.Substring(0, 1).ToUpper(), key.Substring(1)));
                }
                else
                {
                    str = str.Replace(m.Groups[0].ToString(), String.Format("\");\r\n\ttemplateBuilder.Append(({0}.{1}{2}+\"\").Trim());\r\n\ttemplateBuilder.Append(\"", m.Groups[2], key.Substring(0, 1).ToUpper(), key.Substring(1)));
                }
            }
            foreach (Match m in r[12].Matches(str))
            {
                if (flag)
                {
                    str = str.Replace(m.Groups[0].ToString(), "DNTRequest.GetString(\"" + m.Groups[2] + "\")");
                }
                else
                {
                    str = str.Replace(m.Groups[0].ToString(), String.Format("\" + DNTRequest.GetString(\"{0}\") + \"", m.Groups[2]));
                }
            }
            foreach (Match m in r[13].Matches(str))
            {
                if (flag)
                {
                    if (Utils.IsNumeric(m.Groups[3].ToString()))
                    {
                        str = str.Replace(m.Groups[0].ToString(), m.Groups[2] + "[" + m.Groups[3] + "].ToString().Trim()");
                    }
                    else
                    {
                        if (m.Groups[3].ToString() == "_id")
                        {
                            str = str.Replace(m.Groups[0].ToString(), m.Groups[2] + "__loop__id");
                        }
                        else
                        {
                            str = str.Replace(m.Groups[0].ToString(), m.Groups[2] + "[\"" + m.Groups[3] + "\"].ToString().Trim()");
                        }
                    }
                }
                else
                {
                    if (Utils.IsNumeric(m.Groups[3].ToString()))
                    {
                        str = str.Replace(m.Groups[0].ToString(), String.Format("\" + {0}[{1}].ToString().Trim() + \"", m.Groups[2], m.Groups[3]));
                    }
                    else
                    {
                        if (m.Groups[3].ToString() == "_id")
                        {
                            str = str.Replace(m.Groups[0].ToString(), String.Format("\" + {0}__loop__id.ToString() + \"", m.Groups[2]));
                        }
                        else
                        {
                            str = str.Replace(m.Groups[0].ToString(), String.Format("\" + {0}[\"{1}\"].ToString().Trim() + \"", m.Groups[2], m.Groups[3]));
                        }
                    }
                }
            }
            str = ReplaceSpecialTemplate(forumPath, skinName, str);
            foreach (Match m in r[14].Matches(str))
            {
                if (m.Groups[0].ToString() == "{commonversion}")
                {
                    str = str.Replace(m.Groups[0].ToString(), Utils.Version);
                }
            }
            foreach (Match m in r[15].Matches(str))
            {
                if (flag)
                {
                    str = str.Replace(m.Groups[0].ToString(), m.Groups[2].ToString());
                }
                else
                {
                    str = str.Replace(m.Groups[0].ToString(), String.Format("\");\r\n\ttemplateBuilder.Append({0}.ToString());\r\n\ttemplateBuilder.Append(\"", m.Groups[2].ToString().Trim()));
                }
            }
            foreach (Match m in r[16].Matches(str))
            {
                str = str.Replace(m.Groups[0].ToString(), m.Groups[2] + "\"" + m.Groups[3] + "\"");
            }
            foreach (Match m in r[18].Matches(str))
            {
                flag = true;
                str = str.Replace(m.Groups[0].ToString(), m.Groups[1].ToString().Replace("\r\t\r", "\r\n\t").Replace("\\\"", "\""));
            }
            if (flag)
            {
                text = str + "\r\n";
            }
            else
            {
                if (!str.IsNullOrEmpty())
                {
                    text = "\ttemplateBuilder.Append(\"" + str.Replace("\r\r\r", "\\r\\n") + "\");";
                    text = text.Replace("\\r\\n<?xml", "<?xml");
                    text = text.Replace("\\r\\n<!DOCTYPE", "<!DOCTYPE");
                }
            }
            return text;
        }
    }
}