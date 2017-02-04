using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BBX.Common;
using BBX.Config;
using BBX.Control;
using BBX.Entity;

namespace BBX.Web.Admin
{
    public class attach : AdminPage
    {
        protected HtmlForm Form1;
        protected BBX.Control.RadioButtonList attachsave;
        protected BBX.Control.RadioButtonList showattachmentpath;
        protected BBX.Control.TextBox attachimgmaxheight;
        protected BBX.Control.TextBox attachimgquality;
        protected BBX.Control.TextBox attachimgmaxwidth;
        protected BBX.Control.RadioButtonList attachrefcheck;
        protected BBX.Control.RadioButtonList watermarktype;
        protected Literal position;
        protected BBX.Control.TextBox watermarktext;
        protected BBX.Control.DropDownList watermarkfontname;
        protected BBX.Control.TextBox watermarkpic;
        protected BBX.Control.TextBox watermarkfontsize;
        protected BBX.Control.TextBox watermarktransparency;
        protected Hint Hint1;
        protected BBX.Control.Button SaveInfo;

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
            this.attachrefcheck.SelectedValue = config.Attachrefcheck.ToString();
            this.attachsave.SelectedValue = config.Attachsave.ToString();
            this.watermarktype.SelectedValue = config.Watermarktype.ToString();
            this.showattachmentpath.SelectedValue = config.Showattachmentpath.ToString();
            this.attachimgmaxheight.Text = config.Attachimgmaxheight.ToString();
            this.attachimgmaxwidth.Text = config.Attachimgmaxwidth.ToString();
            this.attachimgquality.Text = config.Attachimgquality.ToString();
            this.watermarkfontsize.Text = config.Watermarkfontsize.ToString();
            this.watermarktext.Text = config.Watermarktext.ToString();
            this.watermarkpic.Text = config.Watermarkpic.ToString();
            this.watermarktransparency.Text = config.Watermarktransparency.ToString();
            this.LoadPosition(config.Watermarkstatus);
            this.LoadSystemFont();
            try
            {
                this.watermarkfontname.SelectedValue = config.Watermarkfontname.ToString();
            }
            catch
            {
                this.watermarkfontname.SelectedIndex = 0;
            }
        }

        private void LoadSystemFont()
        {
            this.watermarkfontname.Items.Clear();
            InstalledFontCollection installedFontCollection = new InstalledFontCollection();
            FontFamily[] families = installedFontCollection.Families;
            for (int i = 0; i < families.Length; i++)
            {
                FontFamily fontFamily = families[i];
                this.watermarkfontname.Items.Add(new ListItem(fontFamily.Name, fontFamily.Name));
            }
        }

        public void LoadPosition(int selectid)
        {
            this.position.Text = "<table width=\"256\" height=\"207\" border=\"0\" background=\"../images/flower.jpg\">";
            for (int i = 1; i < 10; i++)
            {
                if (i % 3 == 1)
                {
                    Literal expr_23 = this.position;
                    expr_23.Text += "<tr>";
                }
                Literal expr_3E = this.position;
                expr_3E.Text += ((selectid == i) ? "<td width=\"33%\" align=\"center\" style=\"vertical-align:middle;\"><input type=\"radio\" id=\"watermarkstatus\" name=\"watermarkstatus\" value=\"" + i + "\" checked><b>#" + i + "</b></td>" : "<td width=\"33%\" align=\"center\" style=\"vertical-align:middle;\"><input type=\"radio\" id=\"watermarkstatus\" name=\"watermarkstatus\" value=\"" + i + "\" ><b>#" + i + "</b></td>");
                if (i % 3 == 0)
                {
                    Literal expr_CD = this.position;
                    expr_CD.Text += "</tr>";
                }
            }
            Literal expr_F4 = this.position;
            expr_F4.Text += "</table><input type=\"radio\" id=\"watermarkstatus\" name=\"watermarkstatus\" value=\"0\" ";
            if (selectid == 0)
            {
                Literal expr_112 = this.position;
                expr_112.Text += " checked";
            }
            Literal expr_12D = this.position;
            expr_12D.Text += ">不启用水印功能";
        }

