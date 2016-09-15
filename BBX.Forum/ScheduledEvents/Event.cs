using System;
using System.Xml.Serialization;
using BBX.Entity;
using NewLife.Log;

namespace BBX.Forum.ScheduledEvents
{
    class Event
    {
        private IEvent _ievent;
        public IEvent IEventInstance
        {
            get
            {
                this.LoadIEvent();
                return this._ievent;
            }
        }

        private string _key;
        /// <summary>键</summary>
        public string Key { get { return _key; } set { _key = value; } }

        private int _timeOfDay = -1;
        /// <summary>每天定时执行的时间，分钟</summary>
        public int TimeOfDay { get { return _timeOfDay; } set { _timeOfDay = value; } }

        private int _minutes = 60;
        /// <summary>定时多少分钟执行一次</summary>
        public int Minutes
        {
            get
            {
                if (this._minutes < EventManager.TimerMinutesInterval)
                {
                    return EventManager.TimerMinutesInterval;
                }
                return this._minutes;
            }
            set { _minutes = value; }
        }

        private string _scheduleType;
        [XmlAttribute("type")]
        public string ScheduleType { get { return _scheduleType; } set { _scheduleType = value; } }

        private bool dateWasSet;
        private DateTime _lastCompleted;
        [XmlIgnore]
        public DateTime LastCompleted
        {
            get { return _lastCompleted; }
            set
            {
                this.dateWasSet = true;
                this._lastCompleted = value;
            }
        }

        /// <summary>是否应该执行</summary>
        [XmlIgnore]
        public bool ShouldExecute
        {
            get
            {
                if (!dateWasSet)
                {
                    LastCompleted = ScheduledEvent.GetLast(Key, Environment.MachineName);
                }
                if (TimeOfDay > -1)
                {
                    var now = DateTime.Now;
                    // 执行时间
                    var runtime = now.Date.AddMinutes(TimeOfDay);
                    // 如果上一次执行在这一次之前，并且今天时间已到
                    return this.LastCompleted < runtime && runtime <= now;
                }
                return LastCompleted.AddMinutes(Minutes) < DateTime.Now;
            }
        }

        private void LoadIEvent()
        {
            if (this._ievent == null)
            {
                if (this.ScheduleType == null)
                {
                    XTrace.WriteLine("计划任务没有定义其 type 属性");
                }

                Type type = Type.GetType(this.ScheduleType);
                if (type == null)
                {
                    XTrace.WriteLine(string.Format("计划任务 {0} 无法被正确识别", this.ScheduleType));
                    return;
                }
                this._ievent = (IEvent)Activator.CreateInstance(type);
                if (this._ievent == null)
                {
                    XTrace.WriteLine(string.Format("计划任务 {0} 未能正确加载", this.ScheduleType));
                }
            }
        }

        public void UpdateTime()
        {
            this.LastCompleted = DateTime.Now;
            ScheduledEvent.SetLast(Key, Environment.MachineName, LastCompleted);
        }
    }
}