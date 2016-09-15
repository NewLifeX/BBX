using System.IO;
using BBX.Common;
using BBX.Config;

namespace BBX.Plugin.Preview
{
    public class PreviewHelper
    {
        public static string GetPreviewCachePhysicalPath()
        {
            string strPath = BaseConfigs.GetForumPath + "cache/plugin/preview/";
            return Utils.GetMapPath(strPath);
        }

        public static void CreateDirectory(string path)
        {
            Utils.CreateDir(path);
        }

        public static bool IsFileExist(string fileName)
        {
            return Utils.FileExists(fileName);
        }

        public static bool IsDirectoryExist(string path)
        {
            return Directory.Exists(path);
        }
    }
}