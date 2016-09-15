namespace Discuz.Entity
{
    public class MySqlInfo
    {
        
        private string _tablename;
        public string tablename { get { return _tablename; } set { _tablename = value; } }

        private string _tabletype;
        public string tabletype { get { return _tabletype; } set { _tabletype = value; } }

        private string _rowcount;
        public string rowcount { get { return _rowcount; } set { _rowcount = value; } }

        private string _index;
        public string index { get { return _index; } set { _index = value; } }

        private string _datafree;
        public string datafree { get { return _datafree; } set { _datafree = value; } }

        private string _tabledata;
        public string tabledata { get { return _tabledata; } set { _tabledata = value; } }
    }
}