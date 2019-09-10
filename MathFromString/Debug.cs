using System;
using System.Collections.Generic;
using System.Text;

using System.Threading;

namespace MathFromString
{
    public static class Debug
    {
        public static void Log(object data, ConsoleColor color = ConsoleColor.White, bool newLine = true)
        {
            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            if (newLine)
            {
                Console.WriteLine(data);
            }
            else
            {
                Console.Write(data);
            }
            Console.ForegroundColor = originalColor;
        }

        public static void LogWarning(object data, bool newLine = true)
        {
            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            if (newLine)
            {
                Console.WriteLine(data);
            }
            else
            {
                Console.Write(data);
            }
            Console.ForegroundColor = originalColor;
        }

        public static void LogError(object data, bool newLine = true)
        {
            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            if (newLine)
            {
                Console.WriteLine(data);
            }
            else
            {
                Console.Write(data);
            }
            Console.ForegroundColor = originalColor;
        }

        /// <summary>
        /// Displays fatal error
        /// </summary>
        /// <param name="data">Message to display</param>
        /// <param name="exitCode">If different from 0 exit application with this code</param>
        public static void FatalError(object data, int exitCode = 0, int sleepTime = 0)
        {
            LogError("\n\n\n\n" + "FATAL ERROR:\n" + data);

            //MessageBox.Show(data.ToString(), "FATAL ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

            if(sleepTime != 0)
            {
                Thread.Sleep(sleepTime);
            }

            if(exitCode != 0)
            {
                Environment.Exit(exitCode);
            }

            
        }
    }
}
