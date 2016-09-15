using System.Xml;

namespace BBX.Common.Xml
{
    public class XmlNodeInnerTextVisitor : IXmlNodeVisitor
    {
        private XmlNode xmlNode;

        public string this[string __nodeName] { get { return xmlNode.SelectSingleNode(__nodeName).InnerText; } set { xmlNode.SelectSingleNode(__nodeName).InnerText = value; } }

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