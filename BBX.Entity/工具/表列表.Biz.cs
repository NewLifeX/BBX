﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using BBX.Cache;
using BBX.Common;
using BBX.Config;
using NewLife.Log;
using XCode;

namespace BBX.Entity
{
	/// <summary>表列表</summary>
	public partial class TableList : Entity<TableList>
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

			if (isNew && !Dirtys[_.CreateDateTime]) CreateDateTime = DateTime.Now;
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
			if (XTrace.Debug) XTrace.WriteLine("开始初始化{0}表列表数据……", typeof(TableList).Name);

			var entity = new TableList();
			entity.CreateDateTime = DateTime.Now;
			entity.Description = String.Format("{0}posts{1}", BaseConfigs.GetTablePrefix, 1);
			entity.MintID = 1;
			entity.MaxtID = 0;
			entity.Insert();

			if (XTrace.Debug) XTrace.WriteLine("完成初始化{0}表列表数据！", typeof(TableList).Name);
		}

		///// <summary>已重载。基类先调用Valid(true)验证数据，然后在事务保护内调用OnInsert</summary>
		///// <returns></returns>
		//public override Int32 Insert()
		//{
		//    return base.Insert();
		//}

		///// <summary>已重载。在事务保护范围内处理业务，位于Valid之后</summary>
		///// <returns></returns>
		//protected override Int32 OnInsert()
		//{
		//    return base.OnInsert();
		//}
		#endregion

		#region 扩展属性﻿
		private static TableList _Current;
		/// <summary>当前统计</summary>
		public static TableList Current
		{
			get
			{
				if (_Current == null)
				{
					var list = FindAll(null, _.ID.Desc(), null, 0, 1);
					if (list.Count > 0)
						_Current = list[0];
					else
					{
						var entity = new TableList();
						entity.CreateDateTime = DateTime.Now;
						entity.Description = String.Format("{0}posts{1}", BaseConfigs.GetTablePrefix, 1);
						entity.MintID = 1;
						entity.MaxtID = 0;
						entity.Insert();
						//entity.Save();

						_Current = entity;
					}
				}
				return _Current;
			}
		}

		/// <summary>表名</summary>
		public static String CurrentTableName { get { return Current.TableName; } }

		/// <summary>表名</summary>
		public String TableName { get { return FormatPostTableName(ID); } }
		#endregion

		#region 扩展查询﻿
		/// <summary>根据编号查找</summary>
		/// <param name="id">编号</param>
		/// <returns></returns>
		[DataObjectMethod(DataObjectMethodType.Select, false)]
		public static TableList FindByID(Int32 id)
		{
			if (Meta.Count >= 1000)
				return Find(_.ID, id);
			else // 实体缓存
				return Meta.Cache.Entities.Find(_.ID, id);
			// 单对象缓存
			//return Meta.SingleCache[id];
		}
		#endregion

		#region 高级查询
		#endregion

		#region 扩展操作
		#endregion

		#region 业务
		public static EntityList<TableList> GetAllPostTable()
		{
			return FindAllWithCache().Sort(__.ID, true);
		}

		/// <summary>根据帖子ID范围查找合适的帖子表。这个范围的帖子可能同时在几个帖子表里面</summary>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <returns></returns>
		public static List<TableList> GetAllPostTableByMinAndMax(Int32 min, Int32 max)
		{
			return GetAllPostTable().ToList().Where(e => e.MintID <= max && (e.MaxtID <= 0 || e.MaxtID >= min)).ToList();
		}

		/// <summary>根据帖子ID找帖子表ID</summary>
		/// <param name="tid"></param>
		/// <returns></returns>
		public static String GetPostTableId(Int32 tid = 0)
		{
			var id = GetPostTableId_(tid);
			return id > 0 ? id.ToString() : null;
		}

		public static Int32 GetPostTableId_(Int32 tid = 0)
		{
			var list = GetAllPostTable();
			if (list.Count < 1) return 0;

			if (tid > 0)
			{
				var list2 = GetAllPostTableByMinAndMax(tid, tid);
				if (list2.Count > 0) return list2[0].ID;
			}

			return list[0].ID;
		}

		/// <summary>根据帖子ID找帖子表表名</summary>
		/// <param name="tid"></param>
		/// <returns></returns>
		public static String GetPostTableName(int tid = 0)
		{
			var ptid = GetPostTableId_(tid);
			if (ptid <= 0) return null;

			return FormatPostTableName(ptid);
		}

		/// <summary>格式化帖子表表名</summary>
		/// <param name="ptid"></param>
		/// <returns></returns>
		public static String FormatPostTableName(int ptid)
		{
			if (ptid <= 0) ptid = 1;

			// 第一个表就是默认表
			if (ptid == 1) return Post.Meta.Table.TableName;

			return String.Format("{0}posts{1}", BaseConfigs.GetTablePrefix, ptid);
		}

		public static void ResetPostTables()
		{
			if (GeneralConfigInfo.Current.Webgarden > 1 && Environment.Version.Major < 4)
			{
				Utils.RestartIISProcess();
				return;
			}
			XCache.Remove("/Forum/PostTables");
			XCache.Remove("/Forum/PostTableId");
		}

		public static int GetPostTableCount(string tableName)
		{
			//return DatabaseProvider.GetInstance().GetPostCount(tableName);

			return (Int32)Post.ProcessWithTable(tableName, () => Topic.Meta.Count);
		}

		public static int GetPostsCount(Int32 postTableid)
		{
			var tb = String.Format("{0}posts{1}", BaseConfigs.GetTablePrefix, postTableid);
			return GetPostTableCount(tb);
		}

		public static int UpdateDetachTable(int detachTableId, string description)
		{
			//return DatabaseProvider.GetInstance().UpdateDetachTable(detachTableId, description);
			var tb = FindByID(detachTableId);
			if (tb == null) return 0;

			tb.Description = description;
			return tb.Update();
		}

		public static void UpdateMinMaxField(string postTableName, int tablelistmaxid)
		{
			//DatabaseProvider.GetInstance().UpdateMinMaxField(posttablename, tablelistmaxid);
			var tb = FindByID(tablelistmaxid);
			if (tb != null)
			{
				tb.MintID = Post.GetMinPostTableTid(postTableName);
				tb.MaxtID = Post.GetMaxPostTableTid(postTableName);
				tb.Save();
			}
		}

		public static void UpdateMinMaxField()
		{
			string prefix = BaseConfigs.GetTablePrefix + "posts";
			//foreach (DataRow dataRow in Posts.GetAllPostTable().Rows)
			foreach (var item in TableList.GetAllPostTable())
			{
				if (TableList.CurrentTableName != prefix + item.ID)
				{
					UpdateMinMaxField(prefix + item.ID, item.ID);
				}
			}
		}

		public static bool UpdateMinMaxField(int tablelistmaxid)
		{
			if (tablelistmaxid > 0 && tablelistmaxid < 213)
			{
				UpdateMinMaxField(BaseConfigs.GetTablePrefix + "posts" + tablelistmaxid, tablelistmaxid);
				return true;
			}
			return false;
		}

		public static void AddPostTableToTableList(string description, int minTid)
		{
			//DatabaseProvider.GetInstance().AddPostTableToTableList(description, posttablename, 0);

			var tb = new TableList();
			tb.Description = description;
			tb.MintID = minTid;
			tb.MaxtID = 0;
			tb.Insert();
		}

		//public static int GetMaxPostTableTid(string posttabelname)
		//{
		//    return DatabaseProvider.GetInstance().GetMaxPostTableTid(posttabelname);
		//}

		//public static DataTable GetMaxTid()
		//{
		//    return DatabaseProvider.GetInstance().GetMaxTid();
		//}

		//public static void CreateStoreProc(int tablelistmaxid)
		//{
		//    DatabaseProvider.GetInstance().CreateStoreProc(tablelistmaxid);
		//}

		//public static bool CreateORFillIndex(string DbName, string postid)
		//{
		//    return DatabaseProvider.GetInstance().CreateORFillIndex(DbName, postid);
		//}
		#endregion
	}
}