using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using BBX.Aggregation;
using BBX.Common.Xml;
using BBX.Config;
using BBX.Control;
using BBX.Entity;
using BBX.Forum;
using XCode;

namespace BBX.Web.Admin
{
	public class forumaggset : AdminPage
	{
		private string configPath;
		protected HtmlForm Form1;
		protected BBX.Control.DropDownList showtype;
		protected BBX.Control.TextBox topnumber;
		protected BBX.Control.Button SaveTopicDisplay;
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

		protected void Page_Load(object sender, EventArgs e)
		{
			//this.forumid.BuildTree(Forums.GetOpenForumList(), "name", "fid");
			var list = new EntityList<XForum>(XForum.GetOpenForumList());
			this.forumid.BuildTree(list.ToDataTable(false), "name", "fid");
			if (!base.IsPostBack)
			{
				this.LoadWebSiteConfig();
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
			XmlNodeList xmlNodeList = xmlDocumentExtender.SelectNodes("/Aggregationinfo/Aggregationdata/Websiteaggregationdata/Topiclist/Topic");
			XmlNodeList xmlNodeList2 = xmlDocumentExtender.SelectNodes("/Aggregationinfo/Aggregationpage/Website/Forum/Topiclist/Topic");
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
			this.topnumber.Text = xmlDocumentExtender.GetSingleNodeValue(xmlDocumentExtender.SelectNodes("/Aggregationinfo/Aggregationpage/Website/Forum")[0], "Bbs/Topnumber");
			this.showtype.SelectedValue = xmlDocumentExtender.GetSingleNodeValue(xmlDocumentExtender.SelectNodes("/Aggregationinfo/Aggregationpage/Website/Forum")[0], "Bbs/Showtype");
		}

		private void SaveTopic_Click(object sender, EventArgs e)
		{
            string forumtopicstatus = Request["forumtopicstatus"];
            if (String.IsNullOrEmpty(forumtopicstatus))
			{
				var doc = new XmlDocumentExtender();
				doc.Load(this.configPath);
				doc.RemoveNodeAndChildNode("/Aggregationinfo/Aggregationdata/Websiteaggregationdata/Topiclist");
				doc.RemoveNodeAndChildNode("/Aggregationinfo/Aggregationpage/Website/Forum/Topiclist");
				doc.Save(this.configPath);
				base.Response.Redirect("aggregation_editforumaggset.aspx");
				return;
			}
			var postListFromFile = new ForumAggregationData().GetPostListFromFile("Website");
            Posts.WriteAggregationPostData(postListFromFile, this.tablelist.SelectedValue, forumtopicstatus, this.configPath, "/Aggregationinfo/Aggregationdata/Websiteaggregationdata/Topiclist", "/Aggregationinfo/Aggregationpage/Website/Forum/Topiclist");
			AggregationFacade.BaseAggregation.ClearAllDataBind();
			base.Response.Redirect("aggregation_editforumaggset.aspx");
		}

		protected void SaveTopicDisplay_Click(object sender, EventArgs e)
		{
			XmlDocumentExtender xmlDocumentExtender = new XmlDocumentExtender();
			xmlDocumentExtender.Load(this.configPath);
			xmlDocumentExtender.RemoveNodeAndChildNode("/Aggregationinfo/Aggregationpage/Website/Forum/Bbs");
			if (xmlDocumentExtender.SelectSingleNode("/Aggregationinfo/Aggregationpage/Website/Forum") == null)
			{
				xmlDocumentExtender.InitializeNode("/Aggregationinfo/Aggregationpage/Website/Forum");
			}
			XmlElement newChild = xmlDocumentExtender.CreateElement("Bbs");
			xmlDocumentExtender.AppendChildElementByNameValue(ref newChild, "Topnumber", this.topnumber.Text, false);
			xmlDocumentExtender.AppendChildElementByNameValue(ref newChild, "Showtype", this.showtype.SelectedValue, false);
			xmlDocumentExtender.SelectSingleNode("/Aggregationinfo/Aggregationpage/Website/Forum").AppendChild(newChild);
			xmlDocumentExtender.Save(this.configPath);

			//AggregationConfig.ResetConfig();
			AggregationConfigInfo.Current = null;
			AggregationFacade.ForumAggregation.ClearAllDataBind();
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
			this.configPath = base.Server.MapPath(BaseConfigs.GetForumPath + "config/aggregation.config");
		}
	}
}