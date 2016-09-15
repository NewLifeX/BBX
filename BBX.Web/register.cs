using System;
using System.Text.RegularExpressions;
using BBX.Common;
using BBX.Config;
using BBX.Entity;
using BBX.Forum;
using NewLife.Web;
using XUser = BBX.Entity.User;

namespace BBX.Web
{
    public class register : PageBase
    {
        public string action = Utils.HtmlEncode(DNTRequest.GetString("action"));
        public int createuser = DNTRequest.GetInt("createuser", 0);
        public string agree = (GeneralConfigInfo.Current.Rules == 0) ? "true" : Utils.HtmlEncode(DNTRequest.GetFormString("agree"));
        public string invitecode = Utils.HtmlEncode(DNTRequest.GetString("invitecode"));
        public bool allowinvite;
        public string verifycode = Utils.HtmlEncode(DNTRequest.GetString("verifycode"));
        public VerifyReg verifyinfo;
        public string errorControlId = "";

        protected override void ShowPage()
        {
            this.pagetitle = "用户注册";
            if (this.userid > 0)
            {
                base.SetUrl(BaseConfigs.GetForumPath);
                base.SetMetaRefresh();
                base.SetShowBackLink(false);
                base.AddMsgLine("不能重复注册用户");
                this.ispost = true;
                return;
            }
            if (this.config.Regstatus < 1)
            {
                base.AddErrLine("论坛当前禁止新用户注册");
                return;
            }
            if (string.IsNullOrEmpty(this.action))
            {
                this.action = ((this.config.Rules == 1 && this.infloat == 0) ? "rules" : ((this.config.Regverify == 1) ? "verify" : "reg"));
            }
            else
            {
                if (!Utils.InArray(this.action, "rules,verify,reg"))
                {
                    base.AddErrLine("参数错误");
                    return;
                }
                if (this.action == "rules" && (this.config.Rules == 0 || this.infloat == 1))
                {
                    this.action = ((this.config.Regverify == 1) ? "verify" : "reg");
                }
            }
            string text = Users.CheckRegisterDateDiff(WebHelper.UserHost);
            if (text != null)
            {
                base.AddErrLine(text);
                return;
            }
            if (this.action == "verify" && this.config.Regverify == 1 && this.config.Regctrl > 0)
            {
                //var vi = Users.GetVerifyRegisterInfoByIp(WebHelper.UserHost);
                var vi = VerifyReg.FindByIP(WebHelper.UserHost);
                if (vi != null)
                {
                    var ts = vi.CreateTime.AddHours(config.Regctrl) - DateTime.Now;
                    //int num = Utils.StrDateDiffHours(vi.CreateTime, this.config.Regctrl);
                    var num = ts.TotalHours;
                    if (num == 0)
                    {
                        base.AddErrLine("抱歉, 系统设置了IP注册间隔限制, 您必须在 " + ts.TotalMinutes + " 分钟后才可以提交请求");
                        return;
                    }
                    if (num < 0)
                    {
                        base.AddErrLine("抱歉, 系统设置了IP注册间隔限制, 您必须在 " + num * -1 + " 小时后才可以提交请求");
                        return;
                    }
                }
            }
            if (this.action == "reg" && this.config.Regverify == 1)
            {
                //this.verifyinfo = Users.GetVerifyRegisterInfo(this.verifycode);
                this.verifyinfo = VerifyReg.FindByVerifyCode(this.verifycode);
                if (verifyinfo == null || (verifyinfo.CreateTime != verifyinfo.ExpireTime && verifyinfo.ExpireTime < DateTime.Now))
                {
                    base.AddErrLine("该注册链接不存在或已过期,请点击注册重新获取链接");
                    return;
                }
                this.invitecode = this.verifyinfo.InviteCode;
            }
            this.allowinvite = Utils.InArray(this.config.Regstatus.ToString(), "2,3");
            string a;
            if (this.ispost && (a = this.action) != null)
            {
                if (a == "rules")
                {
                    this.action = (string.IsNullOrEmpty(this.agree) ? this.action : ((this.config.Regverify == 1) ? "verify" : "reg"));
                    this.ispost = false;
                    return;
                }
                if (a == "verify")
                {
                    this.SendRegisterVerifyLink();
                    return;
                }
                if (!(a == "reg"))
                {
                    return;
                }
                if (this.createuser == 1)
                {
                    this.Register();
                }
            }
        }

