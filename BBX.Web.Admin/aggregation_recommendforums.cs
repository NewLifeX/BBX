using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Xml;
using BBX.Aggregation;
using BBX.Common;

using BBX.Common.Xml;
using BBX.Config;
using BBX.Control;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web.Admin
{
    public class aggregation_recommendforums : AdminPage
    {
        protected HtmlForm Form1;
        protected pageinfo info1;
        protected ListBoxTreeList list1;
        protected Button Btn_SaveInfo;
        private string configPath;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                this.list1.BuildTree(Forums.GetForumListForDataTable(), "name", "fid");
                this.list1.TypeID.Items.RemoveAt(0);
                this.list1.TypeID.Width = 260;
                this.list1.TypeID.Height = 290;
                this.list1.TypeID.SelectedIndex = 0;
                this.LoadInfo();
            }
        }

        private void LoadInfo()
        {
            XmlDocumentExtender xmlDocumentExtender = new XmlDocumentExtender();
            xmlDocumentExtender.Load(this.configPath);
            XmlNode xmlNode = xmlDocumentExtender.SelectSingleNode("/Aggregationinfo/Aggregationpage/Website/Website_forumrecomend/fidlist");
            if (xmlNode == null)
            {
                return;
            }
            string innerText = xmlNode.InnerText;
            string text = "";
            List<IXForum> forumList = Forums.GetForumList();
            string[] array = innerText.Split(',');
            for (int i = 0; i < array.Length; i++)
            {
                string b = array[i];
                foreach (var current in forumList)
                {
                    if (current.Fid.ToString() == b)
                    {
                        object obj = text;
                        text = obj + "{'fid':'" + current.Fid + "','forumtitle':'" + current.Name + "'},";
                        break;
                    }
                }
            }
            if (!String.IsNullOrEmpty(text))
            {
                text = text.TrimEnd(',');
            }
            text = "<script type='text/javascript'>\r\nvar fidlist = [" + text + "];\r\nfor(var i = 0 ; i < fidlist.length ; i++)\r\n{\r\nvar no = new Option();\r\nno.value = fidlist[i]['fid'];\r\nno.text = fidlist[i]['forumtitle'];\r\nForm1.list2.options[Form1.list2.options.length] = no;\r\n}\r\n</script>";
            base.RegisterStartupScript("", text);
        }

        private void Btn_SaveInfo_Click(object sender, EventArgs e)
        {
            XmlDocumentExtender xmlDocumentExtender = new XmlDocumentExtender();
            xmlDocumentExtender.Load(this.configPath);
            XmlNode xmlNode = xmlDocumentExtender.InitializeNode("/Aggregationinfo/Aggregationpage/Website/Website_forumrecomend/fidlist");
            xmlNode.InnerText = Request["rst"];
            xmlDocumentExtender.Save(this.configPath);
            AggregationFacade.BaseAggregation.ClearAllDataBind();
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