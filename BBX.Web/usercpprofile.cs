using System;
using System.Text.RegularExpressions;
using BBX.Common;
using BBX.Config;
using BBX.Entity;
using BBX.Forum;
using BBX.Web.UI;

namespace BBX.Web
{
    public class usercpprofile : UserCpPage
    {
        public string avatarFlashParam = "";
        public string avatarImage = "";
        public string sig = string.Empty;
        public string action = DNTRequest.GetString("action");

        protected override void ShowPage()
        {
            this.pagetitle = "用户控制面板";
            if (!base.IsLogin()) return;

            var uid = userid;
            if (DNTRequest.IsPost())
            {
                if (ForumUtils.IsCrossSitePost())
                {
                    base.AddErrLine("您的请求来路不正确，无法提交。如果您安装了某种默认屏蔽来路信息的个人防火墙软件(如 Norton Internet Security)，请设置其不要禁止来路信息后再试。");
                    return;
                }
                this.ValidateInfo();
                if (base.IsErr()) return;

                if (this.page_err == 0)
                {
                    var userInfo = Users.GetUserInfo(uid);
                    //var userInfo2 = (User)userInfo.Clone();
                    var uf = userInfo.Field;
                    this.sig = userInfo.Field.Sightml;
                    //userInfo.ID = uid;
                    userInfo.Name = this.username;
                    userInfo.NickName = Utils.HtmlEncode(ForumUtils.BanWordFilter(DNTRequest.GetString("nickname")));
                    userInfo.Gender = DNTRequest.GetInt("gender", 0);

                    uf.RealName = DNTRequest.GetHtmlEncodeString("realname");
                    uf.Idcard = DNTRequest.GetHtmlEncodeString("idcard");
                    uf.Mobile = DNTRequest.GetHtmlEncodeString("mobile");
                    uf.Phone = DNTRequest.GetHtmlEncodeString("phone");

                    var email = DNTRequest.GetHtmlEncodeString("email").Trim().ToLower();
                    if (email != userInfo.Email && !Users.ValidateEmail(email, userInfo.ID))
                    {
                        base.AddErrLine("Email: \"" + userInfo.Email + "\" 已经被其它用户注册使用");
                        return;
                    }
                    userInfo.Email = email;

                    userInfo.Bday = DNTRequest.GetHtmlEncodeString("bday");
                    userInfo.ShowEmail = DNTRequest.GetInt("showemail", 1) != 0;

                    if (DNTRequest.GetString("website").IndexOf(".") > -1 && !DNTRequest.GetString("website").ToLower().StartsWith("http"))
                        uf.Website = Utils.HtmlEncode("http://" + DNTRequest.GetString("website"));
                    else
                        uf.Website = DNTRequest.GetHtmlEncodeString("website");
                    uf.Icq = DNTRequest.GetHtmlEncodeString("icq");
                    uf.qq = DNTRequest.GetHtmlEncodeString("qq");
                    uf.Yahoo = DNTRequest.GetHtmlEncodeString("yahoo");
                    uf.Msn = DNTRequest.GetHtmlEncodeString("msn");
                    uf.Skype = DNTRequest.GetHtmlEncodeString("skype");
                    uf.Location = DNTRequest.GetHtmlEncodeString("location");
                    uf.Bio = ForumUtils.BanWordFilter(DNTRequest.GetHtmlEncodeString("bio"));

                    var postpramsInfo = new PostpramsInfo();
                    postpramsInfo.Usergroupid = this.usergroupid;
                    postpramsInfo.Attachimgpost = this.config.Attachimgpost;
                    postpramsInfo.Showattachmentpath = this.config.Showattachmentpath;
                    postpramsInfo.Hide = 0;
                    postpramsInfo.Price = 0;
                    postpramsInfo.Sdetail = ForumUtils.BanWordFilter(DNTRequest.GetHtmlEncodeString("signature"));
                    postpramsInfo.Smileyoff = 1;
                    postpramsInfo.BBCode = this.usergroupinfo.AllowSigbbCode;
                    postpramsInfo.Parseurloff = 1;
                    postpramsInfo.Showimages = this.usergroupinfo.AllowSigimgCode ? 1 : 0;
                    postpramsInfo.Allowhtml = 0;
                    postpramsInfo.Signature = 1;
                    postpramsInfo.Smiliesinfo = Smilies.GetSmiliesListWithInfo();
                    postpramsInfo.Customeditorbuttoninfo = null;
                    postpramsInfo.Smiliesmax = this.config.Smiliesmax;
                    postpramsInfo.Signature = 1;

                    uf.Sightml = UBB.UBBToHTML(postpramsInfo);
                    if (this.sig != uf.Sightml)
                    {
                        Sync.UpdateSignature(userInfo.ID, userInfo.Name, uf.Sightml, "");
                    }
                    if (uf.Sightml.Length >= 1000)
                    {
                        base.AddErrLine("您的签名转换后超出系统最大长度， 请返回修改");
                        return;
                    }
                    uf.Signature = postpramsInfo.Sdetail;
                    userInfo.Sigstatus = ((DNTRequest.GetInt("sigstatus", 0) != 0) ? 1 : 0);
                    //throw new NotImplementedException("CheckModified");
                    ////if (this.CheckModified(userInfo, userInfo2))
                    //{
                    //    //Users.UpdateUserProfile(userInfo2);
                    //    Sync.UpdateProfile(userInfo.ID, userInfo.Name, "");
                    //}
                    userInfo.Save();
                    Sync.UpdateProfile(userInfo.ID, userInfo.Name, "");

                    Online.DeleteUserByUid(userInfo.ID);
                    ForumUtils.WriteCookie("sigstatus", userInfo.Sigstatus.ToString());

                    base.SetUrl("usercpprofile.aspx");
                    base.SetMetaRefresh();
                    base.SetShowBackLink(true);
                    base.AddMsgLine("修改个人档案完毕");
                    return;
                }
            }
            else
            {
                this.pagename += ((String.IsNullOrEmpty(this.action)) ? "" : ("?action=" + this.action));
                var userInfo3 = Users.GetUserInfo(uid);
                this.avatarFlashParam = Utils.GetRootUrl(BaseConfigs.GetForumPath) + "images/common/camera.swf?nt=1&inajax=1&appid=" + (Utils.MD5(userInfo3.Name + userInfo3.Password + userInfo3.ID + this.olid)) + "&input=" + (DES.Encode(uid + "," + this.olid, this.config.Passwordkey)) + "&ucapi=" + (Utils.UrlEncode(Utils.GetRootUrl(BaseConfigs.GetForumPath) + "tools/ajax.ashx"));
                this.avatarImage = Avatars.GetAvatarUrl(uid);
            }
        }

