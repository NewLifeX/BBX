using System;
using System.Web.UI.HtmlControls;
using BBX.Common;
using BBX.Config;
using BBX.Control;
using BBX.Entity;

namespace BBX.Web.Admin
{
    public class editorset : AdminPage
    {
        protected HtmlForm Form1;
        protected RadioButtonList bbcodemode;
        protected RadioButtonList defaulteditormode;
        protected RadioButtonList allowswitcheditor;
        protected RadioButtonList swfupload;
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
            GeneralConfigInfo config = GeneralConfigInfo.Current;
            this.allowswitcheditor.SelectedValue = config.Allowswitcheditor.ToString();
            this.bbcodemode.SelectedValue = config.Bbcodemode.ToString();
            this.defaulteditormode.SelectedValue = config.Defaulteditormode.ToString();
            this.swfupload.SelectedValue = config.Swfupload.ToString();
        }

        private void SaveInfo_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                GeneralConfigInfo config = GeneralConfigInfo.Current;
                config.Bbcodemode = this.bbcodemode.SelectedValue.ToInt();
                config.Defaulteditormode = this.defaulteditormode.SelectedValue.ToInt();
                config.Allowswitcheditor = this.allowswitcheditor.SelectedValue.ToInt();
                config.Swfupload = (int)Convert.ToInt16(this.swfupload.SelectedValue);
                config.Save();

                //config.Save();;
                AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "编辑器设置", "");
                base.RegisterStartupScript("PAGE", "window.location.href='global_editorset.aspx';");
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