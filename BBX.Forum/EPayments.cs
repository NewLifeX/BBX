using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using BBX.Common;
using BBX.Config;

namespace BBX.Forum
{
    public class EPayments
    {
        public static bool CheckPayment(string notifyId)
        {
            var config = GeneralConfigInfo.Current;
            string arg = (config.Alipaypartnerid != "") ? config.Alipaypartnerid : "2088002872555901";
            string arg2 = "https://www.alipay.com/cooperate/gateway.do?service=notify_verify&";
            string arg3 = "http://notify.alipay.com/trade/notify_query.do?";
            string http = EPayments.GetHttp(string.Format("{0}partner={1}&notify_id={2}", arg2, arg, notifyId));
            if (http != "true")
            {
                http = EPayments.GetHttp(string.Format("{0}partner={1}&notify_id={2}", arg3, arg, notifyId));
            }
            if (http != "true") return false;
            if (HttpContext.Current.Request.HttpMethod.Equals("GET")) return true;

            string[] allKeys = HttpContext.Current.Request.Form.AllKeys;
            var sb = new StringBuilder();
            //if (config.Usealipaycustompartnerid == 0)
            //{
            //    string url = "http://pay.newlifex.com/gateway/alipay.ashx?_type=alipay&_action=verify&_product=" + Utils.Version + "&_version=" + Utils.Version;
            //    for (int i = 0; i < allKeys.Length; i++)
            //    {
            //        if (DNTRequest.GetString(allKeys[i]) != "")
            //        {
            //            if (stringBuilder.ToString() == "")
            //            {
            //                stringBuilder.Append(allKeys[i] + "=" + Utils.UrlEncode(DNTRequest.GetString(allKeys[i])));
            //            }
            //            else
            //            {
            //                stringBuilder.Append("&" + allKeys[i] + "=" + Utils.UrlEncode(DNTRequest.GetString(allKeys[i])));
            //            }
            //        }
            //    }
            //    return EPayments.GetHttp(url, stringBuilder.ToString()) == "true";
            //}
            for (int j = 0; j < allKeys.Length; j++)
            {
                if (DNTRequest.GetString(allKeys[j]) != "" && allKeys[j] != "sign" && allKeys[j] != "sign_type")
                {
                    if (String.IsNullOrEmpty(sb.ToString()))
                    {
                        sb.Append(allKeys[j] + "=" + DNTRequest.GetString(allKeys[j]));
                    }
                    else
                    {
                        sb.Append("&" + allKeys[j] + "=" + DNTRequest.GetString(allKeys[j]));
                    }
                }
            }
            sb.Append(config.Alipaypartnercheckkey);
            return EPayments.GetMD5(sb.ToString(), "utf-8") == DNTRequest.GetString("sign");
        }
        public static int ConvertAlipayTradeStatus(string alipayStatus)
        {
            int result = 0;
            if (alipayStatus != null)
            {
                if (!(alipayStatus == "WAIT_BUYER_PAY"))
                {
                    if (!(alipayStatus == "TRADE_FINISHED"))
                    {
                        if (alipayStatus == "TRADE_SUCCESS")
                        {
                            result = 2;
                        }
                    }
                    else
                    {
                        result = 2;
                    }
                }
                else
                {
                    result = 0;
                }
            }
            return result;
        }
        public static string GetMD5(string str, string inputCharset)
        {
            MD5 mD = new MD5CryptoServiceProvider();
            byte[] array = mD.ComputeHash(Encoding.GetEncoding(inputCharset).GetBytes(str));
            var sb = new StringBuilder(32);
            for (int i = 0; i < array.Length; i++)
            {
                sb.Append(array[i].ToString("x").PadLeft(2, '0'));
            }
            return sb.ToString();
        }
        public static bool IsOpenEPayments()
        {
            bool result = true;
            var config = GeneralConfigInfo.Current;
            if (config.Cashtocreditrate <= 0)
            {
                result = false;
            }
            if (string.IsNullOrEmpty(config.Alipayaccout) && string.IsNullOrEmpty(config.Tenpayaccout))
            {
                result = false;
            }
            return result;
        }
        public static string GetHttp(string url)
        {
            return EPayments.GetHttp(url, "");
        }
        public static string GetHttp(string url, string postData)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentLength = (long)postData.Length;
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            httpWebRequest.Timeout = 120000;
            HttpWebResponse httpWebResponse = null;
            string result;
            try
            {
                var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream());
                streamWriter.Write(postData);
                if (streamWriter != null)
                {
                    streamWriter.Close();
                }
                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.Default))
                {
                    result = streamReader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                result = "错误：" + ex.Message;
            }
            finally
            {
                if (httpWebResponse != null)
                {
                    httpWebResponse.Close();
                }
            }
            return result;
        }
    }
}