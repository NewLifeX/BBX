using System;

namespace Discuz.Plugin.Mail
{
    [AttributeUsage(AttributeTargets.All)]
    public class SmtpEmailAttribute : Attribute
    {
        
        private string _plugInName;
        public string PlugInName { get { return _plugInName; } set { _plugInName = value; } }

        private string _version;
        public string Version { get { return _version; } set { _version = value; } }

        private string _author;
        public string Author { get { return _author; } set { _author = value; } }

        private string _dllFileName;
        public string DllFileName { get { return _dllFileName; } set { _dllFileName = value; } }

        public SmtpEmailAttribute(string Name)
        {
            this._plugInName = Name;
        }
    }
}