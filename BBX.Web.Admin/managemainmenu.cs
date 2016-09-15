using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Xml;
using Discuz.Common;
using Discuz.Common.Xml;
using Discuz.Config;
using Discuz.Control;
using Discuz.Forum;

namespace Discuz.Web.Admin
{
    public class managemainmenu : AdminPage
    {
        protected HtmlForm form1;
        protected pageinfo info1;
        protected DataGrid DataGrid1;
        protected Button createMenu;
        private string configPath;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.configPath = Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "admin/xml/navmenu.config");
            string menuid = Request["menuid"];
            string mode = Request["mode"];
            if (mode != "")
            {
                if (mode == "del")
                {
                    MenuManage.DeleteMainMenu(int.Parse(menuid));
                }
                else
                {
                    if (menuid == "0")
                    {
                        MenuManage.NewMainMenu(Request["menutitle"], Request["defaulturl"]);
                    }
                    else
                    {
                        MenuManage.EditMainMenu(int.Parse(menuid), Request["menutitle"], Request["defaulturl"]);
                    }
                }
                base.Response.Redirect("managemainmenu.aspx", true);
                return;
            }
            this.BindDataGrid();
        }

        private void BindDataGrid()
        {
            this.DataGrid1.TableHeaderName = "菜单管理";
            XmlDocumentExtender xmlDocumentExtender = new XmlDocumentExtender();
            xmlDocumentExtender.Load(this.configPath);
            XmlNodeList xmlNodeList = xmlDocumentExtender.SelectNodes("/dataset/toptabmenu");
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add(new DataColumn("id"));
            dataTable.Columns.Add(new DataColumn("title"));
            dataTable.Columns.Add(new DataColumn("defaulturl"));
            dataTable.Columns.Add(new DataColumn("system"));
            dataTable.Columns.Add(new DataColumn("delitem"));
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                DataRow dataRow = dataTable.NewRow();
                dataRow["id"] = xmlNode["id"].InnerText;
                dataRow["title"] = xmlNode["title"].InnerText;
                dataRow["defaulturl"] = xmlNode["defaulturl"].InnerText;
                dataRow["system"] = ((xmlNode["system"].InnerText != "0") ? "是" : "否");
                if (xmlNode["mainmenulist"].InnerText != "")
                {
                    dataRow["delitem"] = "删除";
                }
                else
                {
                    dataRow["delitem"] = "<a href='managemainmenu.aspx?mode=del&menuid=" + xmlNode["id"].InnerText + "' onclick='return confirm(\"您确认要删除此菜单项吗?\")'>删除</a>";
                }
                dataTable.Rows.Add(dataRow);
            }
            this.DataGrid1.DataSource = dataTable;
            this.DataGrid1.DataBind();
        }

        protected void createMenu_Click(object sender, EventArgs e)
        {
            MenuManage.CreateMenuJson();
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.createMenu.Click += new EventHandler(this.createMenu_Click);
        }
    }
}