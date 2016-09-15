using System;
using BBX.Config;
using BBX.Entity;

namespace BBX.Web.Admin
{
    public partial class setting : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                GeneralConfigInfo config = GeneralConfigInfo.Current;
                this.forumtitle.Text = config.Forumtitle.ToString();
                this.forumurl.Text = config.Forumurl.ToString();
                this.webtitle.Text = config.Webtitle.ToString();
                this.weburl.Text = config.Weburl.ToString().ToLower();
                this.SetOption(config);
            }
        }

        public void SetOption(GeneralConfigInfo configInfo)
        {
            if (configInfo.Maxonlines == 500)
            {
                this.size.SelectedValue = "1";
            }
            if (configInfo.Maxonlines == 5000)
            {
                this.size.SelectedValue = "2";
            }
            if (configInfo.Maxonlines == 50000)
            {
                this.size.SelectedValue = "3";
            }
            if (configInfo.Regctrl == 0)
            {
                this.safe.SelectedValue = "1";
            }
            if (configInfo.Regctrl == 12)
            {
                this.safe.SelectedValue = "2";
            }
            if (configInfo.Regctrl == 48)
            {
                this.safe.SelectedValue = "3";
            }
            if (configInfo.Visitedforums == 0)
            {
                this.func.SelectedValue = "1";
            }
            if (configInfo.Visitedforums == 10)
            {
                this.func.SelectedValue = "2";
            }
            if (configInfo.Visitedforums == 20)
            {
                this.func.SelectedValue = "3";
            }
        }

        private void submitsetting_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                GeneralConfigInfo config = GeneralConfigInfo.Current;
                string selectedValue;
                if ((selectedValue = this.size.SelectedValue) != null)
                {
                    if (!(selectedValue == "1"))
                    {
                        if (!(selectedValue == "2"))
                        {
                            if (selectedValue == "3")
                            {
                                config.Attachsave = 2;
                                config.Fullmytopics = 1;
                                config.Maxonlines = 50000;
                                config.Starthreshold = 2;
                                config.Searchctrl = 60;
                                config.Hottopic = 100;
                                config.Maxmodworksmonths = 1;
                                config.Moddisplay = 1;
                                config.Tpp = 15;
                                config.Ppp = 10;
                                config.Maxpolloptions = 20000;
                                config.Maxfavorites = 100;
                                config.Nocacheheaders = 0;
                                config.Guestcachepagetimeout = 20;
                                config.Topiccachemark = 50;
                                config.Postinterval = 15;
                                config.Maxspm = 3;
                                config.Fulltextsearch = 0;
                                //config.TopicQueueStats = 1;
                                //config.TopicQueueStatsCount = 100;
                            }
                        }
                        else
                        {
                            config.Attachsave = 1;
                            config.Fullmytopics = 1;
                            config.Maxonlines = 5000;
                            config.Starthreshold = 2;
                            config.Searchctrl = 30;
                            config.Hottopic = 20;
                            config.Losslessdel = 200;
                            config.Maxmodworksmonths = 3;
                            config.Moddisplay = 0;
                            config.Tpp = 20;
                            config.Ppp = 15;
                            config.Maxpolloptions = 1000;
                            config.Maxpostsize = 10000;
                            config.Maxfavorites = 200;
                            config.Nocacheheaders = 0;
                            config.Guestcachepagetimeout = 10;
                            config.Topiccachemark = 20;
                            config.Postinterval = 10;
                            config.Maxspm = 4;
                            config.Fulltextsearch = 0;
                            //config.TopicQueueStats = 1;
                            //config.TopicQueueStatsCount = 30;
                        }
                    }
                    else
                    {
                        config.Attachsave = 0;
                        config.Fullmytopics = 0;
                        config.Maxonlines = 500;
                        config.Starthreshold = 2;
                        config.Searchctrl = 10;
                        config.Hottopic = 10;
                        config.Losslessdel = 365;
                        config.Maxmodworksmonths = 5;
                        config.Moddisplay = 0;
                        config.Tpp = 30;
                        config.Ppp = 20;
                        config.Maxpolloptions = 10;
                        config.Maxpostsize = 10000;
                        config.Maxfavorites = 500;
                        config.Nocacheheaders = 1;
                        config.Guestcachepagetimeout = 0;
                        config.Topiccachemark = 0;
                        config.Postinterval = 5;
                        config.Maxspm = 5;
                        config.Fulltextsearch = 0;
                        //config.TopicQueueStats = 0;
                        //config.TopicQueueStatsCount = 20;
                    }
                }
                string selectedValue2;
                if ((selectedValue2 = this.safe.SelectedValue) != null)
                {
                    if (!(selectedValue2 == "1"))
                    {
                        if (!(selectedValue2 == "2"))
                        {
                            if (selectedValue2 == "3")
                            {
                                config.Attachrefcheck = 1;
                                config.Doublee = 0;
                                config.Dupkarmarate = 0;
                                config.Hideprivate = 1;
                                config.Memliststatus = 0;
                                config.Seccodestatus = "login.aspx";
                                config.Rules = 1;
                                config.Edittimelimit = 10;
                                config.Karmaratelimit = 4;
                                config.Newbiespan = 4;
                                config.Regctrl = 48;
                                config.Regstatus = 1;
                                config.Regverify = 1;
                                config.Secques = 20;
                                config.Defaulteditormode = 1;
                                config.Allowswitcheditor = 1;
                                config.Watermarktype = 1;
                                config.Attachimgquality = 100;
                            }
                        }
                        else
                        {
                            config.Attachrefcheck = 1;
                            config.Doublee = 0;
                            config.Dupkarmarate = 0;
                            config.Hideprivate = 1;
                            config.Memliststatus = 1;
                            config.Seccodestatus = "login.aspx";
                            config.Rules = 1;
                            config.Edittimelimit = 20;
                            config.Karmaratelimit = 1;
                            config.Newbiespan = 1;
                            config.Regctrl = 12;
                            config.Regstatus = 1;
                            config.Regverify = 1;
                            config.Secques = 10;
                            config.Defaulteditormode = 0;
                            config.Allowswitcheditor = 1;
                            config.Watermarktype = 1;
                            config.Attachimgquality = 85;
                        }
                    }
                    else
                    {
                        config.Doublee = 1;
                        config.Dupkarmarate = 1;
                        config.Hideprivate = 0;
                        config.Memliststatus = 1;
                        config.Seccodestatus = "";
                        config.Rules = 0;
                        config.Edittimelimit = 0;
                        config.Karmaratelimit = 0;
                        config.Regctrl = 0;
                        config.Regstatus = 1;
                        config.Regverify = 0;
                        config.Secques = 5;
                        config.Defaulteditormode = 0;
                        config.Allowswitcheditor = 0;
                        config.Watermarktype = 0;
                        config.Attachimgquality = 80;
                    }
                }
                string selectedValue3;
                if ((selectedValue3 = this.func.SelectedValue) != null)
                {
                    if (!(selectedValue3 == "1"))
                    {
                        if (!(selectedValue3 == "2"))
                        {
                            if (selectedValue3 == "3")
                            {
                                config.Archiverstatus = 1;
                                config.Attachimgpost = 1;
                                config.Fastpost = 1;
                                config.Editedby = 1;
                                config.Forumjump = 1;
                                config.Modworkstatus = 1;
                                config.Rssstatus = 1;
                                config.Smileyinsert = 1;
                                config.Stylejump = 1;
                                config.Subforumsindex = 1;
                                config.Visitedforums = 20;
                                config.Welcomemsg = 1;
                                config.Watermarkstatus = 1;
                                config.Whosonlinestatus = 1;
                                config.Debug = 1;
                                config.Regadvance = 1;
                                config.Showsignatures = 1;
                            }
                        }
                        else
                        {
                            config.Archiverstatus = 1;
                            config.Attachimgpost = 1;
                            config.Fastpost = 1;
                            config.Editedby = 1;
                            config.Forumjump = 1;
                            config.Modworkstatus = 0;
                            config.Rssstatus = 1;
                            config.Smileyinsert = 1;
                            config.Stylejump = 0;
                            config.Subforumsindex = 0;
                            config.Visitedforums = 10;
                            config.Welcomemsg = 0;
                            config.Watermarkstatus = 0;
                            config.Whosonlinestatus = 1;
                            config.Debug = 1;
                            config.Regadvance = 0;
                            config.Showsignatures = 1;
                        }
                    }
                    else
                    {
                        config.Archiverstatus = 0;
                        config.Attachimgpost = 0;
                        config.Fastpost = 0;
                        config.Editedby = 0;
                        config.Forumjump = 0;
                        config.Modworkstatus = 0;
                        config.Rssstatus = 0;
                        config.Smileyinsert = 0;
                        config.Stylejump = 0;
                        config.Subforumsindex = 0;
                        config.Visitedforums = 0;
                        config.Welcomemsg = 0;
                        config.Watermarkstatus = 0;
                        config.Whosonlinestatus = 0;
                        config.Debug = 0;
                        config.Regadvance = 0;
                        config.Showsignatures = 0;
                    }
                }
                config.Forumtitle = this.forumtitle.Text.Trim();
                config.Forumurl = this.forumurl.Text.Trim().ToLower();
                config.Webtitle = this.webtitle.Text.Trim();
                config.Weburl = this.weburl.Text.Trim().ToLower();
                config.Save(); ;
                AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "快速设置", "");
                base.RegisterStartupScript("PAGE", "window.location.href='setting.aspx';");
            }
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.submitsetting.Click += new EventHandler(this.submitsetting_Click);
            this.forumtitle.IsReplaceInvertedComma = false;
            this.forumurl.IsReplaceInvertedComma = false;
            this.webtitle.IsReplaceInvertedComma = false;
            this.weburl.IsReplaceInvertedComma = false;
        }
    }
}