using System;
using NewLife;
namespace BBX.Plugin.Mail
{
    /// <summary>Smtp邮件接口</summary>
    public interface ISmtpMail
    {
        /// <summary>SMTP服务器</summary>
        string Server { set; }

        /// <summary>端口</summary>
        int Port { set; }

        string UserName { set; }

        string PassWord { set; }

        string From { get; set; }

        string FromName { get; set; }

        string RecipientName { get; set; }

        string Subject { get; set; }

        string Body { get; set; }

        bool Html { get; set; }

        bool AddRecipient(params string[] username);

        bool Send();
    }

    /// <summary>Smtp邮件基类</summary>
    public abstract class SmtpMailBase : DisposeBase
    {
        private string _Server = "";
        /// <summary>SMTP服务器</summary>
        public string Server
        {
            get { return _Server; }
            set
            {
                string text = (value + "").Trim();
                if (text != "")
                {
                    int p = text.IndexOf("@");
                    if (p != -1)
                    {
                        string text2 = text.Substring(0, p);
                        UserName = text2.Substring(0, text2.IndexOf(":"));
                        PassWord = text2.Substring(text2.IndexOf(":") + 1, text2.Length - text2.IndexOf(":") - 1);
                        text = text.Substring(p + 1, text.Length - p - 1);
                    }
                    p = text.IndexOf(":");
                    if (p != -1)
                    {
                        _Server = text.Substring(0, p);
                        _Port = Convert.ToInt32(text.Substring(p + 1, text.Length - p - 1));
                        return;
                    }
                    _Server = text;
                }
            }
        }

        private int _Port = 25;
        /// <summary>端口</summary>
        public int Port { get { return _Port; } set { _Port = value; } }

        private string _UserName = "";
        public string UserName
        {
            get { return _UserName; }
            set
            {
                if (!value.IsNullOrWhiteSpace())
                    _UserName = value.Trim();
                else
                    _UserName = "";
            }
        }

        private string _PassWord = "";
        public string PassWord { get { return _PassWord; } set { _PassWord = value; } }

        private string _from = "";
        public string From { get { return _from; } set { _from = value; } }

        private string _fromName = "";
        public string FromName { get { return _fromName; } set { _fromName = value; } }

        private string _recipientName = "";
        public string RecipientName { get { return _recipientName; } set { _recipientName = value; } }

        private string _subject;
        public string Subject { get { return _subject; } set { _subject = value; } }

        private string _body;
        public string Body { get { return _body; } set { _body = value; } }

        private bool _html;
        public bool Html { get { return _html; } set { _html = value; } }

    }
}