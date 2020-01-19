using System;
using System.Collections.Generic;
using CrackingCodingInterviewQuestions;
using CrackingCodingInterviewQuestions.ArraysAndStrings;
using DataStructure;
using DataStructure.LinkedList;
using DesignPatterns.AdapterPattern;
using DesignPatterns.FactoryPattern;
using DesignPatterns.ObserverPattern;
using DesignPatterns.SingletonPattern;
using Launcher.Console.DataStructure;
using Launcher.Console.DataStructure.LinkedList;
using System.Linq;
using DataStructure.ArrayStrings;
using DataStructure.BinarySearchTree;
using System.CodeDom;
using DataStructure.MultiDimentionalArray;
using System.Runtime.InteropServices.ComTypes;
using System.Text.RegularExpressions;

namespace Launcher.Console
{
    public class Program
    {
        static void Main(string[] args)
        {
            //GeneralQuestions.Q1();
            if (Regex.IsMatch("abc123", @"^[a-zA-Z0-9]+$"))
            {
                System.Console.WriteLine("String");
            }
            else
            {
                System.Console.WriteLine(" Not String");
            }
            
            System.Console.ReadKey();
        }

        public static string MostFreeTime(string[] strArr)
        {

            TimeSpan longestTimeDiff = TimeSpan.Zero;
            List<DateTime> eventTimeList = new List<DateTime>();
            //DateTime fstartTime, fendTime;

            for (int i=0;i<strArr.Length-1;i++)
            {
                string[] eventTimes = strArr[i].Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                DateTime fstartTime = Convert.ToDateTime(eventTimes[0]); 
                DateTime fendTime = Convert.ToDateTime(eventTimes[1]);

                eventTimes = strArr[i+1].Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                DateTime sstartTime = Convert.ToDateTime(eventTimes[0]);
                DateTime sendTime = Convert.ToDateTime(eventTimes[1]);

                TimeSpan timeDiff = sstartTime.Subtract(fendTime);
                if (longestTimeDiff == TimeSpan.Zero)
                    longestTimeDiff = timeDiff;
                else
                {
                    if (longestTimeDiff < timeDiff)
                    {
                        longestTimeDiff = timeDiff;
                    }
                }
            }

            //eventTimeList.Sort();
            //longestTimeDiff = eventTimeList[2].Subtract(eventTimeList[1]);
            //int i = 0;
            //TimeSpan timeDiff;
            //for (; i < eventTimeList.Count - 1; i++)
            //{
            //    timeDiff = eventTimeList[i + 2].Subtract(eventTimeList[i+1]);
            //    if (timeDiff > longestTimeDiff)
            //    {
            //        longestTimeDiff = timeDiff;
            //    }
            //}

            return longestTimeDiff.ToString(@"hh\:mm");

        }

        public static long getNumber()
        {
            long binaryNum = Convert.ToInt64("0000011111");
            //64 digit test case was failing
            long baseValue = 2, decValue = 0;
            while (binaryNum > 0)
            {
                long rem = binaryNum % 10;
                decValue = decValue + rem * baseValue;
                binaryNum = binaryNum / 10;
                baseValue = baseValue * 2;
            }
            return decValue;
        }
    }
}
