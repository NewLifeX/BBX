using System;
using System.Data;
using System.IO;
using System.Text;
using System.Xml;
using BBX.Common;
using BBX.Common.Xml;
using BBX.Config;

namespace BBX.Forum
{
	public class MenuManage
	{
		private static readonly string configPath = Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "admin/xml/navmenu.config");

		public static int NewMainMenu(string title, string defaulturl)
		{
			var xmlDocumentExtender = new XmlDocumentExtender();
			xmlDocumentExtender.Load(configPath);

			var xmlNodeList = xmlDocumentExtender.SelectNodes("/dataset/toptabmenu");
			int result = xmlNodeList.Count + 1;
			XmlElement xmlElement = xmlDocumentExtender.CreateElement("toptabmenu");
			XmlElement xmlElement2 = xmlDocumentExtender.CreateElement("id");
			xmlElement2.InnerText = result.ToString();
			xmlElement.AppendChild(xmlElement2);
			xmlElement2 = xmlDocumentExtender.CreateElement("title");
			xmlElement2.InnerText = title;
			xmlElement.AppendChild(xmlElement2);
			xmlElement2 = xmlDocumentExtender.CreateElement("mainmenulist");
			xmlElement2.InnerText = "";
			xmlElement.AppendChild(xmlElement2);
			xmlElement2 = xmlDocumentExtender.CreateElement("mainmenuidlist");
			xmlElement2.InnerText = "";
			xmlElement.AppendChild(xmlElement2);
			xmlElement2 = xmlDocumentExtender.CreateElement("defaulturl");
			xmlElement2.InnerText = defaulturl;
			xmlElement.AppendChild(xmlElement2);
			xmlElement2 = xmlDocumentExtender.CreateElement("system");
			xmlElement2.InnerText = "0";
			xmlElement.AppendChild(xmlElement2);
			xmlDocumentExtender.SelectSingleNode("/dataset").AppendChild(xmlElement);
			xmlDocumentExtender.Save(configPath);
			return result;
		}

		public static bool EditMainMenu(int menuid, string title, string defaulturl)
		{
			var xmlDocumentExtender = new XmlDocumentExtender();
			xmlDocumentExtender.Load(configPath);

			var xmlNodeList = xmlDocumentExtender.SelectNodes("/dataset/toptabmenu");
			bool flag = false;
			foreach (XmlNode xmlNode in xmlNodeList)
			{
				if (xmlNode["id"].InnerText == menuid.ToString())
				{
					xmlNode["title"].InnerText = title;
					xmlNode["defaulturl"].InnerText = defaulturl;
					flag = true;
					break;
				}
			}
			if (flag)
			{
				xmlDocumentExtender.Save(configPath);
			}
			return flag;
		}

		//public static bool EditMainMenu(string oldMainMenuTitle, string newMainMenuTitle, string defaulturl)
		//{
		//	int num = FindMenuID(oldMainMenuTitle);
		//	return num != -1 && EditMainMenu(num, newMainMenuTitle, defaulturl);
		//}

