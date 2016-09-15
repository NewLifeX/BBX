using System;
using System.ComponentModel;
using NewLife.Xml;

namespace BBX.Config
{
    [XmlConfigFile("config/spread.config")]
    /// <summary>推广设置</summary>
    [Description("推广设置")]
    [Serializable]
    public class SpreadConfig : XmlConfig2<SpreadConfig>
    {
        private string _transferUrl;
        /// <summary>推广转向网址</summary>
        [Description("推广转向网址")]
        public string TransferUrl { get { return _transferUrl; } set { _transferUrl = value; } }

        private string _spreadCredits;
        /// <summary>推广积分</summary>
        [Description("推广积分")]
        public string SpreadCredits { get { return _spreadCredits; } set { _spreadCredits = value; } }
    }
}