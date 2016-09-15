using System;
using System.ComponentModel;
using System.Text;
using System.Web.UI;

namespace BBX.Control
{
    [DefaultEvent("Click"), DefaultProperty("Text"), ToolboxData("<{0}:Button runat=server></{0}:Button>")]
    public class Button : WebControl, IPostBackEventHandler
    {
        public enum ButtonType
        {
            Normal,
            WithImage
        }

        protected static readonly object EventClick;
        private string onClientClick = "";
        
        public event EventHandler Click
        {
            add
            {
                base.Events.AddHandler(EventClick, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventClick, value);
            }
        }

        public string OnClientClick { get { return onClientClick; } set { onClientClick = (value.EndsWith(";") ? value : (value + ";")); } }

        public virtual string PostBackUrl
        {
            get
            {
                string text = (string)this.ViewState["PostBackUrl"];
                if (text != null)
                {
                    return text;
                }
                return string.Empty;
            }
            set { ViewState["PostBackUrl"] = value; }
        }

        private bool _autoPostBack = true;
        public bool AutoPostBack { get { return _autoPostBack; } set { _autoPostBack = value; } }

        private bool _showPostDiv = true;
        public bool ShowPostDiv { get { return _showPostDiv; } set { _showPostDiv = value; } }

        private bool _validateForm;
        public bool ValidateForm { get { return _validateForm; } set { _validateForm = value; } }

        public ButtonType ButtontypeMode
        {
            get
            {
                object obj = this.ViewState["ButtontypeMode"];
                if (obj != null)
                {
                    return (ButtonType)obj;
                }
                return ButtonType.WithImage;
            }
            set { ViewState["ButtontypeMode"] = value; }
        }

        [Bindable(true), Category("Appearance"), DefaultValue(" 提 交 ")]
        public string Text
        {
            get
            {
                object obj = this.ViewState["ButtonText"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return " 提 交 ";
            }
            set { ViewState["ButtonText"] = value; }
        }

        [DefaultValue("../images/submit.gif"), Description("图版按钮链接")]
        public string ButtonImgUrl
        {
            get
            {
                object obj = this.ViewState["ButtonImgUrl"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "../images/submit.gif";
            }
            set { ViewState["ButtonImgUrl"] = value; }
        }

        [DefaultValue("../images/"), Description("图版按钮链接")]
        public string XpBGImgFilePath
        {
            get
            {
                object obj = this.ViewState["XpBGImgFilePath"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "../images/";
            }
            set { ViewState["XpBGImgFilePath"] = value; }
        }

        [DefaultValue("../images/"), Description("图版按钮链接")]
        public string ScriptContent
        {
            get
            {
                object obj = this.ViewState["ScriptContent"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "";
            }
            set { ViewState["ScriptContent"] = value; }
        }

        static Button()
        {
            EventClick = new object();
            EventClick = new object();
        }

        public Button()
        {
            this.ButtontypeMode = ButtonType.WithImage;
        }

        protected virtual void OnClick(EventArgs e)
        {
            EventHandler eventHandler = (EventHandler)base.Events[EventClick];
            if (eventHandler != null)
            {
                eventHandler(this, e);
            }
        }

        public void RaisePostBackEvent(string eventArgument)
        {
            this.OnClick(new EventArgs());
        }

        void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
        {
            this.RaisePostBackEvent(eventArgument);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }

        protected override void LoadViewState(object savedState)
        {
            if (savedState != null)
            {
                base.LoadViewState(savedState);
                string text = (string)this.ViewState["Text"];
                if (text != null)
                {
                    this.Text = text;
                }
            }
        }

        protected override void AddParsedSubObject(object obj)
        {
            if (this.HasControls())
            {
                base.AddParsedSubObject(obj);
                return;
            }
            if (obj is LiteralControl)
            {
                this.Text = ((LiteralControl)obj).Text;
                return;
            }
            string text = this.Text;
            if (text.Length != 0)
            {
                this.Text = string.Empty;
                base.AddParsedSubObject(new LiteralControl(text));
            }
            base.AddParsedSubObject(obj);
        }

        protected override void Render(HtmlTextWriter output)
        {
            if (base.HintInfo != "")
            {
                output.WriteBeginTag("span id=\"" + this.ClientID + "\"  onmouseover=\"showhintinfo(this," + base.HintLeftOffSet + "," + base.HintTopOffSet + ",'" + base.HintTitle + "','" + base.HintInfo + "','" + base.HintHeight + "','" + base.HintShowType + "');\" onmouseout=\"hidehintinfo();\">");
            }
            string text = "";
            if (!this.Enabled)
            {
                text = " disabled=\"true\"";
            }
            if (this.AutoPostBack)
            {
                StringBuilder stringBuilder = new StringBuilder();
                if (!string.IsNullOrEmpty(this.OnClientClick))
                {
                    stringBuilder.Append(this.OnClientClick);
                }
                stringBuilder.Append("if (typeof(Page_ClientValidate) == 'function') { if (Page_ClientValidate() == false) { return false; }}");
                stringBuilder.Append("this.disabled=true;");
                if (this.ValidateForm)
                {
                    stringBuilder.Append("if(validate(this.form)){");
                    if (this.ShowPostDiv)
                    {
                        stringBuilder.Append("document.getElementById('success').style.display = 'block';HideOverSels('success');");
                    }
                    stringBuilder.Append(this.Page.ClientScript.GetPostBackEventReference(this, "") + ";}else{this.disabled=false;}");
                }
                else
                {
                    stringBuilder.Append(this.Page.ClientScript.GetPostBackEventReference(this, "") + ";");
                }
                if (this.ScriptContent != "")
                {
                    stringBuilder.Append(this.ScriptContent);
                }
                if (this.ButtontypeMode == ButtonType.Normal)
                {
                    output.Write("<span><button type=\"button\" class=\"ManagerButton\" id=\"" + this.UniqueID + "\"" + text + " onclick=\"" + stringBuilder.ToString() + "\">" + this.Text + "</button></span>");
                }
                if (this.ButtontypeMode == ButtonType.WithImage)
                {
                    output.Write("<span><button type=\"button\" class=\"ManagerButton\" id=\"" + this.UniqueID + "\"" + text + " onclick=\"" + stringBuilder.ToString() + "\"><img src=\"" + this.ButtonImgUrl + "\"/>" + this.Text + "</button></span>");
                }
            }
            else
            {
                if (this.ButtontypeMode == ButtonType.Normal)
                {
                    output.Write("<span><button type=\"button\" class=\"ManagerButton\" id=\"" + this.UniqueID + "\"" + text + " onclick=\"" + this.OnClientClick + this.ScriptContent + "\">" + this.Text + "</button></span>");
                }
                if (this.ButtontypeMode == ButtonType.WithImage)
                {
                    output.Write("<span><button type=\"button\" class=\"ManagerButton\" id=\"" + this.UniqueID + "\"" + text + " onclick=\"" + this.OnClientClick + this.ScriptContent + "\"><img src=\"" + this.ButtonImgUrl + "\"/>" + this.Text + "</button></span>");
                }
            }
            if (base.HintInfo != "")
            {
                output.WriteEndTag("span");
            }
        }
    }
}