namespace Discuz.Entity
{
    public class UserBindConnectLog
    {
        
        private string openId;
        public string OpenId { get { return openId; } set { openId = value; } }

        private int uid;
        public int Uid { get { return uid; } set { uid = value; } }

        private int type;
        public int Type { get { return type; } set { type = value; } }

        private int bindCount;
        public int BindCount { get { return bindCount; } set { bindCount = value; } }
    }
}