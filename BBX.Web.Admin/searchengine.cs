using System;
using System.Web.UI.HtmlControls;
using BBX.Cache;
using BBX.Config;
using BBX.Control;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web.Admin
{
    public class searchengine : AdminPage
    {
        protected HtmlForm Form1;
        protected RadioButtonList archiverstatus;
        protected TextareaResize seotitle;
        protected TextareaResize seodescription;
        protected TextareaResize seokeywords;
        protected TextareaResize seohead;
        protected RadioButtonList sitemapstatus;
        protected TextBox sitemapttl;
        protected RadioButtonList aspxrewrite;
        protected TextBox extname;
        protected RadioButtonList iisurlrewrite;
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
            var config = GeneralConfigInfo.Current;
            this.seotitle.Text = config.Seotitle;
            this.seokeywords.Text = config.Seokeywords;
            this.seodescription.Text = config.Seodescription;
            this.seohead.Text = config.Seohead;
            this.archiverstatus.SelectedValue = config.Archiverstatus.ToString();
            this.sitemapstatus.SelectedValue = config.Sitemapstatus.ToString();
            this.sitemapttl.Text = config.Sitemapttl.ToString();
            this.aspxrewrite.SelectedValue = config.Aspxrewrite.ToString();
            this.extname.Text = config.Extname;
            this.iisurlrewrite.SelectedValue = config.Iisurlrewrite.ToString();
        }

        private void SaveInfo_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                var config = GeneralConfigInfo.Current;
                config.Seotitle = this.seotitle.Text;
                config.Seokeywords = this.seokeywords.Text;
                config.Seodescription = this.seodescription.Text;
                config.Seohead = this.seohead.Text;
                config.Archiverstatus = (int)Convert.ToInt16(this.archiverstatus.SelectedValue);
                config.Sitemapstatus = (int)Convert.ToInt16(this.sitemapstatus.SelectedValue);
                config.Sitemapttl = this.sitemapttl.Text.ToInt();
                config.Aspxrewrite = (int)Convert.ToInt16(this.aspxrewrite.SelectedValue);
                if (this.extname.Text.IsNullOrWhiteSpace())
                {
                    base.RegisterStartupScript("", "<script>alert('您未输入相应的伪静态url扩展名!');</script>");
                    return;
                }
                config.Extname = this.extname.Text.Trim();
                //if (config.Aspxrewrite == 1)
                //{
                //    AdminForums.SetForumsPathList(true, config.Extname);
                //}
                //else
                //{
                //    AdminForums.SetForumsPathList(false, config.Extname);
                //}
                XCache.Remove(CacheKeys.FORUM_FORUM_LIST);
                config.Iisurlrewrite = iisurlrewrite.SelectedValue.ToInt();
                config.Save(); ;
                AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "搜索引擎优化设置", "");
                base.RegisterStartupScript("PAGE", "window.location.href='global_searchengine.aspx';");
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