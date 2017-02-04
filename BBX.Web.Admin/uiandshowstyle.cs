using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BBX.Common;
using BBX.Config;
using BBX.Control;
using BBX.Entity;
using BBX.Forum;
using NewLife;

namespace BBX.Web.Admin
{
    public class uiandshowstyle : AdminPage
    {
        protected HtmlForm Form1;
        protected Hint Hint1;
        protected BBX.Control.DropDownList templateid;
        protected HtmlImage preview;
        protected BBX.Control.RadioButtonList browsecreatetemplate;
        protected BBX.Control.RadioButtonList stylejump;
        protected BBX.Control.TextBox visitedforums;
        protected BBX.Control.TextBox maxsigrows;
        protected BBX.Control.RadioButtonList showsignatures;
        protected BBX.Control.RadioButtonList showimages;
        protected BBX.Control.RadioButtonList smileyinsert;
        protected BBX.Control.RadioButtonList showauthorstatusinpost;
        protected BBX.Control.RadioButtonList forumjump;
        protected BBX.Control.TextBox viewnewtopicminute;
        protected BBX.Control.RadioButtonList isframeshow;
        protected BBX.Control.RadioButtonList ratelisttype;
        protected BBX.Control.TextBox showratecount;
        protected TextareaResize postnocustom;
        protected BBX.Control.RadioButtonList quickforward;
        protected TextareaResize msgforwardlist;
        protected BBX.Control.RadioButtonList moddisplay;
        protected BBX.Control.RadioButtonList showavatars;
        protected BBX.Control.CheckBoxList allowfloatwin;
        protected BBX.Control.TextBox smiliesmax;
        protected BBX.Control.RadioButtonList whosonlinestatus;
        protected BBX.Control.RadioButtonList whosonlinecontact;
        protected BBX.Control.TextBox maxonlinelist;
        protected BBX.Control.TextBox onlinetimeout;
        protected BBX.Control.RadioButtonList openshare;
        protected BBX.Control.RadioButtonList shownewposticon;
        protected BBX.Control.RadioButtonList memliststatus;
        protected BBX.Control.RadioButtonList Indexpage;
        protected BBX.Control.RadioButtonList userstatusby;
        protected BBX.Control.TextBox tpp;
        protected BBX.Control.TextBox ppp;
        protected BBX.Control.RadioButtonList fastpost;
        protected BBX.Control.RadioButtonList moderactions;
        protected BBX.Control.RadioButtonList allowchangewidth;
        protected BBX.Control.RadioButtonList showwidthmode;
        protected BBX.Control.RadioButtonList debug;
        protected BBX.Control.RadioButtonList datediff;
        protected BBX.Control.Button SaveInfo;
        public string[] score;
        public string[] postleftarray;
        public string[] userfacearray;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadCustomauthorinfo();
            if (!this.Page.IsPostBack)
            {
                this.LoadConfigInfo();
                this.quickforward.Items[0].Attributes.Add("onclick", "setStatus(true)");
                this.quickforward.Items[1].Attributes.Add("onclick", "setStatus(false)");
                this.openshare.Items[0].Attributes.Add("onclick", "openShare(true)");
                this.openshare.Items[1].Attributes.Add("onclick", "openShare(false)");
            }
        }

        private void LoadCustomauthorinfo()
        {
            this.score = Scoresets.GetValidScoreName();
            var config = GeneralConfigInfo.Current;
            string customauthorinfo = config.Customauthorinfo;
            string text = Utils.SplitString(customauthorinfo, "|")[0];
            string text2 = Utils.SplitString(customauthorinfo, "|")[1];
            this.postleftarray = text.Split(',');
            this.userfacearray = text2.Split(',');
        }

