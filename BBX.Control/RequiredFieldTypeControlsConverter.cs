using System.Collections;
using System.ComponentModel;

namespace BBX.Control
{
    public class RequiredFieldTypeControlsConverter : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new TypeConverter.StandardValuesCollection(new ArrayList
			{
				"暂无校验",
				"数据校验",
				"电子邮箱",
				"移动手机",
				"家用电话",
				"身份证号码",
				"网页地址",
				"日期",
				"日期时间",
				"金额",
				"IP地址",
				"IP地址带端口"
			});
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }
    }
}