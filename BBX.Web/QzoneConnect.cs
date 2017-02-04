using System;
using BBX.Common;
using BBX.Config;
using BBX.Entity;
using BBX.Forum;
using NewLife.Log;
using NewLife.Web;
using XCode;

namespace BBX.Web
{
    public class QzoneConnect : PageBase
    {
        public string action = Utils.HtmlEncode(DNTRequest.GetString("act")).ToLower();
        public string qzoneconnectpagename;
        public string qqnikename = string.Empty;
        public int qqgender = -1;
        public string avatarurl = "";
        protected String nowdate = DateTime.Now.ToString("yyyy-MM-dd");
        protected String nowdatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        public string postusername = Utils.HtmlEncode(DNTRequest.GetString("loginusername"));
        public string postpassword = Utils.HtmlEncode(DNTRequest.GetString("password"));

        public string email = Utils.HtmlEncode(DNTRequest.GetString("con_x_email"));
        public bool allowreg;
        public bool isbindoverflow;
        //public DiscuzCloudConfigInfo cloudconfig = DiscuzCloudConfigInfo.Current;
        public QzoneConnectConfigInfo qzconfig = QzoneConnectConfigInfo.Current;

        protected override void ShowPage()
        {
            base.ShowPage();

            if (!qzconfig.EnableConnect)
            {
                AddErrLine("QQ登录功能已关闭");
                return;
            }

            qzoneconnectpagename = qzconfig.QzoneConnectPage;

            var action = Request.QueryString["act"];
            if (action != null) action = action.ToLower();

            if (string.IsNullOrEmpty(action))
                Authorize();
            else if (action == "access")
                Access();
            else if (action == "bind")
                Bind();
            else if (action == "unbind")
                Unbind();
        }

        private void Authorize()
        {
            var referrer = Request.UrlReferrer;
            var path = string.Empty;
            if (referrer != null)
            {
                path = Request.UrlReferrer.PathAndQuery.ToLower();
                if (!path.StartsWith("/logout.aspx")
                    && !path.StartsWith("/login.aspx")
                    && !path.StartsWith("/register.aspx")
                    && !path.StartsWith("/qqconnect.aspx"))
                    path = referrer.ToString();
                else
                    path = string.Empty;
            }

            // 获取授权Url，跳过去验证QQ登录
            var connect = new QzoneConnectContext(path);
            var url = connect.GetAuthorizationUrl();
            //XTrace.WriteLine("Authorize -> " + url);
            Response.Redirect(url);
        }

        private void Access()
        {
            var connect = QzoneConnectContext.Current;
            var code = Request.QueryString["code"];
            if (connect == null || string.IsNullOrEmpty(code))
            {
                XTrace.WriteLine("Access -> QzoneConnectContext.Current == null || IsNullOrEmpty(code)");
                AddErrLine("QQ登录过程错误");
                return;
            }
            var token = connect.GetAccessToken(code);
            if (token == null)
            {
                XTrace.WriteLine("Access -> token == null code={0}", code);
                AddErrLine("QQ登录过程错误 code={0}".F(code));
                return;
            }

            var storedToken = QzoneConnectToken.FindByOpenId(token.OpenId);
            // 注意，非pure（匿名）的不能删掉过期的token，因为没有绑定log。若删除了会导致同一个qq可以注册多个匿名账号
            if (storedToken != null && storedToken.ExpiresAt < DateTime.Now && !storedToken.IsPure)
            {
                storedToken.Delete();
                storedToken = null;
            }

            if (this.userid <= 0 && storedToken == null)
            {
                var url = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Host, BaseConfigInfo.Current.Forumpath + qzconfig.QzoneConnectPage + "?act=bind");
                XTrace.WriteLine("Access -> redirect -> " + url);
                Response.Redirect(url);
            }
            else
            {
                if (this.userid <= 0)
                {
                    var shortUserInfo = BBX.Entity.User.FindByID(storedToken.Uid);
                    if (shortUserInfo != null)
                        LoginUser(shortUserInfo);
                    else
                        XTrace.WriteLine("竟然没找到ID={0}的用户{1}", storedToken.Uid, storedToken);
                    token.Uid = storedToken.Uid;
                }
                else
                {
                    token.Uid = this.userid;
                }

                if (storedToken == null)
                    token.Insert();
                else
                    token.Update();

                XTrace.WriteLine("Access -> success, redirect to " + connect.Callback);
                Response.Redirect(connect.Callback);
            }
        }

