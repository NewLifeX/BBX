using System;
using System.Web.UI.HtmlControls;
using BBX.Common;
using BBX.Control;
using BBX.Entity;
using BBX.Forum;
using NewLife;

namespace BBX.Web.Admin
{
    public class auditingtopic : AdminPage
    {
        protected HtmlForm Form1;
        protected DropDownTreeList forumid;
        protected TextBox title;
        protected Calendar postdatetimeStart;
        protected Calendar postdatetimeEnd;
        protected TextBox poster;
        protected TextBox moderatorname;
        protected Calendar deldatetimeStart;
        protected Calendar deldatetimeEnd;
        protected Button SearchTopicAudit;
        protected TextBox RecycleDay;
        protected Button DeleteRecycle;
        protected ajaxpostinfo AjaxPostInfo1;
        protected Hint Hint1;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.forumid.BuildTree(XForum.Root, "name", "fid");
        }

        public void SearchTopicAudit_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                string topicAuditCondition = Topic.SearchTopicAudit(this.forumid.SelectedValue.ToInt(0), this.poster.Text, this.title.Text, this.moderatorname.Text, this.postdatetimeStart.SelectedDate, this.postdatetimeEnd.SelectedDate, this.deldatetimeStart.SelectedDate, this.deldatetimeEnd.SelectedDate);
                this.Session["audittopicswhere"] = topicAuditCondition;
                base.Response.Redirect("forum_audittopicgrid.aspx");
            }
        }

        public void DeleteRecycle_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                //Topics.DeleteRecycleTopic(this.RecycleDay.Text.ToInt());
                var list = Topic.Search(this.RecycleDay.Text.ToInt());
                list.Delete();
                base.RegisterStartupScript("PAGE", "window.location.href='forum_auditingtopic.aspx';");
            }
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.SearchTopicAudit.Click += new EventHandler(this.SearchTopicAudit_Click);
            this.DeleteRecycle.Click += new EventHandler(this.DeleteRecycle_Click);
        }
    }
}