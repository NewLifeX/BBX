using System.Collections.Generic;
using BBX.Common;
using BBX.Entity;
using BBX.Web.UI;

namespace BBX.Web
{
    public class usercpnotice : UserCpPage
    {
        public List<Notice> noticeinfolist = new List<Notice>();
        public string filter = DNTRequest.GetString("filter", true).ToLower();
        public int reccount;

        protected override void ShowPage()
        {
            pagetitle = "用户控制面板";
            if (!base.IsLogin()) return;

            NoticeType noticetype = Notice.GetNoticetype(filter);
            reccount = Notice.GetNoticeCountByUid(userid, noticetype);
            base.BindItems(reccount, "usercpnotice.aspx?filter=" + filter);
            //noticeinfolist = Notices.GetNoticeinfoCollectionByUid(userid, noticetype, pageid, 16);
            noticeinfolist = Notice.FindAllByUidAndType(userid, noticetype, (pageid - 1) * 16, 16);
            newnoticecount = Notice.GetNewNoticeCountByUid(userid);
            Notice.UpdateNoticeNewByUid(userid, 0);
            //OnlineUsers.UpdateNewNotices(olid);
            var online = Online.FindByID(olid);
            if (online != null) online.UpdateNewNotices(0);
        }
    }
}