using BBX.Cache;
using BBX.Common;
using BBX.Data;
using System;
using System.Data;
namespace BBX.Forum
{
	public class Identifys
	{
		public static bool AddIdentify(string name, string fileName)
		{
			XCache.Remove("/Forum/TopicIdentifys");
			XCache.Remove("/Forum/TopicIndentifysJsArray");
			return BBX.Data.Identifys.AddIdentify(name, fileName);
		}
		public static DataTable GetAllIdentify()
		{
			return BBX.Data.Identifys.GetAllIdentify();
		}
		public static void DeleteIdentify(string idlist)
		{
			if (Utils.IsNumericList(idlist))
			{
				BBX.Data.Identifys.DeleteIdentify(idlist);
				XCache.Remove("/Forum/TopicIdentifys");
				XCache.Remove("/Forum/TopicIndentifysJsArray");
			}
		}
		public static bool UpdateIdentifyById(int id, string name)
		{
			return id > 0 && BBX.Data.Identifys.UpdateIdentifyById(id, name);
		}
	}
}
