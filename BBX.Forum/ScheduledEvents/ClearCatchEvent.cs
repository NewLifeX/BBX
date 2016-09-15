using System.IO;
using BBX.Common;
using BBX.Config;
using BBX.Forum.ScheduledEvents;

namespace BBX.Forum.ScheduledEvents
{
    public class ClearCatchEvent : IEvent
    {
        public void Execute(object state)
        {
            try
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(Utils.GetMapPath(BaseConfigs.GetForumPath + "cache/showtopic"));
                FileSystemInfo[] fileSystemInfos = directoryInfo.GetFileSystemInfos();
                for (int i = 0; i < fileSystemInfos.Length; i++)
                {
                    FileSystemInfo fileSystemInfo = fileSystemInfos[i];
                    fileSystemInfo.Delete();
                }
            }
            catch
            {
            }
        }
    }
}