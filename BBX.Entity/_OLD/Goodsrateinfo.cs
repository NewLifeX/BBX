using System;

namespace Discuz.Entity
{
    public class Goodsrateinfo
    {
        
        private int _id;
        public int Id { get { return _id; } set { _id = value; } }

        private int _goodstradelogid;
        public int Goodstradelogid { get { return _goodstradelogid; } set { _goodstradelogid = value; } }

        private string _message;
        public string Message { get { return _message; } set { _message = value; } }

        private string _explain;
        public string Explain { get { return _explain; } set { _explain = value; } }

        private string _ip;
        public string Ip { get { return _ip; } set { _ip = value; } }

        private int _uid;
        public int Uid { get { return _uid; } set { _uid = value; } }

        private int _uidtype;
        public int Uidtype { get { return _uidtype; } set { _uidtype = value; } }

        private int _ratetouid;
        public int Ratetouid { get { return _ratetouid; } set { _ratetouid = value; } }

        private string _ratetousername;
        public string Ratetousername { get { return _ratetousername; } set { _ratetousername = value; } }

        private string _username;
        public string Username { get { return _username; } set { _username = value; } }

        private DateTime _postdatetime;
        public DateTime Postdatetime { get { return _postdatetime; } set { _postdatetime = value; } }

        private int _goodsid;
        public int Goodsid { get { return _goodsid; } set { _goodsid = value; } }

        private string _goodstitle;
        public string Goodstitle { get { return _goodstitle; } set { _goodstitle = value; } }

        private decimal _price;
        public decimal Price { get { return _price; } set { _price = value; } }

        private int _ratetype;
        public int Ratetype { get { return _ratetype; } set { _ratetype = value; } }
    }
}