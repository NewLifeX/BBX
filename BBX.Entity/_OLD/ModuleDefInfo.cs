namespace Discuz.Entity
{
    public class ModuleDefInfo
    {
        
        private int _moduleDefID;
        public int ModuleDefID { get { return _moduleDefID; } set { _moduleDefID = value; } }

        private string _moduleName;
        public string ModuleName { get { return _moduleName; } set { _moduleName = value; } }

        private int _cacheTime;
        public int CacheTime { get { return _cacheTime; } set { _cacheTime = value; } }

        private string _description;
        public string Description { get { return _description; } set { _description = value; } }

        private string _configFile;
        public string ConfigFile { get { return _configFile; } set { _configFile = value; } }

        private string _bussinessController;
        public string BussinessController { get { return _bussinessController; } set { _bussinessController = value; } }

        private string _folderName;
        public string FolderName { get { return _folderName; } set { _folderName = value; } }
    }
}