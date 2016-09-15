using Discuz.Cache;
using Discuz.Common;

using Discuz.Data;
using Discuz.Entity;
using System;
using System.Collections.Generic;
namespace Discuz.Forum
{
    public class PrivateMessages
    {
        public const string SystemUserName = "系统";
        //public static PrivateMessageInfo GetPrivateMessageInfo(int pmid)
        //{
        //    if (pmid <= 0)
        //    {
        //        return null;
        //    }
        //    return Discuz.Data.PrivateMessages.GetPrivateMessageInfo(pmid);
        //}
        //public static int GetPrivateMessageCount(int userId, int folder)
        //{
        //    if (userId <= 0)
        //    {
        //        return 0;
        //    }
        //    return PrivateMessages.GetPrivateMessageCount(userId, folder, -1);
        //}
        //public static int GetPrivateMessageCount(int userId, int folder, int state)
        //{
        //    if (userId <= 0)
        //    {
        //        return 0;
        //    }
        //    return Discuz.Data.PrivateMessages.GetPrivateMessageCount(userId, folder, state);
        //}
        //public static int GetAnnouncePrivateMessageCount()
        //{
        //    var cacheService = XCache.Current;
        //    int num = Utils.StrToInt(cacheService.RetrieveObject("/Forum/AnnouncePrivateMessageCount"), -1);
        //    if (num < 0)
        //    {
        //        num = Discuz.Data.PrivateMessages.GetAnnouncePrivateMessageCount();
        //        XCache.Add("/Forum/AnnouncePrivateMessageCount", num);
        //    }
        //    return num;
        //}
        //public static int CreatePrivateMessage(PrivateMessageInfo privatemessageinfo, int savetosentbox)
        //{
        //    return Discuz.Data.PrivateMessages.CreatePrivateMessage(privatemessageinfo, savetosentbox);
        //}
        //public static int DeletePrivateMessage(int userId, string[] pmitemid)
        //{
        //    if (!Utils.IsNumericArray(pmitemid))
        //    {
        //        return -1;
        //    }
        //    int num = Discuz.Data.PrivateMessages.DeletePrivateMessages(userId, string.Join(",", pmitemid));
        //    if (num > 0)
        //    {
        //        Discuz.Data.Users.SetUserNewPMCount(userId, Discuz.Data.PrivateMessages.GetNewPMCount(userId));
        //    }
        //    return num;
        //}
        //public static int DeletePrivateMessage(int userId, int pmid)
        //{
        //    if (userId <= 0) return 0;

        //    return ShortMessage.DeletePrivateMessage(userId, pmid.ToString());
        //}
        //public static int SetPrivateMessageState(int pmid, byte state)
        //{
        //    if (pmid <= 0) return 0;

        //    return Discuz.Data.PrivateMessages.SetPrivateMessageState(pmid, state);
        //}
        //public static List<PrivateMessageInfo> GetPrivateMessageCollection(int userId, int folder, int pagesize, int pageindex, int readStatus)
        //{
        //    //if (userId <= 0 || pageindex <= 0 || pagesize <= 0)  return null;
            
        //    //return Discuz.Data.PrivateMessages.GetPrivateMessageCollection(userId, folder, pagesize, pageindex, readStatus);
        //    return ShortMessage.GetPrivateMessageCollection(userId, folder, pagesize, pageindex, readStatus);
        //}
        //public static List<PrivateMessageInfo> GetAnnouncePrivateMessageCollection(int pagesize, int pageindex)
        //{
        //    if (pagesize == -1)
        //    {
        //        return Discuz.Data.PrivateMessages.GetAnnouncePrivateMessageCollection(-1, 0);
        //    }
        //    if (pagesize <= 0 || pageindex <= 0)
        //    {
        //        return null;
        //    }
        //    return Discuz.Data.PrivateMessages.GetAnnouncePrivateMessageCollection(pagesize, pageindex);
        //}
        //public static List<PrivateMessageInfo> GetPrivateMessageListForIndex(int userId, int pagesize, int pageindex, int inttype)
        //{
        //    List<PrivateMessageInfo> privateMessageCollection = PrivateMessages.GetPrivateMessageCollection(userId, 0, pagesize, pageindex, inttype);
        //    if (privateMessageCollection.Count > 0)
        //    {
        //        for (int i = 0; i < privateMessageCollection.Count; i++)
        //        {
        //            privateMessageCollection[i].Message = Utils.GetSubString(privateMessageCollection[i].Message, 20, "...");
        //        }
        //    }
        //    return privateMessageCollection;
        //}
        //public static int GetLatestPMID(int userId)
        //{
        //    List<PrivateMessageInfo> privateMessageCollection = Discuz.Data.PrivateMessages.GetPrivateMessageCollection(userId, 0, 1, 1, 0);
        //    int result = 0;
        //    using (List<PrivateMessageInfo>.Enumerator enumerator = privateMessageCollection.GetEnumerator())
        //    {
        //        if (enumerator.MoveNext())
        //        {
        //            PrivateMessageInfo current = enumerator.Current;
        //            result = current.Pmid;
        //        }
        //    }
        //    return result;
        //}
        //public static string DeletePrivateMessages(bool isNew, string postDateTime, string msgFromList, bool lowerUpper, string subject, string message, bool isUpdateUserNewPm)
        //{
        //    return Discuz.Data.PrivateMessages.DeletePrivateMessages(isNew, postDateTime, msgFromList, lowerUpper, subject, message, isUpdateUserNewPm);
        //}
    }
}
