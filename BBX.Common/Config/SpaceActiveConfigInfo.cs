using System.ComponentModel;
using Discuz.Common;
using NewLife.Xml;

namespace Discuz.Config
{
    /// <summary>空间设置</summary>
    [Description("空间设置")]
    [XmlConfigFile("config/space.config", 60000)]
    public class SpaceActiveConfigInfo : XmlConfig2<SpaceActiveConfigInfo>
    {
        private string m_allowPostcount = "0";
        /// <summary>允许发帖数量</summary>
        [Description("允许发帖数量")]
        public string AllowPostcount { get { return m_allowPostcount; } set { m_allowPostcount = value; } }

        private string m_postcount = "0";
        /// <summary>发帖数量</summary>
        [Description("发帖数量")]
        public string Postcount { get { return m_postcount; } set { m_postcount = value; } }

        private string m_allowDigestcount = "0";
        /// <summary>允许摘录数量</summary>
        [Description("允许摘录数量")]
        public string AllowDigestcount { get { return m_allowDigestcount; } set { m_allowDigestcount = value; } }

        private string m_digestcount = "0";
        /// <summary>摘录数量</summary>
        [Description("摘录数量")]
        public string Digestcount { get { return m_digestcount; } set { m_digestcount = value; } }

        private string m_allowScore = "0";
        /// <summary>空间设置</summary>
        [Description("空间设置")]
        public string AllowScore { get { return m_allowScore; } set { m_allowScore = value; } }

        private string m_score = "0";
        /// <summary>积分</summary>
        [Description("积分")]
        public string Score { get { return m_score; } set { m_score = value; } }

        private string m_allowUsergroups = "1";
        /// <summary>允许用户组</summary>
        [Description("允许用户组")]
        public string AllowUsergroups { get { return m_allowUsergroups; } set { m_allowUsergroups = value; } }

        private string m_usergroups = "1,2,3,10,11,12,13,14,15";
        /// <summary>用户组列表</summary>
        [Description("用户组列表")]
        public string Usergroups { get { return m_usergroups; } set { m_usergroups = value; } }

        private string m_activeType = "1";
        /// <summary>激活类型</summary>
        [Description("激活类型")]
        public string ActiveType { get { return m_activeType; } set { m_activeType = value; } }

        private string m_spacefooterinfo = "";
        /// <summary>尾部信息</summary>
        [Description("尾部信息")]
        public string SpaceFooterInfo { get { return Utils.HtmlDecode(this.m_spacefooterinfo); } set { m_spacefooterinfo = Utils.HtmlEncode(value); } }

        private string m_spacegreeting = "欢迎开通个人空间";
        /// <summary>欢迎词</summary>
        [Description("欢迎词")]
        public string Spacegreeting { get { return Utils.HtmlDecode(this.m_spacegreeting); } set { m_spacegreeting = Utils.HtmlEncode(value); } }

        private int m_enablespacerewrite;
        /// <summary>启用空间Url重写</summary>
        [Description("启用空间Url重写")]
        public int Enablespacerewrite { get { return m_enablespacerewrite; } set { m_enablespacerewrite = value; } }
    }
}