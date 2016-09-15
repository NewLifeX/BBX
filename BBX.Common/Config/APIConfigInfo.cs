using System;
using System.Collections.Generic;
using System.ComponentModel;
using NewLife.Xml;

namespace BBX.Config
{
    [XmlConfigFile("config/api.config", 60000)]
    /// <summary>API配置</summary>
    [Description("API配置")]
    [Serializable]
    public class APIConfigInfo : XmlConfig2<APIConfigInfo>
    {

        private bool _enable;
        /// <summary>启用</summary>
        [Description("启用")]
        public bool Enable { get { return _enable; } set { _enable = value; } }

        private List<ApplicationInfo> _appCollection = new List<ApplicationInfo>();
        /// <summary>应用集合</summary>
        [Description("应用集合")]
        public List<ApplicationInfo> AppCollection { get { return _appCollection; } set { _appCollection = value; } }
    }

    /// <summary>应用信息</summary>
    [Description("应用信息")]
    [Serializable]
    public class ApplicationInfo
    {
        private string _appName;
        /// <summary>名称</summary>
        [Description("名称")]
        public string AppName { get { return _appName; } set { _appName = value; } }

        private string _appUrl;
        /// <summary>地址</summary>
        [Description("地址")]
        public string AppUrl { get { return _appUrl; } set { _appUrl = value; } }

        private string _apiKey;
        /// <summary>应用Key</summary>
        [Description("应用Key")]
        public string APIKey { get { return _apiKey; } set { _apiKey = value; } }

        private string _secret;
        /// <summary>安全码</summary>
        [Description("安全码")]
        public string Secret { get { return _secret; } set { _secret = value; } }

        private string _callbackUrl;
        /// <summary>回调地址</summary>
        [Description("回调地址")]
        public string CallbackUrl { get { return _callbackUrl; } set { _callbackUrl = value; } }

        private string _syncUrl;
        /// <summary>同步数据的 URL 地址</summary>
        [Description("同步数据的 URL 地址")]
        public string SyncUrl { get { return _syncUrl; } set { _syncUrl = value; } }

        private int _syncMode;
        /// <summary>同步模式 开启1/关闭0/自定义2</summary>
        [Description("同步模式 开启1/关闭0/自定义2")]
        public int SyncMode { get { return _syncMode; } set { _syncMode = value; } }

        private string _syncList;
        /// <summary>同步数据的事件列表</summary>
        [Description("同步数据的事件列表")]
        public string SyncList { get { return _syncList; } set { _syncList = value; } }

        private int _applicationType = 1;
        /// <summary>应用类型 Web=1/桌面=2</summary>
        [Description("应用类型 Web=1/桌面=2")]
        public int ApplicationType { get { return _applicationType; } set { _applicationType = value; } }

        private string _ipAddresses;
        /// <summary>允许的服务器IP地址</summary>
        [Description("允许的服务器IP地址")]
        public string IPAddresses { get { return _ipAddresses; } set { _ipAddresses = value; } }

        private string _description;
        /// <summary>描述</summary>
        [Description("描述")]
        public string Description { get { return _description; } set { _description = value; } }

        private string _icon;
        /// <summary>图标</summary>
        [Description("图标")]
        public string Icon { get { return _icon; } set { _icon = value; } }

        private string _logo;
        /// <summary>Logo</summary>
        [Description("Logo")]
        public string Logo { get { return _logo; } set { _logo = value; } }
    }
}