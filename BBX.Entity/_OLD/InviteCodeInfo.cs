namespace Discuz.Entity
{
    public class InviteCodeInfo
    {
        
        private int m_inviteid;
        public int InviteId { get { return m_inviteid; } set { m_inviteid = value; } }

        private string m_invitecode;
        public string Code { get { return m_invitecode; } set { m_invitecode = value; } }

        private int m_creatorid;
        public int CreatorId { get { return m_creatorid; } set { m_creatorid = value; } }

        private string m_creator;
        public string Creator { get { return m_creator; } set { m_creator = value; } }

        private int m_successcount;
        public int SuccessCount { get { return m_successcount; } set { m_successcount = value; } }

        private string m_createtime;
        public string CreateTime { get { return m_createtime; } set { m_createtime = value; } }

        private string m_expiretime;
        public string ExpireTime { get { return m_expiretime; } set { m_expiretime = value; } }

        private int m_maxcount;
        public int MaxCount { get { return m_maxcount; } set { m_maxcount = value; } }

        private int m_invitetype;
        public int InviteType { get { return m_invitetype; } set { m_invitetype = value; } }
    }
}