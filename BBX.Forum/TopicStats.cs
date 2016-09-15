using System.Collections.Generic;
using System.Threading;
using BBX.Config;
using BBX.Entity;

namespace BBX.Forum
{
    public class TopicStats
    {
        private class ProcessStats
        {
            protected TopicViewCollection<TopicView> _tvc;

            public ProcessStats(TopicViewCollection<TopicView> tvc)
            {
                this._tvc = tvc;
            }

            public void Enqueue()
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.Process));
            }

            private void Process(object state)
            {
                TrackTopic(this._tvc);
            }
        }

        public class TopicViewCollection<T> : List<T> where T : TopicView
        {
            private int _viewCount;

            public int ViewCount { get { return _viewCount; } set { _viewCount = value; } }

            public TopicViewCollection()
            {
            }

            public TopicViewCollection(IEnumerable<T> collection)
                : base(collection)
            {
            }

            public TopicViewCollection(int capacity)
                : base(capacity)
            {
            }
        }

        private static TopicViewCollection<TopicView> queuedStatsList;
        private static int queuedAllowCount;

        private TopicStats() { }

        static TopicStats()
        {
            queuedStatsList = null;
            queuedAllowCount = 20;
            if (GeneralConfigInfo.Current.TopicQueueStats == 1)
            {
                SetQueueCount();
            }
        }

        static bool Track(TopicView tv)
        {
            if (tv == null) return false;

            if (queuedStatsList == null)
            {
                SetQueueCount();
            }
            if (GeneralConfigInfo.Current.TopicQueueStats == 1)
            {
                return AddQuedStats(tv);
            }
            //return TrackTopic(tv);
            Topic.UpdateViewCount(tv.TopicID, tv.ViewCount);
            return true;
        }

        public static bool Track(int tid, int viewcount)
        {
            return tid >= 1 && viewcount >= 1 && Track(new TopicView
            {
                TopicID = tid,
                ViewCount = viewcount
            });
        }

        //public static int UpdateTopicViewCount(int tid, int viewcount)
        //{
        //    return DatabaseProvider.GetInstance().UpdateTopicViewCount(tid, viewcount);
        //    //return BBX.Data.TopicStats.UpdateTopicViewCount(tid, viewcount);
        //}

        //public static bool TrackTopic(TopicView tv)
        //{
        //    return UpdateTopicViewCount(tv.TopicID, tv.ViewCount) == 1;
        //}

        public static void SetQueueCount()
        {
            if (GeneralConfigInfo.Current.TopicQueueStatsCount > 20 && GeneralConfigInfo.Current.TopicQueueStatsCount <= 1000)
            {
                queuedAllowCount = GeneralConfigInfo.Current.TopicQueueStatsCount;
            }
            if (queuedStatsList == null)
            {
                queuedStatsList = new TopicViewCollection<TopicView>();
            }
        }

        //public static bool ClearQueue(bool save)
        //{
        //	lock (queuedStatsList)
        //	{
        //		if (save)
        //		{
        //			TopicView[] array = new TopicView[queuedStatsList.Count];
        //			queuedStatsList.CopyTo(array, 0);
        //			ClearTrackTopicQueue(new TopicViewCollection<TopicView>(array));
        //		}
        //		queuedStatsList.Clear();
        //	}
        //	return true;
        //}

        static bool AddQuedStats(TopicView tv)
        {
            if (tv == null) return false;

            if (queuedAllowCount != GeneralConfigInfo.Current.TopicQueueStatsCount || queuedStatsList == null)
            {
                SetQueueCount();
            }
            lock (queuedStatsList)
            {
                if ((queuedStatsList.ViewCount >= queuedAllowCount || queuedStatsList.Count >= 5) && (queuedStatsList.ViewCount >= queuedAllowCount || queuedStatsList.Count >= 5))
                {
                    var array = new TopicView[queuedStatsList.Count];
                    queuedStatsList.CopyTo(array, 0);
                    ClearTrackTopicQueue(new TopicViewCollection<TopicView>(array));
                    queuedStatsList.Clear();
                    queuedStatsList.ViewCount = 0;
                }
                bool flag = false;
                foreach (TopicView current in queuedStatsList)
                {
                    if (current.TopicID == tv.TopicID)
                    {
                        current.ViewCount += tv.ViewCount;
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    queuedStatsList.Add(tv);
                }
            }
            queuedStatsList.ViewCount = queuedStatsList.ViewCount + 1;
            return true;
        }

        static bool ClearTrackTopicQueue(TopicViewCollection<TopicView> tvc)
        {
            new ProcessStats(tvc).Enqueue();
            return true;
        }

        static bool TrackTopic(TopicViewCollection<TopicView> tvc)
        {
            if (tvc == null)
            {
                return false;
            }
            foreach (var item in tvc)
            {
                //UpdateTopicViewCount(item.TopicID, item.ViewCount);
                Topic.UpdateViewCount(item.TopicID, item.ViewCount);
            }
            return true;
        }

        public static int GetStoredTopicViewCount(int tid)
        {
            if (queuedStatsList != null)
            {
                foreach (TopicView current in queuedStatsList)
                {
                    if (current != null && current.TopicID == tid)
                    {
                        return current.ViewCount;
                    }
                }
                return 0;
            }
            return 0;
        }
    }
}