using System;
using System.Data;
using System.Text;
using System.Web;
using BBX.Common;
using BBX.Config;
using BBX.Entity;
using BBX.Forum;
using NewLife.Web;
using XCode;

namespace BBX.Web
{
	public class login : PageBase
	{
		public string postusername = DNTRequest.GetHtmlEncodeString("postusername").Trim();
		public string loginauth = DNTRequest.GetString("loginauth");
		public string postpassword = "";
		public string referer = DNTRequest.GetHtmlEncodeString("referer");
		public bool loginsubmit = DNTRequest.GetString("loginsubmit") == "true";
		public string authstr = "";
		public int needactiveuid = -1;
		public string timestamp = "";
		public string email = "";
		public int inapi;

		protected override void ShowPage()
		{
			this.pagetitle = "用户登录";
			this.inapi = DNTRequest.GetInt("inapi", 0);
			if (this.userid != -1)
			{
				base.SetUrl(BaseConfigs.GetForumPath);
				base.AddMsgLine("您已经登录，无须重复登录");
				this.ispost = true;
				this.SetLeftMenuRefresh();
				if (APIConfigInfo.Current.Enable)
				{
					this.APILogin(APIConfigInfo.Current);
				}
			}
			if (LoginLogs.UpdateLoginLog(WebHelper.UserHost, false) >= 5)
			{
				base.AddErrLine("您已经多次输入密码错误, 请15分钟后再登录");
				this.loginsubmit = false;
				return;
			}
			this.SetReUrl();
			var username = DNTRequest.GetString("username");
			if (DNTRequest.IsPost())
			{
				this.SetBackLink();
				if (this.isseccode && String.IsNullOrEmpty(DNTRequest.GetString("vcode")))
				{
					this.postusername = username;
					this.loginauth = DES.Encode(DNTRequest.GetString("password"), this.config.Passwordkey).Replace("+", "[");
					this.loginsubmit = true;
					return;
				}
				if (this.config.Emaillogin == 1 && Utils.IsValidEmail(username))
				{
					//var userInfoByEmail = Users.GetUserInfoByEmail(username);
					var list = BBX.Entity.User.FindAllByEmail(username);
					//if (userInfoByEmail.Rows.Count == 0)
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
				if (this.config.Emaillogin == 0 && Users.GetUserId(username) == 0)
				{
					base.AddErrLine("用户不存在");
				}
				if (Utils.StrIsNullOrEmpty(DNTRequest.GetString("password")) && Utils.StrIsNullOrEmpty(DNTRequest.GetString("loginauth")))
				{
					base.AddErrLine("密码不能为空");
				}
				if (base.IsErr())
				{
					return;
				}
				IUser user = this.GetShortUserInfo();
				if (user != null)
				{
					if ((user.GroupID == 4 || user.GroupID == 5) && user.GroupExpiry != 0 && user.GroupExpiry <= Utils.StrToInt(DateTime.Now.ToString("yyyyMMdd"), 0))
					{
						var creditsUserGroupId = CreditsFacade.GetCreditsUserGroupId((float)user.Credits);
						this.usergroupid = ((creditsUserGroupId.ID != 0) ? creditsUserGroupId.ID : this.usergroupid);
						user.GroupID = this.usergroupid;
						//Users.UpdateUserGroup(user.ID, this.usergroupid);
						//XUser user = XUser.FindByID(postInfo.Posterid);
						user.GroupID = 6;
						(user as IEntity).Save();
					}
					if (user.GroupID == 5)
					{
						base.AddErrLine("您所在的用户组，已经被禁止访问");
						return;
					}
					if (user.GroupID == 8)
					{
						if (this.config.Regverify == 1)
						{
							this.needactiveuid = user.ID;
							this.email = user.Email;
							this.timestamp = DateTime.Now.Ticks.ToString();
							this.authstr = Utils.MD5(user.Password + this.config.Passwordkey + this.timestamp);
							base.AddMsgLine("请您到您的邮箱中点击激活链接来激活您的帐号");
						}
						else
						{
							if (this.config.Regverify == 2)
							{
								base.AddMsgLine("您需要等待一些时间, 待系统管理员审核您的帐户后才可登录使用");
							}
							else
							{
								base.AddErrLine("抱歉, 您的用户身份尚未得到验证");
							}
						}
						this.loginsubmit = false;
						return;
					}
					if (!Utils.StrIsNullOrEmpty(user.Secques) && this.loginsubmit && Utils.StrIsNullOrEmpty(DNTRequest.GetString("loginauth")))
					{
						this.loginauth = DES.Encode(DNTRequest.GetString("password"), this.config.Passwordkey).Replace("+", "[");
					}
					else
					{
						base.AddMsgLine("登录成功, 返回登录前页面");
						ForumUtils.WriteUserCookie(user.ID, DNTRequest.GetInt("expires", -1), this.config.Passwordkey, DNTRequest.GetInt("templateid", 0), DNTRequest.GetInt("loginmode", -1));
						this.oluserinfo = Online.UpdateInfo(user.ID, "");
						this.olid = this.oluserinfo.ID;
						this.username = username;
						this.userid = user.ID;
						this.usergroupinfo = UserGroup.FindByID(user.GroupID);
						this.useradminid = this.usergroupinfo.RadminID;
						Online.UpdateAction(this.olid, UserAction.Login, 0);
						LoginLogs.DeleteLoginLog(WebHelper.UserHost);
						Users.UpdateUserCreditsAndVisit(user, WebHelper.UserHost);
						if (APIConfigInfo.Current.Enable)
						{
							this.APILogin(APIConfigInfo.Current);
						}
						this.loginsubmit = false;
						string text = Utils.UrlDecode(ForumUtils.GetReUrl());
						base.SetUrl((text.IndexOf("register.aspx") < 0) ? text : (this.forumpath + "index.aspx"));
						this.SetLeftMenuRefresh();
						if (APIConfigInfo.Current.Enable)
						{
							base.AddMsgLine(Sync.GetLoginScript(this.userid, this.username));
						}
						if (!APIConfigInfo.Current.Enable || !Sync.NeedAsyncLogin())
						{
							base.MsgForward("login_succeed", true);
						}
					}
				}
				else
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
				}
				if (base.IsErr())
				{
					return;
				}
				ForumUtils.WriteUserCreditsCookie(user, this.usergroupinfo.GroupTitle);
			}
		}

