using System.Collections.Generic;
using XUser = BBX.Entity.User;
using System.Data;
using BBX.Common;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web
{
    public class showuser : PageBase
    {
        public List<IUser> userlist;
        public int pageid = DNTRequest.GetInt("page", 1);
        public int totalusers;
        public int pagecount;
        public string pagenumbers;
        public string orderby = "";
        public string ordertype = "";

        protected override void ShowPage()
        {
            this.pagetitle = "用户";
            if (this.config.Memliststatus != 1)
            {
                base.AddErrLine("系统不允许查看用户列表");
                return;
            }
            this.orderby = DNTRequest.GetHtmlEncodeString("orderby", true).Trim();
            if (!"id|name|credits|posts|adminid|joindate|lastactivity".Contains(this.orderby))
            {
                this.orderby = "";
            }
            if (!Utils.StrIsNullOrEmpty(this.orderby) && !Utils.InArray(this.orderby, "id,name,credits,posts,adminid,joindate,lastactivity"))
            {
                this.orderby = "id";
            }
            this.ordertype = DNTRequest.GetHtmlEncodeString("ordertype", true).Trim();
            if (!"asc|desc".Contains(this.ordertype))
            {
                this.ordertype = "";
            }
            if (!this.ordertype.Equals("desc") && !this.ordertype.Equals("asc"))
            {
                this.ordertype = "desc";
            }
            //this.totalusers = Users.GetUserCountByAdmin(DNTRequest.GetString("orderby"));
            this.totalusers = XUser.Meta.Count;
            this.pagecount = ((this.totalusers % 20 == 0) ? (this.totalusers / 20) : (this.totalusers / 20 + 1));
            this.pagecount = ((this.pagecount == 0) ? 1 : this.pagecount);
            this.pageid = ((this.pageid < 1) ? 1 : this.pageid);
            this.pageid = ((this.pageid > this.pagecount) ? this.pagecount : this.pageid);
            //this.userlist = Users.GetUserList(20, this.pageid, this.orderby, this.ordertype);
            this.userlist = XUser.GetUserList(20, this.pageid, this.orderby, this.ordertype);
            this.pagenumbers = Utils.GetPageNumbers(this.pageid, this.pagecount, string.Format("showuser.aspx{0}", string.Format("?orderby={0}&ordertype={1}", this.orderby, this.ordertype)), 8);
        }
    }
}