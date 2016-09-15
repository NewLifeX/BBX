using System;
using System.Web;
using System.Web.UI;
using BBX.Common;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web.UI
{
	public class NotifyPage : Page
	{
		public NotifyPage()
		{
			if (!EPayments.CheckPayment(DNTRequest.GetString("notify_id")))
			{
				HttpContext.Current.Response.Write("fail");
				return;
			}
			int status = EPayments.ConvertAlipayTradeStatus(DNTRequest.GetString("trade_status"));
			string outTradeNo = DNTRequest.GetString("out_trade_no", true);
			string tradeNo = DNTRequest.GetString("trade_no", true);
			if (string.IsNullOrEmpty(outTradeNo) || string.IsNullOrEmpty(tradeNo) || status <= 0)
			{
				return;
			}
            //var ci = CreditOrders.GetCreditOrderInfoByOrderCode(outTradeNo);
            var ci = Order.FindByCode(outTradeNo);
			if (ci != null && ci.Status < 2)
			{
				float[] array = new float[8];
				array[ci.Credit - 1] = (float)ci.Amount;
				if (CreditsFacade.UpdateUserExtCredits(ci.Uid, array, true) != 1)
				{
					status = 0;
				}
				//CreditsLogs.AddCreditsLog(ci.Uid, ci.Uid, ci.Credit, ci.Credit, 0f, (float)ci.Amount, Utils.GetDateTime(), 3);
				CreditsLog.Add(ci.Uid, ci.Uid, ci.Credit, ci.Credit, 0, ci.Amount, 3);
				var notice = new Notice
				{
					PostDateTime = DateTime.Now,
					Type = (Int32)NoticeType.GoodsTradeNotice,
					Poster = "系统",
					PosterID = 0,
					Uid = ci.Uid,
					Note = string.Format("您购买的积分 {0} 已经成功充值，请<a href=\"usercpcreaditstransferlog.aspx\">查收</a>!(支付宝订单号:{1})", ForumUtils.ConvertCreditAndAmountToWord(ci.Credit, ci.Amount), tradeNo)
				};
				notice.Insert();

                //CreditOrders.UpdateCreditOrderInfo(ci.OrderId, tradeNo, num, Utils.GetDateTime());
                ci.TradeNo = tradeNo;
                ci.Status = status;
                ci.ConfirmedTime = DateTime.Now;
                ci.Update();
			}
			if (DNTRequest.IsPost())
			{
				HttpContext.Current.Response.Write("success");
				return;
			}
			HttpContext.Current.Response.Redirect("../usercpcreaditstransferlog.aspx?paysuccess=true");
		}
	}
}