namespace Discuz.Entity
{
    public class ModuleRequire
    {
        private FeatureType _feature = FeatureType.None;

        public FeatureType Feature { get { return _feature; } set { _feature = value; } }

        public ModuleRequire()
        {
        }

        public ModuleRequire(string featureType)
        {
            switch (featureType)
            {
                case "setprefs":
                    this._feature = FeatureType.SetPrefs;
                    return;
                case "dynamic-height":
                    this._feature = FeatureType.Dynamic_Height;
                    return;
                case "settitle":
                    this._feature = FeatureType.SetTitle;
                    return;
                case "tabs":
                    this._feature = FeatureType.Tabs;
                    return;
                case "drag":
                    this._feature = FeatureType.Drag;
                    return;
                case "grid":
                    this._feature = FeatureType.Grid;
                    return;
                case "minimessage":
                    this._feature = FeatureType.MiniMessage;
                    return;
                case "analytics":
                    this._feature = FeatureType.Analytics;
                    return;
                case "flash":
                    this._feature = FeatureType.Flash;
                    break;

                    //return;
            }
        }
    }
}