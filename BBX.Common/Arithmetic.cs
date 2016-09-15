using System;
using System.Collections;

namespace Discuz.Common
{
    public class Arithmetic
    {
        public class ArrayListCopy
        {
            private ArrayListCopy()
            {
            }

            public static ArrayList CopyBewteenTo(ArrayList alist, int iLeft, int iRight)
            {
                ArrayList arrayList = new ArrayList();
                bool flag = false;
                for (int i = iLeft; i < iRight; i++)
                {
                    arrayList.Add(alist[i]);
                    flag = true;
                }
                if (flag)
                {
                    return arrayList;
                }
                return null;
            }

            public static ArrayList CopyNotBetweenTo(ArrayList alist, int iLeft, int iRight)
            {
                ArrayList arrayList = new ArrayList();
                bool flag = false;
                for (int i = 0; i < iLeft - 1; i++)
                {
                    arrayList.Add(alist[i]);
                    flag = true;
                }
                if (flag)
                {
                    arrayList.Add("@");
                    for (int j = iRight + 1; j < alist.Count; j++)
                    {
                        arrayList.Add(alist[j]);
                        flag = true;
                    }
                }
                if (flag)
                {
                    return arrayList;
                }
                return null;
            }

            public static int GetSubStringCount(string str, string sin)
            {
                int num = 0;
                int num2 = 0;
                while (true)
                {
                    num2 = str.IndexOf(sin, num2);
                    if (num2 <= 0)
                    {
                        break;
                    }
                    num2 += sin.Length;
                    num++;
                }
                return num;
            }
        }

        public class OperatorMap
        {
            public struct Map
            {
                public int Priority;
                public string Operator;

                public Map(int iPrior, string sOperator)
                {
                    this.Priority = iPrior;
                    this.Operator = sOperator;
                }
            }

            private OperatorMap()
            {
            }

            public static Arithmetic.OperatorMap.Map[] map()
            {
                Arithmetic.OperatorMap.Map[] array = new Arithmetic.OperatorMap.Map[]
				{
					new Arithmetic.OperatorMap.Map(5, "*"),
					new Arithmetic.OperatorMap.Map(5, "/"),
					default(Arithmetic.OperatorMap.Map),
					default(Arithmetic.OperatorMap.Map),
					default(Arithmetic.OperatorMap.Map),
					default(Arithmetic.OperatorMap.Map),
					default(Arithmetic.OperatorMap.Map),
					default(Arithmetic.OperatorMap.Map),
					default(Arithmetic.OperatorMap.Map),
					default(Arithmetic.OperatorMap.Map),
					default(Arithmetic.OperatorMap.Map),
					default(Arithmetic.OperatorMap.Map),
					default(Arithmetic.OperatorMap.Map),
					default(Arithmetic.OperatorMap.Map),
					default(Arithmetic.OperatorMap.Map),
					default(Arithmetic.OperatorMap.Map),
					default(Arithmetic.OperatorMap.Map),
					default(Arithmetic.OperatorMap.Map),
					default(Arithmetic.OperatorMap.Map),
					default(Arithmetic.OperatorMap.Map),
					default(Arithmetic.OperatorMap.Map),
					default(Arithmetic.OperatorMap.Map),
					default(Arithmetic.OperatorMap.Map),
					default(Arithmetic.OperatorMap.Map),
					default(Arithmetic.OperatorMap.Map),
					default(Arithmetic.OperatorMap.Map),
					default(Arithmetic.OperatorMap.Map),
					default(Arithmetic.OperatorMap.Map),
					default(Arithmetic.OperatorMap.Map),
					new Arithmetic.OperatorMap.Map(5, "%")
				};
                array[2] = new Arithmetic.OperatorMap.Map(10, "+");
                array[3] = new Arithmetic.OperatorMap.Map(10, "-");
                array[4] = new Arithmetic.OperatorMap.Map(20, ">");
                array[5] = new Arithmetic.OperatorMap.Map(20, ">=");
                array[6] = new Arithmetic.OperatorMap.Map(20, "<");
                array[7] = new Arithmetic.OperatorMap.Map(20, "<=");
                array[8] = new Arithmetic.OperatorMap.Map(20, "<>");
                array[9] = new Arithmetic.OperatorMap.Map(20, "!=");
                array[10] = new Arithmetic.OperatorMap.Map(20, "==");
                array[11] = new Arithmetic.OperatorMap.Map(20, "=");
                array[12] = new Arithmetic.OperatorMap.Map(41, "!");
                array[13] = new Arithmetic.OperatorMap.Map(42, "||");
                array[14] = new Arithmetic.OperatorMap.Map(43, "&&");
                array[15] = new Arithmetic.OperatorMap.Map(40, "++");
                array[16] = new Arithmetic.OperatorMap.Map(40, "--");
                array[17] = new Arithmetic.OperatorMap.Map(40, "+=");
                array[18] = new Arithmetic.OperatorMap.Map(40, "-=");
                array[19] = new Arithmetic.OperatorMap.Map(40, "*=");
                array[20] = new Arithmetic.OperatorMap.Map(40, "/=");
                array[21] = new Arithmetic.OperatorMap.Map(40, "&");
                array[22] = new Arithmetic.OperatorMap.Map(40, "|");
                array[23] = new Arithmetic.OperatorMap.Map(40, "&=");
                array[24] = new Arithmetic.OperatorMap.Map(40, "|=");
                array[25] = new Arithmetic.OperatorMap.Map(40, ">>");
                array[26] = new Arithmetic.OperatorMap.Map(40, "<<");
                array[27] = new Arithmetic.OperatorMap.Map(3, "(");
                array[28] = new Arithmetic.OperatorMap.Map(3, ")");
                return array;
            }

