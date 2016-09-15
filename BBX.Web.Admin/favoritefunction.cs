using System;
using System.Xml;
using BBX.Common;
using BBX.Common.Xml;
using BBX.Config;
using BBX.Forum;

namespace BBX.Web.Admin
{
    public class favoritefunction : UserControlsPageBase
    {
        public string resultmessage;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.resultmessage = "<img src='../images/existmenu.gif' style='vertical-align:middle'/> 已经收藏";
            string mapPath = Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "admin/xml/navmenu.config");
            string childElementValue = "";
            string menuparentid = "";
            string text = Request["url"].ToLower();
            XmlDocumentExtender xmlDocumentExtender = new XmlDocumentExtender();
            xmlDocumentExtender.Load(mapPath);
            XmlNodeList xmlNodeList = xmlDocumentExtender.SelectNodes("/dataset/shortcut");
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                if (xmlNode.SelectSingleNode("link").InnerText == text.ToLower().Trim())
                {
                    return;
                }
            }
            XmlNodeList xmlNodeList2 = xmlDocumentExtender.SelectNodes("/dataset/submain");
            XmlNodeInnerTextVisitor xmlNodeInnerTextVisitor = new XmlNodeInnerTextVisitor();
            foreach (XmlNode node in xmlNodeList2)
            {
                xmlNodeInnerTextVisitor.SetNode(node);
                if (xmlNodeInnerTextVisitor["link"].ToLower() == text)
                {
                    childElementValue = xmlNodeInnerTextVisitor["menutitle"];
                    menuparentid = xmlNodeInnerTextVisitor["menuparentid"];
                }
            }
            string[] parm = this.GetParm(xmlDocumentExtender, menuparentid);
            XmlElement newChild = xmlDocumentExtender.CreateElement("shortcut");
            xmlDocumentExtender.AppendChildElementByNameValue(ref newChild, "link", text);
            xmlDocumentExtender.AppendChildElementByNameValue(ref newChild, "menutitle", childElementValue);
            xmlDocumentExtender.AppendChildElementByNameValue(ref newChild, "frameid", "main");
            xmlDocumentExtender.AppendChildElementByNameValue(ref newChild, "custommenu", "true");
            xmlDocumentExtender.AppendChildElementByNameValue(ref newChild, "showmenuid", parm[0]);
            xmlDocumentExtender.AppendChildElementByNameValue(ref newChild, "toptabmenuid", parm[1]);
            xmlDocumentExtender.AppendChildElementByNameValue(ref newChild, "mainmenulist", parm[2]);
            xmlDocumentExtender.SelectSingleNode("/dataset").AppendChild(newChild);
            xmlDocumentExtender.Save(mapPath);
            MenuManage.CreateMenuJson();
        }

        public string[] GetParm(XmlDocumentExtender doc, string menuparentid)
        {
            string[] array = new string[3];
            XmlNodeList xmlNodeList = doc.SelectNodes("/dataset/mainmenu");
            XmlNodeInnerTextVisitor xmlNodeInnerTextVisitor = new XmlNodeInnerTextVisitor();
            foreach (XmlNode node in xmlNodeList)
            {
                xmlNodeInnerTextVisitor.SetNode(node);
                if (xmlNodeInnerTextVisitor["menuid"] == menuparentid)
                {
                    array[0] = xmlNodeInnerTextVisitor["id"];
                    break;
                }
            }
            XmlNodeList xmlNodeList2 = doc.SelectNodes("/dataset/toptabmenu");
            foreach (XmlNode node2 in xmlNodeList2)
            {
                xmlNodeInnerTextVisitor.SetNode(node2);
                if (("," + xmlNodeInnerTextVisitor["mainmenulist"] + ",").IndexOf("," + array[0] + ",") != -1)
                {
                    array[1] = xmlNodeInnerTextVisitor["id"];
                    array[2] = xmlNodeInnerTextVisitor["mainmenulist"];
                    break;
                }
            }
            return array;
        }
    }
}