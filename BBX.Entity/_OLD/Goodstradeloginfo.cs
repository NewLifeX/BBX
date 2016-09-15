using System;

namespace Discuz.Entity
{
    public class Goodstradeloginfo
    {
        
        private int _id;
        public int Id { get { return _id; } set { _id = value; } }

        private int _goodsid;
        public int Goodsid { get { return _goodsid; } set { _goodsid = value; } }

        private string _orderid = "";
        public string Orderid { get { return _orderid; } set { _orderid = value; } }

        private string _tradeno = "";
        public string Tradeno { get { return _tradeno; } set { _tradeno = value; } }

        private string _subject = "";
        public string Subject { get { return _subject; } set { _subject = value; } }

        private decimal _price;
        public decimal Price { get { return _price; } set { _price = value; } }

        private int _quality;
        public int Quality { get { return _quality; } set { _quality = value; } }

        private int _categoryid;
        public int Categoryid { get { return _categoryid; } set { _categoryid = value; } }

        private int _number;
        public int Number { get { return _number; } set { _number = value; } }

        private decimal _tax;
        public decimal Tax { get { return _tax; } set { _tax = value; } }

        private string _locus = "";
        public string Locus { get { return _locus; } set { _locus = value; } }

        private int _sellerid;
        public int Sellerid { get { return _sellerid; } set { _sellerid = value; } }

        private string _seller = "";
        public string Seller { get { return _seller; } set { _seller = value; } }

        private string _selleraccount = "";
        public string Selleraccount { get { return _selleraccount; } set { _selleraccount = value; } }

        private int _buyerid;
        public int Buyerid { get { return _buyerid; } set { _buyerid = value; } }

        private string _buyer = "";
        public string Buyer { get { return _buyer; } set { _buyer = value; } }

        private string _buyercontact = "";
        public string Buyercontact { get { return _buyercontact; } set { _buyercontact = value; } }

        private int _buyercredits;
        public int Buyercredit { get { return _buyercredits; } set { _buyercredits = value; } }

        private string _buyermsg = "";
        public string Buyermsg { get { return _buyermsg; } set { _buyermsg = value; } }

        private int _status;
        public int Status { get { return _status; } set { _status = value; } }

        private DateTime _lastupdate;
        public DateTime Lastupdate { get { return _lastupdate; } set { _lastupdate = value; } }

        private int _offline;
        public int Offline { get { return _offline; } set { _offline = value; } }

        private string _buyername = "";
        public string Buyername { get { return _buyername; } set { _buyername = value; } }

        private string _buyerzip = "";
        public string Buyerzip { get { return _buyerzip; } set { _buyerzip = value; } }

        private string _buyerphone = "";
        public string Buyerphone { get { return _buyerphone; } set { _buyerphone = value; } }

        private string _buyermobile = "";
        public string Buyermobile { get { return _buyermobile; } set { _buyermobile = value; } }

        private int _transport;
        public int Transport { get { return _transport; } set { _transport = value; } }

        private int _transportpay;
        public int Transportpay { get { return _transportpay; } set { _transportpay = value; } }

        private decimal _transportfee;
        public decimal Transportfee { get { return _transportfee; } set { _transportfee = value; } }

        private decimal _tradesum;
        public decimal Tradesum { get { return _tradesum; } set { _tradesum = value; } }

        private decimal _baseprice;
        public decimal Baseprice { get { return _baseprice; } set { _baseprice = value; } }

        private int _discount;
        public int Discount { get { return _discount; } set { _discount = value; } }

        private int _ratestatus;
        public int Ratestatus { get { return _ratestatus; } set { _ratestatus = value; } }

        private string _message = "";
        public string Message { get { return _message; } set { _message = value; } }
    }
}