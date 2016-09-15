using System;
using System.Web.UI.HtmlControls;
using BBX.Common;
using BBX.Control;
using BBX.Entity;

namespace BBX.Web.Admin
{
    public class edithelp : AdminPage
    {
        public int id = DNTRequest.GetInt("id", 0);
        public Help helpinfo = new Help();
        protected HtmlHead Head1;
        protected HtmlLink css;
        protected HtmlLink Link1;
        protected HtmlForm Form1;
        protected TextBox title;
        protected DropDownList type;
        protected TextBox orderby;
        protected OnlineEditor help;
        protected Button updatehelp;
        protected TextBox poster;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.helpinfo = Help.FindByID(this.id);
            if (this.helpinfo.Pid == 0)
            {
                base.Response.Redirect("global_edithelpclass.aspx?id=" + this.id);
                return;
            }
            if (!this.Page.IsPostBack && this.username != null && this.username != "")
            {
                if (this.id == 0)
                {
                    return;
                }
                this.poster.Text = this.username;
                this.type.AddTableData(Help.Meta.Cache.Entities.ToDataTable(), "title", "id");
                this.type.SelectedValue = this.helpinfo.Pid.ToString();
                this.orderby.Text = this.helpinfo.Orderby.ToString();
                this.title.Text = this.helpinfo.Title;
                this.help.Text = this.helpinfo.Message;
                this.updatehelp.ValidateForm = true;
                this.title.AddAttributes("maxlength", "200");
                this.title.AddAttributes("rows", "2");
                this.type.DataBind();
            }
        }

        protected void updatehelp_Click(object sender, EventArgs e)
        {
            //Helps.UpdateHelp(this.id, this.title.Text, Request["helpmessage_hidden"].Trim(), int.Parse(this.type.SelectedValue), int.Parse(this.orderby.Text));
            var entity = Help.FindByID(id);
            if (entity != null)
            {
                entity.Title = title.Text;
                entity.Message = Request["helpmessage_hidden"].Trim();
                entity.Pid = Int32.Parse(this.type.SelectedValue);
                entity.Orderby = Int32.Parse(orderby.Text);
                entity.Save();
            }
            base.Response.Redirect("global_helplist.aspx");
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.updatehelp.Click += new EventHandler(this.updatehelp_Click);
        }
    }
}