using System;

namespace Discuz.Entity
{
    [Serializable]
    public class TemplateInfo
    {
        
        private int m_templateid;
        public int Templateid { get { return m_templateid; } set { m_templateid = value; } }

        private string m_name;
        public string Name { get { return m_name; } set { m_name = value; } }

        private string m_directory;
        public string Directory { get { return m_directory; } set { m_directory = value; } }

        private string m_copyright;
        public string Copyright { get { return m_copyright; } set { m_copyright = value; } }

        private string m_templateurl = "";
        public string Templateurl { get { return m_templateurl; } set { m_templateurl = value; } }
    }
}