namespace Discuz.Entity
{
    public class StatVarInfo
    {
        private string _type;
        private string _variable;
        
        public string Type { get { return _type.Trim(); } set { _type = value; } }

        public string Variable { get { return _variable.Trim(); } set { _variable = value; } }

        private string _value;
        public string Value { get { return _value; } set { _value = value; } }
    }
}