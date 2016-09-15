using BBX.Common;
using BBX.Data;
using BBX.Entity;
using System;
using System.Data;

namespace BBX.Forum
{
	public class Favorites
	{
		//public static int CreateFavorites(int uid, int tid)
		//{
		//	if (uid < 0)
		//	{
		//		return 0;
		//	}
		//	return Favorites.CreateFavorites(uid, tid, FavoriteType.ForumTopic);
		//}
		public static int CreateFavorites(int uid, int tid, FavoriteType type)
		{
			return BBX.Data.Favorites.CreateFavorites(uid, tid, (byte)type);
		}
		public static int DeleteFavorites(int uid, string[] fitemid, FavoriteType type)
		{
			for (int i = 0; i < fitemid.Length; i++)
			{
				string expression = fitemid[i];
				if (!Utils.IsNumeric(expression))
				{
					return -1;
				}
			}
			return BBX.Data.Favorites.DeleteFavorites(uid, string.Join(",", fitemid), (byte)type);
		}
		public static DataTable GetFavoritesList(int uid, int pagesize, int pageindex, FavoriteType type)
		{
			return BBX.Data.Favorites.GetFavoritesList(uid, pagesize, pageindex, (int)type);
		}
		public static int GetFavoritesCount(int uid, FavoriteType type)
		{
			if (uid <= 0)
			{
				return 0;
			}
			return BBX.Data.Favorites.GetFavoritesCount(uid, (int)type);
		}
		public static int CheckFavoritesIsIN(int uid, int tid, FavoriteType type)
		{
			return BBX.Data.Favorites.CheckFavoritesIsIN(uid, tid, (byte)type);
		}
		public static int UpdateUserFavoriteViewTime(int uid, int tid)
		{
			return BBX.Data.Favorites.UpdateUserFavoriteViewTime(uid, tid);
		}
	}
}