            public static bool CheckLeftBracket(string str)
            {
                return str == "(";
            }

            public static bool CheckRightBracket(string str)
            {
                return str == ")";
            }

            public static bool CheckBracket(string str)
            {
                return str == "(" || str == ")";
            }

            public static bool CheckOperator(string scheck)
            {
                string[] array = new string[]
				{
					"+",
					"-",
					"*",
					"/",
					"%",
					">",
					">=",
					"<",
					"<=",
					"<>",
					"!=",
					"==",
					"=",
					"!",
					"||",
					"&&",
					"++",
					"--",
					"+=",
					"-=",
					"*=",
					"/=",
					"&",
					"|",
					"&=",
					"|=",
					">>",
					"<<",
					")",
					"("
				};
                bool result = false;
                for (int i = 0; i < array.Length - 1; i++)
                {
                    if (array[i] == scheck)
                    {
                        result = true;
                        break;
                    }
                }
                return result;
            }

            public static Arithmetic.OperatorMap.Map GetMap(string Operator)
            {
                if (Arithmetic.OperatorMap.CheckOperator(Operator))
                {
                    Arithmetic.OperatorMap.Map[] array = Arithmetic.OperatorMap.map();
                    for (int i = 0; i < array.Length; i++)
                    {
                        Arithmetic.OperatorMap.Map result = array[i];
                        if (result.Operator == Operator)
                        {
                            return result;
                        }
                    }
                }
                return new Arithmetic.OperatorMap.Map(99, Operator);
            }

            public static int Getprior(string Operator)
            {
                return Arithmetic.OperatorMap.GetMap(Operator).Priority;
            }

            public static int GetMaxprior(string Loperator, string Roperator)
            {
                return Arithmetic.OperatorMap.GetMap(Loperator).Priority - Arithmetic.OperatorMap.GetMap(Roperator).Priority;
            }

            public static bool IsVar(string svar)
            {
                return (svar[0] >= '0' && svar[0] <= '9') || (svar[0] >= 'a' && svar[0] <= 'z') || (svar[0] >= 'A' && svar[0] <= 'Z');
            }
        }

        private Arithmetic()
        {
        }

