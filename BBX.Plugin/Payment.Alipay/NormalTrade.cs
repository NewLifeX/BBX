using System;

namespace BBX.Plugin.Payment.Alipay
{
    public class NormalTrade : DigitalTrade
    {
        private decimal _discount;
        private LogisticsInfo[] _logistics_info;
        private string _receive_name;
        private string _receive_address;
        private string _receive_zip;
        private string _receive_phone;
        private string _receive_mobile;

        public decimal Discount
        {
            get
            {
                this._discount = decimal.Round(this._discount, 2);
                return this._discount;
            }
            set
            {
                if (value < -10000000.00m || value > 100000000.00m)
                {
                    throw new ArgumentNullException(this._discount.ToString(), "Discount(折扣) 必须为-10000000.00在100000000.00之间");
                }
                this._discount = value;
            }
        }

        public override string Service { get { return "trade_create_by_buyer"; } } 

        public LogisticsInfo[] Logistics_Info
        {
            get { return _logistics_info; }
            set
            {
                if (value != null && value.Length <= 0)
                {
                    throw new ArgumentOutOfRangeException("无效的 物流信息(收货人姓名)", value, value.ToString());
                }
                this._logistics_info = value;
            }
        }

        public string Receive_Name
        {
            get { return _receive_name; }
            set
            {
                if (value != null && value.Length > 128)
                {
                    throw new ArgumentOutOfRangeException("无效的 Receive_Name(收货人姓名)", value, value.ToString());
                }
                this._receive_name = ((String.IsNullOrEmpty(value.Trim())) ? null : value.Trim());
            }
        }

        public string Receive_Address
        {
            get { return _receive_address; }
            set
            {
                if (value != null && value.Length > 256)
                {
                    throw new ArgumentOutOfRangeException("无效的 Receive_Address(收货人地址)", value, value.ToString());
                }
                this._receive_address = ((String.IsNullOrEmpty(value.Trim())) ? null : value.Trim());
            }
        }

        public string Receive_Zip
        {
            get { return _receive_zip; }
            set
            {
                if (value != null && value.Length > 6)
                {
                    throw new ArgumentOutOfRangeException("无效的 Receive_Zip(收货人邮编)", value, value.ToString());
                }
                this._receive_zip = ((String.IsNullOrEmpty(value.Trim())) ? null : value.Trim());
            }
        }

        public string Receive_Phone
        {
            get { return _receive_phone; }
            set
            {
                if (value != null && value.Length > 6)
                {
                    throw new ArgumentOutOfRangeException("无效的 Receive_Phone(收货人电话)", value, value.ToString());
                }
                this._receive_phone = ((String.IsNullOrEmpty(value.Trim())) ? null : value.Trim());
            }
        }

        public string Receive_Mobile
        {
            get { return _receive_mobile; }
            set
            {
                if (value != null && value.Length > 6)
                {
                    throw new ArgumentOutOfRangeException("无效的 Receive_Phone(收货人手机)", value, value.ToString());
                }
                this._receive_mobile = ((String.IsNullOrEmpty(value.Trim())) ? null : value.Trim());
            }
        }

        public new string Royalty_Type { get { return null; } } 

        public new string Royalty_Parameters { get { return null; } } 
    }
}