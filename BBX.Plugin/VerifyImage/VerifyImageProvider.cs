using System;
using BBX.Config;

namespace BBX.Plugin.VerifyImage
{
    public static class VerifyImageProvider
    {
        private static IVerifyImage _Current;
        private static String _name;

        public static IVerifyImage Current
        {
            get
            {
                var config = GeneralConfigInfo.Current;
                if (_Current != null)
                {
                    if (_name == config.VerifyImageAssemly) return _Current;

                    _Current = null;
                }
                _name = config.VerifyImageAssemly;
                if (!_name.IsNullOrWhiteSpace())
                {
                    try
                    {
                        _Current = (IVerifyImage)Activator.CreateInstance(Type.GetType(string.Format("BBX.Plugin.VerifyImage.{0}.VerifyImage, BBX.Plugin.VerifyImage.{0}", _name), false, true));
                    }
                    catch { }
                }
                if (_Current == null) _Current = new BBX.Plugin.VerifyImage.JpegImage.VerifyImage();

                return _Current;
            }
        }

        //private static Dictionary<String, IVerifyImage> _instance = new Dictionary<string, IVerifyImage>(StringComparer.OrdinalIgnoreCase);
        //private static object lockHelper = new object();

        //public static IVerifyImage GetInstance(string assemlyName)
        //{
        //    if (!_instance.ContainsKey(assemlyName))
        //    {
        //        lock (lockHelper)
        //        {
        //            if (!_instance.ContainsKey(assemlyName))
        //            {
        //                IVerifyImage value = null;
        //                try
        //                {
        //                    value = (IVerifyImage)Activator.CreateInstance(Type.GetType(string.Format("BBX.Plugin.VerifyImage.{0}.VerifyImage, BBX.Plugin.VerifyImage.{0}", assemlyName), false, true));
        //                }
        //                catch
        //                {
        //                    value = new BBX.Plugin.VerifyImage.JpegImage.VerifyImage();
        //                }
        //                _instance.Add(assemlyName, value);
        //            }
        //        }
        //    }
        //    return _instance[assemlyName];
        //}
    }
}