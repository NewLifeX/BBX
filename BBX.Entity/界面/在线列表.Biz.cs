﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;
using BBX.Config;
using NewLife.Log;
using NewLife.Web;
using XCode;
using XCode.Configuration;

namespace BBX.Entity
{
    /// <summary>在线列表</summary>
    public partial class OnlineList : EntityBase<OnlineList>
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
            if (XTrace.Debug) XTrace.WriteLine("开始初始化{0}在线列表数据……", typeof(OnlineList).Name);

            Add(0, 999, "用户", "member.gif");
            Add(1, 1, "管理员", "admin.gif");
            Add(2, 2, "超级版主", "supermoder.gif");
            Add(3, 3, "版主", "moder.gif");
            Add(4, 4, "禁止发言", "");
            Add(5, 5, "禁止访问", "");
            Add(6, 6, "禁止 IP", "");
            Add(7, 7, "游客", "guest.gif");
            Add(8, 8, "等待验证会员", "");
            Add(9, 9, "乞丐", "");
            Add(10, 10, "新手上路", "");
            Add(11, 11, "注册会员", "");
            Add(12, 12, "中级会员", "");
            Add(13, 13, "高级会员", "");
            Add(14, 14, "金牌会员", "");
            Add(15, 15, "论坛元老", "");

            if (XTrace.Debug) XTrace.WriteLine("完成初始化{0}在线列表数据！", typeof(OnlineList).Name);
        }

        static void Add(Int16 gid, Int32 order, String name, String img)
        {
            var entity = new OnlineList();
            entity.GroupID = gid;
            entity.DisplayOrder = order;
            entity.Title = name;
            entity.Img = img;
            entity.Insert();
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
        private static OnlineList _Default;
        /// <summary>默认</summary>
        public static OnlineList Default
        {
            get
            {
                if (_Default == null)
                {
                    _Default = GetGroupIcon(0);
                    if (_Default == null)
                    {
                        _Default = new OnlineList();
                        _Default.Img = "member.gif";
                        _Default.Save();
                    }
                }
                return _Default;
            }
        }
        #endregion

        #region 扩展查询﻿
        /// <summary>根据编号查找</summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static OnlineList FindByGroupID(Int32 id)
        {
            if (Meta.Count >= 1000)
                return Find(__.GroupID, id);
            else // 实体缓存
                return Meta.Cache.Entities.Find(__.GroupID, id);
            // 单对象缓存
            //return Meta.SingleCache[id];
        }
        #endregion

        #region 高级查询
        public static OnlineList GetGroupIcon(Int32 groupid)
        {
            if (Meta.Count > 1000)
                return Find(_.GroupID == groupid);
            else
                return Meta.Cache.Entities.Find(__.GroupID, groupid);
        }

        public static EntityList<OnlineList> GetGroupIcons()
        {
            if (Meta.Count > 1000)
                //return FindAll(_.Img.NotIsNullOrEmpty(), _.DisplayOrder.Asc(), null, 0, 0);
                return FindAll(_.Img.NotIsNull(), _.DisplayOrder.Asc(), null, 0, 0).FindAll(e => !String.IsNullOrEmpty(e.Img));
            else
                return Meta.Cache.Entities.FindAll(e => !String.IsNullOrEmpty(e.Img));
        }
        #endregion

        #region 扩展操作
        #endregion

        #region 业务
        public static OnlineList Add(Int32 groupid, String title)
        {
            var entity = new OnlineList();
            entity.GroupID = (Int16)groupid;
            entity.Title = title;
            entity.Save();

            return entity;
        }


        public static string GetOnlineGroupIconList()
        {

            var sb = new StringBuilder();
            var list = FindAllWithCache().ToList().Where(e => !e.Img.IsNullOrWhiteSpace());
            foreach (var item in list)
            {
                sb.AppendFormat("<img src=\"{0}images/groupicons/{1}\" /> {2} &nbsp; &nbsp; &nbsp; ", BaseConfigs.GetForumPath, item.Img, item.Title);
            }
            return sb.ToString();
        }
        #endregion
    }
}