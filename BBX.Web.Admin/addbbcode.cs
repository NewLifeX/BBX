using System;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
using BBX.Common;
using BBX.Control;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web.Admin
{
    public class addbbcode : AdminPage
    {
        protected HtmlForm Form1;
        protected RadioButtonList available;
        protected UpFile icon;
        protected TextBox tag;
        protected TextareaResize replacement;
        protected TextareaResize example;
        protected TextareaResize explanation;
        protected TextBox param;
        protected TextBox nest;
        protected TextareaResize paramsdescript;
        protected TextareaResize paramsdefvalue;
        protected Hint Hint1;
        protected Button AddAdInfo;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.icon.UpFilePath = base.Server.MapPath(this.icon.UpFilePath);
            }
        }

        private void AddAdInfo_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                this.replacement.is_replace = (this.example.is_replace = (this.explanation.is_replace = (this.paramsdescript.is_replace = (this.paramsdefvalue.is_replace = false))));
                BbCode.CreateBBCCode(int.Parse(this.available.SelectedValue), Regex.Replace(this.tag.Text.Replace("<", "").Replace(">", ""), "^[\\>]|[\\{]|[\\}]|[\\[]|[\\]]|[\\']|[\\.]", ""), this.icon.UpdateFile(), this.replacement.Text, this.example.Text, this.explanation.Text, this.param.Text, this.nest.Text, this.paramsdescript.Text, this.paramsdefvalue.Text);
                AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "添加" + Utils.ProductName + "代码", "TAG为:" + this.tag.Text);
                base.RegisterStartupScript("", "<script>window.location.href='forum_bbcodegrid.aspx';</script>");
            }
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.AddAdInfo.Click += new EventHandler(this.AddAdInfo_Click);
        }
    }
}