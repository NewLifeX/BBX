﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using NewLife.Log;
using XCode;

namespace BBX.Entity
{
	/// <summary>用户组类型</summary>
	public enum UserGroupKinds
	{
		管理员 = 1,
		超级版主 = 2,
		版主 = 3,
		禁止发言 = 4,
		禁止访问 = 5,
		禁止IP = 6,
		游客 = 7,
		等待验证会员 = 8,
		乞丐 = 9,
		新手上路 = 10,
		注册会员 = 11,
		中级会员 = 12,
		高级会员 = 13,
		金牌会员 = 14,
		论坛元老 = 15
	}

	/// <summary>用户组</summary>
	public partial class UserGroup : EntityBase<UserGroup>
	{
		#region 对象操作﻿
		/// <summary>验证数据，通过抛出异常的方式提示验证失败。</summary>
		/// <param name="isNew"></param>
		public override void Valid(Boolean isNew)
		{
			// 这里验证参数范围，建议抛出参数异常，指定参数名，前端用户界面可以捕获参数异常并聚焦到对应的参数输入框
			//if (String.IsNullOrEmpty(Name)) throw new ArgumentNullException(_.Name, _.Name.DisplayName + "无效！");
			//if (!isNew && ID < 1) throw new ArgumentOutOfRangeException(_.ID, _.ID.DisplayName + "必须大于0！");

			// 建议先调用基类方法，基类方法会对唯一索引的数据进行验证
			base.Valid(isNew);

			// 在新插入数据或者修改了指定字段时进行唯一性验证，CheckExist内部抛出参数异常
			//if (isNew || Dirtys[_.Name]) CheckExist(_.Name);

		}

