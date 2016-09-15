using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Discuz.Common
{
    public abstract class PageTemplate
    {
        public static Regex[] r;
        private Dictionary<string, string> headerTemplateCache = new Dictionary<string, string>();

        public abstract string ReplaceSpecialTemplate(string forumPath, string skinName, string strTemplate);

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
            r[10] = new Regex("(<%datetostr\\(([^\\s]+?),(.*?)\\)%>)", option);
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

        public virtual string GetTemplate(string forumPath, string skinName, string templateName, int nest, int templateId)
        {
            return GetTemplate(forumPath, skinName, templateName, "", nest, templateId);
        }

        public virtual string GetTemplate(string forumPath, string skinName, string templateName, string templateSubDirectory, int nest, int templateId)
        {
            var key = forumPath + skinName + "/" + templateName;
            if (nest == 2 && headerTemplateCache.ContainsKey(key)) return headerTemplateCache[key];

            var sb = new StringBuilder(220000);
            if (nest < 1)
                nest = 1;
            else if (nest > 5)
                return "";

            var text = "";
            if (templateSubDirectory != string.Empty && !templateSubDirectory.EndsWith("\\")) templateSubDirectory += "\\";

            var cfg = "{0}\\{1}\\{2}{3}.config";
            var htm = "{0}\\{1}\\{2}{3}.htm";
            var name = "Discuz.Web." + templateName;
            var tempPath = Utils.GetMapPath(forumPath + "templates");

            var path = string.Format(cfg, tempPath, skinName, templateSubDirectory, templateName);
            if (!File.Exists(path))
            {
                path = string.Format(cfg, tempPath, skinName, "", templateName);
                if (!File.Exists(path))
                {
                    path = string.Format(cfg, tempPath, "default", templateSubDirectory, templateName);
                    if (!File.Exists(path))
                    {
                        path = string.Format(cfg, tempPath, "default", "", templateName);
                        if (!File.Exists(path))
                        {
                            path = string.Format(htm, tempPath, skinName, templateSubDirectory, templateName);
                            if (!File.Exists(path))
                            {
                                path = string.Format(htm, tempPath, skinName, "", templateName);
                                if (!File.Exists(path))
                                {
                                    path = string.Format(htm, tempPath, "default", templateSubDirectory, templateName);
                                    if (!File.Exists(path))
                                    {
                                        path = string.Format(htm, tempPath, "default", "", templateName);
                                        if (!File.Exists(path)) return "";
                                    }
                                }
                            }
                        }
                    }
                }
            }
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
                        sb2.Replace(m.Groups[0].ToString(), string.Empty);
                    }
                    foreach (Match m in r[22].Matches(sb2.ToString()))
                    {
                        name = m.Groups[1].ToString();
                        sb2.Replace(m.Groups[0].ToString(), string.Empty);
                    }
                    if ("\"".Equals(name)) name = "Discuz.Forum.PageBase";
                }
                foreach (Match m in r[18].Matches(sb2.ToString()))
                {
                    sb2.Replace(m.Groups[0].ToString(), m.Groups[0].ToString().Replace("\r\n", "\r\t\r"));
                }
                sb2.Replace("\r\n", "\r\r\r");
                sb2.Replace("<%", "\r\r\n<%");
                sb2.Replace("%>", "%>\r\r\n");
                sb2.Replace("<%csharp%>\r\r\n", "<%csharp%>").Replace("\r\r\n<%/csharp%>", "<%/csharp%>");
                var array = Utils.SplitString(sb2.ToString(), "\r\r\n");
                //int upperBound = array.GetUpperBound(0);
                //for (int i = 0; i <= upperBound; i++)
                //{
                //    if (!(array[i] == ""))
                //    {
                //        sb.Append(this.ConvertTags(nest, forumPath, skinName, templateSubDirectory, array[i], templateId));
                //    }
                //}
                foreach (var item in array)
                {
                    if (!String.IsNullOrEmpty(item)) sb.Append(this.ConvertTags(nest, forumPath, skinName, templateSubDirectory, item, templateId));
                }
            }
            if (nest == 1)
            {
                var s = string.Format("<%@ Page language=\"c#\" AutoEventWireup=\"false\" EnableViewState=\"false\" Inherits=\"{0}\" %>\r\n<%@ Import namespace=\"System.Data\" %>\r\n<%@ Import namespace=\"Discuz.Common\" %>\r\n<%@ Import namespace=\"Discuz.Forum\" %>\r\n<%@ Import namespace=\"Discuz.Entity\" %>\r\n<%@ Import namespace=\"Discuz.Config\" %>\r\n{1}\r\n<script runat=\"server\">\r\noverride protected void OnInit(EventArgs e)\r\n{{\r\n\r\n\t/* \r\n\t\t本页面代码由模板引擎生成. \r\n\t*/\r\n\r\n\tbase.OnInit(e);\r\n\r\n\ttemplateBuilder.Capacity = {3};\r\n{4}\r\n\tResponse.Write(templateBuilder.ToString());\r\n}}\r\n</script>\r\n", name,
                    text, DateTime.Now, sb.Capacity, Regex.Replace(sb.ToString(), "\\r\\n\\s*templateBuilder\\.Append\\(\"\"\\);", ""));
                var mapPath = Utils.GetMapPath(forumPath + "aspx\\" + templateId + "\\");
                if (!Directory.Exists(mapPath)) Utils.CreateDir(mapPath);

                var path2 = mapPath + templateName + ".aspx";
                using (var fileStream = new FileStream(path2, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    var bytes = Encoding.UTF8.GetBytes(s);
                    fileStream.Write(bytes, 0, bytes.Length);
                    fileStream.Close();
                }
            }
            if (nest == 2) headerTemplateCache.Add(key, sb.ToString());

            return sb.ToString();
        }

        private string ConvertTags(int nest, string forumPath, string skinName, string templateSubDirectory, string inputStr, int templateid)
        {
            var text = "";
            var str = inputStr.Replace("\\", "\\\\");
            str = str.Replace("\"", "\\\"");
            str = str.Replace("</script>", "</\");\r\n\ttemplateBuilder.Append(\"script>");
            bool flag = false;
            foreach (Match m in r[0].Matches(str))
            {
                flag = true;
                str = str.Replace(m.Groups[0].ToString(), "\r\n" + GetTemplate(forumPath, skinName, m.Groups[1].ToString(), templateSubDirectory, nest + 1, templateid) + "\r\n");
            }
            foreach (Match m in r[1].Matches(str))
            {
                flag = true;
                if (m.Groups[3].ToString() == "")
                {
                    str = str.Replace(m.Groups[0].ToString(), string.Format("\r\n\tint {0}__loop__id=0;\r\n\tforeach(DataRow {0} in {1}.Rows)\r\n\t{{\r\n\t\t{0}__loop__id++;\r\n", m.Groups[4], m.Groups[5]));
                }
                else
                {
                    str = str.Replace(m.Groups[0].ToString(), string.Format("\r\n\tint {1}__loop__id=0;\r\n\tforeach({0} {1} in {2})\r\n\t{{\r\n\t\t{1}__loop__id++;\r\n", m.Groups[3], m.Groups[4], m.Groups[5]));
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
                str = str.Replace(m.Groups[0].ToString(), string.Format("\r\n\tint {0}__loop__id=0;\r\nwhile({0}.Read())\r\n\t{{\r\n{0}__loop__id++;\r\n", m.Groups[1]));
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
                if (m.Groups[1].ToString() == string.Empty)
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
                string arg = "";
                if (m.Groups[3].ToString() != string.Empty)
                {
                    arg = m.Groups[3].ToString();
                }
                str = str.Replace(m.Groups[0].ToString(), string.Format("\t{0} {1} = {2};\r\n\t", arg, m.Groups[4], m.Groups[5]).Replace("\\\"", "\""));
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
                str = str.Replace(m.Groups[0].ToString(), "Utils.StrToInt(" + m.Groups[2] + ", 0)");
            }
            foreach (Match m in r[9].Matches(str))
            {
                flag = true;
                str = str.Replace(m.Groups[0].ToString(), "templateBuilder.Append(Utils.UrlEncode(" + m.Groups[2] + "));");
            }
            foreach (Match m in r[10].Matches(str))
            {
                flag = true;
                str = str.Replace(m.Groups[0].ToString(), string.Format("\ttemplateBuilder.Append(TypeConverter.StrToDateTime({0}).ToString(\"{1}\"));", m.Groups[2], m.Groups[3].ToString().Replace("\\\"", string.Empty)));
            }
            foreach (Match m in r[20].Matches(str))
            {
                flag = true;
                str = str.Replace(m.Groups[0].ToString(), string.Format("\ttemplateBuilder.Append(Utils.GetUnicodeSubString({0},{1},\"{2}\"));", m.Groups[2], m.Groups[3], m.Groups[4].ToString().Replace("\\\"", string.Empty)));
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
                    str = str.Replace(m.Groups[0].ToString(), string.Format("\" + DNTRequest.GetString(\"{0}\") + \"", m.Groups[2]));
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
                        str = str.Replace(m.Groups[0].ToString(), string.Format("\" + {0}[{1}].ToString().Trim() + \"", m.Groups[2], m.Groups[3]));
                    }
                    else
                    {
                        if (m.Groups[3].ToString() == "_id")
                        {
                            str = str.Replace(m.Groups[0].ToString(), string.Format("\" + {0}__loop__id.ToString() + \"", m.Groups[2]));
                        }
                        else
                        {
                            str = str.Replace(m.Groups[0].ToString(), string.Format("\" + {0}[\"{1}\"].ToString().Trim() + \"", m.Groups[2], m.Groups[3]));
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
                    str = str.Replace(m.Groups[0].ToString(), string.Format("\");\r\n\ttemplateBuilder.Append({0}.ToString());\r\n\ttemplateBuilder.Append(\"", m.Groups[2].ToString().Trim()));
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
                if (str.Trim() != "")
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