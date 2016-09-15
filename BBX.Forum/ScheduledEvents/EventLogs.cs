using System;
using System.IO;
using System.Text;

namespace BBX.Forum.ScheduledEvents
{
    public sealed class EventLogs
    {
        public static string LogFileName = string.Empty;
        public static void WriteFailedLog(string logContent)
        {
            var sb = new StringBuilder();
            sb.Append(DateTime.Now);
            sb.Append("\t");
            sb.Append(Environment.MachineName);
            sb.Append("\t");
            sb.Append(logContent);
            sb.Append("\r\n");
            using (FileStream fileStream = new FileStream(EventLogs.LogFileName, FileMode.Append, FileAccess.Write, FileShare.Write))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(sb.ToString());
                fileStream.Write(bytes, 0, bytes.Length);
                fileStream.Close();
            }
        }
    }
}