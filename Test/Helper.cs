using System;
using System.Collections.Generic;
using System.Text;

namespace Test
{
    static class Helper
    {
        public static String CutLeft(this String str, String cut)
        {
            if (str.IsNullOrWhiteSpace()) return str;

            if (!str.StartsWith(cut)) return str;

            return str.Substring(cut.Length);
        }

        public static String CutRight(this String str, String cut)
        {
            if (str.IsNullOrWhiteSpace()) return str;

            if (!str.EndsWith(cut)) return str;

            return str.Substring(0, str.Length - cut.Length);
        }

        /// <summary>如果结尾为某字符串，替换为另一个</summary>
        /// <param name="str"></param>
        /// <param name="p"></param>
        /// <param name="replace"></param>
        /// <returns></returns>
        public static String EnsureEnd(this String str, String p, String replace)
        {
            if (str.IsNullOrWhiteSpace()) return str;

            if (!str.EndsWith(p)) return str;

            return str.Substring(0, str.Length - p.Length) + replace;
        }

        /// <summary>确保某单词开头，并且下一个字母大写</summary>
        /// <param name="str"></param>
        /// <param name="p"></param>
        /// <param name="replace"></param>
        /// <returns></returns>
        public static String EnsureStart(this String str, String p, String replace)
        {
            if (str.IsNullOrWhiteSpace()) return str;

            if (!str.StartsWith(p))
            {
                if (!str.StartsWith(replace)) return str;

                p = replace;
            }

            if (str.Length > p.Length)
                return replace + str.Substring(p.Length, 1).ToUpper() + str.Substring(p.Length + 1);
            else
                return replace + str.Substring(p.Length);
        }
    }
}