using System;
using Newtonsoft.Json;

namespace BBX.Entity
{
    [Serializable]
    public class TagInfo : IComparable<TagInfo>
    {
        private string _tagname;
        private string _color;
        
        private int _tagid;
        [JsonProperty("tagid")]
        public int Tagid { get { return _tagid; } set { _tagid = value; } }

        [JsonProperty("tagname")]
        public string Tagname { get { return _tagname; } set { _tagname = value.Trim(); } }

        private int _userid;
        public int Userid { get { return _userid; } set { _userid = value; } }

        private DateTime _postdatetime;
        public DateTime Postdatetime { get { return _postdatetime; } set { _postdatetime = value; } }

        private int _orderid;
        public int Orderid { get { return _orderid; } set { _orderid = value; } }

        public string Color { get { return _color; } set { _color = value.Trim(); } }

        private int _count;
        public int Count { get { return _count; } set { _count = value; } }

        private int _fcount;
        public int Fcount { get { return _fcount; } set { _fcount = value; } }

        private int _pcount;
        public int Pcount { get { return _pcount; } set { _pcount = value; } }

        private int _scount;
        public int Scount { get { return _scount; } set { _scount = value; } }

        private int _vcount;
        public int Vcount { get { return _vcount; } set { _vcount = value; } }

        private int _gcount;
        public int Gcount { get { return _gcount; } set { _gcount = value; } }

        public int CompareTo(TagInfo tag)
        {
            return this.Tagid.CompareTo(tag.Tagid);
        }
    }
}