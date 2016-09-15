using System.Data;
using BBX.Forum;
using BBX.Web.UI;
using BBX.Entity;
using XCode;

namespace BBX.Web
{
    public class usercpcreditspayinlog : UserCpPage
    {
        public EntityList<PaymentLog> payloglist;
        public int payinlogcount;

        protected override void ShowPage()
        {
            this.pagetitle = "用户控制面板";
            if (!base.IsLogin())
            {
                return;
            }
            this.payinlogcount = PaymentLog.GetPaymentLogInRecordCount(this.userid);
            base.BindItems(this.payinlogcount);
            this.payloglist = PaymentLog.GetPayLogInList(16, this.pageid, this.userid);
        }
    }
}