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


                //dataToCalculate = "sqrt(1-(1256^2/1426789^2))/sin(6732+124)";
                const double x = 125123.1532;
                dataToCalculate = "(sqrt(2*" + x.ToString() + "+1)-sqrt(" + x.ToString() + "+6))/(2*" + x.ToString() + "^2-7*" + x.ToString() + "-15)";
                int retries = 1;

                double avg = 0;
                int repeats = 0;
                while (true)
                {
                    //Debug.Log("Test scientific notation: " + MUtil.ToStandardNotationString(2.5));

                    double mmath = 0;
                    double native = 0;

                    stopwatch.Restart();



                    for (int i = 0; i < retries; i++)
                    {
                        result = MathFromString.Calculate(dataToCalculate);
                    }

                    stopwatch.Stop();
                    mmath = stopwatch.ElapsedTicks - 5;

                    //Debug.Log("Result: " + result, ConsoleColor.Yellow);
                    Console.Clear();

                    Debug.Log("MMathFromString result: " + result, ConsoleColor.Cyan);
                    Debug.Log("Time for " + retries + " retries: " + stopwatch.ElapsedMilliseconds + "ms, " + (stopwatch.ElapsedTicks - 5) + " ticks", ConsoleColor.Cyan);
                    Debug.Log("Time per calculation: " + (stopwatch.ElapsedMilliseconds) / (float)retries + "ms, " + (stopwatch.ElapsedTicks - 5) / (float)retries + " ticks\n", ConsoleColor.Cyan);


                    stopwatch.Restart();
                    for (int i = 0; i < retries; i++)
                    {
                        //result = MUtil.Power(1500,150) * MUtil.Power(1500, 150) / MUtil.Power(1500, 150);
                        //result = Math.Sin(Math.Pow(4, 1.5));
                        //result = Math.Sqrt(1 - (Math.Pow(1256, 2) / Math.Pow(1426789, 2))) / Math.Sin(6732 + 124);
                        result = (Math.Sqrt(2 * x + 1) - Math.Sqrt(x + 6)) / (2 * Math.Pow(x, 2) - 7 * x - 15);
                    }

                    stopwatch.Stop();
                    native = stopwatch.ElapsedTicks - 5;

                    Debug.Log("Native result: " + result, ConsoleColor.Yellow);
                    Debug.Log("Time for " + retries + " retries: " + stopwatch.ElapsedMilliseconds + "ms, " + (stopwatch.ElapsedTicks - 5) + " ticks", ConsoleColor.Yellow);
                    Debug.Log("Time per calculation: " + (stopwatch.ElapsedMilliseconds) / (float)retries + "ms, " + (stopwatch.ElapsedTicks - 5) / (float)retries + " ticks\n", ConsoleColor.Yellow);


                    repeats++;
                    avg += (mmath / native);
                    Debug.Log("\nAbout " + (int)(mmath / native) + "x slower", ConsoleColor.DarkMagenta);
                    Debug.Log("On averge: " + (int)(avg / repeats) + "x slower", ConsoleColor.Magenta);
                    Thread.Sleep(500);


                }

                Environment.Exit((int)(result * 100));
            }
        }
    }
}