        public void Register()
        {
            base.SetShowBackLink(true);
            var inviteCodeInfo = this.allowinvite ? this.ValidateInviteInfo() : null;
            if (base.IsErr())
            {
                return;
            }
            string username = DNTRequest.GetString(this.config.Antispamregisterusername);
            string email = (this.config.Regverify == 1) ? this.verifyinfo.Email : DNTRequest.GetString(this.config.Antispamregisteremail).Trim().ToLower();
            string bday = DNTRequest.GetString("bday").Trim();
            if (String.IsNullOrEmpty(bday))
            {
                bday = string.Format("{0}-{1}-{2}", DNTRequest.GetString("bday_y").Trim(), DNTRequest.GetString("bday_m").Trim(), DNTRequest.GetString("bday_d").Trim());
            }
            if (bday == "--") bday = "";
            this.ValidateUserInfo(username, email, bday);
            if (base.IsErr()) return;

            if (Users.GetUserId(username) > 0)
            {
                base.AddErrLine("请不要重复提交！");
                return;
            }
            var userInfo = this.CreateUser(username, email, bday);
            if (this.config.Regverify == 1)
            {
                //Users.DeleteVerifyRegisterInfo(this.verifyinfo.RegId);
                //var vi = VerifyReg.FindByID(verifyinfo.ID);
                verifyinfo.Delete();
            }
            if (inviteCodeInfo != null)
            {
                //Invitation.UpdateInviteCodeSuccessCount(inviteCodeInfo.InviteId);
                inviteCodeInfo.SuccessCount++;
                if (this.config.Regstatus == 3 && inviteCodeInfo.SuccessCount + 1 >= inviteCodeInfo.MaxCount)
                {
                    //Invitation.DeleteInviteCode(inviteCodeInfo.InviteId);
                    inviteCodeInfo.IsDeleted = true;
                }
                inviteCodeInfo.Save();
            }
            if (this.config.Welcomemsg == 1)
            {
                var msg = new ShortMessage
                {
                    Message = this.config.Welcomemsgtxt,
                    Subject = "欢迎您的加入! (请勿回复本信息)",
                    Msgto = userInfo.Name,
                    MsgtoID = userInfo.ID,
                    Msgfrom = "系统",
                    MsgfromID = 0,
                    Folder = 0
                };
                msg.Create();
            }
            Sync.UserRegister(userInfo.ID, userInfo.Name, userInfo.Password, "");
            base.SetUrl("index.aspx");
            base.SetShowBackLink(false);
            base.SetMetaRefresh((this.config.Regverify != 2) ? 2 : 5);
            //Statistic.ReSetStatisticsCache();
            //Statistic.UpdateStatisticsLastUserName(userInfo.ID, userInfo.Name);
            Statistic.Reset();
            if (this.config.Regverify != 2)
            {
                CreditsFacade.UpdateUserCredits(userInfo.ID);
                ForumUtils.WriteUserCookie(userInfo, -1, this.config.Passwordkey);
                Online.UpdateAction(this.olid, UserAction.Register, 0, this.config.Onlinetimeout);
                base.MsgForward("register_succeed");
                base.AddMsgLine("注册成功, 返回登录页");
            }
            else
            {
                base.AddMsgLine("注册成功, 但需要系统管理员审核您的帐户后才可登录使用");
            }
            this.agree = "yes";
        }

        public void SendRegisterVerifyLink()
        {
            string email = DNTRequest.GetString(this.config.Antispamregisteremail).Trim().ToLower();
            this.ValidateEmail(email);
            if (base.IsErr())
            {
                return;
            }
            if (this.allowinvite)
            {
                this.ValidateInviteInfo();
            }
            if (base.IsErr())
            {
                return;
            }
            var vi = Users.CreateVerifyRegisterInfo(email, this.allowinvite ? this.invitecode : string.Empty);
            if (vi != null)
            {
                string arg = string.Format("{0}register.aspx?action=reg&verifycode={1}", Utils.GetRootUrl(this.forumpath), vi.VerifyCode);
                string body = string.Format(this.config.Verifyregisteremailtemp, vi.Email.Split('@')[0], arg);
                //EmailMultiThread em = new EmailMultiThread(verifyRegisterInfo.Email.Split('@')[0], verifyRegisterInfo.Email, string.Format("{0} 的安全注册链接,欢迎注册!", this.config.Forumtitle), body);
                //new Thread(new ThreadStart(em.Send)).Start();
                Emails.SendAsync(vi.Email.Split('@')[0], vi.Email, string.Format("{0} 的安全注册链接,欢迎注册!", this.config.Forumtitle), body);
            }
            base.SetUrl("index.aspx");
            base.SetShowBackLink(false);
            base.SetMetaRefresh(2);
            base.AddMsgLine("请求已经发送,请查收邮箱");
        }

