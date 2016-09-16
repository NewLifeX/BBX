using System;
using System.Linq;
using System.ComponentModel;
using BBX.Config;
using XCode;

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
        #endregion

        #region 扩展操作
        #endregion

        #region 业务
        public static Invitation GetInviteCodeByUid(Int32 uid)
        {
            //return Find(__.CreatorID, uid);
            return FindAllByCreatorID(uid).ToList().FirstOrDefault();
        }

        public static Int32 GetUserInviteCodeCount(Int32 uid)
        {
            return FindCount(__.CreatorID, uid);
        }

        public static EntityList<Invitation> GetUserInviteCodeList(Int32 uid, Int32 pageindex)
        {
            return FindAllByName(__.CreatorID, uid, null, (pageindex - 1) * 10, 10);
        }

        public static Invitation FindByCode(String code)
        {
            return Find(__.Code, code);
        }

        public static Int32 GetTodayUserCreatedInviteCode(Int32 uid)
        {
            return FindCount(__.CreatorID, uid);
        }

        public static int CreateInviteCode(IUser userInfo)
        {
            var cfg = InvitationConfigInfo.Current;
            var entity = new Invitation();
            entity.CreatorID = userInfo.ID;
            entity.Creator = userInfo.Name;
            entity.Code = BuildInviteCode();
            while (entity.Exist(__.Code))
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

        //public static void ClearExpireInviteCode()
        //{
        //    Update(_.IsDeleted == true, _.ExpireTime < DateTime.Now);
        //}
        #endregion
    }
}