using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BBX.Common;
using BBX.Control;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web.Admin
{
	public class ratescorelog : AdminPage
	{
		protected HtmlForm Form1;
		protected Hint Hint1;
		protected BBX.Control.Calendar postdatetimeStart;
		protected BBX.Control.Calendar postdatetimeEnd;
		protected BBX.Control.TextBox others;
		protected BBX.Control.TextBox Username;
		protected BBX.Control.Button SearchLog;
		protected BBX.Control.DataGrid DataGrid1;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				this.postdatetimeStart.SelectedDate = DateTime.Now.AddDays(-30.0);
				this.postdatetimeEnd.SelectedDate = DateTime.Now;
				this.BindData();
			}
		}

		public void BindData()
		{
			this.DataGrid1.AllowCustomPaging = true;
			this.DataGrid1.VirtualItemCount = this.GetRecordCount();
			var index = this.DataGrid1.CurrentPageIndex + 1;
			var size = this.DataGrid1.PageSize;
			//var where = this.ViewState["condition"] + "";
			////if (this.ViewState["condition"] == null)
			////if (!where.IsNullOrWhiteSpace())
			//{
			//	//this.DataGrid1.DataSource = AdminRateLogs.GetRateLogList(this.DataGrid1.PageSize, this.DataGrid1.CurrentPageIndex + 1);
			//	this.DataGrid1.DataSource = RateLog.FindAll(where, null, null, (index - 1) * size, size);
			//}
			////else
			////{
			////	this.DataGrid1.DataSource = AdminRateLogs.LogList(this.DataGrid1.PageSize, this.DataGrid1.CurrentPageIndex + 1, this.ViewState["condition"].ToString());
			////}
			this.DataGrid1.DataSource = RateLog.Search(Username.Text, postdatetimeStart.SelectedDate, postdatetimeEnd.SelectedDate, others.Text, null, (index - 1) * size, size);
			this.DataGrid1.DataBind();
		}

		public string ExtcreditsStr(string extcredits, string score)
		{
			DataRow dataRow = Scoresets.GetScoreSet().Rows[0];
			string result = "";
			switch (extcredits)
			{
				case "1":
					result = dataRow["extcredits1"].ToString() + " " + score;
					break;
				case "2":
					result = dataRow["extcredits2"].ToString() + " " + score;
					break;
				case "3":
					result = dataRow["extcredits3"].ToString() + " " + score;
					break;
				case "4":
					result = dataRow["extcredits4"].ToString() + " " + score;
					break;
				case "5":
					result = dataRow["extcredits5"].ToString() + " " + score;
					break;
				case "6":
					result = dataRow["extcredits6"].ToString() + " " + score;
					break;
				case "7":
					result = dataRow["extcredits7"].ToString() + " " + score;
					break;
				case "8":
					result = dataRow["extcredits8"].ToString() + " " + score;
					break;
			}
			return result;
		}

		private void SearchLog_Click(object sender, EventArgs e)
		{
			if (base.CheckCookie())
			{
				//string searchRateLogCondition = AdminRateLogs.GetSearchRateLogCondition(this.postdatetimeStart.SelectedDate, this.postdatetimeEnd.SelectedDate, this.Username.Text, this.others.Text);
				//this.ViewState["condition"] = searchRateLogCondition;
				this.DataGrid1.CurrentPageIndex = 0;
				this.BindData();
			}
		}

		private int GetRecordCount()
		{
			//if (this.ViewState["condition"] == null)
			//{
			//	//return AdminRateLogs.RecordCount();
			//	return RateLog.FindCount();
			//}
			//return AdminRateLogs.RecordCount(this.ViewState["condition"].ToString());

			return RateLog.SearchCount(Username.Text, postdatetimeStart.SelectedDate, postdatetimeEnd.SelectedDate, others.Text);
		}

		protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
		{
			this.DataGrid1.CurrentPageIndex = e.NewPageIndex;
			this.BindData();
		}

		public void GoToPagerButton_Click(object sender, EventArgs e)
		{
			this.BindData();
		}

		private void DataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
		{
			if (e.Item.Cells[9].Text.ToString().Length > 8)
			{
				e.Item.Cells[9].Text = Utils.HtmlEncode(e.Item.Cells[9].Text.Substring(0, 8)) + "…";
			}
		}

		protected override void OnInit(EventArgs e)
		{
			this.InitializeComponent();
			base.OnInit(e);
		}

		private void InitializeComponent()
		{
			this.SearchLog.Click += new EventHandler(this.SearchLog_Click);
			this.DataGrid1.GoToPagerButton.Click += new EventHandler(this.GoToPagerButton_Click);
			this.DataGrid1.ItemDataBound += new DataGridItemEventHandler(this.DataGrid_ItemDataBound);
			this.DataGrid1.TableHeaderName = "用户评分日志";
			this.DataGrid1.AllowSorting = false;
			this.DataGrid1.ColumnSpan = 8;
		}
	}
}