namespace BBX.Config
{
    /// <summary>基本配置</summary>
    /// <remarks>因为引用的地方太多了，为了保持一定的兼容性，暂时不废弃。推荐使用<see cref="BaseConfigInfo.Current"/></remarks>
    public class BaseConfigs
    {
        //public static string GetDBConnectString { get { return BaseConfigInfo.Current.Dbconnectstring; } }

        //public static string GetTablePrefix { get { return BaseConfigInfo.Current.Tableprefix; } }

        public static int GetFounderUid { get { return BaseConfigInfo.Current.Founderuid; } }

        public static string GetForumPath { get { return BaseConfigInfo.Current.Forumpath; } }

        //public static string GetDbType { get { return BaseConfigInfo.Current.Dbtype; } }
    }
}