		private void SetBackLink()
		{
			var stringBuilder = new StringBuilder();
			string[] allKeys = HttpContext.Current.Request.QueryString.AllKeys;
			for (int i = 0; i < allKeys.Length; i++)
			{
				string text = allKeys[i];
				if (!string.IsNullOrEmpty(text) && !Utils.InArray(text, "postusername"))
				{
					stringBuilder.AppendFormat("&{0}={1}", text, DNTRequest.GetQueryString(text));
				}
			}
			this.question = DNTRequest.GetFormInt("question", 0);
			if (this.question > 0)
			{
				stringBuilder.AppendFormat("&question={0}", this.question);
			}
			base.SetBackLink("login.aspx?postusername=" + Utils.UrlEncode(DNTRequest.GetString("username")) + stringBuilder);
		}

		private IUser GetShortUserInfo()
		{
			this.postpassword = ((!Utils.StrIsNullOrEmpty(this.loginauth)) ? DES.Decode(this.loginauth.Replace("[", "+"), this.config.Passwordkey) : DNTRequest.GetString("password"));
			this.postusername = (Utils.StrIsNullOrEmpty(this.postusername) ? DNTRequest.GetString("username") : this.postusername);
			//int num;

			IUser user = null;
			//switch (this.config.Passwordmode)
			//{
			//    case 0:
			if (this.config.Secques == 1 && (!Utils.StrIsNullOrEmpty(this.loginauth) || !this.loginsubmit))
			{
				//num = Users.CheckPasswordAndSecques(this.postusername, this.postpassword, true, DNTRequest.GetFormInt("question", 0), DNTRequest.GetString("answer"));
				user = BBX.Entity.User.Login(this.postusername, this.postpassword, true, DNTRequest.GetFormInt("question", 0), DNTRequest.GetString("answer"));
			}
			else
			{
				//num = Users.CheckPassword(this.postusername, this.postpassword, true);
				user = BBX.Entity.User.Login(this.postusername, this.postpassword);
			}
			//break;
			//    case 1:
			//        if (this.config.Secques == 1 && (!Utils.StrIsNullOrEmpty(this.loginauth) || !this.loginsubmit))
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

		private void SetReUrl()
		{
			if (!DNTRequest.IsPost() || !String.IsNullOrEmpty(referer))
			{
                var url = Request.UrlReferrer + "";
				if (String.IsNullOrEmpty(referer))
				{
					if (String.IsNullOrEmpty(url) || url.IndexOf("login") > -1 || url.IndexOf("logout") > -1)
					{
                        url = "index.aspx";
					}
				}
                var reurl = Request["reurl"];
                Utils.WriteCookie("reurl", (String.IsNullOrEmpty(reurl) || reurl.IndexOf("login.aspx") > -1) ? url : reurl);
			}
		}

		private void APILogin(APIConfigInfo apiInfo)
		{
			ApplicationInfo applicationInfo = null;
			var appCollection = apiInfo.AppCollection;
			foreach (var current in appCollection)
			{
				if (current.APIKey == DNTRequest.GetString("api_key"))
				{
					applicationInfo = current;
					break;
				}
			}
			if (applicationInfo == null)
			{
				return;
			}
			this.RedirectAPILogin(applicationInfo);
		}

		private void RedirectAPILogin(ApplicationInfo appInfo)
		{
			string text = DNTRequest.GetFormString("expires");
			DateTime date;
			if (Utils.StrIsNullOrEmpty(text))
			{
				date = BBX.Entity.User.FindByID(this.userid).LastVisit.ToUniversalTime().AddSeconds(Convert.ToDouble(Utils.GetCookie("bbx", "expires")));
			}
			else
			{
				date = DateTime.UtcNow.AddSeconds(Convert.ToDouble(text));
			}
			text = Utils.ConvertToUnixTimestamp(date).ToString();
			string next = DNTRequest.GetString("next");
			//this.olid = OnlineUsers.GetOlidByUid(this.userid);
			//if (this.olid > 0)
			//{
			//    this.oluserinfo = Online.FindByID(this.olid);
			//}
			this.oluserinfo = Online.FindByUserID(this.userid);
			string arg;
			if (this.oluserinfo == null)
			{
				arg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
			}
			else
			{
				arg = oluserinfo.LastUpdateTime.ToString("yyyy-MM-dd HH:mm:ss");
			}
			string text2 = DES.Encode(string.Format("{0},{1},{2}", this.olid, arg, text), appInfo.Secret.Substring(0, 10)).Replace("+", "[");
			HttpContext.Current.Response.Redirect(string.Format("{0}{1}auth_token={2}{3}", new object[]
            {
                appInfo.CallbackUrl,
                (appInfo.CallbackUrl.IndexOf("?") > 0) ? "&" : "?",
                text2,
                (String.IsNullOrEmpty(next )) ? next : ("&next=" + next)
            }));
		}

		private void SetLeftMenuRefresh()
		{
			base.SetMetaRefresh();
			base.SetShowBackLink(false);
			base.AddScript("if (top.document.getElementById('leftmenu')){top.frames['leftmenu'].location.reload();}");
		}
	}
}