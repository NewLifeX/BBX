using System;
using System.Data;
using System.Text;
using BBX.Common;
using BBX.Entity;

namespace BBX.Forum
{
    public class UserGroups
    {
        private static DataTable CreateGroupScoreTable()
        {
            DataTable dataTable = new DataTable("templateDT");
            dataTable.Columns.Add("id", typeof(Int32));
            dataTable.Columns.Add("available", typeof(Boolean));
            dataTable.Columns.Add("ScoreCode", typeof(Int32));
            dataTable.Columns.Add("ScoreName", typeof(String));
            dataTable.Columns.Add("Min", typeof(String));
            dataTable.Columns.Add("Max", typeof(String));
            dataTable.Columns.Add("MaxInDay", typeof(String));
            dataTable.Columns.Add("Options", typeof(String));
            for (int i = 0; i < 8; i++)
            {
                DataRow dataRow = dataTable.NewRow();
                dataRow["id"] = i + 1;
                dataRow["available"] = false;
                dataRow["ScoreCode"] = i + 1;
                dataRow["ScoreName"] = "";
                dataRow["Min"] = "";
                dataRow["Max"] = "";
                dataRow["MaxInDay"] = "";
                dataTable.Rows.Add(dataRow);
            }
            DataRow dataRow2 = Scoresets.GetScoreSet().Rows[0];
            for (int j = 0; j < 8; j++)
            {
                if (!Utils.StrIsNullOrEmpty(dataRow2[j + 2].ToString()) && dataRow2[j + 2].ToString().Trim() != "0")
                {
                    dataTable.Rows[j]["ScoreName"] = dataRow2[j + 2].ToString().Trim();
                }
            }
            return dataTable;
        }

        public static DataTable GroupParticipateScore(int groupid)
        {
            var ug = UserGroup.FindByID(groupid);
            //var userGroupRateRange = BBX.Data.UserGroups.GetUserGroupRateRange(groupid);
            if (ug == null) return null;

            var dataTable = CreateGroupScoreTable();
            if (ug.Raterange.IsNullOrEmpty()) return dataTable;

            int num = 0;
            string[] array = ug.Raterange.Trim().Split('|');
            for (int i = 0; i < array.Length; i++)
            {
                string text = array[i];
                if (!text.IsNullOrEmpty())
                {
                    string[] array2 = text.Split(',');
                    if (array2[1].Trim() == "True")
                    {
                        dataTable.Rows[num]["available"] = true;
                    }
                    dataTable.Rows[num]["Min"] = array2[4].Trim();
                    dataTable.Rows[num]["Max"] = array2[5].Trim();
                    dataTable.Rows[num]["MaxInDay"] = array2[6].Trim();
                }
                num++;
            }
            return dataTable;
        }

        public static DataTable GroupParticipateScore(int uid, int gid)
        {
            var dataTable = GroupParticipateScore(gid);
            int[] array = new int[9];
            var stringBuilder = new StringBuilder();
            if (dataTable != null)
            {
                int[] array2 = RateLog.GroupParticipateScore(uid);
                for (int i = dataTable.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dataRow = dataTable.Rows[i];
                    int num = dataRow["Max"].ToInt();
                    int num2 = dataRow["Min"].ToInt();
                    int num3 = dataRow["MaxInDay"].ToInt();
                    num3 -= array2[dataRow["ScoreCode"].ToInt()];
                    dataRow["MaxInDay"] = num3;
                    int num4 = (num > num3) ? num3 : num;
                    if (!Convert.ToBoolean(dataRow["available"]) || num3 <= 0)
                    {
                        dataRow.Delete();
                    }
                    else
                    {
                        int num5 = (Int32)Math.Abs(Math.Ceiling((double)(num4 - num2) / 10.0));
                        num5 = ((num5 <= 0) ? 1 : num5);
                        stringBuilder.Remove(0, stringBuilder.Length);
                        int num6 = num4;
                        while (num6 >= dataRow["Min"].ToDouble())
                        {
                            if (num6 != 0 && Math.Abs(num6) <= num3)
                            {
                                stringBuilder.AppendFormat("\n<li>{0}{1}</li>", (num6 > 0) ? "+" : "", num6);
                            }
                            num6 -= num5;
                        }
                        dataRow["Options"] = stringBuilder.ToString();
                    }
                }
                dataTable.AcceptChanges();
            }
            if (dataTable == null) dataTable = new DataTable();

            return dataTable;
        }

        //public static int CheckUserGroupMaxPrice(UserGroup usergroupinfo, int price)
        //{
        //	if (price <= 0) return 0;

        //	if (price > 0 && price <= usergroupinfo.MaxPrice) return price;

        //	return usergroupinfo.MaxPrice;
        //}

        //public static int GetMaxUserGroupId()
        //{
        //    var userGroupList = UserGroup.FindAllWithCache();
        //    int num = 0; 
        //    foreach (var current in userGroupList)
        //    {
        //        if (current.ID > num)
        //        {
        //            num = current.ID;
        //        }
        //    }
        //    return num;
        //}

        //public static void UpdateUserGroupRaterange(string raterange, int groupid)
        //{
        //	BBX.Data.UserGroups.UpdateUserGroupRaterange(raterange, groupid);
        //}

        //public static void ChangeAllUserGroupId(int sourceGroupId, int targetGroupId)
        //{
        //	BBX.Data.UserGroups.ChangeAllUserGroupId(sourceGroupId, targetGroupId);
        //}

        //public static DataTable GetUserGroupExceptGroupid(int groupid)
        //{
        //    return BBX.Data.UserGroups.GetUserGroupExceptGroupid(groupid);
        //}

        //public static DataTable GetAdminGroups()
        //{
        //    return BBX.Data.UserGroups.GetAdminGroups();
        //}

        //public static DataTable GetOnlineList()
        //{
        //    return BBX.Data.UserGroups.GetOnlineList();
        //}

        //public static int UpdateOnlineList(int groupid, int displayorder, string img, string title)
        //{
        //    return BBX.Data.UserGroups.UpdateOnlineList(groupid, displayorder, img, title);
        //}

        //public static DataTable GetRateRange(int scoreid)
        //{
        //	return BBX.Data.UserGroups.GetRateRange(scoreid);
        //}

        //public static void UpdateRateRange(string raterange, int groupid)
        //{
        //	BBX.Data.UserGroups.UpdateRateRange(raterange, groupid);
        //}

        //public static DataTable GetUserGroupWithOutGuestTitle()
        //{
        //    return BBX.Data.UserGroups.GetUserGroupWithOutGuestTitle();
        //}

        //public static DataTable GetUserGroupForDataTable()
        //{
        //    //return BBX.Data.UserGroups.GetUserGroupForDataTable();
        //    return UserGroup.FindAllWithCache().Sort(UserGroup._.ID.Name, false).ToDataTable(false);
        //}

        //public static DataTable GetCreditUserGroup()
        //{
        //    return BBX.Data.UserGroups.GetCreditUserGroup();
        //}

        //public static DataTable GetSpecialUserGroup()
        //{
        //    return BBX.Data.UserGroups.GetSpecialUserGroup();
        //}
    }
}