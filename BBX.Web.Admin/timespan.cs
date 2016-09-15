using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using BBX.Common;
using BBX.Config;
using BBX.Control;
using BBX.Entity;

namespace BBX.Web.Admin
{
    public class timespan : AdminPage
    {
        protected HtmlForm Form1;
        protected TextareaResize visitbanperiods;
        protected TextareaResize postmodperiods;
        protected TextareaResize searchbanperiods;
        protected TextareaResize postbanperiods;
        protected TextareaResize attachbanperiods;
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
            this.visitbanperiods.Text = config.Visitbanperiods;
            this.postbanperiods.Text = config.Postbanperiods;
            this.postmodperiods.Text = config.Postmodperiods;
            this.searchbanperiods.Text = config.Searchbanperiods;
            this.attachbanperiods.Text = config.Attachbanperiods;
        }

        private void SaveInfo_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                var config = GeneralConfigInfo.Current;
                var hashtable = new Hashtable();
                hashtable.Add("禁止访问时间段", this.visitbanperiods.Text);
                hashtable.Add("禁止发帖时间段", this.postbanperiods.Text);
                hashtable.Add("发帖审核时间段", this.postmodperiods.Text);
                hashtable.Add("禁止下载附件时间段", this.attachbanperiods.Text);
                hashtable.Add("禁止全文搜索时间段", this.searchbanperiods.Text);
                string text = "";
                if (!Utils.IsRuleTip(hashtable, "timesect", out text))
                {
                    base.RegisterStartupScript("erro", "<script>alert('" + text + ",时间格式错误');</script>");
                    return;
                }
                config.Visitbanperiods = this.visitbanperiods.Text;
                config.Postbanperiods = this.postbanperiods.Text;
                config.Postmodperiods = this.postmodperiods.Text;
                config.Searchbanperiods = this.searchbanperiods.Text;
                config.Attachbanperiods = this.attachbanperiods.Text;
                config.Save(); ;
                AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "时间段设置", "");
                base.RegisterStartupScript("PAGE", "window.location.href='global_timespan.aspx';");
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