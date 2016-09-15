using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using BBX.Cache;
using BBX.Common;
using BBX.Config;
using BBX.Entity;

namespace BBX.Forum
{
    /// <summary>论坛页面模版</summary>
    public class ForumPageTemplate : PageTemplate
    {
        public override string ReplaceSpecialTemplate(string forumPath, string skinName, string strTemplate)
        {
            var sb = new StringBuilder();
            sb.Append(strTemplate);
            var regex = new Regex("({([^\\[\\]/\\{\\}='\\s]+)})", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
            var match = regex.Match(strTemplate);
            while (match.Success)
            {
                if (match.Groups[0].ToString() == "{forumversion}")
                {
                    sb = sb.Replace(match.Groups[0].ToString(), Utils.Version);
                }
                else
                {
                    if (match.Groups[0].ToString() == "{forumproductname}")
                    {
                        sb = sb.Replace(match.Groups[0].ToString(), Utils.ProductName);
                    }
                }
                match = match.NextMatch();
            }
            foreach (DataRow dataRow in GetTemplateVarList(forumPath, skinName).Rows)
            {
                sb = sb.Replace(dataRow["variablename"].ToString().Trim(), dataRow["variablevalue"].ToString().Trim());
            }
            return sb.ToString();
        }

        public static DataTable GetTemplateVarList(string forumpath, string skinName)
        {
            var cacheService = XCache.Current;
            DataTable dataTable = cacheService.RetrieveObject("/Forum/" + skinName + "/TemplateVariable") as DataTable;
            if (dataTable == null)
            {
                DataSet dataSet = new DataSet("template");
                var file = Utils.GetMapPath(forumpath + "templates/" + skinName + "/templatevariable.xml");

                if (Utils.FileExists(file))
                {
                    dataSet.ReadXml(file);
                    if (dataSet.Tables.Count == 0)
                    {
                        dataSet.Tables.Add(TemplateVariableTable());
                    }
                }
                else
                {
                    dataSet.Tables.Add(TemplateVariableTable());
                }
                dataTable = dataSet.Tables[0];
                XCache.Add("/Forum/" + skinName + "/TemplateVariable", dataTable, file);
            }
            return dataTable;
        }

        private static DataTable TemplateVariableTable()
        {
            return new DataTable("TemplateVariable")
            {
                Columns = { { "id", typeof(Int32) }, { "variablename", typeof(String) }, { "variablevalue", typeof(String) } }
            };
        }

        public static void BuildTemplate(String dir)
        {
            var tmp = Template.FindByPath(dir);
            if (tmp != null) BuildTemplate(tmp);

        }

        public static void BuildTemplate(Template tmp)
        {
            var dic = new Dictionary<String, String>();
            GetTemplates("default", dic);
            if (!tmp.Directory.EqualIgnoreCase("default")) GetTemplates(tmp.Directory, dic);

            var fpt = new ForumPageTemplate();
            foreach (var item in dic)
            {
                var templateName = item.Key.Split('.')[0];
                var ss = item.Value.Split('\\');
                fpt.GetTemplate(BaseConfigs.GetForumPath, ss[0], templateName, (ss.Length >= 2) ? ss[ss.Length - 1] : "", 1, tmp.Name);
            }
        }

        private static void GetTemplates(string directorypath, Dictionary<String, String> dic)
        {
            var di = BaseConfigs.GetForumPath.CombinePath("templates", directorypath).AsDirectory();
            var fis = di.GetFileSystemInfos();
            foreach (var fi in di.GetFileSystemInfos())
            {
                if (fi.Name != "images")
                {
                    if (fi.Attributes == FileAttributes.Directory)
                    {
                        GetTemplates(directorypath + "\\" + fi.Name, dic);
                    }
                    else
                    {
                        if (fi != null && fi.Extension.EqualIgnoreCase(".htm", ".config") && !fi.Name.StartsWith("_"))
                        {
                            dic[fi.Name] = directorypath;
                        }
                    }
                }
            }
        }
    }
}