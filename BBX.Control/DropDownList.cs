using System.ComponentModel;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using XCode;

namespace BBX.Control
{
    [DefaultProperty("Text"), ToolboxData("<{0}:DropDownList runat=server></{0}:DropDownList>")]
    public class DropDownList : System.Web.UI.WebControls.DropDownList, IPostBackDataHandler
    {
        
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
                    base.Attributes.Add("onChange", "document.getElementById('" + value + "').focus();");
                }
            }
        }

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

        public void AddTableData(DataTable dt, string textName, string valueName)
        {
            this.Items.Clear();
            this.Items.Add(new ListItem("请选择     ", "0"));
            foreach (DataRow dataRow in dt.Rows)
            {
                this.Items.Add(new ListItem(dataRow[textName].ToString(), dataRow[valueName].ToString()));
            }
            this.DataBind();
        }

        public void AddTableData(IEntityList list, string textName, string valueName)
        {
            this.Items.Clear();
            this.Items.Add(new ListItem("请选择     ", "0"));
            foreach (var entity in list)
            {
                this.Items.Add(new ListItem(entity[textName] + "", entity[valueName] + ""));
            }
            this.DataBind();
        }

        protected override void Render(HtmlTextWriter output)
        {
            if (this.HintInfo != "")
            {
                output.WriteBeginTag("span id=\"" + this.ClientID + "\"  onmouseover=\"showhintinfo(this," + this.HintLeftOffSet + "," + this.HintTopOffSet + ",'" + this.HintTitle + "','" + this.HintInfo + "','" + this.HintHeight + "','" + this.HintShowType + "');\" onmouseout=\"hidehintinfo();\">");
            }
            base.Render(output);
            if (this.HintInfo != "")
            {
                output.WriteEndTag("span");
            }
        }
    }
}