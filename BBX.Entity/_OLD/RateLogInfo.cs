namespace BBX.Entity
{
    public class RateLogInfo
    {
        
        private int id;
        public int Id { get { return id; } set { id = value; } }

        private int pid;
        public int Pid { get { return pid; } set { pid = value; } }

        private int uid;
        public int Uid { get { return uid; } set { uid = value; } }

        private string userName;
        public string UserName { get { return userName; } set { userName = value; } }

        private int extCredits;
        public int ExtCredits { get { return extCredits; } set { extCredits = value; } }

        private string postDateTime;
        public string PostDateTime { get { return postDateTime; } set { postDateTime = value; } }

        private int score;
        public int Score { get { return score; } set { score = value; } }

        private string reason;
        public string Reason { get { return reason; } set { reason = value; } }
    }
}