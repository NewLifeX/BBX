namespace BBX.Entity
{
    public class UserInfo : ShortUserInfo
    {
        private string m_website;
        private string m_icq;
        private string m_qq;
        private string m_yahoo;
        private string m_msn;
        private string m_location;
        private string m_bio;
        private string m_signature;
        private string m_sightml;

        public string Website { get { return (m_website ?? "").Trim(); } set { m_website = value; } }

        public string Icq { get { return (m_icq ?? "").Trim(); } set { m_icq = value; } }

        public string Qq { get { return (m_qq ?? "").Trim(); } set { m_qq = value; } }

        public string Yahoo { get { return (m_yahoo ?? "").Trim(); } set { m_yahoo = value; } }

        public string Msn { get { return (m_msn ?? "").Trim(); } set { m_msn = value; } }

        private string m_skype;
        public string Skype { get { return m_skype; } set { m_skype = value; } }

        public string Location { get { return (m_location ?? "").Trim(); } set { m_location = value; } }

        private string m_customstatus;
        public string Customstatus { get { return m_customstatus; } set { m_customstatus = value; } }

        private string m_medals;
        public string Medals { get { return m_medals; } set { m_medals = value; } }

        public string Bio { get { return (m_bio ?? "").Trim(); } set { m_bio = value; } }

        public string Signature { get { return (m_signature ?? "").Trim(); } set { m_signature = value; } }

        public string Sightml { get { return (m_sightml ?? "").Trim(); } set { m_sightml = value; } }

        private string m_authstr;
        public string Authstr { get { return m_authstr; } set { m_authstr = value; } }

        private string m_authtime;
        public string Authtime { get { return m_authtime; } set { m_authtime = value; } }

        private byte m_authflag;
        public byte Authflag { get { return m_authflag; } set { m_authflag = value; } }

        private string m_realname;
        public string Realname { get { return m_realname; } set { m_realname = value; } }

        private string m_idcard;
        public string Idcard { get { return m_idcard; } set { m_idcard = value; } }

        private string m_mobile;
        public string Mobile { get { return m_mobile; } set { m_mobile = value; } }

        private string m_phone;
        public string Phone { get { return m_phone; } set { m_phone = value; } }

        private string m_ignorepm;
        public string Ignorepm { get { return m_ignorepm ?? string.Empty; } set { m_ignorepm = value; } }

        public UserInfo Clone()
        {
            return (UserInfo)base.MemberwiseClone();
        }
    }
}