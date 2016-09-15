using Discuz.Cache;
using Discuz.Data;
using System;
using System.Data;
using System.Text.RegularExpressions;
namespace Discuz.Forum
{
	public class ForumLinks
	{
		public static int CreateForumLink(int displayOrder, string name, string url, string note, string logo)
		{
			DNTCache.Current.RemoveObject(CacheKeys.FORUM_FORUM_LINK_LIST);
			return Discuz.Data.ForumLinks.CreateForumLink(displayOrder, name, url, note, logo);
		}
		public static DataTable GetForumLinks()
		{
			return Discuz.Data.ForumLinks.GetForumLinks();
		}
		public static int UpdateForumLink(int id, int displayorder, string name, string url, string note, string logo)
		{
			Regex regex = new Regex("(http|https)://([\\w-]+\\.)+[\\w-]+(/[\\w-./?%&=]*)?");
			if (name == "" || !regex.IsMatch(url.Replace("'", "''")))
			{
				return -1;
			}
			return Discuz.Data.ForumLinks.UpdateForumLink(id, displayorder, name, url, note, logo);
		}
		public static int DeleteForumLink(string forumlinkidlist)
		{
			DNTCache.Current.RemoveObject(CacheKeys.FORUM_FORUM_LINK_LIST);
			return Discuz.Data.ForumLinks.DeleteForumLink(forumlinkidlist);
		}
	}
}
