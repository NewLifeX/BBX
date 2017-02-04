using System;
using System.Web;
using NewLife.Web;

namespace BBX.Common
{
    public class DNTRequest
    {
        public static bool IsPost()
        {
            return HttpContext.Current.Request.HttpMethod.Equals("POST");
        }

        //public static bool IsGet()
        //{
        //    return HttpContext.Current.Request.HttpMethod.Equals("GET");
        //}

        //public static string GetServerString(string strName)
        //{
        //    if (HttpContext.Current.Request.ServerVariables[strName] == null)
        //    {
        //        return "";
        //    }
        //    return HttpContext.Current.Request.ServerVariables[strName].ToString();
        //}

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
            if (HttpContext.Current.Request.QueryString[strName] == null)
            {
                return "";
            }
            if (sqlSafeCheck && !Utils.IsSafeSqlString(HttpContext.Current.Request.QueryString[strName]))
            {
                return "unsafe string";
            }
            return HttpContext.Current.Request.QueryString[strName];
        }

        public static string GetPageName()
        {
            string[] array = HttpContext.Current.Request.Url.AbsolutePath.Split('/');
            return array[array.Length - 1].ToLower();
        }

        ///// <summary>参数个数，包括GET/POST</summary>
        ///// <returns></returns>
        //public static int GetParamCount()
        //{
        //    return HttpContext.Current.Request.Form.Count + HttpContext.Current.Request.QueryString.Count;
        //}

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

        //public static int GetQueryInt(string strName)
        //{
        //    return Utils.StrToInt(HttpContext.Current.Request.QueryString[strName], 0);
        //}

        public static int GetQueryInt(string strName, int defValue)
        {
            return Utils.StrToInt(HttpContext.Current.Request.QueryString[strName], defValue);
        }

        public static int GetFormInt(string strName, int defValue)
        {
            return Utils.StrToInt(HttpContext.Current.Request.Form[strName], defValue);
        }

        public static int GetInt(string strName, int defValue = 0)
        {
            if (GetQueryInt(strName, defValue) == defValue)
            {
                return GetFormInt(strName, defValue);
            }
            return GetQueryInt(strName, defValue);
        }

        //public static float GetQueryFloat(string strName, float defValue)
        //{
        //    return Utils.StrToFloat(HttpContext.Current.Request.QueryString[strName], defValue);
        //}

        //public static float GetFormFloat(string strName, float defValue)
        //{
        //    return Utils.StrToFloat(HttpContext.Current.Request.Form[strName], defValue);
        //}

        //public static float GetFloat(string strName, float defValue)
        //{
        //    if (GetQueryFloat(strName, defValue) == defValue)
        //    {
        //        return GetFormFloat(strName, defValue);
        //    }
        //    return GetQueryFloat(strName, defValue);
        //}

        //public static string GetIP()
        //{
        //    //string text = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        //    //if (string.IsNullOrEmpty(text))
        //    //{
        //    //    text = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        //    //}
        //    //if (string.IsNullOrEmpty(text))
        //    //{
        //    //    text = HttpContext.Current.Request.UserHostAddress;
        //    //}
        //    //if (string.IsNullOrEmpty(text) || !Utils.IsIP(text))
        //    //{
        //    //    return "127.0.0.1";
        //    //}
        //    //return text;
        //    return WebHelper.UserHost;
        //}

        //public static void SaveRequestFile(string path)
        //{
        //    if (HttpContext.Current.Request.Files.Count > 0)
        //    {
        //        HttpContext.Current.Request.Files[0].SaveAs(path);
        //    }
        //}
    }
}