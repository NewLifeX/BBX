namespace Discuz.Entity
{
    public class SpaceLinkInfo
    {
        
        private int _linkid;
        public int LinkId { get { return _linkid; } set { _linkid = value; } }

        private int _userid;
        public int UserId { get { return _userid; } set { _userid = value; } }

        private string _linktitle;
        public string LinkTitle { get { return _linktitle; } set { _linktitle = value; } }

        private string _linkurl;
        public string LinkUrl { get { return _linkurl; } set { _linkurl = value; } }

        private string _description;
        public string Description { get { return _description; } set { _description = value; } }
    }
}