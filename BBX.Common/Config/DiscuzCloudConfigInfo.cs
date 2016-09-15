using System;
using System.ComponentModel;
using NewLife.Xml;

namespace Discuz.Config
{
    [XmlConfigFile("config/dzcloud.config", 15000)]
    /// <summary>云配置</summary>
    [Description("云配置")]
    [Serializable]
    public class DiscuzCloudConfigInfo : XmlConfig2<DiscuzCloudConfigInfo>
    {
        private string m_sitekey = "";
        
        private int m_cloudenabled;
        /// <summary>启用</summary>
        [Description("启用")]
        public int Cloudenabled { get { return m_cloudenabled; } set { m_cloudenabled = value; } }

        private string m_cloudsiteid = "";
        /// <summary>站点标识</summary>
        [Description("站点标识")]
        public string Cloudsiteid { get { return m_cloudsiteid; } set { m_cloudsiteid = value; } }

        private string m_cloudsitekey = "";
        /// <summary>云站点Key</summary>
        [Description("云站点Key")]
        public string Cloudsitekey { get { return m_cloudsitekey; } set { m_cloudsitekey = value; } }

        /// <summary>启用</summary>
        [Description("站点Key")]
        public string Sitekey
        {
            get
            {
                if (this.m_sitekey == "")
                {
                    string text = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
                    Random random = new Random();
                    for (int i = 1; i <= 16; i++)
                    {
                        this.m_sitekey += text.Substring(random.Next(text.Length), 1);
                    }
                }
                return this.m_sitekey;
            }
            set { m_sitekey = value; }
        }

        private int m_connectenabled;
        /// <summary>启用连接</summary>
        [Description("启用连接")]
        public int Connectenabled { get { return m_connectenabled; } set { m_connectenabled = value; } }

        private string m_connectappid = "";
        /// <summary>应用ID</summary>
        [Description("应用ID")]
        public string Connectappid { get { return m_connectappid; } set { m_connectappid = value; } }

        private string m_connectappkey = "";
        /// <summary>应用Key</summary>
        [Description("应用Key")]
        public string Connectappkey { get { return m_connectappkey; } set { m_connectappkey = value; } }

        private int m_allowconnectregister = 1;
        /// <summary>允许注册</summary>
        [Description("允许注册")]
        public int Allowconnectregister { get { return m_allowconnectregister; } set { m_allowconnectregister = value; } }

        private int m_allowuseqzavater = 1;
        /// <summary>允许头像</summary>
        [Description("允许头像")]
        public int Allowuseqzavater { get { return m_allowuseqzavater; } set { m_allowuseqzavater = value; } }

        private int m_maxuserbindcount;
        /// <summary>最大绑定用户数</summary>
        [Description("最大绑定用户数")]
        public int Maxuserbindcount { get { return m_maxuserbindcount; } set { m_maxuserbindcount = value; } }

        //public static DiscuzCloudConfigInfo CreateInstance()
        //{
        //    return new DiscuzCloudConfigInfo();
        //}
    }
}