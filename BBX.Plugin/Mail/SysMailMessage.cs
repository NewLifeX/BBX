using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace BBX.Plugin.Mail
{
    /// <summary>.Net邮件发送程序</summary>
    [Description(".Net邮件发送程序")]
    public class SysMailMessage : SmtpMailBase, ISmtpMail
    {
        private List<String> _recipient = new List<string>();

        public bool AddRecipient(params string[] users)
        {
            //_recipient = username[0].Trim();
            _recipient.AddRange(users);

            return true;
        }

        private string Base64Encode(string str)
        {
            byte[] bytes = Encoding.Default.GetBytes(str);
            return Convert.ToBase64String(bytes);
        }

        public bool Send()
        {
            var msg = new MailMessage();
            var encoding = Encoding.UTF8;
            msg.From = new MailAddress(From, Subject, encoding);
            foreach (var item in _recipient)
            {
                if (!String.IsNullOrEmpty(item)) msg.To.Add(item);
            }
            msg.Subject = Subject;
            msg.IsBodyHtml = true;
            msg.Body = this.Body;
            msg.Priority = MailPriority.Normal;
            msg.BodyEncoding = encoding;

            var client = new SmtpClient();
            client.Host = Server;
            client.Port = Port;
            client.Credentials = new NetworkCredential(UserName, PassWord);

            if (this.Port != 25) client.EnableSsl = true;

            client.Send(msg);

            return true;
        }
    }
}