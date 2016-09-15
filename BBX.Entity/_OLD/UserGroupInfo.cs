using System;

namespace Discuz.Entity
{
    [Serializable]
    public class UserGroupInfo
    {
        private string m_grouptitle;
        private string m_raterange;
        
        private int m_groupid;
        public int Groupid { get { return m_groupid; } set { m_groupid = value; } }

        private int m_radminid;
        public int Radminid { get { return m_radminid; } set { m_radminid = value; } }

        private int m_type;
        public int Type { get { return m_type; } set { m_type = value; } }

        private int m_system;
        public int System { get { return m_system; } set { m_system = value; } }

        public string Grouptitle
        {
            get { return m_grouptitle; }
            set
            {
                if (this.m_color != null && this.m_color != string.Empty)
                {
                    this.m_grouptitle = string.Format("<font color=\"{0}\">{1}</font>", this.m_color, value);
                    return;
                }
                this.m_grouptitle = value;
            }
        }

        private int m_creditshigher;
        public int Creditshigher { get { return m_creditshigher; } set { m_creditshigher = value; } }

        private int m_creditslower;
        public int Creditslower { get { return m_creditslower; } set { m_creditslower = value; } }

        private int m_stars;
        public int Stars { get { return m_stars; } set { m_stars = value; } }

        private string m_color;
        public string Color { get { return m_color; } set { m_color = value; } }

        private string m_groupavatar;
        public string Groupavatar { get { return m_groupavatar; } set { m_groupavatar = value; } }

        private int m_readaccess;
        public int Readaccess { get { return m_readaccess; } set { m_readaccess = value; } }

        private int m_allowvisit;
        public int Allowvisit { get { return m_allowvisit; } set { m_allowvisit = value; } }

        private int m_allowpost;
        public int Allowpost { get { return m_allowpost; } set { m_allowpost = value; } }

        private int m_allowreply;
        public int Allowreply { get { return m_allowreply; } set { m_allowreply = value; } }

        private int m_allowpostpoll;
        public int Allowpostpoll { get { return m_allowpostpoll; } set { m_allowpostpoll = value; } }

        private int m_allowdirectpost;
        public int Allowdirectpost { get { return m_allowdirectpost; } set { m_allowdirectpost = value; } }

        private int m_allowgetattach;
        public int Allowgetattach { get { return m_allowgetattach; } set { m_allowgetattach = value; } }

        private int m_allowpostattach;
        public int Allowpostattach { get { return m_allowpostattach; } set { m_allowpostattach = value; } }

        private int m_allowvote;
        public int Allowvote { get { return m_allowvote; } set { m_allowvote = value; } }

        private int m_allowmultigroups;
        public int Allowmultigroups { get { return m_allowmultigroups; } set { m_allowmultigroups = value; } }

        private int m_allowsearch;
        public int Allowsearch { get { return m_allowsearch; } set { m_allowsearch = value; } }

        public int Allowavatar
        {
            get { return 3; }
            set { } }

        private int m_allowcstatus;
        public int Allowcstatus { get { return m_allowcstatus; } set { m_allowcstatus = value; } }

        private int m_allowuseblog;
        public int Allowuseblog { get { return m_allowuseblog; } set { m_allowuseblog = value; } }

        private int m_allowinvisible;
        public int Allowinvisible { get { return m_allowinvisible; } set { m_allowinvisible = value; } }

        private int m_allowtransfer;
        public int Allowtransfer { get { return m_allowtransfer; } set { m_allowtransfer = value; } }

        private int m_allowsetreadperm;
        public int Allowsetreadperm { get { return m_allowsetreadperm; } set { m_allowsetreadperm = value; } }

        private int m_allowsetattachperm;
        public int Allowsetattachperm { get { return m_allowsetattachperm; } set { m_allowsetattachperm = value; } }

        private int m_allowhidecode;
        public int Allowhidecode { get { return m_allowhidecode; } set { m_allowhidecode = value; } }

