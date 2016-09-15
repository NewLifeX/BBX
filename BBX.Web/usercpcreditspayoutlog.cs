using System.Data;
using BBX.Entity;
using BBX.Web.UI;
using XCode;

namespace BBX.Web
{
    public class usercpcreditspayoutlog : UserCpPage
    {
        public EntityList<PaymentLog> payloglist;
        public int payoutlogcount;

        protected override void ShowPage()
        {
            this.pagetitle = "用户控制面板";
            if (!base.IsLogin())
            {
                return;
            }
            this.payoutlogcount = PaymentLog.GetPaymentLogOutRecordCount(this.userid);
            base.BindItems(this.payoutlogcount);
            this.payloglist = PaymentLog.GetPayLogOutList(16, this.pageid, this.userid);
        }
    }
}