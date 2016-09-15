using System.Data;
using BBX.Entity;

namespace BBX.Plugin
{
    public abstract class PluginBase : IFeed, IPost, IUser2, ISearch
    {
        public void Create(UserInfo user)
        {
            this.OnUserCreated(user);
        }

        public void Ban(int userid)
        {
            this.OnUserBanned(userid);
        }

        public void UnBan(int userid)
        {
            this.OnUserUnBanned(userid);
        }

        public void Delete(int userid)
        {
            this.OnUserDeleted(userid);
        }

        public void LogIn(UserInfo user)
        {
            this.OnUserLoggedIn(user);
        }

        public void LogOut(UserInfo user)
        {
            this.OnUserLoggedOut(user);
        }

        public void CreateTopic(TopicInfo topic, PostInfo post, AttachmentInfo[] attachs)
        {
            this.OnTopicCreated(topic, post, attachs);
        }

        public void CreatePost(PostInfo post)
        {
            this.OnPostCreated(post);
        }

        public void Edit(PostInfo post)
        {
            this.OnPostEdited(post);
        }

        public void Ban(PostInfo post)
        {
            this.OnPostBanned(post);
        }

        public void UnBan(PostInfo post)
        {
            this.OnPostUnBanned(post);
        }

        public void Delete(PostInfo post)
        {
            this.OnPostDeleted(post);
        }

        public string CreateAttachment(AttachmentInfo[] attachs, int usergroupid, int userid, string username)
        {
            return this.OnAttachCreated(attachs, usergroupid, userid, username);
        }

        public string GetFeed(int ttl, int uid)
        {
            return this.GetFeedXML(ttl, uid);
        }

        public string GetFeed(int ttl)
        {
            return this.GetFeedXML(ttl);
        }

        public DataTable GetResult(int pagesize, string idstr)
        {
            return this.GetSearchResult(pagesize, idstr);
        }

        protected virtual void OnUserCreated(UserInfo user)
        {
        }

        protected virtual void OnUserBanned(int userid)
        {
        }

        protected virtual void OnUserUnBanned(int userid)
        {
        }

        protected virtual void OnUserDeleted(int userid)
        {
        }

        protected virtual void OnUserLoggedIn(UserInfo user)
        {
        }

        protected virtual void OnUserLoggedOut(UserInfo user)
        {
        }

        protected virtual void OnTopicCreated(TopicInfo topic, PostInfo post, AttachmentInfo[] attachs)
        {
        }

        protected virtual void OnPostCreated(PostInfo post)
        {
        }

        protected virtual void OnPostEdited(PostInfo post)
        {
        }

        protected virtual void OnPostBanned(PostInfo post)
        {
        }

        protected virtual void OnPostUnBanned(PostInfo post)
        {
        }

        protected virtual void OnPostDeleted(PostInfo post)
        {
        }

        protected virtual string GetFeedXML(int ttl, int uid)
        {
            return "";
        }

        protected virtual string GetFeedXML(int ttl)
        {
            return "";
        }

        protected virtual DataTable GetSearchResult(int pagesize, string idstr)
        {
            return new DataTable();
        }

        protected virtual string OnAttachCreated(AttachmentInfo[] attachs, int usergroupid, int userid, string username)
        {
            return "";
        }
    }
}