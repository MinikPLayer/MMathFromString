using System;
using System.Collections.Generic;
using System.Text;

namespace MathFromString
{
    public static class MathFromString
    {

        private static class OperationFunctions
        {
            public static double Add(double l1, double l2)
            {
                return l1 + l2;
            }

            public static double Subtract(double l1, double l2)
            {
                return l1 - l2;
            }

            public static double Multiply(double l1, double l2)
            {
                return l1 * l2;
            }

            public static double Divide(double l1, double l2)
            {
                if(double.IsInfinity(l2))
                {
                    return double.NegativeInfinity;
                }

                if(double.IsInfinity(l1))
                {
                    return double.PositiveInfinity;
                }

                if(l2 == 0)
                {
                    return double.NaN;
                }
                return l1 / l2;
            }

            public static double Power(double l1, double l2)
            {
                return MUtil.Power(l1, l2);
            }

            public static double Rt(double l1, double n)
            {
                return Math.Pow(l1, 1.0 / n);
            }

            public static double Sqrt(double l1)
            {
                return Math.Sqrt((float)l1);
            }

            public static double Sin(double l1)
            {
                return Math.Sin((float)l1);
            }

            public static double Cos(double l1)
            {
                return Math.Cos((float)l1);
            }

            public static double Tg(double l1)
            {
                return Math.Tan((float)l1);
            }

            public static double Ctg(double l1)
            {
                return 1f/Math.Tan((float)l1);
            }

            public static double Abs(double l1)
            {
                return Math.Abs(l1);
            }

            public static double Log(double l1, double n)
            {
                return Math.Log(l1, n);
            }

            public static double Ln(double l1)
            {
                return Math.Log(l1);
            }
        }

        public struct BracketOperation
        {
            public int positionInString;
            public string operation;

            public Function specialFunction;

            public BracketOperation(string _operation, int _position, Function _specialFunction = null)
            {
                positionInString = _position;
                operation = _operation;

                specialFunction = _specialFunction;
            }
        }

        public class Operation
        {
            public string name;
            public char[] operatorChars;
            /// <summary>
            /// Returns result of operation, arguments: (double) L1, (double) L2
            /// </summary>
            public Func<double, double, double> function;
            public Operation(string _name, char _operatorChar, Func<double, double, double> _operationFunction)
            {
                name = _name;

                operatorChars = new char[1];
                operatorChars[0] = _operatorChar;

                function = _operationFunction;
            }
            public Operation(string _name, char[] _operatorChars, Func<double, double, double> _operationFunction)
            {
                name = _name;

                operatorChars = _operatorChars;

                function = _operationFunction;
            }
        }

        public class Function
        {
            public string name;
            public string prefix;
            public string suffix;

            public bool beforeNumber;

            public Func<double, double> function = null;
            public Func<double, double, double> twoArgFunction = null;

            public Function(string _name, string _prefix, string _suffix, Func<double, double> _function, Func<double, double, double> twoArgFunction = null)
            {
                if(_prefix.Length > 0 && _suffix.Length > 0)
                {
                    Debug.LogError("Prefix and suffix are set, setting to default ( prefix only )");
                    _suffix = "";
                }

                name = _name;
                prefix = _prefix;
                suffix = _suffix;

                if(prefix.Length > 0)
                {
                    beforeNumber = true;
                }
                else if(suffix.Length > 0)
                {
                    beforeNumber = false;
                }
                else
                {
                    Debug.LogError("Empty suffix and prefix");
                    suffix = "";
                    beforeNumber = false;
                }

                function = _function;
                this.twoArgFunction = twoArgFunction;
            }
        }

        private static Function[] bracketsFunctions;

        private static Operation[] operations;

        public static void FillOperationsList()
        {
            operations = new Operation[] {
                new Operation ("Add", '+', OperationFunctions.Add),
                new Operation ("Substract", '-', OperationFunctions.Subtract),
                new Operation ("Multiply", '*', OperationFunctions.Multiply),
                new Operation ("Divide", new char[]{'/', '\\', ':' }, OperationFunctions.Divide),
                new Operation ("Power", '^', OperationFunctions.Power),
            };
        }

