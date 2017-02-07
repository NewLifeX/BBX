using System;
using System.Data;
using System.Web.UI.WebControls;
using BBX.Common;
using BBX.Config;
using BBX.Entity;
using BBX.Forum;
using NewLife.Web;
using XUser = BBX.Entity.User;

namespace BBX.Web.Admin
{
    public partial class edituser : AdminPage
    {
        public User userInfo;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                //if (!this.AllowEditUser(this.userid, DNTRequest.GetInt("uid", -1)))
                //{
                //    base.Response.Write("<script>alert('非创始人身份不能修改其它管理员的信息!');window.location.href='usergrid.aspx';</script>");
                //    base.Response.End();
                //    return;
                //}
                this.IsEditUserName.Attributes.Add("onclick", "document.getElementById('" + this.userName.ClientID + "').disabled = !document.getElementById('" + this.IsEditUserName.ClientID + "').checked;");
            }
        }

        //private bool AllowEditUser(int managerUId, int targetUId)
        //{
        //    int num = Users.GetUserInfo(managerUId).GroupID;
        //    if (Users.GetUserInfo(managerUId).AdminID == 0)
        //    {
        //        return false;
        //    }
        //    int num2 = Users.GetUserInfo(targetUId).GroupID;
        //    int founderuid = BaseConfigInfo.Current.Founderuid;
        //    return managerUId == targetUId || managerUId == founderuid || num != num2;
        //}

        //public bool AllowEditUserInfo(int uid, bool redirect)
        //{
        //    if (BaseConfigInfo.Current.Founderuid == uid && uid == this.userid)
        //    {
        //        return true;
        //    }
        //    if (BaseConfigInfo.Current.Founderuid != uid)
        //    {
        //        return true;
        //    }
        //    if (redirect)
        //    {
        //        base.RegisterStartupScript("", "<script>alert('您要编辑信息是论坛创始人信息,请您以创始人身份登陆后台才能修改!');</script>");
        //    }
        //    return false;
        //}

        public bool IsValidScoreName(int scoreid)
        {
            bool result = false;
            foreach (DataRow dataRow in Scoresets.GetScoreSet().Rows)
            {
                if (dataRow["id"].ToString() != "1" && dataRow["id"].ToString() != "2" && dataRow[scoreid + 1].ToString().Trim() != "0")
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        public void LoadScoreInf(string fid, string fieldname)
        {
            DataRow dataRow = Scoresets.GetScoreSet().Rows[0];
            if (dataRow[2].ToString().Trim() != "")
            {
                this.extcredits1name.Text = dataRow[2].ToString().Trim();
            }
            else
            {
                if (!this.IsValidScoreName(1))
                {
                    this.extcredits1.Enabled = false;
                }
            }
            if (dataRow[3].ToString().Trim() != "")
            {
                this.extcredits2name.Text = dataRow[3].ToString().Trim();
            }
            else
            {
                if (!this.IsValidScoreName(2))
                {
                    this.extcredits2.Enabled = false;
                }
            }
            if (dataRow[4].ToString().Trim() != "")
            {
                this.extcredits3name.Text = dataRow[4].ToString().Trim();
            }
            else
            {
                if (!this.IsValidScoreName(3))
                {
                    this.extcredits3.Enabled = false;
                }
            }
            if (dataRow[5].ToString().Trim() != "")
            {
                this.extcredits4name.Text = dataRow[5].ToString().Trim();
            }
            else
            {
                if (!this.IsValidScoreName(4))
                {
                    this.extcredits4.Enabled = false;
                }
            }
            if (dataRow[6].ToString().Trim() != "")
            {
                this.extcredits5name.Text = dataRow[6].ToString().Trim();
            }
            else
            {
                if (!this.IsValidScoreName(5))
                {
                    this.extcredits5.Enabled = false;
                }
            }
            if (dataRow[7].ToString().Trim() != "")
            {
                this.extcredits6name.Text = dataRow[7].ToString().Trim();
            }
            else
            {
                if (!this.IsValidScoreName(6))
                {
                    this.extcredits6.Enabled = false;
                }
            }
            if (dataRow[8].ToString().Trim() != "")
            {
                this.extcredits7name.Text = dataRow[8].ToString().Trim();
            }
            else
            {
                if (!this.IsValidScoreName(7))
                {
                    this.extcredits7.Enabled = false;
                }
            }
            if (dataRow[9].ToString().Trim() != "")
            {
                this.extcredits8name.Text = dataRow[9].ToString().Trim();
            }
            else
            {
                if (!this.IsValidScoreName(8))
                {
                    this.extcredits8.Enabled = false;
                }
            }
            this.lblScoreCalFormula.Text = Scoresets.GetScoreCalFormula();
        }

        public void LoadCurrentUserInfo(int uid)
        {
            this.ViewState["username"] = this.userInfo.Name;
            this.userName.Text = this.userInfo.Name;
            if (this.userInfo.GroupID == 8 && this.config.Regverify == 1)
            {
                this.ReSendEmail.Visible = true;
            }
            else
            {
                this.ReSendEmail.Visible = false;
            }
            this.nickname.Text = this.userInfo.NickName;
            this.accessmasks.SelectedValue = this.userInfo.AccessMasks + "";
            this.bday.Text = this.userInfo.Bday;
            this.credits.Text = this.userInfo.Credits.ToString();
            this.digestposts.Text = this.userInfo.DigestPosts.ToString();
            this.email.Text = this.userInfo.Email;
            this.gender.SelectedValue = this.userInfo.Gender.ToString();
            //if (this.userInfo.GroupID.ToString() == "")
            //{
            //    this.groupid.SelectedValue = "0";
            //}
            //else
            //{
            if (this.groupid.Items.FindByValue(this.userInfo.GroupID.ToString()) != null)
            {
                this.groupid.SelectedValue = this.userInfo.GroupID.ToString();
            }
            else
            {
                UserGroup ug = CreditsFacade.GetCreditsUserGroupId((float)this.userInfo.Credits);
                if (ug != null) this.groupid.SelectedValue = ug.ID.ToString();
            }
            //}
            if (uid == BaseConfigs.GetFounderUid)
            {
                this.groupid.Enabled = false;
            }
            if (this.userInfo.GroupID == 4)
            {
                this.StopTalk.Text = "取消禁言";
                this.StopTalk.HintInfo = "取消禁言将会把当前用户所在的 \\'系统禁言\\' 组进行系统调整成为非禁言组";
            }
            this.ViewState["GroupID"] = this.userInfo.GroupID.ToString();
            this.invisible.SelectedValue = this.userInfo.Invisible.ToString();
            this.joindate.Text = this.userInfo.JoinDate.ToString("yyyy-MM-dd HH:mm:ss");
            this.lastactivity.Text = this.userInfo.LastActivity.ToString("yyyy-MM-dd HH:mm:ss");
            this.lastip.Text = this.userInfo.LastIP;
            this.lastpost.Text = this.userInfo.LastPost.ToString("yyyy-MM-dd HH:mm:ss");
            this.lastvisit.Text = this.userInfo.LastVisit.ToString("yyyy-MM-dd HH:mm:ss"); ;
            this.newpm.SelectedValue = this.userInfo.Newpm.ToString();
            switch ((ReceivePMSettingType)this.userInfo.NewsLetter)
            {
                case ReceivePMSettingType.ReceiveNone:
                    this.SetNewsLetter(false, false, false);
                    break;
                case ReceivePMSettingType.ReceiveSystemPM:
                    this.SetNewsLetter(true, false, false);
                    break;
                case ReceivePMSettingType.ReceiveUserPM:
                    this.SetNewsLetter(false, true, false);
                    break;
                case ReceivePMSettingType.ReceiveAllPM:
                    this.SetNewsLetter(true, true, false);
                    break;
                case ReceivePMSettingType.ReceiveSystemPMWithHint:
                    this.SetNewsLetter(true, false, true);
                    break;
                case ReceivePMSettingType.ReceiveUserPMWithHint:
                    this.SetNewsLetter(false, true, true);
                    break;
                default:
                    this.SetNewsLetter(true, true, true);
                    break;
            }
            this.oltime.Text = this.userInfo.OLTime.ToString();
            this.pageviews.Text = this.userInfo.PageViews.ToString();
            this.pmsound.Text = this.userInfo.Pmsound.ToString();
            this.posts.Text = this.userInfo.Posts.ToString();
            this.ppp.Text = this.userInfo.Ppp.ToString();
            this.regip.Text = this.userInfo.RegIP;
            this.showemail.SelectedValue = this.userInfo.ShowEmail.ToString();
            this.sigstatus.SelectedValue = this.userInfo.Sigstatus.ToString();
            if (this.userInfo.TemplateID != 0)
            {
                this.templateid.SelectedValue = this.userInfo.TemplateID.ToString();
            }
            this.tpp.Text = this.userInfo.Tpp.ToString();
            this.extcredits1.Text = this.userInfo.ExtCredits1.ToString();
            this.extcredits2.Text = this.userInfo.ExtCredits2.ToString();
            this.extcredits3.Text = this.userInfo.ExtCredits3.ToString();
            this.extcredits4.Text = this.userInfo.ExtCredits4.ToString();
            this.extcredits5.Text = this.userInfo.ExtCredits5.ToString();
            this.extcredits6.Text = this.userInfo.ExtCredits6.ToString();
            this.extcredits7.Text = this.userInfo.ExtCredits7.ToString();
            this.extcredits8.Text = this.userInfo.ExtCredits8.ToString();

            IUserField uf = userInfo.Field;
            this.website.Text = uf.Website;
            this.icq.Text = uf.Icq;
            this.qq.Text = uf.qq;
            this.yahoo.Text = uf.Yahoo;
            this.msn.Text = uf.Msn;
            this.skype.Text = uf.Skype;
            this.location.Text = uf.Location;
            this.customstatus.Text = uf.Customstatus;
            this.bio.Text = uf.Bio;
            this.signature.Text = uf.Signature;
            this.realname.Text = uf.RealName;
            this.idcard.Text = uf.Idcard;
            this.mobile.Text = uf.Mobile;
            this.phone.Text = uf.Phone;
            this.givenusername.Text = this.userInfo.Name;
            if (String.IsNullOrEmpty(uf.Medals)) uf.Medals = "0";

            string text = "," + uf.Medals + ",";
            //DataTable availableMedal = Medals.GetAvailableMedal();
            DataTable availableMedal = Medal.GetAvailable().ToDataTable(false);
            if (availableMedal != null)
            {
                DataColumn dataColumn = new DataColumn();
                dataColumn.ColumnName = "isgiven";
                dataColumn.DataType = typeof(Boolean);
                dataColumn.DefaultValue = false;
                dataColumn.AllowDBNull = false;
                availableMedal.Columns.Add(dataColumn);
                foreach (DataRow dataRow in availableMedal.Rows)
                {
                    if (text.IndexOf("," + dataRow["id"].ToString() + ",") >= 0)
                    {
                        dataRow["isgiven"] = true;
                    }
                }
                this.medalslist.DataSource = availableMedal;
                this.medalslist.DataBind();
            }
        }

        private void SetNewsLetter(bool item1, bool item2, bool item3)
        {
            this.newsletter.Items[0].Selected = item2;
            this.newsletter.Items[1].Selected = item3;
            if (!item2)
            {
                this.newsletter.Items[1].Selected = false;
                this.newsletter.Items[1].Enabled = false;
            }
        }

        private int GetNewsLetter()
        {
            int num = 0;
            int num2 = 0;
            if (this.newsletter.Items[0].Selected)
            {
                num = 2;
            }
            if (this.newsletter.Items[1].Selected)
            {
                num2 = 4;
            }
            return num | num2;
        }

        private void IsEditUserName_CheckedChanged(object sender, EventArgs e)
        {
            if (this.IsEditUserName.Checked)
            {
                this.userName.Enabled = true;
                return;
            }
            this.userName.Enabled = false;
        }

        public string BeGivenMedal(string isgiven, string medalid)
        {
            if (isgiven == "True")
            {
                return "<INPUT id=\"medalid\"  type=\"checkbox\" value=\"" + medalid + "\"  name=\"medalid\" checked>";
            }
            return "<INPUT id=\"medalid\"  type=\"checkbox\" value=\"" + medalid + "\"  name=\"medalid\">";
        }

        private void GivenMedal_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                int @int = DNTRequest.GetInt("uid", -1);
                this.GivenUserMedal(@int);
                if (Request["codition"] == "")
                {
                    this.Session["codition"] = null;
                }
                else
                {
                    this.Session["codition"] = Request["codition"].Replace("^", "'");
                }
                base.RegisterStartupScript("PAGE", "window.location.href='edituser.aspx?uid=" + @int + "&condition=" + Request["condition"] + "';");
            }
        }

        private void GivenUserMedal(int uid)
        {
            Users.UpdateMedals(uid, Request["medalid"], this.userid, this.username, WebHelper.UserHost, this.reason.Text.Trim());
        }

        private void ResetUserDigestPost_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                AdminForumStats.ReSetUserDigestPosts(DNTRequest.GetInt("uid", -1), DNTRequest.GetInt("uid", -1));
                base.RegisterStartupScript("PAGE", "window.location.href='edituser.aspx?uid=" + this.userInfo.ID + "&condition=" + Request["condition"] + "';");
            }
        }

        private void ResetUserPost_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                AdminForumStats.ReSetUserPosts(DNTRequest.GetInt("uid", -1), DNTRequest.GetInt("uid", -1));
                base.RegisterStartupScript("PAGE", "window.location.href='edituser.aspx?uid=" + this.userInfo.ID + "&condition=" + Request["condition"] + "';");
            }
        }

        private void ResetPassWord_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                //if (!this.AllowEditUserInfo(DNTRequest.GetInt("uid", -1), true))
                //{
                //    return;
                //}
                base.Response.Redirect("resetpassword.aspx?uid=" + Request["uid"]);
            }
        }

        private void StopTalk_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                this.userInfo = Users.GetUserInfo(DNTRequest.GetInt("uid", -1));
                //if (!this.AllowEditUserInfo(DNTRequest.GetInt("uid", -1), true))
                //{
                //    return;
                //}
                if (this.ViewState["GroupID"].ToString() != "4")
                {
                    if (this.userInfo.ID > 1)
                    {
                        Users.UpdateUserToStopTalkGroup(this.userInfo.ID.ToString());
                        base.RegisterStartupScript("PAGE", "window.location.href='edituser.aspx?uid=" + this.userInfo.ID + "&condition=" + Request["condition"] + "';");
                    }
                    else
                    {
                        base.RegisterStartupScript("", "<script>alert('操作失败,你要禁言的用户是系统初始化时的用户,因此不能操作!');window.location.href='edituser.aspx?uid=" + this.userInfo.ID + "&condition=" + Request["condition"] + "';</script>");
                    }
                }
                else
                {
                    if (CreditsFacade.GetCreditsUserGroupId(0f) != null)
                    {
                        int groupId = CreditsFacade.GetCreditsUserGroupId((float)this.userInfo.Credits).ID;
                        //Users.UpdateUserGroup(this.userInfo.ID, groupId);
                        userInfo.GroupID = groupId;
                        userInfo.Save();
                        base.RegisterStartupScript("PAGE", "window.location.href='edituser.aspx?uid=" + this.userInfo.ID + "&condition=" + Request["condition"] + "';");
                    }
                    else
                    {
                        base.RegisterStartupScript("", "<script>alert('操作失败,系统未能找到合适的用户组来调整当前用户所处的组!');window.location.href='edituser.aspx?uid=" + this.userInfo.ID + "&condition=" + Request["condition"] + "';</script>");
                    }
                }
                Online.DeleteUserByUid(this.userInfo.ID);
            }
        }

        private void DelPosts_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                int @int = DNTRequest.GetInt("uid", -1);
                //if (!this.AllowEditUserInfo(@int, true))
                //{
                //    return;
                //}
                Posts.ClearPosts(@int, 0);
                base.RegisterStartupScript("", "<script>alert('请到 论坛维护->论坛数据维护->重建指定主题区间帖数 对出现因为该操作产生\"读取信息失败\"的主题进行修复 ')</script>");
                base.RegisterStartupScript("PAGE", "window.location.href='edituser.aspx?uid=" + @int + "&condition=" + Request["condition"] + "';");
            }
        }

        private void ReSendEmail_Click(object sender, EventArgs e)
        {
            string authstr = ForumUtils.CreateAuthStr(20);
            Emails.SendRegMail(this.userName.Text, this.email.Text, "", authstr);
            string @string = Request["uid"];
            Users.UpdateEmailValidateInfo(authstr, DateTime.Now, int.Parse(@string));
            base.RegisterStartupScript("PAGE", "window.location.href='edituser.aspx?uid=" + @string + "&condition=" + Request["condition"] + "';");
        }

        private void SaveUserInfo_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                int uid = DNTRequest.GetInt("uid", -1);
                string text = "";
                //if (!this.AllowEditUserInfo(@int, true))
                //{
                //    return;
                //}
                if (this.userName.Text != this.ViewState["username"].ToString() && Users.GetUserId(this.userName.Text) > 0)
                {
                    base.RegisterStartupScript("", "<script>alert('您所输入的用户名已被使用过, 请输入其他的用户名!');</script>");
                    return;
                }
                if (this.userName.Text == "")
                {
                    base.RegisterStartupScript("", "<script>alert('用户名不能为空!');</script>");
                    return;
                }
                if (this.groupid.SelectedValue == "0")
                {
                    base.RegisterStartupScript("", "<script>alert('您未选中任何用户组!');</script>");
                    return;
                }
                this.userInfo = Users.GetUserInfo(uid);
                this.userInfo.Name = this.userName.Text;
                this.userInfo.NickName = this.nickname.Text;
                this.userInfo.AccessMasks = Utility.ToInt(this.accessmasks.SelectedValue, 0);
                if (this.userInfo.GroupID.ToString() != this.groupid.SelectedValue)
                {
                    this.userInfo.AdminID = UserGroup.FindByID(int.Parse(this.groupid.SelectedValue)).RadminID;
                }
                if (this.bday.Text == "0000-00-00" || (this.bday.Text == "0000-0-0" | this.bday.Text.Trim() == ""))
                {
                    this.userInfo.Bday = "";
                }
                else
                {
                    if (!Utils.IsDateString(this.bday.Text.Trim()))
                    {
                        base.RegisterStartupScript("", "<script>alert('用户生日不是有效的日期型数据!');</script>");
                        return;
                    }
                    this.userInfo.Bday = this.bday.Text;
                }
                if (!String.IsNullOrEmpty(email.Text) && !Users.ValidateEmail(this.email.Text, uid))
                {
                    base.RegisterStartupScript("", "<script>alert('当前用户的邮箱地址已被使用过, 请输入其他的邮箱!');</script>");
                    return;
                }
                this.userInfo.Email = this.email.Text;
                this.userInfo.Gender = Utility.ToInt(this.gender.SelectedValue, 0);
                this.userInfo.GroupExpiry = 0;
                this.userInfo.ExtGroupIds = this.extgroupids.GetSelectString(",");
                if (this.groupid.SelectedValue != "1" && this.userInfo.ID == BaseConfigs.GetFounderUid)
                {
                    base.RegisterStartupScript("", "<script>alert('创始人的所属用户组不能被修改为其它组!');window.location.href='edituser.aspx?uid=" + Request["uid"] + "';</script>");
                    return;
                }
                this.userInfo.GroupID = Utility.ToInt(this.groupid.SelectedValue, 0);
                this.userInfo.Invisible = Utility.ToInt(this.invisible.SelectedValue, 0) != 0;
                this.userInfo.JoinDate = Utility.ToDateTime(this.joindate.Text);
                this.userInfo.LastActivity = Utility.ToDateTime(this.lastactivity.Text);
                this.userInfo.LastIP = this.lastip.Text;
                this.userInfo.LastPost = Utility.ToDateTime(this.lastpost.Text);
                this.userInfo.LastVisit = Utility.ToDateTime(this.lastvisit.Text);
                this.userInfo.Newpm = Utility.ToInt(this.newpm.SelectedValue, 0) != 0;
                //this.userInfo.NewsLetter = (ReceivePMSettingType)this.GetNewsLetter();
                this.userInfo.NewsLetter = this.GetNewsLetter();
                this.userInfo.OLTime = Utility.ToInt(this.oltime.Text, 0);
                this.userInfo.PageViews = Utility.ToInt(this.pageviews.Text, 0);
                this.userInfo.Pmsound = Utility.ToInt(this.pmsound.Text, 0);
                this.userInfo.Posts = Utility.ToInt(this.posts.Text, 0);
                this.userInfo.Ppp = Utility.ToInt(this.ppp.Text, 0);
                this.userInfo.RegIP = this.regip.Text;
                this.userInfo.DigestPosts = Utility.ToInt(this.digestposts.Text, 0);
                if (this.secques.SelectedValue == "1") this.userInfo.Secques = "";

                this.userInfo.ShowEmail = Utility.ToInt(this.showemail.SelectedValue, 0) != 0;
                this.userInfo.Sigstatus = Utility.ToInt(this.sigstatus.SelectedValue, 0);
                this.userInfo.TemplateID = Utility.ToInt(this.templateid.SelectedValue, 0);
                this.userInfo.Tpp = Utility.ToInt(this.tpp.Text, 0);
                if (!Utils.IsNumeric(this.extcredits1.Text.Replace("-", "")))
                {
                    base.RegisterStartupScript("", "<script>alert('用户扩展积分不能为空或大于7位 !');</script>");
                    return;
                }
                this.userInfo.ExtCredits1 = float.Parse(this.extcredits1.Text);
                if (!Utils.IsNumeric(this.extcredits2.Text.Replace("-", "")))
                {
                    base.RegisterStartupScript("", "<script>alert('用户扩展积分不能为空或大于7位 !');</script>");
                    return;
                }
                this.userInfo.ExtCredits2 = float.Parse(this.extcredits2.Text);
                if (!Utils.IsNumeric(this.extcredits3.Text.Replace("-", "")))
                {
                    base.RegisterStartupScript("", "<script>alert('用户扩展积分不能为空或大于7位 !');</script>");
                    return;
                }
                this.userInfo.ExtCredits3 = float.Parse(this.extcredits3.Text);
                if (!Utils.IsNumeric(this.extcredits4.Text.Replace("-", "")))
                {
                    base.RegisterStartupScript("", "<script>alert('用户扩展积分不能为空或大于7位 !');</script>");
                    return;
                }
                this.userInfo.ExtCredits4 = float.Parse(this.extcredits4.Text);
                if (!Utils.IsNumeric(this.extcredits5.Text.Replace("-", "")))
                {
                    base.RegisterStartupScript("", "<script>alert('用户扩展积分不能为空或大于7位 !');</script>");
                    return;
                }
                this.userInfo.ExtCredits5 = float.Parse(this.extcredits5.Text);
                if (!Utils.IsNumeric(this.extcredits6.Text.Replace("-", "")))
                {
                    base.RegisterStartupScript("", "<script>alert('用户扩展积分不能为空或大于7位 !');</script>");
                    return;
                }
                this.userInfo.ExtCredits6 = float.Parse(this.extcredits6.Text);
                if (!Utils.IsNumeric(this.extcredits7.Text.Replace("-", "")))
                {
                    base.RegisterStartupScript("", "<script>alert('用户扩展积分不能为空或大于7位 !');</script>");
                    return;
                }
                this.userInfo.ExtCredits7 = float.Parse(this.extcredits7.Text);
                if (!Utils.IsNumeric(this.extcredits8.Text.Replace("-", "")))
                {
                    base.RegisterStartupScript("", "<script>alert('用户扩展积分不能为空或大于7位 !');</script>");
                    return;
                }
                this.userInfo.ExtCredits8 = float.Parse(this.extcredits8.Text);
                this.userInfo.Credits = CreditsFacade.GetUserCreditsByUserInfo(this.userInfo);
                //if (UserGroups.IsCreditUserGroup(this.userInfo.GroupID))
                if (userInfo.Group.IsCreditUserGroup)
                {
                    var g = CreditsFacade.GetCreditsUserGroupId((float)this.userInfo.Credits);
                    this.userInfo.GroupID = g.ID;
                    userInfo.Group = g;
                }
                IUserField uf = userInfo.Field;
                uf.Website = this.website.Text;
                uf.Icq = this.icq.Text;
                uf.qq = this.qq.Text;
                uf.Yahoo = this.yahoo.Text;
                uf.Msn = this.msn.Text;
                uf.Skype = this.skype.Text;
                uf.Location = this.location.Text;
                uf.Customstatus = this.customstatus.Text;
                uf.Bio = this.bio.Text;
                if (this.signature.Text.Length > userInfo.Group.MaxSigSize)
                {
                    text = "更新的签名长度超过 " + userInfo.Group.MaxSigSize + " 字符的限制，未能更新。";
                }
                else
                {
                    uf.Signature = this.signature.Text;
                    PostpramsInfo postpramsInfo = new PostpramsInfo();
                    postpramsInfo.Showimages = userInfo.Group.AllowSigimgCode ? 1 : 0;
                    postpramsInfo.Sdetail = this.signature.Text;
                    uf.Sightml = UBB.UBBToHTML(postpramsInfo);
                }
                uf.RealName = this.realname.Text;
                uf.Idcard = this.idcard.Text;
                uf.Mobile = this.mobile.Text;
                uf.Phone = this.phone.Text;
                uf.Medals = Request["medalid"];
                if (this.IsEditUserName.Checked && this.userName.Text != this.ViewState["username"].ToString())
                {
                    throw new NotImplementedException("UserNameChange");
                    //AdminUsers.UserNameChange(this.userInfo, this.ViewState["username"].ToString());
                    //Sync.RenameUser(userInfo.ID, ViewState["username"].ToString(), this.userInfo.Name, "");
                }
                if (userInfo.Save() > 0)
                {
                    Online.DeleteUserByUid(this.userInfo.ID);
                    if (this.delavart.Checked)
                    {
                        Avatars.DeleteAvatar(this.userInfo.ID.ToString());
                    }
                    AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "后台编辑用户", "用户名:" + this.userName.Text);
                    if (text == "")
                    {
                        base.RegisterStartupScript("PAGE", "window.location.href='usergrid.aspx?condition=" + Request["condition"] + "';");
                        return;
                    }
                    base.RegisterStartupScript("PAGE", "alert('" + text + "');window.location.href='usergrid.aspx?condition=" + Request["condition"] + "';");
                    return;
                }
                else
                {
                    base.RegisterStartupScript("", "<script>alert('操作失败');window.location.href='usergrid.aspx?condition=" + Request["condition"] + "';</script>");
                }
            }
        }

        private void DelUserInfo_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                int uid = DNTRequest.GetInt("uid", -1);
                if (this.AllowDeleteUser(this.userid, uid))
                {
                    bool delposts = this.deltype.SelectedValue.IndexOf("1") < 0;
                    bool delpms = this.deltype.SelectedValue.IndexOf("2") < 0;

                    User user = XUser.FindByID(uid);
                    if (user.Delete(delposts, delpms))
                    {
                        Sync.DeleteUsers(uid.ToString(), "");
                        Avatars.DeleteAvatar(uid.ToString());
                        XForum.UpdateForumsFieldModerators(this.userName.Text);
                        Online.DeleteUserByUid(this.userInfo.ID);
                        AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "后台删除用户", "用户名:" + this.userName.Text);
                        base.RegisterStartupScript("PAGE", "window.location.href='usergrid.aspx?condition=" + Request["condition"] + "';");
                        return;
                    }
                    base.RegisterStartupScript("", "<script>alert('操作失败');window.location.href='usergrid.aspx?condition=" + Request["condition"] + "';</script>");
                    return;
                }
                else
                {
                    base.RegisterStartupScript("", "<script>alert('操作失败,你要删除的用户是创始人用户或是其它管理员,因此不能删除!');window.location.href='usergrid.aspx?condition=" + Request["condition"] + "';</script>");
                }
            }
        }

        private bool AllowDeleteUser(int managerUId, int byDeleterUId)
        {
            int num = Users.GetUserInfo(managerUId).GroupID;
            int num2 = Users.GetUserInfo(byDeleterUId).GroupID;
            int founderuid = BaseConfigInfo.Current.Founderuid;
            return byDeleterUId != founderuid && (managerUId == founderuid || num != num2);
        }

        private void CalculatorScore_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                IUser shortUserInfo = BBX.Entity.User.FindByID(DNTRequest.GetInt("uid", -1));
                if (this.userInfo != null)
                {
                    this.credits.Text = CreditsFacade.GetUserCreditsByUserInfo(shortUserInfo).ToString();
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
            this.StopTalk.Click += new EventHandler(this.StopTalk_Click);
            this.DelPosts.Click += new EventHandler(this.DelPosts_Click);
            this.SaveUserInfo.Click += new EventHandler(this.SaveUserInfo_Click);
            this.ResetPassWord.Click += new EventHandler(this.ResetPassWord_Click);
            this.IsEditUserName.CheckedChanged += new EventHandler(this.IsEditUserName_CheckedChanged);
            this.DelUserInfo.Click += new EventHandler(this.DelUserInfo_Click);
            this.ReSendEmail.Click += new EventHandler(this.ReSendEmail_Click);
            this.CalculatorScore.Click += new EventHandler(this.CalculatorScore_Click);
            this.ResetUserDigestPost.Click += new EventHandler(this.ResetUserDigestPost_Click);
            this.ResetUserPost.Click += new EventHandler(this.ResetUserPost_Click);
            this.GivenMedal.Click += new EventHandler(this.GivenMedal_Click);

            this.userInfo = Users.GetUserInfo(DNTRequest.GetInt("uid", -1));

            UserGroup creditsUserGroupId = CreditsFacade.GetCreditsUserGroupId((float)this.userInfo.Credits);
            this.groupid.Items.Add(new ListItem(creditsUserGroupId.GroupTitle, creditsUserGroupId.ID.ToString()));
            foreach (UserGroup current in UserGroup.FindAllWithCache())
            {
                if ((current.System != 0 || current.RadminID != 0) && current.ID != 7)
                {
                    this.groupid.Items.Add(new ListItem(current.GroupTitle, current.ID.ToString()));
                    this.extgroupids.Items.Add(new ListItem(current.GroupTitle, current.ID.ToString()));
                }
            }
            this.templateid.AddTableData(Template.GetValids(), "name", "id");
            this.templateid.Items[0].Text = "默认";
            this.TabControl1.InitTabPage();
            if (Request["uid"] == "")
            {
                base.Response.Redirect("usergrid.aspx");
                return;
            }
            this.LoadCurrentUserInfo(DNTRequest.GetInt("uid", -1));
            this.LoadScoreInf(Request["uid"], Request["fieldname"]);
        }
    }

    public enum ReceivePMSettingType
    {
        ReceiveNone,
        ReceiveSystemPM,
        ReceiveUserPM,
        ReceiveAllPM,
        ReceiveSystemPMWithHint = 5,
        ReceiveUserPMWithHint,
        ReceiveAllPMWithHint
    }
}