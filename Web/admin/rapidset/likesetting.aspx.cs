using System;
using System.IO;
using System.Xml;
using BBX.Common.Xml;

namespace BBX.Web.Admin
{
    public partial class likesetting : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string text = this.Page.Server.MapPath("../xml/user_" + this.userid + ".config");
            if (!base.IsPostBack)
            {
                if (File.Exists(text))
                {
                    XmlDocumentExtender xmlDocumentExtender = new XmlDocumentExtender();
                    xmlDocumentExtender.Load(text);
                    this.showhelp.SelectedValue = ((xmlDocumentExtender.SelectSingleNode("/UserConfig/ShowInfo") != null) ? xmlDocumentExtender.SelectSingleNode("/UserConfig/ShowInfo").InnerText : "1");
                    this.showupgrade.SelectedValue = ((xmlDocumentExtender.SelectSingleNode("/UserConfig/ShowUpgrade") != null) ? xmlDocumentExtender.SelectSingleNode("/UserConfig/ShowUpgrade").InnerText : "1");
                    return;
                }
                this.showhelp.SelectedValue = "1";
                this.showupgrade.SelectedValue = "1";
            }
        }

        protected void saveinfo_Click(object sender, EventArgs e)
        {
            string text = this.Page.Server.MapPath("../xml/user_" + this.userid + ".config");
            if (File.Exists(text))
            {
                File.Delete(text);
            }
            XmlDocumentExtender xmlDocumentExtender = new XmlDocumentExtender();
            XmlElement xmlElement = xmlDocumentExtender.CreateElement("UserConfig");
            XmlElement xmlElement2 = xmlDocumentExtender.CreateElement("ShowInfo");
            xmlElement2.InnerText = this.showhelp.SelectedValue.ToString();
            xmlElement.AppendChild(xmlElement2);
            xmlDocumentExtender.AppendChild(xmlElement);
            XmlElement xmlElement3 = xmlDocumentExtender.CreateElement("ShowUpgrade");
            xmlElement3.InnerText = this.showupgrade.SelectedValue.ToString();
            xmlElement.AppendChild(xmlElement3);
            xmlDocumentExtender.Save(text);
            base.RegisterStartupScript("PAGE", "window.location='likesetting.aspx'");
        }
    }
}