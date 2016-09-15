using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web.Admin
{
    public class urlgrid : AdminPage
    {
        public DataSet dsSrc = new DataSet();
        protected HtmlForm Form1;
        protected pageinfo info1;
        protected pageinfo PageInfo1;
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
            this.DataGrid1.DataKeyField = "name";
            this.DataGrid1.TableHeaderName = "伪静态url的替换规则";
            this.dsSrc.ReadXml(base.Server.MapPath("../../config/urls.config"));
            this.DataGrid1.DataSource = this.dsSrc.Tables[0];
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
            this.DataGrid1.EditItemIndex = E.Item.ItemIndex;
            this.dsSrc.Reset();
            this.dsSrc.ReadXml(base.Server.MapPath("../../config/urls.config"));
            this.DataGrid1.DataSource = this.dsSrc.Tables[0];
            this.DataGrid1.DataBind();
        }

        protected void DataGrid_Cancel(object sender, DataGridCommandEventArgs E)
        {
            this.DataGrid1.EditItemIndex = -1;
            this.dsSrc.Reset();
            this.dsSrc.ReadXml(base.Server.MapPath("../../config/urls.config"));
            this.DataGrid1.DataSource = this.dsSrc.Tables[0];
            this.DataGrid1.DataBind();
        }

        protected void DataGrid_Update(object sender, DataGridCommandEventArgs E)
        {
            string text = ((System.Web.UI.WebControls.TextBox)E.Item.FindControl("nametext")).Text;
            string text2 = ((System.Web.UI.WebControls.TextBox)E.Item.Cells[2].Controls[0]).Text;
            string text3 = ((System.Web.UI.WebControls.TextBox)E.Item.Cells[3].Controls[0]).Text;
            string text4 = ((System.Web.UI.WebControls.TextBox)E.Item.Cells[4].Controls[0]).Text;
            string text5 = ((System.Web.UI.WebControls.TextBox)E.Item.Cells[5].Controls[0]).Text;
            this.dsSrc.Reset();
            this.dsSrc.ReadXml(base.Server.MapPath("../../config/urls.config"));
            foreach (DataRow dataRow in this.dsSrc.Tables["rewrite"].Rows)
            {
                if (text == dataRow["name"].ToString().Trim())
                {
                    dataRow["path"] = text2;
                    dataRow["pattern"] = text3;
                    dataRow["page"] = text4;
                    dataRow["querystring"] = text5;
                }
            }
            try
            {
                this.dsSrc.WriteXml(base.Server.MapPath("../../config/urls.config"));
                this.dsSrc.Reset();
                this.dsSrc.Dispose();
                SiteUrls.Current = null;
                AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "Url设置", "");
                this.DataGrid1.EditItemIndex = -1;
                this.dsSrc.Reset();
                this.dsSrc.ReadXml(base.Server.MapPath("../../config/urls.config"));
                this.DataGrid1.DataSource = this.dsSrc.Tables[0];
                this.DataGrid1.DataBind();
            }
            catch
            {
                base.RegisterStartupScript("", "<script>alert('无法更新数据库.');window.location.href='global_urlgrid.aspx';</script>");
            }
        }

        private void DataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.EditItem)
            {
                System.Web.UI.WebControls.TextBox textBox = (System.Web.UI.WebControls.TextBox)e.Item.FindControl("nametext");
                textBox.Attributes.Add("size", "5");
                textBox.ReadOnly = true;
                textBox.Text = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "name"));
                for (int i = 0; i < this.DataGrid1.Columns.Count; i++)
                {
                    if (i >= 2)
                    {
                        System.Web.UI.WebControls.TextBox textBox2 = (System.Web.UI.WebControls.TextBox)e.Item.Cells[i].Controls[0];
                        textBox2.Width = 120;
                    }
                }
            }
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
            this.DataGrid1.DataKeyField = "name";
            this.DataGrid1.AllowSorting = false;
            this.DataGrid1.AllowPaging = false;
            this.DataGrid1.ShowFooter = false;
        }
    }
}