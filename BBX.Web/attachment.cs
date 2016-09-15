using System.IO;
using System;
using System.Linq;
using NewLife;
using BBX.Common;
using BBX.Entity;
using BBX.Forum;
using NewLife.Web;

namespace BBX.Web
{
    public class attachment : PageBase
    {
        public Topic topic;
        public Attachment attachmentinfo;
        public int attachmentid = DNTRequest.GetInt("attachmentid", -1);
        public bool needlogin;
        public IXForum forum;
        private string msg = "";
        private bool ismoder;

        protected override void ShowPage()
        {
            this.pagetitle = "附件下载";
            if (this.attachmentid == -1)
            {
                base.AddErrLine("无效的附件ID");
                return;
            }
            if (this.useradminid != 1 && !this.usergroupinfo.DisablePeriodctrl)
            {
                string str = "";
                if (Scoresets.BetweenTime(this.config.Attachbanperiods, out str))
                {
                    base.AddErrLine("在此时间段( " + str + " )内用户不可以下载附件");
                    return;
                }
            }

            var att = Attachment.FindByID(attachmentid);
            this.attachmentinfo = att;
            if (att == null)
            {
                base.AddErrLine("不存在的附件ID");
                return;
            }
            if ((userid > 0 || userid == -1) && userid == att.Uid && att.Tid == 0 && att.FileType.StartsWith("image/"))
            {
                Response.Clear();
                if (att.IsLocal)
                    Response.TransmitFile(att.FullFileName);
                else
                    Response.Redirect(att.FileName);

                Response.End();
                return;
            }
            this.topic = Topic.FindByID(att.Tid);
            if (this.topic == null)
            {
                base.AddErrLine("不存在的主题ID");
                return;
            }
            var fi = XForum.FindByID(this.topic.Fid);
            this.pagetitle = Utils.RemoveHtml(fi.Name);
            if (!UserAuthority.VisitAuthority(fi, this.usergroupinfo, userid, ref this.msg))
            {
                base.AddErrLine(this.msg);
                if (userid == -1)
                {
                    this.needlogin = true;
                }
                return;
            }
            if (!UserAuthority.CheckUsertAttachAuthority(fi, this.usergroupinfo, userid, ref this.msg))
            {
                base.AddErrLine(this.msg);
                if (userid == -1)
                {
                    this.needlogin = true;
                }
                return;
            }
            this.ismoder = Moderators.IsModer(this.useradminid, userid, fi.ID);
            var q = WebHelper.RequestInt("q");
            if (att.ReadPerm > this.usergroupinfo.Readaccess && att.Uid != userid && !this.ismoder)
            {
                if (q != 1)
                {
                    Response.Clear();
                    Response.Redirect("/images/common/imgerror2.png");
                    Response.End();
                    return;
                }
                base.AddErrLine("您的阅读权限不够");
                if (userid == -1)
                {
                    this.needlogin = true;
                }
                return;
            }

            if (att.IsLocal && !File.Exists(att.FullFileName))
            {
                base.AddErrLine("该附件文件不存在或已被删除");
                return;
            }
            var key = "attachment_" + attachmentid;
            if ((!att.ImgPost || config.Showimages != 1) && userid != att.Uid && !ismoder && Utils.GetCookie(key).IsNullOrEmpty())
            {
                if (Scoresets.IsSetDownLoadAttachScore() && CreditsFacade.IsEnoughCreditsDownloadAttachment(userid, 1) && CreditsFacade.DowlnLoadAttachments(userid, 1) == -1)
                {
                    string msg = "";
                    if (EPayments.IsOpenEPayments()) msg = "<br/><span><a href=\"usercpcreditspay.aspx\">点击充值积分</a></span>";

                    base.AddErrLine("您的积分不足" + msg);
                    return;
                }
                Utils.WriteCookie(key, "true", 5);
            }
            if (AttachPaymentLog.HasBoughtAttach(userid, this.usergroupinfo.RadminID, att))
            {
                if (q != 1)
                {
                    Response.Clear();
                    Response.Redirect("/images/common/imgerror2.png");
                    Response.End();
                    return;
                }
                base.AddErrLine("该附件为交易附件, 请先行购买!");
                return;
            }

            if (!attachmentinfo.ImgPost)
            {
                attachmentinfo.Downloads++;
                attachmentinfo.Save();
            }
            if (!att.IsLocal)
            {
                try
                {
                    Response.Clear();
                    Response.Redirect(att.FileName);
                    Response.End();
                }
                catch { }
                return;
            }

            Utils.ResponseFile(att.FullFileName, Path.GetFileName(att.Name), att.FileType);
        }
    }
}