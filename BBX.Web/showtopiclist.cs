using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using BBX.Common;
using BBX.Config;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web
{
    public class showtopiclist : PageBase
    {
        public DataTable onlineuserlist;
        public string onlineiconlist;
        public List<Topic> topiclist;
        public List<IXForum> subforumlist;
        //public List<PrivateMessageInfo> pmlist;
        public IXForum forum;
        public AdminGroup admingroupinfo = new AdminGroup();
        public Int32 forumtotalonline;
        public int forumtotalonlineuser;
        public int forumtotalonlineguest;
        public int forumtotalonlineinvisibleuser;
        public int forumid = DNTRequest.GetInt("forumid", -1);
        public string forumnav = "";
        public int showforumlogin;
        public int pageid = DNTRequest.GetInt("page", 1);
        public int topiccount;
        public int pagecount;
        public string pagenumbers = "";
        //public int toptopiccount;
        public int tpp = ForumUtils.GetCookie("tpp").ToInt(GeneralConfigInfo.Current.Tpp);
        public int order = DNTRequest.GetInt("order", 2);
        public int direct = DNTRequest.GetInt("direct", 1);
        public string type = DNTRequest.GetString("type", true);
        public int newtopic = DNTRequest.GetInt("newtopic", 600);
        public string forums = "";
        public string forumcheckboxlist = "";
        //public string goodscategoryfid = (GeneralConfigInfo.Current.Enablemall > 0) ? MallPluginProvider.GetInstance().GetGoodsCategoryWithFid() : "{}";
        private string msg = "";
        public string navhomemenu = "";
        private int maxseachnumber = 10000;
        //private string condition = "";

        protected override void ShowPage()
        {
            //type = DNTRequest.GetString("type", true);
            if (userid > 0 && useradminid > 0) admingroupinfo = AdminGroup.FindByID(usergroupid);
            if (config.Rssstatus == 1) base.AddLinkRss("tools/rss.aspx", "最新主题");

            if (forumid == -1)
            {
                var vs = Request["fidlist"];
                if (vs.IsNullOrWhiteSpace()) vs = (Request["forums"] + "").ToLower();
                if (vs.IsNullOrWhiteSpace() || vs.EqualIgnoreCase("all")) vs = GetForums();
                vs = GetAllowviewForums(vs);
                forums = vs;
            }
            navhomemenu = Caches.GetForumListMenuDivCache(usergroupid, userid, config.Extname);
            if (forumid > 0)
            {
                forum = Forums.GetForumInfo(forumid);
                if (forum == null)
                {
                    base.AddErrLine("不存在的版块ID");
                    return;
                }
                pagetitle = Utils.RemoveHtml(forum.Name);
                forumnav = ForumUtils.UpdatePathListExtname(forum.Pathlist.Trim(), config.Extname);
                showforumlogin = ShowForumLogin();
                if (!UserAuthority.VisitAuthority(forum, usergroupinfo, userid, ref msg))
                {
                    base.AddErrLine(msg);
                    return;
                }
                subforumlist = Forums.GetSubForumCollection(forumid, forum.ColCount, config.Hideprivate, usergroupid, config.Moddisplay);
            }
            var condition = GetCondition();
            if (base.IsErr()) return;

            pagetitle = ((type == "digest") ? "查看精华" : "查看新帖");
            SetPageIdAndNumber(condition);
            topiclist = Topics.GetTopicListByCondition(tpp, pageid, 0, 10, config.Hottopic, forum == null ? 0 : forum.AutoClose, forum == null ? 0 : forum.Topictypeprefix, condition, GetOrder(), direct);
            Online.UpdateAction(olid, UserAction.ShowForum, forumid, config.Onlinetimeout);
            ForumUtils.UpdateVisitedForumsOptions(forumid);
        }

        private string GetOrder()
        {
            if (Utils.StrIsNullOrEmpty(DNTRequest.GetString("search")))
            {
                return "";
            }
            if (order != 1)
            {
                return "tid";
            }
            return "lastpostid";
        }

        private void SetPageIdAndNumber(String condition)
        {
            //topiccount = Topics.GetTopicCount(condition);
            topiccount = Topic.GetTopicCount(0, true, condition);
            topiccount = ((maxseachnumber > topiccount) ? topiccount : maxseachnumber);
            if (tpp <= 0)
            {
                tpp = config.Tpp;
            }
            if (userid != -1)
            {
                IUser user = BBX.Entity.User.FindByID(userid);
                if (user != null)
                {
                    if (user.Tpp > 0) tpp = user.Tpp;
                    if (!user.Newpm) newpmcount = 0;
                }
            }
            pagecount = ((topiccount % tpp == 0) ? (topiccount / tpp) : (topiccount / tpp + 1));
            if (pagecount == 0)
            {
                pagecount = 1;
            }
            pageid = ((pageid < 1) ? 1 : pageid);
            pageid = ((pageid > pagecount) ? pagecount : pageid);
            if (pageid * tpp > topiccount)
            {
                tpp -= pageid * tpp - topiccount;
            }
            var search = Request["search"];
            pagenumbers = search.IsNullOrWhiteSpace() ?
                Utils.GetPageNumbers(pageid, pagecount, string.Format("showtopiclist.aspx?type={0}&newtopic={1}&forumid={2}&forums={3}", type, newtopic, forumid, forums), 8) :
                Utils.GetPageNumbers(pageid, pagecount, string.Format("showtopiclist.aspx?search=1&type={0}&newtopic={1}&order={2}&direct={3}&forumid={4}&forums={5}", new object[]
            {
                type,
                newtopic,
                DNTRequest.GetHtmlEncodeString("order", true),
                DNTRequest.GetHtmlEncodeString("direct", true),
                forumid,
                forums
            }), 8);
        }

        private String GetCondition()
        {
            var condition = Topic.GetTopicCountCondition(type, newtopic, forumid, forums);
            if (forumid > 0) return condition;

            if (!Utils.IsNumericList(forums))
            {
                base.AddErrLine("版块ID不合法或没有该版块访问权限");
            }
            return condition;
        }

        private int ShowForumLogin()
        {
            int result = 1;
            if (String.IsNullOrEmpty(forum.Password))
            {
                result = 0;
            }
            else
            {
                if (Utils.MD5(forum.Password) == ForumUtils.GetCookie("forum" + forumid + "password"))
                {
                    result = 0;
                }
                else
                {
                    if (forum.Password == DNTRequest.GetString("forumpassword"))
                    {
                        ForumUtils.WriteCookie("forum" + forumid + "password", Utils.MD5(forum.Password));
                        result = 0;
                    }
                }
            }
            return result;
        }

        private string GetAllowviewForums(string forums)
        {
            if (!Utils.IsNumericList(forums))
            {
                return "";
            }
            string text = "";
            string[] array = forums.Split(',');
            for (int i = 0; i < array.Length; i++)
            {
                string text2 = array[i];
                int num = text2.ToInt(0);
                var forumInfo = Forums.GetForumInfo(num);
                if (forumInfo != null && forumInfo.Layer != 0 && forumInfo.Visible && forumInfo.AllowView(usergroupid) && (forumInfo.Password.IsNullOrEmpty() || Utils.MD5(forumInfo.Password.Trim()) == ForumUtils.GetCookie("forum" + text2.Trim() + "password")))
                {
                    text += string.Format(",{0}", num);
                }
            }
            return text.Trim(',');
        }

        private string GetForums()
        {
            string text = string.Empty;
            foreach (var current in Forums.GetForumList())
            {
                text += string.Format(",{0}", current.Fid);
            }
            return text.Trim(',');
        }

        public string GetForumCheckBoxListCache()
        {
            var sb = new StringBuilder();
            forums = "," + forums + ",";
            var forumList = Forums.GetForumList(GetAllowviewForums(GetForums()));
            int num = 1;
            foreach (var item in forumList)
            {
                if (forums == ",all," || forums.IndexOf("," + item.Fid + ",") >= 0)
                {
                    sb.AppendFormat("<td><input id=\"fidlist\" onclick=\"javascript:SH_SelectOne(this)\" type=\"checkbox\" value=\"{0}\"\tname=\"fidlist\"  checked/> {1}</td>\r\n", item.Fid, item.Name);
                }
                else
                {
                    sb.AppendFormat("<td><input id=\"fidlist\" onclick=\"javascript:SH_SelectOne(this)\" type=\"checkbox\" value=\"{0}\"\tname=\"fidlist\"  /> {1}</td>\r\n", item.Fid, item.Name);
                }
                if (num > 3)
                {
                    sb.Append("\t\t\t  </tr>\r\n");
                    sb.Append("\t\t\t  <tr>\r\n");
                    num = 0;
                }
                num++;
            }
            return sb.ToString();
        }
    }
}