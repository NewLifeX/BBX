using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Linq;
using NewLife.Xml;

namespace BBX.Config
{
    [XmlConfigFile("config/ftp.config", 60000)]
    [XmlRoot("ArrayOfFTPConfigInfo")]
    /// <summary>FTP配置</summary>
    [Description("FTP配置")]
    public class FTPConfigInfos : XmlConfig2<FTPConfigInfos>
    {
        private List<FTPConfigInfo> _Infos;

        /// <summary>FTP信息</summary>
        [Description("FTP信息")]
        [XmlArrayItem("FTPConfigInfo")]
        public List<FTPConfigInfo> Infos { get { return _Infos; } set { _Infos = value; } }

        public FTPConfigInfo this[String name]
        {
            get
            {
                if (Infos == null || Infos.Count < 1) return new FTPConfigInfo();
                return Infos.FirstOrDefault(inf => inf.Name.EqualIgnoreCase(name));
            }
        }
    }

    /// <summary>FTP信息</summary>
    [Description("FTP信息")]
    [Serializable]
    public class FTPConfigInfo
    {
        private string m_name = "";
        /// <summary>名称</summary>
        [Description("名称")]
        public string Name { get { return m_name; } set { m_name = value; } }

        private string m_serveraddress = "";
        /// <summary>FTP服务器</summary>
        [Description("FTP服务器")]
        public string Serveraddress { get { return m_serveraddress; } set { m_serveraddress = value; } }

        private int m_serverport = 21;
        /// <summary>端口</summary>
        [Description("端口")]
        public int Serverport { get { return m_serverport; } set { m_serverport = value; } }

        private string m_username = "";
        /// <summary>用户名。该帐号必需具有以下权限:读取文件 写入文件 删除文件 创建目录 子目录继承</summary>
        [DisplayName("用户名")]
        [Description("该帐号必需具有以下权限:读取文件 写入文件 删除文件 创建目录 子目录继承")]
        public string Username { get { return m_username; } set { m_username = value; } }

        private string m_password = "";
        /// <summary>密码</summary>
        [Description("密码")]
        public string Password { get { return m_password; } set { m_password = value; } }

        private int m_mode = 1;
        /// <summary>模式</summary>
        [Description("模式")]
        public int Mode
        {
            get
            {
                if (this.m_mode > 0)
                {
                    return this.m_mode;
                }
                return 1;
            }
            set { m_mode = ((value <= 0) ? 1 : this.m_mode); }
        }

        private int m_allowupload;
        /// <summary>允许上传</summary>
        [Description("允许上传")]
        public int Allowupload { get { return m_allowupload; } set { m_allowupload = value; } }

        private string m_uploadpath = "";
        /// <summary>上传目录</summary>
        [Description("上传目录")]
        public string Uploadpath { get { return m_uploadpath; } set { m_uploadpath = value; } }

        private int m_timeout = 10;
        /// <summary>超时时间(秒)。单位:秒,10 为服务器默认.0为不受超时时间限制.</summary>
        [DisplayName("超时时间(秒)")]
        [Description("单位:秒,10 为服务器默认.0为不受超时时间限制.")]
        public int Timeout
        {
            get
            {
                if (this.m_timeout > 0) return this.m_timeout;

                return 10;
            }
            set { m_timeout = ((value <= 0) ? 10 : this.m_timeout); }
        }

        private string m_remoteurl = "";
        /// <summary>远程地址</summary>
        [Description("远程地址")]
        public string Remoteurl { get { return m_remoteurl; } set { m_remoteurl = value; } }

        private int m_reservelocalattach;
        /// <summary>是否保留本地附件</summary>
        [Description("是否保留本地附件")]
        public int Reservelocalattach { get { return m_reservelocalattach; } set { m_reservelocalattach = value; } }

        private int m_reserveremoteattach = 1;
        /// <summary>删帖时是否保留远程附件</summary>
        [Description("删帖时是否保留远程附件")]
        public int Reserveremoteattach { get { return m_reserveremoteattach; } set { m_reserveremoteattach = value; } }
    }
}