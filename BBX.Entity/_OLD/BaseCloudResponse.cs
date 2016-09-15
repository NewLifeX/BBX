using Newtonsoft.Json;

namespace Discuz.Entity
{
    public class BaseCloudResponse<T>
    {
        
        private int errCode;
        [JsonProperty("errCode")]
        public int ErrCode { get { return errCode; } set { errCode = value; } }

        private string errMessage;
        [JsonProperty("errMessage")]
        public string ErrMessage { get { return errMessage; } set { errMessage = value; } }

        private T result;
        [JsonProperty("result")]
        public T Result { get { return result; } set { result = value; } }
    }
}