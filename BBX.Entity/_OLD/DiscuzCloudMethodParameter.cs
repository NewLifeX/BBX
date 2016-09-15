using System.Collections.Generic;
using System.Text;
using Discuz.Common;

namespace Discuz.Entity
{
    public class DiscuzCloudMethodParameter
    {
        public const string PARAMKEYTEMP = "args%5B{0}%5D";
        private List<DiscuzCloudKeyValue> _paramList;
        private bool _inArgs = true;

        public DiscuzCloudMethodParameter()
        {
            this._paramList = new List<DiscuzCloudKeyValue>();
        }

        public DiscuzCloudMethodParameter(bool inArgs)
        {
            this._inArgs = inArgs;
            this._paramList = new List<DiscuzCloudKeyValue>();
        }

        public void Add(string key, string value)
        {
            this._paramList.Add(DiscuzCloudKeyValue.Create(key, value));
        }

        public string Find(string key)
        {
            foreach (DiscuzCloudKeyValue current in this._paramList)
            {
                if (key == current.Name)
                {
                    return current.Value;
                }
            }
            return null;
        }

        public string GetPostData()
        {
            StringBuilder stringBuilder = new StringBuilder();
            this._paramList.Sort();
            foreach (DiscuzCloudKeyValue current in this._paramList)
            {
                stringBuilder.AppendFormat("&{0}={1}", this._inArgs ? string.Format("args%5B{0}%5D", current.Name) : current.Name, Utils.PHPUrlEncode(current.Value));
            }
            return stringBuilder.ToString();
        }
    }
}