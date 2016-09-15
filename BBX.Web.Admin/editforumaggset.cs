using System;
using System.Data;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using BBX.Aggregation;
using BBX.Cache;
using BBX.Common;
using BBX.Common.Xml;
using BBX.Config;
using BBX.Control;

namespace BBX.Web.Admin
{
    public class editforumaggset : AdminPage
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
        private string fid;
        protected string fidstr = "";
        protected string returnlink = "aggregation_forumaggset.aspx";

        protected void Page_Load(object sender, EventArgs e)
        {
            this.fid = Request["fid"];
            if (String.IsNullOrEmpty(this.fid))
            {
                this.configPath = base.Server.MapPath(BaseConfigs.GetForumPath + "config/aggregation.config");
            }
            else
            {
                this.configPath = base.Server.MapPath(BaseConfigs.GetForumPath + "config/agg_" + this.fid + ".config");
                this.fidstr = "&fid=" + this.fid;
                this.returnlink = "aggregation_forumaggsetbyfid.aspx?fid=" + this.fid;
            }
            if (!base.IsPostBack)
            {
                DataTable websiteConfig = this.GetWebsiteConfig();
                this.websiteconfig.TableHeaderName = "帖子列表";
				this.websiteconfig.DataKeyField = "ID";
                this.websiteconfig.DataSource = websiteConfig;
                this.websiteconfig.DataBind();
                string tid = Request["tid"];
                if (tid != "")
                {
                    this.BindEditData(tid);
                }
            }
        }

        private void BindEditData(string tid)
        {
            this.panel1.Visible = true;
            XmlDocumentExtender xmlDocumentExtender = new XmlDocumentExtender();
            if (!File.Exists(this.configPath))
            {
                return;
            }
            xmlDocumentExtender.Load(this.configPath);
            string xpath = "/Aggregationinfo/Aggregationpage/Website/Forum/Topiclist/Topic";
            if (this.fid != "")
            {
                xpath = "/Aggregationinfo/Forum/Topiclist/Topic";
            }
            XmlNodeList xmlNodeList = xmlDocumentExtender.SelectNodes(xpath);
            XmlNodeInnerTextVisitor xmlNodeInnerTextVisitor = new XmlNodeInnerTextVisitor();
            foreach (XmlNode node in xmlNodeList)
            {
                xmlNodeInnerTextVisitor.SetNode(node);
                if (xmlNodeInnerTextVisitor["topicid"] == tid)
                {
                    this.topicid.Value = xmlNodeInnerTextVisitor["topicid"];
                    this.title.Text = xmlNodeInnerTextVisitor["title"];
                    this.poster.Text = xmlNodeInnerTextVisitor["poster"];
                    this.postdatetime.Text = xmlNodeInnerTextVisitor["postdatetime"];
                    this.shortdescription.Text = xmlNodeInnerTextVisitor["shortdescription"];
                    this.fulldescription.Value = xmlNodeInnerTextVisitor["fulldescription"];
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
            string xpath = "/Aggregationinfo/Aggregationpage/Website/Forum/Topiclist/Topic";
            if (this.fid != "")
            {
                xpath = "/Aggregationinfo/Forum/Topiclist/Topic";
            }
            XmlNodeList xmlNodeList = xmlDocumentExtender.SelectNodes(xpath);
            XmlNodeInnerTextVisitor xmlNodeInnerTextVisitor = new XmlNodeInnerTextVisitor();
            foreach (XmlNode node in xmlNodeList)
            {
                xmlNodeInnerTextVisitor.SetNode(node);
                DataRow dataRow = dataTable.NewRow();
                dataRow["tid"] = xmlNodeInnerTextVisitor["topicid"];
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
            if (!File.Exists(this.configPath))
            {
                return;
            }
            xmlDocumentExtender.Load(this.configPath);
            string topicPath = "/Aggregationinfo/Aggregationpage/Website/Forum/Topiclist/Topic";
            string topicPath2 = "/Aggregationinfo/Aggregationdata/Websiteaggregationdata/Topiclist/Topic";
            if (this.fid != "")
            {
                topicPath = "/Aggregationinfo/Forum/Topiclist/Topic";
                topicPath2 = "/Aggregationinfo/Data/Topiclist/Topic";
            }
            this.ModifyNode(xmlDocumentExtender, topicPath);
            this.ModifyNode(xmlDocumentExtender, topicPath2);
            xmlDocumentExtender.Save(this.configPath);
            if (String.IsNullOrEmpty(this.fid))
            {
                AggregationFacade.BaseAggregation.ClearAllDataBind();
                base.Response.Redirect("aggregation_editforumaggset.aspx");
                return;
            }
            XCache.Remove("/Aggregation/TopicByForumId_" + this.fid);
            base.Response.Redirect("aggregation_editforumaggset.aspx?fid=" + this.fid);
        }

        private void ModifyNode(XmlDocumentExtender doc, string topicPath)
        {
            XmlNodeList xmlNodeList = doc.SelectNodes(topicPath);
            XmlNodeInnerTextVisitor xmlNodeInnerTextVisitor = new XmlNodeInnerTextVisitor();
            foreach (XmlNode node in xmlNodeList)
            {
                xmlNodeInnerTextVisitor.SetNode(node);
                if (xmlNodeInnerTextVisitor["topicid"] == this.topicid.Value)
                {
                    xmlNodeInnerTextVisitor["topicid"] = this.topicid.Value;
                    xmlNodeInnerTextVisitor["title"] = this.title.Text;
                    xmlNodeInnerTextVisitor["poster"] = this.poster.Text;
                    xmlNodeInnerTextVisitor["postdatetime"] = this.postdatetime.Text;
                    XmlCDataSection xmlCDataSection = doc.CreateCDataSection("shortdescription");
                    xmlCDataSection.InnerText = this.shortdescription.Text;
                    xmlNodeInnerTextVisitor.GetNode("shortdescription").RemoveAll();
                    xmlNodeInnerTextVisitor.GetNode("shortdescription").AppendChild(xmlCDataSection);
                    break;
                }
            }
        }

        protected string GetEditLink(string tid)
        {
            return "?tid=" + tid + this.fidstr;
        }
    }
}