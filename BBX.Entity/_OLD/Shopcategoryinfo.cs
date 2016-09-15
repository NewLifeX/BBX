namespace Discuz.Entity
{
    public class Shopcategoryinfo
    {
        private string _parentidlist = "";
        private string _name = "";
        
        private int _categoryid;
        public int Categoryid { get { return _categoryid; } set { _categoryid = value; } }

        private int _parentid;
        public int Parentid { get { return _parentid; } set { _parentid = value; } }

        public string Parentidlist { get { return _parentidlist.Trim(); } set { _parentidlist = value.Trim(); } }

        private int _layer;
        public int Layer { get { return _layer; } set { _layer = value; } }

        private int _childcount;
        public int Childcount { get { return _childcount; } set { _childcount = value; } }

        private int _syscategoryid;
        public int Syscategoryid { get { return _syscategoryid; } set { _syscategoryid = value; } }

        public string Name { get { return _name.Trim(); } set { _name = value.Trim(); } }

        private string _categorypic = "";
        public string Categorypic { get { return _categorypic; } set { _categorypic = value; } }

        private int _shopid;
        public int Shopid { get { return _shopid; } set { _shopid = value; } }

        private int _displayorder;
        public int Displayorder { get { return _displayorder; } set { _displayorder = value; } }
    }
}