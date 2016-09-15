namespace Discuz.Entity
{
    public class StatInfo
    {
        private string _type;
        private string _variable;
        
        public string Type { get { return _type.Trim(); } set { _type = value; } }

        public string Variable { get { return _variable.Trim(); } set { _variable = value; } }

        private int _count;
        public int Count { get { return _count; } set { _count = value; } }
    }
}