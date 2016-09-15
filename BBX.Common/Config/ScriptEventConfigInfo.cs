using System;
using System.ComponentModel;
using System.Xml.Serialization;
using NewLife.Xml;

namespace BBX.Config
{
    [XmlConfigFile("config/scriptevent.config")]
    /// <summary>脚本事件配置</summary>
    [Description("脚本事件配置")]
    [Serializable]
    public class ScriptEventConfigInfo : XmlConfig2<ScriptEventConfigInfo>
    {
        /// <summary>脚本事件</summary>
        [Description("脚本事件")]
        [XmlArray("scriptevent")]
        public ScriptEventInfo[] ScriptEvents = new ScriptEventInfo[0];

        //static ScriptEventConfigInfo()
        //{
        //    if (!_.ConfigFile.IsNullOrWhiteSpace())
        //        _.ConfigFile = _.ConfigFile.Replace("scriptevent.config", "scriptevent_" + BaseConfigs.GetDbType + ".config");
        //}
    }

    [Serializable]
    public class ScriptEventInfo
    {
        private string _key;
        [XmlAttribute("key")]
        public string Key { get { return _key; } set { _key = value; } }

        private string _title;
        [XmlAttribute("title")]
        public string Title { get { return _title; } set { _title = value; } }

        private string _script;
        [XmlText]
        public string Script { get { return _script; } set { _script = value; } }

        private int _timeofday;
        [XmlAttribute("timeofday")]
        public int Timeofday { get { return _timeofday; } set { _timeofday = value; } }

        private int _miniutes;
        [XmlAttribute("miniutes")]
        public int Miniutes { get { return _miniutes; } set { _miniutes = value; } }

        [XmlIgnore]
        public bool ShouldExecute { get { return true; } } 

        private bool _enabled;
        [XmlAttribute("enabled")]
        public bool Enabled { get { return _enabled; } set { _enabled = value; } }
    }
}