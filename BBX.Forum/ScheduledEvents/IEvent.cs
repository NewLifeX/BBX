using System;
namespace BBX.Forum.ScheduledEvents
{
	public interface IEvent
	{
		void Execute(object state);
	}
}
