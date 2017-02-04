using System;
using System.Web.UI.HtmlControls;
using BBX.Common;
using BBX.Control;
using BBX.Entity;

namespace BBX.Web.Admin
{
    public class addhelp : AdminPage
    {
        protected HtmlLink css;
        protected HtmlLink Link1;
        protected HtmlForm Form1;
        protected TextBox title;
        protected DropDownList type;
        protected OnlineEditor message;
        protected TextBox poster;
        protected Button Addhelp;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack && !this.username.IsNullOrEmpty())
            {
                this.poster.Text = this.username;
                this.type.AddTableData(Help.Meta.Cache.Entities.ToDataTable(), "title", "id");
                this.Addhelp.ValidateForm = true;
                this.title.AddAttributes("maxlength", "200");
                this.title.AddAttributes("rows", "2");
                this.type.DataBind();
            }
        }

        protected void Addhelp_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                if (int.Parse(this.type.SelectedItem.Value) == 0)
                {
                    base.RegisterStartupScript("", "<script>alert('您未选中任何选项');window.location.href='global_addhelp.aspx';</script>");
                    return;
                }

                //Helps.AddHelp(this.title.Text, Request["helpmessage_hidden"].Trim(), int.Parse(this.type.SelectedItem.Value));
                var entity = new Help();
                entity.Title = title.Text;
                entity.Message = Request["helpmessage_hidden"].Trim();
                entity.Pid = Int32.Parse(this.type.SelectedItem.Value);
                entity.Save();

                AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "添加帮助", "添加帮助,标题为:" + this.title.Text);
                base.RegisterStartupScript("", "<script>window.location.href='global_helplist.aspx';</script>");
            }
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.Addhelp.Click += new EventHandler(this.Addhelp_Click);
        }
    }
}