using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BBX.Common;
using BBX.Config;
using BBX.Control;
using BBX.Entity;

namespace BBX.Web.Admin
{
    public class editadvs : AdminPage
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
        protected BBX.Control.Button UpdateADInfo;
        protected BBX.Control.Button DeleteADInfo;
        protected BBX.Control.Button AddAdInfo;
        protected Literal msg;

        public void LoadAnnounceInf(int advid)
        {
            var config = GeneralConfigInfo.Current;
            for (int i = 1; i <= config.Ppp; i++)
            {
                this.inpostfloor.Items.Add(new ListItem(" >#" + i, i.ToString()));
            }

            var entity = Advertisement.FindByID(advid);
            {
                displayorder.Text = entity.DisplayOrder.ToString();
                available.SelectedValue = entity.Available.ToString();
                type.SelectedValue = entity.Type.ToString().Trim();
                title.Text = entity.Title;
                if (entity.StartTime > DateTime.MinValue) starttime.SelectedDate = entity.StartTime; ;
                if (entity.EndTime > DateTime.MinValue) endtime.SelectedDate = entity.EndTime;
                code.Text = entity.Code;

                parameters.Items.Clear();
                parameters.Items.Add(new ListItem("代码", "htmlcode"));
                if (type.SelectedValue != Convert.ToInt16(AdType.FloatAd).ToString() && type.SelectedValue != Convert.ToInt16(AdType.DoubleAd).ToString())
                {
                    parameters.Items.Add(new ListItem("文字", "word"));
                }
                parameters.Items.Add(new ListItem("图片", "image"));
                parameters.Items.Add(new ListItem("flash", "flash"));
                var ps = Utils.SplitString(entity.Parameters.Trim(), "|", 9);
                parameters.SelectedValue = ps[0].Trim();
                parameters.Attributes.Add("onChange", "showparameters();");
                wordlink.Text = ps[4].Trim();
                wordcontent.Text = ps[5].Trim();
                wordfont.Text = ps[6].Trim();
                imgsrc.Text = ps[1].Trim();
                imgwidth.Text = ps[2].Trim();
                imgheight.Text = ps[3].Trim();
                imglink.Text = ps[4].Trim();
                imgtitle.Text = ps[5].Trim();
                flashsrc.Text = ps[1].Trim();
                flashwidth.Text = ps[2].Trim();
                flashheight.Text = ps[3].Trim();
                if (type.SelectedValue == Convert.ToInt16(AdType.InPostAd).ToString())
                {
                    inpostposition.SelectedValue = ps[7].Trim();
                    string text = "";
                    string[] array2 = ps[8].Trim().Split(',');
                    for (int j = 0; j < array2.Length; j++)
                    {
                        string text2 = array2[j];
                        if (text2.ToInt(0) > config.Ppp)
                        {
                            text += text2 + ",";
                        }
                        else
                        {
                            foreach (ListItem listItem in this.inpostfloor.Items)
                            {
                                if (Utils.InArray(listItem.Value, ps[8].Trim()))
                                {
                                    listItem.Selected = true;
                                }
                            }
                        }
                    }
                    if (text != "")
                    {
                        base.RegisterStartupScript("", "<script>window.onload = function(){alert('每页帖数已经改变，原#" + text.TrimEnd(',') + "层大于现在" + config.Ppp + "层');}</script>");
                    }
                }
                if (this.type.SelectedValue == Convert.ToInt16(AdType.MediaAd).ToString())
                {
                    this.slwmvsrc.Text = ps[1].Trim();
                    this.slimage.Text = ps[2].Trim();
                    this.buttomimg.Text = ps[4].Trim();
                    this.words1.Text = ps[5].Trim();
                    this.words2.Text = ps[6].Trim();
                    this.words3.Text = ps[7].Trim();
                }
            }
        }

        private void UpdateADInfo_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                string targets = Request["TargetFID"];
                if ((String.IsNullOrEmpty(targets) || targets == ",") && this.type.SelectedIndex < 10)
                {
                    base.RegisterStartupScript("", "<script>alert('请您先选取相关的投放范围,再点击提交按钮');showadhint(Form1.type.value);showparameters(Form1.parameters.value);</script>");
                    return;
                }
                if (this.endtime.SelectedDate.ToString().IndexOf("1900") == 0)
                {
                    base.RegisterStartupScript("", "<script>alert('结束时间不能为空');showadhint(Form1.type.value);showparameters(Form1.parameters.value);</script>");
                    return;
                }
                if (this.starttime.SelectedDate.ToString().IndexOf("1900") < 0 && this.endtime.SelectedDate.ToString().IndexOf("1900") < 0 && Convert.ToDateTime(this.starttime.SelectedDate.ToString()) >= Convert.ToDateTime(this.endtime.SelectedDate.ToString()))
                {
                    base.RegisterStartupScript("", "<script>alert('生效时间应该早于结束时间');showadhint(Form1.type.value);showparameters(Form1.parameters.value);</script>");
                    return;
                }
                if (this.endtime.SelectedDate < DateTime.Now)
                {
                    base.RegisterStartupScript("", "<script>alert('您选择的结束日期已过期,请重新选择一个大于今天的日期');showadhint(Form1.type.value);showparameters(Form1.parameters.value);</script>");
                    return;
                }
                string code;
                if (this.type.SelectedValue == Convert.ToInt16(AdType.QuickEditorBgAd).ToString())
                {
                    code = this.imglink.Text + "\r" + this.imgsrc.Text;
                }
                else
                {
                    code = this.GetCode();
                }

