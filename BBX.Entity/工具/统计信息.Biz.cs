﻿using System;
using System.ComponentModel;
using BBX.Common;
using NewLife.Log;
using XCode;

namespace BBX.Entity
{
	/// <summary>统计信息</summary>
	public partial class Stat : Entity<Stat>
	{
		#region 对象操作﻿
		static Stat()
		{
			Meta.Factory.AdditionalFields.Add(__.Count);
			//Meta.Session.HoldCache = true;

			//// 设置单对象缓存
			//var sc = Meta.SingleCache;
			//sc.AllowNull = false;
			//sc.AutoSave = true;
			//sc.Expriod = 10 * 60; // 5分钟过期
			//sc.FindKeyMethod = s =>
			//{
			//    var str = s + "";
			//    if (str == String.Empty) return null;

			//    var p = str.IndexOf("_");
			//    if (p < 0) return null;

			//    var type = str.Substring(0, p);
			//    var variable = str.Substring(p + 1);

			//    if (Meta.Count > 1000)
			//        return Find(_.Type == type & _.Variable == variable);
			//    else
			//        return Meta.Cache.Entities.Find(e => (e.Type + "").Trim().EqualIgnoreCase(type) && (e.Variable + "").Trim().EqualIgnoreCase(variable));
			//};
		}

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

		//public override void LoadData(DataRow dr)
		//{
		//    base.LoadData(dr);

		//    if (Type != null) Type = Type.Trim();
		//    if (Variable != null) Variable = Variable.Trim();
		//}
		protected override bool OnPropertyChanging(string fieldName, object newValue)
		{
			if (fieldName == __.Type)
				newValue = (newValue + "").Trim();
			else if (fieldName == __.Variable)
				newValue = (newValue + "").Trim();

			return base.OnPropertyChanging(fieldName, newValue);
		}

		///// <summary>首次连接数据库时初始化数据，仅用于实体类重载，用户不应该调用该方法</summary>
		//[EditorBrowsable(EditorBrowsableState.Never)]
		//protected override void InitData()
		//{
		//    base.InitData();

		//    // InitData一般用于当数据表没有数据时添加一些默认数据，该实体类的任何第一次数据库操作都会触发该方法，默认异步调用
		//    // Meta.Count是快速取得表记录数
		//    if (Meta.Count > 0) return;

		//    // 需要注意的是，如果该方法调用了其它实体类的首次数据库操作，目标实体类的数据初始化将会在同一个线程完成
		//    if (XTrace.Debug) XTrace.WriteLine("开始初始化{0}统计信息数据……", typeof(Stat).Name);

		//    var entity = new Stat();
		//    entity.Type = "abc";
		//    entity.Variable = "abc";
		//    entity.Count = 0;
		//    entity.Insert();

		//    if (XTrace.Debug) XTrace.WriteLine("完成初始化{0}统计信息数据！", typeof(Stat).Name);
		//}


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
		#endregion

		#region 扩展查询﻿
		/// <summary>根据类型、变量查找</summary>
		/// <param name="type">类型</param>
		/// <param name="variable">变量</param>
		/// <returns></returns>
		[DataObjectMethod(DataObjectMethodType.Select, false)]
		public static Stat FindByTypeAndVariable(String type, String variable)
		{
			if (Meta.Count >= 1000)
				return Find(new String[] { __.Type, __.Variable }, new Object[] { type, variable });
			else // 实体缓存
				return Meta.Cache.Entities.Find(e => e.Type.Trim().EqualIgnoreCase(type) && e.Variable.Trim().EqualIgnoreCase(variable));

			//// 单对象缓存
			//return Meta.SingleCache[String.Format("{0}_{1}", type, variable)];
		}
		#endregion

		#region 高级查询
		public static EntityList<Stat> GetAll()
		{
			//return FindAll(null, _.Type.Asc() & _.Variable.Asc(), null, 0, 0);
			var list = FindAllWithCache();
			list.Sort(__.Variable, false);
			list.Sort(__.Type, false);
			return list;
		}
		#endregion

		#region 扩展操作
		#endregion

		#region 业务
		static Object syncLock = new Object();

