using System;
using System.ComponentModel;
using System.Web.UI.WebControls;
using BBX.Common;
using BBX.Config;
using BBX.Forum;
using BBX.Plugin.Mail;
using NewLife.Reflection;
using NewLife.Log;
using NewLife.Web;

namespace BBX.Web.Admin
{
    public partial class emailconfig : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                EmailConfigInfo config = EmailConfigInfo.Current;
                smtp.Text = config.Smtp;
                port.Text = config.Port.ToString();
                sysemail.Text = config.Sysemail;
                userName.Text = config.Username;
                password.Text = config.Password;

                // 加载所有邮件插件
                smtpemail.DataSource = Emails.LoadPlugins();
                smtpemail.DataBind();

                try
                {
                    smtpemail.SelectedValue = config.PluginName;
                }
                catch { }
            }
        }

        protected void SaveEmailInfo_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                EmailConfigInfo config = EmailConfigInfo.Current;
                config.Smtp = smtp.Text;
                config.Port = Convert.ToInt32(port.Text);
                config.Sysemail = sysemail.Text;
                config.Username = userName.Text;
                config.Password = password.Text;
                config.PluginName = smtpemail.SelectedValue;
                //try
                //{
                //    config.PluginNameSpace = smtpemail.SelectedValue.Split(',')[0];
                //    config.DllFileName = smtpemail.SelectedValue.Split(',')[1];
                //}
                //catch { }
                config.Save();

                //Emails.ReSetISmtpMail();

                base.RegisterStartupScript("PAGE", "window.location.href='emailconfig.aspx';");
            }
        }

        protected void sendTestEmail_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                if (this.testEmail.Text != "")
                {
                    try
                    {
                        Emails.SendMailToUser(this.testEmail.Text, "测试邮件", "这是一封" + Utils.ProductName + "邮箱设置页面发送的测试邮件!");
                        base.RegisterStartupScript("PAGE", "window.location.href='emailconfig.aspx';");
                    }
                    catch (Exception ex)
                    {
                        XTrace.WriteException(ex);
                        WebHelper.Alert(ex.ToString());
                    }
                    return;
                }
                base.RegisterStartupScript("", "<script>alert('请输入测试发送EMAIL地址!');</script>");
            }
        }
    }
}