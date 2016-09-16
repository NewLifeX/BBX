/*
 * XCoder v5.1.5002.17097
 * 作者：nnhy/X
 * 时间：2013-09-11 09:31:01
 * 版权：版权所有 (C) 新生命开发团队 2002~2013
*/
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using BBX.Config;
using NewLife.Threading;
using XCode;

namespace BBX.Entity
{
    /// <summary>通知类型</summary>
    public enum NoticeType
    {
        All = -1,
        PostReplyNotice = 1,
        AlbumCommentNotice,
        SpaceCommentNotice,
        GoodsTradeNotice,
        GoodsLeaveWordNotice,
        BanVisitNotice,
        BanPostNotice,
        AttentionNotice,
        TopicAdmin,
        ApplicationNotice,
        ApplicationCustomNotice
    }

    /// <summary>通知</summary>
    public partial class Notice : Entity<Notice>
    {
        #region 对象操作﻿
        static Notice()
        {
            new TimerX(DeleteNotice, null, 5000, 3600 * 1000);
        }

        /// <summary>验证数据，通过抛出异常的方式提示验证失败。</summary>
        /// <param name="isNew"></param>
        public override void Valid(Boolean isNew)
        {
            // 这里验证参数范围，建议抛出参数异常，指定参数名，前端用户界面可以捕获参数异常并聚焦到对应的参数输入框
            //if (String.IsNullOrEmpty(Name)) throw new ArgumentNullException(_.Name, _.Name.DisplayName + "无效！");
            //if (!isNew && ID < 1) throw new ArgumentOutOfRangeException(_.ID, _.ID.DisplayName + "必须大于0！");

            // 建议先调用基类方法，基类方法会对唯一索引的数据进行验证
            base.Valid(isNew);

            if (isNew)
            {
                if (!Dirtys[__.New]) New = 1;
                if (!Dirtys[__.PostDateTime]) PostDateTime = DateTime.Now;
            }
        }
        #endregion

        #region 扩展属性﻿
        #endregion

        #region 扩展查询﻿
        /// <summary>根据编号查找</summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static Notice FindByID(Int32 id)
        {
            if (Meta.Count >= 1000)
                return Find(_.ID, id);
            else // 实体缓存
                return Meta.Cache.Entities.Find(__.ID, id);
            // 单对象缓存
            //return Meta.SingleCache[id];
        }

        /// <summary>根据U编号查找</summary>
        /// <param name="uid">U编号</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static EntityList<Notice> FindAllByUid(Int32 uid)
        {
            if (Meta.Count >= 1000)
                return FindAll(_.Uid, uid);
            else // 实体缓存
                return Meta.Cache.Entities.FindAll(__.Uid, uid);
        }

        public static EntityList<Notice> FindAllByUidAndType(Int32 uid, NoticeType type, Int32 start = 0, Int32 max = 0)
        {
            if (Meta.Count >= 1000)
                return FindAll(_.Uid == uid & _.Type == type, _.ID.Desc(), null, start, max);
            else // 实体缓存
            {
                var list = Meta.Cache.Entities.FindAll(__.Uid, uid);
                if (type != NoticeType.All) list = list.FindAll(__.Type, type);
                return list.Sort(__.ID, true).Page(start, max);
            }
        }

        public static Int32 FindCountByUidAndNew(Int32 uid, Int32 nw)
        {
            return FindCount(_.Uid == uid & _.New == nw, null, null, 0, 0);
        }

        public static Int32 GetNewNoticeCountByUid(Int32 uid)
        {
            return FindCount(_.Uid == uid & _.New == 1, null, null, 0, 0);
        }

        public static Int32 GetNoticeCountByUid(Int32 uid, NoticeType type)
        {
            if (type != NoticeType.All)
                return FindCount(_.Uid == uid & _.Type == type, null, null, 0, 0);
            else
                return FindCount(_.Uid == uid, null, null, 0, 0);
        }
        #endregion

        #region 高级查询
        #endregion

        #region 扩展操作
        public static Notice Create(Int32 uid, NoticeType type, String message)
        {
            var notice = new Notice();
            notice.Uid = uid;
            notice.Type = (Int32)type;
            notice.Note = message;
            notice.Insert();

            return notice;
        }
        #endregion

