using System.Collections.Generic;
using System.Web;
using BBX.Common;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web
{
    public class search : PageBase
    {
        public int searchid = DNTRequest.GetInt("searchid", -1);
        public int pageid = DNTRequest.GetInt("page", 1);
        public int topiccount;
        public int pagecount;
        public string pagenumbers = "";
        public int searchresultcount;
        public List<Topic> topiclist = new List<Topic>();
        public bool searchpost;
        public string type = DNTRequest.GetHtmlEncodeString("type", true).ToLower();
        public int topicpageid = DNTRequest.GetInt("topicpage", 1);
        public int topicpagecount;
        public string topicpagenumbers = "";
        private string msg = "";
        public string keyword = DNTRequest.GetHtmlEncodeString("keyword").Trim();
        public string poster = DNTRequest.GetHtmlEncodeString("poster").Trim();
        public int advsearch = DNTRequest.GetInt("advsearch", 0);
        public int searchsubmit = DNTRequest.GetInt("searchsubmit", 0);
        public SearchType searchType;

        protected override void ShowPage()
        {
            this.pagetitle = "搜索";
            this.GetSearchType();
            if (this.searchsubmit == 0 && !this.ispost)
            {
                if (!UserAuthority.Search(this.usergroupinfo, ref this.msg))
                {
                    base.AddErrLine(this.msg);
                    return;
                }
                if (this.searchid <= 0) return;

                if (this.searchType == SearchType.Error)
                {
                    base.AddErrLine("非法的参数信息");
                    return;
                }
                switch (this.searchType)
                {
                    case SearchType.ByPoster:
                        this.topiclist = SearchCache.GetSearchCacheList(this.searchid, 16, this.topicpageid, out this.topiccount, SearchType.TopicTitle);
                        this.topicpageid = this.CalculateCurrentPage(this.topiccount, this.topicpageid, out this.topicpagecount);
                        this.topicpagenumbers = ((this.topicpagecount > 1) ? Utils.GetPageNumbers(this.topicpageid, this.topicpagecount, "search.aspx?type=" + this.type + "&searchid=" + this.searchid + "&keyword=" + this.keyword + "&poster=" + this.poster, 8, "topicpage", "#1") : "");
                        return;
                }
                this.topiclist = SearchCache.GetSearchCacheList(this.searchid, 16, this.pageid, out this.topiccount, this.searchType);
                if (this.topiccount == 0)
                {
                    base.AddErrLine("不存在的searchid");
                    return;
                }
                this.CalculateCurrentPage();
                this.pagenumbers = ((this.pagecount > 1) ? Utils.GetPageNumbers(this.pageid, this.pagecount, "search.aspx?type=" + this.type + "&searchid=" + this.searchid + "&keyword=" + this.keyword + "&poster=" + this.poster, 8) : "");
                return;
            }
            else
            {
                if (!UserAuthority.Search(this.userid, this.lastsearchtime, this.useradminid, this.usergroupinfo, ref this.msg))
                {
                    base.AddErrLine(this.msg);
                    return;
                }
                if (this.searchType == SearchType.Error)
                {
                    base.AddErrLine("非法的参数信息");
                    return;
                }
                this.searchpost = true;
                string searchforumid = DNTRequest.GetString("searchforumid").Trim();
                int posterid = this.CheckSearchInfo(searchforumid);
                if (base.IsErr()) return;

                this.searchid = SearchCache.Search(this.userid, this.usergroupid, this.keyword, posterid, this.searchType, searchforumid, DNTRequest.GetInt("searchtime", 0), DNTRequest.GetInt("searchtimetype", 0), DNTRequest.GetInt("resultorder", 0), DNTRequest.GetInt("resultordertype", 0));
                if (this.searchid > 0)
                {
                    Response.Redirect(this.forumpath + "search.aspx?type=" + this.type + "&searchid=" + this.searchid + "&keyword=" + this.keyword + "&poster=" + this.poster, false);
                    return;
                }
                base.AddErrLine("抱歉, 没有搜索到符合要求的记录");
                return;
            }
        }

        private void CalculateCurrentPage()
        {
            this.pagecount = ((this.topiccount % 16 == 0) ? (this.topiccount / 16) : (this.topiccount / 16 + 1));
            this.pagecount = ((this.pagecount == 0) ? 1 : this.pagecount);
            this.pageid = ((this.pageid < 1) ? 1 : this.pageid);
            this.pageid = ((this.pageid > this.pagecount) ? this.pagecount : this.pageid);
        }

        private int CalculateCurrentPage(int listcount, int pageid, out int pagecount)
        {
            pagecount = ((listcount % 16 == 0) ? (listcount / 16) : (listcount / 16 + 1));
            pagecount = ((pagecount == 0) ? 1 : pagecount);
            pageid = ((pageid < 1) ? 1 : pageid);
            pageid = ((pageid > pagecount) ? pagecount : pageid);
            return pageid;
        }

        private void GetSearchType()
        {
            string key;
            switch (key = this.type)
            {
                case "":
                case "topic":
                    this.searchType = SearchType.TopicTitle;
                    return;
                case "author":
                    this.searchType = SearchType.ByPoster;
                    return;
                case "post":
                    this.searchType = SearchType.PostContent;
                    return;
                case "digest":
                    this.searchType = SearchType.DigestTopic;
                    return;
            }
            this.searchType = SearchType.Error;
        }

        public string LightKeyWord(string str, string keyword)
        {
            if (string.IsNullOrEmpty(keyword)) return str;

            return str.Replace(keyword, "<font color=\"#ff0000\">" + keyword + "</font>");
        }

        private int CheckSearchInfo(string searchforumid)
        {
            int num = (DNTRequest.GetString("posterid").ToLower().Trim() == "current") ? this.userid : DNTRequest.GetInt("posterid", -1);
            if (Utils.StrIsNullOrEmpty(this.keyword) && Utils.StrIsNullOrEmpty(this.poster) && Utils.StrIsNullOrEmpty(DNTRequest.GetString("posterid")))
            {
                base.AddErrLine("关键字和用户名不能同时为空");
                return num;
            }
            if (num > 0 && BBX.Entity.User.FindByID(num) == null)
            {
                base.AddErrLine("指定的用户ID不存在");
                return num;
            }
            if (!Utils.StrIsNullOrEmpty(this.poster))
            {
                num = Users.GetUserId(this.poster);
                if (num == -1)
                {
                    base.AddErrLine("搜索用户名不存在");
                    return num;
                }
            }
            if (!Utils.StrIsNullOrEmpty(searchforumid))
            {
                string[] array = Utils.SplitString(searchforumid, ",");
                for (int i = 0; i < array.Length; i++)
                {
                    string expression = array[i];
                    if (!Utils.IsNumeric(expression))
                    {
                        base.AddErrLine("非法的搜索版块ID");
                        return num;
                    }
                }
            }
            return num;
        }
    }
}