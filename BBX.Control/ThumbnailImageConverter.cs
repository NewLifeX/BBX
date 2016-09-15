using System.Collections;
using System.ComponentModel;

namespace BBX.Control
{
    public class ThumbnailImageConverter : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new TypeConverter.StandardValuesCollection(new ArrayList
			{
				"不生成缩略图",
				"生成缩略图"
			});
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }
    }
}