namespace BBX.Entity
{
    public struct BatchSetParams
    {
        
        private bool setpassword;
        public bool SetPassWord { get { return setpassword; } set { setpassword = value; } }

        private bool setattachextensions;
        public bool SetAttachExtensions { get { return setattachextensions; } set { setattachextensions = value; } }

        private bool setpostcredits;
        public bool SetPostCredits { get { return setpostcredits; } set { setpostcredits = value; } }

        private bool setreplycredits;
        public bool SetReplyCredits { get { return setreplycredits; } set { setreplycredits = value; } }

        private bool setsetting;
        public bool SetSetting { get { return setsetting; } set { setsetting = value; } }

        private bool setviewperm;
        public bool SetViewperm { get { return setviewperm; } set { setviewperm = value; } }

        private bool setpostperm;
        public bool SetPostperm { get { return setpostperm; } set { setpostperm = value; } }

        private bool setreplyperm;
        public bool SetReplyperm { get { return setreplyperm; } set { setreplyperm = value; } }

        private bool setgetattachperm;
        public bool SetGetattachperm { get { return setgetattachperm; } set { setgetattachperm = value; } }

        private bool setpostattachperm;
        public bool SetPostattachperm { get { return setpostattachperm; } set { setpostattachperm = value; } }
    }
}