        public static void FillBracketsFunctions()
        {
            bracketsFunctions = new Function[]
            {
                new Function("Square Root", "sqrt", "", OperationFunctions.Sqrt),
                new Function("Root", "rt", "", null, OperationFunctions.Rt),
                new Function("Sinus", "sin", "", OperationFunctions.Sin),
                new Function("Cosinus", "cos", "", OperationFunctions.Cos),
                new Function("Tangent", "tg", "", OperationFunctions.Tg),
                new Function("Cotangent", "ctg", "", OperationFunctions.Ctg),
                new Function("Absolute", "abs", "", OperationFunctions.Abs),
                new Function("Logarithm", "log", "", null, OperationFunctions.Log),
                new Function("Natural Logarithm", "ln", "", OperationFunctions.Ln)
            };
        }

        private static Function FindFunction(string str, bool prefix)
        {
            if (prefix)
            {
                for (int i = 0; i < bracketsFunctions.Length; i++)
                {
                    if (bracketsFunctions[i].prefix.Equals(str))
                    {
                        return bracketsFunctions[i];
                    }
                }
            }
            else
            {
                for (int i = 0; i < bracketsFunctions.Length; i++)
                {
                    if (bracketsFunctions[i].suffix == str)
                    {
                        return bracketsFunctions[i];
                    }
                }
            }

            return null;
        }

        private static Operation FindOperation(char opChar)
        {
            for(int i = 0;i<operations.Length;i++)
            {
                for(int j = 0;j<operations[i].operatorChars.Length;j++)
                {
                    if(operations[i].operatorChars[j] == opChar)
                    {
                        return operations[i];
                    }
                }
            }

            return null; 
        }

        public static bool DoMaths(ref string number, ref double l1, ref double l2, ref Operation op, Operation newOp, ref bool firstOperation)
        {
            // Do not assign l2 if it's first time
            if (!firstOperation)
            {
                if (!double.TryParse(number, out l2))
                {
                    number = MUtil.ExpNotationToDecimalNotation(number);
                    if (!double.TryParse(number, out l1))
                    {

                        Debug.LogError("Cannot parse " + number + " to number ( double )");
                        return false;
                    }
                }
                



                l1 = op.function.Invoke(l1, l2);
            }
            else
            {
                firstOperation = false;
            }
            number = "";
            op = newOp;

            return true;
        }

       

