using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BBX.Common;
using BBX.Forum;
using BBX.Entity;

namespace BBX.Web.Admin
{
    public class bbcodegrid : AdminPage
    {
        protected HtmlForm Form1;
        protected pageinfo info1;
        protected BBX.Control.DataGrid DataGrid1;
        protected BBX.Control.Button DelRec;
        protected BBX.Control.Button SetAvailable;
        protected BBX.Control.Button SetUnAvailable;

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
            this.DataGrid1.DataSource = BbCode.FindAllWithCache();
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

        private void DelRec_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                if (Request["id"] != "")
                {
                    BbCode.DeleteBBCode(Request["id"]);
                    base.Response.Redirect("forum_bbcodegrid.aspx");
                    return;
                }
                base.RegisterStartupScript("", "<script>alert('您未选中任何选项');window.location.href='forum_bbcodegrid.aspx';</script>");
            }
        }

        public string BoolStr(string closed)
        {
            if (closed == "1")
            {
                return "<div align=center><img src=../images/OK.gif /></div>";
            }
            return "<div align=center><img src=../images/Cancel.gif /></div>";
        }

        private void SetAvailable_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                if (Request["id"] != "")
                {
                    BbCode.BatchUpdateAvailable(1, Request["id"]);
                    base.Response.Redirect("forum_bbcodegrid.aspx");
                    return;
                }
                base.RegisterStartupScript("", "<script>alert('您未选中任何选项');window.location.href='forum_bbcodegrid.aspx';</script>");
            }
        }

        private void SetUnAvailable_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                if (Request["id"] != "")
                {
                    BbCode.BatchUpdateAvailable(0, Request["id"]);
                    base.Response.Redirect("forum_bbcodegrid.aspx");
                    return;
                }
                base.RegisterStartupScript("", "<script>alert('您未选中任何选项');window.location.href='forum_bbcodegrid.aspx';</script>");
            }
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.DelRec.Click += new EventHandler(this.DelRec_Click);
            this.SetAvailable.Click += new EventHandler(this.SetAvailable_Click);
            this.SetUnAvailable.Click += new EventHandler(this.SetUnAvailable_Click);
            this.DataGrid1.TableHeaderName = Utils.ProductName + "代码列表";
            this.DataGrid1.ColumnSpan = 6;
        }
    }
}