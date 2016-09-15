using System.Collections;
using System.Xml;

namespace Discuz.Entity
{
    public class UserPrefsSaved
    {
        private Hashtable _userPrefs = new Hashtable();

        public UserPrefsSaved(string xmlcontent)
        {
            if (xmlcontent == null || xmlcontent == string.Empty)
            {
                return;
            }
            XmlDocument xmlDocument = new XmlDocument();
            try
            {
                xmlDocument.LoadXml(xmlcontent);
            }
            catch
            {
                return;
            }
            XmlNodeList xmlNodeList = xmlDocument.SelectNodes("/Module/UserPref");
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                this._userPrefs.Add(xmlNode.Attributes["name"].Value, xmlNode.Attributes["value"].Value);
            }
        }

        public string GetValueByName(string name)
        {
            if (this._userPrefs[name] != null)
            {
                return this._userPrefs[name].ToString();
            }
            return "";
        }
    }
}