        public static string CalculateSpecificOperations(string data, Operation[] allowedOperations)
        {
            double l1 = 0, l2 = 0;
            Operation op = null; // First operation is always addition


            string actualNumber = "";
            bool noPrevOperators = true;

            bool operationIsAllowed = false;

            int startIndex = 0;
            string startData = "";
            for(int i = 0;i<data.Length;i++)
            {
                op = FindOperation(data[i]);

                if (op != null)
                {
                    if(i == 0 && data[0] == '-')
                    {
                        startData += data[0];
                        continue;
                    }
                    startIndex = i;
                    break;
                }

                startData += data[i];
            }
            
            // No operator
            if(op == null)
            {
                return data;
            }

            if(startIndex == 0)
            {
                Debug.LogError("Operator without number: \"" + data + "\" op: " + op.name);
                return "EOPWTHTNMBR";
            }

            

            if(!double.TryParse(startData, out l1))
            {

                if (startData[startData.Length - 1] == 'E')
                {
                    for (int i = startData.Length; i < data.Length; i++)
                    {
                        if (MUtil.IsNumber(data[i]) || data[i] == '+')
                        {
                            startData += data[i];
                        } 
                        else
                        {
                            break;
                        }
                    }

                    for(int i = 0;i<data.Length;i++)
                    {
                        if(data[i] == 'E')
                        {
                            if(data.Length > i + 1)
                            {
                                if(data[i+1] == '+')
                                {
                                    i += 2;
                                    
                                }
                            }
                        }

                        op = FindOperation(data[i]);
                        if (op != null)
                        {
                            break;
                        }
                    }

                    l1 = MUtil.GetDoubleFromExpNotation(startData);
                    data = data.Remove(0, startData.Length);

                    string decimalStartData = MUtil.ExpNotationToDecimalNotation(startData);
                    data = data.Insert(0, decimalStartData);
                    startData = decimalStartData;

                    startIndex = startData.Length;

                    Debug.Log("Data after converting exponent notation: " + data);
                }
                else
                {
                    Debug.LogError("Cannot convert " + startData + " to l1 ( double )");
                    return "ECONVERSION";
                }

                if (l1.Equals(double.NaN))
                {
                    Debug.LogError("Cannot convert " + startData + " to l1 ( double ). l1 = NaN");
                    return "ECONVERSION";
                }
            }

            actualNumber = startData;
            bool firstOperation = true;

            // Multiply
            for (int i = startIndex; i < data.Length; i++)
            {
                Operation aop = FindOperation(data[i]);
                if (aop != null)
                {
                    if(actualNumber.Length == 0 && data[i] == '-')
                    {
                        actualNumber += data[i];
                        continue;   
                    }

                    operationIsAllowed = false;
                    for(int j = 0;j< allowedOperations.Length;j++)
                    {
                        if(op == allowedOperations[j])
                        {
                            operationIsAllowed = true;
                            break;
                        }
                    }
                    if (operationIsAllowed)
                    {
                        if(data[i] == '+' && i > 0 && (data[i-1] == 'E'))
                        {
                            actualNumber += data[i];
                            continue;
                        }

                        //Debug.Log("Data before: " + data);
                        if(noPrevOperators)
                        {
                            data = data.Remove(0, actualNumber.Length);
                            i = 0;
                        }
                        else
                        {
                            string l1Str = MUtil.ToStandardNotationString(l1);

                            data = data.Remove(i - actualNumber.Length - 1 - l1Str.Length, actualNumber.Length + 1 + l1Str.Length);
                            i -= actualNumber.Length + 1 + l1Str.Length;
                        }
                        
                        noPrevOperators = false;

                        //l1 = l2;

                        //actualNumber = MUtil.ExpNotationToDecimalNotation(actualNumber);
                        
                        if (!DoMaths(ref actualNumber, ref l1, ref l2, ref op, aop, ref firstOperation))
                        {
                            return "ECONVERSION";
                        }


                        string standardNotL1 = MUtil.ToStandardNotationString(l1);

                        data = data.Insert(i, standardNotL1);
                        i += standardNotL1.Length;
                        actualNumber = "";
                        op = aop;
                        //Debug.Log("Data after: " + data);
                    }
                    else
                    {
                        if (data[i] == '+' && i > 0 && (data[i - 1] == 'E'))
                        {
                            actualNumber += data[i];
                            continue;
                        }
                        if (!double.TryParse(actualNumber, out l1))
                        {
                            actualNumber = MUtil.ExpNotationToDecimalNotation(actualNumber);
                            if (!double.TryParse(actualNumber, out l1))
                            {

                                Debug.LogError("Cannot parse " + actualNumber + " to number ( double ) #2");
                                return "ECONVERSION";
                            }
                        }

                        op = aop;
                        actualNumber = "";
                        noPrevOperators = false;

                        firstOperation = false;
                    }
                }
                else
                {
                    actualNumber += data[i];
                }
            }

            operationIsAllowed = false;
            for (int j = 0; j < allowedOperations.Length; j++)
            {
                if (op == allowedOperations[j])
                {
                    operationIsAllowed = true;
                    break;
                }
            }
            if (operationIsAllowed)
            {
                if (data[data.Length - 1] == '+' && data.Length > 0 && (data[data.Length - 1] == 'E'))
                {
                    return data;
                }

                //Debug.Log("l1.ToString(): " + MUtil.ExpNotationToDecimalNotation(l1.ToString()) + ", lenght: " + MUtil.ExpNotationToDecimalNotation(l1.ToString()).Length);
                string l1Str = MUtil.ToStandardNotationString(l1);
                data = data.Remove(data.Length - actualNumber.Length - 1 - l1Str.Length, actualNumber.Length + 1 + l1Str.Length);

                noPrevOperators = false;

                //l1 = l2;
                //actualNumber = MUtil.ExpNotationToDecimalNotation(actualNumber);

                if (!DoMaths(ref actualNumber, ref l1, ref l2, ref op, operations[0], ref firstOperation))
                {
                    return "ECONVERSION";
                }



                data = data.Insert(data.Length, MUtil.ToStandardNotationString(l1));
                //i += l1.ToString().Length;
                actualNumber = "";
                op = operations[0];
                //Debug.Log("Data after: " + data);
            }


            return data;
        }

