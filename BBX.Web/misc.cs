using System;
using System.Text;
using BBX.Common;
using BBX.Entity;
using BBX.Forum;
using XCode;

namespace BBX.Web
{
    public class misc : PageBase
    {
        protected int tid = DNTRequest.GetInt("tid", 0);
        protected string emailcontent = "";
        protected string topictitle = "";
        protected string title = "";
        protected string action = DNTRequest.GetString("action");
        protected string voters = "";
        protected Int32 polloptionid = DNTRequest.GetInt("polloptionid", 0);
        protected EntityList<PollOption> pollOptionList;

        protected override void ShowPage()
        {
            if (!this.ispost)
            {
                if (this.tid <= 0)
                {
                    base.AddErrLine("不存在的主题ID");
                    return;
                }
                var topicInfo = Topic.FindByID(this.tid);
                if (topicInfo == null)
                {
                    base.AddErrLine("不存在的主题");
                    return;
                }
                string a;
                if ((a = this.action) != null)
                {
                    if (!(a == "emailfriend"))
                    {
                        if (!(a == "viewvote"))
                        {
                            return;
                        }
                        this.title = "参与投票的会员";

                        var pollInfo = Poll.FindByTid(tid);
                        if (pollInfo == null)
                        {
                            base.AddErrLine("不存在的调查");
                            return;
                        }
                        if (pollInfo.AllowView != true && pollInfo.Uid != this.userid && !Moderators.IsModer(this.useradminid, this.userid, topicInfo.Fid))
                        {
                            base.AddErrLine("您没有查看投票人的权限");
                            return;
                        }
                        this.pollOptionList = PollOption.FindAllByTid(tid);

                        if (polloptionid == 0)
                        {
                            bool flag;
                            this.voters = GetVoters(this.tid, this.userid, this.username, out flag);
                            return;
                        }
                        foreach (var item in pollOptionList)
                        {
                            if (item.ID == polloptionid)
                            {
                                string[] array = Utils.SplitString(item.VoterNames.Trim(), " <");
                                string[] array2 = array;
                                for (int i = 0; i < array2.Length; i++)
                                {
                                    string text = array2[i];
                                    this.voters = this.voters + "<li>" + (text.StartsWith("<") ? text : ("<" + text)) + "</li>";
                                }
                            }
                        }
                        if (String.IsNullOrEmpty(this.voters))
                        {
                            this.voters = "<li>暂无人投票</li>";
                            return;
                        }
                    }
                    else
                    {
                        this.title = "分享";
                        this.emailcontent = "你好！我在 {0} 看到了这篇帖子，认为很有价值，特推荐给你。\r\n{1}\r\n地址 {2}\r\n希望你能喜欢。";
                        if (topicInfo != null)
                        {
                            this.topictitle = topicInfo.Title;
                            this.emailcontent = string.Format(this.emailcontent, this.config.Forumtitle, this.topictitle, DNTRequest.GetUrlReferrer());
                            return;
                        }
                    }
                }
            }
            else
            {
                this.SendEmail();
            }
        }

        private void SendEmail()
        {
            string sendtoemail = DNTRequest.GetString("sendtoemail");
            if (String.IsNullOrEmpty(sendtoemail))
            {
                base.AddErrLine("接收者的Email不能为空");
                return;
            }
            if (!Utils.IsValidEmail(sendtoemail))
            {
                base.AddErrLine("接收者的Email不正确");
                return;
            }
            string body = string.Format("这封信是由 {0} 的 {1} 发送的。\r\n\r\n您收到这封邮件，是因为在 {1} 通过 {0} 的“推荐给朋友”\r\n功能推荐了如下的内容给您，如果您对此不感兴趣，请忽略这封邮件。您不\r\n需要退订或进行其他进一步的操作。\r\n\r\n----------------------------------------------------------------------\r\n信件原文开始\r\n----------------------------------------------------------------------\r\n\r\n{2}\r\n\r\n----------------------------------------------------------------------\r\n信件原文结束\r\n----------------------------------------------------------------------\r\n\r\n请注意这封信仅仅是由用户使用 “推荐给朋友”发送的，不是论坛官方邮件，\r\n论坛管理团队不会对这类邮件负责。\r\n\r\n欢迎您访问 {0}\r\n{3}", new object[]
			{
				this.config.Forumtitle,
				this.username,
				DNTRequest.GetString("message"),
				Utils.GetRootUrl(this.forumpath)
			});
            var tp = Topic.FindByID(tid);
            Emails.SendMailToUser(sendtoemail, string.Format("[{0}] {1} 推荐给您: {2} ", this.config.Forumtitle, this.username, tp.Title), body);
        }

        //直接从原来的方法拷贝过来，进行了修改
        public static string GetVoters(int tid, int userid, string username, out bool allowvote)
        {
            var poll = Poll.FindByTid(tid);
            string pollUserNameList = poll == null ? "" : poll.Voternames;// BBX.Data.Polls.GetPollUserNameList(tid);
            allowvote = true;
            if (Utils.StrIsNullOrEmpty(pollUserNameList))
            {
                return "<li>暂无人投票</li>";
            }
            string[] array = Utils.SplitString(pollUserNameList.Trim(), "\r\n");
            var sb = new StringBuilder();
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].Trim() == username)
                {
                    allowvote = false;
                }
                if (userid == -1 && Utils.InArray(tid.ToString(), ForumUtils.GetCookie("polled")))
                {
                    allowvote = false;
                }
                if (array[i].IndexOf(' ') == -1)
                {
                    sb.AppendFormat("<li><a href=\"userinfo.aspx?username={0}\">{1}</a></li>", Utils.UrlEncode(array[i].Trim()), array[i]);
                }
                else
                {
                    sb.Append(array[i].Substring(0, array[i].LastIndexOf(".") + 1).Trim().Replace(" ", string.Empty) + "]");
                }
                sb.Append("&nbsp; ");
            }
            return sb.ToString();
        }
    }
}