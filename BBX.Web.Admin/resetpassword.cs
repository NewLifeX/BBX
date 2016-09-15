using System;
using System.Web.UI.HtmlControls;
using Discuz.Common;
using Discuz.Control;
using Discuz.Entity;
using Discuz.Forum;

namespace Discuz.Web.Admin
{
    public class resetpassword : AdminPage
    {
        protected HtmlForm Form1;
        protected TextBox userName;
        protected TextBox password;
        protected TextBox passwordagain;
        protected Button ResetUserPWs;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                if (Request["uid"] == "")
                {
                    base.Response.Write("用户UID不能为空");
                    base.Response.End();
                    return;
                }
                User userInfo = Users.GetUserInfo(int.Parse(Request["uid"]));
                if (userInfo != null)
                {
                    this.userName.Text = userInfo.Name;
                    return;
                }
                base.Response.Write("当前用户不存在");
                base.Response.End();
            }
        }

        private void ResetUserPWs_Click(object sender, EventArgs e)
        {
            if (!Utils.StrIsNullOrEmpty(this.password.Text) && this.password.Text == this.passwordagain.Text)
            {
                User userInfo = Users.GetUserInfo(int.Parse(Request["uid"]));
                userInfo.Password = this.password.Text.Trim();
                Users.ResetPassword(userInfo);
                Sync.UpdatePassword(userInfo.Name, userInfo.Password, "");
                base.RegisterStartupScript("PAGE", "window.location.href='global_edituser.aspx?uid=" + Request["uid"] + "';");
                return;
            }
            base.RegisterStartupScript("", "<script>alert('新密码不能为空, 且两次输入的密码必须相同!');</script>");
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.ResetUserPWs.Click += new EventHandler(this.ResetUserPWs_Click);
        }
    }
}