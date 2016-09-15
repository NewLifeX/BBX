using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BBX.Cache;
using BBX.Common;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web.Admin
{
    public class forumsgrid : AdminPage
    {
        protected HtmlForm Form1;
        protected pageinfo info1;
        protected BBX.Control.DataGrid DataGrid1;
        protected BBX.Control.Button SaveForum;
        protected System.Web.UI.WebControls.Button SysteAutoSet;

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
            this.DataGrid1.TableHeaderName = "论坛列表";
            this.DataGrid1.DataSource = XForum.Root.AllChilds;
            this.DataGrid1.DataBind();
        }

        protected void Sort_Grid(object sender, DataGridSortCommandEventArgs e)
        {
            DataView dataView = new DataView(XForum.Root.AllChilds.ToDataTable(false));
            dataView.Sort = e.SortExpression.ToString();
            this.DataGrid1.DataSource = dataView;
            this.DataGrid1.DataBind();
        }

        protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            this.DataGrid1.CurrentPageIndex = e.NewPageIndex;
            this.BindData();
        }

        private void SaveForum_Click(object sender, EventArgs e)
        {
            int num = -1;
            bool flag = false;
            foreach (object current in this.DataGrid1.GetKeyIDArray())
            {
                int fid = int.Parse(current.ToString());
                string text = this.DataGrid1.GetControlValue(num, "name").Trim();
                //string text2 = this.DataGrid1.GetControlValue(num, "subforumcount").Trim();
                string text3 = this.DataGrid1.GetControlValue(num, "displayorder").Trim();
                if (String.IsNullOrEmpty(text) || !Utils.IsNumeric(text3))
                {
                    flag = true;
                }
                else
                {
                    var forumInfo = Forums.GetForumInfo(fid);
                    forumInfo.Name = text;
                    //forumInfo.SubforumCount = int.Parse(text2);
                    forumInfo.DisplayOrder = int.Parse(text3);
                    AdminForums.UpdateForumInfo(forumInfo);
                    num++;
                }
            }
            XCache.Remove(CacheKeys.FORUM_UI_FORUM_LIST_BOX_OPTIONS);
            XCache.Remove(CacheKeys.FORUM_FORUM_LIST);
            if (flag)
            {
                base.RegisterStartupScript("PAGE", "alert('某些记录取值不正确，未能被更新！');window.location.href='forum_forumsgrid.aspx';");
                return;
            }
            base.RegisterStartupScript("PAGE", "window.location.href='forum_forumsgrid.aspx';");
        }

        private void DataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                System.Web.UI.WebControls.TextBox textBox = (System.Web.UI.WebControls.TextBox)e.Item.Cells[1].Controls[0];
                textBox.Attributes.Add("maxlength", "50");
                textBox.Width = 80;
                textBox = (System.Web.UI.WebControls.TextBox)e.Item.Cells[5].Controls[0];
                textBox.Attributes.Add("maxlength", "8");
                textBox.Width = 30;
                textBox = (System.Web.UI.WebControls.TextBox)e.Item.Cells[6].Controls[0];
                textBox.Attributes.Add("maxlength", "8");
                textBox.Width = 30;
            }
        }

        //private DataTable buildGridData()
        //{
        //    DataTable forumListForDataTable = Forums.GetForumListForDataTable();
        //    foreach (DataRow dataRow in forumListForDataTable.Rows)
        //    {
        //        dataRow["parentidlist"] = dataRow["parentidlist"].ToString().Trim();
        //        dataRow["name"] = dataRow["name"].ToString().Trim().Replace("\"", "'");
        //    }
        //    return forumListForDataTable;
        //}

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.SaveForum.Click += new EventHandler(this.SaveForum_Click);
            this.DataGrid1.ItemDataBound += new DataGridItemEventHandler(this.DataGrid_ItemDataBound);
            this.DataGrid1.DataKeyField = "fid";
            this.DataGrid1.TableHeaderName = "论坛列表";
            this.DataGrid1.AllowPaging = false;
            this.DataGrid1.ShowFooter = false;
            this.DataGrid1.SaveDSViewState = true;
        }
    }
}