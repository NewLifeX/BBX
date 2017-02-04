using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using BBX.Common;

namespace BBX.Control
{
    [DefaultProperty("Text"), Designer("System.Web.UI.Design.WebControls.PreviewControlDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"), ToolboxData("<{0}:TextBox runat=server></{0}:TextBox>")]
    public class TextBox : System.Web.UI.WebControls.TextBox
    {
        protected RequiredFieldValidator CanBeNullRFV = new RequiredFieldValidator();
        protected RegularExpressionValidator RequiredFieldTypeREV = new RegularExpressionValidator();
        protected RangeValidator NumberRV = new RangeValidator();
        
        [Bindable(true), Category("Appearance"), DefaultValue("")]
        public string SetFocusButtonID
        {
            get
            {
                object obj = this.ViewState[this.ClientID + "_SetFocusButtonID"];
                if (obj != null)
                {
                    return obj.ToString();
                }
                return "";
            }
            set
            {
                this.ViewState[this.ClientID + "_SetFocusButtonID"] = value;
                if (value != "")
                {
                    base.Attributes.Add("onkeydown", "if(event.keyCode==13){document.getElementById('" + value + "').focus();}");
                }
            }
        }

        [Bindable(true), Category("Appearance"), DefaultValue("")]
        public override int MaxLength
        {
            get
            {
                object obj = this.ViewState["TextBox_MaxLength"];
                if (obj != null)
                {
                    int result = obj.ToString().ToInt(4);
                    this.AddAttributes("maxlength", result.ToString());
                    return result;
                }
                return -1;
            }
            set
            {
                this.ViewState["TextBox_MaxLength"] = value;
                this.AddAttributes("maxlength", value.ToString());
            }
        }

        [Bindable(false), Category("Behavior"), DefaultValue(TextBoxMode.SingleLine), Description("要滚动的对象。")]
        public override TextBoxMode TextMode
        {
            get { return base.TextMode; }
            set
            {
                if (value == TextBoxMode.MultiLine)
                {
                    base.Attributes.Add("onkeyup", "return isMaxLen(this)");
                }
                base.TextMode = value;
            }
        }

        [Bindable(false), Category("Behavior"), DefaultValue(""), Description("要滚动的对象。"), TypeConverter(typeof(RequiredFieldTypeControlsConverter))]
        public string RequiredFieldType
        {
            get
            {
                object obj = this.ViewState["RequiredFieldType"];
                if (obj != null)
                {
                    return obj.ToString();
                }
                return "";
            }
            set { ViewState["RequiredFieldType"] = value; }
        }

        [Bindable(false), Category("Behavior"), DefaultValue("可为空"), Description("要滚动的对象。"), TypeConverter(typeof(CanBeNullControlsConverter))]
        public string CanBeNull
        {
            get
            {
                object obj = this.ViewState["CanBeNull"];
                if (obj != null)
                {
                    return obj.ToString();
                }
                return "";
            }
            set { ViewState["CanBeNull"] = value; }
        }

        [Bindable(true), Category("Appearance"), DefaultValue("")]
        public bool IsReplaceInvertedComma
        {
            get
            {
                object obj = this.ViewState["IsReplaceInvertedComma"];
                return obj == null || string.IsNullOrEmpty(obj.ToString().Trim()) || obj.ToString().ToLower() == "true";
            }
            set { ViewState["IsReplaceInvertedComma"] = value; }
        }

        [Bindable(true), Category("Appearance"), DefaultValue("")]
        public string ValidationExpression
        {
            get
            {
                object obj = this.ViewState["ValidationExpression"];
                if (obj == null || string.IsNullOrEmpty(obj.ToString().Trim()))
                {
                    return null;
                }
                return obj.ToString().ToLower();
            }
            set { ViewState["ValidationExpression"] = value; }
        }

