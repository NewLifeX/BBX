using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using Discuz.Common;

using Discuz.Config;
using Discuz.Entity;
using Discuz.Forum;

namespace Discuz.Web
{
    public class connect : PageBase
    {
        public string action = Utils.HtmlEncode(DNTRequest.GetString("action"));
        public string usedusernames = DNTRequest.GetString("con_x_usernames");
        public string birthday = Utils.HtmlEncode(DNTRequest.GetString("con_x_birthday"));
        public int gender = DNTRequest.GetInt("con_x_sex", 0);
        public string email = Utils.HtmlEncode(DNTRequest.GetString("con_x_email"));
        public string avatarurl = "";
        public string openid = Utils.HtmlEncode(DNTRequest.GetString("openid"));
        public bool allowreg;
        public int connectswitch;
        public bool isbindoverflow;
        public string postusername = Utils.HtmlEncode(DNTRequest.GetString("loginusername"));
        public string postpassword = Utils.HtmlEncode(DNTRequest.GetString("password"));
        public string notifyscript = "";
        public UserConnect userconnectinfo;
        public DiscuzCloudConfigInfo cloudconfig = DiscuzCloudConfigInfo.Current;

        protected override void OnInit(EventArgs e)
        {
            if (!DiscuzCloud.GetCloudServiceEnableStatus("connect"))
            {
                base.AddErrLine("QQ登录功能已关闭");
                return;
            }
            string a;
            if ((a = this.action) != null)
            {
                if (!(a == "access"))
                {
                    if (!(a == "bind"))
                    {
                        if (a == "unbind")
                        {
                            if (this.userid < 1)
                            {
                                base.AddErrLine("未登录用户无法进行该操作");
                                return;
                            }
                            this.userconnectinfo = DiscuzCloud.GetUserConnectInfo(this.userid);
                            if (this.userconnectinfo == null)
                            {
                                base.AddErrLine("您并没有绑定过QQ,不需要执行该操作");
                                return;
                            }
                            if (this.ispost)
                            {
                                if (!this.userconnectinfo.IsSetPassword)
                                {
                                    string @string = DNTRequest.GetString("newpasswd");
                                    if (string.IsNullOrEmpty(@string))
                                    {
                                        base.AddErrLine("您必须为帐号设置新密码才能解除绑定");
                                        return;
                                    }
                                    if (@string.Length < 6)
                                    {
                                        base.AddErrLine("密码不得少于6个字符");
                                        return;
                                    }
                                    if (@string != DNTRequest.GetString("confirmpasswd"))
                                    {
                                        base.AddErrLine("两次输入的新密码不一致");
                                        return;
                                    }
                                    var userInfo = Users.GetUserInfo(this.userid);
                                    userInfo.Password = @string;
                                    Users.ResetPassword(userInfo);
                                    Sync.UpdatePassword(userInfo.Name, userInfo.Password, "");
                                    if (!Utils.StrIsNullOrEmpty(DNTRequest.GetString("changesecques")))
                                    {
                                        Users.UpdateUserSecques(this.userid, DNTRequest.GetInt("question", 0), DNTRequest.GetString("answer"));
                                    }
                                    ForumUtils.WriteCookie("password", ForumUtils.SetCookiePassword(userInfo.Password, this.config.Passwordkey));
                                    OnlineUsers.UpdatePassword(this.olid, userInfo.Password);
                                }
                                DiscuzCloud.UnbindUserConnectInfo(this.userconnectinfo.OpenId);
                                ConnectbindLog userConnectBindLog = DiscuzCloud.GetUserConnectBindLog(this.userconnectinfo.OpenId);
                                if (userConnectBindLog != null)
                                {
                                    userConnectBindLog.Type = 2;
                                    DiscuzCloud.UpdateUserConnectBindLog(userConnectBindLog);
                                }
                                base.AddMsgLine("解绑成功");
                                string text = Utils.UrlDecode(ForumUtils.GetReUrl());
                                base.SetUrl((text.IndexOf("register.aspx") < 0) ? text : (this.forumpath + "index.aspx"));
                                base.SetMetaRefresh();
                                return;
                            }
                            return;
                        }
                    }
                    else
                    {
                        if (!this.ispost)
                        {
                            return;
                        }
                        if (DNTRequest.GetString("bind_type") == "new")
                        {
                            this.RegisterAndBind();
                            return;
                        }
                        if (this.userid < 0)
                        {
                            this.BindForumExistedUser();
                            return;
                        }
                        this.BindLoginedUser();
                        return;
                    }
                }
                else
                {
                    if (!this.CheckCallbackSignature(DNTRequest.GetString("con_sig")))
                    {
                        base.AddErrLine("非法请求");
                        return;
                    }
                    OAuthAccessTokenInfo connectAccessTokenInfo = DiscuzCloud.GetConnectAccessTokenInfo();
                    if (connectAccessTokenInfo == null)
                    {
                        base.AddErrLine("QQ登录过程中出现异常,请尝试再次登录");
                        return;
                    }
                    userconnectinfo = DiscuzCloud.GetUserConnectInfo(connectAccessTokenInfo.Openid);
                    if (userconnectinfo == null)
                    {
                        userconnectinfo = new UserConnect();
                        userconnectinfo.OpenId = connectAccessTokenInfo.Openid;
                        userconnectinfo.Token = connectAccessTokenInfo.Token;
                        userconnectinfo.Secret = connectAccessTokenInfo.Secret;
                        userconnectinfo.AllowVisitQQUserInfo = DNTRequest.GetInt("con_is_user_info", 0) != 0;
                        userconnectinfo.AllowPushFeed = DNTRequest.GetInt("con_is_feed", 0) != 0;
                        userconnectinfo.CallbackInfo = usedusernames + "&" + birthday + "&" + gender + "&" + email;
                        //DiscuzCloud.CreateUserConnectInfo(this.userconnectinfo);
                        userconnectinfo.Insert();
                    }
                    else
                    {
                        if (this.userconnectinfo.Uid > 0)
                        {
                            if (this.userid > 0)
                            {
                                base.SetBackLink("index.aspx");
                                base.AddErrLine((this.userconnectinfo.Uid != this.userid) ? "该QQ已经绑定了其他帐号" : "该QQ用户已登录");
                                return;
                            }
                            IUser shortUserInfo = Discuz.Entity.User.FindByID(this.userconnectinfo.Uid);
                            string url;
                            if (shortUserInfo == null)
                            {
                                DiscuzCloud.UnbindUserConnectInfo(this.userconnectinfo.OpenId);
                                url = HttpContext.Current.Request.RawUrl;
                            }
                            else
                            {
                                url = HttpContext.Current.Request.QueryString["url"];
                                if (string.IsNullOrEmpty(url))
                                    url = this.forumpath + "index.aspx";
                                if (connectAccessTokenInfo.Token != this.userconnectinfo.Token || connectAccessTokenInfo.Secret != this.userconnectinfo.Secret)
                                {
                                    this.userconnectinfo.Token = connectAccessTokenInfo.Token;
                                    this.userconnectinfo.Secret = connectAccessTokenInfo.Secret;
                                    DiscuzCloud.UpdateUserConnectInfo(this.userconnectinfo);
                                }
                                this.LoginUser(shortUserInfo);
                            }
                            HttpContext.Current.Response.Redirect(url);
                            HttpContext.Current.ApplicationInstance.CompleteRequest();
                        }
                        else
                        {
                            string[] array = this.userconnectinfo.CallbackInfo.Split('&');
                            if (array.Length == 4)
                            {
                                this.usedusernames = (string.IsNullOrEmpty(this.usedusernames) ? array[0] : this.usedusernames);
                                this.birthday = (string.IsNullOrEmpty(this.birthday) ? array[1] : this.birthday);
                                this.gender = ((this.gender == 0) ? Utils.StrToInt(array[2], 0) : this.gender);
                                this.email = (string.IsNullOrEmpty(this.email) ? array[3] : this.email);
                            }
                        }
                    }
                    ConnectbindLog userConnectBindLog2 = DiscuzCloud.GetUserConnectBindLog(this.userconnectinfo.OpenId);
                    this.isbindoverflow = (userConnectBindLog2 != null && this.cloudconfig.Maxuserbindcount > 0 && userConnectBindLog2.BindCount >= this.cloudconfig.Maxuserbindcount);
                    this.allowreg = (this.config.Regstatus != 0 && this.cloudconfig.Allowconnectregister == 1 && !this.isbindoverflow);
                    this.connectswitch = ((this.allowreg && this.userid < 0) ? 1 : 2);
                    byte[] bytes = Convert.FromBase64String(this.usedusernames);
                    this.usedusernames = Encoding.Default.GetString(bytes);
                    this.avatarurl = string.Format("http://avatar.connect.discuz.qq.com/{0}/{1}", DiscuzCloudConfigInfo.Current.Connectappid, this.userconnectinfo.OpenId);
                    this.openid = this.userconnectinfo.OpenId;
                    return;
                }
            }
            if (this.isbindconnect)
            {
                base.AddErrLine("用户已登录");
                return;
            }
            HttpContext.Current.Response.Redirect(DiscuzCloud.GetConnectLoginPageUrl(this.userid));
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        private void BindLoginedUser()
        {
            this.userconnectinfo = DiscuzCloud.GetUserConnectInfo(this.openid);
            if (this.userconnectinfo == null || this.userconnectinfo.Uid > 0)
            {
                base.AddErrLine("Connect信息异常,登录失败,请尝试再次登录");
                return;
            }
            if (DiscuzCloud.IsBindConnect(this.userid))
            {
                base.AddErrLine("该用户已经绑定了QQ,无法再次绑定");
                return;
            }
            this.userconnectinfo.Uid = this.userid;
            this.userconnectinfo.IsSetPassword = true;
            DiscuzCloud.UpdateUserConnectInfo(this.userconnectinfo);
            ConnectbindLog userConnectBindLog = DiscuzCloud.GetUserConnectBindLog(this.userconnectinfo.OpenId);
            if (userConnectBindLog == null)
            {
                DiscuzCloud.CreateUserConnectBindLog(new ConnectbindLog
                {
                    OpenID = this.userconnectinfo.OpenId,
                    Uid = this.userconnectinfo.Uid,
                    Type = 1,
                    BindCount = 1
                });
            }
            else
            {
                userConnectBindLog.Uid = this.userconnectinfo.Uid;
                userConnectBindLog.Type = 1;
                DiscuzCloud.UpdateUserConnectBindLog(userConnectBindLog);
            }
            base.SetUrl("index.aspx");
            base.SetMetaRefresh();
            base.SetShowBackLink(false);
            base.AddMsgLine("QQ绑定成功,继续浏览");
            Utils.WriteCookie("bindconnect", "1");
            IUser shortUserInfo = Discuz.Entity.User.FindByID(this.userid);
            this.notifyscript = this.GetNotifyScript(this.userconnectinfo, shortUserInfo.Name, shortUserInfo.Bday, shortUserInfo.Gender, shortUserInfo.Email, shortUserInfo.ShowEmail, DNTRequest.GetInt("useqqavatar", 2), "loginbind");
        }

        private void BindForumExistedUser()
        {
            if (LoginLogs.UpdateLoginLog(DNTRequest.GetIP(), false) >= 5)
            {
                base.AddErrLine("您已经多次输入密码错误, 请15分钟后再登录");
                return;
            }
            if (this.config.Emaillogin == 1 && Utils.IsValidEmail(this.postusername))
            {
                var list = Discuz.Entity.User.FindAllByEmail(postusername);
                if (list.Count == 0)
                {
                    base.AddErrLine("用户不存在");
                    return;
                }
                if (list.Count > 1)
                {
                    base.AddErrLine("您所使用Email不唯一，请使用用户名登陆");
                    return;
                }
                if (list.Count == 1)
                {
                    this.postusername = list[0].Name;
                }
            }
            if (this.config.Emaillogin == 0 && Users.GetUserId(this.postusername) == 0)
            {
                base.AddErrLine("用户不存在");
            }
            if (string.IsNullOrEmpty(this.postpassword))
            {
                base.AddErrLine("密码不能为空");
            }
            if (base.IsErr())
            {
                return;
            }
            IUser shortUserInfo = this.GetShortUserInfo();
            if (shortUserInfo == null)
            {
                int num = LoginLogs.UpdateLoginLog(DNTRequest.GetIP(), true);
                if (num > 5)
                {
                    base.AddErrLine("您已经输入密码5次错误, 请15分钟后再试");
                }
                else
                {
                    base.AddErrLine(string.Format("密码或安全提问第{0}次错误, 您最多有5次机会重试", num));
                }
                base.IsErr();
                return;
            }
            if ((shortUserInfo.GroupID == 4 || shortUserInfo.GroupID == 5) && shortUserInfo.GroupExpiry != 0 && shortUserInfo.GroupExpiry <= Utils.StrToInt(DateTime.Now.ToString("yyyyMMdd"), 0))
            {
                var creditsUserGroupId = CreditsFacade.GetCreditsUserGroupId((float)shortUserInfo.Credits);
                this.usergroupid = ((creditsUserGroupId.ID != 0) ? creditsUserGroupId.ID : this.usergroupid);
                shortUserInfo.GroupID = this.usergroupid;
                Users.UpdateUserGroup(shortUserInfo.ID, this.usergroupid);
            }
            if (shortUserInfo.GroupID == 5)
            {
                base.AddErrLine("该用户已经被禁止访问,无法绑定");
                return;
            }
            this.userconnectinfo = DiscuzCloud.GetUserConnectInfo(this.openid);
            if (this.userconnectinfo == null || this.userconnectinfo.Uid > 0)
            {
                base.AddErrLine("Connect信息异常,登录失败,请尝试再次登录");
                return;
            }
            if (DiscuzCloud.IsBindConnect(shortUserInfo.ID))
            {
                base.AddErrLine("该用户已经绑定了QQ,无法再次绑定");
                return;
            }
            this.userconnectinfo.Uid = shortUserInfo.ID;
            this.userconnectinfo.IsSetPassword = true;
            DiscuzCloud.UpdateUserConnectInfo(this.userconnectinfo);
            ConnectbindLog userConnectBindLog = DiscuzCloud.GetUserConnectBindLog(this.userconnectinfo.OpenId);
            if (userConnectBindLog == null)
            {
                DiscuzCloud.CreateUserConnectBindLog(new ConnectbindLog
                {
                    OpenID = this.userconnectinfo.OpenId,
                    Uid = this.userconnectinfo.Uid,
                    Type = 1,
                    BindCount = 1
                });
            }
            else
            {
                userConnectBindLog.Uid = this.userconnectinfo.Uid;
                userConnectBindLog.Type = 1;
                DiscuzCloud.UpdateUserConnectBindLog(userConnectBindLog);
            }
            if (shortUserInfo.GroupID != 8)
            {
                this.LoginUser(shortUserInfo);
                base.AddMsgLine("QQ登录成功,继续浏览");
            }
            else
            {
                base.AddMsgLine("帐号绑定成功,但需要管理员审核通过才能登录");
            }
            base.SetUrl("index.aspx");
            base.SetMetaRefresh();
            base.SetShowBackLink(false);
            this.notifyscript = this.GetNotifyScript(this.userconnectinfo, shortUserInfo.Name, shortUserInfo.Bday, shortUserInfo.Gender, shortUserInfo.Email, shortUserInfo.ShowEmail, DNTRequest.GetInt("useqqavatar", 2), "registerbind");
        }

        private void RegisterAndBind()
        {
            if (this.userid > 0)
            {
                base.AddErrLine("当前已有用户登录,无法注册");
                return;
            }
            if (this.config.Regstatus < 1 || this.cloudconfig.Allowconnectregister == 0)
            {
                base.AddErrLine("论坛当前禁止新的QQ会员登录");
                return;
            }
            string @string = DNTRequest.GetString(this.config.Antispamregisterusername);
            string text = DNTRequest.GetString(this.config.Antispamregisteremail).Trim().ToLower();
            string text2 = DNTRequest.GetString("bday").Trim();
            string errinfo = "";
            if (!Users.PageValidateUserName(@string, out errinfo) || !Users.PageValidateEmail(text, false, out errinfo))
            {
                base.AddErrLine(errinfo);
                return;
            }
            if (!Utils.IsDateString(text2) && !string.IsNullOrEmpty(text2))
            {
                base.AddErrLine("生日格式错误, 如果不想填写生日请置空");
                return;
            }
            if (Users.GetUserId(@string) > 0)
            {
                base.AddErrLine("请不要重复提交！");
                return;
            }
            this.userconnectinfo = DiscuzCloud.GetUserConnectInfo(this.openid);
            if (this.userconnectinfo == null || this.userconnectinfo.Uid > 0)
            {
                base.AddErrLine("Connect信息异常,登录失败,请尝试再次登录");
                return;
            }
            ConnectbindLog userConnectBindLog = DiscuzCloud.GetUserConnectBindLog(this.userconnectinfo.OpenId);
            if (this.cloudconfig.Maxuserbindcount != 0 && userConnectBindLog != null && userConnectBindLog.Type != 1 && userConnectBindLog.BindCount >= this.cloudconfig.Maxuserbindcount)
            {
                base.AddErrLine("当前QQ用户解绑次数过多,无法绑定新注册的用户");
                return;
            }
            User userInfo = this.CreateUser(@string, text, text2);
            this.userconnectinfo.Uid = userInfo.ID;
            DiscuzCloud.UpdateUserConnectInfo(this.userconnectinfo);
            if (userConnectBindLog == null)
            {
                DiscuzCloud.CreateUserConnectBindLog(new ConnectbindLog
                {
                    OpenID = this.userconnectinfo.OpenId,
                    Uid = this.userconnectinfo.Uid,
                    Type = 1,
                    BindCount = 1
                });
            }
            else
            {
                userConnectBindLog.BindCount++;
                userConnectBindLog.Uid = this.userconnectinfo.Uid;
                userConnectBindLog.Type = 1;
                DiscuzCloud.UpdateUserConnectBindLog(userConnectBindLog);
            }
            if (this.config.Welcomemsg == 1)
            {
                PrivateMessages.CreatePrivateMessage(new PrivateMessageInfo
                {
                    Message = this.config.Welcomemsgtxt,
                    Subject = "欢迎您的加入! (请勿回复本信息)",
                    Msgto = userInfo.Name,
                    Msgtoid = userInfo.ID,
                    Msgfrom = "系统",
                    Msgfromid = 0,
                    New = 1,
                    Postdatetime = Utils.GetDateTime(),
                    Folder = 0
                }, 0);
            }
            Sync.UserRegister(userInfo.ID, userInfo.Name, userInfo.Password, "");
            if (this.cloudconfig.Allowuseqzavater == 1 && DNTRequest.GetString("use_qzone_avatar") == "1")
            {
                QZoneAvatar qZoneAvatar = new QZoneAvatar();
                qZoneAvatar.AsyncGetAvatar(this.userconnectinfo);
            }
            base.SetUrl("index.aspx");
            base.SetShowBackLink(false);
            base.SetMetaRefresh((this.config.Regverify != 2) ? 2 : 5);
            Statistics.ReSetStatisticsCache();
            if (this.config.Regverify != 2)
            {
                CreditsFacade.UpdateUserCredits(userInfo.ID);
                ForumUtils.WriteUserCookie(userInfo, -1, this.config.Passwordkey);
                Utils.WriteCookie("bindconnect", "1");
                OnlineUsers.UpdateAction(this.olid, UserAction.Register.ActionID, 0, this.config.Onlinetimeout);
                base.AddMsgLine("QQ登录成功,继续浏览");
            }
            else
            {
                base.AddMsgLine("QQ数据绑定完成, 但需要系统管理员审核您的帐户后才可登录使用");
            }
            this.notifyscript = this.GetNotifyScript(this.userconnectinfo, userInfo.Name, userInfo.Bday, userInfo.Gender, userInfo.Email, userInfo.ShowEmail, DNTRequest.GetInt("useqqavatar", 2), "register");
        }

        private User CreateUser(string userName, string email, string birthday)
        {
            var userInfo = new User();
            userInfo.Name = userName;
            userInfo.Email = email;
            userInfo.Bday = birthday;
            userInfo.NickName = Utils.HtmlEncode(ForumUtils.BanWordFilter(DNTRequest.GetString("nickname")));
            userInfo.Password = Utils.MD5(ForumUtils.CreateAuthStr(16));
            userInfo.Secques = "";
            userInfo.Gender = DNTRequest.GetInt("gender", 0);
            userInfo.RegIP = userInfo.LastIP = DNTRequest.GetIP();
            userInfo.JoinDate = userInfo.LastVisit = userInfo.LastActivity = userInfo.LastPost = DateTime.Now;
            userInfo.ExtCredits1 = Scoresets.GetScoreSet(1).Init;
            userInfo.ExtCredits2 = Scoresets.GetScoreSet(2).Init;
            userInfo.ExtCredits3 = Scoresets.GetScoreSet(3).Init;
            userInfo.ExtCredits4 = Scoresets.GetScoreSet(4).Init;
            userInfo.ExtCredits5 = Scoresets.GetScoreSet(5).Init;
            userInfo.ExtCredits6 = Scoresets.GetScoreSet(6).Init;
            userInfo.ExtCredits7 = Scoresets.GetScoreSet(7).Init;
            userInfo.ExtCredits8 = Scoresets.GetScoreSet(8).Init;
            userInfo.Sigstatus = ((DNTRequest.GetInt("sigstatus", 1) != 0) ? 1 : 0);
            userInfo.Tpp = DNTRequest.GetInt("tpp", 0);
            userInfo.Ppp = DNTRequest.GetInt("ppp", 0);
            userInfo.TemplateID = DNTRequest.GetInt("templateid", 0);
            userInfo.Pmsound = DNTRequest.GetInt("pmsound", 0);
            userInfo.ShowEmail = DNTRequest.GetInt("showemail", 0);
            userInfo.Salt = "";
            int newsletter = (this.config.Regadvance == 0) ? 3 : DNTRequest.GetInt("receivesetting", 3);
            //userInfo.Newsletter = (ReceivePMSettingType)newsletter;
            userInfo.NewsLetter = newsletter;
            userInfo.Invisible = DNTRequest.GetInt("invisible", 0);
            userInfo.Newpm = ((this.config.Welcomemsg == 1) ? 1 : 0);
            userInfo.AccessMasks = DNTRequest.GetInt("accessmasks", 0);

            var uf = userInfo.Field;
            uf.Medals = "";
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
                BBCode = this.usergroupinfo.AllowSigbbCode ,
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
                userInfo.GroupID = 8;
                uf.Authflag = 1;
            }
            else
            {
                uf.Authstr = "";
                uf.Authflag = 0;
                userInfo.GroupID = CreditsFacade.GetCreditsUserGroupId(0f).ID;
            }
            //userInfo.ID = Users.CreateUser(userInfo);
            userInfo.Save();
            return userInfo;
        }

