using System;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BBX.Common;
using BBX.Entity;

namespace BBX.Web.Admin
{
	public class onlinelistgrid : AdminPage
	{
		protected HtmlForm Form1;
		protected pageinfo info1;
		protected BBX.Control.DataGrid DataGrid1;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				this.BindData();
			}
		}

		public void BindData()
		{
			this.DataGrid1.AllowCustomPaging = false;
			this.DataGrid1.TableHeaderName = "在线列表定制";
			this.DataGrid1.DataSource = this.LoadDataInfo();
			this.DataGrid1.DataBind();
		}

		private DataSet LoadDataInfo()
		{
			DataSet dataSet = new DataSet();
			//DataTable onlineList = UserGroups.GetOnlineList();
			DataTable onlineList = OnlineList.FindAllWithCache().ToDataTable(false);
			dataSet.Tables.Add(onlineList.Clone());
			foreach (DataRow row in onlineList.Rows)
			{
				dataSet.Tables[0].ImportRow(row);
			}
			dataSet.Tables[0].Columns.Add("newdisplayorder");
			dataSet.Tables[0].Rows[0]["Title"] = "普通用户";
			foreach (DataRow dataRow in dataSet.Tables[0].Rows)
			{
				if (!Utils.IsNumeric(dataRow["displayorder"].ToString()) || dataRow["displayorder"].ToString() == "0")
				{
					dataRow["newdisplayorder"] = "不显示";
				}
				else
				{
					dataRow["newdisplayorder"] = dataRow["displayorder"].ToString();
				}
			}
			DataTable dataTable = new DataTable("img");
			dataTable.Columns.Add("imgfile", typeof(String));
			DataRow dataRow2 = dataTable.NewRow();
			dataRow2["imgfile"] = "";
			dataTable.Rows.Add(dataRow2);
			try
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(base.Server.MapPath("../../images/groupicons"));
				FileSystemInfo[] fileSystemInfos = directoryInfo.GetFileSystemInfos();
				for (int i = 0; i < fileSystemInfos.Length; i++)
				{
					FileSystemInfo fileSystemInfo = fileSystemInfos[i];
					if (fileSystemInfo != null && fileSystemInfo.Extension != "")
					{
						dataRow2 = dataTable.NewRow();
						if (!(fileSystemInfo.Name.ToLower() == "thumbs.db"))
						{
							dataRow2["imgfile"] = fileSystemInfo.Name;
							dataTable.Rows.Add(dataRow2);
						}
					}
				}
				dataSet.Tables.Add(dataTable);
				dataSet.Relations.Add(dataTable.Columns["imgfile"], dataSet.Tables[0].Columns["img"]);
			}
			catch
			{
			}
			return dataSet;
		}

		protected void DataGrid_Update(object sender, DataGridCommandEventArgs E)
		{
			int groupid = this.DataGrid1.DataKeys[E.Item.ItemIndex].ToString().ToInt(0);
			int num = ((TextBox)E.Item.Cells[2].Controls[0]).Text.ToInt(0);
			if (num < 0) num = 0;

			string text = ((TextBox)E.Item.Cells[3].Controls[0]).Text;
			string selectedValue = ((DropDownList)E.Item.FindControl("imgdropdownlist")).SelectedValue;
			AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "在线列表定制", text);
			try
			{
				//UserGroups.UpdateOnlineList(groupid, num, selectedValue, text);
				var gp = OnlineList.FindByGroupID(groupid);
				gp.DisplayOrder = num;
				gp.Title = text;
				gp.Img = selectedValue;
				gp.Update();

				this.BindData();
				//XCache.Remove(CacheKeys.FORUM_UI_ONLINE_ICON_LIST);
				base.RegisterStartupScript("PAGE", "window.location.href='global_onlinelistgrid.aspx';");
			}
			catch
			{
				base.RegisterStartupScript("", "<script>alert('无法更新数据库');window.location.href='global_onlinelistgrid.aspx';</script>");
			}
		}

		private void DataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.EditItem)
			{
				DropDownList dropDownList = (DropDownList)e.Item.FindControl("imgdropdownlist");
				dropDownList.Items.Clear();
				foreach (DataRow dataRow in this.LoadDataInfo().Tables[1].Rows)
				{
					dropDownList.Items.Add(new ListItem(dataRow[0].ToString(), dataRow[0].ToString()));
				}
				dropDownList.DataBind();
				try
				{
					dropDownList.SelectedValue = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "img"));
				}
				catch
				{
				}
				TextBox textBox = (TextBox)e.Item.Cells[2].Controls[0];
				textBox.Attributes.Add("maxlength", "6");
				textBox.Attributes.Add("size", "5");
				if (!Utils.IsNumeric(textBox.Text))
				{
					textBox.Text = "0";
				}
				textBox = (TextBox)e.Item.Cells[3].Controls[0];
				textBox.Attributes.Add("maxlength", "50");
				textBox.Attributes.Add("size", "30");
			}
		}

		protected void Sort_Grid(object sender, DataGridSortCommandEventArgs e)
		{
			this.DataGrid1.Sort = e.SortExpression.ToString();
			this.BindData();
		}

		protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
		{
			this.DataGrid1.CurrentPageIndex = e.NewPageIndex;
			this.BindData();
		}

		protected void DataGrid_Edit(object sender, DataGridCommandEventArgs E)
		{
			this.DataGrid1.EditItemIndex = E.Item.ItemIndex;
			this.BindData();
		}

		protected void DataGrid_Cancel(object sender, DataGridCommandEventArgs E)
		{
			this.DataGrid1.EditItemIndex = -1;
			this.BindData();
		}

		public string GetImgLink(string img)
		{
			if (img != "")
			{
				return "<img src=../../images/groupicons/" + img + " height=16px width=16px border=0 />";
			}
			return "";
		}

		protected override void OnInit(EventArgs e)
		{
			this.InitializeComponent();
			base.OnInit(e);
		}

		private void InitializeComponent()
		{
			this.DataGrid1.EditCommand += new DataGridCommandEventHandler(this.DataGrid_Edit);
			this.DataGrid1.CancelCommand += new DataGridCommandEventHandler(this.DataGrid_Cancel);
			this.DataGrid1.ItemDataBound += new DataGridItemEventHandler(this.DataGrid_ItemDataBound);
			this.DataGrid1.UpdateCommand += new DataGridCommandEventHandler(this.DataGrid_Update);
			this.DataGrid1.LoadEditColumn();
			this.DataGrid1.TableHeaderName = "在线列表";
			this.DataGrid1.DataKeyField = "groupid";
			this.DataGrid1.ColumnSpan = 5;
			this.DataGrid1.SaveDSViewState = true;
		}
	}
}