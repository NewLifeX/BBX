using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BBX.Common;
using BBX.Config;
using BBX.Control;
using BBX.Forum;
using BBX.Entity;

namespace BBX.Web.Admin
{
    public class detachtable : AdminPage
    {
        protected HtmlForm Form1;
        protected pageinfo info1;
        protected pageinfo info2;
        protected BBX.Control.DataGrid DataGrid1;
        protected BBX.Control.Button StartFullIndex;
        protected BBX.Control.TextBox detachtabledescription;
        protected BBX.Control.Button SaveInfo;
        protected Hint Hint1;

        protected void Page_Load(object sender, EventArgs e)
        {
            string postTableName = TableList.CurrentTableName;
            this.info2.Text = "系统当前使用的帖子分表是: <b>" + postTableName + "</b>";
            if (!this.Page.IsPostBack)
            {
                //if (!Databases.IsFullTextSearchEnabled())
                {
                    this.StartFullIndex.Visible = false;
                    this.DataGrid1.Columns[0].Visible = false;
                }
                this.BindData();
                this.detachtabledescription.AddAttributes("maxlength", "50");
                this.SaveInfo.Attributes.Add("onclick", "if(!confirm('您目前表中帖子数不足" + Posts.GetPostTableCount(postTableName) + "万,要进行帖子分表吗?')){return false;}");
            }
        }

        public void BindData()
        {
            //this.DataGrid1.DataSource = Posts.GetPostTableList();
            this.DataGrid1.DataSource = TableList.GetAllPostTable();
            this.DataGrid1.DataBind();
        }

        protected void Sort_Grid(object sender, DataGridSortCommandEventArgs e)
        {
            //DataView dataView = new DataView(Posts.GetPostTableList());
            //dataView.Sort = e.SortExpression.ToString();
            //this.DataGrid1.DataSource = dataView;
            var list = TableList.GetAllPostTable();
            if (!String.IsNullOrEmpty(e.SortExpression))
            {
                var name = e.SortExpression;
                var desc = false;
                var p = e.SortExpression.IndexOf(" ");
                if (p >= 0)
                {
                    name = e.SortExpression.Substring(0, p);
                    var dir = e.SortExpression.Substring(p + 1).Trim();
                    if (dir.EqualIgnoreCase("desc")) desc = true;
                }
                list.Sort(name, desc);
            }
            this.DataGrid1.DataSource = list;
            this.DataGrid1.DataBind();
        }

        protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            this.DataGrid1.CurrentPageIndex = e.NewPageIndex;
            this.BindData();
        }

        protected void DataGrid_Edit(object sender, DataGridCommandEventArgs E)
        {
            this.DataGrid1.EditItemIndex = E.Item.ItemIndex;
            this.BindData();
        }

        protected void DataGrid_Cancel(object sender, DataGridCommandEventArgs e)
        {
            this.DataGrid1.EditItemIndex = -1;
            this.BindData();
        }

        protected void DataGrid_Update(object sender, DataGridCommandEventArgs e)
        {
            try
            {
                TableList.UpdateDetachTable(Utils.StrToInt(this.DataGrid1.DataKeys[e.Item.ItemIndex].ToString(), 0), ((System.Web.UI.WebControls.TextBox)e.Item.Cells[4].Controls[0]).Text);
                base.RegisterStartupScript("", "<script>window.location.href='global_detachtable.aspx';</script>");
            }
            catch
            {
                base.RegisterStartupScript("", "<script>alert('无法更新数据库.');window.location.href='global_detachtable.aspx';</script>");
            }
        }

