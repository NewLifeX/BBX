using System;
using BBX.Entity;
using BBX.Forum;
using XUser = BBX.Entity.User;

namespace BBX.Web.Admin
{
    public partial class combinationuser : AdminPage
    {
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
                if (String.IsNullOrEmpty(text))
                {
                    base.RegisterStartupScript("PAGE", "window.location.href='usergrid.aspx';");
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
                    //AdminUsers.CombinationUser(userId, targetUid);
                    User src = XUser.FindByID(userId);
                    User des = XUser.FindByID(targetUid);
                    des.Credits += src.Credits;
                    des.ExtCredits1 += src.ExtCredits1;
                    des.ExtCredits2 += src.ExtCredits2;
                    des.ExtCredits3 += src.ExtCredits3;
                    des.ExtCredits4 += src.ExtCredits4;
                    des.ExtCredits5 += src.ExtCredits5;
                    des.ExtCredits6 += src.ExtCredits6;
                    des.ExtCredits7 += src.ExtCredits7;
                    des.ExtCredits8 += src.ExtCredits8;
                    //Users.UpdateUser(userInfo2);
                    des.Save();
                    //BBX.Data.Users.CombinationUser(TableList.CurrentTableName, userInfo2, userInfo);
                    des.CombinationFrom(src);
                    src.Delete(true, true);

                    XForum.UpdateForumsFieldModerators(userName);
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