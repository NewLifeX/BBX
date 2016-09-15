using Newtonsoft.Json;

namespace Discuz.Entity
{
    public class RegisterCloud
    {
        
        private string cloudSiteId;
        [JsonProperty("sId")]
        public string CloudSiteId { get { return cloudSiteId; } set { cloudSiteId = value; } }

        private string cloudSiteKey;
        [JsonProperty("sKey")]
        public string CloudSiteKey { get { return cloudSiteKey; } set { cloudSiteKey = value; } }
    }
}