using System;
using System.ComponentModel;
using System.Web;
using System.Xml.Serialization;
using NewLife.Xml;

namespace BBX.Config
{
    [XmlConfigFile("config/base.config", 60000)]
    /// <summary>基础设置</summary>
    [Description("基础设置")]
    [Serializable]
    public class BaseConfigInfo : XmlConfig2<BaseConfigInfo>
    {
        private string m_dbconnectstring = "Data Source=.;Integrated Security=SSPI;Initial Catalog=BBX;Pooling=true";
        /// <summary>数据库连接字符串</summary>
        [Description("数据库连接字符串")]
        [XmlIgnore]
        public string Dbconnectstring { get { return m_dbconnectstring; } set { m_dbconnectstring = value; } }

        private string m_tableprefix = "";
        /// <summary>表前缀</summary>
        [Description("表前缀")]
        public string Tableprefix { get { return m_tableprefix; } set { m_tableprefix = value; } }

        //private string _forumpath = "/";
        //public string Forumpath { get { return _forumpath; } set { _forumpath = value; } }
        [XmlIgnore]
        public string Forumpath { get { return fpath; } }

        //private string m_dbtype = "SqlServer";
        ///// <summary>数据库类型</summary>
        //[Description("数据库类型")]
        //[XmlIgnore]
        //public string Dbtype { get { return m_dbtype; } set { m_dbtype = value; } }

        private int m_founderuid;
        /// <summary>创建者</summary>
        [Description("创建者")]
        public int Founderuid { get { return m_founderuid; } set { m_founderuid = value; } }

        static String fpath;
        static BaseConfigInfo()
        {
            fpath = HttpRuntime.AppDomainAppVirtualPath.EnsureEnd("/");
        }

        /// <summary>构造函数</summary>
        public BaseConfigInfo()
        {
            var cfg = NewLife.Configuration.Config.ConnectionStrings["BBX"];
            if (cfg != null && !cfg.ConnectionString.IsNullOrWhiteSpace()) m_dbconnectstring = cfg.ConnectionString;
        }
    }
}