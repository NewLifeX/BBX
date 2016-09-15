using System.Data;
using Discuz.Cache;
using Discuz.Common;

namespace Discuz.Forum
{
    public class BanWords
    {
        public static DataTable GetBanWordList()
        {
            return Discuz.Data.BanWords.GetBanWordList();
        }

        public static int UpdateBanWord(int id, string find, string replacement)
        {
            if (id <= 0 || !(find != replacement))
            {
                return 0;
            }
            return Discuz.Data.BanWords.UpdateBanWord(id, find, replacement);
        }

        public static int DeleteBanWords(string idList)
        {
            if (Utils.IsNumericList(idList))
            {
                int result = Discuz.Data.BanWords.DeleteBanWords(idList);
                XCache.Remove(CacheKeys.FORUM_BAN_WORD_LIST);
                return result;
            }
            return 0;
        }

        public static bool IsExistBanWord(string banWord)
        {
            banWord = banWord.Trim('|');
            foreach (DataRow dataRow in BanWords.GetBanWordList().Rows)
            {
                if (dataRow["find"].ToString().Trim() == banWord)
                {
                    return true;
                }
            }
            return false;
        }

        public static int CreateBanWord(string adminUserName, string find, string replacement)
        {
            if (find != replacement)
            {
                XCache.Remove(CacheKeys.FORUM_BAN_WORD_LIST);
                return Discuz.Data.BanWords.CreateBanWord(adminUserName, find.Trim('|'), replacement);
            }
            return 0;
        }

        public static void UpdateBadWords(string find, string replacement)
        {
            if (find != replacement)
            {
                Discuz.Data.BanWords.UpdateBadWords(find.Trim('|'), replacement);
            }
        }

        public static string ConvertRegexCode(string originalCode)
        {
            string[] array = new string[]
			{
				"\\",
				"+",
				"*",
				"?",
				"[",
				"^",
				"]",
				"$",
				"(",
				")",
				"=",
				"!",
				"<",
				">",
				"|",
				":"
			};
            string[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                string text = array2[i];
                originalCode = originalCode.Replace(text, '\\' + text);
            }
            return originalCode;
        }
    }
}