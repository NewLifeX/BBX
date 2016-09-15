using Newtonsoft.Json;

namespace Discuz.Entity
{
    public class OAuthAccessTokenInfo : OAuthTokenInfo
    {
        private string _openid;

        [JsonProperty("openid")]
        public string Openid { get { return _openid; } set { _openid = value; } }
    }
}