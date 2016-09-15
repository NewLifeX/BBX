using System;
using System.ComponentModel;
using System.Web.UI;

namespace BBX.Control
{
    [ToolboxItem(false), ParseChildren(false), PersistChildren(true)]
    public class TabPage : WebControl, INamingContainer
    {
        private string _ClientID;
        private TabControl _tabControl;
        
        private string _ActionLink;
        [Browsable(true), Description(""), NotifyParentProperty(true)]
        private string ActionLink { get { return _ActionLink; } set { _ActionLink = value; } }

        private string _Caption;
        [Browsable(true), Description(""), NotifyParentProperty(true)]
        public string Caption { get { return _Caption; } set { _Caption = value; } }

        private bool _Selected;
        internal bool Selected { get { return _Selected; } set { _Selected = value; } }

        public TabPage()
        {
            this._Selected = false;
            this._Caption = string.Empty;
            this._ClientID = string.Empty;
            this._ActionLink = string.Empty;
        }

        public object GetTabControl()
        {
            return this._tabControl;
        }

        protected override void Render(HtmlTextWriter pOutPut)
        {
            if (this._tabControl == null || this._tabControl.GetType().ToString() != "BBX.Control.TabControl")
            {
                throw new ArgumentException("TabPage 必须是 TabControl 的子控件");
            }
            if (this.Selected)
            {
                pOutPut.Write("<div id=\"" + this.UniqueID + "\" class=\"tab-page\" style=\"display: block;background: #fff;\">");
            }
            else
            {
                pOutPut.Write("<div id=\"" + this.UniqueID + "\" class=\"tab-page\" style=\"display: none;background: #fff;\">");
            }
            this.RenderChildren(pOutPut);
            pOutPut.Write("</div>");
        }

        internal void RenderDownLevelContent(HtmlTextWriter pOutPut)
        {
            this.Render(pOutPut);
        }

        internal void SetTabControl(TabControl pTabControl)
        {
            this._tabControl = pTabControl;
        }
    }
}