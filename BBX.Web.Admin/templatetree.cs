using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BBX.Common;
using BBX.Config;
using BBX.Control;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web.Admin
{
    public class templatetree : AdminPage
    {
        protected HtmlForm Form1;
        protected pageinfo info1;
        protected pageinfo PageInfo1;
        protected TabControl TabControl1;
        protected TabPage tabPage22;
        protected BBX.Control.CheckBoxList TreeView1;
        protected BBX.Control.Button CreateTemplate;
        protected BBX.Control.Button DeleteTemplateFile;
        protected TabPage tabPage33;
        protected Repeater TreeView2;
        protected Label lblClientSideCheck;
        protected Label lblCheckedNodes;
        protected Label lblServerSideCheck;
        private string skinpath;
        private int templateCounter = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.skinpath = Request["path"];
            this.DeleteTemplateFile.Enabled = (this.CreateTemplate.Enabled = (this.TreeView1.SelectedIndex != -1));
        }

        private DataTable LoadTemplateFileDT()
        {
            DataTable dataTable = new DataTable("templatefilelist");
            dataTable.Columns.Add("fullfilename", typeof(String));
            dataTable.Columns.Add("filename", typeof(String));
            dataTable.Columns.Add("id", typeof(Int32));
            dataTable.Columns.Add("extension", typeof(String));
            dataTable.Columns.Add("parentid", typeof(String));
            dataTable.Columns.Add("filepath", typeof(String));
            dataTable.Columns.Add("filedescription", typeof(String));
            var skin = Request["path"];
            this.CreateTemplateFileList(dataTable, "default", false);
            if (skin.ToLower() != "defalut")
            {
                this.CreateTemplateFileList(dataTable, skin, true);
            }
            foreach (DataRow dataRow in dataTable.Rows)
            {
                DataRow[] array = dataTable.Select("filename like '" + dataRow["filename"] + "_%%'");
                for (int i = 0; i < array.Length; i++)
                {
                    DataRow dataRow2 = array[i];
                    if (dataRow["filename"].ToString() != dataRow2["filename"].ToString())
                    {
                        dataRow2["parentid"] = dataRow["id"].ToString();
                    }
                }
            }
            return dataTable;
        }

        private void CreateTemplateFileList(DataTable templateFileList, string skin, bool resetList)
        {
            var di = GetTemp(skin).AsDirectory();
            foreach (var fi in di.GetFileSystemInfos())
            {
                if (fi != null && !(fi.Name == "images"))
                {
                    if (fi.Attributes == FileAttributes.Directory)
                    {
                        this.CreateTemplateFileList(templateFileList, skin + "\\" + fi.Name, resetList);
                    }
                    else
                    {
                        if (fi.Name.ToLower().EndsWith(".htm"))
                        {
                            if (resetList)
                            {
                                DataRow[] array = templateFileList.Select("filename='" + fi.Name.Substring(0, fi.Name.LastIndexOf(".")) + "'");
                                if (array.Length != 0)
                                {
                                    array[0]["filename"] = fi.Name.Substring(0, fi.Name.LastIndexOf("."));
                                    array[0]["fullfilename"] = skin + "\\" + array[0]["filename"] + fi.Extension.ToLower();
                                    array[0]["extension"] = fi.Extension.ToLower();
                                    array[0]["filepath"] = skin;
                                }
                            }
                            else
                            {
                                DataRow dataRow = templateFileList.NewRow();
                                dataRow["id"] = this.templateCounter;
                                dataRow["filename"] = fi.Name.Substring(0, fi.Name.LastIndexOf("."));
                                dataRow["fullfilename"] = skin + "\\" + dataRow["filename"] + fi.Extension.ToLower();
                                dataRow["extension"] = fi.Extension.ToLower();
                                dataRow["parentid"] = "0";
                                dataRow["filepath"] = skin;
                                dataRow["filedescription"] = "";
                                templateFileList.Rows.Add(dataRow);
                                this.templateCounter++;
                            }
                        }
                    }
                }
            }
        }

        private DataTable LoadOtherFileDT()
        {
            var dataTable = new DataTable("otherfilelist");
            dataTable.Columns.Add("id", typeof(Int32));
            dataTable.Columns.Add("filename", typeof(String));
            dataTable.Columns.Add("extension", typeof(String));
            dataTable.Columns.Add("parentid", typeof(String));
            dataTable.Columns.Add("filepath", typeof(String));
            dataTable.Columns.Add("filedescription", typeof(String));
            var skin = Request["path"];
            int num = 1;
            var di = GetTemp(skin).AsDirectory();
            foreach (var fi in di.GetFileSystemInfos())
            {
                string text = fi.Extension.ToLower();
                if (text.IndexOf("js") > 0 || text.IndexOf("css") > 0 || text.IndexOf("xml") > 0 || text.IndexOf(".html") > 0)
                {
                    DataRow dataRow = dataTable.NewRow();
                    dataRow["id"] = num;
                    dataRow["filename"] = fi.Name.Substring(0, fi.Name.LastIndexOf("."));
                    dataRow["extension"] = fi.Extension.ToLower();
                    dataRow["parentid"] = "0";
                    dataRow["filepath"] = skin;
                    dataRow["filedescription"] = "";
                    dataTable.Rows.Add(dataRow);
                    num++;
                }
            }
            foreach (DataRow dataRow2 in dataTable.Rows)
            {
                DataRow[] array = dataTable.Select("filename like '" + dataRow2["filename"] + "_%%'");
                for (int j = 0; j < array.Length; j++)
                {
                    DataRow dataRow3 = array[j];
                    if (dataRow2["filename"].ToString() != dataRow3["filename"].ToString())
                    {
                        dataRow3["parentid"] = dataRow2["id"].ToString();
                    }
                }
            }
            foreach (DataRow dataRow4 in dataTable.Rows)
            {
                string text2 = dataRow4["extension"].ToString().Substring(1);
                dataRow4["filename"] = "<img src=\"../images/" + text2 + ".gif\" border=\"0\"> <a href=\"global_templatesedit.aspx?path=" + dataRow4["filepath"].ToString().Replace(" ", "%20") + "&filename=" + dataRow4["filename"] + dataRow4["extension"] + "&templateid=" + Request["templateid"] + "&templatename=" + Request["templatename"].Replace(" ", "%20") + "\" title=\"" + text2 + "文件\">" + dataRow4["filename"].ToString().Trim() + "</a>";
            }
            return dataTable;
        }

        protected void DeleteTemplateFile_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                string selectString = this.TreeView1.GetSelectString(",");
                if (String.IsNullOrEmpty(selectString))
                {
                    base.RegisterStartupScript("", "<script>alert('您未选中任何模板');</script>");
                    return;
                }
                try
                {
                    string[] array = selectString.Split(',');
                    for (int i = 0; i < array.Length; i++)
                    {
                        string filename = array[i];
                        this.DeleteFile(filename);
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    base.RegisterStartupScript("", "<script>alert('您的目录设置了权限导致无法在此删除此文件');</script>");
                    return;
                }
                base.RegisterStartupScript("PAGE", "window.location.href='global_templatetree.aspx?templateid=" + base.Request.Params["templateid"] + "&path=" + base.Request.Params["path"] + "&templatename=" + base.Request.Params["templatename"] + "';");
            }
        }

        public bool DeleteFile(string filename)
        {
            var fi = GetTemp(filename).AsFile();
            if (!fi.Exists) return false;

            fi.Delete();
            return true;
        }

        protected void CreateTemplate_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                string text = this.TreeView1.GetSelectString(",");
                if (String.IsNullOrEmpty(text))
                {
                    base.RegisterStartupScript("", "<script>alert('您未选中任何模板');</script>");
                    return;
                }
                if (String.IsNullOrEmpty(Request["chkall"]) && text.Contains("_"))
                {
                    text = this.RemadeTemplatePathList(text);
                }
                int tid = DNTRequest.GetInt("templateid", 1);
                var tmp = Template.FindByID(tid);
                int num = 0;
                var fpt = new ForumPageTemplate();
                string[] array = text.Split(',');
                for (int i = 0; i < array.Length; i++)
                {
                    string file = array[i];
                    string text3 = Path.GetFileName(file).ToLower();
                    string ext = Path.GetExtension(text3);
                    if ((ext.Equals(".htm") || ext.Equals(".config")) && !text3.Contains("_"))
                    {
                        string subdir = "";
                        if (file.Split('\\').Length >= 3)
                        {
                            subdir = Path.GetDirectoryName(file).Substring("\\");
                        }
                        fpt.GetTemplate(BaseConfigs.GetForumPath, this.skinpath, Path.GetFileNameWithoutExtension(text3), subdir, 1, tmp.Name);
                        num++;
                    }
                }
                base.RegisterStartupScript("PAGETemplate", "共" + num + " 个模板已更新");
            }
        }

        private string RemadeTemplatePathList(string templatePathList)
        {
            if (!templatePathList.Contains("\\_")) return templatePathList;

            //var sb = new StringBuilder();
            var list = new List<String>();
            var names = new List<String>();
            string[] array = templatePathList.Split(',');
            for (int i = 0; i < array.Length; i++)
            {
                string text = array[i];
                string name = Path.GetFileName(text).ToLower();
                string str = Path.GetFileNameWithoutExtension(name).ToLower();
                string ext = Path.GetExtension(name).ToLower();
                if (ext.Equals(".htm") || ext.Equals(".config"))
                {
                    if (!name.StartsWith("_"))
                    {
                        if (!names.Contains(name))
                        {
                            names.Add(name);
                            list.Add(text);
                        }
                    }
                    else
                    {
                        string value = "<%template " + str + "%>";
                        foreach (var fi in Directory.GetFiles(GetTemp("default")))
                        {
                            string fileName = Path.GetFileName(fi);
                            if (!fileName.StartsWith("_") && (Path.GetExtension(fileName) == ".htm" || Path.GetExtension(fileName) == ".config"))
                            {
                                if (File.ReadAllText(fi).Contains(value) && !names.Contains(fileName))
                                {
                                    names.Add(fileName);
                                    if (File.Exists(Utils.GetMapPath(GetTemp(skinpath).CombinePath(fileName))))
                                        list.Add(skinpath + "\\" + fileName + ",");
                                    else
                                        list.Add("default\\" + fileName + ",");
                                }
                            }
                        }
                    }
                }
            }
            return list.Join(",");
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            var dataTable = this.LoadTemplateFileDT();
            string tid = Request["templateid"];
            string text = Request["templatename"].Replace(" ", "%20");
            foreach (DataRow dataRow in dataTable.Rows)
            {
                string text2 = dataRow["extension"].ToString().Substring(1);
                dataRow["filename"] = string.Format("<img src=../images/{0}.gif border=\"0\"  style=\"position:relative;top:5 px;height:16 px\"> {1} <a href=\"global_templatesedit.aspx?path={2}&filename={1}{3}&templateid={4}&templatename={5}\" title=\"编辑{1}.{0}模板文件\"><img src='../images/editfile.gif' border='0'/></a>", new object[]
				{
					text2,
					dataRow["filename"].ToString().Trim(),
					dataRow["filepath"].ToString().Replace(" ", "%20"),
					dataRow["extension"].ToString().Trim(),
					tid,
					text
				});
            }
            this.TreeView1.AddTableData(dataTable);
            for (int i = 0; i < this.TreeView1.Items.Count; i++)
            {
                this.TreeView1.Items[i].Attributes.Add("onclick", "checkedEnabledButton1(form,'TabControl1:tabPage22:CreateTemplate','TabControl1:tabPage22:DeleteTemplateFile')");
                this.TreeView1.Items[i].Attributes.Add("value", this.TreeView1.Items[i].Value);
            }
            this.TreeView2.DataSource = this.LoadOtherFileDT();
            //this.TreeView2.DataBind();
        }

        String GetTemp(String skin)
        {
            return BaseConfigs.GetForumPath.CombinePath("templates", skin).GetFullPath();
        }
    }
}