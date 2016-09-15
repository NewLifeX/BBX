using System;
using System.Web.UI.HtmlControls;
using BBX.Common;
using BBX.Control;
using BBX.Entity;

namespace BBX.Web.Admin
{
    public class edithelpclass : AdminPage
    {
        public int id = DNTRequest.GetInt("id", 0);
        public Help helpinfo = new Help();
        protected HtmlLink css;
        protected HtmlLink Link1;
        protected HtmlForm Form1;
        protected TextBox title;
        protected TextBox orderby;
        protected Button updateclass;
        protected TextBox poster;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.helpinfo = Help.FindByID(this.id);
                if (this.username != null && this.username != "")
                {
                    if (this.id == 0)
                    {
                        return;
                    }
                    this.poster.Text = this.username;
                    this.orderby.Text = this.helpinfo.Orderby.ToString();
                    this.title.Text = this.helpinfo.Title;
                }
            }
        }

        protected void updateclass_Click(object sender, EventArgs e)
        {
            //Helps.UpdateHelp(this.id, this.title.Text, "", 0, int.Parse(this.orderby.Text));
            var entity = Help.FindByID(id);
            if (entity != null)
            {
                entity.Title = title.Text;
                entity.Pid = 0;
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
            this.updateclass.Click += new EventHandler(this.updateclass_Click);
        }
    }
}