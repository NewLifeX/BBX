using Discuz.Cache;
using Discuz.Common;
using Discuz.Config;
using Discuz.Data;
using Discuz.Entity;
using System;
using System.Data;
using System.IO;
using System.Threading;
using System.Xml;
namespace Discuz.Forum
{
	public class Templates
	{
		private static object SynObject = new object();
		public static DataTable GetTemplateVariable1(string templatename)
		{
			string mapPath = Utils.GetMapPath("../../templates/" + templatename + "/templatevariable.xml");
			if (!File.Exists(mapPath))
			{
				return null;
			}
			DataTable result;
			using (DataSet dataSet = new DataSet())
			{
				dataSet.ReadXml(mapPath);
				result = dataSet.Tables[0];
			}
			return result;
		}
		public static int GetTemplateWidth(string templatePath)
		{
			var cacheService = XCache.Current;
			string text = cacheService.RetrieveObject("/Forum/TemplateWidth/" + templatePath) as string;
			if (text == null)
			{
				text = Templates.GetTemplateAboutInfo(Utils.GetMapPath(BaseConfigs.GetForumPath + "templates/" + templatePath + "/")).width;
				XCache.Add("/Forum/TemplateWidth/" + templatePath, text);
			}
			return TypeConverter.StrToInt(text);
		}
		public static TemplateAboutInfo GetTemplateAboutInfo(string xmlPath)
		{
			TemplateAboutInfo templateAboutInfo = new TemplateAboutInfo();
			if (!File.Exists(xmlPath + "\\about.xml"))
			{
				return templateAboutInfo;
			}
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(xmlPath + "\\about.xml");
			try
			{
				XmlNode xmlNode = xmlDocument.SelectSingleNode("about");
				foreach (XmlNode xmlNode2 in xmlNode.ChildNodes)
				{
					if (xmlNode2.NodeType != XmlNodeType.Comment && xmlNode2.Name.ToLower() == "template")
					{
						templateAboutInfo.name = ((xmlNode2.Attributes["name"] != null) ? xmlNode2.Attributes["name"].Value.ToString() : "");
						templateAboutInfo.author = ((xmlNode2.Attributes["author"] != null) ? xmlNode2.Attributes["author"].Value.ToString() : "");
						templateAboutInfo.createdate = ((xmlNode2.Attributes["createdate"] != null) ? xmlNode2.Attributes["createdate"].Value.ToString() : "");
						templateAboutInfo.ver = ((xmlNode2.Attributes["ver"] != null) ? xmlNode2.Attributes["ver"].Value.ToString() : "");
						templateAboutInfo.fordntver = ((xmlNode2.Attributes["fordntver"] != null) ? xmlNode2.Attributes["fordntver"].Value.ToString() : "");
						templateAboutInfo.copyright = ((xmlNode2.Attributes["copyright"] != null) ? xmlNode2.Attributes["copyright"].Value.ToString() : "");
						templateAboutInfo.width = ((xmlNode2.Attributes["width"] != null) ? xmlNode2.Attributes["width"].Value.ToString() : "600");
					}
				}
			}
			catch
			{
				templateAboutInfo = new TemplateAboutInfo();
			}
			return templateAboutInfo;
		}
		public static DataTable GetValidTemplateList()
		{
			DataTable result;
			lock(Templates.SynObject)
			{
				var cacheService = XCache.Current;
				DataTable dataTable = cacheService.RetrieveObject("/Forum/TemplateList") as DataTable;
				if (dataTable == null)
				{
					dataTable = Discuz.Data.Templates.GetValidTemplateList();
					XCache.Add("/Forum/TemplateList", dataTable);
				}
				result = dataTable;
			}
			return result;
		}
		public static string GetValidTemplateIDList()
		{
			string result;
			lock(Templates.SynObject)
			{
				var cacheService = XCache.Current;
				string text = cacheService.RetrieveObject(CacheKeys.FORUM_TEMPLATE_ID_LIST) as string;
				if (text == null)
				{
					text = Discuz.Data.Templates.GetValidTemplateIDList();
					if (!Utils.StrIsNullOrEmpty(text))
					{
						text = text.Substring(1);
					}
					XCache.Add(CacheKeys.FORUM_TEMPLATE_ID_LIST, text);
				}
				result = text;
			}
			return result;
		}
		public static TemplateInfo GetTemplateItem(int templateid)
		{
			if (templateid <= 0)
			{
				return null;
			}
			TemplateInfo templateInfo = null;
			DataRow[] array = Templates.GetValidTemplateList().Select("templateid = " + templateid.ToString());
			if (array.Length > 0)
			{
				templateInfo = new TemplateInfo();
				templateInfo.Templateid = (int)short.Parse(array[0]["templateid"].ToString());
				templateInfo.Name = array[0]["name"].ToString();
				templateInfo.Directory = array[0]["directory"].ToString();
				templateInfo.Copyright = array[0]["copyright"].ToString();
				templateInfo.Templateurl = array[0]["templateurl"].ToString();
			}
			if (templateInfo == null)
			{
				array = Templates.GetValidTemplateList().Select("templateid = 1");
				if (array.Length > 0)
				{
					templateInfo = new TemplateInfo();
					templateInfo.Templateid = (int)short.Parse(array[0]["templateid"].ToString());
					templateInfo.Name = array[0]["name"].ToString();
					templateInfo.Directory = array[0]["directory"].ToString();
					templateInfo.Copyright = array[0]["copyright"].ToString();
					templateInfo.Templateurl = array[0]["templateurl"].ToString();
				}
			}
			return templateInfo;
		}
		public static void CreateTemplate(string name, string directory, string copyright, string author, string createdate, string ver, string fordntver)
		{
			Discuz.Data.Templates.CreateTemplate(name, directory, copyright, author, createdate, ver, fordntver);
		}
	}
}
