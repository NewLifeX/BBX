using System;
using System.Web;

namespace BBX.Common
{
    public class DNTRequest
    {
        public static bool IsPost()
        {
            return HttpContext.Current.Request.HttpMethod.Equals("POST");
        }

        public static string GetUrlReferrer()
        {
            var context = HttpContext.Current;
            if (context == null) return String.Empty;

            var request = context.Request;
            if (request == null || request.UrlReferrer == null) return String.Empty;

            return request.UrlReferrer + "";
        }

        public static string GetCurrentFullHost()
        {
            HttpRequest request = HttpContext.Current.Request;
            if (!request.Url.IsDefaultPort)
            {
                return string.Format("{0}:{1}", request.Url.Host, request.Url.Port.ToString());
            }
            return request.Url.Host;
        }

        public static string GetQueryString(string strName)
        {
            return GetQueryString(strName, false);
        }

        public static string GetQueryString(string strName, bool sqlSafeCheck)
        {
            var req = HttpContext.Current?.Request;
            if (req == null) return "";

            var val = req.QueryString[strName];
            if (val.IsNullOrEmpty()) return "";

            if (sqlSafeCheck && !Utils.IsSafeSqlString(val)) return "unsafe string";

            return val;
        }

        public static string GetPageName()
        {
            string[] array = HttpContext.Current.Request.Url.AbsolutePath.Split('/');
            return array[array.Length - 1].ToLower();
        }

        public static string GetFormString(string strName)
        {
            return GetFormString(strName, false);
        }

        public static string GetFormString(string strName, bool sqlSafeCheck)
        {
            if (HttpContext.Current.Request.Form[strName] == null)
            {
                return "";
            }
            if (sqlSafeCheck && !Utils.IsSafeSqlString(HttpContext.Current.Request.Form[strName]))
            {
                return "unsafe string";
            }
            return HttpContext.Current.Request.Form[strName];
        }

        public static string GetString(string strName)
        {
            return GetString(strName, false);
        }

        public static string GetHtmlEncodeString(string strName)
        {
            return GetHtmlEncodeString(strName, false);
        }

        public static string GetHtmlEncodeString(string strName, bool isEncodeQuotationMark)
        {
            string text = Utils.HtmlEncode(GetString(strName, false));
            if (!isEncodeQuotationMark)
            {
                return text;
            }
            return text.Replace("'", "&acute;").Replace("\"", "&quot;");
        }

        public static string GetString(string strName, bool sqlSafeCheck)
        {
            if ("".Equals(GetQueryString(strName)))
            {
                return GetFormString(strName, sqlSafeCheck);
            }
            return GetQueryString(strName, sqlSafeCheck);
        }

        public static int GetQueryInt(string strName, int defValue)
        {
            return HttpContext.Current.Request.QueryString[strName].ToInt(defValue);
        }

        public static int GetFormInt(string strName, int defValue)
        {
            return HttpContext.Current.Request.Form[strName].ToInt(defValue);
        }

        public static int GetInt(string strName, int defValue = 0)
        {
            if (GetQueryInt(strName, defValue) == defValue)
            {
                return GetFormInt(strName, defValue);
            }
            return GetQueryInt(strName, defValue);
        }
    }
}