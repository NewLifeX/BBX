using System;
using System.Collections;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BBX.Common;
using BBX.Config;
using BBX.Control;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web.Admin
{
    public class registerandvisit : AdminPage
    {
        public string[] extCreditsName = new string[8];
        public string[] extCreditsUnits = new string[9];
        protected HtmlForm Form1;
        protected BBX.Control.RadioButtonList regstatus;
        protected BBX.Control.RadioButtonList realnamesystem;
        protected TextareaResize censoruser;
        protected BBX.Control.RadioButtonList regverify;
        protected TextareaResize accessemail;
        protected TextareaResize censoremail;
        protected BBX.Control.TextBox regctrl;
        protected RegularExpressionValidator mycheck;
        protected BBX.Control.TextBox verifyregisterexpired;
        protected RegularExpressionValidator RegularExpressionValidator7;
        protected OnlineEditor verifyemailtemp;
        protected BBX.Control.TextBox newbiespan;
        protected RegularExpressionValidator RegularExpressionValidator1;
        protected BBX.Control.RadioButtonList doublee;
        protected BBX.Control.RadioButtonList emaillogin;
        protected TextareaResize ipregctrl;
        protected BBX.Control.RadioButtonList rules;
        protected TextareaResize rulestxt;
        protected BBX.Control.RadioButtonList welcomemsg;
        protected TextareaResize welcomemsgtxt;
        protected BBX.Control.RadioButtonList passwordmode;
        protected BBX.Control.TextBox CookieDomain;
        protected Hint Hint1;
        protected BBX.Control.RadioButtonList hideprivate;
        protected TextareaResize ipaccess;
        protected TextareaResize ipdenyaccess;
        protected TextareaResize adminipaccess;
        protected BBX.Control.TextBox invitecodeexpiretime;
        protected RegularExpressionValidator RegularExpressionValidator2;
        protected BBX.Control.TextBox addextcreditsline;
        protected RegularExpressionValidator RegularExpressionValidator3;
        protected BBX.Control.TextBox invitecodemaxcount;
        protected RegularExpressionValidator RegularExpressionValidator4;
        protected BBX.Control.TextBox invitecodeusermaxbuy;
        protected RegularExpressionValidator RegularExpressionValidator5;
        protected BBX.Control.TextBox invitecodeusercreateperday;
        protected RegularExpressionValidator RegularExpressionValidator6;
        protected BBX.Control.TextBox invitecodeprice0;
        protected BBX.Control.TextBox invitecodeprice1;
        protected BBX.Control.TextBox invitecodeprice2;
        protected BBX.Control.TextBox invitecodeprice3;
        protected BBX.Control.TextBox invitecodeprice4;
        protected BBX.Control.TextBox invitecodeprice5;
        protected BBX.Control.TextBox invitecodeprice6;
        protected BBX.Control.TextBox invitecodeprice7;
        protected OnlineEditor invitationuserdescription;
        protected OnlineEditor invitationvisitordescription;
        protected OnlineEditor invitationemailmodel;
        protected BBX.Control.Button SaveInfo;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.LoadConfigInfo();
                this.regstatus.Items[0].Attributes.Add("onclick", "setStatus()");
                this.regstatus.Items[1].Attributes.Add("onclick", "setStatus()");
                this.regstatus.Items[2].Attributes.Add("onclick", "setStatus()");
                this.regstatus.Items[3].Attributes.Add("onclick", "setStatus()");
            }
        }

        public void LoadConfigInfo()
        {
            GeneralConfigInfo config = GeneralConfigInfo.Current;
            InvitationConfigInfo config2 = InvitationConfigInfo.Current;
            this.regstatus.SelectedValue = config.Regstatus.ToString();
            this.censoruser.Text = config.Censoruser;
            this.doublee.SelectedValue = config.Doublee.ToString();
            this.emaillogin.SelectedValue = config.Emaillogin.ToString();
            this.regverify.SelectedValue = config.Regverify.ToString();
            this.accessemail.Text = config.Accessemail;
            this.censoremail.Text = config.Censoremail;
            this.hideprivate.SelectedValue = config.Hideprivate.ToString();
            this.ipdenyaccess.Text = config.Ipdenyaccess;
            this.ipaccess.Text = config.Ipaccess;
            this.regctrl.Text = config.Regctrl.ToString();
            this.ipregctrl.Text = config.Ipregctrl;
            this.adminipaccess.Text = config.Adminipaccess;
            this.welcomemsg.SelectedValue = config.Welcomemsg.ToString();
            this.welcomemsgtxt.Text = config.Welcomemsgtxt;
            this.rules.SelectedValue = config.Rules.ToString();
            this.rulestxt.Text = config.Rulestxt;
            this.newbiespan.Text = config.Newbiespan.ToString();
            this.realnamesystem.SelectedValue = config.Realnamesystem.ToString();
            this.invitecodeexpiretime.Text = config2.InviteCodeExpireTime.ToString();
            this.invitecodemaxcount.Text = config2.InviteCodeMaxCount.ToString();
            this.addextcreditsline.Text = config2.InviteCodePayCount.ToString();
            this.invitationuserdescription.Text = config2.InvitationLoginUserDescription;
            this.invitationvisitordescription.Text = config2.InvitationVisitorDescription;
            this.invitationemailmodel.Text = config2.InvitationEmailTemplate;
            this.invitecodeusermaxbuy.Text = config2.InviteCodeMaxCountToBuy.ToString();
            this.invitecodeusercreateperday.Text = config2.InviteCodeUserCreatePerDay.ToString();
            this.passwordmode.SelectedValue = config.Passwordmode.ToString();
            this.CookieDomain.Text = config.CookieDomain;
            this.verifyregisterexpired.Text = config.Verifyregisterexpired.ToString();
            this.verifyemailtemp.text = config.Verifyregisteremailtemp;
            string[] array = Utils.SplitString(config2.InviteCodePrice, ",");
            this.extCreditsUnits = Scoresets.GetValidScoreUnit();
            var scorePaySet = Scoresets.GetScorePaySet(0);
            for (int i = 0; i < 8; i++)
            {
                this.extCreditsName[i] = "";
                var textBox = this.FindControl("invitecodeprice" + i) as BBX.Control.TextBox;
                textBox.Text = array[i];
                textBox.Visible = false;
            }
            foreach (DataRow dataRow in scorePaySet.Rows)
            {
                this.extCreditsName[Utils.StrToInt(dataRow["id"].ToString(), 0) - 1] = dataRow["name"] + ":";
                var textBox2 = this.FindControl("invitecodeprice" + (Utils.StrToInt(dataRow[0].ToString(), 0) - 1).ToString()) as BBX.Control.TextBox;
                textBox2.Visible = true;
            }
        }

        private void SaveInfo_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                GeneralConfigInfo config = GeneralConfigInfo.Current;
                var config2 = InvitationConfigInfo.Current;
                if (Convert.ToInt16(this.regverify.SelectedValue) == 1 && Request["verifyemailtempmessage_hidden"].IndexOf("{1}") == -1)
                {
                    base.RegisterStartupScript("erro", "<script>alert('验证请求信息邮件内容模板中必须包含\"{1}\"');</script>");
                    return;
                }
                config.Regstatus = (int)Convert.ToInt16(this.regstatus.SelectedValue);
                config.Censoruser = this.DelNullRowOrSpace(this.censoruser.Text);
                config.Doublee = (int)Convert.ToInt16(this.doublee.SelectedValue);
                config.Emaillogin = (int)Convert.ToInt16(this.emaillogin.SelectedValue);
                config.Regverify = (int)Convert.ToInt16(this.regverify.SelectedValue);
                config.Accessemail = this.accessemail.Text;
                config.Censoremail = this.censoremail.Text;
                config.Hideprivate = (int)Convert.ToInt16(this.hideprivate.SelectedValue);
                config.Ipdenyaccess = this.ipdenyaccess.Text;
                config.Ipaccess = this.ipaccess.Text;
                config.Regctrl = (int)Convert.ToInt16(this.regctrl.Text);
                config.Ipregctrl = this.ipregctrl.Text;
                config.Adminipaccess = this.adminipaccess.Text;
                config.Welcomemsg = (int)Convert.ToInt16(this.welcomemsg.SelectedValue);
                config.Welcomemsgtxt = this.welcomemsgtxt.Text;
                config.Rules = (int)Convert.ToInt16(this.rules.SelectedValue);
                config.Rulestxt = this.rulestxt.Text;
                config.Newbiespan = (int)Convert.ToInt16(this.newbiespan.Text);
                config.Realnamesystem = (int)Convert.ToInt16(this.realnamesystem.SelectedValue);
                config.Passwordmode = (int)Convert.ToInt16(this.passwordmode.SelectedValue);
                config.CookieDomain = this.CookieDomain.Text;
                config.Verifyregisterexpired = (int)Convert.ToInt16(this.verifyregisterexpired.Text);
                config.Verifyregisteremailtemp = this.RepairEmailTemplateCodeParameter(Request["verifyemailtempmessage_hidden"]);
                config2.InviteCodePayCount = Utils.StrToInt(this.addextcreditsline.Text, 0);
                config2.InviteCodeExpireTime = Utils.StrToInt(this.invitecodeexpiretime.Text, 0);
                config2.InviteCodeMaxCount = Utils.StrToInt(this.invitecodemaxcount.Text, 0);
                config2.InviteCodePrice = this.CreateInviteCodePriceString();
                config2.InvitationLoginUserDescription = Request["invitationuserdescriptionmessage_hidden"];
                config2.InvitationVisitorDescription = Request["invitationvisitordescriptionmessage_hidden"];
                config2.InvitationEmailTemplate = this.RepairEmailTemplateCodeParameter(Request["invitationemailmodelmessage_hidden"]);
                config2.InviteCodeMaxCountToBuy = (int)Convert.ToInt16(this.invitecodeusermaxbuy.Text);
                config2.InviteCodeUserCreatePerDay = (int)Convert.ToInt16(this.invitecodeusercreateperday.Text);
                Hashtable hashtable = new Hashtable();
                hashtable.Add("特殊 IP 注册限制", this.ipregctrl.Text);
                hashtable.Add("IP 禁止访问列表", this.ipdenyaccess.Text);
                hashtable.Add("IP 访问列表", this.ipaccess.Text);
                hashtable.Add("管理员后台IP访问列表", this.adminipaccess.Text);
                string text = "";
                if (!Utils.IsRuleTip(hashtable, "ip", out text))
                {
                    base.RegisterStartupScript("erro", "<script>alert('" + text.ToString() + ",IP格式错误');</script>");
                    return;
                }
                Hashtable hashtable2 = new Hashtable();
                hashtable2.Add("Email 允许地址", this.accessemail.Text);
                hashtable2.Add("Email 禁止地址", this.censoremail.Text);
                string text2 = "";
                if (!Utils.IsRuleTip(hashtable2, "email", out text2))
                {
                    base.RegisterStartupScript("erro", "<script>alert('" + text2.ToString() + ",Email格式错误');</script>");
                    return;
                }
                config.Save();

                //config.Save();;
                //InvitationConfigs.Serialiaze(config2, base.Server.MapPath("../../config/invitation.config"));
                config2.Save();
                AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "注册与访问控制设置", "");
                base.RegisterStartupScript("PAGE", "window.location.href='global_registerandvisit.aspx';");
            }
        }

        public string CreateInviteCodePriceString()
        {
            string text = "";
            for (int i = 0; i < 8; i++)
            {
                BBX.Control.TextBox textBox = this.FindControl("invitecodeprice" + i.ToString()) as BBX.Control.TextBox;
                text = text + textBox.Text.ToDouble() + ",";
            }
            return text.Trim(',');
        }

        public string RepairEmailTemplateCodeParameter(string tmpStr)
        {
            return tmpStr.Replace("%7B", "{").Replace("%7D", "}");
        }

        public string DelNullRowOrSpace(string desStr)
        {
            string text = "";
            string[] array = Utils.SplitString(desStr.Replace(" ", ""), "\r\n");
            for (int i = 0; i < array.Length; i++)
            {
                string text2 = array[i];
                if (!text2.IsNullOrEmpty())
                {
                    if (String.IsNullOrEmpty(text))
                    {
                        text = text2;
                    }
                    else
                    {
                        text = text + "\r\n" + text2;
                    }
                }
            }
            return text;
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.SaveInfo.ValidateForm = false;
            this.SaveInfo.Click += new EventHandler(this.SaveInfo_Click);
        }
    }
}