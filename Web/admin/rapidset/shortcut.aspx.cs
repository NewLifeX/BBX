using System;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;
using BBX.Common;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web.Admin
{
    public partial class shortcut : AdminPage
    {
        public string filenamelist = "";
        protected bool isNew;
        protected bool isHotFix;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadTemplateInfo();
            this.forumid.BuildTree(XForum.Root, "Name", "ID");
        }

        public void LoadTemplateInfo()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(base.Server.MapPath("../../templates/" + this.Templatepath.SelectedValue + "/"));
            FileSystemInfo[] fileSystemInfos = directoryInfo.GetFileSystemInfos();
            for (int i = 0; i < fileSystemInfos.Length; i++)
            {
                FileSystemInfo fileSystemInfo = fileSystemInfos[i];
                if (fileSystemInfo != null)
                {
                    string text = fileSystemInfo.Extension.ToLower();
                    if (text.Equals(".htm") && fileSystemInfo.Name.IndexOf("_") != 0)
                    {
                        this.filenamelist = this.filenamelist + fileSystemInfo.Name.Split('.')[0] + "|";
                    }
                }
            }
        }

        private void EditForum_Click(object sender, EventArgs e)
        {
            if (this.forumid.SelectedValue != "0")
            {
                base.Response.Redirect("../forum/forum_EditForums.aspx?fid=" + this.forumid.SelectedValue);
                return;
            }
            base.RegisterStartupScript("", "<script>alert('请您选择有效的论坛版块!');</script>");
        }

        private void EditUserGroup_Click(object sender, EventArgs e)
        {
            if (this.Usergroupid.SelectedValue != "0")
            {
                int num = Convert.ToInt32(this.Usergroupid.SelectedValue);
                if (num >= 1 && num <= 3)
                {
                    base.Response.Redirect("../user/editadminusergroup.aspx?groupid=" + this.Usergroupid.SelectedValue);
                    return;
                }
                if (num >= 4 && num <= 8)
                {
                    base.Response.Redirect("../user/editsysadminusergroup.aspx?groupid=" + this.Usergroupid.SelectedValue);
                    return;
                }
                int radminid = UserGroup.FindByID(Utils.StrToInt(this.Usergroupid.SelectedValue, 0)).RadminID;
                if (radminid == 0)
                {
                    base.Response.Redirect("../user/editusergroup.aspx?groupid=" + this.Usergroupid.SelectedValue);
                    return;
                }
                if (radminid > 0)
                {
                    base.Response.Redirect("../user/editadminusergroup.aspx?groupid=" + this.Usergroupid.SelectedValue);
                    return;
                }
                if (radminid < 0)
                {
                    base.Response.Redirect("../user/editusergroupspecial.aspx?groupid=" + this.Usergroupid.SelectedValue);
                    return;
                }
            }
            else
            {
                base.RegisterStartupScript("", "<script>alert('请您选择有效的用户组!');</script>");
            }
        }

        private void UpdateForumStatistics_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                Caches.ReSetStatistics();
                base.RegisterStartupScript("PAGE", "window.location.href='shortcut.aspx';");
            }
        }

        private void UpdateCache_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                Caches.ReSetAllCache();
                base.RegisterStartupScript("PAGE", "window.location.href='shortcut.aspx';");
            }
        }

        private void CreateTemplate_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                ForumPageTemplate.BuildTemplate(this.Templatepath.SelectedValue);
                base.RegisterStartupScript("PAGE", "window.location.href='shortcut.aspx';");
            }
        }

        private void EditUser_Click(object sender, EventArgs e)
        {
            base.Response.Redirect("../user/usergrid.aspx?username=" + this.Username.Text.Trim());
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.EditUser.Click += new EventHandler(this.EditUser_Click);
            this.EditForum.Click += new EventHandler(this.EditForum_Click);
            this.EditUserGroup.Click += new EventHandler(this.EditUserGroup_Click);
            this.UpdateCache.Click += new EventHandler(this.UpdateCache_Click);
            this.CreateTemplate.Click += new EventHandler(this.CreateTemplate_Click);
            this.UpdateForumStatistics.Click += new EventHandler(this.UpdateForumStatistics_Click);
            foreach (Template tmp in Template.GetValids())
            {
                this.Templatepath.Items.Add(new ListItem(tmp.Name, tmp.Directory));
            }
            this.Username.AddAttributes("onkeydown", "if(event.keyCode==13) return(document.forms(0).EditUser.focus());");
            this.Usergroupid.AddTableData(UserGroup.GetAll(), "grouptitle", "id");
        }
    }
}