using System;

namespace BBX.Cache
{
    /// <summary>X缓存</summary>
    public class XCache
    {
        private static ICacheStrategy cs = new DefaultCacheStrategy();
        private static object lockHelper = new object();

        private static volatile XCache instance = null;
        /// <summary>当前缓存实现</summary>
        public static XCache Current
        {
            get
            {
                if (instance == null)
                {
                    lock (lockHelper)
                    {
                        if (instance == null) instance = new XCache();
                    }
                }
                return instance;
            }
        }

        //public virtual void AddObject(string xpath, object o)
        //{
        //    lock (lockHelper)
        //    {
        //        cs.AddObject(xpath, o);
        //    }
        //}

        public virtual void AddObject(string xpath, object o, int expire)
        {
            lock (lockHelper)
            {
                cs.AddObject(xpath, o, expire);
            }
        }

        public virtual void AddObject(string xpath, object o, string[] files)
        {
            xpath = xpath.Replace(" ", "_SPACE_");
            lock (lockHelper)
            {
                cs.AddObject(xpath, o);
            }
        }

        public virtual object RetrieveObject(string xpath)
        {
            try
            {
                return cs.RetrieveObject(xpath);
            }
            catch
            {
                return null;
            }
        }

        public virtual void RemoveObject(string xpath)
        {
            lock (lockHelper)
            {
                try
                {
                    cs.RemoveObject(xpath);
                }
                catch { }
            }
        }

        //public void LoadDefaultCacheStrategy()
        //{
        //    lock (lockHelper)
        //    {
        //        cs = new DefaultCacheStrategy();
        //    }
        //}

        public void FlushAll() { cs.FlushAll(); }

        #region 静态
        /// <summary>添加缓存</summary>
        /// <param name="xpath"></param>
        /// <param name="value"></param>
        /// <param name="expire">过期时间，秒</param>
        public static void Add(String xpath, Object value, Int32 expire = 0)
        {
            Current.AddObject(xpath, value, expire);
        }

        /// <summary>添加缓存</summary>
        /// <param name="xpath"></param>
        /// <param name="value"></param>
        /// <param name="files"></param>
        public static void Add(String xpath, Object value, params String[] files)
        {
            Current.AddObject(xpath, value, files);
        }

        /// <summary>获取缓存</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public static T Retrieve<T>(String xpath)
        {
            return (T)Current.RetrieveObject(xpath);
        }

        /// <summary>删除缓存</summary>
        /// <param name="xpaths"></param>
        public static void Remove(params String[] xpaths)
        {
            foreach (var item in xpaths)
            {
                Current.RemoveObject(item);
            }
        }
        #endregion
    }
}