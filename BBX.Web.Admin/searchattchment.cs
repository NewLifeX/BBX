using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BBX.Common;
using BBX.Control;
using BBX.Forum;
using BBX.Entity;

namespace BBX.Web.Admin
{
    public class searchattchment : AdminPage
    {
        protected HtmlForm Form1;
        protected DropDownTreeList forumid;
        protected BBX.Control.TextBox filesizemin;
        protected BBX.Control.TextBox downloadsmin;
        protected BBX.Control.TextBox postdatetime;
        protected BBX.Control.TextBox description;
        protected BBX.Control.TextBox filesizemax;
        protected BBX.Control.TextBox downloadsmax;
        protected BBX.Control.TextBox filename;
        protected BBX.Control.TextBox poster;
        protected Hint Hint1;
        protected BBX.Control.Button SaveSearchCondition;

        private void SaveSearchCondition_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                this.Session["attchmentwhere"] = Attachment.SearchWhere(forumid.SelectedValue.ToInt(), this.filesizemin.Text.ToInt(), this.filesizemax.Text.ToInt(), this.downloadsmin.Text.ToInt(), this.downloadsmax.Text.ToInt(), this.postdatetime.Text.ToInt(), this.filename.Text, this.description.Text, this.poster.Text);
                base.Response.Redirect("forum_attchemntgrid.aspx");
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
            this.forumid.BuildTree(XForum.Root, "Name", "ID");
            this.forumid.TypeID.Items.RemoveAt(0);
            this.forumid.TypeID.Items.Insert(0, new ListItem("全部", "0"));
        }
    }
}