        #region 业务
        public static Notice[] GetNewNotices(Int32 userid)
        {
            if (userid <= 0) return null;

            return FindAll(_.Uid == userid & _.New == 1, _.PostDateTime.Desc(), null, 0, 5).ToArray();
        }

        public static void SendPostReplyNotice(Post postinfo, Topic topicinfo, int replyuserid)
        {
            var notice = new Notice();
            notice.Note = String.Format("<a href=\"userinfo.aspx?userid={0}\">{1}</a> 给您回帖, <a href =\"showtopic.aspx?topicid={2}&postid={3}#{3}\">{4}</a>.", postinfo.PosterID, postinfo.Poster, topicinfo.ID, postinfo.ID, topicinfo.Title);
            notice.Type = (Int32)NoticeType.PostReplyNotice;
            notice.New = 1;
            notice.PosterID = postinfo.PosterID;
            notice.Poster = postinfo.Poster;
            notice.PostDateTime = DateTime.Now;
            notice.FromID = topicinfo.ID;
            notice.Uid = replyuserid;
            if (postinfo.PosterID != replyuserid && replyuserid > 0)
            {
                CreateNoticeInfo(notice);
            }
            if (postinfo.PosterID != topicinfo.PosterID && topicinfo.PosterID != replyuserid && topicinfo.PosterID > 0)
            {
                notice.Uid = topicinfo.PosterID;
                CreateNoticeInfo(notice);
            }
        }

        public static int CreateNoticeInfo(Notice inf)
        {
            if (inf.PosterID == inf.Uid) return 0;

            int num = inf.Insert();
            if (num > 0)
            {
                var online = Online.FindByUserID(inf.Uid);
                if (online != null)
                {
                    online.UpdateNewNotices(0);
                }
            }
            return num;
        }

        public static void DeleteNotice(Object state)
        {
            DeleteNotice(NoticeType.All, GeneralConfigInfo.Current.Notificationreserveddays);
        }

        public static void DeleteNotice(NoticeType noticeType, int days)
        {
            if (noticeType == NoticeType.All)
            {
                var list = FindAll(_.PostDateTime < DateTime.Now.AddDays(-1), null, null, 0, 0);
                list.Delete();
            }
            else
            {
                var list = FindAll(_.PostDateTime < DateTime.Now.AddDays(-days) & _.Type == noticeType, null, null, 0, 0);
                list.Delete();
            }
        }

        public static int ReNewNotice(NoticeType type, int uid)
        {
            //DbParameter[] commandParameters = new DbParameter[]
            //{
            //    DbHelper.MakeInParam("@type", DbType.Double, 4, type),
            //    DbHelper.MakeInParam("@date", DbType.Currency, 4, DateTime.Now),
            //    DbHelper.MakeInParam("@uid", DbType.Double, 4, uid)
            //};
            //string commandText = string.Format("UPDATE [{0}notices] SET [new]=1,[postdatetime]=@date WHERE [type]=@type AND [uid]=@uid", TablePrefix);
            //int num = DbHelper.ExecuteNonQuery(CommandType.Text, commandText, commandParameters);
            //if (num > 1)
            //{
            //    commandText = string.Format("DELETE FROM [{0}notices] WHERE [type]=@type AND [uid]=@uid", TablePrefix);
            //    DbHelper.ExecuteNonQuery(CommandType.Text, commandText, commandParameters);
            //    return 0;
            //}
            //return num;

            var list = FindAllByUidAndType(uid, type);
            if (list.Count <= 0) return 0;

            // 实在没搞懂下面什么逻辑
            //if (list.Count == 1)
            //{
            //    list[0].New = 1;
            //    list[0].PostDateTime = DateTime.Now;
            //    list[0].Update();
            //}
            //else
            //    list.Delete();

            return list.Count;
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

        public static void UpdateNoticeNewByUid(Int32 uid, Int32 newtype)
        {
            var list = FindAllByUid(uid);
            list.SetItem(_.New, newtype);
            list.Update();
        }
        #endregion
    }
}