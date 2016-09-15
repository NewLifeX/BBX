using System;
using BBX.Common;
using BBX.Entity;
using BBX.Forum;
using BBX.Web.UI;

namespace BBX.Web
{
	public class usercpcreditstransfer : UserCpPage
	{
		public float creditstax = Scoresets.GetCreditsTax();
		public string creditstrans = Scoresets.GetCreditsTrans().ToString();

		protected override void ShowPage()
		{
			this.pagetitle = "用户控制面板";
			if (!base.IsLogin())
			{
				return;
			}
			if (DNTRequest.IsPost())
			{
				if (ForumUtils.IsCrossSitePost())
				{
					base.AddErrLine("您的请求来路不正确，无法提交。如果您安装了某种默认屏蔽来路信息的个人防火墙软件(如 Norton Internet Security)，请设置其不要禁止来路信息后再试。");
					return;
				}
				bool flag = true;
				//switch (this.config.Passwordmode)
				//{
				//    case 0:
				flag = Utils.MD5(DNTRequest.GetString("password")).EqualIgnoreCase(this.password);
				//        break;
				//    case 1:
				//        flag = (Utils.MD5(DNTRequest.GetString("password")) != this.password);
				//        break;
				//    default:
				//        if (PasswordModeProvider.GetInstance() != null)
				//        {
				//            flag = !PasswordModeProvider.GetInstance().CheckPassword(this.user, DNTRequest.GetString("password"));
				//        }
				//        break;
				//}
				if (flag)
				{
					base.AddErrLine("密码错误");
					return;
				}
				int paynum = DNTRequest.GetInt("paynum", 0);
				if (paynum <= 0)
				{
					base.AddErrLine("数量必须是大于等于0的整数");
					return;
				}
				int userId = Users.GetUserId(DNTRequest.GetString("fromto").Trim());
				if (userId <= 0)
				{
					base.AddErrLine("指定的转帐接受人不存在");
					return;
				}
				int int2 = DNTRequest.GetInt("extcredits", 0);
				if (int2 < 1 || int2 > 8)
				{
					base.AddErrLine("请正确选择要转帐的积分类型!");
					return;
				}
				string text = Scoresets.GetScoreSet(int2).Name.Trim();
				if (Utils.StrIsNullOrEmpty(text))
				{
					base.AddErrLine("错误的输入!");
					return;
				}
				if (Users.GetUserExtCredits(this.userid, int2) - (float)paynum < (float)Scoresets.GetTransferMinCredits())
				{
					base.AddErrLine(string.Format("抱歉, 您的 \"{0}\" 不足.系统当前规定转帐余额不得小于{1}", text, Scoresets.GetTransferMinCredits().ToString()));
					return;
				}
				var num = (Int32)Math.Round((double)((float)paynum * (1f - this.creditstax)), 2);
				BBX.Entity.User.UpdateUserExtCredits(this.userid, int2, (float)(paynum * -1));
				BBX.Entity.User.UpdateUserExtCredits(userId, int2, num);
				//CreditsLogs.AddCreditsLog(this.userid, userId, int2, int2, (float)@int, num, Utils.GetDateTime(), 2);
				CreditsLog.Add(this.userid, userId, int2, int2, paynum, num, 2);
				base.SetUrl("usercpcreaditstransferlog.aspx");
				base.SetMetaRefresh();
				base.SetShowBackLink(false);
				base.AddMsgLine("积分转帐完毕, 正在返回积分兑换与转帐记录");
			}
		}
	}
}