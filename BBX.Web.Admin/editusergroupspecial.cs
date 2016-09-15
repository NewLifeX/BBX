using System;
using System.Collections;
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
    public class editusergroupspecial : AdminPage
    {
        public UserGroup userGroupInfo = new UserGroup();
        protected bool haveAlbum;
        protected bool haveSpace;
        protected HtmlForm Form1;
        protected TabControl TabControl1;
        protected TabPage tabPage51;
        protected Discuz.Control.TextBox groupTitle;
        protected ColorPicker color;
        protected Discuz.Control.TextBox readaccess;
        protected RegularExpressionValidator RegularExpressionValidator1;
        protected Discuz.Control.TextBox maxpmnum;
        protected RegularExpressionValidator RegularExpressionValidator4;
        protected Discuz.Control.TextBox maxattachsize;
        protected Discuz.Control.TextBox maxspaceattachsize;
        protected Discuz.Control.CheckBoxList attachextensions;
        protected Discuz.Control.DropDownList radminid;
        protected Discuz.Control.TextBox stars;
        protected RegularExpressionValidator RegularExpressionValidator2;
        protected Discuz.Control.TextBox maxprice;
        protected RegularExpressionValidator RegularExpressionValidator3;
        protected Discuz.Control.TextBox maxsigsize;
        protected RegularExpressionValidator RegularExpressionValidator5;
        protected Discuz.Control.TextBox maxsizeperday;
        protected Discuz.Control.TextBox maxspacephotosize;
        protected TabPage tabPage22;
        protected usergrouppowersetting usergrouppowersetting;
        protected Hint Hint1;
        protected Discuz.Control.Button UpdateUserGroupInf;
        protected Discuz.Control.Button DeleteUserGroupInf;
        protected Discuz.Control.TextBox groupavatar;
        protected Discuz.Control.TextBox maxfriendscount;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.haveAlbum = (AlbumPluginProvider.GetInstance() != null);
            this.haveSpace = (SpacePluginProvider.GetInstance() != null);
        }

        public void LoadUserGroupInf(int groupid)
        {
            this.userGroupInfo = UserGroup.FindByID(groupid);
            this.groupTitle.Text = Utils.RemoveFontTag(this.userGroupInfo.GroupTitle);
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
            this.radminid.SelectedValue = ((this.userGroupInfo.RadminID == -1) ? "0" : this.userGroupInfo.RadminID.ToString());
            this.ViewState["radminid"] = this.userGroupInfo.RadminID;
            Attachments.GetAttachmentType();
            this.attachextensions.SetSelectByID(this.userGroupInfo.AttachExtensions.Trim());
            this.usergrouppowersetting.Bind(this.userGroupInfo);
            if (this.userGroupInfo.System == 1)
            {
                this.DeleteUserGroupInf.Enabled = false;
            }
        }

        private void DeleteUserGroupInf_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                if (AdminUserGroups.DeleteUserGroupInfo(DNTRequest.GetInt("groupid", -1)))
                {
                    DNTCache.Current.RemoveObject(CacheKeys.FORUM_USER_GROUP_LIST);
                    //UserGroup.FindAllWithCache();
                    AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "后台删除特殊用户组", "组ID:" + DNTRequest.GetInt("groupid", -1));
                    base.RegisterStartupScript("PAGE", "window.location.href='global_usergroupspecialgrid.aspx';");
                    return;
                }
                if (AdminUserGroups.opresult != "")
                {
                    base.RegisterStartupScript("", "<script>alert('操作失败,原因:" + AdminUserGroups.opresult + "');window.location.href='global_usergroupspecialgrid.aspx';</script>");
                    return;
                }
                base.RegisterStartupScript("", "<script>alert('操作失败');window.location.href='global_usergroupspecialgrid.aspx';</script>");
            }
        }

        public int BoolToInt(bool a)
        {
            if (a)
            {
                return 1;
            }
            return 0;
        }

        private void UpdateUserGroupInf_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                foreach (DictionaryEntry dictionaryEntry in new Hashtable
                {
                    {
                        "附件最大尺寸",
                        this.maxattachsize.Text
                    },

                    {
                        "每天最大附件总尺寸",
                        this.maxsizeperday.Text
                    },

                    {
                        "个人空间附件总尺寸",
                        this.maxspaceattachsize.Text
                    },

                    {
                        "相册空间总尺寸",
                        this.maxspacephotosize.Text
                    }
                })
                {
                    if (!Utils.IsInt(dictionaryEntry.Value.ToString()))
                    {
                        base.RegisterStartupScript("", "<script>alert('输入错误," + dictionaryEntry.Key.ToString() + "只能是0或者正整数');window.location.href='global_usergroupspecialgrid.aspx';</script>");
                        return;
                    }
                }
                this.userGroupInfo = UserGroup.FindByID(DNTRequest.GetInt("groupid", -1));
                this.userGroupInfo.System = 0;
                this.userGroupInfo.Type = 0;
                this.userGroupInfo.Readaccess = Convert.ToInt32(this.readaccess.Text);
                int num = (this.radminid.SelectedValue == "0") ? -1 : Convert.ToInt32(this.radminid.SelectedValue);
                this.userGroupInfo.RadminID = num;
                if (num.ToString() != this.ViewState["radminid"].ToString())
                {
                    Users.UpdateUserAdminIdByGroupId(this.userGroupInfo.RadminID, this.userGroupInfo.ID);
                }
                this.userGroupInfo.GroupTitle = this.groupTitle.Text;
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
                    UserGroup.FindAllWithCache();
                    AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "后台删除特殊用户组", "组ID:" + DNTRequest.GetInt("groupid", -1));
                    base.RegisterStartupScript("PAGE", "window.location.href='global_usergroupspecialgrid.aspx';");
                    return;
                }
                if (AdminUserGroups.opresult != "")
                {
                    base.RegisterStartupScript("", "<script>alert('操作失败,原因:" + AdminUserGroups.opresult + "');window.location.href='global_usergroupspecialgrid.aspx';</script>");
                    return;
                }
                base.RegisterStartupScript("", "<script>alert('操作失败');window.location.href='global_usergroupspecialgrid.aspx';</script>");
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
            this.DeleteUserGroupInf.Click += new EventHandler(this.DeleteUserGroupInf_Click);
            this.radminid.AddTableData(UserGroups.GetAdminGroups(), "grouptitle", "groupid");
            DataTable attachmentType = Attachments.GetAttachmentType();
            this.attachextensions.AddTableData(attachmentType);
            if (Request["groupid"] != "")
            {
                this.LoadUserGroupInf(DNTRequest.GetInt("groupid", -1));
                return;
            }
            base.Response.Redirect("global_sysadminusergroupgrid.aspx");
        }
    }
}