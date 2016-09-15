using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using BBX.Cache;
using BBX.Common;
using BBX.Common.Xml;
using BBX.Config;
using BBX.Control;
using BBX.Entity;

namespace BBX.Web.Admin
{
    public class forum_attchemnttypes : AdminPage
    {
        private DataTable att = new DataTable();
		//private DataTable dt;
        protected HtmlForm Form1;
        protected BBX.Control.DataGrid DataGrid1;
        protected BBX.Control.Button DelRec;
        protected BBX.Control.TextBox typename;
        protected BBX.Control.CheckBoxList attachextensions;
        protected BBX.Control.Button AddNewRec;
        protected Hint Hint1;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.att.Columns.Add("typeid");
            this.att.Columns.Add("typename");
            this.att.Columns.Add("extname");
            XmlDocumentExtender xmlDocumentExtender = new XmlDocumentExtender();
            xmlDocumentExtender.Load(base.Server.MapPath(BaseConfigs.GetForumPath + "config/myattachment.config"));
            XmlNodeList xmlNodeList = xmlDocumentExtender.SelectNodes("/MyAttachmentsTypeConfigInfo/attachtypes/AttachmentType");
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                DataRow dataRow = this.att.NewRow();
                dataRow["typeid"] = xmlNode["TypeId"].InnerText;
                dataRow["typename"] = xmlNode["TypeName"].InnerText;
                dataRow["extname"] = ((xmlNode["ExtName"].InnerText != "") ? xmlNode["ExtName"].InnerText : "无绑定类型");
                this.att.Rows.Add(dataRow);
            }
			//this.dt = Attachments.GetAttachmentType();
            //Request["typeid"];
            if (!this.Page.IsPostBack)
            {
                this.BindData();
                string text = "";
                if (this.att != null)
                {
                    foreach (DataRow dataRow2 in this.att.Rows)
                    {
                        text = text + dataRow2["extname"].ToString() + ",";
                    }
                    text = text.TrimEnd(',');
                }
				this.attachextensions.AddTableData(AttachType.FindAllWithCache());
                string[] array = text.Split(',');
                for (int i = 0; i < array.Length; i++)
                {
                    string a = array[i];
                    for (int j = 0; j < this.attachextensions.Items.Count; j++)
                    {
                        if (a == this.attachextensions.Items[j].Text)
                        {
                            this.attachextensions.Items[j].Enabled = false;
                            break;
                        }
                    }
                }
                string text2 = "var atttype = \r\n{";
                if (this.att != null)
                {
                    foreach (DataRow dataRow3 in this.att.Rows)
                    {
                        string text3 = text2;
                        text2 = text3 + "\r\n\ttype" + dataRow3["typeid"].ToString() + ":{typename:'" + dataRow3["typename"].ToString() + "',extname:'" + dataRow3["extname"].ToString() + "'},";
                    }
                    text2 = text2.TrimEnd(',');
                }
                text2 += "\r\n};";
                base.RegisterStartupScript("", "<script type='text/javascript'>\r\n" + text2 + "\r\n</script>");
            }
        }

        public void BindData()
        {
            this.DataGrid1.AllowCustomPaging = false;
            this.DataGrid1.TableHeaderName = "附件分类列表";
            if (this.att == null)
            {
                return;
            }
            this.DataGrid1.DataSource = this.att;
			this.DataGrid1.DataKeyField = "ID";
            this.DataGrid1.DataBind();
        }

        private void DataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.EditItem)
            {
                System.Web.UI.WebControls.TextBox textBox = (System.Web.UI.WebControls.TextBox)e.Item.Cells[4].Controls[0];
                textBox.Attributes.Add("maxlength", "254");
                textBox.Attributes.Add("size", "30");
                textBox = (System.Web.UI.WebControls.TextBox)e.Item.Cells[5].Controls[0];
                textBox.Attributes.Add("maxlength", "254");
                textBox.Attributes.Add("size", "30");
            }
        }

        private void DelRec_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                if (Request["typeid"] != "")
                {
                    string @string = Request["typeid"];
                    XmlDocumentExtender xmlDocumentExtender = new XmlDocumentExtender();
                    xmlDocumentExtender.Load(base.Server.MapPath(BaseConfigs.GetForumPath + "config/myattachment.config"));
                    XmlNodeList xmlNodeList = xmlDocumentExtender.SelectNodes("/MyAttachmentsTypeConfigInfo/attachtypes/AttachmentType");
                    string[] array = @string.Split(',');
                    for (int i = 0; i < array.Length; i++)
                    {
                        string a = array[i];
                        foreach (XmlNode xmlNode in xmlNodeList)
                        {
                            if (a == xmlNode["TypeId"].InnerText)
                            {
                                xmlNode.ParentNode.RemoveChild(xmlNode);
                                break;
                            }
                        }
                    }
                    xmlDocumentExtender.Save(base.Server.MapPath(BaseConfigs.GetForumPath + "config/myattachment.config"));
                    this.UpdateAttchmentTypes();
                    base.Response.Redirect("forum_attchemnttypes.aspx");
                    return;
                }
                base.RegisterStartupScript("", "<script>alert('您未选中任何选项');window.location.href='forum_attchemnttypes.aspx';</script>");
            }
        }

        private int GetMaxTypeid()
        {
            if (this.att == null || this.att.Rows.Count == 0)
            {
                return 0;
            }
            return int.Parse(this.att.Rows[this.att.Rows.Count - 1]["typeid"].ToString());
        }

        private string GetAttTypeList()
        {
            string text = "";
            for (int i = 0; i < this.attachextensions.Items.Count; i++)
            {
                text += ((DNTRequest.GetString("attachextensions:" + i) == "on") ? (this.attachextensions.Items[i].Text + ",") : "");
            }
            return text.TrimEnd(',');
        }

        private void AddNewRec_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.typename.Text))
            {
                base.RegisterStartupScript("", "<script>alert('附件分类名称不能为空!');window.location.href='forum_attchemnttypes.aspx';</script>");
                return;
            }
            XmlDocumentExtender xmlDocumentExtender = new XmlDocumentExtender();
            xmlDocumentExtender.Load(base.Server.MapPath(BaseConfigs.GetForumPath + "config/myattachment.config"));
            if (String.IsNullOrEmpty(Request["atttypeid"]))
            {
                XmlNode xmlNode = xmlDocumentExtender.SelectSingleNode("/MyAttachmentsTypeConfigInfo/attachtypes");
                XmlElement xmlElement = xmlDocumentExtender.CreateElement("AttachmentType");
                XmlElement xmlElement2 = xmlDocumentExtender.CreateElement("TypeId");
                int maxTypeid = this.GetMaxTypeid();
                xmlElement2.InnerText = (maxTypeid + 1).ToString();
                xmlElement.AppendChild(xmlElement2);
                xmlElement2 = xmlDocumentExtender.CreateElement("TypeName");
                xmlElement2.InnerText = this.typename.Text;
                xmlElement.AppendChild(xmlElement2);
                xmlElement2 = xmlDocumentExtender.CreateElement("ExtName");
                xmlElement2.InnerText = this.GetAttTypeList();
                xmlElement.AppendChild(xmlElement2);
                xmlNode.AppendChild(xmlElement);
            }
            else
            {
                XmlNodeList xmlNodeList = xmlDocumentExtender.SelectNodes("/MyAttachmentsTypeConfigInfo/attachtypes/AttachmentType");
                foreach (XmlNode xmlNode2 in xmlNodeList)
                {
                    if (xmlNode2["TypeId"].InnerText == Request["atttypeid"])
                    {
                        xmlNode2["TypeName"].InnerText = this.typename.Text;
                        xmlNode2["ExtName"].InnerText = this.GetAttTypeList();
                    }
                }
            }
            xmlDocumentExtender.Save(base.Server.MapPath(BaseConfigs.GetForumPath + "config/myattachment.config"));
            this.UpdateAttchmentTypes();
            base.RegisterStartupScript("", "<script>window.location.href='forum_attchemnttypes.aspx';</script>");
        }

        private void UpdateAttchmentTypes()
        {
            XCache.Remove("/Forum/MyAttachments");
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.AddNewRec.Click += new EventHandler(this.AddNewRec_Click);
            this.DelRec.Click += new EventHandler(this.DelRec_Click);
            this.DataGrid1.ItemDataBound += new DataGridItemEventHandler(this.DataGrid_ItemDataBound);
            this.DataGrid1.ColumnSpan = 5;
        }
    }
}