using System;
using System.Web;
using System.Web.UI;
using BBX.Common;
using BBX.Config;
using BBX.Entity;
using BBX.Forum;
using NewLife.Web;

namespace BBX.Web.Admin
{
    public class UserControlsPageBase : UserControl
    {
        protected internal GeneralConfigInfo config;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            //}
            //public UserControlsPageBase()
            //{
            this.config = GeneralConfigInfo.Current;
            if (!this.config.Adminipaccess.IsNullOrEmpty())
            {
                string[] iparray = Utils.SplitString(this.config.Adminipaccess, "\n");
                if (!Utils.InIPArray(WebHelper.UserHost, iparray))
                {
                    this.Context.Response.Redirect(BaseConfigs.GetForumPath + "admin/syslogin.aspx");
                    return;
                }
            }
            var online = Online.UpdateInfo();
            //var userGroupInfo = UserGroup.FindByID((int)online.GroupID);
            if (online.UserID <= 0 || !online.Group.Is¹ÜÀíÔ±)
            {
                this.Context.Response.Redirect(BaseConfigs.GetForumPath + "admin/syslogin.aspx");
                return;
            }
            string secques = Users.GetUserInfo(online.UserID).Secques;
            if (this.Context.Request.Cookies["bbx_admin"] == null || this.Context.Request.Cookies["bbx_admin"]["key"] == null || ForumUtils.GetCookiePassword(this.Context.Request.Cookies["bbx_admin"]["key"].ToString(), this.config.Passwordkey) != online.Password + secques + online.UserID.ToString())
            {
                this.Context.Response.Redirect(BaseConfigs.GetForumPath + "admin/syslogin.aspx");
                return;
            }
            HttpCookie httpCookie = HttpContext.Current.Request.Cookies["bbx_admin"];
            httpCookie.Values["key"] = ForumUtils.SetCookiePassword(online.Password + secques + online.UserID.ToString(), this.config.Passwordkey);
            httpCookie.Values["userid"] = online.UserID.ToString();
            httpCookie.Expires = DateTime.Now.AddMinutes(30.0);
            HttpContext.Current.Response.AppendCookie(httpCookie);
        }
    }
}