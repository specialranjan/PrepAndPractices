using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace CrackingCodingInterviewQuestions.ArraysAndStrings
{
    public partial class InterviewQuestions
    {
        /* Is Unique: Implement an algorithm to determine if a string has all unique characters. 
         * What if you cannot use additional data structures? 
         * Hints: #44, #7 7 7, #732 */
        public static void Q1_1()
        {
            string sampleString = "#732";
            //Dictionary<char, char> dictionary = new Dictionary<char, char>();                
            //foreach (char c in sampleString)
            //{
            //    if (dictionary.ContainsKey(c))
            //    {
            //        Console.WriteLine("The string '{0}' doesn't have all characters unique", sampleString);
            //        break;
            //    }

            //    dictionary.Add(c, c);
            //}

            int unicodeNo=2;
            for (int i = 1; i <= 21; i++)
            {
                unicodeNo += unicodeNo * 2;
            }


            if (sampleString.Length > 128)
            {
                return;
            }

            bool[] charSet = new bool[128];
            for (int i = 0; i < sampleString.Length; i++)
            {
                int val = sampleString[i];
                if (charSet[val])
                {
                    Console.WriteLine("The string '{0}' doesn't have all characters unique", sampleString);
                    return;
                }
                charSet[val] = true;
            }

            Console.WriteLine("The string '{0}' have all characters unique", sampleString);
        }
    }
}