        private void Bind()
        {
            var connect = QzoneConnectContext.Current;
            if (!connect.IsOnlineUserBindConnect) //prerequisites is current user have a openId
                return;

            allowreg = config.Regstatus != 0 && qzconfig.EnableConnect && !this.isbindoverflow;
            if (!allowreg) return;

            var type = Request["bind_type"];
            if (type != null) type = type.ToLower();

            if (type == "new")
                BindNew();
            else if (type == "bind")
                BindExisted();
            else if (type == "pure")
                BindPure();
            else
            {
                qqnikename = connect.GetQqNickname();
                qqgender = connect.GetQqSex();
                avatarurl = connect.GetAvatarUrl();
            }
        }

        private void BindNew()
        {
            if (userid > 0)
            {
                base.AddErrLine("当前已有用户登录,无法注册");
                return;
            }
            //if (config.Regstatus < 1 || cloudconfig.Allowconnectregister == 0)
            //{
            //    base.AddErrLine("论坛当前禁止新的QQ会员登录");
            //    return;
            //}
            string username = DNTRequest.GetString(config.Antispamregisterusername);
            string email = DNTRequest.GetString(config.Antispamregisteremail).Trim().ToLower();
            string birthday = DNTRequest.GetString("bday").Trim();
            string errinfo = "";
            if (!Users.PageValidateUserName(username, out errinfo) || !Users.PageValidateEmail(email, false, out errinfo))
            {
                base.AddErrLine(errinfo);
                return;
            }
            if (!Utils.IsDateString(birthday) && !string.IsNullOrEmpty(birthday))
            {
                base.AddErrLine("生日格式错误, 如果不想填写生日请置空");
                return;
            }
            if (Users.GetUserId(username) > 0)
            {
                base.AddErrLine("请不要重复提交！");
                return;
            }

            var connect = QzoneConnectContext.Current;

            var storedToken = QzoneConnectToken.FindByOpenId(connect.Token.OpenId);
            if (storedToken != null)
            {
                var url = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Host, BaseConfigInfo.Current.Forumpath + qzconfig.QzoneConnectPage + "?act=access");
                XTrace.WriteLine("BindNew -> redirect -> " + url);
                Response.Redirect(url);
                return;
            }

            User userInfo = this.CreateUser(username, connect.GetQqNickname(), email, birthday);
            connect.Token.Uid = userInfo.ID;
            connect.Token.Save();
            //QzoneConnectToken.Insert(connect.Token);