        private void DataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.EditItem)
            {
                System.Web.UI.WebControls.TextBox textBox = (System.Web.UI.WebControls.TextBox)e.Item.Cells[4].Controls[0];
                textBox.Attributes.Add("maxlength", "50");
                textBox.Attributes.Add("size", "20");
            }
            if (e.Item.ItemType == ListItemType.Item && e.Item.Cells[2].Text.ToString().Length > 40)
            {
                e.Item.Cells[2].Text = e.Item.Cells[2].Text.Substring(0, 40) + "…";
            }
        }

        private void StartFullIndex_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                //string text = new Databases().StartFullIndex(Request["id"], Databases.GetDbName(), this.username);
                //base.RegisterStartupScript(text.StartsWith("window") ? "PAGE" : "", text);
            }
        }

        private void SaveInfo_Click(object sender, EventArgs e)
        {
            if (this.CreateDetachTable(this.detachtabledescription.Text))
            {
                Caches.ReSetLastPostTableName();
                Caches.ReSetAllPostTableName();
                TableList.ResetPostTables();
                base.RegisterStartupScript("PAGE", "window.location.href='global_detachtable.aspx';");
                if (Request["createtype"] == "on")
                {
                    Utils.RestartIISProcess();
                }
            }
        }

        public bool CreateDetachTable(string description)
        {
            bool result;
            try
            {
                string text = BaseConfigs.GetTablePrefix + "posts";
                int maxPostTableId = Posts.GetMaxPostTableId();
                if (!TableList.UpdateMinMaxField(maxPostTableId))
                {
                    base.RegisterStartupScript("", "<script>alert('表值总数不能大于213,当前最大值为" + maxPostTableId + "!');window.location.href='global_detachtable.aspx';</script>");
                    result = false;
                }
                else
                {
                    string tablename = text + (maxPostTableId + 1);
                    try
                    {
                        //Databases.CreatePostTableAndIndex(tablename);
                    }
                    catch (Exception ex)
                    {
                        string text2 = ex.Message.Replace("'", " ");
                        text2 = text2.Replace("\\", "/");
                        text2 = text2.Replace("\r\n", "\\r\\n");
                        text2 = text2.Replace("\r", "\\r");
                        text2 = text2.Replace("\n", "\\n");
                        base.RegisterStartupScript("", "<script>alert('" + text2 + "');</script>");
                    }
                    finally
                    {
                        if (maxPostTableId > 0)
                        {
                            TableList.AddPostTableToTableList(description, Post.GetMaxPostTableTid(text + maxPostTableId));
                        }
                        else
                        {
                            TableList.AddPostTableToTableList(description, Post.GetMaxPostTableTid(text));
                        }
                        Caches.ReSetPostTableInfo();
                        //Posts.CreateStoreProc(maxPostTableId + 1);
                    }
                    result = true;
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public string DisplayTid(string mintid, string maxtid)
        {
            if (!(maxtid == "0")) return mintid + " 至 " + maxtid;

            var maxTid = Topic.GetMaxPostTableTid();
            if (maxTid >= 0) return mintid + " 至 " + maxTid;

            return mintid + " 至 " + maxtid;
        }

        public string CurrentPostsCount(string postsid)
        {
            return Post.Meta.Count + "";
            //string result;
            //try
            //{
            //    DataTable postCountFromIndex = Posts.GetPostCountFromIndex(postsid);
            //    if (postCountFromIndex.Rows.Count > 0)
            //    {
            //        result = postCountFromIndex.Rows[0][0].ToString();
            //    }
            //    else
            //    {
            //        result = "0";
            //    }
            //}
            //catch
            //{
            //    DataTable postCountTable = Posts.GetPostCountTable(postsid);
            //    if (postCountTable.Rows.Count > 0)
            //    {
            //        result = postCountTable.Rows[0][0].ToString();
            //    }
            //    else
            //    {
            //        result = "0";
            //    }
            //}
            //return result;
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.DataGrid1.EditCommand += new DataGridCommandEventHandler(this.DataGrid_Edit);
            this.DataGrid1.CancelCommand += new DataGridCommandEventHandler(this.DataGrid_Cancel);
            this.DataGrid1.ItemDataBound += new DataGridItemEventHandler(this.DataGrid_ItemDataBound);
            this.DataGrid1.UpdateCommand += new DataGridCommandEventHandler(this.DataGrid_Update);
            this.SaveInfo.Click += new EventHandler(this.SaveInfo_Click);
            this.StartFullIndex.Click += new EventHandler(this.StartFullIndex_Click);
            base.Load += new EventHandler(this.Page_Load);
            this.DataGrid1.LoadEditColumn();
            this.DataGrid1.DataKeyField = "id";
            this.DataGrid1.TableHeaderName = "帖子分表列表";
            this.DataGrid1.ColumnSpan = 4;
            this.DataGrid1.SaveDSViewState = true;
        }
    }
}