using System;
using System.Xml;
using BBX.Common;
using BBX.Config;

namespace BBX.Web.Admin
{
    public class shortcutmenu : UserControlsPageBase
    {
        public string shortcutmenustr;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.shortcutmenustr = "";
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "admin/xml/navmenu.config"));
            XmlNodeList xmlNodeList = xmlDocument.SelectNodes("/dataset/shortcut");
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                this.shortcutmenustr = this.shortcutmenustr + "<dt><a href='#' onclick=\"resetindexmenu('" + xmlNode.SelectSingleNode("showmenuid").InnerText + "','";
                this.shortcutmenustr = this.shortcutmenustr + xmlNode.SelectSingleNode("toptabmenuid").InnerText + "','" + xmlNode.SelectSingleNode("mainmenulist").InnerText;
                this.shortcutmenustr = this.shortcutmenustr + "','" + xmlNode.SelectSingleNode("link").InnerText + "');\">";
                this.shortcutmenustr = this.shortcutmenustr + xmlNode.SelectSingleNode("menutitle").InnerText + "</a></dt>";
            }
            if (this.shortcutmenustr != "")
            {
                this.shortcutmenustr += "<hr class='line' />";
            }
        }
    }
}