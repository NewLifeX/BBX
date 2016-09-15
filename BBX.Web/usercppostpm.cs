using System;
using System.Text;
using BBX.Common;
using BBX.Entity;
using BBX.Forum;
using BBX.Web.UI;

namespace BBX.Web
{
    public class usercppostpm : UserCpPage
    {
        public string msgto = DNTRequest.GetHtmlEncodeString("msgto");
        public string subject = DNTRequest.GetHtmlEncodeString("subject");
        public string message = DNTRequest.GetHtmlEncodeString("message");
        public int msgtoid = DNTRequest.GetInt("msgtoid", 0);
        private ShortMessage pm;

        protected override void ShowPage()
        {
            this.pagetitle = "撰写短消息";
            if (!base.IsLogin())
            {
                return;
            }
            if (!this.CheckPermission())
            {
                return;
            }
            if (DNTRequest.IsPost() && !ForumUtils.IsCrossSitePost())
            {
                if (!this.CheckPermissionAfterPost())
                {
                    return;
                }
                this.SendPM();
                if (base.IsErr())
                {
                    return;
                }
            }
            var shortUserInfo = BBX.Entity.User.FindByID(this.msgtoid);
            string text = (shortUserInfo != null) ? shortUserInfo.Name : "";
            this.msgto = ((this.msgtoid > 0) ? text : this.msgto);
            string text2 = DNTRequest.GetQueryString("action").ToLower();
            if ((text2.CompareTo("re") == 0 || text2.CompareTo("fw") == 0) && DNTRequest.GetQueryInt("pmid", -1) != -1)
            {
                var msg = ShortMessage.FindByID(DNTRequest.GetQueryInt("pmid", -1));
                if (msg != null && (msg.MsgtoID == userid || msg.MsgfromID == userid))
                {
                    this.msgto = ((text2.CompareTo("re") == 0) ? Utils.HtmlEncode(msg.Msgfrom) : "");
                    this.subject = Utils.HtmlEncode(text2) + ":" + msg.Subject;
                    this.message = Utils.HtmlEncode("> ") + msg.Message.Replace("\n", "\n> ") + "\r\n\r\n";
                }
            }
            if (DNTRequest.GetString("operation") == "pmfriend")
            {
                this.CreatePmFriendMessage();
            }
            this.newnoticecount = Notice.GetNewNoticeCountByUid(this.userid);
        }

        private bool CheckPermissionAfterPost()
        {
            if (ForumUtils.IsCrossSitePost())
            {
                base.AddErrLine("您的请求来路不正确，无法提交。如果您安装了某种默认屏蔽来路信息的个人防火墙软件(如 Norton Internet Security)，请设置其不要禁止来路信息后再试。");
                return false;
            }
            if (Utils.StrIsNullOrEmpty(DNTRequest.GetString("message")) || DNTRequest.GetString("message").Length > 3000)
            {
                base.AddErrLine("内容不能为空,且不能超过3000字");
                return false;
            }
            if (Utils.StrIsNullOrEmpty(DNTRequest.GetString("msgto")))
            {
                base.AddErrLine("接收人不能为空");
                return false;
            }
            if (Utils.StrIsNullOrEmpty(DNTRequest.GetString("subject")) || DNTRequest.GetString("subject").Trim().Length > 60)
            {
                base.AddErrLine("标题不能为空,且不能超过60字");
                return false;
            }
            if (DNTRequest.GetString("msgto") == "系统")
            {
                base.AddErrLine("不能给系统发送消息");
                return false;
            }
            this.msgtoid = Users.GetUserId(DNTRequest.GetString("msgto"));
            if (this.msgtoid <= 0)
            {
                base.AddErrLine("接收人不是注册用户");
                return false;
            }
            return true;
        }

        private bool CheckPermission()
        {
            var adminGroupInfo = AdminGroup.FindByID(this.usergroupid);
            if (adminGroupInfo == null || !adminGroupInfo.DisablePostctrl)
            {
                var ts = DateTime.Now - this.lastpostpmtime.AddSeconds(config.Postinterval * 2);
                if (ts.TotalSeconds < 0)
                {
                    base.AddErrLine(string.Format("系统规定发帖或发短消息间隔为{0}秒, 您还需要等待 {1} 秒", this.config.Postinterval * 2, ts.TotalSeconds * -1));
                    return false;
                }
            }
            if (!CreditsFacade.IsEnoughCreditsPM(this.userid))
            {
                base.AddErrLine("您的积分不足, 不能发送短消息");
                return false;
            }
            return true;
        }

        public void SendNotifyEmail(string email, ShortMessage pm)
        {
            string text = string.Format("http://{0}/usercpshowpm.aspx?pmid={1}", DNTRequest.GetCurrentFullHost(), pm.ID);
            var sb = new StringBuilder("# 论坛短消息: <a href=\"" + text + "\" target=\"_blank\">" + pm.Subject + "</a>");
            sb.AppendFormat("\r\n\r\n{0}\r\n<hr/>", pm.Message);
            sb.AppendFormat("作 者:{0}\r\n", pm.Msgfrom);
            sb.AppendFormat("Email:<a href=\"mailto:{0}\" target=\"_blank\">{0}</a>\r\n", BBX.Entity.User.FindByID(this.userid).Email.Trim());
            sb.AppendFormat("URL:<a href=\"{0}\" target=\"_blank\">{0}</a>\r\n", text);
            sb.AppendFormat("时 间:{0}", pm.PostDateTime);
            Emails.SendEmailNotify(email, "[" + this.config.Forumtitle + "短消息通知]" + pm.Subject, sb.ToString());
        }

