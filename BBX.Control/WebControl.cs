using System.ComponentModel;

namespace BBX.Control
{
    public class WebControl : System.Web.UI.WebControls.WebControl
    {
        
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
        [Bindable(true), Category("Appearance"), DefaultValue(50)]
        public int HintHeight { get { return _hintHeight; } set { _hintHeight = value; } }
    }
}