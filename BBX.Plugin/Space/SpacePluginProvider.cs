using System;
using NewLife.Reflection;

namespace Discuz.Plugin.Space
{
    public class SpacePluginProvider
    {
        private static SpacePluginBase _sp;

        private SpacePluginProvider() { }

        static SpacePluginProvider()
        {
            //try
            //{
            //    _sp = (SpacePluginBase)Activator.CreateInstance(Type.GetType("Discuz.Space.SpacePlugin, Discuz.Space", false, true));
            //}
            //catch
            //{
            //    _sp = null;
            //}
            foreach (var item in AssemblyX.FindAllPlugins(typeof(SpacePluginBase), true))
            {
                _sp = TypeX.CreateInstance(item) as SpacePluginBase;
                break;
            }
        }

        public static SpacePluginBase GetInstance()
        {
            return _sp;
        }
    }
}