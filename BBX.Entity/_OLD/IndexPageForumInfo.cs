namespace Discuz.Entity
{
    public class IndexPageForumInfo : ForumInfo
    {
        
        private string _havenew;
        public string Havenew { get { return _havenew; } set { _havenew = value; } }

        private string _collapse = string.Empty;
        public string Collapse { get { return _collapse; } set { _collapse = value; } }
    }
}