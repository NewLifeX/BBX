using System;
using System.Collections;
using System.Data;
using BBX.Cache;
using BBX.Common;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web.Admin
{
    public partial class editusergroup : AdminPage
    {
        public UserGroup userGroupInfo = new UserGroup();

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void LoadUserGroupInf(int groupid)
        {
            this.userGroupInfo = UserGroup.FindByID(groupid);
            this.groupTitle.Text = Utils.RemoveFontTag(this.userGroupInfo.GroupTitle);
            this.creditshigher.Text = this.userGroupInfo.Creditshigher.ToString();
            this.creditslower.Text = this.userGroupInfo.Creditslower.ToString();
            if (UserGroup.GetUserGroupExceptGroupid(groupid).Count == 0)
            {
                this.creditshigher.Enabled = false;
                this.creditslower.Enabled = false;
            }
            this.ViewState["creditshigher"] = this.userGroupInfo.Creditshigher.ToString();
            this.ViewState["creditslower"] = this.userGroupInfo.Creditslower.ToString();
            this.stars.Text = this.userGroupInfo.Stars.ToString();
            this.color.Text = this.userGroupInfo.Color;
            this.groupavatar.Text = this.userGroupInfo.Groupavatar;
            this.readaccess.Text = this.userGroupInfo.Readaccess.ToString();
            this.maxprice.Text = this.userGroupInfo.MaxPrice.ToString();
            this.maxpmnum.Text = this.userGroupInfo.MaxPmNum.ToString();
            this.maxsigsize.Text = this.userGroupInfo.MaxSigSize.ToString();
            this.maxattachsize.Text = this.userGroupInfo.MaxAttachSize.ToString();
            this.maxsizeperday.Text = this.userGroupInfo.MaxSizeperday.ToString();
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
                int groupid = DNTRequest.GetInt("groupid", -1);
                UserGroup ug = UserGroup.FindByID(groupid);
                if (ug != null)
                {
                    try
                    {
                        AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "后台删除用户组", "组ID:" + DNTRequest.GetInt("groupid", -1));
                        base.RegisterStartupScript("PAGE", "window.location.href='usergroupgrid.aspx';");
                        return;
                    }
                    catch (Exception ex)
                    {
                        base.RegisterStartupScript("", "<script>alert('操作失败,原因:" + ex.Message + "');window.location.href='usergroupgrid.aspx';</script>");
                        return;
                    }
                }
                base.RegisterStartupScript("", "<script>alert('操作失败');window.location.href='usergroupgrid.aspx';</script>");
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
                        base.RegisterStartupScript("", "<script>alert('输入错误," + dictionaryEntry.Key.ToString() + "只能是0或者正整数');window.location.href='editusergroup.aspx';</script>");
                        return;
                    }
                }
                if (this.creditshigher.Enabled && (Convert.ToInt32(this.creditshigher.Text) < Convert.ToInt32(this.ViewState["creditshigher"].ToString()) || Convert.ToInt32(this.creditslower.Text) > Convert.ToInt32(this.ViewState["creditslower"].ToString())))
                {
                    base.RegisterStartupScript("", "<script>alert('操作失败, 您所输入的积分上下限范围应在" + this.ViewState["creditshigher"].ToString() + "至" + this.ViewState["creditslower"].ToString() + "之间');</script>");
                }
                else
                {
                    this.userGroupInfo = UserGroup.FindByID(DNTRequest.GetInt("groupid", -1));
                    this.userGroupInfo.System = 0;
                    this.userGroupInfo.Type = 0;
                    this.userGroupInfo.Readaccess = Convert.ToInt32(this.readaccess.Text);
                    this.usergrouppowersetting.GetSetting(ref this.userGroupInfo);
                    this.userGroupInfo.GroupTitle = this.groupTitle.Text;
                    this.userGroupInfo.Creditshigher = Convert.ToInt32(this.creditshigher.Text);
                    this.userGroupInfo.Creditslower = Convert.ToInt32(this.creditslower.Text);
                    if (this.userGroupInfo.Creditshigher >= this.userGroupInfo.Creditslower)
                    {
                        base.RegisterStartupScript("", "<script>alert('操作失败, 积分下限必须小于积分上限');</script>");
                        return;
                    }
                    if (this.userGroupInfo.AllowBonus && this.userGroupInfo.MinBonusprice >= this.userGroupInfo.MaxBonusprice)
                    {
                        base.RegisterStartupScript("", "<script>alert('操作失败, 最低悬赏价格必须小于最高悬赏价格');</script>");
                        return;
                    }
                    this.userGroupInfo.Stars = Convert.ToInt32(this.stars.Text);
                    this.userGroupInfo.Color = this.color.Text;
                    this.userGroupInfo.Groupavatar = this.groupavatar.Text;
                    this.userGroupInfo.MaxPrice = Convert.ToInt32(this.maxprice.Text);
                    this.userGroupInfo.MaxPmNum = Convert.ToInt32(this.maxpmnum.Text);
                    this.userGroupInfo.MaxSigSize = Convert.ToInt32(this.maxsigsize.Text);
                    this.userGroupInfo.MaxAttachSize = Convert.ToInt32(this.maxattachsize.Text);
                    this.userGroupInfo.MaxSizeperday = Convert.ToInt32(this.maxsizeperday.Text);
                    this.userGroupInfo.AttachExtensions = this.attachextensions.GetSelectString(",");
                    try
                    {
                        userGroupInfo.UpdateWithCredits();
                        UserGroup.FindAllWithCache();
                        AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "后台更新用户组", "组ID:" + DNTRequest.GetInt("groupid", -1));
                        base.RegisterStartupScript("PAGE", "window.location.href='usergroupgrid.aspx';");
                    }
                    catch (Exception ex)
                    {
                        base.RegisterStartupScript("", "<script>alert('操作失败,原因:" + ex.Message + "');window.location.href='usergroupgrid.aspx';</script>");
                    }
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
            this.attachextensions.AddTableData(AttachType.FindAllWithCache(), null, null);
            if (Request["groupid"] != "")
            {
                this.LoadUserGroupInf(DNTRequest.GetInt("groupid", -1));
                return;
            }
            base.Response.Redirect("usergroupgrid.aspx");
        }
    }
}