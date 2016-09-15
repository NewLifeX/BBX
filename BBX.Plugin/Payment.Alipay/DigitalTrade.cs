using System;
using BBX.Common;
using BBX.Config;

namespace BBX.Plugin.Payment.Alipay
{
    public class DigitalTrade : ITrade
    {
        protected string _input_charset = "utf-8";
        protected string _agent;
        protected string _sign = "gh0bis45h89m5mwcoe85us4qrwispes0";
        protected string _sign_type = "MD5";
        protected string _body;
        protected string _subject;
        protected string _out_trade_no;
        protected decimal _price;
        protected string _show_url;
        protected int _quantity;
        protected int _payment_type;
        protected string _partner = "2088002872555901";
        protected string _notify_url;
        protected string _return_url;
        protected string _seller_email;
        protected string _seller_id;
        protected string _buyer_email;
        protected string _buyer_id;
        protected string _buyer_msg;
        protected string _royaltyprice;
        protected string _pay_method = "bankPay";
        protected int userCustomPartnerId = GeneralConfigInfo.Current.Usealipaycustompartnerid;

        public string _Type { get { return "alipay"; } } 

        public string _Action { get { return "create"; } } 

        public string _Product { get { return Utils.ProductName; } } 

        public string _Version { get { return Utils.Version; } } 

        public string Body
        {
            get { return _body; }
            set
            {
                if (value != null && value.Trim().Length > 400)
                {
                    this._body = Utils.RemoveHtml(Utils.CutString(value, 0, 400));
                    return;
                }
                this._body = ((string.IsNullOrEmpty (value.Trim())) ? null : value.Trim());
            }
        }

        public string Subject
        {
            get
            {
                if (this._subject == null)
                {
                    throw new ArgumentNullException(this._subject);
                }
                return this._subject;
            }
            set
            {
                if (value != null && value.Length > 256)
                {
                    this._subject = Utils.RemoveHtml(Utils.CutString(value, 0, 256));
                    return;
                }
                this._subject = ((String.IsNullOrEmpty (value.Trim() )) ? null : value.Trim());
            }
        }

        public string Out_Trade_No
        {
            get
            {
                if (this._out_trade_no == null)
                {
                    this._out_trade_no = Utils.GetDateTime();
                    this._out_trade_no = this._out_trade_no.Replace("-", "");
                    this._out_trade_no = this._out_trade_no.Replace(":", "");
                    this._out_trade_no = this._out_trade_no.Replace(" ", "");
                    string text = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
                    Random random = new Random();
                    for (int i = 1; i <= 18; i++)
                    {
                        this._out_trade_no += text.Substring(random.Next(text.Length), 1);
                    }
                    return this._out_trade_no;
                }
                return this._out_trade_no;
            }
            set
            {
                if (value != null && value.Length > 64)
                {
                    throw new ArgumentOutOfRangeException("无效的 OutTradeNo(外部交易号)", value, value.ToString());
                }
                this._out_trade_no = ((String.IsNullOrEmpty (value.Trim())) ? null : value.Trim());
            }
        }

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

        public string PayMethod
        {
            get { return _pay_method; }
            set
            {
                if (value != "bankPay" && value != "cartoon" && value != "directPay")
                {
                    throw new ArgumentOutOfRangeException("无效的默认支付方式", value, value.ToString());
                }
                this._pay_method = value.Trim();
            }
        }

        public string Show_Url
        {
            get { return _show_url; }
            set
            {
                if (value != null && value.Length > 400)
                {
                    throw new ArgumentOutOfRangeException("无效的 ShowUrl(商品展示网址)", value, value.ToString());
                }
                this._show_url = ((String.IsNullOrEmpty (value.Trim())) ? null : value.Trim());
            }
        }

        public int Quantity
        {
            get { return _quantity; }
            set
            {
                if (value < 0 || value > 1000000)
                {
                    throw new ArgumentNullException(this._quantity.ToString(), "Quantity(购买数量) 必须为小于1000000的正整数");
                }
                this._quantity = value;
            }
        }

        public int Payment_Type
        {
            get { return _payment_type; }
            set
            {
                if (value < 0 && value > 6)
                {
                    throw new ArgumentOutOfRangeException("无效的 PaymentType(支付类型)", value, value.ToString());
                }
                this._payment_type = value;
            }
        }

        public virtual string Service { get { return "create_direct_pay_by_user"; } } 

        public string Agent
        {
            get { return _agent; }
            set
            {
                if (value != null && value.Length > 16)
                {
                    throw new ArgumentOutOfRangeException("无效的 Agent(代理)", value, value.ToString());
                }
                this._agent = value;
            }
        }

