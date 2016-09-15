using System;
using BBX.Common;
using BBX.Config;
using BBX.Forum;

namespace BBX.Web.UI
{
    public class StatusPage : PageBase
    {
        public StatusPage()
        {
            base.Load += new EventHandler(this.Status_Load);
        }

        private void Status_Load(object sender, EventArgs e)
        {
            if (!APIConfigInfo.Current.Enable)
            {
                return;
            }
            ApplicationInfo applicationInfo = null;
            foreach (ApplicationInfo current in APIConfigInfo.Current.AppCollection)
            {
                if (current.APIKey == DNTRequest.GetString("api_key"))
                {
                    applicationInfo = current;
                }
            }
            if (applicationInfo == null)
            {
                return;
            }
            if (DNTRequest.GetString("format").Trim().ToLower() == "json")
            {
                base.Response.ContentType = "text/html";
                base.Response.Write((this.userid > 0).ToString().ToLower());
                base.Response.End();
                return;
            }
            base.Response.Redirect(string.Format("{0}{1}user_status={2}{3}", new object[]
			{
				applicationInfo.CallbackUrl,
				(applicationInfo.CallbackUrl.IndexOf("?") > 0) ? "&" : "?",
				(this.userid > 0) ? "1" : "0",
				(String.IsNullOrEmpty(DNTRequest.GetString("next"))) ? DNTRequest.GetString("next") : ("&next=" + DNTRequest.GetString("next"))
			}));
        }
    }
}