using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BBX.Common;
using BBX.Control;
using BBX.Entity;

namespace BBX.Web.Admin
{
    public class paymentloggrid : AdminPage
    {
        protected HtmlForm Form1;
        protected Hint Hint1;
        protected BBX.Control.Calendar postdatetimeStart;
        protected BBX.Control.Calendar postdatetimeEnd;
        protected BBX.Control.TextBox Username;
        protected BBX.Control.Button SearchLog;
        protected BBX.Control.DataGrid DataGrid1;
        protected BBX.Control.TextBox deleteNum;
        protected BBX.Control.Calendar deleteFrom;
        protected BBX.Control.Button DelRec;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.postdatetimeStart.SelectedDate = DateTime.Now.AddDays(-30.0);
                this.postdatetimeEnd.SelectedDate = DateTime.Now;
                this.BindData();
            }
        }

        public void BindData()
        {
            this.DataGrid1.AllowCustomPaging = true;
            this.DataGrid1.VirtualItemCount = PaymentLog.RecordCount((this.ViewState["condition"] == null) ? "" : this.ViewState["condition"].ToString());
            if (this.ViewState["condition"] == null)
            {
                this.DataGrid1.DataSource = PaymentLog.LogList(this.DataGrid1.PageSize, this.DataGrid1.CurrentPageIndex + 1);
            }
            else
            {
                this.DataGrid1.DataSource = PaymentLog.LogList(this.DataGrid1.PageSize, this.DataGrid1.CurrentPageIndex + 1, this.ViewState["condition"].ToString());
            }
            this.DataGrid1.DataBind();
        }

        private void DelRec_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                //string deleteModeratorManageCondition = AdminPaymentLogs.GetSearchPaymentLogCondition(base.Request.Form["deleteMode"].ToString(), Request["id"].ToString(), this.deleteNum.Text.ToString(), this.deleteFrom.SelectedDate.ToString());
                var deleteModeratorManageCondition = "";
                if (deleteModeratorManageCondition != "")
                {
                    PaymentLog.DeleteLog(deleteModeratorManageCondition);
                    base.Response.Redirect("forum_paymentloggrid.aspx");
                    return;
                }
                base.RegisterStartupScript("", "<script>alert('您未选中任何选项或输入参数错误！');window.location.href='forum_paymentloggrid.aspx';</script>");
            }
        }

        private void SearchLog_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                string searchPaymentLogCondition = PaymentLog.GetSearchPaymentLogCondition(this.postdatetimeStart.SelectedDate, this.postdatetimeEnd.SelectedDate, this.Username.Text);
                this.ViewState["condition"] = searchPaymentLogCondition;
                this.DataGrid1.CurrentPageIndex = 0;
                this.BindData();
            }
        }

        protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            this.DataGrid1.CurrentPageIndex = e.NewPageIndex;
            this.BindData();
        }

        public void GoToPagerButton_Click(object sender, EventArgs e)
        {
            this.BindData();
        }

        private void DataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.Cells[8].Text.ToString().Length > 8)
            {
                e.Item.Cells[8].Text = Utils.HtmlEncode(e.Item.Cells[8].Text.Substring(0, 8)) + "…";
            }
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.SearchLog.Click += new EventHandler(this.SearchLog_Click);
            this.DelRec.Click += new EventHandler(this.DelRec_Click);
            this.DataGrid1.GoToPagerButton.Click += new EventHandler(this.GoToPagerButton_Click);
            this.DataGrid1.ItemDataBound += new DataGridItemEventHandler(this.DataGrid_ItemDataBound);
            this.DataGrid1.TableHeaderName = "积分交易记录";
            this.DataGrid1.AllowSorting = false;
            this.DataGrid1.ColumnSpan = 8;
        }
    }
}