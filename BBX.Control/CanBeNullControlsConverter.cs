using System.Collections;
using System.ComponentModel;

namespace BBX.Control
{
    public class CanBeNullControlsConverter : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new TypeConverter.StandardValuesCollection(new ArrayList
			{
				"可为空",
				"必填"
			});
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }
    }
}