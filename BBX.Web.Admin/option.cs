using System;
using System.Web.UI.HtmlControls;
using BBX.Common;
using BBX.Config;
using BBX.Control;
using BBX.Entity;
using BBX.Forum;
using NewLife;

namespace BBX.Web.Admin
{
    public class option : AdminPage
    {
        protected HtmlForm Form1;
        protected RadioButtonList forcewww;
        protected RadioButtonList statstatus;
        protected RadioButtonList userstatusby;
        protected TextBox maxmodworksmonths;
        protected RadioButtonList reasonpm;
        protected TextBox hottopic;
        protected RadioButtonList enabletag;
        protected TextBox hottagcount;
        protected RadioButtonList fastpost;
        protected RadioButtonList modworkstatus;
        protected TextBox losslessdel;
        protected RadioButtonList editedby;
        protected TextBox starthreshold;
        protected RadioButtonList allowswitcheditor;
        protected TextBox ratevalveset1;
        protected TextBox ratevalveset2;
        protected TextBox ratevalveset3;
        protected TextBox ratevalveset4;
        protected TextBox ratevalveset5;
        protected RadioButtonList replynotificationstatus;
        protected RadioButtonList replyemailstatus;
        protected TextBox viewnewtopicminute;
        protected RadioButtonList quickforward;
        protected TextareaResize msgforwardlist;
        protected RadioButtonList rssstatus;
        protected TextBox rssttl;
        protected RadioButtonList cachelog;
        //protected RadioButtonList silverlight;
        protected Hint Hint1;
        protected Button SaveInfo;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.LoadConfigInfo();
                this.quickforward.Items[0].Attributes.Add("onclick", "setStatus(true)");
                this.quickforward.Items[1].Attributes.Add("onclick", "setStatus(false)");
            }
        }

        public void LoadConfigInfo()
        {
            GeneralConfigInfo config = GeneralConfigInfo.Current;
            this.modworkstatus.SelectedValue = config.Modworkstatus.ToString();
            this.userstatusby.SelectedValue = ((config.Userstatusby.ToString() != "0") ? "1" : "0");
            this.rssttl.Text = config.Rssttl.ToString();
            this.losslessdel.Text = config.Losslessdel.ToString();
            this.editedby.SelectedValue = config.Editedby.ToString();
            this.allowswitcheditor.SelectedValue = config.Allowswitcheditor.ToString();
            this.reasonpm.SelectedValue = config.Reasonpm.ToString();
            this.hottopic.Text = config.Hottopic.ToString();
            this.starthreshold.Text = config.Starthreshold.ToString();
            this.fastpost.SelectedValue = config.Fastpost.ToString();
            this.enabletag.SelectedValue = config.Enabletag ? "1" : "0";
            string[] array = config.Ratevalveset.Split(',');
            this.ratevalveset1.Text = array[0];
            this.ratevalveset2.Text = array[1];
            this.ratevalveset3.Text = array[2];
            this.ratevalveset4.Text = array[3];
            this.ratevalveset5.Text = array[4];
            this.forcewww.SelectedValue = config.Forcewww.ToString();
            this.statstatus.SelectedValue = config.Statstatus.ToString().ToLower();
            this.hottagcount.Text = config.Hottagcount.ToString();
            this.maxmodworksmonths.Text = config.Maxmodworksmonths.ToString();
            this.replynotificationstatus.SelectedValue = config.Replynotificationstatus.ToString();
            this.replyemailstatus.SelectedValue = config.Replyemailstatus.ToString();
            this.quickforward.SelectedValue = config.Quickforward.ToString();
            this.viewnewtopicminute.Text = config.Viewnewtopicminute.ToString();
            this.rssstatus.SelectedValue = config.Rssstatus.ToString();
            this.msgforwardlist.Text = config.Msgforwardlist.Replace(",", "\r\n");
            this.cachelog.SelectedValue = config.Cachelog.ToString();
            //this.silverlight.SelectedValue = this.config.Silverlight.ToString();
        }

        private void SaveInfo_Click(object sender, EventArgs e)
        {
            string[][] array = new string[][]
            {
                new string[]
                {
                    this.losslessdel.Text,
                    this.starthreshold.Text,
                    this.hottopic.Text
                },
                new string[]
                {
                    "删帖不减积分时间",
                    "每页主题数",
                    "每页主题数",
                    "星星升级阀值",
                    "热门话题最低帖数"
                }
            };
            for (int i = 0; i < array[0].Length; i++)
            {
                if (array[0][i].ToInt(-1) < 0)
                {
                    base.RegisterStartupScript("", "<script>alert('输入错误:" + array[1][i] + ",只能是0或者正整数');window.location.href='forum_option.aspx';</script>");
                    return;
                }
            }
            if (this.losslessdel.Text.ToInt() > 9999 || this.losslessdel.Text.ToInt() < 0)
            {
                base.RegisterStartupScript("", "<script>alert('删帖不减积分时间期限只能在0-9999之间');window.location.href='forum_option.aspx';</script>");
                return;
            }
            if (Convert.ToInt16(this.starthreshold.Text) > 9999 || Convert.ToInt16(this.starthreshold.Text) < 0)
            {
                base.RegisterStartupScript("", "<script>alert('星星升级阀值只能在0-9999之间');window.location.href='forum_option.aspx';</script>");
                return;
            }
            if (Convert.ToInt16(this.hottopic.Text) > 9999 || Convert.ToInt16(this.hottopic.Text) < 0)
            {
                base.RegisterStartupScript("", "<script>alert('热门话题最低帖数只能在0-9999之间');window.location.href='forum_option.aspx';</script>");
                return;
            }
            if (Convert.ToInt16(this.hottagcount.Text) > 60 || Convert.ToInt16(this.hottagcount.Text) < 0)
            {
                base.RegisterStartupScript("", "<script>alert('首页热门标签(Tag)数量只能在0-60之间');window.location.href='forum_option.aspx';</script>");
            }
            if (this.viewnewtopicminute.Text.ToInt() > 14400 || this.viewnewtopicminute.Text.ToInt() < 5)
            {
                base.RegisterStartupScript("", "<script>alert('查看新帖的设置必须在5-14400之间');window.location.href='forum_option.aspx';</script>");
                return;
            }
            if (!this.ValidateRatevalveset(this.ratevalveset1.Text))
            {
                return;
            }
            if (!this.ValidateRatevalveset(this.ratevalveset2.Text))
            {
                return;
            }
            if (!this.ValidateRatevalveset(this.ratevalveset3.Text))
            {
                return;
            }
            if (!this.ValidateRatevalveset(this.ratevalveset4.Text))
            {
                return;
            }
            if (!this.ValidateRatevalveset(this.ratevalveset5.Text))
            {
                return;
            }
            if (Convert.ToInt16(this.ratevalveset1.Text) >= Convert.ToInt16(this.ratevalveset2.Text) || Convert.ToInt16(this.ratevalveset2.Text) >= Convert.ToInt16(this.ratevalveset3.Text) || Convert.ToInt16(this.ratevalveset3.Text) >= Convert.ToInt16(this.ratevalveset4.Text) || Convert.ToInt16(this.ratevalveset4.Text) >= Convert.ToInt16(this.ratevalveset5.Text))
            {
                base.RegisterStartupScript("", "<script>alert('评分阀值不是递增取值');window.location.href='forum_option.aspx';</script>");
                return;
            }
            if (base.CheckCookie())
            {
                GeneralConfigInfo config = GeneralConfigInfo.Current;
                config.Modworkstatus = (int)Convert.ToInt16(this.modworkstatus.SelectedValue);
                config.Userstatusby = (int)Convert.ToInt16(this.userstatusby.SelectedValue);
                config.Rssttl = this.rssttl.Text.ToInt();
                config.Losslessdel = (int)Convert.ToInt16(this.losslessdel.Text);
                config.Editedby = (int)Convert.ToInt16(this.editedby.SelectedValue);
                config.Allowswitcheditor = (int)Convert.ToInt16(this.allowswitcheditor.SelectedValue);
                config.Reasonpm = (int)Convert.ToInt16(this.reasonpm.SelectedValue);
                config.Hottopic = (int)Convert.ToInt16(this.hottopic.Text);
                config.Starthreshold = (int)Convert.ToInt16(this.starthreshold.Text);
                config.Fastpost = (int)Convert.ToInt16(this.fastpost.SelectedValue);
                config.Enabletag = this.enabletag.SelectedValue.ToInt() == 1;
                config.Ratevalveset = this.ratevalveset1.Text + "," + this.ratevalveset2.Text + "," + this.ratevalveset3.Text + "," + this.ratevalveset4.Text + "," + this.ratevalveset5.Text;
                config.Forcewww = (int)Convert.ToInt16(this.forcewww.SelectedValue);
                config.Statstatus = this.statstatus.SelectedValue.ToBoolean();
                config.Hottagcount = (int)Convert.ToInt16(this.hottagcount.Text);
                config.Maxmodworksmonths = (int)Convert.ToInt16(this.maxmodworksmonths.Text);
                config.Replynotificationstatus = (int)Convert.ToInt16(this.replynotificationstatus.SelectedValue);
                config.Replyemailstatus = (int)Convert.ToInt16(this.replyemailstatus.SelectedValue);
                config.Viewnewtopicminute = this.viewnewtopicminute.Text.ToInt();
                config.Quickforward = this.quickforward.SelectedValue.ToInt();
                config.Msgforwardlist = this.msgforwardlist.Text.Replace("\r\n", ",");
                config.Rssstatus = (int)Convert.ToInt16(this.rssstatus.SelectedValue);
                config.Cachelog = (int)Convert.ToInt16(this.cachelog.SelectedValue);
                //config.Silverlight = (int)Convert.ToInt16(this.silverlight.SelectedValue);
                config.Save();

                //config.Save();;
                //TopicStats.SetQueueCount();
                Caches.ReSetConfig();
                AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "站点功能", "");
                base.RegisterStartupScript("PAGE", "window.location.href='forum_option.aspx';");
            }
        }

        private bool ValidateRatevalveset(string val)
        {
            if (!Utils.IsNumeric(val))
            {
                base.RegisterStartupScript("", "<script>alert('评分各项阀值只能是数字');window.location.href='forum_option.aspx';</script>");
                return false;
            }
            if (Convert.ToInt16(val) > 999 || Convert.ToInt16(val) < 1)
            {
                base.RegisterStartupScript("", "<script>alert('评分各项阀值只能在1-999之间');window.location.href='forum_option.aspx';</script>");
                return false;
            }
            return true;
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