using System;
using System.Data;
using System.IO;
using BBX.Common;
using BBX.Config;
using BBX.Forum;

namespace BBX.Web.Admin
{
    public partial class managemenubackupfile : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                this.BindDataGrid();
            }
            if (Request["filename"] != "")
            {
                Utils.RestoreFile(Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "admin/xml/backup/" + Request["filename"]), Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "admin/xml/navmenu.config"));
                MenuManage.CreateMenuJson();
                base.RegisterStartupScript("", "<script>alert('恢复成功！');window.location.href='managemainmenu.aspx';</script>");
            }
        }

        private void BindDataGrid()
        {
            this.DataGrid1.TableHeaderName = "菜单备份管理";
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("backupname");
            dataTable.Columns.Add("backupdate");
            string[] files = Directory.GetFiles(base.Server.MapPath("../xml/backup"), "*.config");
            for (int i = files.Length - 1; i >= 0; i--)
            {
                string fileName = Path.GetFileName(files[i]);
                DataRow dataRow = dataTable.NewRow();
                dataRow["backupname"] = fileName;
                dataRow["backupdate"] = Path.GetFileNameWithoutExtension(fileName).Replace("_", ":");
                dataTable.Rows.Add(dataRow);
            }
            this.DataGrid1.DataSource = dataTable;
            this.DataGrid1.DataKeyField = "backupname";
            this.DataGrid1.DataBind();
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            base.Load += new EventHandler(this.Page_Load);
        }

        protected void Delbackupfile_Click(object sender, EventArgs e)
        {
            string @string = Request["backupname"];
            if (@string == "")
            {
                base.RegisterStartupScript("", "<script>alert('未选中任何记录！');</script>");
                return;
            }
            string[] array = @string.Split(',');
            for (int i = 0; i < array.Length; i++)
            {
                string str = array[i];
                File.Delete(base.Server.MapPath("../xml/backup/" + str));
            }
            base.RegisterStartupScript("", "<script>alert('删除成功！');window.location.href='managemenubackupfile.aspx';</script>");
        }

        protected void backupfile_Click(object sender, EventArgs e)
        {
            MenuManage.BackupMenuFile();
            base.RegisterStartupScript("", "<script>alert('备份成功！');window.location.href='managemenubackupfile.aspx';</script>");
        }
    }
}