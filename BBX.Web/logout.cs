using System;
using System.Web;
using BBX.Common;
using BBX.Config;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web
{
    public class logout : PageBase
    {
        protected override void ShowPage()
        {
            int userid = this.userid;
            this.pagetitle = "用户退出";
            this.username = "游客";
            this.userid = -1;
            base.AddScript("if (top.document.getElementById('leftmenu')){ top.frames['leftmenu'].location.reload(); }");
            var reurl = Request["reurl"];
            if (!DNTRequest.IsPost() || String.IsNullOrEmpty(reurl))
            {
                var url = reurl;
                if (String.IsNullOrEmpty(reurl))
                {
                    url = Request.UrlReferrer + "";
                    if (string.IsNullOrEmpty(url) || url.IndexOf("login") > -1 || url.IndexOf("logout") > -1) url = "index.aspx";
                }
                Utils.WriteCookie("reurl", (String.IsNullOrEmpty(reurl) || reurl.IndexOf("login.aspx") > -1) ? url : reurl);
            }
            if (DNTRequest.GetString("userkey") == this.userkey || this.IsApplicationLogout())
            {
                base.AddMsgLine("已经清除了您的登录信息, 稍后您将以游客身份返回首页");

                var entity = Online.FindByID(olid);
                if (entity != null) entity.Delete();

                ForumUtils.ClearUserCookie();
                Utils.WriteCookie(Utils.GetTemplateCookieName(), "", -999999);
                Response.AppendCookie(new HttpCookie("bbx_admin"));
                if (APIConfigInfo.Current.Enable)
                {
                    base.AddMsgLine(Sync.GetLogoutScript(userid));
                }
                if (!APIConfigInfo.Current.Enable || !Sync.NeedAsyncLogout())
                {
                    base.MsgForward("logout_succeed");
                }
            }
            else
            {
                base.AddMsgLine("无法确定您的身份, 稍后返回首页");
            }
            base.SetUrl(Utils.UrlDecode(ForumUtils.GetReUrl()));
            base.SetMetaRefresh();
            base.SetShowBackLink(false);
        }

        private bool IsApplicationLogout()
        {
            return APIConfigInfo.Current.Enable && DNTRequest.GetFormInt("confirm", -1) == 1;
        }
    }
}