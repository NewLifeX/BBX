using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BBX.Common;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web.Admin
{
	public class allowparticipatescore : AdminPage
	{
		protected DataTable templateDT = new DataTable("templateDT");
		protected HtmlForm Form1;
		protected pageinfo info1;
		protected pageinfo PageInfo1;
		protected BBX.Control.DataGrid DataGrid1;
		protected BBX.Control.Button SetAvailable;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				if (Request["groupid"] != "")
				{
					this.BindData();
					return;
				}
				base.Response.Write("<script>history.go(-1);</script>");
				base.Response.End();
			}
		}

		public void BindData()
		{
			this.DataGrid1.AllowCustomPaging = false;
			this.DataGrid1.TableHeaderName = "允许评分范围列表";
			this.DataGrid1.DataSource = this.LoadDataInfo();
			this.DataGrid1.DataBind();
		}

		protected void Sort_Grid(object sender, DataGridSortCommandEventArgs e)
		{
			this.DataGrid1.Sort = e.SortExpression.ToString();
		}

		protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
		{
			this.DataGrid1.LoadCurrentPageIndex(e.NewPageIndex);
			this.BindData();
		}

		protected void DataGrid_Edit(object sender, DataGridCommandEventArgs E)
		{
			if (this.ViewState["validrow"].ToString().IndexOf("," + E.Item.ItemIndex + ",") >= 0)
			{
				this.DataGrid1.EditItemIndex = E.Item.ItemIndex;
				this.DataGrid1.DataSource = this.LoadDataInfo();
				this.DataGrid1.DataBind();
				return;
			}
			base.RegisterStartupScript("", this.GetMessageScript("操作失败,您所修改的积分行是无效的,具体操作请看注释!"));
		}

		protected void DataGrid_Cancel(object sender, DataGridCommandEventArgs e)
		{
			this.DataGrid1.EditItemIndex = -1;
			this.DataGrid1.DataSource = this.LoadDataInfo();
			this.DataGrid1.DataBind();
		}

		private DataTable LoadDataInfo()
		{
			this.templateDT = UserGroups.GroupParticipateScore(DNTRequest.GetInt("groupid", 0));
			DataRow dataRow = Scoresets.GetScoreSet().Rows[0];
			string text = "";
			for (int i = 0; i < 8; i++)
			{
				if (!dataRow[i + 2].ToString().IsNullOrEmpty() && dataRow[i + 2].ToString().Trim() != "0")
				{
					this.templateDT.Rows[i]["ScoreName"] = dataRow[i + 2].ToString().Trim();
					text = text + "," + i;
				}
				if (this.IsValidScoreName(i + 1))
				{
					text = text + "," + i;
				}
			}
			this.ViewState["validrow"] = text + ",";
			return this.templateDT;
		}

		public bool IsValidScoreName(int scoreid)
		{
			bool result = false;
			foreach (DataRow dataRow in Scoresets.GetScoreSet().Rows)
			{
				if (dataRow["id"].ToString() != "1" && dataRow["id"].ToString() != "2" && dataRow[scoreid + 1].ToString().Trim() != "0")
				{
					result = true;
					break;
				}
			}
			return result;
		}

		protected void DataGrid_Update(object sender, DataGridCommandEventArgs e)
		{
			string value = this.DataGrid1.DataKeys[e.Item.ItemIndex].ToString();
			bool @checked = ((CheckBox)e.Item.FindControl("available")).Checked;
			string text = ((System.Web.UI.WebControls.TextBox)e.Item.Cells[5].Controls[0]).Text.Trim();
			string text2 = ((System.Web.UI.WebControls.TextBox)e.Item.Cells[6].Controls[0]).Text.Trim();
			string text3 = ((System.Web.UI.WebControls.TextBox)e.Item.Cells[7].Controls[0]).Text.Trim();
			this.LoadDataInfo();
			int index = (int)(Convert.ToInt16(value) - 1);
			this.templateDT.Rows[index]["available"] = @checked;
			if (String.IsNullOrEmpty(text) || String.IsNullOrEmpty(text2) || String.IsNullOrEmpty(text3))
			{
				base.RegisterStartupScript("", this.GetMessageScript("评分的最小值,最大值以及24小时最大评分数不能为空."));
				return;
			}
			if ((text != "" && !Utils.IsNumeric(text.Replace("-", ""))) || (text2 != "" && !Utils.IsNumeric(text2.Replace("-", ""))) || (text3 != "" && !Utils.IsNumeric(text3.Replace("-", ""))))
			{
				base.RegisterStartupScript("", this.GetMessageScript("输入的数据必须是数字."));
				return;
			}
			if (Convert.ToInt16(Utils.SBCCaseToNumberic(text)) >= Convert.ToInt16(Utils.SBCCaseToNumberic(text2)))
			{
				base.RegisterStartupScript("", this.GetMessageScript("评分的最小值必须小于评分最大值."));
				return;
			}
			this.templateDT.Rows[index]["Min"] = Convert.ToInt16(Utils.SBCCaseToNumberic(text));
			this.templateDT.Rows[index]["Max"] = Convert.ToInt16(Utils.SBCCaseToNumberic(text2));
			this.templateDT.Rows[index]["MaxInDay"] = Convert.ToInt16(Utils.SBCCaseToNumberic(text3));
			try
			{
				this.WriteScoreInf(this.templateDT);
				this.DataGrid1.EditItemIndex = -1;
				this.DataGrid1.DataSource = this.LoadDataInfo();
				this.DataGrid1.DataBind();
				base.RegisterStartupScript("PAGE", "window.location.href='global_allowparticipatescore.aspx?pagename=" + Request["pagename"] + "&groupid=" + Request["groupid"] + "';");
			}
			catch
			{
				base.RegisterStartupScript("", this.GetMessageScript("无法更新数据库."));
			}
		}

		private string GetMessageScript(string message)
		{
			return string.Format("<script>alert('{0}');window.location.href='global_allowparticipatescore.aspx?pagename={1}&groupid={2}';</script>", message, Request["pagename"], Request["groupid"]);
		}

		public void WriteScoreInf(DataTable dt)
		{
			string text = "";
			foreach (DataRow dataRow in dt.Rows)
			{
				text += string.Format("{0},{1},{2},{3},{4},{5},{6}|", new object[]
                {
                    dataRow["id"].ToString(),
                    dataRow["available"].ToString(),
                    dataRow["ScoreCode"].ToString(),
                    dataRow["ScoreName"].ToString(),
                    dataRow["Min"].ToString(),
                    dataRow["Max"].ToString(),
                    dataRow["MaxInDay"].ToString()
                });
			}
			//UserGroup.UpdateUserGroupRaterange(text.Substring(0, text.Length - 1), DNTRequest.GetInt("groupid", 0));
			var ug = UserGroup.FindByID(DNTRequest.GetInt("groupid", 0));
			ug.Raterange = text.Substring(0, text.Length - 1);
			ug.Update();

			this.templateDT.Clear();
			//Caches.ReSetUserGroupList();
		}

		public bool GetAvailable(string available)
		{
			return available == "True";
		}

		public string GetImgLink(string available)
		{
			if (!(available == "True"))
			{
				return "<div align=center><img src=../images/Cancel.gif /></div>";
			}
			return "<div align=center><img src=../images/OK.gif /></div>";
		}

		private void DataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.EditItem)
			{
				System.Web.UI.WebControls.TextBox textBox = (System.Web.UI.WebControls.TextBox)e.Item.Cells[5].Controls[0];
				textBox.Attributes.Add("maxlength", "3");
				textBox.Attributes.Add("size", "4");
				textBox = (System.Web.UI.WebControls.TextBox)e.Item.Cells[6].Controls[0];
				textBox.Attributes.Add("maxlength", "3");
				textBox.Attributes.Add("size", "4");
				textBox = (System.Web.UI.WebControls.TextBox)e.Item.Cells[7].Controls[0];
				textBox.Attributes.Add("maxlength", "4");
				textBox.Attributes.Add("size", "4");
			}
		}

		protected override void OnInit(EventArgs e)
		{
			this.InitializeComponent();
			base.OnInit(e);
		}

		private void InitializeComponent()
		{
			this.DataGrid1.ItemDataBound += new DataGridItemEventHandler(this.DataGrid_ItemDataBound);
			this.DataGrid1.LoadEditColumn();
		}
	}
}