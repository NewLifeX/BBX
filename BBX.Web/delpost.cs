using BBX.Common;
using NewLife;
using System;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web
{
    public class delpost : PageBase
    {
        public int postid = DNTRequest.GetInt("postid", -1);
        public Post post;
        public Topic topic;
        public int forumid;
        public string forumname;
        public int topicid = DNTRequest.GetInt("topicid", -1);
        public string topictitle = "";
        public string forumnav = "";
        public IXForum forum;
        public int forumpageid = DNTRequest.GetInt("forumpage", 1);
        private bool allowDelPost;

        protected override void ShowPage()
        {
            if (this.postid == -1)
            {
                base.AddErrLine("无效的帖子ID");
                return;
            }
            this.post = Post.FindByID(this.postid);
            if (this.post == null)
            {
                base.AddErrLine("不存在的帖子ID");
                return;
            }
            this.topic = Topic.FindByID(this.topicid);
            if (this.topic == null)
            {
                base.AddErrLine("不存在的主题ID");
                return;
            }
            if (this.topicid != this.post.Tid)
            {
                base.AddErrLine("主题ID无效");
                return;
            }
            this.topictitle = this.topic.Title;
            this.forumid = this.topic.Fid;
            this.forum = Forums.GetForumInfo(this.forumid);
            this.forumname = this.forum.Name;
            this.pagetitle = string.Format("删除{0}", this.post.Title);
            this.forumnav = base.ShowForumAspxRewrite(this.forum.Pathlist.Trim(), this.forumid, this.forumpageid);
            if (!this.CheckPermission(this.post, DNTRequest.GetInt("opinion", -1)))
            {
                return;
            }
            if (!this.allowDelPost)
            {
                base.AddErrLine("当前不允许删帖");
                return;
            }
            if (this.post.Layer == 0)
            {
                TopicAdmins.DeleteTopics(this.topicid.ToString(), forum.Recyclebin, false);
                XForum.SetRealCurrentTopics(this.forum.Fid);
                Tag.DeleteTopicTags(this.topicid);
            }
            else
            {
                if (this.topic.Special == 4)
                {
                    if (DNTRequest.GetInt("opinion", -1) != 1 && DNTRequest.GetInt("opinion", -1) != 2)
                    {
                        base.AddErrLine("参数错误");
                        return;
                    }
                    Posts.DeletePost(post, false, true);
                    Debate.DeleteDebatePost(this.topicid, DNTRequest.GetInt("opinion", -1), this.postid);
                }
                else
                {
                    Posts.DeletePost(post, false, true);
                }
                ForumUtils.DeleteTopicCacheFile(this.topicid);
                Topics.UpdateTopicReplyCount(this.topic.ID);
                //Forums.UpdateLastPost(this.forum);
                (forum as XForum).ResetLastPost();
            }
            base.SetUrl((this.post.Layer == 0) ? base.ShowForumAspxRewrite(this.post.Fid, 0) : Urls.ShowTopicAspxRewrite(this.post.Tid, 1));
            base.SetMetaRefresh();
            base.SetShowBackLink(false);
            base.AddMsgLine("删除帖子成功, 返回主题");
        }

        private bool CheckPermission(Post post, int opinion)
        {
            if (this.userid != post.PosterID)
            {
                return false;
            }
            if (post.Layer < 1 && this.topic.Replies > 0)
            {
                base.AddErrLine("已经被回复过的主帖不能被删除");
                return false;
            }
            if (this.config.Deletetimelimit == -1)
            {
                base.AddErrLine("抱歉,系统不允许删除帖子");
                return false;
            }
            var time = this.config.Deletetimelimit;
            if (!Moderators.IsModer(this.useradminid, this.userid, this.forumid) && ((time != 0 && post.PostDateTime.AddMinutes(time) < DateTime.Now) || post.PosterID != this.userid))
            {
                base.AddErrLine(string.Format("已经超过了{0}分钟的删除帖子时限,不能删除帖子", this.config.Deletetimelimit));
                return false;
            }
            this.allowDelPost = true;
            return true;
        }
    }
}