		/// <summary>首次连接数据库时初始化数据，仅用于实体类重载，用户不应该调用该方法</summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		protected override void InitData()
		{
			base.InitData();

			// InitData一般用于当数据表没有数据时添加一些默认数据，该实体类的任何第一次数据库操作都会触发该方法，默认异步调用
			// Meta.Count是快速取得表记录数
			if (Meta.Count > 0) return;

			// 需要注意的是，如果该方法调用了其它实体类的首次数据库操作，目标实体类的数据初始化将会在同一个线程完成
			if (XTrace.Debug) XTrace.WriteLine("开始初始化{0}用户组数据……", typeof(UserGroup).Name);

			#region 用户组默认参数
			Object[][] ps ={
new Object[]{1, 0, 1, "管理员",		0, 0, 9,"" , "", 255,		1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 3, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 30, 200, 500, 99999999,99999999, "", "1,True,extcredits1,威望,-50,50,300|2,False,extcredits2,金钱,-50,50,300|3,False,extcredits3,,,,|4,False,extcredits4,,,,|5,False,extcredits5,,,,|6,False,extcredits6,,,,|7,False,extcredits7,,,,|8,False,extcredits8,,,,", 1,99999999, 99999999, 1, 1, 1, 100, 1, 1},
new Object[]{2, 0, 1, "超级版主",	0, 0, 8, "", "", 255,		1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 3, 0, 0, 0, 0, 1, 1, 1, 1, 0, 1, 0, 1, 1, 1, 1, 1, 3, 20, 120, 300, 99999999, 99999999, "", "1,True,extcredits1,威望,-50,50,100|2,False,extcredits2,金钱,-30,30,50|3,False,extcredits3,,,,|4,False,extcredits4,,,,|5,False,extcredits5,,,,|6,False,extcredits6,,,,|7,False,extcredits7,,,,|8,False,extcredits8,,,,", 1, 99999999, 99999999, 1, 1, 1, 90, 1, 1},
new Object[]{3, 0, 1, "版主",		0, 0, 7, "", "", 200,		1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 3, 0, 0, 0, 0, 1, 1, 1, 1, 0, 1, 0, 1, 1, 1, 1, 1, 3, 10, 80, 200, 4194304, 33554432, "", "1,True,extcredits1,威望,-30,30,50|2,False,extcredits2,金钱,-10,10,30|3,False,extcredits3,,,,|4,False,extcredits4,,,,|5,False,extcredits5,,,,|6,False,extcredits6,,,,|7,False,extcredits7,,,,|8,False,extcredits8,,,,", 1, 33554432, 33554432, 1, 1, 1, 80, 1, 1},
new Object[]{0, 0, 1, "禁止发言",	0, 0, 0,"", "", 0,			1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, "", "", 0, 0, 0, 0, 0, 0, 0, 0, 1},
new Object[]{0, 0, 1, "禁止访问",	0, 0, 0, "", "", 0,			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "", "", 0, 0, 0, 0, 0, 0, 0, 0, 0},
new Object[]{0, 0, 1, "禁止 IP",	0, 0, 0, "", "", 0,			0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "", "", 0, 0, 0, 0, 0, 0, 0, 0, 0},
new Object[]{0, 0, 1, "游客",		0, 0, 0, "", "", 1,			1, 0, 0, 0, 0, 1, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, "", "", 0, 0, 0, 0, 0, 0, 0, 0, 0},
new Object[]{0, 0, 1, "等待验证会员", 0, 0, 0, "", "", 0,		1, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 50, 0, 0, "", "", 0, 0, 0, 0, 0, 0, 0, 0, 0},
new Object[]{0, 0, 0, "乞丐", -9999999, 0, 0, "", "", 0,		1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, "", "", 0, 0, 0, 0, 0, 0, 0, 0, 0},
new Object[]{0, 0, 0, "新手上路", 0, 50, 1, "", "", 10,			1, 1, 1, 0, 0, 1, 1, 1, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 1, 1, 0, 0, 0, 0, 20, 80, 524288, 1048576, "", "", 1, 1048576, 1048576, 0, 0, 0, 0, 0, 1},
new Object[]{0, 0, 0, "注册会员", 50, 200, 2, "", "", 20,		1, 1, 1, 1, 1, 1, 1, 1, 0, 2, 1, 0, 0, 0, 0, 0, 0, 1, 1, 0, 1, 0, 1, 0, 1, 1, 0, 3, 0, 30, 100, 1048576, 2097152, "", "", 1, 2097152, 2097152, 1, 1, 1, 20, 0, 1},
new Object[]{0, 0, 0, "中级会员", 200, 500, 3, "", "", 30,		1, 1, 1, 1, 1, 1, 1, 1, 0, 2, 2, 0, 0, 0, 0, 0, 0, 1, 1, 0, 1, 0, 1, 0, 1, 1, 0, 3, 0, 50, 150, 2097152, 4194304, "", "", 1, 4194304, 4194304, 1, 1, 1, 30, 1, 1},
new Object[]{0, 0, 0, "高级会员", 500, 1000, 4, "", "", 50,		1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 3, 0, 0, 0, 0, 0, 0, 1, 1, 0, 1, 0, 1, 0, 1, 1, 0, 3, 0, 60, 200, 4194304, 8388608, "", "", 1, 8388608, 8388608, 1, 1, 1, 50, 1, 1},
new Object[]{0, 0, 0, "金牌会员", 1000, 3000, 6, "", "", 70,	1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 3, 0, 0, 0, 0, 1, 1, 1, 1, 0, 1, 0, 1, 1, 1, 1, 0, 3, 20, 80, 300, 4194304, 16777216, "", "", 1, 16777216, 16777216, 1, 1, 1, 60, 1, 1},
new Object[]{0, 0, 0, "论坛元老", 3000, 9999999, 8, "", "", 100,1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 3, 0, 0, 0, 0, 1, 1, 1, 1, 0, 1, 0, 1, 1, 1, 1, 0, 3, 0, 100, 500, 4194304, 33554432, "", "", 1, 33554432, 33554432, 1, 1, 1, 70, 1, 1},
        };
			#endregion

			// 批量添加用户组
			foreach (var item in Enum.GetNames(typeof(UserGroupKinds)))
			{
				var v = (Int32)Enum.Parse(typeof(UserGroupKinds), item);

				var entity = new UserGroup();
				entity.GroupTitle = item;

				if (v <= 3) entity.RadminID = v;
				if (v > 1 && v < 9) entity.System = 1;

				if (v <= 3)
					entity.Stars = 10 - v;
				else if (v > 9)
					entity.Stars = v - 9;

				// 设置其它参数
				var pis = ps[v - 1];
                var fs = Meta.FieldNames.ToArray();
				for (int i = 4; i < pis.Length; i++)
				{
					// 要排除掉ID和前面已经复制的4项
                    var name = fs[i + 1];
					//XTrace.WriteLine("{0}={1}", name, pis[i]);
					entity[name] = pis[i];
				}

				entity.Save();
			}

			if (XTrace.Debug) XTrace.WriteLine("完成初始化{0}用户组数据！", typeof(UserGroup).Name);
		}

