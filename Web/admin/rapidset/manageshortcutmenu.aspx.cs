using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Xml;
using BBX.Common;
using BBX.Common.Xml;
using BBX.Config;
using BBX.Forum;

namespace BBX.Web.Admin
{
    public partial class manageshortcutmenu : AdminPage
    {
        private string configPath;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.configPath = Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "admin/xml/navmenu.config");
            if (!base.IsPostBack)
            {
                this.BindDataGrid();
            }
        }

        private void BindDataGrid()
        {
            this.DataGrid1.TableHeaderName = "快捷菜单管理";
            XmlDocumentExtender xmlDocumentExtender = new XmlDocumentExtender();
            xmlDocumentExtender.Load(this.configPath);
            XmlNodeList xmlNodeList = xmlDocumentExtender.SelectNodes("/dataset/mainmenu");
            XmlNodeList xmlNodeList2 = xmlDocumentExtender.SelectNodes("/dataset/toptabmenu");
            string[] array = new string[xmlNodeList.Count];
            for (int i = 0; i < array.Length; i++)
            {
                foreach (XmlNode xmlNode in xmlNodeList2)
                {
                    if (("," + xmlNode["mainmenulist"].InnerText + ",").IndexOf("," + xmlNodeList[i].SelectSingleNode("id").InnerText + ",") != -1)
                    {
                        array[i] = xmlNode["title"].InnerText + "->" + xmlNodeList[i].SelectSingleNode("menutitle").InnerText;
                        break;
                    }
                }
            }
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add(new DataColumn("id"));
            dataTable.Columns.Add(new DataColumn("local"));
            XmlNodeList xmlNodeList3 = xmlDocumentExtender.SelectNodes("/dataset/shortcut");
            XmlNodeInnerTextVisitor xmlNodeInnerTextVisitor = new XmlNodeInnerTextVisitor();
            foreach (XmlNode node in xmlNodeList3)
            {
                xmlNodeInnerTextVisitor.SetNode(node);
                DataRow dataRow = dataTable.NewRow();
                dataRow["id"] = xmlNodeInnerTextVisitor["link"];
                dataRow["local"] = array[int.Parse(xmlNodeInnerTextVisitor["showmenuid"]) - 1] + "->" + xmlNodeInnerTextVisitor["menutitle"];
                dataTable.Rows.Add(dataRow);
            }
            if (dataTable.Rows.Count == 0)
            {
                DataRow dataRow2 = dataTable.NewRow();
                dataRow2["id"] = "";
                dataRow2["local"] = "(暂无收藏)";
                dataTable.Rows.Add(dataRow2);
            }
            this.DataGrid1.DataSource = dataTable;
            this.DataGrid1.DataBind();
        }

        protected void DataGrid1_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            int itemIndex = e.Item.ItemIndex;
            XmlDocumentExtender xmlDocumentExtender = new XmlDocumentExtender();
            xmlDocumentExtender.Load(this.configPath);
            XmlNodeList xmlNodeList = xmlDocumentExtender.SelectNodes("/dataset/shortcut");
            int num = 0;
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                if (num == itemIndex)
                {
                    xmlNode.ParentNode.RemoveChild(xmlNode);
                }
                num++;
            }
            xmlDocumentExtender.Save(this.configPath);
            MenuManage.CreateMenuJson();
            base.RegisterStartupScript("delete", "<script type='text/javascript'>window.parent.LoadShortcutMenu();</script>");
            this.BindDataGrid();
        }

        protected void DataGrid1_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.Cells[0].Text == "(暂无收藏)")
            {
                e.Item.Cells[1].Controls.Remove(e.Item.Cells[1].Controls[0]);
            }
        }
    }
}