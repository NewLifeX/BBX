using System.Collections;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace BBX.Control
{
    public class FormControlsConverter : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            ControlCollection controls = ((Page)context.Container.Components[0]).Controls;
            ArrayList arrayList = new ArrayList();
            for (int i = 0; i < controls.Count; i++)
            {
                if (controls[i] is HtmlTable || controls[i] is HtmlForm || controls[i] is HtmlGenericControl || controls[i] is HtmlImage || controls[i] is Label || controls[i] is DataGrid || controls[i] is DataList || controls[i] is Table || controls[i] is Repeater || controls[i] is Image || controls[i] is Panel || controls[i] is PlaceHolder || controls[i] is Calendar || controls[i] is AdRotator || controls[i] is Xml)
                {
                    arrayList.Add(controls[i].ClientID);
                }
            }
            return new TypeConverter.StandardValuesCollection(arrayList);
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }
    }
}