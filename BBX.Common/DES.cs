using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BBX.Common
{
    /// <summary>DES加解密算法，固定初始化向量IV</summary>
    public class DES
    {
        /// <summary>固定了的初始化向量IV</summary>
        private static byte[] Keys = new byte[] { 18, 52, 86, 120, 144, 171, 205, 239 };

        /// <summary>加密，密码固定为8字符</summary>
        /// <param name="encryptString"></param>
        /// <param name="encryptKey"></param>
        /// <returns></returns>
        public static string Encode(string encryptString, string encryptKey)
        {
            encryptKey = Utils.GetSubString(encryptKey, 8, "");
            encryptKey = encryptKey.PadRight(8, ' ');

            byte[] bytes = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
            byte[] keys = DES.Keys;
            byte[] bytes2 = Encoding.UTF8.GetBytes(encryptString);

            var ms = new MemoryStream();
            var cs = new CryptoStream(ms, new DESCryptoServiceProvider().CreateEncryptor(bytes, keys), CryptoStreamMode.Write);
            cs.Write(bytes2, 0, bytes2.Length);
            cs.FlushFinalBlock();
            return Convert.ToBase64String(ms.ToArray());
        }

        /// <summary>解码，密码固定为8字符</summary>
        /// <param name="decryptString"></param>
        /// <param name="decryptKey"></param>
        /// <returns></returns>
        public static string Decode(string decryptString, string decryptKey)
        {
            if (decryptString == string.Empty) return string.Empty;

            try
            {
                decryptKey = Utils.GetSubString(decryptKey, 8, "");
                decryptKey = decryptKey.PadRight(8, ' ');

                byte[] bytes = Encoding.UTF8.GetBytes(decryptKey);
                byte[] keys = DES.Keys;
                byte[] array = Convert.FromBase64String(decryptString);

                var ms = new MemoryStream();
                var cs = new CryptoStream(ms, new DESCryptoServiceProvider().CreateDecryptor(bytes, keys), CryptoStreamMode.Write);
                cs.Write(array, 0, array.Length);
                cs.FlushFinalBlock();
                return Encoding.UTF8.GetString(ms.ToArray());
            }
            catch
            {
                return "";
            }
        }
    }
}