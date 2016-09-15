using System;
using System.IO;
using System.Web.UI.WebControls;
using System.Xml;
using Discuz.Common;
using Discuz.Config;
using Discuz.Web.Admin.AutoUpdateManager;

namespace Discuz.Web.Admin
{
    public class hotfix : AdminPage
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
                    XmlNodeList xmlNodeList = xmlDocument.SelectNodes("/localupgrade/optionalupgrade/dnt" + Utils.Version + "/item");
                    XmlDocument xmlDocument2 = new XmlDocument();
                    xmlDocument2.LoadXml(versionList);
                    XmlNodeList xmlNodeList2 = xmlDocument2.SelectNodes("/versionlist/" + BaseConfigs.GetDbType.ToLower() + "/optionalupgrade/dnt" + Utils.Version + "/item");
                    int num = 0;
                    string text = "";
                    foreach (XmlNode xmlNode in xmlNodeList2)
                    {
                        bool flag = false;
                        foreach (XmlNode xmlNode2 in xmlNodeList)
                        {
                            if (xmlNode.FirstChild.InnerText == xmlNode2.InnerText)
                            {
                                flag = true;
                                break;
                            }
                        }
                        if (!flag)
                        {
                            this.isNew = true;
                            Label expr_188 = this.info;
                            object text2 = expr_188.Text;
                            expr_188.Text = text2 + "<input type='checkbox' value='" + xmlNode["version"].InnerText + "' id='checkbox" + num + "' checked='checked'><label for='checkbox" + num + "'>" + xmlNode["versiondescription"].InnerText + "</label>";
                            Label expr_21F = this.info;
                            expr_21F.Text = expr_21F.Text + "<p style='border: 1px dotted rgb(219, 221, 211); background: rgb(255, 255, 204);font-size:12px;padding:0px 0px 0px 15px;'>" + xmlNode["description"].InnerText + "</p>";
                            string text3 = text;
                            text = text3 + "{\"version\":\"" + xmlNode["version"].InnerText + "\",\"versiondescription\":\"" + xmlNode["versiondescription"].InnerText + "\",\"link\":\"" + xmlNode["link"].InnerText + "\"},";
                            num++;
                        }
                    }
                    if (!this.isNew)
                    {
                        this.info.Text = "暂无更新";
                    }
                    else
                    {
                        base.RegisterStartupScript("", "<script>var versionList = [" + text.TrimEnd(',') + "]</script>");
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