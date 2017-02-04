using System;
using System.Data;
using BBX.Common;
using BBX.Entity;
using BBX.Forum;
using BBX.Web.UI;

namespace BBX.Web
{
    public class usercpcreditspay : UserCpPage
    {
        public float creditstax = Scoresets.GetCreditsTax();
        public string jscreditsratearray = "<script type=\"text/javascript\">\r\nvar creditsrate = new Array();\r\n{0}\r\n</script>";
        public int creditstrans = Scoresets.GetCreditsTrans();
        public string creditstransname = Scoresets.GetValidScoreName()[Scoresets.GetCreditsTrans()];
        public string creditstransunit = Scoresets.GetValidScoreUnit()[Scoresets.GetCreditsTrans()];
        public int creditsamount = DNTRequest.GetInt("amount", 1);

        protected override void ShowPage()
        {
            this.pagetitle = "用户控制面板";
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
                    base.AddErrLine("数量必须是大于0的整数");
                    return;
                }
                if (DNTRequest.GetInt("extcredits1", 0) < 1 || DNTRequest.GetInt("extcredits2", 0) < 1 || DNTRequest.GetInt("extcredits1", 0) > 8 || DNTRequest.GetInt("extcredits2", 0) > 8)
                {
                    base.AddErrLine("请正确选择要兑换的积分类型!");
                    return;
                }
                if (DNTRequest.GetInt("extcredits1", 0) == DNTRequest.GetInt("extcredits2", 0))
                {
                    base.AddErrLine("不能兑换相同类型的积分");
                    return;
                }
                var scoreSet = Scoresets.GetScoreSet(DNTRequest.GetInt("extcredits1", 0));
                var scoreSet2 = Scoresets.GetScoreSet(DNTRequest.GetInt("extcredits2", 0));
                if (scoreSet.Name.IsNullOrEmpty() || scoreSet2.Name.IsNullOrEmpty())
                {
                    base.AddErrLine("错误的输入!");
                    return;
                }
                if (Users.GetUserExtCredits(this.userid, DNTRequest.GetInt("extcredits1", 0)) - (float)paynum < (float)Scoresets.GetExchangeMinCredits())
                {
                    base.AddErrLine("抱歉, 您的 \"" + scoreSet.Name + "\" 不足.系统当前规定转帐余额不得小于" + Scoresets.GetExchangeMinCredits());
                    return;
                }
                Users.GetUserInfo(this.userid);
                var num = (Int32)Math.Round((double)((float)paynum * (scoreSet.Rate / scoreSet2.Rate) * (1f - this.creditstax)), 2);
                BBX.Entity.User.UpdateUserExtCredits(this.userid, DNTRequest.GetInt("extcredits1", 0), paynum * -1);
                BBX.Entity.User.UpdateUserExtCredits(this.userid, DNTRequest.GetInt("extcredits2", 0), num);
                //CreditsLogs.AddCreditsLog(this.userid, this.userid, DNTRequest.GetInt("extcredits1", 0), DNTRequest.GetInt("extcredits2", 0), (float)@int, num, Utils.GetDateTime(), 1);
                CreditsLog.Add(userid, userid, DNTRequest.GetInt("extcredits1", 0), DNTRequest.GetInt("extcredits2", 0), paynum, num, 1);
                base.SetUrl("usercpcreaditstransferlog.aspx");
                base.SetMetaRefresh();
                base.SetShowBackLink(false);
                base.AddMsgLine("积分兑换完毕, 正在返回积分兑换与转帐记录");
            }
        }
    }
}