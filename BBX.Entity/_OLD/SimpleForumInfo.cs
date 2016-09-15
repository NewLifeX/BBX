using System;

namespace BBX.Entity
{
    [Serializable]
    public class SimpleForumInfo
    {
        private string m_name = "";
        
        private int m_fid;
        public int Fid { get { return m_fid; } set { m_fid = value; } }

        public string Name { get { return m_name; } set { m_name = value.Trim(); } }

        private string m_url;
        public string Url { get { return m_url; } set { m_url = value; } }

        private int m_postbytopictype;
        public int Postbytopictype { get { return m_postbytopictype; } set { m_postbytopictype = value; } }

        private string m_topictypes;
        public string Topictypes { get { return m_topictypes; } set { m_topictypes = value; } }
    }
}