using System;
using System.Data;
using BBX.Common;
using BBX.Entity;

namespace BBX.Forum
{
    public class AdminUserGroups : UserGroups
    {
        public static string opresult = "";

        //public static UserGroupInfo AdminGetUserGroupInfo(int groupid)
        //{
        //    return UserGroup.FindByID(groupid);
        //}

        //public static AdminGroup AdminGetAdminGroupInfo(int groupid)
        //{
        //    //return AdminGroups.GetAdminGroupInfo(groupid);
        //    return AdminGroup.FindByID(groupid);
        //}

        public static bool AddUserGroupInfo(UserGroup userGroupInfo)
        {
            bool result;
            try
            {
                int creditshigher = userGroupInfo.Creditshigher;
                int creditslower = userGroupInfo.Creditslower;
                DataTable userGroupByCreditsHigherAndLower = BBX.Data.UserGroups.GetUserGroupByCreditsHigherAndLower(creditshigher, creditslower);
                if (userGroupByCreditsHigherAndLower.Rows.Count > 0)
                {
                    result = false;
                }
                else
                {
                    if (!userGroupInfo.Is管理团队 && !SystemCheckCredits("add", ref creditshigher, ref creditslower, 0))
                    {
                        result = false;
                    }
                    else
                    {
                        userGroupInfo.Creditshigher = creditshigher;
                        userGroupInfo.Creditslower = creditslower;
                        //BBX.Data.UserGroups.CreateUserGroup(userGroupInfo);
                        userGroupInfo.Save();
                        //BBX.Data.OnlineUsers.AddOnlineList(userGroupInfo.GroupTitle);
                        OnlineList.Add(userGroupInfo.ID, userGroupInfo.GroupTitle);
                        //Caches.ReSetAdminGroupList();
                        Caches.ReSetUserGroupList();
                        result = true;
                    }
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public static bool SystemCheckCredits(string opname, ref int creditsHigher, ref int creditsLower, int groupid)
        {
            opresult = "";
            string a;
            if ((a = opname.ToLower()) != null)
            {
                if (!(a == "add"))
                {
                    if (!(a == "delete"))
                    {
                        if (a == "update")
                        {
                            var userGroupInfo = UserGroup.FindByID(groupid);
                            int creditshigher = userGroupInfo.Creditshigher;
                            int creditslower = userGroupInfo.Creditslower;
                            DataTable dataTable = BBX.Data.UserGroups.GetMinCreditHigher();
                            if (dataTable.Rows.Count > 0)
                            {
                                int num = Convert.ToInt32(dataTable.Rows[0][0].ToString());
                                if (creditsLower <= num)
                                {
                                    creditsLower = num;
                                    opresult = "由您所输入的积分下限小于或等于系统最大值,因此系统已将其调整为" + num;
                                    BBX.Data.UserGroups.UpdateUserGroupsCreditsHigherByCreditsHigher(creditshigher, creditslower);
                                    return true;
                                }
                            }
                            dataTable = BBX.Data.UserGroups.GetMaxCreditLower();
                            if (dataTable.Rows.Count > 0)
                            {
                                int num2 = Convert.ToInt32(dataTable.Rows[0][0].ToString());
                                if (creditsHigher >= num2)
                                {
                                    creditsHigher = num2;
                                    opresult = "由您所输入的积分上限大于或等于系统最大值,因此系统已将其调整为" + num2;
                                    BBX.Data.UserGroups.UpdateUserGroupsCreditsLowerByCreditsLower(creditslower, creditshigher);
                                    return true;
                                }
                            }
                            dataTable = BBX.Data.UserGroups.GetUserGroupByCreditshigher(creditsHigher);
                            if (dataTable.Rows.Count <= 0)
                            {
                                opresult = "系统未提到合适的位置保存您提交的信息!";
                                return false;
                            }
                            Convert.ToInt32(dataTable.Rows[0][0].ToString());
                            int num3 = Convert.ToInt32(dataTable.Rows[0][1].ToString());
                            int num4 = Convert.ToInt32(dataTable.Rows[0][2].ToString());
                            if (creditsLower > num4)
                            {
                                opresult = "由您所输入的积分上限大于或等于所属有效积分上限的最大值" + num4 + ",因此系统无效提交您的数据!";
                                return false;
                            }
                            if (creditsHigher == num3)
                            {
                                if (creditsLower < num4)
                                {
                                    BBX.Data.UserGroups.UpdateUserGroupsCreditsHigherByCreditsHigher(creditsLower, num4);
                                }
                            }
                            else
                            {
                                opresult = "系统已自动将您提交的积分上限调整为" + num4;
                                BBX.Data.UserGroups.UpdateUserGroupsCreditsHigherByCreditsHigher(creditsLower, num4);
                                BBX.Data.UserGroups.UpdateUserGroupsCreditsLowerByCreditsLower(creditsHigher, num3);
                            }
                        }
                    }
                    else
                    {
                        if (BBX.Data.UserGroups.GetGroupCountByCreditsLower(creditsHigher) > 0)
                        {
                            BBX.Data.UserGroups.UpdateUserGroupsCreditsLowerByCreditsLower(creditsLower, creditsHigher);
                        }
                        else
                        {
                            BBX.Data.UserGroups.UpdateUserGroupsCreditsHigherByCreditsHigher(creditsHigher, creditsLower);
                        }
                    }
                }
                else
                {
                    DataTable dataTable2 = BBX.Data.UserGroups.GetMinCreditHigher();
                    if (dataTable2.Rows.Count > 0)
                    {
                        int num5 = Convert.ToInt32(dataTable2.Rows[0][0].ToString());
                        if (creditsLower <= num5)
                        {
                            creditsLower = num5;
                            opresult = "由您所输入的积分下限小于或等于系统最小值,因此系统已将其调整为" + num5;
                            return true;
                        }
                    }
                    dataTable2 = BBX.Data.UserGroups.GetMaxCreditLower();
                    if (dataTable2.Rows.Count > 0)
                    {
                        int num6 = Convert.ToInt32(dataTable2.Rows[0][0].ToString());
                        if (creditsHigher >= num6)
                        {
                            creditsHigher = num6;
                            opresult = "由您所输入的积分上限大于或等于系统最大值,因此系统已将其调整为" + num6;
                            return true;
                        }
                    }
                    dataTable2 = BBX.Data.UserGroups.GetUserGroupByCreditshigher(creditsHigher);
                    if (dataTable2.Rows.Count <= 0)
                    {
                        opresult = "系统未提到合适的位置保存您提交的信息!";
                        return false;
                    }
                    int groupid2 = Convert.ToInt32(dataTable2.Rows[0][0].ToString());
                    int num7 = Convert.ToInt32(dataTable2.Rows[0][1].ToString());
                    int num8 = Convert.ToInt32(dataTable2.Rows[0][2].ToString());
                    if (creditsLower > num8)
                    {
                        return false;
                    }
                    if (creditsHigher == num7)
                    {
                        if (creditsLower >= num8)
                        {
                            opresult = "由您所输入的积分上限大于或等于所属有效积分上限的最大值" + num8 + ",因此系统无效提交您的数据!";
                            return false;
                        }
                        var userGroupInfo2 = UserGroup.FindByID(groupid2);
                        userGroupInfo2.Creditshigher = creditsLower;
                        //UserGroups.UpdateUserGroup(userGroupInfo2);
                        userGroupInfo2.Save();
                    }
                    else
                    {
                        creditsLower = num8;
                        BBX.Data.UserGroups.UpdateUserGroupCreidtsLower(num7, creditsHigher);
                    }
                }
            }
            return true;
        }

        //public static bool UpdateUserGroupInfo(UserGroup userGroupInfo)
        //{
        //    int creditshigher = userGroupInfo.Creditshigher;
        //    int creditslower = userGroupInfo.Creditslower;
        //    if (userGroupInfo.ID >= 9 && !userGroupInfo.Is管理团队)
        //    {
        //        DataTable userGroupByCreditsHigherAndLower = BBX.Data.UserGroups.GetUserGroupByCreditsHigherAndLower(creditshigher, creditslower);
        //        if (userGroupByCreditsHigherAndLower.Rows.Count > 0 && userGroupInfo.ID.ToString() != userGroupByCreditsHigherAndLower.Rows[0][0].ToString())
        //        {
        //            return false;
        //        }
        //        if (!SystemCheckCredits("update", ref creditshigher, ref creditslower, userGroupInfo.ID))
        //        {
        //            return false;
        //        }
        //    }
        //    //UserGroups.UpdateUserGroup(userGroupInfo);
        //    userGroupInfo.Save();
        //    //BBX.Data.UserGroups.UpdateOnlineList(userGroupInfo);
        //    throw new NotImplementedException("BBX.Data.UserGroups.UpdateOnlineList");
        //    //Caches.ReSetAdminGroupList();
        //    Caches.ReSetUserGroupList();
        //    return true;
        //}

        //public static bool DeleteUserGroupInfo(int groupid)
        //{
        //    bool result;
        //    try
        //    {
        //        if (BBX.Data.UserGroups.IsSystemOrTemplateUserGroup(groupid))
        //        {
        //            result = false;
        //        }
        //        else
        //        {
        //            var userGroupInfo = UserGroup.FindByID(groupid);
        //            if (groupid >= 9)
        //            {
        //                DataTable userGroupExceptGroupid = UserGroups.GetUserGroupExceptGroupid(groupid);
        //                if (userGroupExceptGroupid.Rows.Count > 1)
        //                {
        //                    if (!userGroupInfo.Is管理团队)
        //                    {
        //                        int creditshigher = userGroupInfo.Creditshigher;
        //                        int creditslower = userGroupInfo.Creditslower;
        //                        SystemCheckCredits("delete", ref creditshigher, ref creditslower, groupid);
        //                    }
        //                }
        //                else
        //                {
        //                    if (userGroupExceptGroupid.Rows.Count != 1)
        //                    {
        //                        opresult = "当前用户组为系统中唯一的用户组,因此系统无法删除";
        //                        result = false;
        //                        return result;
        //                    }
        //                    BBX.Data.UserGroups.UpdateUserGroupLowerAndHigherToLimit(Utils.StrToInt(userGroupExceptGroupid.Rows[0][0], 0));
        //                }
        //            }
        //            //UserGroups.DeleteUserGroupInfo(groupid);
        //            userGroupInfo.Delete();
        //            //AdminGroups.DeleteAdminGroupInfo(short.Parse(groupid.ToString()));
        //            var adg = AdminGroup.FindByID(groupid);
        //            if (adg != null) adg.Delete();
        //            //BBX.Data.OnlineUsers.DeleteOnlineByUserGroup(groupid);
        //            Online.DeleteByUserGroup(groupid);
        //            //Caches.ReSetAdminGroupList();
        //            Caches.ReSetUserGroupList();
        //            result = true;
        //        }
        //    }
        //    catch
        //    {
        //        result = false;
        //    }
        //    return result;
        //}
    }
}