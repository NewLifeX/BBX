using System;
using BBX.Common;
using BBX.Entity;
using BBX.Forum;
using BBX.Web.UI;

namespace BBX.Web
{
    public class usercpshowpm : UserCpPage
    {
        public string msgto = "";
        public string msgfrom = "";
        public string subject = "";
        public string message = "";
        public string resubject = "";
        public string remessage = "";
        public string postdatetime = "";
        public int pmid = DNTRequest.GetQueryInt("pmid", -1);
        public bool canreplypm = true;

        protected override void ShowPage()
        {
            if (!base.IsLogin())
            {
                return;
            }
            this.pagetitle = "查看短消息";
            if (this.pmid <= 0)
            {
                base.AddErrLine("参数无效");
                return;
            }
            if (!CreditsFacade.IsEnoughCreditsPM(this.userid))
            {
                this.canreplypm = false;
            }
            var msg = ShortMessage.FindByID(this.pmid);
            if (msg == null)
            {
                base.AddErrLine("无效的短消息ID");
                return;
            }
            if (msg.Msgfrom == "系统" && msg.MsgfromID == 0)
            {
                msg.Message = Utils.HtmlDecode(msg.Message);
            }
            if (msg == null || (msg.MsgtoID != this.userid && msg.MsgfromID != this.userid))
            {
                base.AddErrLine("对不起, 短消息不存在或已被删除.");
                this.newnoticecount = Notice.GetNewNoticeCountByUid(this.userid);
                return;
            }
            if (DNTRequest.GetQueryString("action").CompareTo("delete") != 0)
            {
                if (DNTRequest.GetQueryString("action").CompareTo("noread") == 0)
                {
                    //PrivateMessages.SetPrivateMessageState(this.pmid, 1);
                    msg.New = true;
                    msg.Update();
                    this.ispost = true;
                    if (!msg.New && msg.Folder == 0)
                    {
                        Users.UpdateUserNewPMCount(this.userid, this.olid);
                        base.AddMsgLine("指定消息已被置成未读状态,现在将转入消息列表");
                        base.SetUrl("usercpinbox.aspx");
                        base.SetMetaRefresh();
                    }
                }
                else
                {
                    //PrivateMessages.SetPrivateMessageState(this.pmid, 0);
                    msg.New = false;
                    msg.Update();

                    if (msg.New && msg.Folder == 0)                                
                    {
                        Users.UpdateUserNewPMCount(this.userid, this.olid);
                    }
                }
                this.msgto = ((msg.Folder == 0) ? msg.Msgfrom : msg.Msgto);
                this.msgfrom = msg.Msgfrom;
                this.subject = msg.Subject;
                this.message = UBB.ParseUrl(Utils.StrFormat(msg.Message));
                this.postdatetime = msg.PostDateTime.ToFullString();
                this.resubject = "re:" + msg.Subject;
                this.remessage = Utils.HtmlEncode("> ") + msg.Message.Replace("\n", "\n> ") + "\r\n\r\n";
                return;
            }
            this.ispost = true;
            msg.Delete();
            //if (ShortMessage.DeletePrivateMessage(this.userid, pmid + "") < 1)
            //{
            //    base.AddErrLine("消息未找到,可能已被删除");
            //    return;
            //}
            base.AddMsgLine("指定消息成功删除,现在将转入消息列表");
            base.SetUrl("usercpinbox.aspx");
            base.SetMetaRefresh();
        }
    }
}