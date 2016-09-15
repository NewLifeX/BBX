using System;
using System.ComponentModel;
using NewLife.Xml;

namespace BBX.Config
{
    [XmlConfigFile("config/email.config", 60000)]
    /// <summary>邮件设置</summary>
    [Description("邮件设置")]
    [Serializable]
    public class EmailConfigInfo : XmlConfig2<EmailConfigInfo>
    {
        private string smtp;
        public string Smtp { get { return smtp; } set { smtp = value; } }

        private int port = 25;
        public int Port { get { return port; } set { port = value; } }

        private string sysemail;
        public string Sysemail { get { return sysemail; } set { sysemail = value; } }

        private string username;
        public string Username { get { return username; } set { username = value; } }

        private string password;
        public string Password { get { return password; } set { password = value; } }

        private string emailcontent;
        public string Emailcontent { get { return emailcontent; } set { emailcontent = value; } }

        private String _PluginName;
        /// <summary>插件名</summary>
        [Description("插件名")]
        public String PluginName { get { return _PluginName; } set { _PluginName = value; } }

        //private string pluginNameSpace;
        //public string PluginNameSpace { get { return pluginNameSpace; } set { pluginNameSpace = value; } }

        //private string dllFileName;
        //public string DllFileName { get { return dllFileName; } set { dllFileName = value; } }
    }
}