namespace BBX.Entity
{
    public class Goodsattachmentinfo : AttachmentInfo
    {
        
        private int _goodsid;
        public int Goodsid { get { return _goodsid; } set { _goodsid = value; } }

        private int _categoryid;
        public int Categoryid { get { return _categoryid; } set { _categoryid = value; } }

        private int _goodscount;
        public int Goodscount { get { return _goodscount; } set { _goodscount = value; } }
    }
}