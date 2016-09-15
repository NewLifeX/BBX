namespace BBX.Entity
{
    public class VerifyRegisterInfo
    {
        private string _inviteCode = "";
        
        private int _regId;
        public int RegId { get { return _regId; } set { _regId = value; } }

        private string _ip = "";
        public string IP { get { return _ip; } set { _ip = value; } }

        private string _email = "";
        public string Email { get { return _email; } set { _email = value; } }

        private string _createTime = "";
        public string CreateTime { get { return _createTime; } set { _createTime = value; } }

        private string _expireTime = "";
        public string ExpireTime { get { return _expireTime; } set { _expireTime = value; } }

        public string InviteCode { get { return _inviteCode.Trim(); } set { _inviteCode = value; } }

        private string _verifyCode = "";
        public string VerifyCode { get { return _verifyCode; } set { _verifyCode = value; } }
    }
}