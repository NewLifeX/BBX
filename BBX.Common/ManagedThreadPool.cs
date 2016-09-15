using System;
using System.Collections;
using System.Threading;

namespace Discuz.Common
{
    public class ManagedThreadPool
    {
        private class WaitingCallback
        {
            
            private WaitCallback _callback;
            public WaitCallback Callback { get { return _callback; } } 

            private object _state;
            public object State { get { return _state; } } 

            public WaitingCallback(WaitCallback callback, object state)
            {
                this._callback = callback;
                this._state = state;
            }
        }

        private const int _maxWorkerThreads = 10;
        private static Queue _waitingCallbacks;
        private static Semaphore _workerThreadNeeded;
        private static ArrayList _workerThreads;
        private static int _inUseThreads;
        private static object _poolLock;

        public static int MaxThreads { get { return 10; } } 

        public static int ActiveThreads { get { return _inUseThreads; } } 

        public static int WaitingCallbacks
        {
            get
            {
                int count;
                lock(_poolLock)
                {
                    count = _waitingCallbacks.Count;
                }
                return count;
            }
        }

        static ManagedThreadPool()
        {
            _poolLock = new object();
            Initialize();
        }

        private static void Initialize()
        {
            _waitingCallbacks = new Queue();
            _workerThreads = new ArrayList();
            _inUseThreads = 0;
            _workerThreadNeeded = new Semaphore(0);
            for (int i = 0; i < 10; i++)
            {
                Thread thread = new Thread(new ThreadStart(ProcessQueuedItems));
                _workerThreads.Add(thread);
                thread.Name = "ManagedPoolThread #" + i.ToString();
                thread.IsBackground = true;
                thread.Start();
            }
        }

        public static void QueueUserWorkItem(WaitCallback callback)
        {
            QueueUserWorkItem(callback, null);
        }

        public static void QueueUserWorkItem(WaitCallback callback, object state)
        {
            WaitingCallback obj = new WaitingCallback(callback, state);
            lock(_poolLock)
            {
                _waitingCallbacks.Enqueue(obj);
            }
            _workerThreadNeeded.AddOne();
        }

        public static void Reset()
        {
            lock(_poolLock)
            {
                try
                {
                    foreach (object current in _waitingCallbacks)
                    {
                        WaitingCallback waitingCallback = (WaitingCallback)current;
                        if (waitingCallback.State is IDisposable)
                        {
                            ((IDisposable)waitingCallback.State).Dispose();
                        }
                    }
                }
                catch
                {
                }
                try
                {
                    foreach (Thread thread in _workerThreads)
                    {
                        if (thread != null)
                        {
                            thread.Abort("reset");
                        }
                    }
                }
                catch
                {
                }
                Initialize();
            }
        }

        private static void ProcessQueuedItems()
        {
            while (true)
            {
                _workerThreadNeeded.WaitOne();
                WaitingCallback waitingCallback = null;
                lock(_poolLock)
                {
                    if (_waitingCallbacks.Count > 0)
                    {
                        try
                        {
                            waitingCallback = (WaitingCallback)_waitingCallbacks.Dequeue();
                        }
                        catch
                        {
                        }
                    }
                }
                if (waitingCallback != null)
                {
                    try
                    {
                        Interlocked.Increment(ref _inUseThreads);
                        waitingCallback.Callback(waitingCallback.State);
                    }
                    catch
                    {
                    }
                    finally
                    {
                        Interlocked.Decrement(ref _inUseThreads);
                    }
                }
            }
        }
    }
}