        public void LoadConfigInfo()
        {
            this.templateid.Attributes.Add("onchange", "LoadImage(this.selectedIndex)");
            var config = GeneralConfigInfo.Current;
            this.stylejump.SelectedValue = config.Stylejump.ToString();
            this.browsecreatetemplate.SelectedValue = config.BrowseCreateTemplate.ToString();
            this.templateid.AddTableData(Template.GetValids(), "name", "id");
            this.debug.SelectedValue = config.Debug.ToString();
            this.templateid.SelectedValue = config.Templateid.ToString();
            this.templateid.Items.RemoveAt(0);
            string text = "<script type=\"text/javascript\">\r\n";
            text += "images = new Array();\r\n";
            for (int i = 0; i < this.templateid.Items.Count; i++)
            {
                object obj = text;
                text = obj + "images[" + i + "]=\"../../templates/" + this.templateid.Items[i].Text + "/about.png\";\r\n";
            }
            text += "</script>";
            base.RegisterStartupScript("", text);
            this.preview.Src = "../../templates/" + this.templateid.SelectedItem.Text + "/about.png";
            this.isframeshow.SelectedValue = config.Isframeshow.ToString();
            this.whosonlinestatus.SelectedValue = config.Whosonlinestatus.ToString();
            this.maxonlinelist.Text = config.Maxonlinelist.ToString();
            this.forumjump.SelectedValue = config.Forumjump.ToString();
            if (config.Onlinetimeout >= 0)
            {
                this.showauthorstatusinpost.SelectedValue = "2";
            }
            else
            {
                this.showauthorstatusinpost.SelectedValue = "1";
            }
            this.onlinetimeout.Text = Math.Abs(config.Onlinetimeout).ToString();
            this.smileyinsert.SelectedValue = config.Smileyinsert.ToString();
            this.visitedforums.Text = config.Visitedforums.ToString();
            this.moddisplay.SelectedValue = config.Moddisplay.ToString();
            this.showsignatures.SelectedValue = config.Showsignatures.ToString();
            this.showavatars.SelectedValue = config.Showavatars.ToString();
            this.showimages.SelectedValue = config.Showimages.ToString();
            this.maxsigrows.Text = config.Maxsigrows.ToString();
            this.smiliesmax.Text = config.Smiliesmax.ToString();
            this.whosonlinecontact.SelectedValue = config.WhosOnlineContract.ToString();
            this.postnocustom.Text = config.Postnocustom;
            this.quickforward.SelectedValue = config.Quickforward.ToString();
            this.msgforwardlist.Text = (config.Msgforwardlist + "").Replace(",", "\r\n");
            foreach (ListItem listItem in this.allowfloatwin.Items)
            {
                listItem.Selected = !config.Disallowfloatwin.Contains(listItem.Value);
            }
            this.openshare.SelectedValue = config.Disableshare.ToString();
            this.shownewposticon.SelectedValue = config.Shownewposticon.ToString();
            this.ratelisttype.SelectedValue = config.Ratelisttype.ToString();
            this.showratecount.Text = config.DisplayRateCount.ToString();
            this.moderactions.SelectedValue = config.Moderactions.ToString();
            this.memliststatus.SelectedValue = config.Memliststatus.ToString();
            this.Indexpage.SelectedIndex = Convert.ToInt32(config.Indexpage.ToString());
            this.tpp.Text = config.Tpp.ToString();
            this.ppp.Text = config.Ppp.ToString();
            this.fastpost.SelectedValue = config.Fastpost.ToString();
            this.userstatusby.SelectedValue = ((config.Userstatusby.ToString() != "0") ? "1" : "0");
            this.allowchangewidth.SelectedValue = config.Allowchangewidth.ToString();
            this.showwidthmode.SelectedValue = config.Showwidthmode.ToString();
            this.datediff.SelectedValue = config.DateDiff.ToString();
        }