            XTrace.WriteLine("BindNew -> success, uid=" + connect.Token.Uid);

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
                msg.Create(false);
            }
            //Sync.UserRegister(userInfo.ID, userInfo.Name, userInfo.Password, "");
            if (qzconfig.AllowUseQZAvater && DNTRequest.GetString("use_qzone_avatar") == "1")
            {
                var qZoneAvatar = new QZoneAvatar();
                qZoneAvatar.AsyncGetAvatar(connect);
            }
            LoginUser(userInfo);
            //Statistics.ReSetStatisticsCache();
            Statistic.Reset();
            if (this.config.Regverify != 2)
            {
                CreditsFacade.UpdateUserCredits(userInfo.ID);
                Online.UpdateAction(this.olid, UserAction.Register, 0, this.config.Onlinetimeout);
                base.AddMsgLine("QQ登录成功,继续浏览");
            }
            else
            {
                base.AddMsgLine("QQ数据绑定完成, 但需要系统管理员审核您的帐户后才可登录使用");
            }
            base.SetUrl(connect.Callback);
            base.SetMetaRefresh();
            base.SetShowBackLink(false);

            XTrace.WriteLine("BindNew -> finish, uid=" + connect.Token.Uid);
        }

        private void BindExisted()
        {
            if (LoginLogs.UpdateLoginLog(WebHelper.UserHost, false) >= 5)
            {
                base.AddErrLine("您已经多次输入密码错误, 请15分钟后再登录");
                return;
            }
            if (this.config.Emaillogin == 1 && Utils.IsValidEmail(this.postusername))
            {
                var list = BBX.Entity.User.FindAllByEmail(postusername);
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
            var user = this.GetShortUserInfo();
            if (user == null)
            {
                int num = LoginLogs.UpdateLoginLog(WebHelper.UserHost, true);
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
            if ((user.GroupID == 4 || user.GroupID == 5) && user.GroupExpiry != 0 && user.GroupExpiry <= DateTime.Now.ToString("yyyyMMdd").ToInt(0))
            {
                var creditsUserGroupId = CreditsFacade.GetCreditsUserGroupId((float)user.Credits);
                this.usergroupid = ((creditsUserGroupId.ID != 0) ? creditsUserGroupId.ID : this.usergroupid);
                user.GroupID = this.usergroupid;
                //Users.UpdateUserGroup(user.ID, this.usergroupid);
                user.GroupID = usergroupid;
                (user as IEntity).Save();
            }
            if (user.GroupID == 5)
            {
                base.AddErrLine("该用户已经被禁止访问,无法绑定");
                return;
            }

            var connect = QzoneConnectContext.Current;
            connect.Token.Uid = user.ID;
            connect.Token.Save();
            //QzoneConnectToken.Insert(connect.Token);

            XTrace.WriteLine("BindExisted -> success, uid=" + connect.Token.Uid);

            if (user.GroupID != 8)
            {
                this.LoginUser(user);
                base.AddMsgLine("QQ登录成功,继续浏览");
            }
            else
            {
                base.AddMsgLine("帐号绑定成功,但需要管理员审核通过才能登录");
            }
            base.SetUrl(connect.Callback);
            base.SetMetaRefresh();
            base.SetShowBackLink(false);
        }

        private void BindPure()
        {
            if (this.userid > 0)
            {
                base.AddErrLine("当前已有用户登录,无法注册");
                return;
            }
            //if (this.config.Regstatus < 1 || this.cloudconfig.Allowconnectregister == 0)
            //{
            //    base.AddErrLine("论坛当前禁止新的QQ会员登录");
            //    return;
            //}

            var connect = QzoneConnectContext.Current;

            var storedToken = QzoneConnectToken.FindByOpenId(connect.Token.OpenId);
            if (storedToken != null)
            {
                var url = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Host, BaseConfigInfo.Current.Forumpath + qzconfig.QzoneConnectPage + "?act=access");
                XTrace.WriteLine("BindNew -> redirect -> " + url);
                Response.Redirect(url);
                return;
            }

            var nickName = connect.GetQqNickname();
            User userInfo = this.CreateUser(connect.Token.AccessToken.Substring(0, 10), nickName, "", "");
            //修复bug，保存后才能有userid
            connect.Token.Uid = userInfo.ID;

            // 更新用户名为友好名称，优先使用QQ昵称
            if (BBX.Entity.User.FindByName(nickName) == null)
                userInfo.Name = nickName;
            else
                userInfo.Name = "QQ#" + userInfo.ID;
            userInfo.Update();

            connect.Token.IsPure = true;
            connect.Token.Save();
            //QzoneConnectToken.Insert(connect.Token);

            XTrace.WriteLine("BindPure -> success, uid=" + connect.Token.Uid);

            //Statistic.UpdateStatisticsLastUserName(userInfo.ID, userInfo.Name);
            Statistic.Reset();

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
            //Sync.UserRegister(userInfo.ID, userInfo.Name, userInfo.Password, "");
            LoginUser(userInfo);
            //Statistics.ReSetStatisticsCache();
            Statistic.Reset();
            if (this.config.Regverify != 2)
            {
                CreditsFacade.UpdateUserCredits(userInfo.ID);
                Online.UpdateAction(this.olid, UserAction.Register, 0, this.config.Onlinetimeout);
                base.AddMsgLine("QQ登录成功,继续浏览");
            }
            else
            {
                base.AddMsgLine("QQ数据绑定完成, 但需要系统管理员审核您的帐户后才可登录使用");
            }
            base.SetUrl(connect.Callback);
            base.SetMetaRefresh();
            base.SetShowBackLink(false);

            XTrace.WriteLine("BindPure -> finish, uid=" + connect.Token.Uid);
        }

        private void LoginUser(IUser userInfo)
        {
            XTrace.WriteLine("{0}({1}) 登录", userInfo.Name, userInfo.NickName);

            ForumUtils.WriteUserCookie(userInfo.ID, DNTRequest.GetInt("expires", -1), this.config.Passwordkey, DNTRequest.GetInt("templateid", 0), DNTRequest.GetInt("loginmode", -1));
            this.oluserinfo = Online.UpdateInfo(userInfo.ID, "");
            this.olid = this.oluserinfo.ID;
            this.username = userInfo.Name;
            this.userid = userInfo.ID;
            this.usergroupinfo = UserGroup.FindByID(userInfo.GroupID);
            this.useradminid = this.usergroupinfo.RadminID;
            Online.UpdateAction(this.olid, UserAction.Login, 0);
            LoginLogs.DeleteLoginLog(WebHelper.UserHost);
            Users.UpdateUserCreditsAndVisit(userInfo, WebHelper.UserHost);
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
                user = BBX.Entity.User.Login(this.postusername, this.postpassword, true, DNTRequest.GetFormInt("question", 0), DNTRequest.GetString("answer"));
            }
            else
            {
                //num = Users.CheckPassword(this.postusername, this.postpassword, true);
                user = BBX.Entity.User.Login(this.postusername, this.postpassword);
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

            //Users.UpdateTrendStat(TrendType.Login);
            TrendStat.Today.Login++;

            return user;
        }

        private User CreateUser(string userName, string nickname, string email, string birthday)
        {
            var userInfo = new User();
            userInfo.Name = userName;
            userInfo.Email = email;
            userInfo.Bday = birthday;
            userInfo.NickName = nickname;
            userInfo.Password = Utils.MD5(ForumUtils.CreateAuthStr(16));
            userInfo.Secques = "";
            userInfo.Gender = DNTRequest.GetInt("gender", 0);
            userInfo.AdminID = 0;
            userInfo.GroupExpiry = 0;
            userInfo.ExtGroupIds = "";
            userInfo.RegIP = userInfo.LastIP = WebHelper.UserHost;
            userInfo.JoinDate = userInfo.LastVisit = userInfo.LastActivity = userInfo.LastPost = DateTime.Now;
            userInfo.LastPostID = 0;
            userInfo.LastPostTitle = "";
            userInfo.Posts = 0;
            userInfo.DigestPosts = 0;
            userInfo.OLTime = 0;
            userInfo.PageViews = 0;
            userInfo.Credits = 0;
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
            userInfo.ShowEmail = DNTRequest.GetInt("showemail", 0) != 0;
            userInfo.Salt = "";
            int newsletter = (this.config.Regadvance == 0) ? 3 : DNTRequest.GetInt("receivesetting", 3);
            //userInfo.Newsletter = (ReceivePMSettingType)newsletter;
            userInfo.NewsLetter = newsletter;
            userInfo.Invisible = DNTRequest.GetInt("invisible", 0) != 0;
            userInfo.Newpm = ((this.config.Welcomemsg == 1) ? 1 : 0) != 0;
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
            uf.Customstatus = usergroupinfo.AllowCstatus ? DNTRequest.GetHtmlEncodeString("customstatus") : "";
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

        private void Unbind()
        {
            if (this.userid > 0)
            {
                var connect = QzoneConnectContext.Current;
                var token = connect.Token;
                if (!token.IsPure)
                {
                    QzoneConnectToken.Delete(token);
                    connect.UnbindToken();
                    var referrer = Request.UrlReferrer;
                    if (referrer == null)
                        this.SetUrl(this.forumpath + "index.aspx");
                    else
                        this.SetUrl(this.forumpath + "usercpprofile.aspx");

                    base.AddMsgLine("已成功解除qq绑定");
                    base.SetMetaRefresh();

                    XTrace.WriteLine("Unbind -> success");
                }
                else
                {
                    AddMsgLine("尚未注册本站账号，不能解除绑定");
                }
            }
            else
            {
                AddMsgLine("未登录");
            }
        }
    }
}