		public static bool DeleteMainMenu(int menuid)
		{
			var xmlDocumentExtender = new XmlDocumentExtender();
			xmlDocumentExtender.Load(configPath);
			XmlNodeList xmlNodeList = xmlDocumentExtender.SelectNodes("/dataset/toptabmenu");
			XmlNode xmlNode = null;
			int num = menuid;
			bool flag = false;
			foreach (XmlNode xmlNode2 in xmlNodeList)
			{
				if (xmlNode2["id"].InnerText == menuid.ToString())
				{
					if (!xmlNode2["mainmenulist"].InnerText.IsNullOrEmpty())
					{
						return false;
					}
					xmlNode = xmlNode2;
					flag = true;
					break;
				}
				else
				{
					if (xmlNode != null)
					{
						xmlNode2["id"].InnerText = num.ToString();
						num++;
					}
				}
			}
			if (flag)
			{
				xmlNode.ParentNode.RemoveChild(xmlNode);
				xmlDocumentExtender.Save(configPath);
			}
			return flag;
		}
		//public static bool DeleteMainMenu(string menuTitle)
		//{
		//	int num = FindMenuID(menuTitle);
		//	return num != -1 && DeleteMainMenu(num);
		//}
		public static bool EditSubMenu(int submenuid, string menutitle)
		{
			var xmlDocumentExtender = new XmlDocumentExtender();
			xmlDocumentExtender.Load(configPath);
			XmlNodeList xmlNodeList = xmlDocumentExtender.SelectNodes("/dataset/mainmenu");
			bool flag = false;
			foreach (XmlNode xmlNode in xmlNodeList)
			{
				if (xmlNode["id"].InnerText == submenuid.ToString())
				{
					xmlNode["menutitle"].InnerText = menutitle;
					flag = true;
					break;
				}
			}
			if (flag)
			{
				xmlDocumentExtender.Save(configPath);
			}
			return flag;
		}
		//public static bool EditSubMenu(string mainMenuTitle, string oldSubMenuTitle, string newSubMenuTitle)
		//{
		//	int num = FindMenuID(mainMenuTitle, oldSubMenuTitle);
		//	return num != -1 && EditSubMenu(num, newSubMenuTitle);
		//}
		public static int NewSubMenu(int mainmenuid, string menutitle)
		{
			int num = 1;
			int num2 = 100;
			XmlDocumentExtender xmlDocumentExtender = new XmlDocumentExtender();
			xmlDocumentExtender.Load(configPath);
			XmlNodeList xmlNodeList = xmlDocumentExtender.SelectNodes("/dataset/mainmenu");
			num += int.Parse(xmlNodeList.Item(xmlNodeList.Count - 1)["id"].InnerText);
			num2 += int.Parse(xmlNodeList.Item(xmlNodeList.Count - 1)["menuid"].InnerText);
			XmlElement xmlElement = xmlDocumentExtender.CreateElement("mainmenu");
			XmlElement xmlElement2 = xmlDocumentExtender.CreateElement("id");
			xmlElement2.InnerText = num.ToString();
			xmlElement.AppendChild(xmlElement2);
			xmlElement2 = xmlDocumentExtender.CreateElement("menuid");
			xmlElement2.InnerText = num2.ToString();
			xmlElement.AppendChild(xmlElement2);
			xmlElement2 = xmlDocumentExtender.CreateElement("menutitle");
			xmlElement2.InnerText = menutitle;
			xmlElement.AppendChild(xmlElement2);
			xmlDocumentExtender.SelectSingleNode("/dataset").AppendChild(xmlElement);
			XmlNodeList xmlNodeList2 = xmlDocumentExtender.SelectNodes("/dataset/toptabmenu");
			foreach (XmlNode xmlNode in xmlNodeList2)
			{
				if (xmlNode["id"].InnerText == mainmenuid.ToString())
				{
					XmlElement expr_14C = xmlNode["mainmenulist"];
					expr_14C.InnerText = expr_14C.InnerText + "," + num;
					XmlElement expr_173 = xmlNode["mainmenuidlist"];
					expr_173.InnerText = expr_173.InnerText + "," + num2;
					xmlNode["mainmenulist"].InnerText = xmlNode["mainmenulist"].InnerText.TrimStart(',');
					xmlNode["mainmenuidlist"].InnerText = xmlNode["mainmenuidlist"].InnerText.TrimStart(',');
					break;
				}
			}
			xmlDocumentExtender.Save(configPath);
			return num2;
		}
		//public static int NewSubMenu(string mainMenuTitle, string newSubMenuTitle)
		//{
		//	int num = FindMenuID(mainMenuTitle);
		//	if (num == -1)
		//	{
		//		return -1;
		//	}
		//	return NewSubMenu(num, newSubMenuTitle);
		//}
		public static bool DeleteSubMenu(int submenuid, int mainmenuid)
		{
			XmlDocumentExtender xmlDocumentExtender = new XmlDocumentExtender();
			xmlDocumentExtender.Load(configPath);
			XmlNodeList xmlNodeList = xmlDocumentExtender.SelectNodes("/dataset/mainmenu");
			bool flag = false;
			string text = "";
			foreach (XmlNode xmlNode in xmlNodeList)
			{
				if (xmlNode["id"].InnerText == submenuid.ToString())
				{
					text = xmlNode["menuid"].InnerText;
					XmlNodeList xmlNodeList2 = xmlDocumentExtender.SelectNodes("/dataset/submain");
					foreach (XmlNode xmlNode2 in xmlNodeList2)
					{
						if (xmlNode2["menuparentid"].InnerText == text)
						{
							return false;
						}
					}
					xmlNode.ParentNode.RemoveChild(xmlNode);
					flag = true;
					break;
				}
			}
			XmlNodeList xmlNodeList3 = xmlDocumentExtender.SelectNodes("/dataset/toptabmenu");
			foreach (XmlNode xmlNode3 in xmlNodeList3)
			{
				if (xmlNode3["id"].InnerText == mainmenuid.ToString())
				{
					xmlNode3["mainmenulist"].InnerText = ("," + xmlNode3["mainmenulist"].InnerText + ",").Replace("," + submenuid + ",", ",");
					xmlNode3["mainmenuidlist"].InnerText = ("," + xmlNode3["mainmenuidlist"].InnerText + ",").Replace("," + text + ",", ",");
					xmlNode3["mainmenulist"].InnerText = xmlNode3["mainmenulist"].InnerText.TrimStart(',').TrimEnd(',');
					xmlNode3["mainmenuidlist"].InnerText = xmlNode3["mainmenuidlist"].InnerText.TrimStart(',').TrimEnd(',');
					break;
				}
			}
			if (flag)
			{
				xmlDocumentExtender.Save(configPath);
			}
			return flag;
		}
		//public static bool DownSubMenu(int submenuid, int mainmenuid)
		//{
		//	return true;
		//}
		//public static bool DeleteSubMenu(string mainMenuTitle, string subMenuTitle)
		//{
		//	int num = FindMenuID(mainMenuTitle);
		//	int num2 = FindMenuID(mainMenuTitle, subMenuTitle);
		//	return num != -1 && num2 != -1 && DeleteSubMenu(num2, num);
		//}
		public static bool NewMenuItem(int menuparentid, string title, string link)
		{
			XmlDocumentExtender xmlDocumentExtender = new XmlDocumentExtender();
			xmlDocumentExtender.Load(configPath);
			XmlNodeList xmlNodeList = xmlDocumentExtender.SelectNodes("/dataset/submain");
			foreach (XmlNode xmlNode in xmlNodeList)
			{
				if (xmlNode["link"].InnerText == link)
				{
					return false;
				}
			}
			XmlElement xmlElement = xmlDocumentExtender.CreateElement("submain");
			XmlElement xmlElement2 = xmlDocumentExtender.CreateElement("menuparentid");
			xmlElement2.InnerText = menuparentid.ToString();
			xmlElement.AppendChild(xmlElement2);
			xmlElement2 = xmlDocumentExtender.CreateElement("menutitle");
			xmlElement2.InnerText = title;
			xmlElement.AppendChild(xmlElement2);
			xmlElement2 = xmlDocumentExtender.CreateElement("link");
			xmlElement2.InnerText = link;
			xmlElement.AppendChild(xmlElement2);
			xmlElement2 = xmlDocumentExtender.CreateElement("frameid");
			xmlElement2.InnerText = "main";
			xmlElement.AppendChild(xmlElement2);
			xmlDocumentExtender.SelectSingleNode("/dataset").AppendChild(xmlElement);
			xmlDocumentExtender.Save(configPath);
			return true;
		}
		//public static bool NewMenuItem(string mainMenuTitle, string subMenutitle, string newMenuItemTitle, string link)
		//{
		//	int num = FindMenuMenuid(mainMenuTitle, subMenutitle);
		//	return num != -1 && NewMenuItem(num, newMenuItemTitle, link);
		//}
		public static bool EditMenuItem(int id, string title, string link)
		{
			XmlDocumentExtender xmlDocumentExtender = new XmlDocumentExtender();
			xmlDocumentExtender.Load(configPath);
			XmlNodeList xmlNodeList = xmlDocumentExtender.SelectNodes("/dataset/submain");
			int num = 0;
			foreach (XmlNode xmlNode in xmlNodeList)
			{
				if (num.ToString() != id.ToString() && xmlNode["link"].InnerText == link)
				{
					return false;
				}
				num++;
			}
			string innerText = xmlNodeList.Item(id)["link"].InnerText;
			xmlNodeList.Item(id)["menutitle"].InnerText = title;
			xmlNodeList.Item(id)["link"].InnerText = link;
			XmlNodeList xmlNodeList2 = xmlDocumentExtender.SelectNodes("/dataset/shortcut");
			foreach (XmlNode xmlNode2 in xmlNodeList2)
			{
				if (xmlNode2["link"].InnerText == innerText)
				{
					xmlNode2["link"].InnerText = link;
					xmlNode2["menutitle"].InnerText = title;
					break;
				}
			}
			xmlDocumentExtender.Save(configPath);
			return true;
		}
		//public static bool EditMenuItem(string mainMenuTitle, string subMenuTitle, string oldItemTitle, string newItemTitle, string link)
		//{
		//	int num = FindMenuID(mainMenuTitle, subMenuTitle, oldItemTitle);
		//	return num != -1 && EditMenuItem(num, newItemTitle, link);
		//}
		public static void DeleteMenuItem(int id)
		{
			XmlDocumentExtender xmlDocumentExtender = new XmlDocumentExtender();
			xmlDocumentExtender.Load(configPath);
			XmlNodeList xmlNodeList = xmlDocumentExtender.SelectNodes("/dataset/submain");
			string innerText = xmlNodeList.Item(id)["link"].InnerText;
			xmlNodeList.Item(id).ParentNode.RemoveChild(xmlNodeList.Item(id));
			XmlNodeList xmlNodeList2 = xmlDocumentExtender.SelectNodes("/dataset/shortcut");
			foreach (XmlNode xmlNode in xmlNodeList2)
			{
				if (xmlNode["link"].InnerText == innerText)
				{
					xmlNode.ParentNode.RemoveChild(xmlNode);
					break;
				}
			}
			xmlDocumentExtender.Save(configPath);
		}
		//public static bool DeleteMenuItem(string mainMenuTitle, string subMenuTitle, string menuItemTitle)
		//{
		//	int num = FindMenuID(mainMenuTitle, subMenuTitle, menuItemTitle);
		//	if (num == -1)
		//	{
		//		return false;
		//	}
		//	DeleteMenuItem(num);
		//	return true;
		//}
		//private static int FindMenuID(string mainMenuTitle)
		//{
		//	XmlDocumentExtender xmlDocumentExtender = new XmlDocumentExtender();
		//	xmlDocumentExtender.Load(configPath);
		//	XmlNodeList xmlNodeList = xmlDocumentExtender.SelectNodes("/dataset/toptabmenu");
		//	foreach (XmlNode xmlNode in xmlNodeList)
		//	{
		//		if (xmlNode["title"].InnerText == mainMenuTitle)
		//		{
		//			return int.Parse(xmlNode["id"].InnerText);
		//		}
		//	}
		//	return -1;
		//}
		//private static int FindMenuID(string mainMenuTitle, string subMenuTitle)
		//{
		//	XmlDocumentExtender xmlDocumentExtender = new XmlDocumentExtender();
		//	xmlDocumentExtender.Load(configPath);
		//	XmlNodeList xmlNodeList = xmlDocumentExtender.SelectNodes("/dataset/toptabmenu");
		//	bool flag = false;
		//	string str = "";
		//	foreach (XmlNode xmlNode in xmlNodeList)
		//	{
		//		if (xmlNode["title"].InnerText == mainMenuTitle)
		//		{
		//			str = xmlNode["mainmenulist"].InnerText;
		//			flag = true;
		//		}
		//	}
		//	if (!flag)
		//	{
		//		return -1;
		//	}
		//	XmlNodeList xmlNodeList2 = xmlDocumentExtender.SelectNodes("/dataset/mainmenu");
		//	foreach (XmlNode xmlNode2 in xmlNodeList2)
		//	{
		//		if (("," + str + ",").IndexOf("," + xmlNode2["id"].InnerText + ",") != -1 && xmlNode2["menutitle"].InnerText == subMenuTitle)
		//		{
		//			return int.Parse(xmlNode2["id"].InnerText);
		//		}
		//	}
		//	return -1;
		//}
		//private static int FindMenuID(string mainMenuTitle, string subMenuTitle, string menuItemTitle)
		//{
		//	XmlDocumentExtender xmlDocumentExtender = new XmlDocumentExtender();
		//	xmlDocumentExtender.Load(configPath);
		//	XmlNodeList xmlNodeList = xmlDocumentExtender.SelectNodes("/dataset/toptabmenu");
		//	bool flag = false;
		//	string str = "";
		//	foreach (XmlNode xmlNode in xmlNodeList)
		//	{
		//		if (xmlNode["title"].InnerText == mainMenuTitle)
		//		{
		//			str = xmlNode["mainmenulist"].InnerText;
		//			flag = true;
		//		}
		//	}
		//	if (!flag)
		//	{
		//		return -1;
		//	}
		//	XmlNodeList xmlNodeList2 = xmlDocumentExtender.SelectNodes("/dataset/mainmenu");
		//	flag = false;
		//	string b = "";
		//	foreach (XmlNode xmlNode2 in xmlNodeList2)
		//	{
		//		if (("," + str + ",").IndexOf("," + xmlNode2["id"].InnerText + ",") != -1 && xmlNode2["menutitle"].InnerText == subMenuTitle)
		//		{
		//			b = xmlNode2["menuid"].InnerText;
		//			flag = true;
		//		}
		//	}
		//	if (!flag)
		//	{
		//		return -1;
		//	}
		//	XmlNodeList xmlNodeList3 = xmlDocumentExtender.SelectNodes("/dataset/submain");
		//	int num = 0;
		//	foreach (XmlNode xmlNode3 in xmlNodeList3)
		//	{
		//		if (xmlNode3["menuparentid"].InnerText == b && xmlNode3["menutitle"].InnerText == menuItemTitle)
		//		{
		//			return num;
		//		}
		//		num++;
		//	}
		//	return -1;
		//}
		//private static int FindMenuMenuid(string mainMenuTitle, string subMenuTitle)
		//{
		//	XmlDocumentExtender xmlDocumentExtender = new XmlDocumentExtender();
		//	xmlDocumentExtender.Load(configPath);
		//	XmlNodeList xmlNodeList = xmlDocumentExtender.SelectNodes("/dataset/toptabmenu");
		//	bool flag = false;
		//	string str = "";
		//	foreach (XmlNode xmlNode in xmlNodeList)
		//	{
		//		if (xmlNode["title"].InnerText == mainMenuTitle)
		//		{
		//			str = xmlNode["mainmenulist"].InnerText;
		//			flag = true;
		//		}
		//	}
		//	if (!flag)
		//	{
		//		return -1;
		//	}
		//	XmlNodeList xmlNodeList2 = xmlDocumentExtender.SelectNodes("/dataset/mainmenu");
		//	foreach (XmlNode xmlNode2 in xmlNodeList2)
		//	{
		//		if (("," + str + ",").IndexOf("," + xmlNode2["id"].InnerText + ",") != -1 && xmlNode2["menutitle"].InnerText == subMenuTitle)
		//		{
		//			return int.Parse(xmlNode2["menuid"].InnerText);
		//		}
		//	}
		//	return -1;
		//}
		//public static bool FindMenu(string mainMenuTitle)
		//{
		//	return FindMenuID(mainMenuTitle) != -1;
		//}
		//public static bool FindMenu(string mainMenuTitle, string subMenuTitle)
		//{
		//	return FindMenuID(mainMenuTitle, subMenuTitle) != -1;
		//}
		//public static bool FindMenu(string mainMenuTitle, string subMenuTitle, string menuItemTitle)
		//{
		//	return FindMenuID(mainMenuTitle, subMenuTitle, menuItemTitle) != -1;
		//}
		//public static int FindPluginMainMenu()
		//{
		//	XmlDocumentExtender xmlDocumentExtender = new XmlDocumentExtender();
		//	xmlDocumentExtender.Load(configPath);
		//	XmlNodeList xmlNodeList = xmlDocumentExtender.SelectNodes("/dataset/toptabmenu");
		//	foreach (XmlNode xmlNode in xmlNodeList)
		//	{
		//		if (xmlNode["system"].InnerText == "2")
		//		{
		//			return int.Parse(xmlNode["id"].InnerText);
		//		}
		//	}
		//	return -1;
		//}
		public static void CreateMenuJson()
		{
			string mapPath = Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "admin/xml/navmenu.js");

