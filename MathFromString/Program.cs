using System;
using System.Diagnostics;

using System.Threading;

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
            string dataToCalculate = "2+2";
            if(args.Length > 0)
            {
                dataToCalculate = args[0];
            }
            else if(dataToCalculate.Length == 0)
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
            double result = 0;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();


            dataToCalculate = "1500^150 * 1500^150 / 1500^150";
            int retries = 50000;
            while (true)
            {
                //Debug.Log("Test scientific notation: " + MUtil.ToStandardNotationString(2.5));
                
                
                stopwatch.Restart();
                


                for (int i = 0; i < retries; i++)
                {
                    result = MathFromString.Calculate(dataToCalculate);
                }

                stopwatch.Stop();

                //Debug.Log("Result: " + result, ConsoleColor.Yellow);
                Console.Clear();

                Debug.Log("MMathFromString: ", ConsoleColor.Cyan);
                Debug.Log("Time: " + stopwatch.ElapsedMilliseconds + "ms, " + (stopwatch.ElapsedTicks - 5) + " ticks", ConsoleColor.Cyan);
                Debug.Log("Time per calculation: " + (stopwatch.ElapsedMilliseconds) / (float)retries + "ms, " + (stopwatch.ElapsedTicks - 5) / (float)retries + "\n", ConsoleColor.Cyan);

                stopwatch.Restart();
                for(int i = 0;i< retries; i++)
                {
                    result = MUtil.Power(1500,150) * MUtil.Power(1500, 150) / MUtil.Power(1500, 150);
                }

                stopwatch.Stop();

                Debug.Log("Native: ", ConsoleColor.Yellow);
                Debug.Log("Time: " + stopwatch.ElapsedMilliseconds + "ms, " + (stopwatch.ElapsedTicks - 5) + " ticks", ConsoleColor.Yellow);
                Debug.Log("Time per calculation: " + (stopwatch.ElapsedMilliseconds) / (float)retries + "ms, " + (stopwatch.ElapsedTicks - 5) / (float)retries + "\n", ConsoleColor.Yellow);

                Thread.Sleep(500);

                
            }

            Environment.Exit((int)(result*100));
        }
    }
}
