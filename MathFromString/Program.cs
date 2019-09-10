using System;

namespace MathFromString
{
    class Program
    {
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

            double result = MathFromString.Calculate(dataToCalculate);
            Debug.Log("Result: " + result, ConsoleColor.Yellow);
        }
    }
}
