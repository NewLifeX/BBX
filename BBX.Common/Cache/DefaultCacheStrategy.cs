using System;
using System.Web;
using System.Web.Caching;

using WebCache = System.Web.Caching.Cache;

namespace BBX.Cache
{
    class DefaultCacheStrategy : ICacheStrategy
    {
        protected static volatile WebCache webCache = HttpRuntime.Cache;

        private int _timeOut = 3600;
        /// <summary>默认缓存超时时间</summary>
        public virtual int TimeOut
        {
            get
            {
                if (_timeOut <= 0) return 3600;

                return _timeOut;
            }
            set { _timeOut = value > 0 ? value : 3600; }
        }

        //public static WebCache GetWebCacheObj { get { return webCache; } }

        public virtual void AddObject(string objId, object o)
        {
            if (objId == null || objId.Length == 0 || o == null) return;

            if (TimeOut >= 7200)
            {
                webCache.Insert(objId, o, null, DateTime.MaxValue, TimeSpan.Zero, CacheItemPriority.High, null);
                return;
            }
            webCache.Insert(objId, o, null, DateTime.Now.AddSeconds((double)TimeOut), WebCache.NoSlidingExpiration, CacheItemPriority.High, null);
        }

        /// <summary>添加缓存对象，并指定缓存时间</summary>
        /// <param name="objId"></param>
        /// <param name="o"></param>
        /// <param name="expire">缓存时间，秒</param>
        public virtual void AddObject(string objId, object o, int expire)
        {
            if (objId == null || objId.Length == 0 || o == null) return;

            if (expire == 0)
                webCache.Insert(objId, o, null, DateTime.MaxValue, TimeSpan.Zero, CacheItemPriority.High, null);
            else
                webCache.Insert(objId, o, null, DateTime.Now.AddSeconds((double)expire), WebCache.NoSlidingExpiration, CacheItemPriority.High, null);
        }

        public virtual void AddObjectWithFileChange(string objId, object o, string[] files)
        {
            if (objId == null || objId.Length == 0 || o == null) return;

            var dependencies = new CacheDependency(files, DateTime.Now);
            webCache.Insert(objId, o, dependencies, DateTime.Now.AddSeconds((double)TimeOut), WebCache.NoSlidingExpiration, CacheItemPriority.High, onRemove);
        }

        public virtual void AddObjectWithDepend(string objId, object o, string[] dependKey)
        {
            if (objId == null || objId.Length == 0 || o == null) return;

            var dependencies = new CacheDependency(null, dependKey, DateTime.Now);
            webCache.Insert(objId, o, dependencies, DateTime.Now.AddSeconds((double)TimeOut), WebCache.NoSlidingExpiration, CacheItemPriority.High, onRemove);
        }

        public void onRemove(string key, object val, CacheItemRemovedReason reason)
        {
            switch (reason)
            {
                case CacheItemRemovedReason.Removed:
                case CacheItemRemovedReason.Expired:
                case CacheItemRemovedReason.Underused:
                case CacheItemRemovedReason.DependencyChanged:
                    return;
            }
        }

        public virtual void RemoveObject(string objId)
        {
            if (objId == null || objId.Length == 0) return;

            webCache.Remove(objId);
        }

        public virtual object RetrieveObject(string objId)
        {
            if (objId == null || objId.Length == 0) return null;

            return webCache.Get(objId);
        }

        public virtual void FlushAll()
        {
            //var enumerator = HttpRuntime.Cache.GetEnumerator();
            //while (enumerator.MoveNext())
            //{
            //    webCache.Remove(enumerator.Key.ToString());
            //}
            foreach (var item in HttpRuntime.Cache)
            {
                webCache.Remove("" + item);
            }
        }
    }
}