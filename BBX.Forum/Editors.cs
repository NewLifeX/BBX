using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using BBX.Entity;

namespace BBX.Forum
{
	public class Editors
	{
		public static Regex[] regexCustomTag;
		static Editors()
		{
			InitRegexCustomTag();
		}
		public static void InitRegexCustomTag()
		{
			var list = GetCustomEditButtonListWithInfo();
			if (list != null)
			{
				ResetRegexCustomTag(list);
			}
		}
		public static void ResetRegexCustomTag(CustomEditorButtonInfo[] tagList)
		{
			int num = tagList.Length;
			if (regexCustomTag == null || num != regexCustomTag.Length)
			{
				regexCustomTag = new Regex[num];
			}
			var sb = new StringBuilder();
			for (int i = 0; i < num; i++)
			{
				if (sb.Length > 0)
				{
					sb.Remove(0, sb.Length);
				}
				sb.Append("(\\[");
				sb.Append(tagList[i].Tag);
				if (tagList[i].Params > 1)
				{
					sb.Append("=");
					for (int j = 2; j <= tagList[i].Params; j++)
					{
						sb.Append("(.*?)");
						if (j < tagList[i].Params)
						{
							sb.Append(",");
						}
					}
				}
				sb.Append("\\])([\\s\\S]+?)\\[\\/");
				sb.Append(tagList[i].Tag);
				sb.Append("\\]");
				regexCustomTag[i] = new Regex(sb.ToString(), RegexOptions.IgnoreCase);
			}
		}
		public static CustomEditorButtonInfo[] GetCustomEditButtonListWithInfo()
		{
			//var cacheService = XCache.Current;
			//CustomEditorButtonInfo[] array = cacheService.RetrieveObject(CacheKeys.FORUM_UI_CUSTOM_EDIT_BUTTON_INFO) as CustomEditorButtonInfo[];
			//if (array == null)
			//{
			//	array = BBX.Data.Editors.GetCustomEditButtonListWithInfo();
			//	XCache.Add(CacheKeys.FORUM_UI_CUSTOM_EDIT_BUTTON_INFO, array);
			//	Editors.ResetRegexCustomTag(array);
			//}
			//return array;

			return BbCode.FindAllByAvailable(1).ToList().Select(e => e.Cast<CustomEditorButtonInfo>()).ToArray();
		}
	}
}