using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BBX.Common;
using BBX.Config;
using BBX.Control;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web.Admin
{
    public class moderatormanagelog : AdminPage
    {
        protected HtmlForm Form1;
        protected Hint Hint1;
        protected BBX.Control.Calendar postdatetimeStart;
        protected BBX.Control.Calendar postdatetimeEnd;
        protected BBX.Control.TextBox others;
        protected BBX.Control.TextBox Username;
        protected BBX.Control.Button SearchLog;
        protected BBX.Control.DataGrid DataGrid1;
        protected BBX.Control.TextBox deleteNum;
        protected BBX.Control.Calendar deleteFrom;
        protected BBX.Control.Button DelRec;

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
            //DataTable dataTable = (ViewState["condition"] == null) ? AdminModeratorLogs.LogList(DataGrid1.PageSize, DataGrid1.CurrentPageIndex + 1) : AdminModeratorLogs.LogList(DataGrid1.PageSize, DataGrid1.CurrentPageIndex + 1, ViewState["condition"].ToString());
            var size = DataGrid1.PageSize;
            var startRow = DataGrid1.CurrentPageIndex * size;
            var dataTable = ModeratorManageLog.Search(others.Text, Username.Text, postdatetimeStart.SelectedDate, postdatetimeEnd.SelectedDate, null, startRow, size).ToDataTable(false);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                dataRow["reason"] = dataRow["reason"].ToString().Trim();
                dataRow["title"] = ((!dataRow["title"].ToString().IsNullOrEmpty()) ? string.Format("<a href=\"{0}\" target=\"_blank\">{1}</a>", BaseConfigs.GetForumPath + Urls.ShowTopicAspxRewrite(dataRow["tid"].ToInt(), 1), dataRow["title"]) : "没有标题");
            }
            DataGrid1.DataSource = dataTable;
            DataGrid1.DataBind();
        }

        private int GetRecordCount()
        {
            var size = DataGrid1.PageSize;
            var startRow = DataGrid1.CurrentPageIndex * size;
            return ModeratorManageLog.SearchCount(others.Text, Username.Text, postdatetimeStart.SelectedDate, postdatetimeEnd.SelectedDate, null, startRow, size);
            //if (ViewState["condition"] == null)
            //{
            //    return ModeratorManageLog.Meta.Count;
            //}
            //return AdminModeratorLogs.RecordCount(ViewState["condition"].ToString());
        }

        private void DelRec_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                var rs = ModeratorManageLog.Del(Request["deleteMode"], Request["id"], Int32.Parse(deleteNum.Text), deleteFrom.SelectedDate);
                if (rs > 0)
                {
                    base.Response.Redirect("forum_moderatormanagelog.aspx");
                    return;
                }
                base.RegisterStartupScript("", "<script>alert('您未选中任何选项或输入参数错误！');window.location.href='forum_moderatormanagelog.aspx';</script>");
            }
        }

        public string GroupName(string groupid)
        {
            var userGroupInfo = UserGroup.FindByID(groupid.ToInt());
            if (userGroupInfo == null)
            {
                return "";
            }
            return userGroupInfo.GroupTitle;
        }

        private void SearchLog_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                //string searchModeratorManageLogCondition = AdminModeratorLogs.GetSearchModeratorManageLogCondition(postdatetimeStart.SelectedDate, postdatetimeEnd.SelectedDate, Username.Text, others.Text);
                //ViewState["condition"] = searchModeratorManageLogCondition;
                DataGrid1.CurrentPageIndex = 0;
                BindData();
            }
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

        private void DataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.Cells[9].Text.ToString().Length > 8)
            {
                e.Item.Cells[9].Text = Utils.HtmlEncode(e.Item.Cells[9].Text.Substring(0, 8)) + "…";
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
            DataGrid1.ItemDataBound += new DataGridItemEventHandler(DataGrid_ItemDataBound);
            base.Load += new EventHandler(Page_Load);
            DataGrid1.TableHeaderName = "版主管理日志记录";
            DataGrid1.AllowSorting = false;
            DataGrid1.ColumnSpan = 9;
        }
    }
}