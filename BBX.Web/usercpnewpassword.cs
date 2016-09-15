using BBX.Common;
using BBX.Entity;
using BBX.Forum;
using BBX.Web.UI;

namespace BBX.Web
{
    public class usercpnewpassword : UserCpPage
    {
        public bool isconnectsetpassword;

        protected override void ShowPage()
        {
            this.pagetitle = "用户控制面板";
            if (!base.IsLogin())
            {
                return;
            }
            UserConnect userConnectInfo = UserConnect.FindByUid(userid);
            if (this.isbindconnect)
            {
                //userConnectInfo = DiscuzCloud.GetUserConnectInfo(this.userid);
                this.isconnectsetpassword = (userConnectInfo != null && !userConnectInfo.IsSetPassword);
            }
            if (DNTRequest.IsPost())
            {
                var userInfo = Users.GetUserInfo(this.userid);
                string @string = DNTRequest.GetString("newpassword");
                if (!this.isconnectsetpassword)
                {
                    //if (this.config.Passwordmode > 1 && PasswordModeProvider.GetInstance() != null)
                    //{
                    //    if (!PasswordModeProvider.GetInstance().CheckPassword(userInfo, DNTRequest.GetString("oldpassword")))
                    //    {
                    //        base.AddErrLine("您的原密码错误");
                    //        return;
                    //    }
                    //}
                    //else
                    {
                        if (BBX.Entity.User.Check(this.userid, DNTRequest.GetString("oldpassword"), true) == null)
                        {
                            base.AddErrLine("您的原密码错误");
                            return;
                        }
                    }
                }
                if (@string != DNTRequest.GetString("newpassword2"))
                {
                    base.AddErrLine("新密码两次输入不一致");
                    return;
                }
                if (Utils.StrIsNullOrEmpty(@string))
                {
                    @string = DNTRequest.GetString("oldpassword");
                }
                if (@string.Length < 6)
                {
                    base.AddErrLine("密码不得少于6个字符");
                    return;
                }
                userInfo.Password = @string;
                Users.ResetPassword(userInfo);
                Sync.UpdatePassword(userInfo.Name, userInfo.Password, "");
                if (!Utils.StrIsNullOrEmpty(DNTRequest.GetString("changesecques")))
                {
                    Users.UpdateUserSecques(this.userid, DNTRequest.GetInt("question", 0), DNTRequest.GetString("answer"));
                }
                ForumUtils.WriteCookie("password", ForumUtils.SetCookiePassword(userInfo.Password, this.config.Passwordkey));
                Online.UpdatePassword(this.olid, userInfo.Password);
                if (this.isconnectsetpassword && userConnectInfo.Uid == this.userid)
                {
                    userConnectInfo.IsSetPassword = true;
                    //DiscuzCloud.UpdateUserConnectInfo(userConnectInfo);
                    userConnectInfo.Save();
                }
                base.SetUrl("usercpnewpassword.aspx");
                base.SetMetaRefresh();
                base.SetShowBackLink(true);
                base.AddMsgLine("修改密码完毕, 同时已经更新了您的登录信息");
            }
        }
    }
}