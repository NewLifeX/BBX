using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Discuz.Common;
using Discuz.Control;
using Discuz.Entity;

namespace Discuz.Web.Admin
{
    public class adminvisitloggrid : AdminPage
    {
        protected HtmlForm Form1;
        protected Hint Hint1;
        protected Discuz.Control.Calendar postdatetimeStart;
        protected Discuz.Control.Calendar postdatetimeEnd;
        protected Discuz.Control.TextBox others;
        protected Discuz.Control.TextBox Username;
        protected Discuz.Control.Button SearchLog;
        protected Discuz.Control.DataGrid DataGrid1;
        protected Discuz.Control.TextBox deleteNum;
        protected Discuz.Control.Calendar deleteFrom;
        protected Discuz.Control.Button DelRec;

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
            var start = postdatetimeStart.SelectedDate;
            var end = postdatetimeEnd.SelectedDate;
            var idx = DataGrid1.CurrentPageIndex;
            var size = DataGrid1.PageSize;
            return AdminVisitLog.Search(others.Text, Username.Text, start, end, null, idx * size, size).ToDataTable();
        }

        private int GetRecordCount()
        {
            var start = postdatetimeStart.SelectedDate;
            var end = postdatetimeEnd.SelectedDate;
            var idx = DataGrid1.CurrentPageIndex;
            var size = DataGrid1.PageSize;
            return AdminVisitLog.SearchCount(others.Text, Username.Text, start, end, null, idx * size, size);
        }

        private void DelRec_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                var rs = AdminVisitLog.Del(Request["deleteMode"], Request["visitid"], Int32.Parse(deleteNum.Text), deleteFrom.SelectedDate);
                if (rs > 0) base.Response.Redirect("forum_adminvisitloggrid.aspx");

                base.RegisterStartupScript("", "<script>alert('您未选中任何选项或输入参数错误！');window.location.href='forum_adminvisitloggrid.aspx';</script>");
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