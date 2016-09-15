using System;

namespace BBX.Entity
{
    [Serializable]
    public class TopicInfo
    {
        private string m_poster;
        private string m_title = "";
        
        private int m_attention;
        public int Attention { get { return m_attention; } set { m_attention = value; } }

        private int m_tid;
        public int Tid { get { return m_tid; } set { m_tid = value; } }

        private string m_forumname = "";
        public string Forumname { get { return m_forumname; } set { m_forumname = value; } }

        private int m_fid;
        public int Fid { get { return m_fid; } set { m_fid = value; } }

        private int m_iconid;
        public int Iconid { get { return m_iconid; } set { m_iconid = value; } }

        private int m_typeid;
        public int Typeid { get { return m_typeid; } set { m_typeid = value; } }

        private int m_readperm;
        public int Readperm { get { return m_readperm; } set { m_readperm = value; } }

        private int m_price;
        public int Price { get { return m_price; } set { m_price = value; } }

        public string Poster { get { return m_poster.Trim(); } set { m_poster = value; } }

        private int m_posterid;
        public int Posterid { get { return m_posterid; } set { m_posterid = value; } }

        public string Title { get { return m_title.Trim(); } set { m_title = value; } }

        private string m_postdatetime;
        public string Postdatetime { get { return m_postdatetime; } set { m_postdatetime = value; } }

        private string m_lastpost;
        public string Lastpost { get { return m_lastpost; } set { m_lastpost = value; } }

        private int m_lastpostid;
        public int Lastpostid { get { return m_lastpostid; } set { m_lastpostid = value; } }

        private string m_lastposter;
        public string Lastposter { get { return m_lastposter; } set { m_lastposter = value; } }

        private int m_lastposterid;
        public int Lastposterid { get { return m_lastposterid; } set { m_lastposterid = value; } }

        private int m_views;
        public int Views { get { return m_views; } set { m_views = value; } }

        private int m_replies;
        public int Replies { get { return m_replies; } set { m_replies = value; } }

        private int m_displayorder;
        public int Displayorder { get { return m_displayorder; } set { m_displayorder = value; } }

        private string m_highlight = "";
        public string Highlight { get { return m_highlight; } set { m_highlight = value; } }

        private int m_digest;
        public int Digest { get { return m_digest; } set { m_digest = value; } }

        private int m_rate;
        public int Rate { get { return m_rate; } set { m_rate = value; } }

        private int m_hide;
        public int Hide { get { return m_hide; } set { m_hide = value; } }

        private int m_attachment;
        public int Attachment { get { return m_attachment; } set { m_attachment = value; } }

        private int m_moderated;
        public int Moderated { get { return m_moderated; } set { m_moderated = value; } }

        private int m_closed;
        public int Closed { get { return m_closed; } set { m_closed = value; } }

        private int m_magic;
        public int Magic { get { return m_magic; } set { m_magic = value; } }

        private int m_identify;
        public int Identify { get { return m_identify; } set { m_identify = value; } }

        private byte m_special;
        public byte Special { get { return m_special; } set { m_special = value; } }

        private string m_folder = string.Empty;
        public string Folder { get { return m_folder; } set { m_folder = value; } }

        private string m_topictypename = string.Empty;
        public string Topictypename { get { return m_topictypename; } set { m_topictypename = value; } }
    }
}