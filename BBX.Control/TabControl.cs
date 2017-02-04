using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BBX.Common;

namespace BBX.Control
{
    [DefaultEvent("SelectedIndexChanged"), Description("WebControl TabControl"), DesignTimeVisible(true), ParseChildren(true, "Items"), PersistChildren(false), ToolboxData("<{0}:TabControl runat=server></{0}:TabControl>")]
    public class TabControl : WebControl, IPostBackDataHandler, IPostBackEventHandler, INamingContainer
    {
        public enum HeightUnitEnum
        {
            percent,
            px
        }

        public enum WidthUnitEnum
        {
            percent,
            px
        }

        public enum SelectionModeEnum
        {
            Client,
            Server
        }

        private static readonly object TabSelectedIndexChangedEvent;
        private TabControl.HeightUnitEnum _HeightUnitMode;
        private TabControl.WidthUnitEnum _WidthUnitMode;
        private Unit _Height;
        private int _SelectedIndex;
        private TabControl.SelectionModeEnum _SelectionMode;
        private Unit _Width;
        private HtmlInputHidden SelectedTab;

        public event EventHandler TabSelectedIndexChanged
        {
            add
            {
                base.Events.AddHandler(TabControl.TabSelectedIndexChangedEvent, value);
            }
            remove
            {
                base.Events.RemoveHandler(TabControl.TabSelectedIndexChangedEvent, value);
            }
        }

