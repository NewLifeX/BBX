using System;
using System.Collections.Generic;
using System.IO;
using System.Timers;
using BBX.Common;

namespace BBX.Aggregation
{
    /// <summary>定时器每15秒检查一次更新</summary>
    public class AggregationDataSubject
    {
        public static DateTime fileoldchange;
        public static DateTime filenewchange;
        private static Timer _timer = new Timer(15000.0);
        private static object lockHelper = new object();
        private static List<AggregationData> _list = new List<AggregationData>();

        static AggregationDataSubject()
        {
            //_timer = new Timer(15000.0);
            //lockHelper = new object();
            //_list = new ArrayList();
            var file = AggregationData.DataFile;
            if (!Utils.FileExists(file))
            {
                var text = "<?xml version=\"1.0\" standalone=\"yes\"?>\r\n";
                text += "<remove>\r\n<table1 xpath=\"example\" removedatetime=\"" + DateTime.Now.ToShortDateString() + "\" />\r\n</remove>";
                //using (var fileStream = new FileStream(file, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                //{
                //    byte[] bytes = Encoding.UTF8.GetBytes(text);
                //    fileStream.Write(bytes, 0, bytes.Length);
                //    fileStream.Close();
                //}
                File.WriteAllText(file, text);
            }
            fileoldchange = File.GetLastWriteTime(file);
            _timer.AutoReset = true;
            _timer.Enabled = true;
            _timer.Elapsed += Timer_Elapsed;
            _timer.Start();
        }

        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (IsFileHadRewrite())
            {
                ReSetFileChangeTime();
                AggregationData.ReadAggregationConfig();
                NotifyClearDataBind();
            }
        }

        public static bool IsFileHadRewrite()
        {
            filenewchange = File.GetLastWriteTime(AggregationData.DataFile);
            if (fileoldchange != filenewchange)
            {
                lock (lockHelper)
                {
                    if (fileoldchange != filenewchange)
                    {
                        return true;
                    }
                }
                return false;
            }
            return false;
        }

        private static void ReSetFileChangeTime()
        {
            fileoldchange = filenewchange;
        }

        public static void Attach(AggregationData data)
        {
            _list.Add(data);
        }

        public static void Detach(AggregationData data)
        {
            _list.Remove(data);
        }

        public static void NotifyClearDataBind()
        {
            foreach (var item in _list)
            {
                item.ClearDataBind();
            }
        }
    }
}