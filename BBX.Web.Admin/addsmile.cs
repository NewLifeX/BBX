using System;
using System.Data;
using System.Web.UI.HtmlControls;
using BBX.Common;
using BBX.Control;
using BBX.Entity;
using BBX.Forum;
using NewLife.Web;

namespace BBX.Web.Admin
{
    public class addsmile : AdminPage
    {
        protected HtmlForm Form1;
        protected TextBox displayorder;
        protected UpFile url;
        protected TextBox code;
        protected Button AddSmileInfo;
        protected Hint Hint1;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                string arg = "";
                var typeid = WebHelper.RequestInt("typeid");
                //foreach (var sm in Smilie.FindAllWithCache())
                //{
                //    if (sm.ID == typeid)
                //    {
                //        arg = sm.Url;
                //    }
                //}
                var sm = Smilie.FindByID(typeid);
                if (sm != null) arg = sm.Url;
                this.url.UpFilePath = base.Server.MapPath(string.Format("../../editor/images/smilies/{0}/", arg));
            }
        }

        private void AddSmileInfo_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                var typeid = WebHelper.RequestInt("typeid");
                var sm = Smilie.FindByID(typeid);
                AdminForums.CreateSmilies(int.Parse(this.displayorder.Text), typeid, this.code.Text, sm.Url + "/" + this.url.UpdateFile(), this.userid, this.username, this.usergroupid, this.grouptitle, this.ip);
                base.RegisterStartupScript("PAGE", "window.location.href='forum_smilegrid.aspx?typeid=" + typeid + "';");
            }
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.AddSmileInfo.Click += new EventHandler(this.AddSmileInfo_Click);
        }
    }
}