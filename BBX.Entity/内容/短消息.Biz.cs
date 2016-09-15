using System;
using System.ComponentModel;
using XCode;

namespace BBX.Entity
{
    /// <summary>项目经理</summary>
    public partial class ShortMessage : Entity<ShortMessage>
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

            if (isNew)
            {
                if (!Dirtys[__.New]) New = true;
                if (!Dirtys[__.PostDateTime]) PostDateTime = DateTime.Now;
            }
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
        //    if (XTrace.Debug) XTrace.WriteLine("开始初始化{0}项目经理数据……", typeof(Pm).Name);

        //    var entity = new Pm();
        //    entity.Msgfrom = "abc";
        //    entity.MsgfromID = 0;
        //    entity.Msgto = "abc";
        //    entity.MsgtoID = 0;
        //    entity.Folder = 0;
        //    entity.New = 0;
        //    entity.Subject = "abc";
        //    entity.PostDateTime = DateTime.Now;
        //    entity.Message = "abc";
        //    entity.Insert();

        //    if (XTrace.Debug) XTrace.WriteLine("完成初始化{0}项目经理数据！", typeof(Pm).Name);
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
        /// <summary>根据收件人UID查找</summary>
        /// <param name="msgtoid">收件人UID</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static EntityList<ShortMessage> FindAllByMsgtoID(Int32 msgtoid)
        {
            if (Meta.Count >= 1000)
                return FindAll(__.MsgtoID, msgtoid);
            else // 实体缓存
                return Meta.Cache.Entities.FindAll(__.MsgtoID, msgtoid);
        }

        public static EntityList<ShortMessage> FindAllByMsgfromID(Int32 uid)
        {
            if (Meta.Count >= 1000)
                return FindAll(__.MsgfromID, uid);
            else // 实体缓存
                return Meta.Cache.Entities.FindAll(__.MsgfromID, uid);
        }

        /// <summary>根据编号查找</summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static ShortMessage FindByID(Int32 id)
        {
            if (Meta.Count >= 1000)
                return Find(__.ID, id);
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
        public static int GetAnnouncePrivateMessageCount()
        {
            //return TypeConverter.ObjectToInt(DbHelper.ExecuteScalarInMasterDB(CommandType.Text, string.Format("SELECT COUNT(pmid) FROM [{0}pms] WHERE [msgtoid] = 0", TablePrefix)));
            return FindCount(_.MsgtoID, 0);
        }

        public static int DeletePrivateMessage(int userId, String pmitemids)
        {
            if (String.IsNullOrEmpty(pmitemids)) return -1;

            var list = FindAll(_.ID.In(pmitemids.SplitAsInt()), null, null, 0, 0);
            var count = 0;
            foreach (var item in list)
            {
                if (item.MsgtoID == userId || item.MsgfromID == userId)
                {
                    item.Delete();
                    count++;
                }
            }

            return count;
        }

        public static int GetNewPMCount(int userId)
        {
            //string commandText = string.Format("SELECT COUNT([pmid]) AS [pmcount] FROM [{0}pms] WHERE [new] = 1 AND [folder] = 0 AND [msgtoid] = {1}", TablePrefix, userId);
            //return TypeConverter.ObjectToInt(DbHelper.ExecuteScalar(CommandType.Text, commandText));
            return FindCount(_.New == 1 & _.Folder == 0 & _.MsgtoID == userId, null, null, 0, 0);
        }

        public static int GetPrivateMessageCount(int userId, int folder, int state = -1)
        {
            if (folder == -1)
            {
                var exp = (_.MsgtoID == userId & _.Folder == 0)
                    | (_.MsgfromID == userId & _.Folder == 1)
                    | (_.MsgfromID == userId & _.Folder == 2);
                return FindCount(exp, null, null, 0, 0);
            }
            else
            {
                var fi = _.MsgtoID;
                if (folder != 0) fi = _.MsgfromID;
               
                var exp = fi == userId & _.Folder == folder;
                if (state == 2)
                    exp &= _.New == 1 & _.PostDateTime > DateTime.Now.AddDays(-3);
                else if (state != -1)
                    exp &= _.New == state;
                //todo 异常，应该是消息为0条时产生的，Mysql数据库
                return FindCount(exp, null, null, 0, 0);
            }
        }

        public static int DeletePrivateMessages(bool isNew, Int32 postDateTime, string msgFromList, bool lowerUpper, string subject, string message, bool isUpdateUserNewPm)
        {
            var exp = _.ID > 0;
            if (isNew) exp &= _.New == 0;
            if (postDateTime >= 0) exp &= _.PostDateTime >= DateTime.Now.AddDays(postDateTime);
            if (!String.IsNullOrEmpty(msgFromList)) exp &= _.Msgfrom.In(msgFromList.Split(","));
            if (!String.IsNullOrEmpty(subject)) exp &= _.Subject.Contains(subject);
            if (!String.IsNullOrEmpty(message)) exp &= _.Message.Contains(message);

            var list = FindAll(exp, null, null, 0, 0);
            list.Delete();

            if (isUpdateUserNewPm)
            {
                foreach (var item in list)
                {
                    var user = User.FindByID(item.MsgtoID);
                    if (user != null)
                    {
                        user.Newpm = true;
                        user.Update();
                    }
                }
            }

            return list.Count;
        }

        public static EntityList<ShortMessage> GetList(int userId, int folder, int pageSize, int pageIndex, int intType)
        {
            var fi = _.MsgfromID;
            if (folder == 0) fi = _.MsgtoID;

            var exp = _.Folder == folder;
            exp &= fi == userId;

            if (intType == 1) exp &= _.New == 1;

            return FindAll(exp, _.ID.Desc(), null, (pageIndex - 1) * pageSize, pageSize);
        }

        public static EntityList<ShortMessage> GetAnnouncePrivateMessageList(int pageSize, int pageIndex)
        {
            //if (pageSize == -1) return FindAll(_.MsgtoID == 0, _.ID.Desc(), null, 0, 0);

            return FindAll(_.MsgtoID == 0, _.ID.Desc(), null, (pageIndex - 1) * pageSize, pageSize);
        }

        /// <summary>创建短消息</summary>
        /// <param name="savetosentbox">是否保存到发件箱</param>
        public void Create(Boolean savetosentbox = false)
        {
            this.Insert();

            var user = User.FindByID(this.MsgtoID);
            if (user != null)
            {
                user.NewpmCount++;
                user.Newpm = true;
                user.Save();
            }

            // 保存到发件箱
            if (savetosentbox)
            {
                Folder = 1;
                this.Insert();
            }
        }

        public static void UpdatePMSenderAndReceiver(int uid, string newUserName)
        {
            Update(_.Msgto == newUserName, _.MsgtoID == uid);
            Update(_.Msgfrom == newUserName, _.MsgfromID == uid);
        }
        #endregion
    }
}