using System.Data;

namespace BBX.Forum
{
    public class CreditsLogs
    {
        public static int AddCreditsLog(int uid, int fromto, int sendcredits, int receivecredits, float send, float receive, string paydate, int operation)
        {
            if (uid <= 0)
            {
                return 0;
            }
            return BBX.Data.CreditsLogs.AddCreditsLog(uid, fromto, sendcredits, receivecredits, send, receive, paydate, operation);
        }

        public static DataTable GetCreditsLogList(int pagesize, int currentpage, int uid)
        {
            if (uid <= 0 || currentpage <= 0)
            {
                return new DataTable();
            }
            return BBX.Data.CreditsLogs.GetCreditsLogList(pagesize, currentpage, uid);
        }

        public static int GetCreditsLogRecordCount(int uid)
        {
            if (uid <= 0)
            {
                return 0;
            }
            return BBX.Data.CreditsLogs.GetCreditsLogRecordCount(uid);
        }
    }
}