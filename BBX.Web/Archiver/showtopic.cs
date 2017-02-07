using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using BBX.Common;
using BBX.Config;
using BBX.Entity;
using BBX.Forum;
using BBX.Web.UI;

namespace BBX.Web.Archiver
{
    public class showtopic : ArchiverPage
    {
        public List<Attachment> attachmentlist = new List<Attachment>();
        public List<Post> postlist = new List<Post>();

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            int topicid = DNTRequest.GetInt("topicid", -1);
            if (topicid == -1)
            {
                base.ShowMsg("无效的主题ID");
                return;
            }
            var topic = Topic.FindByID(topicid);
            if (topic == null || topic.Closed > 1)
            {
                base.ShowMsg("不存在的主题ID");
                return;
            }
            if (topic.DisplayOrder == -1)
            {
                base.ShowMsg("此主题已被删除！");
                return;
            }
            if (topic.ReadPerm > this.usergroupinfo.Readaccess && topic.PosterID != this.userid)
            {
                base.ShowMsg(string.Format("本主题阅读权限为: {0}, 您当前的身份 \"{1}\" 阅读权限不够", topic.ReadPerm, this.usergroupinfo.GroupTitle));
                return;
            }
            var forum = Forums.GetForumInfo(topic.Fid);
            if (forum.ViewPerm.IsNullOrEmpty())
            {
                if (!this.usergroupinfo.AllowVisit)
                {
                    base.ShowMsg("您当前的身份 \"" + this.usergroupinfo.GroupTitle + "\" 没有浏览该版块的权限");
                    return;
                }
            }
            else
            {
                if (!forum.AllowView(this.usergroupinfo.ID))
                {
                    base.ShowMsg("您没有浏览该版块的权限");
                    return;
                }
            }
            if (!String.IsNullOrEmpty(forum.Password))
            {
                base.ShowMsg("简洁版本无法浏览设置了密码的版块");
                return;
            }
            int num = Moderators.IsModer(this.useradminid, this.userid, forum.ID) ? 1 : 0;
            int num2 = 0;
            if (topic.Price > 0 && this.userid != topic.PosterID && num != 1)
            {
                num2 = topic.Price;
                var charge = Scoresets.GetMaxChargeSpan();
                if (PaymentLog.IsBuyer(topicid, this.userid) || charge != 0 && topic.PostDateTime.AddHours(charge) < DateTime.Now)
                {
                    num2 = -1;
                }
            }
            if (num2 > 0)
            {
                base.ShowMsg(string.Format("此帖需转到完整版处购买后才可查看.<a href=\"{0}buytopic.aspx?topicid={1}\">点击购买</a>", BaseConfigs.GetForumPath, topic.ID));
                return;
            }
            int num3 = topic.Replies + 1;
            int num4 = 1;
            int num5 = 30;
            int num6 = (num3 % num5 == 0) ? (num3 / num5) : (num3 / num5 + 1);
            if (num6 == 0)
            {
                num6 = 1;
            }
            if (DNTRequest.GetString("page").ToLower().Equals("end"))
            {
                num4 = num6;
            }
            else
            {
                num4 = DNTRequest.GetInt("page", 1);
            }
            if (num4 < 1)
            {
                num4 = 1;
            }
            if (num4 > num6)
            {
                num4 = num6;
            }
            int hide = 1;
            if (topic.Hide == 1 && (Post.IsReplier(topicid, this.userid) || num == 1))
            {
                hide = -1;
            }
            var pi = new PostpramsInfo();
            pi.Fid = forum.ID;
            pi.Tid = topicid;
            pi.Jammer = forum.Jammer;
            pi.Pagesize = num5;
            pi.Pageindex = num4;
            pi.Getattachperm = forum.GetattachPerm;
            pi.Usergroupid = this.usergroupinfo.ID;
            pi.Attachimgpost = this.config.Attachimgpost;
            pi.Showattachmentpath = this.config.Showattachmentpath;
            pi.Hide = hide;
            pi.Price = topic.Price;
            pi.Usergroupreadaccess = this.usergroupinfo.Readaccess;
            pi.CurrentUserid = this.userid;
            pi.Showimages = forum.AllowImgCode ? 1 : 0;
            pi.Smileyoff = 1;
            pi.Smiliesmax = 0;
            pi.Smiliesinfo = null;
            pi.Customeditorbuttoninfo = null;
            pi.Bbcodemode = 0;
            pi.BBCode = false;
            pi.CurrentUserGroup = this.usergroupinfo;
            pi.Onlinetimeout = this.config.Onlinetimeout;
            User userInfo = Users.GetUserInfo(this.userid);
            pi.Usercredits = ((userInfo == null) ? 0 : userInfo.Credits);
            this.postlist = Posts.GetPostList(pi, out this.attachmentlist, num == 1);
            if (this.postlist.Count <= 0)
            {
                base.ShowMsg("读取信息失败");
                return;
            }
            base.ShowTitle(topic.Title + " - ");
            base.ShowBody();
            Response.Write("<h1>" + this.config.Forumtitle + "</h1>");
            Response.Write("<div class=\"forumnav\">");
            Response.Write("<a href=\"index.aspx\">首页</a> &raquo; ");
            if (this.config.Aspxrewrite == 1)
            {
                Response.Write(string.Format("{0} &raquo; <a href=\"showtopic-{1}{2}\">{3}</a>", new object[]
                {
                    ForumUtils.UpdatePathListExtname(forum.Pathlist.Trim(), this.config.Extname).Replace("</a><", "</a> &raquo; <"),
                    topicid.ToString(),
                    this.config.Extname,
                    topic.Title
                }));
            }
            else
            {
                Response.Write(string.Format("{0} &raquo; <a href=\"showtopic.aspx?topicid={1}\">{2}</a>", ForumUtils.UpdatePathListExtname(forum.Pathlist.Trim(), "aspx").Replace("</a><", "</a> &raquo; <"), topicid.ToString(), topic.Title));
            }
            Response.Write("</div>\r\n");
            Regex regex = new Regex("<img alt=.*? imageid=\"(.*?)\".*?newsrc=\"(.*?)\".*?/>", RegexOptions.IgnoreCase);
            Regex regex2 = new Regex("<img imageid=\"(.*?)\" src=\"(.*?)\".*?/>", RegexOptions.IgnoreCase);
            foreach (var item in this.postlist)
            {
                Response.Write("<div class=\"postitem\">\r\n");
                Response.Write("\t<div class=\"postitemtitle\">\r\n");
                Response.Write(Utils.HtmlEncode(item.Poster) + " - " + item.PostDateTime.ToFullString());
                Response.Write("</div><div class=\"postitemcontent\">");
                var msg = item.Message;
                if (this.config.Showimgattachmode == 1)
                {
                    Match match = regex.Match(msg);
                    while (match.Success)
                    {
                        msg = msg.Replace(match.Value, string.Format("<a href=\"{0}\" target=\"_blank\">点击显示图片:{1}</a>", match.Groups[2].Value, match.Groups[1].Value));
                        match = match.NextMatch();
                    }
                }
                else
                {
                    Match match = regex2.Match(msg);
                    while (match.Success)
                    {
                        msg = msg.Replace(match.Value, string.Format("<img alt=\"{0}\" src=\"{1}\" />", match.Groups[1].Value, match.Groups[2].Value));
                        match = match.NextMatch();
                    }
                }
                Response.Write(msg);
                foreach (var att in this.attachmentlist)
                {
                    if (att.Pid == item.ID)
                    {
                        Response.Write(string.Format("<br /><br />附件: <a href=\"../attachment.aspx?attachmentid={0}\">{1}</a>", att.ID, Utils.HtmlEncode(att.Name)));
                    }
                }
                Response.Write("\t</div>\r\n</div>\r\n");
            }
            Response.Write("<div class=\"pagenumbers\">");
            if (this.config.Aspxrewrite == 1)
            {
                Response.Write(Utils.GetStaticPageNumbers(num4, num6, "showtopic-" + topicid, this.config.Extname, 8));
            }
            else
            {
                Response.Write(Utils.GetPageNumbers(num4, num6, "showtopic.aspx?topicid=" + topicid, 8, "page"));
            }
            Response.Write("</div>\r\n");
            //Topic.UpdateViewCount(topicid, 1);
            topic.Views++;
            topic.SaveAsync(10000);
            if (this.config.Aspxrewrite == 1)
            {
                Response.Write(string.Format("<div class=\"fullversion\">查看完整版本: <a href=\"../showtopic-{0}{1}\">{2}</a></div>\r\n", topicid, this.config.Extname, topic.Title));
            }
            else
            {
                Response.Write(string.Format("<div class=\"fullversion\">查看完整版本: <a href=\"../showtopic.aspx?topicid={0}\">{1}</a></div>\r\n", topicid, topic.Title));
            }
            base.ShowFooter();
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
    }
}