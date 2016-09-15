using System;
using System.Collections.Generic;
using BBX.Common;
using BBX.Config;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web
{
    public class invite : PageBase
    {
        public string inviteurl = "";
        private IUser userinfo;
        public Invitation invitecodeinfo;
        public string invitecode = DNTRequest.GetQueryString("Code");
        public string action = DNTRequest.GetQueryString("action");
        public string userscore = "";
        public string invitecodeprice = "";
        public List<Invitation> invitecodelist = new List<Invitation>();
        public int invitecodecount;
        public int pageindex = DNTRequest.GetQueryInt("page", 1);
        public string pagenumber = "";
        public int pagecount;
        public string emailpreview = "";
        public string usersaid = Utils.HtmlEncode(Utils.RemoveHtml(DNTRequest.GetString("usersaid")));
        public InvitationConfigInfo invitationconfiginfo = InvitationConfigInfo.Current;
        public bool isuseusersaid;
        public string[] extcreditnames = Scoresets.GetValidScoreName();
        public string[] extcreditunits = Scoresets.GetValidScoreUnit();
        public string avatarSmall = "";
        public string avatarMedium = "";
        public string avatarLarge = "";
        public int datediff;

        protected override void ShowPage()
        {
            this.pagetitle = "邀请注册";
            if (!Utils.InArray(this.config.Regstatus.ToString(), "2,3"))
            {
                base.AddErrLine("当前站点没有开启邀请功能！");
                return;
            }
            if (this.userid > 0)
            {
                if (this.action == "floatwinemail")
                {
                    return;
                }
                this.avatarSmall = Avatars.GetAvatarUrl(this.userid, AvatarSize.Small);
                this.avatarMedium = Avatars.GetAvatarUrl(this.userid, AvatarSize.Medium);
                this.avatarLarge = Avatars.GetAvatarUrl(this.userid, AvatarSize.Large);
                this.userinfo = Users.GetUserInfo(this.userid);
                if (this.config.Regstatus == 2)
                {
                    this.invitecodeinfo = Invitation.GetInviteCodeByUid(this.userid);
                    if (this.invitecodeinfo != null)
                    {
                        this.inviteurl = this.GetUserInviteUrl(this.invitecodeinfo.Code, false);
                        this.userscore = this.GetUserInviteScore(this.invitecodeinfo.SuccessCount);
                        this.usersaid = string.Format("邀请附言:<div id=\"usersaidinemail\">{0}</div>", this.usersaid);
                        if (!this.ispost)
                        {
                            this.CreateEmailPreview();
                        }
                    }
                }
                else
                {
                    this.invitecodecount = Invitation.GetUserInviteCodeCount(this.userid);
                    this.invitecodelist = Invitation.GetUserInviteCodeList(this.userid, this.pageindex);
                    this.invitecodeprice = this.GetInviteCodePrice();
                    this.pagecount = (this.invitecodecount - 1) / 10 + 1;
                    this.pagenumber = Utils.GetPageNumbers(this.pageindex, this.pagecount, "invite.aspx", 8);
                }
                if (this.ispost)
                {
                    string a;
                    if ((a = this.action) == null)
                    {
                        return;
                    }
                    if (!(a == "createcode"))
                    {
                        if (!(a == "convertcode"))
                        {
                            if (!(a == "buycode"))
                            {
                                if (!(a == "floatwinemailsend"))
                                {
                                    return;
                                }
                                this.SendEmail();
                            }
                            else
                            {
                                this.BuyInviteCode();
                            }
                        }
                        else
                        {
                            this.ConvertInviteCode();
                        }
                    }
                    else
                    {
                        this.CreateInviteCode();
                    }
                }
            }
            if (userid == -1 && !String.IsNullOrEmpty(invitecode))
            {
                //this.invitecodeinfo = Invitation.GetInviteCodeByCode(this.Code);
                invitecodeinfo = Invitation.FindByCode(invitecode);
            }
        }

        public void CreateInviteCode()
        {
            if (this.config.Regstatus == 2 && this.invitationconfiginfo.InviteCodeUserCreatePerDay > 0 && Invitation.GetTodayUserCreatedInviteCode(this.userid) >= this.invitationconfiginfo.InviteCodeUserCreatePerDay)
            {
                base.AddErrLine("您今天申请邀请码的数量过多，请明天再试!");
                return;
            }
            if (this.invitecodeinfo == null)
            {
                Invitation.CreateInviteCode(this.userinfo);
                base.SetUrl("invite.aspx");
                base.SetMetaRefresh();
                base.SetShowBackLink(false);
                base.AddMsgLine("创建邀请码成功");
            }
        }

        public void ConvertInviteCode()
        {
            if (this.config.Regstatus == 2 && this.invitecodeinfo != null)
            {
                Invitation.ConvertInviteCodeToCredits(this.invitecodeinfo, this.invitationconfiginfo.InviteCodePayCount);
                //Invitation.DeleteInviteCode(this.invitecodeinfo.InviteId);
                invitecodeinfo.Delete();
                string strinfo = (this.invitecodeinfo.SuccessCount - this.invitationconfiginfo.InviteCodePayCount > -1) ? "兑换成功" : "删除成功";
                base.SetUrl("invite.aspx");
                base.SetMetaRefresh();
                base.SetShowBackLink(false);
                base.AddMsgLine(strinfo);
            }
        }

        public void SendEmail()
        {
            List<string> list = new List<string>(Utils.SplitString(DNTRequest.GetString("email"), ","));
            if (String.IsNullOrEmpty(invitecode))
            {
                base.AddErrLine("丢失参数导致邮件发送失败，请检查本地杀毒软件设置");
                return;
            }
            //this.invitecodeinfo = Invitation.GetInviteCodeByCode(this.Code);
            invitecodeinfo = Invitation.FindByCode(invitecode);
            int num = 0;
            foreach (string current in list)
            {
                if (!string.IsNullOrEmpty(current) && Utils.IsValidEmail(current))
                {
                    if (Emails.SendEmailNotify(current, "来自您的好友:" + this.invitecodeinfo.Creator + "的邀请!", string.Format(this.invitationconfiginfo.InvitationEmailTemplate, new object[]
                    {
                        current,
                        this.userid,
                        this.invitecodeinfo.Creator,
                        this.GetUserInviteUrl(this.invitecodeinfo.Code, true),
                        this.config.Forumtitle,
                        !String.IsNullOrEmpty(this.usersaid) ? this.usersaid : "",
                        this.rooturl,
                        this.avatarSmall,
                        this.avatarMedium,
                        this.avatarLarge
                    })))
                    {
                        num++;
                    }
                    if (num > 19)
                    {
                        break;
                    }
                }
            }
            if (num > 0)
            {
                base.AddMsgLine("成功发送" + num.ToString() + "封Email");
                return;
            }
            base.AddErrLine("发送失败，请检查Email地址是否正确");
        }

        public void BuyInviteCode()
        {
            if (this.invitecodecount >= this.invitationconfiginfo.InviteCodeMaxCountToBuy)
            {
                base.AddErrLine("您所拥有的邀请码数量超过了系统上限，无法再购买");
                return;
            }
            string[] array = Utils.SplitString(this.invitationconfiginfo.InviteCodePrice, ",");
            float[] array2 = new float[8];
            for (int i = 0; i < 8; i++)
            {
                array2[i] = (Single)array[i].ToDouble() * -1f;
            }
            if (CreditsFacade.UpdateUserExtCredits(this.userid, array2, false) > 0)
            {
                this.CreateInviteCode();
                return;
            }
            string str = "";
            if (EPayments.IsOpenEPayments())
            {
                str = "<br/><span><a href=\"usercpcreditspay.aspx\">点击充值积分</a></span>";
            }
            base.AddErrLine("积分不足，无法购买邀请码" + str);
        }

        public string GetUserInviteUrl(string code, bool isCreateLink)
        {
            if (isCreateLink)
            {
                return string.Format("<a href=\"{0}\" target=\"_blank\">{0}</a>", this.rooturl + "invite.aspx?Code=" + code);
            }
            return this.rooturl + "invite.aspx?Code=" + code;
        }

        public string GetUserInviteScore(int mount)
        {
            float num = (float)(mount - this.invitationconfiginfo.InviteCodePayCount);
            if (num > 0f)
            {
                float[] userExtCredits = Scoresets.GetUserExtCredits(CreditsOperationType.Invite);
                string text = "";
                for (int i = 0; i < userExtCredits.Length; i++)
                {
                    if ((double)userExtCredits[i] != 0.0)
                    {
                        text += string.Format("{0}:{1}{2} ;", this.extcreditnames[i + 1], userExtCredits[i] * (float)mount, this.extcreditunits[i + 1]);
                    }
                }
                return text;
            }
            return "该邀请码尚未达到兑换条件!";
        }

        public string GetInviteCodePrice()
        {
            string text = "";
            string[] array = Utils.SplitString(this.invitationconfiginfo.InviteCodePrice, ",");
            for (int i = 0; i < 8; i++)
            {
                if (array[i].ToDouble() != 0f)
                {
                    text += string.Format("{0}:{1}{2} ,", this.extcreditnames[i + 1], array[i], this.extcreditunits[i + 1]);
                }
            }
            if (!(text != ""))
            {
                return "暂无定价;";
            }
            return text;
        }

        public void CreateEmailPreview()
        {
            this.isuseusersaid = (this.invitationconfiginfo.InvitationEmailTemplate.IndexOf("{5}") > 0);
            this.emailpreview = string.Format(this.invitationconfiginfo.InvitationEmailTemplate, new object[]
            {
                "[friend]",
                this.userid,
                this.invitecodeinfo.Creator,
                this.GetUserInviteUrl(this.invitecodeinfo.Code, true),
                this.config.Forumtitle,
                this.usersaid,
                this.rooturl,
                this.avatarSmall,
                this.avatarMedium,
                this.avatarLarge
            });
        }

        public string InviteCodeExpireTip(DateTime time)
        {
            this.datediff = (Int32)(time - DateTime.Now).TotalHours;
            if (this.datediff < 0)
            {
                string str;
                switch (this.datediff / 24)
                {
                    case 0:
                        str = "明天过期";
                        break;

                    case 1:
                        str = "后天过期";
                        break;

                    default:
                        str = (this.datediff / 24 + 1).ToString() + "天后过期";
                        break;
                }
                return "您的邀请链接会在" + str;
            }
            return "<font color=red><b>您的邀请链接已过期</b></font>";
        }

        public string CreateUserExtCreditsStr()
        {
            string text = "";
            string format = " {0}:{1}{2} ;";
            if (this.extcreditnames[1] != string.Empty)
            {
                text += string.Format(format, this.extcreditnames[1], this.userinfo.ExtCredits1, this.extcreditunits[1]);
            }
            if (this.extcreditnames[2] != string.Empty)
            {
                text += string.Format(format, this.extcreditnames[2], this.userinfo.ExtCredits2, this.extcreditunits[2]);
            }
            if (this.extcreditnames[3] != string.Empty)
            {
                text += string.Format(format, this.extcreditnames[3], this.userinfo.ExtCredits3, this.extcreditunits[3]);
            }
            if (this.extcreditnames[4] != string.Empty)
            {
                text += string.Format(format, this.extcreditnames[4], this.userinfo.ExtCredits4, this.extcreditunits[4]);
            }
            if (this.extcreditnames[5] != string.Empty)
            {
                text += string.Format(format, this.extcreditnames[5], this.userinfo.ExtCredits5, this.extcreditunits[5]);
            }
            if (this.extcreditnames[6] != string.Empty)
            {
                text += string.Format(format, this.extcreditnames[6], this.userinfo.ExtCredits6, this.extcreditunits[6]);
            }
            if (this.extcreditnames[7] != string.Empty)
            {
                text += string.Format(format, this.extcreditnames[7], this.userinfo.ExtCredits7, this.extcreditunits[7]);
            }
            if (this.extcreditnames[8] != string.Empty)
            {
                text += string.Format(format, this.extcreditnames[8], this.userinfo.ExtCredits8, this.extcreditunits[8]);
            }
            return text.TrimEnd(';');
        }
    }
}