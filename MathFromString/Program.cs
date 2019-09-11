using System;
using System.Diagnostics;

using System.Threading;

using System.Data;


using System.Collections.Generic;

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
            const bool testMode = false;

            string dataToCalculate = "";
            if(args.Length > 0)
            {
                dataToCalculate = args[0];
            }
            else if(dataToCalculate.Length == 0 && !testMode)
            {
                while(dataToCalculate.Length == 0)
                {
                    Debug.Log("Write data to calculate: ", ConsoleColor.White, false);
                    dataToCalculate = Console.ReadLine();
                    Console.WriteLine(" ");
                }
            }

            if(dataToCalculate.Length == 0 && !testMode)
            {
                Debug.FatalError("Data to calculate is empty", -1, 5000);
                return;
            }

            if(dataToCalculate.ToLower() == "f")
            {
                Debug.Log("To pay respect", ConsoleColor.Yellow);
                return;
            }
            if (!testMode)
            {
                double result = MathFromString.Calculate(dataToCalculate);
                if (double.IsInfinity(result))
                {
                    Debug.LogWarning("Infinity");
                }
                else
                {
                    Debug.Log("Result: " + MUtil.ToStandardNotationString(result), ConsoleColor.Blue);
                }
            }
            else
            {
                double result = 0;
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();


                dataToCalculate = "4 ^ 1.5";
                int retries = 10000;
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

                    Debug.Log("MMathFromString result: " + result, ConsoleColor.Cyan);
                    Debug.Log("Time for " + retries + " retries: " + stopwatch.ElapsedMilliseconds + "ms, " + (stopwatch.ElapsedTicks - 5) + " ticks", ConsoleColor.Cyan);
                    Debug.Log("Time per calculation: " + (stopwatch.ElapsedMilliseconds) / (float)retries + "ms, " + (stopwatch.ElapsedTicks - 5) / (float)retries + "\n", ConsoleColor.Cyan);


                    stopwatch.Restart();
                    for (int i = 0; i < retries; i++)
                    {
                        //result = MUtil.Power(1500,150) * MUtil.Power(1500, 150) / MUtil.Power(1500, 150);
                        result = Math.Pow(4, 1.5);
                    }

                    stopwatch.Stop();

                    Debug.Log("Native result: " + result, ConsoleColor.Yellow);
                    Debug.Log("Time for " + retries + " retries: " + stopwatch.ElapsedMilliseconds + "ms, " + (stopwatch.ElapsedTicks - 5) + " ticks", ConsoleColor.Yellow);
                    Debug.Log("Time per calculation: " + (stopwatch.ElapsedMilliseconds) / (float)retries + "ms, " + (stopwatch.ElapsedTicks - 5) / (float)retries + "\n", ConsoleColor.Yellow);


                    Thread.Sleep(500);


                }

                Environment.Exit((int)(result * 100));
            }
        }
    }
}
