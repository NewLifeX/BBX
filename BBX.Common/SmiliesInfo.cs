using System;

namespace BBX.Common
{
    [Serializable]
    public class SmiliesInfo
    {
        
        private int m_id;
        public int Id { get { return m_id; } set { m_id = value; } }

        private int m_displayorder;
        public int Displayorder { get { return m_displayorder; } set { m_displayorder = value; } }

        private int m_type;
        public int Type { get { return m_type; } set { m_type = value; } }

        private string m_code;
        public string Code { get { return m_code; } set { m_code = value; } }

        private string m_url;
        public string Url { get { return m_url; } set { m_url = value; } }
    }
}