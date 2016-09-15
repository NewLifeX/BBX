using System;

namespace BBX.Entity
{
    [Serializable]
    public class PostInfo
    {
        private string m_title = string.Empty;
        private string m_topictitle = string.Empty;
        private string m_ip = string.Empty;
        
        private int m_pid;
        public int Pid { get { return m_pid; } set { m_pid = value; } }

        private int m_fid;
        public int Fid { get { return m_fid; } set { m_fid = value; } }

        private string m_forumname = string.Empty;
        public string Forumname { get { return m_forumname; } set { m_forumname = value; } }

        private int m_tid;
        public int Tid { get { return m_tid; } set { m_tid = value; } }

        private int m_parentid;
        public int Parentid { get { return m_parentid; } set { m_parentid = value; } }

        private int m_layer;
        public int Layer { get { return m_layer; } set { m_layer = value; } }

        private string m_poster = string.Empty;
        public string Poster { get { return m_poster; } set { m_poster = value; } }

        private int m_posterid;
        public int Posterid { get { return m_posterid; } set { m_posterid = value; } }

        public string Title { get { return m_title.Trim(); } set { m_title = value; } }

        public string Topictitle { get { return m_topictitle.Trim(); } set { m_topictitle = value; } }

        private string m_postdatetime = string.Empty;
        public string Postdatetime { get { return m_postdatetime; } set { m_postdatetime = value; } }

        private string m_message = string.Empty;
        public string Message { get { return m_message; } set { m_message = value; } }

        public string Ip { get { return m_ip.Trim(); } set { m_ip = value; } }

        private string m_lastedit = string.Empty;
        public string Lastedit { get { return m_lastedit; } set { m_lastedit = value; } }

        private int m_invisible;
        public int Invisible { get { return m_invisible; } set { m_invisible = value; } }

        private int m_usesig;
        public int Usesig { get { return m_usesig; } set { m_usesig = value; } }

        private int m_htmlon;
        public int Htmlon { get { return m_htmlon; } set { m_htmlon = value; } }

        private int m_smileyoff;
        public int Smileyoff { get { return m_smileyoff; } set { m_smileyoff = value; } }

        private int m_bbcodeoff;
        public int Bbcodeoff { get { return m_bbcodeoff; } set { m_bbcodeoff = value; } }

        private int m_parseurloff;
        public int Parseurloff { get { return m_parseurloff; } set { m_parseurloff = value; } }

        private int m_attachment;
        public int Attachment { get { return m_attachment; } set { m_attachment = value; } }

        private int m_rate;
        public int Rate { get { return m_rate; } set { m_rate = value; } }

        private int m_ratetimes;
        public int Ratetimes { get { return m_ratetimes; } set { m_ratetimes = value; } }

        private int m_debateopinion;
        public int Debateopinion { get { return m_debateopinion; } set { m_debateopinion = value; } }

        private string m_forumrewritename = string.Empty;
        public string ForumRewriteName { get { return m_forumrewritename; } set { m_forumrewritename = value; } }
    }
}