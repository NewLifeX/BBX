using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BBX.Common;
using BBX.Config;
using BBX.Entity;
using BBX.Forum;
using NewLife;

namespace BBX.Web.Admin
{
	public class identifymanage : AdminPage
	{
		protected HtmlForm Form1;
		protected BBX.Control.DataGrid identifygrid;
		protected BBX.Control.Button EditIdentify;
		protected BBX.Control.Button DelRec;
		protected Literal fileinfoList;
		protected BBX.Control.Button SubmitButton;
		private ArrayList fileList = new ArrayList();

		private void Page_Load(object sender, EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				this.BindData();
			}
			this.BindFilesList();
		}

		public void BindData()
		{
			this.identifygrid.AllowCustomPaging = false;
			this.identifygrid.TableHeaderName = "论坛鉴定列表";
			this.identifygrid.DataKeyField = "ID";
			//this.identifygrid.BindData(Identifys.GetAllIdentify());
			this.identifygrid.BindData(TopicIdentify.FindAllWithCache().ToDataTable(false));
		}

		protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
		{
			this.identifygrid.LoadCurrentPageIndex(e.NewPageIndex);
		}

		private void DataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
		{
			if (e.Item.ItemType != ListItemType.Item)
			{
				ListItemType arg_1B_0 = e.Item.ItemType;
			}
		}

