using Discuz.Common;
using System;
using System.Text;

namespace Discuz.Forum
{
    public class MyMethodes
    {
        //public static string AuthCode(string str, string operation, string key, int expiry)
        //{
        //    int num = 4;
        //    key = Utils.MD5(key);
        //    string str2 = Utils.MD5(key.Substring(0, 16));
        //    string str3 = Utils.MD5(key.Substring(16, 16));
        //    string text = Utils.MD5(MicroTime());
        //    string text2 = (num > 0) ? (operation.Equals("DECODE") ? str.Substring(0, num) : text.Substring(text.Length - num)) : string.Empty;
        //    string text3 = str2 + Utils.MD5(str2 + text2);
        //    int length = text3.Length;
        //    str = (operation.Equals("DECODE") ? Base64Decode(str.Substring(num)) : (((expiry > 0) ? ((long)expiry + Time()) : 0L).ToString("D10") + Utils.MD5(str + str3).Substring(0, 16) + str));
        //    int length2 = str.Length;
        //    string text4 = string.Empty;
        //    int[] array = new int[256];
        //    for (int i = 0; i < 256; i++)
        //    {
        //        array[i] = i;
        //    }
        //    int[] array2 = new int[256];
        //    for (int j = 0; j < 256; j++)
        //    {
        //        array2[j] = (int)text3[j % length];
        //    }
        //    int k = 0;
        //    int num2 = 0;
        //    while (k < 256)
        //    {
        //        num2 = (num2 + array[k] + array2[k]) % 256;
        //        int num3 = array[k];
        //        array[k] = array[num2];
        //        array[num2] = num3;
        //        k++;
        //    }
        //    byte[] array3 = new byte[length2];
        //    int num4 = 0;
        //    int num5 = 0;
        //    for (int l = 0; l < length2; l++)
        //    {
        //        num4 = (num4 + 1) % 256;
        //        num5 = (num5 + array[num4]) % 256;
        //        int num6 = array[num4];
        //        array[num4] = array[num5];
        //        array[num5] = num6;
        //        array3[l] = (byte)((int)str[l] ^ array[(array[num4] + array[num5]) % 256]);
        //    }
        //    text4 = Encoding.UTF8.GetString(array3);
        //    if (!operation.Equals("DECODE"))
        //    {
        //        return text2 + Base64Encode(array3).Replace("=", string.Empty);
        //    }
        //    if ((long.Parse(text4.Substring(0, 10)) == 0L || long.Parse(text4.Substring(0, 10)) > Time()) && text4.Substring(10, 16).Equals(Utils.MD5(text4.Substring(26) + str3).Substring(0, 16)))
        //    {
        //        return text4.Substring(26);
        //    }
        //    return string.Empty;
        //}
        public static long Time()
        {
            DateTime dateTime = new DateTime(1970, 1, 1);
            return (DateTime.UtcNow.Ticks - dateTime.Ticks) / 10000000L;
        }
        //private static string MicroTime()
        //{
        //    long num = Time();
        //    string str = "0." + DateTime.UtcNow.Millisecond.ToString().PadRight(8, '0');
        //    return str + " " + num.ToString();
        //}
        //public static string Base64Encode(byte[] bytes)
        //{
        //    return Convert.ToBase64String(bytes);
        //}
        //public static string Base64Encode(string thisEncode)
        //{
        //    string result = "";
        //    try
        //    {
        //        byte[] bytes = Encoding.UTF8.GetBytes(thisEncode);
        //        result = Convert.ToBase64String(bytes);
        //    }
        //    catch
        //    {
        //        result = thisEncode;
        //    }
        //    return result;
        //}
        //public static string Base64Decode(string code)
        //{
        //    while (code.Length % 4 != 0)
        //    {
        //        code += "=";
        //    }
        //    string text = "";
        //    byte[] array = Convert.FromBase64String(code);
        //    byte[] array2 = array;
        //    for (int i = 0; i < array2.Length; i++)
        //    {
        //        byte b = array2[i];
        //        text += (char)b;
        //    }
        //    return text;
        //}
        //public static string EncodeCode(string str)
        //{
        //    string text = string.Empty;
        //    string text2 = "_-.1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        //    for (int i = 0; i < str.Length; i++)
        //    {
        //        string text3 = str.Substring(i, 1);
        //        if (text2.Contains(text3))
        //        {
        //            text += text3;
        //        }
        //        else
        //        {
        //            byte[] bytes = Encoding.UTF8.GetBytes(text3);
        //            byte[] array = bytes;
        //            for (int j = 0; j < array.Length; j++)
        //            {
        //                byte b = array[j];
        //                text = text + "%" + b.ToString("X");
        //            }
        //        }
        //    }
        //    return text;
        //}
    }
}
