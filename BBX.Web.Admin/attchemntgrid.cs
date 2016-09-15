using System;
using System.Data;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BBX.Common;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web.Admin
{
    public class attchemntgrid : AdminPage
    {
        protected HtmlForm Form1;
        protected BBX.Control.DataGrid DataGrid1;
        protected BBX.Control.Button DeleteAttachment;
        protected BBX.Control.Button DeleteAll;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                if (this.Session["attchmentwhere"] == null)
                {
                    base.Response.Redirect("forum_searchattchment.aspx");
                    return;
                }
                this.ViewState["condition"] = this.Session["attchmentwhere"].ToString();
                this.BindData();
            }
        }

        public void BindData()
        {
            this.DataGrid1.AllowCustomPaging = false;
            this.DataGrid1.TableHeaderName = "附件列表";
            this.DataGrid1.BindData(Attachment.GetAttachList(this.ViewState["condition"].ToString()));
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
            if (e.Item.Cells[5].Text.ToString().Length > 15)
            {
                e.Item.Cells[5].Text = e.Item.Cells[5].Text.Substring(0, 15) + "…";
            }
        }

        //private bool DeleteFile(string filename)
        //{
        //	if (filename.ToLower().Contains("http://"))
        //	{
        //		return true;
        //	}
        //	if (Utils.FileExists(Utils.GetMapPath("..\\..\\upload\\" + filename)))
        //	{
        //		File.Delete(Utils.GetMapPath("..\\..\\upload\\" + filename));
        //		return true;
        //	}
        //	return false;
        //}

        private void DeleteAttachment_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                if (Request["aid"] != "")
                {
                    string ids = Request["aid"];
                    //DataTable attachList = Attachments.GetAttachList(this.ViewState["condition"].ToString(), TableList.CurrentTableName);
                    //DataRow[] array = attachList.Select("aid IN(" + ids + ")");
                    //for (int i = 0; i < array.Length; i++)
                    //{
                    //	DataRow dataRow = array[i];
                    //	this.DeleteFile(dataRow["filename"].ToString());
                    //}
                    //Attachments.DeleteAttachment(ids);
                    var list = Attachment.FindAllByIDs(ids);
                    list.Delete();
                    AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "删除附件", "ID:" + ids);
                    base.RegisterStartupScript("PAGE", "window.location.href='forum_searchattchment.aspx';");
                    return;
                }
                base.RegisterStartupScript("", "<script>alert('您未选中任何选项');window.location.href='forum_searchattchment.aspx';</script>");
            }
        }

        private void DeleteAll_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                var attachList = Attachment.GetAttachList(this.ViewState["condition"].ToString());
                var text = attachList.Join("ID", ",");
                attachList.Delete();
                //string text = "0";
                //foreach (DataRow dataRow in attachList.Rows)
                //{
                //	//this.DeleteFile(dataRow["filename"].ToString());
                //	text = text + "," + dataRow["aid"].ToString();
                //}
                ////Attachments.DeleteAttachment(text);
                //var list = Attachment.FindAllByIDs(text);
                //list.Delete();
                AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "删除附件", "ID:" + text);
                base.RegisterStartupScript("PAGE", "window.location.href='forum_searchattchment.aspx';");
            }
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.DeleteAttachment.Click += new EventHandler(this.DeleteAttachment_Click);
            this.DeleteAll.Click += new EventHandler(this.DeleteAll_Click);
            this.DataGrid1.ItemDataBound += new DataGridItemEventHandler(this.DataGrid_ItemDataBound);
            this.DataGrid1.TableHeaderName = "附件列表";
            this.DataGrid1.ColumnSpan = 7;
            this.DataGrid1.DataKeyField = "ID";
        }
    }
}