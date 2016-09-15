using System.Text.RegularExpressions;

namespace BBX.Common
{
    public class Validator
    {
        public static bool IsNumeric(object expression)
        {
            return expression != null && Validator.IsNumeric(expression.ToString());
        }

        public static bool IsNumeric(string expression)
        {
            return expression != null && expression.Length > 0 && expression.Length <= 11 && Regex.IsMatch(expression, "^[-]?[0-9]*[.]?[0-9]*$") && (expression.Length < 10 || (expression.Length == 10 && expression[0] == '1') || (expression.Length == 11 && expression[0] == '-' && expression[1] == '1'));
        }

        public static bool IsDouble(object expression)
        {
            return expression != null && Regex.IsMatch(expression.ToString(), "^([0-9])[0-9]*(\\.\\w*)?$");
        }

        public static bool IsNumericArray(string[] strNumber)
        {
            if (strNumber == null)
            {
                return false;
            }
            if (strNumber.Length < 1)
            {
                return false;
            }
            for (int i = 0; i < strNumber.Length; i++)
            {
                string expression = strNumber[i];
                if (!Validator.IsNumeric(expression))
                {
                    return false;
                }
            }
            return true;
        }
    }
}