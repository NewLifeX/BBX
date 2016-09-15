namespace Discuz.Entity
{
    public class EnumValue
    {
        
        private string _value = string.Empty;
        public string Value { get { return _value; } set { _value = value; } }

        private string _displayValue = string.Empty;
        public string DisplayValue { get { return _displayValue; } set { _displayValue = value; } }

        public EnumValue()
        {
        }

        public EnumValue(string value)
        {
            this._value = value;
        }
    }
}