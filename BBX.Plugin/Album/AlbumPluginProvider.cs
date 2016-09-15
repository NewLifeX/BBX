using System;
using NewLife.Reflection;

namespace Discuz.Plugin.Album
{
    public class AlbumPluginProvider
    {
        private static AlbumPluginBase _sp;

        private AlbumPluginProvider() { }

        static AlbumPluginProvider()
        {
            //try
            //{
            //    _sp = (AlbumPluginBase)Activator.CreateInstance(Type.GetType("Discuz.Album.AlbumPlugin, Discuz.Album", false, true));
            //}
            //catch
            //{
            //    _sp = null;
            //}

            foreach (var item in AssemblyX.FindAllPlugins(typeof(AlbumPluginBase), true))
            {
                _sp = TypeX.CreateInstance(item) as AlbumPluginBase;
                break;
            }
        }

        public static AlbumPluginBase GetInstance()
        {
            return _sp;
        }
    }
}