        private void SaveInfo_Click(object sender, EventArgs e)
        {
            if (base.CheckCookie())
            {
                foreach (DictionaryEntry item in new Hashtable
                {
                    {
                        "图片附件文字水印大小",
                        this.watermarkfontsize.Text
                    },

                    {
                        "JPG图片质量",
                        this.attachimgquality.Text
                    },

                    {
                        "图片最大高度",
                        this.attachimgmaxheight.Text
                    },

                    {
                        "图片最大宽度",
                        this.attachimgmaxwidth.Text
                    }
                })
                {
                    if (item.Value.ToInt(-1) < 0)
                    {
                        base.RegisterStartupScript("", this.GetMessageScript("输入错误," + item.Key + "只能是0或者正整数"));
                        return;
                    }
                }
                if (Convert.ToInt16(this.attachimgquality.Text) > 100 || Convert.ToInt16(this.attachimgquality.Text) < 0)
                {
                    base.RegisterStartupScript("", this.GetMessageScript("JPG图片质量只能在0-100之间"));
                }
                else
                {
                    if (Convert.ToInt16(this.watermarktransparency.Text) > 10 || Convert.ToInt16(this.watermarktransparency.Text) < 1)
                    {
                        base.RegisterStartupScript("", this.GetMessageScript("图片水印透明度取值范围1-10"));
                        return;
                    }
                    if (Convert.ToInt16(this.watermarkfontsize.Text) <= 0)
                    {
                        base.RegisterStartupScript("", this.GetMessageScript("图片附件添加文字水印的大小必须大于0"));
                        return;
                    }
                    if (Convert.ToInt16(this.attachimgmaxheight.Text) < 0)
                    {
                        base.RegisterStartupScript("", this.GetMessageScript("图片最大高度必须大于或等于0"));
                        return;
                    }
                    if (Convert.ToInt16(this.attachimgmaxwidth.Text) < 0)
                    {
                        base.RegisterStartupScript("", this.GetMessageScript("图片最大宽度必须大于或等于0"));
                        return;
                    }
                    this.SaveGeneralConfigInfo();
                    base.RegisterStartupScript("PAGE", "window.location.href='global_attach.aspx';");
                    return;
                }
                return;
            }
        }

        private void SaveGeneralConfigInfo()
        {
            GeneralConfigInfo config = GeneralConfigInfo.Current;
            config.Attachrefcheck = this.attachrefcheck.SelectedValue.ToInt();
            config.Attachsave = this.attachsave.SelectedValue.ToInt();
            config.Watermarkstatus = DNTRequest.GetInt("watermarkstatus", 0);
            config.Watermarktype = (int)Convert.ToInt16(this.watermarktype.SelectedValue);
            config.Showattachmentpath = this.showattachmentpath.SelectedValue.ToInt();
            config.Attachimgmaxheight = this.attachimgmaxheight.Text.ToInt();
            config.Attachimgmaxwidth = this.attachimgmaxwidth.Text.ToInt();
            config.Attachimgquality = this.attachimgquality.Text.ToInt();
            config.Watermarktext = this.watermarktext.Text;
            config.Watermarkpic = this.watermarkpic.Text;
            config.Watermarkfontname = this.watermarkfontname.SelectedValue;
            config.Watermarkfontsize = this.watermarkfontsize.Text.ToInt();
            config.Watermarktransparency = (int)Convert.ToInt16(this.watermarktransparency.Text);
            config.Save();

            //config.Save();;
            AdminVisitLog.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "附件设置", "");
        }

        private string GetMessageScript(string message)
        {
            return string.Format("<script>alert('{0}');window.location.href='global_attach.aspx';</script>", message);
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
            this.SaveInfo.Click += new EventHandler(this.SaveInfo_Click);
        }
    }
}