		protected override void OnLoad()
		{
			base.OnLoad();

			//GroupTitle = (GroupTitle + "").Trim();
			//Raterange = (Raterange + "").Trim();

			this.TrimField();
		}
		#endregion

		#region 扩展属性﻿
		/// <summary>是否认证用户组</summary>
		public Boolean IsCreditUserGroup { get { return RadminID == 0 && System == 0; } }

		/// <summary>游客用户组</summary>
		public static UserGroup Guest { get { return FindByID((Int32)UserGroupKinds.游客); } }

		private List<String> hasLoad = new List<String>();
		private String _OnlineImage;
		/// <summary>在线图片</summary>
		public String OnlineImage
		{
			get
			{
				if (_OnlineImage == null && !hasLoad.Contains("OnlineImage"))
				{
					var e = OnlineList.GetGroupIcon(ID);
					if (e != null) _OnlineImage = e.Img;
					hasLoad.Add("OnlineImage");
				}
				return _OnlineImage;
			}
			set { _OnlineImage = value; }
		}

		public Boolean Is管理员 { get { return RadminID == (Int32)UserGroupKinds.管理员; } }
		public Boolean Is超级版主 { get { return RadminID == (Int32)UserGroupKinds.超级版主; } }
		public Boolean Is版主 { get { return RadminID == (Int32)UserGroupKinds.版主; } }

		public Boolean Is管理团队 { get { return RadminID > 0; } }
		#endregion

		#region 扩展查询﻿
		/// <summary>根据编号查找</summary>
		/// <param name="id">编号</param>
		/// <returns></returns>
		[DataObjectMethod(DataObjectMethodType.Select, false)]
		public static UserGroup FindByID(Int32 id)
		{
			if (Meta.Cache.Entities.Count == 0 || Meta.Count >= 1000)
				return Find(__.ID, id);
			else // 实体缓存
				return Meta.Cache.Entities.Find(__.ID, id);
			// 单对象缓存
			//return Meta.SingleCache[id];
		}

		public static EntityList<UserGroup> FindAll管理组()
		{
			return FindAllWithCache().FindAll(e => e.RadminID > 0);
		}

		public static EntityList<UserGroup> FindAll积分组()
		{
			return FindAllWithCache().FindAll(e => e.RadminID == 0 && e.System == 0);
		}

		public static EntityList<UserGroup> FindAll系统组()
		{
			// 系统组，包括版主和超级版主
			return FindAllWithCache().FindAll(e => e.System == 1);
		}

		public static EntityList<UserGroup> FindAll特殊组()
		{
			return FindAllWithCache().FindAll(e => e.RadminID == -1 && e.System == 0);
		}

		/// <summary>获取指定组以外的所有组</summary>
		/// <param name="groupid"></param>
		/// <returns></returns>
		public static EntityList<UserGroup> GetUserGroupExceptGroupid(Int32 groupid)
		{
			return FindAllWithCache().FindAll(e => e.ID != groupid);
		}
		#endregion

