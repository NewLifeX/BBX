using System;
using System.ComponentModel;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Discuz.Common;
using Discuz.Config;
using Discuz.Control;
using Discuz.Forum;
using Discuz.Plugin.Mail;
using NewLife.Reflection;

namespace Discuz.Web.Admin
{
    public class emailconfig : AdminPage
    {
        protected HtmlForm Form1;
        protected Discuz.Control.TextBox smtp;
        protected Discuz.Control.TextBox sysemail;
        protected Discuz.Control.TextBox password;
        protected Discuz.Control.TextBox port;
        protected Discuz.Control.TextBox userName;
        protected Discuz.Control.DropDownList smtpemail;
        protected Discuz.Control.Button SaveEmailInfo;
        protected Discuz.Control.TextBox testEmail;
        protected Discuz.Control.Button sendTestEmail;
        protected Hint Hint1;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                var config = EmailConfigInfo.Current;
                smtp.Text = config.Smtp;
                port.Text = config.Port.ToString();
                sysemail.Text = config.Sysemail;
                userName.Text = config.Username;
                password.Text = config.Password;
                try
                {
                    // 加载所有邮件插件
                    foreach (var item in AssemblyX.FindAllPlugins(typeof(ISmtpMail)))
                    {
                        var des = item.GetCustomAttribute<DescriptionAttribute>(true);
                        var name = des == null ? item.FullName : des.Description;
                        smtpemail.Items.Add(new ListItem(name, item.AssemblyQualifiedName));
                    }
                }
                catch { }
                try
                {
                    smtpemail.SelectedValue = config.PluginNameSpace + "," + config.DllFileName;
                }
                catch { }
            }
        }

        private void SaveEmailInfo_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                var config = EmailConfigInfo.Current;
                config.Smtp = smtp.Text;
                config.Port = Convert.ToInt32(port.Text);
                config.Sysemail = sysemail.Text;
                config.Username = userName.Text;
                config.Password = password.Text;
                try
                {
                    config.PluginNameSpace = smtpemail.SelectedValue.Split(',')[0];
                    config.DllFileName = smtpemail.SelectedValue.Split(',')[1];
                }
                catch { }
                config.Save();
                Emails.ReSetISmtpMail();
                base.RegisterStartupScript("PAGE", "window.location.href='global_emailconfig.aspx';");
            }
        }

        private void sendTestEmail_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                if (this.testEmail.Text != "")
                {
                    Emails.SendMailToUser(this.testEmail.Text, "测试邮件", "这是一封" + Utils.ProductName + "邮箱设置页面发送的测试邮件!");
                    base.RegisterStartupScript("PAGE", "window.location.href='global_emailconfig.aspx';");
                    return;
                }
                base.RegisterStartupScript("", "<script>alert('请输入测试发送EMAIL地址!');</script>");
            }
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.SaveEmailInfo.Click += new EventHandler(SaveEmailInfo_Click);
            this.sendTestEmail.Click += new EventHandler(sendTestEmail_Click);
        }
    }
}