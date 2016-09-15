using System;
using Discuz.Common;
using Discuz.Config;
using Discuz.Forum;

namespace Discuz.Web.Admin
{
    public class createtemplate : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["type"] == "single")
            {
                string @string = Request["filename"];
                string string2 = Request["path"];
                this.CreateSingleTemplate(@string, string2);
                return;
            }
            if (Request["type"] == "template")
            {
                string string3 = Request["templatepath"];
                this.CreateTemplate(string3);
            }
        }

        private void CreateSingleTemplate(string filename, string path)
        {
            int num = -1;
            int templateId = Convert.ToInt32(AdminTemplates.GetAllTemplateList(Utils.GetMapPath("..\\..\\templates\\")).Select("directory='" + path + "'")[0]["templateid"].ToString());
            if (filename != "")
            {
                ForumPageTemplate forumPageTemplate = new ForumPageTemplate();
                forumPageTemplate.GetTemplate(BaseConfigs.GetForumPath, path, filename, 1, templateId);
                num = 1;
            }
            base.Response.Write(num);
            base.Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1.0);
            base.Response.Expires = -1;
            base.Response.End();
        }

        private void CreateTemplate(string templatepath)
        {
            Globals.BuildTemplate(templatepath);
        }
    }
}