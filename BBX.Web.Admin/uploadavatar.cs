using System;
using System.Web.UI.HtmlControls;
using BBX.Cache;
using BBX.Control;

namespace BBX.Web.Admin
{
    public class uploadavatar : AdminPage
    {
        protected HtmlForm Form1;
        protected pageinfo info1;
        protected UpFile url;
        protected Button UpdateAvatarCache;
        protected Hint Hint1;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.url.UpFilePath = base.Server.MapPath(this.url.UpFilePath);
            }
        }

        private void UpdateAvatarCache_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                XCache.Remove(CacheKeys.FORUM_COMMON_AVATAR_LIST);
                this.url.UpdateFile();
                base.RegisterStartupScript("PAGE", "window.location.href='global_avatargrid.aspx';");
            }
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.UpdateAvatarCache.Click += new EventHandler(this.UpdateAvatarCache_Click);
        }
    }
}