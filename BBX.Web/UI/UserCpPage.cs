using BBX.Common;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web.UI
{
    public class UserCpPage : PageBase
    {
        public int pageid = DNTRequest.GetInt("page", 1);
        public int pagecount;
        public string pagenumbers = "";
        public IUser user;
        //public int announcepmcount = PrivateMessages.GetAnnouncePrivateMessageCount();
        public int announcepmcount = ShortMessage.GetAnnouncePrivateMessageCount();
        public int newnoticecount;
        public int pmcount;
        public string[] score = Scoresets.GetValidScoreName();
        public int maxmsg;
        public int usedmsgcount;
        public int usedmsgbarwidth;
        public int unusedmsgbarwidth;

        protected bool IsLogin()
        {
            if (this.userid == -1)
            {
                base.AddErrLine("你尚未登录");
                return false;
            }
            this.user = Users.GetUserInfo(this.userid);
            if (this.user == null)
            {
                base.AddErrLine("用户不存在");
                return false;
            }
            return true;
        }

        protected void BindItems(int recordCount, string pageName)
        {
            this.user = Users.GetUserInfo(this.userid);
            this.pagecount = ((recordCount % 16 == 0) ? (recordCount / 16) : (recordCount / 16 + 1));
            this.pagecount = ((this.pagecount == 0) ? 1 : this.pagecount);
            this.pageid = ((this.pageid < 1) ? 1 : this.pageid);
            this.pageid = ((this.pageid > this.pagecount) ? this.pagecount : this.pageid);
            this.pagenumbers = Utils.GetPageNumbers(this.pageid, this.pagecount, pageName, 8);
        }

        protected void BindItems(int recordCount)
        {
            this.BindItems(recordCount, this.pagename.ToLower());
        }

        protected void BindPrivateMessage(int folder)
        {
            this.pmcount = ShortMessage.GetPrivateMessageCount(this.userid, folder);
            this.usedmsgcount = ShortMessage.GetPrivateMessageCount(this.userid, folder);
            this.maxmsg = this.usergroupinfo.MaxPmNum;
            if (this.maxmsg > 0)
            {
                this.usedmsgbarwidth = this.usedmsgcount * 100 / this.maxmsg;
                this.unusedmsgbarwidth = 100 - this.usedmsgbarwidth;
            }
            this.BindItems(this.pmcount);
        }
    }
}