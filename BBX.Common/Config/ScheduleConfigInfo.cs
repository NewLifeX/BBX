using System;
using System.ComponentModel;
using System.Xml.Serialization;
using NewLife.Xml;

namespace BBX.Config
{
    [XmlConfigFile("config/schedule.config")]
    /// <summary>计划任务设置</summary>
    [Description("计划任务设置")]
    [Serializable]
    public class ScheduleConfigInfo : XmlConfig2<ScheduleConfigInfo>
    {
        /// <summary>启用</summary>
        [Description("启用")]
        [XmlElement("enabled")]
        public bool Enabled;

        /// <summary>事件</summary>
        [Description("事件")]
        [XmlArray("events")]
        public Event[] Events;

        /// <summary>时间间隔，分钟</summary>
        [Description("时间间隔，分钟")]
        [XmlElement("minutes_interval")]
        public int TimerMinutesInterval;
    }

    [XmlRoot("event")]
    [Serializable]
    public class Event
    {
        private string _key;
        /// <summary>键</summary>
        [Description("键")]
        [XmlAttribute("key")]
        public string Key { get { return _key; } set { _key = value; } }

        private int _timeOfDay = -1;
        /// <summary>每天定时执行的时间，分钟</summary>
        [Description("每天定时执行的时间，分钟")]
        [XmlAttribute("time_of_day")]
        public int TimeOfDay { get { return _timeOfDay; } set { _timeOfDay = value; } }

        private int _minutes = 60;
        /// <summary>定时多少分钟执行一次</summary>
        [Description("定时多少分钟执行一次")]
        [XmlAttribute("minutes")]
        public int Minutes { get { return _minutes; } set { _minutes = value; } }

        private string _scheduleType;
        [XmlAttribute("type")]
        public string ScheduleType { get { return _scheduleType; } set { _scheduleType = value; } }

        private bool _enabled;
        [XmlAttribute("enabled")]
        public bool Enabled { get { return _enabled; } set { _enabled = value; } }

        private bool _isSystemEvent;
        [XmlAttribute("is_system_event")]
        public bool IsSystemEvent { get { return _isSystemEvent; } set { _isSystemEvent = value; } }
    }
}