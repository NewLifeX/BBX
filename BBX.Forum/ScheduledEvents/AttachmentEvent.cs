using System;
using BBX.Entity;
using BBX.Forum;
using BBX.Forum.ScheduledEvents;
using NewLife.Log;

namespace BBX.Forum.ScheduledEvents
{
    public class AttachmentEvent : IEvent
    {
        public void Execute(object state)
        {
            // 删除所有人今天未使用的附件
            var list = Attachment.FindAllNoUsed(0, DateTime.MinValue);
            if (list.Count > 0)
            {
                //XTrace.WriteLine("删除所有人今天未使用的附件：{0}", list.ToList().Join(",", e => e.ID + ""));
                XTrace.WriteLine("删除所有人今天未使用的附件：{0}", list.Join("ID", ","));
                list.Delete();
            }
        }
    }
}