using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;
using BBX.Common;
using NewLife;

namespace BBX.Plugin.Mail
{
    /// <summary>BBX邮件发送程序</summary>
    [Description("BBX邮件发送程序")]
    public class SmtpMail : SmtpMailBase, ISmtpMail
    {
        #region 属性
        private TcpClient tc;
        private NetworkStream ns;

        private List<String> Recipient = new List<String>();
        private List<String> Attachments = new List<String>();

        private string _charset = "GB2312";
        public string Charset { get { return _charset; } set { _charset = value; } }

        private MailPriority _Priority = MailPriority.Normal;
        public MailPriority Priority { get { return _Priority; } set { _Priority = value; } }

        private StringBuilder logs = new StringBuilder();
        public string Logs { get { return logs.ToString(); } }

        private int recipientmaxnum = 5;
        /// <summary>最大收件人。默认5</summary>
        public int RecipientMaxNum { set { recipientmaxnum = value; } }
        #endregion

        #region 构造
        static SmtpMail() { SMTPCodeAdd(); }

        protected override void OnDispose(bool disposing)
        {
            base.OnDispose(disposing);

            if (ns != null) ns.Close();
            if (tc != null) tc.Close();
        }
        #endregion

        #region 代码
        static Dictionary<String, String> ErrCodes = new Dictionary<String, String>();
        static Dictionary<String, String> RightCodes = new Dictionary<String, String>();

        //public static string ErrCodeHTMessage(string code) { return ErrCodeHT[code]; }

        static void SMTPCodeAdd()
        {
            var es = ErrCodes;
            es.Add("500", "邮箱地址错误");
            es.Add("501", "参数格式错误");
            es.Add("502", "命令不可实现");
            es.Add("503", "服务器需要SMTP验证");
            es.Add("504", "命令参数不可实现");
            es.Add("421", "服务未就绪,关闭传输信道");
            es.Add("450", "要求的邮件操作未完成,邮箱不可用(例如,邮箱忙)");
            es.Add("550", "要求的邮件操作未完成,邮箱不可用(例如,邮箱未找到,或不可访问)");
            es.Add("451", "放弃要求的操作;处理过程中出错");
            es.Add("551", "用户非本地,请尝试<forward-path>");
            es.Add("452", "系统存储不足, 要求的操作未执行");
            es.Add("552", "过量的存储分配, 要求的操作未执行");
            es.Add("553", "邮箱名不可用, 要求的操作未执行(例如邮箱格式错误)");
            es.Add("432", "需要一个密码转换");
            es.Add("534", "认证机制过于简单");
            es.Add("538", "当前请求的认证机制需要加密");
            es.Add("454", "临时认证失败");
            es.Add("530", "需要认证");

            var rs = RightCodes;
            rs.Add("220", "服务就绪");
            rs.Add("250", "要求的邮件操作完成");
            rs.Add("251", "用户非本地, 将转发向<forward-path>");
            rs.Add("354", "开始邮件输入, 以<enter>.<enter>结束");
            rs.Add("221", "服务关闭传输信道");
            rs.Add("334", "服务器响应验证Base64字符串");
            rs.Add("235", "验证成功");
        }
        #endregion

        #region 发送准备
        public void AddAttachment(params string[] files)
        {
            if (files == null) throw new ArgumentNullException("files)");

            for (int i = 0; i < files.Length; i++)
            {
                if (!String.IsNullOrEmpty(files[i])) Attachments.Add(files[i]);
            }
        }

        public bool AddRecipient(params string[] rcpts)
        {
            for (int i = 0; i < rcpts.Length; i++)
            {
                string text = rcpts[i].Trim();
                if (String.IsNullOrEmpty(text))
                {
                    Dispose();
                    throw new ArgumentNullException("Recipients[" + i + "]");
                }
                if (text.IndexOf("@") == -1)
                {
                    Dispose();
                    throw new ArgumentException("Recipients.IndexOf(\"@\")==-1", "Recipients");
                }
                //if (!AddRecipient(text)) return false;
                if (Recipient.Count > recipientmaxnum)
                {
                    Dispose();
                    throw new ArgumentOutOfRangeException("Recipients", "收件人过多不可多于 " + recipientmaxnum + " 个");
                }

                Recipient.Add(text);
            }
            return true;
        }

        //private bool AddRecipient(string Recipients)
        //{
        //    Recipient.Clear();
        //    if (Recipient.Count < recipientmaxnum)
        //    {
        //        Recipient.Add(Recipients);
        //        return true;
        //    }
        //    Dispose();
        //    throw new ArgumentOutOfRangeException("Recipients", "收件人过多不可多于 " + recipientmaxnum + " 个");
        //}

