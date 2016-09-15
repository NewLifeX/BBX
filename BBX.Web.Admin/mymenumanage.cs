using System;
using System.Data;
using System.IO;
using System.Text;
using System.Web.UI.WebControls;
using System.Xml;
using BBX.Common;
using BBX.Common.Xml;
using BBX.Config;
using BBX.Control;

namespace BBX.Web.Admin
{
    public class mymenumanage : AdminPage
    {
        private string configPath;
        protected BBX.Control.Button DelRec;
        protected BBX.Control.Button addmenu;
        protected BBX.Control.Button SaveMyMenu;
        protected BBX.Control.DataGrid DataGrid1;
        protected BBX.Control.TextBox atext;
        protected BBX.Control.TextBox ahref;
        protected BBX.Control.TextBox aonclick;
        protected BBX.Control.TextBox atarget;
        protected Hint Hint1;

        private void Page_Load(object sender, EventArgs e)
        {
            this.atext.Attributes.Add("onkeyup", "setexample()");
            this.ahref.Attributes.Add("onkeyup", "setexample()");
            this.aonclick.Attributes.Add("onkeyup", "setexample()");
            this.atarget.Attributes.Add("onkeyup", "setexample()");
            if (!this.Page.IsPostBack)
            {
                this.BindData();
            }
        }

        public void BindData()
        {
            this.DataGrid1.AllowCustomPaging = false;
            this.DataGrid1.DataKeyField = "menuid";
            this.DataGrid1.TableHeaderName = "我的菜单列表";
            DataSet dataSet = new DataSet();
            XmlDocumentExtender xmlDocumentExtender = new XmlDocumentExtender();
            xmlDocumentExtender.Load(this.configPath);
            XmlNode xmlNode = xmlDocumentExtender.SelectSingleNode("/menuset");
            if (xmlNode == null || xmlNode.ChildNodes.Count == 0)
            {
                return;
            }
            XmlNodeReader reader = new XmlNodeReader(xmlNode);
            dataSet.ReadXml(reader);
            dataSet.Tables[0].Columns.Add("menuid");
            int num = 0;
            foreach (DataRow dataRow in dataSet.Tables[0].Rows)
            {
                dataRow["menuid"] = num.ToString();
                num++;
            }
            this.DataGrid1.DataSource = dataSet.Tables[0];
            this.DataGrid1.DataBind();
        }

        private void addmenu_Click(object sender, EventArgs e)
        {
            if (!this.atext.Text.IsNullOrEmpty() && !this.ahref.Text.IsNullOrEmpty())
            {
                XmlDocumentExtender xmlDocumentExtender = new XmlDocumentExtender();
                xmlDocumentExtender.Load(this.configPath);
                int num = 0;
                if (xmlDocumentExtender.SelectSingleNode("/menuset").ChildNodes.Count != 0)
                {
                    num = int.Parse(xmlDocumentExtender.SelectSingleNode("/menuset").LastChild["menuorder"].InnerText);
                }
                num++;
                XmlElement newChild = xmlDocumentExtender.CreateElement("menuitem");
                xmlDocumentExtender.AppendChildElementByNameValue(ref newChild, "menuorder", num.ToString());
                xmlDocumentExtender.AppendChildElementByNameValue(ref newChild, "text", this.atext.Text.Trim());
                xmlDocumentExtender.AppendChildElementByNameValue(ref newChild, "href", this.ahref.Text.Trim());
                xmlDocumentExtender.AppendChildElementByNameValue(ref newChild, "onclick", this.aonclick.Text.Trim(), true);
                xmlDocumentExtender.AppendChildElementByNameValue(ref newChild, "target", this.atarget.Text.Trim());
                xmlDocumentExtender.CreateNode("/menuset").AppendChild(newChild);
                xmlDocumentExtender.Save(this.configPath);
                this.CreateJsFile();
                try
                {
                    this.BindData();
                    this.ResetForm();
                    base.RegisterStartupScript("PAGE", "window.location.href='forum_mymenumanage.aspx';");
                    return;
                }
                catch
                {
                    base.RegisterStartupScript("", "<script>alert('无法更新XML文件');window.location.href='forum_mymenumanage.aspx';</script>");
                    return;
                }
            }
            base.RegisterStartupScript("", "<script>alert('链接文字和链接地址是必须输入的，如果无链接地址请输入\"#\".');window.location.href='forum_mymenumanage.aspx';</script>");
        }

        private void ResetForm()
        {
            this.atext.Text = "";
            this.ahref.Text = "";
            this.aonclick.Text = "";
            this.atarget.Text = "";
        }

        protected void Sort_Grid(object sender, DataGridSortCommandEventArgs e)
        {
            this.DataGrid1.Sort = e.SortExpression.ToString();
        }

        protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            this.DataGrid1.LoadCurrentPageIndex(e.NewPageIndex);
            this.BindData();
        }

