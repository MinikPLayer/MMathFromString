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
                //Debug.Log("Adding...", ConsoleColor.Cyan);
                return l1 + l2;
            }

            public static double Subtract(double l1, double l2)
            {
                //Debug.Log("Substracting...", ConsoleColor.Cyan);
                return l1 - l2;
            }

            public static double Multiply(double l1, double l2)
            {
                //Debug.Log("Multiplying...", ConsoleColor.Cyan);
                return l1 * l2;
            }

            public static double Divide(double l1, double l2)
            {
                //Debug.Log("Dividing...", ConsoleColor.Cyan);
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
        }

        public struct BracketOperation
        {
            public int positionInString;
            public string operation;

            public BracketOperation(string _operation, int _position)
            {
                positionInString = _position;
                operation = _operation;
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

        private static List<Operation> operations;

        public static void FillOperationsList()
        {
            operations = new List<Operation>();

            operations.Add(new Operation("Add", '+', OperationFunctions.Add));
            operations.Add(new Operation("Subtract", '-', OperationFunctions.Subtract));
            operations.Add(new Operation("Multiply", '*', OperationFunctions.Multiply));
            operations.Add(new Operation("Divide", new char[] {'/', '\\', ':' }, OperationFunctions.Divide));
            operations.Add(new Operation("Power", '^', OperationFunctions.Power));
        }

        private static Operation FindOperation(char opChar)
        {
            for(int i = 0;i<operations.Count;i++)
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
            //l1 = l2;
            //Debug.Log("Actual number: " + number + ", operator: " + op.name);

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

            //Debug.Log("L1: " + l1);
            //Debug.Log("L2: " + l2);

            number = "";
            op = newOp;

            return true;
        }

       

        public static string CalculateSpecificOperations(string data, Operation[] allowedOperations)
        {
            double l1 = 0, l2 = 0;
            Operation op = operations[0]; // First operation is always addition


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
                Debug.LogError("Operator without number: " + data);
                return "EOPWTHTNMBR";
            }

            

            if(!double.TryParse(startData, out l1))
            {

                if (startData[startData.Length - 1] == 'E' || startData[startData.Length - 1] == 'e')
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
                        if(data[i] == 'e' || data[i] == 'E')
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
                        if(data[i] == '+' && i > 0 && (data[i-1] == 'e' || data[i-1] == 'E'))
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
                        if (data[i] == '+' && i > 0 && (data[i - 1] == 'e' || data[i - 1] == 'E'))
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
                if (data[data.Length - 1] == '+' && data.Length > 0 && (data[data.Length - 1] == 'e' || data[data.Length - 1] == 'E'))
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
            //Debug.Log("Data after powers: " + data);
            data = CalculateSpecificOperations(data, new Operation[] { operations[2] /* multiply */ , operations[3] /* divide */ });
            //Debug.Log("Data after multiplying and dividing: " + data);
            data = CalculateSpecificOperations(data, new Operation[] { operations[0] /* add */ , operations[1] /* substract */ });
            //Debug.Log("Data after adding and substracting: " + data);

            return data;
        }

        /// <summary>
        /// Gets bracket operation and creates list from them
        /// </summary>
        /// <param name="data">String with expression</param>
        /// <param name="bracketOperations"></param>
        /// <returns>Data without brackets operations</returns>
        private static string GetBrackets(string data)
        {
            string actExpr = "";
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
                        BracketOperation operation = new BracketOperation(actExpr.Remove(0,1).Remove(actExpr.Length - 2, 1), i - actExpr.Length + 1);
                        //Debug.Log("Bracket operation: \"" + operation.operation + "\"");
                        //Debug.Log("Bracket operation start: " + operation.positionInString);

                        bracketOperations.Add(operation);


                        data = data.Remove(i - actExpr.Length + 1, actExpr.Length);
                        i -= actExpr.Length;
                        actExpr = "";

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
                op.operation = GetBrackets(bracketOperations[i].operation);
                bracketOperations[i] = op;
            }

            // Calcualate brackets
            for(int i = 0;i<bracketOperations.Count;i++)
            {
                string result = CalculateExpression(bracketOperations[i].operation);
                double res;
                if(!double.TryParse(result, out res))
                {
                    Debug.LogError("Cannot convert " + result + " to result ( double )");
                    return "ECONVERSION";
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

            //Debug.Log("Data before removing spaces: " + data);
            data = MUtil.RemoveSpaces(data);
            //Debug.Log("Data after removing spaces: " + data);

            data = ChangePunctuationMark(data);
            data = MUtil.ExpNotationToDecimalNotation(data);

            data = GetBrackets(data);
            //Debug.Log("Data after bracket calculating: \"" + data + "\"");

            // Multiply
            data = CalculateExpression(data);

            /*actualNumber = "";
            l1 = l2 = 0;
            op = operations[0];

            // Rest
            for (int i = 0; i < data.Length; i++)
            {
                Operation aop = FindOperation(data[i]);
                if (aop != null)
                {
                    if (!DoMaths(ref actualNumber, ref l1, ref l2, ref op, aop))
                    {
                        return -2;
                    }
                }
                else
                {
                    actualNumber += data[i];
                }
            }

            //Debug.Log("Actual number: " + actualNumber + ", operator: " + op.name);
            if (!DoMaths(ref actualNumber, ref l1, ref l2, ref op, operations[0]))
            {
                return -2;
            }*/


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