        public bool Send()
        {
            if (String.IsNullOrEmpty(Server)) throw new ArgumentNullException("Recipient", "必须指定SMTP服务器");

            return SendEmail();
        }

        public bool Send(string smtpserver)
        {
            Server = smtpserver;
            return Send();
        }

        public bool Send(string smtpserver, string from, string fromname, string to, string toname, bool html, string subject, string body)
        {
            Server = smtpserver;
            From = from;
            FromName = fromname;
            AddRecipient(to);
            RecipientName = toname;
            Html = html;
            Subject = subject;
            Body = body;
            return Send();
        }
        #endregion

        #region 编码辅助
        private string Base64Encode(string str)
        {
            byte[] bytes = Encoding.Default.GetBytes(str);
            return Convert.ToBase64String(bytes);
        }

        private string Base64Decode(string str)
        {
            byte[] bytes = Convert.FromBase64String(str);
            return Encoding.Default.GetString(bytes);
        }

        private string GetStream(string FilePath)
        {
            //FileStream fileStream = new FileStream(FilePath, FileMode.Open);
            //byte[] array = new byte[Convert.ToInt32(fileStream.Length)];
            //fileStream.Read(array, 0, array.Length);
            //fileStream.Close();
            //return Convert.ToBase64String(array);
            return Convert.ToBase64String(File.ReadAllBytes(FilePath));
        }
        #endregion

        #region 命令处理
        private bool SendCommand(string cmd)
        {
            if (cmd == null || cmd.Trim() == string.Empty) return true;

            cmd += Environment.NewLine;
            logs.Append(cmd);
            var buf = Encoding.Default.GetBytes(cmd);
            ns.Write(buf, 0, buf.Length);
            return true;
        }

        private string RecvResponse()
        {
            var buf = new byte[1024];
            var count = ns.Read(buf, 0, buf.Length);
            if (count == 0) return String.Empty;

            var text = Encoding.Default.GetString(buf).Substring(0, count);
            logs.AppendLine(text);
            return text;
        }

        private bool ProcessCommand(string cmd, string errstr)
        {
            if (String.IsNullOrEmpty(cmd)) return true;

            if (!SendCommand(cmd)) return false;

            string rs = RecvResponse();
            if (rs == "false") return false;

            string code = rs.Length < 3 ? rs : rs.Substring(0, 3);
            if (RightCodes.ContainsKey(code)) return true;

            var err = "";
            if (ErrCodes.TryGetValue(code, out err))
                throw new Exception(code + err + " " + errstr);
            else
                throw new Exception(rs + " " + errstr);
        }

