using System;
using System.Text;
using BBX.Common;
using BBX.Entity;
using BBX.Forum;
using NewLife.Web;

namespace BBX.Web
{
    public class setnewpassword : PageBase
    {
        private int uid = DNTRequest.GetInt("uid", 0);
        private string Authstr = (DNTRequest.GetQueryString("id") != "") ? DNTRequest.GetQueryString("id") : DNTRequest.GetString("authstr");

        protected override void ShowPage()
        {
            this.pagetitle = "密码找回";
            this.username = DNTRequest.GetString("username");
            base.SetBackLink("/");
            var userInfo = Users.GetUserInfo(this.uid);
            if (userInfo == null)
            {
                base.AddErrLine("用户名不存在,你无法重设密码");
                return;
            }
            if (!userInfo.Field.Authstr.Equals(this.Authstr) || userInfo.Field.AuthTime.ToDateTime() < DateTime.Now.AddDays(-3.0))
            {
                this.ReSendMail(userInfo.ID, userInfo.Name, userInfo.Email.Trim());
                base.SetUrl("/");
                base.SetMetaRefresh(5);
                base.SetShowBackLink(false);
                base.AddErrLine("验证码已失效,新的验证码已经通过 Email 发送到您的信箱中,<BR />请在 3 天之内到论坛修改您的密码.");
                return;
            }
            if (DNTRequest.IsPost())
            {
                if (ForumUtils.IsCrossSitePost())
                {
                    base.AddErrLine("您的请求来路不正确，无法提交。如果您安装了某种默认屏蔽来路信息的个人防火墙软件(如 Norton Internet Security)，请设置其不要禁止来路信息后再试。");
                    return;
                }
                base.SetBackLink("setnewpassword.aspx?uid=" + this.uid + "&id=" + this.Authstr);
                if (String.IsNullOrEmpty(DNTRequest.GetString("newpassword")))
                {
                    base.AddErrLine("新密码不能为空");
                }
                if (!DNTRequest.GetString("newpassword").Equals(DNTRequest.GetString("confirmpassword")))
                {
                    base.AddErrLine("两次密码输入不一致");
                }
                if (Utils.StrIsNullOrEmpty(this.Authstr) || !userInfo.Field.Authstr.Equals(this.Authstr))
                {
                    base.AddErrLine("您所提供的验证码与注册信息不符.");
                }
                if (base.IsErr())
                {
                    return;
                }
                if (Utils.IsSafeSqlString(DNTRequest.GetString("newpassword")) && Users.UpdateUserPassword(this.uid, DNTRequest.GetString("newpassword"), true))
                {
                    Users.UpdateAuthStr(this.uid, "", 0);
                    base.SetUrl("login.aspx");
                    base.SetMetaRefresh(5);
                    base.SetShowBackLink(false);
                    base.MsgForward("setnewpassword_succeed");
                    base.AddMsgLine("你的密码已被重新设置,请用新密码登录.");
                    return;
                }
                base.AddErrLine("用户名,Email 地址或安全提问不匹配,请返回修改.");
            }
        }

        private void ReSendMail(int uid, string username, string email)
        {
            string text = this.Context.Request.Url.ToString().ToLower().Substring(0, this.Context.Request.Url.ToString().ToLower().IndexOf("/aspx/"));
            string text2 = ForumUtils.CreateAuthStr(20);
            string title = this.config.Forumtitle + " 取回密码说明";
            Users.UpdateAuthStr(uid, text2, 2);
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(username);
            stringBuilder.Append("您好!<BR />这封信是由 ");
            stringBuilder.Append(this.config.Forumtitle);
            stringBuilder.Append(" 发送的.<BR /><BR />您收到这封邮件,是因为在我们的论坛上这个邮箱地址被登记为用户邮箱,且该用户请求使用 Email 密码重置功能所致.");
            stringBuilder.Append("<BR /><BR />----------------------------------------------------------------------");
            stringBuilder.Append("<BR />重要！");
            stringBuilder.Append("<BR /><BR />----------------------------------------------------------------------");
            stringBuilder.Append("<BR /><BR />如果您没有提交密码重置的请求或不是我们论坛的注册用户,请立即忽略并删除这封邮件.只在您确认需要重置密码的情况下,才继续阅读下面的内容.");
            stringBuilder.Append("<BR /><BR />----------------------------------------------------------------------");
            stringBuilder.Append("<BR />密码重置说明");
            stringBuilder.Append("<BR /><BR />----------------------------------------------------------------------");
            stringBuilder.Append("<BR /><BR />您只需在提交请求后的三天之内,通过点击下面的链接重置您的密码:");
            stringBuilder.Append("<BR /><BR /><a href=" + text + "/setnewpassword.aspx?uid=" + uid + "&id=" + text2 + " target=_blank>");
            stringBuilder.Append(text);
            stringBuilder.Append("/setnewpassword.aspx?uid=");
            stringBuilder.Append(uid);
            stringBuilder.Append("&id=");
            stringBuilder.Append(text2);
            stringBuilder.Append("</a>");
            stringBuilder.Append("<BR /><BR />(如果上面不是链接形式,请将地址手工粘贴到浏览器地址栏再访问)");
            stringBuilder.Append("<BR /><BR />上面的页面打开后,输入新的密码后提交,之后您即可使用新的密码登录论坛了.您可以在用户控制面板中随时修改您的密码.");
            stringBuilder.Append("<BR /><BR />本请求提交者的 IP 为 ");
            stringBuilder.Append(WebHelper.UserHost);
            stringBuilder.Append("<BR /><BR /><BR /><BR />");
            stringBuilder.Append("<BR />此致 <BR /><BR />");
            stringBuilder.Append(this.config.Forumtitle);
            stringBuilder.Append(" 管理团队.");
            stringBuilder.Append("<BR />");
            stringBuilder.Append(text);
            stringBuilder.Append("<BR /><BR />");
            Emails.SendMailToUser(email, title, stringBuilder.ToString());
        }
    }
}