using System;
using System.Web.UI.HtmlControls;
using BBX.Cache;
using BBX.Common;
using BBX.Config;
using BBX.Control;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web.Admin
{
    public class siteset : AdminPage
    {
        protected BBX.Control.RadioButtonList iisurlrewrite;
        protected HtmlForm Form1;
        protected BBX.Control.TextBox webtitle;
        protected BBX.Control.TextBox weburl;
        protected BBX.Control.TextBox forumtitle;
        protected BBX.Control.RadioButtonList licensed;
        protected BBX.Control.TextBox icp;
        protected BBX.Control.RadioButtonList debug;
        protected TextareaResize Statcode;
        protected TextareaResize Linktext;
        protected Hint Hint1;
        protected BBX.Control.RadioButtonList closed;
        protected TextareaResize closedreason;
        protected BBX.Control.Button SaveInfo;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.LoadConfigInfo();
                this.closed.Items[0].Attributes.Add("onclick", "setStatus(true)");
                this.closed.Items[1].Attributes.Add("onclick", "setStatus(false)");
            }
        }

        public void LoadConfigInfo()
        {
            GeneralConfigInfo config = GeneralConfigInfo.Current;
            this.forumtitle.Text = config.Forumtitle;
            this.webtitle.Text = config.Webtitle;
            this.weburl.Text = config.Weburl;
            this.licensed.SelectedValue = config.Licensed + "";
            this.icp.Text = config.Icp;
            this.debug.SelectedValue = config.Debug + "";
            this.Statcode.Text = config.Statcode;
            this.Linktext.Text = config.Linktext;
            this.closed.SelectedValue = config.Closed + "";
            this.closedreason.Text = config.Closedreason;
        }

        private void SaveInfo_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                GeneralConfigInfo config = GeneralConfigInfo.Current;
                config.Forumtitle = this.forumtitle.Text;
                config.Webtitle = this.webtitle.Text;
                config.Weburl = this.weburl.Text;
                config.Licensed = this.licensed.SelectedValue.ToInt();
                config.Icp = this.icp.Text;
                config.Debug = this.debug.SelectedValue.ToInt();
                config.Statcode = this.Statcode.Text;
                config.Linktext = this.Linktext.Text;
                config.Closed = this.closed.SelectedValue.ToInt();
                config.Closedreason = this.closedreason.Text;
                config.Save(); ;
                Caches.ReSetConfig();
                AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "站点设置", "");
                base.RegisterStartupScript("PAGE", "window.location.href='global_siteset.aspx';");
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
            this.forumtitle.IsReplaceInvertedComma = false;
            this.webtitle.IsReplaceInvertedComma = false;
            this.weburl.IsReplaceInvertedComma = false;
            this.icp.IsReplaceInvertedComma = false;
        }
    }
}