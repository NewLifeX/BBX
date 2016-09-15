using System.Collections.Generic;
using BBX.Config;
using BBX.Entity;

namespace BBX.Forum
{
    public class CreditOrders
    {
        public static int CreateCreditOrder(int uId, string buyer, int credit, int amount, int paytype, string outTradeNo)
        {
            if (uId < 0 || string.IsNullOrEmpty(buyer) || credit < 1 || credit > 8 || amount < 1 || string.IsNullOrEmpty(outTradeNo))
            {
                return 0;
            }
            return BBX.Data.CreditOrders.CreateCreditOrder(new CreditOrderInfo
            {
                Uid = uId,
                OrderCode = outTradeNo,
                Amount = amount,
                Credit = credit,
                Buyer = buyer,
                OrderStatus = 0,
                PayType = paytype,
                Price = decimal.Round(amount / GeneralConfigInfo.Current.Cashtocreditrate, 2)
            });
        }

        public static int GetCreditOrderCount(int status, int orderId, string tradeNo, string buyer, string submitStartTime, string submitLastTime, string confirmStartTime, string confirmLastTime)
        {
            return BBX.Data.CreditOrders.GetCreditOrderCount(status, orderId, tradeNo, buyer, submitStartTime, submitLastTime, confirmStartTime, confirmLastTime);
        }

        public static List<CreditOrderInfo> GetCreditOrderList(int pageIndex, int status, int orderId, string tradeNo, string buyer, string submitStartTime, string submitLastTime, string confirmStartTime, string confirmLastTime)
        {
            return BBX.Data.CreditOrders.GetCreditOrderList(pageIndex, status, orderId, tradeNo, buyer, submitStartTime, submitLastTime, confirmStartTime, confirmLastTime);
        }

        public static CreditOrderInfo GetCreditOrderInfoByOrderCode(string orderCode)
        {
            return BBX.Data.CreditOrders.GetCreditOrderByOrderCode(orderCode);
        }

        public static int UpdateCreditOrderInfo(int orderId, string tradeNo, int orderStatus, string confirmedTime)
        {
            return BBX.Data.CreditOrders.UpdateCreditOrderInfo(orderId, tradeNo, orderStatus, confirmedTime);
        }
    }
}