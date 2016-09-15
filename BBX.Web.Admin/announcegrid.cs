using System;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BBX.Common;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web.Admin
{
    public class announcegrid : AdminPage
    {
        protected HtmlForm Form1;
        protected BBX.Control.TextBox poster;
        protected BBX.Control.TextBox title;
        protected BBX.Control.Calendar postdatetimeStart;
        protected BBX.Control.Calendar postdatetimeEnd;
        protected BBX.Control.Button Search;
        protected BBX.Control.DataGrid DataGrid1;
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
            this.DataGrid1.AllowCustomPaging = false;
            this.DataGrid1.BindData(Announcement.FindAllWithCache());
        }

        protected void Sort_Grid(object sender, DataGridSortCommandEventArgs e)
        {
            this.DataGrid1.Sort = e.SortExpression.ToString();
        }

        protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            this.DataGrid1.LoadCurrentPageIndex(e.NewPageIndex);
        }

        private void DelRec_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                if (Request["id"] != "")
                {
                    //Announcements.DeleteAnnouncements(Request["id"]);
                    var entity = Announcement.FindByID(Int32.Parse(Request["id"]));
                    if (entity != null) entity.Delete();
                    AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "删除公告", "删除公告,公告ID为: " + Request["id"]);
                    base.Response.Redirect("global_announcegrid.aspx");
                    return;
                }
                base.RegisterStartupScript("", "<script>alert('您未选中任何选项');window.location.href='global_announcegrid.aspx';</script>");
            }
        }

        private void Search_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                StringBuilder stringBuilder = new StringBuilder();
                if (!String.IsNullOrEmpty(this.poster.Text))
                {
                    stringBuilder.Append("[poster] LIKE '%");
                    stringBuilder.Append(this.poster.Text);
                    stringBuilder.Append("%'");
                }
                if (!String.IsNullOrEmpty(this.title.Text))
                {
                    if (stringBuilder.Length > 0)
                    {
                        stringBuilder.Append(" AND ");
                    }
                    stringBuilder.Append("[title] LIKE '%");
                    stringBuilder.Append(this.title.Text);
                    stringBuilder.Append("%'");
                }
                if (this.postdatetimeStart.SelectedDate > DateTime.MinValue)
                {
                    if (stringBuilder.Length > 0)
                    {
                        stringBuilder.Append(" AND ");
                    }
                    stringBuilder.Append("[starttime] >= '");
                    stringBuilder.Append(this.postdatetimeStart.SelectedDate.ToString());
                    stringBuilder.Append("'");
                }
                if (this.postdatetimeEnd.SelectedDate > DateTime.MinValue)
                {
                    if (stringBuilder.Length > 0)
                    {
                        stringBuilder.Append(" AND ");
                    }
                    stringBuilder.Append("[starttime] <= '");
                    stringBuilder.Append(this.postdatetimeEnd.SelectedDate.ToString());
                    stringBuilder.Append("'");
                }
                if (stringBuilder.Length > 0)
                {
                    stringBuilder.Insert(0, " WHERE ");
                }
                //this.DataGrid1.BindData(Announcements.GetAnnouncementsByCondition(stringBuilder.ToString()));
                DataGrid1.BindData(Announcement.Search(poster.Text, title.Text, postdatetimeStart.SelectedDate, postdatetimeEnd.SelectedDate));
            }
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.Search.Click += new EventHandler(this.Search_Click);
            this.DelRec.Click += new EventHandler(this.DelRec_Click);
            this.DataGrid1.TableHeaderName = "公告列表";
            this.DataGrid1.ColumnSpan = 7;
        }
    }
}