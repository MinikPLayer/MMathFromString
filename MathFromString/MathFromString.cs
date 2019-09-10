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
                return l1 / l2;
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
                    Debug.LogError("Cannot parse " + number + " to number ( double )");
                    return false;
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
                Debug.LogError("Operator without number");
                return "EOPWTHTNMBR";
            }

            if(!double.TryParse(startData, out l1))
            {
                Debug.LogError("Cannot convert " + startData + " to l1 ( double )");
                return "ECONVERSION";
            }

            actualNumber = startData;
            bool firstOperation = true;

            // Multiply
            for (int i = startIndex; i < data.Length; i++)
            {
                Operation aop = FindOperation(data[i]);
                if (aop != null)
                {
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


                        //Debug.Log("Data before: " + data);
                        if(noPrevOperators)
                        {
                            data = data.Remove(0, actualNumber.Length);
                            i = 0;
                        }
                        else
                        {
                            data = data.Remove(i - actualNumber.Length - 1 - l1.ToString().Length, actualNumber.Length + 1 + l1.ToString().Length);
                            i -= actualNumber.Length + 1 + l1.ToString().Length;
                        }
                        
                        noPrevOperators = false;

                        //l1 = l2;

                        
                        if (!DoMaths(ref actualNumber, ref l1, ref l2, ref op, aop, ref firstOperation))
                        {
                            return "ECONVERSION";
                        }




                        data = data.Insert(i, l1.ToString());
                        i += l1.ToString().Length;
                        actualNumber = "";
                        op = aop;
                        //Debug.Log("Data after: " + data);
                    }
                    else
                    {
                        if (!double.TryParse(actualNumber, out l1))
                        {
                            Debug.LogError("Cannot parse " + actualNumber + " to number ( double )");
                            return "ECONVERSION";
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

                data = data.Remove(data.Length - actualNumber.Length - 1 - l1.ToString().Length, actualNumber.Length + 1 + l1.ToString().Length);

                noPrevOperators = false;

                //l1 = l2;


                if (!DoMaths(ref actualNumber, ref l1, ref l2, ref op, operations[0], ref firstOperation))
                {
                    return "ECONVERSION";
                }




                data = data.Insert(data.Length, l1.ToString());
                //i += l1.ToString().Length;
                actualNumber = "";
                op = operations[0];
                //Debug.Log("Data after: " + data);
            }
            else
            {
                if (!double.TryParse(actualNumber, out l2))
                {
                    Debug.LogError("Cannot parse " + actualNumber + " to number ( double )");
                    return "ECONVERSION";
                }

                op = operations[0];
                actualNumber = "";
            }

            return data;
        }

        public static double Calculate(string data)
        {
            if (data.Length == 0)
            {
                Debug.LogError("Data is empty");
                return -1;
            }

            if (operations == null)
            {
                FillOperationsList();
            }

            data = MUtil.RemoveSpaces(data);

            // Multiply
            data = CalculateSpecificOperations(data, new Operation[] { operations[2] /* multiply */ , operations[3] /* divide */ });
            Debug.Log("Data after multiplying and dividing: " + data);
            data = CalculateSpecificOperations(data, new Operation[] { operations[0] /* add */ , operations[1] /* substract */ });
            Debug.Log("Data after adding and substracting: " + data);

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

            

            return -2;
        }
    }
}
