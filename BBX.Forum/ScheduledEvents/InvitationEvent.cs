using BBX.Entity;
using BBX.Forum.ScheduledEvents;

namespace BBX.Forum.ScheduledEvents
{
    public class InvitationEvent : IEvent
    {
        public void Execute(object state)
        {
            Invitation.ClearExpireInviteCode();
        }
    }
}