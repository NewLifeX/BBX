using System;
using System.Web;
using BBX.Common;
using NewLife.Web;

namespace BBX.Web.Admin
{
    public partial class systeminf : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.LoadSystemInf();
            }
        }

        protected void LoadSystemInf()
        {
            base.Response.Expires = 0;
            base.Response.CacheControl = "no-cache";
            DateTime now = DateTime.Now;
            this.servername.Text = base.Server.MachineName;
            this.serverip.Text = base.Request.ServerVariables["LOCAL_ADDR"];
            this.server_name.Text = base.Request.ServerVariables["SERVER_NAME"];
            int build = Environment.Version.Build;
            int major = Environment.Version.Major;
            int minor = Environment.Version.Minor;
            int revision = Environment.Version.Revision;
            this.servernet.Text = ".NET CLR  " + major + "." + minor + "." + build + "." + revision;
            this.serverms.Text = Environment.OSVersion.ToString();
            this.serversoft.Text = base.Request.ServerVariables["SERVER_SOFTWARE"];
            this.serverport.Text = base.Request.ServerVariables["SERVER_PORT"];
            this.serverout.Text = base.Server.ScriptTimeout.ToString();
            this.cl.Text = base.Request.ServerVariables["HTTP_ACCEPT_LANGUAGE"];
            this.servertime.Text = DateTime.Now.ToString();
            this.servernpath.Text = base.Request.ServerVariables["PATH_TRANSLATED"];
            this.serverhttps.Text = base.Request.ServerVariables["HTTPS"];
            HttpBrowserCapabilities browser = base.Request.Browser;
            this.ie.Text = browser.Browser.ToString();
            this.cookies.Text = browser.Cookies.ToString();
            this.frames.Text = browser.Frames.ToString();
            this.javaa.Text = browser.JavaApplets.ToString();
            this.javas.Text = browser.EcmaScriptVersion.ToString();
            this.ms.Text = browser.Platform.ToString();
            this.vbs.Text = browser.VBScript.ToString();
            this.vi.Text = browser.Version.ToString();
            this.cip.Text = WebHelper.UserHost;
            DateTime now2 = DateTime.Now;
            this.runtime.Text = (now2 - now).TotalMilliseconds.ToString();
        }

        public bool chkobj(string obj)
        {
            bool result;
            try
            {
                base.Server.CreateObject(obj);
                result = true;
            }
            catch (Exception ex)
            {
                ex.ToString();
                result = false;
            }
            return result;
        }

        private void for5000_Click(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            int num = 0;
            for (int i = 1; i <= 50000000; i++)
            {
                num += i;
            }
            DateTime now2 = DateTime.Now;
            this.l5000.Text = (now2 - now).TotalMilliseconds.ToString() + "毫秒";
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.for5000.Click += new EventHandler(this.for5000_Click);
        }
    }
}