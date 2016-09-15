using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web.Admin
{
    public partial class adminusergroupgrid : AdminPage
    {
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
            this.DataGrid1.TableHeaderName = "管理组列表";
            this.DataGrid1.Attributes.Add("borderStyle", "2");
            this.DataGrid1.BindData<UserGroup>(UserGroup.FindAll管理组());
        }

        protected void Sort_Grid(object sender, DataGridSortCommandEventArgs e)
        {
            this.DataGrid1.Sort = e.SortExpression;
        }

        protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            this.DataGrid1.LoadCurrentPageIndex(e.NewPageIndex);
            this.BindData();
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.DataGrid1.DataKeyField = "ID";
            this.DataGrid1.ColumnSpan = 12;
        }
    }
}