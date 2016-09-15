using System;

namespace Discuz.Entity
{
    public class Goodsleavewordinfo
    {
        
        private int _id;
        public int Id { get { return _id; } set { _id = value; } }

        private int _goodsid;
        public int Goodsid { get { return _goodsid; } set { _goodsid = value; } }

        private int _tradelogid;
        public int Tradelogid { get { return _tradelogid; } set { _tradelogid = value; } }

        private int _isbuyer;
        public int Isbuyer { get { return _isbuyer; } set { _isbuyer = value; } }

        private int _uid;
        public int Uid { get { return _uid; } set { _uid = value; } }

        private string _username;
        public string Username { get { return _username; } set { _username = value; } }

        private string _message;
        public string Message { get { return _message; } set { _message = value; } }

        private int _invisible;
        public int Invisible { get { return _invisible; } set { _invisible = value; } }

        private string _ip;
        public string Ip { get { return _ip; } set { _ip = value; } }

        private int _usesig;
        public int Usesig { get { return _usesig; } set { _usesig = value; } }

        private int _htmlon;
        public int Htmlon { get { return _htmlon; } set { _htmlon = value; } }

        private int _smileyoff;
        public int Smileyoff { get { return _smileyoff; } set { _smileyoff = value; } }

        private int _parseurloff;
        public int Parseurloff { get { return _parseurloff; } set { _parseurloff = value; } }

        private int _bbcodeoff;
        public int Bbcodeoff { get { return _bbcodeoff; } set { _bbcodeoff = value; } }

        private DateTime _postdatetime;
        public DateTime Postdatetime { get { return _postdatetime; } set { _postdatetime = value; } }
    }
}