using System;
using System.IO;
using System.Web.UI.WebControls;
using System.Xml;
using Discuz.Common;
using Discuz.Config;
using Discuz.Web.Admin.AutoUpdateManager;

namespace Discuz.Web.Admin
{
    public class onlineupgrade : AdminPage
    {
        protected Label info = new Label();
        protected bool isNew;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                try
                {
                    this.MergeUpgradeInfo();
                    AutoUpdate autoUpdate = new AutoUpdate();
                    string versionList = autoUpdate.GetVersionList();
                    StreamWriter streamWriter = new StreamWriter(Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "config/upgradeini.config"));
                    streamWriter.Write(versionList.Replace("\n", "\r\n"));
                    streamWriter.Close();
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "config/localupgradeini.config"));
                    DateTime t = this.StrToDateTime(xmlDocument.SelectSingleNode("/localupgrade/requiredupgrade").InnerText);
                    XmlDocument xmlDocument2 = new XmlDocument();
                    xmlDocument2.LoadXml(versionList);
                    XmlNodeList xmlNodeList = xmlDocument2.SelectNodes("/versionlist/" + BaseConfigs.GetDbType.ToLower() + "/requiredupgrade/item");
                    XmlNode xmlNode = xmlNodeList.Item(xmlNodeList.Count - 1);
                    DateTime t2 = this.StrToDateTime(xmlNode["version"].InnerText);
                    this.isNew = (t2 > t);
                    if (this.isNew)
                    {
                        this.info.Text = "<span style='font-size:20px;padding:0px 0px 15px 0px;display:block;'>检测到最新版本：" + xmlNode["versiondescription"].InnerText + "</span><div style='border: 1px dotted rgb(219, 221, 211);background: #FFFFCC'>" + xmlNode["description"].InnerText + "</div>";
                        string text = "var versionList = [";
                        foreach (XmlNode xmlNode2 in xmlNodeList)
                        {
                            if (this.StrToDateTime(xmlNode2.FirstChild.InnerText) > t)
                            {
                                string text2 = text;
                                text = text2 + "{\"version\":\"" + xmlNode2["version"].InnerText + "\",\"versiondescription\":\"" + xmlNode2["versiondescription"].InnerText + "\",\"link\":\"" + xmlNode2["link"].InnerText + "\"},";
                            }
                        }
                        text = text.TrimEnd(',') + "];";
                        base.RegisterStartupScript("", "<script>" + text + "</script>");
                    }
                    else
                    {
                        this.info.Text = "暂无新版本可更新！";
                        base.RegisterStartupScript("", "<script>var versionList = []; </script>");
                    }
                }
                catch
                {
                    this.info.Text = "升级发生异常，请稍后再试……";
                }
            }
        }

        private void MergeUpgradeInfo()
        {
            if (!File.Exists(Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "config/commonupgradeini.config")))
            {
                return;
            }
            XmlDocument xmlDocument = new XmlDocument();
            string text = "";
            string innerText = "";
            xmlDocument.Load(Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "config/commonupgradeini.config"));
            if (xmlDocument.SelectSingleNode("/requiredupgrade") != null)
            {
                text = xmlDocument.SelectSingleNode("/requiredupgrade").InnerText;
            }
            else
            {
                innerText = xmlDocument.SelectSingleNode("/optionalupgrade").InnerText;
            }
            File.Delete(Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "config/commonupgradeini.config"));
            xmlDocument.Load(Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "config/localupgradeini.config"));
            if (text != "")
            {
                if (this.StrToDateTime(xmlDocument.SelectSingleNode("/localupgrade/requiredupgrade").InnerText) >= this.StrToDateTime(text))
                {
                    return;
                }
                xmlDocument.SelectSingleNode("/localupgrade/requiredupgrade").InnerText = text;
            }
            else
            {
                XmlNode xmlNode = xmlDocument.SelectSingleNode("/localupgrade/optionalupgrade/dnt" + Utils.Version);
                if (xmlNode == null)
                {
                    xmlNode = xmlDocument.CreateElement("dnt" + Utils.Version);
                }
                XmlElement xmlElement = xmlDocument.CreateElement("item");
                xmlElement.InnerText = innerText;
                xmlNode.AppendChild(xmlElement);
                xmlDocument.SelectSingleNode("/localupgrade/optionalupgrade").AppendChild(xmlNode);
            }
            xmlDocument.Save(Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "config/localupgradeini.config"));
        }

        private DateTime StrToDateTime(string str)
        {
            string text = str.Substring(0, 4) + "-" + str.Substring(4, 2) + "-" + str.Substring(6, 2);
            if (str.Length == 8)
            {
                text += " 00:00:00";
            }
            else
            {
                string text2 = text;
                text = text2 + " " + str.Substring(8, 2) + ":" + str.Substring(10, 2) + ":" + str.Substring(12, 2);
            }
            return Convert.ToDateTime(text);
        }
    }
}