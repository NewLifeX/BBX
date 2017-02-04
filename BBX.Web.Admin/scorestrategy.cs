using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BBX.Cache;
using BBX.Common;
using BBX.Control;
using BBX.Entity;
using BBX.Forum;
using NewLife;

namespace BBX.Web.Admin
{
	public class scorestrategy : AdminPage
	{
		protected HtmlForm Form1;
		protected pageinfo info1;
		protected Literal Literal1;
		protected BBX.Control.RadioButtonList available;
		protected Literal extcredits1name;
		protected BBX.Control.TextBox extcredits1;
		protected Literal extcredits3name;
		protected BBX.Control.TextBox extcredits3;
		protected Literal extcredits5name;
		protected BBX.Control.TextBox extcredits5;
		protected Literal extcredits7name;
		protected BBX.Control.TextBox extcredits7;
		protected Literal extcredits2name;
		protected BBX.Control.TextBox extcredits2;
		protected Literal extcredits4name;
		protected BBX.Control.TextBox extcredits4;
		protected Literal extcredits6name;
		protected BBX.Control.TextBox extcredits6;
		protected Literal extcredits8name;
		protected BBX.Control.TextBox extcredits8;
		protected BBX.Control.Button SaveInfo;
		protected BBX.Control.Button DeleteSet;
		protected Hint Hint1;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				if (Request["fid"] != "" && Request["fieldname"] != "")
				{
					this.LoadScoreInf(Request["fid"], Request["fieldname"]);
					return;
				}
				base.Response.Write("<script>alert('数据不存在');self.close();</script>");
				base.Response.End();
			}
		}

		public void LoadScoreInf(string fid, string fieldName)
		{
			if (fieldName == "postcredits")
			{
				this.Literal1.Text = "发主题积分策略";
			}
			else
			{
				this.Literal1.Text = "发回复积分策略";
			}
			//DataTable forumField = Forums.GetForumField(fid.ToInt(0), fieldName);
			//if (forumField.Rows.Count == 0)
			var f = XForum.FindByID(fid.ToInt());
			if (f == null)
			{
				base.Response.Write("<script>alert('数据不存在');</script>");
			}
			else
			{
				//string text = forumField.Rows[0][0].ToString().Trim();
				var text = (f as IXForum).PostcrEdits;
				if (text != "" && text != "0")
				{
					string[] array = text.Split(',');
					this.available.SelectedValue = array[0].Trim();
					this.extcredits1.Text = array[1].Trim();
					this.extcredits2.Text = array[2].Trim();
					this.extcredits3.Text = array[3].Trim();
					this.extcredits4.Text = array[4].Trim();
					this.extcredits5.Text = array[5].Trim();
					this.extcredits6.Text = array[6].Trim();
					this.extcredits7.Text = array[7].Trim();
					this.extcredits8.Text = array[8].Trim();
				}
			}
			DataRow dataRow = Scoresets.GetScoreSet().Rows[0];
			if (!dataRow[2].ToString().IsNullOrEmpty())
			{
				this.extcredits1name.Text = dataRow[2].ToString().Trim();
			}
			else
			{
				this.extcredits1.Enabled = false;
			}
			if (!dataRow[3].ToString().IsNullOrEmpty())
			{
				this.extcredits2name.Text = dataRow[3].ToString().Trim();
			}
			else
			{
				this.extcredits2.Enabled = false;
			}
			if (!dataRow[4].ToString().IsNullOrEmpty())
			{
				this.extcredits3name.Text = dataRow[4].ToString().Trim();
			}
			else
			{
				this.extcredits3.Enabled = false;
			}
			if (!dataRow[5].ToString().IsNullOrEmpty())
			{
				this.extcredits4name.Text = dataRow[5].ToString().Trim();
			}
			else
			{
				this.extcredits4.Enabled = false;
			}
			if (!dataRow[6].ToString().IsNullOrEmpty())
			{
				this.extcredits5name.Text = dataRow[6].ToString().Trim();
			}
			else
			{
				this.extcredits5.Enabled = false;
			}
			if (!dataRow[7].ToString().IsNullOrEmpty())
			{
				this.extcredits6name.Text = dataRow[7].ToString().Trim();
			}
			else
			{
				this.extcredits6.Enabled = false;
			}
			if (!dataRow[8].ToString().IsNullOrEmpty())
			{
				this.extcredits7name.Text = dataRow[8].ToString().Trim();
			}
			else
			{
				this.extcredits7.Enabled = false;
			}
			if (!dataRow[9].ToString().IsNullOrEmpty())
			{
				this.extcredits8name.Text = dataRow[9].ToString().Trim();
				return;
			}
			this.extcredits8.Enabled = false;
		}

		private void DeleteSet_Click(object sender, EventArgs e)
		{
			if (base.CheckCookie())
			{
				//Forums.UpdateForumField(DNTRequest.GetInt("fid", 0), Request["fieldname"], "''");
				var f = XForum.FindByID(Request["fid"].ToInt());
				f.Field.SetItem(Request["fieldname"], null);
				f.Field.Save();
				base.RegisterStartupScript("PAGE", "window.location.href='forum_editforums.aspx?fid=" + Request["fid"] + "&tabindex=1';");
			}
		}

		private void SaveInfo_Click(object sender, EventArgs e)
		{
			if (base.CheckCookie())
			{
				string fieldvalue = this.available.SelectedValue + "," + this.extcredits1.Text + "," + this.extcredits2.Text + "," + this.extcredits3.Text + "," + this.extcredits4.Text + "," + this.extcredits5.Text + "," + this.extcredits6.Text + "," + this.extcredits7.Text + "," + this.extcredits8.Text;
				//Forums.UpdateForumField(DNTRequest.GetInt("fid", 0), Request["fieldname"], fieldvalue);
				var f = XForum.FindByID(Request["fid"].ToInt());
				f.Field.SetItem(Request["fieldname"], fieldvalue);
				f.Field.Save();
				XCache.Remove(CacheKeys.FORUM_FORUM_LIST);
				base.RegisterStartupScript("PAGE", "window.location.href='forum_editforums.aspx?fid=" + Request["fid"] + "&tabindex=1';");
			}
		}

		protected override void OnInit(EventArgs e)
		{
			this.InitializeComponent();
			base.OnInit(e);
		}

		private void InitializeComponent()
		{
			this.SaveInfo.Click += new EventHandler(this.SaveInfo_Click);
			this.DeleteSet.Click += new EventHandler(this.DeleteSet_Click);
		}
	}
}