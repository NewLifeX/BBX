using System;
using System.Data;
using System.IO;
using Discuz.Cache;
using Discuz.Common;
using Discuz.Config;
using Discuz.Entity;

namespace Discuz.Forum
{
    public class AdminTemplates : Templates
    {
        public static void DeleteTemplateItem(string templateidlist)
        {
            Discuz.Data.Templates.DeleteTemplateItem(templateidlist);
        }

        public static DataTable GetAllTemplateList(string templatePath)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(templatePath);
            DataTable allTemplateList = Discuz.Data.Templates.GetAllTemplateList();
            allTemplateList.Columns.Add("valid", typeof(Int16));
            string text = ",";
            foreach (DataRow dataRow in allTemplateList.Rows)
            {
                TemplateAboutInfo templateAboutInfo = Templates.GetTemplateAboutInfo(templatePath + dataRow["directory"].ToString());
                dataRow["valid"] = 1;
                AdminTemplates.SetTemplateDataRow(dataRow, templateAboutInfo);
                text = text + dataRow["directory"].ToString() + ",";
            }
            int num = TypeConverter.ObjectToInt(Discuz.Data.Templates.GetValidTemplateList().Compute("Max(templateid)", "")) + 1;
            DirectoryInfo[] directories = directoryInfo.GetDirectories();
            for (int i = 0; i < directories.Length; i++)
            {
                DirectoryInfo directoryInfo2 = directories[i];
                if (directoryInfo2 != null && text.IndexOf("," + directoryInfo2 + ",") < 0)
                {
                    TemplateAboutInfo templateAboutInfo2 = Templates.GetTemplateAboutInfo(directoryInfo2.FullName);
                    DataRow dataRow2 = allTemplateList.NewRow();
                    dataRow2["templateid"] = num++;
                    dataRow2["directory"] = directoryInfo2.Name;
                    dataRow2["valid"] = 0;
                    AdminTemplates.SetTemplateDataRow(dataRow2, templateAboutInfo2);
                    allTemplateList.Rows.Add(dataRow2);
                }
            }
            allTemplateList.AcceptChanges();
            return allTemplateList;
        }

        private static void SetTemplateDataRow(DataRow dr, TemplateAboutInfo aboutInfo)
        {
            dr["name"] = aboutInfo.name;
            dr["author"] = aboutInfo.author;
            dr["createdate"] = aboutInfo.createdate;
            dr["ver"] = aboutInfo.ver;
            dr["fordntver"] = aboutInfo.fordntver;
            dr["copyright"] = aboutInfo.copyright;
        }

        public static void RemoveTemplateInDB(string templateIdList, int uid, string userName, int groupId, string groupTitle, string ip)
        {
            GeneralConfigInfo config = GeneralConfigInfo.Current;
            if (("," + templateIdList + ",").IndexOf("," + config.Templateid + ",") >= 0)
            {
                config.Templateid = 1;
            }
            //GeneralConfigs.Serialiaze(config, Utils.GetMapPath("../../config/general.config"));
            config.Save();
            Discuz.Data.Forums.UpdateForumAndUserTemplateId(templateIdList);
            Discuz.Data.Templates.DeleteTemplateItem(templateIdList);
            var cache = XCache.Current;
            XCache.Remove("/Forum/TemplateList");
            XCache.Remove(CacheKeys.FORUM_TEMPLATE_ID_LIST);
            XCache.Remove(CacheKeys.FORUM_UI_TEMPLATE_LIST_BOX_OPTIONS_FOR_FORUMINDEX);
            XCache.Remove(CacheKeys.FORUM_UI_TEMPLATE_LIST_BOX_OPTIONS);
            AdminVisitLog.InsertLog(uid, userName, groupId, groupTitle, ip, "从数据库中删除模板文件", "ID为:" + templateIdList);
        }

        public static void DeleteTemplate(string templateIdList, int uid, string userName, int groupId, string groupTitle, string ip)
        {
            AdminTemplates.RemoveTemplateInDB(templateIdList, uid, userName, groupId, groupTitle, ip);
            string[] array = templateIdList.Split(',');
            for (int i = 0; i < array.Length; i++)
            {
                string str = array[i];
                string @string = DNTRequest.GetString("temp" + str);
                if (!(String.IsNullOrEmpty(@string)))
                {
                    string mapPath = Utils.GetMapPath("..\\..\\templates\\" + @string);
                    if (Directory.Exists(mapPath))
                    {
                        Directory.Delete(mapPath, true);
                    }
                    string mapPath2 = Utils.GetMapPath("..\\..\\aspx\\" + str);
                    if (Directory.Exists(mapPath2))
                    {
                        Directory.Delete(mapPath2, true);
                    }
                }
            }
            AdminVisitLog.InsertLog(uid, userName, groupId, groupTitle, ip, "从模板库中删除模板文件", "ID为:" + templateIdList);
        }
    }
}