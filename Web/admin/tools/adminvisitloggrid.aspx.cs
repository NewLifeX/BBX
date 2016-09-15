using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BBX.Common;
using BBX.Control;
using BBX.Entity;

namespace BBX.Web.Admin
{
    public partial class adminvisitloggrid : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                postdatetimeStart.SelectedDate = DateTime.Now.AddDays(-30.0);
                postdatetimeEnd.SelectedDate = DateTime.Now;
                BindData();
            }
        }

        public void BindData()
        {
            DataGrid1.AllowCustomPaging = true;
            DataGrid1.VirtualItemCount = GetRecordCount();
            DataGrid1.DataSource = buildGridData();
            DataGrid1.DataBind();
        }

        protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid1.CurrentPageIndex = e.NewPageIndex;
            BindData();
        }

        public void GoToPagerButton_Click(object sender, EventArgs e)
        {
            BindData();
        }

        private DataTable buildGridData()
        {
            DateTime start = postdatetimeStart.SelectedDate;
            DateTime end = postdatetimeEnd.SelectedDate;
            Int32 idx = DataGrid1.CurrentPageIndex;
            Int32 size = DataGrid1.PageSize;
            return AdminVisitLog.Search(others.Text, Username.Text, start, end, null, idx * size, size).ToDataTable(false);
        }

        private int GetRecordCount()
        {
            DateTime start = postdatetimeStart.SelectedDate;
            DateTime end = postdatetimeEnd.SelectedDate;
            Int32 idx = DataGrid1.CurrentPageIndex;
            Int32 size = DataGrid1.PageSize;
            return AdminVisitLog.SearchCount(others.Text, Username.Text, start, end, null, idx * size, size);
        }

        private void DelRec_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                Int32 rs = AdminVisitLog.Del(Request["deleteMode"], Request["visitid"], Int32.Parse(deleteNum.Text), deleteFrom.SelectedDate);
                if (rs > 0) base.Response.Redirect("adminvisitloggrid.aspx");

                base.RegisterStartupScript("", "<script>alert('您未选中任何选项或输入参数错误！');window.location.href='adminvisitloggrid.aspx';</script>");
            }
        }

        public string BoolStr(string closed)
        {
            if (!(closed == "1"))
            {
                return "<div align=center><img src=../images/Cancel.gif /></div>";
            }
            return "<div align=center><img src=../images/OK.gif /></div>";
        }

        private void SearchLog_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                //string visitLogCondition = AdminVisitLog.Search(others.Text, Username.Text, postdatetimeStart.SelectedDate, postdatetimeEnd.SelectedDate);
                //ViewState["condition"] = visitLogCondition;
                DataGrid1.CurrentPageIndex = 0;
                BindData();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            SearchLog.Click += new EventHandler(SearchLog_Click);
            DelRec.Click += new EventHandler(DelRec_Click);
            DataGrid1.GoToPagerButton.Click += new EventHandler(GoToPagerButton_Click);
            DataGrid1.TableHeaderName = "后台访问记录";
            DataGrid1.AllowSorting = false;
            DataGrid1.DataKeyField = "ID";
            DataGrid1.ColumnSpan = 7;
        }
    }
}