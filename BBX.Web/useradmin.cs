using System;
using BBX.Common;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web
{
    public class useradmin : PageBase
    {
        public string operationtitle = "操作提示";
        public int operateduid = DNTRequest.GetInt("uid", -1);
        public string operatedusername;
        public int bantype;
        public string action = DNTRequest.GetQueryString("action");
        private AdminGroup admininfo;
        private IUser operateduser;
        public bool titlemessage;
        public string groupexpiry = "";

        protected override void ShowPage()
        {
            this.pagetitle = "用户管理";
            if (this.userid == -1)
            {
                base.AddErrLine("请先登录");
                return;
            }
            if (ForumUtils.IsCrossSitePost() || this.action.IsNullOrEmpty())
            {
                base.AddErrLine("非法提交");
                return;
            }
            if (String.IsNullOrEmpty(this.action))
            {
                base.AddErrLine("操作类型参数为空");
                return;
            }
            this.admininfo = AdminGroup.FindByID(this.usergroupid);
            if (this.admininfo == null)
            {
                base.AddErrLine("你没有管理权限");
                return;
            }
            if (this.operateduid == -1)
            {
                base.AddErrLine("没有选择要操作的用户");
                return;
            }
            this.operateduser = BBX.Entity.User.FindByID(this.operateduid);
            if (this.operateduser == null)
            {
                base.AddErrLine("选择的用户不存在");
                return;
            }
            if (this.operateduser.AdminID > 0)
            {
                base.AddErrLine("无法对拥有管理权限的用户进行操作, 请管理员登录后台进行操作");
                return;
            }
            this.operatedusername = this.operateduser.Name;
            if (!this.ispost)
            {
                Utils.WriteCookie("reurl", DNTRequest.GetUrlReferrer());
                if (this.action == "banuser")
                {
                    this.operationtitle = "禁止用户";
                    switch (this.operateduser.GroupID)
                    {
                        case 4:
                            this.bantype = 1;
                            this.groupexpiry = "(" + Utils.FormatDate(this.operateduser.GroupExpiry) + ")";
                            break;
                        case 5:
                            this.bantype = 2;
                            this.groupexpiry = "(" + Utils.FormatDate(this.operateduser.GroupExpiry) + ")";
                            break;
                        case 6:
                            this.bantype = 3;
                            this.groupexpiry = "(" + Utils.FormatDate(this.operateduser.GroupExpiry) + ")";
                            break;
                        default:
                            this.bantype = 0;
                            break;
                    }
                    if (!admininfo.AllowBanUser)
                    {
                        base.AddErrLine("您没有禁止用户的权限");
                        return;
                    }
                }
            }
            else
            {
                if (this.action == "banuser")
                {
                    this.operationtitle = "禁止用户";
                    this.DoBanUserOperation();
                }
            }
        }

        private void DoBanUserOperation()
        {
            this.ispost = false;
            if (this.usergroupinfo.ReasonPm == 1 && Utils.StrIsNullOrEmpty(DNTRequest.GetString("reason")))
            {
                this.titlemessage = true;
                base.AddErrLine("请填写操作原因");
                return;
            }
            int formInt = DNTRequest.GetFormInt("banexpirynew", -1);
            string text = (formInt == 0) ? "29990101" : string.Format("{0:yyyyMMdd}", DateTime.Now.AddDays((double)formInt));
            string title;
            string actions;
            switch (DNTRequest.GetInt("bantype", -1))
            {
                case 0:
                    Users.UpdateBanUser(CreditsFacade.GetCreditsUserGroupId((float)this.operateduser.Credits).ID, "0", this.operateduid);
                    title = string.Format("取消对 <a href=\"../../userinfo-{1}.aspx\" target=\"_blank\">{0}</a> 的禁止", this.operatedusername, this.operateduid);
                    actions = "取消禁止";
                    break;
                case 1:
                    Users.UpdateBanUser(4, text, this.operateduid);
                    title = string.Format("禁止 <a href=\"../../userinfo-{1}.aspx\" target=\"_blank\">{0}</a> 发言", this.operatedusername, this.operateduid);
                    actions = "禁止发言";
                    break;
                case 2:
                    Users.UpdateBanUser(5, text, this.operateduid);
                    title = string.Format("禁止 <a href=\"../../userinfo-{1}.aspx\" target=\"_blank\">{0}</a> 访问", this.operatedusername, this.operateduid);
                    actions = "禁止访问";
                    break;
                default:
                    this.titlemessage = true;
                    base.AddErrLine("错误的禁止类型");
                    return;
            }
            ModeratorManageLog.Add(userid, username, usergroupid, usergroupinfo.GroupTitle, 0, "", 0, title, actions, DNTRequest.GetString("reason").Trim());
            this.ispost = true;
            base.SetShowBackLink(false);
            base.SetUrl(Utils.UrlDecode(ForumUtils.GetReUrl()));
            base.SetMetaRefresh();
            base.MsgForward("useradmin_succeed", true);
        }
    }
}