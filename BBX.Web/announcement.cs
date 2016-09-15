using System.Data;
using BBX.Common;
using BBX.Forum;
using BBX.Entity;
using System.Collections.Generic;

namespace BBX.Web
{
    public class announcement : PageBase
    {
        //public DataTable announcementlist = Announcements.GetAnnouncementList(Utils.GetDateTime(), "2099-12-31 23:59:59");
        public List<Announcement> announcementlist = Announcement.GetAvailableList();

        protected override void ShowPage()
        {
            this.pagetitle = "公告";
            if (this.announcementlist.Count == 0)
            {
                base.AddErrLine("当前没有任何公告");
            }
        }
    }
}