using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using BBX.Cache;
using BBX.Common;
using BBX.Config;
using BBX.Control;
using BBX.Entity;
using BBX.Forum;

namespace BBX.Web.Admin
{
    public class siteoptimization : AdminPage
    {
        protected HtmlForm Form1;
        protected RadioButtonList fulltextsearch;
        protected RadioButtonList nocacheheaders;
        protected TextBox maxonlines;
        protected TextBox searchctrl;
        //protected HtmlInputRadioButton Topicqueuestats_1;
        //protected HtmlInputRadioButton Topicqueuestats_0;
        //protected TextBox topicqueuestatscount;
        protected TextBox statscachelife;
        protected TextBox guestcachepagetimeout;
        protected TextBox oltimespan;
        protected TextBox topiccachemark;
        protected RadioButtonList showauthorstatusinpost;
        protected TextBox onlinetimeout;
        protected TextBox notificationreserveddays;
        protected TextBox maxindexsubforumcount;
        protected TextBox deletingexpireduserfrequency;
        protected RadioButtonList onlineoptimization;
        protected RadioButtonList avatarmethod;
        protected RadioButtonList showimgattachmode;
        protected RadioButtonList posttimestoragemedia;
        protected TextBox onlineusercountcacheminute;
        protected TextBox jqueryurl;
        protected TextBox txtImageServer;
        protected TextBox txtJsServer;
        protected TextBox txtCssServer;
        protected Button SaveInfo;
        protected RadioButtonList iisurlrewrite;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.LoadConfigInfo();
            }
        }

        public void LoadConfigInfo()
        {
            GeneralConfigInfo config = GeneralConfigInfo.Current;
            this.fulltextsearch.SelectedValue = config.Fulltextsearch.ToString();
            this.nocacheheaders.SelectedValue = config.Nocacheheaders.ToString();
            this.maxonlines.Text = config.Maxonlines.ToString();
            this.searchctrl.Text = config.Searchctrl.ToString();
            this.statscachelife.Text = config.Statscachelife.ToString();
            this.guestcachepagetimeout.Text = config.Guestcachepagetimeout.ToString();
            this.oltimespan.Text = config.Oltimespan.ToString();
            this.topiccachemark.Text = config.Topiccachemark.ToString();
            this.notificationreserveddays.Text = config.Notificationreserveddays.ToString();
            this.maxindexsubforumcount.Text = config.Maxindexsubforumcount.ToString();
            this.deletingexpireduserfrequency.Text = config.DeletingExpiredUserFrequency.ToString();
            this.onlineoptimization.SelectedValue = config.Onlineoptimization.ToString();
            this.avatarmethod.SelectedValue = config.AvatarStatic.ToString().ToLower();
            this.showimgattachmode.SelectedValue = config.Showimgattachmode.ToString();
            this.onlineusercountcacheminute.Text = config.OnlineUserCountCacheMinute.ToString();
            this.posttimestoragemedia.SelectedValue = config.PostTimeStorageMedia.ToString();
            this.showauthorstatusinpost.SelectedValue = ((config.Onlinetimeout >= 0) ? "2" : "1");
            this.onlinetimeout.Text = (((config.Onlinetimeout > 0) ? 1 : -1) * config.Onlinetimeout).ToString();
            this.jqueryurl.Text = config.Jqueryurl;
            this.txtImageServer.Text = config.ImageServer;
            this.txtJsServer.Text = config.JsServer;
            this.txtCssServer.Text = config.CssServer;
        }

        private void SaveInfo_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                var config = GeneralConfigInfo.Current;
                var dic = new Dictionary<String, Int32>();
                dic["最大在线人数"] = maxonlines.Text.ToInt(-1);
                dic["搜索时间限制"] = searchctrl.Text.ToInt(-1);
                foreach (var item in dic)
                {
                    if (item.Value < 0)
                    {
                        base.RegisterStartupScript("", "<script>alert('输入错误:" + item.Key + ",只能是0或者正整数');window.location.href='global_safecontrol.aspx';</script>");
                        return;
                    }
                }
                if (this.fulltextsearch.SelectedValue == "1")
                {
                    //string text = "";
                    //config.Fulltextsearch = Databases.TestFullTextIndex(ref text);
                    throw new Exception("不支持全文索引");
                }
                else
                {
                    config.Fulltextsearch = 0;
                }
                if (notificationreserveddays.Text.ToInt(-1) < 0)
                {
                    base.RegisterStartupScript("", "<script>alert('通知保留天数只能为正数或0!');</script>");
                }
                else
                {
                    if (maxindexsubforumcount.Text.ToInt(-1) < 0)
                    {
                        base.RegisterStartupScript("", "<script>alert('首页每个分类下最多显示版块数只能为正数或0!');</script>");
                        return;
                    }
                    if (deletingexpireduserfrequency.Text.ToInt(0) < 1)
                    {
                        base.RegisterStartupScript("", "<script>alert('删除离线用户频率只能为正数!');</script>");
                        return;
                    }
                    config.DeletingExpiredUserFrequency = this.deletingexpireduserfrequency.Text.ToInt(1);
                    config.Maxindexsubforumcount = this.maxindexsubforumcount.Text.ToInt(0);
                    config.Notificationreserveddays = this.notificationreserveddays.Text.ToInt(0);
                    //config.TopicQueueStatsCount = this.topicqueuestatscount.Text.ToInt();
                    config.Nocacheheaders = (int)Convert.ToInt16(this.nocacheheaders.SelectedValue);
                    config.Maxonlines = this.maxonlines.Text.ToInt();
                    config.Searchctrl = this.searchctrl.Text.ToInt();
                    config.Statscachelife = (int)Convert.ToInt16(this.statscachelife.Text);
                    config.Guestcachepagetimeout = (int)Convert.ToInt16(this.guestcachepagetimeout.Text);
                    config.Oltimespan = (int)Convert.ToInt16(this.oltimespan.Text);
                    config.Topiccachemark = (int)Convert.ToInt16(this.topiccachemark.Text);
                    config.Onlineoptimization = this.onlineoptimization.SelectedValue.ToInt();
                    config.AvatarStatic = avatarmethod.SelectedValue.ToBoolean();
                    config.Showimgattachmode = (int)Convert.ToInt16(this.showimgattachmode.SelectedValue);
                    //config.TopicQueueStats = (this.Topicqueuestats_1.Checked ? 1 : 0);
                    config.OnlineUserCountCacheMinute = this.onlineusercountcacheminute.Text.ToInt();
                    config.PostTimeStorageMedia = this.posttimestoragemedia.SelectedValue.ToInt();
                    config.Onlinetimeout = ((this.showauthorstatusinpost.SelectedValue == "1") ? (-this.onlinetimeout.Text.ToInt()) : this.onlinetimeout.Text.ToInt());
                    config.Jqueryurl = this.jqueryurl.Text;
                    config.ImageServer = this.txtImageServer.Text;
                    config.JsServer = this.txtJsServer.Text;
                    config.CssServer = this.txtCssServer.Text;
                    config.Save(); ;

                    Caches.ReSetConfig();
                    AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "站点优化", "");
                    base.RegisterStartupScript("PAGE", "window.location.href='global_siteoptimization.aspx';");
                    return;
                }
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