        private int m_allowhtmltitle;
        public int Allowhtmltitle { get { return m_allowhtmltitle; } set { m_allowhtmltitle = value; } }

        private int m_allowhtml;
        public int Allowhtml { get { return m_allowhtml; } set { m_allowhtml = value; } }

        private int m_allowcusbbcode;
        public int Allowcusbbcode { get { return m_allowcusbbcode; } set { m_allowcusbbcode = value; } }

        private int m_allownickname;
        public int Allownickname { get { return m_allownickname; } set { m_allownickname = value; } }

        private int m_allowsigbbcode;
        public int Allowsigbbcode { get { return m_allowsigbbcode; } set { m_allowsigbbcode = value; } }

        private int m_allowsigimgcode;
        public int Allowsigimgcode { get { return m_allowsigimgcode; } set { m_allowsigimgcode = value; } }

        private int m_allowviewpro;
        public int Allowviewpro { get { return m_allowviewpro; } set { m_allowviewpro = value; } }

        private int m_allowviewstats;
        public int Allowviewstats { get { return m_allowviewstats; } set { m_allowviewstats = value; } }

        private int m_disableperiodctrl;
        public int Disableperiodctrl { get { return m_disableperiodctrl; } set { m_disableperiodctrl = value; } }

        private int m_reasonpm;
        public int Reasonpm { get { return m_reasonpm; } set { m_reasonpm = value; } }

        private int m_maxprice;
        public int Maxprice { get { return m_maxprice; } set { m_maxprice = value; } }

        private int m_maxpmnum;
        public int Maxpmnum { get { return m_maxpmnum; } set { m_maxpmnum = value; } }

        private int m_maxsigsize;
        public int Maxsigsize { get { return m_maxsigsize; } set { m_maxsigsize = value; } }

        private int m_maxattachsize;
        public int Maxattachsize { get { return m_maxattachsize; } set { m_maxattachsize = value; } }

        private int m_maxsizeperday;
        public int Maxsizeperday { get { return m_maxsizeperday; } set { m_maxsizeperday = value; } }

        private string m_attachextensions;
        public string Attachextensions { get { return m_attachextensions; } set { m_attachextensions = value; } }

        public string Raterange { get { return m_raterange.Trim(); } set { m_raterange = value; } }

        private int m_allowspace;
        public int Allowspace { get { return m_allowspace; } set { m_allowspace = value; } }

        private int m_maxspaceattachsize;
        public int Maxspaceattachsize { get { return m_maxspaceattachsize; } set { m_maxspaceattachsize = value; } }

        private int m_maxspacephotosize;
        public int Maxspacephotosize { get { return m_maxspacephotosize; } set { m_maxspacephotosize = value; } }

        private int m_allowdebate;
        public int Allowdebate { get { return m_allowdebate; } set { m_allowdebate = value; } }

        private int m_allowbonus;
        public int Allowbonus { get { return m_allowbonus; } set { m_allowbonus = value; } }

        private int m_minbonusprice;
        public int Minbonusprice { get { return m_minbonusprice; } set { m_minbonusprice = value; } }

        private int m_maxbonusprice;
        public int Maxbonusprice { get { return m_maxbonusprice; } set { m_maxbonusprice = value; } }

        private int m_allowtrade;
        public int Allowtrade { get { return m_allowtrade; } set { m_allowtrade = value; } }

        private int m_allowdiggs;
        public int Allowdiggs { get { return m_allowdiggs; } set { m_allowdiggs = value; } }

        private int m_modnewtopics;
        public int ModNewTopics { get { return m_modnewtopics; } set { m_modnewtopics = value; } }

        private int m_modnewposts;
        public int ModNewPosts { get { return m_modnewposts; } set { m_modnewposts = value; } }

        private int m_ignoreseccode;
        public int Ignoreseccode { get { return m_ignoreseccode; } set { m_ignoreseccode = value; } }
    }
}