			var dataSet = new DataSet();
			dataSet.ReadXml(configPath);

			var sb = new StringBuilder();
			sb.Append("var toptabmenu = ");
			sb.Append(Utils.DataTableToJSON(dataSet.Tables[2]));
			sb.Append("\r\nvar mainmenu = ");
			sb.Append(Utils.DataTableToJSON(dataSet.Tables[0]));
			sb.Append("\r\nvar submenu = ");
			sb.Append(Utils.DataTableToJSON(dataSet.Tables[1]));
			sb.Append("\r\nvar shortcut = ");
			if (dataSet.Tables.Count < 4)
			{
				sb.Append("[]");
			}
			else
			{
				sb.Append(Utils.DataTableToJSON(dataSet.Tables[3]));
			}
			WriteJsonFile(mapPath, sb);
			dataSet.Dispose();
		}
		private static void WriteJsonFile(string path, StringBuilder json_sb)
		{
			using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
			{
				byte[] bytes = Encoding.UTF8.GetBytes(json_sb.ToString());
				fileStream.Write(bytes, 0, bytes.Length);
				fileStream.Close();
			}
		}
		//public static void ImportPluginMenu(string menuConfigFile)
		//{
		//	if (!File.Exists(menuConfigFile))
		//	{
		//		return;
		//	}
		//	BackupMenuFile();
		//	XmlDocumentExtender xmlDocumentExtender = new XmlDocumentExtender();
		//	xmlDocumentExtender.Load(menuConfigFile);
		//	XmlNodeList xmlNodeList = xmlDocumentExtender.SelectNodes("/pluginmenu/menuitem");
		//	int mainmenuid = FindPluginMainMenu();
		//	foreach (XmlNode xmlNode in xmlNodeList)
		//	{
		//		int menuparentid = NewSubMenu(mainmenuid, xmlNode.Attributes["title"].InnerText);
		//		XmlNodeList childNodes = xmlNode.ChildNodes;
		//		foreach (XmlNode xmlNode2 in childNodes)
		//		{
		//			NewMenuItem(menuparentid, xmlNode2["title"].InnerText, xmlNode2["link"].InnerText);
		//		}
		//	}
		//	CreateMenuJson();
		//}
		public static void BackupMenuFile()
		{
			Utils.BackupFile(configPath, Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "admin/xml/backup/" + DateTime.Now.ToString("yyyy-MM-dd HH_mm_ss") + ".config"));
		}
	}
}