        //private bool CheckModified(UserInfo oldUserInfo, UserInfo userInfo)
        //{
        //    return userInfo.Uid != oldUserInfo.Uid || userInfo.Nickname != oldUserInfo.Nickname || userInfo.Gender != oldUserInfo.Gender || userInfo.Realname != oldUserInfo.Realname || userInfo.Idcard != oldUserInfo.Idcard || userInfo.Mobile != oldUserInfo.Mobile || userInfo.Phone != oldUserInfo.Phone || userInfo.Email != oldUserInfo.Email || userInfo.Bday != oldUserInfo.Bday || userInfo.Showemail != oldUserInfo.Showemail || userInfo.Website != oldUserInfo.Website || userInfo.Icq != oldUserInfo.Icq || userInfo.Qq != oldUserInfo.Qq || userInfo.Yahoo != oldUserInfo.Yahoo || userInfo.Msn != oldUserInfo.Msn || userInfo.Skype != oldUserInfo.Skype || userInfo.Location != oldUserInfo.Location || userInfo.Bio != oldUserInfo.Bio || userInfo.Signature != oldUserInfo.Signature || userInfo.Sigstatus != oldUserInfo.Sigstatus;
        //}

        public void ValidateInfo()
        {
            if (this.config.Realnamesystem == 1)
            {
                if (String.IsNullOrEmpty(DNTRequest.GetString("realname").Trim()))
                {
                    base.AddErrLine("真实姓名不能为空");
                }
                else
                {
                    if (DNTRequest.GetString("realname").Trim().Length > 10)
                    {
                        base.AddErrLine("真实姓名不能大于10个字符");
                    }
                }
                if (String.IsNullOrEmpty(DNTRequest.GetString("idcard").Trim()))
                {
                    base.AddErrLine("身份证号码不能为空");
                }
                else
                {
                    if (DNTRequest.GetString("idcard").Trim().Length > 20)
                    {
                        base.AddErrLine("身份证号码不能大于20个字符");
                    }
                }
                if (String.IsNullOrEmpty(DNTRequest.GetString("mobile").Trim()) && String.IsNullOrEmpty(DNTRequest.GetString("phone").Trim()))
                {
                    base.AddErrLine("移动电话号码和是固定电话号码必须填写其中一项");
                }
                if (DNTRequest.GetString("mobile").Trim().Length > 20)
                {
                    base.AddErrLine("移动电话号码不能大于20个字符");
                }
                if (DNTRequest.GetString("phone").Trim().Length > 20)
                {
                    base.AddErrLine("固定电话号码不能大于20个字符");
                }
            }
            if (!DNTRequest.GetString("idcard").IsNullOrEmpty() && !Regex.IsMatch(DNTRequest.GetString("idcard").Trim(), "^[\\x20-\\x80]+$"))
            {
                base.AddErrLine("身份证号码中含有非法字符");
            }
            if (!DNTRequest.GetString("mobile").IsNullOrEmpty() && !Regex.IsMatch(DNTRequest.GetString("mobile").Trim(), "^[\\d|-]+$"))
            {
                base.AddErrLine("移动电话号码中含有非法字符");
            }
            if (!DNTRequest.GetString("phone").IsNullOrEmpty() && !Regex.IsMatch(DNTRequest.GetString("phone").Trim(), "^[\\d|-]+$"))
            {
                base.AddErrLine("固定电话号码中含有非法字符");
            }
            string text = DNTRequest.GetString("email").Trim().ToLower();
            if (Utils.StrIsNullOrEmpty(text))
            {
                base.AddErrLine("Email不能为空");
                return;
            }
            if (!Utils.IsValidEmail(text))
            {
                base.AddErrLine("Email格式不正确");
                return;
            }
            if (!Utils.StrIsNullOrEmpty(this.config.Accessemail) && !Utils.InArray(Utils.GetEmailHostName(text), this.config.Accessemail.Replace("\r\n", "\n"), "\n"))
            {
                base.AddErrLine("Email: \"" + text + "\" 不在本论坛允许范围之类, 本论坛只允许用户使用这些Email地址注册: " + this.config.Accessemail.Replace("\n", ",&nbsp;"));
                return;
            }
            if (!Utils.StrIsNullOrEmpty(this.config.Censoremail) && Utils.InArray(Utils.GetEmailHostName(text), this.config.Censoremail.Replace("\r\n", "\n"), "\n"))
            {
                base.AddErrLine("Email: \"" + text + "\" 不允许在本论坛使用, 本论坛不允许用户使用的Email地址包括: " + this.config.Censoremail.Replace("\n", ",&nbsp;"));
                return;
            }
            if (DNTRequest.GetString("bio").Length > 500)
            {
                base.AddErrLine("自我介绍不得超过500个字符");
                return;
            }
            if (DNTRequest.GetString("signature").Length > 500)
            {
                base.AddErrLine("签名不得超过500个字符");
                return;
            }
            var nickname = Request["nickname"];
            if (!nickname.IsNullOrWhiteSpace() && ForumUtils.IsBanUsername(nickname, this.config.Censoruser))
            {
                base.AddErrLine("昵称 \"" + nickname + "\" 不允许在本论坛使用");
                return;
            }
            if (DNTRequest.GetString("signature").Length > this.usergroupinfo.MaxSigSize)
            {
                base.AddErrLine(string.Format("您的签名长度超过 {0} 字符的限制，请返回修改。", this.usergroupinfo.MaxSigSize));
            }
        }
    }
}