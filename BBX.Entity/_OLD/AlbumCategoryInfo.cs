namespace Discuz.Entity
{
    public class AlbumCategoryInfo
    {
        private string title = string.Empty;
        private string description = string.Empty;
        
        private int albumcateid;
        public int Albumcateid { get { return albumcateid; } set { albumcateid = value; } }

        public string Title { get { return title.Trim(); } set { title = value; } }

        public string Description { get { return description.Trim(); } set { description = value; } }

        private int albumcount;
        public int Albumcount { get { return albumcount; } set { albumcount = value; } }

        private int displayorder;
        public int Displayorder { get { return displayorder; } set { displayorder = value; } }
    }
}