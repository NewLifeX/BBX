using System;

namespace BBX.Entity
{
    [Serializable]
    public class ShowtopicPagePostInfo : ICloneable
    {
        
        private string m_lastvisit;
        public string Lastvisit { get { return m_lastvisit; } set { m_lastvisit = value; } }

        private string m_oltime;
        public string Oltime { get { return m_oltime; } set { m_oltime = value; } }

        private string m_ubbmessage;
        public string Ubbmessage { get { return m_ubbmessage; } set { m_ubbmessage = value; } }

        private bool m_digged = true;
        public bool Digged { get { return m_digged; } set { m_digged = value; } }

        private int m_id;
        /// <summary>楼层</summary>
		public int Id { get { return m_id; } set { m_id = value; } }
        public int ID { get { return m_pid; } set { m_pid = value; } }

        private string m_postnocustom = "";
        public string Postnocustom { get { return m_postnocustom; } set { m_postnocustom = value; } }

        private int m_pid;
        public int Pid { get { return m_pid; } set { m_pid = value; } }

        private int m_fid;
        public int Fid { get { return m_fid; } set { m_fid = value; } }

        private string m_title;
        public string Title { get { return m_title; } set { m_title = value; } }

        private int m_layer;
        public int Layer { get { return m_layer; } set { m_layer = value; } }

        private string m_message;
        public string Message { get { return m_message; } set { m_message = value; } }

        private string m_ip;
        public string Ip { get { return m_ip; } set { m_ip = value; } }

        private string m_lastedit;
        public string Lastedit { get { return m_lastedit; } set { m_lastedit = value; } }

        private string m_postdatetime;
        public string Postdatetime { get { return m_postdatetime; } set { m_postdatetime = value; } }

        private int m_attachment;
        public int Attachment { get { return m_attachment; } set { m_attachment = value; } }

        private string m_poster;
        public string Poster { get { return m_poster; } set { m_poster = value; } }

        private int m_posterid;
        public int Posterid { get { return m_posterid; } set { m_posterid = value; } }

        private int m_invisible;
        public int Invisible { get { return m_invisible; } set { m_invisible = value; } }

        private int m_usesig;
        public int Usesig { get { return m_usesig; } set { m_usesig = value; } }

        private int m_htmlon;
        public int Htmlon { get { return m_htmlon; } set { m_htmlon = value; } }

        private int m_smileyoff;
        public int Smileyoff { get { return m_smileyoff; } set { m_smileyoff = value; } }

        private int m_parseurloff;
        public int Parseurloff { get { return m_parseurloff; } set { m_parseurloff = value; } }

        private int m_bbcodeoff;
        public int Bbcodeoff { get { return m_bbcodeoff; } set { m_bbcodeoff = value; } }

        private int m_rate;
        public int Rate { get { return m_rate; } set { m_rate = value; } }

        private int m_ratetimes;
        public int Ratetimes { get { return m_ratetimes; } set { m_ratetimes = value; } }

        private string m_nickname;
        public string Nickname { get { return m_nickname; } set { m_nickname = value; } }

        private string m_username;
        public string Username { get { return m_username; } set { m_username = value; } }

        private int m_groupid;
        public int Groupid { get { return m_groupid; } set { m_groupid = value; } }

        private string m_email;
        public string Email { get { return m_email; } set { m_email = value; } }

        private int m_showemail;
        public int Showemail { get { return m_showemail; } set { m_showemail = value; } }

        private int m_digestposts;
        public int Digestposts { get { return m_digestposts; } set { m_digestposts = value; } }

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

        private int m_posts;
        public int Posts { get { return m_posts; } set { m_posts = value; } }

        private string m_joindate;
        public string Joindate { get { return m_joindate; } set { m_joindate = value; } }

        private int m_onlinestate;
        public int Onlinestate { get { return m_onlinestate; } set { m_onlinestate = value; } }

        private string m_lastactivity;
        public string Lastactivity { get { return m_lastactivity; } set { m_lastactivity = value; } }

        private int m_userinvisible;
        public int Userinvisible { get { return m_userinvisible; } set { m_userinvisible = value; } }

        private string m_avatar;
        public string Avatar { get { return m_avatar; } set { m_avatar = value; } }

        private int m_avatarwidth;
        public int Avatarwidth { get { return m_avatarwidth; } set { m_avatarwidth = value; } }

        private int m_avatarheight;
        public int Avatarheight { get { return m_avatarheight; } set { m_avatarheight = value; } }

        private string m_medals;
        public string Medals { get { return m_medals; } set { m_medals = value; } }

        private string m_signature;
        public string Signature { get { return m_signature; } set { m_signature = value; } }

        private string m_location;
        public string Location { get { return m_location; } set { m_location = value; } }

        private string m_customstatus;
        public string Customstatus { get { return m_customstatus; } set { m_customstatus = value; } }

        private string m_website;
        public string Website { get { return m_website; } set { m_website = value; } }

        private string m_icq;
        public string Icq { get { return m_icq; } set { m_icq = value; } }

        private string m_qq;
        public string Qq { get { return m_qq; } set { m_qq = value; } }

        private string m_msn;
        public string Msn { get { return m_msn; } set { m_msn = value; } }

        private string m_yahoo;
        public string Yahoo { get { return m_yahoo; } set { m_yahoo = value; } }

        private string m_skype;
        public string Skype { get { return m_skype; } set { m_skype = value; } }

        private string m_status;
        public string Status { get { return m_status; } set { m_status = value; } }

        private int m_stars;
        public int Stars { get { return m_stars; } set { m_stars = value; } }

        private int m_adindex;
        public int Adindex { get { return m_adindex; } set { m_adindex = value; } }

        private int m_spaceid;
        public int Spaceid { get { return m_spaceid; } set { m_spaceid = value; } }

        private int m_gender;
        public int Gender { get { return m_gender; } set { m_gender = value; } }

        private string m_bday;
        public string Bday { get { return m_bday; } set { m_bday = value; } }

        private int m_debateopinion;
        public int Debateopinion { get { return m_debateopinion; } set { m_debateopinion = value; } }

        private int m_diggs;
        public int Diggs { get { return m_diggs; } set { m_diggs = value; } }

        public object Clone()
        {
            return base.MemberwiseClone();
        }
    }
}