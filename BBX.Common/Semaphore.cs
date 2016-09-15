using System;
using System.Threading;

namespace Discuz.Common
{
    public class Semaphore
    {
        private int _count;
        private object _semLock = new object();

        public Semaphore()
            : this(1)
        {
        }

        public Semaphore(int count)
        {
            if (count < 0)
            {
                throw new ArgumentException("Semaphore must have a count of at least 0.", "count");
            }
            this._count = count;
        }

        public void AddOne()
        {
            this.V();
        }

        public void WaitOne()
        {
            this.P();
        }

        public void P()
        {
            lock(this._semLock)
            {
                while (this._count <= 0)
                {
                    Monitor.Wait(this._semLock, -1);
                }
                this._count--;
            }
        }

        public void V()
        {
            lock(this._semLock)
            {
                this._count++;
                Monitor.Pulse(this._semLock);
            }
        }

        public void Reset(int count)
        {
            lock(this._semLock)
            {
                this._count = count;
            }
        }
    }
}