        [Bindable(true), Category("Appearance"), DefaultValue("")]
        public override string Text
        {
            get
            {
                if (this.RequiredFieldType == "日期")
                {
                    try
                    {
                        string result = base.Text.ToDateTime().ToString("yyyy-MM-dd");
                        return result;
                    }
                    catch
                    {
                        string result = "1900-1-1";
                        return result;
                    }
                }
                if (this.RequiredFieldType == "日期时间")
                {
                    try
                    {
                        string result = base.Text.ToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
                        return result;
                    }
                    catch
                    {
                        string result = "1900-1-1 00:00:00";
                        return result;
                    }
                }
                if (!this.IsReplaceInvertedComma)
                {
                    return base.Text;
                }
                return base.Text.Replace("'", "''").Trim();
            }
            set
            {
                if (this.RequiredFieldType.IndexOf("日期") >= 0)
                {
                    try
                    {
                        base.Text = value.ToDateTime().ToString("yyyy-MM-dd");
                    }
                    catch
                    {
                        base.Text = "";
                    }
                }
                if (this.RequiredFieldType.IndexOf("日期时间") >= 0)
                {
                    try
                    {
                        base.Text = value.ToDateTime().ToString("yyyy-MM-dd HH:mm:ss");
                        return;
                    }
                    catch
                    {
                        base.Text = "";
                        return;
                    }
                }
                base.Text = value;
            }
        }

        [Bindable(true), Category("Appearance"), DefaultValue(30)]
        public int Cols { get { return base.Columns; } set { base.Columns = value; } }

        private int _size = 30;
        [Bindable(true), Category("Appearance"), DefaultValue(30)]
        public int Size { get { return _size; } set { _size = value; } }

        private string _maximumValue;
        [Bindable(true), Category("Appearance"), DefaultValue(null)]
        public string MaximumValue { get { return _maximumValue; } set { _maximumValue = value; } }

        private string _minimumValue;
        [Bindable(true), Category("Appearance"), DefaultValue(null)]
        public string MinimumValue { get { return _minimumValue; } set { _minimumValue = value; } }

        private string _hintTitle = "";
        [Bindable(true), Category("Appearance"), DefaultValue("")]
        public string HintTitle { get { return _hintTitle; } set { _hintTitle = value; } }

        private string _hintInfo = "";
        [Bindable(true), Category("Appearance"), DefaultValue("")]
        public string HintInfo { get { return _hintInfo; } set { _hintInfo = value; } }

        private int _hintLeftOffSet;
        [Bindable(true), Category("Appearance"), DefaultValue(0)]
        public int HintLeftOffSet { get { return _hintLeftOffSet; } set { _hintLeftOffSet = value; } }

        private int _hintTopOffSet;
        [Bindable(true), Category("Appearance"), DefaultValue(0)]
        public int HintTopOffSet { get { return _hintTopOffSet; } set { _hintTopOffSet = value; } }

        private string _hintShowType = "up";
        [Bindable(true), Category("Appearance"), DefaultValue("up")]
        public string HintShowType { get { return _hintShowType; } set { _hintShowType = value; } }

        private int _hintHeight = 50;
        [Bindable(true), Category("Appearance"), DefaultValue(130)]
        public int HintHeight { get { return _hintHeight; } set { _hintHeight = value; } }

        public TextBox()
        {
            base.Attributes.Add("onfocus", "this.className='txt_focus';");
            base.Attributes.Add("onblur", "this.className='txt';");
            base.CssClass = "txt";
        }

        public void AddAttributes(string key, string valuestr)
        {
            base.Attributes.Add(key, valuestr);
        }

