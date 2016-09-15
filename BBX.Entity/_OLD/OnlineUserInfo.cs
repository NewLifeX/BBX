namespace Discuz.Entity
{
    public class OnlineUserInfo
    {
        
        private int m_olid;
        public int Olid { get { return m_olid; } set { m_olid = value; } }

        private int m_userid;
        public int Userid { get { return m_userid; } set { m_userid = value; } }

        private string m_username;
        public string Username { get { return m_username; } set { m_username = value; } }

        private string m_nickname;
        public string Nickname { get { return m_nickname; } set { m_nickname = value; } }

        private string m_password;
        public string Password { get { return m_password; } set { m_password = value; } }

        private string m_ip;
        public string Ip { get { return m_ip; } set { m_ip = value; } }

        private short m_groupid;
        public short Groupid { get { return m_groupid; } set { m_groupid = value; } }

        private string m_olimg;
        public string Olimg { get { return m_olimg; } set { m_olimg = value; } }

        private short m_adminid;
        public short Adminid { get { return m_adminid; } set { m_adminid = value; } }

        private short m_invisible;
        public short Invisible { get { return m_invisible; } set { m_invisible = value; } }

        private short m_action;
        public short Action { get { return m_action; } set { m_action = value; } }

        private string m_actionname;
        public string Actionname { get { return m_actionname; } set { m_actionname = value; } }

        private short m_lastactivity;
        public short Lastactivity { get { return m_lastactivity; } set { m_lastactivity = value; } }

        private string m_lastposttime;
        /// <summary>最后发帖时间</summary>
        public string Lastposttime { get { return m_lastposttime; } set { m_lastposttime = value; } }

        private string m_lastpostpmtime;
        /// <summary>最后发送消息时间（老邱说的）</summary>
        public string Lastpostpmtime { get { return m_lastpostpmtime; } set { m_lastpostpmtime = value; } }

        private string m_lastsearchtime;
        public string Lastsearchtime { get { return m_lastsearchtime; } set { m_lastsearchtime = value; } }

        private string m_lastupdatetime;
        public string Lastupdatetime { get { return m_lastupdatetime; } set { m_lastupdatetime = value; } }

        private int m_forumid;
        public int Forumid { get { return m_forumid; } set { m_forumid = value; } }

        private string m_forumname;
        public string Forumname { get { return m_forumname; } set { m_forumname = value; } }

        private int m_titleid;
        public int Titleid { get { return m_titleid; } set { m_titleid = value; } }

        private string m_title;
        public string Title { get { return m_title; } set { m_title = value; } }

        private string m_verifycode;
        public string Verifycode { get { return m_verifycode; } set { m_verifycode = value; } }

        private short m_newpms;
        public short Newpms { get { return m_newpms; } set { m_newpms = value; } }

        private short m_newnotices;
        public short Newnotices { get { return m_newnotices; } set { m_newnotices = value; } }
    }
}