        private TabPageCollection _Items;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), MergableProperty(false), PersistenceMode(PersistenceMode.InnerDefaultProperty)]
        public TabPageCollection Items { get { return _Items; } }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelectedIndex
        {
            get
            {
                if (this.Items.Count <= 0)
                {
                    return this._SelectedIndex = -1;
                }
                if (this._SelectedIndex == -1)
                {
                    for (int i = 0; i < this.Items.Count; i++)
                    {
                        if (this.Items[i].Visible && this.Items[i].UniqueID == this.SelectedTabPageID)
                        {
                            return this._SelectedIndex = i;
                        }
                    }
                    return this._SelectedIndex = 0;
                }
                if (this._SelectedIndex >= this.Items.Count)
                {
                    return this._SelectedIndex = 0;
                }
                return this._SelectedIndex;
            }
            set
            {
                if (value < -1 || value >= this.Items.Count)
                {
                    throw new ArgumentOutOfRangeException("选项页必须小于" + this.Items.Count.ToString());
                }
                this._SelectedIndex = value;
            }
        }

        protected string SelectedTabPageID
        {
            get
            {
                if (this.ViewState["SelectedTabPageID"] != null)
                {
                    return (string)this.ViewState["SelectedTabPageID"];
                }
                return string.Empty;
            }
            set { ViewState["SelectedTabPageID"] = value; }
        }

        [DefaultValue("./"), Description("Javascript脚本文件所在目录。")]
        public string TabScriptPath
        {
            get
            {
                object obj = this.ViewState["TabScriptPath"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "../js/tabstrip.js";
            }
            set { ViewState["TabScriptPath"] = value; }
        }

        [DefaultValue("./"), Description("css文件所在目录。")]
        public string TabCssPath
        {
            get
            {
                object obj = this.ViewState["TabCssPath"];
                if (obj != null)
                {
                    return (string)obj;
                }
                return "../styles/tab.css";
            }
            set { ViewState["TabCssPath"] = value; }
        }

        [DefaultValue(0), Description("顶部属性页标题距左边偏移量")]
        public int LeftOffSetX
        {
            get
            {
                object obj = this.ViewState["LeftOffSetX"];
                if (obj != null)
                {
                    return obj.ToString().ToInt(0);
                }
                return 0;
            }
            set { ViewState["LeftOffSetX"] = value; }
        }

        public TabControl.SelectionModeEnum SelectionMode { get { return _SelectionMode; } set { _SelectionMode = value; } }

        public TabControl.HeightUnitEnum HeightUnitMode { get { return _HeightUnitMode; } set { _HeightUnitMode = value; } }

        public TabControl.WidthUnitEnum WidthUnitMode { get { return _WidthUnitMode; } set { _WidthUnitMode = value; } }

        static TabControl()
        {
            TabControl.TabSelectedIndexChangedEvent = new object();
        }

        public TabControl()
        {
            this.SelectedTab = new HtmlInputHidden();
            this._SelectedIndex = -1;
            this.SelectedTab.Value = string.Empty;
            this._Width = Unit.Pixel(350);
            this._Height = Unit.Pixel(150);
            this._Items = new TabPageCollection(this);
            this._SelectionMode = TabControl.SelectionModeEnum.Client;
            this.Height = Unit.Pixel(100);
            this.Width = Unit.Pixel(100);
            this._HeightUnitMode = TabControl.HeightUnitEnum.percent;
            this._WidthUnitMode = TabControl.WidthUnitEnum.percent;
            this.LeftOffSetX = 0;
        }

        protected override void AddParsedSubObject(object parsedObj)
        {
            if (parsedObj is TabPage)
            {
                this.Items.Add((TabPage)parsedObj);
            }
        }

        protected override void CreateChildControls()
        {
            this.CreateControlCollection();
            this.SelectedTab.ID = this.UniqueID;
            for (int i = 0; i < this.Items.Count; i++)
            {
                this.Controls.Add(this.Items[i]);
            }
            base.ChildControlsCreated = true;
            base.CreateChildControls();
        }

        protected override void OnPreRender(EventArgs args)
        {
            base.OnPreRender(args);
            int selectedIndex = this.SelectedIndex;
            if (selectedIndex != -1)
            {
                this.Items[selectedIndex].Selected = true;
                this.SelectedTab.Value = this.Items[selectedIndex].UniqueID;
            }
            else
            {
                this.SelectedTab.Value = string.Empty;
            }
            string script = string.Format("<SCRIPT language=\"javascript\" src=\"{0}\"></SCRIPT>\r\n<LINK href=\"{1}\" type=\"text/css\" rel=\"stylesheet\">\r\n", this.TabScriptPath, this.TabCssPath);
            if (!this.Page.ClientScript.IsClientScriptBlockRegistered("TabWindow"))
            {
                this.Page.ClientScript.RegisterClientScriptBlock(base.GetType(), "TabWindow", script);
            }
            base.OnPreRender(args);
        }

        protected void OnTabSelectedIndexChanged(EventArgs e)
        {
            if (base.Events != null)
            {
                EventHandler eventHandler = (EventHandler)base.Events[TabControl.TabSelectedIndexChangedEvent];
                if (eventHandler != null)
                {
                    eventHandler(this, e);
                }
            }
        }

        protected override void Render(HtmlTextWriter pOutPut)
        {
            if (this.LeftOffSetX > 0)
            {
                pOutPut.Write("<div Class=\"tabs\" ID=\"" + this.UniqueID + "_Tab\" style=\"padding-left:" + this.LeftOffSetX + ";\">");
            }
            else
            {
                pOutPut.Write("<div Class=\"tabs\" ID=\"" + this.UniqueID + "_Tab\" >");
            }
            this.SelectedTab.RenderControl(pOutPut);
            pOutPut.Write("<ul>");
            this.RenderTabButton(pOutPut);
            pOutPut.Write("</ul></div><div id=\"" + this.UniqueID + "tabarea\" class=\"tabarea\">");
            this.RenderTabContent(pOutPut);
            pOutPut.Write("</div>");
        }

        internal void RenderDownLevelContent(HtmlTextWriter output)
        {
            this.Render(output);
        }

        private void RenderTabButton(HtmlTextWriter pOutPut)
        {
            if (this.SelectionMode == TabControl.SelectionModeEnum.Server)
            {
                for (int i = 0; i < this.Items.Count; i++)
                {
                    if (this.Items[i].Selected)
                    {
                        pOutPut.Write("<li class=\"CurrentTabSelect\" ><a href=\"#\" class=\"current\" onfocus=\"this.blur();\">" + this.Items[i].Caption + "</a></li>");
                    }
                    else
                    {
                        pOutPut.Write("<li class=\"TabSelect\" onmouseover=\"tabpage_mouseover(this)\" onmouseout=\"tabpage_mouseout(this)\" onClick=\"tabpage_selectonserver(this,'" + this.Items[i].UniqueID + "');" + this.Page.ClientScript.GetPostBackEventReference(this, "") + "\"><a href=\"#\" onfocus=\"this.blur();\">" + this.Items[i].Caption + "</a></li>");
                    }
                }
                return;
            }
            for (int j = 0; j < this.Items.Count; j++)
            {
                if (this.Items[j].Selected)
                {
                    pOutPut.Write("<li id=\"" + this.Items[j].UniqueID + "_li\" class=\"CurrentTabSelect\" onclick=\"tabpage_selectonclient(this,'" + this.Items[j].UniqueID + "');\"><a href=\"#\" class=\"current\" onfocus=\"this.blur();\">" + this.Items[j].Caption + "</a></li>");
                }
                else
                {
                    pOutPut.Write("<li id=\"" + this.Items[j].UniqueID + "_li\" class=\"TabSelect\" onmouseover=\"tabpage_mouseover(this)\" onMouseOut=\"tabpage_mouseout(this)\" onclick=\"tabpage_selectonclient(this,'" + this.Items[j].UniqueID + "');\"><a href=\"#\" onfocus=\"this.blur();\">" + this.Items[j].Caption + "</a></li>");
                }
            }
        }

        private void RenderTabContent(HtmlTextWriter pOutPut)
        {
            for (int i = 0; i < this.Items.Count; i++)
            {
                this.Items[i].RenderControl(pOutPut);
            }
        }

        bool IPostBackDataHandler.LoadPostData(string ControlDataKey, NameValueCollection PostBackDataCollection)
        {
            string text = PostBackDataCollection[ControlDataKey];
            if (text != null && text != this.SelectedTabPageID)
            {
                this.SelectedTabPageID = text;
                return true;
            }
            return false;
        }

        void IPostBackDataHandler.RaisePostDataChangedEvent()
        {
            this.OnTabSelectedIndexChanged(EventArgs.Empty);
        }

        void IPostBackEventHandler.RaisePostBackEvent(string pEventArgument)
        {
        }

        public void InitTabPage()
        {
            this.CreateChildControls();
        }
    }
}