		private void DelRec_Click(object sender, EventArgs e)
		{
			if (base.CheckCookie())
			{
				if (Request["id"] != "")
				{
					string id = Request["id"];
					//Identifys.DeleteIdentify(id);
					var ti = TopicIdentify.FindByID(id.ToInt());
					if (ti != null) ti.Delete();
					AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "鉴定文件删除", id);
					base.Response.Redirect("forum_identifymanage.aspx");
					return;
				}
				base.RegisterStartupScript("", "<script>alert('您未选中任何选项');window.location.href='forum_identifymanage.aspx';</script>");
			}
		}

		private void EditIdentify_Click(object sender, EventArgs e)
		{
			int num = 0;
			bool flag = true;
			foreach (object item in this.identifygrid.GetKeyIDArray())
			{
				//if (!Identifys.UpdateIdentifyById(int.Parse(item.ToString()), this.identifygrid.GetControlValue(num, "name")))
				//{
				//	flag = false;
				//}
				var ti = TopicIdentify.FindByID(item.ToInt());
				if (ti == null)
					flag = false;
				else
				{
					ti.Name = identifygrid.GetControlValue(num, "name");
					ti.Update();
				}
				num++;
			}
			AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "鉴定文件修改", "");
			if (!flag)
			{
				base.RegisterStartupScript("", "<script>alert('某些记录未能更新，因为与原有的记录名称相同');window.location.href='forum_identifymanage.aspx';</script>");
				return;
			}
			base.RegisterStartupScript("", "<script>window.location.href='forum_identifymanage.aspx';</script>");
		}

		public string PicStr(string filename, int size)
		{
			return "<img src='../../images/identify/" + filename + "'" + ((size != 0) ? " height='" + size + "px' width='" + size + "px'" : "") + " border='0' />";
		}

		public string PicStr(string filename)
		{
			return this.PicStr(filename, 0);
		}

		private ArrayList GetIdentifyFileList()
		{
			string strPath = BaseConfigs.GetForumPath + "images/identify/";
			DirectoryInfo directoryInfo = new DirectoryInfo(Utils.GetMapPath(strPath));
			if (!directoryInfo.Exists)
			{
				throw new IOException("鉴定图片文件夹不存在!");
			}
			FileInfo[] files = directoryInfo.GetFiles();
			ArrayList arrayList = new ArrayList();
			FileInfo[] array = files;
			for (int i = 0; i < array.Length; i++)
			{
				FileInfo fileInfo = array[i];
				if (!fileInfo.Name.Contains("_small."))
				{
					arrayList.Add(fileInfo.Name);
				}
			}
			return arrayList;
		}

		private void BindFilesList()
		{
			try
			{
				this.fileinfoList.Text = "";
				this.fileList = this.GetIdentifyFileList();
				//DataTable allIdentify = Identifys.GetAllIdentify();
				//foreach (DataRow dataRow in allIdentify.Rows)
				//{
				//	StateBag viewState;
				//	(viewState = this.ViewState)["code"] = viewState["code"] + "" + dataRow["name"] + ",";
				//	string obj = dataRow["filename"].ToString();
				//	this.fileList.Remove(obj);
				//}
				var list = TopicIdentify.FindAllWithCache();
				var sb = new StringBuilder();
				foreach (var item in list)
				{
					//if (sb.Length > 0) 
					sb.Append(",");
					sb.Append(item.Name);
					fileList.Remove(item.FileName);
				}
				ViewState["code"] += sb.ToString();
				this.fileList.Remove("Thumbs.db");
				int num = 1;
				foreach (string text in this.fileList)
				{
					Literal expr_E7 = this.fileinfoList;
					expr_E7.Text += "<tr class='mouseoutstyle' onmouseover='this.className=\"mouseoverstyle\"' onmouseout='this.className=\"mouseoutstyle\"' >\n";
					Literal expr_102 = this.fileinfoList;
					object text2 = expr_102.Text;
					expr_102.Text = text2 + "<td nowrap='nowrap' style='border-color:#EAE9E1;border-width:1px;border-style:solid;width:50px;' align='center'><input type='checkbox' id='id" + num + "' name='id" + num + "' value='" + num + "'/></td>\n";
					Literal expr_16C = this.fileinfoList;
					object text3 = expr_16C.Text;
					expr_16C.Text = text3 + "<td nowrap='nowrap' style='border-color:#EAE9E1;border-width:1px;border-style:solid;width:50px;' align='left'><input type='text' id='name" + num + "' name='name" + num + "' value='鉴定帖" + num + "' class=\"FormBase\" onfocus=\"this.className='FormFocus';\" onblur=\"this.className='FormBase';\" style='width:200px' /></td>\n";
					Literal expr_1D6 = this.fileinfoList;
					object text4 = expr_1D6.Text;
					expr_1D6.Text = text4 + "<td nowrap='nowrap' style='border-color:#EAE9E1;border-width:1px;border-style:solid;width:50px;'><input type='hidden' name='file" + num + "' value='" + text + "' /><div id='ilayer" + num + "' onmouseover='showMenu(this.id,false)'>" + this.PicStr(text, 20) + "</div>";
					Literal expr_255 = this.fileinfoList;
					object text5 = expr_255.Text;
					expr_255.Text = text5 + "<div id='ilayer" + num + "_menu' style='display:none'>" + this.PicStr(text) + "</div></td>\n";
					Literal expr_2AE = this.fileinfoList;
					expr_2AE.Text += "</tr>\n";
					num++;
				}
				this.SubmitButton.Visible = (this.fileList.Count != 0);
			}
			catch (IOException ex)
			{
				base.RegisterStartupScript("", "<script>alert('" + ex.Message + "');window.location.href='forum_identifymanage.aspx';</script>");
			}
		}

		public void SubmitButton_Click(object sender, EventArgs e)
		{
			bool flag = true;
			for (int i = 1; i <= this.fileList.Count; i++)
			{
				if (DNTRequest.GetFormString("id" + i) != "")
				{
					try
					{
						if (!TopicIdentify.Add(DNTRequest.GetString("name" + i), DNTRequest.GetString("file" + i)))
						{
							flag = false;
						}
					}
					catch
					{
						base.RegisterStartupScript("", "<script>alert('出现错误，可能名称超出长度！');window.location.href='forum_identifymanage.aspx';</script>");
					}
				}
			}
			AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "鉴定文件增加", "");
			if (!flag)
			{
				base.RegisterStartupScript("", "<script>alert('某些记录未能插入，因为与数据库中原有的名称相同');window.location.href='forum_identifymanage.aspx';</script>");
				return;
			}
			base.RegisterStartupScript("", "<script>window.location.href='forum_identifymanage.aspx';</script>");
		}

		protected override void OnInit(EventArgs e)
		{
			this.InitializeComponent();
			base.OnInit(e);
		}

		private void InitializeComponent()
		{
			this.DelRec.Click += new EventHandler(this.DelRec_Click);
			this.identifygrid.ItemDataBound += new DataGridItemEventHandler(this.DataGrid_ItemDataBound);
			this.EditIdentify.Click += new EventHandler(this.EditIdentify_Click);
			this.SubmitButton.Click += new EventHandler(this.SubmitButton_Click);
			this.SubmitButton.Attributes.Add("onclick", "return validate()");
			this.identifygrid.ColumnSpan = 3;
		}
	}
}