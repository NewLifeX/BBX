﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;
using BBX.Common;
using BBX.Config;
using NewLife.Log;
using NewLife.Web;
using XCode;
using XCode.Configuration;

namespace BBX.Entity
{
    /// <summary>邀请</summary>
    public partial class Invitation : EntityBase<Invitation>
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

            if (isNew && !Dirtys[_.CreateTime]) CreateTime = DateTime.Now;
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
        //    if (XTrace.Debug) XTrace.WriteLine("开始初始化{0}邀请数据……", typeof(Invitation).Name);

        //    var entity = new Invitation();
        //    entity.Code = "abc";
        //    entity.CreatorID = 0;
        //    entity.Creator = "abc";
        //    entity.SuccessCount = 0;
        //    entity.CreatedTime = DateTime.Now;
        //    entity.ExpireTime = DateTime.Now;
        //    entity.MaxCount = 0;
        //    entity.InviteType = 0;
        //    entity.IsDeleted = true;
        //    entity.Insert();

        //    if (XTrace.Debug) XTrace.WriteLine("完成初始化{0}邀请数据！", typeof(Invitation).Name);
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
        /// <summary>根据邀请码表查找</summary>
        /// <param name="Code">邀请码表</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static EntityList<Invitation> FindAllByInviteCode(String Code)
        {
            if (Meta.Count >= 1000)
                return FindAll(__.Code, Code);
            else // 实体缓存
                return Meta.Cache.Entities.FindAll(__.Code, Code);
        }

        /// <summary>根据CreatorID查找</summary>
        /// <param name="creatorid"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static EntityList<Invitation> FindAllByCreatorID(Int32 creatorid)
        {
            if (Meta.Count >= 1000)
                return FindAll(__.CreatorID, creatorid);
            else // 实体缓存
                return Meta.Cache.Entities.FindAll(__.CreatorID, creatorid);
        }

        /// <summary>根据InviteType查找</summary>
        /// <param name="invitetype"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static EntityList<Invitation> FindAllByInviteType(Int32 invitetype)
        {
            if (Meta.Count >= 1000)
                return FindAll(__.InviteType, invitetype);
            else // 实体缓存
                return Meta.Cache.Entities.FindAll(__.InviteType, invitetype);
        }

        /// <summary>根据编号查找</summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static Invitation FindByID(Int32 id)
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
        //public static EntityList<Invitation> Search(String key, String orderClause, Int32 startRowIndex, Int32 maximumRows)
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
        public static Invitation GetInviteCodeByUid(Int32 uid)
        {
            return Find(_.CreatorID, uid);
        }

        public static Int32 GetUserInviteCodeCount(Int32 uid)
        {
            return FindCount(_.CreatorID, uid);
        }

        public static EntityList<Invitation> GetUserInviteCodeList(Int32 uid, Int32 pageindex)
        {
            return FindAllByName(_.CreatorID, uid, null, (pageindex - 1) * 10, 10);
        }

        public static Invitation FindByCode(String code)
        {
            return Find(_.Code, code);
        }

        public static Int32 GetTodayUserCreatedInviteCode(Int32 uid)
        {
            return FindCount(_.CreatorID, uid);
        }

        public static int CreateInviteCode(IUser userInfo)
        {
            var cfg = InvitationConfigInfo.Current;
            var entity = new Invitation();
            entity.CreatorID = userInfo.ID;
            entity.Creator = userInfo.Name;
            entity.Code = BuildInviteCode();
            while (entity.Exist(_.Code))
            {
                entity.Code = BuildInviteCode();
            }
            entity.CreateTime = DateTime.Now;
            entity.InviteType = GeneralConfigInfo.Current.Regstatus;
            entity.ExpireTime = DateTime.Now.AddDays(cfg.InviteCodeExpireTime);
            if (entity.InviteType == 3)
                entity.MaxCount = cfg.InviteCodeMaxCount > 1 ? cfg.InviteCodeMaxCount : 1;
            else
                entity.MaxCount = cfg.InviteCodeMaxCount;

            return entity.Insert();
        }

        private static string BuildInviteCode()
        {
            var random = new Random();
            var cs = new Char[8];
            for (int i = 0; i < cs.Length; i++)
            {
                cs[i] = (Char)('A' + random.Next(0, 26));
            }
            return new String(cs);
        }

        public static void ConvertInviteCodeToCredits(Invitation inviteCode, int inviteCodePayCount)
        {
            int num = inviteCode.SuccessCount - inviteCodePayCount;
            if (num > -1)
            {
                //CreditsFacade.Invite(inviteCode.CreatorId, inviteCode.SuccessCount);
                //TODO: 更新积分
            }
        }

        public bool Check()
        {
            var config = GeneralConfigInfo.Current;

            if (InviteType != config.Regstatus) return false;

            if (CreateTime != ExpireTime && ExpireTime > DateTime.MinValue) return false;

            int num = (InviteType == 2) ? InvitationConfigInfo.Current.InviteCodeMaxCount : MaxCount;
            return num <= 0 || SuccessCount < num;
        }

        public static void ClearExpireInviteCode()
        {
            Update(_.IsDeleted == true, _.ExpireTime < DateTime.Now);
        }
        #endregion
    }
}