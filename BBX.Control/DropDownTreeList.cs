using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XCode;

namespace BBX.Control
{
    [DefaultProperty("Text"), ToolboxData("<{0}:DropDownTreeList runat=server></{0}:DropDownTreeList>")]
    public class DropDownTreeList : WebControl, IPostBackDataHandler, IPostBackEventHandler
    {
        public System.Web.UI.WebControls.DropDownList TypeID = new System.Web.UI.WebControls.DropDownList();

        [Bindable(true), Browsable(true), Category("Appearance"), DefaultValue("")]
        public string SelectedValue { get { return TypeID.SelectedValue; } set { TypeID.SelectedValue = value; } }

        private string m_parentid = "parentid";
        [Bindable(true), Category("Appearance"), DefaultValue("parentid")]
        public string ParentID { get { return m_parentid; } set { m_parentid = value; } }

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
                    this.TypeID.Attributes.Add("onChange", "document.getElementById('" + value + "').focus();");
                }
            }
        }

        public DropDownTreeList()
        {
            this.BorderStyle = BorderStyle.Groove;
            this.BorderWidth = 1;
        }

        public void BuildTree(DataTable dt, string textName, string valueName)
        {
            string selectedValue = "0";
            this.TypeID.SelectedValue = selectedValue;
            this.Controls.Add(this.TypeID);
            this.TypeID.Items.Clear();
            this.TypeID.Items.Add(new ListItem("请选择     ", "0"));
            DataRow[] array = dt.Select(this.ParentID + "=0");
            DataRow[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                DataRow dataRow = array2[i];
                this.TypeID.Items.Add(new ListItem(dataRow[textName].ToString(), dataRow[valueName].ToString()));
                string blank = HttpUtility.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;");
                this.BindNode(dataRow[valueName].ToString(), dt, blank, textName, valueName);
            }
            this.TypeID.DataBind();
        }

        private void BindNode(string sonparentid, DataTable dt, string blank, string textName, string valueName)
        {
            DataRow[] array = dt.Select(this.ParentID + "=" + sonparentid);
            DataRow[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                DataRow dataRow = array2[i];
                string text = dataRow[valueName].ToString();
                string text2 = dataRow[textName].ToString();
                text2 = blank + text2;
                this.TypeID.Items.Add(new ListItem(text2, text));
                string blank2 = HttpUtility.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;" + blank);
                this.BindNode(text, dt, blank2, textName, valueName);
            }
        }

        public void BuildTree(IEntityTree tree, string textName, string valueName)
        {
            string selectedValue = "0";
            this.TypeID.SelectedValue = selectedValue;
            this.Controls.Add(this.TypeID);
            this.TypeID.Items.Clear();
            this.TypeID.Items.Add(new ListItem("请选择     ", "0"));
            foreach (var item in tree.Childs)
            {
                this.TypeID.Items.Add(new ListItem(item[textName] + "", item[valueName] + ""));
                string blank = HttpUtility.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;");
                this.BindNode(item as IEntityTree, blank, textName, valueName);
            }
            this.TypeID.DataBind();
        }

        private void BindNode(IEntityTree tree, string blank, string textName, string valueName)
        {
            foreach (var item in tree.Childs)
            {
                this.TypeID.Items.Add(new ListItem(blank + item[textName], item[valueName] + ""));
                string blank2 = HttpUtility.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;" + blank);
                this.BindNode(item as IEntityTree, blank2, textName, valueName);
            }
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

        public void RaisePostDataChangedEvent()
        {
        }

        public void RaisePostBackEvent(string eventArgument)
        {
        }

        public bool LoadPostData(string postDataKey, NameValueCollection postCollection)
        {
            string selectedValue = this.TypeID.SelectedValue;
            string value = postCollection[postDataKey];
            return !selectedValue.Equals(value);
        }
    }
}