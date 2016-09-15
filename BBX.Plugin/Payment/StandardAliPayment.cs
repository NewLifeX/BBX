using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using BBX.Common;
using BBX.Config;
using BBX.Plugin.Payment.Alipay;

namespace BBX.Plugin.Payment
{
    public class StandardAliPayment : IPayment
    {
        private static volatile StandardAliPayment instance = null;
        private static object lockHelper = new object();
        private string _aliPay = "https://www.alipay.com/cooperate/gateway.do?";

        public string PayUrl { get { return _aliPay; } set { _aliPay = value; } }

        private StandardAliPayment()
        {
        }

        public static StandardAliPayment GetService()
        {
            if (instance == null)
            {
                lock(lockHelper)
                {
                    if (instance == null)
                    {
                        instance = new StandardAliPayment();
                    }
                }
            }
            return instance;
        }

        public static int Partition(string[] strArray, int low, int high)
        {
            string text = strArray[low];
            while (low < high)
            {
                while (low < high && string.CompareOrdinal(strArray[high], text) >= 0)
                {
                    high--;
                }
                Swap(ref strArray[high], ref strArray[low]);
                while (low < high && string.CompareOrdinal(strArray[low], text) <= 0)
                {
                    low++;
                }
                Swap(ref strArray[high], ref strArray[low]);
            }
            strArray[low] = text;
            return low;
        }

        public static void Swap(ref string i, ref string j)
        {
            string text = i;
            i = j;
            j = text;
        }

        public static void QuickSort(string[] strArray, int low, int high)
        {
            if (low <= high - 1)
            {
                int num = Partition(strArray, low, high);
                QuickSort(strArray, low, num - 1);
                QuickSort(strArray, num + 1, high);
            }
        }

        public static string GetMD5(string s, string _input_charset)
        {
            MD5 mD = new MD5CryptoServiceProvider();
            byte[] array = mD.ComputeHash(Encoding.GetEncoding(_input_charset).GetBytes(s));
            StringBuilder stringBuilder = new StringBuilder(32);
            for (int i = 0; i < array.Length; i++)
            {
                stringBuilder.Append(array[i].ToString("x").PadLeft(2, '0'));
            }
            return stringBuilder.ToString();
        }

        public string CreateDigitalGoodsTradeUrl(ITrade _goods)
        {
            DigitalTrade digitalTrade = (DigitalTrade)_goods;
            string[] urlParam = GetUrlParam(digitalTrade);
            QuickSort(urlParam, 0, urlParam.Length - 1);
            string str = CreateTradeUrl(urlParam);
            string text = CreateEncodeUrl(urlParam);
            return string.Format("{0}{1}&sign={2}&sign_type={3}", new object[]
			{
				this.PayUrl,
				text,
				GetMD5(str + GeneralConfigInfo.Current.Alipaypartnercheckkey, "utf-8"),
				digitalTrade.Sign_Type
			});
        }

        public string CreateNormalGoodsTradeUrl(ITrade _goods)
        {
            DigitalTrade digitalTrade = (NormalTrade)_goods;
            string[] urlParam = GetUrlParam(digitalTrade);
            QuickSort(urlParam, 0, urlParam.Length - 1);
            CreateTradeUrl(urlParam);
            string str = CreateEncodeUrl(urlParam);
            return this.PayUrl + str + string.Format("&sign_type={0}", digitalTrade.Sign_Type);
        }

        private static string CreateTradeUrl(string[] strArray)
        {
            string text = "";
            for (int i = 0; i < strArray.Length; i++)
            {
                string text2 = strArray[i];
                if (string.IsNullOrEmpty(text))
                {
                    text = text2;
                }
                else
                {
                    text = text + "&" + text2;
                }
            }
            return text;
        }

        private static string CreateEncodeUrl(string[] strArray)
        {
            string text = "";
            for (int i = 0; i < strArray.Length; i++)
            {
                string text2 = strArray[i];
                if (string.IsNullOrEmpty (text))
                {
                    text = text2.Split('=')[0] + "=" + Utils.UrlEncode(text2.Split('=')[1]);
                }
                else
                {
                    text = text + "&" + text2.Split('=')[0] + "=" + Utils.UrlEncode(text2.Split('=')[1]);
                }
            }
            return text;
        }

        private static string[] GetUrlParam(object obj)
        {
            PropertyInfo[] properties = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            string text = "";
            PropertyInfo[] array = properties;
            for (int i = 0; i < array.Length; i++)
            {
                PropertyInfo propertyInfo = array[i];
                if (propertyInfo.GetValue(obj, null) != null && !(propertyInfo.Name == "Sign") && !(propertyInfo.Name == "Sign_Type") && !(propertyInfo.Name == "_Action") && !(propertyInfo.Name == "_Product") && !(propertyInfo.Name == "_Version") && !(propertyInfo.Name == "_Type"))
                {
                    if (propertyInfo.Name == "Logistics_Info")
                    {
                        LogisticsInfo[] logistics_Info = ((NormalTrade)obj).Logistics_Info;
                        int num = 0;
                        LogisticsInfo[] array2 = logistics_Info;
                        for (int j = 0; j < array2.Length; j++)
                        {
                            LogisticsInfo logisticsInfo = array2[j];
                            if (logisticsInfo.Logistics_Type != "")
                            {
                                string text2 = "";
                                if (num > 0)
                                {
                                    text2 = "_" + num;
                                }
                                object obj2 = text;
                                text = obj2 + "logistics_type" + text2 + "=" + logisticsInfo.Logistics_Type + "&logistics_fee" + text2 + "=" + logisticsInfo.Logistics_Fee + "&logistics_payment" + text2 + "=" + logisticsInfo.Logistics_Payment + "&";
                                num++;
                            }
                        }
                    }
                    else
                    {
                        string text3 = text;
                        text = text3 + propertyInfo.Name.ToLower().Replace("input_charset", "_input_charset") + "=" + propertyInfo.GetValue(obj, null).ToString() + "&";
                    }
                }
            }
            if (text.EndsWith("&"))
            {
                text = text.Substring(0, text.Length - 1);
            }
            return text.Split('&');
        }
    }
}