        public static ArrayList ConvertExpression(string sExpression)
        {
            ArrayList arrayList = new ArrayList();
            string text = null;
            int i = 0;
            string text2 = "";
            while (i < sExpression.Length)
            {
                if (text != null && text != "" && text.Substring(0, 1) == "\"")
                {
                    while (true)
                    {
                        text2 = sExpression[i++].ToString();
                        if (text2 == "\"")
                        {
                            break;
                        }
                        text += text2;
                        text2 = null;
                        if (i >= sExpression.Length)
                        {
                            goto IL_82;
                        }
                    }
                    arrayList.Add(text + text2);
                    text2 = (text = null);
                }
            IL_82:
                if (i > sExpression.Length - 1)
                {
                    arrayList.Add(text);
                    arrayList.Add(text2);
                    break;
                }
                string key;
                if ((key = (text2 = sExpression[i++].ToString())) == null)
                {
                    goto IL_9DD;
                }

                switch (key)
                {
                    case "\"":

                        #region 引号
                        if (i > sExpression.Length - 1)
                        {
                            arrayList.Add(text);
                            arrayList.Add(text2);
                            text2 = (text = null);
                        }
                        else
                        {
                            text = text2;
                            while (true)
                            {
                                text2 = sExpression[i++].ToString();
                                if (text2 == "\"")
                                {
                                    break;
                                }
                                text += text2;
                                text2 = null;
                                if (i >= sExpression.Length)
                                {
                                    goto Block_11;
                                }
                            }
                            arrayList.Add(text + text2);
                            text2 = (text = null);
                        Block_11: ;
                        }
                        break;
                        #endregion

                    case "(":
                        arrayList.Add(text);
                        arrayList.Add(text2);
                        text2 = (text = null);
                        break;

                    case ")":
                        arrayList.Add(text);
                        arrayList.Add(text2);
                        text2 = (text = null);
                        break;

                    case " ":
                        arrayList.Add(text);
                        text2 = (text = null);
                        break;

                    case "+":

                        #region 加号
                        if (i > sExpression.Length - 1)
                        {
                            arrayList.Add(text);
                            arrayList.Add(text2);
                            text2 = (text = null);
                        }
                        else
                        {
                            string a;
                            if ((a = sExpression[i++].ToString()) != null)
                            {
                                if (a == "+")
                                {
                                    arrayList.Add(text);
                                    arrayList.Add("++");
                                    text2 = (text = null);
                                    break;
                                }
                                if (a == "=")
                                {
                                    arrayList.Add(text);
                                    arrayList.Add("+=");
                                    text2 = (text = null);
                                    break;
                                }
                            }
                            arrayList.Add(text);
                            arrayList.Add("+");
                            text2 = (text = null);
                            i--;
                        }
                        break;
                        #endregion

                    case "-":

                        #region 减号
                        if (i > sExpression.Length - 1)
                        {
                            arrayList.Add(text);
                            arrayList.Add(text2);
                            text2 = (text = null);
                        }
                        else
                        {
                            string a2;
                            if ((a2 = sExpression[i++].ToString()) != null)
                            {
                                if (a2 == "-")
                                {
                                    arrayList.Add(text);
                                    arrayList.Add("--");
                                    text2 = (text = null);
                                    break;
                                }
                                if (a2 == "=")
                                {
                                    arrayList.Add(text);
                                    arrayList.Add("-=");
                                    text2 = (text = null);
                                    break;
                                }
                            }
                            arrayList.Add(text);
                            arrayList.Add("-");
                            text2 = (text = null);
                            i--;
                        }
                        break;
                        #endregion

                    case "*":

                        #region 乘法
                        if (i > sExpression.Length - 1)
                        {
                            arrayList.Add(text);
                            arrayList.Add(text2);
                            text2 = (text = null);
                        }
                        else
                        {
                            string a3;
                            if ((a3 = sExpression[i++].ToString()) != null && a3 == "=")
                            {
                                arrayList.Add(text);
                                arrayList.Add("*=");
                                text2 = (text = null);
                            }
                            else
                            {
                                arrayList.Add(text);
                                arrayList.Add("*");
                                text2 = (text = null);
                                i--;
                            }
                        }
                        break;
                        #endregion

                    case "/":

                        #region 除法
                        if (i > sExpression.Length - 1)
                        {
                            arrayList.Add(text);
                            arrayList.Add(text2);
                            text2 = (text = null);
                        }
                        else
                        {
                            string a4;
                            if ((a4 = sExpression[i++].ToString()) != null && a4 == "=")
                            {
                                arrayList.Add(text);
                                arrayList.Add("/=");
                                text2 = (text = null);
                            }
                            else
                            {
                                arrayList.Add(text);
                                arrayList.Add("/");
                                text2 = (text = null);
                                i--;
                            }
                        }
                        break;
                        #endregion

                    case "%":

                        #region 取余
                        if (i > sExpression.Length - 1)
                        {
                            arrayList.Add(text);
                            arrayList.Add(text2);
                            text2 = (text = null);
                        }
                        else
                        {
                            string a5;
                            if ((a5 = sExpression[i++].ToString()) != null && a5 == "=")
                            {
                                arrayList.Add(text);
                                arrayList.Add("%=");
                                text2 = (text = null);
                            }
                            else
                            {
                                arrayList.Add(text);
                                arrayList.Add("%");
                                text2 = (text = null);
                                i--;
                            }
                        }
                        break;
                        #endregion

                    case ">":

                        #region 大于
                        if (i > sExpression.Length - 1)
                        {
                            arrayList.Add(text);
                            arrayList.Add(text2);
                            text2 = (text = null);
                        }
                        else
                        {
                            string a6;
                            if ((a6 = sExpression[i++].ToString()) != null)
                            {
                                if (a6 == ">")
                                {
                                    arrayList.Add(text);
                                    arrayList.Add(">>");
                                    text2 = (text = null);
                                    break;
                                }
                                if (a6 == "=")
                                {
                                    arrayList.Add(text);
                                    arrayList.Add(">=");
                                    text2 = (text = null);
                                    break;
                                }
                            }
                            arrayList.Add(text);
                            arrayList.Add(">");
                            text2 = (text = null);
                            i--;
                        }
                        break;
                        #endregion

                    case "<":

                        #region 小于
                        if (i > sExpression.Length - 1)
                        {
                            arrayList.Add(text);
                            arrayList.Add(text2);
                            text2 = (text = null);
                        }
                        else
                        {
                            string a7;
                            if ((a7 = sExpression[i++].ToString()) != null)
                            {
                                if (a7 == "<")
                                {
                                    arrayList.Add(text);
                                    arrayList.Add("<<");
                                    text2 = (text = null);
                                    break;
                                }
                                if (a7 == ">")
                                {
                                    arrayList.Add(text);
                                    arrayList.Add("<>");
                                    text2 = (text = null);
                                    break;
                                }
                                if (a7 == "=")
                                {
                                    arrayList.Add(text);
                                    arrayList.Add("<=");
                                    text2 = (text = null);
                                    break;
                                }
                            }
                            arrayList.Add(text);
                            arrayList.Add("<");
                            text2 = (text = null);
                            i--;
                        }
                        break;
                        #endregion

                    case "=":

                        #region 等于
                        if (i > sExpression.Length - 1)
                        {
                            arrayList.Add(text);
                            arrayList.Add(text2);
                            text2 = (text = null);
                        }
                        else
                        {
                            string a8;
                            if ((a8 = sExpression[i++].ToString()) != null && a8 == "=")
                            {
                                arrayList.Add(text);
                                arrayList.Add("==");
                                text2 = (text = null);
                            }
                            else
                            {
                                arrayList.Add(text);
                                arrayList.Add("=");
                                text2 = (text = null);
                                i--;
                            }
                        }
                        break;
                        #endregion

                    case "!":

                        #region 取反
                        if (i > sExpression.Length - 1)
                        {
                            arrayList.Add(text);
                            arrayList.Add(text2);
                            text2 = (text = null);
                        }
                        else
                        {
                            string a9;
                            if ((a9 = sExpression[i++].ToString()) != null && a9 == "=")
                            {
                                arrayList.Add(text);
                                arrayList.Add("!=");
                                text2 = (text = null);
                            }
                            else
                            {
                                arrayList.Add(text);
                                arrayList.Add("!");
                                text2 = (text = null);
                                i--;
                            }
                        }
                        break;
                        #endregion

                    case "|":

                        #region 或运算
                        if (i > sExpression.Length - 1)
                        {
                            arrayList.Add(text);
                            arrayList.Add(text2);
                            text2 = (text = null);
                        }
                        else
                        {
                            string a10;
                            if ((a10 = sExpression[i++].ToString()) != null)
                            {
                                if (a10 == "=")
                                {
                                    arrayList.Add(text);
                                    arrayList.Add("|=");
                                    text2 = (text = null);
                                    break;
                                }
                                if (a10 == "|")
                                {
                                    arrayList.Add(text);
                                    arrayList.Add("||");
                                    text2 = (text = null);
                                    break;
                                }
                            }
                            arrayList.Add(text);
                            arrayList.Add("|");
                            text2 = (text = null);
                            i--;
                        }
                        break;
                        #endregion

                    case "&":

                        #region 与运算
                        if (i > sExpression.Length - 1)
                        {
                            arrayList.Add(text);
                            arrayList.Add(text2);
                            text2 = (text = null);
                        }
                        else
                        {
                            string a11;
                            if ((a11 = sExpression[i++].ToString()) != null)
                            {
                                if (a11 == "=")
                                {
                                    arrayList.Add(text);
                                    arrayList.Add("&=");
                                    text2 = (text = null);
                                    break;
                                }
                                if (a11 == "&")
                                {
                                    arrayList.Add(text);
                                    arrayList.Add("&&");
                                    text2 = (text = null);
                                    break;
                                }
                            }
                            arrayList.Add(text);
                            arrayList.Add("&");
                            text2 = (text = null);
                            i--;
                        }
                        break;
                        #endregion

                    default:
                        goto IL_9DD;
                }
            IL_9E5:
                if (i == sExpression.Length)
                {
                    arrayList.Add(text);
                    continue;
                }
                continue;
            IL_9DD:
                text += text2;
                goto IL_9E5;
            }
            ArrayList arrayList2 = new ArrayList();
            foreach (object current in arrayList)
            {
                if (current != null && !(current.ToString().Trim() == ""))
                {
                    arrayList2.Add(current);
                }
            }
            return arrayList2;
        }

