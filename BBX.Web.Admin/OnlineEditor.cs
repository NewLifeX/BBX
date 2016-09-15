using System.Web.UI.HtmlControls;
using BBX.Common;

namespace BBX.Web.Admin
{
    public class OnlineEditor : UserControlsPageBase
    {
        protected HtmlTextArea DataTextarea;
        public int postminchars;
        public int postmaxchars = 200;
        public string text = "";

        public string Text
        {
            get { return DNTRequest.GetString(this.ID + "message_hidden").Replace("'", "''"); }
            set { text = value; }
        }
    }
}