using BBX.Common;
using BBX.Entity;
using BBX.Forum;
using BBX.Web.UI;
using XCode;

namespace BBX.Web
{
    public class usercpignorelist : UserCpPage
    {
        public string ignoreexample = "{ALL}";

        protected override void ShowPage()
        {
            this.pagetitle = "黑名单";
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
                if (DNTRequest.GetFormString("Ignorepm") != this.user.Ignorepm)
                {
                    this.user.Ignorepm = Utils.CutString(DNTRequest.GetFormString("Ignorepm"), 0, 999);
                    //Users.UpdateUserPMSetting(this.user);
                    (user as IEntity).Save();
                }
                base.SetUrl("usercpignorelist.aspx");
                base.SetMetaRefresh();
                base.SetShowBackLink(true);
                base.AddMsgLine("操作完毕");
            }
            this.newnoticecount = Notice.GetNewNoticeCountByUid(this.userid);
        }
    }
}