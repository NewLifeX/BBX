using System;
using System.Web.UI.HtmlControls;
using BBX.Common;
using BBX.Control;
using BBX.Entity;

namespace BBX.Web.Admin
{
    public class addhelpclass : AdminPage
    {
        protected HtmlLink css;
        protected HtmlLink Link1;
        protected HtmlForm Form1;
        protected TextBox title;
        protected Button add;
        protected TextBox poster;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack && !this.username.IsNullOrEmpty())
            {
                this.poster.Text = this.username;
            }
        }

        protected void add_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                //Helps.AddHelp(this.title.Text, "", 0);
                var entity = new Help();
                entity.Title = title.Text;
                entity.Save();

                AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "添加帮助分类", "添加帮助分类,标题为:" + this.title.Text);
                base.RegisterStartupScript("", "<script>window.location.href='global_helplist.aspx';</script>");
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }
    }
}