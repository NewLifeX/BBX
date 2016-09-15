using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Discuz.Cache;
using Discuz.Common;
using Discuz.Control;
using Discuz.Entity;
using Discuz.Forum;
using Discuz.Plugin.Album;
using Discuz.Plugin.Space;

namespace Discuz.Web.Admin
{
    public class editsysadminusergroup : AdminPage
    {
        protected HtmlForm Form1;
        protected TabControl TabControl1;
        protected TabPage tabPage51;
        protected Discuz.Control.DropDownList radminid;
        protected ColorPicker color;
        protected Discuz.Control.TextBox readaccess;
        protected Discuz.Control.TextBox maxpmnum;
        protected Discuz.Control.TextBox maxattachsize;
        protected Discuz.Control.TextBox maxspaceattachsize;
        protected Discuz.Control.CheckBoxList attachextensions;
        protected Discuz.Control.TextBox groupTitle;
        protected Discuz.Control.TextBox stars;
        protected Discuz.Control.TextBox maxprice;
        protected Discuz.Control.TextBox maxsigsize;
        protected Discuz.Control.TextBox maxsizeperday;
        protected Discuz.Control.TextBox maxspacephotosize;
        protected TabPage tabPage22;
        protected usergrouppowersetting usergrouppowersetting;
        protected TabPage tabPage33;
        protected Discuz.Control.CheckBoxList admingroupright;
        protected Discuz.Control.DropDownList allowstickthread;
        protected Hint Hint1;
        protected Discuz.Control.Button UpdateUserGroupInf;
        protected Discuz.Control.TextBox creditshigher;
        protected Discuz.Control.TextBox creditslower;
        protected Discuz.Control.TextBox groupavatar;
        public UserGroup userGroupInfo = new UserGroup();
        protected bool haveAlbum;
        protected bool haveSpace;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.haveAlbum = (AlbumPluginProvider.GetInstance() != null);
            this.haveSpace = (SpacePluginProvider.GetInstance() != null);
        }

        public void LoadUserGroupInf(int groupid)
        {
            this.userGroupInfo = UserGroup.FindByID(groupid);
            this.groupTitle.Text = Utils.RemoveFontTag(this.userGroupInfo.GroupTitle);
            this.creditshigher.Text = this.userGroupInfo.Creditshigher.ToString();
            this.creditslower.Text = this.userGroupInfo.Creditslower.ToString();
            this.stars.Text = this.userGroupInfo.Stars.ToString();
            this.color.Text = this.userGroupInfo.Color;
            this.groupavatar.Text = this.userGroupInfo.Groupavatar;
            this.readaccess.Text = this.userGroupInfo.Readaccess.ToString();
            this.maxprice.Text = this.userGroupInfo.MaxPrice.ToString();
            this.maxpmnum.Text = this.userGroupInfo.MaxPmNum.ToString();
            this.maxsigsize.Text = this.userGroupInfo.MaxSigSize.ToString();
            this.maxattachsize.Text = this.userGroupInfo.MaxAttachSize.ToString();
            this.maxsizeperday.Text = this.userGroupInfo.MaxSizeperday.ToString();
            this.maxspaceattachsize.Text = this.userGroupInfo.MaxSpaceattachSize.ToString();
            this.maxspacephotosize.Text = this.userGroupInfo.MaxSpacephotoSize.ToString();
            this.attachextensions.SetSelectByID(this.userGroupInfo.AttachExtensions.Trim());
            if (groupid > 0 && groupid <= 3)
            {
                this.radminid.Enabled = false;
            }
            this.radminid.SelectedValue = this.userGroupInfo.RadminID.ToString();
            this.usergrouppowersetting.Bind(this.userGroupInfo);
            if (this.radminid.SelectedValue == "1")
            {
                this.allowstickthread.Enabled = false;
                this.allowstickthread.SelectedValue = "3";
            }
        }

        public int BoolToInt(bool a)
        {
            if (!a)
            {
                return 0;
            }
            return 1;
        }

        public byte BoolToByte(bool a)
        {
            return (Byte)(a ? 1 : 0);
        }

