using System.Threading;
using Discuz.Config;

namespace Discuz.Forum
{
    /// <summary>多线程发送邮件</summary>
    public class EmailMultiThread
    {
        private string m_username = "";
        public string UserName { get { return m_username; } }

        private string m_email = "";
        public string Email { get { return m_email; } }

        private string m_title = "";
        public string Title { get { return m_title; } }

        private string m_body = "";
        public string Body { get { return m_body; } }

        private bool m_issuccess;
        public bool IsSuccess { get { return m_issuccess; } set { m_issuccess = value; } }

        public EmailMultiThread(string UserName, string Email, string Title, string Body)
        {
            this.m_username = UserName;
            this.m_email = Email;
            this.m_title = Title;
            this.m_body = Body;
        }

        public void Send()
        {
            var inf = Emails.emailinfo;
            lock (inf)
            {
                try
                {
                    var sm = Emails.ESM;
                    sm.MailDomainPort = inf.Port;
                    sm.AddRecipient(new string[] { this.Email });
                    sm.RecipientName = this.UserName;
                    sm.From = inf.Sysemail;
                    sm.FromName = GeneralConfigInfo.Current.Webtitle;
                    sm.Html = true;
                    sm.Subject = this.Title;
                    sm.Body = "<pre style=\"width:100%;word-wrap:break-word\">" + this.Body.ToString() + "</pre>";
                    sm.MailDomain = inf.Smtp;
                    sm.MailServerUserName = inf.Username;
                    sm.MailServerPassWord = inf.Password;
                    this.IsSuccess = sm.Send();
                }
                catch { }
            }
            Thread.CurrentThread.Abort();
        }
    }
}