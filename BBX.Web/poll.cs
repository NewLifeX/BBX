using System;
using BBX.Common;
using BBX.Entity;
using BBX.Forum;
using NewLife.Web;

namespace BBX.Web
{
    public class poll : PageBase
    {
        public Topic topic;
        public int forumid;
        public string forumnav = "";
        public int topicid = DNTRequest.GetInt("topicid", -1);
        public string topictitle = "";

        protected override void ShowPage()
        {
            if (this.topicid == -1)
            {
                base.AddErrLine("无效的主题ID");
                return;
            }
            this.topic = Topic.FindByID(this.topicid);
            if (this.topic == null)
            {
                base.AddErrLine("不存在的主题ID");
                return;
            }
            this.topictitle = (Utils.StrIsNullOrEmpty(this.topic.Title) ? "" : this.topic.Title);
            this.forumid = this.topic.Fid;
            var forumInfo = Forums.GetForumInfo(this.forumid);
            this.pagetitle = Utils.RemoveHtml(forumInfo.Name);
            this.forumnav = ForumUtils.UpdatePathListExtname(forumInfo.Pathlist.Trim(), this.config.Extname);
            if (this.topic.Special != 1)
            {
                base.AddErrLine("不存在的投票ID");
                return;
            }
            if (!this.usergroupinfo.AllowVote)
            {
                base.AddErrLine("您当前的身份 \"" + this.usergroupinfo.GroupTitle + "\" 没有投票的权限");
                return;
            }

            if (Convert.ToDateTime(Poll.GetPollEnddatetime(this.topic.ID)).Date < DateTime.Today)
            {
                base.AddErrLine("投票已经过期");
                return;
            }

            if (this.userid != -1 && !Poll.AllowVote(this.topicid, this.username))
            {
                base.AddErrLine("你已经投过票");
                return;
            }
            if (Utils.InArray(this.topic.ID.ToString(), ForumUtils.GetCookie("polled")))
            {
                base.AddErrLine("你已经投过票");
                return;
            }
            if (Utils.StrIsNullOrEmpty(DNTRequest.GetString("pollitemid")))
            {
                base.AddErrLine("您未选择任何投票项！");
                return;
            }
            if (DNTRequest.GetString("pollitemid").Split(',').Length > Poll.FindByTid(topicid).MaxChoices)
            {
                base.AddErrLine("您的投票项多于最大投票数");
                return;
            }
            if (Poll.UpdatePoll(this.topicid, DNTRequest.GetString("pollitemid"), (this.userid == -1) ? string.Format("{0} [{1}]", this.usergroupinfo.GroupTitle, WebHelper.UserHost) : this.username) < 0)
            {
                base.AddErrLine("提交投票信息中包括非法内容");
                return;
            }
            if (this.userid == -1)
            {
                ForumUtils.WriteCookie("polled", string.Format("{0},{1}", (this.userid != -1) ? "" : ForumUtils.GetCookie("polled"), this.topic.ID));
            }
            base.SetUrl(base.ShowTopicAspxRewrite(this.topicid, 0));
            base.SetMetaRefresh();
            base.SetShowBackLink(false);
            base.MsgForward("poll_succeed");
            base.AddMsgLine("投票成功, 返回主题");
            CreditsFacade.Vote(this.userid);
            ForumUtils.DeleteTopicCacheFile(this.topicid);
        }
    }
}