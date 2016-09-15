using System;

namespace BBX.Entity
{
    public class Goodsinfo
    {
        private string _title = "";
        private string _lastbuyer = "";
        
        private int _goodsid;
        public int Goodsid { get { return _goodsid; } set { _goodsid = value; } }

        private int _shopid;
        public int Shopid { get { return _shopid; } set { _shopid = value; } }

        private int _categoryid;
        public int Categoryid { get { return _categoryid; } set { _categoryid = value; } }

        private string _parentcategorylist;
        public string Parentcategorylist { get { return _parentcategorylist; } set { _parentcategorylist = value; } }

        private string _shopcategorylist = "";
        public string Shopcategorylist { get { return _shopcategorylist; } set { _shopcategorylist = value; } }

        private int _recommend;
        public int Recommend { get { return _recommend; } set { _recommend = value; } }

        private int _discount;
        public int Discount { get { return _discount; } set { _discount = value; } }

        private int _selleruid;
        public int Selleruid { get { return _selleruid; } set { _selleruid = value; } }

        private string _seller;
        public string Seller { get { return _seller; } set { _seller = value; } }

        private string _account;
        public string Account { get { return _account; } set { _account = value; } }

        public string Title { get { return _title.Trim(); } set { _title = value.Trim(); } }

        private int _magic;
        public int Magic { get { return _magic; } set { _magic = value; } }

        private decimal _price;
        public decimal Price { get { return _price; } set { _price = value; } }

        private int _amount;
        public int Amount { get { return _amount; } set { _amount = value; } }

        private int _quality;
        public int Quality { get { return _quality; } set { _quality = value; } }

        private int _lid;
        public int Lid { get { return _lid; } set { _lid = value; } }

        private string _locus;
        public string Locus { get { return _locus; } set { _locus = value; } }

        private int _transport;
        public int Transport { get { return _transport; } set { _transport = value; } }

        private decimal _ordinaryfee;
        public decimal Ordinaryfee { get { return _ordinaryfee; } set { _ordinaryfee = value; } }

        private decimal _expressfee;
        public decimal Expressfee { get { return _expressfee; } set { _expressfee = value; } }

        private decimal _emsfee;
        public decimal Emsfee { get { return _emsfee; } set { _emsfee = value; } }

        private int _itemtype;
        public int Itemtype { get { return _itemtype; } set { _itemtype = value; } }

        private DateTime _dateline;
        public DateTime Dateline { get { return _dateline; } set { _dateline = value; } }

        private DateTime _expiration;
        public DateTime Expiration { get { return _expiration; } set { _expiration = value; } }

        public string Lastbuyer { get { return _lastbuyer; } set { _lastbuyer = value.Trim(); } }

        private DateTime _lasttrade;
        public DateTime Lasttrade { get { return _lasttrade; } set { _lasttrade = value; } }

        private DateTime _lastupdate;
        public DateTime Lastupdate { get { return _lastupdate; } set { _lastupdate = value; } }

        private int _totalitems;
        public int Totalitems { get { return _totalitems; } set { _totalitems = value; } }

        private decimal _tradesum;
        public decimal Tradesum { get { return _tradesum; } set { _tradesum = value; } }

        private int _closed;
        public int Closed { get { return _closed; } set { _closed = value; } }

        private int _aid;
        public int Aid { get { return _aid; } set { _aid = value; } }

        private string _goodspic = "";
        public string Goodspic { get { return _goodspic; } set { _goodspic = value; } }

        private int _displayorder;
        public int Displayorder { get { return _displayorder; } set { _displayorder = value; } }

        private decimal _costprice;
        public decimal Costprice { get { return _costprice; } set { _costprice = value; } }

        private int _invoice;
        public int Invoice { get { return _invoice; } set { _invoice = value; } }

        private int _repair;
        public int Repair { get { return _repair; } set { _repair = value; } }

        private string _message;
        public string Message { get { return _message; } set { _message = value; } }

        private string _otherlink;
        public string Otherlink { get { return _otherlink; } set { _otherlink = value; } }

        private int _readperm;
        public int Readperm { get { return _readperm; } set { _readperm = value; } }

        private int _tradetype;
        public int Tradetype { get { return _tradetype; } set { _tradetype = value; } }

        private int _tagid;
        public int Tagid { get { return _tagid; } set { _tagid = value; } }

        private int _viewcount;
        public int Viewcount { get { return _viewcount; } set { _viewcount = value; } }

        private int _smileyoff;
        public int Smileyoff { get { return _smileyoff; } set { _smileyoff = value; } }

        private int _bbcodeoff;
        public int Bbcodeoff { get { return _bbcodeoff; } set { _bbcodeoff = value; } }

        private int _parseurloff;
        public int Parseurloff { get { return _parseurloff; } set { _parseurloff = value; } }

        private string _highlight = "";
        public string Highlight { get { return _highlight; } set { _highlight = value; } }

        private string _htmltitle = "";
        public string Htmltitle { get { return _htmltitle; } set { _htmltitle = value; } }
    }
}