using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Discuz.Entity;
using Discuz.Forum;

namespace Discuz.Web.Admin
{
    public class adminusergroupgrid : AdminPage
    {
        protected HtmlForm Form1;
        protected pageinfo info1;
        protected Discuz.Control.DataGrid DataGrid1;

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
            this.DataGrid1.BindData<UserGroup>(Discuz.Entity.UserGroup.GetAdminUserGroup());
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
            this.DataGrid1.DataKeyField = "groupid";
            this.DataGrid1.ColumnSpan = 12;
        }
    }
}