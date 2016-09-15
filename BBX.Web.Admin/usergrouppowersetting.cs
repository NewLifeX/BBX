using System;
using System.Web.UI.WebControls;
using BBX.Common;
using BBX.Entity;

namespace BBX.Web.Admin
{
	public class usergrouppowersetting : UserControlsPageBase
	{
		protected BBX.Control.CheckBoxList usergroupright;
		protected BBX.Control.RadioButtonList allowavatar;
		protected BBX.Control.RadioButtonList allowsearch;
		protected BBX.Control.RadioButtonList reasonpm;
		protected Literal outscript;

		protected void Page_Load(object sender, EventArgs e)
		{
			//if (!base.IsPostBack && MallPluginProvider.GetInstance() == null)
			if (!base.IsPostBack)
			{
				this.usergroupright.Items.RemoveAt(this.usergroupright.Items.Count - 1);
			}
		}

		public void Bind(UserGroup usergroupinfo)
		{
			if (!usergroupinfo.AllowSearch)
			{
				this.allowsearch.Items[0].Selected = true;
			}
			else
			{
				this.allowsearch.Items[1].Selected = true;
			}
			//if (usergroupinfo.Allowsearch.ToString() == "2")
			//{
			//    this.allowsearch.Items[2].Selected = true;
			//}
			this.reasonpm.Items[usergroupinfo.ReasonPm].Selected = true;
			if (usergroupinfo.AllowVisit)
			{
				this.usergroupright.Items[0].Selected = true;
			}
			if (usergroupinfo.AllowPost)
			{
				this.usergroupright.Items[1].Selected = true;
			}
			if (usergroupinfo.AllowReply)
			{
				this.usergroupright.Items[2].Selected = true;
			}
			if (usergroupinfo.AllowPostpoll)
			{
				this.usergroupright.Items[3].Selected = true;
			}
			if (usergroupinfo.AllowVote)
			{
				this.usergroupright.Items[4].Selected = true;
			}
			if (usergroupinfo.AllowPostattach)
			{
				this.usergroupright.Items[5].Selected = true;
			}
			if (usergroupinfo.AllowGetattach)
			{
				this.usergroupright.Items[6].Selected = true;
			}
			if (usergroupinfo.AllowSetreadPerm)
			{
				this.usergroupright.Items[7].Selected = true;
			}
			if (usergroupinfo.AllowSetattachPerm)
			{
				this.usergroupright.Items[8].Selected = true;
			}
			if (usergroupinfo.AllowHideCode)
			{
				this.usergroupright.Items[9].Selected = true;
			}
			if (usergroupinfo.AllowCusbbCode)
			{
				this.usergroupright.Items[10].Selected = true;
			}
			if (usergroupinfo.AllowSigbbCode)
			{
				this.usergroupright.Items[11].Selected = true;
			}
			if (usergroupinfo.AllowSigimgCode)
			{
				this.usergroupright.Items[12].Selected = true;
			}
			if (usergroupinfo.AllowViewpro)
			{
				this.usergroupright.Items[13].Selected = true;
			}
			if (usergroupinfo.DisablePeriodctrl)
			{
				this.usergroupright.Items[14].Selected = true;
			}
			if (usergroupinfo.AllowDebate)
			{
				this.usergroupright.Items[15].Selected = true;
			}
			if (usergroupinfo.AllowBonus)
			{
				this.usergroupright.Items[16].Selected = true;
			}
			if (usergroupinfo.AllowViewstats)
			{
				this.usergroupright.Items[17].Selected = true;
			}
			if (usergroupinfo.AllowDiggs)
			{
				this.usergroupright.Items[18].Selected = true;
			}
			if (usergroupinfo.AllowHtmlTitle)
			{
				this.usergroupright.Items[19].Selected = true;
			}
			if (usergroupinfo.AllowHtml)
			{
				this.usergroupright.Items[20].Selected = true;
			}
			if (usergroupinfo.ModNewTopics == 1)
			{
				this.usergroupright.Items[21].Selected = true;
			}
			if (usergroupinfo.ModNewPosts == 1)
			{
				this.usergroupright.Items[22].Selected = true;
			}
			if (usergroupinfo.IgnoresecCode == 1)
			{
				this.usergroupright.Items[23].Selected = true;
			}
			//if (MallPluginProvider.GetInstance() != null && usergroupinfo.AllowTrade)
			//{
			//	this.usergroupright.Items[this.usergroupright.Items.Count - 1].Selected = true;
			//}
			string text = "<script type='text/javascript'>\r\nfunction insertBonusPrice()\r\n{\r\n\t";
			text = text + "\r\n\tvar tdelement = document.getElementById('" + this.usergroupright.ClientID + "_16').parentNode;";
			object obj = text;
			text = obj + "\r\n\ttdelement.innerHTML += '&nbsp;最低悬赏价格:<input type=\"text\" name=\"minbonusprice\" id=\"minbonusprice\" class=\"FormBase\" onblur=\"this.className=\\'FormBase\\';\" onfocus=\"this.className=\\'FormFocus\\';\" size=\"4\" maxlength=\"5\" value=\"" + usergroupinfo.MinBonusprice + "\"" + (!usergroupinfo.AllowBonus ? " disabled=\"disabled \"" : "") + " />'";
			object obj2 = text;
			text = obj2 + "\r\n\ttdelement.innerHTML += '&nbsp;最高悬赏价格:<input type=\"text\" name=\"maxbonusprice\" id=\"maxbonusprice\" class=\"FormBase\" onblur=\"this.className=\\'FormBase\\';\" onfocus=\"this.className=\\'FormFocus\\';\" size=\"4\" maxlength=\"5\" value=\"" + usergroupinfo.MaxBonusprice + "\"" + (!usergroupinfo.AllowBonus ? " disabled=\"disabled \"" : "") + " />'";
			text += "\r\n}\r\ninsertBonusPrice();\r\n</script>\r\n";
			this.outscript.Text = text;
			this.usergroupright.Items[16].Attributes.Add("onclick", "bonusPriceSet(this.checked)");
		}

