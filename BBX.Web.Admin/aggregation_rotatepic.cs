using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Xml;
using BBX.Aggregation;
using BBX.Cache;
using BBX.Common;
using BBX.Common.Xml;
using BBX.Config;
using BBX.Control;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web.Admin
{
    public class aggregation_rotatepic : AdminPage
    {
        private string configPath;
        private string nodeName;
        private string targetNode;
        public DataSet dsSrc = new DataSet();
        protected BBX.Control.Button DelRec;
        protected BBX.Control.Button addrota;
        protected BBX.Control.DataGrid DataGrid1;
        protected BBX.Control.TextBox rotaimg;
        protected BBX.Control.TextBox url;
        protected BBX.Control.TextBox titlecontent;
        protected BBX.Control.Button SaveRotatepic;
        protected Hint Hint1;

        private void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.BindData();
            }
        }

        public void BindData()
        {
            this.DataGrid1.AllowCustomPaging = false;
            this.DataGrid1.DataKeyField = "rotatepicid";
            this.DataGrid1.TableHeaderName = "聚合轮换图片列表";
            var doc = new XmlDocumentExtender();
            doc.Load(this.configPath);
            var xmlNode = doc.SelectSingleNode(this.targetNode);
            if (xmlNode == null || xmlNode.ChildNodes.Count == 0)
            {
                this.DataGrid1.Visible = (this.SaveRotatepic.Visible = false);
                return;
            }
            var reader = new XmlNodeReader(xmlNode);
            this.dsSrc.ReadXml(reader);
            this.dsSrc.Tables[0].Columns.Add("rowid");
            int num = 0;
            foreach (DataRow dataRow in this.dsSrc.Tables[0].Rows)
            {
                dataRow["rowid"] = num.ToString();
                num++;
            }
            this.DataGrid1.DataSource = this.dsSrc.Tables[0];
            this.DataGrid1.DataBind();
        }

        private void addrota_Click(object sender, EventArgs e)
        {
            if (this.rotaimg.Text.Trim() != "" && this.url.Text.Trim() != "" && this.titlecontent.Text.Trim() != "")
            {
                if (!Utils.IsURL(this.rotaimg.Text.Trim()) || !Utils.IsURL(this.url.Text.Trim()))
                {
                    base.RegisterStartupScript("", "<script>alert('图片路径或点击链接可能是非法URL');</script>");
                    this.BindData();
                    this.ResetForm();
                    return;
                }
                XmlDocumentExtender xmlDocumentExtender = new XmlDocumentExtender();
                xmlDocumentExtender.Load(this.configPath);
                int num = 0;
                if (xmlDocumentExtender.SelectSingleNode(this.targetNode) != null && xmlDocumentExtender.SelectSingleNode(this.targetNode).InnerText != "")
                {
                    num = int.Parse(xmlDocumentExtender.SelectSingleNode(this.targetNode).LastChild["rotatepicid"].InnerText);
                }
                num++;
                XmlElement newChild = xmlDocumentExtender.CreateElement(this.nodeName);
                xmlDocumentExtender.AppendChildElementByNameValue(ref newChild, "rotatepicid", num.ToString());
                xmlDocumentExtender.AppendChildElementByNameValue(ref newChild, "pagetype", "1");
                xmlDocumentExtender.AppendChildElementByNameValue(ref newChild, "img", this.rotaimg.Text.Trim());
                xmlDocumentExtender.AppendChildElementByNameValue(ref newChild, "url", this.url.Text.Trim());
                xmlDocumentExtender.AppendChildElementByNameValue(ref newChild, "titlecontent", this.titlecontent.Text.Trim());
                xmlDocumentExtender.CreateNode(this.targetNode).AppendChild(newChild);
                xmlDocumentExtender.Save(this.configPath);
                AggregationFacade.BaseAggregation.ClearAllDataBind();
                AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "添加聚合页图版轮换广告", "添加聚合页图版轮换广告,名称为: " + this.titlecontent.Text.Trim());
                try
                {
                    this.BindData();
                    XCache.Remove(CacheKeys.FORUM_FORUM_LINK_LIST);
                    this.ResetForm();
                    base.RegisterStartupScript("PAGE", "window.location.href='aggregation_rotatepic.aspx?pagename=" + Request["pagename"] + "';");
                    return;
                }
                catch
                {
                    base.RegisterStartupScript("", "<script>alert('无法更新XML文件');window.location.href='aggregation_rotatepic.aspx?pagename=" + Request["pagename"] + "';</script>");
                    return;
                }
            }
            base.RegisterStartupScript("", "<script>alert('图片或链接地址以及标题不能为空.');window.location.href='aggregation_rotatepic.aspx?pagename=" + Request["pagename"] + "';</script>");
        }

        private void ResetForm()
        {
            this.rotaimg.Text = "";
            this.url.Text = "";
            this.titlecontent.Text = "";
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

        private void SaveRotatepic_Click(object sender, EventArgs e)
        {
            int num = 0;
            bool flag = false;
            var doc = new XmlDocumentExtender();
            doc.Load(this.configPath);
            var childNodes = doc.SelectSingleNode(this.targetNode).ChildNodes;
            if (childNodes != null && childNodes.Count > 0)
            {
                doc.InitializeNode(this.targetNode);
            }
            foreach (object arg_5F_0 in this.DataGrid1.GetKeyIDArray())
            {
                string controlValue = this.DataGrid1.GetControlValue(num, "rotatepicid");
                string controlValue2 = this.DataGrid1.GetControlValue(num, "img");
                string controlValue3 = this.DataGrid1.GetControlValue(num, "url");
                string text = this.DataGrid1.GetControlValue(num, "titlecontent").Trim();
                if (!Utils.IsNumeric(controlValue) || !Utils.IsURL(controlValue2) || !Utils.IsURL(controlValue3) || String.IsNullOrEmpty(text))
                {
                    flag = true;
                    break;
                }
                bool flag2 = false;
                XmlElement newChild = doc.CreateElement(this.nodeName);
                doc.AppendChildElementByNameValue(ref newChild, "rotatepicid", controlValue);
                doc.AppendChildElementByNameValue(ref newChild, "pagetype", "1");
                doc.AppendChildElementByNameValue(ref newChild, "img", controlValue2);
                doc.AppendChildElementByNameValue(ref newChild, "url", controlValue3);
                doc.AppendChildElementByNameValue(ref newChild, "titlecontent", text);
                foreach (XmlNode xmlNode in childNodes)
                {
                    if (int.Parse(xmlNode["rotatepicid"].InnerText) > int.Parse(controlValue))
                    {
                        doc.SelectSingleNode(this.targetNode).InsertBefore(newChild, xmlNode);
                        flag2 = true;
                        break;
                    }
                }
                if (!flag2)
                {
                    doc.SelectSingleNode(this.targetNode).AppendChild(newChild);
                }
                num++;
            }
            AggregationFacade.BaseAggregation.ClearAllDataBind();
            if (!flag)
            {
                SiteUrls.Current = null;
                AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "聚合页面论坛广告编辑", "");
                doc.Save(this.configPath);
                base.RegisterStartupScript("PAGE", "window.location.href='aggregation_rotatepic.aspx?pagename=" + Request["pagename"] + "';");
                return;
            }
            base.RegisterStartupScript("", "<script>alert('某行序号、图片路径或点击链接可能是非法URL或说明文字为空，不能进行更新.');window.location.href='aggregation_rotatepic.aspx?pagename=" + Request["pagename"] + "';</script>");
        }

        private void DataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                System.Web.UI.WebControls.TextBox textBox = (System.Web.UI.WebControls.TextBox)e.Item.Cells[1].Controls[0];
                textBox.Attributes.Add("size", "5");
                textBox.Width = 30;
                textBox = (System.Web.UI.WebControls.TextBox)e.Item.Cells[2].Controls[0];
                textBox.Width = 200;
                textBox = (System.Web.UI.WebControls.TextBox)e.Item.Cells[3].Controls[0];
                textBox.Width = 200;
                textBox = (System.Web.UI.WebControls.TextBox)e.Item.Cells[4].Controls[0];
                textBox.Width = 200;
            }
        }

        private void DelRec_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                string rowid = Request["rowid"];
                if (rowid != "")
                {
                    int num = 0;
                    XmlDocumentExtender doc = new XmlDocumentExtender();
                    doc.Load(this.configPath);
                    XmlNodeList childNodes = doc.SelectSingleNode(this.targetNode).ChildNodes;
                    string[] array = rowid.Split(',');
                    for (int i = 0; i < array.Length; i++)
                    {
                        string s = array[i];
                        doc.SelectSingleNode(this.targetNode).RemoveChild(childNodes.Item(int.Parse(s) - num));
                        num++;
                    }
                    doc.Save(this.configPath);
                    AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "删除选定的图版轮换页", "删除选定的图版轮换页,ID为: " + Request["id"].Replace("0 ", ""));
                    base.Response.Redirect("aggregation_rotatepic.aspx?pagename=" + Request["pagename"]);
                    return;
                }
                base.RegisterStartupScript("", "<script>alert('您未选中任何选项');window.location.href='aggregation_rotatepic.aspx?pagename=" + Request["pagename"] + "';</script>");
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
            this.addrota.Click += new EventHandler(this.addrota_Click);
            this.SaveRotatepic.Click += new EventHandler(this.SaveRotatepic_Click);
            this.DataGrid1.ItemDataBound += new DataGridItemEventHandler(this.DataGrid_ItemDataBound);
            this.configPath = base.Server.MapPath(BaseConfigs.GetForumPath + "config/aggregation.config");
            string a;
            if ((a = Request["pagename"].ToLower()) != null)
            {
                if (a == "website")
                {
                    this.nodeName = "Website_rotatepic";
                    this.targetNode = "/Aggregationinfo/Aggregationpage/Website/Website_rotatepiclist";
                    return;
                }
                if (a == "spaceindex")
                {
                    this.nodeName = "Spaceindex_rotatepic";
                    this.targetNode = "/Aggregationinfo/Aggregationpage/Spaceindex/Spaceindex_rotatepiclist";
                    return;
                }
                if (a == "albumindex")
                {
                    this.nodeName = "Albumindex_rotatepic";
                    this.targetNode = "/Aggregationinfo/Aggregationpage/Albumindex/Albumindex_rotatepiclist";
                    return;
                }
            }
            this.nodeName = "Website_rotatepic";
            this.targetNode = "/Aggregationinfo/Aggregationpage/Website/Website_rotatepiclist";
        }
    }
}