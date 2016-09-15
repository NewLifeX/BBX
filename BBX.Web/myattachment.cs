using System.Collections.Generic;
using BBX.Common;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web
{
    public class myattachment : PageBase
    {
        public List<MyAttachment> myattachmentlist;
        public int pageid = DNTRequest.GetInt("page", 1);
        public int pagecount;
        public int attachmentcount;
        public string pagenumbers;
        public IUser user;
        public int typeid = DNTRequest.GetInt("typeid", 0);

        protected override void ShowPage()
        {
            this.pagetitle = "用户控制面板";
            if (this.userid == -1)
            {
                base.AddErrLine("你尚未登录");
                return;
            }
            this.user = Users.GetUserInfo(this.userid);
            this.attachmentcount = MyAttachment.SearchCount(userid, typeid);
            this.pagecount = ((this.attachmentcount % 16 == 0) ? (this.attachmentcount / 16) : (this.attachmentcount / 16 + 1));
            this.pagecount = ((this.pagecount == 0) ? 1 : this.pagecount);
            this.pageid = ((this.pageid < 1) ? 1 : this.pageid);
            this.pageid = ((this.pageid > this.pagecount) ? this.pagecount : this.pageid);
            this.myattachmentlist = MyAttachment.Search(userid, typeid, null, (pageid - 1) * 16, 16);
            this.pagenumbers = Utils.GetPageNumbers(this.pageid, this.pagecount, (this.typeid > 0) ? ("myattachment.aspx?typeid=" + this.typeid) : "myattachment.aspx", 10);
        }
    }
}