        private void LoginUser(IUser userInfo)
        {
            ForumUtils.WriteUserCookie(userInfo.ID, TypeConverter.StrToInt(DNTRequest.GetString("expires"), -1), this.config.Passwordkey, DNTRequest.GetInt("templateid", 0), DNTRequest.GetInt("loginmode", -1));
            this.oluserinfo = OnlineUsers.UpdateInfo(this.config.Passwordkey, this.config.Onlinetimeout, userInfo.ID, "");
            this.olid = this.oluserinfo.Olid;
            this.username = userInfo.Name;
            this.userid = userInfo.ID;
            this.usergroupinfo = UserGroup.FindByID(userInfo.GroupID);
            this.useradminid = this.usergroupinfo.RadminID;
            Utils.WriteCookie("bindconnect", "1");
            OnlineUsers.UpdateAction(this.olid, UserAction.Login.ActionID, 0);
            LoginLogs.DeleteLoginLog(DNTRequest.GetIP());
            Users.UpdateUserCreditsAndVisit(userInfo.ID, DNTRequest.GetIP());
        }

        private IUser GetShortUserInfo()
        {
            //int num;

            IUser user = null;
            //switch (this.config.Passwordmode)
            //{
            //    case 0:
            if (this.config.Secques == 1)
            {
                //num = Users.CheckPasswordAndSecques(this.postusername, this.postpassword, true, DNTRequest.GetFormInt("question", 0), DNTRequest.GetString("answer"));
                user = Discuz.Entity.User.Login(this.postusername, this.postpassword, true, DNTRequest.GetFormInt("question", 0), DNTRequest.GetString("answer"));
            }
            else
            {
                //num = Users.CheckPassword(this.postusername, this.postpassword, true);
                user = Discuz.Entity.User.Login(this.postusername, this.postpassword);
            }
            //        break;

            //    case 1:
            //        if (this.config.Secques == 1)
            //        {
            //            num = Users.CheckDvBbsPasswordAndSecques(this.postusername, this.postpassword, DNTRequest.GetFormInt("question", 0), DNTRequest.GetString("answer"));
            //        }
            //        else
            //        {
            //            num = Users.CheckDvBbsPassword(this.postusername, this.postpassword);
            //        }
            //        break;

            //    default:
            //        return Users.CheckThirdPartPassword(this.postusername, this.postpassword, DNTRequest.GetFormInt("question", 0), DNTRequest.GetString("answer"));
            //}
            if (user == null) return null;

            Users.UpdateTrendStat(TrendType.Login);

            return user;
        }

