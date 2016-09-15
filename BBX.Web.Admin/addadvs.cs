using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BBX.Cache;
using BBX.Common;
using BBX.Config;
using BBX.Control;
using BBX.Entity;

namespace BBX.Web.Admin
{
    public class addadvs : AdminPage
    {
        protected HtmlForm Form1;
        protected pageinfo info1;
        protected pageinfo PageInfo1;
        protected pageinfo PageInfo2;
        protected pageinfo PageInfo3;
        protected pageinfo PageInfo4;
        protected pageinfo PageInfo5;
        protected pageinfo PageInfo6;
        protected pageinfo PageInfo7;
        protected pageinfo PageInfo8;
        protected pageinfo PageInfo9;
        protected BBX.Control.RadioButtonList available;
        protected BBX.Control.DropDownList parameters;
        protected BBX.Control.Calendar starttime;
        protected BBX.Control.DropDownList type;
        protected BBX.Control.TextBox displayorder;
        protected BBX.Control.Calendar endtime;
        protected BBX.Control.TextBox title;
        protected forumtree TargetFID;
        protected BBX.Control.RadioButtonList inpostposition;
        protected BBX.Control.ListBox inpostfloor;
        protected BBX.Control.TextBox code;
        protected BBX.Control.TextBox wordcontent;
        protected BBX.Control.TextBox wordfont;
        protected BBX.Control.TextBox wordlink;
        protected BBX.Control.TextBox imgsrc;
        protected BBX.Control.TextBox imgwidth;
        protected BBX.Control.TextBox imglink;
        protected BBX.Control.TextBox imgheight;
        protected BBX.Control.TextBox imgtitle;
        protected BBX.Control.TextBox flashwidth;
        protected BBX.Control.TextBox flashheight;
        protected BBX.Control.TextBox flashsrc;
        protected BBX.Control.TextBox slwmvsrc;
        protected BBX.Control.TextBox slimage;
        protected BBX.Control.TextBox buttomimg;
        protected BBX.Control.TextBox words1;
        protected BBX.Control.TextBox words2;
        protected BBX.Control.TextBox words3;
        protected Hint Hint1;
        protected BBX.Control.Button AddAdInfo;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                GeneralConfigInfo config = GeneralConfigInfo.Current;
                for (int i = 1; i <= config.Ppp; i++)
                {
                    this.inpostfloor.Items.Add(new ListItem(" >#" + i, i.ToString()));
                }
            }
        }

        private void AddAdInfo_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                string TargetFID = Request["TargetFID"];
                if ((String.IsNullOrEmpty(TargetFID) || TargetFID == ",") && this.type.SelectedIndex < 10)
                {
                    base.RegisterStartupScript("", "<script>alert('请您先选取相关的投放范围,再点击提交按钮');showadhint(Form1.type.value);showparameters(Form1.parameters.value);</script>");
                    return;
                }
                string text = this.starttime.SelectedDate.ToString();
                string text2 = this.endtime.SelectedDate.ToString();
                if (text.IndexOf("1900") < 0 && text2.IndexOf("1900") < 0 && Convert.ToDateTime(this.starttime.SelectedDate.ToString()) >= Convert.ToDateTime(this.endtime.SelectedDate.ToString()))
                {
                    base.RegisterStartupScript("", "<script>alert('生效时间应该早于结束时间');showadhint(Form1.type.value);showparameters(Form1.parameters.value);</script>");
                    return;
                }
                if (this.endtime.SelectedDate < DateTime.Now)
                {
                    base.RegisterStartupScript("", "<script>alert('您选择的结束日期已过期,请重新选择一个大于今天的日期');showadhint(Form1.type.value);showparameters(Form1.parameters.value);</script>");
                    return;
                }
                Advertisement.CreateAd(Utils.StrToInt(available.SelectedValue, 0), type.SelectedValue, Utils.StrToInt(this.displayorder.Text, 0), title.Text, TargetFID, GetParameters(), GetCode(), text, text2);
                XCache.Remove(CacheKeys.FORUM_ADVERTISEMENTS);
                base.RegisterStartupScript("PAGE", "window.location.href='advsgrid.aspx';");
            }
        }

        public string GetCode()
        {
            string result = "";
            string selectedValue;
            if ((selectedValue = this.parameters.SelectedValue) != null)
            {
                if (!(selectedValue == "htmlcode"))
                {
                    if (!(selectedValue == "word"))
                    {
                        if (!(selectedValue == "image"))
                        {
                            if (selectedValue == "flash")
                            {
                                result = string.Format("<embed wmode=\"opaque\"{0}{1} src=\"{2}\" type=\"application/x-shockwave-flash\"></embed>", this.flashwidth.Text.IsNullOrWhiteSpace() ? "" : (" width=\"" + this.flashwidth.Text.Trim() + "\""), this.flashheight.Text.IsNullOrWhiteSpace() ? "" : (" height=\"" + this.flashheight.Text.Trim() + "\""), this.flashsrc.Text.Trim());
                            }
                        }
                        else
                        {
                            result = string.Format("<a href=\"{0}\" target=\"_blank\"><img src=\"{1}\"{2}{3} alt=\"{4}\" border=\"0\" /></a>", new object[]
                            {
                                this.imglink.Text.Trim(),
                                this.imgsrc.Text.Trim(),
                                this.imgwidth.Text.IsNullOrWhiteSpace() ? "" : (" width=\"" + this.imgwidth.Text.Trim() + "\""),
                                this.imgheight.Text.IsNullOrWhiteSpace() ? "" : (" height=\"" + this.imgheight.Text.Trim() + "\""),
                                this.imgtitle.Text.Trim()
                            });
                        }
                    }
                    else
                    {
                        result = string.Format("<a href=\"{0}\" target=\"_blank\" style=\"font-size:{1}\">{2}</a>", this.wordlink.Text.Trim(), this.wordfont.Text, this.wordcontent.Text.Trim());
                    }
                }
                else
                {
                    result = this.code.Text.Trim();
                }
            }
            if (this.type.SelectedValue == Convert.ToInt16(AdType.MediaAd).ToString())
            {
                result = "<script type='text/javascript' src='templates/{0}/mediaad.js'></script><script type='text/javascript'>printMediaAD('{1}', {2});</script>";
            }
            return result;
        }

        public string GetParameters()
        {
            string text = "";
            string selectedValue;
            if ((selectedValue = this.parameters.SelectedValue) != null)
            {
                if (!(selectedValue == "htmlcode"))
                {
                    if (!(selectedValue == "word"))
                    {
                        if (!(selectedValue == "image"))
                        {
                            if (selectedValue == "flash")
                            {
                                text = string.Format("flash|{0}|{1}|{2}||||", this.flashsrc.Text.Trim(), this.flashwidth.Text.Trim(), this.flashheight.Text);
                            }
                        }
                        else
                        {
                            text = string.Format("image|{0}|{1}|{2}|{3}|{4}||", new object[]
                            {
                                this.imgsrc.Text.Trim(),
                                this.imgwidth.Text.Trim(),
                                this.imgheight.Text.Trim(),
                                this.imglink.Text.Trim(),
                                this.imgtitle.Text.Trim()
                            });
                        }
                    }
                    else
                    {
                        text = string.Format("word| | | | {0}|{1}|{2}|", this.wordlink.Text.Trim(), this.wordcontent.Text.Trim(), this.wordfont.Text);
                    }
                }
                else
                {
                    text = "htmlcode|||||||";
                }
            }
            if (this.type.SelectedValue == Convert.ToInt16(AdType.MediaAd).ToString())
            {
                text = string.Format("silverlight|{0}|{1}|{2}|{3}|{4}|{5}|{6}", new object[]
                {
                    this.slwmvsrc.Text.Trim(),
                    this.slimage.Text.Trim(),
                    this.slimage.Text,
                    this.buttomimg.Text,
                    this.words1.Text,
                    this.words2.Text,
                    this.words3.Text
                });
            }
            if (this.type.SelectedValue == Convert.ToInt16(AdType.InPostAd).ToString())
            {
                text += string.Format("{0}|{1}|", this.inpostposition.SelectedValue, this.GetMultipleSelectedValue(this.inpostfloor));
            }
            return text;
        }

        private string GetMultipleSelectedValue(BBX.Control.ListBox lb)
        {
            string text = string.Empty;
            foreach (ListItem listItem in lb.Items)
            {
                if (listItem.Selected && listItem.Value != "-1")
                {
                    text = text + listItem.Value + ",";
                }
            }
            if (text.Length > 0)
            {
                text = text.Substring(0, text.Length - 1);
            }
            return text;
        }

        private void type_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.type.SelectedValue == Convert.ToInt16(AdType.FloatAd).ToString() || this.type.SelectedValue == Convert.ToInt16(AdType.DoubleAd).ToString())
            {
                if (this.parameters.Items[1].Value == "word")
                {
                    this.parameters.Items.RemoveAt(1);
                    return;
                }
            }
            else
            {
                if (this.parameters.Items[1].Value != "word")
                {
                    this.parameters.Items.Insert(1, new ListItem("文字", "word"));
                }
            }
        }

        //protected override void SavePageStateToPersistenceMedium(object viewState)
        //{
        //    base.MySavePageState(viewState);
        //}

        //protected override object LoadPageStateFromPersistenceMedium()
        //{
        //    return base.MyLoadPageState();
        //}

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.type.SelectedIndexChanged += new EventHandler(this.type_SelectedIndexChanged);
            this.AddAdInfo.Click += new EventHandler(this.AddAdInfo_Click);
            this.starttime.SelectedDate = DateTime.Now;
            this.endtime.SelectedDate = DateTime.Now.AddDays(7.0);
            this.title.AddAttributes("maxlength", "40");
            this.title.AddAttributes("size", "40");
            this.type.Items.Clear();
            this.type.Items.Add(new ListItem("头部横幅广告", Convert.ToInt16(AdType.HeaderAd).ToString()));
            this.type.Items.Add(new ListItem("尾部横幅广告", Convert.ToInt16(AdType.FooterAd).ToString()));
            this.type.Items.Add(new ListItem("页内文字广告", Convert.ToInt16(AdType.PageWordAd).ToString()));
            this.type.Items.Add(new ListItem("帖内广告", Convert.ToInt16(AdType.InPostAd).ToString()));
            this.type.Items.Add(new ListItem("帖间通栏广告", Convert.ToInt16(AdType.PostLeaderboardAd).ToString()));
            this.type.Items.Add(new ListItem("浮动广告", Convert.ToInt16(AdType.FloatAd).ToString()));
            this.type.Items.Add(new ListItem("对联广告", Convert.ToInt16(AdType.DoubleAd).ToString()));
            this.type.Items.Add(new ListItem("分类间广告", Convert.ToInt16(AdType.InForumAd).ToString()));
            this.type.Items.Add(new ListItem("快速发帖栏上方广告", Convert.ToInt16(AdType.QuickEditorAd).ToString()));
            this.type.Items.Add(new ListItem("快速编辑器背景广告", Convert.ToInt16(AdType.QuickEditorBgAd).ToString()));
            this.type.Items.Add(new ListItem("聚合首页头部广告", Convert.ToInt16(AdType.WebSiteHeaderAd).ToString()));
            this.type.Items.Add(new ListItem("聚合首页热贴下方广告", Convert.ToInt16(AdType.WebSiteHotTopicAd).ToString()));
            this.type.Items.Add(new ListItem("聚合首页发帖排行上方广告", Convert.ToInt16(AdType.WebSiteUserPostTopAd).ToString()));
            this.type.Items.Add(new ListItem("聚合首页推荐版块上方广告", Convert.ToInt16(AdType.WebSiteRecForumTopAd).ToString()));
            this.type.Items.Add(new ListItem("聚合首页推荐版块下方广告", Convert.ToInt16(AdType.WebSiteRecForumBottomAd).ToString()));
            this.type.Items.Add(new ListItem("聚合首页推荐相册下方广告", Convert.ToInt16(AdType.WebSiteRecAlbumAd).ToString()));
            this.type.Items.Add(new ListItem("聚合首页底部广告", Convert.ToInt16(AdType.WebSiteBottomAd).ToString()));
            this.type.Items.Add(new ListItem("页内横幅广告", Convert.ToInt16(AdType.PageAd).ToString()));
            this.type.Attributes.Add("onChange", "showadhint();");
            this.type.SelectedIndex = 0;
            this.parameters.Items.Clear();
            this.parameters.Items.Add(new ListItem("代码", "htmlcode"));
            this.parameters.Items.Add(new ListItem("文字", "word"));
            this.parameters.Items.Add(new ListItem("图片", "image"));
            this.parameters.Items.Add(new ListItem("flash", "flash"));
            this.parameters.Attributes.Add("onChange", "showparameters();");
            this.parameters.SelectedIndex = 0;
        }
    }
}