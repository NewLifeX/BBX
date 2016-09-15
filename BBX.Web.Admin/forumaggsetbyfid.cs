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
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web.Admin
{
    public class forumaggsetbyfid : AdminPage
    {
        protected HtmlForm Form1;
        protected BBX.Control.DropDownList tablelist;
        protected DropDownTreeList forumid;
        protected BBX.Control.TextBox title;
        protected BBX.Control.TextBox poster;
        protected BBX.Control.Calendar postdatetimeStart;
        protected BBX.Control.Calendar postdatetimeEnd;
        protected BBX.Control.Button SearchTopicAudit;
        protected ajaxtopicinfo AjaxTopicInfo1;
        protected Literal forumlist;
        protected BBX.Control.Button SaveTopic;
        protected Hint Hint1;
        protected HtmlInputHidden forumtopicstatus;
        protected pageinfo info1;
        private string configPath;
        protected string fid = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            this.fid = Request["fid"];
            if (!base.IsPostBack)
            {
                if (File.Exists(this.configPath))
                {
                    this.LoadWebSiteConfig();
                }
                this.LoadPostTableList();
            }
        }

        private void LoadPostTableList()
        {
            //DataTable allPostTable = Posts.GetAllPostTable();
            //foreach (DataRow dataRow in allPostTable.Rows)
            foreach (var item in TableList.GetAllPostTable())
            {
                this.tablelist.Items.Add(new ListItem(BaseConfigs.GetTablePrefix + "posts" + item.ID, item.ID.ToString()));
            }
        }

        private void LoadWebSiteConfig()
        {
            XmlDocumentExtender xmlDocumentExtender = new XmlDocumentExtender();
            xmlDocumentExtender.Load(this.configPath);
            XmlNodeList xmlNodeList = xmlDocumentExtender.SelectNodes("/Aggregationinfo/Data/Topiclist/Topic");
            XmlNodeList xmlNodeList2 = xmlDocumentExtender.SelectNodes("/Aggregationinfo/Forum/Topiclist/Topic");
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
                    if (xmlNodeInnerTextVisitor["topicid"].ToString() == xmlNodeInnerTextVisitor2["topicid"].ToString())
                    {
                        flag = true;
                        break;
                    }
                }
                Literal expr_E4 = this.forumlist;
                object text = expr_E4.Text;
                expr_E4.Text = text + "<div class='mo' id='m" + num + "' flag='f" + num + "'><h1><input type='checkbox' name='tid' " + (flag ? "checked" : "") + " value='" + xmlNodeInnerTextVisitor["topicid"] + "'>" + xmlNodeInnerTextVisitor["title"] + "</h1></div>\n";
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
                    xmlDocumentExtender.RemoveNodeAndChildNode("/Aggregationinfo/Data/Topiclist");
                    xmlDocumentExtender.RemoveNodeAndChildNode("/Aggregationinfo/Forum/Topiclist");
                    xmlDocumentExtender.Save(this.configPath);
                    XCache.Remove("/Aggregation/TopicByForumId_" + this.fid);
                }
                base.Response.Redirect("aggregation_editforumaggset.aspx?fid=" + this.fid);
                return;
            }
            var postListFromFile = new ForumAggregationData().GetPostListFromFile("Website");
            Posts.WriteAggregationPostData(postListFromFile, this.tablelist.SelectedValue, @string, this.configPath, "/Aggregationinfo/Data/Topiclist", "/Aggregationinfo/Forum/Topiclist");
            XCache.Remove("/Aggregation/TopicByForumId_" + this.fid);
            base.Response.Redirect("aggregation_editforumaggset.aspx?fid=" + this.fid);
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
            this.configPath = base.Server.MapPath(BaseConfigs.GetForumPath + "config/agg_" + Request["fid"] + ".config");
        }
    }
}