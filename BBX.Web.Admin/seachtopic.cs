using System;
using NewLife;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BBX.Common;
using BBX.Control;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web.Admin
{
    public class seachtopic : AdminPage
    {
        protected HtmlForm Form1;
        protected DropDownTreeList forumid;
        protected BBX.Control.TextBox viewsmin;
        protected BBX.Control.TextBox repliesmin;
        protected BBX.Control.TextBox rate;
        protected BBX.Control.TextBox lastpost;
        protected BBX.Control.TextBox keyword;
        protected BBX.Control.RadioButtonList digest;
        protected BBX.Control.TextBox viewsmax;
        protected BBX.Control.TextBox repliesmax;
        protected BBX.Control.Calendar postdatetimeStart;
        protected BBX.Control.Calendar postdatetimeEnd;
        protected BBX.Control.TextBox poster;
        protected HtmlInputCheckBox lowerupper;
        protected BBX.Control.RadioButtonList displayorder;
        protected BBX.Control.RadioButtonList attachment;
        protected Hint Hint1;
        protected BBX.Control.Button SaveSearchCondition;
        protected BBX.Control.DropDownList typeid;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.postdatetimeStart.SelectedDate = DateTime.Now.AddDays(-30.0);
                this.postdatetimeEnd.SelectedDate = DateTime.Now;
            }
            this.forumid.BuildTree(XForum.Root, "Name", "ID");
            this.forumid.TypeID.Items.RemoveAt(0);
            this.forumid.TypeID.Items.Insert(0, new ListItem("全部", "0"));
        }

        private void SaveSearchCondition_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                //string searchTopicsCondition = Topics.GetSearchTopicsCondition(Utils.StrToInt(this.forumid.SelectedValue, 0), this.keyword.Text, this.displayorder.SelectedValue, this.digest.SelectedValue, this.attachment.SelectedValue, this.poster.Text, this.lowerupper.Checked, this.viewsmin.Text, this.viewsmax.Text, this.repliesmax.Text, this.repliesmin.Text, this.rate.Text, this.lastpost.Text, this.postdatetimeStart.SelectedDate, this.postdatetimeEnd.SelectedDate);
                //this.Session["topicswhere"] = searchTopicsCondition;
                this.Session["topicswhere"] = Topic.SearchWhere(forumid.SelectedValue.ToInt(), this.keyword.Text, this.displayorder.SelectedValue, this.digest.SelectedValue, this.attachment.SelectedValue, this.poster.Text, this.viewsmin.Text.ToInt(), this.viewsmax.Text.ToInt(), this.repliesmax.Text.ToInt(), this.repliesmin.Text.ToInt(), this.rate.Text.ToInt(), this.lastpost.Text.ToInt(), this.postdatetimeStart.SelectedDate, this.postdatetimeEnd.SelectedDate);
                base.Response.Redirect("forum_topicsgrid.aspx");
            }
        }

        //protected override void SavePageStateToPersistenceMedium(object viewState)
        //{
        //    base.MySavePageState(viewState);
        //}

        //protected override object LoadPageStateFromPersistenceMedium()
        //{
        //    return base.MyLoadPageState();
        //}

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.SaveSearchCondition.Click += new EventHandler(this.SaveSearchCondition_Click);
        }
    }
}