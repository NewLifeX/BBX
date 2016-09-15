using System;
using System.Collections;
using System.IO;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BBX.Common;
using BBX.Config;
using BBX.Control;
using BBX.Entity;

namespace BBX.Web.Admin
{
    public class safecontrol : AdminPage
    {
        protected HtmlForm Form1;
        protected BBX.Control.DropDownList VerifyImage;
        protected BBX.Control.TextBox seccodestatus;
        protected BBX.Control.RadioButtonList secques;
        protected BBX.Control.TextBox postinterval;
        protected BBX.Control.TextBox maxspm;
        protected BBX.Control.RadioButtonList admintools;
        protected BBX.Control.TextBox antispamusername;
        protected BBX.Control.TextBox antispamemail;
        protected BBX.Control.TextBox antispamtitle;
        protected BBX.Control.TextBox antispammessage;
        protected BBX.Control.RadioButtonList disablepostad;
        protected HtmlGenericControl postadstatus;
        protected BBX.Control.TextBox disablepostadregminute;
        protected BBX.Control.TextBox disablepostadpostcount;
        protected TextareaResize disablepostadregular;
        protected Hint Hint1;
        protected BBX.Control.Button SaveInfo;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.LoadConfigInfo();
                this.seccodestatus.Attributes.Add("style", "line-height:16px");
            }
        }

        public void LoadConfigInfo()
        {
            string[] files = Directory.GetFiles(HttpRuntime.BinDirectory, "BBX.Plugin.VerifyImage.*.dll");
            string[] array = files;
            for (int i = 0; i < array.Length; i++)
            {
                string text = array[i];
                string item = text.ToString().Substring(text.ToString().IndexOf("BBX.Plugin.VerifyImage")).Replace("BBX.Plugin.VerifyImage.", "").Replace(".dll", "");
                this.VerifyImage.Items.Add(item);
            }
            var config = GeneralConfigInfo.Current;
            this.postinterval.Text = config.Postinterval.ToString();
            this.maxspm.Text = config.Maxspm.ToString();
            this.seccodestatus.AddAttributes("readonly", "");
            this.seccodestatus.Attributes.Add("onfocus", "this.className='';");
            this.seccodestatus.Attributes.Add("onblur", "this.className='';");
            this.admintools.SelectedValue = config.Admintools.ToString();
            this.VerifyImage.Items.Add(new ListItem("系统默认验证码", ""));
            this.seccodestatus.Text = config.Seccodestatus.Replace(",", "\r\n");
            this.ViewState["Seccodestatus"] = config.Seccodestatus.ToString();
            this.VerifyImage.SelectedValue = config.VerifyImageAssemly;
            this.antispamusername.Text = config.Antispamregisterusername;
            this.antispamemail.Text = config.Antispamregisteremail;
            this.antispamtitle.Text = config.Antispamposttitle;
            this.antispammessage.Text = config.Antispampostmessage;
            this.disablepostad.SelectedValue = config.DisablePostAD ? "1" : "0";
            this.disablepostad.Items[0].Attributes.Add("onclick", "$('" + this.postadstatus.ClientID + "').style.display='';");
            this.disablepostad.Items[1].Attributes.Add("onclick", "$('" + this.postadstatus.ClientID + "').style.display='none';");
            this.disablepostadregminute.Text = config.DisablePostADRegMinute.ToString();
            this.disablepostadpostcount.Text = config.DisablePostADPostCount.ToString();
            this.disablepostadregular.Text = config.DisablePostADRegular.ToString();
            try
            {
                this.secques.SelectedValue = config.Secques.ToString();
            }
            catch
            {
                this.secques.SelectedValue = "1";
            }
            if (!config.DisablePostAD)
            {
                this.postadstatus.Attributes.Add("style", "display:none");
            }
        }

        private void SaveInfo_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                foreach (DictionaryEntry dictionaryEntry in new Hashtable
				{
					{
						"发帖灌水预防",
						this.postinterval.Text
					},

					{
						"60 秒最大搜索次数",
						this.maxspm.Text
					}
				})
                {
                    if (!Utils.IsInt(dictionaryEntry.Value.ToString()))
                    {
                        base.RegisterStartupScript("", "<script>alert('输入错误:" + dictionaryEntry.Key.ToString().Trim() + ",只能是0或者正整数');window.location.href='global_safecontrol.aspx';</script>");
                        return;
                    }
                }
                if (!(this.disablepostad.SelectedValue == "1") || !(String.IsNullOrEmpty(this.disablepostadregular.Text)))
                {
                    string text = ("|" + this.antispamusername.Text + "|" + this.antispamemail.Text + "|" + this.antispamtitle.Text + "|" + this.antispammessage.Text + "|");
                    string[] array = new string[]
					{
						"|" + this.antispamusername.Text + "|",
						"|" + this.antispamemail.Text + "|",
						"|" + this.antispamtitle.Text + "|",
						"|" + this.antispammessage.Text + "|"
					};
                    string[] array2 = array;
                    for (int i = 0; i < array2.Length; i++)
                    {
                        string value = array2[i];
                        if (string.IsNullOrEmpty(value))
                        {
                            base.RegisterStartupScript("", "<script>alert('防注册机设置不可为空 , 请返回重新填写!');window.location.href='global_safecontrol.aspx';</script>");
                            return;
                        }
                        if (text.IndexOf(value) != text.LastIndexOf(value))
                        {
                            base.RegisterStartupScript("", "<script>alert('防注册机设置不可重复 , 请返回重新填写!');window.location.href='global_safecontrol.aspx';</script>");
                            return;
                        }
                    }
                    var config = GeneralConfigInfo.Current;
                    config.VerifyImageAssemly = this.VerifyImage.SelectedValue;
                    config.Postinterval = this.postinterval.Text.ToInt();
                    config.Seccodestatus = this.seccodestatus.Text.Trim().Replace("\r\n", ",");
                    config.Maxspm = this.maxspm.Text.ToInt();
                    config.Secques = this.secques.SelectedValue.ToInt();
                    config.Admintools = (int)Convert.ToInt16(this.admintools.SelectedValue);
                    config.Antispamregisterusername = this.antispamusername.Text.Trim();
                    config.Antispamregisteremail = this.antispamemail.Text.Trim();
                    config.Antispamposttitle = this.antispamtitle.Text.Trim();
                    config.Antispampostmessage = this.antispammessage.Text.Trim();
                    config.DisablePostAD = (int)Convert.ToInt16(this.disablepostad.SelectedValue) != 0;
                    config.DisablePostADRegMinute = (int)Convert.ToInt16(this.disablepostadregminute.Text);
                    config.DisablePostADPostCount = (int)Convert.ToInt16(this.disablepostadpostcount.Text);
                    config.DisablePostADRegular = this.disablepostadregular.Text;
                    config.Save();

                    //config.Save();;
                    AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "安全与防灌水", "");
                    base.RegisterStartupScript("PAGE", "window.location.href='global_safecontrol.aspx';");
                    return;
                }
                base.RegisterStartupScript("", "<script>alert('新用户广告强力屏蔽正则表达式为空');window.location.href='global_safecontrol.aspx';</script>");
                return;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.SaveInfo.Click += new EventHandler(this.SaveInfo_Click);
        }
    }
}