using System;
using System.Web.UI.HtmlControls;
using BBX.Control;

namespace BBX.Web.Admin
{
    public class TextareaResize : UserControlsPageBase
    {
        protected HtmlTextArea posttextarea;
        public string controlname;
        public string imagepath = "";
        public int rows = 5;
        public int cols = 45;
        public bool is_replace;
        public string maxlength;

        public string Text
        {
            get
            {
                if (this.is_replace)
                {
                    return this.posttextarea.InnerText.Replace("'", "''");
                }
                return this.posttextarea.InnerText;
            }
            set
            {
                posttextarea.InnerText = value;
            }
        }

        private string _hintTitle = "";

        public string HintTitle { get { return _hintTitle; } set { _hintTitle = value; } }

        private string _hintInfo = "";

        public string HintInfo { get { return _hintInfo; } set { _hintInfo = value; } }

        private int _hintLeftOffSet;

        public int HintLeftOffSet { get { return _hintLeftOffSet; } set { _hintLeftOffSet = value; } }

        private int _hintTopOffSet;

        public int HintTopOffSet { get { return _hintTopOffSet; } set { _hintTopOffSet = value; } }

        private string _hintShowType = "up";

        public string HintShowType { get { return _hintShowType; } set { _hintShowType = value; } }

        private int _hintHeight = 30;

        public int HintHeight { get { return _hintHeight; } set { _hintHeight = value; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.posttextarea.Rows = this.rows;
            this.posttextarea.Cols = this.cols;
            if (this.maxlength != null)
            {
                this.posttextarea.Attributes.Add("onkeyup", "return isMaxLen(this)");
                this.posttextarea.Attributes.Add("maxlength", this.maxlength);
            }
        }
    }
}