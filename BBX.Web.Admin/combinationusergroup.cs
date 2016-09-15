using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Discuz.Cache;
using Discuz.Entity;
using Discuz.Forum;

namespace Discuz.Web.Admin
{
    public class combinationusergroup : AdminPage
    {
        protected HtmlForm Form1;
        protected pageinfo info1;
        protected pageinfo PageInfo1;
        protected Discuz.Control.DropDownList sourceusergroup;
        protected Discuz.Control.DropDownList targetusergroup;
        protected Discuz.Control.Button ComUsergroup;
        protected Discuz.Control.DropDownList sourceadminusergroup;
        protected Discuz.Control.DropDownList targetadminusergroup;
        protected Discuz.Control.Button ComAdminUsergroup;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                foreach (var current in UserGroup.FindAllWithCache())
                {
                    if (current.RadminID == 0)
                    {
                        this.sourceusergroup.Items.Add(new ListItem(current.GroupTitle, current.ID.ToString()));
                        this.targetusergroup.Items.Add(new ListItem(current.GroupTitle, current.ID.ToString()));
                    }
                }
                foreach (var current2 in UserGroup.GetAdminAndSpecialGroup())
                {
                    this.sourceadminusergroup.Items.Add(new ListItem(current2.GroupTitle, current2.ID.ToString()));
                    this.targetadminusergroup.Items.Add(new ListItem(current2.GroupTitle, current2.ID.ToString()));
                }
            }
        }

        private void ComUsergroup_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                if (this.sourceusergroup.SelectedIndex == 0 || this.targetusergroup.SelectedIndex == 0)
                {
                    base.RegisterStartupScript("", "<script>alert('操作失败,请您选择有效的用户组!');</script>");
                    return;
                }
                if (this.sourceusergroup.SelectedValue == this.targetusergroup.SelectedValue)
                {
                    base.RegisterStartupScript("", "<script>alert('操作失败,同一个用户组不能够合并!');</script>");
                    return;
                }
                var src = UserGroup.FindByID(int.Parse(this.sourceusergroup.SelectedValue));
                var des = UserGroup.FindByID(int.Parse(this.targetusergroup.SelectedValue));
                if (src.Creditslower != des.Creditshigher)
                {
                    base.RegisterStartupScript("", "<script>alert('操作失败,要合并的用户组必须是积分相连的两个用户组!');</script>");
                    return;
                }
                //var userGroupInfo = UserGroup.FindByID(int.Parse(this.targetusergroup.SelectedValue));
                des.Creditshigher = src.Creditshigher;
                //UserGroups.UpdateUserGroup(userGroupInfo);
                des.Save();
                //UserGroups.DeleteUserGroupInfo(int.Parse(this.sourceusergroup.SelectedValue));
                src.Delete();
                UserGroups.ChangeAllUserGroupId(int.Parse(this.sourceusergroup.SelectedValue), int.Parse(this.targetusergroup.SelectedValue));
                DNTCache.Current.RemoveObject(CacheKeys.FORUM_USER_GROUP_LIST);
                AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "合并用户组", "把组ID:" + this.sourceusergroup.SelectedIndex + " 合并到组ID:" + this.targetusergroup.SelectedIndex);
                base.RegisterStartupScript("PAGE", "window.location.href='global_usergroupgrid.aspx';");
            }
        }

        private void ComAdminUsergroup_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                if (this.sourceadminusergroup.SelectedIndex == 0 || this.targetadminusergroup.SelectedIndex == 0)
                {
                    base.RegisterStartupScript("", "<script>alert('操作失败,请您选择有效的管理组!');</script>");
                    return;
                }
                if (Convert.ToInt32(this.sourceadminusergroup.SelectedValue) <= 3 || Convert.ToInt32(this.sourceadminusergroup.SelectedValue) <= 3)
                {
                    base.RegisterStartupScript("", "<script>alert('操作失败,您选择的组为系统初始化的管理组,这些组不允许合并!');</script>");
                    return;
                }
                if (this.sourceadminusergroup.SelectedValue == this.targetadminusergroup.SelectedValue)
                {
                    base.RegisterStartupScript("", "<script>alert('操作失败,同一个管理组不能够合并!');</script>");
                    return;
                }

                //AdminGroups.DeleteAdminGroupInfo(Convert.ToInt16(this.sourceadminusergroup.SelectedValue));
                var adg = AdminGroup.FindByID(Convert.ToInt32(sourceadminusergroup.SelectedValue));
                if (adg != null) adg.Delete();
                //UserGroups.DeleteUserGroupInfo(int.Parse(this.sourceadminusergroup.SelectedValue));
                var ug = UserGroup.FindByID(adg.AdmingID);
                if (ug != null) ug.Delete();
                UserGroups.ChangeAllUserGroupId(int.Parse(this.sourceusergroup.SelectedValue), int.Parse(this.targetadminusergroup.SelectedValue));
                DNTCache.Current.RemoveObject(CacheKeys.FORUM_USER_GROUP_LIST);
                AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "合并管理组", "把组ID:" + this.sourceusergroup.SelectedIndex + " 合并到组ID:" + this.targetusergroup.SelectedIndex);
                base.RegisterStartupScript("PAGE", "window.location.href='global_adminusergroupgrid.aspx';");
            }
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.ComUsergroup.Click += new EventHandler(this.ComUsergroup_Click);
            this.ComAdminUsergroup.Click += new EventHandler(this.ComAdminUsergroup_Click);
        }
    }
}