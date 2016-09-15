using System;
using Newtonsoft.Json;

namespace Discuz.Entity
{
    public class AttachPaymentlogInfo
    {
        
        private int _id;
        [JsonProperty("id")]
        public int Id { get { return _id; } set { _id = value; } }

        private int _uid;
        [JsonProperty("uid")]
        public int Uid { get { return _uid; } set { _uid = value; } }

        private string _username;
        [JsonProperty("username")]
        public string UserName { get { return _username; } set { _username = value; } }

        private int _aid;
        [JsonProperty("aid")]
        public int Aid { get { return _aid; } set { _aid = value; } }

        private int _authorid;
        [JsonProperty("authorid")]
        public int Authorid { get { return _authorid; } set { _authorid = value; } }

        [JsonProperty("postdatetime")]
        public string PostDateTimeString { get { return _postdatetime.ToString("yyyy-MM-dd HH:mm:ss"); } } 

        private DateTime _postdatetime;
        [JsonIgnore]
        public DateTime PostDateTime { get { return _postdatetime; } set { _postdatetime = value; } }

        private int _amount;
        [JsonProperty("amount")]
        public int Amount { get { return _amount; } set { _amount = value; } }

        private int _netamount;
        [JsonProperty("netamount")]
        public int NetAmount { get { return _netamount; } set { _netamount = value; } }
    }
}