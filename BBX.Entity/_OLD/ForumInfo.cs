using System;

namespace Discuz.Entity
{
    [Serializable]
    public class ForumInfo
    {
        private string m_parentidlist = "";
        private string m_name = "";
        private string m_description = string.Empty;
        private string m_rewritename;

        private int m_fid;
        public int Fid { get { return m_fid; } set { m_fid = value; } }

        private int m_parentid;
        public int Parentid { get { return m_parentid; } set { m_parentid = value; } }

        private int m_layer;
        public int Layer { get { return m_layer; } set { m_layer = value; } }

        private string m_pathlist = "";
        public string Pathlist { get { return m_pathlist; } set { m_pathlist = value; } }

        public string Parentidlist { get { return m_parentidlist.Trim(); } set { m_parentidlist = value; } }

        private int m_subforumcount;
        public int Subforumcount { get { return m_subforumcount; } set { m_subforumcount = value; } }

        public string Name { get { return m_name; } set { m_name = value.Trim(); } }

        private int m_status;
        public int Status { get { return m_status; } set { m_status = value; } }

        private int m_colcount;
        public int Colcount { get { return m_colcount; } set { m_colcount = value; } }

        private int m_displayorder;
        public int Displayorder { get { return m_displayorder; } set { m_displayorder = value; } }

        private int m_templateid;
        public int Templateid { get { return m_templateid; } set { m_templateid = value; } }

        private int m_topics;
        public int Topics { get { return m_topics; } set { m_topics = value; } }

        private int m_curtopics;
        public int CurrentTopics { get { return m_curtopics; } set { m_curtopics = value; } }

        private int m_posts;
        public int Posts { get { return m_posts; } set { m_posts = value; } }

        private int m_todayposts;
        public int Todayposts { get { return m_todayposts; } set { m_todayposts = value; } }

        private string m_lastpost;
        public string Lastpost { get { return m_lastpost; } set { m_lastpost = value; } }

        private string m_lastposter;
        public string Lastposter { get { return m_lastposter; } set { m_lastposter = value; } }

        private int m_lastposterid;
        public int Lastposterid { get { return m_lastposterid; } set { m_lastposterid = value; } }

        private int m_lasttid;
        public int Lasttid { get { return m_lasttid; } set { m_lasttid = value; } }

        private string m_lasttitle;
        public string Lasttitle { get { return m_lasttitle; } set { m_lasttitle = value; } }

        private int m_allowsmilies;
        public int Allowsmilies { get { return m_allowsmilies; } set { m_allowsmilies = value; } }

        private int m_allowrss;
        public int Allowrss { get { return m_allowrss; } set { m_allowrss = value; } }

        private int m_allowhtml;
        public int Allowhtml { get { return m_allowhtml; } set { m_allowhtml = value; } }

        private int m_allowbbcode;
        public int Allowbbcode { get { return m_allowbbcode; } set { m_allowbbcode = value; } }

        private int m_allowimgcode;
        public int Allowimgcode { get { return m_allowimgcode; } set { m_allowimgcode = value; } }

        private int m_allowblog;
        public int Allowblog { get { return m_allowblog; } set { m_allowblog = value; } }

        private int m_istrade;
        public int Istrade { get { return m_istrade; } set { m_istrade = value; } }

        private int m_allowpostspecial;
        public int Allowpostspecial { get { return m_allowpostspecial; } set { m_allowpostspecial = value; } }

        private int m_allowspecialonly;
        public int AllowSpecialonly { get { return m_allowspecialonly; } set { m_allowspecialonly = value; } }

        private int m_alloweditrules;
        public int Alloweditrules { get { return m_alloweditrules; } set { m_alloweditrules = value; } }

        private int m_allowthumbnail;
        public int Allowthumbnail { get { return m_allowthumbnail; } set { m_allowthumbnail = value; } }

        private int m_allowtag;
        public int Allowtag { get { return m_allowtag; } set { m_allowtag = value; } }

        private int m_recyclebin;
        public int Recyclebin { get { return m_recyclebin; } set { m_recyclebin = value; } }

        private int m_modnewposts;
        public int Modnewposts { get { return m_modnewposts; } set { m_modnewposts = value; } }

        private int m_modnewtopics;
        public int Modnewtopics { get { return m_modnewtopics; } set { m_modnewtopics = value; } }

        private int m_jammer;
        public int Jammer { get { return m_jammer; } set { m_jammer = value; } }

        private int m_disablewatermark;
        public int Disablewatermark { get { return m_disablewatermark; } set { m_disablewatermark = value; } }

        private int m_inheritedmod;
        public int Inheritedmod { get { return m_inheritedmod; } set { m_inheritedmod = value; } }

        private int m_autoclose;
        public int Autoclose { get { return m_autoclose; } set { m_autoclose = value; } }

        public string Description { get { return m_description.Trim(); } set { m_description = value; } }

        private string m_password;
        public string Password { get { return m_password; } set { m_password = value; } }

        private string m_icon;
        public string Icon { get { return m_icon; } set { m_icon = value; } }

        private string m_postcredits;
        public string Postcredits { get { return m_postcredits; } set { m_postcredits = value; } }

        private string m_replycredits;
        public string Replycredits { get { return m_replycredits; } set { m_replycredits = value; } }

        private string m_redirect;
        public string Redirect { get { return m_redirect; } set { m_redirect = value; } }

        private string m_attachextensions;
        public string Attachextensions { get { return m_attachextensions; } set { m_attachextensions = value; } }

        private string m_moderators = "";
        public string Moderators { get { return m_moderators; } set { m_moderators = value; } }

        private string m_rules;
        public string Rules { get { return m_rules; } set { m_rules = value; } }

        private string m_topictypes;
        public string Topictypes { get { return m_topictypes; } set { m_topictypes = value; } }

        private string m_viewperm = "";
        public string Viewperm { get { return m_viewperm; } set { m_viewperm = value; } }

        private string m_postperm;
        public string PostPerm { get { return m_postperm; } set { m_postperm = value; } }

        private string m_replyperm;
        public string Replyperm { get { return m_replyperm; } set { m_replyperm = value; } }

        private string m_getattachperm;
        public string Getattachperm { get { return m_getattachperm; } set { m_getattachperm = value; } }

        private string m_postattachperm;
        public string Postattachperm { get { return m_postattachperm; } set { m_postattachperm = value; } }

        private int m_applytopictype;
        public int Applytopictype { get { return m_applytopictype; } set { m_applytopictype = value; } }

        private int m_postbytopictype;
        public int Postbytopictype { get { return m_postbytopictype; } set { m_postbytopictype = value; } }

        private int m_viewbytopictype;
        public int Viewbytopictype { get { return m_viewbytopictype; } set { m_viewbytopictype = value; } }

        private int m_topictypeprefix;
        public int Topictypeprefix { get { return m_topictypeprefix; } set { m_topictypeprefix = value; } }

        private string m_permuserlist;
        public string Permuserlist { get { return m_permuserlist; } set { m_permuserlist = value; } }

        private string m_seokeywords;
        public string Seokeywords { get { return m_seokeywords; } set { m_seokeywords = value; } }

        private string m_seodescription;
        public string Seodescription { get { return m_seodescription; } set { m_seodescription = value; } }

        public string Rewritename
        {
            get
            {
                if (this.m_rewritename != null)
                {
                    return this.m_rewritename;
                }
                return "";
            }
            set { m_rewritename = value; }
        }

        public ForumInfo Clone()
        {
            return (ForumInfo)base.MemberwiseClone();
        }
    }
}