using System;
using System.Web.UI.WebControls;
using BBX.Common;
using BBX.Config;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web.Admin
{
    public partial class AddUser : AdminPage
    {
       
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (!this.Page.IsPostBack)
            {
                foreach (UserGroup current in UserGroup.FindAllWithCache())
                {
                    this.groupid.Items.Add(new ListItem(current.GroupTitle, current.ID.ToString()));
                }
                this.AddUserInfo.Attributes.Add("onclick", "return IsValidPost();");
                string text = "var creditarray = new Array(";
                for (int i = 1; i < this.groupid.Items.Count; i++)
                {
                    text = text + UserGroup.FindByID(Convert.ToInt32(this.groupid.Items[i].Value)).Creditshigher.ToString() + ",";
                }
                text = text.TrimEnd(',') + ");";
                base.RegisterStartupScript("begin", "<script type='text/javascript'>" + text + "</script>");
                this.groupid.Attributes.Add("onchange", "document.getElementById('" + this.credits.ClientID + "').value=creditarray[this.selectedIndex];");
                this.groupid.Items.RemoveAt(0);
                try
                {
                    this.groupid.SelectedValue = "10";
                }
                catch
                {
                    this.groupid.SelectedValue = ((CreditsFacade.GetCreditsUserGroupId(0f) != null) ? CreditsFacade.GetCreditsUserGroupId(0f).ID.ToString() : "3");
                }
                try
                {
                    UserGroup userGroupInfo = UserGroup.FindByID(Convert.ToInt32(this.groupid.SelectedValue));
                    this.credits.Text = userGroupInfo.Creditshigher.ToString();
                }
                catch
                {
                }
            }
        }

        private void AddUserInfo_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                if (String.IsNullOrEmpty(this.userName.Text) || String.IsNullOrEmpty(this.password.Text))
                {
                    base.RegisterStartupScript("", "<script>alert('用户名或密码为空,因此无法提交!');window.location.href='adduser.aspx';</script>");
                    return;
                }
                if (!Utils.IsSafeSqlString(this.userName.Text))
                {
                    base.RegisterStartupScript("", "<script>alert('您输入的用户名包含不安全的字符,因此无法提交!');window.location.href='adduser.aspx';</script>");
                    return;
                }
                if ("系统" == this.userName.Text)
                {
                    base.RegisterStartupScript("", "<script>alert('您不能创建该用户名,因为它是系统保留的用户名,请您输入其它的用户名!');window.location.href='adduser.aspx';</script>");
                    return;
                }
                if (!Utils.IsValidEmail(this.email.Text.Trim()))
                {
                    base.RegisterStartupScript("", "<script>alert('E-mail为空或格式不正确,因此无法提交!');window.location='adduser.aspx';</script>");
                    return;
                }
                User userInfo = this.CreateUserInfo();
                if (Users.GetUserId(this.userName.Text) > 0)
                {
                    base.RegisterStartupScript("", "<script>alert('您所输入的用户名已被使用过, 请输入其他的用户名!');window.location.href='adduser.aspx';</script>");
                    return;
                }
                if (!Users.ValidateEmail(this.email.Text))
                {
                    base.RegisterStartupScript("", "<script>alert('您所输入的邮箱地址已被使用过, 请输入其他的邮箱!');window.location.href='adduser.aspx';</script>");
                    return;
                }
                //if (this.config.Passwordmode > 1 && PasswordModeProvider.GetInstance() != null)
                //{
                //    //PasswordModeProvider.GetInstance().CreateUserInfo(userInfo);
                //    throw new NotImplementedException();
                //}
                //else
                {
                    userInfo.Password = Utils.MD5(userInfo.Password);
                    //Users.CreateUser(userInfo);
                    userInfo.Save();
                }
                AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "后台添加用户", "用户名:" + this.userName.Text);
                if (this.sendemail.Checked)
                {
                    this.SendEmail(this.email.Text);
                }
                base.RegisterStartupScript("PAGE", "window.location.href='usergrid.aspx';");
            }
        }

        private User CreateUserInfo()
        {
            User user = new User();
            user.Name = this.userName.Text;
            user.NickName = this.userName.Text;
            user.Password = this.password.Text;
            user.Secques = "";
            user.Gender = 0;
            int num = Convert.ToInt32(this.groupid.SelectedValue);
            user.AdminID = UserGroup.FindByID(num).RadminID;
            user.GroupID = num;
            //user.JoinDate = DateTime.Now;
            //user.LastIP = "";
            //user.LastVisit = DateTime.Now;
            //user.LastActivity = DateTime.Now;
            //user.LastPost = DateTime.Now;
            //user.LastPostID = 0;
            //user.LastPostTitle = "";
            //user.Posts = 0;
            //user.DigestPosts = 0;
            //user.OLTime = 0;
            //user.PageViews = 0;
            user.Credits = Convert.ToInt32(this.credits.Text);
            //user.ExtCredits1 = 0f;
            //user.ExtCredits2 = 0f;
            //user.ExtCredits3 = 0f;
            //user.ExtCredits4 = 0f;
            //user.ExtCredits5 = 0f;
            //user.ExtCredits6 = 0f;
            //user.ExtCredits7 = 0f;
            //user.ExtCredits8 = 0f;
            user.Salt = "0";
            user.Email = this.email.Text;
            user.Bday = "";
            user.Sigstatus = 0;
            user.TemplateID = GeneralConfigInfo.Current.Templateid;
            user.Tpp = 16;
            user.Ppp = 16;
            user.Pmsound = 1;
            user.ShowEmail = true;
            //user.NewsLetter = (Int32)ReceivePMSettingType.ReceiveAllPMWithHint;
            user.NewsLetter = 7;
            user.Invisible = false;
            user.Newpm = false;
            user.AccessMasks = 0;

            IUserField uf = user.Field;
            //uf.Website = "";
            //uf.Icq = "";
            //uf.qq = "";
            //uf.Yahoo = "";
            //uf.Msn = "";
            //uf.Skype = "";
            //uf.Location = "";
            //uf.Customstatus = "";
            //uf.Medals = "";
            //uf.Bio = "";
            uf.Signature = this.userName.Text;
            //uf.Sightml = "";
            //uf.Authstr = "";
            uf.RealName = this.realname.Text;
            uf.Idcard = this.idcard.Text;
            uf.Mobile = this.mobile.Text;
            uf.Phone = this.phone.Text;
            return user;
        }

        public string SendEmail(string emailaddress)
        {
            bool flag = Emails.SendRegMail(this.userName.Text, emailaddress, this.password.Text, "");
            if (flag)
            {
                return "您的密码已经成功发送到您的E-mail中, 请注意查收!";
            }
            return "但发送邮件错误, 请您重新取回密码!";
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.AddUserInfo.Click += new EventHandler(this.AddUserInfo_Click);
            this.userName.IsReplaceInvertedComma = false;
            this.password.IsReplaceInvertedComma = false;
            this.email.IsReplaceInvertedComma = false;
        }
    }
}