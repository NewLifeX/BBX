using System.Linq;
using System.Text.RegularExpressions;
using BBX.Cache;
using BBX.Entity;

namespace BBX.Forum
{
    public class Smilies
    {
        public static Regex[] regexSmile;

        static Smilies()
        {
            InitRegexSmilies();
        }

        public static void InitRegexSmilies()
        {
            var list = GetSmiliesListWithInfo();
            for (int i = list.Length - 1; i >= 1; i--)
            {
                for (int j = 0; j <= i - 1; j++)
                {
                    if (list[j].Code.Length < list[j + 1].Code.Length)
                    {
                        var smiliesInfo = list[j];
                        list[j] = list[j + 1];
                        list[j + 1] = smiliesInfo;
                    }
                }
            }
            regexSmile = new Regex[list.Length];
            for (int k = 0; k < list.Length; k++)
            {
                regexSmile[k] = new Regex(Regex.Escape(list[k].Code), RegexOptions.None);
            }
        }

        public static void ResetRegexSmilies(Smilie[] smiliesList)
        {
            int num = smiliesList.Length;
            if (regexSmile == null || regexSmile.Length != num)
            {
                regexSmile = new Regex[num];
            }
            for (int i = 0; i < num; i++)
            {
                regexSmile[i] = new Regex(Regex.Escape(smiliesList[i].Code), RegexOptions.None);
            }
        }

        public static Smilie[] GetSmiliesListWithInfo()
        {
            var cacheService = XCache.Current;
            var array = cacheService.RetrieveObject(CacheKeys.FORUM_UI_SMILIES_LIST_WITH_INFO) as Smilie[];
            if (array == null)
            {
                //array = BBX.Data.GetSmiliesListWithoutType();
                array = Smilie.FindAllWithCache().ToList().Where(e => e.Type != 0).ToArray();
                XCache.Add(CacheKeys.FORUM_UI_SMILIES_LIST_WITH_INFO, array);
                ResetRegexSmilies(array);
            }
            return array;
        }

        //public static SmiliesInfo GetSmiliesTypeById(int smiliesId)
        //{
        //    var smiliesTypesInfo = BBX.Data.GetSmiliesTypesInfo();
        //    var array = smiliesTypesInfo;
        //    for (int i = 0; i < array.Length; i++)
        //    {
        //        var smiliesInfo = array[i];
        //        if (smiliesInfo.Id == smiliesId)
        //        {
        //            return smiliesInfo;
        //        }
        //    }
        //    return null;
        //}
        //public static SmiliesInfo GetSmiliesById(int smiliesId)
        //{
        //    var smiliesListWithInfo = GetSmiliesListWithInfo();
        //    var array = smiliesListWithInfo;
        //    for (int i = 0; i < array.Length; i++)
        //    {
        //        var smiliesInfo = array[i];
        //        if (smiliesInfo.Id == smiliesId)
        //        {
        //            return smiliesInfo;
        //        }
        //    }
        //    return null;
        //}
        //public static DataTable GetSmiliesTypes()
        //{
        //    return BBX.Data.GetSmiliesTypes();
        //}
        //public static DataTable GetSmilieByType(int typeId)
        //{
        //    if (typeId <= 0)
        //    {
        //        return new DataTable();
        //    }
        //    return BBX.Data.GetSmiliesInfoByType(typeId);
        //}
        //public static string ClearEmptySmiliesType()
        //{
        //    string text = "";
        //    DataTable smiliesTypes = BBX.Data.GetSmiliesTypes();
        //    foreach (DataRow dataRow in smiliesTypes.Rows)
        //    {
        //        if (BBX.Data.GetSmiliesInfoByType(int.Parse(dataRow["id"].ToString())).Rows.Count == 0)
        //        {
        //            text = text + dataRow["code"].ToString() + ",";
        //            BBX.Data.DeleteSmilies(dataRow["id"].ToString());
        //        }
        //    }
        //    return text.TrimEnd(',');
        //}
        //public static int GetMaxSmiliesId()
        //{
        //    return BBX.Data.GetMaxSmiliesId();
        //}
        //public static DataTable GetSmilies()
        //{
        //    return BBX.Data.GetSmilies();
        //}
        public static bool IsExistSameSmilieCode(string code, int currentid)
        {
            //foreach (DataRow dataRow in BBX.Data.GetSmiliesListDataTable().Rows)
            //{
            //    if (dataRow["code"].ToString() == code && dataRow["id"].ToString() != currentid.ToString())
            //    {
            //        return true;
            //    }
            //}
            //return false;

            var sm = Smilie.FindByCode(code);
            return sm != null && sm.ID != currentid;
        }
        //public static void DeleteSmilyByType(int type)
        //{
        //    if (type > 0)
        //    {
        //        BBX.Data.DeleteSmilyByType(type);
        //    }
        //}
    }
}