        public static ArrayList ConvertExpression(ArrayList alExpression, string mapVar, string mapValue)
        {
            for (int i = 0; i < alExpression.Count; i++)
            {
                if (alExpression[i].ToString() == mapVar)
                {
                    alExpression[i] = mapValue;
                    break;
                }
            }
            return alExpression;
        }

        public static ArrayList ConvertExpression(ArrayList alExpression, string[] mapVar, string[] mapValue)
        {
            for (int i = 0; i < alExpression.Count; i++)
            {
                for (int j = 0; j < mapVar.Length; j++)
                {
                    if (alExpression[i].ToString() == mapVar[j])
                    {
                        alExpression[i] = mapValue[j];
                        break;
                    }
                }
            }
            return alExpression;
        }

        public static int Find_First_RightBracket(ArrayList alExpression)
        {
            for (int i = 0; i < alExpression.Count; i++)
            {
                if (Arithmetic.OperatorMap.CheckRightBracket(alExpression[i].ToString()))
                {
                    return i;
                }
            }
            return 0;
        }

        public static int Find_Near_LeftBracket(ArrayList alExpression, int iRightBracket)
        {
            for (int i = iRightBracket - 2; i >= 0; i--)
            {
                if (Arithmetic.OperatorMap.CheckLeftBracket(alExpression[i].ToString()))
                {
                    return i;
                }
            }
            return 0;
        }

