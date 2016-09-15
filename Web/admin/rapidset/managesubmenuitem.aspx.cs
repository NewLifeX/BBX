using System;
using System.Data;
using System.Xml;
using BBX.Common;
using BBX.Common.Xml;
using BBX.Config;
using BBX.Forum;

namespace BBX.Web.Admin
{
    public partial class managesubmenuitem : AdminPage
    {
        private string configPath;
        public string menuid;
        public string submenuid;
        public string pagename;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.configPath = Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "admin/xml/navmenu.config");
            this.menuid = Request["menuid"];
            this.submenuid = Request["submenuid"];
            this.pagename = Request["pagename"];
            string id = Request["id"];
            string mode = Request["mode"];
            if (!String.IsNullOrEmpty(id))
            {
                if (mode == "del")
                {
                    MenuManage.DeleteMenuItem(int.Parse(id));
                }
                else
                {
                    if (id == "-1")
                    {
                        MenuManage.NewMenuItem(int.Parse(this.submenuid), Request["menutitle"], Request["link"]);
                    }
                    else
                    {
                        MenuManage.EditMenuItem(int.Parse(id), Request["menutitle"], Request["link"]);
                    }
                }
                base.Response.Redirect("managesubmenuitem.aspx?menuid=" + this.menuid + "&submenuid=" + this.submenuid + "&pagename=" + Request["pagename"], true);
                return;
            }
            if (!base.IsPostBack)
            {
                this.BindDataGrid();
            }
        }

        private void BindDataGrid()
        {
            this.DataGrid1.TableHeaderName = this.pagename + " 子菜单项管理";
            XmlDocumentExtender xmlDocumentExtender = new XmlDocumentExtender();
            xmlDocumentExtender.Load(this.configPath);
            string @string = Request["submenuid"];
            XmlNodeList xmlNodeList = xmlDocumentExtender.SelectNodes("/dataset/submain");
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add(new DataColumn("id"));
            dataTable.Columns.Add(new DataColumn("menutitle"));
            dataTable.Columns.Add(new DataColumn("link"));
            int num = 0;
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                if (xmlNode["menuparentid"].InnerText == @string)
                {
                    DataRow dataRow = dataTable.NewRow();
                    dataRow["id"] = num.ToString();
                    dataRow["menutitle"] = xmlNode["menutitle"].InnerText;
                    dataRow["link"] = xmlNode["link"].InnerText;
                    dataTable.Rows.Add(dataRow);
                }
                num++;
            }
            this.DataGrid1.DataSource = dataTable;
            this.DataGrid1.DataBind();
        }
    }
}