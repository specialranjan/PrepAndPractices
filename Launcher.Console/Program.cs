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

namespace Launcher.Console
{
    public class Program
    {
        static void Main(string[] args)
        {
            GeneralQuestions.Q1();
            System.Console.ReadKey();
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
