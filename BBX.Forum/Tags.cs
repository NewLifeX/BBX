using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using BBX.Common;

using BBX.Data;
using BBX.Entity;

namespace BBX.Forum
{
    public class Tags
    {
        public static TagInfo GetTagInfo(int tagid)
        {
            if (tagid <= 0)
            {
                return null;
            }
            return BBX.Data.Tags.GetTagInfo(tagid);
        }

        public static void WriteTagsCacheFile(string filename, List<TagInfo> tags, string jsonp_callback, bool outputcountfield)
        {
            if (tags.Count > 0)
            {
                if (!Directory.Exists(Path.GetDirectoryName(filename)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(filename));
                }
                StringBuilder stringBuilder = new StringBuilder();
                if (!Utils.StrIsNullOrEmpty(jsonp_callback))
                {
                    stringBuilder.Append(jsonp_callback);
                    stringBuilder.Append("(");
                }
                stringBuilder.Append("[\r\n  ");
                foreach (TagInfo current in tags)
                {
                    if (outputcountfield)
                    {
                        stringBuilder.Append(string.Format("{{'tagid' : '{0}', 'tagname' : '{1}', 'fcount' : '{2}', 'pcount' : '{3}', 'scount' : '{4}', 'vcount' : '{5}', 'gcount' : '{6}'}}, ", new object[]
						{
							current.Tagid,
							current.Tagname,
							current.Fcount,
							current.Pcount,
							current.Scount,
							current.Vcount,
							current.Gcount
						}));
                    }
                    else
                    {
                        stringBuilder.Append(string.Format("{{'tagid' : '{0}', 'tagname' : '{1}'}}, ", current.Tagid, current.Tagname));
                    }
                }
                stringBuilder.Append("\r\n]");
                if (!Utils.StrIsNullOrEmpty(jsonp_callback))
                {
                    stringBuilder.Append(")");
                }
                try
                {
                    using (FileStream fileStream = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        byte[] bytes = Encoding.UTF8.GetBytes(stringBuilder.ToString());
                        fileStream.Write(bytes, 0, bytes.Length);
                        fileStream.Close();
                    }
                }
                catch
                {
                }
            }
        }

        public static bool UpdateForumTags(int tagid, int orderid, string color)
        {
            Regex regex = new Regex("^#?([0-9|A-F]){6}$");
            if (color != "" && !regex.IsMatch(color))
            {
                return false;
            }
            BBX.Data.Tags.UpdateForumTags(tagid, orderid, color.Replace("#", ""));
            return true;
        }

        public static DataTable GetForumTags(string tagName, int type)
        {
            return BBX.Data.Tags.GetForumTags(tagName, type);
        }
    }
}