using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using BBX.Config;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web.Admin
{
    public class logout : Page
    {
        protected internal GeneralConfigInfo config;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.config = GeneralConfigInfo.Current;
            var online = Online.UpdateInfo();
            //if (UserGroup.FindByID((int)online.GroupID).RadminID != 1)
            if (!online.Group.Isπ‹¿Ì‘±)
            {
                HttpContext.Current.Response.Redirect("../");
                return;
            }
            int olid = online.ID;
            //OnlineUsers.DeleteRows(olid);
            var entity = Online.FindByID(olid);
            if (entity != null) entity.Delete();
            ForumUtils.ClearUserCookie();
            HttpCookie cookie = new HttpCookie("bbx_admin");
            HttpContext.Current.Response.AppendCookie(cookie);
            FormsAuthentication.SignOut();
        }
    }
}