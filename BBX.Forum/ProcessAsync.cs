using BBX.Common;
using System;
using System.Threading;

namespace BBX.Forum
{
    /// <summary>“Ï≤Ω¥¶¿Ì</summary>
    public class ProcessAsync
    {
        protected string _url;

        public ProcessAsync(string url)
        {
            this._url = url;
        }

        public void Enqueue()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(this.Process));
        }

        private void Process(object state)
        {
            try
            {
                Utils.GetHttpWebResponse(this._url);
            }
            catch { }
        }
    }
}