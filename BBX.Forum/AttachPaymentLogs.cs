using Discuz.Entity;
using Newtonsoft.Json;

namespace Discuz.Forum
{
    public class AttachPaymentLogs
    {
        public static int CreateAttachPaymentLog(AttachPaymentlogInfo attachPaymentLogInfo)
        {
            return Discuz.Data.AttachPaymentLogs.CreateAttachPaymentLog(attachPaymentLogInfo);
        }

        public static string GetAttachPaymentLogJsonByAid(int aid)
        {
            if (aid <= 0)
            {
                return "";
            }
            return JavaScriptConvert.SerializeObject(Discuz.Data.AttachPaymentLogs.GetAttachPaymentLogList(aid).ToArray());
        }

        public static bool HasBoughtAttach(int userid, int radminid, AttachmentInfo attachmentinfo)
        {
            return attachmentinfo.Attachprice > 0 && attachmentinfo.Uid != userid && radminid != 1 && !Discuz.Data.AttachPaymentLogs.HasBoughtAttach(userid, attachmentinfo.Aid);
        }
    }
}