		#region 高级查询
		//public static List<UserGroup> GetAdminUserGroup()
		//{
		//    return GetUserGroupByAdminIdList("1,2,3");
		//}

		public static EntityList<UserGroup> GetAll()
		{
			var list = FindAllWithCache().Clone();
			return list.Sort(_.ID.Name, false);
		}

		/// <summary>获取管理组和特殊组</summary>
		/// <returns></returns>
		public static List<UserGroup> GetAdminAndSpecialGroup()
		{
			//return GetUserGroupByAdminIdList("-1,1,2,3");
			return FindAllWithCache().FindAll(e => e.RadminID >= -1 && e.RadminID <= 3);
		}

		//private static List<UserGroup> GetUserGroupByAdminIdList(string adminIdList)
		//{
		//    adminIdList = adminIdList.EnsureStart(",").EnsureEnd(",");

		//    var userGroupList = UserGroup.FindAllWithCache();
		//    var list = new List<UserGroup>();
		//    foreach (var current in userGroupList)
		//    {
		//        if (adminIdList.Contains("," + current.RadminID + ","))
		//        {
		//            list.Add(current);
		//        }
		//    }
		//    return list;
		//}

		public static EntityList<UserGroup> FindAllByCredits(int creditshigher, int creditslower)
		{
			//return DatabaseProvider.GetInstance().GetUserGroupByCreditsHigherAndLower(Creditshigher, Creditslower);
			//SELECT [groupid] FROM [{0}usergroups] WHERE [groupid]>8 AND [radminid]=0 AND [Creditshigher]=@Creditshigher AND [Creditslower]=@Creditslower
			return FindAll积分组().FindAll(e => e.Creditshigher == creditshigher && e.Creditslower == creditslower);
		}

		/// <summary>取得积分上限最小的用户组</summary>
		/// <returns></returns>
		public static UserGroup GetMinCreditHigher()
		{
			//SELECT MIN(Creditshigher) FROM [{0}usergroups] WHERE [groupid]>8 AND [radminid]=0
			var list = FindAll积分组();
			if (list.Count <= 0) return null;

			return list.ToList().OrderBy(e => e.Creditshigher).FirstOrDefault();
		}

		/// <summary>取得积分下限最大的用户组</summary>
		/// <returns></returns>
		public static UserGroup GetMaxCreditLower()
		{
			//SELECT MAX(Creditslower) FROM [{0}usergroups] WHERE [groupid]>8 AND [radminid]=0
			var list = FindAll积分组();
			if (list.Count <= 0) return null;

			return list.ToList().OrderByDescending(e => e.Creditslower).FirstOrDefault();
		}

		/// <summary>根据积分所在区间找用户组</summary>
		/// <param name="creditshigher"></param>
		/// <returns></returns>
		public static UserGroup FindByCreditsHigher(Int32 creditshigher)
		{
			//SELECT TOP 1 [groupid],[creditshigher],[creditslower] FROM [{0}usergroups] WHERE [groupid]>8 AND [radminid]=0  AND [Creditshigher]<=@Creditshigher AND @Creditshigher<[Creditslower]
			return FindAll积分组().Find(e => e.Creditshigher <= creditshigher && creditshigher < e.Creditslower);
		}

		public static UserGroup FindByCreditsLower(Int32 creditlower)
		{
			return FindAll积分组().Find(e => e.Creditslower <= creditlower && creditlower < e.Creditslower);
		}
		#endregion

