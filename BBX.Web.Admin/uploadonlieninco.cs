using System;
using System.Data;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BBX.Cache;
using BBX.Control;

namespace BBX.Web.Admin
{
    public class uploadonlieninco : AdminPage
    {
        protected HtmlForm Form1;
        protected Repeater incolist;
        protected UpFile image;
        protected BBX.Control.Button UpdateOnLineIncoCache;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.incolist.DataSource = this.LoadDataInfo();
                this.incolist.DataBind();
            }
        }

        public DataTable LoadDataInfo()
        {
            this.image.UpFilePath = base.Server.MapPath(this.image.UpFilePath);
            DataTable dataTable = new DataTable("img");
            dataTable.Columns.Add("imgfile", typeof(String));
            DirectoryInfo directoryInfo = new DirectoryInfo(base.Server.MapPath("../../images/groupicons"));
            FileSystemInfo[] fileSystemInfos = directoryInfo.GetFileSystemInfos();
            for (int i = 0; i < fileSystemInfos.Length; i++)
            {
                FileSystemInfo fileSystemInfo = fileSystemInfos[i];
                if (fileSystemInfo != null && fileSystemInfo.Extension != "" && (fileSystemInfo.Extension.ToLower() == ".jpg" || fileSystemInfo.Extension.ToLower() == ".gif" || fileSystemInfo.Extension.ToLower() == ".png" || fileSystemInfo.Extension.ToLower() == ".jpeg"))
                {
                    DataRow dataRow = dataTable.NewRow();
                    dataRow["imgfile"] = fileSystemInfo.Name;
                    dataTable.Rows.Add(dataRow);
                }
            }
            return dataTable;
        }

        private void UpdateOnLineIncoCache_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                //XCache.Remove(CacheKeys.FORUM_UI_ONLINE_ICON_LIST);
                this.image.UpdateFile();
                base.RegisterStartupScript("PAGE", "window.location.href='global_onlinelistgrid.aspx';");
            }
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.UpdateOnLineIncoCache.Click += new EventHandler(this.UpdateOnLineIncoCache_Click);
        }
    }
}