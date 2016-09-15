using System;
using System.Web.UI.HtmlControls;
using Discuz.Control;
using Discuz.Entity;
using Discuz.Forum;

namespace Discuz.Web.Admin
{
    public class combinationuser : AdminPage
    {
        protected HtmlForm Form1;
        protected pageinfo info1;
        protected TextBox username1;
        protected TextBox username3;
        protected TextBox username2;
        protected TextBox targetusername;
        protected Hint Hint1;
        protected Button CombinationUserInfo;

        private void CombinationUserInfo_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                int userId = Users.GetUserId(this.targetusername.Text);
                string text = "";
                if (userId > 0)
                {
                    text = this.CombinationUser(this.username1.Text.Trim(), this.targetusername.Text.Trim(), userId);
                    text += this.CombinationUser(this.username2.Text.Trim(), this.targetusername.Text.Trim(), userId);
                    text += this.CombinationUser(this.username3.Text.Trim(), this.targetusername.Text.Trim(), userId);
                }
                else
                {
                    text = text + "目标用户:" + this.targetusername.Text + "不存在!,";
                }
                if (text == "")
                {
                    base.RegisterStartupScript("PAGE", "window.location.href='global_usergrid.aspx';");
                    return;
                }
                base.RegisterStartupScript("", "<script>alert('" + text.Replace("'", "’") + "');</script>");
            }
        }

        private string CombinationUser(string userName, string targetUserName, int targetUid)
        {
            string result = "";
            if (userName != "" && targetUserName != userName)
            {
                int userId = Users.GetUserId(userName);
                if (userId > 0)
                {
                    AdminUsers.CombinationUser(userId, targetUid);
                    AdminUsers.UpdateForumsFieldModerators(userName);
                    AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "合并用户", "把用户" + userName + " 合并到" + targetUserName);
                }
                else
                {
                    result = "用户:" + userName + "不存在!,";
                }
            }
            return result;
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.CombinationUserInfo.Click += new EventHandler(this.CombinationUserInfo_Click);
        }
    }
}