        private static string CalculateExpression(string data)
        {
            data = CalculateSpecificOperations(data, new Operation[] { operations[4] /* power */});
            data = CalculateSpecificOperations(data, new Operation[] { operations[2] /* multiply */ , operations[3] /* divide */ });
            data = CalculateSpecificOperations(data, new Operation[] { operations[0] /* add */ , operations[1] /* substract */ });

            return data;
        }

        /// <summary>
        /// Replaces PI with 3.14
        /// </summary>
        /// <param name="data">Src in which you want to replace constants</param>
        /// <returns>Replaced string</returns>
        private static string ReplaceConstants(string data)
        {
            const string pi = "3,14159265358979";
            const string e =  "2,71828182845904";
            const string fi = "1,61803398874989";

            return data.Replace("Pi", pi).Replace("PI", pi).Replace("pi", pi).Replace("e", e).Replace("fi", fi).Replace("Fi", fi).Replace("FI", fi);
        }

        /// <summary>
        /// Gets bracket operation and creates list from them
        /// </summary>
        /// <param name="data">String with expression</param>
        /// <param name="bracketOperations"></param>
        /// <returns>Data without brackets operations</returns>
        private static string CalculateBrackets(string data)
        {
            string actExpr = "";
            string specialMode = "";
            int bracketLevel = 0;

            List<BracketOperation> bracketOperations = new List<BracketOperation>();
            
            for(int i = 0;i<data.Length;i++)
            {
                if (bracketLevel > 0)
                {
                    actExpr += data[i];
                }

                if(data[i] == ')')
                {
                    if(bracketLevel > 1)
                    {
                        bracketLevel--;
                    }
                    else if(bracketLevel == 1)
                    {
                        BracketOperation operation = new BracketOperation(actExpr.Remove(0, 1).Remove(actExpr.Length - 2, 1), i - actExpr.Length + 1 - specialMode.Length);
                        if (specialMode.Length > 0)
                        {
                            Function f = FindFunction(specialMode, true);
                            if(f == null)
                            {
                                Debug.LogError("Cannot find function " + specialMode);
                                return "ENOFUNCTION";
                            }

                            operation.specialFunction = f;
                        }

                        
                        //Debug.Log("Bracket operation: \"" + operation.operation + "\"");
                        //Debug.Log("Bracket operation start: " + operation.positionInString);

                        bracketOperations.Add(operation);


                        data = data.Remove(i - actExpr.Length + 1 - specialMode.Length, actExpr.Length + specialMode.Length);
                        i -= actExpr.Length + specialMode.Length;
                        actExpr = "";
                        specialMode = "";

                        bracketLevel = 0;
                    }
                    else
                    {
                        Debug.LogError("Closing bracket without opening");
                        return double.NaN.ToString();
                    }
                }
                else if(data[i] == '(')
                {
                    bracketLevel++;
                    if (bracketLevel == 1)
                    {
                        actExpr += data[i];
                        string s = "";
                        for (int j = i - 1;j>=0;j--)
                        {
                            if (MUtil.IsNumber(data[j])) break;
                            if (FindOperation(data[j]) != null) break;

                            s = "" + data[j];
                            specialMode = specialMode.Insert(0, s);
                        }
                    }
                }
            }

            //Debug.Log("Bracket level: " + bracketLevel);
            if(bracketLevel > 0)
            {
                Debug.LogError("No closing bracket");
                return "ENOCLOSEBRACKET";
            }

            // Recurently get additional brackets from brackets
            for(int i = 0;i<bracketOperations.Count;i++)
            {

                BracketOperation op = bracketOperations[i];
                op.operation = CalculateBrackets(bracketOperations[i].operation);
                bracketOperations[i] = op;
            }

            // Calcualate brackets
            for(int i = 0;i<bracketOperations.Count;i++)
            {

                string rtBeforeComma = "";
                string rtAfterComma = "";
                bool rtIsAfterComma = false;

                string result;

                // Two args functions
                if (bracketOperations[i].specialFunction != null && bracketOperations[i].specialFunction.function == null && bracketOperations[i].specialFunction.twoArgFunction != null)
                {

                    for (int j = 0; j < bracketOperations[i].operation.Length; j++)
                    {
                        if (bracketOperations[i].operation[j] == ';')
                        {
                            rtIsAfterComma = true;
                            continue;
                        }

                        if (rtIsAfterComma)
                        {
                            rtAfterComma += bracketOperations[i].operation[j];
                        }
                        else
                        {
                            rtBeforeComma += bracketOperations[i].operation[j];
                        }
                    }

                    rtBeforeComma = CalculateExpression(rtBeforeComma);
                    //Debug.Log("rtBeforeComma after: " + rtBeforeComma);
                    rtAfterComma = CalculateExpression(rtAfterComma);
                    //Debug.Log("rtAfterComma after: " + rtAfterComma);

                    result = "0";
                }
                else
                {
                    result = CalculateExpression(bracketOperations[i].operation);
                }
                double res;
                if(!double.TryParse(result, out res))
                {
                    Debug.LogError("Cannot convert " + result + " to result ( double )");
                    return "ECONVERSION";
                }

                if(bracketOperations[i].specialFunction != null)
                {
                    if (bracketOperations[i].specialFunction.function != null)
                    {
                        res = bracketOperations[i].specialFunction.function.Invoke(res);
                        result = MUtil.ToStandardNotationString(res);
                        if (result.Length == 0) result = "0";
                    }
                    else
                    {
                        if(bracketOperations[i].specialFunction.function == null && bracketOperations[i].specialFunction.twoArgFunction != null)
                        {


                            if(rtBeforeComma.Length == 0 || rtAfterComma.Length == 0)
                            {
                                Debug.LogError("Bad 2 arg function arguments");
                                return "EBADROOTARG";
                            }
                            else
                            {
                                double l1;
                                double n;
                                if(!double.TryParse(rtBeforeComma, out l1))
                                {
                                    Debug.LogError("Cannot convert first root argument to l1 ( double )");
                                    return "EBADCONVERSION";
                                }
                                if(!double.TryParse(rtAfterComma, out n))
                                {
                                    Debug.LogError("Cannot convert second root argument to n ( double )");
                                    return "EBADCONVERSION";
                                }

                                res = bracketOperations[i].specialFunction.twoArgFunction(l1, n); //OperationFunctions.Rt(l1, n);
                                result = MUtil.ToStandardNotationString(res);
                                if (result.Length == 0) result = "0";
                            }
                        }
                    }

                    //Debug.Log("Result: " + result);
                }

                data = data.Insert(bracketOperations[i].positionInString, result);
                
                // Fix offsets
                for(int j = i + 1;j<bracketOperations.Count;j++)
                {
                    BracketOperation tOp = bracketOperations[j];
                    tOp.positionInString += result.Length;

                    bracketOperations[j] = tOp;
                }
            }

            return data;
        }

        /// <summary>
        /// Changes . to ,
        /// </summary>
        /// <param name="data">Data to change</param>
        /// <returns>Changed data</returns>
        public static string ChangePunctuationMark(string data)
        {
            return data.Replace(".", ",");
        }

        public static double Calculate(string data)
        {
            if (data.Length == 0)
            {
                Debug.LogError("Data is empty");
                return double.NaN;
            }

            if (operations == null)
            {
                FillOperationsList();
            }

            if(bracketsFunctions == null)
            {
                FillBracketsFunctions();
            }

            data = MUtil.RemoveSpaces(data);

            data = ChangePunctuationMark(data);
            data = MUtil.ExpNotationToDecimalNotation(data);

            data = ReplaceConstants(data);

            data = CalculateBrackets(data);
            if(data.StartsWith("E"))
            {
                return double.NaN;
            }


            data = CalculateExpression(data);


            double result = 0;

            if(!double.TryParse(data, out result))
            {
                Debug.LogError("Cannot parse " + data + " to result ( double )");
                return double.NaN;
            }

            return result;
        }
    }
}
