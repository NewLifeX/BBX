using System;
using System.ComponentModel;
using NewLife.Xml;

namespace BBX.Config
{
    /// <summary>QQ登录配置</summary>
    [XmlConfigFile("config/QzoneConnect.config", 60000)]
    [Description("QQ登录配置")]
    [Serializable]
    public class QzoneConnectConfigInfo : XmlConfig2<QzoneConnectConfigInfo>
    {
        /// <summary>启用QQ登录</summary>
        [Description("启用QQ登录")]
        public bool EnableConnect { get; set; }
        /// <summary>推送到QQ空间</summary>
        [Description("推送到QQ空间")]
        public bool EnablePushToQzone { get; set; }
        /// <summary>推送到微博</summary>
        [Description("推送到微博")]
        public bool EnablePushToWeibo { get; set; }
        /// <summary>允许头像</summary>
        [Description("允许头像")]
        public Boolean AllowUseQZAvater { get; set; }
        /// <summary>应用Key</summary>
        [Description("应用Key")]
        public string AppKey { get; set; }
        /// <summary>应用安全码</summary>
        [Description("应用安全码")]
        public string AppSecret { get; set; }
        /// <summary>QQ连接页面</summary>
        [Description("QQ连接页面")]
        public string QzoneConnectPage { get; set; }
    }
}