        protected override void CreateChildControls()
        {
            if (this.MaximumValue != null || this.MinimumValue != null)
            {
                this.NumberRV.ControlToValidate = this.ID;
                this.NumberRV.Type = ValidationDataType.Double;
                if (this.MaximumValue != null && this.MinimumValue != null)
                {
                    this.NumberRV.MaximumValue = this.MaximumValue;
                    this.NumberRV.MinimumValue = this.MinimumValue;
                    this.NumberRV.ErrorMessage = "当前输入数据应在" + this.MinimumValue + "和" + this.MaximumValue + "之间!";
                }
                else
                {
                    if (this.MaximumValue != null)
                    {
                        this.NumberRV.MaximumValue = this.MaximumValue;
                        this.NumberRV.MinimumValue = Int32.MinValue.ToString();
                        this.NumberRV.ErrorMessage = "当前输入数据允许最大值为" + this.MaximumValue;
                    }
                    if (this.MinimumValue != null)
                    {
                        this.NumberRV.MinimumValue = this.MinimumValue;
                        this.NumberRV.MaximumValue = Int32.MaxValue.ToString();
                        this.NumberRV.ErrorMessage = "当前输入数据允许最小值为" + this.MinimumValue;
                    }
                }
                this.NumberRV.Display = ValidatorDisplay.Static;
                this.Controls.AddAt(0, this.NumberRV);
            }
            if (this.RequiredFieldType != null && this.RequiredFieldType != "" && this.RequiredFieldType != "暂无校验")
            {
                this.RequiredFieldTypeREV.Display = ValidatorDisplay.Dynamic;
                this.RequiredFieldTypeREV.ControlToValidate = this.ID;
                string requiredFieldType;
                switch (requiredFieldType = this.RequiredFieldType)
                {
                    case "数据校验":
                        this.RequiredFieldTypeREV.ValidationExpression = ((this.ValidationExpression != null) ? this.ValidationExpression : "^[-]?\\d+[.]?\\d*$");
                        this.RequiredFieldTypeREV.ErrorMessage = "数字的格式不正确";
                        break;
                    case "电子邮箱":
                        this.RequiredFieldTypeREV.ValidationExpression = ((this.ValidationExpression != null) ? this.ValidationExpression : "^([\\w-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([\\w-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$");
                        this.RequiredFieldTypeREV.ErrorMessage = "邮箱的格式不正确";
                        break;
                    case "移动手机":
                        this.RequiredFieldTypeREV.ValidationExpression = ((this.ValidationExpression != null) ? this.ValidationExpression : "\\d{11}");
                        this.RequiredFieldTypeREV.ErrorMessage = "手机的位数应为11位!";
                        break;
                    case "家用电话":
                        this.RequiredFieldTypeREV.ValidationExpression = ((this.ValidationExpression != null) ? this.ValidationExpression : "((\\(\\d{3}\\) ?)|(\\d{3}-))?\\d{3}-\\d{4}|((\\(\\d{3}\\) ?)|(\\d{4}-))?\\d{4}-\\d{4}");
                        this.RequiredFieldTypeREV.ErrorMessage = "请依 (XXX)XXX-XXXX 格式或 (XXX)XXXX-XXXX 输入电话号码！";
                        break;
                    case "身份证号码":
                        this.RequiredFieldTypeREV.ValidationExpression = ((this.ValidationExpression != null) ? this.ValidationExpression : "^\\d{15}$|^\\d{18}$");
                        this.RequiredFieldTypeREV.ErrorMessage = "请依15或18位数据的身份证号！";
                        break;
                    case "网页地址":
                        this.RequiredFieldTypeREV.ValidationExpression = ((this.ValidationExpression != null) ? this.ValidationExpression : "^(http|https)\\://([a-zA-Z0-9\\.\\-]+(\\:[a-zA-Z0-9\\.&%\\$\\-]+)*@)*((25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])|localhost|([a-zA-Z0-9\\-]+\\.)*[a-zA-Z0-9\\-]+\\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{1,10}))(\\:[0-9]+)*(/($|[a-zA-Z0-9\\.\\,\\?\\'\\\\\\+&%\\$#\\=~_\\-]+))*$");
                        this.RequiredFieldTypeREV.ErrorMessage = "请输入正确的网址";
                        break;
                    case "日期":
                        this.RequiredFieldTypeREV.ValidationExpression = ((this.ValidationExpression != null) ? this.ValidationExpression : "^((((1[6-9]|[2-9]\\d)\\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\\d|3[01]))|(((1[6-9]|[2-9]\\d)\\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\\d|30))|(((1[6-9]|[2-9]\\d)\\d{2})-0?2-(0?[1-9]|1\\d|2[0-9]))|(((1[6-9]|[2-9]\\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-))$");
                        this.RequiredFieldTypeREV.ErrorMessage = "请输入正确的日期,如:2006-1-1";
                        break;
                    case "日期时间":
                        this.RequiredFieldTypeREV.ValidationExpression = ((this.ValidationExpression != null) ? this.ValidationExpression : "^((((1[6-9]|[2-9]\\d)\\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\\d|3[01]))|(((1[6-9]|[2-9]\\d)\\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\\d|30))|(((1[6-9]|[2-9]\\d)\\d{2})-0?2-(0?[1-9]|1\\d|2[0-9]))|(((1[6-9]|[2-9]\\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-)) (20|21|22|23|[0-1]?\\d):[0-5]?\\d:[0-5]?\\d$");
                        this.RequiredFieldTypeREV.ErrorMessage = "请输入正确的日期,如: 2006-1-1 23:59:59";
                        break;
                    case "金额":
                        this.RequiredFieldTypeREV.ValidationExpression = ((this.ValidationExpression != null) ? this.ValidationExpression : "^([0-9]|[0-9].[0-9]{0-2}|[1-9][0-9]*.[0-9]{0,2})$");
                        this.RequiredFieldTypeREV.ErrorMessage = "请输入正确的金额";
                        break;
                    case "IP地址":
                        this.RequiredFieldTypeREV.ValidationExpression = ((this.ValidationExpression != null) ? this.ValidationExpression : "^(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])$");
                        this.RequiredFieldTypeREV.ErrorMessage = "请输入正确的IP地址";
                        break;
                    case "IP地址带端口":
                        this.RequiredFieldTypeREV.ValidationExpression = ((this.ValidationExpression != null) ? this.ValidationExpression : "^(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9]):\\d{1,5}?$");
                        this.RequiredFieldTypeREV.ErrorMessage = "请输入正确的带端口的IP地址";
                        break;
                }
                this.Controls.AddAt(0, this.RequiredFieldTypeREV);
            }
            string canBeNull;
            if ((canBeNull = this.CanBeNull) != null && !(canBeNull == "可为空"))
            {
                if (!(canBeNull == "必填"))
                {
                    return;
                }
                this.CanBeNullRFV.Display = ValidatorDisplay.Dynamic;
                this.CanBeNullRFV.ControlToValidate = this.ID;
                this.CanBeNullRFV.ErrorMessage = "<font color=red>请务必输入内容!</font>";
                this.Controls.AddAt(0, this.CanBeNullRFV);
            }
        }