        protected void SaveMyMenu_Click(object sender, EventArgs e)
        {
            int num = 0;
            bool flag = false;
            XmlDocumentExtender xmlDocumentExtender = new XmlDocumentExtender();
            xmlDocumentExtender.Load(this.configPath);
            XmlNodeList childNodes = xmlDocumentExtender.SelectSingleNode("/menuset").ChildNodes;
            if (childNodes != null && childNodes.Count > 0)
            {
                xmlDocumentExtender.InitializeNode("/menuset");
            }
            foreach (object arg_5D_0 in this.DataGrid1.GetKeyIDArray())
            {
                string controlValue = this.DataGrid1.GetControlValue(num, "menuorder");
                string controlValue2 = this.DataGrid1.GetControlValue(num, "text");
                string controlValue3 = this.DataGrid1.GetControlValue(num, "href");
                string controlValue4 = this.DataGrid1.GetControlValue(num, "onclick");
                string controlValue5 = this.DataGrid1.GetControlValue(num, "target");
                if (String.IsNullOrEmpty(controlValue2.Trim()) && String.IsNullOrEmpty(controlValue3.Trim()))
                {
                    flag = true;
                }
                else
                {
                    bool flag2 = false;
                    XmlElement newChild = xmlDocumentExtender.CreateElement("menuitem");
                    xmlDocumentExtender.AppendChildElementByNameValue(ref newChild, "menuorder", controlValue);
                    xmlDocumentExtender.AppendChildElementByNameValue(ref newChild, "text", controlValue2);
                    xmlDocumentExtender.AppendChildElementByNameValue(ref newChild, "href", controlValue3);
                    xmlDocumentExtender.AppendChildElementByNameValue(ref newChild, "onclick", controlValue4, true);
                    xmlDocumentExtender.AppendChildElementByNameValue(ref newChild, "target", controlValue5);
                    foreach (XmlNode xmlNode in childNodes)
                    {
                        if (int.Parse(xmlNode["menuorder"].InnerText) > int.Parse(controlValue))
                        {
                            xmlDocumentExtender.SelectSingleNode("/menuset").InsertBefore(newChild, xmlNode);
                            flag2 = true;
                            break;
                        }
                    }
                    if (!flag2)
                    {
                        xmlDocumentExtender.SelectSingleNode("/menuset").AppendChild(newChild);
                    }
                    num++;
                }
            }
            xmlDocumentExtender.Save(this.configPath);
            this.CreateJsFile();
            if (flag)
            {
                base.RegisterStartupScript("", "<script>alert('链接文字和链接地址是必须输入的，如果无链接地址请输入\"#\".');window.location.href='forum_mymenumanage.aspx';</script>");
                return;
            }
            base.RegisterStartupScript("", "<script>window.location.href='forum_mymenumanage.aspx';</script>");
        }

        private void DataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ((System.Web.UI.WebControls.TextBox)e.Item.Cells[1].Controls[0]).Width = 30;
                ((System.Web.UI.WebControls.TextBox)e.Item.Cells[2].Controls[0]).Width = 60;
                ((System.Web.UI.WebControls.TextBox)e.Item.Cells[3].Controls[0]).Width = 150;
                ((System.Web.UI.WebControls.TextBox)e.Item.Cells[4].Controls[0]).Width = 150;
                ((System.Web.UI.WebControls.TextBox)e.Item.Cells[5].Controls[0]).Width = 50;
            }
        }

        private void DelRec_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                string @string = Request["menuid"];
                if (@string != "")
                {
                    int num = 0;
                    XmlDocumentExtender xmlDocumentExtender = new XmlDocumentExtender();
                    xmlDocumentExtender.Load(this.configPath);
                    XmlNodeList childNodes = xmlDocumentExtender.SelectSingleNode("/menuset").ChildNodes;
                    string[] array = @string.Split(',');
                    for (int i = 0; i < array.Length; i++)
                    {
                        string s = array[i];
                        xmlDocumentExtender.SelectSingleNode("/menuset").RemoveChild(childNodes.Item(int.Parse(s) - num));
                        num++;
                    }
                    xmlDocumentExtender.Save(this.configPath);
                    this.CreateJsFile();
                    base.Response.Redirect("forum_mymenumanage.aspx");
                }
                else
                {
                    base.RegisterStartupScript("", "<script>alert('您未选中任何选项');window.location.href='forum_mymenumanage.aspx';</script>");
                }
                this.CreateJsFile();
            }
        }

        private void CreateJsFile()
        {
            string path = base.Server.MapPath(BaseConfigs.GetForumPath + "javascript/mymenu.js");
            XmlDocumentExtender xmlDocumentExtender = new XmlDocumentExtender();
            xmlDocumentExtender.Load(this.configPath);
            XmlNodeList childNodes = xmlDocumentExtender.SelectSingleNode("/menuset").ChildNodes;
            if (childNodes.Count == 0)
            {
                return;
            }
            StringBuilder stringBuilder = new StringBuilder();
            foreach (XmlNode xmlNode in childNodes)
            {
                stringBuilder.Append(string.Format("document.write('<li><a href=\"{0}\" {1} {2}>{3}</a></li>');\r\n", new object[]
				{
					xmlNode["href"].InnerText,
					(String.IsNullOrEmpty(xmlNode["onclick"].InnerText)) ? "" : ("onclick=\"" + xmlNode["onclick"].InnerText.Replace("'", "\\'") + "\""),
					(String.IsNullOrEmpty(xmlNode["target"].InnerText)) ? "" : ("target=\"" + xmlNode["target"].InnerText + "\""),
					xmlNode["text"].InnerText
				}));
            }
            stringBuilder = stringBuilder.Replace(" onclick=\"\"", "").Replace(" target=\"\"", "");
            using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(stringBuilder.ToString());
                fileStream.Write(bytes, 0, bytes.Length);
                fileStream.Close();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.DelRec.Click += new EventHandler(this.DelRec_Click);
            this.addmenu.Click += new EventHandler(this.addmenu_Click);
            this.SaveMyMenu.Click += new EventHandler(this.SaveMyMenu_Click);
            this.DataGrid1.ItemDataBound += new DataGridItemEventHandler(this.DataGrid_ItemDataBound);
            this.configPath = base.Server.MapPath(BaseConfigs.GetForumPath + "config/mymenu.config");
        }
    }
}