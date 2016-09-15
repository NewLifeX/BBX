using System.Xml;

namespace BBX.Common.Xml
{
    public class XmlNodeAttributeValueVisitor : IXmlNodeVisitor
    {
        private XmlNode xmlNode;

        public string this[string __nodeName] { get { return xmlNode.Attributes[__nodeName].Value; } set { xmlNode.Attributes[__nodeName].Value = value; } }

        public void SetNode(XmlNode __xmlNode)
        {
            this.xmlNode = __xmlNode;
        }

        public XmlNode GetNode(string __nodeName)
        {
            return this.xmlNode.SelectSingleNode(__nodeName);
        }
    }
}