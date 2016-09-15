using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Discuz.Common
{
    public class DiscuzOAuth
    {
        private Random random = new Random();

        public string GetOAuthUrl(string url, string httpMethod, string customAppId, string customAppSecret, string tokenKey, string tokenSecrect, string verify, string callbackUrl, List<DiscuzOAuthParameter> parameters, out string queryString)
        {
            parameters.Add(new DiscuzOAuthParameter("oauth_nonce", this.GenerateNonce()));
            parameters.Add(new DiscuzOAuthParameter("oauth_timestamp", this.GenerateTimeStamp()));
            parameters.Add(new DiscuzOAuthParameter("oauth_signature_method", "HMAC_SHA1"));
            parameters.Add(new DiscuzOAuthParameter("oauth_consumer_key", customAppId));
            if (!string.IsNullOrEmpty(tokenKey))
            {
                parameters.Add(new DiscuzOAuthParameter("oauth_token", tokenKey));
            }
            if (!string.IsNullOrEmpty(verify))
            {
                parameters.Add(new DiscuzOAuthParameter("oauth_verifier", verify));
            }
            if (!string.IsNullOrEmpty(callbackUrl))
            {
                parameters.Add(new DiscuzOAuthParameter("oauth_callback", callbackUrl));
            }
            string text = this.NormalizeRequestParameters(parameters);
            string text2 = url;
            if (!string.IsNullOrEmpty(text))
            {
                text2 = text2 + "?" + text;
            }
            Uri url2 = new Uri(text2);
            string result = null;
            string str = this.GenerateSignature(url2, customAppSecret, tokenSecrect, httpMethod, parameters, out result);
            queryString = text + "&response_format=json&oauth_signature=" + str;
            return result;
        }

        private string GenerateSignature(Uri url, string consumerSecret, string tokenSecret, string httpMethod, List<DiscuzOAuthParameter> parameters, out string normalizedUrl)
        {
            string signatureBase = this.GenerateSignatureBase(url, httpMethod, parameters, out normalizedUrl);
            return this.GenerateSignatureUsingHash(signatureBase, new HMACSHA1
            {
                Key = Encoding.ASCII.GetBytes(string.Format("{0}&{1}", consumerSecret, string.IsNullOrEmpty(tokenSecret) ? "" : tokenSecret))
            });
        }

        private string GenerateSignatureBase(Uri url, string httpMethod, List<DiscuzOAuthParameter> parameters, out string normalizedUrl)
        {
            normalizedUrl = null;
            parameters.Sort(new ParameterComparer());
            normalizedUrl = string.Format("{0}://{1}", url.Scheme, url.Authority);
            normalizedUrl += url.AbsolutePath;
            string oriString = this.FormEncodeParameters(parameters);
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("{0}&{1}&{2}", httpMethod.ToUpper(), Utils.PHPUrlEncode(normalizedUrl), Utils.PHPUrlEncode(oriString));
            return stringBuilder.ToString();
        }

        private string FormEncodeParameters(List<DiscuzOAuthParameter> parameters)
        {
            List<DiscuzOAuthParameter> list = new List<DiscuzOAuthParameter>();
            foreach (DiscuzOAuthParameter current in parameters)
            {
                if (current.Name.IndexOf("oauth_") != -1)
                {
                    list.Add(new DiscuzOAuthParameter(current.Name, Utils.PHPUrlEncode(current.Value)));
                }
            }
            return this.NormalizeRequestParameters(list);
        }

        private string GenerateSignatureUsingHash(string signatureBase, HashAlgorithm hash)
        {
            return this.ComputeHash(hash, signatureBase);
        }

        private string ComputeHash(HashAlgorithm hashAlgorithm, string data)
        {
            if (hashAlgorithm == null)
            {
                throw new ArgumentNullException("hashAlgorithm");
            }
            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentNullException("data");
            }
            byte[] bytes = Encoding.ASCII.GetBytes(data);
            byte[] array = hashAlgorithm.ComputeHash(bytes);
            StringBuilder stringBuilder = new StringBuilder();
            byte[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                byte b = array2[i];
                stringBuilder.Append(((b < 16) ? "0" : "") + Convert.ToString(b, 16));
            }
            return stringBuilder.ToString();
        }

        private string NormalizeRequestParameters(List<DiscuzOAuthParameter> parameters)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < parameters.Count; i++)
            {
                DiscuzOAuthParameter discuzOAuthParameter = parameters[i];
                stringBuilder.AppendFormat("{0}={1}", discuzOAuthParameter.Name, Utils.PHPUrlEncode(discuzOAuthParameter.Value));
                if (i < parameters.Count - 1)
                {
                    stringBuilder.Append("&");
                }
            }
            return stringBuilder.ToString();
        }

        private string GenerateTimeStamp()
        {
            return Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds).ToString();
        }

        private string GenerateNonce()
        {
            return this.random.Next(123400, 9999999).ToString();
        }
    }

    public class ParameterComparer : IComparer<DiscuzOAuthParameter>
    {
        public int Compare(DiscuzOAuthParameter x, DiscuzOAuthParameter y)
        {
            if (!(x.Name == y.Name))
            {
                return string.Compare(x.Name, y.Name);
            }
            return string.Compare(x.Value, y.Value);
        }
    }

    public class DiscuzOAuthParameter
    {
        
        private string name;
        public string Name { get { return name; } } 

        private string value;
        public string Value { get { return value; } } 

        public DiscuzOAuthParameter(string name, string value)
        {
            this.name = name;
            this.value = value;
        }
    }
}