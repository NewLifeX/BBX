using System;
using System.Data;
using System.Web;
using BBX.Common;
using BBX.Entity;
using BBX.Forum;
using BBX.Plugin.Payment;
using BBX.Plugin.Payment.Alipay;
using BBX.Web.UI;

namespace BBX.Web
{
    public class usercpcreditsbuy : UserCpPage
    {
        public float creditstax = Scoresets.GetCreditsTax();
        public string jscreditsratearray = "<script type=\"text/javascript\">\r\nvar creditsrate = new Array();\r\n{0}\r\n</script>";
        public int creditstrans = Scoresets.GetCreditsTrans();
        public string creditstransname = Scoresets.GetValidScoreName()[Scoresets.GetCreditsTrans()];
        public string creditstransunit = Scoresets.GetValidScoreUnit()[Scoresets.GetCreditsTrans()];
        public int creditsamount = DNTRequest.GetInt("amount", 1);

        protected override void ShowPage()
        {
            this.pagetitle = "积分充值";
            if (!EPayments.IsOpenEPayments())
            {
                base.AddErrLine("论坛未开启积分充值服务！");
                return;
            }
            string text = "";
            foreach (DataRow dataRow in Scoresets.GetScorePaySet(0).Rows)
            {
                object obj = text;
                text = obj + "creditsrate[" + dataRow["id"] + "] = " + dataRow["rate"] + ";\r\n";
            }
            this.jscreditsratearray = string.Format(this.jscreditsratearray, text);
            if (!base.IsLogin())
            {
                return;
            }
            string @string;
            if (!string.IsNullOrEmpty(DNTRequest.GetString("redirect")) && (@string = DNTRequest.GetString("redirect")) != null)
            {
                if (!(@string == "alipay"))
                {
                    return;
                }
                this.RedirectToAlipay();
            }
        }

        public void RedirectToAlipay()
        {
            DigitalTrade digitalTrade = new DigitalTrade();
            digitalTrade.Subject = string.Format("{0} 论坛积分充值({1}:{2}{3}),用户:{4}", new object[]
			{
				this.config.Forumtitle,
				this.creditstransname,
				this.creditsamount,
				this.creditstransunit,
				this.username
			});
            if (Utils.IsValidEmail(this.config.Alipayaccout))
            {
                digitalTrade.Seller_Email = this.config.Alipayaccout;
            }
            else
            {
                digitalTrade.Seller_Id = this.config.Alipayaccout;
            }
            digitalTrade.Return_Url = Utils.GetRootUrl(this.forumpath) + "tools/notifypage.aspx";
            digitalTrade.Notify_Url = Utils.GetRootUrl(this.forumpath) + "tools/notifypage.aspx";
            digitalTrade.Quantity = 1;
            decimal num = decimal.Round(this.creditsamount / this.config.Cashtocreditrate, 2);
            digitalTrade.Price = ((num > 0.1m) ? num : 0.1m);
            digitalTrade.Payment_Type = 1;
            digitalTrade.PayMethod = "bankPay";
            digitalTrade.Partner = this.config.Alipaypartnerid;
            digitalTrade.Sign = this.config.Alipaypartnercheckkey;
            string url = StandardAliPayment.GetService().CreateDigitalGoodsTradeUrl(digitalTrade);
            //CreditOrders.CreateCreditOrder(this.userid, this.username, this.creditstrans, this.creditsamount, 1, digitalTrade.Out_Trade_No);
            Order.Create(userid, username, creditstrans, creditsamount, 1, digitalTrade.Out_Trade_No);

            HttpContext.Current.Response.Redirect(url);
        }
    }
}