		#region 扩展操作
		public Int32 InsertWithCredits()
		{
			var ug = this;
			int creditshigher = ug.Creditshigher;
			int creditslower = ug.Creditslower;
			//DataTable userGroupByCreditsHigherAndLower = UserGroup.GetUserGroupByCreditsHigherAndLower(creditshigher, creditslower);
			//if (userGroupByCreditsHigherAndLower.Rows.Count > 0) return false;
			// 检查该积分用户组是否存在
			var list = UserGroup.FindAllByCredits(creditshigher, creditslower);
			if (list.Count > 0) return -1;

			if (!ug.Is管理团队)
			{
				var rs = SystemCheckCredits("add", ref creditshigher, ref creditslower, 0);
				if (rs == null) return -1;
			}

			ug.Creditshigher = creditshigher;
			ug.Creditslower = creditslower;
			//UserGroup.CreateUserGroup(userGroupInfo);
			ug.Insert();
			//BBX.Data.OnlineUsers.AddOnlineList(userGroupInfo.GroupTitle);
			OnlineList.Add(ug.ID, ug.GroupTitle);
			//Caches.ReSetAdminGroupList();
			//Caches.ReSetUserGroupList();

			return 0;
		}

		public Int32 UpdateWithCredits()
		{
			if (ID >= 9 && !Is管理团队)
			{
				var list = FindAllByCredits(Creditshigher, Creditslower);
				if (list.Count > 0 && ID != list[0].ID) return -1;

				var creditshigher = Creditshigher;
				var creditslower = Creditslower;
				var rs = SystemCheckCredits("update", ref creditshigher, ref creditslower, ID);
				if (rs == null) return -1;
			}

			return Update();
		}

		public Int32 DeleteWithCredits()
		{
			if (UserGroup.IsSystemOrTemplateUserGroup(ID)) return -1;

			var ug = this;
			if (ID >= 9)
			{
				//var list = UserGroup.FindAllWithCache().FindAll(e => e.ID != ug.ID);
				var list = UserGroup.GetUserGroupExceptGroupid(ug.ID);
				//DataTable userGroupExceptGroupid = UserGroups.GetUserGroupExceptGroupid(groupid);
				//if (userGroupExceptGroupid.Rows.Count > 1)
				if (list.Count > 1)
				{
					if (!ug.Is管理团队)
					{
						int creditshigher = ug.Creditshigher;
						int creditslower = ug.Creditslower;
						//SystemCheckCredits("delete", ref creditshigher, ref creditslower, ID);

						// 找到紧挨着的上一个积分组，把它的下限改为当前下限
						var ug2 = FindByCreditsLower(ug.Creditshigher);
						if (ug2 != null)
						{
							//UpdateCreditsLowerByCreditsLower(creditsLower, creditsHigher);
							ug2.Creditslower = ug.Creditslower;
							ug2.Update();
						}
						// 找下一个积分组，它的上限改为当前上限
						else
						{
							//UpdateCreditsHigherByCreditsHigher(creditsHigher, creditsLower);
							ug2 = FindByCreditsHigher(ug.Creditslower);
							if (ug2 != null)
							{
								//UpdateCreditsLowerByCreditsLower(creditsLower, creditsHigher);
								ug2.Creditshigher = ug.Creditshigher;
								ug2.Update();
							}
						}
					}
				}
				else
				{
					if (list.Count != 1) throw new Exception("当前用户组为系统中唯一的用户组,因此系统无法删除");

					var ug2 = list[0];
					ug2.Creditshigher = -9999999;
					ug2.Creditslower = 9999999;
					ug2.Update();
				}
			}

			var adg = AdminGroup.FindByID(ID);
			if (adg != null) adg.Delete();

			Online.DeleteByUserGroup(ID);

			return ug.Delete();
		}

