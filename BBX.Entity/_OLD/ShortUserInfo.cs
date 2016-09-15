namespace BBX.Entity
{
    public class ShortUserInfo
    {
        private string m_username;
        private string m_nickname;
        private string m_password;
        private string m_secques;
        private string m_email = "";
        private string m_bday;

        private int m_uid;
        public int Uid { get { return m_uid; } set { m_uid = value; } }

        public string Username { get { return m_username.Trim(); } set { m_username = value; } }

        public string Password
        {
            get
            {
                if (!string.IsNullOrEmpty(this.m_password))
                {
                    return this.m_password.Trim();
                }
                return "";
            }
            set { m_password = value; }
        }

        private int m_spaceid;
        public int Spaceid { get { return m_spaceid; } set { m_spaceid = value; } }

        public string Secques
        {
            get
            {
                if (!string.IsNullOrEmpty(this.m_secques))
                {
                    return this.m_secques.Trim();
                }
                return "";
            }
            set { m_secques = value; }
        }

        private int m_gender;
        public int Gender { get { return m_gender; } set { m_gender = value; } }

        private int m_adminid;
        public int Adminid { get { return m_adminid; } set { m_adminid = value; } }

        private int m_groupid;
        public int Groupid { get { return m_groupid; } set { m_groupid = value; } }

        private int m_groupexpiry;
        public int Groupexpiry { get { return m_groupexpiry; } set { m_groupexpiry = value; } }

        private string m_extgroupids;
        public string Extgroupids { get { return m_extgroupids ?? string.Empty; } set { m_extgroupids = value; } }

        private string m_regip;
        public string Regip { get { return m_regip ?? string.Empty; } set { m_regip = value; } }

        private string m_joindate;
        public string Joindate { get { return m_joindate; } set { m_joindate = value; } }

        private string m_lastip;
        public string Lastip { get { return m_lastip; } set { m_lastip = value; } }

        private string m_lastvisit;
        public string Lastvisit { get { return m_lastvisit; } set { m_lastvisit = value; } }

        private string m_lastactivity;
        public string Lastactivity { get { return m_lastactivity; } set { m_lastactivity = value; } }

        private string m_lastpost;
        public string Lastpost { get { return m_lastpost; } set { m_lastpost = value; } }

        private int m_lastpostid;
        public int Lastpostid { get { return m_lastpostid; } set { m_lastpostid = value; } }

        private string m_lastposttitle;
        public string Lastposttitle { get { return m_lastposttitle; } set { m_lastposttitle = value; } }

        private int m_posts;
        public int Posts { get { return m_posts; } set { m_posts = value; } }

        private int m_digestposts;
        public int Digestposts { get { return m_digestposts; } set { m_digestposts = value; } }

        private int m_oltime;
        public int Oltime { get { return m_oltime; } set { m_oltime = value; } }

        private int m_pageviews;
        public int Pageviews { get { return m_pageviews; } set { m_pageviews = value; } }

        private int m_credits;
        public int Credits { get { return m_credits; } set { m_credits = value; } }

        private float m_extcredits1;
        public float Extcredits1 { get { return m_extcredits1; } set { m_extcredits1 = value; } }

        private float m_extcredits2;
        public float Extcredits2 { get { return m_extcredits2; } set { m_extcredits2 = value; } }

        private float m_extcredits3;
        public float Extcredits3 { get { return m_extcredits3; } set { m_extcredits3 = value; } }

        private float m_extcredits4;
        public float Extcredits4 { get { return m_extcredits4; } set { m_extcredits4 = value; } }

        private float m_extcredits5;
        public float Extcredits5 { get { return m_extcredits5; } set { m_extcredits5 = value; } }

        private float m_extcredits6;
        public float Extcredits6 { get { return m_extcredits6; } set { m_extcredits6 = value; } }

        private float m_extcredits7;
        public float Extcredits7 { get { return m_extcredits7; } set { m_extcredits7 = value; } }

        private float m_extcredits8;
        public float Extcredits8 { get { return m_extcredits8; } set { m_extcredits8 = value; } }

        public string Email { get { return m_email.Trim(); } set { m_email = value; } }

        public string Bday { get { return m_bday.Trim(); } set { m_bday = value; } }

        private int m_sigstatus;
        public int Sigstatus { get { return m_sigstatus; } set { m_sigstatus = value; } }

        private int m_tpp;
        public int Tpp { get { return m_tpp; } set { m_tpp = value; } }

        private int m_ppp;
        public int Ppp { get { return m_ppp; } set { m_ppp = value; } }

        private int m_templateid;
        public int Templateid { get { return m_templateid; } set { m_templateid = value; } }

        private int m_pmsound;
        public int Pmsound { get { return m_pmsound; } set { m_pmsound = value; } }

        private int m_showemail;
        public int Showemail { get { return m_showemail; } set { m_showemail = value; } }

        private ReceivePMSettingType m_newsletter;
        public ReceivePMSettingType Newsletter { get { return m_newsletter; } set { m_newsletter = value; } }

        private int m_invisible;
        public int Invisible { get { return m_invisible; } set { m_invisible = value; } }

        private int m_newpm;
        public int Newpm { get { return m_newpm; } set { m_newpm = value; } }

        private int m_newpmcount;
        public int Newpmcount { get { return m_newpmcount; } set { m_newpmcount = value; } }

        private int m_accessmasks;
        public int Accessmasks { get { return m_accessmasks; } set { m_accessmasks = value; } }

        private int m_onlinestate;
        public int Onlinestate { get { return m_onlinestate; } set { m_onlinestate = value; } }

        public string Nickname { get { return m_nickname.Trim(); } set { m_nickname = value; } }

        private string m_salt;
        public string Salt { get { return m_salt; } set { m_salt = value; } }
    }
}