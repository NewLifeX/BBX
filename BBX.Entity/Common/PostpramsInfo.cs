using System;
using BBX.Common;

namespace BBX.Entity
{
    /// <summary>发帖参数信息</summary>
    public class PostpramsInfo
    {
        private string condition;
        private string sdetail;
        private Smilie[] smiliesinfo;
        private CustomEditorButtonInfo[] customeditorbuttoninfo;
        
        private int fid;
        public int Fid { get { return fid; } set { fid = value; } }

        private int tid;
        public int Tid { get { return tid; } set { tid = value; } }

        private int pid;
        public int Pid { get { return pid; } set { pid = value; } }

        private int pagesize;
        public int Pagesize { get { return pagesize; } set { pagesize = value; } }

        private int pageindex;
        public int Pageindex { get { return pageindex; } set { pageindex = value; } }

        private string getattachperm;
        public string Getattachperm { get { return getattachperm; } set { getattachperm = value; } }

        private bool ubbmode;
        public bool Ubbmode { get { return ubbmode; } set { ubbmode = value; } }

        private int usergroupid;
        public int Usergroupid { get { return usergroupid; } set { usergroupid = value; } }

        private int usergroupreadaccess;
        public int Usergroupreadaccess { get { return usergroupreadaccess; } set { usergroupreadaccess = value; } }

        private int attachimgpost;
        public int Attachimgpost { get { return attachimgpost; } set { attachimgpost = value; } }

        private int showattachmentpath;
        public int Showattachmentpath { get { return showattachmentpath; } set { showattachmentpath = value; } }

        private int hide;
        public int Hide { get { return hide; } set { hide = value; } }

        private int price;
        public int Price { get { return price; } set { price = value; } }

        public string Condition
        {
            get
            {
                if (this.condition != null)
                {
                    return this.condition;
                }
                return "";
            }
            set { condition = value; }
        }

        public string Sdetail
        {
            get
            {
                if (this.sdetail != null)
                {
                    return this.sdetail;
                }
                return "";
            }
            set { sdetail = value; }
        }

        private int smileyoff;
        public int Smileyoff { get { return smileyoff; } set { smileyoff = value; } }

        private Boolean bbcodeoff;
        public Boolean BBCode { get { return bbcodeoff; } set { bbcodeoff = value; } }

        private int parseurloff;
        public int Parseurloff { get { return parseurloff; } set { parseurloff = value; } }

        private int showimages;
        public int Showimages { get { return showimages; } set { showimages = value; } }

        private int allowhtml;
        public int Allowhtml { get { return allowhtml; } set { allowhtml = value; } }

        public Smilie[] Smiliesinfo { get { return smiliesinfo; } set { smiliesinfo = value; } }

        public CustomEditorButtonInfo[] Customeditorbuttoninfo { get { return customeditorbuttoninfo; } set { customeditorbuttoninfo = value; } }

        private int smiliesmax;
        public int Smiliesmax { get { return smiliesmax; } set { smiliesmax = value; } }

        private int bbcodemode;
        public int Bbcodemode { get { return bbcodemode; } set { bbcodemode = value; } }

        private int jammer;
        public int Jammer { get { return jammer; } set { jammer = value; } }

        private int onlinetimeout;
        public int Onlinetimeout { get { return onlinetimeout; } set { onlinetimeout = value; } }

        private int currentuserid;
        public int CurrentUserid { get { return currentuserid; } set { currentuserid = value; } }

        private int usercredits;
        public int Usercredits { get { return usercredits; } set { usercredits = value; } }

        private UserGroup currentusergroup;
        public UserGroup CurrentUserGroup { get { return currentusergroup; } set { currentusergroup = value; } }

        private int signature;
        public int Signature { get { return signature; } set { signature = value; } }

        private int isforspace;
        public int Isforspace { get { return isforspace; } set { isforspace = value; } }

        //private TopicInfo topicinfo;
        //public TopicInfo Topicinfo { get { return topicinfo; } set { topicinfo = value; } }

        private int templatewidth = 600;
        public int TemplateWidth { get { return templatewidth; } set { templatewidth = value; } }

        private int invisible;
        public int Invisible { get { return invisible; } set { invisible = value; } }
    }
}