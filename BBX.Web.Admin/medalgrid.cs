using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BBX.Cache;
using BBX.Common;
using BBX.Config;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web.Admin
{
    public class medalgrid : AdminPage
    {
        protected HtmlForm Form1;
        protected pageinfo info1;
        protected BBX.Control.DataGrid DataGrid1;
        protected BBX.Control.Button SaveMedal;
        protected BBX.Control.Button Available;
        protected BBX.Control.Button UnAvailable;
        protected BBX.Control.Button ImportMedal;

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
            this.DataGrid1.TableHeaderName = "论坛勋章列表";
            //this.DataGrid1.BindData(Medals.GetMedal());
            this.DataGrid1.BindData(Medal.FindAllWithCache().ToDataTable(false));
        }

        private void DataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.EditItem)
            {
                System.Web.UI.WebControls.TextBox textBox = (System.Web.UI.WebControls.TextBox)e.Item.Cells[3].Controls[0];
                textBox.Attributes.Add("maxlength", "50");
                textBox.Attributes.Add("size", "30");
                textBox = (System.Web.UI.WebControls.TextBox)e.Item.Cells[4].Controls[0];
                textBox.Attributes.Add("maxlength", "30");
                textBox.Attributes.Add("size", "30");
            }
        }

        private void SaveMedal_Click(object send, EventArgs e)
        {
            int num = 0;
            bool flag = false;
            foreach (object current in this.DataGrid1.GetKeyIDArray())
            {
                int medalid = int.Parse(current.ToString());
                string name = this.DataGrid1.GetControlValue(num, "name").Trim();
                string image = this.DataGrid1.GetControlValue(num, "image").Trim();
                if (String.IsNullOrEmpty(name) || String.IsNullOrEmpty(image))
                {
                    flag = true;
                }
                else
                {
                    //Medals.UpdateMedal(medalid, name, image);
                    var entity = Medal.FindByID(medalid);
                    entity.Name = name;
                    entity.Image = image;
                    entity.Update();

                    num++;
                }
            }
            AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "批量更新勋章信息", "");
            //XCache.Remove(CacheKeys.FORUM_UI_MEDALS_LIST);
            if (flag)
            {
                base.RegisterStartupScript("PAGE", "alert('某些信息不完整，未能更新！');window.location.href='global_medalgrid.aspx';");
                return;
            }
            base.RegisterStartupScript("PAGE", "window.location.href='global_medalgrid.aspx';");
        }

        private void Available_Click(object sender, EventArgs e)
        {
            if (Request["medalid"] != "")
            {
                Medal.SetAvailable(Request["medalid"], true);
                AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "勋章文件为有效", "ID:" + Request["medalid"].Replace("0 ", ""));
                base.Response.Redirect("global_medalgrid.aspx");
                return;
            }
            base.RegisterStartupScript("", "<script>alert('您未选中任何选项');window.location.href='global_medalgrid.aspx';</script>");
        }

        private void ImportMedal_Click(object sender, EventArgs e)
        {
            AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "导入勋章", "");
            //Medals.InsertMedalList(this.GetMedalFileList());
            Medal.Import(GetMedalFileList());
            base.RegisterStartupScript("PAGE", "window.location.href='global_medalgrid.aspx';");
        }

        private String[] GetMedalFileList()
        {
            string strPath = BaseConfigs.GetForumPath + "images/medals";
            DirectoryInfo directoryInfo = new DirectoryInfo(Utils.GetMapPath(strPath));
            if (!directoryInfo.Exists)
            {
                throw new IOException("勋章文件夹不存在!");
            }
            FileInfo[] files = directoryInfo.GetFiles();
            List<String> arrayList = new List<String>();
            FileInfo[] array = files;
            for (int i = 0; i < array.Length; i++)
            {
                FileInfo fileInfo = array[i];
                arrayList.Add(fileInfo.Name.ToLower());
            }
            return arrayList.ToArray();
        }

        private void UnAvailable_Click(object sender, EventArgs e)
        {
            if (Request["medalid"] != "")
            {
                Medal.SetAvailable(Request["medalid"], false);
                AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "勋章文件为无效", "ID:" + Request["medalid"]);
                base.Response.Redirect("global_medalgrid.aspx");
                return;
            }
            base.RegisterStartupScript("", "<script>alert('您未选中任何选项');window.location.href='global_medalgrid.aspx';</script>");
        }

        protected void Sort_Grid(object sender, DataGridSortCommandEventArgs e)
        {
            this.DataGrid1.Sort = e.SortExpression.ToString();
        }

        protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            this.DataGrid1.LoadCurrentPageIndex(e.NewPageIndex);
        }

        public string PicStr(string filename)
        {
            if (filename != "")
            {
                return "<img src=../../images/medals/" + filename + " height=25px width=14px />";
            }
            return "";
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.Available.Click += new EventHandler(this.Available_Click);
            this.UnAvailable.Click += new EventHandler(this.UnAvailable_Click);
            this.ImportMedal.Click += new EventHandler(this.ImportMedal_Click);
            this.SaveMedal.Click += new EventHandler(this.SaveMedal_Click);
            this.DataGrid1.ItemDataBound += new DataGridItemEventHandler(this.DataGrid_ItemDataBound);
            this.DataGrid1.TableHeaderName = "论坛勋章列表";
            this.DataGrid1.DataKeyField = "id";
            this.DataGrid1.PageSize = 15;
            this.DataGrid1.ColumnSpan = 6;
        }
    }
}