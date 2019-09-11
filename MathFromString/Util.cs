using System;
using System.Collections.Generic;
using System.Text;

namespace MathFromString
{
    public static class MUtil
    {
        public static bool IsNumber(char data)
        {
            /*if(data >= 48 && data <= 57)
            {
                return true;
            }

            return false;*/

            return (data >= 48 && data <= 57);
        }

        public static double Power(double a, double n)
        {
            if(n%1 != 0)
            {
                return Math.Pow(a, n);
            }

            if (a == 0) return 0;
            if (a == 1) return 1;
            if (n == 0) return 1;

            double t = a;
            for(int i = 1;i<n;i++)
            {
                a *= t;
            }

            if(n < 0)
            {
                return 1.0/a;
            }
            else
            {
                return a;
            }
        }

        public static string RemoveSpaces(string src)
        {
            return RemoveChars(src, new char[] { ' ' });
        }

        public static double GetDoubleFromExpNotation(string number, bool stopOnDifferentChar = true)
        {
            string value = ExpNotationToDecimalNotation(number, stopOnDifferentChar, false);
            double returnValue;
            if(!double.TryParse(value, out returnValue))
            {
                Debug.LogError("Cannot convert " + value + " to returnValue ( double )");
                return double.NaN;
            }

            return returnValue;
        }

        public static string ExpNotationToDecimalNotation(string number, bool stopOnDifferentChar = true, bool includeRest = true)
        {
            //Debug.Log("Converting " + number);

            bool bigE = false;

            if(number.Contains("E+"))
            {
                bigE = true;
            }
            else
            {
                return number;
            }

            string beforeComma = "";
            string afterComma = "";
            string expValue = "";
            string rest = "";


            bool wasComma = false;

            for(int i = 0;i<number.Length;i++)
            {

                if(number[i] == ',')
                {
                    wasComma = true;
                    continue;
                }

                if (number[i] == 'E')
                {
                    if (i + 2 >= number.Length)
                    {
                        Debug.LogError("No exp value 1E");
                        return "ERROR";
                    }

                    if (number[i + 1] != '+')
                    {
                        Debug.LogError("Unknown char E in data");
                        return "ERROR";
                    }


                    for (int j = i + 2; j < number.Length; j++)
                    {
                        if (!IsNumber(number[j]))
                        {
                            if (stopOnDifferentChar)
                            {
                                rest = number.Substring(j, number.Length - j);
                                break;
                            }
                        }

                        expValue += number[j];
                    }

                    break;
                }

                if (wasComma)
                {
                   

                    afterComma += number[i];
                }
                else
                {
                    beforeComma += number[i];
                }
            }

            if(expValue.Length == 0)
            {
                Debug.LogError("No exp value 2");
                return "ERROR";
            }

            double exp;
            if(!double.TryParse(expValue, out exp))
            {
                Debug.LogError("Bad exp value");
                return "ERROR";
            }

            //Debug.Log("Before comma: " + beforeComma);
            //Debug.Log("After comma: " + afterComma);
            //Debug.Log("Exp: " + expValue);
            //Debug.Log("Rest: " + rest);

            for(int i = 0;i<exp;i++)
            {
                if(i >= afterComma.Length)
                {
                    beforeComma += "0";
                }
                else
                {
                    beforeComma += afterComma[i];
                }
            }

            //Debug.Log("Result: " + beforeComma + "\n\n");

            if (includeRest)
            {
                return beforeComma + rest;
            }
            else
            {
                return beforeComma;
            }

        }

        /// <summary>
        /// Converts the double to a standard notation string.
        /// </summary>
        /// <param name="d">The double to convert.</param>
        /// <returns>The double as a standard notation string.</returns>
        public static String ToStandardNotationString(double d)
        {
            //Keeps precision of double up to is maximum
            return d.ToString(".#####################################################################################################################################################################################################################################################################################################################################");

        }

        public static string RemoveChars(string src, char[] charsToRemove)
        {
            for(int i = 0;i<src.Length;i++)
            {
                for(int j = 0;j<charsToRemove.Length;j++)
                {
                    if(src[i] == charsToRemove[j])
                    {
                        src = src.Remove(i, 1);
                        i--;
                    }
                }
            }

            return src;
        }

        public static string GetStringToSpecialChar(string str, char specialChar)
        {
            string value = "";

            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == specialChar) return value;
                value += str[i];
            }

            return value;
        }

        /// <summary>
        /// Returns string from special character ( without it )
        /// </summary>
        /// <param name="str"></param>
        /// <param name="specialChar"></param>
        /// <param name="strToSpecialChar"></param>
        /// <returns></returns>
        public static string GetStringToSpecialCharAndDelete(string str, char specialChar, out string strToSpecialChar)
        {
            strToSpecialChar = GetStringToSpecialChar(str, specialChar);

            return str.Remove(0, strToSpecialChar.Length + 1);
        }

        public static bool AskUserYesNo(string action = "do this")
        {
            ConsoleColor orColor = Console.ForegroundColor;

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Are you sure you want to " + action + "? Y - yes, N - no");
                Console.ForegroundColor = orColor;

                ConsoleKeyInfo info = Console.ReadKey();
                if(info.Key == ConsoleKey.Y)
                {
                    return true;
                }
                if(info.Key == ConsoleKey.N)
                {
                    return false;
                }
            }


            return false;

        }

        public static List<string> RemoveEmptyLines(List<string> lines, bool removeAlsoNLandCR = true)
        {
            

            //List<string> linesList = new List<string>(lines);
            for(int i = 0;i< lines.Count;i++)
            {
                Debug.Log("Checking " + i + ", it's " + lines[i].Length + " chars long");
                if(lines[i].Length == 0)
                {
                    Debug.LogWarning("Removing at " + i);
                    lines.RemoveAt(i);
                    i--;
                    continue;
                }
                if(removeAlsoNLandCR)
                {
                    if(lines[i] == "\n" || lines[i] == "\r")
                    {
                        Debug.LogWarning("Removing at " + i);
                        lines.RemoveAt(i);
                        i--;
                        continue;
                    }
                }
            }

            

            return lines;
        }

        public static string[] RemoveEmptyLines(string[] lines)
        {
            Debug.Log("Size before cutting : " + lines.Length);
            string[] lns = RemoveEmptyLines(new List<string>(lines)).ToArray();
            Debug.Log("Size after cutting: " + lns.Length);
            return lns;
        }

        public static string[] StringToStringArray(string input)
        {
            List<string> array = new List<string>();
            string actualLine = "";
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '\n' || input[i] == '\r')
                {
                    Debug.Log("Actual line: \"" + actualLine + "\" lenght: " + actualLine.Length);
                    array.Add(actualLine);


                    actualLine = "";
                    continue;
                }

                actualLine += input[i];
            }

            if (actualLine.Length != 0)
            {
                array.Add(actualLine);
                Debug.Log("Actual line: \"" + actualLine + "\" lenght: " + actualLine.Length);
            }

            return array.ToArray();
        }

        public static string ByteArrayToString(byte[] array)
        {
            return Convert.ToBase64String(array);
        }

        public static byte[] StringToByteArray(string input)
        {
            return Convert.FromBase64String(input);
        }
    }
}