                //Advertisements.UpdateAdvertisement(DNTRequest.GetInt("advid", 0), available.SelectedValue.ToInt(), type.SelectedValue, displayorder.Text.ToInt(), title.Text, targets, GetParameters(), text, starttime.SelectedDate.ToString(), endtime.SelectedDate.ToString());

                var entity = Advertisement.FindByID(DNTRequest.GetInt("advid", 0));
                if (entity != null)
                {
                    entity.Available = Int32.Parse(available.SelectedValue);
                    entity.Type = type.SelectedValue;
                    entity.DisplayOrder = Int32.Parse(displayorder.Text);
                    entity.Title = title.Text;

                    targets = targets.IndexOf("全部") >= 0 ? ",全部," : ("," + targets + ",");
                    entity.Targets = targets;
                    entity.Parameters = GetParameters();
                    entity.Code = code;
                    entity.StartTime = starttime.SelectedDate;
                    entity.EndTime = endtime.SelectedDate;
                    entity.Save();
                }

                base.Response.Redirect("advsgrid.aspx");
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
                                result = "<embed wmode=\"opaque\"" + ((String.IsNullOrEmpty(this.flashwidth.Text.Trim())) ? "" : (" width=\"" + this.flashwidth.Text.Trim() + "\"")) + (this.flashheight.Text.IsNullOrWhiteSpace() ? "" : (" height=\"" + this.flashheight.Text.Trim() + "\"")) + " src=\"" + this.flashsrc.Text.Trim() + "\" type=\"application/x-shockwave-flash\"></embed>";
                            }
                        }
                        else
                        {
                            result = "<a href=\"" + this.imglink.Text.Trim() + "\" target=\"_blank\"><img src=\"" + this.imgsrc.Text.Trim() + "\"" + (this.imgwidth.Text.IsNullOrWhiteSpace() ? "" : (" width=\"" + this.imgwidth.Text.Trim() + "\"")) + ((this.imgheight.Text.IsNullOrEmpty()) ? "" : (" height=\"" + this.imgheight.Text.Trim() + "\"")) + " alt=\"" + this.imgtitle.Text.Trim() + "\" border=\"0\" /></a>";
                        }
                    }
                    else
                    {
                        result = "<a href=\"" + this.wordlink.Text.Trim() + "\" target=\"_blank\" style=\"font-size: " + this.wordfont.Text + "\">" + this.wordcontent.Text.Trim() + "</a>";
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
                                text = "flash|" + this.flashsrc.Text.Trim() + "|" + this.flashwidth.Text.Trim() + "|" + this.flashheight.Text + "||||";
                            }
                        }
                        else
                        {
                            text = "image|" + this.imgsrc.Text.Trim() + "|" + this.imgwidth.Text.Trim() + "|" + this.imgheight.Text.Trim() + "|" + this.imglink.Text.Trim() + "|" + this.imgtitle.Text.Trim() + "||";
                        }
                    }
                    else
                    {
                        text = "word| | | | " + this.wordlink.Text.Trim() + "|" + this.wordcontent.Text.Trim() + "|" + this.wordfont.Text + "|";
                    }
                }
                else
                {
                    text = "htmlcode|||||||";
                }
            }
            if (this.type.SelectedValue == Convert.ToInt16(AdType.MediaAd).ToString())
            {
                text = "silverlight|" + this.slwmvsrc.Text.Trim() + "|" + this.slimage.Text.Trim() + "|" + this.slimage.Text + "|" + this.buttomimg.Text + "|" + this.words1.Text + "|" + this.words2.Text + "|" + this.words3.Text;
            }
            if (this.type.SelectedValue == Convert.ToInt16(AdType.InPostAd).ToString())
            {
                string text2 = text;
                text = text2 + this.inpostposition.SelectedValue + "|" + this.GetMultipleSelectedValue(this.inpostfloor) + "|";
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

        private void DeleteADInfo_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                Advertisement.DeleteAll(Request["advid"]);
                base.RegisterStartupScript("PAGE", "window.location.href='advsgrid.aspx';");
            }
        }

        private void type_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.type.SelectedValue == Convert.ToInt16(AdType.FloatAd).ToString() || this.type.SelectedValue == Convert.ToInt16(AdType.DoubleAd).ToString())
            {
                if (this.parameters.Items[1].Value == "word")
                {
                    this.parameters.Items.RemoveAt(1);
                }
            }
            else
            {
                if (this.parameters.Items[1].Value != "word")
                {
                    this.parameters.Items.Insert(1, new ListItem("文字", "word"));
                }
            }
            if (this.type.SelectedValue == Convert.ToInt16(AdType.QuickEditorBgAd).ToString())
            {
                for (int i = 0; i < this.parameters.Items.Count; i++)
                {
                    if (this.parameters.Items[i].Value != "image")
                    {
                        this.parameters.Items.RemoveAt(i);
                    }
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
            this.UpdateADInfo.Click += new EventHandler(this.UpdateADInfo_Click);
            this.DeleteADInfo.Click += new EventHandler(this.DeleteADInfo_Click);
            this.title.AddAttributes("maxlength", "40");
            this.title.AddAttributes("size", "40");
            this.type.Items.Clear();
            this.type.Items.Add(new ListItem("请选择     ", "-1"));
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
            if (String.IsNullOrEmpty(Request["advid"]))
            {
                base.Response.Redirect("advertisementsgrid.aspx");
                return;
            }
            this.LoadAnnounceInf(DNTRequest.GetInt("advid", -1));
        }
    }
}