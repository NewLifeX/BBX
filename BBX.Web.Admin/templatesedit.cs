using System;
using System.IO;
using System.Text;
using System.Web.UI.HtmlControls;
using BBX.Common;
using BBX.Control;

namespace BBX.Web.Admin
{
    public class templatesedit : AdminPage
    {
        protected HtmlForm Form1;
        protected ColorPicker ColorPicker1;
        protected TextareaResize templatenew;
        protected Button SavaTemplateInfo;
        public string filenamefullpath;
        public string path;
        public string filename;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.path = Request["path"];
            if (String.IsNullOrEmpty(this.path))
            {
                base.Response.Redirect("global_templatetree.aspx");
                return;
            }
            this.filename = Request["filename"];
            if (this.filename.EndsWith(".htm") || this.filename.EndsWith(".html") || this.filename.EndsWith(".js") || this.filename.EndsWith(".css") || this.filename.EndsWith(".xml"))
            {
                this.filenamefullpath = "../../templates/" + this.path + "/" + this.filename;
                this.ViewState["path"] = this.path;
                this.ViewState["filename"] = this.filename;
                this.ViewState["templateid"] = Request["templateid"];
                this.ViewState["templatename"] = Request["templatename"];
                if (this.Page.IsPostBack)
                {
                    return;
                }
                using (StreamReader streamReader = new StreamReader(base.Server.MapPath(this.filenamefullpath), Encoding.UTF8))
                {
                    this.templatenew.Text = streamReader.ReadToEnd();
                    streamReader.Close();
                    return;
                }
            }
            base.Response.Redirect("global_templatetree.aspx");
        }

        private void SavaTemplateInfo_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                string str = this.ViewState["path"].ToString();
                string text = this.ViewState["filename"].ToString();
                this.filenamefullpath = base.Server.MapPath("../../templates/" + str + "/" + text);
                if ((text.EndsWith(".htm") || text.EndsWith(".html") || text.EndsWith(".js") || text.EndsWith(".css") || text.EndsWith(".xml")) && Utils.FileExists(this.filenamefullpath))
                {
                    using (FileStream fileStream = new FileStream(this.filenamefullpath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        byte[] bytes = Encoding.UTF8.GetBytes(this.templatenew.Text);
                        fileStream.Write(bytes, 0, bytes.Length);
                        fileStream.Close();
                    }
                }
                base.RegisterStartupScript("PAGE", "window.location.href='global_templatetree.aspx?path=" + this.ViewState["path"].ToString().Split('\\')[0] + "&templateid=" + this.ViewState["templateid"].ToString() + "&templatename=" + this.ViewState["templatename"].ToString() + "';");
            }
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.SavaTemplateInfo.Click += new EventHandler(this.SavaTemplateInfo_Click);
        }
    }
}