using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BBX.Common;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web.Admin
{
    public class auditnewtopic : AdminPage
    {
        public int pageid = DNTRequest.GetInt("pageid", 1);
        public List<Topic> auditTopicList = new List<Topic>();
        public int pageCount;
        public int topicCount;
        protected HtmlForm Form1;
        protected BBX.Control.Button SelectPass;
        protected BBX.Control.Button SelectDelete;
        protected ajaxpostinfo AjaxPostInfo1;
        protected Literal msg;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.BindData();
            }
        }

        public void BindData()
        {
            this.topicCount = Topic.GetUnauditNewTopicCount(null, -2);
            this.pageCount = (this.topicCount - 1) / 20 + 1;
            this.pageid = ((this.pageid > this.pageCount) ? this.pageCount : this.pageid);
            this.pageid = ((this.pageid < 1) ? 1 : this.pageid);
            this.auditTopicList = Topic.GetUnauditNewTopic(null, -2, this.pageid, 20);
        }

        private void SelectPass_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                string tid = Request["tid"];
                if (tid != "")
                {
                    //Topics.PassAuditNewTopic(tid);
                    var list = Topic.FindAllByIDs(tid);
                    foreach (var item in list)
                    {
                        item.Post.Invisible = 0;
                        item.Post.Save();

                        CreditsFacade.PostTopic(item.PosterID, item.Forum);
                    }
                    base.RegisterStartupScript("", "<script>window.location='forum_auditnewtopic.aspx';</script>");
                    return;
                }
                base.RegisterStartupScript("", "<script>alert('您未选中任何选项');window.location='forum_auditnewtopic.aspx';</script>");
            }
        }

        private void SelectDelete_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                if (Request["tid"] != "")
                {
                    TopicAdmins.DeleteTopicsWithoutChangingCredits(Request["tid"], false);
                    base.RegisterStartupScript("", "<script>window.location='forum_auditnewtopic.aspx';</script>");
                    return;
                }
                base.RegisterStartupScript("", "<script>alert('您未选中任何选项');window.location='forum_auditnewtopic.aspx';</script>");
            }
        }

        public string GetTopicType(string topicType)
        {
            if (topicType != null)
            {
                if (topicType == "0")
                {
                    return "普通主题";
                }
                if (topicType == "1")
                {
                    return "投票帖";
                }
                if (topicType == "2" || topicType == "3")
                {
                    return "悬赏帖";
                }
                if (topicType == "4")
                {
                    return "辩论帖";
                }
            }
            return topicType;
        }

        protected string GetTopicStatus(string displayOrder)
        {
            int num = int.Parse(displayOrder);
            if (num > 0)
            {
                return "置顶";
            }
            if (num == 0)
            {
                return "正常";
            }
            if (num == -1)
            {
                return "回收站";
            }
            if (num == -2)
            {
                return "待审核";
            }
            if (num == -3)
            {
                return "被忽略";
            }
            return displayOrder;
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.SelectPass.Click += new EventHandler(this.SelectPass_Click);
            this.SelectDelete.Click += new EventHandler(this.SelectDelete_Click);
        }

        public string ShowPageIndex()
        {
            string text = "";
            int num = (this.pageid - 5 > 0) ? (this.pageid - 5 - ((this.pageid + 5 < this.pageCount) ? 0 : (this.pageid + 5 - this.pageCount))) : 1;
            int num2 = (this.pageid + 5 < this.pageCount) ? (this.pageid + 5 + ((this.pageid - 5 > 0) ? 0 : ((this.pageid - 5) * -1 + 1))) : this.pageCount;
            for (int i = num; i <= num2; i++)
            {
                if (i != this.pageid)
                {
                    text += string.Format("<a style=\"border:1px solid #E8E8E8;padding:2px 4px;margin-right:2px;\" href=\"forum_auditnewtopic.aspx?pageid={0}\">{0}</a>", i);
                }
                else
                {
                    text += string.Format("<span style=\"border:1px solid #E8E8E8;padding:2px 4px;margin-right:2px;background:#09C;color:#FFF\" >{0}</span> ", i);
                }
            }
            return text;
        }
    }
}