		public static String SystemCheckCredits(string opname, ref int creditsHigher, ref int creditsLower, int groupid)
		{
			var act = opname.ToLower();
			if (act == null) return null;

			if (act == "update")
			{
				var ugi = FindByID(groupid);
				int creditshigher = ugi.Creditshigher;
				int creditslower = ugi.Creditslower;

				var ug = GetMinCreditHigher();
				if (ug != null)
				{
					if (creditsLower <= ug.Creditshigher)
					{
						creditsLower = ug.Creditshigher;
						//UserGroup.UpdateUserGroupsCreditsHigherByCreditsHigher(creditshigher, creditslower);
						// UPDATE [{0}usergroups] SET [Creditshigher]=@Creditshigher WHERE [groupid]>8 AND [radminid]=0 AND [Creditshigher]=@Creditslower
						ug.Creditshigher = creditshigher;
						ug.Update();
						return "由您所输入的积分下限小于或等于系统最大值,因此系统已将其调整为" + ug.Creditshigher;
					}
				}
				ug = GetMaxCreditLower();
				if (ug != null)
				{
					if (creditsHigher >= ug.Creditslower)
					{
						creditsHigher = ug.Creditslower;
						//UserGroup.UpdateUserGroupsCreditsLowerByCreditsLower(creditslower, creditshigher);
						// UPDATE [{0}usergroups] SET [creditslower]=@Creditslower WHERE [groupid]>8 AND [radminid]=0 AND [creditslower]=@Creditshigher
						ug.Creditslower = creditslower;
						ug.Update();
						return "由您所输入的积分上限大于或等于系统最大值,因此系统已将其调整为" + ug.Creditslower;
					}
				}
				ug = UserGroup.FindByCreditsHigher(creditsHigher);
				if (ug != null) return "系统未提到合适的位置保存您提交的信息!";

				//Convert.ToInt32(dataTable.Rows[0][0].ToString());
				//int num3 = Convert.ToInt32(dataTable.Rows[0][1].ToString());
				//int num4 = Convert.ToInt32(dataTable.Rows[0][2].ToString());
				if (creditsLower > ug.Creditslower) return "由您所输入的积分上限大于或等于所属有效积分上限的最大值" + ug.Creditslower + ",因此系统无效提交您的数据!";

				if (creditsHigher == ug.Creditshigher)
				{
					if (creditsLower < ug.Creditslower)
					{
						UpdateCreditsHigherByCreditsHigher(creditsLower, ug.Creditslower);
					}
				}
				else
				{
					UpdateCreditsHigherByCreditsHigher(creditsLower, ug.Creditslower);
					UpdateCreditsLowerByCreditsLower(creditsHigher, ug.Creditshigher);
					return "系统已自动将您提交的积分上限调整为" + ug.Creditslower;
				}

				return null;
			}

			if (act == "delete")
			{
				//// 找到紧挨着的上一个积分组，把它的下限改为当前下限
				//var ug = FindByCreditsLower(creditsHigher);
				//if (ug != null)
				//{
				//    //UpdateCreditsLowerByCreditsLower(creditsLower, creditsHigher);
				//    ug.Creditslower = creditsLower;
				//    ug.Update();
				//}
				//else
				//    UpdateCreditsHigherByCreditsHigher(creditsHigher, creditsLower);

				return null;
			}

			if (act == "add")
			{
				var ug = GetMinCreditHigher();
				if (ug != null)
				{
					if (creditsLower <= ug.Creditshigher)
					{
						creditsLower = ug.Creditshigher;
						return "由您所输入的积分下限小于或等于系统最小值,因此系统已将其调整为" + ug.Creditshigher;
					}
				}
				ug = GetMaxCreditLower();
				if (ug != null)
				{
					if (creditsHigher >= ug.Creditslower)
					{
						creditsHigher = ug.Creditslower;
						return "由您所输入的积分上限大于或等于系统最大值,因此系统已将其调整为" + ug.Creditslower;
					}
				}
				ug = FindByCreditsHigher(creditsHigher);
				if (ug != null) return "系统未提到合适的位置保存您提交的信息!";

				if (creditsLower > ug.Creditslower) return null;

				if (creditsHigher == ug.Creditshigher)
				{
					if (creditsLower >= ug.Creditslower) return "由您所输入的积分上限大于或等于所属有效积分上限的最大值" + ug.Creditslower + ",因此系统无效提交您的数据!";

					ug.Creditshigher = creditsLower;
					ug.Save();
				}
				else
				{
					creditsLower = ug.Creditslower;
					UpdateCreidtsLower(ug.Creditshigher, creditsHigher);
				}
				return "";
			}

			return null;
		}