        private string GetNotifyScript(UserConnect connectInfo, string userName, string birthday, int gender, string email, int isPublicEmail, int isUsedQQAvatar, string type)
        {
            return string.Format("<script type=\"text/javascript\" src=\"{0}\" ></script>", DiscuzCloud.GetBindUserNotifyUrl(connectInfo, userName, birthday, gender, email, (isPublicEmail == 1) ? 1 : 2, isUsedQQAvatar, type));
        }

        private bool CheckCallbackSignature(string sig)
        {
            StringBuilder stringBuilder = new StringBuilder();
            List<DiscuzOAuthParameter> list = new List<DiscuzOAuthParameter>();
            string[] allKeys = HttpContext.Current.Request.QueryString.AllKeys;
            for (int i = 0; i < allKeys.Length; i++)
            {
                string text = allKeys[i];
                if ((text != "con_sig" && text != "url") && text.StartsWith("con_"))
                {
                    list.Add(new DiscuzOAuthParameter(text, DNTRequest.GetString(text)));
                }
            }
            list.Sort(new ParameterComparer());
            foreach (DiscuzOAuthParameter current in list)
            {
                if (!string.IsNullOrEmpty(current.Value))
                {
                    stringBuilder.AppendFormat("{0}={1}&", current.Name, current.Value);
                }
            }
            stringBuilder.Append(DiscuzCloudConfigInfo.Current.Connectappkey);
            return sig == Utils.MD5(stringBuilder.ToString());
        }
    }
}