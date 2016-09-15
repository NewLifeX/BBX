using System;
using System.Threading;
using System.Web;
using BBX.Common;
using BBX.Config;
using BBX.Entity;
using BBX.Forum;
using NewLife.Web;

namespace BBX.Web.Admin
{
    public class managerbody : AdminPage
    {
        public int olid;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
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
                //if (online.UserID <= 0 || userGroupInfo.RadminID != 1)
                if (online.UserID <= 0 || !online.Group.Is¹ÜÀíÔ±)
                {
                    this.Context.Response.Redirect(BaseConfigs.GetForumPath + "admin/syslogin.aspx");
                    return;
                }
                string secques = Users.GetUserInfo(online.UserID).Secques;
                if (this.Context.Request.Cookies["bbx_admin"] == null || this.Context.Request.Cookies["bbx_admin"]["key"] == null || ForumUtils.GetCookiePassword(this.Context.Request.Cookies["bbx_admin"]["key"].ToString(), this.config.Passwordkey) != online.Password + secques + online.UserID)
                {
                    this.Context.Response.Redirect(BaseConfigs.GetForumPath + "admin/syslogin.aspx");
                    return;
                }
                HttpCookie httpCookie = HttpContext.Current.Request.Cookies["bbx_admin"];
                httpCookie.Values["key"] = ForumUtils.SetCookiePassword(online.Password + secques + online.UserID, this.config.Passwordkey);
                httpCookie.Expires = DateTime.Now.AddMinutes(30.0);
                HttpContext.Current.Response.AppendCookie(httpCookie);
            }
        }
    }
}