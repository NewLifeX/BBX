using System;

namespace BBX.Plugin.Payment.Alipay
{
    public class LogisticsInfo
    {
        private string _logistics_type;
        private decimal _logistics_fee;
        private string _logistics_payment;

        public string Logistics_Type
        {
            get { return _logistics_type; }
            set
            {
                if (value != null && value.Length > 50)
                {
                    throw new ArgumentOutOfRangeException("无效的 LogisticsType(物流类型)", value, value.ToString());
                }
                this._logistics_type = value;
            }
        }

        public decimal Logistics_Fee
        {
            get
            {
                this._logistics_fee = decimal.Round(this._logistics_fee, 2);
                return this._logistics_fee;
            }
            set
            {
                if (value < 0.00m || value > 100000000.00m)
                {
                    throw new ArgumentNullException(this._logistics_fee.ToString(), "Price(商品单价) 必须为0.01在100000000.00之间");
                }
                this._logistics_fee = value;
            }
        }

        public string Logistics_Payment
        {
            get { return _logistics_payment; }
            set
            {
                if (value != null && value.Length > 50)
                {
                    throw new ArgumentOutOfRangeException("无效的 LogisticsPayment(物流支付类型)", value, value.ToString());
                }
                this._logistics_payment = value;
            }
        }

        public LogisticsInfo(string logistics_type, decimal logistics_fee, string logistics_payment)
        {
            this._logistics_type = logistics_type;
            this._logistics_fee = logistics_fee;
            this._logistics_payment = logistics_payment;
        }
    }
}