		static void Inc(string type, string variable)
		{
			// 使用同步锁来解决重复插入值的问题
			lock (syncLock)
			{
				var st = FindByTypeAndVariable(type, variable);
#if DEBUG
				if (st == null)
				{
					st = Find(_.Type == type & _.Variable == variable);
					if (st != null) XTrace.WriteLine("实体缓存没有查找到 Type={0} Variable={1}的数据，而数据库存在", type, variable);
				}
#endif
				if (st == null)
				{
					st = new Stat();
					st.Type = type;
					st.Variable = variable;
					st.Save();

					// 再查一次，确保是单对象缓存
					//st = FindByTypeAndVariable(type, variable);
				}

				// 单对象缓存会自动保存
				st.Type = type;
				st.Variable = variable;
				st.Count++;
			}
		}

		public static void IncCount(bool isguest, bool sessionexists)
		{
			Meta.BeginTrans();
			try
			{
				if (!sessionexists)
				{
					string clientBrower = Utils.GetClientBrower();
					string clientOS = Utils.GetClientOS();

					Inc("browser", clientBrower);
					Inc("os", clientOS);
					if (isguest)
						Inc("total", "guests");
					else
						Inc("total", "members");
				}

				var now = DateTime.Now;
				var month = now.ToString("yyyyMM");
				var week = (int)now.DayOfWeek + "";
				var hour = now.Hour.ToString("00");

				Inc("total", "hits");
				Inc("month", month);
				Inc("week", week);
				Inc("hour", hour);

				Meta.Commit();
			}
			catch { Meta.Rollback(); throw; }

			//var exp = new WhereExpression();
			//if (!sessionexists)
			//{
			//    exp |= (_.Type == "browser" & _.Variable == clientBrower);
			//    exp |= (_.Type == "os" & _.Variable == clientOS);

			//    if (isguest)
			//        exp |= (_.Type == "total" & _.Variable == "guests");
			//    else
			//        exp |= (_.Type == "total" & _.Variable == "members");
			//}
			//ThreadPool.QueueUserWorkItem(s => UpdateStatCount(clientBrower, clientOS, exp.ToString()));
		}

		//static void UpdateStatCount(string browser, string os, string visitorsAdd)
		//{
		//    var now = DateTime.Now;
		//    var month = now.ToString("yyyyMM");
		//    var week = (int)now.DayOfWeek + "";
		//    var hour = now.Hour.ToString("00");

		//    var exp = _.Type == "total" & _.Variable == "hits";
		//    exp |= visitorsAdd;
		//    exp |= (_.Type == "month" & _.Variable == month);
		//    exp |= (_.Type == "week" & _.Variable == week);
		//    exp |= (_.Type == "hour" & _.Variable == hour);

		//    //string text = DateTime.Now.Year + DateTime.Now.Month.ToString("00");
		//    //string text2 = ((int)DateTime.Now.DayOfWeek).ToString();
		//    //string commandText = string.Format("UPDATE [{0}stats] SET [count]=[count]+1 WHERE ([type]='total' AND [variable]='hits') {1} OR ([type]='month' AND [variable]='{2}') OR ([type]='week' AND [variable]='{3}') OR ([type]='hour' AND [variable]='{4}')", new object[]
		//    //{
		//    //    TablePrefix,
		//    //    visitorsAdd,
		//    //    text,
		//    //    text2,
		//    //    DateTime.Now.Hour.ToString("00")
		//    //});
		//    //int num = DbHelper.ExecuteNonQuery(CommandType.Text, commandText);
		//    var num = Update(String.Format("{0}={0}+1", __.Count), exp);
		//    int num2 = String.IsNullOrEmpty(visitorsAdd) ? 4 : 7;
		//    if (num2 > num)
		//    {
		//        Stat.Inc("browser", browser, 0);
		//        Stat.Inc("os", os, 0);
		//        Stat.Inc("total", "members", 0);
		//        Stat.Inc("total", "guests", 0);
		//        Stat.Inc("total", "hits", 0);
		//        Stat.Inc("month", month, 0);
		//        Stat.Inc("week", week, 0);
		//        Stat.Inc("hour", hour, 0);
		//    }
		//}
		#endregion
	}
}