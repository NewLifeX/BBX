using System;
using BBX.Common;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web
{
    public class favorites : PageBase
    {
        public Topic topic;
        public int forumid;
        public string forumname = "";
        public int topicid = DNTRequest.GetInt("topicid", -1);
        public string topictitle = "";
        public string forumnav = "";
        public int blogid = DNTRequest.GetInt("postid", -1);
        public IXForum forum;
        private string referer = ForumUtils.GetCookie("referer");

        protected override void ShowPage()
        {
            if (this.userid == -1)
            {
                base.AddErrLine("你尚未登录");
                return;
            }
            if (this.topicid != -1)
            {
                var topicInfo = Topic.FindByID(this.topicid);
                if (topicInfo == null)
                {
                    base.AddErrLine("不存在的主题ID");
                    return;
                }
                this.topictitle = topicInfo.Title;
                this.forumid = topicInfo.Fid;
                this.forum = Forums.GetForumInfo(this.forumid);
                this.forumname = this.forum.Name;
                this.pagetitle = Utils.RemoveHtml(this.forum.Name);
                this.forumnav = this.forum.Pathlist;
                this.CheckFavorite(FavoriteType.ForumTopic, this.topicid, "主题");
            }
        }

        private void CheckFavorite(FavoriteType favoriteType, int id, string favoriteName)
        {
            if (this.config.Maxfavorites <= Favorite.SearchCount(this.userid, null, favoriteType))
            {
                base.AddErrLine("您收藏的" + favoriteName + "数目已经达到系统设置的数目上限");
                return;
            }
            if (Favorite.SearchCount(this.userid, new Int32[] { id }, favoriteType) > 0)
            {
                base.AddErrLine("您过去已经收藏过该" + favoriteName);
                return;
            }
            if (Favorite.Create(this.userid, id, favoriteType) != null)
            {
                base.AddMsgLine("指定" + favoriteName + "已成功添加到收藏夹中");
                base.SetUrl(this.referer);
                base.SetMetaRefresh();
                base.SetShowBackLink(false);
            }
        }
    }
}