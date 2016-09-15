using System;
using BBX.Common;
using BBX.Entity;
using BBX.Forum;
using BBX.Web.UI;
using XCode;

namespace BBX.Web
{
	public class usercppreference : UserCpPage
	{
		public int receivepmsetting;

		protected override void ShowPage()
		{
			this.pagetitle = "用户控制面板";
			if (!base.IsLogin())
			{
				return;
			}
			this.receivepmsetting = user.NewsLetter;
			if (DNTRequest.IsPost())
			{
				//Users.UpdateUserPreference(this.userid, "", 0, 0, DNTRequest.GetInt("templateid", 0));
				user.Avatar = "";
				user.Avatarwidth = 0;
				user.Avatarheight = 0;
				user.TemplateID = Request["templateid"].ToInt();
				(user as IEntity).Save();

				this.UpdateUserForumSetting();
				Online.UpdateInvisible(this.olid, this.user.Invisible);
				this.WriteCookie();
				this.receivepmsetting = DNTRequest.GetInt("receivesetting", 1);
				//this.user.NewsLetter = (ReceivePMSettingType)this.receivepmsetting;
				this.user.NewsLetter = receivepmsetting;
				//Users.UpdateUserPMSetting(this.user);
				(user as IEntity).Save();
				base.SetUrl("usercppreference.aspx");
				base.SetMetaRefresh();
				base.SetShowBackLink(true);
				base.AddMsgLine("修改个性设置完毕");
			}
		}

		private void WriteCookie()
		{
			ForumUtils.WriteCookie("tpp", this.user.Tpp.ToString());
			ForumUtils.WriteCookie("ppp", this.user.Ppp.ToString());
			ForumUtils.WriteCookie("pmsound", this.user.Pmsound.ToString());
			Utils.WriteCookie(Utils.GetTemplateCookieName(), DNTRequest.GetInt("templateid", 0).ToString(), 999999);
		}

		public void UpdateUserForumSetting()
		{
			this.user.ID = this.userid;
			this.user.Tpp = DNTRequest.GetInt("tpp", 0);
			this.user.Ppp = DNTRequest.GetInt("ppp", 0);
			this.user.Pmsound = DNTRequest.GetInt("pmsound", 0);
			this.user.Invisible = DNTRequest.GetInt("invisible", 0) != 0;
			this.user.Field.Customstatus = Utils.HtmlEncode(ForumUtils.BanWordFilter(DNTRequest.GetString("customstatus")));
			//Users.UpdateUserForumSetting(this.user);
			(user as IEntity).Save();
		}
	}
}