        public static ArrayList ConvertToPostfix(ArrayList alexpression)
        {
            ArrayList arrayList = new ArrayList();
            Stack stack = new Stack();
            int count = alexpression.Count;
            int i = 0;
            while (i < count)
            {
                string text = alexpression[i++].ToString();
                if (Arithmetic.OperatorMap.CheckLeftBracket(text))
                {
                    stack.Push(text);
                }
                else
                {
                    if (Arithmetic.OperatorMap.CheckRightBracket(text))
                    {
                        while (stack.Count != 0)
                        {
                            string a = stack.Peek().ToString();
                            if (a == "(")
                            {
                                stack.Pop();
                                break;
                            }
                            arrayList.Add(stack.Pop());
                        }
                    }
                    else
                    {
                        if (Arithmetic.OperatorMap.IsVar(text))
                        {
                            arrayList.Add(text);
                        }
                        else
                        {
                            if (Arithmetic.OperatorMap.CheckOperator(text))
                            {
                                while (stack.Count > 0)
                                {
                                    string text2 = stack.Peek().ToString();
                                    if (text2 == "(" || Arithmetic.OperatorMap.GetMaxprior(text, text2) < 0)
                                    {
                                        break;
                                    }
                                    arrayList.Add(stack.Pop().ToString());
                                }
                                stack.Push(text);
                            }
                        }
                    }
                }
            }
            while (stack.Count > 0)
            {
                string value = stack.Pop().ToString();
                arrayList.Add(value);
            }
            return arrayList;
        }

