using System;
using System.Collections;

namespace BBX.Forum.Common
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

            public static OperatorMap.Map[] map()
            {
                OperatorMap.Map[] array = new OperatorMap.Map[]
				{
					new OperatorMap.Map(5, "*"),
					new OperatorMap.Map(5, "/"),
					default(OperatorMap.Map),
					default(OperatorMap.Map),
					default(OperatorMap.Map),
					default(OperatorMap.Map),
					default(OperatorMap.Map),
					default(OperatorMap.Map),
					default(OperatorMap.Map),
					default(OperatorMap.Map),
					default(OperatorMap.Map),
					default(OperatorMap.Map),
					default(OperatorMap.Map),
					default(OperatorMap.Map),
					default(OperatorMap.Map),
					default(OperatorMap.Map),
					default(OperatorMap.Map),
					default(OperatorMap.Map),
					default(OperatorMap.Map),
					default(OperatorMap.Map),
					default(OperatorMap.Map),
					default(OperatorMap.Map),
					default(OperatorMap.Map),
					default(OperatorMap.Map),
					default(OperatorMap.Map),
					default(OperatorMap.Map),
					default(OperatorMap.Map),
					default(OperatorMap.Map),
					default(OperatorMap.Map),
					new OperatorMap.Map(5, "%")
				};
                array[2] = new OperatorMap.Map(10, "+");
                array[3] = new OperatorMap.Map(10, "-");
                array[4] = new OperatorMap.Map(20, ">");
                array[5] = new OperatorMap.Map(20, ">=");
                array[6] = new OperatorMap.Map(20, "<");
                array[7] = new OperatorMap.Map(20, "<=");
                array[8] = new OperatorMap.Map(20, "<>");
                array[9] = new OperatorMap.Map(20, "!=");
                array[10] = new OperatorMap.Map(20, "==");
                array[11] = new OperatorMap.Map(20, "=");
                array[12] = new OperatorMap.Map(41, "!");
                array[13] = new OperatorMap.Map(42, "||");
                array[14] = new OperatorMap.Map(43, "&&");
                array[15] = new OperatorMap.Map(40, "++");
                array[16] = new OperatorMap.Map(40, "--");
                array[17] = new OperatorMap.Map(40, "+=");
                array[18] = new OperatorMap.Map(40, "-=");
                array[19] = new OperatorMap.Map(40, "*=");
                array[20] = new OperatorMap.Map(40, "/=");
                array[21] = new OperatorMap.Map(40, "&");
                array[22] = new OperatorMap.Map(40, "|");
                array[23] = new OperatorMap.Map(40, "&=");
                array[24] = new OperatorMap.Map(40, "|=");
                array[25] = new OperatorMap.Map(40, ">>");
                array[26] = new OperatorMap.Map(40, "<<");
                array[27] = new OperatorMap.Map(3, "(");
                array[28] = new OperatorMap.Map(3, ")");
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

            public static OperatorMap.Map GetMap(string Operator)
            {
                if (OperatorMap.CheckOperator(Operator))
                {
                    OperatorMap.Map[] array = OperatorMap.map();
                    for (int i = 0; i < array.Length; i++)
                    {
                        OperatorMap.Map result = array[i];
                        if (result.Operator == Operator)
                        {
                            return result;
                        }
                    }
                }
                return new OperatorMap.Map(99, Operator);
            }

            public static int Getprior(string Operator)
            {
                return OperatorMap.GetMap(Operator).Priority;
            }

            public static int GetMaxprior(string Loperator, string Roperator)
            {
                return OperatorMap.GetMap(Loperator).Priority - OperatorMap.GetMap(Roperator).Priority;
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
                if (current != null && !(String.IsNullOrEmpty(current.ToString().Trim())))
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
                if (OperatorMap.CheckRightBracket(alExpression[i].ToString()))
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
                if (OperatorMap.CheckLeftBracket(alExpression[i].ToString()))
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
                if (OperatorMap.CheckLeftBracket(text))
                {
                    stack.Push(text);
                }
                else
                {
                    if (OperatorMap.CheckRightBracket(text))
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
                        if (OperatorMap.IsVar(text))
                        {
                            arrayList.Add(text);
                        }
                        else
                        {
                            if (OperatorMap.CheckOperator(text))
                            {
                                while (stack.Count > 0)
                                {
                                    string text2 = stack.Peek().ToString();
                                    if (text2 == "(" || OperatorMap.GetMaxprior(text, text2) < 0)
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
                    if (OperatorMap.IsVar(text))
                    {
                        stack.Push(text);
                    }
                    else
                    {
                        if (OperatorMap.CheckOperator(text))
                        {
                            if (!CheckOneOperator(text))
                            {
                                string sR = stack.Pop().ToString();
                                string text2 = stack.Pop().ToString();
                                string obj = ComputeTwo(text2, sR, text).ToString();
                                stack.Push(obj);
                            }
                            else
                            {
                                string text2 = stack.Pop().ToString();
                                string obj = ComputeOne(text2, text).ToString();
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
            return ComputePostfix(ConvertToPostfix(ConvertExpression(sExpression)));
        }

        public static object ComputeExpression(string sExpression, string mapVar, string mapValue)
        {
            return ComputePostfix(ConvertToPostfix(ConvertExpression(ConvertExpression(sExpression), mapVar, mapValue)));
        }

        public static object ComputeExpression(string sExpression, string[] mapVar, string[] mapValue)
        {
            return ComputePostfix(ConvertToPostfix(ConvertExpression(ConvertExpression(sExpression), mapVar, mapValue)));
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

        public static object ComputeTwoNumber(double dL, double dR, string op)
        {
            if (op == null) return false;

            switch (op)
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
                    if (dR == 0) return false;
                    return dL / dR;
                case "+=":
                    return dL += dR;
                case "-=":
                    return dL -= dR;
                case "*=":
                    return dL *= dR;
                case "/=":
                    if (dR == 0) return false;
                    return dL /= dR;
                case "=":
                    return dL == dR;
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
                    return (int)dL | (int)dR;
                case "&=":
                    return (int)dL & (int)dR;
                default:
                    return false;
            }
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
            if (CheckNumber(sL))
            {
                if (CheckNumber(sR))
                {
                    return ComputeTwoNumber(Convert.ToDouble(sL), Convert.ToDouble(sR), sO);
                }
                if (CheckString(sR))
                {
                    return ComputeTwoString(sL, sR, sO);
                }
            }
            else
            {
                if (CheckBoolean(sL))
                {
                    if (CheckBoolean(sR))
                    {
                        return ComputeTwoBoolean(Convert.ToBoolean(sL), Convert.ToBoolean(sR), sO);
                    }
                    if (CheckString(sR))
                    {
                        return ComputeTwoString(sL, sR, sO);
                    }
                }
                else
                {
                    if (CheckString(sL))
                    {
                        return ComputeTwoString(sL, sR, sO);
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
            if (CheckNumber(str))
            {
                return ComputeOneNumber(Convert.ToDouble(str), sO);
            }
            if (CheckBoolean(str))
            {
                return ComputeOneBoolean(Convert.ToBoolean(str), sO);
            }
            if (CheckString(str))
            {
                return ComputeOneString(str, sO);
            }
            return "ComputerOne [" + str + "][" + sO + "] Sorry!";
        }
    }
}