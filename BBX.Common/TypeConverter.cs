using System;
using System.Text.RegularExpressions;

namespace BBX.Common
{
    public class TypeConverter
    {
        //public static bool StrToBool(object expression, bool defValue)
        //{
        //    if (expression != null)
        //    {
        //        return TypeConverter.StrToBool(expression, defValue);
        //    }
        //    return defValue;
        //}

        //public static bool StrToBool(string expression, bool defValue)
        //{
        //    if (expression != null)
        //    {
        //        if (string.Compare(expression, "true", true) == 0)
        //        {
        //            return true;
        //        }
        //        if (string.Compare(expression, "false", true) == 0)
        //        {
        //            return false;
        //        }
        //    }
        //    return defValue;
        //}

        //public static int ObjectToInt(object expression)
        //{
        //    return TypeConverter.ObjectToInt(expression, 0);
        //}

        //public static int ObjectToInt(object expression, int defValue)
        //{
        //    if (expression != null)
        //    {
        //        return TypeConverter.StrToInt(expression.ToString(), defValue);
        //    }
        //    return defValue;
        //}

        public static int StrToInt(string str)
        {
            return TypeConverter.StrToInt(str, 0);
        }

        public static int StrToInt(string str, int defValue)
        {
            if (str.EqualIgnoreCase("True")) return 1;
            if (str.EqualIgnoreCase("False")) return 0;

            if (string.IsNullOrEmpty(str) || str.Trim().Length >= 11 || !Regex.IsMatch(str.Trim(), "^([-]|[0-9])[0-9]*(\\.\\w*)?$"))
            {
                return defValue;
            }
            int result;
            if (int.TryParse(str, out result)) return result;

            return (Int32)str.ToDouble((Double)defValue);
        }

        //public static float StrToFloat(object strValue, float defValue)
        //{
        //    if (strValue == null)
        //    {
        //        return defValue;
        //    }
        //    return TypeConverter.StrToFloat(strValue.ToString(), defValue);
        //}

        //public static float ObjectToFloat(object strValue, float defValue)
        //{
        //    if (strValue == null)
        //    {
        //        return defValue;
        //    }
        //    return TypeConverter.StrToFloat(strValue.ToString(), defValue);
        //}

        //public static float ObjectToFloat(object strValue)
        //{
        //    return TypeConverter.ObjectToFloat(strValue.ToString(), 0f);
        //}

        //public static float StrToFloat(string strValue)
        //{
        //    if (strValue == null)
        //    {
        //        return 0f;
        //    }
        //    return TypeConverter.StrToFloat(strValue.ToString(), 0f);
        //}

        //public static float StrToFloat(string strValue, float defValue)
        //{
        //    if (strValue == null || strValue.Length > 10)
        //    {
        //        return defValue;
        //    }
        //    float result = defValue;
        //    if (strValue != null)
        //    {
        //        bool flag = Regex.IsMatch(strValue, "^([-]|[0-9])[0-9]*(\\.\\w*)?$");
        //        if (flag)
        //        {
        //            float.TryParse(strValue, out result);
        //        }
        //    }
        //    return result;
        //}

        //public static DateTime StrToDateTime(string str, DateTime defValue)
        //{
        //    DateTime result;
        //    if (!string.IsNullOrEmpty(str) && DateTime.TryParse(str, out result))
        //    {
        //        return result;
        //    }
        //    return defValue;
        //}

        //public static DateTime StrToDateTime(string str)
        //{
        //    return TypeConverter.StrToDateTime(str, DateTime.Now);
        //}

        //public static DateTime ObjectToDateTime(object obj)
        //{
        //    return TypeConverter.StrToDateTime(obj.ToString());
        //}

        //public static DateTime ObjectToDateTime(object obj, DateTime defValue)
        //{
        //    return TypeConverter.StrToDateTime(obj.ToString(), defValue);
        //}

        //public static int[] StringToIntArray(string idList)
        //{
        //    return TypeConverter.StringToIntArray(idList, -1);
        //}

        //public static int[] StringToIntArray(string idList, int defValue)
        //{
        //    if (string.IsNullOrEmpty(idList))
        //    {
        //        return null;
        //    }
        //    string[] array = Utils.SplitString(idList, ",");
        //    int[] array2 = new int[array.Length];
        //    for (int i = 0; i < array.Length; i++)
        //    {
        //        array2[i] = array[i], defValue.ToInt();
        //    }
        //    return array2;
        //}

        //public static bool IntStringToBoolean(string str)
        //{
        //    return str.Trim() == "1";
        //}

        //public static string BooleanToIntString(bool b)
        //{
        //    return b ? "1" : "0";
        //}
    }
}