        private void UpdateUserGroupInf_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                this.userGroupInfo = UserGroup.FindByID(DNTRequest.GetInt("groupid", -1));
                this.userGroupInfo.System = 0;
                this.userGroupInfo.Type = 0;
                this.userGroupInfo.Readaccess = Convert.ToInt32(this.readaccess.Text);
                this.userGroupInfo.AllowViewstats = false;
                this.userGroupInfo.AllowNickName = false;
                this.userGroupInfo.AllowHtml = false;
                this.userGroupInfo.AllowCstatus = false;
                this.userGroupInfo.AllowUsebLog = false;
                this.userGroupInfo.AllowInvisible = false;
                this.userGroupInfo.AllowTransfer = false;
                this.userGroupInfo.AllowMultigroups = false;
                this.userGroupInfo.ReasonPm = 0;
                Users.UpdateUserAdminIdByGroupId(this.userGroupInfo.RadminID, this.userGroupInfo.ID);
                this.userGroupInfo.GroupTitle = this.groupTitle.Text;
                this.userGroupInfo.Creditshigher = Convert.ToInt32(this.creditshigher.Text);
                this.userGroupInfo.Creditslower = Convert.ToInt32(this.creditslower.Text);
                this.userGroupInfo.Stars = Convert.ToInt32(this.stars.Text);
                this.userGroupInfo.Color = this.color.Text;
                this.userGroupInfo.Groupavatar = this.groupavatar.Text;
                this.userGroupInfo.MaxPrice = Convert.ToInt32(this.maxprice.Text);
                this.userGroupInfo.MaxPmNum = Convert.ToInt32(this.maxpmnum.Text);
                this.userGroupInfo.MaxSigSize = Convert.ToInt32(this.maxsigsize.Text);
                this.userGroupInfo.MaxAttachSize = Convert.ToInt32(this.maxattachsize.Text);
                this.userGroupInfo.MaxSizeperday = Convert.ToInt32(this.maxsizeperday.Text);
                this.userGroupInfo.MaxSpaceattachSize = Convert.ToInt32(this.maxspaceattachsize.Text);
                this.userGroupInfo.MaxSpacephotoSize = Convert.ToInt32(this.maxspacephotosize.Text);
                this.userGroupInfo.AttachExtensions = this.attachextensions.GetSelectString(",");
                this.usergrouppowersetting.GetSetting(ref this.userGroupInfo);
                //if (AdminUserGroups.UpdateUserGroupInfo(this.userGroupInfo))
                userGroupInfo.Save();
                {
                    DNTCache.Current.RemoveObject(CacheKeys.FORUM_USER_GROUP_LIST);

                    //DNTCache.Current.RemoveObject(CacheKeys.FORUM_ADMIN_GROUP_LIST);
                    AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "后台更新系统组", "组ID:" + DNTRequest.GetInt("groupid", -1));
                    base.RegisterStartupScript("PAGE", "window.location.href='global_sysadminusergroupgrid.aspx';");
                    return;
                }
                base.RegisterStartupScript("", "<script>alert('操作失败');window.location.href='global_sysadminusergroupgrid.aspx';</script>");
            }
        }

        private void radminid_SelectedIndexChanged(object sender, EventArgs e)
        {
            var userGroupInfo = UserGroup.FindByID(int.Parse(this.radminid.SelectedValue));
            if (userGroupInfo != null)
            {
                this.creditshigher.Text = userGroupInfo.Creditslower.ToString();
                this.creditslower.Text = userGroupInfo.Creditslower.ToString();
                this.stars.Text = userGroupInfo.Stars.ToString();
                this.color.Text = userGroupInfo.Color;
                this.groupavatar.Text = userGroupInfo.Groupavatar;
                this.readaccess.Text = userGroupInfo.Readaccess.ToString();
                this.maxprice.Text = userGroupInfo.MaxPrice.ToString();
                this.maxpmnum.Text = userGroupInfo.MaxPmNum.ToString();
                this.maxsigsize.Text = userGroupInfo.MaxSigSize.ToString();
                this.maxattachsize.Text = userGroupInfo.MaxAttachSize.ToString();
                this.maxsizeperday.Text = userGroupInfo.MaxSizeperday.ToString();
                DataTable attachmentType = Attachments.GetAttachmentType();
                this.attachextensions.AddTableData(attachmentType, userGroupInfo.AttachExtensions);
            }
            var adminGroupInfo = AdminGroup.FindByID(int.Parse(this.radminid.SelectedValue));
            if (adminGroupInfo != null)
            {
                admingroupright.SelectedIndex = -1;
                admingroupright.Items[0].Selected = adminGroupInfo.AllowEditPost;
                admingroupright.Items[1].Selected = adminGroupInfo.AllowEditpoll;
                admingroupright.Items[2].Selected = adminGroupInfo.AllowDelPost;
                admingroupright.Items[3].Selected = adminGroupInfo.AllowMassprune;
                admingroupright.Items[4].Selected = adminGroupInfo.AllowViewIP;
                admingroupright.Items[5].Selected = adminGroupInfo.AllowEditUser;
                admingroupright.Items[6].Selected = adminGroupInfo.AllowViewLog;
                admingroupright.Items[7].Selected = adminGroupInfo.DisablePostctrl;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.TabControl1.InitTabPage();
            this.UpdateUserGroupInf.Click += new EventHandler(this.UpdateUserGroupInf_Click);
            this.radminid.Items.Add(new ListItem("请选择     ", "0"));
            foreach (var current in UserGroup.GetAdminUserGroup())
            {
                this.radminid.Items.Add(new ListItem(current.GroupTitle, current.ID.ToString()));
            }
            DataTable attachmentType = Attachments.GetAttachmentType();
            this.attachextensions.AddTableData(attachmentType);
            string @string = Request["groupid"];
            if (@string != "")
            {
                this.LoadUserGroupInf(DNTRequest.GetInt("groupid", -1));
                return;
            }
            base.Response.Redirect("sysglobal_sysadminusergroupgrid.aspx");
        }
    }
}