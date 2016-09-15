using BBX.Entity;

namespace BBX.Plugin
{
    public interface IPost
    {
        void CreateTopic(TopicInfo topic, PostInfo post, AttachmentInfo[] attachs);

        void CreatePost(PostInfo post);

        void Edit(PostInfo post);

        void Ban(PostInfo post);

        void UnBan(PostInfo post);

        void Delete(PostInfo post);

        string CreateAttachment(AttachmentInfo[] attachs, int usergroupid, int userid, string username);
    }
}