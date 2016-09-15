using System;
using System.IO;
using BBX.Common;
using NewLife.Xml;

namespace BBX.Config
{
    /// <summary>Xml配置文件基类</summary>
    /// <typeparam name="TConfig"></typeparam>
    public class XmlConfig2<TConfig> : XmlConfig<TConfig> where TConfig : XmlConfig2<TConfig>, new()
    {
        static XmlConfig2()
        {
            // 修正配置文件路径，加上论坛目录，因为论坛可能不是在顶级
            var file = _.ConfigFile;
            if (!file.IsNullOrWhiteSpace())
            {
                if (typeof(TConfig) != typeof(BaseConfigInfo)) file = Utils.GetMapPath(BaseConfigs.GetForumPath + _.ConfigFile);
                if (!File.Exists(file)) file = _.ConfigFile.GetFullPath();
                _.ConfigFile = file;
            }
        }
    }
}