using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    public static class Helper
    {
        public static String ToFullStr(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}