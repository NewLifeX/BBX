using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;

using BBX.Common.Xml;
using BBX.Config;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web.Admin
{
    public class aggregation_recommendtopic : AdminPage
    {
        protected HtmlForm Form1;
        protected BBX.Control.DataGrid DataGrid1;
        protected BBX.Control.Button Btn_SaveInfo;
        private string configPath;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                this.LoadInfo();
            }
        }

        private void LoadInfo()
        {
            XmlDocumentExtender xmlDocumentExtender = new XmlDocumentExtender();
            xmlDocumentExtender.Load(this.configPath);
            string innerText = xmlDocumentExtender.SelectSingleNode("/Aggregationinfo/Aggregationpage/Website/Website_forumrecomend/fidlist").InnerText;
            XmlNodeList xmlNodeList = xmlDocumentExtender.SelectNodes("/Aggregationinfo/Aggregationpage/Website/Website_forumrecomendtopiclist/Website_forumrecomendtopic");
            List<IXForum> forumList = Forums.GetForumList();
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("fid");
            dataTable.Columns.Add("name");
            dataTable.Columns.Add("tid");
            dataTable.Columns.Add("title");
            dataTable.Columns.Add("img");
            string[] array = innerText.Split(',');
            for (int i = 0; i < array.Length; i++)
            {
                string text = array[i];
                foreach (var current in forumList)
                {
                    if (current.Fid.ToString() == text)
                    {
                        DataRow dataRow = dataTable.NewRow();
                        dataRow["fid"] = text;
                        dataRow["name"] = current.Name;
                        dataRow["tid"] = "";
                        dataRow["title"] = "";
                        dataRow["img"] = "";
                        dataTable.Rows.Add(dataRow);
                        break;
                    }
                }
            }
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                foreach (DataRow dataRow2 in dataTable.Rows)
                {
                    if (xmlNode["fid"].InnerText == dataRow2["fid"].ToString())
                    {
                        dataRow2["tid"] = xmlNode["tid"].InnerText;
                        dataRow2["title"] = xmlNode["title"].InnerText;
                        dataRow2["img"] = xmlNode["img"].InnerText;
                        break;
                    }
                }
            }
            this.DataGrid1.TableHeaderName = "推荐版块图片选择";
            this.DataGrid1.DataKeyField = "fid";
            this.DataGrid1.DataSource = dataTable;
            this.DataGrid1.DataBind();
        }

        protected void DataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                System.Web.UI.WebControls.TextBox textBox = (System.Web.UI.WebControls.TextBox)e.Item.Cells[1].Controls[0];
                textBox.Attributes.Add("size", "5");
                textBox.Width = 60;
                textBox = (System.Web.UI.WebControls.TextBox)e.Item.Cells[2].Controls[0];
                textBox.Width = 200;
                textBox = (System.Web.UI.WebControls.TextBox)e.Item.Cells[3].Controls[0];
                textBox.Width = 200;
            }
        }

        private void Btn_SaveInfo_Click(object sender, EventArgs e)
        {
            XmlDocumentExtender xmlDocumentExtender = new XmlDocumentExtender();
            xmlDocumentExtender.Load(this.configPath);
            int num = 0;
            XmlNode xmlNode = xmlDocumentExtender.InitializeNode("/Aggregationinfo/Aggregationpage/Website/Website_forumrecomendtopiclist");
            foreach (object current in this.DataGrid1.GetKeyIDArray())
            {
                string childElementValue = current.ToString();
                string controlValue = this.DataGrid1.GetControlValue(num, "tid");
                string controlValue2 = this.DataGrid1.GetControlValue(num, "title");
                string controlValue3 = this.DataGrid1.GetControlValue(num, "img");
                XmlElement newChild = xmlDocumentExtender.CreateElement("Website_forumrecomendtopic");
                xmlDocumentExtender.AppendChildElementByNameValue(ref newChild, "fid", childElementValue);
                xmlDocumentExtender.AppendChildElementByNameValue(ref newChild, "tid", controlValue);
                xmlDocumentExtender.AppendChildElementByNameValue(ref newChild, "img", controlValue3);
                xmlDocumentExtender.AppendChildElementByNameValue(ref newChild, "title", controlValue2);
                xmlNode.AppendChild(newChild);
                num++;
            }
            xmlDocumentExtender.Save(this.configPath);
            base.Response.Redirect("aggregation_recommendtopic.aspx");
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.Btn_SaveInfo.Click += new EventHandler(this.Btn_SaveInfo_Click);
            this.configPath = base.Server.MapPath(BaseConfigs.GetForumPath + "config/aggregation.config");
        }
    }
}