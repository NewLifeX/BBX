using System;

namespace Discuz.Entity
{
    public class PollInfo
    {
        private string _expiration;
        private string _voternames;
        
        private int _pollid;
        public int Pollid { get { return _pollid; } set { _pollid = value; } }

        private int _tid;
        public int Tid { get { return _tid; } set { _tid = value; } }

        private int _displayorder;
        public int Displayorder { get { return _displayorder; } set { _displayorder = value; } }

        private int _multiple;
        public int Multiple { get { return _multiple; } set { _multiple = value; } }

        private int _visible;
        public int Visible { get { return _visible; } set { _visible = value; } }

        private int _maxchoices;
        public int Maxchoices { get { return _maxchoices; } set { _maxchoices = value; } }

        public string Expiration
        {
            get
            {
                if (this._expiration == null)
                {
                    return DateTime.Now.ToString();
                }
                return this._expiration;
            }
            set { _expiration = value; }
        }

        private int _uid;
        public int Uid { get { return _uid; } set { _uid = value; } }

        public string Voternames
        {
            get
            {
                if (this._voternames != null)
                {
                    return this._voternames;
                }
                return "";
            }
            set { _voternames = value; }
        }

        private int _allowview;
        public int Allowview { get { return _allowview; } set { _allowview = value; } }
    }
}