        public static object ComputePostfix(ArrayList alexpression)
        {
            object result;
            try
            {
                Stack stack = new Stack();
                int count = alexpression.Count;
                int i = 0;
                while (i < count)
                {
                    string text = alexpression[i++].ToString();
                    if (Arithmetic.OperatorMap.IsVar(text))
                    {
                        stack.Push(text);
                    }
                    else
                    {
                        if (Arithmetic.OperatorMap.CheckOperator(text))
                        {
                            if (!Arithmetic.CheckOneOperator(text))
                            {
                                string sR = stack.Pop().ToString();
                                string text2 = stack.Pop().ToString();
                                string obj = Arithmetic.ComputeTwo(text2, sR, text).ToString();
                                stack.Push(obj);
                            }
                            else
                            {
                                string text2 = stack.Pop().ToString();
                                string obj = Arithmetic.ComputeOne(text2, text).ToString();
                                stack.Push(obj);
                            }
                        }
                    }
                }
                string text3 = stack.Pop().ToString();
                result = text3;
            }
            catch
            {
                Console.WriteLine("Result:表达式不符合运算规则!Sorry!");
                result = "Sorry!Error!";
            }
            return result;
        }

        public static object ComputeExpression(string sExpression)
        {
            return Arithmetic.ComputePostfix(Arithmetic.ConvertToPostfix(Arithmetic.ConvertExpression(sExpression)));
        }

        public static object ComputeExpression(string sExpression, string mapVar, string mapValue)
        {
            return Arithmetic.ComputePostfix(Arithmetic.ConvertToPostfix(Arithmetic.ConvertExpression(Arithmetic.ConvertExpression(sExpression), mapVar, mapValue)));
        }

        public static object ComputeExpression(string sExpression, string[] mapVar, string[] mapValue)
        {
            return Arithmetic.ComputePostfix(Arithmetic.ConvertToPostfix(Arithmetic.ConvertExpression(Arithmetic.ConvertExpression(sExpression), mapVar, mapValue)));
        }

