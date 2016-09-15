using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BBX.Cache;
using BBX.Common;
using BBX.Config;
using BBX.Control;
using BBX.Entity;
using BBX.Forum;
using NewLife.Web;

namespace BBX.Web.Admin
{
    public class forumlinksgrid : AdminPage
    {
        protected HtmlForm Form1;
        protected BBX.Control.DataGrid DataGrid1;
        protected BBX.Control.Button SaveFriend;
        protected BBX.Control.Button DelRec;
        protected Hint Hint1;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.BindData();
            }
            if (DNTRequest.GetString("displayorder") != "" && DNTRequest.GetString("name") != "")// Request["displayorder"].IsNullOrEmpty() && Request["name"].IsNullOrEmpty())
            {
                Regex regex = new Regex("(http|https)://([\\w-]+\\.)+[\\w-]+(/[\\w-./?%&=]*)?");
                if (!regex.IsMatch(Request["url"].Replace("'", "''")))
                {
                    base.RegisterStartupScript("", "<script>alert('链接地址不是有效的网页地址.');</script>");
                    return;
                }
                try
                {
                    //ForumLinks.CreateForumLink(DNTRequest.GetInt("displayorder", 0), Request["name"], Request["url"], Request["note"], Request["logo"]);

                    var lnk = new ForumLink();
                    lnk.DisplayOrder = WebHelper.RequestInt("displayorder");
                    lnk.Name = Request["name"];
                    lnk.Url = Request["url"];
                    lnk.Note = Request["note"];
                    lnk.Logo = Request["logo"];
                    lnk.Save();

                    AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "添加友情链接", "添加友情链接,名称为: " + Request["name"]);
                    XCache.Remove(CacheKeys.FORUM_FORUM_LINK_LIST);
                    this.BindData();
                }
                catch
                {
                    base.RegisterStartupScript("", "<script>alert('无法更新数据库');window.location.href='global_forumlinksgrid.aspx';</script>");
                }
            }
        }

        public void BindData()
        {
            this.DataGrid1.AllowCustomPaging = false;
            this.DataGrid1.TableHeaderName = "友情链接列表";
            DataTable forumLinks = ForumLink.FindAllWithCache().ToDataTable();
            DataTable dataTable = forumLinks.Clone();
            dataTable.Rows.Clear();
            string[] array = new string[]
			{
				"note",
				"logo",
				""
			};
            string[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                string text = array2[i];
                foreach (DataRow dataRow in forumLinks.Rows)
                {
                    DataRow dataRow2 = dataTable.NewRow();
                    dataRow2.ItemArray = dataRow.ItemArray;
                    string a;
                    if ((a = text) != null)
                    {
                        if (!(a == "note"))
                        {
                            if (a == "logo")
                            {
                                if (!dataRow["logo"].ToString().IsNullOrEmpty() && String.IsNullOrEmpty(dataRow["note"].ToString().Trim()))
                                {
                                    dataTable.Rows.Add(dataRow2);
                                    continue;
                                }
                                continue;
                            }
                        }
                        else
                        {
                            if (!dataRow["note"].ToString().IsNullOrEmpty())
                            {
                                dataTable.Rows.Add(dataRow2);
                                continue;
                            }
                            continue;
                        }
                    }
                    if (String.IsNullOrEmpty(dataRow["logo"].ToString().Trim()) && String.IsNullOrEmpty(dataRow["note"].ToString().Trim()))
                    {
                        dataTable.Rows.Add(dataRow2);
                    }
                }
            }
            this.DataGrid1.BindData(dataTable);
        }

        protected void Sort_Grid(object sender, DataGridSortCommandEventArgs e)
        {
            this.DataGrid1.Sort = e.SortExpression.ToString();
        }

        protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            this.DataGrid1.LoadCurrentPageIndex(e.NewPageIndex);
        }

        protected void SaveFriend_Click(object sender, EventArgs e)
        {
            int num = 0;
            bool flag = false;
            foreach (object current in this.DataGrid1.GetKeyIDArray())
            {
                int displayorder = int.Parse(this.DataGrid1.GetControlValue(num, "displayorder"));
                string name = this.DataGrid1.GetControlValue(num, "name").Trim();
                string url = this.DataGrid1.GetControlValue(num, "url").Trim();
                string note = this.DataGrid1.GetControlValue(num, "note").Trim();
                string logo = this.DataGrid1.GetControlValue(num, "logo").Trim();

                var lnk = ForumLink.FindByID(int.Parse(current.ToString()));
                if (lnk != null)
                {
                    lnk.DisplayOrder = displayorder;
                    lnk.Name = name;
                    lnk.Url = url;
                    lnk.Note = note;
                    lnk.Logo = logo;
                    lnk.Save();

                    num++;
                }
            }
            AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "批量更新友情链接", "");
            XCache.Remove(CacheKeys.FORUM_FORUM_LINK_LIST);
            if (flag)
            {
                base.RegisterStartupScript("PAGE", "alert('某些信息不完整，未能更新！');window.location.href='global_forumlinksgrid.aspx';");
                return;
            }
            base.RegisterStartupScript("PAGE", "window.location.href='global_forumlinksgrid.aspx';");
        }

        private void DataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                System.Web.UI.WebControls.TextBox textBox = (System.Web.UI.WebControls.TextBox)e.Item.Cells[2].Controls[0];
                textBox.Attributes.Add("maxlength", "6");
                textBox.Width = 40;
                textBox = (System.Web.UI.WebControls.TextBox)e.Item.Cells[3].Controls[0];
                textBox.Attributes.Add("maxlength", "100");
                textBox.Width = 80;
                textBox = (System.Web.UI.WebControls.TextBox)e.Item.Cells[4].Controls[0];
                textBox.Attributes.Add("maxlength", "100");
                textBox.Width = 120;
                textBox = (System.Web.UI.WebControls.TextBox)e.Item.Cells[5].Controls[0];
                textBox.Attributes.Add("maxlength", "200");
                textBox.Width = 300;
                textBox = (System.Web.UI.WebControls.TextBox)e.Item.Cells[6].Controls[0];
                textBox.Attributes.Add("maxlength", "100");
                textBox.Width = 100;
            }
        }

        private void DelRec_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                if (Request["delid"] != "")
                {
                    //ForumLinks.DeleteForumLink(Request["delid"]);
                    //var ids = TypeConverter.StringToIntArray(Request["delid"]);
                    var ids = Request["delid"].SplitAsInt(",");
                    //var link = ForumLink.FindByID(WebHelper.RequestInt("delid"));
                    //if (link != null) link.Delete();
                    var links = ForumLink.FindAllByIds(ids);
                    if (links != null && links.Count > 0) links.Delete();

                    AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "删除友情链接", "删除友情链接,ID为: " + Request["delid"].Replace("0 ", ""));
                    XCache.Remove(CacheKeys.FORUM_FORUM_LINK_LIST);
                    base.Response.Redirect("global_forumlinksgrid.aspx");
                    return;
                }
                base.RegisterStartupScript("", "<script>alert('您未选中任何选项');window.location.href='global_forumlinksgrid.aspx';</script>");
            }
        }

        public string LogoStr(string filename)
        {
            if (filename.IsNullOrEmpty())
            {
                return "";
            }
            if (filename.ToLower().StartsWith("http"))
            {
                return "<div align=left><img src=" + filename + " width=91px height=32px  /></div>";
            }
            return "<div align=left><img src=" + BaseConfigs.GetForumPath + filename + " width=91px height=32px  /></div>";
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.DelRec.Click += new EventHandler(this.DelRec_Click);
            this.SaveFriend.Click += new EventHandler(this.SaveFriend_Click);
            this.DataGrid1.ItemDataBound += new DataGridItemEventHandler(this.DataGrid_ItemDataBound);
            this.DataGrid1.ColumnSpan = 8;
        }
    }
}