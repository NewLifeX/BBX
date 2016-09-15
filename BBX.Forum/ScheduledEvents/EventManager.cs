using System.Collections.Generic;
using BBX.Config;
using NewLife.Threading;

namespace BBX.Forum.ScheduledEvents
{
    /// <summary>事件管理</summary>
    public class EventManager
    {
        public static string RootPath;

        /// <summary>定时器间隔，分钟</summary>
        public static readonly int TimerMinutesInterval;

        private EventManager() { }

        static EventManager()
        {
            var cfg = ScheduleConfigInfo.Current;
            TimerMinutesInterval = cfg.TimerMinutesInterval;
            if (TimerMinutesInterval <= 0) TimerMinutesInterval = 5;
        }

        /// <summary>执行</summary>
        public static void Execute()
        {
            var list = new List<Event>();
            var events = ScheduleConfigInfo.Current.Events;
            if (events != null && events.Length > 0)
            {
                foreach (var ev in events)
                {
                    if (ev.Enabled)
                    {
                        list.Add(new Event
                        {
                            Key = ev.Key,
                            Minutes = ev.Minutes,
                            ScheduleType = ev.ScheduleType,
                            TimeOfDay = ev.TimeOfDay
                        });
                    }
                }
            }

            foreach (var ev in list)
            {
                if (ev.ShouldExecute)
                {
                    ev.UpdateTime();
                    if (ev.IEventInstance != null) ThreadPoolX.QueueUserWorkItem(ev.IEventInstance.Execute);
                }
            }
        }
    }
}