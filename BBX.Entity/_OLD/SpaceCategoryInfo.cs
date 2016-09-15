namespace Discuz.Entity
{
    public class SpaceCategoryInfo
    {
        
        private int _categoryid;
        public int CategoryID { get { return _categoryid; } set { _categoryid = value; } }

        private string _title;
        public string Title { get { return _title; } set { _title = value; } }

        private int _uid;
        public int Uid { get { return _uid; } set { _uid = value; } }

        private string _description;
        public string Description { get { return _description; } set { _description = value; } }

        private int _typeid;
        public int TypeID { get { return _typeid; } set { _typeid = value; } }

        private int _categorycount;
        public int CategoryCount { get { return _categorycount; } set { _categorycount = value; } }

        private int _displayorder;
        public int Displayorder { get { return _displayorder; } set { _displayorder = value; } }
    }
}