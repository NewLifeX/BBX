namespace Discuz.Entity
{
    public class AlbumInfo
    {
        private string m_title;
        private string m_description;
        private string m_logo;
        private string m_password;
        private string m_createdatetime;

        private int m_albumid;
        public int Albumid { get { return m_albumid; } set { m_albumid = value; } }

        private int m_albumcateid;
        public int Albumcateid { get { return m_albumcateid; } set { m_albumcateid = value; } }

        private int m_userid;
        public int Userid { get { return m_userid; } set { m_userid = value; } }

        private string m_username;
        public string Username { get { return m_username; } set { m_username = value; } }

        public string Title { get { return m_title; } set { m_title = value.Trim(); } }

        public string Description { get { return m_description; } set { m_description = value.Trim(); } }

        public string Logo { get { return m_logo; } set { m_logo = value.Trim(); } }

        public string Password { get { return m_password; } set { m_password = value.Trim(); } }

        private int m_imgcount;
        public int Imgcount { get { return m_imgcount; } set { m_imgcount = value; } }

        private int m_views;
        public int Views { get { return m_views; } set { m_views = value; } }

        private int m_type;
        public int Type { get { return m_type; } set { m_type = value; } }

        public string Createdatetime { get { return m_createdatetime; } set { m_createdatetime = value.Trim(); } }
    }
}