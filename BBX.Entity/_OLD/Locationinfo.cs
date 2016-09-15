namespace Discuz.Entity
{
    public class Locationinfo
    {
        
        private int _lid;
        public int Lid { get { return _lid; } set { _lid = value; } }

        private string _city;
        public string City { get { return _city; } set { _city = value; } }

        private string _state;
        public string State { get { return _state; } set { _state = value; } }

        private string _country;
        public string Country { get { return _country; } set { _country = value; } }

        private string _zipcode;
        public string Zipcode { get { return _zipcode; } set { _zipcode = value; } }
    }
}