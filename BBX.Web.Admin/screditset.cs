using System;
using System.Web;
using BBX.Common;
using BBX.Config;
using BBX.Plugin.Payment;
using BBX.Plugin.Payment.Alipay;

namespace BBX.Web.Admin
{
    public class screditset : AdminPage
    {
        public GeneralConfigInfo configInfo = GeneralConfigInfo.Current;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsFounderUid(this.userid))
            {
                base.Response.Write(base.GetShowMessage());
                base.Response.End();
                return;
            }
            if (!string.IsNullOrEmpty(Request["accout"]))
            {
                this.TestAccout(Request["accout"]);
            }
            if (base.IsPostBack)
            {
                this.configInfo.Alipayaccout = DNTRequest.GetFormString("alipayaccount");
                this.configInfo.Cashtocreditrate = DNTRequest.GetFormInt("cashtocreditsrate", 0);
                int num = DNTRequest.GetFormInt("mincreditstobuy", 0);
                if (this.configInfo.Cashtocreditrate > 0)
                {
                    while (num / this.configInfo.Cashtocreditrate < 0.10m)
                    {
                        num++;
                    }
                }
                this.configInfo.Mincreditstobuy = num;
                this.configInfo.Maxcreditstobuy = DNTRequest.GetFormInt("maxcreditstobuy", 0);
                this.configInfo.Userbuycreditscountperday = DNTRequest.GetFormInt("userbuycreditscountperday", 0);
                this.configInfo.Alipaypartnercheckkey = DNTRequest.GetFormString("alipaypartnercheckkey");
                this.configInfo.Alipaypartnerid = DNTRequest.GetFormString("alipaypartnerid");
                this.configInfo.Usealipaycustompartnerid = DNTRequest.GetFormInt("usealipaycustompartnerid", 1);
                this.configInfo.Usealipayinstantpay = DNTRequest.GetFormInt("usealipayinstantpay", 0);

                //GeneralConfigs.SaveConfig(this.configInfo);
                //GeneralConfigs.ResetConfig();
                configInfo.Save();
                GeneralConfigInfo.Current = null;
                base.RegisterStartupScript("PAGE", "window.location.href='global_screditset.aspx';");
            }
        }

        public void TestAccout(string accout)
        {
            int @int = DNTRequest.GetInt("openpartner", 0);
            string @string = Request["partnerid"];
            string string2 = Request["partnerKey"];
            DigitalTrade digitalTrade = new DigitalTrade();
            digitalTrade.Subject = "测试支付宝充值功能";
            if (Utils.IsValidEmail(accout))
            {
                digitalTrade.Seller_Email = accout;
            }
            else
            {
                digitalTrade.Seller_Id = accout;
            }
            digitalTrade.Return_Url = Utils.GetRootUrl(BaseConfigs.GetForumPath) + "tools/notifypage.aspx";
            digitalTrade.Notify_Url = Utils.GetRootUrl(BaseConfigs.GetForumPath) + "tools/notifypage.aspx";
            digitalTrade.Quantity = 1;
            digitalTrade.Price = 0.1m;
            digitalTrade.Payment_Type = 1;
            digitalTrade.PayMethod = "bankPay";
            string url;
            if (@int == 1)
            {
                digitalTrade.Partner = @string;
                digitalTrade.Sign = string2;
                url = StandardAliPayment.GetService().CreateDigitalGoodsTradeUrl(digitalTrade);
            }
            else
            {
                url = AliPayment.GetService().CreateDigitalGoodsTradeUrl(digitalTrade);
            }
            HttpContext.Current.Response.Redirect(url);
        }
    }
}