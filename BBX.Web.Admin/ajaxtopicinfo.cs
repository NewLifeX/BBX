using System;
using System.Collections.Generic;
using System.Data;
using BBX.Common;
using BBX.Config;
using BBX.Entity;
using BBX.Forum;
using NewLife;

namespace BBX.Web.Admin
{
    public class ajaxtopicinfo : UserControlsPageBase
    {
        public List<Topic> topics;
        public string pagelink;
        public int currentpage;
        //public string postname;
        //private int tablelist;
        private int forumid;
        private string posterlist;
        private string keylist;
        private DateTime startdate;
        private DateTime enddate;
        public int pagesize = 4;
        public int fid;

        public ajaxtopicinfo()
        {
            if (Context.Request.Url.ToString().Contains("forumhottopic") || DNTRequest.GetString("pagename") == "forumhottopic")
            {
                if (DNTRequest.GetInt("postnumber", 0) > 0)
                {
                    pagesize = DNTRequest.GetInt("postnumber", 0);
                }
                currentpage = DNTRequest.GetInt("currentpage", 1);
                int num = 0;
                string text = "replies";
                int num2 = 7;
                if (Request["search"] != "")
                {
                    num = DNTRequest.GetInt("forumid", 0);
                    text = ((string.IsNullOrEmpty(Request["showtype"])) ? "replies" : DNTRequest.GetString("showtype", true));
                    num2 = DNTRequest.GetInt("timebetween", 7);
                }
                int hotTopicsCount = Topic.GetHotTopicsCount(num, num2);
                topics = Topic.GetHotTopicsList(20, currentpage, num, text, num2);
                pagelink = AjaxHotTopicPagination(hotTopicsCount, 20, currentpage, string.Format("&forumid={0}&showtype={1}&timebetween={2}", num, text, num2));
                return;
            }
            forumid = DNTRequest.GetInt("_ctl0", 0);
            if (forumid == 0)
            {
                forumid = DNTRequest.GetInt("fid", 0);
            }
            posterlist = DNTRequest.GetString("poster", true);
            keylist = DNTRequest.GetString("title", true);
            startdate = DNTRequest.GetString("postdatetimeStart:postdatetimeStart", true).ToDateTime();
            enddate = DNTRequest.GetString("postdatetimeEnd:postdatetimeEnd", true).ToDateTime();
            currentpage = DNTRequest.GetInt("currentpage", 1);
            //tablelist = DNTRequest.GetInt("tablelist", Posts.GetMaxPostTableId());
            //postname = BaseConfigs.GetTablePrefix + "posts" + tablelist;
            if (DNTRequest.GetInt("postnumber", 0) > 0)
            {
                pagesize = DNTRequest.GetInt("postnumber", 0);
            }
            //int topicListCount = Topics.GetTopicListCount(postname, forumid, posterlist, keylist, startdate, enddate);
            //dt = Topics.GetTopicList(postname, forumid, posterlist, keylist, startdate, enddate, 10, currentpage);
            int topicListCount = Topic.GetTopicListCount(forumid, posterlist, keylist, startdate, enddate);
            topics = Topic.GetTopicList(forumid, posterlist, keylist, startdate, enddate, 10, currentpage);
            pagelink = AjaxPagination(topicListCount, 10, currentpage);
        }

        public string AjaxPagination(int recordcount, int pagesize, int currentpage)
        {
            if (DNTRequest.GetInt("postnumber", 0) > 0)
            {
                return AjaxPagination(recordcount, pagesize, currentpage, "../usercontrols/ajaxtopicinfo.ascx", "_ctl0=" + forumid + "&poster=" + posterlist + "&title=" + keylist + "&postdatetimeStart:postdatetimeStart=" + startdate + "&postdatetimeEnd:postdatetimeEnd=" + enddate + "&postnumber=" + DNTRequest.GetInt("postnumber", 0), "topiclistgrid");
            }
            return AjaxPagination(recordcount, pagesize, currentpage, "../usercontrols/ajaxtopicinfo.ascx", "_ctl0=" + forumid + "&poster=" + posterlist + "&title=" + keylist + "&postdatetimeStart:postdatetimeStart=" + startdate + "&postdatetimeEnd:postdatetimeEnd=" + enddate, "topiclistgrid");
        }

        public string AjaxHotTopicPagination(int recordcount, int pagesize, int currentpage, string condition)
        {
            if (DNTRequest.GetInt("postnumber", 0) > 0)
            {
                return AjaxPagination(recordcount, pagesize, currentpage, "../usercontrols/ajaxtopicinfo.ascx", "pagename=forumhottopic&postnumber=" + DNTRequest.GetInt("postnumber", 0) + condition, "topiclistgrid");
            }
            return AjaxPagination(recordcount, pagesize, currentpage, "../usercontrols/ajaxtopicinfo.ascx", "pagename=forumhottopic" + condition, "topiclistgrid");
        }

        public string AjaxPagination(int recordcount, int pagesize, int currentpage, string usercontrolname, string paramstr, string divname)
        {
            int num = 0;
            string text = "<BR />";
            if (currentpage < 1)
            {
                currentpage = 1;
            }
            if (pagesize != 0)
            {
                num = recordcount / pagesize;
                num = ((recordcount % pagesize != 0) ? (num + 1) : num);
                num = ((num == 0) ? 1 : num);
            }
            int num2 = currentpage + 1;
            int num3 = currentpage - 1;
            int num4 = (currentpage + 5 > num) ? (num - 9) : (currentpage - 4);
            int num5 = (currentpage < 5) ? 10 : (currentpage + 5);
            if (num4 < 1)
            {
                num4 = 1;
            }
            if (num < num5)
            {
                num5 = num;
            }
            if (num4 > 1)
            {
                text += ((currentpage > 1) ? "&nbsp;&nbsp;<a href=\"###\"  onclick=\"javascript:AjaxHelper.Updater('" + usercontrolname + "','" + divname + "', 'load=true&" + paramstr + "&currentpage=" + num3 + "');\" title=\"上一页\">上一页</a>" : "");
            }
            if (num5 > 1)
            {
                for (int i = num4; i <= num5; i++)
                {
                    text += ((currentpage == i) ? ("&nbsp;" + i) : "&nbsp;<a href=\"###\"  onclick=\"javascript:AjaxHelper.Updater('" + usercontrolname + "','" + divname + "', 'load=true&" + paramstr + "&currentpage=" + i + "');\">" + i + "</a>");
                }
            }
            if (num5 < num)
            {
                text += ((currentpage != num) ? "&nbsp;&nbsp;<a href=\"###\" onclick=\"javascript:AjaxHelper.Updater('" + usercontrolname + "','" + divname + "', 'load=true&" + paramstr + "&currentpage=" + num2 + "');\" title=\"下一页\">下一页</a>&nbsp;&nbsp;" : "");
            }
            if (num5 > 1)
            {
                text += "&nbsp; &nbsp; &nbsp; &nbsp;";
            }
            object obj = text;
            return obj + "共 " + num + " 页, 当前第 " + currentpage + " 页, 共 " + recordcount + " 条记录";
        }
    }
}