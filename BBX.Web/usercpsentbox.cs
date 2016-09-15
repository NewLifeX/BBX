using BBX.Common;
using BBX.Entity;
using BBX.Forum;
using BBX.Web.UI;

namespace BBX.Web
{
    public class usercpsentbox : UserCpPage
    {
        protected override void ShowPage()
        {
            this.pagetitle = "短消息发件箱";
            if (!base.IsLogin())
            {
                return;
            }
            if (DNTRequest.IsPost())
            {
                if (ForumUtils.IsCrossSitePost())
                {
                    base.AddErrLine("您的请求来路不正确，无法提交。如果您安装了某种默认屏蔽来路信息的个人防火墙软件(如 Norton Internet Security)，请设置其不要禁止来路信息后再试。");
                    return;
                }
                if (ShortMessage.DeletePrivateMessage(this.userid, DNTRequest.GetFormString("pmitemid")) <= 0)
                {
                    base.AddErrLine("参数无效<br />");
                    return;
                }
                base.SetShowBackLink(false);
                base.AddMsgLine("删除完毕");
            }
            else
            {
                base.BindPrivateMessage(1);
            }
            this.newnoticecount = Notice.GetNewNoticeCountByUid(this.userid);
        }
    }
}