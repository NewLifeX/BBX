namespace Discuz.Entity
{
    public class UserLogInfo
    {
        
        private int uId;
        public int UId { get { return uId; } set { uId = value; } }

        private UserLogActionEnum action;
        public UserLogActionEnum Action { get { return action; } set { action = value; } }

        private string dateTime = "";
        public string DateTime { get { return dateTime; } set { dateTime = value; } }
    }
}