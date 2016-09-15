using BBX.Cache;
using BBX.Common;
using BBX.Data;
using System;
using System.Collections;
using System.Data;
namespace BBX.Forum
{
	public class Medals
	{
		public static DataTable GetMedal()
		{
			return BBX.Data.Medals.GetMedal();
		}
		public static DataTable GetMedal(int medalId)
		{
			DataTable dataTable = Medals.GetMedal().Clone();
			foreach (DataRow dataRow in Medals.GetMedal().Rows)
			{
				if (dataRow["medalid"].ToString() == medalId.ToString())
				{
					dataTable.ImportRow(dataRow);
				}
			}
			return dataTable;
		}
		public static DataTable GetAvailableMedal()
		{
			DataTable medal = Medals.GetMedal();
			DataTable dataTable = medal.Clone();
			DataRow[] array = medal.Select("available=1");
			for (int i = 0; i < array.Length; i++)
			{
				DataRow row = array[i];
				dataTable.ImportRow(row);
			}
			return dataTable;
		}
		public static void CreateMedal(string medalName, int available, string image)
		{
			BBX.Data.Medals.CreateMedal(medalName, available, image);
			XCache.Remove(CacheKeys.FORUM_UI_MEDALS_LIST);
		}
		public static void UpdateMedal(int medalid, string name, string image)
		{
			if (medalid > 0)
			{
				BBX.Data.Medals.UpdateMedal(medalid, name, image);
			}
		}
		public static void InsertMedal(int medalid, string name, string image)
		{
			if (medalid > 0)
			{
				BBX.Data.Medals.InsertMedal(medalid, name, image);
			}
		}
		public static void SetAvailableForMedal(int available, string medailIdList)
		{
			if (Utils.IsNumericList(medailIdList))
			{
				BBX.Data.Medals.SetAvailableForMedal(available, medailIdList);
				XCache.Remove(CacheKeys.FORUM_UI_MEDALS_LIST);
			}
		}
		public static void InsertMedalList(ArrayList medalFiles)
		{
			medalFiles.Remove("thumbs.db");
			DataTable existMedalList = BBX.Data.Medals.GetExistMedalList();
			foreach (DataRow dataRow in existMedalList.Rows)
			{
				medalFiles.Remove(dataRow["image"].ToString().ToLower());
			}
			int num = TypeConverter.ObjectToInt(existMedalList.Rows[existMedalList.Rows.Count - 1]["medalid"]) + 1;
			for (int i = 0; i < medalFiles.Count; i++)
			{
				int num2 = num + i;
				Medals.InsertMedal(num2, "Medal No." + num2, medalFiles[i].ToString());
			}
			XCache.Remove(CacheKeys.FORUM_UI_MEDALS_LIST);
		}
	}
}
