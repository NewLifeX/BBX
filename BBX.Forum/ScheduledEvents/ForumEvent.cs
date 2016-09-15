using BBX.Forum;
using BBX.Forum.ScheduledEvents;

namespace BBX.Forum.ScheduledEvents
{
    public class ForumEvent : IEvent
    {
        public void Execute(object state)
        {
            Topics.NeatenRelateTopics();
        }
    }
}