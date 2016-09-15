using BBX.Common;
using BBX.Entity;
using BBX.Forum;
using BBX.Web.UI;

namespace BBX.Web
{
    public class usercpdraftbox : UserCpPage
    {
        protected override void ShowPage()
        {
            this.pagetitle = "短消息草稿箱";
            if (!base.IsLogin())
            {
                return;
            }
            if (DNTRequest.IsPost())
            {
                if (ShortMessage.DeletePrivateMessage(this.userid, DNTRequest.GetFormString("pmitemid")) <= 0)
                {
                    base.AddErrLine("参数无效<br />");
                    return;
                }
                base.SetMetaRefresh();
                base.SetShowBackLink(true);
                base.AddMsgLine("删除完毕");
            }
            else
            {
                base.BindPrivateMessage(2);
            }
            this.newnoticecount = Notice.GetNewNoticeCountByUid(this.userid);
        }
    }
}