using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using BBX.Common;
using BBX.Config;
using BBX.Plugin.Mail;
using NewLife.Reflection;

namespace BBX.Forum
{
    /// <summary>邮件控制类</summary>
    public class Emails
    {
        static GeneralConfigInfo config = GeneralConfigInfo.Current;
        static String plugingName;
        static Type provider;

        public static Dictionary<String, Type> LoadPlugins()
        {
            var dic = new Dictionary<String, Type>();
            foreach (var item in typeof(ISmtpMail).GetAllSubclasses(true))
            {
                var name = item.GetDescription() ?? item.FullName;
                dic.Add(name, item);
            }

            return dic;
        }

        private static ISmtpMail Create()
        {
            var cfg = EmailConfigInfo.Current;
            if (provider == null || plugingName != cfg.PluginName)
            {
                plugingName = cfg.PluginName;

                var dic = LoadPlugins();
                if (!dic.TryGetValue(cfg.PluginName, out provider))
                    provider = typeof(SysMailMessage);
            }

            var sm = Activator.CreateInstance(provider) as ISmtpMail;
            sm.Server = cfg.Smtp;
            sm.Port = cfg.Port;
            sm.UserName = cfg.Username;
            sm.PassWord = cfg.Password;
            sm.From = cfg.Sysemail;
            sm.FromName = config.Webtitle;
            sm.Html = true;

            return sm;
        }

        public static bool SendRegMail(string userName, string email, string pass, string authstr = "")
        {
            userName = (userName + "").Trim();
            email = (email + "").Trim();
            pass = (pass + "").Trim();
            if (String.IsNullOrEmpty(email)) return false;

            string text = "http://" + DNTRequest.GetCurrentFullHost() + BaseConfigs.GetForumPath.ToLower();

            var sm = Create();
            sm.RecipientName = userName;
            sm.AddRecipient(email);
            sm.Subject = "已成功创建你的 " + config.Forumtitle + "帐户,请查收.";

            var sb = new StringBuilder();
            sb.Append(EmailConfigInfo.Current.Emailcontent.Replace("{webtitle}", config.Webtitle));
            sb.Replace("{weburl}", string.Format("<a href=\"{0}\">{0}</a>", config.Weburl));
            sb.Replace("{forumurl}", string.Format("<a href=\"{0}\">{0}</a>", text));
            sb.Replace("{forumtitle}", config.Forumtitle);
            if (String.IsNullOrEmpty(authstr))
                sm.Body = "<pre style=\"width:100%;word-wrap:break-word\">" + sb.ToString() + "</pre>";
            else
                sm.Body = "<pre style=\"width:100%;word-wrap:break-word\">" + sb.ToString() + "\r\n\r\n激活您帐户的链接为:<a href=" + text + "activationuser.aspx?authstr=" + authstr.Trim() + "  target=_blank>" + text + "activationuser.aspx?authstr=" + authstr.Trim() + "</a></pre>";

            return sm.Send();
        }

        public static bool SendEmailNotify(string email, string title, string body)
        {
            if (String.IsNullOrEmpty(email)) return false;

            var sm = Create();
            sm.AddRecipient(email);
            sm.Subject = title;
            sm.Body = "<pre style=\"width:100%;word-wrap:break-word\">" + body.ToString() + "</pre>";

            return sm.Send();
        }

        //public static string SendMailToUsers(string uidlist, string subject, string body)
        //{
        //	//var mailTable = DatabaseProvider.GetInstance().GetMailTable(uidlist);
        //	//if (mailTable == null || mailTable.Rows.Count < 1) return "";

        //	var list = User.FindAllByIdList(uidlist);
        //	if (list.Count <= 0) return null;

        //	//var array = new Thread[mailTable.Rows.Count];
        //	int num = 0;
        //	var sb = new StringBuilder();
        //	int num3 = 0;
        //	foreach (var user in list)
        //	{
        //		if (num3 > 100) break;

        //		//var emt = new EmailMultiThread(dr["username"].ToString().Trim(), dr["email"].ToString().Trim(), subject, body);
        //		//array[num] = new Thread(new ThreadStart(emt.Send));
        //		//array[num].Start();
        //		SendAsync(user.Name.Trim(), user.Email.Trim(), subject, body);
        //		// 每5封邮件停留5秒
        //		if (num >= 5)
        //		{
        //			Thread.Sleep(5000);
        //			num = 0;
        //		}
        //		sb.Append(user.ID);
        //		sb.Append(",");
        //		num3++;
        //		num++;
        //	}
        //	return sb.ToString().TrimEnd(',');
        //}

        public static bool SendMailToUser(string email, string title, string body)
        {
            if (String.IsNullOrEmpty(email)) return false;

            var sm = Create();
            sm.AddRecipient(email);
            sm.Subject = title;
            sm.Body = "<pre style=\"width:100%;word-wrap:break-word\">" + body.ToString() + "</pre>";

            return sm.Send();
        }

        /// <summary>异步发送邮件</summary>
        /// <param name="userName"></param>
        /// <param name="email"></param>
        /// <param name="title"></param>
        /// <param name="body"></param>
        public static Boolean SendAsync(string userName, string email, string title, string body)
        {
            if (String.IsNullOrEmpty(email)) return false;

            var sm = Create();
            sm.AddRecipient(email);
            sm.RecipientName = userName;
            sm.Subject = title;
            sm.Body = "<pre style=\"width:100%;word-wrap:break-word\">" + body + "</pre>";

            //sm.Send();
            ThreadPool.QueueUserWorkItem(s => (s as ISmtpMail).Send(), sm);

            return true;
        }
    }
}