		public void Bind()
		{
			this.allowsearch.Items[0].Selected = true;
			this.reasonpm.Items[0].Selected = true;
			for (int i = 0; i < this.usergroupright.Items.Count; i++)
			{
				this.usergroupright.Items[i].Selected = false;
			}
			string text = "<script type='text/javascript'>\r\nfunction insertBonusPrice()\r\n{\r\n\t";
			text = text + "\r\n\tvar tdelement = document.getElementById('" + this.usergroupright.ClientID + "_16').parentNode;";
			text += "\r\n\ttdelement.innerHTML += '&nbsp;最低悬赏价格:<input type=\"text\" name=\"minbonusprice\" id=\"minbonusprice\" class=\"FormBase\" onblur=\"this.className=\\'FormBase\\';\" onfocus=\"this.className=\\'FormFocus\\';\" size=\"4\" maxlength=\"5\" value=\"10\" disabled=\"disabled\" />'";
			text += "\r\n\ttdelement.innerHTML += '&nbsp;最高悬赏价格:<input type=\"text\" name=\"maxbonusprice\" id=\"maxbonusprice\" class=\"FormBase\" onblur=\"this.className=\\'FormBase\\';\" onfocus=\"this.className=\\'FormFocus\\';\" size=\"4\" maxlength=\"5\" value=\"20\" disabled=\"disabled\" />'";
			text += "\r\n}\r\ninsertBonusPrice();\r\n</script>\r\n";
			this.outscript.Text = text;
			this.usergroupright.Items[16].Attributes.Add("onclick", "bonusPriceSet(this.checked)");
		}

		public void GetSetting(ref UserGroup usergroupinfo)
		{
			usergroupinfo.AllowSearch = this.allowsearch.SelectedValue.ToInt() > 0;
			usergroupinfo.ReasonPm = this.reasonpm.SelectedValue.ToInt();
			usergroupinfo.AllowVisit = this.usergroupright.Items[0].Selected;
			usergroupinfo.AllowPost = this.usergroupright.Items[1].Selected;
			usergroupinfo.AllowReply = this.usergroupright.Items[2].Selected;
			usergroupinfo.AllowPostpoll = this.usergroupright.Items[3].Selected;
			usergroupinfo.AllowVote = this.usergroupright.Items[4].Selected;
			usergroupinfo.AllowPostattach = this.usergroupright.Items[5].Selected;
			usergroupinfo.AllowGetattach = this.usergroupright.Items[6].Selected;
			usergroupinfo.AllowSetreadPerm = this.usergroupright.Items[7].Selected;
			usergroupinfo.AllowSetattachPerm = this.usergroupright.Items[8].Selected;
			usergroupinfo.AllowHideCode = this.usergroupright.Items[9].Selected;
			usergroupinfo.AllowCusbbCode = this.usergroupright.Items[10].Selected;
			usergroupinfo.AllowSigbbCode = this.usergroupright.Items[11].Selected;
			usergroupinfo.AllowSigimgCode = this.usergroupright.Items[12].Selected;
			usergroupinfo.AllowViewpro = this.usergroupright.Items[13].Selected;
			usergroupinfo.DisablePeriodctrl = this.usergroupright.Items[14].Selected;
			usergroupinfo.AllowDebate = this.usergroupright.Items[15].Selected;
			usergroupinfo.AllowBonus = this.usergroupright.Items[16].Selected;
			if (this.usergroupright.Items[16].Selected)
			{
				usergroupinfo.MinBonusprice = (short)DNTRequest.GetInt("minbonusprice", 0);
				usergroupinfo.MaxBonusprice = (short)DNTRequest.GetInt("maxbonusprice", 0);
			}
			else
			{
				usergroupinfo.MinBonusprice = 0;
				usergroupinfo.MaxBonusprice = 0;
			}
			usergroupinfo.AllowViewstats = this.usergroupright.Items[17].Selected;
			usergroupinfo.AllowDiggs = this.usergroupright.Items[18].Selected;
			usergroupinfo.AllowHtmlTitle = this.usergroupright.Items[19].Selected;
			usergroupinfo.AllowHtml = this.usergroupright.Items[20].Selected;
			usergroupinfo.ModNewTopics = (short)(this.usergroupright.Items[21].Selected ? 1 : 0);
			usergroupinfo.ModNewPosts = (short)(this.usergroupright.Items[22].Selected ? 1 : 0);
			usergroupinfo.IgnoresecCode = this.usergroupright.Items[23].Selected ? 1 : 0;
			//if (MallPluginProvider.GetInstance() != null)
			//{
			//	usergroupinfo.AllowTrade = this.usergroupright.Items[this.usergroupright.Items.Count - 1].Selected;
			//}
		}
	}
}