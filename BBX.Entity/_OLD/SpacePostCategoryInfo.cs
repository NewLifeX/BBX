namespace Discuz.Entity
{
    public class SpacePostCategoryInfo
    {
        
        private int _id;
        public int ID { get { return _id; } set { _id = value; } }

        private int _postid;
        public int PostID { get { return _postid; } set { _postid = value; } }

        private int _categoryid;
        public int CategoryID { get { return _categoryid; } set { _categoryid = value; } }
    }
}