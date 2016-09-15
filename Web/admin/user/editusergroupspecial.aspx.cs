using System;
using System.Collections;
using BBX.Common;
using BBX.Entity;
using BBX.Forum;
using NewLife.Web;

namespace BBX.Web.Admin
{
    public partial class editusergroupspecial : AdminPage
    {
        public UserGroup userGroupInfo = new UserGroup();

        protected void Page_Load(object sender, EventArgs e)
        {
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
            this.radminid.SelectedValue = ((this.userGroupInfo.RadminID == -1) ? "0" : this.userGroupInfo.RadminID.ToString());
            this.ViewState["radminid"] = this.userGroupInfo.RadminID;
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
                Int32 groupid = WebHelper.RequestInt("groupid");
                UserGroup ug = UserGroup.FindByID(groupid);
                if (ug != null)
                {
                    try
                    {
                        ug.DeleteWithCredits();
                        AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "后台删除特殊用户组", "组ID:" + DNTRequest.GetInt("groupid", -1));
                        base.RegisterStartupScript("PAGE", "window.location.href='usergroupspecialgrid.aspx';");
                        return;
                    }
                    catch (Exception ex)
                    {
                        base.RegisterStartupScript("", "<script>alert('操作失败,原因:" + ex.Message + "');window.location.href='usergroupspecialgrid.aspx';</script>");
                        return;
                    }
                }
                base.RegisterStartupScript("", "<script>alert('操作失败');window.location.href='usergroupspecialgrid.aspx';</script>");
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
                Hashtable ht = new Hashtable();
                ht.Add("附件最大尺寸", this.maxattachsize.Text);
                ht.Add("每天最大附件总尺寸", this.maxsizeperday.Text);
                foreach (DictionaryEntry dictionaryEntry in ht)
                {
                    if (!Utils.IsInt(dictionaryEntry.Value.ToString()))
                    {
                        base.RegisterStartupScript("", "<script>alert('输入错误," + dictionaryEntry.Key.ToString() + "只能是0或者正整数');window.location.href='usergroupspecialgrid.aspx';</script>");
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
                this.userGroupInfo.AttachExtensions = this.attachextensions.GetSelectString(",");
                this.usergrouppowersetting.GetSetting(ref this.userGroupInfo);
                try
                {
                    userGroupInfo.UpdateWithCredits();
                    AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "后台删除特殊用户组", "组ID:" + DNTRequest.GetInt("groupid", -1));
                    base.RegisterStartupScript("PAGE", "window.location.href='usergroupspecialgrid.aspx';");
                    return;
                }
                catch (Exception ex)
                {
                    base.RegisterStartupScript("", "<script>alert('操作失败,原因:" + ex.Message + "');window.location.href='usergroupspecialgrid.aspx';</script>");
                    return;
                }
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
            this.radminid.AddTableData(UserGroup.FindAll管理组(), "grouptitle", "id");
            this.attachextensions.AddTableData(AttachType.FindAllWithCache(), null, null);
            if (Request["groupid"] != "")
            {
                this.LoadUserGroupInf(DNTRequest.GetInt("groupid", -1));
                return;
            }
            base.Response.Redirect("sysadminusergroupgrid.aspx");
        }
    }
}