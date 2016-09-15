using System;
using System.Web.UI.HtmlControls;
using BBX.Config;
using BBX.Control;
using BBX.Entity;

namespace BBX.Web.Admin
{
    public class restoresetupinf : AdminPage
    {
        protected HtmlForm Form1;
        protected Button RestoreInf;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                //GeneralConfigInfo.Current;
            }
        }

        private void RestoreInf_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                var config = GeneralConfigInfo.Current;
                config.Save();

                //config.Save();;
                AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "恢复论坛初始化设置", "");
                base.RegisterStartupScript("PAGE", "window.location.href='global_restoresetupinf.aspx';");
            }
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.RestoreInf.Click += new EventHandler(this.RestoreInf_Click);
        }
    }
}