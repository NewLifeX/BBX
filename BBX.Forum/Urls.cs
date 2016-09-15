using System;
using BBX.Common;
using BBX.Config;

namespace BBX.Forum
{
    public class Urls
    {
        public static GeneralConfigInfo config { get { return GeneralConfigInfo.Current; } }
        public static string ShowForumAspxRewrite(int forumid, int pageid)
        {
            if (config.Aspxrewrite == 1)
            {
                if (pageid > 1)
                {
                    return "showforum-" + forumid + "-" + pageid + config.Extname;
                }
                return "showforum-" + forumid + config.Extname;
            }
            else
            {
                if (pageid > 1)
                {
                    return "showforum.aspx?forumid=" + forumid + "&page=" + pageid;
                }
                return "showforum.aspx?forumid=" + forumid;
            }
        }
        public static string ShowTopicAspxRewrite(int topicid, int pageid)
        {
            return ShowTopicAspxRewrite(topicid, pageid, -1);
        }
        public static string ShowTopicAspxRewrite(int topicid, int pageid, int typeid)
        {
            if (config.Aspxrewrite == 1 && typeid < 0)
            {
                if (pageid > 1)
                {
                    return "showtopic-" + topicid + "-" + pageid + config.Extname;
                }
                return "showtopic-" + topicid + config.Extname;
            }
            else
            {
                if (pageid > 1)
                {
                    return "showtopic.aspx?topicid=" + topicid + "&page=" + pageid + ((typeid > 0) ? ("&typeid=" + typeid) : "");
                }
                return "showtopic.aspx?topicid=" + topicid + ((typeid >= 0) ? ("&typeid=" + typeid) : "");
            }
        }
        public static string ShowDebateAspxRewrite(int topicid)
        {
            return ShowDebateAspxRewrite(topicid, 0);
        }
        public static string ShowDebateAspxRewrite(int topicid, int typeid)
        {
            if (config.Aspxrewrite == 1 && typeid < 0)
            {
                return string.Format("showdebate-{0}{1}", topicid, config.Extname);
            }
            return string.Format("showdebate.aspx?topicid={0}{1}", topicid, (typeid >= 0) ? ("&typeid=" + typeid) : "");
        }
        //public static string ShowBonusAspxRewrite(int topicid, int pageid)
        //{
        //	return ShowBonusAspxRewrite(topicid, pageid, -1);
        //}
        //public static string ShowBonusAspxRewrite(int topicid, int pageid, int typeid)
        //{
        //	if (config.Aspxrewrite == 1 && typeid < 0)
        //	{
        //		if (pageid > 1)
        //		{
        //			return "showbonus-" + topicid + "-" + pageid + config.Extname;
        //		}
        //		return "showbonus-" + topicid + config.Extname;
        //	}
        //	else
        //	{
        //		if (pageid > 1)
        //		{
        //			return "showbonus.aspx?topicid=" + topicid + "&page=" + pageid + ((typeid > 0) ? ("&typeid=" + typeid) : "");
        //		}
        //		return "showbonus.aspx?topicid=" + topicid + ((typeid >= 0) ? ("&typeid=" + typeid) : "");
        //	}
        //}
        public static string UserInfoAspxRewrite(int userid)
        {
            if (config.Aspxrewrite == 1)
            {
                return "userinfo-" + userid + config.Extname;
            }
            return "userinfo.aspx?userid=" + userid;
        }
        public static string RssAspxRewrite(int forumid)
        {
            if (config.Aspxrewrite == 1)
            {
                return "rss-" + forumid + config.Extname;
            }
            return "rss.aspx?forumid=" + forumid;
        }
        public static string ShowGoodsAspxRewrite(int goodsid)
        {
            if (config.Aspxrewrite == 1)
            {
                return "showgoods-" + goodsid + config.Extname;
            }
            return "showgoods.aspx?goodsid=" + goodsid;
        }
        public static string ShowGoodsListAspxRewrite(int categoryid, int pageid)
        {
            if (config.Aspxrewrite == 1)
            {
                if (pageid > 1)
                {
                    return "showgoodslist-" + categoryid + "-" + pageid + config.Extname;
                }
                return "showgoodslist-" + categoryid + config.Extname;
            }
            else
            {
                if (pageid > 1)
                {
                    return "showgoodslist.aspx?categoryid=" + categoryid + "&page=" + pageid;
                }
                return "showgoodslist.aspx?categoryid=" + categoryid;
            }
        }
        public static string ShowForumAspxRewrite(string pathlist, int forumid, int pageid)
        {
            if (String.IsNullOrEmpty(pathlist)) return "";

            if (config.Aspxrewrite == 1)
            {
                if (pageid > 1)
                {
                    pathlist = pathlist.Replace("showforum-" + forumid + config.Extname, "showforum-" + forumid + "-" + pageid + config.Extname);
                }
                else
                {
                    pathlist = pathlist.Replace("showforum-" + forumid + config.Extname, "showforum-" + forumid + config.Extname);
                }
            }
            else
            {
                if (pageid > 1)
                {
                    pathlist = pathlist.Replace(config.Extname + "?forumid=" + forumid + "\"", config.Extname + "?forumid=" + forumid + "&page=" + pageid + "\"");
                }
                else
                {
                    pathlist = pathlist.Replace(config.Extname + "?forumid=" + forumid + "\"", config.Extname + "?forumid=" + forumid + "\"");
                }
            }
            return pathlist;
        }
        public static string ShowForumAspxRewrite(int forumid, int pageid, string rewritename)
        {
            if (!Utils.StrIsNullOrEmpty(rewritename))
            {
                if (config.Iisurlrewrite == 1)
                {
                    return rewritename = rewritename + ((pageid > 1) ? ("/" + pageid) : "") + "/";
                }
                if (config.Aspxrewrite == 1)
                {
                    return rewritename = rewritename + ((pageid > 1) ? ("/" + pageid) : "") + "/list.aspx";
                }
            }
            return ShowForumAspxRewrite(forumid, pageid);
        }
    }
}