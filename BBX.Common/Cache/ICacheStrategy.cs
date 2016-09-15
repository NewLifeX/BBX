namespace BBX.Cache
{
    /// <summary>缓存策略接口</summary>
    interface ICacheStrategy
    {
        /// <summary>默认缓存超时时间</summary>
        int TimeOut { get; set; }

        void AddObject(string objId, object o);

        /// <summary>添加缓存对象，并指定缓存时间</summary>
        /// <param name="objId"></param>
        /// <param name="o"></param>
        /// <param name="expire">缓存时间，秒</param>
        void AddObject(string objId, object o, int expire);

        void AddObjectWithFileChange(string objId, object o, string[] files);

        void AddObjectWithDepend(string objId, object o, string[] dependKey);

        void RemoveObject(string objId);

        object RetrieveObject(string objId);

        void FlushAll();
    }
}