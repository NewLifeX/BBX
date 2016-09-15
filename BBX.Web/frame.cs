using System.Web;
using BBX.Common;
using BBX.Config;
using BBX.Forum;

namespace BBX.Web
{
    public class frame : PageBase
    {
        protected override void ShowPage()
        {
            this.pagetitle = "分栏";
            int num = DNTRequest.GetInt("f", 1);
            if (num == 1)
            {
                ForumUtils.WriteCookie("isframe", "1");
            }
            else
            {
                num = Utils.StrToInt(ForumUtils.GetCookie("isframe"), -1);
                num = ((num == -1) ? this.config.Isframeshow : num);
            }
            if (num == 0)
            {
                HttpContext.Current.Response.Redirect(BaseConfigs.GetForumPath);
                HttpContext.Current.Response.End();
            }
        }
    }
}