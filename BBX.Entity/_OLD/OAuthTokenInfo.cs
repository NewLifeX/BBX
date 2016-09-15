using Newtonsoft.Json;

namespace Discuz.Entity
{
    public class OAuthTokenInfo
    {
        
        private string _token;
        [JsonProperty("oauth_token")]
        public string Token { get { return _token; } set { _token = value; } }

        private string _secret;
        [JsonProperty("oauth_token_secret")]
        public string Secret { get { return _secret; } set { _secret = value; } }
    }
}