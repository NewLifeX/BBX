using System.Data;
using BBX.Common;
using BBX.Entity;
using BBX.Forum;
using BBX.Web.UI;

namespace BBX.Web
{
	public class usercpcreaditstransferlog : UserCpPage
	{
		public int creditslogcount;
		public DataTable creditsloglist = new DataTable();
		public bool isshowmsg = !string.IsNullOrEmpty(DNTRequest.GetString("paysuccess"));

		protected override void ShowPage()
		{
			this.pagetitle = "用户控制面板";
			if (!base.IsLogin())
			{
				return;
			}
			if (this.isshowmsg)
			{
				base.SetUrl("usercpcreaditstransferlog.aspx");
				base.SetMetaRefresh(5);
				base.SetShowBackLink(false);
				base.AddMsgLine("积分充值操作完成，充值成功后会发送站内通知告知");
				return;
			}
			//this.creditslogcount = CreditsLogs.GetCreditsLogRecordCount(this.userid);
			this.creditslogcount = CreditsLog.SearchCount(this.userid);
			base.BindItems(this.creditslogcount);
			//this.creditsloglist = CreditsLogs.GetCreditsLogList(16, this.pageid, this.userid);
			this.creditsloglist = CreditsLog.Search(this.userid, (pageid - 1) * 16, 16).ToDataTable(false);
		}
	}
}