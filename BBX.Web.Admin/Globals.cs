using System;
using System.Collections;
using System.IO;
using BBX.Common;
using BBX.Config;
using BBX.Forum;
using BBX.Entity;
using System.Collections.Generic;

namespace BBX.Web.Admin
{
    public class Globals
    {
        public static void BuildTemplate(string directorypath)
        {
            var tmp = Template.FindByPath(directorypath);
            if (tmp == null) return;

            //var templateId = tmp.ID;
            var dic = new Dictionary<String, String>();
            GetTemplates("default", dic);
            if (directorypath != "default") GetTemplates(directorypath, dic);

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