        public void SendPM()
        {
            if (pm == null) pm = new ShortMessage();

            if (this.useradminid == 1)
            {
                this.pm.Message = DNTRequest.GetHtmlEncodeString("message");
                this.pm.Subject = DNTRequest.GetHtmlEncodeString("subject");
            }
            else
            {
                this.pm.Message = Utils.HtmlEncode(ForumUtils.BanWordFilter(DNTRequest.GetString("message")));
                this.pm.Subject = Utils.HtmlEncode(ForumUtils.BanWordFilter(DNTRequest.GetString("subject")));
            }
            if (this.useradminid != 1 && (ForumUtils.HasBannedWord(this.pm.Message) || ForumUtils.HasBannedWord(this.pm.Subject) || ForumUtils.HasAuditWord(this.pm.Message) || ForumUtils.HasAuditWord(this.pm.Subject)))
            {
                string arg = (ForumUtils.GetBannedWord(this.pm.Message) == string.Empty) ? ForumUtils.GetBannedWord(this.pm.Subject) : ForumUtils.GetBannedWord(this.pm.Message);
                base.AddErrLine(string.Format("对不起, 您提交的内容包含不良信息 <font color=\"red\">{0}</font>, 因此无法提交, 请返回修改!", arg));
                return;
            }
            string text = "," + Users.GetUserInfo(this.msgtoid).Ignorepm + ",";
            if (text.IndexOf("{ALL}") >= 0 || text.IndexOf("," + this.username + ",") >= 0)
            {
                base.AddErrLine("短消息发送失败!");
                return;
            }
            this.pm.Message = ForumUtils.BanWordFilter(this.pm.Message);
            this.pm.Subject = ForumUtils.BanWordFilter(this.pm.Subject);
            this.pm.Msgto = DNTRequest.GetString("msgto");
            this.pm.MsgtoID = this.msgtoid;
            this.pm.Msgfrom = this.username;
            this.pm.MsgfromID = this.userid;
            if (!Utils.StrIsNullOrEmpty(DNTRequest.GetString("savetousercpdraftbox")))
            {
                this.CreatePM(2, 0, "usercpdraftbox.aspx", "已将消息保存到草稿箱");
                return;
            }
            if (!Utils.StrIsNullOrEmpty(DNTRequest.GetString("savetosentbox")))
            {
                this.CreatePM(0, 1, "usercpsentbox.aspx", "发送完毕, 且已将消息保存到发件箱");
            }
            else
            {
                this.CreatePM(0, 0, "usercpinbox.aspx", "发送完毕");
            }
            if (!base.IsErr())
            {
                Online.UpdatePostPMTime(this.olid);
                //int olidByUid = OnlineUsers.GetOlidByUid(this.pm.MsgtoID);
                //if (olidByUid > 0)
                var entity = Online.FindByUserID(pm.MsgtoID);
                if (entity != null)
                {
                    Users.UpdateUserNewPMCount(this.pm.MsgtoID, entity.ID);
                }
            }
        }

        private void CreatePM(int folder, int saveToSendBox, string url, string msg)
        {
            if (folder != 2)
            {
                User userInfo = Users.GetUserInfo(this.msgtoid);
                var ug = UserGroup.FindByID(this.usergroupid);
                if (!ug.Is管理团队 && ShortMessage.GetPrivateMessageCount(this.msgtoid, -1) >= UserGroup.FindByID(userInfo.GroupID).MaxPmNum)
                {
                    base.AddErrLine("抱歉,接收人的短消息已达到上限,无法接收");
                    return;
                }
                if (!Utils.InArray(userInfo.NewsLetter.ToInt().ToString(), "2,3,6,7"))
                {
                    base.AddErrLine("抱歉,接收人拒绝接收短消息");
                    return;
                }
            }
            if (url != "usercpinbox.aspx" && ShortMessage.GetPrivateMessageCount(this.userid, -1) >= this.usergroupinfo.MaxPmNum)
            {
                base.AddErrLine("抱歉,您的短消息已达到上限,无法保存到发件箱");
                return;
            }
            this.pm.Folder = (Int16)folder;
            if (CreditsFacade.SendPM(this.userid) == -1)
            {
                base.AddErrLine("您的积分不足, 不能发送短消息");
                return;
            }
            //this.pm.Pmid = PrivateMessages.CreatePrivateMessage(this.pm, saveToSendBox);
            pm.Create(saveToSendBox != 0);
            if (DNTRequest.GetString("emailnotify") == "on")
            {
                this.SendNotifyEmail(Users.GetUserInfo(this.msgtoid).Email.Trim(), this.pm);
            }
            base.SetUrl(url);
            base.SetMetaRefresh();
            base.SetShowBackLink(true);
            base.MsgForward("usercppostpm_succeed");
            base.AddMsgLine(msg);
        }

        private void CreatePmFriendMessage()
        {
            int tid = DNTRequest.GetInt("tid", 0);
            if (tid == 0) return;

            var tp = Topic.FindByID(tid);
            this.message = string.Format("你好！我在 {0} 看到了这篇帖子，认为很有价值，特推荐给你。\r\n\r\n{1}\r\n地址 {2}\r\n\r\n希望你能喜欢。", this.config.Forumtitle, tp.Title, DNTRequest.GetUrlReferrer());
        }
    }
}