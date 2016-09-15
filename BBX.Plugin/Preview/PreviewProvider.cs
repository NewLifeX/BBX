using System;
using System.Collections;
using System.Threading;

namespace BBX.Plugin.Preview
{
    public class PreviewProvider
    {
        private static Hashtable _instance = new Hashtable();
        private static object lockHelper = new object();

        public static IPreview GetInstance(string extname)
        {
            if (!_instance.ContainsKey(extname))
            {
                lock (lockHelper)
                {
                    if (!_instance.ContainsKey(extname))
                    {
                        IPreview value = null;
                        try
                        {
                            var name = string.Format("BBX.Plugin.Preview.{0}.Viewer, BBX.Plugin.Preview.{0}", extname);
                            var type = Type.GetType(name, false, true);
                            if (type != null) value = (IPreview)Activator.CreateInstance(type);
                        }
                        catch
                        {
                            value = null;
                        }
                        _instance.Add(extname, value);
                    }
                }
            }
            return (IPreview)_instance[extname];
        }
    }
}