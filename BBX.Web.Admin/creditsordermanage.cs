using System;
using System.Collections.Generic;
using BBX.Common;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web.Admin
{
    public class creditsordermanage : AdminPage
    {
        public List<Order> orderList;
        public int status = DNTRequest.GetInt("orderstatus", -1);
        public int orderId = DNTRequest.GetInt("orderid", 0);
        public string tradeNo = DNTRequest.GetString("tradeno", true);
        public string buyer;
        public string submitStartDate;
        public string submitLastDate;
        public string confirmedStartDate;
        public string confirmedLastDate;
        public int pageIndex = DNTRequest.GetInt("page", 1);
        public int pageCount;
        public int orderCount;

        protected void Page_Load(object sender, EventArgs e)
        {
            buyer = DNTRequest.GetString("buyer");
            submitStartDate = Utils.IsDateString(Request["submitstartdate"]) ? Request["submitstartdate"] : "";
            submitLastDate = Utils.IsDateString(Request["submitlastdate"]) ? Request["submitlastdate"] : "";
            confirmedStartDate = Utils.IsDateString(Request["confirmstartdate"]) ? Request["confirmstartdate"] : "";
            confirmedLastDate = Utils.IsDateString(Request["confirmlastdate"]) ? Request["confirmlastdate"] : "";

            this.orderList = Order.Search(this.status, this.orderId, this.tradeNo, this.buyer, this.submitStartDate.ToDateTime(), this.submitLastDate.ToDateTime(), this.confirmedStartDate.ToDateTime(), this.confirmedLastDate.ToDateTime(), null, (pageIndex - 1) * 20, 20);
            this.orderCount = Order.SearchCount(this.status, this.orderId, this.tradeNo, this.buyer, this.submitStartDate.ToDateTime(), this.submitLastDate.ToDateTime(), this.confirmedStartDate.ToDateTime(), this.confirmedLastDate.ToDateTime());
            this.pageCount = (this.orderCount - 1) / 20 + 1;
        }

        public string ConvertStatusNoToWord(int number)
        {
            switch (number)
            {
                case 0:
                    return "等待付款";
                case 1:
                    return "已付款，等待发货";
                case 2:
                    return "交易成功";
                default:
                    return "未知状态";
            }
        }

        public string ShowPageIndex()
        {
            string text = "";
            int num = (this.pageIndex - 5 > 0) ? (this.pageIndex - 5 - ((this.pageIndex + 5 < this.pageCount) ? 0 : (this.pageIndex + 5 - this.pageCount))) : 1;
            int num2 = (this.pageIndex + 5 < this.pageCount) ? (this.pageIndex + 5 + ((this.pageIndex - 5 > 0) ? 0 : ((this.pageIndex - 5) * -1 + 1))) : this.pageCount;
            for (int i = num; i <= num2; i++)
            {
                if (i != this.pageIndex)
                {
                    text += string.Format("<td style=\"height:20px;line-height:20px;\"><a style=\"border:1px solid #E8E8E8;padding:2px 4px;margin-right:2px;\" href=\"###\" onclick=\"goPageIndex({0})\">{0}</a></td>", i);
                }
                else
                {
                    text += string.Format("<td style=\"height:20px;line-height:20px;font-weight:700;\"><span style=\"border:1px solid #E8E8E8;padding:2px 4px;margin-right:2px;background:#09C;color:#FFF\" >{0}</span></td> ", i);
                }
            }
            return text;
        }
    }
}