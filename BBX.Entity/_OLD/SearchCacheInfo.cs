namespace Discuz.Entity
{
    public class SearchCacheInfo
    {
        
        private int m_searchid;
        public int Searchid { get { return m_searchid; } set { m_searchid = value; } }

        private string m_keywords;
        public string Keywords { get { return m_keywords; } set { m_keywords = value; } }

        private string m_searchstring;
        public string Searchstring { get { return m_searchstring; } set { m_searchstring = value; } }

        private string m_ip;
        public string Ip { get { return m_ip; } set { m_ip = value; } }

        private int m_uid;
        public int Uid { get { return m_uid; } set { m_uid = value; } }

        private int m_groupid;
        public int Groupid { get { return m_groupid; } set { m_groupid = value; } }

        private string m_postdatetime;
        public string Postdatetime { get { return m_postdatetime; } set { m_postdatetime = value; } }

        private string m_expiration;
        public string Expiration { get { return m_expiration; } set { m_expiration = value; } }

        private int m_topics;
        public int Topics { get { return m_topics; } set { m_topics = value; } }

        private string m_tids;
        public string Tids { get { return m_tids; } set { m_tids = value; } }
    }
}