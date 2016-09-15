using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace BBX.Control
{
    [DefaultProperty("ScriptPath"), ToolboxData("<{0}:Calendar runat=server></{0}:Calendar>")]
    public class Calendar : WebControl, IPostBackDataHandler, INamingContainer
    {
        protected TextBox DateTextBox = new TextBox();
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
                return "../images/btn_calendar.gif";
            }
            set
            {
                base.ViewState["imageurl"] = value;
                this.ImgHtmlImage.Src = value;
            }
        }

        [DefaultValue(""), Description("当前选择的日期。")]
        public DateTime SelectedDate
        {
            get
            {
                DateTime result;
                try
                {
                    result = this.DateTextBox.Text.ToDateTime();
                }
                catch
                {
                    result = Convert.ToDateTime("1900-1-1");
                }
                return result;
            }
            set
            {
                try
                {
                    this.DateTextBox.Text = value.ToString("yyyy-MM-dd");
                }
                catch
                {
                    this.DateTextBox.Text = "";
                }
            }
        }

        private bool readOnly;
        [DefaultValue(true), Description("是否是只读。")]
        public bool ReadOnly { get { return readOnly; } set { readOnly = value; } }

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
                return "../js/calendar.js";
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

        protected override void CreateChildControls()
        {
            if (this.ReadOnly)
            {
                this.DateTextBox.Attributes.Add("readonly", "readonly");
            }
            this.DateTextBox.Size = 8;
            this.DateTextBox.ID = this.ID;
            this.Controls.Add(this.DateTextBox);
            this.ImgHtmlImage.Src = this.ImageUrl;
            this.ImgHtmlImage.Align = "bottom";
            this.ImgHtmlImage.Attributes.Add("onclick", "showcalendar(event, $('" + this.ID + "_" + this.ID + "'))");
            this.ImgHtmlImage.Attributes.Add("class", "calendarimg");
            this.Controls.Add(this.ImgHtmlImage);
            RegularExpressionValidator regularExpressionValidator = new RegularExpressionValidator();
            regularExpressionValidator.ID = regularExpressionValidator.ClientID;
            regularExpressionValidator.Display = ValidatorDisplay.Dynamic;
            regularExpressionValidator.ControlToValidate = this.DateTextBox.ID;
            regularExpressionValidator.ValidationExpression = "^((((1[6-9]|[2-9]\\d)\\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\\d|3[01]))|(((1[6-9]|[2-9]\\d)\\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\\d|30))|(((1[6-9]|[2-9]\\d)\\d{2})-0?2-(0?[1-9]|1\\d|2[0-9]))|(((1[6-9]|[2-9]\\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-))$";
            regularExpressionValidator.ErrorMessage = "请输入正确的日期,如:2006-1-1";
            this.Controls.Add(regularExpressionValidator);
            base.CreateChildControls();
        }

        public void AddAttributes(string key, string valuestr)
        {
            this.DateTextBox.Attributes.Add(key, valuestr);
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (!this.Page.ClientScript.IsClientScriptBlockRegistered("CalendarSet"))
            {
                this.Page.ClientScript.RegisterClientScriptBlock(base.GetType(), "CalendarSet", string.Format("<SCRIPT language='javascript' src='{0}'></SCRIPT>", this.ScriptPath));
            }
            base.OnPreRender(e);
        }

        public void RaisePostDataChangedEvent()
        {
        }

        public bool LoadPostData(string postDataKey, NameValueCollection postCollection)
        {
            string text = this.DateTextBox.Text;
            string text2 = postCollection[postDataKey];
            if (!text.Equals(text2))
            {
                this.DateTextBox.Text = text2;
                return true;
            }
            return false;
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
        }
    }
}