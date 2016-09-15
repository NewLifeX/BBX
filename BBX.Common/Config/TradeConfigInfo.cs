using System;
using System.ComponentModel;
using NewLife.Xml;

namespace BBX.Config
{
    [XmlConfigFile("config/trade.config")]
    /// <summary>基础设置</summary>
    [Description("基础设置")]
    [Serializable]
    public class TradeConfigInfo : XmlConfig2<TradeConfigInfo>
    {
        private AliPayConfigInfo _alipayconfiginfo;

        public AliPayConfigInfo Alipayconfiginfo { get { return _alipayconfiginfo; } set { _alipayconfiginfo = value; } }
    }

    [Serializable]
    public class AliPayConfigInfo
    {
        private string _inputCharset = "utf-8";
        private string _partner = "2088002052150939";
        private string _sign = "gh0bis45h89m5mwcoe85us4qrwispes0";
        private string _acount = "";

        public string Inputcharset
        {
            get
            {
                if (this._inputCharset == null)
                {
                    return "utf-8";
                }
                return this._inputCharset;
            }
            set
            {
                if (value != null && value.Length > 50)
                {
                    throw new ArgumentOutOfRangeException("��Ч�� Input_Charset(���������ַ���)", value, value.ToString());
                }
                this._inputCharset = value;
            }
        }

        public string Partner
        {
            get
            {
                if (this._partner == null)
                {
                    throw new ArgumentNullException(this._partner);
                }
                return this._partner;
            }
            set
            {
                if (value != null && value.Length > 16)
                {
                    throw new ArgumentOutOfRangeException("��Ч�� Partner(�������ID)", value, value.ToString());
                }
                this._partner = value;
            }
        }

        public string Sign
        {
            get
            {
                if (this._sign == null)
                {
                    throw new ArgumentNullException(this._sign);
                }
                return this._sign;
            }
            set
            {
                if (value != null && value.Length > 50)
                {
                    throw new ArgumentOutOfRangeException("��Ч�� Sign(ǩ��)", value, value.ToString());
                }
                this._sign = value;
            }
        }

        public string Acount
        {
            get
            {
                if (this._acount == null)
                {
                    throw new ArgumentNullException(this._acount);
                }
                return this._acount;
            }
            set
            {
                if (value != null && value.Length > 16)
                {
                    throw new ArgumentOutOfRangeException("��Ч��֧�����ʺ�", value, value.ToString());
                }
                this._acount = value;
            }
        }
    }
}