using System;
using System.Web.UI.HtmlControls;

namespace BBX.Web.Admin
{
    public class ajax : AdminPage
    {
        protected internal string ascxpath = "UserControls/";
        protected HtmlForm AjaxCallBackForm;

        private void InitializeComponent()
        {
            this.ID = "Ajax_CallBack_Form";
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void Page_Load(object sender, EventArgs e)
        {
            if (base.Request.Params["AjaxTemplate"] != null)
            {
                try
                {
                    this.AjaxCallBackForm.Controls.Add(base.LoadControl(base.Request.Params["AjaxTemplate"].ToLower().EndsWith(".ascx") ? (this.ascxpath + base.Request.Params["AjaxTemplate"]) : (this.ascxpath + base.Request.Params["AjaxTemplate"] + ".ascx")));
                }
                catch
                {
                }
            }
        }
    }
}