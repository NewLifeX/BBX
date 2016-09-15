namespace BBX.Entity
{
    public class PrivateMessageInfo
    {
        
        private int m_pmid;
        public int Pmid { get { return m_pmid; } set { m_pmid = value; } }

        private string m_msgfrom;
        public string Msgfrom { get { return m_msgfrom; } set { m_msgfrom = value; } }

        private int m_msgfromid;
        public int Msgfromid { get { return m_msgfromid; } set { m_msgfromid = value; } }

        private string m_msgto;
        public string Msgto { get { return m_msgto; } set { m_msgto = value; } }

        private int m_msgtoid;
        public int Msgtoid { get { return m_msgtoid; } set { m_msgtoid = value; } }

        private int m_folder;
        public int Folder { get { return m_folder; } set { m_folder = value; } }

        private int m_new;
        public int New { get { return m_new; } set { m_new = value; } }

        private string m_subject;
        public string Subject { get { return m_subject; } set { m_subject = value; } }

        private string m_postdatetime;
        public string Postdatetime { get { return m_postdatetime; } set { m_postdatetime = value; } }

        private string m_message;
        public string Message { get { return m_message; } set { m_message = value; } }
    }
}