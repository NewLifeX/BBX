namespace Discuz.Entity
{
    public class ModuleContent
    {
        
        private ModuleContentType _type;
        public ModuleContentType Type { get { return _type; } set { _type = value; } }

        private string _href = string.Empty;
        public string Href { get { return _href; } set { _href = value; } }

        private string _cdata = string.Empty;
        public string CData { get { return _cdata; } set { _cdata = value; } }

        private string _contentHtml = string.Empty;
        public string ContentHtml { get { return _contentHtml; } set { _contentHtml = value; } }
    }
}