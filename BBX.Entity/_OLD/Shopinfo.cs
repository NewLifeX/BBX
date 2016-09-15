using System;

namespace Discuz.Entity
{
    public class Shopinfo
    {
        private string _logo = "";
        
        private int _shopid;
        public int Shopid { get { return _shopid; } set { _shopid = value; } }

        public string Logo { get { return _logo.Trim(); } set { _logo = value.Trim(); } }

        private string _shopname;
        public string Shopname { get { return _shopname; } set { _shopname = value; } }

        private int _themeid;
        public int Themeid { get { return _themeid; } set { _themeid = value; } }

        private string _themepath;
        public string Themepath { get { return _themepath; } set { _themepath = value; } }

        private int _uid;
        public int Uid { get { return _uid; } set { _uid = value; } }

        private string _username;
        public string Username { get { return _username; } set { _username = value; } }

        private string _introduce;
        public string Introduce { get { return _introduce; } set { _introduce = value; } }

        private int _lid;
        public int Lid { get { return _lid; } set { _lid = value; } }

        private string _locus;
        public string Locus { get { return _locus; } set { _locus = value; } }

        private string _bulletin;
        public string Bulletin { get { return _bulletin; } set { _bulletin = value; } }

        private DateTime _createdatetime;
        public DateTime Createdatetime { get { return _createdatetime; } set { _createdatetime = value; } }

        private int _invisible;
        public int Invisible { get { return _invisible; } set { _invisible = value; } }

        private int _viewcount;
        public int Viewcount { get { return _viewcount; } set { _viewcount = value; } }
    }
}