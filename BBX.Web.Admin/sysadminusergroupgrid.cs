using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using BBX.Entity;
using BBX.Forum;

namespace BBX.Web.Admin
{
    public class sysadminusergroupgrid : AdminPage
    {
        protected CheckBox chkConfirmInsert;
        protected CheckBox chkConfirmUpdate;
        protected CheckBox chkConfirmDelete;
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
            this.DataGrid1.TableHeaderName = "系统组列表";
            var list = new List<UserGroup>();
            foreach (var current in UserGroup.FindAllWithCache())
            {
                if (current.System == 1)
                {
                    list.Add(current);
                }
            }
            this.DataGrid1.BindData<UserGroup>(list);
        }

        protected string GetLink(string radminid, string groupid)
        {
            int num = int.Parse(radminid);
            if (num > 0 && num <= 3)
            {
                return "global_editadminusergroup.aspx?groupid=" + groupid;
            }
            return "global_editsysadminusergroup.aspx?groupid=" + groupid;
        }

        protected void Sort_Grid(object sender, DataGridSortCommandEventArgs e)
        {
            this.DataGrid1.Sort = e.SortExpression.ToString();
        }

        protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            this.DataGrid1.LoadCurrentPageIndex(e.NewPageIndex);
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