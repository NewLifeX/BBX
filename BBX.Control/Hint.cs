using System.ComponentModel;
using System.Text;
using System.Web.UI;

namespace BBX.Control
{
    [DefaultEvent("Click"), DefaultProperty("Text"), ToolboxData("<{0}:Hint runat=server></{0}:Hint>")]
    public class Hint : System.Web.UI.WebControls.WebControl
    {
        [Bindable(true), Category("Appearance"), DefaultValue("")]
        public string HintImageUrl
        {
            get
            {
                if (base.ViewState["hintimageurl"] != null)
                {
                    return (string)base.ViewState["hintimageurl"];
                }
                return "../images";
            }
            set { base.ViewState["hintimageurl"] = value; }
        }

        protected override void Render(HtmlTextWriter output)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<!--提示层部分开始-->");
            stringBuilder.Append("<span id=\"hintdivup\" style=\"display:none; position:absolute;z-index:500;\">\r\n");
            stringBuilder.Append("<div style=\"position:absolute; visibility: visible; width: 271px;z-index:501;\">\r\n");
            stringBuilder.Append("<p><img src=\"" + this.HintImageUrl + "/commandbg.gif\" /></p>\r\n");
            stringBuilder.Append("<div class=\"messagetext\"><img src=\"" + this.HintImageUrl + "/dot.gif\" /><span id=\"hintinfoup\" ></span></div>\r\n");
            stringBuilder.Append("<p><img src=\"" + this.HintImageUrl + "/commandbg2.gif\" /></p>\r\n");
            stringBuilder.Append("</div>\r\n");
            stringBuilder.Append("<iframe id=\"hintiframeup\" style=\"position:absolute;z-index:100;width:266px;scrolling:no;\" frameborder=\"0\"></iframe>\r\n");
            stringBuilder.Append("</span>\r\n");
            stringBuilder.Append("<span id=\"hintdivdown\" style=\"display:none; position:absolute;z-index:500;\">\r\n");
            stringBuilder.Append("<div style=\"position:absolute; visibility: visible; width: 271px;z-index:501;\">\r\n");
            stringBuilder.Append("<p><img src=\"" + this.HintImageUrl + "/commandbg3.gif\" /></p>\r\n");
            stringBuilder.Append("<div class=\"messagetext\"><img src=\"" + this.HintImageUrl + "/dot.gif\" /><span id=\"hintinfodown\" ></span></div>\r\n");
            stringBuilder.Append("<p><img src=\"" + this.HintImageUrl + "/commandbg4.gif\" /></p>\r\n");
            stringBuilder.Append("</div>\r\n");
            stringBuilder.Append("<iframe id=\"hintiframedown\" style=\"position:absolute;z-index:100;width:266px;scrolling:no;\" frameborder=\"0\"></iframe>\r\n");
            stringBuilder.Append("</span>\r\n");
            stringBuilder.Append("<!--提示层部分结束-->\r\n");
            output.Write(stringBuilder.ToString());
        }
    }
}