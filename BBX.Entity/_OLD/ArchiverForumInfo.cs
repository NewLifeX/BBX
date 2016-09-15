namespace Discuz.Entity
{
    public class ArchiverForumInfo
    {
        private string name;
        private string parentidList;
        private string viewPerm;

        private int fid;
        public int Fid { get { return fid; } set { fid = value; } }

        public string Name { get { return name.Trim(); } set { name = value; } }

        public string ParentidList { get { return parentidList.Trim(); } set { parentidList = value; } }

        private int status;
        public int Status { get { return status; } set { status = value; } }

        private int layer;
        public int Layer { get { return layer; } set { layer = value; } }

        public string ViewPerm { get { return viewPerm.Trim(); } set { viewPerm = value; } }
    }
}