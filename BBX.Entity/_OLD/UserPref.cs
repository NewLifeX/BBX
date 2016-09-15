using System.Collections.Generic;

namespace Discuz.Entity
{
    public class UserPref
    {
        private List<EnumValue> _enumValues;

        private string _name = string.Empty;
        public string Name { get { return _name; } set { _name = value; } }

        private string _displayName = string.Empty;
        public string DisplayName { get { return _displayName; } set { _displayName = value; } }

        private UserPrefDataType _dataType;
        public UserPrefDataType DataType { get { return _dataType; } set { _dataType = value; } }

        private string _defaultValue = string.Empty;
        public string DefaultValue { get { return _defaultValue; } set { _defaultValue = value; } }

        private string _urlParam = string.Empty;
        public string UrlParam { get { return _urlParam; } set { _urlParam = value; } }

        private bool _required;
        public bool Required { get { return _required; } set { _required = value; } }

        public List<EnumValue> EnumValues { get { return _enumValues; } set { _enumValues = value; } }

        public string ToHtml()
        {
            return this.ToHtml("");
        }

        public string ToHtml(string value)
        {
            string text = string.Empty;
            value = ((string.IsNullOrEmpty(value)) ? this._defaultValue : value);
            string text2 = (string.IsNullOrEmpty(this._displayName)) ? this._name : this._displayName;
            string text3 = this._required ? "<font color=\"red\">*</font>" : "";
            switch (this._dataType)
            {
                case UserPrefDataType.StringType:
                    text = string.Format("<tr><td colspan=\"1\" align=\"right\" width=\"35%\">{0}{3}</td><td colspan=\"2\" align=\"left\" nowrap=\"nowrap\" width=\"65%\">&nbsp;<input type=\"text\" size=\"20\" maxlen=\"200\" name=\"m___MODULE_ID___up_{1}\" value=\"{2}\" /></td></tr>", new object[]
				{
					text2,
					this._name,
					value,
					text3
				});
                    break;

                case UserPrefDataType.EnumType:
                    foreach (EnumValue current in this._enumValues)
                    {
                        string arg = (string.IsNullOrEmpty(current.DisplayValue)) ? current.Value : current.DisplayValue;
                        if (current.Value == value)
                        {
                            text += string.Format("<option value=\"{0}\" selected>{1}</option>\r\n", current.Value, arg);
                        }
                        else
                        {
                            text += string.Format("<option value=\"{0}\">{1}</option>\r\n", current.Value, arg);
                        }
                    }
                    text = string.Format("<tr><td colspan=\"1\" align=\"right\" width=\"35%\">{0}{3}</td><td colspan=\"2\" align=\"left\" nowrap=\"nowrap\" width=\"65%\">&nbsp;<select id=\"m___MODULE_ID_____ITEM_INDEX__\" name=\"m___MODULE_ID___up_{1}\">{2}</select></td></tr>", new object[]
				{
					text2,
					this._name,
					text,
					text3
				});
                    break;

                case UserPrefDataType.HiddenType:
                    text = "";
                    break;

                case UserPrefDataType.BoolType:
                    {
                        int num = 0;
                        if (value.ToLower() == "true" || value == "1")
                        {
                            value = "checked";
                            num = 1;
                        }
                        else
                        {
                            value = "";
                        }
                        text = string.Format("<tr><td colspan=\"1\" align=\"right\" width=\"35%\">{0}{4}</td><td colspan=\"2\" align=\"left\" nowrap=\"nowrap\" width=\"65%\">&nbsp;<input id=\"m___MODULE_ID_____ITEM_INDEX__\" name=\"m___MODULE_ID___up_{3}\" value=\"{1}\" type=\"hidden\" /><input type=\"checkbox\" {2} onclick=\"_gel('m___MODULE_ID_____ITEM_INDEX__').value = this.checked ? '1' : '0';\" /></td></tr>", new object[]
				{
					text2,
					num,
					value,
					this._name,
					text3
				});
                        break;
                    }
                case UserPrefDataType.ListType:
                    text = string.Format("<tr><td colspan=\"1\" align=\"right\" width=\"35%\">{0}{1}</td><td width=\"65%\" nowrap=\"\" align=\"left\" colspan=\"2\"><script>check_ac___MODULE_ID_____ITEM_INDEX__ = null;</script><nobr><input type=\"text\" name=\"m___MODULE_ID___up_{3}_val\" value=\"\" id=\"m___MODULE_ID_____ITEM_INDEX___val\" maxlen=\"200\" size=\"20\"/><input id=\"m___MODULE_ID_____ITEM_INDEX___add\" type=\"button\" onclick=\"m___MODULE_ID_____ITEM_INDEX___App.add();\" value=\"Add\"/><input id=\"m___MODULE_ID_____ITEM_INDEX__\" type=\"hidden\" value=\"{3}\" name=\"m___MODULE_ID___up_{2}\"/></nobr></td></tr>", new object[]
				{
					text2,
					text3,
					this._name,
					value
				});
                    text += string.Format("<tr><td /><td><div id=\"m___MODULE_ID_____ITEM_INDEX___disp\" style=\"padding-top: 4px;\">", new object[0]);
                    text += string.Format("</div><script type=\"text/javascript\"><!--//\r\nlistcontrol___MODULE_ID__.push([_gel(\"m___MODULE_ID_____ITEM_INDEX___val\"),check_ac___MODULE_ID_____ITEM_INDEX__]);var m___MODULE_ID_____ITEM_INDEX___TextVal = _gel('m___MODULE_ID_____ITEM_INDEX__').value;var m___MODULE_ID_____ITEM_INDEX___App = new _PrefListApp(\"__ITEM_INDEX__\",\"up_{0}\",m___MODULE_ID_____ITEM_INDEX___TextVal,_ListItem,\"__MODULE_ID__\");m___MODULE_ID_____ITEM_INDEX___App.refresh();_gel('m___MODULE_ID_____ITEM_INDEX__').listApp = m___MODULE_ID_____ITEM_INDEX___App;_gel('m___MODULE_ID_____ITEM_INDEX___val').listApp = m___MODULE_ID_____ITEM_INDEX___App;\r\n// --></script></td></tr>", this._name);
                    break;

                case UserPrefDataType.LoactionType:
                    text = "";
                    break;
            }
            return text;
        }
    }
}