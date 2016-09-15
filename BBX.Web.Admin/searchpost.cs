using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BBX.Common;
using BBX.Control;
using BBX.Entity;

namespace BBX.Web.Admin
{
    public class searchpost : AdminPage
    {
        protected HtmlForm Form1;
        protected DropDownTreeList forumid;
        protected BBX.Control.Calendar postdatetimeStart;
        protected BBX.Control.Calendar postdatetimeEnd;
        protected BBX.Control.TextBox Ip;
        //protected DropDownPost postlist;
        protected BBX.Control.TextBox poster;
        protected HtmlInputCheckBox lowerupper;
        protected BBX.Control.TextBox message;
        protected Hint Hint1;
        protected BBX.Control.Button SaveConditionInf;

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

        private void SaveConditionInf_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                string value = Post.SearchSQL(Utils.StrToInt(this.forumid.SelectedValue, 0), this.postdatetimeStart.SelectedDate, this.postdatetimeEnd.SelectedDate, this.poster.Text, this.lowerupper.Checked, this.Ip.Text, this.message.Text);
                this.Session["seachpost_fid"] = this.forumid.SelectedValue;
                //this.Session["posttablename"] = BaseConfigs.GetTablePrefix + "posts" + Request["postlist:postslist"];
                this.Session["postswhere"] = value;
                base.Response.Redirect("forum_postgridmanage.aspx");
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
            this.SaveConditionInf.Click += new EventHandler(this.SaveConditionInf_Click);
        }
    }
}