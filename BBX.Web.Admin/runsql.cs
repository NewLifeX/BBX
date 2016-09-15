using System;
using System.Web.UI.HtmlControls;
using BBX.Config;
using BBX.Control;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web.Admin
{
    public class runsql : AdminPage
    {
        protected HtmlForm Form1;
        protected pageinfo info1;
        protected TextareaResize sqlstring;
        protected Button RunSqlString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack && !base.IsFounderUid(this.userid))
            {
                base.Response.Write(base.GetShowMessage());
                base.Response.End();
            }
        }

        private void RunSqlString_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                if (!base.IsFounderUid(this.userid))
                {
                    base.Response.Write(base.GetShowMessage());
                    base.Response.End();
                    return;
                }
                if (String.IsNullOrEmpty(this.sqlstring.Text.Trim()))
                {
                    base.RegisterStartupScript("", "<script language=\"javascript\">alert('请您输入SQL语句!');</script>");
                    return;
                }
                //string text = Databases.RunSql(this.sqlstring.Text.Replace("dnt_", BaseConfigs.GetTablePrefix));
                string text = XForum.Meta.Session.Execute(this.sqlstring.Text) + "";
                if (text != string.Empty)
                {
                    base.RegisterStartupScript("", "<script language=\"javascript\">showalert('" + text + "');</script>");
                    return;
                }
                AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "运行SQL语句", this.sqlstring.Text);
                base.RegisterStartupScript("PAGE", "window.location.href='global_runsql.aspx';");
            }
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.RunSqlString.Click += new EventHandler(this.RunSqlString_Click);
        }
    }
}