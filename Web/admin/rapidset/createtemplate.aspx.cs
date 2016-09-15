using System;
using BBX.Config;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web.Admin
{
    public partial class createtemplate : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["type"] == "single")
            {
                this.CreateSingleTemplate(Request["filename"], Request["path"]);
                return;
            }
            if (Request["type"] == "template")
            {
                ForumPageTemplate.BuildTemplate(Request["templatepath"]);
            }
        }

        private void CreateSingleTemplate(string filename, string path)
        {
            int num = -1;
            //int templateId = Convert.ToInt32(AdminTemplates.GetAllTemplateList(Utils.GetMapPath("..\\..\\templates\\")).Select("directory='" + path + "'")[0]["templateid"].ToString());
            Template tmp = Template.FindByPath(path);
            Int32 templateId = tmp.ID;
            if (filename != "")
            {
                ForumPageTemplate forumPageTemplate = new ForumPageTemplate();
                forumPageTemplate.GetTemplate(BaseConfigs.GetForumPath, path, filename, 1, tmp.Name);
                num = 1;
            }
            base.Response.Write(num);
            base.Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1.0);
            base.Response.Expires = -1;
            base.Response.End();
        }
    }
}