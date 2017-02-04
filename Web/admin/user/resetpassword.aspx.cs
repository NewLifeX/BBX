using System;
using System.Web.UI.HtmlControls;
using BBX.Common;
using BBX.Control;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web.Admin
{
    public partial class resetpassword : AdminPage
    {
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
            if (!this.password.Text.IsNullOrEmpty() && this.password.Text == this.passwordagain.Text)
            {
                User userInfo = Users.GetUserInfo(int.Parse(Request["uid"]));
                userInfo.Password = this.password.Text.Trim();
                Users.ResetPassword(userInfo);
                Sync.UpdatePassword(userInfo.Name, userInfo.Password, "");
                base.RegisterStartupScript("PAGE", "window.location.href='edituser.aspx?uid=" + Request["uid"] + "';");
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