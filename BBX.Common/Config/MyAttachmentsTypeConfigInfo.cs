using System;
using System.ComponentModel;
using System.Xml.Serialization;
using NewLife.Xml;

namespace BBX.Config
{
    [XmlConfigFile("config/myattachment.config", 60000)]
    /// <summary>附件类型设置</summary>
    [Description("附件类型设置")]
    [Serializable]
    public class MyAttachmentsTypeConfigInfo : XmlConfig2<MyAttachmentsTypeConfigInfo>
    {
        [XmlArray("attachtypes")]
        public AttachmentType[] AttachmentType;
    }

    [Serializable]
    public class AttachmentType
    {
        [XmlElement("TypeName")]
        public string TypeName;

        [XmlElement("TypeId")]
        public int TypeId;

        [XmlElement("ExtName")]
        public string ExtName;
    }
}