        private void ValidateUserInfo(string userName, string email, string bday)
        {
            this.errorControlId = "username";
            string errinfo = "";
            if (!Users.PageValidateUserName(userName, out errinfo))
            {
                base.AddErrLine(errinfo);
                return;
            }
            this.errorControlId = "password";
            if (String.IsNullOrEmpty(DNTRequest.GetString("password")))
            {
                base.AddErrLine("密码不能为空");
                return;
            }
            if (!DNTRequest.GetString("password").Equals(DNTRequest.GetString("password2")))
            {
                base.AddErrLine("两次密码输入必须相同");
                return;
            }
            if (DNTRequest.GetString("password").Length < 6)
            {
                base.AddErrLine("密码不得少于6个字符");
                return;
            }
            this.ValidateEmail(email);
            if (base.IsErr())
            {
                return;
            }
            string text = DNTRequest.GetString("realname").Trim();
            string text2 = DNTRequest.GetString("idcard").Trim();
            string text3 = DNTRequest.GetString("mobile").Trim();
            string text4 = DNTRequest.GetString("phone").Trim();
            if (!string.IsNullOrEmpty(text2) && !Regex.IsMatch(text2, "^[\\x20-\\x80]+$"))
            {
                base.AddErrLine("身份证号码中含有非法字符");
                return;
            }
            if (!string.IsNullOrEmpty(text3) && !Regex.IsMatch(text3, "^[\\d|-]+$"))
            {
                base.AddErrLine("移动电话号码中含有非法字符");
                return;
            }
            if (!string.IsNullOrEmpty(text4) && !Regex.IsMatch(text4, "^[\\d|-]+$"))
            {
                base.AddErrLine("固定电话号码中含有非法字符");
                return;
            }
            if (this.config.Realnamesystem == 1)
            {
                if (string.IsNullOrEmpty(text) || Utils.GetStringLength(text) > 10)
                {
                    base.AddErrLine("真实姓名不能为空且不能大于10个字符");
                    return;
                }
                if (string.IsNullOrEmpty(text2) || text2.Length > 20)
                {
                    base.AddErrLine("身份证号码不能为空且不能大于20个字符");
                    return;
                }
                if (string.IsNullOrEmpty(text3) && string.IsNullOrEmpty(text4))
                {
                    base.AddErrLine("移动电话号码或固定电话号码必须填写其中一项");
                    return;
                }
                if (text3.Length > 20)
                {
                    base.AddErrLine("移动电话号码不能大于20个字符");
                    return;
                }
                if (text4.Length > 20)
                {
                    base.AddErrLine("固定电话号码不能大于20个字符");
                    return;
                }
            }
            if (!Utils.IsDateString(bday) && !string.IsNullOrEmpty(bday))
            {
                base.AddErrLine("生日格式错误, 如果不想填写生日请置空");
                return;
            }
            if (Utils.GetStringLength(DNTRequest.GetString("bio").Trim()) > 500)
            {
                base.AddErrLine("自我介绍不得超过500个字符");
                return;
            }
            if (Utils.GetStringLength(DNTRequest.GetString("signature").Trim()) > 500)
            {
                base.AddErrLine("签名不得超过500个字符");
            }
        }

        private Invitation ValidateInviteInfo()
        {
            this.errorControlId = "invitecode";
            if (this.config.Regstatus == 3 && string.IsNullOrEmpty(this.invitecode))
            {
                base.AddErrLine("邀请码不能为空！");
                return null;
            }
            if (string.IsNullOrEmpty(this.invitecode)) return null;

            var inviteCodeInfo = Invitation.FindByCode(this.invitecode.ToUpper());
            if (inviteCodeInfo == null || !inviteCodeInfo.Check())
            {
                base.AddErrLine("邀请码不合法或已过期！");
                return null;
            }
            return inviteCodeInfo;
        }

        private void ValidateEmail(string email)
        {
            this.errorControlId = "email";
            string errinfo = "";
            if (!Users.PageValidateEmail(email, this.action == "verify", out errinfo))
            {
                base.AddErrLine(errinfo);
            }
        }

