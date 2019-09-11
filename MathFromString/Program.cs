using System;

namespace MathFromString
{
    class Program
    {
        /// <summary>
        /// Calculates data and returns with result multiplied by 100
        /// </summary>
        /// <param name="args">Empty => user would be prompted to write data, args[0] => automatically calculate these</param>
        static void Main(string[] args)
        {
            string dataToCalculate = "";
            if(args.Length > 0)
            {
                dataToCalculate = args[0];
            }
            else
            {
                while(dataToCalculate.Length == 0)
                {
                    Debug.Log("Write data to calculate: ", ConsoleColor.White, false);
                    dataToCalculate = Console.ReadLine();
                    Console.WriteLine(" ");
                }
            }

            if(dataToCalculate.Length == 0)
            {
                Debug.FatalError("Data to calculate is empty", -1, 5000);
                return;
            }

            Debug.Log("Test scientific notation: " + MUtil.ToStandardNotationString(2.5));

            double result = MathFromString.Calculate(dataToCalculate);
            Debug.Log("Result: " + result, ConsoleColor.Yellow);

            Environment.Exit((int)(result*100));
        }
    }
}
