using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BBX.Common;
using BBX.Entity;
using BBX.Forum;
using NewLife.Web;

namespace BBX.Web
{
    public class onlineuser : PageBase
    {
        public List<Online> onlineuserlist;
        public int onlineusernumber;
        public int pageid = DNTRequest.GetInt("page", 1);
        public int pagecount;
        public string pagenumbers = "";
        public int totalonline;
        public int totalonlineuser;
        public int totalonlineguest;
        public int totalonlineinvisibleuser;
        public string highestonlineusercount;
        public string highestonlineusertime;
        private int startrow;
        private int endrow;

        public String dir;

        protected override void ShowPage()
        {
            this.pagetitle = "在线列表";

            var order = Request["order"];
            if (String.IsNullOrEmpty(order)) order = WebHelper.ReadCookie("onlinelist", "order");
            dir = Request["dir"];
            if (String.IsNullOrEmpty(dir)) dir = WebHelper.ReadCookie("onlinelist", "dir");
            var desc = dir == "desc";

            this.onlineusernumber = this.onlineusercount;

            var pagesize = WebHelper.RequestInt("PageSize");
            if (pagesize <= 0) pagesize = 16;
            this.pagecount = ((this.onlineusernumber % pagesize == 0) ? (this.onlineusernumber / pagesize) : (this.onlineusernumber / pagesize + 1));
            this.pagecount = ((this.pagecount == 0) ? 1 : this.pagecount);
            if (this.pageid <= 1)
            {
                this.pageid = 1;
                this.startrow = 0;
                this.endrow = pagesize - 1;
            }
            else
            {
                this.pageid = ((this.pageid > this.pagecount) ? this.pagecount : this.pageid);
                this.startrow = (this.pageid - 1) * pagesize;
                this.endrow = this.pageid * pagesize;
            }

            this.startrow = ((this.startrow >= this.onlineusernumber) ? (this.onlineusernumber - 1) : this.startrow);
            this.endrow = ((this.endrow >= this.onlineusernumber) ? (this.onlineusernumber - 1) : this.endrow);
            //onlineuserlist = onlineuserlist.Skip(startrow).Take(endrow - startrow + 1).ToList();

            //onlineuserlist = Online.GetOnlineUserList(this.onlineusercount, order, desc, out this.totalonlineguest, out this.totalonlineuser, out this.totalonlineinvisibleuser);
            onlineuserlist = Online.GetList(0, order, desc, startrow, pagesize);

            var st = Online.GetStat();
            this.totalonline = st.Total;
            this.totalonlineuser = st.User;
            this.totalonlineinvisibleuser = st.Invisible;
            this.totalonlineguest = st.Guest;

            // 写入Cookie
            WebHelper.WriteCookie("onlinelist", "order", order);
            WebHelper.WriteCookie("onlinelist", "dir", dir);
            dir = desc ? "asc" : "desc";
            this.pagenumbers = ((String.IsNullOrEmpty(DNTRequest.GetString("search"))) ? Utils.GetPageNumbers(this.pageid, this.pagecount, "onlineuser.aspx", 8) : Utils.GetPageNumbers(this.pageid, this.pagecount, "onlineuser.aspx", 8));
            this.totalonline = this.onlineusercount;
            this.highestonlineusercount = Statistic.Current.HighestOnlineUserCount + "";
            this.highestonlineusertime = Statistic.Current.HighestOnlineUserTime.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}