        public string Input_Charset
        {
            get
            {
                if (this._input_charset == null)
                {
                    return "utf-8";
                }
                return this._input_charset;
            }
            set
            {
                if (value != null && value.Length > 50)
                {
                    throw new ArgumentOutOfRangeException("无效的 Input_Charset(参数编码字符集)", value, value.ToString());
                }
                this._input_charset = value;
            }
        }

        public string Sign
        {
            get
            {
                if (this._sign == null)
                {
                    this._sign = "gh0bis45h89m5mwcoe85us4qrwispes0";
                }
                return this._sign;
            }
            set
            {
                if (value != null && value.Length > 50)
                {
                    throw new ArgumentOutOfRangeException("无效的 Sign(签名)", value, value.ToString());
                }
                this._sign = value;
            }
        }

        public string Sign_Type { get { return "MD5"; } } 

        public string Partner
        {
            get
            {
                if (this._partner == null)
                {
                    this._partner = "2088002872555901";
                }
                return this._partner;
            }
            set
            {
                if (value != null && value.Length > 16)
                {
                    throw new ArgumentOutOfRangeException("无效的 Partner(合作伙伴ID)", value, value.ToString());
                }
                this._partner = value;
            }
        }

        public string Notify_Url { get { return _notify_url; } set { _notify_url = value; } }

        public string Return_Url
        {
            get { return _return_url; }
            set
            {
                if (value != null && value.Length > 100)
                {
                    throw new ArgumentOutOfRangeException("无效的 Return_Url(结果返回URL)", value, value.ToString());
                }
                this._return_url = value;
            }
        }

        public string Seller_Email
        {
            get
            {
                if (this._seller_email == null && this._seller_id == null)
                {
                    throw new ArgumentNullException(this._seller_email, "Seller_Id,Seller_Email不能同时为空");
                }
                return this._seller_email;
            }
            set
            {
                if (value != null && value.Length > 100)
                {
                    throw new ArgumentOutOfRangeException("无效的 SellerEmail(卖家在支付宝的注册EMAIL)", value, value.ToString());
                }
                this._seller_email = ((String.IsNullOrEmpty(value.Trim())) ? null : value.Trim());
            }
        }

        public string Seller_Id
        {
            get
            {
                if (this._seller_email == null && this._seller_id == null)
                {
                    throw new ArgumentNullException(this._seller_id, "Seller_Id,Seller_Email不能同时为空");
                }
                return this._seller_id;
            }
            set
            {
                if (value != null && value.Length > 30)
                {
                    throw new ArgumentOutOfRangeException("无效的 SellerId(卖家在支付宝的注册ID)", value, value.ToString());
                }
                this._seller_id = ((String.IsNullOrEmpty(value.Trim())) ? null : value.Trim());
            }
        }

        public string Buyer_Email
        {
            get { return _buyer_email; }
            set
            {
                if (value != null && value.Length > 100)
                {
                    throw new ArgumentOutOfRangeException("无效的 BuyerEmail(买家在支付宝的注册Email)", value, value.ToString());
                }
                this._buyer_email = ((String.IsNullOrEmpty(value.Trim())) ? null : value.Trim());
            }
        }

        public string Buyer_Id
        {
            get { return _buyer_id; }
            set
            {
                if (value != null && value.Length > 50)
                {
                    throw new ArgumentOutOfRangeException("无效的 BuyerId(买家在支付宝的注册ID)", value, value.ToString());
                }
                this._buyer_id = ((String.IsNullOrEmpty(value.Trim())) ? null : value.Trim());
            }
        }

        public string Buyer_Msg
        {
            get { return _buyer_msg; }
            set
            {
                if (value != null && value.Length > 200)
                {
                    throw new ArgumentOutOfRangeException("无效的 BuyerMsg(买家留言)", value, value.ToString());
                }
                this._buyer_msg = ((String.IsNullOrEmpty(value.Trim())) ? null : value.Trim());
            }
        }

        public string Royalty_Type
        {
            get
            {
                if (this.userCustomPartnerId == 0 && Math.Round(Convert.ToDouble(this.Price) * 0.015, 2) > 0.0)
                {
                    return "10";
                }
                return null;
            }
        }

        public string Royalty_Parameters
        {
            get
            {
                if (this.userCustomPartnerId == 0 && Math.Round(Convert.ToDouble(this.Price) * 0.015, 2) > 0.0)
                {
                    return "support@newlifex.com^" + Math.Round(Convert.ToDouble(this.Price) * 0.015, 2) + "^手续费";
                }
                return null;
            }
        }

        public DigitalTrade()
        {
            var config = TradeConfigInfo.Current;
            _input_charset = config.Alipayconfiginfo.Inputcharset;
            _partner = config.Alipayconfiginfo.Partner;
            _sign = config.Alipayconfiginfo.Sign;
            config.Save();
        }
    }
}