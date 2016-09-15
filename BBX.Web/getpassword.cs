using BBX.Common;
using BBX.Forum;

namespace BBX.Web
{
    public class getpassword : PageBase
    {
        public string findusername = "";

        protected override void ShowPage()
        {
            this.pagetitle = "密码找回";
            this.findusername = DNTRequest.GetString("findusername");
            string errinfo = "";
            if (!string.IsNullOrEmpty(this.findusername) && !Users.PageValidateUserName(this.findusername, out errinfo, false))
            {
                base.AddErrLine(errinfo);
                return;
            }
            if (DNTRequest.IsPost())
            {
                if (ForumUtils.IsCrossSitePost())
                {
                    base.AddErrLine("您的请求来路不正确，无法提交。如果您安装了某种默认屏蔽来路信息的个人防火墙软件(如 Norton Internet Security)，请设置其不要禁止来路信息后再试。");
                    return;
                }
                base.SetBackLink("getpassword.aspx?findusername=" + this.findusername.Replace("\"", "").Replace("'", "").Replace("<", "").Replace(">", ""));
                if (Users.GetUserId(this.findusername) == 0)
                {
                    base.AddErrLine("用户不存在");
                    return;
                }
                if (Utils.StrIsNullOrEmpty(DNTRequest.GetString("email")))
                {
                    base.AddErrLine("电子邮件不能为空");
                    return;
                }
                if (base.IsErr())
                {
                    return;
                }
                if (Users.CheckEmailAndSecques(this.findusername, DNTRequest.GetString("email"), DNTRequest.GetInt("question", 0), DNTRequest.GetString("answer"), this.GetForumPath()))
                {
                    base.SetUrl(this.forumpath);
                    base.SetMetaRefresh(5);
                    base.SetShowBackLink(false);
                    base.MsgForward("getpassword_succeed");
                    base.AddMsgLine("取回密码的方法已经通过 Email 发送到您的信箱中,<br />请在 3 天之内到论坛修改您的密码.");
                    return;
                }
                base.AddErrLine("用户名,Email 地址或安全提问不匹配,请返回修改.");
            }
        }

        private string GetForumPath()
        {
            return this.Context.Request.Url.ToString().ToLower().Substring(0, this.Context.Request.Url.ToString().ToLower().IndexOf("/aspx/"));
        }
    }
}