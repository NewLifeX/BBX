using Newtonsoft.Json;

namespace Discuz.Entity
{
    public class ConnectResponse<T>
    {
        
        private int _status;
        [JsonProperty("status")]
        public int Status { get { return _status; } set { _status = value; } }

        private T _result;
        [JsonProperty("result")]
        public T Result { get { return _result; } set { _result = value; } }
    }
}