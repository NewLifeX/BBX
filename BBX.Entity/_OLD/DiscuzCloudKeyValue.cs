using System;
using System.Text;
using Discuz.Common;

namespace Discuz.Entity
{
    public class DiscuzCloudKeyValue : IComparable
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
                    return DiscuzCloudKeyValue.ConvertArrayToString(this.value as Array);
                }
                return this.value.ToString();
            }
        }

        protected DiscuzCloudKeyValue(string name, object value)
        {
            this.name = name;
            this.value = value;
        }

        public override string ToString()
        {
            return string.Format("{0}={1}", this.Name, Utils.UrlEncode(this.Value));
        }

        public static DiscuzCloudKeyValue Create(string name, object value)
        {
            return new DiscuzCloudKeyValue(name, value);
        }

        public int CompareTo(object obj)
        {
            if (!(obj is DiscuzCloudKeyValue))
            {
                return -1;
            }
            char[] array = this.name.ToCharArray();
            char[] array2 = (obj as DiscuzCloudKeyValue).name.ToCharArray();
            int num = (array.Length > array2.Length) ? array2.Length : array.Length;
            int num2 = -1;
            while (++num2 < num)
            {
                if (array[num2] != array2[num2])
                {
                    if (array[num2] >= array2[num2])
                    {
                        return 1;
                    }
                    return -1;
                }
            }
            return -1;
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