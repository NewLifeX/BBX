using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BBX.Control;
using BBX.Entity;
using BBX.Forum;
using NewLife;

namespace BBX.Web.Admin
{
	public class attachtypesgrid : AdminPage
	{
		protected HtmlForm Form1;
		protected pageinfo info1;
		protected pageinfo info2;
		protected BBX.Control.DataGrid DataGrid1;
		protected BBX.Control.Button SaveAttachType;
		protected BBX.Control.Button DelRec;
		protected BBX.Control.TextBox extension;
		protected BBX.Control.TextBox maxsize;
		protected BBX.Control.Button AddNewRec;
		protected Hint Hint1;

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
			this.DataGrid1.TableHeaderName = "上传附件类型列表";
			this.DataGrid1.BindData(AttachType.FindAllWithCache());
		}

		protected void Sort_Grid(object sender, DataGridSortCommandEventArgs e)
		{
			this.DataGrid1.Sort = e.SortExpression.ToString();
		}

		protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
		{
			this.DataGrid1.LoadCurrentPageIndex(e.NewPageIndex);
		}

		private void DataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.EditItem)
			{
				System.Web.UI.WebControls.TextBox textBox = (System.Web.UI.WebControls.TextBox)e.Item.Cells[3].Controls[0];
				textBox.Attributes.Add("maxlength", "255");
				textBox.Attributes.Add("size", "30");
				textBox = (System.Web.UI.WebControls.TextBox)e.Item.Cells[4].Controls[0];
				textBox.Attributes.Add("maxlength", "9");
				textBox.Attributes.Add("size", "10");
			}
			if ((e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item) && e.Item.Cells[3].Text.ToString().Length > 40)
			{
				e.Item.Cells[3].Text = e.Item.Cells[3].Text.Substring(0, 40) + "…";
			}
		}

		private void DelRec_Click(object sender, EventArgs e)
		{
			if (base.CheckCookie())
			{
				if (Request["id"] != "")
				{
					//Attachments.DeleteAttchType(Request["id"]);
					AttachType.FindAllByIDs(Request["id"]).Delete();
					AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "删除附件类型", "删除附件类型,ID为:" + Request["id"].Replace("0 ", ""));
					base.Response.Redirect("forum_attachtypesgrid.aspx");
					return;
				}
				base.RegisterStartupScript("", "<script>alert('您未选中任何选项');window.location.href='forum_attachtypesgrid.aspx';</script>");
			}
		}

		private void AddNewRec_Click(object sender, EventArgs e)
		{
			if (String.IsNullOrEmpty(this.extension.Text))
			{
				base.RegisterStartupScript("", "<script>alert('要添加的附件扩展名不能为空');window.location.href='forum_attachtypesgrid.aspx';</script>");
				return;
			}
			if (String.IsNullOrEmpty(this.maxsize.Text) || this.maxsize.Text.ToInt() <= 0)
			{
				base.RegisterStartupScript("", "<script>alert('要添加的附件最大尺寸不能为空且要大于0');window.location.href='forum_attachtypesgrid.aspx';</script>");
				return;
			}
			foreach (var at in AttachType.FindAllWithCache())
			{
				if (at.Extension.EqualIgnoreCase(this.extension.Text))
				{
					base.RegisterStartupScript("", "<script>alert('数据库中已存在相同的附件扩展名');window.location.href='forum_attachtypesgrid.aspx';</script>");
					return;
				}
			}
			AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "添加附件类型", "添加附件类型,扩展名为:" + this.extension.Text);
			try
			{
				//Attachments.AddAttchType(this.extension.Text, this.maxsize.Text);
				AttachType.Add(extension.Text, maxsize.Text.ToInt());
				
				base.RegisterStartupScript("PAGE", "window.location.href='forum_attachtypesgrid.aspx';");
			}
			catch
			{
				base.RegisterStartupScript("", "<script>alert('无法更新数据库.');window.location.href='forum_attachtypesgrid.aspx';</script>");
			}
		}

		private void SaveAttachType_Click(object sender, EventArgs e)
		{
			int num = 0;
			bool flag = false;
			foreach (object current in this.DataGrid1.GetKeyIDArray())
			{
				string text = this.DataGrid1.GetControlValue(num, "extension").Trim();
				string text2 = this.DataGrid1.GetControlValue(num, "maxsize").Trim();
				int num2;
				if (String.IsNullOrEmpty(text) || String.IsNullOrEmpty(text2) || !int.TryParse(text2, out num2))
				{
					flag = true;
				}
				else
				{
					//Attachments.UpdateAttchType(text, text2, int.Parse(current.ToString()));
					var at = AttachType.FindByID(current.ToInt());
					if (at != null)
					{
						at.Extension = text;
						at.MaxSize = text2.ToInt();
						at.Save();
					}
					AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "编辑附件类型", "编辑附件类型,扩展名为:" + text);
					num++;
				}
			}
			//XCache.Remove(CacheKeys.FORUM_FORUM_SETTING_ATTACHMENT_TYPE);
			if (flag)
			{
				base.RegisterStartupScript("", "<script>alert('某些记录取值不正确，未能被更新！');window.location.href='forum_attachtypesgrid.aspx';</script>");
				return;
			}
			base.RegisterStartupScript("PAGE", "window.location.href='forum_attachtypesgrid.aspx';");
		}

		protected override void OnInit(EventArgs e)
		{
			this.InitializeComponent();
			base.OnInit(e);
		}

		private void InitializeComponent()
		{
			this.AddNewRec.Click += new EventHandler(this.AddNewRec_Click);
			this.DelRec.Click += new EventHandler(this.DelRec_Click);
			this.DataGrid1.ItemDataBound += new DataGridItemEventHandler(this.DataGrid_ItemDataBound);
			this.SaveAttachType.Click += new EventHandler(this.SaveAttachType_Click);
			this.DataGrid1.ColumnSpan = 4;
		}
	}
}