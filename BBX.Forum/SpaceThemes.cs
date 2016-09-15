using System;
using System.IO;
using System.Xml;
namespace BBX.Forum
{
	public class SpaceThemes
	{
		public struct SpaceThemeAboutInfo
		{
			public string name;
			public string author;
			public string createdate;
			public string ver;
			public string fordntver;
			public string copyright;
		}
		public static SpaceThemes.SpaceThemeAboutInfo GetThemeAboutInfo(string xmlPath)
		{
			SpaceThemes.SpaceThemeAboutInfo result = default(SpaceThemes.SpaceThemeAboutInfo);
			result.name = "";
			result.author = "";
			result.createdate = "";
			result.ver = "";
			result.fordntver = "";
			result.copyright = "";
			if (!File.Exists(xmlPath + "\\about.xml"))
			{
				return result;
			}
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(xmlPath + "\\about.xml");
			foreach (XmlNode xmlNode in xmlDocument.SelectSingleNode("about").ChildNodes)
			{
				if (xmlNode.NodeType != XmlNodeType.Comment && xmlNode.Name.ToLower() == "template")
				{
					XmlAttribute xmlAttribute = xmlNode.Attributes["name"];
					XmlAttribute xmlAttribute2 = xmlNode.Attributes["author"];
					XmlAttribute xmlAttribute3 = xmlNode.Attributes["createdate"];
					XmlAttribute xmlAttribute4 = xmlNode.Attributes["ver"];
					XmlAttribute xmlAttribute5 = xmlNode.Attributes["fordntver"];
					XmlAttribute xmlAttribute6 = xmlNode.Attributes["copyright"];
					if (xmlAttribute != null)
					{
						result.name = xmlAttribute.Value;
					}
					if (xmlAttribute2 != null)
					{
						result.author = xmlAttribute2.Value;
					}
					if (xmlAttribute3 != null)
					{
						result.createdate = xmlAttribute3.Value;
					}
					if (xmlAttribute4 != null)
					{
						result.ver = xmlAttribute4.Value;
					}
					if (xmlAttribute5 != null)
					{
						result.fordntver = xmlAttribute5.Value;
					}
					if (xmlAttribute6 != null)
					{
						result.copyright = xmlAttribute6.Value;
					}
				}
			}
			return result;
		}
	}
}
