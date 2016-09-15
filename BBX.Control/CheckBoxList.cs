using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using XCode;

namespace BBX.Control
{
    [DefaultProperty("Text"), ToolboxData("<{0}:CheckBoxList runat=server></{0}:CheckBoxList>")]
    public class CheckBoxList : System.Web.UI.WebControls.CheckBoxList, IPostBackDataHandler
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

        public CheckBoxList()
        {
            this.RepeatColumns = 2;
            this.Width = Unit.Percentage(100.0);
            this.RepeatDirection = RepeatDirection.Vertical;
            this.RepeatLayout = RepeatLayout.Table;
            this.CssClass = "buttonlist";
        }

        public void AddTableData(DataTable dt)
        {
            this.Items.Clear();
            foreach (DataRow dataRow in dt.Rows)
            {
                this.Items.Add(new ListItem(dataRow[1].ToString(), dataRow[0].ToString()));
            }
            this.DataBind();
        }

        public void AddTableData(DataTable dt, string textName, string valueName)
        {
            this.Items.Clear();
            foreach (DataRow dataRow in dt.Rows)
            {
                this.Items.Add(new ListItem(dataRow[textName].ToString(), dataRow[valueName].ToString()));
            }
            this.DataBind();
        }

        public void AddTableData(IEntityList list, string textName = null, string valueName = null)
        {
            this.Items.Clear();
            if (list.Count == 0) return;

            var eop = EntityFactory.CreateOperate(list[0].GetType());
            var fs = eop.FieldNames.ToArray();
            if (textName == null) textName = fs[1];
            if (valueName == null) valueName = fs[0];
            foreach (var entity in list)
            {
                this.Items.Add(new ListItem(entity[textName] + "", entity[valueName] + ""));
            }
            this.DataBind();
        }

        //public void AddTableData(string sqlstring, string selectid)
        //{
        //    selectid = "," + selectid + ",";
        //    DataTable dataTable = DbHelper.ExecuteDataset(CommandType.Text, sqlstring).Tables[0];
        //    this.Items.Clear();
        //    for (int i = 0; i < dataTable.Rows.Count; i++)
        //    {
        //        this.Items.Add(new ListItem(dataTable.Rows[i][1].ToString(), dataTable.Rows[i][0].ToString()));
        //        if (selectid.IndexOf("," + dataTable.Rows[i][0].ToString() + ",") >= 0)
        //        {
        //            this.Items[i].Selected = true;
        //        }
        //    }
        //    this.DataBind();
        //}

        public void AddTableData(DataTable dt, string selectid)
        {
            selectid = "," + selectid + ",";
            this.Items.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                this.Items.Add(new ListItem(dt.Rows[i][1].ToString(), dt.Rows[i][0].ToString()));
                if (selectid.IndexOf("," + dt.Rows[i][0].ToString() + ",") >= 0)
                {
                    this.Items[i].Selected = true;
                }
            }
            this.DataBind();
        }

        public void SetSelectByID(string selectid)
        {
            selectid = "," + selectid + ",";
            for (int i = 0; i < this.Items.Count; i++)
            {
                if (selectid.IndexOf("," + this.Items[i].Value + ",") >= 0)
                {
                    this.Items[i].Selected = true;
                }
            }
            this.DataBind();
        }

        public string GetSelectString()
        {
            return this.GetSelectString(",");
        }

        public string GetSelectString(string split)
        {
            split = split.Trim();
            string text = "";
            foreach (ListItem listItem in this.Items)
            {
                if (listItem.Selected)
                {
                    text = text + listItem.Value + split;
                }
            }
            return text.TrimEnd(split.ToCharArray());
        }

        public string GetSelectString(string split, ListItemCollection items)
        {
            split = split.Trim();
            string text = "";
            foreach (ListItem listItem in items)
            {
                if (listItem.Selected)
                {
                    text = text + listItem.Value + split;
                }
            }
            return text.TrimEnd(split.ToCharArray());
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