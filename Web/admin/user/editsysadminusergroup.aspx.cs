using System;
using System.Data;
using System.Web.UI.WebControls;
using BBX.Cache;
using BBX.Common;
using BBX.Entity;
using BBX.Forum;
using NewLife.Log;
using XCode;
using XUser = BBX.Entity.User;

namespace BBX.Web.Admin
{
    public partial class editsysadminusergroup : AdminPage
    {
        public UserGroup userGroupInfo = new UserGroup();
        //protected bool haveAlbum;
        //protected bool haveSpace;

        protected void Page_Load(object sender, EventArgs e)
        {
            //this.haveAlbum = (AlbumPluginProvider.GetInstance() != null);
            //this.haveSpace = (SpacePluginProvider.GetInstance() != null);
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
            //this.maxspaceattachsize.Text = this.userGroupInfo.MaxSpaceattachSize.ToString();
            //this.maxspacephotosize.Text = this.userGroupInfo.MaxSpacephotoSize.ToString();
            this.attachextensions.SetSelectByID(this.userGroupInfo.AttachExtensions.Trim());
            if (groupid > 0 && groupid <= 3)
            {
                this.radminid.Enabled = false;
            }
            this.radminid.SelectedValue = this.userGroupInfo.RadminID.ToString();
            this.usergrouppowersetting.Bind(this.userGroupInfo);
            if (this.radminid.SelectedValue == "1")
            {
                //this.allowstickthread.Enabled = false;
                //this.allowstickthread.SelectedValue = "3";
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
                UserGroup ug = UserGroup.FindByID(DNTRequest.GetInt("groupid", -1));
                this.userGroupInfo = ug;
                ug.System = 0;
                ug.Type = 0;
                ug.Readaccess = Convert.ToInt32(this.readaccess.Text);
                ug.AllowViewstats = false;
                ug.AllowNickName = false;
                ug.AllowHtml = false;
                ug.AllowCstatus = false;
                ug.AllowUsebLog = false;
                ug.AllowInvisible = false;
                ug.AllowTransfer = false;
                ug.AllowMultigroups = false;
                ug.ReasonPm = 0;
                Users.UpdateUserAdminIdByGroupId(ug.RadminID, ug.ID);
                ug.GroupTitle = this.groupTitle.Text;
                ug.Creditshigher = Convert.ToInt32(this.creditshigher.Text);
                ug.Creditslower = Convert.ToInt32(this.creditslower.Text);
                ug.Stars = Convert.ToInt32(this.stars.Text);
                ug.Color = this.color.Text;
                ug.Groupavatar = this.groupavatar.Text;
                ug.MaxPrice = Convert.ToInt32(this.maxprice.Text);
                ug.MaxPmNum = Convert.ToInt32(this.maxpmnum.Text);
                ug.MaxSigSize = Convert.ToInt32(this.maxsigsize.Text);
                ug.MaxAttachSize = Convert.ToInt32(this.maxattachsize.Text);
                ug.MaxSizeperday = Convert.ToInt32(this.maxsizeperday.Text);
                //ug.MaxSpaceattachSize = Convert.ToInt32(this.maxspaceattachsize.Text);
                //ug.MaxSpacephotoSize = Convert.ToInt32(this.maxspacephotosize.Text);
                ug.AttachExtensions = this.attachextensions.GetSelectString(",");
                this.usergrouppowersetting.GetSetting(ref ug);
                //if (AdminUserGroups.UpdateUserGroupInfo(ug))
                try
                {
                    userGroupInfo.Save();
                    //{
                    //XCache.Remove(CacheKeys.FORUM_USER_GROUP_LIST);

                    //DNTCache.Current.RemoveObject(CacheKeys.FORUM_ADMIN_GROUP_LIST);
                    AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "后台更新系统组", "组ID:" + DNTRequest.GetInt("groupid", -1));
                    base.RegisterStartupScript("PAGE", "window.location.href='sysadminusergroupgrid.aspx';");
                    return;
                }
                catch (Exception ex)
                {
                    XTrace.WriteException(ex);
                    base.RegisterStartupScript("", "<script>alert('操作失败');window.location.href='sysadminusergroupgrid.aspx';</script>");
                }
            }
        }

        private void radminid_SelectedIndexChanged(object sender, EventArgs e)
        {
            UserGroup userGroupInfo = UserGroup.FindByID(int.Parse(this.radminid.SelectedValue));
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
				DataTable attachmentType = AttachType.FindAllWithCache().ToDataTable(false);
                this.attachextensions.AddTableData(attachmentType, userGroupInfo.AttachExtensions);
            }
            AdminGroup adminGroupInfo = AdminGroup.FindByID(int.Parse(this.radminid.SelectedValue));
            if (adminGroupInfo != null)
            {
                //admingroupright.SelectedIndex = -1;
                //admingroupright.Items[0].Selected = adminGroupInfo.AllowEditPost;
                //admingroupright.Items[1].Selected = adminGroupInfo.AllowEditpoll;
                //admingroupright.Items[2].Selected = adminGroupInfo.AllowDelPost;
                //admingroupright.Items[3].Selected = adminGroupInfo.AllowMassprune;
                //admingroupright.Items[4].Selected = adminGroupInfo.AllowViewIP;
                //admingroupright.Items[5].Selected = adminGroupInfo.AllowEditUser;
                //admingroupright.Items[6].Selected = adminGroupInfo.AllowViewLog;
                //admingroupright.Items[7].Selected = adminGroupInfo.DisablePostctrl;
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
            foreach (UserGroup current in UserGroup.FindAll管理组())
            {
                this.radminid.Items.Add(new ListItem(current.GroupTitle, current.ID.ToString()));
            }
			//DataTable attachmentType = Attachments.GetAttachmentType();
			this.attachextensions.AddTableData(AttachType.FindAllWithCache(), null, null);
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