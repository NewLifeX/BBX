using System;
using System.Web.UI.HtmlControls;
using BBX.Cache;
using BBX.Common;
using BBX.Control;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web.Admin
{
    public class addannounce : AdminPage
    {
        protected HtmlForm Form1;
        protected TextBox displayorder;
        protected TextBox title;
        protected TextBox starttime;
        protected TextBox endtime;
        protected OnlineEditor message;
        protected TextBox poster;
        protected Button AddAnnounceInfo;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack && !this.username.IsNullOrEmpty())
            {
                this.poster.Text = this.username;
                this.starttime.Text = DateTime.Now.ToString();
                this.endtime.Text = DateTime.Now.AddDays(7.0).ToString();
                this.AddAnnounceInfo.ValidateForm = true;
                this.title.AddAttributes("maxlength", "200");
                this.title.AddAttributes("rows", "2");
            }
        }

        private void AddAnnounceInfo_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                //Announcements.CreateAnnouncement(this.username, this.userid, this.title.Text, Utils.StrToInt(this.displayorder.Text, 0), this.starttime.Text, this.endtime.Text, Request["announcemessage_hidden"]);
                Announcement.Create(userid, username, title.Text, Request["announcemessage_hidden"], Utility.ToDateTime(starttime.Text), Utility.ToDateTime(endtime.Text),Int32.Parse(displayorder.Text));
                XCache.Remove(CacheKeys.FORUM_ANNOUNCEMENT_LIST);
                XCache.Remove(CacheKeys.FORUM_SIMPLIFIED_ANNOUNCEMENT_LIST);
                AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "添加公告", "添加公告,标题为:" + this.title.Text);
                base.RegisterStartupScript("PAGE", "window.location.href='global_announcegrid.aspx';");
            }
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.AddAnnounceInfo.Click += new EventHandler(this.AddAnnounceInfo_Click);
            base.Load += new EventHandler(this.Page_Load);
        }
    }
}