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
    public class managesubmenu : AdminPage
    {
        protected HtmlForm Form1;
        protected pageinfo info1;
        protected DataGrid DataGrid1;
        private string configPath;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.configPath = Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "admin/xml/navmenu.config");
            string @string = Request["menuid"];
            string string2 = Request["submenuid"];
            string string3 = Request["mode"];
            if (string2 != "")
            {
                if (string3 == "del")
                {
                    MenuManage.DeleteSubMenu(int.Parse(string2), int.Parse(@string));
                }
                else
                {
                    if (string2 == "0")
                    {
                        MenuManage.NewSubMenu(int.Parse(@string), Request["menutitle"]);
                    }
                    else
                    {
                        MenuManage.EditSubMenu(int.Parse(string2), Request["menutitle"]);
                    }
                }
                base.Response.Redirect("managesubmenu.aspx?menuid=" + @string, true);
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