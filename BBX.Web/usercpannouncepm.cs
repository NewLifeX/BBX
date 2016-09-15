using System.Collections.Generic;
using BBX.Entity;
using BBX.Forum;
using BBX.Web.UI;

namespace BBX.Web
{
    public class usercpannouncepm : UserCpPage
    {
        public List<ShortMessage> announcepmlist;

        protected override void ShowPage()
        {
            pagetitle = "公共消息";
            if (!base.IsLogin()) return;

            base.BindItems(announcepmcount);
            announcepmlist = ShortMessage.GetAnnouncePrivateMessageList(16, pageid);
            newnoticecount = Notice.GetNewNoticeCountByUid(userid);
        }
    }
}