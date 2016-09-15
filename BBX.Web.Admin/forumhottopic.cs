using System;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using BBX.Cache;
using BBX.Common;
using BBX.Common.Xml;
using BBX.Config;
using BBX.Control;
using BBX.Forum;

namespace BBX.Web.Admin
{
    public class forumhottopic : AdminPage
    {
        private string configPath;
        protected string fid = "";
        public int forumid = DNTRequest.GetInt("forumid", 0);
        public string showtype;
        public int timebetween = DNTRequest.GetInt("timebetween", 7);
        protected HtmlForm Form1;
        protected ajaxtopicinfo AjaxTopicInfo1;
        protected Literal forumlist;
        protected BBX.Control.Button SaveTopic;
        protected Hint Hint1;
        protected HtmlInputHidden forumtopicstatus;
        protected pageinfo info1;

        public forumhottopic()
        {
            showtype = (String.IsNullOrEmpty(Request["showtype"])) ? "replies" : Request["showtype"];
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.fid = Request["fid"];
            if (!base.IsPostBack && File.Exists(this.configPath))
            {
                this.LoadWebSiteConfig();
            }
        }

        private void LoadWebSiteConfig()
        {
            XmlDocumentExtender xmlDocumentExtender = new XmlDocumentExtender();
            xmlDocumentExtender.Load(this.configPath);
            XmlNodeList xmlNodeList = xmlDocumentExtender.SelectNodes("/Aggregationinfo/Data/Hottopiclist/Topic");
            XmlNodeList xmlNodeList2 = xmlDocumentExtender.SelectNodes("/Aggregationinfo/Forum/Hottopiclist/Topic");
            XmlNodeInnerTextVisitor xmlNodeInnerTextVisitor = new XmlNodeInnerTextVisitor();
            XmlNodeInnerTextVisitor xmlNodeInnerTextVisitor2 = new XmlNodeInnerTextVisitor();
            this.forumlist.Text = "";
            int num = 0;
            foreach (XmlNode node in xmlNodeList)
            {
                xmlNodeInnerTextVisitor.SetNode(node);
                bool flag = false;
                foreach (XmlNode node2 in xmlNodeList2)
                {
                    xmlNodeInnerTextVisitor2.SetNode(node2);
                    if (xmlNodeInnerTextVisitor["tid"].ToString() == xmlNodeInnerTextVisitor2["tid"].ToString())
                    {
                        flag = true;
                        break;
                    }
                }
                Literal expr_E4 = this.forumlist;
                object text = expr_E4.Text;
                expr_E4.Text = text + "<div class='mo' id='m" + num + "' flag='f" + num + "'><h1><input type='checkbox' name='tid' " + (flag ? "checked" : "") + " value='" + xmlNodeInnerTextVisitor["tid"] + "'>" + xmlNodeInnerTextVisitor["title"] + "</h1></div>\n";
                num++;
            }
        }

        private void SaveTopic_Click(object sender, EventArgs e)
        {
            string @string = Request["forumtopicstatus"];
            if (String.IsNullOrEmpty(@string))
            {
                if (File.Exists(this.configPath))
                {
                    XmlDocumentExtender xmlDocumentExtender = new XmlDocumentExtender();
                    xmlDocumentExtender.Load(this.configPath);
                    xmlDocumentExtender.RemoveNodeAndChildNode("/Aggregationinfo/Data/Hottopiclist");
                    xmlDocumentExtender.RemoveNodeAndChildNode("/Aggregationinfo/Forum/Hottopiclist");
                    xmlDocumentExtender.Save(this.configPath);
                    XCache.Remove("/Aggregation/Hottopiclist");
                }
                base.Response.Redirect("aggregation_forumhottopic.aspx");
                return;
            }
            Posts.WriteAggregationHotTopicsData(@string, this.configPath, "/Aggregationinfo/Data/Hottopiclist", "/Aggregationinfo/Forum/Hottopiclist");
            XCache.Remove("/Aggregation/Hottopiclist");
            base.Response.Redirect("aggregation_edithottopic.aspx");
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.SaveTopic.Click += new EventHandler(this.SaveTopic_Click);
            this.SaveTopic.ValidateForm = true;
            this.configPath = base.Server.MapPath(BaseConfigs.GetForumPath + "config/agg_hottopics.config");
        }
    }
}