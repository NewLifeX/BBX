using BBX.Config;
using BBX.Entity;

namespace BBX.Forum.ScheduledEvents
{
    public class NoticesEvent : IEvent
    {
        public void Execute(object state)
        {
            if (GeneralConfigInfo.Current.Notificationreserveddays > 0)
            {
                //Notices.DeleteNotice();
                Notice.DeleteNotice();
            }
        }
    }
}