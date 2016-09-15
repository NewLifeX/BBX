using System;
using BBX.Common;
using BBX.Entity;
using BBX.Forum;
using BBX.Web.UI;

namespace BBX.Web
{
    public class usercpinbox : UserCpPage
    {
        protected override void ShowPage()
        {
            this.pagetitle = "短消息收件箱";
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
                if (Utils.StrIsNullOrEmpty(DNTRequest.GetFormString("pmitemid")))
                {
                    base.AddErrLine("您未选中任何短消息，当前操作失败！");
                    return;
                }
                if (!Utils.IsNumericList(DNTRequest.GetFormString("pmitemid")))
                {
                    base.AddErrLine("参数信息错误！");
                    return;
                }
                var ids = DNTRequest.GetFormString("pmitemid");
                if (!String.IsNullOrEmpty(ids) || ShortMessage.DeletePrivateMessage(this.userid, ids) <= 0)
                {
                    base.AddErrLine("参数无效");
                    return;
                }
                Users.UpdateUserNewPMCount(this.userid, this.olid);
                base.SetUrl("usercpinbox.aspx");
                base.SetMetaRefresh();
                base.SetShowBackLink(true);
                base.AddMsgLine("删除完毕");
            }
            else
            {
                base.BindPrivateMessage(0);
            }
            this.newnoticecount = Notice.GetNewNoticeCountByUid(this.userid);
        }
    }
}