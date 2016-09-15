using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace BBX.Control
{
    [DefaultProperty("ScriptPath"), ToolboxData("<{0}:ColorPicker runat=server></{0}:ColorPicker>")]
    public class ColorPicker : WebControl, IPostBackDataHandler, INamingContainer
    {
        protected TextBox ColorTextBox = new TextBox();
        protected HtmlImage ImgHtmlImage = new HtmlImage();

        [Bindable(true), Category("Appearance"), DefaultValue("")]
        public string ImageUrl
        {
            get
            {
                if (base.ViewState["imageurl"] != null)
                {
                    return (string)base.ViewState["imageurl"];
                }
                return "../images/colorpicker.gif";
            }
            set
            {
                base.ViewState["imageurl"] = value;
                this.ImgHtmlImage.Src = value;
            }
        }

        [DefaultValue(""), Description("当前选择的颜色值")]
        public string Text { get { return ColorTextBox.Text; } set { ColorTextBox.Text = (value + "").Trim(); } }

        [DefaultValue(true), Description("是否是只读")]
        public bool ReadOnly
        {
            get { return Environment.Version.Major == 1 && this.ColorTextBox.ReadOnly; }
            set
            {
                if (Environment.Version.Major == 1)
                {
                    this.ColorTextBox.ReadOnly = value;
                }
            }
        }

        [DefaultValue("./"), Description("Javascript脚本文件所在目录。")]
        public string ScriptPath
        {
            get
            {
                object obj = this.ViewState["ScriptPath"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "../js/colorpicker.js";
            }
            set { ViewState["ScriptPath"] = value; }
        }

        [Category("Appearance"), DefaultValue(""), Description("边框的样式。"), TypeConverter(typeof(BorderStyleConverter))]
        public new string BorderStyle
        {
            get
            {
                object obj = this.ViewState["BorderStyle"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "solid";
            }
            set { ViewState["BorderStyle"] = value; }
        }

        [Category("Appearance"), DefaultValue(""), Description("边框的宽度。")]
        public new string BorderWidth
        {
            get
            {
                object obj = this.ViewState["BorderWidth"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "1";
            }
            set { ViewState["BorderWidth"] = value; }
        }

        [Category("Appearance"), DefaultValue(""), Description("边框的颜色。")]
        public override Color BorderColor
        {
            get
            {
                object obj = this.ViewState["BorderColor"];
                if (obj != null)
                {
                    return (Color)obj;
                }
                return Color.FromArgb(153, 153, 153);
            }
            set { ViewState["BorderColor"] = value; }
        }

        [Category("Appearance"), DefaultValue(""), Description("CSS文件路径。")]
        public string Css_Path
        {
            get
            {
                object obj = this.ViewState["ColorPickerCssPath"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "../styles/colorpicker.css";
            }
            set { ViewState["ColorPickerCssPath"] = value; }
        }

        [DefaultValue(0), Description("向上偏移量")]
        public float TopOffSet
        {
            get
            {
                object obj = this.ViewState["TopOffSet"];
                if (obj != null)
                {
                    return (float)obj;
                }
                return 0f;
            }
            set { ViewState["TopOffSet"] = value; }
        }

        [DefaultValue(0), Description("向左偏移量")]
        public float LeftOffSet
        {
            get
            {
                object obj = this.ViewState["LeftOffSet"];
                if (obj != null)
                {
                    return (float)obj;
                }
                return 0f;
            }
            set { ViewState["LeftOffSet"] = value; }
        }

        protected override void CreateChildControls()
        {
            this.ColorTextBox.Size = 8;
            this.ColorTextBox.ID = this.ID;
            this.Controls.Add(this.ColorTextBox);
            this.ImgHtmlImage.ID = "ColorPreview";
            this.ImgHtmlImage.Src = this.ImageUrl;
            this.ImgHtmlImage.Attributes.Add("onclick", "IsShowColorPanel('" + this.ColorTextBox.ClientID + "','" + this.ImgHtmlImage.ClientID + "'," + this.LeftOffSet + "," + this.TopOffSet + ")");
            this.ImgHtmlImage.Attributes.Add("class", "img");
            this.ImgHtmlImage.Attributes.Add("title", "选择颜色");
            this.Controls.Add(this.ImgHtmlImage);
            base.CreateChildControls();
        }

        public void AddAttributes(string key, string valuestr)
        {
            this.ColorTextBox.Attributes.Add(key, valuestr);
        }

        protected override void OnPreRender(EventArgs e)
        {
            string script = string.Format("<link href=\"{0}\" type=\"text/css\" rel=\"stylesheet\">\r\n<script language=\"javascript\" src=\"{1}\"></script>\r\n", this.Css_Path, this.ScriptPath);
            if (!this.Page.ClientScript.IsClientScriptBlockRegistered("ColorPickerSet"))
            {
                this.Page.ClientScript.RegisterClientScriptBlock(base.GetType(), "ColorPickerSet", script);
            }
            base.OnPreRender(e);
        }

        public void RaisePostDataChangedEvent()
        {
        }

        public bool LoadPostData(string postDataKey, NameValueCollection postCollection)
        {
            string text = this.ColorTextBox.Text;
            string text2 = postCollection[postDataKey];
            if (!text.Equals(text2))
            {
                this.ColorTextBox.Text = text2;
                return true;
            }
            return false;
        }

        public string ColorPickHtmlContent()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<span id=\"ColorPicker{0}\" style=\"display:none; position:absolute;z-index:500;\" onmouseout=\"HideColorPanel('{0}');\"  onmouseover=\"ShowColorPanel('{0}','{1}',{2},{3});\">");
            stringBuilder.Append("<div  style=\"display:block;cursor:crosshair;z-index:501\" class=\"article\" >");
            stringBuilder.Append("<table border=0 cellPadding=0 cellSpacing=10 onmouseover=\"ShowColorPanel('{0}','{1}',{2},{3});\">");
            stringBuilder.Append("<tbody>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<script language=\"javaScript\">");
            stringBuilder.Append("WriteColorPanel('{0}','{1}',{2},{3});");
            stringBuilder.Append("</script>");
            stringBuilder.Append("</tr></tbody></table>");
            stringBuilder.Append("<table style=\"font-size:12px;word-break:break-all;width:100%;border:0px\"  cellPadding=0 cellSpacing=10 onmouseover=\"ShowColorPanel('{0}','{1}',{2},{3});\">");
            stringBuilder.Append("<tbody>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align=middle rowSpan=2>选中色彩");
            stringBuilder.Append("<table border=1 cellPadding=0 cellSpacing=0 height=30 id=ShowColor{0} width=40 bgcolor=\"\">");
            stringBuilder.Append("<tbody>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td></td></tr></tbody></table></td>");
            stringBuilder.Append("<td rowSpan=2>基色: <SPAN id=RGB{0}></SPAN><br />亮度: <SPAN id=GRAY{0}>120</SPAN><br />代码: <INPUT id=SelColor{0} size=7 value=\"\" border=0></TD>");
            stringBuilder.Append("<td><input type=\"button\" onclick=\"javascript:ColorPickerOK('{0}','{1}');\" value=\"确定\"></TD></TR>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td><input type=\"button\" onclick=\"javascript:document.getElementById('{0}').value='';document.getElementById('{1}').style.background='#FFFFFF';HideColorPanel('{0}');\" value=\"取消\"></TD>");
            stringBuilder.Append("</tr></tbody></table>");
            stringBuilder.Append("</DIV>");
            stringBuilder.Append("<iframe id=\"pickcoloriframe{0}\" style=\"position:absolute;z-index:102;top:-1px;width:250px;scrolling:no;height:237px;\" frameborder=\"0\"></iframe>");
            stringBuilder.Append("</span>");
            stringBuilder.Append("<script language=javascript>\r\n");
            stringBuilder.Append("InitColorPicker('{1}','" + this.Text + "');\r\n");
            stringBuilder.Append("</script>\r\n");
            return string.Format(stringBuilder.ToString(), new object[]
			{
				this.ColorTextBox.ClientID,
				this.ImgHtmlImage.ClientID,
				this.LeftOffSet,
				this.TopOffSet
			});
        }

        protected override void Render(HtmlTextWriter output)
        {
            if (base.HintInfo != "")
            {
                output.WriteBeginTag("span id=\"" + this.ClientID + "\"  onmouseover=\"showhintinfo(this," + base.HintLeftOffSet + "," + base.HintTopOffSet + ",'" + base.HintTitle + "','" + base.HintInfo + "','" + base.HintHeight + "','" + base.HintShowType + "');\" onmouseout=\"hidehintinfo();\">");
            }
            this.RenderChildren(output);
            if (base.HintInfo != "")
            {
                output.WriteEndTag("span");
            }
            output.Write(this.ColorPickHtmlContent());
        }
    }
}