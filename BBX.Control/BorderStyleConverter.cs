using System.ComponentModel;

namespace BBX.Control
{
    public class BorderStyleConverter : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }

        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new TypeConverter.StandardValuesCollection(new string[]
			{
				"none",
				"dotted",
				"dashed",
				"solid",
				"double",
				"groove",
				"ridge",
				"inset",
				"window-inset",
				"outset"
			});
        }
    }
}