        private User CreateUser(string tmpUsername, string email, string tmpBday)
        {
            var user = new User();
            user.Name = tmpUsername;
            user.NickName = Utils.HtmlEncode(ForumUtils.BanWordFilter(DNTRequest.GetString("nickname")));
            user.Password = DNTRequest.GetString("password");
            user.Secques = ForumUtils.GetUserSecques(DNTRequest.GetInt("question", 0), DNTRequest.GetString("answer"));
            user.Gender = DNTRequest.GetInt("gender", 0);
            user.AdminID = 0;
            user.GroupExpiry = 0;
            user.ExtGroupIds = "";
            user.RegIP = WebHelper.UserHost;
            user.JoinDate = DateTime.Now;
            user.LastIP = WebHelper.UserHost;
            user.LastVisit = DateTime.Now;
            user.LastActivity = DateTime.Now;
            user.LastPost = DateTime.Now;
            user.ExtCredits1 = Scoresets.GetScoreSet(1).Init;
            user.ExtCredits2 = Scoresets.GetScoreSet(2).Init;
            user.ExtCredits3 = Scoresets.GetScoreSet(3).Init;
            user.ExtCredits4 = Scoresets.GetScoreSet(4).Init;
            user.ExtCredits5 = Scoresets.GetScoreSet(5).Init;
            user.ExtCredits6 = Scoresets.GetScoreSet(6).Init;
            user.ExtCredits7 = Scoresets.GetScoreSet(7).Init;
            user.ExtCredits8 = Scoresets.GetScoreSet(8).Init;
            user.Email = email + "";
            user.Bday = tmpBday + "";
            user.Sigstatus = ((DNTRequest.GetInt("sigstatus", 1) != 0) ? 1 : 0);
            user.Tpp = DNTRequest.GetInt("tpp", 0);
            user.Ppp = DNTRequest.GetInt("ppp", 0);
            user.TemplateID = DNTRequest.GetInt("templateid", 0);
            user.Pmsound = DNTRequest.GetInt("pmsound", 0);
            user.ShowEmail = DNTRequest.GetInt("showemail", 0) != 0;
            user.Salt = "";
            int newsletter = (this.config.Regadvance == 0) ? 3 : DNTRequest.GetInt("receivesetting", 3);
            //userInfo.NewsLetter = (ReceivePMSettingType)newsletter;
            user.NewsLetter = newsletter;
            user.Invisible = DNTRequest.GetInt("invisible", 0) != 0;
            user.Newpm = config.Welcomemsg != 0;
            user.AccessMasks = DNTRequest.GetInt("accessmasks", 0);

            var uf = user.Field;
            uf.Website = DNTRequest.GetHtmlEncodeString("website");
            uf.Icq = DNTRequest.GetHtmlEncodeString("icq");
            uf.qq = DNTRequest.GetHtmlEncodeString("qq");
            uf.Yahoo = DNTRequest.GetHtmlEncodeString("yahoo");
            uf.Msn = DNTRequest.GetHtmlEncodeString("msn");
            uf.Skype = DNTRequest.GetHtmlEncodeString("skype");
            uf.Location = DNTRequest.GetHtmlEncodeString("location");
            uf.Customstatus = ((this.usergroupinfo.AllowCstatus) ? DNTRequest.GetHtmlEncodeString("customstatus") : "");
            uf.Bio = ForumUtils.BanWordFilter(DNTRequest.GetString("bio"));
            uf.Signature = Utils.HtmlEncode(ForumUtils.BanWordFilter(DNTRequest.GetString("signature")));
            uf.Sightml = UBB.UBBToHTML(new PostpramsInfo
            {
                Usergroupid = this.usergroupid,
                Attachimgpost = this.config.Attachimgpost,
                Showattachmentpath = this.config.Showattachmentpath,
                Hide = 0,
                Price = 0,
                Sdetail = uf.Signature,
                Smileyoff = 1,
                BBCode = this.usergroupinfo.AllowSigbbCode,
                Parseurloff = 1,
                Showimages = this.usergroupinfo.AllowSigimgCode ? 1 : 0,
                Allowhtml = 0,
                Smiliesinfo = Smilies.GetSmiliesListWithInfo(),
                Customeditorbuttoninfo = Editors.GetCustomEditButtonListWithInfo(),
                Smiliesmax = this.config.Smiliesmax
            });
            uf.AuthTime = DateTime.Now;
            uf.RealName = DNTRequest.GetString("realname");
            uf.Idcard = DNTRequest.GetString("idcard");
            uf.Mobile = DNTRequest.GetString("mobile");
            uf.Phone = DNTRequest.GetString("phone");
            if (this.config.Regverify == 2)
            {
                uf.Authstr = DNTRequest.GetString("website");
                user.GroupID = 8;
                uf.Authflag = 1;
            }
            else
            {
                uf.Authstr = "";
                uf.Authflag = 0;
                user.GroupID = CreditsFacade.GetCreditsUserGroupId(0f).ID;
            }
            //if (this.config.Passwordmode > 1 && PasswordModeProvider.GetInstance() != null)
            //{
            //    userInfo.Uid = PasswordModeProvider.GetInstance().CreateUserInfo(userInfo);
            //}
            //else
            {
                user.Password = Utils.MD5(user.Password);
                //userInfo.Uid = Users.CreateUser(userInfo);
            }
            user.Save();
            return user;
        }

        private void SendEmail(string username, string password, string emailaddress, string authstr)
        {
            Emails.SendRegMail(username, emailaddress, password, authstr);
        }
    }
}