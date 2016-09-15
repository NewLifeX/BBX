using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Xml;
using NewLife.Reflection;

namespace BBX.Common.Xml
{
    public class XmlDocumentExtender : XmlDocument
    {
        public XmlDocumentExtender()
        {
        }

        public XmlDocumentExtender(XmlNameTable nt)
            : base(new XmlImplementation(nt))
        {
        }

        public override void Load(string filename)
        {
            if (Utils.FileExists(filename))
            {
                base.Load(filename);
                return;
            }
            throw new Exception("文件: " + filename + " 不存在!");
        }

        public bool AppendChildElementByNameValue(ref XmlElement xmlElement, string childElementName, object childElementValue)
        {
            return this.AppendChildElementByNameValue(ref xmlElement, childElementName, childElementValue, false);
        }

        public bool AppendChildElementByNameValue(ref XmlElement xmlElement, string childElementName, object childElementValue, bool IsCDataSection)
        {
            if (xmlElement != null && xmlElement.OwnerDocument != null)
            {
                if (IsCDataSection)
                {
                    XmlCDataSection xmlCDataSection = xmlElement.OwnerDocument.CreateCDataSection(childElementName);
                    xmlCDataSection.InnerText = this.FiltrateControlCharacter(childElementValue.ToString());
                    XmlElement xmlElement2 = xmlElement.OwnerDocument.CreateElement(childElementName);
                    xmlElement2.AppendChild(xmlCDataSection);
                    xmlElement.AppendChild(xmlElement2);
                }
                else
                {
                    XmlElement xmlElement3 = xmlElement.OwnerDocument.CreateElement(childElementName);
                    xmlElement3.InnerText = this.FiltrateControlCharacter(childElementValue.ToString());
                    xmlElement.AppendChild(xmlElement3);
                }
                return true;
            }
            return false;
        }

        public bool AppendChildElementByNameValue(ref XmlNode xmlNode, string childElementName, object childElementValue)
        {
            return this.AppendChildElementByNameValue(ref xmlNode, childElementName, childElementValue, false);
        }

        public bool AppendChildElementByNameValue(ref XmlNode xmlNode, string childElementName, object childElementValue, bool IsCDataSection)
        {
            if (xmlNode != null && xmlNode.OwnerDocument != null)
            {
                if (IsCDataSection)
                {
                    XmlCDataSection xmlCDataSection = xmlNode.OwnerDocument.CreateCDataSection(childElementName);
                    xmlCDataSection.InnerText = this.FiltrateControlCharacter(childElementValue.ToString());
                    XmlElement xmlElement = xmlNode.OwnerDocument.CreateElement(childElementName);
                    xmlElement.AppendChild(xmlCDataSection);
                    xmlNode.AppendChild(xmlElement);
                }
                else
                {
                    XmlElement xmlElement2 = xmlNode.OwnerDocument.CreateElement(childElementName);
                    xmlElement2.InnerText = this.FiltrateControlCharacter(childElementValue.ToString());
                    xmlNode.AppendChild(xmlElement2);
                }
                return true;
            }
            return false;
        }

        public bool AppendChildElementByDataRow(ref XmlElement xmlElement, DataColumnCollection dcc, DataRow dr)
        {
            return this.AppendChildElementByDataRow(ref xmlElement, dcc, dr, null);
        }

        public bool AppendChildElementByDataRow(ref XmlElement xmlElement, DataColumnCollection dcc, DataRow dr, string removecols)
        {
            if (xmlElement != null && xmlElement.OwnerDocument != null)
            {
                foreach (DataColumn dataColumn in dcc)
                {
                    if (String.IsNullOrEmpty(removecols) || ("," + removecols + ",").ToLower().IndexOf("," + dataColumn.Caption.ToLower() + ",") < 0)
                    {
                        var xmlElement2 = xmlElement.OwnerDocument.CreateElement(dataColumn.Caption);
                        xmlElement2.InnerText = this.FiltrateControlCharacter(dr[dataColumn.Caption].ToString().Trim());
                        xmlElement.AppendChild(xmlElement2);
                    }
                }
                return true;
            }
            return false;
        }

        public bool AppendChildElementByEntity(ref XmlElement node, IIndexAccessor entity, string removecols)
        {
            if (node != null && node.OwnerDocument != null)
            {
                foreach (var pi in entity.GetType().GetProperties())
                {
                    if (!pi.CanRead || !pi.CanWrite) continue;

                    if (String.IsNullOrEmpty(removecols) || ("," + removecols + ",").ToLower().IndexOf("," + pi.Name.ToLower() + ",") < 0)
                    {
                        var elm = node.OwnerDocument.CreateElement(pi.Name);
                        elm.InnerText = this.FiltrateControlCharacter((entity[pi.Name] + "").Trim());
                        node.AppendChild(elm);
                    }
                }
                return true;
            }
            return false;
        }

        public XmlNode InitializeNode(string xmlpath)
        {
            return this.InitializeNode(xmlpath, true);
        }

        public XmlNode InitializeNode(string xmlpath, bool isClear)
        {
            XmlNode xmlNode = base.SelectSingleNode(xmlpath);
            if (xmlNode == null)
            {
                return this.CreateNode(xmlpath);
            }
            if (isClear)
            {
                xmlNode.RemoveAll();
            }
            return xmlNode;
        }

        public void RemoveNodeAndChildNode(string xmlpath)
        {
            XmlNodeList xmlNodeList = base.SelectNodes(xmlpath);
            if (xmlNodeList.Count > 0)
            {
                foreach (XmlNode xmlNode in xmlNodeList)
                {
                    xmlNode.RemoveAll();
                    xmlNode.ParentNode.RemoveChild(xmlNode);
                }
            }
        }

        public XmlNode CreateNode(string xmlpath)
        {
            string[] array = xmlpath.Split('/');
            string text = "";
            XmlNode xmlNode = this;
            for (int i = 1; i < array.Length; i++)
            {
                if (base.SelectSingleNode(text + "/" + array[i]) == null)
                {
                    XmlElement newChild = base.CreateElement(array[i]);
                    xmlNode.AppendChild(newChild);
                }
                text = text + "/" + array[i];
                xmlNode = base.SelectSingleNode(text);
            }
            return xmlNode;
        }

        public string GetSingleNodeValue(XmlNode xmlnode, string path)
        {
            if (xmlnode == null)
            {
                return null;
            }
            if (xmlnode.SelectSingleNode(path) == null)
            {
                return null;
            }
            if (xmlnode.SelectSingleNode(path).LastChild != null)
            {
                return xmlnode.SelectSingleNode(path).LastChild.Value;
            }
            return "";
        }

        private string FiltrateControlCharacter(string content)
        {
            return Regex.Replace(content, "[\0-\b|\v-\f|\u000e-\u001f]", "");
        }
    }
}