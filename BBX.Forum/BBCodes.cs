using System.Data;
using System.Text.RegularExpressions;
using Discuz.Common;

namespace Discuz.Forum
{
    public class BBCodes
    {
        //public static DataTable GetBBCode()
        //{
        //    return Discuz.Data.BBCodes.GetBBCode();
        //}

        public static DataTable GetBBCode(int id)
        {
            if (id <= 0)
            {
                return new DataTable();
            }
            return Discuz.Data.BBCodes.GetBBCode(id);
        }

        public static void UpdateBBCode(int available, string tag, string icon, string replacement, string example, string explanation, string param, string nest, string paramsDescription, string paramsDefaultValue, int id)
        {
            if (id > 0)
            {
                tag = Regex.Replace(tag.Replace("<", "").Replace(">", ""), "^[\\>]|[\\{]|[\\}]|[\\[]|[\\]]|[\\']|[\\.]", "");
                Discuz.Data.BBCodes.UpdateBBCode(available, tag, icon, replacement, example, explanation, param, nest, paramsDescription, paramsDefaultValue, id);
                Caches.ReSetCustomEditButtonList();
            }
        }

        public static void DeleteBBCode(string idList)
        {
            if (Utils.IsNumericList(idList))
            {
                //Discuz.Data.BBCodes.DeleteBBCode(idList);
                Caches.ReSetCustomEditButtonList();
            }
        }

        public static void BatchUpdateAvailable(int status, string idList)
        {
            if (Utils.IsNumericList(idList))
            {
                Discuz.Data.BBCodes.BatchUpdateAvailable(status, idList);
                Caches.ReSetCustomEditButtonList();
            }
        }

        public static void CreateBBCCode(int available, string tag, string icon, string replacement, string example, string explanation, string param, string nest, string paramsDescript, string paramsDefvalue)
        {
            Discuz.Data.BBCodes.CreateBBCCode(available, tag, icon, replacement, example, explanation, param, nest, paramsDescript, paramsDefvalue);
            Caches.ReSetCustomEditButtonList();
        }
    }
}