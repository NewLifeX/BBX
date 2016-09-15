using System;

namespace BBX.Entity
{
    public class CreditOrderInfo
    {
        private string _buyer;
        private string _tradeNo;
        private decimal _price;
        private string _orderCode;
        
        private int _orderId;
        public int OrderId { get { return _orderId; } set { _orderId = value; } }

        public string OrderCode { get { return _orderCode.Trim(); } set { _orderCode = value; } }

        private int _uid;
        public int Uid { get { return _uid; } set { _uid = value; } }

        public string Buyer { get { return _buyer.Trim(); } set { _buyer = value; } }

        private int _payType;
        public int PayType { get { return _payType; } set { _payType = value; } }

        public string TradeNo { get { return _tradeNo.Trim(); } set { _tradeNo = value; } }

        public decimal Price
        {
            get
            {
                this._price = decimal.Round(this._price, 2);
                return this._price;
            }
            set
            {
                if (value < 0.01m || value > 100000000.00m)
                {
                    throw new ArgumentNullException(this._price.ToString(), "Price(商品单价) 必须为0.01在100000000.00之间");
                }
                this._price = value;
            }
        }

        private int _orderStatus;
        public int OrderStatus { get { return _orderStatus; } set { _orderStatus = value; } }

        private string _createdTime;
        public string CreatedTime { get { return _createdTime; } set { _createdTime = value; } }

        private string _confirmedTime;
        public string ConfirmedTime { get { return _confirmedTime; } set { _confirmedTime = value; } }

        private int _credit;
        public int Credit { get { return _credit; } set { _credit = value; } }

        private int _amount;
        public int Amount { get { return _amount; } set { _amount = value; } }
    }
}