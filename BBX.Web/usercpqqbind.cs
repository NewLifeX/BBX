using BBX.Common;
using BBX.Entity;
using BBX.Forum;
using BBX.Web.UI;

namespace BBX.Web
{
    public class usercpqqbind : UserCpPage
    {
        public bool ispublishfeed;
        public bool ispublisht;

        protected override void ShowPage()
        {
            this.pagetitle = "QQ绑定";
            if (!base.IsLogin())
            {
                return;
            }
            if (!this.isbindconnect)
            {
                base.AddErrLine("您未绑定QQ");
                return;
            }
            var connectToken = QzoneConnectToken.FindByUid(this.userid);
            //UserConnect userConnectInfo = DiscuzCloud.GetUserConnectInfo(this.userid);
            if (connectToken == null)
            {
                Utils.WriteCookie("bindconnect", "1");
                base.AddErrLine("您未绑定QQ");
                return;
            }
            if (this.ispost)
            {
                connectToken.PushToQzone = DNTRequest.GetInt("ispublishfeed", 0) != 0;
                connectToken.PushToWeibo = DNTRequest.GetInt("ispublisht", 0) != 0;
                //DiscuzCloud.UpdateUserConnectInfo(userConnectInfo);
                Utils.WriteCookie("cloud_feed_status", string.Format("{0}|{1}", this.userid, connectToken.PushToQzone));
                base.SetUrl("usercpqqbind.aspx");
                base.SetMetaRefresh();
                base.SetShowBackLink(true);
                base.AddMsgLine("绑定设置修改完毕");
                return;
            }
            this.ispublishfeed = connectToken.PushToQzone;
            this.ispublisht = connectToken.PushToWeibo;
            this.ispublisht = false;
        }
    }
}