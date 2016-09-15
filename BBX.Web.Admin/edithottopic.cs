using System;
using System.Data;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using BBX.Cache;
using BBX.Common;
using BBX.Common.Xml;
using BBX.Config;
using BBX.Control;

namespace BBX.Web.Admin
{
    public class edithottopic : AdminPage
    {
        protected HtmlForm Form1;
        protected BBX.Control.DataGrid websiteconfig;
        protected Panel panel1;
        protected HtmlInputHidden topicid;
        protected BBX.Control.TextBox title;
        protected BBX.Control.TextBox poster;
        protected BBX.Control.TextBox postdatetime;
        protected TextareaResize shortdescription;
        protected HtmlInputHidden fulldescription;
        protected BBX.Control.Button savetopic;
        protected Hint hint1;
        private string configPath;
        protected string fidstr = "";
        protected string returnlink = "aggregation_edithottopic.aspx";

        protected void Page_Load(object sender, EventArgs e)
        {
            this.configPath = base.Server.MapPath(BaseConfigs.GetForumPath + "config/agg_hottopics.config");
            if (!base.IsPostBack)
            {
                DataTable websiteConfig = this.GetWebsiteConfig();
                this.websiteconfig.TableHeaderName = "热帖列表";
                this.websiteconfig.DataKeyField = "tid";
                this.websiteconfig.DataSource = websiteConfig;
                this.websiteconfig.DataBind();
                string @string = Request["tid"];
                if (@string != "")
                {
                    this.BindEditData(@string);
                }
            }
        }

        private void BindEditData(string tid)
        {
            this.panel1.Visible = true;
            XmlDocumentExtender xmlDocumentExtender = new XmlDocumentExtender();
            xmlDocumentExtender.Load(this.configPath);
            XmlNodeList xmlNodeList = xmlDocumentExtender.SelectNodes("/Aggregationinfo/Forum/Hottopiclist/Topic");
            XmlNodeInnerTextVisitor xmlNodeInnerTextVisitor = new XmlNodeInnerTextVisitor();
            foreach (XmlNode node in xmlNodeList)
            {
                xmlNodeInnerTextVisitor.SetNode(node);
                if (xmlNodeInnerTextVisitor["tid"] == tid)
                {
                    this.topicid.Value = xmlNodeInnerTextVisitor["tid"];
                    this.title.Text = xmlNodeInnerTextVisitor["title"];
                    this.poster.Text = xmlNodeInnerTextVisitor["poster"];
                    this.postdatetime.Text = xmlNodeInnerTextVisitor["postdatetime"];
                }
            }
        }

        private DataTable GetWebsiteConfig()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("tid");
            dataTable.Columns.Add("title");
            dataTable.Columns.Add("poster");
            dataTable.Columns.Add("postdatetime");
            dataTable.Columns.Add("showtype");
            XmlDocumentExtender xmlDocumentExtender = new XmlDocumentExtender();
            if (!File.Exists(this.configPath))
            {
                return new DataTable();
            }
            xmlDocumentExtender.Load(this.configPath);
            XmlNodeList xmlNodeList = xmlDocumentExtender.SelectNodes("/Aggregationinfo/Forum/Hottopiclist/Topic");
            XmlNodeInnerTextVisitor xmlNodeInnerTextVisitor = new XmlNodeInnerTextVisitor();
            foreach (XmlNode node in xmlNodeList)
            {
                xmlNodeInnerTextVisitor.SetNode(node);
                DataRow dataRow = dataTable.NewRow();
                dataRow["tid"] = xmlNodeInnerTextVisitor["tid"];
                dataRow["title"] = xmlNodeInnerTextVisitor["title"];
                dataRow["poster"] = xmlNodeInnerTextVisitor["poster"];
                dataRow["postdatetime"] = xmlNodeInnerTextVisitor["postdatetime"];
                dataTable.Rows.Add(dataRow);
            }
            return dataTable;
        }

        protected void savetopic_Click(object sender, EventArgs e)
        {
            XmlDocumentExtender xmlDocumentExtender = new XmlDocumentExtender();
            xmlDocumentExtender.Load(this.configPath);
            XmlNodeList xmlNodeList = xmlDocumentExtender.SelectNodes("/Aggregationinfo/Forum/Hottopiclist/Topic");
            XmlNodeInnerTextVisitor xmlNodeInnerTextVisitor = new XmlNodeInnerTextVisitor();
            foreach (XmlNode node in xmlNodeList)
            {
                xmlNodeInnerTextVisitor.SetNode(node);
                if (xmlNodeInnerTextVisitor["tid"] == this.topicid.Value)
                {
                    xmlNodeInnerTextVisitor["tid"] = this.topicid.Value;
                    xmlNodeInnerTextVisitor["title"] = this.title.Text;
                    break;
                }
            }
            xmlNodeList = xmlDocumentExtender.SelectNodes("/Aggregationinfo/Data/Hottopiclist/Topic");
            foreach (XmlNode node2 in xmlNodeList)
            {
                xmlNodeInnerTextVisitor.SetNode(node2);
                if (xmlNodeInnerTextVisitor["tid"] == this.topicid.Value)
                {
                    xmlNodeInnerTextVisitor["tid"] = this.topicid.Value;
                    xmlNodeInnerTextVisitor["title"] = this.title.Text;
                    break;
                }
            }
            xmlDocumentExtender.Save(this.configPath);
            XCache.Remove("/Aggregation/Hottopiclist");
            base.Response.Redirect("aggregation_edithottopic.aspx");
        }
    }
}