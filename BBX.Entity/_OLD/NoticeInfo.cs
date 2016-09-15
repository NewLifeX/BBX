namespace Discuz.Entity
{
    public class NoticeInfo
    {
        
        private int m_nid;
        public int Nid { get { return m_nid; } set { m_nid = value; } }

        private int m_uid;
        public int Uid { get { return m_uid; } set { m_uid = value; } }

        private NoticeType m_type;
        public NoticeType Type { get { return m_type; } set { m_type = value; } }

        private int m_fromid;
        public int Fromid { get { return m_fromid; } set { m_fromid = value; } }

        private int m_new;
        public int New { get { return m_new; } set { m_new = value; } }

        private int m_posterid;
        public int Posterid { get { return m_posterid; } set { m_posterid = value; } }

        private string m_poster;
        public string Poster { get { return m_poster; } set { m_poster = value; } }

        private string m_note;
        public string Note { get { return m_note; } set { m_note = value; } }

        private string m_postdatetime;
        public string Postdatetime { get { return m_postdatetime; } set { m_postdatetime = value; } }
    }
}