		public static void UpdateCreidtsLower(int currentCreditsHigher, int creditshigher)
		{
			// DbHelper.MakeInParam("@creditslower", DbType.Double, 4, creditsHigher),
			// DbHelper.MakeInParam("@creditshigher", DbType.Double, 4, currentCreditsHigher)
			// UPDATE [{0}usergroups] SET [creditslower]=@creditslower WHERE [groupid]>8 AND [radminid]=0 AND [creditshigher]=@creditshigher

			var ug = FindAll积分组().Find(__.Creditshigher, currentCreditsHigher);
			if (ug != null)
			{
				ug.Creditslower = creditshigher;
				ug.Update();
			}
		}

		public static void UpdateCreditsLowerByCreditsLower(int creditslower, int creditshigher)
		{
			//DatabaseProvider.GetInstance().UpdateUserGroupsCreditsLowerByCreditsLower(Creditslower, Creditshigher);
			//UPDATE [{0}usergroups] SET [creditslower]=@Creditslower WHERE [groupid]>8 AND [radminid]=0 AND [creditslower]=@Creditshigher

			var ug = FindAll积分组().Find(__.Creditslower, creditshigher);
			if (ug != null)
			{
				ug.Creditslower = creditshigher;
				ug.Update();
			}
		}

		public static void UpdateCreditsHigherByCreditsHigher(int creditshigher, int creditslower)
		{
			//DatabaseProvider.GetInstance().UpdateUserGroupsCreditsHigherByCreditsHigher(Creditshigher, Creditslower);
			//UPDATE [{0}usergroups] SET [Creditshigher]=@Creditshigher WHERE [groupid]>8 AND [radminid]=0 AND [Creditshigher]=@Creditslower

			var ug = FindAll积分组().Find(__.Creditshigher, creditslower);
			if (ug != null)
			{
				ug.Creditslower = creditshigher;
				ug.Update();
			}
		}
		#endregion

		#region 业务
		public static bool IsSystemOrTemplateUserGroup(int groupid)
		{
			//return DatabaseProvider.GetInstance().IsSystemOrTemplateUserGroup(groupid);
			var ug = FindByID(groupid);
			if (ug == null) return false;

			return ug.System == 1 || ug.Type == 1;
		}

		/// <summary>设置最大最小极限</summary>
		public void SetLimit()
		{
			//DatabaseProvider.GetInstance().UpdateUserGroupLowerAndHigherToLimit(groupid);
			//UPDATE [{0}usergroups] SET [creditshigher]=-9999999 ,creditslower=9999999  WHERE [groupid]={1}
			var ug = this;
			ug.Creditshigher = -9999999;
			ug.Creditslower = 9999999;
			ug.Update();
		}

		public static int GetMaxUserGroupId()
		{
			var list = FindAllWithCache().Sort(__.ID, true);
			return list[0].ID;
		}

		/// <summary>改变用户的用户组</summary>
		/// <param name="sourceGroupId"></param>
		/// <param name="targetGroupId"></param>
		public static void ChangeAllUserGroupId(int sourceGroupId, int targetGroupId)
		{
			//BBX.Data.UserGroups.ChangeAllUserGroupId(sourceGroupId, targetGroupId);

			var list = User.FindAllWithCache(User._.GroupID, sourceGroupId);
			list.SetItem(User._.GroupID, targetGroupId);
			list.Save();
		}

		//public static void UpdateUserGroupRaterange(string raterange, int groupid)
		//{
		//	//BBX.Data.UserGroups.UpdateUserGroupRaterange(raterange, groupid);
		//	// UPDATE [{0}usergroups] SET [raterange]=@raterange WHERE [groupid]=@groupid
		//}

		public int CheckMaxPrice(int price)
		{
			if (price <= 0) return 0;

			if (price > 0 && price <= MaxPrice) return price;

			return MaxPrice;
		}
		#endregion
	}
}