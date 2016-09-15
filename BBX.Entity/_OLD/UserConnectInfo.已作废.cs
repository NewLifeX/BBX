namespace Discuz.Entity
{
    public class UserConnectInfo
    {
        
        private string openId = "";
        public string OpenId { get { return openId; } set { openId = value; } }

        private int uid = -1;
        public int Uid { get { return uid; } set { uid = value; } }

        private string token = "";
        public string Token { get { return token; } set { token = value; } }

        private string secret = "";
        public string Secret { get { return secret; } set { secret = value; } }

        private int allowVisitQQUserInfo;
        public int AllowVisitQQUserInfo { get { return allowVisitQQUserInfo; } set { allowVisitQQUserInfo = value; } }

        private int allowPushFeed;
        public int AllowPushFeed { get { return allowPushFeed; } set { allowPushFeed = value; } }

        private int isSetPassword;
        public int IsSetPassword { get { return isSetPassword; } set { isSetPassword = value; } }

        private string callbackInfo = "";
        public string CallbackInfo { get { return callbackInfo; } set { callbackInfo = value; } }
    }
}