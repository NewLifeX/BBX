using System;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BBX.Entity;

namespace BBX.Web.Admin
{
    public class postgrid : AdminPage
    {
        protected HtmlForm Form1;
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
            //DataTable dataTable = AdminTopics.AdminGetPostList(DNTRequest.GetInt("tid", -1), this.DataGrid1.PageSize, this.DataGrid1.CurrentPageIndex + 1).Tables[0];
            //DataRow[] array = dataTable.Select("layer=0");
            //for (int i = 0; i < array.Length; i++)
            //{
            //    DataRow row = array[i];
            //    dataTable.Rows.Remove(row);
            //}
            var list = Post.FindAllByTid(Request["tid"].ToInt());
            var list2 = list.ToList().Where(e => e.Layer != 0).ToList();
            this.DataGrid1.DataSource = list2;
            this.DataGrid1.DataBind();
        }

        protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            this.DataGrid1.CurrentPageIndex = e.NewPageIndex;
            this.BindData();
        }

        protected void DataGrid_Cancel(object sender, DataGridCommandEventArgs E)
        {
            this.DataGrid1.Cancel();
        }

        public string Invisible(string invisible)
        {
            if (!(invisible == "1"))
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
			this.DataGrid1.DataKeyField = "ID";
            this.DataGrid1.TableHeaderName = "发帖列表";
            this.DataGrid1.ColumnSpan = 7;
        }
    }
}