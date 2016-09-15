/*
 * XCoder v5.1.4974.18563
 * 作者：nnhy/X2
 * 时间：2013-08-26 19:56:16
 * 版权：版权所有 (C) 新生命开发团队 2002~2013
*/
﻿using System;
using System.ComponentModel;
using XCode;

namespace BBX.Entity
{
    /// <summary>订单</summary>
    public partial class Order : Entity<Order>
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
            //if (isNew || Dirtys[__.Name]) CheckExist(__.Name);

            // 货币保留6位小数
            if (Dirtys[__.Price]) Price = Math.Round(Price, 6);
            if (isNew && !Dirtys[__.CreatedTime]) CreatedTime = DateTime.Now;
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
        //    if (XTrace.Debug) XTrace.WriteLine("开始初始化{0}[{1}]数据……", typeof(Order).Name, Meta.Table.DataTable.DisplayName);

        //    var entity = new Order();
        //    entity.Code = "abc";
        //    entity.Uid = 0;
        //    entity.Buyer = "abc";
        //    entity.PayType = 0;
        //    entity.Tradeno = "abc";
        //    entity.Price = 0;
        //    entity.Tatus = 0;
        //    entity.CreatedTime = DateTime.Now;
        //    entity.ConfirmedTime = DateTime.Now;
        //    entity.Credit = 0;
        //    entity.Amount = 0;
        //    entity.Insert();

        //    if (XTrace.Debug) XTrace.WriteLine("完成初始化{0}[{1}]数据！", typeof(Order).Name, Meta.Table.DataTable.DisplayName);
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
        /// <summary>根据代码查找</summary>
        /// <param name="code">代码</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static Order FindByCode(String code)
        {
            if (Meta.Count >= 1000)
                return Find(__.Code, code);
            else // 实体缓存
                return Meta.Cache.Entities.Find(__.Code, code);
        }

        /// <summary>根据编号查找</summary>
        /// <param name="id">编号</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static Order FindByID(Int32 id)
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
        public static EntityList<Order> Search(Int32 status, Int32 orderId, String tradeNo, String buyer, DateTime submitStartTime, DateTime submitLastTime, DateTime confirmStartTime, DateTime confirmLastTime, String orderClause, Int32 startRowIndex, Int32 maximumRows)
        {
            return FindAll(SearchWhere(status, orderId, tradeNo, buyer, submitStartTime, submitLastTime, confirmStartTime, confirmLastTime), orderClause, null, startRowIndex, maximumRows);
        }

        public static Int32 SearchCount(Int32 status, Int32 orderId, String tradeNo, String buyer, DateTime submitStartTime, DateTime submitLastTime, DateTime confirmStartTime, DateTime confirmLastTime, String orderClause = null, Int32 startRowIndex = 0, Int32 maximumRows = -1)
        {
            return FindCount(SearchWhere(status, orderId, tradeNo, buyer, submitStartTime, submitLastTime, confirmStartTime, confirmLastTime), null, null, 0, 0);
        }

        private static String SearchWhere(Int32 status, Int32 orderId, String tradeNo, String buyer, DateTime submitStartTime, DateTime submitLastTime, DateTime confirmStartTime, DateTime confirmLastTime)
        {
            var exp = new WhereExpression();

            if (status > 0) exp &= _.Status == status;
            //if(orderId>0)exp&=_.or
            if (!tradeNo.IsNullOrWhiteSpace()) exp &= _.TradeNo == tradeNo;
            if (!buyer.IsNullOrWhiteSpace()) exp &= _.Buyer == buyer;
            exp &= _.CreatedTime.Between(submitStartTime, submitLastTime);
            exp &= _.ConfirmedTime.Between(confirmStartTime, confirmLastTime);

            return exp;
        }
        #endregion

        #region 扩展操作
        #endregion

        #region 业务
        public static Order Create(Int32 uId, String buyer, Int32 credit, Int32 amount, Int32 paytype, String outTradeNo)
        {
            if (uId < 0 || String.IsNullOrEmpty(buyer) || credit < 1 || credit > 8 || amount < 1 || String.IsNullOrEmpty(outTradeNo)) return null;

            var entity = new Order();
            entity.Uid = uId;
            entity.Buyer = buyer;
            entity.Credit = credit;
            entity.Amount = amount;
            entity.PayType = paytype;
            entity.TradeNo = outTradeNo;
            entity.Insert();

            return entity;
        }
        #endregion
    }
}