using System;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI.HtmlControls;
using BBX.Common;
using BBX.Config;
using BBX.Control;
using BBX.Forum;

namespace BBX.Web.Admin
{
    public class backupandrestore : AdminPage
    {
        private delegate bool delegateBackUpDatabase(string ServerName, string UserName, string Password, string strDbName, string strFileName);

        protected HtmlForm Form1;
        protected Hint Hint1;
        protected DataGrid Grid1;
        protected Button Restore;
        protected Button DeleteBackup;
        protected TextBox ServerName;
        protected TextBox UserName;
        protected TextBox backupname;
        protected TextBox strDbName;
        protected TextBox Password;
        protected Button BackUP;
        private static string backuppath = HttpContext.Current.Server.MapPath("backup/");
        private backupandrestore.delegateBackUpDatabase aysncallback;

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Databases.IsBackupDatabase())
            //{
            //    if (!this.Page.IsPostBack)
            //    {
            //        if (!base.IsFounderUid(this.userid))
            //        {
            //            base.Response.Write(base.GetShowMessage());
            //            base.Response.End();
            //            return;
            //        }
            //        this.LoadDataBaseConnectString();
            //        this.Grid1.DataSource = this.buildGridData();
            //        this.Grid1.DataBind();
            //    }
            //    backupandrestore.backuppath = HttpContext.Current.Server.MapPath("backup/");
            //    return;
            //}
            base.Response.Write("<script>alert('您所使用的数据库不支持在线备份!');history.go(-1);</script>");
            base.Response.End();
        }

        private void LoadDataBaseConnectString()
        {
            string[] array = BaseConfigInfo.Current.Dbconnectstring.Split(';');
            for (int i = 0; i < array.Length; i++)
            {
                string text = array[i];
                if (text.ToLower().IndexOf("data source") >= 0 || text.ToLower().IndexOf("server") >= 0)
                {
                    this.ServerName.Text = text.Split('=')[1].Trim();
                }
                else
                {
                    if (text.ToLower().IndexOf("user id") >= 0 || text.ToLower().IndexOf("uid") >= 0)
                    {
                        this.UserName.Text = text.Split('=')[1].Trim();
                    }
                    else
                    {
                        if (text.ToLower().IndexOf("password") >= 0 || text.ToLower().IndexOf("pwd") >= 0)
                        {
                            this.Password.Text = text.Split('=')[1].Trim();
                        }
                        else
                        {
                            if (text.ToLower().IndexOf("initial catalog") >= 0 || text.ToLower().IndexOf("database") >= 0)
                            {
                                this.strDbName.Text = text.Split('=')[1].Trim();
                                return;
                            }
                        }
                    }
                }
            }
        }

        public DataTable buildGridData()
        {
            DataTable dataTable = new DataTable("templatefilelist");
            int num = 1;
            dataTable.Columns.Add("id", typeof(Int32));
            dataTable.Columns.Add("filename", typeof(String));
            dataTable.Columns.Add("createtime", typeof(String));
            dataTable.Columns.Add("fullname", typeof(String));
            FileSystemInfo[] fileSystemInfos = new DirectoryInfo(base.Server.MapPath("backup")).GetFileSystemInfos();
            for (int i = 0; i < fileSystemInfos.Length; i++)
            {
                FileSystemInfo fileSystemInfo = fileSystemInfos[i];
                if (fileSystemInfo.Extension == ".config")
                {
                    DataRow dataRow = dataTable.NewRow();
                    dataRow["id"] = num++;
                    dataRow["filename"] = fileSystemInfo.Name.Substring(0, fileSystemInfo.Name.LastIndexOf("."));
                    dataRow["createtime"] = fileSystemInfo.CreationTime.ToString();
                    dataRow["fullname"] = "backup/" + fileSystemInfo.Name;
                    dataTable.Rows.Add(dataRow);
                }
            }
            return dataTable;
        }

        public bool BackUPDB(string ServerName, string UserName, string Password, string strDbName, string strFileName)
        {
            //string text = Databases.BackUpDatabase(backupandrestore.backuppath, ServerName, UserName, Password, strDbName, strFileName);
            //if (text != "")
            //{
            //    base.RegisterStartupScript("", this.GetMessageScript("备份数据库失败,原因:" + text + "!"));
            //    return false;
            //}
            return true;
        }

        public bool RestoreDB(string ServerName, string UserName, string Password, string strDbName, string strFileName)
        {
            //string text = Databases.RestoreDatabase(backupandrestore.backuppath, ServerName, UserName, Password, strDbName, strFileName);
            //if (text != string.Empty)
            //{
            //    base.RegisterStartupScript("", this.GetMessageScript("恢复数据库失败,原因:" + text + "!"));
            //    return false;
            //}
            return true;
        }

        private void BackUP_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                if (!base.IsFounderUid(this.userid))
                {
                    base.Response.Write(base.GetShowMessage());
                    base.Response.End();
                    return;
                }
                if (String.IsNullOrEmpty(this.backupname.Text))
                {
                    base.RegisterStartupScript("PAGE", "alert('备份名称不能为空');");
                    return;
                }
                this.aysncallback = new backupandrestore.delegateBackUpDatabase(this.BackUPDB);
                AsyncCallback callback = new AsyncCallback(this.CallBack);
                this.aysncallback.BeginInvoke(this.ServerName.Text, this.UserName.Text, this.Password.Text, this.strDbName.Text, this.backupname.Text, callback, this.username);
                base.LoadRegisterStartupScript("PAGE", "window.location.href='global_backupandrestore.aspx';");
            }
        }

        public void CallBack(IAsyncResult e)
        {
            this.aysncallback.EndInvoke(e);
        }

        public string GetHttpLink(string filename)
        {
            return "<a href=" + filename + ">下载</a>";
        }

        private void Restore_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                if (!base.IsFounderUid(this.userid))
                {
                    base.Response.Write(base.GetShowMessage());
                    base.Response.End();
                    return;
                }
                if (Request["id"] != "")
                {
                    string @string = Request["id"];
                    if (@string.IndexOf(",0") > 0)
                    {
                        base.RegisterStartupScript("", this.GetMessageScript("您一次只能选择一个备份进行提交"));
                        return;
                    }
                    DataRow[] array = this.buildGridData().Select("id=" + @string.Replace("0 ", ""));
                    this.aysncallback = new backupandrestore.delegateBackUpDatabase(this.RestoreDB);
                    AsyncCallback callback = new AsyncCallback(this.CallBack);
                    this.aysncallback.BeginInvoke(this.ServerName.Text, this.UserName.Text, this.Password.Text, this.strDbName.Text, array[0]["filename"].ToString(), callback, this.username);
                    base.LoadRegisterStartupScript("PAGE", "window.location.href='global_backupandrestore.aspx';");
                    return;
                }
                else
                {
                    base.RegisterStartupScript("", this.GetMessageScript("您未选中任何选项"));
                }
            }
        }

        private string GetMessageScript(string msg)
        {
            return string.Format("<script>alert('{0}');window.location.href='global_backupandrestore.aspx';</script>", msg);
        }

        private void DeleteBackup_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(Request["id"]))
            {
                base.RegisterStartupScript("", this.GetMessageScript("您未选中任何选项"));
                return;
            }
            DataRow[] array = this.buildGridData().Select("id IN(" + Request["id"].Replace("0 ", "") + ")");
            for (int i = 0; i < array.Length; i++)
            {
                DataRow dataRow = array[i];
                File.Delete(Utils.GetMapPath(dataRow["fullname"].ToString()));
            }
            base.RegisterStartupScript("PAGE", "window.location.href='global_backupandrestore.aspx';");
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.BackUP.Click += new EventHandler(this.BackUP_Click);
            this.Restore.Click += new EventHandler(this.Restore_Click);
            this.DeleteBackup.Click += new EventHandler(this.DeleteBackup_Click);
            this.Grid1.TableHeaderName = "数据库备份列表";
        }
    }
}