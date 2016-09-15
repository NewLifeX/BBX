using System;
using NewLife.Reflection;

namespace BBX.Plugin.Mall
{
    /// <summary>交易插件提供者</summary>
    public class MallPluginProvider
    {
        private static MallPluginBase _sp;

        private MallPluginProvider() { }

        static MallPluginProvider()
        {
            //try
            //{
            //    _sp = (MallPluginBase)Activator.CreateInstance(Type.GetType("BBX.Mall.MallPlugin, BBX.Mall", false, true));
            //}
            //catch
            //{
            //    _sp = null;
            //}
            foreach (var item in AssemblyX.FindAllPlugins(typeof(MallPluginBase), true))
            {
                _sp = TypeX.CreateInstance(item) as MallPluginBase;
                break;
            }
        }

        public static MallPluginBase GetInstance()
        {
            return _sp;
        }
    }
}