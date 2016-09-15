using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BBX.Common;
using BBX.Control;
using BBX.Entity;
using BBX.Forum;
using NewLife;
using NewLife.Web;

namespace BBX.Web.Admin
{
    public class medalsloggird : AdminPage
    {
        protected HtmlForm Form1;
        protected Hint Hint1;
        protected BBX.Control.Calendar postdatetimeStart;
        protected BBX.Control.Calendar postdatetimeEnd;
        protected BBX.Control.TextBox reason;
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
            var start = postdatetimeStart.SelectedDate;
            var end = postdatetimeEnd.SelectedDate;
            var username = Username.Text;
            var rs = reason.Text;

            this.DataGrid1.AllowCustomPaging = true;
            this.DataGrid1.VirtualItemCount = MedalsLog.SearchCount(start, end, username, rs, 0, 0);
            this.DataGrid1.DataSource = MedalsLog.Search(start, end, username, rs, 0, 0);
            //if (this.ViewState["condition"] == null)
            //{
            //    this.DataGrid1.DataSource = MedalsLog.Search(this.DataGrid1.PageSize, this.DataGrid1.CurrentPageIndex + 1);
            //}
            //else
            //{
            //    this.DataGrid1.DataSource = MedalsLog.Search(this.DataGrid1.PageSize, this.DataGrid1.CurrentPageIndex + 1, this.ViewState["condition"].ToString());
            //}
            this.DataGrid1.DataBind();
        }

        //private int GetRecordCount()
        //{
        //    if (this.ViewState["condition"] == null)
        //    {
        //        //return AdminMedalLogs.RecordCount();
        //        return MedalsLog.Meta.Count;
        //    }
        //    return MedalsLog.SearchCount(this.ViewState["condition"].ToString());
        //}

        private void DelRec_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                //string delMedalLogCondition = AdminMedalLogs.GetDelMedalLogCondition(base.Request.Form["deleteMode"].ToString(), Request["id"].ToString(), this.deleteNum.Text.ToString(), this.deleteFrom.SelectedDate.ToString());
                //if (delMedalLogCondition != "")
                //{
                //    AdminMedalLogs.DeleteLog(delMedalLogCondition);
                //    base.Response.Redirect("forum_medalsloggird.aspx");
                //    return;
                //}
                //base.RegisterStartupScript("", "<script>alert('您未选中任何选项或输入参数错误！');window.location.href='forum_medalsloggird.aspx';</script>");

                MedalsLog.Delete(Request["deleteMode"], Request["id"], deleteNum.Text.ToInt(), deleteFrom.SelectedDate);
                base.Response.Redirect("forum_medalsloggird.aspx");
            }
        }

        private void SearchLog_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                //string searchMedalLogCondition = AdminMedalLogs.GetSearchMedalLogCondition(this.postdatetimeStart.SelectedDate, this.postdatetimeEnd.SelectedDate, this.Username.Text, this.reason.Text);
                //this.ViewState["condition"] = searchMedalLogCondition;
                this.DataGrid1.CurrentPageIndex = 0;
                this.BindData();
            }
        }

        public string Medals(string medalid)
        {
            if (String.IsNullOrEmpty(medalid.Trim()))
            {
                return "";
            }
            //DataTable medal = BBX.Forum.Medals.GetMedal(medalid.ToInt());
            //if (medal.Rows.Count > 0)
            Medal medal = Medal.FindByID(medalid.ToInt());
            if (medal != null)
            {
                return "<img src=../../images/medals/" + medal.Image + " height=25px> " + medal.Name;
            }
            return "";
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
            if (e.Item.Cells[10].Text.ToString().Length > 8)
            {
                e.Item.Cells[10].Text = Utils.HtmlEncode(e.Item.Cells[10].Text.Substring(0, 8)) + "...";
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
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.SearchLog.Click += new EventHandler(this.SearchLog_Click);
            this.DelRec.Click += new EventHandler(this.DelRec_Click);
            this.DataGrid1.GoToPagerButton.Click += new EventHandler(this.GoToPagerButton_Click);
            this.DataGrid1.ItemDataBound += new DataGridItemEventHandler(this.DataGrid_ItemDataBound);
            this.DataGrid1.TableHeaderName = "勋章授予记录";
            this.DataGrid1.AllowSorting = false;
            this.DataGrid1.ColumnSpan = 8;
        }
    }
}