        protected override void Render(HtmlTextWriter output)
        {
            if (this.TextMode == TextBoxMode.MultiLine)
            {
                output.WriteLine("<script type=\"text/javascript\">");
                output.WriteLine("function isMaxLen(o){");
                output.WriteLine("var nMaxLen=o.getAttribute? parseInt(o.getAttribute(\"maxlength\")):\"\";");
                output.WriteLine(" if(o.getAttribute && o.value.length>nMaxLen){");
                output.WriteLine(" o.value=o.value.substring(0,nMaxLen)");
                output.WriteLine("}}</script>");
                this.AddAttributes("rows", this.Rows.ToString());
                this.AddAttributes("cols", this.Cols.ToString());
                base.Attributes.Add("onfocus", "this.className='FormFocus';");
                base.Attributes.Add("onblur", "this.className='FormBase';");
                base.Attributes.Add("class", "FormBase");
            }
            else
            {
                if (this.TextMode == TextBoxMode.Password)
                {
                    this.AddAttributes("value", this.Text);
                }
                else
                {
                    if (this.Size > 0)
                    {
                        this.AddAttributes("size", this.Size.ToString());
                    }
                }
            }
            if (this.HintInfo != "")
            {
                this.AddAttributes("onmouseover", "showhintinfo(this," + this.HintLeftOffSet + "," + this.HintTopOffSet + ",'" + this.HintTitle + "','" + this.HintInfo + "','" + this.HintHeight + "','" + this.HintShowType + "')");
                this.AddAttributes("onmouseout", "hidehintinfo()");
            }
            base.Render(output);
            this.RenderChildren(output);
        }
    }
}