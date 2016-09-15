using System;
using System.Web.UI.HtmlControls;
using BBX.Config;
using BBX.Control;
using BBX.Entity;

namespace BBX.Web.Admin
{
    public class userrights : AdminPage
    {
        protected HtmlForm Form1;
        protected Hint Hint1;
        protected Button SaveInfo;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.LoadConfigInfo();
            }
        }

        public void LoadConfigInfo()
        {
            //GeneralConfigs.Deserialize(base.Server.MapPath("../../config/general.config"));
        }

        private void SaveInfo_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                GeneralConfigInfo configinfo = GeneralConfigInfo.Current;

                //GeneralConfigs.Serialiaze(configinfo, base.Server.MapPath("../../config/general.config"));
                configinfo.Save();
                AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "用户权限设置", "");
                base.RegisterStartupScript("PAGE", "window.location.href='global_userrights.aspx';");
            }
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.SaveInfo.Click += new EventHandler(this.SaveInfo_Click);
        }
    }
}