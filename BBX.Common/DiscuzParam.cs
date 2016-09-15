using System;
using System.Text;
using System.Web;

namespace Discuz.Common
{
    public class DiscuzParam : IComparable
    {
        private object value;

        private string name;
        public string Name { get { return name; } } 

        public string Value
        {
            get
            {
                if (this.value is Array)
                {
                    return ConvertArrayToString(this.value as Array);
                }
                return this.value.ToString();
            }
        }

        public string EncodedValue
        {
            get
            {
                if (this.value is Array)
                {
                    return HttpUtility.UrlEncode(ConvertArrayToString(this.value as Array));
                }
                return HttpUtility.UrlEncode(this.value.ToString());
            }
        }

        protected DiscuzParam(string name, object value)
        {
            this.name = name;
            this.value = value;
        }

        public override string ToString()
        {
            return string.Format("{0}={1}", this.Name, this.Value);
        }

        public string ToEncodedString()
        {
            return string.Format("{0}={1}", this.Name, this.EncodedValue);
        }

        public static DiscuzParam Create(string name, object value)
        {
            return new DiscuzParam(name, value);
        }

        public int CompareTo(object obj)
        {
            if (!(obj is DiscuzParam))
            {
                return -1;
            }
            return this.name.CompareTo((obj as DiscuzParam).name);
        }

        private static string ConvertArrayToString(Array a)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < a.Length; i++)
            {
                if (i > 0)
                {
                    stringBuilder.Append(",");
                }
                stringBuilder.Append(a.GetValue(i).ToString());
            }
            return stringBuilder.ToString();
        }
    }
}