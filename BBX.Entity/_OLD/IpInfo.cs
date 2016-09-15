using System;

namespace Discuz.Entity
{
    [Serializable]
    public class IpInfo
    {
        
        private int _id;
        public int Id { get { return _id; } set { _id = value; } }

        private int _ip1;
        public int Ip1 { get { return _ip1; } set { _ip1 = value; } }

        private int _ip2;
        public int Ip2 { get { return _ip2; } set { _ip2 = value; } }

        private int _ip3;
        public int Ip3 { get { return _ip3; } set { _ip3 = value; } }

        private int _ip4;
        public int Ip4 { get { return _ip4; } set { _ip4 = value; } }

        private string _username = "";
        public string Username { get { return _username; } set { _username = value; } }

        private string _dateline = "";
        public string Dateline { get { return _dateline; } set { _dateline = value; } }

        private string _expiration = "";
        public string Expiration { get { return _expiration; } set { _expiration = value; } }

        private string _location = "";
        public string Location { get { return _location; } set { _location = value; } }
    }
}