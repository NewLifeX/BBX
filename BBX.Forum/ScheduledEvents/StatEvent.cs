using System;
using BBX.Entity;

namespace BBX.Forum.ScheduledEvents
{
    public class StatEvent : IEvent
    {
        void IEvent.Execute(object state)
        {
            //DatabaseProvider.GetInstance().UpdateYesterdayPosts(TableList.GetPostTableId());
            Statistic.UpdateYesterdayPosts();
            Statistic.Reset();
            //XCache.Remove(CacheKeys.FORUM_STATISTICS);
            if (DateTime.Today.Day == 1)
            {
                //DatabaseProvider.GetInstance().ResetThismonthOnlineTime();
                OnlineTime.ResetThismonthOnlineTime();
                //DatabaseProvider.GetInstance().UpdateStatVars("onlines", "lastupdate", "0");
                StatVar.Update("onlines", "lastupdate", "0");
            }
        }
    }
}