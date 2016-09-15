namespace Discuz.Entity
{
    public class Goodscategoryinfo
    {
        private string _categoryname = "";
        
        private int _categoryid;
        public int Categoryid { get { return _categoryid; } set { _categoryid = value; } }

        private int _parentid;
        public int Parentid { get { return _parentid; } set { _parentid = value; } }

        private int _layer;
        public int Layer { get { return _layer; } set { _layer = value; } }

        private string _parentidlist;
        public string Parentidlist { get { return _parentidlist; } set { _parentidlist = value; } }

        private int m_displayorder;
        public int Displayorder { get { return m_displayorder; } set { m_displayorder = value; } }

        public string Categoryname { get { return _categoryname; } set { _categoryname = value.Trim(); } }

        private int _haschild;
        public int Haschild { get { return _haschild; } set { _haschild = value; } }

        private int _fid;
        public int Fid { get { return _fid; } set { _fid = value; } }

        private string _pathlist;
        public string Pathlist { get { return _pathlist; } set { _pathlist = value; } }

        private int _goodscount;
        public int Goodscount { get { return _goodscount; } set { _goodscount = value; } }
    }
}