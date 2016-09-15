using System.IO;
using BBX.Common;
using BBX.Forum;
using BBX.Entity;

namespace BBX.Web
{
    public class showtemplate : PageBase
    {
        protected override void ShowPage()
        {
            this.pagetitle = "选择模板";
            if (this.userid == -1 && this.config.Guestcachepagetimeout > 0)
            {
                base.AddErrLine("当前的系统设置不允许游客选择模板");
                return;
            }
            int tid = DNTRequest.GetInt("templateid", 0);
            var tmp = Template.FindByID(tid);
            if (tmp != null)
            {
                if (!Directory.Exists(Utils.GetMapPath("../" + tmp.Name)))
                {
                    base.AddErrLine("您所选择的模板不存在！");
                    return;
                }
                //if (!Utils.InArray(num.ToString(), Templates.GetValidTemplateIDList()))
                if (!Template.Has(tid))
                {
                    tid = this.config.Templateid;
                }
                Utils.WriteCookie(Utils.GetTemplateCookieName(), tid.ToString(), 999999);
                string text = string.Format("http://{0}{1}", DNTRequest.GetCurrentFullHost(), this.forumpath);
                if (text != "")
                {
                    base.SetUrl(Utils.InArray(text, "logout.aspx,showtemplate.aspx") ? "index.aspx" : text);
                }
                else
                {
                    base.SetUrl("index.aspx");
                }
                base.MsgForward("showtemplate_succeed", true);
                base.AddMsgLine("切换模板成功, 返回切换模板前页面");
                base.SetMetaRefresh();
                base.SetShowBackLink(false);
                return;
            }
            else
            {
                var url = Request.UrlReferrer + "";
                if (string.IsNullOrEmpty(url) || url.IndexOf("showtemplate") > -1)
                    url = "index.aspx";

                ForumUtils.WriteCookie("reurl", url);
            }
        }
    }
}