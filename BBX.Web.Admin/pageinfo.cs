using System;
using System.IO;
using System.Web;
using System.Xml;
using BBX.Common;
using NewLife.Web;

namespace BBX.Web.Admin
{
    public class pageinfo : UserControlsPageBase
    {
        public enum IconType
        {
            Information,
            Warning
        }

        private pageinfo.IconType _icon;
        private string _text = "";

        public pageinfo.IconType Icon
        {
            get
            {
                return _icon;
            }
            set
            {
                switch (value)
                {
                    case pageinfo.IconType.Information:
                        this._icon = pageinfo.IconType.Information;
                        return;
                    case pageinfo.IconType.Warning:
                        this._icon = pageinfo.IconType.Warning;
                        return;
                    default:
                        return;
                }
            }
        }

        public string Text { get { return _text; } set { _text = Fix(value); } }

        public pageinfo()
        {
            this.Visible = this.ReadAdminUserConfig();
        }

        protected string GetInfoImg()
        {
            switch (this._icon)
            {
                case pageinfo.IconType.Information:
                    return "../images/hint.gif";
                case pageinfo.IconType.Warning:
                    return "../images/warning.gif";
                default:
                    return "../images/hint.gif";
            }
        }

        private bool ReadAdminUserConfig()
        {
            //string str = HttpContext.Current.Request.Cookies["bbx_admin"]["userid"].ToString();
            var str = WebHelper.ReadCookie("bbx_admin", "userid");
            string text = HttpContext.Current.Server.MapPath("../xml/user_" + str + ".config");
            if (File.Exists(text))
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.Load(text);
                var xmlNode = xmlDocument.SelectSingleNode("/UserConfig/ShowInfo");
                return xmlNode == null || xmlNode.InnerText == "1";
            }
            return true;
        }

        String Fix(String str)
        {
            if (String.IsNullOrEmpty(str)) return str;

            str = str.Replace("{forumproductname}", Utils.ProductName);

            return str;
        }
    }
}