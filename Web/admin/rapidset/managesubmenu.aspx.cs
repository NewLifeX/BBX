using System;
using System.Data;
using System.Xml;
using BBX.Common;
using BBX.Common.Xml;
using BBX.Config;
using BBX.Forum;

namespace BBX.Web.Admin
{
    public partial class managesubmenu : AdminPage
    {
        private string configPath;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.configPath = Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "admin/xml/navmenu.config");
            string menuid = Request["menuid"];
            string submenuid = Request["submenuid"];
            string mode = Request["mode"];
            if (!String.IsNullOrEmpty(submenuid))
            {
                if (mode == "del")
                {
                    MenuManage.DeleteSubMenu(int.Parse(submenuid), int.Parse(menuid));
                }
                else
                {
                    if (submenuid == "0")
                    {
                        MenuManage.NewSubMenu(int.Parse(menuid), Request["menutitle"]);
                    }
                    else
                    {
                        MenuManage.EditSubMenu(int.Parse(submenuid), Request["menutitle"]);
                    }
                }
                base.Response.Redirect("managesubmenu.aspx?menuid=" + menuid, true);
                return;
            }
            if (!this.Page.IsPostBack)
            {
                this.BindData();
            }
        }

        public void BindData()
        {
            string @string = Request["menuid"];
            string str = "";
            string text = "";
            XmlDocumentExtender xmlDocumentExtender = new XmlDocumentExtender();
            xmlDocumentExtender.Load(this.configPath);
            XmlNodeList xmlNodeList = xmlDocumentExtender.SelectNodes("/dataset/toptabmenu");
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                if (xmlNode["id"].InnerText == @string)
                {
                    str = xmlNode["title"].InnerText;
                    text = xmlNode["mainmenulist"].InnerText;
                    break;
                }
            }
            this.DataGrid1.TableHeaderName = str + "  菜单项管理";
            XmlNodeList xmlNodeList2 = xmlDocumentExtender.SelectNodes("/dataset/mainmenu");
            DataTable dataTable = new DataTable();
            if (text == "")
            {
                this.DataGrid1.DataSource = dataTable;
                this.DataGrid1.DataBind();
                return;
            }
            dataTable.Columns.Add("id");
            dataTable.Columns.Add("menuid");
            dataTable.Columns.Add("submenuid");
            dataTable.Columns.Add("menutitle");
            dataTable.Columns.Add("delitem");
            foreach (XmlNode xmlNode2 in xmlNodeList2)
            {
                if (("," + text + ",").IndexOf("," + xmlNode2["id"].InnerText + ",") != -1)
                {
                    DataRow dataRow = dataTable.NewRow();
                    dataRow["id"] = xmlNode2["id"].InnerText;
                    dataRow["menuid"] = @string;
                    dataRow["submenuid"] = xmlNode2["menuid"].InnerText;
                    dataRow["menutitle"] = xmlNode2["menutitle"].InnerText;
                    if (this.FindSubMenuItem(xmlNode2["menuid"].InnerText, xmlDocumentExtender))
                    {
                        dataRow["delitem"] = "删除";
                    }
                    else
                    {
                        dataRow["delitem"] = "<a href='managesubmenu.aspx?mode=del&menuid=" + @string + "&submenuid=" + xmlNode2["id"].InnerText + "' onclick='return confirm(\"您确认要删除此菜单项吗？\");'>删除</a>";
                    }
                    dataTable.Rows.Add(dataRow);
                }
            }
            this.DataGrid1.DataSource = dataTable;
            this.DataGrid1.DataBind();
        }

        private bool FindSubMenuItem(string menuid, XmlDocumentExtender doc)
        {
            XmlNodeList xmlNodeList = doc.SelectNodes("/dataset/submain");
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                if (xmlNode["menuparentid"].InnerText == menuid)
                {
                    return true;
                }
            }
            return false;
        }
    }
}