        public static bool CheckNumber(string str)
        {
            bool result;
            try
            {
                Convert.ToDouble(str);
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public static bool CheckBoolean(string str)
        {
            bool result;
            try
            {
                Convert.ToBoolean(str);
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public static bool CheckString(string str)
        {
            bool result;
            try
            {
                str = str.Replace("\"", "");
                char c = str[0];
                if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z'))
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public static bool CheckOneOperator(string sOperator)
        {
            return sOperator == "++" || sOperator == "--" || sOperator == "!";
        }

        public static object ComputeTwoNumber(double dL, double dR, string sO)
        {
            if (sO != null)
            {
                switch (sO)
                {
                    case "+":
                        return dL + dR;
                    case "-":
                        return dL - dR;
                    case "*":
                        return dL * dR;
                    case "%":
                        return dL % dR;
                    case "/":
                        try
                        {
                            object result = dL / dR;
                            return result;
                        }
                        catch
                        {
                            object result = false;
                            return result;
                        }
                        break;

                    case "+=":
                        return dL -= dR;
                        break;

                    case "-=":
                        return dL -= dR;
                    case "*=":
                        return dL *= dR;
                    case "/=":
                        try
                        {
                            object result = dL /= dR;
                            return result;
                        }
                        catch
                        {
                            object result = false;
                            return result;
                        }
                        goto IL_247;
                    case "=":
                        goto IL_247;
                    case "==":
                        return dL == dR;
                    case "!=":
                        return dL != dR;
                    case "<>":
                        return dL != dR;
                    case ">":
                        return dL.CompareTo(dR) > 0;
                    case ">=":
                        return dL.CompareTo(dR) >= 0;
                    case "<":
                        return dL.CompareTo(dR) < 0;
                    case "<=":
                        return dL.CompareTo(dR) <= 0;
                    case ">>":
                        return (int)dL >> (int)dR;
                    case "<<":
                        return (int)dL << (int)dR;
                    case "|":
                        return (int)dL | (int)dR;
                    case "&":
                        return (int)dL & (int)dR;
                    case "|=":
                        {
                            int num2 = (int)dL;
                            int num3 = (int)dR;
                            return num2 | num3;
                        }
                    case "&=":
                        {
                            int num4 = (int)dL;
                            int num5 = (int)dR;
                            return num4 & num5;
                        }
                    default:
                        goto IL_313;
                }
                return dL += dR;
            IL_247:
                return dL == dR;
            }
        IL_313:
            return false;
        }

        public static object ComputeTwoBoolean(bool bL, bool bR, string sO)
        {
            switch (sO)
            {
                case ">":
                    return bL.CompareTo(bR) > 0;
                case ">=":
                    return bL.CompareTo(bR) >= 0;
                case "<":
                    return bL.CompareTo(bR) < 0;
                case "<=":
                    return bL.CompareTo(bR) <= 0;
                case "=":
                    return bL == bR;
                case "==":
                    return bL == bR;
                case "!=":
                    return bL != bR;
                case "<>":
                    return bL != bR;
                case "||":
                    return bL || bR;
                case "&&":
                    return bL && bR;
            }
            return false;
        }

        public static object ComputeTwoString(string sL, string sR, string sO)
        {
            switch (sO)
            {
                case "+":
                    return sL + sR;
                case "=":
                    return sL == sR;
                case "==":
                    return sL == sR;
                case "!=":
                    return sL != sR;
                case "<>":
                    return sL != sR;
                case ">":
                    return sL.CompareTo(sR) > 0;
                case ">=":
                    return sL.CompareTo(sR) >= 0;
                case "<":
                    return sL.CompareTo(sR) < 0;
                case "<=":
                    return sL.CompareTo(sR) <= 0;
            }
            return false;
        }

        public static object ComputeTwo(string sL, string sR, string sO)
        {
            if (Arithmetic.CheckNumber(sL))
            {
                if (Arithmetic.CheckNumber(sR))
                {
                    return Arithmetic.ComputeTwoNumber(Convert.ToDouble(sL), Convert.ToDouble(sR), sO);
                }
                if (Arithmetic.CheckString(sR))
                {
                    return Arithmetic.ComputeTwoString(sL, sR, sO);
                }
            }
            else
            {
                if (Arithmetic.CheckBoolean(sL))
                {
                    if (Arithmetic.CheckBoolean(sR))
                    {
                        return Arithmetic.ComputeTwoBoolean(Convert.ToBoolean(sL), Convert.ToBoolean(sR), sO);
                    }
                    if (Arithmetic.CheckString(sR))
                    {
                        return Arithmetic.ComputeTwoString(sL, sR, sO);
                    }
                }
                else
                {
                    if (Arithmetic.CheckString(sL))
                    {
                        return Arithmetic.ComputeTwoString(sL, sR, sO);
                    }
                }
            }
            return "ComputeTwo [" + sL + "][" + sO + "][" + sR + "] Sorry!";
        }

        public static object ComputeOneNumber(double dou, string sO)
        {
            if (sO != null)
            {
                if (sO == "++")
                {
                    return dou + 1.0;
                }
                if (sO == "--")
                {
                    return dou - 1.0;
                }
            }
            return false;
        }

        public static object ComputeOneString(string str, string sO)
        {
            if (sO != null && sO == "++")
            {
                return str + str;
            }
            return false;
        }

        public static object ComputeOneBoolean(bool bo, string sO)
        {
            if (sO != null && sO == "!")
            {
                return !bo;
            }
            return false;
        }

        public static object ComputeOne(string str, string sO)
        {
            if (Arithmetic.CheckNumber(str))
            {
                return Arithmetic.ComputeOneNumber(Convert.ToDouble(str), sO);
            }
            if (Arithmetic.CheckBoolean(str))
            {
                return Arithmetic.ComputeOneBoolean(Convert.ToBoolean(str), sO);
            }
            if (Arithmetic.CheckString(str))
            {
                return Arithmetic.ComputeOneString(str, sO);
            }
            return "ComputerOne [" + str + "][" + sO + "] Sorry!";
        }
    }
}