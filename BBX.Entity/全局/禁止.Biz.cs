﻿using System;
using System.ComponentModel;
using System.Net;

namespace BBX.Entity
{
    /// <summary>禁止</summary>
    public partial class Banned : EntityBase<Banned>
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

        ///// <summary>首次连接数据库时初始化数据，仅用于实体类重载，用户不应该调用该方法</summary>
        //[EditorBrowsable(EditorBrowsableState.Never)]
        //protected override void InitData()
        //{
        //    base.InitData();

        //    // InitData一般用于当数据表没有数据时添加一些默认数据，该实体类的任何第一次数据库操作都会触发该方法，默认异步调用
        //    // Meta.Count是快速取得表记录数
        //    if (Meta.Count > 0) return;

        //    // 需要注意的是，如果该方法调用了其它实体类的首次数据库操作，目标实体类的数据初始化将会在同一个线程完成
        //    if (XTrace.Debug) XTrace.WriteLine("开始初始化{0}禁止数据……", typeof(Banned).Name);

        //    var entity = new Banned();
        //    entity.ID = 0;
        //    entity.Ip1 = 0;
        //    entity.Ip2 = 0;
        //    entity.Ip3 = 0;
        //    entity.Ip4 = 0;
        //    entity.Admin = "abc";
        //    entity.Dateline = DateTime.Now;
        //    entity.Expiration = DateTime.Now;
        //    entity.Insert();

        //    if (XTrace.Debug) XTrace.WriteLine("完成初始化{0}禁止数据！", typeof(Banned).Name);
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
        private String _Location;
        /// <summary>定位</summary>
        public String Location
        {
            get
            {
                if (_Location == null)
                {
                    _Location = IPAddress.Parse(String.Format("{0}.{1}.{2}.{3}", Ip1, Ip2, Ip3, Ip4)).GetAddress();
                }
                return _Location;
            }
        }
        #endregion

        #region 扩展查询﻿
        /// <summary>根据编号查找</summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static Banned FindByID(Int16 id)
        {
            if (Meta.Count >= 1000)
                return Find(_.ID, id);
            else // 实体缓存
                return Meta.Cache.Entities.Find(__.ID, id);
            // 单对象缓存
            //return Meta.SingleCache[id];
        }
        #endregion

        #region 高级查询
        // 以下为自定义高级查询的例子

        ///// <summary>
        ///// 查询满足条件的记录集，分页、排序
        ///// </summary>
        ///// <param name="key">关键字</param>
        ///// <param name="orderClause">排序，不带Order By</param>
        ///// <param name="startRowIndex">开始行，0表示第一行</param>
        ///// <param name="maximumRows">最大返回行数，0表示所有行</param>
        ///// <returns>实体集</returns>
        //[DataObjectMethod(DataObjectMethodType.Select, true)]
        //public static EntityList<Banned> Search(String key, String orderClause, Int32 startRowIndex, Int32 maximumRows)
        //{
        //    return FindAll(SearchWhere(key), orderClause, null, startRowIndex, maximumRows);
        //}

        ///// <summary>
        ///// 查询满足条件的记录总数，分页和排序无效，带参数是因为ObjectDataSource要求它跟Search统一
        ///// </summary>
        ///// <param name="key">关键字</param>
        ///// <param name="orderClause">排序，不带Order By</param>
        ///// <param name="startRowIndex">开始行，0表示第一行</param>
        ///// <param name="maximumRows">最大返回行数，0表示所有行</param>
        ///// <returns>记录数</returns>
        //public static Int32 SearchCount(String key, String orderClause, Int32 startRowIndex, Int32 maximumRows)
        //{
        //    return FindCount(SearchWhere(key), null, null, 0, 0);
        //}

        /// <summary>构造搜索条件</summary>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        private static String SearchWhere(String key)
        {
            // WhereExpression重载&和|运算符，作为And和Or的替代


            // SearchWhereByKeys系列方法用于构建针对字符串字段的模糊搜索
            var exp = SearchWhereByKeys(key);

            // 以下仅为演示，2、3行是同一个意思的不同写法，Field（继承自FieldItem）重载了==、!=、>、<、>=、<=等运算符（第4行）
            //exp &= _.Name == "testName"
            //    & !String.IsNullOrEmpty(key) & _.Name == key
            //    .AndIf(!String.IsNullOrEmpty(key), _.Name == key)
            //    | _.ID > 0;

            return exp;
        }
        #endregion

        #region 扩展操作
        #endregion

        #region 业务
        public static void EditBanIp(string[] expiration, string[] hiddenexpiration, string[] hiddenid, int useradminid, int userid)
        {
            for (int i = 0; i < expiration.Length; i++)
            {
                var uid = Int32.Parse(hiddenid[i]);
                var user = User.FindByID(uid);
                if ((useradminid == 1 || userid == user.ID) && expiration[i] != hiddenexpiration[i])
                {
                    //BBX.Data.Ips.EditBanIp(hiddenid[i].ToString().ToInt(-1), expiration[i]);
                    var ban = FindByID((Int16)uid);
                    if (ban != null)
                    {
                        ban.Expiration = expiration[i].ToDateTime();
                        ban.Update();
                    }
                }
            }
        }

        public static void Add(String ip, Int32 exp, String username)
        {
            var ss = ip.Split('.');

            var entity = new Banned();
            entity.Ip1 = (Int16)ss[0].ToInt();
            entity.Ip2 = (Int16)ss[1].ToInt();
            entity.Ip3 = (Int16)ss[2].ToInt();
            entity.Ip4 = (Int16)ss[3].ToInt();
            entity.Dateline = DateTime.Now;
            entity.Expiration = DateTime.Now.AddDays(exp);
            entity.Admin = username;
            entity.Insert();
        }
        #endregion
    }
}