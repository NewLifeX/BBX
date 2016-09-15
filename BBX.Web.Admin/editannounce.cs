using System;
using System.Web.UI.HtmlControls;
using BBX.Cache;
using BBX.Common;
using BBX.Control;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web.Admin
{
    public class editannounce : AdminPage
    {
        protected HtmlForm Form1;
        protected TextBox displayorder;
        protected TextBox title;
        protected TextBox starttime;
        protected TextBox endtime;
        protected OnlineEditor announce;
        protected TextBox poster;
        protected Button UpdateAnnounceInfo;
        protected Button DeleteAnnounce;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                if (String.IsNullOrEmpty(Request["id"]))
                {
                    base.Response.Redirect("global_announcegrid.aspx");
                    return;
                }
                this.LoadAnnounceInf(DNTRequest.GetInt("id", -1));
                this.UpdateAnnounceInfo.ValidateForm = true;
                this.title.AddAttributes("maxlength", "200");
                this.title.AddAttributes("rows", "2");
            }
        }

        public void LoadAnnounceInf(int id)
        {
            var announcement = Announcement.FindByID(id);
            if (announcement == null) return;

            this.displayorder.Text = announcement.DisplayOrder.ToString();
            this.title.Text = announcement.Title;
            this.poster.Text = announcement.Poster;
            this.starttime.Text = Utils.GetStandardDateTime(announcement.StartTime.ToString());
            this.endtime.Text = Utils.GetStandardDateTime(announcement.EndTime.ToString());
            this.announce.Text = announcement.Message;
        }

        private void UpdateAnnounceInfo_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                //Announcements.UpdateAnnouncement(new AnnouncementInfo
                //{
                //    Id = DNTRequest.GetInt("id", 0),
                //    Poster = this.poster.Text.Trim(),
                //    Title = this.title.Text.Trim(),
                //    Displayorder = this.displayorder.Text.ToInt(),
                //    Starttime = this.starttime.Text.ToDateTime(),
                //    Endtime = this.endtime.Text.ToDateTime(),
                //    Message = Request["announcemessage_hidden"].Trim()
                //});

                var entity = Announcement.FindByID(DNTRequest.GetInt("id", 0));
                entity.Poster = poster.Text.Trim();
                entity.Title = title.Text.Trim();
                entity.Message = Request["announcemessage_hidden"].Trim();
                entity.StartTime = this.starttime.Text.ToDateTime();
                entity.EndTime = this.endtime.Text.ToDateTime();
                entity.DisplayOrder = Int32.Parse(displayorder.Text);
                entity.Save();

                XCache.Remove(CacheKeys.FORUM_ANNOUNCEMENT_LIST);
                XCache.Remove(CacheKeys.FORUM_SIMPLIFIED_ANNOUNCEMENT_LIST);
                AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "更新公告", "更新公告,标题为:" + this.title.Text);
                base.RegisterStartupScript("PAGE", "window.location.href='global_announcegrid.aspx';");
            }
        }

        private void DeleteAnnounce_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                //Announcements.DeleteAnnouncements(Request["id"]);
                var entity = Announcement.FindByID(Int32.Parse(Request["id"]));
                if (entity != null) entity.Delete();
                AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "删除公告", "删除公告,标题为:" + this.title.Text);
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
            this.UpdateAnnounceInfo.Click += new EventHandler(this.UpdateAnnounceInfo_Click);
            this.DeleteAnnounce.Click += new EventHandler(this.DeleteAnnounce_Click);
        }
    }
}