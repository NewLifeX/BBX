using Discuz.Common;
using Discuz.Data;
using Discuz.Entity;
using System;
namespace Discuz.Forum
{
    public class Notices
    {
        public static int CreateNoticeInfo(NoticeInfo noticeinfo)
        {
            if (noticeinfo.Posterid == noticeinfo.Uid)
            {
                return 0;
            }
            int num = Discuz.Data.Notices.CreateNoticeInfo(noticeinfo);
            if (num > 0)
            {
                int olidByUid = OnlineUsers.GetOlidByUid(noticeinfo.Uid);
                if (olidByUid > 0)
                {
                    OnlineUsers.UpdateNewNotices(olidByUid);
                }
            }
            return num;
        }
        public static void DeleteNotice()
        {
            Discuz.Data.Notices.DeleteNotice();
        }
        public static int GetNoticeCountByUid(int uid, NoticeType noticetype)
        {
            if (uid <= 0)
            {
                return 0;
            }
            return Discuz.Data.Notices.GetNoticeCountByUid(uid, noticetype);
        }
        //public static NoticeinfoCollection GetNoticeinfoCollectionByUid(int uid, NoticeType noticetype, int pageid, int pagesize)
        //{
        //    if (uid <= 0 || pageid <= 0)
        //    {
        //        return null;
        //    }
        //    return Discuz.Data.Notices.GetNoticeinfoCollectionByUid(uid, noticetype, pageid, pagesize);
        //}
        public static void SendPostReplyNotice(PostInfo postinfo, TopicInfo topicinfo, int replyuserid)
        {
            NoticeInfo noticeInfo = new NoticeInfo();
            noticeInfo.Note = Utils.HtmlEncode(string.Format("<a href=\"userinfo.aspx?userid={0}\">{1}</a> 给您回帖, <a href =\"showtopic.aspx?topicid={2}&postid={3}#{3}\">{4}</a>.", new object[]
            {
                postinfo.Posterid,
                postinfo.Poster,
                topicinfo.Tid,
                postinfo.Pid,
                topicinfo.Title
            }));
            noticeInfo.Type = NoticeType.PostReplyNotice;
            noticeInfo.New = 1;
            noticeInfo.Posterid = postinfo.Posterid;
            noticeInfo.Poster = postinfo.Poster;
            noticeInfo.Postdatetime = Utils.GetDateTime();
            noticeInfo.Fromid = topicinfo.Tid;
            noticeInfo.Uid = replyuserid;
            if (postinfo.Posterid != replyuserid && replyuserid > 0)
            {
                Notices.CreateNoticeInfo(noticeInfo);
            }
            if (postinfo.Posterid != topicinfo.Posterid && topicinfo.Posterid != replyuserid && topicinfo.Posterid > 0)
            {
                noticeInfo.Uid = topicinfo.Posterid;
                Notices.CreateNoticeInfo(noticeInfo);
            }
        }
        public static NoticeType GetNoticetype(string filter)
        {
            if (filter != null)
            {
                if (filter == "spacecomment")
                {
                    return NoticeType.SpaceCommentNotice;
                }
                if (filter == "albumcomment")
                {
                    return NoticeType.AlbumCommentNotice;
                }
                if (filter == "postreply")
                {
                    return NoticeType.PostReplyNotice;
                }
                if (filter == "topicadmin")
                {
                    return NoticeType.TopicAdmin;
                }
            }
            return NoticeType.All;
        }
        public static int GetNewNoticeCountByUid(int uid)
        {
            if (uid <= 0)
            {
                return 0;
            }
            return Discuz.Data.Notices.GetNewNoticeCountByUid(uid);
        }
        public static void UpdateNoticeNewByUid(int uid, int newtype)
        {
            if (uid > 0)
            {
                Discuz.Data.Notices.UpdateNoticeNewByUid(uid, newtype);
            }
        }
        public static int GetNoticeCount(int userid, int state)
        {
            if (userid <= 0)
            {
                return 0;
            }
            return Discuz.Data.Notices.GetNoticeCount(userid, state);
        }
        public static int GetLatestNoticeID(int userid)
        {
            if (userid <= 0)
            {
                return 0;
            }
            return Discuz.Data.Notices.GetLatestNoticeID(userid);
        }
        public static NoticeInfo[] GetNewNotices(int userid)
        {
            if (userid <= 0)
            {
                return null;
            }
            return Discuz.Data.Notices.GetNewNotices(userid);
        }
    }
}