        private void SaveInfo_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                SortedList sortedList = new SortedList();
                sortedList.Add("无动作离线时间", this.onlinetimeout.Text);
                sortedList.Add("最大签名高度", this.maxsigrows.Text);
                sortedList.Add("显示最近访问论坛数量", this.visitedforums.Text);
                sortedList.Add("帖子中同一表情符出现的最大次数", this.smiliesmax.Text);
                string[] array = Utils.SplitString(DNTRequest.GetFormString("postleft"), ",");
                string[] array2 = Utils.SplitString(DNTRequest.GetFormString("userface"), ",");
                string text = "";
                string text2 = "";
                foreach (DictionaryEntry item in sortedList)
                {
                    if (item.Value.ToInt(-1) < 0)
                    {
                        base.RegisterStartupScript("", "<script>alert('输入错误:" + item.Key + ",只能是0或者正整数');window.location.href='global_uiandshowstyle.aspx';</script>");
                        return;
                    }
                }
                if (this.onlinetimeout.Text.ToInt() <= 0)
                {
                    base.RegisterStartupScript("", "<script>alert('无动作离线时间必须大于0');</script>");
                }
                else
                {
                    if (this.maxsigrows.Text.ToInt() > 9999 || this.maxsigrows.Text.ToInt() < 0)
                    {
                        base.RegisterStartupScript("", "<script>alert('最大签名高度只能在0-9999之间');window.location.href='.aspx';</script>");
                        return;
                    }
                    if (this.visitedforums.Text.ToInt() > 9999 || this.visitedforums.Text.ToInt() < 0)
                    {
                        base.RegisterStartupScript("", "<script>alert('显示最近访问论坛数量只能在0-9999之间');window.location.href='global_uiandshowstyle.aspx';</script>");
                        return;
                    }
                    if (this.smiliesmax.Text.ToInt() > 1000 || this.smiliesmax.Text.ToInt() < 0)
                    {
                        base.RegisterStartupScript("", "<script>alert('帖子中同一表情符出现的最大次数只能在0-1000之间');window.location.href='global_uiandshowstyle.aspx';</script>");
                        return;
                    }
                    if (this.showratecount.Text.ToInt() > 100 || this.showratecount.Text.ToInt() < 0)
                    {
                        base.RegisterStartupScript("", "<script>alert('显示帖子评分的数量必须在0-100之间');window.location.href='global_uiandshowstyle.aspx';</script>");
                        return;
                    }
                    if (this.tpp.Text.ToInt() > 100 || this.tpp.Text.ToInt() <= 0)
                    {
                        base.RegisterStartupScript("", "<script>alert('每页主题数只能在1-100之间');window.location.href='global_uiandshowstyle.aspx';</script>");
                        return;
                    }
                    if (this.ppp.Text.ToInt() > 100 || this.ppp.Text.ToInt() <= 0)
                    {
                        base.RegisterStartupScript("", "<script>alert('每页帖子数只能在1-100之间');window.location.href='global_uiandshowstyle.aspx';</script>");
                        return;
                    }
                    string[] array3 = array;
                    for (int i = 0; i < array3.Length; i++)
                    {
                        string str = array3[i];
                        text += str;
                        text += ",";
                    }
                    string[] array4 = array2;
                    for (int j = 0; j < array4.Length; j++)
                    {
                        string str2 = array4[j];
                        text2 += str2;
                        text2 += ",";
                    }
                    GeneralConfigInfo config = GeneralConfigInfo.Current;
                    config.Customauthorinfo = text.TrimEnd(',') + "|" + text2.TrimEnd(',');
                    config.Templateid = this.templateid.SelectedValue.ToInt();
                    config.Subforumsindex = 1;
                    config.Stylejump = this.stylejump.SelectedValue.ToInt();
                    config.BrowseCreateTemplate = this.browsecreatetemplate.SelectedValue.ToInt();
                    config.Isframeshow = this.isframeshow.SelectedValue.ToInt();
                    config.Whosonlinestatus = this.whosonlinestatus.SelectedValue.ToInt();
                    if (this.showauthorstatusinpost.SelectedValue == "1")
                    {
                        config.Onlinetimeout = -this.onlinetimeout.Text.ToInt();
                    }
                    else
                    {
                        config.Onlinetimeout = this.onlinetimeout.Text.ToInt();
                    }
                    config.Maxonlinelist = this.maxonlinelist.Text.ToInt();
                    config.Forumjump = this.forumjump.SelectedValue.ToInt();
                    config.Smileyinsert = this.smileyinsert.SelectedValue.ToInt();
                    config.Visitedforums = this.visitedforums.Text.ToInt();
                    config.Moddisplay = this.moddisplay.SelectedValue.ToInt();
                    config.Showsignatures = this.showsignatures.SelectedValue.ToInt();
                    config.Showavatars = this.showavatars.SelectedValue.ToInt();
                    config.Showimages = this.showimages.SelectedValue.ToInt();
                    config.Smiliesmax = this.smiliesmax.Text.ToInt();
                    config.Maxsigrows = this.maxsigrows.Text.ToInt();
                    config.WhosOnlineContract = this.whosonlinecontact.SelectedValue.ToBoolean();
                    config.Postnocustom = this.postnocustom.Text;
                    config.Quickforward = this.quickforward.SelectedValue.ToInt();
                    config.Msgforwardlist = this.msgforwardlist.Text.Replace("\r\n", ",");
                    string text3 = "";
                    foreach (ListItem listItem in this.allowfloatwin.Items)
                    {
                        text3 += ((!listItem.Selected) ? (listItem.Value + "|") : "");
                    }
                    config.Disallowfloatwin = text3.TrimEnd('|');
                    config.Disableshare = this.openshare.SelectedValue.ToInt();
                    config.Shownewposticon = this.shownewposticon.SelectedValue.ToInt();
                    config.Ratelisttype = this.ratelisttype.SelectedValue.ToInt();
                    config.DisplayRateCount = this.showratecount.Text.ToInt();
                    config.Moderactions = this.moderactions.SelectedValue.ToInt();
                    config.Memliststatus = this.memliststatus.SelectedValue.ToInt();
                    config.Indexpage = this.Indexpage.SelectedValue.ToInt();
                    config.Tpp = this.tpp.Text.ToInt();
                    config.Ppp = this.ppp.Text.ToInt();
                    config.Fastpost = this.fastpost.SelectedValue.ToInt();
                    config.Userstatusby = this.userstatusby.SelectedValue.ToInt();
                    config.Allowchangewidth = this.allowchangewidth.SelectedValue.ToInt();
                    config.Showwidthmode = this.showwidthmode.SelectedValue.ToInt();
                    config.Debug = (int)Convert.ToInt16(this.debug.SelectedValue);
                    config.DateDiff = this.datediff.SelectedValue.ToInt();
                    config.Save(); ;
                    AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "界面与显示方式设置", "");
                    base.RegisterStartupScript("PAGE", "window.location.href='global_uiandshowstyle.aspx';");
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