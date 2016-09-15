namespace Discuz.Entity
{
    public class Shoplinkinfo
    {
        private string _name = "";
        
        private int _id;
        public int Id { get { return _id; } set { _id = value; } }

        private int _shopid;
        public int Shopid { get { return _shopid; } set { _shopid = value; } }

        private int _displayorder;
        public int Displayorder { get { return _displayorder; } set { _displayorder = value; } }

        public string Name { get { return _name.Trim(); } set { _name = value.Trim(); } }

        private int _linkshopid;
        public int Linkshopid { get { return _linkshopid; } set { _linkshopid = value; } }
    }
}