        private bool ProcessCommands(string[] str, string errstr)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (!ProcessCommand(str[i], "")) return false;
            }
            return true;
        }
        #endregion

        #region 发送主方法
        private bool SendEmail()
        {
            tc = new TcpClient(Server, Port);

            ns = tc.GetStream();
            var rs = RecvResponse();
            if (!RightCodes.ContainsKey(rs.Substring(0, 3))) throw new XException("无法识别响应{0}", rs);

            if (!UserName.IsNullOrWhiteSpace())
            {
                if (!ProcessCommands(new string[]
                {
                    "EHLO " + Server,
                    "AUTH LOGIN",
                    Base64Encode(UserName),
                    Base64Encode(PassWord)
                }, "SMTP服务器验证失败,请核对用户名和密码。"))
                {
                    return false;
                }
            }
            else
            {
                if (!ProcessCommand("HELO " + Server, "")) return false;
            }
            if (!ProcessCommand("MAIL FROM:<" + From + ">", "发件人地址错误,或不能为空")) return false;

            string[] cmds = new string[recipientmaxnum];
            for (int i = 0; i < Recipient.Count; i++)
            {
                cmds[i] = "RCPT TO:<" + Recipient[i] + ">";
            }
            if (!ProcessCommands(cmds, "收件人地址有误")) return false;

            if (!ProcessCommand("DATA", "")) return false;

            var sb = new StringBuilder();
            sb.AppendLine("From:" + FromName + "<" + From + ">");
            sb.AppendLine("To:=?" + Charset.ToUpper() + "?B?" + (RecipientName.IsNullOrEmpty() ? "" : Base64Encode(RecipientName)) + "?=<" + Recipient[0] + ">");
            sb.Append("CC:");
            for (int j = 1; j < Recipient.Count; j++)
            {
                sb.Append(Recipient[j] + "<" + Recipient[j] + ">,");
            }
            sb.AppendLine();
            sb.AppendLine(((Subject == string.Empty || Subject == null) ? "Subject:" : ((String.IsNullOrEmpty(Charset)) ? ("Subject:" + Subject) : "Subject:=?" + Charset.ToUpper() + "?B?" + Base64Encode(Subject) + "?=")));
            sb.AppendLine("X-Priority:" + Priority);
            sb.AppendLine("X-MSMail-Priority:" + Priority);
            sb.AppendLine("Importance:" + Priority);
            sb.AppendLine("X-Mailer: Lion.Web.Mail.SmtpMail Pubclass [cn]");
            sb.AppendLine("MIME-Version: 1.0");
            if (Attachments.Count != 0)
            {
                sb.AppendLine("Content-Type: multipart/mixed;");
                sb.AppendLine("\tboundary=\"=====" + (Html ? "001_Dragon520636771063_" : "001_Dragon303406132050_") + "=====\"");
                sb.AppendLine();
            }
            if (Html)
            {
                if (Attachments.Count == 0)
                {
                    sb.AppendLine("Content-Type: multipart/alternative;");
                    sb.AppendLine("\tboundary=\"=====003_Dragon520636771063_=====\"");
                    sb.AppendLine();
                    sb.AppendLine("This is a multi-part message in MIME format.");
                    sb.AppendLine();
                }
                else
                {
                    sb.AppendLine("This is a multi-part message in MIME format.");
                    sb.AppendLine();
                    sb.AppendLine("--=====001_Dragon520636771063_=====");
                    sb.AppendLine("Content-Type: multipart/alternative;");
                    sb.AppendLine("\tboundary=\"=====003_Dragon520636771063_=====\"");
                    sb.AppendLine();
                }
                sb.AppendLine("--=====003_Dragon520636771063_=====");
                sb.AppendLine("Content-Type: text/plain;");
                sb.AppendLine(((String.IsNullOrEmpty(Charset)) ? "\tcharset=\"iso-8859-1\"" : ("\tcharset=\"" + Charset.ToLower() + "\"")));
                sb.AppendLine("Content-Transfer-Encoding: base64");
                sb.AppendLine();
                sb.AppendLine(Base64Encode("邮件内容为HTML格式,请选择HTML方式查看"));
                sb.AppendLine();
                sb.AppendLine("--=====003_Dragon520636771063_=====");
                sb.AppendLine("Content-Type: text/html;");
                sb.AppendLine(((String.IsNullOrEmpty(Charset)) ? "\tcharset=\"iso-8859-1\"" : ("\tcharset=\"" + Charset.ToLower() + "\"")));
                sb.AppendLine("Content-Transfer-Encoding: base64");
                sb.AppendLine();
                sb.AppendLine(Base64Encode(Body));
                sb.AppendLine();
                sb.AppendLine("--=====003_Dragon520636771063_=====--");
            }
            else
            {
                if (Attachments.Count != 0) sb.AppendLine("--=====001_Dragon303406132050_=====");

                sb.AppendLine("Content-Type: text/plain;");
                sb.AppendLine(((String.IsNullOrEmpty(Charset)) ? "\tcharset=\"iso-8859-1\"" : ("\tcharset=\"" + Charset.ToLower() + "\"")));
                sb.AppendLine("Content-Transfer-Encoding: base64");
                sb.AppendLine();
                sb.AppendLine(Base64Encode(Body));
            }
            if (Attachments.Count != 0)
            {
                for (int k = 0; k < Attachments.Count; k++)
                {
                    var file = (string)Attachments[k];
                    var fileName = Path.GetFileName(file);

                    sb.AppendLine("--=====" + (Html ? "001_Dragon520636771063_" : "001_Dragon303406132050_") + "=====");
                    sb.AppendLine("Content-Type: text/plain;");
                    sb.AppendLine("\tname=\"=?" + Charset.ToUpper() + "?B?" + Base64Encode(fileName) + "?=\"");
                    sb.AppendLine("Content-Transfer-Encoding: base64");
                    sb.AppendLine("Content-Disposition: attachment;");
                    sb.AppendLine("\tfilename=\"=?" + Charset.ToUpper() + "?B?" + Base64Encode(fileName) + "?=\"");
                    sb.AppendLine();
                    sb.AppendLine(GetStream(file));
                    sb.AppendLine();
                }
                sb.AppendLine("--=====" + (Html ? "001_Dragon520636771063_" : "001_Dragon303406132050_") + "=====--");
                sb.AppendLine();
            }
            sb.AppendLine();
            sb.AppendLine(".");
            if (!ProcessCommand(sb.ToString(), "错误信件信息")) return false;

            if (!ProcessCommand("QUIT", "断开连接时错误")) return false;

            ns.Close();
            tc.Close();
            return true;
        }
        #endregion
    }
}