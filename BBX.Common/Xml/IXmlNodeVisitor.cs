using System.Xml;

namespace BBX.Common.Xml
{
    internal interface IXmlNodeVisitor
    {
        string this[string __nodeName] { get; set; }

        void SetNode(XmlNode __xmlNode);

        XmlNode GetNode(string __nodeName);
    }
}