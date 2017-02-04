using System;
using System.Web.UI.HtmlControls;
using BBX.Common;
using BBX.Control;
using BBX.Forum;

namespace BBX.Web.Admin
{
    public class addiconfile : AdminPage
    {
        protected HtmlForm Form1;
        protected TextBox displayorder;
        protected UpFile url;
        protected TextBox code;
        protected Button AddIncoInfo;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.url.UpFilePath = base.Server.MapPath(this.url.UpFilePath);
            }
        }

        private void AddIncoInfo_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                AdminForums.CreateSmilies(this.displayorder.Text.ToInt(0), 1, this.code.Text, this.url.UpdateFile(), this.userid, this.username, this.usergroupid, this.grouptitle, this.ip);
                base.RegisterStartupScript("PAGE", "window.location.href='forum_iconfilegrid.aspx';");
            }
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.AddIncoInfo.Click += new EventHandler(this.AddIncoInfo_Click);
        }
    }
}